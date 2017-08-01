using Breeze.ContextProvider;
using Breeze.ContextProvider.EF6;
using Newtonsoft.Json.Linq;
using Pentamic.SSBI.Models.DataModel;
using Pentamic.SSBI.Models.DataModel.Objects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AS = Microsoft.AnalysisServices.Tabular;
using AC = Microsoft.AnalysisServices.AdomdClient;


namespace Pentamic.SSBI.Services
{
    public class DataModelService
    {
        private EFContextProvider<DataModelContext> _contextProvider;
        private string _asConnectionString = System.Configuration.ConfigurationManager
                .ConnectionStrings["AnalysisServiceConnection"]
                .ConnectionString;

        public DataModelService()
        {
            _contextProvider = new EFContextProvider<DataModelContext>();
        }

        private DataModelContext Context
        {
            get
            {
                return _contextProvider.Context;
            }
        }

        public string Metadata
        {
            get { return _contextProvider.Metadata(); }
        }

        public IQueryable<Model> Models
        {
            get { return Context.Models.Where(x => x.State != DataModelObjectState.Deleted && x.State != DataModelObjectState.Obsolete); }
        }
        public IQueryable<DataSource> DataSources
        {
            get { return Context.DataSources.Where(x => x.State != DataModelObjectState.Deleted && x.State != DataModelObjectState.Obsolete); }
        }
        public IQueryable<Table> Tables
        {
            get { return Context.Tables.Where(x => x.State != DataModelObjectState.Deleted && x.State != DataModelObjectState.Obsolete); }
        }
        public IQueryable<Column> Columns
        {
            get { return Context.Columns.Where(x => x.State != DataModelObjectState.Deleted && x.State != DataModelObjectState.Obsolete); }
        }
        public IQueryable<Partition> Partitions
        {
            get { return Context.Partitions.Where(x => x.State != DataModelObjectState.Deleted && x.State != DataModelObjectState.Obsolete); }
        }
        public IQueryable<Measure> Measures
        {
            get { return Context.Measures.Where(x => x.State != DataModelObjectState.Deleted && x.State != DataModelObjectState.Obsolete); }
        }
        public IQueryable<Relationship> Relationships
        {
            get { return Context.Relationships.Where(x => x.State != DataModelObjectState.Deleted && x.State != DataModelObjectState.Obsolete); }
        }
        public IQueryable<Perspective> Perspectives
        {
            get { return Context.Perspectives; }
        }
        public IQueryable<Hierarchy> Hierarchies
        {
            get { return Context.Hierarchies; }
        }
        public IQueryable<Level> Levels
        {
            get { return Context.Levels; }
        }
        public IQueryable<SourceFile> SourceFiles
        {
            get { return Context.SourceFiles; }
        }
        //public async Task<DataSource> ImportDataSource(MultipartFormDataStreamProvider provider)
        //{
        //    var file = provider.FileData[0];
        //    var formData = provider.FormData;
        //    var modelId = Convert.ToInt32(formData["modelId"]);
        //    var name = formData["name"];
        //    var description = formData["description"];
        //    var dataSource = new DataSource
        //    {
        //        ModelId = modelId,
        //        Type = DataSourceType.Excel,
        //        Name = name,
        //        OriginalName = name,
        //        Description = description,
        //        FileName = file.Headers.ContentDisposition.FileName.Replace("\"", ""),
        //        FilePath = file.LocalFileName,
        //        State = DataModelObjectState.Added
        //    };
        //    Context.DataSources.Add(dataSource);
        //    await Context.SaveChangesAsync();
        //    return dataSource;
        //}

        public async Task<int> HandleFileUpload(MultipartFormDataStreamProvider provider)
        {
            var file = provider.FileData[0];
            var sourceFile = new SourceFile
            {
                FileName = file.Headers.ContentDisposition.FileName.Replace("\"", ""),
                FilePath = file.LocalFileName
            };
            Context.SourceFiles.Add(sourceFile);
            await Context.SaveChangesAsync();
            return sourceFile.Id;
        }

