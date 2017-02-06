using Breeze.ContextProvider;
using Breeze.ContextProvider.EF6;
using Newtonsoft.Json.Linq;
using Pentamic.SSBI.Models.DataModel;
using System;
using System.Data.Entity;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AS = Microsoft.AnalysisServices.Tabular;
namespace Pentamic.SSBI.Services
{
    public class DataModelService
    {
        private EFContextProvider<DataModelContext> _contextProvider;

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

        public async Task<DataSource> ImportDataSource(MultipartFormDataStreamProvider provider)
        {
            var file = provider.FileData[0];
            var formData = provider.FormData;
            var modelId = Convert.ToInt32(formData["modelId"]);
            var name = formData["name"];
            var description = formData["description"];
            var dataSource = new DataSource
            {
                ModelId = modelId,
                Type = DataSourceType.Excel,
                Name = name,
                OriginalName = name,
                Description = description,
                FileName = file.Headers.ContentDisposition.FileName.Replace("\"", ""),
                FilePath = file.LocalFileName,
                State = DataModelObjectState.Added
            };
            Context.DataSources.Add(dataSource);
            await Context.SaveChangesAsync();
            return dataSource;
        }

        public void DeployModel(int modelId)
        {
            var mo = Context.Models.Find(modelId);
            using (var server = new AS.Server())
            {
                server.Connect(@".\astab16");
                if (mo.State == DataModelObjectState.Deleted)
                {
                    server.Databases.Remove(mo.DatabaseName);
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
                        Description = mo.Description
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
                        }
                        var updatedProps = mo.UpdatedProperties.Split(',');
                        if (database.Model.Description != mo.Description)
                        {
                            database.Model.Description = mo.Description;
                        }
                        mo.State = DataModelObjectState.Unchanged;
                    }
                }

                //Data Sources
                Context.Entry(mo).Collection<DataSource>(x => x.DataSources).Load();
                foreach (var ds in mo.DataSources)
                {
                    if (ds.State == DataModelObjectState.Deleted)
                    {
                        database.Model.DataSources.Remove(ds.OriginalName);
                    }
                    AS.ProviderDataSource dataSource = null;
                    if (ds.State == DataModelObjectState.Added)
                    {
                        dataSource = new AS.ProviderDataSource
                        {
                            Name = ds.Name,
                            Description = ds.Description,
                            ConnectionString = GetDataSourceConnectionString(ds),
                            //ImpersonationMode = Microsoft.AnalysisServices.Tabular.ImpersonationMode.ImpersonateServiceAccount
                            ImpersonationMode = Microsoft.AnalysisServices.Tabular.ImpersonationMode.ImpersonateAccount,
                            Account = "datht.it@live.com",
                            Password = "20111988Gi@ng"
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
                    Context.Entry(ds).Collection(x => x.Tables).Load();
                    foreach (var tb in ds.Tables)
                    {
                        if (tb.State == DataModelObjectState.Deleted)
                        {
                            database.Model.Tables.Remove(tb.OriginalName);
                        }
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

                        Context.Entry(tb).Collection(x => x.Partitions).Load();
                        foreach (var pa in tb.Partitions)
                        {
                            if (pa.State == DataModelObjectState.Deleted)
                            {
                                table.Partitions.Remove(pa.OriginalName);
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
                        Context.Entry(tb).Collection<Column>(x => x.Columns).Load();
                        foreach (var co in tb.Columns)
                        {
                            if (co.State == DataModelObjectState.Deleted)
                            {
                                table.Columns.Remove(co.OriginalName);
                            }
                            if (co.State == DataModelObjectState.Added)
                            {
                                table.Columns.Add(new AS.DataColumn
                                {
                                    Name = co.Name,
                                    DataType = co.DataType.ToDataType(),
                                    SourceColumn = co.SourceColumn
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
                                co.State = DataModelObjectState.Unchanged;
                            }
                        }
                        Context.Entry(tb).Collection<Measure>(x => x.Measures).Load();
                        foreach (var me in tb.Measures)
                        {
                            if (me.State == DataModelObjectState.Deleted)
                            {
                                table.Measures.Remove(me.OriginalName);
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
                            }
                        }
                    }
                }

                //Relationships
                var relationships = Context.Relationships.Where(x => x.ModelId == modelId)
                    .Include(x => x.FromColumn.Table).Include(x => x.ToColumn.Table).ToList();
                foreach (var re in relationships)
                {
                    if (re.State == DataModelObjectState.Deleted)
                    {
                        database.Model.Relationships.Remove(re.OriginalName);
                    }
                    if (re.State == DataModelObjectState.Added)
                    {
                        database.Model.Relationships.Add(new AS.SingleColumnRelationship()
                        {
                            Name = re.Name,
                            ToColumn = database.Model.Tables[re.ToColumn.Table.Name].Columns[re.ToColumn.Name],
                            FromColumn = database.Model.Tables[re.FromColumn.Table.Name].Columns[re.FromColumn.Name]
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
                    }
                }

                //Commit changes
                database.Update(Microsoft.AnalysisServices.UpdateOptions.ExpandFull);
                Context.SaveChanges();
                server.Disconnect();
            }
        }

        public void RefreshModel(int modelId)
        {
            var mo = Context.Models.Find(modelId);
            using (var server = new AS.Server())
            {
                server.Connect(@".\astab16");
                var database = server.Databases[mo.DatabaseName];
                database.Model.RequestRefresh(AS.RefreshType.Full);
                database.Update(Microsoft.AnalysisServices.UpdateOptions.ExpandFull);
            }
        }

        public SaveResult SaveChanges(JObject saveBundle)
        {
            var txSettings = new TransactionSettings { TransactionType = TransactionType.TransactionScope };
            _contextProvider.BeforeSaveEntityDelegate += BeforeSaveEntity;
            return _contextProvider.SaveChanges(saveBundle, txSettings);
        }

        public bool BeforeSaveEntity(EntityInfo info)
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
                        if (info.OriginalValuesMap.Keys.Count > 0)
                        {
                            entity.UpdatedProperties = string.Join(",", info.OriginalValuesMap.Keys).ToUpper();
                            info.OriginalValuesMap["UpdatedProperties"] = null;
                        }
                        entity.State = DataModelObjectState.Modified;
                        info.OriginalValuesMap["State"] = null;
                        break;
                    case Breeze.ContextProvider.EntityState.Deleted:
                        return false;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            return true;
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
                    var builder = new OleDbConnectionStringBuilder()
                    {
                        Provider = "Microsoft.ACE.OLEDB.12.0",
                        DataSource = ds.FilePath,
                        PersistSecurityInfo = false
                    };
                    builder["Mode"] = "Read";
                    var extension = Path.GetExtension(ds.FileName).ToUpper();
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