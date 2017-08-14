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
using System.Data.SqlClient;
using System.Web;
using AN = Microsoft.AnalysisServices;
using System.Diagnostics;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Data;

namespace Pentamic.SSBI.Services
{
    public class DataModelService
    {
        private EFContextProvider<DataModelContext> _contextProvider;
        private string _asConnectionString = System.Configuration.ConfigurationManager
                .ConnectionStrings["AnalysisServiceConnection"]
                .ConnectionString;

        private List<RenameRequest> renameRequests;

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
            get { return Context.Models; }
        }
        public IQueryable<DataSource> DataSources
        {
            get { return Context.DataSources; }
        }
        public IQueryable<Table> Tables
        {
            get { return Context.Tables; }
        }
        public IQueryable<Column> Columns
        {
            get { return Context.Columns; }
        }
        public IQueryable<Partition> Partitions
        {
            get { return Context.Partitions; }
        }
        public IQueryable<Measure> Measures
        {
            get { return Context.Measures; }
        }
        public IQueryable<Relationship> Relationships
        {
            get { return Context.Relationships; }
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

        public async Task<SourceFile> HandleFileUpload(MultipartFormDataStreamProvider provider)
        {
            var file = provider.FileData[0];
            var sourceFile = new SourceFile
            {
                FileName = file.Headers.ContentDisposition.FileName.Replace("\"", ""),
                FilePath = Path.GetFileName(file.LocalFileName)
            };
            Context.SourceFiles.Add(sourceFile);
            await Context.SaveChangesAsync();
            return sourceFile;
        }

        //public void DeployModel(int modelId)
        //{
        //    var mo = Context.Models.Find(modelId);
        //    if (mo == null)
        //    {
        //        throw new Exception("Model is null");
        //    }
        //    using (var server = new AS.Server())
        //    {
        //        server.Connect(_asConnectionString);
        //        if (mo.State == DataModelObjectState.Deleted)
        //        {
        //            server.Databases.Remove(mo.DatabaseName);
        //            mo.State = DataModelObjectState.Obsolete;
        //            return;
        //        }
        //        AS.Database database = null;
        //        if (mo.State == DataModelObjectState.Added)
        //        {
        //            database = new AS.Database
        //            {
        //                ID = mo.DatabaseName,
        //                Name = mo.DatabaseName,
        //                StorageEngineUsed = Microsoft.AnalysisServices.StorageEngineUsed.TabularMetadata,
        //                CompatibilityLevel = 1200
        //            };
        //            database.Model = new AS.Model
        //            {
        //                Name = mo.Name,
        //                Description = mo.Description,
        //                DefaultMode = mo.DefaultMode.ToModeType()
        //            };
        //            server.Databases.Add(database);
        //            mo.State = DataModelObjectState.Unchanged;
        //        }
        //        else
        //        {
        //            database = server.Databases.Find(mo.DatabaseName);
        //            if (mo.State == DataModelObjectState.Modified)
        //            {

        //                if (database.Model.Name != mo.Name)
        //                {
        //                    database.Model.RequestRename(mo.Name);
        //                    database.Update(Microsoft.AnalysisServices.UpdateOptions.ExpandFull);
        //                    if (database.Model.Description != mo.Description)
        //                    {
        //                        database.Model.Description = mo.Description;
        //                    }
        //                    mo.State = DataModelObjectState.Unchanged;
        //                    Context.SaveChanges();
        //                }
        //                else
        //                {
        //                    if (database.Model.Description != mo.Description)
        //                    {
        //                        database.Model.Description = mo.Description;
        //                    }
        //                    mo.State = DataModelObjectState.Unchanged;
        //                }
        //            }
        //        }
        //        //Data Sources
        //        var dataSources = Context.DataSources.Where(x => x.ModelId == modelId).ToList();
        //        foreach (var ds in dataSources)
        //        {
        //            if (ds.State == DataModelObjectState.Deleted)
        //            {
        //                database.Model.DataSources.Remove(ds.OriginalName);
        //                ds.State = DataModelObjectState.Obsolete;
        //            }
        //            AS.ProviderDataSource dataSource = null;
        //            if (ds.State == DataModelObjectState.Added)
        //            {
        //                dataSource = new AS.ProviderDataSource
        //                {
        //                    Name = ds.Name,
        //                    Description = ds.Description,
        //                    ConnectionString = GetDataSourceConnectionString(ds),
        //                    ImpersonationMode = Microsoft.AnalysisServices.Tabular.ImpersonationMode.ImpersonateServiceAccount
        //                    //ImpersonationMode = Microsoft.AnalysisServices.Tabular.ImpersonationMode.ImpersonateAccount,
        //                    //Account = "datht.it@live.com",
        //                    //Password = "20111988Gi@ng"
        //                };
        //                database.Model.DataSources.Add(dataSource);
        //                ds.State = DataModelObjectState.Unchanged;
        //            }
        //            else
        //            {
        //                dataSource = database.Model.DataSources.Find(ds.OriginalName) as AS.ProviderDataSource;
        //                if (ds.State == DataModelObjectState.Modified)
        //                {
        //                    if (dataSource.Name != ds.Name)
        //                    {
        //                        dataSource.RequestRename(ds.Name);
        //                        ds.OriginalName = ds.Name;
        //                    }
        //                    if (dataSource.Description != ds.Description)
        //                    {
        //                        dataSource.Description = ds.Description;
        //                    }
        //                    var newConStr = GetDataSourceConnectionString(ds);
        //                    if (newConStr != dataSource.ConnectionString)
        //                    {
        //                        dataSource.ConnectionString = newConStr;
        //                    }
        //                    ds.State = DataModelObjectState.Unchanged;
        //                }
        //            }
        //        }

        //        var tables = Context.Tables.Where(x => x.ModelId == modelId)
        //            .Include(x => x.Partitions)
        //            .Include(x => x.Columns)
        //            .Include(x => x.Measures).ToList();
        //        foreach (var tb in tables)
        //        {
        //            AS.Table table = null;
        //            if (tb.State == DataModelObjectState.Added)
        //            {
        //                table = new AS.Table
        //                {
        //                    Name = tb.Name,
        //                    Description = tb.Description
        //                };
        //                database.Model.Tables.Add(table);
        //                tb.State = DataModelObjectState.Unchanged;
        //            }
        //            else
        //            {
        //                table = database.Model.Tables.Find(tb.OriginalName);
        //                if (tb.State == DataModelObjectState.Modified)
        //                {
        //                    if (tb.Name != table.Name)
        //                    {
        //                        table.RequestRename(tb.Name);
        //                        tb.OriginalName = tb.Name;
        //                    }
        //                    if (tb.Description != table.Description)
        //                    {
        //                        table.Description = tb.Description;
        //                    }
        //                    tb.State = DataModelObjectState.Unchanged;
        //                }
        //            }

        //            foreach (var pa in tb.Partitions)
        //            {
        //                var ds = pa.DataSource;
        //                if (ds.State == DataModelObjectState.Obsolete || tb.State == DataModelObjectState.Obsolete || pa.State == DataModelObjectState.Deleted)
        //                {
        //                    if (tb.State != DataModelObjectState.Obsolete)
        //                    {
        //                        table.Partitions.Remove(pa.OriginalName);
        //                    }
        //                    pa.State = DataModelObjectState.Obsolete;
        //                }
        //                AS.Partition partition = null;
        //                if (pa.State == DataModelObjectState.Added)
        //                {
        //                    table.Partitions.Add(new AS.Partition
        //                    {
        //                        Name = pa.Name,
        //                        Source = new AS.QueryPartitionSource()
        //                        {
        //                            DataSource = database.Model.DataSources[ds.Name],
        //                            Query = pa.Query
        //                        }
        //                    });
        //                    pa.State = DataModelObjectState.Unchanged;
        //                }
        //                if (pa.State == DataModelObjectState.Modified)
        //                {
        //                    partition = table.Partitions.Find(pa.OriginalName);
        //                    if (partition.Name != pa.Name)
        //                    {
        //                        partition.RequestRename(pa.Name);
        //                        pa.OriginalName = pa.Name;
        //                    }
        //                    var source = partition.Source as AS.QueryPartitionSource;
        //                    if (source.Query != pa.Query)
        //                    {
        //                        source.Query = pa.Query;
        //                    }
        //                    pa.State = DataModelObjectState.Unchanged;
        //                }
        //            }

        //            foreach (var co in tb.Columns)
        //            {
        //                if (tb.State == DataModelObjectState.Obsolete || co.State == DataModelObjectState.Deleted)
        //                {
        //                    if (tb.State != DataModelObjectState.Obsolete)
        //                    {
        //                        table.Columns.Remove(co.OriginalName);
        //                    }
        //                    co.State = DataModelObjectState.Obsolete;
        //                }
        //                if (co.State == DataModelObjectState.Added)
        //                {
        //                    table.Columns.Add(new Column
        //                    {
        //                        Name = co.Name,
        //                        DataType = co.DataType.ToDataType(),
        //                        SourceColumn = co.SourceColumn,
        //                        IsHidden = co.IsHidden,
        //                        DisplayFolder = co.DisplayFolder,
        //                        FormatString = co.FormatString
        //                    });
        //                    co.State = DataModelObjectState.Unchanged;
        //                }
        //                if (co.State == DataModelObjectState.Modified)
        //                {
        //                    var column = table.Columns.Find(co.OriginalName);
        //                    if (co.Name != column.Name)
        //                    {
        //                        column.RequestRename(co.Name);
        //                        co.OriginalName = co.Name;
        //                    }
        //                    if (co.IsHidden != column.IsHidden)
        //                    {
        //                        column.IsHidden = co.IsHidden;
        //                    }
        //                    if (co.DisplayFolder != column.DisplayFolder)
        //                    {
        //                        column.DisplayFolder = co.DisplayFolder;
        //                    }
        //                    if (co.FormatString != column.FormatString)
        //                    {
        //                        column.FormatString = co.FormatString;
        //                    }
        //                    co.State = DataModelObjectState.Unchanged;
        //                }
        //            }

        //            foreach (var me in tb.Measures)
        //            {
        //                if (tb.State == DataModelObjectState.Obsolete || me.State == DataModelObjectState.Deleted)
        //                {
        //                    if (tb.State != DataModelObjectState.Obsolete)
        //                    {
        //                        table.Measures.Remove(me.OriginalName);
        //                    }
        //                    me.State = DataModelObjectState.Obsolete;
        //                }
        //                if (me.State == DataModelObjectState.Added)
        //                {
        //                    table.Measures.Add(new AS.Measure
        //                    {
        //                        Name = me.Name,
        //                        Expression = me.Expression
        //                    });
        //                    me.State = DataModelObjectState.Unchanged;
        //                }
        //                if (me.State == DataModelObjectState.Modified)
        //                {
        //                    var measure = table.Measures.Find(me.OriginalName);
        //                    if (measure.Name != me.Name)
        //                    {
        //                        measure.RequestRename(me.Name);
        //                        me.OriginalName = me.Name;
        //                    }
        //                    if (measure.Expression != me.Expression)
        //                    {
        //                        measure.Expression = me.Expression;
        //                    }
        //                    if (measure.Description != me.Description)
        //                    {
        //                        measure.Description = me.Description;
        //                    }
        //                    me.State = DataModelObjectState.Unchanged;
        //                }
        //            }
        //        }

        //        //Relationships
        //        var relationships = Context.Relationships.Where(x => x.ModelId == modelId && x.State != DataModelObjectState.Obsolete)
        //            .Include(x => x.FromColumn.Table).Include(x => x.ToColumn.Table).ToList();
        //        foreach (var re in relationships)
        //        {
        //            if (re.ToColumn.State == DataModelObjectState.Obsolete ||
        //                re.ToColumn.Table.State == DataModelObjectState.Obsolete ||
        //                re.FromColumn.State == DataModelObjectState.Obsolete ||
        //                re.FromColumn.Table.State == DataModelObjectState.Obsolete ||
        //                re.State == DataModelObjectState.Deleted)
        //            {
        //                database.Model.Relationships.Remove(re.OriginalName);
        //                re.State = DataModelObjectState.Obsolete;
        //            }
        //            if (re.State == DataModelObjectState.Added)
        //            {
        //                database.Model.Relationships.Add(new AS.SingleColumnRelationship()
        //                {
        //                    Name = re.Name,
        //                    ToColumn = database.Model.Tables[re.ToColumn.Table.Name].Columns[re.ToColumn.Name],
        //                    FromColumn = database.Model.Tables[re.FromColumn.Table.Name].Columns[re.FromColumn.Name],
        //                    FromCardinality = AS.RelationshipEndCardinality.Many,
        //                    ToCardinality = AS.RelationshipEndCardinality.One
        //                });
        //                re.State = DataModelObjectState.Unchanged;
        //            }
        //            if (re.State == DataModelObjectState.Modified)
        //            {
        //                var relationship = database.Model.Relationships.Find(re.OriginalName);
        //                if (re.Name != relationship.Name)
        //                {
        //                    relationship.RequestRename(re.Name);
        //                    re.OriginalName = re.Name;
        //                }
        //                re.State = DataModelObjectState.Unchanged;
        //            }
        //        }

        //        //Commit changes
        //        database.Update(Microsoft.AnalysisServices.UpdateOptions.ExpandFull);
        //        Context.SaveChanges();
        //    }
        //}

        public void RefreshModel(int modelId)
        {
            var mo = Context.Models.Find(modelId);
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases[mo.DatabaseName];
                database.Model.RequestRefresh(AS.RefreshType.Full);
                database.Update(AN.UpdateOptions.ExpandFull);
            }
        }