        public void DeployModel(int modelId)
        {
            var mo = Context.Models.Find(modelId);
            if (mo == null)
            {
                throw new Exception("Model is null");
            }
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                if (mo.State == DataModelObjectState.Deleted)
                {
                    server.Databases.Remove(mo.DatabaseName);
                    mo.State = DataModelObjectState.Obsolete;
                    return;
                }
                AS.Database database = null;
                if (mo.State == DataModelObjectState.Added)
                {
                    database = new AS.Database
                    {
                        ID = mo.DatabaseName,
                        Name = mo.DatabaseName,
                        StorageEngineUsed = Microsoft.AnalysisServices.StorageEngineUsed.TabularMetadata,
                        CompatibilityLevel = 1200
                    };
                    database.Model = new AS.Model
                    {
                        Name = mo.Name,
                        Description = mo.Description,
                        DefaultMode = mo.DefaultMode.ToModeType()
                    };
                    server.Databases.Add(database);
                    mo.State = DataModelObjectState.Unchanged;
                }
                else
                {
                    database = server.Databases.Find(mo.DatabaseName);
                    if (mo.State == DataModelObjectState.Modified)
                    {

                        if (database.Model.Name != mo.Name)
                        {
                            database.Model.RequestRename(mo.Name);
                            database.Update(Microsoft.AnalysisServices.UpdateOptions.ExpandFull);
                            if (database.Model.Description != mo.Description)
                            {
                                database.Model.Description = mo.Description;
                            }
                            mo.State = DataModelObjectState.Unchanged;
                            Context.SaveChanges();
                        }
                        else
                        {
                            if (database.Model.Description != mo.Description)
                            {
                                database.Model.Description = mo.Description;
                            }
                            mo.State = DataModelObjectState.Unchanged;
                        }
                    }
                }

                //Data Sources
                var dataSources = Context.DataSources.Where(x => x.ModelId == modelId).ToList();
                foreach (var ds in dataSources)
                {
                    if (ds.State == DataModelObjectState.Deleted)
                    {
                        database.Model.DataSources.Remove(ds.OriginalName);
                        ds.State = DataModelObjectState.Obsolete;
                    }
                    AS.ProviderDataSource dataSource = null;
                    if (ds.State == DataModelObjectState.Added)
                    {
                        dataSource = new AS.ProviderDataSource
                        {
                            Name = ds.Name,
                            Description = ds.Description,
                            ConnectionString = GetDataSourceConnectionString(ds),
                            ImpersonationMode = Microsoft.AnalysisServices.Tabular.ImpersonationMode.ImpersonateServiceAccount
                            //ImpersonationMode = Microsoft.AnalysisServices.Tabular.ImpersonationMode.ImpersonateAccount,
                            //Account = "datht.it@live.com",
                            //Password = "20111988Gi@ng"
                        };
                        database.Model.DataSources.Add(dataSource);
                        ds.State = DataModelObjectState.Unchanged;
                    }
                    else
                    {
                        dataSource = database.Model.DataSources.Find(ds.OriginalName) as AS.ProviderDataSource;
                        if (ds.State == DataModelObjectState.Modified)
                        {
                            if (dataSource.Name != ds.Name)
                            {
                                dataSource.RequestRename(ds.Name);
                                ds.OriginalName = ds.Name;
                            }
                            if (dataSource.Description != ds.Description)
                            {
                                dataSource.Description = ds.Description;
                            }
                            var newConStr = GetDataSourceConnectionString(ds);
                            if (newConStr != dataSource.ConnectionString)
                            {
                                dataSource.ConnectionString = newConStr;
                            }
                            ds.State = DataModelObjectState.Unchanged;
                        }
                    }
                }

                var tables = Context.Tables.Where(x => x.ModelId == modelId)
                    .Include(x => x.Partitions)
                    .Include(x => x.Columns)
                    .Include(x => x.Measures).ToList();
                foreach (var tb in tables)
                {
                    AS.Table table = null;
                    if (tb.State == DataModelObjectState.Added)
                    {
                        table = new AS.Table
                        {
                            Name = tb.Name,
                            Description = tb.Description
                        };
                        database.Model.Tables.Add(table);
                        tb.State = DataModelObjectState.Unchanged;
                    }
                    else
                    {
                        table = database.Model.Tables.Find(tb.OriginalName);
                        if (tb.State == DataModelObjectState.Modified)
                        {
                            if (tb.Name != table.Name)
                            {
                                table.RequestRename(tb.Name);
                                tb.OriginalName = tb.Name;
                            }
                            if (tb.Description != table.Description)
                            {
                                table.Description = tb.Description;
                            }
                            tb.State = DataModelObjectState.Unchanged;
                        }
                    }

                    foreach (var pa in tb.Partitions)
                    {
                        var ds = pa.DataSource;
                        if (ds.State == DataModelObjectState.Obsolete || tb.State == DataModelObjectState.Obsolete || pa.State == DataModelObjectState.Deleted)
                        {
                            if (tb.State != DataModelObjectState.Obsolete)
                            {
                                table.Partitions.Remove(pa.OriginalName);
                            }
                            pa.State = DataModelObjectState.Obsolete;
                        }
                        AS.Partition partition = null;
                        if (pa.State == DataModelObjectState.Added)
                        {
                            table.Partitions.Add(new AS.Partition
                            {
                                Name = pa.Name,
                                Source = new AS.QueryPartitionSource()
                                {
                                    DataSource = database.Model.DataSources[ds.Name],
                                    Query = pa.Query
                                }
                            });
                            pa.State = DataModelObjectState.Unchanged;
                        }
                        if (pa.State == DataModelObjectState.Modified)
                        {
                            partition = table.Partitions.Find(pa.OriginalName);
                            if (partition.Name != pa.Name)
                            {
                                partition.RequestRename(pa.Name);
                                pa.OriginalName = pa.Name;
                            }
                            var source = partition.Source as AS.QueryPartitionSource;
                            if (source.Query != pa.Query)
                            {
                                source.Query = pa.Query;
                            }
                            pa.State = DataModelObjectState.Unchanged;
                        }
                    }

                    foreach (var co in tb.Columns)
                    {
                        if (tb.State == DataModelObjectState.Obsolete || co.State == DataModelObjectState.Deleted)
                        {
                            if (tb.State != DataModelObjectState.Obsolete)
                            {
                                table.Columns.Remove(co.OriginalName);
                            }
                            co.State = DataModelObjectState.Obsolete;
                        }
                        if (co.State == DataModelObjectState.Added)
                        {
                            table.Columns.Add(new AS.DataColumn
                            {
                                Name = co.Name,
                                DataType = co.DataType.ToDataType(),
                                SourceColumn = co.SourceColumn,
                                IsHidden = co.IsHidden,
                                DisplayFolder = co.DisplayFolder,
                                FormatString = co.FormatString
                            });
                            co.State = DataModelObjectState.Unchanged;
                        }
                        if (co.State == DataModelObjectState.Modified)
                        {
                            var column = table.Columns.Find(co.OriginalName);
                            if (co.Name != column.Name)
                            {
                                column.RequestRename(co.Name);
                                co.OriginalName = co.Name;
                            }
                            if (co.IsHidden != column.IsHidden)
                            {
                                column.IsHidden = co.IsHidden;
                            }
                            if (co.DisplayFolder != column.DisplayFolder)
                            {
                                column.DisplayFolder = co.DisplayFolder;
                            }
                            if (co.FormatString != column.FormatString)
                            {
                                column.FormatString = co.FormatString;
                            }
                            co.State = DataModelObjectState.Unchanged;
                        }
                    }

                    foreach (var me in tb.Measures)
                    {
                        if (tb.State == DataModelObjectState.Obsolete || me.State == DataModelObjectState.Deleted)
                        {
                            if (tb.State != DataModelObjectState.Obsolete)
                            {
                                table.Measures.Remove(me.OriginalName);
                            }
                            me.State = DataModelObjectState.Obsolete;
                        }
                        if (me.State == DataModelObjectState.Added)
                        {
                            table.Measures.Add(new AS.Measure
                            {
                                Name = me.Name,
                                Expression = me.Expression
                            });
                            me.State = DataModelObjectState.Unchanged;
                        }
                        if (me.State == DataModelObjectState.Modified)
                        {
                            var measure = table.Measures.Find(me.OriginalName);
                            if (measure.Name != me.Name)
                            {
                                measure.RequestRename(me.Name);
                                me.OriginalName = me.Name;
                            }
                            if (measure.Expression != me.Expression)
                            {
                                measure.Expression = me.Expression;
                            }
                            if (measure.Description != me.Description)
                            {
                                measure.Description = me.Description;
                            }
                            me.State = DataModelObjectState.Unchanged;
                        }
                    }
                }

                //Relationships
                var relationships = Context.Relationships.Where(x => x.ModelId == modelId && x.State != DataModelObjectState.Obsolete)
                    .Include(x => x.FromColumn.Table).Include(x => x.ToColumn.Table).ToList();
                foreach (var re in relationships)
                {
                    if (re.ToColumn.State == DataModelObjectState.Obsolete ||
                        re.ToColumn.Table.State == DataModelObjectState.Obsolete ||
                        re.FromColumn.State == DataModelObjectState.Obsolete ||
                        re.FromColumn.Table.State == DataModelObjectState.Obsolete ||
                        re.State == DataModelObjectState.Deleted)
                    {
                        database.Model.Relationships.Remove(re.OriginalName);
                        re.State = DataModelObjectState.Obsolete;
                    }
                    if (re.State == DataModelObjectState.Added)
                    {
                        database.Model.Relationships.Add(new AS.SingleColumnRelationship()
                        {
                            Name = re.Name,
                            ToColumn = database.Model.Tables[re.ToColumn.Table.Name].Columns[re.ToColumn.Name],
                            FromColumn = database.Model.Tables[re.FromColumn.Table.Name].Columns[re.FromColumn.Name],
                            FromCardinality = AS.RelationshipEndCardinality.Many,
                            ToCardinality = AS.RelationshipEndCardinality.One
                        });
                        re.State = DataModelObjectState.Unchanged;
                    }
                    if (re.State == DataModelObjectState.Modified)
                    {
                        var relationship = database.Model.Relationships.Find(re.OriginalName);
                        if (re.Name != relationship.Name)
                        {
                            relationship.RequestRename(re.Name);
                            re.OriginalName = re.Name;
                        }
                        re.State = DataModelObjectState.Unchanged;
                    }
                }

                //Commit changes
                database.Update(Microsoft.AnalysisServices.UpdateOptions.ExpandFull);
                server.Disconnect();
                Context.SaveChanges();
            }
        }

        public void RefreshModel(int modelId)
        {
            var mo = Context.Models.Find(modelId);
            AS.Server server = null;
            try
            {
                using (server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases[mo.DatabaseName];
                    database.Model.RequestRefresh(AS.RefreshType.Full);
                    database.Update(Microsoft.AnalysisServices.UpdateOptions.ExpandFull);
                    server.Disconnect();
                }
            }
            finally
            {
                if (server != null && server.Connected)
                {
                    server.Disconnect();
                }
            }
        }

        public void RefreshTable(int tableId)
        {
            var tb = Context.Tables.Where(x => x.Id == tableId)
                .Include(x => x.Model)
                .FirstOrDefault();
            AS.Server server = null;
            try
            {
                using (server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases[tb.Model.DatabaseName];
                    var table = database.Model.Tables[tb.Name];
                    table.RequestRefresh(AS.RefreshType.Full);
                    database.Update(Microsoft.AnalysisServices.UpdateOptions.ExpandFull);
                    server.Disconnect();
                }
            }
            finally
            {
                if (server != null && server.Connected)
                {
                    server.Disconnect();
                }
            }
        }

        public void RefreshPartition(int partitionId)
        {
            var pa = Context.Partitions.Where(x => x.Id == partitionId)
                .Include(x => x.Table.Model)
                .FirstOrDefault();
            AS.Server server = null;
            try
            {
                using (server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases[pa.Table.Model.DatabaseName];
                    var partition = database.Model.Tables[pa.Table.Name].Partitions[pa.Name];
                    partition.RequestRefresh(AS.RefreshType.Full);
                    database.Update(Microsoft.AnalysisServices.UpdateOptions.ExpandFull);
                    server.Disconnect();
                }
            }
            finally
            {
                if (server != null && server.Connected)
                {
                    server.Disconnect();
                }
            }
        }