        public void RefreshTable(int tableId)
        {
            var tb = Context.Tables.Where(x => x.Id == tableId)
                .Include(x => x.Model)
                .FirstOrDefault();
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases[tb.Model.DatabaseName];
                var table = database.Model.Tables[tb.Name];
                table.RequestRefresh(AS.RefreshType.Full);
                database.Update(Microsoft.AnalysisServices.UpdateOptions.ExpandFull);
            }
        }

        public void RefreshPartition(int partitionId)
        {
            var pa = Context.Partitions.Where(x => x.Id == partitionId)
                .Include(x => x.Table.Model)
                .FirstOrDefault();
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases[pa.Table.Model.DatabaseName];
                var partition = database.Model.Tables[pa.Table.Name].Partitions[pa.Name];
                partition.RequestRefresh(AS.RefreshType.Full);
                database.Update(Microsoft.AnalysisServices.UpdateOptions.ExpandFull);
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

        public void CloneModel(Model mo, int modelId)
        {
            var fromModel = Context.Models.Find(modelId);
            if (fromModel == null)
            {
                throw new Exception("Clone model not found");
            }
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                AS.Database database = server.Databases.Find(fromModel.DatabaseName);
                if (database == null)
                {
                    throw new Exception("Database not found");
                }
                if (database.CompatibilityLevel < 1200)
                {
                    throw new Exception("Database not supported");
                }
                var newDb = database.Clone();
                newDb.ID = mo.DatabaseName;
                newDb.Name = mo.DatabaseName;
                newDb.Model = database.Model.Clone();
                newDb.Model.Name = mo.Name;
                newDb.Model.Description = mo.Description;
                server.Databases.Add(newDb);
                mo.DataSources = new List<DataSource>();
                mo.Tables = new List<Table>();
                mo.Relationships = new List<Relationship>();
                mo.DefaultMode = (ModeType)database.Model.DefaultMode;
                mo.Description = database.Model.Description;

                //Data Sources
                var dataSources = database.Model.DataSources;
                foreach (AS.ProviderDataSource ds in dataSources)
                {
                    ds.ImpersonationMode = AS.ImpersonationMode.ImpersonateServiceAccount;
                    var conStrBuilder = new OleDbConnectionStringBuilder(ds.ConnectionString);
                    var dataSource = new DataSource
                    {
                        Name = ds.Name,
                        Description = ds.Description,
                        OriginalName = ds.Name,
                        ConnectionString = ds.ConnectionString,
                        Source = conStrBuilder.DataSource
                    };
                    conStrBuilder.TryGetValue("Integrated Security", out object val);
                    if (val != null)
                    {
                        if (val is bool)
                        {
                            dataSource.IntegratedSecurity = (bool)val;
                        }
                        if (val is string)
                        {
                            if ((string)val == "SSPI" || (string)val == "true")
                            {
                                dataSource.IntegratedSecurity = true;
                            }
                        }
                    }
                    conStrBuilder.TryGetValue("Initial Catalog", out val);
                    if (val != null)
                    {
                        dataSource.Catalog = (string)val;
                    }
                    conStrBuilder.TryGetValue("User ID", out val);
                    if (val != null)
                    {
                        dataSource.User = (string)val;
                    }
                    conStrBuilder.TryGetValue("Password", out val);
                    if (val != null)
                    {
                        dataSource.Password = (string)val;
                    }
                    mo.DataSources.Add(dataSource);
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
                                OriginalName = pa.Name,
                                Description = pa.Description,
                                Query = source.Expression,
                                SourceType = PartitionSourceType.Calculated
                            });
                        }
                        else if (pa.Source is AS.QueryPartitionSource)
                        {
                            var source = pa.Source as AS.QueryPartitionSource;
                            table.Partitions.Add(new Partition
                            {
                                Name = pa.Name,
                                OriginalName = pa.Name,
                                Description = pa.Description,
                                Query = source.Query,
                                SourceType = PartitionSourceType.Query
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
                            OriginalName = co.Name,
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
                            OriginalName = me.Name,
                            Description = me.Description,
                            Expression = me.Expression
                        });
                    }
                }

                //Relationships
                var relationships = database.Model.Relationships;
                foreach (AS.SingleColumnRelationship re in relationships)
                {
                    var relationship = new Relationship
                    {
                        Name = re.Name,
                        OriginalName = re.Name
                    };
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
                newDb.Update(AN.UpdateOptions.ExpandFull);
                Context.SaveChanges();
            }
        }

        public void GenerateModelFromDatabase(Model mo)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                AS.Database database = server.Databases.Find(mo.GenerateFromTemplate);
                if (database == null)
                {
                    throw new Exception("Database not found");
                }
                if (database.CompatibilityLevel < 1200)
                {
                    throw new Exception("Database not supported");
                }
                var newDb = database.Clone();
                newDb.ID = mo.DatabaseName;
                newDb.Name = mo.DatabaseName;
                newDb.Model = database.Model.Clone();
                newDb.Model.Name = mo.Name;
                newDb.Model.Description = mo.Description;
                server.Databases.Add(newDb);
                mo.DataSources = new List<DataSource>();
                mo.Tables = new List<Table>();
                mo.Relationships = new List<Relationship>();
                mo.DefaultMode = (ModeType)database.Model.DefaultMode;
                mo.Description = database.Model.Description;

                //Data Sources
                var dataSources = database.Model.DataSources;
                foreach (AS.ProviderDataSource ds in dataSources)
                {
                    ds.ImpersonationMode = AS.ImpersonationMode.ImpersonateServiceAccount;
                    var conStrBuilder = new OleDbConnectionStringBuilder(ds.ConnectionString);
                    var dataSource = new DataSource
                    {
                        Name = ds.Name,
                        Description = ds.Description,
                        OriginalName = ds.Name,
                        ConnectionString = ds.ConnectionString,
                        Source = conStrBuilder.DataSource
                    };
                    conStrBuilder.TryGetValue("Integrated Security", out object val);
                    if (val != null)
                    {
                        if (val is bool)
                        {
                            dataSource.IntegratedSecurity = (bool)val;
                        }
                        if (val is string)
                        {
                            if ((string)val == "SSPI" || (string)val == "true")
                            {
                                dataSource.IntegratedSecurity = true;
                            }
                        }
                    }
                    conStrBuilder.TryGetValue("Initial Catalog", out val);
                    if (val != null)
                    {
                        dataSource.Catalog = (string)val;
                    }
                    conStrBuilder.TryGetValue("User ID", out val);
                    if (val != null)
                    {
                        dataSource.User = (string)val;
                    }
                    conStrBuilder.TryGetValue("Password", out val);
                    if (val != null)
                    {
                        dataSource.Password = (string)val;
                    }
                    mo.DataSources.Add(dataSource);
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
                                OriginalName = pa.Name,
                                Description = pa.Description,
                                Query = source.Expression,
                                SourceType = PartitionSourceType.Calculated
                            });
                        }
                        else if (pa.Source is AS.QueryPartitionSource)
                        {
                            var source = pa.Source as AS.QueryPartitionSource;
                            table.Partitions.Add(new Partition
                            {
                                Name = pa.Name,
                                OriginalName = pa.Name,
                                Description = pa.Description,
                                Query = source.Query,
                                SourceType = PartitionSourceType.Query
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
                            OriginalName = co.Name,
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
                            OriginalName = me.Name,
                            Description = me.Description,
                            Expression = me.Expression
                        });
                    }
                }

                //Relationships
                var relationships = database.Model.Relationships;
                foreach (AS.SingleColumnRelationship re in relationships)
                {
                    var relationship = new Relationship
                    {
                        Name = re.Name,
                        OriginalName = re.Name
                    };
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
                newDb.Update(AN.UpdateOptions.ExpandFull);
                Context.SaveChanges();
            }
        }

        public SaveResult SaveChanges(JObject saveBundle)
        {
            renameRequests = new List<RenameRequest>();
            var txSettings = new TransactionSettings { TransactionType = TransactionType.TransactionScope };
            _contextProvider.BeforeSaveEntityDelegate += BeforeSaveEntity;
            _contextProvider.BeforeSaveEntitiesDelegate += BeforeSaveEntities;
            _contextProvider.AfterSaveEntitiesDelegate += AfterSaveEntities;
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
                        if (info.Entity is Model)
                        {
                            if (info.Entity is Model mo)
                            {
                                mo.DatabaseName = Guid.NewGuid().ToString();
                                info.OriginalValuesMap["DatabaseName"] = null;
                            }
                        }
                        entity.OriginalName = entity.Name;
                        info.OriginalValuesMap["OriginalName"] = null;
                        break;
                    case Breeze.ContextProvider.EntityState.Modified:
                        if (entity.OriginalName != entity.Name)
                        {
                            renameRequests.Add(new RenameRequest
                            {
                                Id = entity.Id,
                                Name = entity.Name,
                                OriginalName = entity.OriginalName,
                                Type = entity.GetType()
                            });
                        }
                        entity.OriginalName = entity.Name;
                        info.OriginalValuesMap["OriginalName"] = null;
                        break;
                    default:
                        break;
                }
            }
            return true;
        }

        protected Dictionary<Type, List<EntityInfo>> BeforeSaveEntities(Dictionary<Type, List<EntityInfo>> saveMap)
        {
            List<EntityInfo> eis;
            if (saveMap.TryGetValue(typeof(DataSource), out eis))
            {
                var names = eis.Where(x => x.EntityState == Breeze.ContextProvider.EntityState.Added).Select(x => x.Entity as DataSource)
                    .GroupBy(x => x.ModelId).Select(x => new
                    {
                        ModelId = x.Key,
                        Names = x.Select(y => y.Name).ToList()
                    });
                foreach (var group in names)
                {
                    if (CheckDataSourceNamesExist(group.ModelId, group.Names))
                    {
                        throw new Exception("Data source name exist");
                    }
                }
            }
            if (saveMap.TryGetValue(typeof(Table), out eis))
            {
                var names = eis.Where(x => x.EntityState == Breeze.ContextProvider.EntityState.Added).Select(x => x.Entity as Table)
                   .GroupBy(x => x.ModelId).Select(x => new
                   {
                       ModelId = x.Key,
                       Names = x.Select(y => y.Name).ToList()
                   });
                foreach (var group in names)
                {
                    if (CheckTableNamesExist(group.ModelId, group.Names))
                    {
                        throw new Exception("Table name exist");
                    }
                }
            }
            if (saveMap.TryGetValue(typeof(Column), out eis))
            {
                var names = eis.Where(x => x.EntityState == Breeze.ContextProvider.EntityState.Added).Select(x => x.Entity as Column)
                   .GroupBy(x => x.TableId).Select(x => new
                   {
                       TableId = x.Key,
                       Names = x.Select(y => y.Name).ToList()
                   });
                foreach (var group in names)
                {
                    if (CheckColumnNamesExist(group.TableId, group.Names))
                    {
                        throw new Exception("Column name exist");
                    }
                }
            }
            if (saveMap.TryGetValue(typeof(Partition), out eis))
            {
                var names = eis.Where(x => x.EntityState == Breeze.ContextProvider.EntityState.Added).Select(x => x.Entity as Partition)
                   .GroupBy(x => x.TableId).Select(x => new
                   {
                       TableId = x.Key,
                       Names = x.Select(y => y.Name).ToList()
                   });
                foreach (var group in names)
                {
                    if (CheckPartitionNamesExist(group.TableId, group.Names))
                    {
                        throw new Exception("Partition name exist");
                    }
                }
            }
            if (saveMap.TryGetValue(typeof(Measure), out eis))
            {
                var names = eis.Where(x => x.EntityState == Breeze.ContextProvider.EntityState.Added).Select(x => x.Entity as Measure)
                   .GroupBy(x => x.TableId).Select(x => new
                   {
                       TableId = x.Key,
                       Names = x.Select(y => y.Name).ToList()
                   });
                foreach (var group in names)
                {
                    if (CheckMeasureNamesExist(group.TableId, group.Names))
                    {
                        throw new Exception("Measure name exist");
                    }
                }
            }

            return saveMap;
        }

        protected void AfterSaveEntities(Dictionary<Type, List<EntityInfo>> saveMap, List<KeyMapping> keyMappings)
        {
            ProcessRenameRequests();
            List<EntityInfo> eis;
            if (saveMap.TryGetValue(typeof(Model), out eis))
            {
                foreach (var ei in eis)
                {
                    var e = ei.Entity as Model;
                    if (ei.EntityState == Breeze.ContextProvider.EntityState.Added)
                    {
                        if (!string.IsNullOrEmpty(e.GenerateFromTemplate))
                        {
                            GenerateModelFromDatabase(e);
                        }
                        else if (e.CloneFromModelId != null)
                        {

                        }
                        else
                        {
                            CreateModel(e);
                        }
                    }
                    if (ei.EntityState == Breeze.ContextProvider.EntityState.Modified)
                    {
                        UpdateModel(e);
                    }
                    if (ei.EntityState == Breeze.ContextProvider.EntityState.Deleted)
                    {
                        DeleteModel(e);
                    }
                }
            }
            if (saveMap.TryGetValue(typeof(DataSource), out eis))
            {
                foreach (var ei in eis)
                {
                    var e = ei.Entity as DataSource;
                    if (ei.EntityState == Breeze.ContextProvider.EntityState.Added)
                    {
                        CreateDataSource(e);
                    }
                    if (ei.EntityState == Breeze.ContextProvider.EntityState.Modified)
                    {
                        UpdateDataSource(e);
                    }
                    if (ei.EntityState == Breeze.ContextProvider.EntityState.Deleted)
                    {
                        DeleteDataSource(e);
                    }
                }
            }
            if (saveMap.TryGetValue(typeof(Table), out eis))
            {
                var newTables = eis.Where(x => x.EntityState == Breeze.ContextProvider.EntityState.Added)
                    .Select(x => x.Entity as Table)
                    .GroupBy(x => x.ModelId).Select(x => new
                    {
                        ModelId = x.Key,
                        Tables = x.ToList()
                    });
                foreach (var group in newTables)
                {
                    CreateTables(group.ModelId, group.Tables);
                }
                foreach (var ei in eis)
                {
                    var e = ei.Entity as Table;
                    if (ei.EntityState == Breeze.ContextProvider.EntityState.Modified)
                    {
                        UpdateTable(e);
                    }
                    if (ei.EntityState == Breeze.ContextProvider.EntityState.Deleted)
                    {
                        DeleteTable(e);
                    }
                }
            }
            if (saveMap.TryGetValue(typeof(Relationship), out eis))
            {
                foreach (var ei in eis)
                {
                    var e = ei.Entity as Relationship;
                    if (ei.EntityState == Breeze.ContextProvider.EntityState.Added)
                    {
                        CreateRelationship(e);
                    }
                    if (ei.EntityState == Breeze.ContextProvider.EntityState.Modified)
                    {
                        UpdateRelationship(e);
                    }
                    if (ei.EntityState == Breeze.ContextProvider.EntityState.Deleted)
                    {
                        DeleteRelationship(e);
                    }
                }
            }
            if (saveMap.TryGetValue(typeof(Column), out eis))
            {
                foreach (var ei in eis)
                {
                    var e = ei.Entity as Column;
                    if (ei.EntityState == Breeze.ContextProvider.EntityState.Modified)
                    {
                        UpdateColumn(e);
                    }
                    if (ei.EntityState == Breeze.ContextProvider.EntityState.Deleted)
                    {
                        DeleteColumn(e);
                    }
                }
            }
            if (saveMap.TryGetValue(typeof(Partition), out eis))
            {
                foreach (var ei in eis)
                {
                    var e = ei.Entity as Partition;
                    if (ei.EntityState == Breeze.ContextProvider.EntityState.Modified)
                    {
                        UpdatePartition(e);
                    }
                }
            }
        }

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
                    var basePath = System.Configuration.ConfigurationManager.AppSettings["UploadBasePath"];
                    if (string.IsNullOrEmpty(basePath))
                    {
                        basePath = HttpContext.Current.Server.MapPath("~/Uploads");
                    }
                    var builder = new OleDbConnectionStringBuilder()
                    {
                        Provider = "Microsoft.ACE.OLEDB.12.0",
                        DataSource = Path.Combine(basePath, ds.SourceFile.FilePath),
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

        public void ProcessRenameRequests()
        {
            foreach (var req in renameRequests)
            {
                if (req.Type == typeof(Model))
                {
                    RenameModel(req.Id, req.OriginalName, req.Name);
                }
                if (req.Type == typeof(DataSource))
                {
                    RenameDataSource(req.Id, req.OriginalName, req.Name);
                }
                if (req.Type == typeof(Table))
                {
                    RenameTable(req.Id, req.OriginalName, req.Name);
                }
                if (req.Type == typeof(Relationship))
                {
                    RenameRelationship(req.Id, req.OriginalName, req.Name);
                }
                if (req.Type == typeof(Column))
                {
                    RenameColumn(req.Id, req.OriginalName, req.Name);
                }
                if (req.Type == typeof(Measure))
                {
                    RenameMeasure(req.Id, req.OriginalName, req.Name);
                }
                if (req.Type == typeof(Partition))
                {
                    RenamePartition(req.Id, req.OriginalName, req.Name);
                }
            }
        }

        public void RenameModel(int id, string oldName, string newName)
        {
            var model = Context.Models.Find(id);
            if (model == null)
            {
                throw new ArgumentException("Model not found");
            }
            try
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases.FindByName(model.DatabaseName);
                    if (database == null)
                    {
                        throw new ArgumentException("Database not found");
                    }
                    database.Model.RequestRename(newName);
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
            catch (AN.OperationException ex)
            {
                foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
                {
                }
                throw;
            }
        }

        public void RenameDataSource(int id, string oldName, string newName)
        {
            var databaseName = Context.DataSources.Where(x => x.Id == id).Select(x => x.Model.DatabaseName).FirstOrDefault();
            if (databaseName == null)
            {
                throw new ArgumentException("Model not found");
            }
            try
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases.FindByName(databaseName);
                    if (database == null)
                    {
                        throw new ArgumentException("Database not found");
                    }
                    var ds = database.Model.DataSources.Find(oldName);
                    if (ds == null)
                    {
                        throw new ArgumentException("Data Source not found");
                    }
                    ds.RequestRename(newName);
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
            catch (AN.OperationException ex)
            {
                foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
                {
                }
                throw;
            }
        }

        public void RenameTable(int id, string oldName, string newName)
        {
            var model = Context.Tables.Where(x => x.Id == id).Select(x => x.Model).FirstOrDefault();
            if (model == null || model.DatabaseName == null)
            {
                throw new ArgumentException("Model not found");
            }
            try
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases.FindByName(model.DatabaseName);
                    if (database == null)
                    {
                        throw new ArgumentException("Database not found");
                    }
                    var tb = database.Model.Tables.Find(oldName);
                    if (tb == null)
                    {
                        throw new ArgumentException("Table not found");
                    }
                    tb.RequestRename(newName);
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
            catch (AN.OperationException ex)
            {
                foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
                {
                }
                throw;
            }
            try
            {
                var cmd = $"UPDATE [Reporting].[ReportTile] SET DataConfig=REPLACE(DataConfig, '\"tableName\":\"{oldName}\"',N'\"tableName\":\"{newName}\"') WHERE ModelId=@modelId";
                Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("modelId", model.Id));
            }
            catch (Exception e) { }
        }

        public void RenameRelationship(int id, string oldName, string newName)
        {
            var databaseName = Context.Relationships.Where(x => x.Id == id).Select(x => x.Model.DatabaseName).FirstOrDefault();
            if (databaseName == null)
            {
                throw new ArgumentException("Model not found");
            }
            try
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases.FindByName(databaseName);
                    if (database == null)
                    {
                        throw new ArgumentException("Database not found");
                    }
                    var re = database.Model.Relationships.Find(oldName);
                    if (re == null)
                    {
                        throw new ArgumentException("Table not found");
                    }
                    re.RequestRename(newName);
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
            catch (AN.OperationException ex)
            {
                foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
                {
                }
                throw;
            }
        }

        public void RenameColumn(int id, string oldName, string newName)
        {
            var info = Context.Columns.Where(x => x.Id == id).Select(x => new
            {
                ModelId = x.Table.ModelId,
                DatabaseName = x.Table.Model.DatabaseName,
                TableName = x.Table.Name
            }).FirstOrDefault();
            if (info.DatabaseName == null)
            {
                throw new ArgumentException("Model not found");
            }
            try
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases.FindByName(info.DatabaseName);
                    if (database == null)
                    {
                        throw new ArgumentException("Database not found");
                    }
                    var tb = database.Model.Tables.Find(info.TableName);
                    if (tb == null)
                    {
                        throw new ArgumentException("Table not found");
                    }
                    var co = tb.Columns.Find(oldName);
                    if (co == null)
                    {
                        throw new ArgumentException("Column not found");
                    }
                    co.RequestRename(newName);
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
            catch (AN.OperationException ex)
            {
                foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
                {
                }
                throw;
            }
            try
            {
                var cmd = $"UPDATE [Reporting].[ReportTile] SET DataConfig=REPLACE(DataConfig, '\"name\":\"{oldName}\"',N'\"name\":\"{newName}\"') WHERE ModelId=@modelId AND DataConfig LIKE '%\"tableName\":\"{info.TableName}\"%'";
                Context.Database.ExecuteSqlCommand(cmd, new SqlParameter("modelId", info.ModelId));
            }
            catch (Exception e) { }
        }

        public void RenameMeasure(int id, string oldName, string newName)
        {
            var info = Context.Measures.Where(x => x.Id == id).Select(x => new
            {
                DatabaseName = x.Table.Model.DatabaseName,
                TableName = x.Table.Name
            }).FirstOrDefault();
            if (info.DatabaseName == null)
            {
                throw new ArgumentException("Model not found");
            }
            try
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases.FindByName(info.DatabaseName);
                    if (database == null)
                    {
                        throw new ArgumentException("Database not found");
                    }
                    var tb = database.Model.Tables.Find(info.TableName);
                    if (tb == null)
                    {
                        throw new ArgumentException("Table not found");
                    }
                    var me = tb.Measures.Find(oldName);
                    if (me == null)
                    {
                        throw new ArgumentException("Measure not found");
                    }
                    me.RequestRename(newName);
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
            catch (AN.OperationException ex)
            {
                foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
                {
                }
                throw;
            }
        }

        public void RenamePartition(int id, string oldName, string newName)
        {
            var info = Context.Partitions.Where(x => x.Id == id).Select(x => new
            {
                DatabaseName = x.Table.Model.DatabaseName,
                TableName = x.Table.Name
            }).FirstOrDefault();
            if (info.DatabaseName == null)
            {
                throw new ArgumentException("Model not found");
            }
            try
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases.FindByName(info.DatabaseName);
                    if (database == null)
                    {
                        throw new ArgumentException("Database not found");
                    }
                    var tb = database.Model.Tables.Find(info.TableName);
                    if (tb == null)
                    {
                        throw new ArgumentException("Table not found");
                    }
                    var pa = tb.Partitions.Find(oldName);
                    if (pa == null)
                    {
                        throw new ArgumentException("Partition not found");
                    }
                    pa.RequestRename(newName);
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
            catch (AN.OperationException ex)
            {
                foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
                {
                }
                throw;
            }
        }

        public void CreateModel(Model model)
        {
            try
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = new AS.Database()
                    {
                        Name = model.DatabaseName,
                        ID = model.DatabaseName,
                        CompatibilityLevel = 1200,
                        StorageEngineUsed = AN.StorageEngineUsed.TabularMetadata,
                    };
                    database.Model = new AS.Model
                    {
                        Name = model.Name,
                        Description = model.Description,
                        DefaultMode = model.DefaultMode.ToModeType()
                    };
                    server.Databases.Add(database);
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
            catch (AN.OperationException ex)
            {
                foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
                {
                }
                throw;
            }
        }

        public void UpdateModel(Model model)
        {
            try
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases.FindByName(model.DatabaseName);
                    if (database == null)
                    {
                        throw new ArgumentException("Database not found");
                    }
                    database.Model.Description = model.Description;
                    database.Model.DefaultMode = model.DefaultMode.ToModeType();
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
            catch (AN.OperationException ex)
            {
                foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
                {
                }
                throw;
            }
        }

        public void DeleteModel(Model model)
        {
            try
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases.FindByName(model.DatabaseName);
                    //if (database == null)
                    //{
                    //    throw new ArgumentException("Database not found");
                    //}
                    if (database != null)
                    {
                        database.Drop();
                    }
                }
            }
            catch (AN.OperationException ex)
            {
                foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
                {
                }
                throw;
            }
        }

        public void CreateDataSource(DataSource dataSource)
        {
            var dbName = Context.Models.Where(x => x.Id == dataSource.ModelId).Select(x => x.DatabaseName).FirstOrDefault();
            try
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases.FindByName(dbName);
                    if (database == null)
                    {
                        throw new ArgumentException("Database not found");
                    }
                    database.Model.DataSources.Add(new AS.ProviderDataSource
                    {
                        Name = dataSource.Name,
                        Description = dataSource.Description,
                        ConnectionString = GetDataSourceConnectionString(dataSource),
                        ImpersonationMode = AS.ImpersonationMode.ImpersonateServiceAccount
                    });
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
            catch (AN.OperationException ex)
            {
                foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
                {
                }
                throw;
            }
        }

        public void AddDataSource(AS.Database database, DataSource dataSource)
        {
            database.Model.DataSources.Add(new AS.ProviderDataSource
            {
                Name = dataSource.Name,
                Description = dataSource.Description,
                ConnectionString = GetDataSourceConnectionString(dataSource),
                ImpersonationMode = AS.ImpersonationMode.ImpersonateServiceAccount
            });
        }


        public void UpdateDataSource(DataSource dataSource)
        {
            var dbName = Context.Models.Where(x => x.Id == dataSource.ModelId).Select(x => x.DatabaseName).FirstOrDefault();
            try
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases.FindByName(dbName);
                    if (database == null)
                    {
                        throw new ArgumentException("Database not found");
                    }
                    var ds = database.Model.DataSources.Find(dataSource.Name) as AS.ProviderDataSource;
                    if (ds == null)
                    {
                        throw new ArgumentException("Data Source not found");
                    }
                    ds.Description = dataSource.Description;
                    ds.ImpersonationMode = AS.ImpersonationMode.ImpersonateServiceAccount;
                    ds.ConnectionString = GetDataSourceConnectionString(dataSource);
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
            catch (AN.OperationException ex)
            {
                foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
                {
                }
                throw;
            }
        }

        public void DeleteDataSource(DataSource dataSource)
        {
            var dbName = Context.Models.Where(x => x.Id == dataSource.ModelId).Select(x => x.DatabaseName).FirstOrDefault();
            try
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases.FindByName(dbName);
                    if (database == null)
                    {
                        throw new ArgumentException("Database not found");
                    }
                    var ds = database.Model.DataSources.Find(dataSource.Name);
                    if (ds == null)
                    {
                        throw new ArgumentException("Data Source not found");
                    }
                    database.Model.DataSources.Remove(ds);
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
            catch (AN.OperationException ex)
            {
                foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
                {
                }
                throw;
            }
        }

        public void CreateTable(Table table)
        {
            var dbName = Context.Models.Where(x => x.Id == table.ModelId).Select(x => x.DatabaseName).FirstOrDefault();
            try
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases.FindByName(dbName);
                    if (database == null)
                    {
                        throw new ArgumentException("Database not found");
                    }
                    var tb = new AS.Table
                    {
                        Name = table.Name,
                        Description = table.Description
                    };
                    if (table.Columns != null)
                    {
                        foreach (var col in table.Columns)
                        {
                            tb.Columns.Add(new AS.DataColumn
                            {
                                Name = col.Name,
                                DataType = col.DataType.ToDataType(),
                                SourceColumn = col.SourceColumn,
                                IsHidden = col.IsHidden,
                                DisplayFolder = col.DisplayFolder,
                                FormatString = col.FormatString
                            });
                        }
                    }
                    if (table.Partitions != null)
                    {
                        foreach (var par in table.Partitions)
                        {
                            DataSource ds;
                            if (par.DataSource == null)
                            {
                                ds = Context.DataSources.Find(par.DataSourceId);
                            }
                            else
                            {
                                ds = par.DataSource;
                            }
                            tb.Partitions.Add(new AS.Partition
                            {
                                Name = par.Name,
                                Source = new AS.QueryPartitionSource()
                                {
                                    DataSource = database.Model.DataSources[ds.Name],
                                    Query = par.Query
                                }
                            });
                        }
                    }
                    if (table.Measures != null)
                    {
                        foreach (var mes in table.Measures)
                        {
                            tb.Measures.Add(new AS.Measure
                            {
                                Name = mes.Name,
                                Expression = mes.Expression,
                                Description = mes.Description,
                                DisplayFolder = mes.DisplayFolder,
                                FormatString = mes.FormatString
                            });
                        }
                    }
                    database.Model.Tables.Add(tb);
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
            catch (AN.OperationException ex)
            {
                foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
                {
                }
                throw;
            }
        }

        public void CreateTables(int modelId, List<Table> tables)
        {
            var dbName = Context.Models.Where(x => x.Id == modelId).Select(x => x.DatabaseName).FirstOrDefault();
            try
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases.FindByName(dbName);
                    if (database == null)
                    {
                        throw new ArgumentException("Database not found");
                    }
                    foreach (var table in tables)
                    {
                        var tb = new AS.Table
                        {
                            Name = table.Name,
                            Description = table.Description
                        };
                        if (table.Columns != null)
                        {
                            foreach (var col in table.Columns)
                            {
                                tb.Columns.Add(new AS.DataColumn
                                {
                                    Name = col.Name,
                                    DataType = col.DataType.ToDataType(),
                                    SourceColumn = col.SourceColumn,
                                    IsHidden = col.IsHidden,
                                    DisplayFolder = col.DisplayFolder,
                                    FormatString = col.FormatString
                                });
                            }
                        }
                        if (table.Partitions != null)
                        {
                            foreach (var par in table.Partitions)
                            {
                                DataSource ds;
                                if (par.DataSource == null)
                                {
                                    ds = Context.DataSources.Find(par.DataSourceId);
                                }
                                else
                                {
                                    ds = par.DataSource;
                                }
                                tb.Partitions.Add(new AS.Partition
                                {
                                    Name = par.Name,
                                    Source = new AS.QueryPartitionSource()
                                    {
                                        DataSource = database.Model.DataSources[ds.Name],
                                        Query = par.Query
                                    }
                                });
                            }
                        }
                        if (table.Measures != null)
                        {
                            foreach (var mes in table.Measures)
                            {
                                tb.Measures.Add(new AS.Measure
                                {
                                    Name = mes.Name,
                                    Expression = mes.Expression,
                                    Description = mes.Description,
                                    DisplayFolder = mes.DisplayFolder,
                                    FormatString = mes.FormatString
                                });
                            }
                        }
                        database.Model.Tables.Add(tb);
                    }
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
            catch (AN.OperationException ex)
            {
                foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
                {
                }
                throw;
            }
        }

        public void AddTables(AS.Database database, int modelId, List<Table> tables)
        {
            foreach (var table in tables)
            {
                var tb = new AS.Table
                {
                    Name = table.Name,
                    Description = table.Description
                };
                if (table.Columns != null)
                {
                    foreach (var col in table.Columns)
                    {
                        tb.Columns.Add(new AS.DataColumn
                        {
                            Name = col.Name,
                            DataType = col.DataType.ToDataType(),
                            SourceColumn = col.SourceColumn,
                            IsHidden = col.IsHidden,
                            DisplayFolder = col.DisplayFolder,
                            FormatString = col.FormatString
                        });
                    }
                }
                if (table.Partitions != null)
                {
                    foreach (var par in table.Partitions)
                    {
                        DataSource ds;
                        if (par.DataSource == null)
                        {
                            ds = Context.DataSources.Find(par.DataSourceId);
                        }
                        else
                        {
                            ds = par.DataSource;
                        }
                        tb.Partitions.Add(new AS.Partition
                        {
                            Name = par.Name,
                            Source = new AS.QueryPartitionSource()
                            {
                                DataSource = database.Model.DataSources[ds.Name],
                                Query = par.Query
                            }
                        });
                    }
                }
                if (table.Measures != null)
                {
                    foreach (var mes in table.Measures)
                    {
                        tb.Measures.Add(new AS.Measure
                        {
                            Name = mes.Name,
                            Expression = mes.Expression,
                            Description = mes.Description,
                            DisplayFolder = mes.DisplayFolder,
                            FormatString = mes.FormatString
                        });
                    }
                }
                database.Model.Tables.Add(tb);
            }

        }

        public AS.Table AddTable(AS.Database database, int modelId, Table table)
        {
            var tb = new AS.Table
            {
                Name = table.Name,
                Description = table.Description
            };
            if (table.Columns != null)
            {
                foreach (var col in table.Columns)
                {
                    var c = new AS.DataColumn
                    {
                        Name = col.Name,
                        DataType = col.DataType.ToDataType(),
                        SourceColumn = col.SourceColumn,
                        IsHidden = col.IsHidden,
                        DisplayFolder = col.DisplayFolder,
                        FormatString = col.FormatString
                    };
                    if (col.SortByColumn != null)
                    {
                        c.SortByColumn = tb.Columns[col.SortByColumn.Name];
                    }
                    tb.Columns.Add(c);
                }
            }
            if (table.Partitions != null)
            {
                foreach (var par in table.Partitions)
                {
                    DataSource ds;
                    if (par.DataSource == null)
                    {
                        ds = Context.DataSources.Find(par.DataSourceId);
                    }
                    else
                    {
                        ds = par.DataSource;
                    }
                    tb.Partitions.Add(new AS.Partition
                    {
                        Name = par.Name,
                        Source = new AS.QueryPartitionSource()
                        {
                            DataSource = database.Model.DataSources[ds.Name],
                            Query = par.Query
                        }
                    });
                }
            }
            if (table.Measures != null)
            {
                foreach (var mes in table.Measures)
                {
                    tb.Measures.Add(new AS.Measure
                    {
                        Name = mes.Name,
                        Expression = mes.Expression,
                        Description = mes.Description,
                        DisplayFolder = mes.DisplayFolder,
                        FormatString = mes.FormatString
                    });
                }
            }
            database.Model.Tables.Add(tb);
            return tb;
        }

        public void UpdateTable(Table table)
        {
            var dbName = Context.Models.Where(x => x.Id == table.ModelId).Select(x => x.DatabaseName).FirstOrDefault();
            try
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases.FindByName(dbName);
                    if (database == null)
                    {
                        throw new ArgumentException("Database not found");
                    }
                    var tb = database.Model.Tables.Find(table.Name);
                    tb.Description = table.Description;
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
            catch (AN.OperationException ex)
            {
                foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
                {
                }
                throw;
            }
        }

        public void DeleteTable(Table table)
        {
            var dbName = Context.Models.Where(x => x.Id == table.ModelId).Select(x => x.DatabaseName)
                .FirstOrDefault();
            if (dbName == null)
            {
                throw new ArgumentException("Model not found");
            }
            try
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases.FindByName(dbName);
                    if (database == null)
                    {
                        throw new ArgumentException("Database not found");
                    }
                    var tb = database.Model.Tables.Find(table.Name);
                    if (tb == null)
                    {
                        throw new ArgumentException("Table not found");
                    }
                    database.Model.Tables.Remove(tb);
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
            catch (AN.OperationException ex)
            {
                foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
                {
                }
                throw;
            }
        }

        public void CreatePartition(Partition partition)
        {
            var info = Context.Partitions.Where(x => x.Id == partition.Id)
                .Select(x => new
                {
                    DatabaseName = x.Table.Model.DatabaseName,
                    TableName = x.Name,
                    DataSourceName = x.DataSource.Name
                }).FirstOrDefault();
            if (info == null || info.DatabaseName == null)
            {
                throw new ArgumentException("Model not found");
            }
            try
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases.FindByName(info.DatabaseName);
                    if (database == null)
                    {
                        throw new ArgumentException("Database not found");
                    }
                    var tb = database.Model.Tables.Find(info.TableName);
                    if (tb == null)
                    {
                        throw new ArgumentException("Table not found");
                    }
                    var pa = new AS.Partition
                    {
                        Name = partition.Name,
                        Description = partition.Description
                    };
                    if (partition.SourceType == PartitionSourceType.Query)
                    {
                        var dataSource = database.Model.DataSources[info.DataSourceName];
                        if (dataSource == null)
                        {
                            throw new ArgumentException("Data Source not found");
                        }
                        pa.Source = new AS.QueryPartitionSource
                        {
                            Query = partition.Query,
                            DataSource = dataSource
                        };
                    }
                    if (partition.SourceType == PartitionSourceType.Calculated)
                    {
                        pa.Source = new AS.CalculatedPartitionSource
                        {
                            Expression = partition.Query
                        };
                    }
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
            catch (AN.OperationException ex)
            {
                foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
                {
                }
                throw;
            }
        }

        public void UpdatePartition(Partition partition)
        {
            var info = Context.Tables.Where(x => x.Id == partition.TableId)
                .Select(x => new
                {
                    DatabaseName = x.Model.DatabaseName,
                    TableName = x.Name
                }).FirstOrDefault();
            if (info == null || info.DatabaseName == null)
            {
                throw new ArgumentException("Model not found");
            }
            try
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases.FindByName(info.DatabaseName);
                    if (database == null)
                    {
                        throw new ArgumentException("Database not found");
                    }
                    var tb = database.Model.Tables.Find(info.TableName);
                    if (tb == null)
                    {
                        throw new ArgumentException("Table not found");
                    }
                    var pa = tb.Partitions.Find(partition.Name);
                    if (pa == null)
                    {
                        throw new ArgumentException("Partition not found");
                    }
                    pa.Description = partition.Description;
                    if (pa.SourceType == AS.PartitionSourceType.Query)
                    {
                        var source = pa.Source as AS.QueryPartitionSource;
                        source.Query = partition.Query;
                    }
                    if (pa.SourceType == AS.PartitionSourceType.Calculated)
                    {
                        var source = pa.Source as AS.CalculatedPartitionSource;
                        source.Expression = partition.Query;
                    }
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
            catch (AN.OperationException ex)
            {
                foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
                {
                }
                throw;
            }
        }

        public void CreateRelationship(Relationship relationship)
        {
            var dbName = Context.Models.Where(x => x.Id == relationship.ModelId).Select(x => x.DatabaseName).FirstOrDefault();
            var info = Context.Relationships.Where(x => x.Id == relationship.Id).Select(x => new
            {
                FromTableName = x.FromColumn.Table.Name,
                FromColumnName = x.FromColumn.Name,
                ToTableName = x.ToColumn.Table.Name,
                ToColumnName = x.ToColumn.Name
            }).FirstOrDefault();
            if (dbName == null)
            {
                throw new ArgumentException("Model not found");
            }
            try
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases.FindByName(dbName);
                    if (database == null)
                    {
                        throw new ArgumentException("Database not found");
                    }
                    database.Model.Relationships.Add(new AS.SingleColumnRelationship
                    {
                        Name = relationship.Name,
                        ToColumn = database.Model.Tables[info.ToTableName].Columns[info.ToColumnName],
                        FromColumn = database.Model.Tables[info.FromTableName].Columns[info.FromColumnName],
                        FromCardinality = AS.RelationshipEndCardinality.Many,
                        ToCardinality = AS.RelationshipEndCardinality.One
                    });
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
            catch (AN.OperationException ex)
            {
                foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
                {
                }
                throw;
            }
        }

        public void UpdateRelationship(Relationship relationship)
        {
            var dbName = Context.Models.Where(x => x.Id == relationship.ModelId).Select(x => x.DatabaseName).FirstOrDefault();
            var info = Context.Relationships.Where(x => x.Id == relationship.Id).Select(x => new
            {
                FromTableName = x.FromColumn.Table.Name,
                FromColumnName = x.FromColumn.Name,
                ToTableName = x.ToColumn.Table.Name,
                ToColumnName = x.ToColumn.Name
            }).FirstOrDefault();
            if (dbName == null)
            {
                throw new ArgumentException("Model not found");
            }
            try
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases.FindByName(dbName);
                    if (database == null)
                    {
                        throw new ArgumentException("Database not found");
                    }
                    var re = database.Model.Relationships.Find(relationship.Name) as AS.SingleColumnRelationship;
                    if (re == null)
                    {
                        throw new ArgumentException("Relationship not found");
                    }
                    re.ToColumn = database.Model.Tables[info.ToTableName].Columns[info.ToColumnName];
                    re.FromColumn = database.Model.Tables[info.FromTableName].Columns[info.FromColumnName];
                    database.Model.Relationships.Remove(re);
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
            catch (AN.OperationException ex)
            {
                foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
                {
                }
                throw;
            }
        }

        public void DeleteRelationship(Relationship relationship)
        {
            var dbName = Context.Models.Where(x => x.Id == relationship.ModelId).Select(x => x.DatabaseName)
                .FirstOrDefault();
            if (dbName == null)
            {
                throw new ArgumentException("Model not found");
            }
            try
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases.FindByName(dbName);
                    if (database == null)
                    {
                        throw new ArgumentException("Database not found");
                    }
                    var re = database.Model.Relationships.Find(relationship.Name);
                    if (re == null)
                    {
                        throw new ArgumentException("Relationship not found");
                    }
                    database.Model.Relationships.Remove(re);
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
            catch (AN.OperationException ex)
            {
                foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
                {
                }
                throw;
            }
        }

        public void UpdateColumn(Column column)
        {
            var info = Context.Columns.Where(x => x.Id == column.Id).Select(x => new
            {
                DatabaseName = x.Table.Model.DatabaseName,
                TableName = x.Table.Name,
                Name = x.Name
            }).FirstOrDefault();
            if (info == null || info.DatabaseName == null)
            {
                throw new ArgumentException("Model not found");
            }
            try
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases.FindByName(info.DatabaseName);
                    if (database == null)
                    {
                        throw new ArgumentException("Database not found");
                    }
                    var tb = database.Model.Tables.Find(info.TableName);
                    if (tb == null)
                    {
                        throw new ArgumentException("Table not found");
                    }
                    var co = tb.Columns.Find(info.Name) as AS.DataColumn;
                    if (co == null)
                    {
                        throw new ArgumentException("Column not found");
                    }
                    co.Description = column.Description;
                    co.DataType = column.DataType.ToDataType();
                    co.DisplayFolder = column.DisplayFolder;
                    co.FormatString = column.FormatString;
                    co.IsHidden = column.IsHidden;
                    co.SourceColumn = column.SourceColumn;
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
            catch (AN.OperationException ex)
            {
                foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
                {
                }
                throw;
            }
        }

        public void DeleteColumn(Column column)
        {
            var info = Context.Tables.Where(x => x.Id == column.TableId).Select(x => new
            {
                DatabaseName = x.Model.DatabaseName,
                TableName = x.Name
            }).FirstOrDefault();
            if (info == null || info.DatabaseName == null)
            {
                throw new ArgumentException("Model not found");
            }
            try
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases.FindByName(info.DatabaseName);
                    if (database == null)
                    {
                        throw new ArgumentException("Database not found");
                    }
                    var tb = database.Model.Tables.Find(info.TableName);
                    if (tb == null)
                    {
                        throw new ArgumentException("Table not found");
                    }
                    var co = tb.Columns.Find(column.Name);
                    if (co == null)
                    {
                        throw new ArgumentException("Column not found");
                    }
                    tb.Columns.Remove(co);
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
            catch (AN.OperationException ex)
            {
                foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
                {
                }
                throw;
            }
        }


        public bool CheckDataSourceNamesExist(int modelId, List<string> names)
        {
            return Context.DataSources.Any(x => x.ModelId == modelId && names.Contains(x.Name));
        }
        public bool CheckTableNamesExist(int modelId, List<string> names)
        {
            return Context.Tables.Any(x => x.ModelId == modelId && names.Contains(x.Name));
        }
        public bool CheckColumnNamesExist(int tableId, List<string> names)
        {
            return Context.Columns.Any(x => x.TableId == tableId && names.Contains(x.Name));
        }
        public bool CheckPartitionNamesExist(int tableId, List<string> names)
        {
            return Context.Partitions.Any(x => x.TableId == tableId && names.Contains(x.Name));
        }
        public bool CheckMeasureNamesExist(int tableId, List<string> names)
        {
            return Context.Measures.Any(x => x.TableId == tableId && names.Contains(x.Name));
        }

        public void CreateDateTable(DateTableCreateModel model)
        {
            if (Context.Tables.Any(x => x.ModelId == model.ModelId && x.Name == model.TableName))
            {
                throw new Exception("Table exist");
            }
            var conStr = Context.Database.Connection.ConnectionString;
            var sourceTable = CreateDateTableSource(model.FromDate, model.ToDate);
            var dbName = Context.Models.Where(x => x.Id == model.ModelId).Select(x => x.DatabaseName).FirstOrDefault();
            var table = new Table
            {
                Name = model.TableName,
                ModelId = model.ModelId,
                SourceTable = sourceTable,
                SourceSchema = "dbo",
                OriginalName = model.TableName,
                Columns = new List<Column>(),
                Partitions = new List<Partition>()
            };
            Context.Tables.Add(table);

            table.Columns.Add(new Column
            {
                Name = "Date",
                DataType = ColumnDataType.DateTime,
                SourceColumn = "Date"
            });
            table.Columns.Add(new Column
            {
                Name = "DateKey",
                DataType = ColumnDataType.Int64,
                SourceColumn = "DateKey"
            });
            table.Columns.Add(new Column
            {
                Name = "DateName",
                DataType = ColumnDataType.String,
                SourceColumn = "DateName",
                SortByColumn = table.Columns.Last()
            });
            table.Columns.Add(new Column
            {
                Name = "Year",
                DataType = ColumnDataType.Int64,
                SourceColumn = "Year"
            });
            table.Columns.Add(new Column
            {
                Name = "YearName",
                DataType = ColumnDataType.String,
                SourceColumn = "YearName",
                SortByColumn = table.Columns.Last()
            });
            table.Columns.Add(new Column
            {
                Name = "Month",
                DataType = ColumnDataType.Int64,
                SourceColumn = "Month"
            });
            table.Columns.Add(new Column
            {
                Name = "MonthName",
                DataType = ColumnDataType.String,
                SourceColumn = "MonthName",
                SortByColumn = table.Columns.Last()
            });
            table.Columns.Add(new Column
            {
                Name = "Quarter",
                DataType = ColumnDataType.Int64,
                SourceColumn = "Quarter"
            });
            table.Columns.Add(new Column
            {
                Name = "QuarterName",
                DataType = ColumnDataType.String,
                SourceColumn = "QuarterName",
                SortByColumn = table.Columns.Last()
            });
            table.Columns.Add(new Column
            {
                Name = "HalfYear",
                DataType = ColumnDataType.Int64,
                SourceColumn = "HalfYear"
            });
            table.Columns.Add(new Column
            {
                Name = "HalfYearName",
                DataType = ColumnDataType.String,
                SourceColumn = "HalfYearName",
                SortByColumn = table.Columns.Last()
            });
            table.Columns.Add(new Column
            {
                Name = "DayOfMonth",
                DataType = ColumnDataType.Int64,
                SourceColumn = "DayOfMonth"
            });
            table.Columns.Add(new Column
            {
                Name = "DayOfMonthName",
                DataType = ColumnDataType.String,
                SourceColumn = "DayOfMonthName",
                SortByColumn = table.Columns.Last()
            });
            table.Columns.Add(new Column
            {
                Name = "DayOfWeek",
                DataType = ColumnDataType.Int64,
                SourceColumn = "DayOfWeek"
            });
            table.Columns.Add(new Column
            {
                Name = "DayOfWeekName",
                DataType = ColumnDataType.String,
                SourceColumn = "DayOfWeekName",
                SortByColumn = table.Columns.Last()
            });
            table.Columns.Add(new Column
            {
                Name = "MonthOfYear",
                DataType = ColumnDataType.Int64,
                SourceColumn = "MonthOfYear"
            });
            table.Columns.Add(new Column
            {
                Name = "MonthOfYearName",
                DataType = ColumnDataType.String,
                SourceColumn = "MonthOfYearName",
                SortByColumn = table.Columns.Last()
            });
            table.Columns.Add(new Column
            {
                Name = "QuarterOfYear",
                DataType = ColumnDataType.Int64,
                SourceColumn = "QuarterOfYear"
            });
            table.Columns.Add(new Column
            {
                Name = "QuarterOfYearName",
                DataType = ColumnDataType.String,
                SourceColumn = "QuarterOfYearName",
                SortByColumn = table.Columns.Last()
            });
            table.Columns.Add(new Column
            {
                Name = "HalfYearOfYear",
                DataType = ColumnDataType.Int64,
                SourceColumn = "HalfYearOfYear"
            });
            table.Columns.Add(new Column
            {
                Name = "HalfYearOfYearName",
                DataType = ColumnDataType.String,
                SourceColumn = "HalfYearOfYearName",
                SortByColumn = table.Columns.Last()
            });
            table.Columns.Add(new Column
            {
                Name = "LunarDate",
                DataType = ColumnDataType.Int64,
                SourceColumn = "LunarDate"
            });
            table.Columns.Add(new Column
            {
                Name = "LunarDateName",
                DataType = ColumnDataType.String,
                SourceColumn = "LunarDateName",
                SortByColumn = table.Columns.Last()
            });
            table.Columns.Add(new Column
            {
                Name = "LunarMonth",
                DataType = ColumnDataType.Int64,
                SourceColumn = "LunarMonth"
            });
            table.Columns.Add(new Column
            {
                Name = "LunarMonthName",
                DataType = ColumnDataType.String,
                SourceColumn = "LunarMonthName",
                SortByColumn = table.Columns.Last()
            });
            table.Columns.Add(new Column
            {
                Name = "LunarQuarter",
                DataType = ColumnDataType.Int64,
                SourceColumn = "LunarQuarter"
            });
            table.Columns.Add(new Column
            {
                Name = "LunarQuarterName",
                DataType = ColumnDataType.String,
                SourceColumn = "LunarQuarterName",
                SortByColumn = table.Columns.Last()
            });
            table.Columns.Add(new Column
            {
                Name = "LunarYear",
                DataType = ColumnDataType.Int64,
                SourceColumn = "LunarYear"
            });
            table.Columns.Add(new Column
            {
                Name = "LunarYearName",
                DataType = ColumnDataType.String,
                SourceColumn = "LunarYearName",
                SortByColumn = table.Columns.Last()
            });
            table.Columns.Add(new Column
            {
                Name = "LunarDayOfWeek",
                DataType = ColumnDataType.Int64,
                SourceColumn = "LunarDayOfWeek"
            });
            table.Columns.Add(new Column
            {
                Name = "LunarDayOfWeekName",
                DataType = ColumnDataType.String,
                SourceColumn = "LunarDayOfWeekName",
                SortByColumn = table.Columns.Last()
            });
            table.Columns.Add(new Column
            {
                Name = "LunarDayOfMonth",
                DataType = ColumnDataType.Int64,
                SourceColumn = "LunarDayOfMonth"
            });
            table.Columns.Add(new Column
            {
                Name = "LunarDayOfMonthName",
                DataType = ColumnDataType.String,
                SourceColumn = "LunarDayOfMonthName",
                SortByColumn = table.Columns.Last()
            });
            table.Columns.Add(new Column
            {
                Name = "LunarMonthOfYear",
                DataType = ColumnDataType.Int64,
                SourceColumn = "LunarMonthOfYear"
            });
            table.Columns.Add(new Column
            {
                Name = "LunarMonthOfYearName",
                DataType = ColumnDataType.String,
                SourceColumn = "LunarMonthOfYearName",
                SortByColumn = table.Columns.Last()
            });
            table.Columns.Add(new Column
            {
                Name = "LunarQuarterOfYear",
                DataType = ColumnDataType.Int64,
                SourceColumn = "LunarQuarterOfYear"
            });
            table.Columns.Add(new Column
            {
                Name = "LunarQuarterOfYearName",
                DataType = ColumnDataType.String,
                SourceColumn = "LunarQuarterOfYearName",
                SortByColumn = table.Columns.Last()
            });
            table.Columns.Add(new Column
            {
                Name = "EventName",
                DataType = ColumnDataType.String,
                SourceColumn = "EventName"
            });
            var dsName = "DS_" + dbName;
            var ds = Context.DataSources.Where(x => x.ModelId == model.ModelId && x.Name == dsName).FirstOrDefault();
            var scb = new SqlConnectionStringBuilder(conStr);
            if (ds == null)
            {
                ds = new DataSource
                {
                    Name = dsName,
                    OriginalName = dsName,
                    ModelId = model.ModelId,
                    ConnectionString = conStr,
                    Catalog = scb.InitialCatalog,
                    IntegratedSecurity = scb.IntegratedSecurity,
                    Source = scb.DataSource,
                    User = scb.UserID,
                    Password = scb.Password,
                    Type = DataSourceType.SqlServer
                };
                Context.DataSources.Add(ds);
            }
            table.Partitions.Add(new Partition
            {
                Name = "DefaultPartition",
                Query = $"SELECT * FROM [dbo].[{sourceTable}]",
                DataSourceId = ds.Id,
                DataSource = ds
            });
            Context.SaveChanges();
            try
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases.FindByName(dbName);
                    if (database == null)
                    {
                        throw new ArgumentException("Database not found");
                    }
                    var dataSource = database.Model.DataSources.Find(ds.Name);
                    if (dataSource == null)
                    {
                        AddDataSource(database, ds);
                    }
                    var tb = AddTable(database, model.ModelId, table);
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
            catch (AN.OperationException ex)
            {
                foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
                {
                }
                throw;
            }
        }

        public string CreateDateTableSource(DateTime fromDate, DateTime toDate)
        {
            var conStr = _contextProvider.Context.Database.Connection.ConnectionString;
            var i = 0;
            var dates = new List<DateData>();
            while (true)
            {
                var currentDate = fromDate.AddDays(i);
                if (currentDate > toDate) { break; }
                var date = new DateData(currentDate);
                dates.Add(date);
                i++;
            }
            var dateTable = dates.ToDataTable();
            var tableName = $"TempDate_{fromDate.ToString("yyyyMMdd")}_{toDate.ToString("yyyyMMdd")}";
            SqlConnection connection;
            var exist = false;
            using (connection = new SqlConnection(conStr))
            {
                var cmd = new SqlCommand($"IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}') SELECT 1 ELSE SELECT 0", connection);
                connection.Open();
                var res = (int)cmd.ExecuteScalar();
                exist = (res == 1);
                connection.Close();
            }
            if (!exist)
            {
                using (connection = new SqlConnection(conStr))
                {
                    var sql = $"CREATE TABLE [dbo].[{tableName}](" +
                                "[DateKey] [int] NOT NULL PRIMARY KEY," +
                                "[Date] [datetime] NOT NULL," +
                                "[DateName] [nvarchar] (64) NOT NULL," +
                                "[Year] [int] NULL," +
                                "[YearName] [nvarchar] (64) NULL," +
                                "[Month] [int] NULL," +
                                "[MonthName] [nvarchar] (64) NULL," +
                                "[Quarter] [int] NULL," +
                                "[QuarterName] [nvarchar] (64) NULL," +
                                "[HalfYear] [int] NULL," +
                                "[HalfYearName] [nvarchar] (64) NULL," +
                                "[DayOfMonth] [int] NULL," +
                                "[DayOfMonthName] [nvarchar] (64) NULL," +
                                "[DayOfWeek] [int] NULL," +
                                "[DayOfWeekName] [nvarchar] (64) NULL," +
                                "[MonthOfYear] [int] NULL," +
                                "[MonthOfYearName] [nvarchar] (64) NULL," +
                                "[QuarterOfYear] [int] NULL," +
                                "[QuarterOfYearName] [nvarchar] (64) NULL," +
                                "[HalfYearOfYear] [int] NULL," +
                                "[HalfYearOfYearName] [nvarchar] (64) NULL," +
                                "[LunarDate] [int] NULL," +
                                "[LunarDateName] [nvarchar] (64) NULL," +
                                "[LunarMonth] [int] NULL," +
                                "[LunarMonthName] [nvarchar] (64) NULL," +
                                "[LunarQuarter] [int] NULL," +
                                "[LunarQuarterName] [nvarchar] (64) NULL," +
                                "[LunarYear] [int] NULL," +
                                "[LunarYearName] [nvarchar] (64) NULL," +
                                "[LunarDayOfWeek] [int] NULL," +
                                "[LunarDayOfWeekName] [nvarchar] (64) NULL," +
                                "[LunarDayOfMonth] [int] NULL," +
                                "[LunarDayOfMonthName] [nvarchar] (64) NULL," +
                                "[LunarMonthOfYear] [int] NULL," +
                                "[LunarMonthOfYearName] [nvarchar] (64) NULL," +
                                "[LunarQuarterOfYear] [int] NULL," +
                                "[LunarQuarterOfYearName] [nvarchar] (64) NULL," +
                                "[EventName] [nvarchar] (256) NULL)";
                    var cmd = new SqlCommand(sql, connection);
                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
                using (connection = new SqlConnection(conStr))
                {
                    var bulkCopy = new SqlBulkCopy
                        (connection,
                        SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction,
                        null)
                    {
                        DestinationTableName = tableName
                    };
                    foreach (DataColumn col in dateTable.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    }
                    connection.Open();
                    bulkCopy.WriteToServer(dateTable);
                    connection.Close();
                }
            }
            else
            {
                var regen = false;
                using (connection = new SqlConnection(conStr))
                {
                    var cmd = new SqlCommand($"SELECT COUNT(DateKey) FROM {tableName}", connection);
                    connection.Open();
                    var res = (int)cmd.ExecuteScalar();
                    regen = (res != i);
                    connection.Close();
                }
                if (regen)
                {
                    using (connection = new SqlConnection(conStr))
                    {
                        var cmd = new SqlCommand($"TRUNCATE TABLE  {tableName}", connection);
                        connection.Open();
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                    using (connection = new SqlConnection(conStr))
                    {
                        var bulkCopy = new SqlBulkCopy
                            (connection,
                            SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction,
                            null)
                        {
                            DestinationTableName = tableName
                        };
                        foreach (DataColumn col in dateTable.Columns)
                        {
                            bulkCopy.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                        }
                        connection.Open();
                        bulkCopy.WriteToServer(dateTable);
                        connection.Close();
                    }
                }
            }
            dateTable.Clear();
            return tableName;
        }
    }
}