        public List<Dictionary<string, object>> GetTableData(TableQueryModel queryModel)
        {
            var model = Context.Models.Find(queryModel.ModelId);
            if (model == null) { throw new Exception("Model not found"); }
            var query = $" EVALUATE TOPN(1000,'{queryModel.TableName}' ";
            if (!string.IsNullOrEmpty(queryModel.OrderBy))
            {
                query += $",[{queryModel.OrderBy}]";
                if (queryModel.OrderDesc)
                {
                    query += ", 0";
                }
                else
                {
                    query += ", 1";
                }
            }
            query += ")";

            var conStrBuilder = new OleDbConnectionStringBuilder(_asConnectionString)
            {
                ["Catalog"] = model.DatabaseName
            };
            using (var conn = new AC.AdomdConnection(conStrBuilder.ToString()))
            {
                try
                {
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = query;
                    using (var reader = command.ExecuteReader())
                    {
                        var result = new List<Dictionary<string, object>>();
                        while (reader.Read())
                        {
                            var row = new Dictionary<string, object>();
                            var columns = new List<string>();
                            for (var i = 0; i < reader.FieldCount; ++i)
                            {
                                var name = reader.GetName(i);
                                var si = name.IndexOf("[") + 1;
                                var ei = name.IndexOf("]");
                                columns.Add(name.Substring(si, ei - si));
                            }
                            for (var i = 0; i < reader.FieldCount; ++i)
                            {
                                row[columns[i]] = reader.GetValue(i);
                            }
                            result.Add(row);
                        }
                        return result;
                    }
                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }

        }

        public void GenerateModelFromDatabase(int modelId)
        {
            var mo = Context.Models.Where(x => x.Id == modelId)
                .Include(x => x.Tables)
                .Include(x => x.Relationships)
                .Include(x => x.DataSources).FirstOrDefault();
            if (mo == null)
            {
                throw new Exception("Model not found");
            }
            mo.State = DataModelObjectState.Unchanged;
            Context.Tables.RemoveRange(mo.Tables);
            Context.DataSources.RemoveRange(mo.DataSources);
            Context.Relationships.RemoveRange(mo.Relationships);
            Context.SaveChanges();
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                AS.Database database = server.Databases.Find(mo.DatabaseName);
                if (database == null)
                {
                    throw new Exception("Database not found");
                }
                mo.DefaultMode = (ModeType)database.Model.DefaultMode;
                mo.Description = database.Model.Description;

                //Data Sources
                var dataSources = database.Model.DataSources;
                foreach (AS.ProviderDataSource ds in dataSources)
                {
                    var conStrBuilder = new OleDbConnectionStringBuilder(ds.ConnectionString);
                    mo.DataSources.Add(new DataSource
                    {
                        Name = ds.Name,
                        Description = ds.Description,
                        OriginalName = ds.Name,
                        ConnectionString = ds.ConnectionString,
                        State = DataModelObjectState.Unchanged
                    });
                }

                //Tables
                var tables = database.Model.Tables;
                foreach (AS.Table tb in tables)
                {
                    var table = new Table
                    {
                        Name = tb.Name,
                        Description = tb.Description,
                        OriginalName = tb.Name,
                        State = DataModelObjectState.Unchanged,
                        Partitions = new List<Partition>(),
                        Measures = new List<Measure>(),
                        Columns = new List<Column>()
                    };
                    mo.Tables.Add(table);

                    foreach (AS.Partition pa in tb.Partitions)
                    {
                        if (pa.Source is AS.CalculatedPartitionSource)
                        {
                            var source = pa.Source as AS.CalculatedPartitionSource;
                            table.Partitions.Add(new Partition
                            {
                                Name = pa.Name,
                                Description = pa.Description,
                                Query = source.Expression,
                                State = DataModelObjectState.Unchanged
                            });
                        }
                        else if (pa.Source is AS.QueryPartitionSource)
                        {
                            var source = pa.Source as AS.QueryPartitionSource;
                            table.Partitions.Add(new Partition
                            {
                                Name = pa.Name,
                                Description = pa.Description,
                                Query = source.Query,
                                State = DataModelObjectState.Unchanged
                            });
                        }
                    }

                    foreach (AS.Column co in tb.Columns)
                    {

                        if (co.Type == AS.ColumnType.RowNumber)
                        {
                            continue;
                        }
                        table.Columns.Add(new Column
                        {
                            Name = co.Name,
                            DataType = (ColumnDataType)co.DataType,
                            Description = co.Description,
                            IsHidden = co.IsHidden,
                            DisplayFolder = co.DisplayFolder,
                            FormatString = co.FormatString
                        });
                    }

                    foreach (AS.Measure me in tb.Measures)
                    {
                        table.Measures.Add(new Measure
                        {
                            Name = me.Name,
                            Description = me.Description,
                            Expression = me.Expression
                        });
                    }
                }

                //Relationships
                var relationships = database.Model.Relationships;
                foreach (AS.SingleColumnRelationship re in relationships)
                {
                    var relationship = new Relationship();
                    foreach (var table in mo.Tables)
                    {
                        if (re.FromTable.Name == table.Name)
                        {
                            foreach (var col in table.Columns)
                            {
                                if (re.FromColumn.Name == col.Name)
                                {
                                    relationship.FromColumn = col;
                                }
                            }
                        }
                        if (re.ToTable.Name == table.Name)
                        {
                            foreach (var col in table.Columns)
                            {
                                if (re.ToColumn.Name == col.Name)
                                {
                                    relationship.ToColumn = col;
                                }
                            }
                        }
                    }
                    mo.Relationships.Add(relationship);
                }

                //Commit changes
                server.Disconnect();
                Context.SaveChanges();
            }
        }

        public SaveResult SaveChanges(JObject saveBundle)
        {
            var txSettings = new TransactionSettings { TransactionType = TransactionType.TransactionScope };
            _contextProvider.BeforeSaveEntityDelegate += BeforeSaveEntity;
            return _contextProvider.SaveChanges(saveBundle, txSettings);
        }

        protected bool BeforeSaveEntity(EntityInfo info)
        {
            if (info.Entity is IDataModelObject)
            {
                var entity = info.Entity as IDataModelObject;
                if (entity == null)
                {
                    return true;
                }
                switch (info.EntityState)
                {
                    case Breeze.ContextProvider.EntityState.Added:
                        entity.State = DataModelObjectState.Added;
                        if (info.Entity is Model)
                        {
                            var mo = info.Entity as Model;
                            if (mo != null)
                            {
                                mo.DatabaseName = Guid.NewGuid().ToString();
                            }
                        }
                        entity.OriginalName = entity.Name;
                        break;
                    case Breeze.ContextProvider.EntityState.Modified:
                        if (entity.State == DataModelObjectState.Modified ||
                            entity.State == DataModelObjectState.Unchanged)
                        {
                            entity.State = DataModelObjectState.Modified;
                            info.OriginalValuesMap["State"] = null;
                        }
                        break;
                    case Breeze.ContextProvider.EntityState.Deleted:
                        info.EntityState = Breeze.ContextProvider.EntityState.Modified;
                        if (entity.State != DataModelObjectState.Added)
                        {
                            entity.State = DataModelObjectState.Deleted;
                        }
                        else
                        {
                            entity.State = DataModelObjectState.Obsolete;
                        }
                        info.OriginalValuesMap["State"] = null;
                        break;
                    default:
                        break;
                }
            }
            return true;
        }

        //protected Dictionary<Type, List<EntityInfo>> BeforeSaveEntities(Dictionary<Type, List<EntityInfo>> saveMap)
        //{
        //    List<EntityInfo> dsInfo = null;
        //    List<EntityInfo> tbInfo = null;
        //    List<int> deletedDataSources = null;
        //    List<int> deletedTables = null;

        //    if (saveMap.TryGetValue(typeof(DataSource), out dsInfo))
        //    {
        //        deletedDataSources = dsInfo.Select(x => x.Entity as DataSource)
        //            .Where(x => x.State == DataModelObjectState.Deleted).Select(x => x.Id).ToList();
        //    }
        //    if (saveMap.TryGetValue(typeof(Table), out tbInfo))
        //    {
        //        if (deletedDataSources != null)
        //        {
        //            tbInfo.Where(x => deletedDataSources.Contains((x.Entity as Table).DataSourceId)).ToList()
        //                .ForEach(x =>
        //                {
        //                    x.EntityState = Breeze.ContextProvider.EntityState.Modified;
        //                    (x.Entity as Table).State = DataModelObjectState.Deleted;
        //                });
        //        }
        //        deletedTables = tbInfo.Select(x => x.Entity as Table)
        //                .Where(x => x.State == DataModelObjectState.Deleted).Select(x => x.Id).ToList();
        //    }

        //    return saveMap;
        //}

        //protected void AfterSaveEntities(Dictionary<Type, List<EntityInfo>> saveMap, List<KeyMapping> keyMappings)
        //{
        //    List<EntityInfo> info;
        //    if (saveMap.TryGetValue(typeof(DataSource), out info))
        //    {
        //        var deletedDataSource = info.Select(x => x.Entity as DataSource)
        //            .Where(x => x.State == DataModelObjectState.Deleted).Select(x => x.Id)
        //            .ToList();
        //        if (deletedDataSource.Count > 0)
        //        {

        //        }
        //    }
        //}

        public string GetDataSourceConnectionString(DataSource ds)
        {
            string cs;
            switch (ds.Type)
            {
                case DataSourceType.SqlServer:
                    if (ds.IntegratedSecurity)
                    {
                        cs = $"Provider=SQLNCLI11;Data Source={ds.Source};Initial Catalog={ds.Catalog};Integrated Security=SSPI;Persist Security Info=false";
                    }
                    else
                    {
                        cs = $"Provider=SQLNCLI11;Data Source={ds.Source};Initial Catalog={ds.Catalog};User ID={ds.User};Password={ds.Password};Persist Security Info=true";
                    }
                    break;
                case DataSourceType.Excel:
                    if (ds.SourceFileId != null && ds.SourceFile == null)
                    {
                        ds.SourceFile = Context.SourceFiles.Find(ds.SourceFileId);
                    }
                    var builder = new OleDbConnectionStringBuilder()
                    {
                        Provider = "Microsoft.ACE.OLEDB.12.0",
                        DataSource = ds.SourceFile.FilePath,
                        PersistSecurityInfo = false
                    };
                    builder["Mode"] = "Read";
                    var extension = Path.GetExtension(ds.SourceFile.FileName).ToUpper();
                    switch (extension)
                    {
                        case ".XLS":
                            builder["Extended Properties"] = "Excel 8.0;HDR=Yes";
                            break;
                        case ".XLSB":
                            builder["Extended Properties"] = "Excel 12.0;HDR=Yes";
                            break;
                        case ".XLSX":
                            builder["Extended Properties"] = "Excel 12.0 Xml;HDR=Yes";
                            break;
                        case ".XLSM":
                            builder["Extended Properties"] = "Excel 12.0 Macro;HDR=Yes";
                            break;
                        default:
                            builder["Extended Properties"] = "Excel 12.0;HDR=Yes";
                            break;
                    }
                    cs = builder.ToString();
                    break;
                default: return null;
            }
            return cs;
        }

    }
}