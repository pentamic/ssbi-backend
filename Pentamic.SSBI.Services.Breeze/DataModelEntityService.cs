using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Globalization;
using System.IO.Compression;
using System.Threading.Tasks;
using Breeze.Persistence;
using Newtonsoft.Json;
using Pentamic.SSBI.Data;
using Pentamic.SSBI.Entities;
using Pentamic.SSBI.Services.SSAS.Dax;
using Pentamic.SSBI.Services.SSAS.Metadata;
using EntityState = Breeze.Persistence.EntityState;

namespace Pentamic.SSBI.Services.Breeze
{
    public class DataModelEntityService
    {
        private readonly DbPersistenceManager<AppDbContext> _persistenceManager;
        private readonly MetadataService _metadataService;
        private readonly DaxExpressionGenerator _daxExpressionGenerator;

        private readonly IUserResolver _userResolver;

        private string UserId => _userResolver.GetUserId();
        private string UserName => _userResolver.GetUserName();

        public DataModelEntityService(DbPersistenceManager<AppDbContext> persistenceManager,
            MetadataService metadataService,
            IUserResolver userResolver, DaxExpressionGenerator daxExpressionGenerator)
        {
            _persistenceManager = persistenceManager;
            _metadataService = metadataService;
            _userResolver = userResolver;
            _daxExpressionGenerator = daxExpressionGenerator;
        }

        private AppDbContext Context => _persistenceManager.Context;

        public IQueryable<Model> Models => Context.Models.Where(x => x.CreatedBy == UserId)
                    .Concat(Context.ModelSharings.Where(x => x.UserId == UserId).Select(x => x.Model));
        public IQueryable<ModelRole> Roles => Context.Roles;
        public IQueryable<DataSource> DataSources => Context.DataSources;
        public IQueryable<Table> Tables => Context.Tables;
        public IQueryable<Column> Columns => Context.Columns;
        public IQueryable<Partition> Partitions => Context.Partitions;
        public IQueryable<Measure> Measures => Context.Measures;
        public IQueryable<Relationship> Relationships => Context.Relationships;
        public IQueryable<Perspective> Perspectives => Context.Perspectives;
        public IQueryable<Hierarchy> Hierarchies => Context.Hierarchies;
        public IQueryable<Level> Levels => Context.Levels;
        public IQueryable<SourceFile> SourceFiles => Context.SourceFiles;
        public IQueryable<ModelRoleTablePermission> RoleTablePermissions => Context.RoleTablePermissions;
        public IQueryable<UserModelRole> UserRoles => Context.UserRoles;
        public IQueryable<ModelSharing> ModelSharings => Context.ModelSharings;
        public IQueryable<UserFavoriteModel> UserFavoriteModels => Context.UserFavoriteModels
            .Where(x => x.UserId == UserId);
        public IQueryable<UserModelActivity> UserModelActivities => Context.UserModelActivities
            .Where(x => x.UserId == UserId);

        public IQueryable<UserModelActivity> GetUserRecentModels()
        {
            return Context.UserModelActivities
                .GroupBy(x => x.ModelId)
                .OrderByDescending(x => x.Max(y => y.CreatedAt))
                .Take(10)
                .Select(x => x.OrderByDescending(y => y.CreatedAt).FirstOrDefault())
                .Include(x => x.Model);
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

        public async Task<SourceFile> CreateSourceFile(string fileName, string filePath)
        {
            var sourceFile = new SourceFile
            {
                FileName = fileName,
                FilePath = filePath,
            };
            Context.SourceFiles.Add(sourceFile);
            await Context.SaveChangesAsync();
            return sourceFile;
        }

        //public Model ImportModel(MultipartFormDataStreamProvider provider)
        //{
        //    var file = provider.FileData[0];
        //    using (FileStream compressedFileStream = File.OpenRead(file.LocalFileName))
        //    {
        //        using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Decompress))
        //        {
        //            string contents;
        //            using (var sr = new StreamReader(compressionStream))
        //            {
        //                contents = sr.ReadToEnd();
        //                var model = JsonConvert.DeserializeObject<Model>(contents);
        //                Context.Models.Add(model);
        //                Context.SaveChanges();
        //                return model;
        //            }
        //        }
        //    }
        //}

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

        //public List<Dictionary<string, object>> GetTableData(TableQueryModel queryModel)
        //{
        //    var model = Context.Models.Find(queryModel.ModelId);
        //    if (model == null) { throw new Exception("Model not found"); }
        //    var query = $" EVALUATE TOPN(50,'{queryModel.TableName}' ";
        //    if (!string.IsNullOrEmpty(queryModel.OrderBy))
        //    {
        //        query += $",[{queryModel.OrderBy}]";
        //        if (queryModel.OrderDesc)
        //        {
        //            query += ", 0";
        //        }
        //        else
        //        {
        //            query += ", 1";
        //        }
        //    }
        //    query += ")";

        //    var conStrBuilder = new OleDbConnectionStringBuilder(_asConnectionString)
        //    {
        //        ["Catalog"] = queryModel.ModelId.ToString()
        //    };
        //    using (var conn = new AC.AdomdConnection(conStrBuilder.ToString()))
        //    {
        //        try
        //        {
        //            conn.Open();
        //            var command = conn.CreateCommand();
        //            command.CommandText = query;
        //            using (var reader = command.ExecuteReader())
        //            {
        //                var result = new List<Dictionary<string, object>>();
        //                while (reader.Read())
        //                {
        //                    var row = new Dictionary<string, object>();
        //                    var columns = new List<string>();
        //                    for (var i = 0; i < reader.FieldCount; ++i)
        //                    {
        //                        var name = reader.GetName(i);
        //                        var si = name.IndexOf("[") + 1;
        //                        var ei = name.IndexOf("]");
        //                        columns.Add(name.Substring(si, ei - si));
        //                    }
        //                    for (var i = 0; i < reader.FieldCount; ++i)
        //                    {
        //                        row[columns[i]] = reader.GetValue(i);
        //                    }
        //                    result.Add(row);
        //                }
        //                return result;
        //            }
        //        }
        //        finally
        //        {
        //            if (conn != null)
        //            {
        //                conn.Close();
        //            }
        //        }
        //    }
        //}

        public SaveResult SaveChanges(JObject saveBundle)
        {
            var txSettings = new TransactionSettings { TransactionType = TransactionType.TransactionScope };
            _persistenceManager.BeforeSaveEntityDelegate += BeforeSaveEntity;
            _persistenceManager.BeforeSaveEntitiesDelegate += BeforeSaveEntities;
            _persistenceManager.AfterSaveEntitiesDelegate += AfterSaveEntities;
            return _persistenceManager.SaveChanges(saveBundle, txSettings);
        }

        public SaveResult SaveImport(JObject saveBundle)
        {
            var txSettings = new TransactionSettings { TransactionType = TransactionType.TransactionScope };
            _persistenceManager.BeforeSaveEntityDelegate += BeforeSaveEntity;
            _persistenceManager.BeforeSaveEntitiesDelegate += BeforeSaveEntities;
            _persistenceManager.AfterSaveEntitiesDelegate += AfterSaveImport;
            return _persistenceManager.SaveChanges(saveBundle, txSettings);
        }

        protected bool BeforeSaveEntity(EntityInfo info)
        {
            if (info.Entity is IAuditable entity1)
            {
                switch (info.EntityState)
                {
                    case EntityState.Added:
                        entity1.CreatedAt = DateTimeOffset.Now;
                        entity1.CreatedBy = UserId;
                        entity1.ModifiedAt = DateTimeOffset.Now;
                        entity1.ModifiedBy = UserId;
                        break;
                    case EntityState.Modified:
                        entity1.ModifiedAt = DateTimeOffset.Now;
                        entity1.ModifiedBy = UserId;
                        break;
                    case EntityState.Detached:
                        break;
                    case EntityState.Unchanged:
                        break;
                    case EntityState.Deleted:
                        break;
                }
            }
            if (info.Entity is IShareInfo)
            {
                var entity = info.Entity as IShareInfo;
                switch (info.EntityState)
                {
                    case EntityState.Added:
                        entity.SharedAt = DateTimeOffset.Now;
                        entity.SharedBy = UserId;
                        break;
                    case EntityState.Modified:
                        entity.SharedAt = DateTimeOffset.Now;
                        break;
                }
            }
            if (info.Entity is UserFavoriteModel ufm && info.EntityState == EntityState.Added)
            {
                ufm.UserId = UserId;
            }
            if (info.Entity is UserModelActivity uma && info.EntityState == EntityState.Added)
            {
                uma.UserId = UserId;
            }
            return true;
        }
        protected Dictionary<Type, List<EntityInfo>> BeforeSaveEntities(Dictionary<Type, List<EntityInfo>> saveMap)
        {
            List<EntityInfo> eis;
            if (saveMap.TryGetValue(typeof(Model), out eis))
            {
                foreach (var ei in eis)
                {
                    if (ei.EntityState == EntityState.Added)
                    {
                        var e = ei.Entity as Model;
                        if (e == null) continue;
                        var r1 = new ModelRole
                        {
                            Name = "Administrator",
                            Description = "Administrator",
                            ModelId = e.Id,
                            ModelPermission = ModelPermission.Administrator
                        };
                        var r2 = new ModelRole
                        {
                            Name = "User",
                            Description = "User",
                            ModelId = e.Id,
                            ModelPermission = ModelPermission.Read
                        };
                        var r1E = _persistenceManager.CreateEntityInfo(r1);
                        var r2E = _persistenceManager.CreateEntityInfo(r2);
                        if (!saveMap.TryGetValue(typeof(ModelRole), out var roles))
                        {
                            roles = new List<EntityInfo>();
                            saveMap.Add(typeof(ModelRole), roles);
                        }
                        roles.Add(r1E);
                        roles.Add(r2E);
                        if (!saveMap.TryGetValue(typeof(UserModelRole), out var userRoles))
                        {
                            userRoles = new List<EntityInfo>();
                            saveMap.Add(typeof(UserModelRole), userRoles);
                        }
                        userRoles.Add(_persistenceManager.CreateEntityInfo(new UserModelRole
                        {
                            UserId = UserId,
                            Role = r1
                        }));
                    }
                }
            }

            if (saveMap.TryGetValue(typeof(DataSource), out eis))
            {
                var names = eis.Where(x => x.EntityState == EntityState.Added).Select(x => x.Entity as DataSource)
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
                var names = eis.Where(x => x.EntityState == EntityState.Added).Select(x => x.Entity as Table)
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
                foreach (var ei in eis)
                {
                    if (ei.EntityState == EntityState.Added)
                    {
                        var e = ei.Entity as Table;
                        if (e.DataCategory == "Time")
                        {
                            var annotations = e.Annotation.Split(':');
                            var fromDate = DateTime.ParseExact(annotations[0], "yyyyMMdd", CultureInfo.InvariantCulture);
                            var toDate = DateTime.ParseExact(annotations[1], "yyyyMMdd", CultureInfo.InvariantCulture);
                            var partition = new Partition
                            {
                                Name = "Administrator",
                                Table = e,
                                TableId = e.Id,
                                SourceType = PartitionSourceType.Calculated,
                                Expression = _daxExpressionGenerator.CreateDateTableExpression(fromDate, toDate)
                            };
                            e.Partitions = new List<Partition>
                            {
                                partition
                            };
                            var partitionEntity = _persistenceManager.CreateEntityInfo(partition);
                            if (!saveMap.TryGetValue(typeof(Partition), out var partitions))
                            {
                                partitions = new List<EntityInfo>();
                                saveMap.Add(typeof(Partition), partitions);
                            }
                        }
                    }
                }
            }
            if (saveMap.TryGetValue(typeof(Column), out eis))
            {
                var names = eis.Where(x => x.EntityState == EntityState.Added).Select(x => x.Entity as Column)
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
                var names = eis.Where(x => x.EntityState == EntityState.Added).Select(x => x.Entity as Partition)
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
                var names = eis.Where(x => x.EntityState == EntityState.Added).Select(x => x.Entity as Measure)
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
            List<EntityInfo> eis;
            if (saveMap.TryGetValue(typeof(Model), out eis))
            {
                foreach (var ei in eis)
                {
                    var e = ei.Entity as Model;
                    if (ei.EntityState == EntityState.Added)
                    {
                        if (!string.IsNullOrEmpty(e.GenerateFromTemplate))
                        {
                            //_metadataService.GenerateModelFromDatabase(e);
                        }
                        else if (e.CloneFromModelId != null)
                        {

                        }
                        else
                        {
                            _metadataService.CreateModel(e);
                        }
                    }
                    if (ei.EntityState == EntityState.Modified)
                    {
                        _metadataService.UpdateModel(e);
                        if (ei.OriginalValuesMap.ContainsKey("RefreshSchedule"))
                        {
                            if (string.IsNullOrEmpty(e.RefreshSchedule))
                            {
                                if (!string.IsNullOrEmpty(e.RefreshJobId))
                                {
                                    //RecurringJob.RemoveIfExists(e.RefreshJobId);
                                }
                                e.RefreshJobId = null;
                                Context.SaveChanges();
                            }
                            else
                            {
                                string jobId;
                                if (string.IsNullOrEmpty(e.RefreshJobId))
                                {
                                    jobId = Guid.NewGuid().ToString();
                                    e.RefreshJobId = jobId;
                                }
                                else
                                {
                                    jobId = e.RefreshJobId;
                                }
                                //RecurringJob.AddOrUpdate(jobId, () => EnqueueRefreshModel(e.Id), e.RefreshSchedule);
                                Context.SaveChanges();
                            }
                        }
                    }
                    if (ei.EntityState == EntityState.Deleted)
                    {
                        _metadataService.DeleteModel(e);
                        if (!string.IsNullOrEmpty(e.RefreshJobId))
                        {
                            //RecurringJob.RemoveIfExists(e.RefreshJobId);
                        }
                    }
                }
            }
            if (saveMap.TryGetValue(typeof(DataSource), out eis))
            {
                foreach (var ei in eis)
                {
                    var e = ei.Entity as DataSource;
                    if (ei.EntityState == EntityState.Added)
                    {
                        if (e.SourceFileId != null && e.SourceFile == null)
                        {
                            Context.Entry(e).Reference(x => x.SourceFile).Load();
                        }
                        _metadataService.CreateDataSource(e, "");
                    }
                    if (ei.EntityState == EntityState.Modified)
                    {
                        if (e.SourceFileId != null && e.SourceFile == null)
                        {
                            Context.Entry(e).Reference(x => x.SourceFile).Load();
                        }
                        _metadataService.UpdateDataSource(e);
                    }
                    if (ei.EntityState == EntityState.Deleted)
                    {
                        _metadataService.DeleteDataSource(e);
                    }
                }
            }
            if (saveMap.TryGetValue(typeof(Table), out eis))
            {
                var newTables = eis.Where(x => x.EntityState == EntityState.Added)
                    .Select(x => x.Entity as Table)
                    .GroupBy(x => x.ModelId).Select(x => new
                    {
                        ModelId = x.Key,
                        Tables = x.ToList()
                    });
                foreach (var group in newTables)
                {
                    _metadataService.CreateTables(group.ModelId, group.Tables);
                }
                foreach (var ei in eis)
                {
                    var e = ei.Entity as Table;
                    if (ei.EntityState == EntityState.Modified)
                    {
                        if (ei.OriginalValuesMap.ContainsKey("Name"))
                        {
                            _metadataService.RenameTable(e.ModelId.ToString(), ei.OriginalValuesMap["Name"].ToString(), e.Name);
                        }
                        _metadataService.UpdateTable(e);
                    }
                    if (ei.EntityState == EntityState.Deleted)
                    {
                        _metadataService.DeleteTable(e);
                    }
                }
            }
            if (saveMap.TryGetValue(typeof(Relationship), out eis))
            {
                foreach (var ei in eis)
                {
                    var e = ei.Entity as Relationship;
                    if (ei.EntityState == EntityState.Added)
                    {
                        _metadataService.CreateRelationship(e.ModelId.ToString(), e);
                    }
                    if (ei.EntityState == EntityState.Modified)
                    {
                        _metadataService.UpdateRelationship(e.ModelId.ToString(), e);
                    }
                    if (ei.EntityState == EntityState.Deleted)
                    {
                        _metadataService.DeleteRelationship(e.ModelId.ToString(), e);
                    }
                }
            }
            if (saveMap.TryGetValue(typeof(Column), out eis))
            {
                foreach (var ei in eis)
                {
                    var e = ei.Entity as Column;
                    var info = Context.Tables.Where(x => x.Id == e.TableId).Select(x => new
                    {
                        x.Name,
                        x.ModelId
                    }).FirstOrDefault();
                    if (ei.EntityState == EntityState.Added)
                    {
                        _metadataService.CreateColumn(info?.ModelId.ToString(), info?.Name, e);
                    }
                    if (ei.EntityState == EntityState.Modified)
                    {

                        if (ei.OriginalValuesMap.ContainsKey("Name"))
                        {
                            _metadataService.RenameColumn(info?.ModelId.ToString(), info?.Name, ei.OriginalValuesMap["Name"].ToString(), e.Name);
                        }
                        _metadataService.UpdateColumn(info?.ModelId.ToString(), info?.Name, e);
                    }
                    if (ei.EntityState == EntityState.Deleted)
                    {
                        _metadataService.DeleteColumn(info?.ModelId.ToString(), info?.Name, e);
                    }
                }
            }
            if (saveMap.TryGetValue(typeof(Partition), out eis))
            {
                foreach (var ei in eis)
                {
                    var e = ei.Entity as Partition;
                    var info = Context.Tables.Where(x => x.Id == e.TableId).Select(x => new
                    {
                        x.Name,
                        x.ModelId
                    }).FirstOrDefault();
                    if (ei.EntityState == EntityState.Modified)
                    {
                        _metadataService.UpdatePartition(info?.ModelId.ToString(), info?.Name, e);
                    }
                }
            }
            if (saveMap.TryGetValue(typeof(Measure), out eis))
            {
                foreach (var ei in eis)
                {
                    var e = ei.Entity as Measure;
                    var info = Context.Tables.Where(x => x.Id == e.TableId).Select(x => new
                    {
                        x.Name,
                        x.ModelId
                    }).FirstOrDefault();
                    if (ei.EntityState == EntityState.Added)
                    {
                        _metadataService.CreateMeasure(info?.ModelId.ToString(), info?.Name, e);
                    }
                    if (ei.EntityState == EntityState.Modified)
                    {
                        if (ei.OriginalValuesMap.ContainsKey("Name"))
                        {
                            _metadataService.RenameMeasure(info?.ModelId.ToString(), info?.Name, ei.OriginalValuesMap["Name"].ToString(), e.Name);
                        }
                        _metadataService.UpdateMeasure(info?.ModelId.ToString(), info?.Name, e);
                    }
                    if (ei.EntityState == EntityState.Deleted)
                    {
                        _metadataService.DeleteMeasure(info?.ModelId.ToString(), info?.Name, e);
                    }
                }
            }
            if (saveMap.TryGetValue(typeof(ModelRole), out eis))
            {
                foreach (var ei in eis)
                {
                    var e = ei.Entity as ModelRole;
                    if (ei.EntityState == EntityState.Added)
                    {
                        _metadataService.CreateRole(e.ModelId.ToString(), e);
                    }
                    if (ei.EntityState == EntityState.Modified)
                    {
                        _metadataService.UpdateRole(e.ModelId.ToString(), e);
                    }
                    if (ei.EntityState == EntityState.Deleted)
                    {
                        _metadataService.DeleteRole(e.ModelId.ToString(), e);
                    }
                }
            }
            if (saveMap.TryGetValue(typeof(ModelRoleTablePermission), out eis))
            {
                foreach (var ei in eis)
                {
                    var e = ei.Entity as ModelRoleTablePermission;
                    var info = Context.RoleTablePermissions.Where(x => x.RoleId == e.RoleId && x.TableId == e.TableId)
                        .Select(x => new
                        {
                            RoleName = x.Role.Name,
                            TableName = x.Table.Name,
                            x.Role.ModelId
                        }).FirstOrDefault();
                    if (ei.EntityState == EntityState.Added)
                    {
                        _metadataService.CreateRoleTablePermission(info.ModelId.ToString(), info.RoleName, info.TableName, e);
                    }
                    if (ei.EntityState == EntityState.Modified)
                    {
                        _metadataService.UpdateRoleTablePermission(info.ModelId.ToString(), info.RoleName, info.TableName, e);
                    }
                    if (ei.EntityState == EntityState.Deleted)
                    {
                        _metadataService.DeleteRoleTablePermission(info.ModelId.ToString(), info.RoleName, e);
                    }
                }
            }

        }

        protected void AfterSaveImport(Dictionary<Type, List<EntityInfo>> saveMap, List<KeyMapping> keyMappings)
        {
            List<EntityInfo> eis;
            if (saveMap.TryGetValue(typeof(Table), out eis))
            {
                var newTables = eis.Where(x => x.EntityState == EntityState.Added)
                    .Select(x => x.Entity as Table)
                    .GroupBy(x => x.ModelId).Select(x => new
                    {
                        ModelId = x.Key,
                        Tables = x.ToList()
                    });
                foreach (var group in newTables)
                {
                    var calTables = _metadataService.CreateTables(group.ModelId, group.Tables);
                    foreach (var calTable in calTables)
                    {
                        Context.Columns.AddRange(calTable.Columns);
                    }
                    Context.SaveChanges();
                }
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

        //public void CreateDateTable(DateTableCreateModel model)
        //{
        //    if (Context.Tables.Any(x => x.ModelId == model.ModelId && x.Name == model.TableName))
        //    {
        //        throw new Exception("Table exist");
        //    }
        //    var table = new Table
        //    {
        //        Name = model.TableName,
        //        ModelId = model.ModelId,
        //        Columns = new List<Column>(),
        //        Partitions = new List<Partition>(),
        //        DataCategory = "Time"
        //    };
        //    Context.Tables.Add(table);
        //    var tmp = new[]
        //        {
        //            "\"DateKey\"", "INTEGER",
        //            "\"Date\"", "DATETIME",
        //            "\"DateName\"","STRING",
        //            "\"PreviousMonthDate\"", "DATETIME",
        //            "\"PreviousQuarterDate\"", "DATETIME",
        //            "\"PreviousYearDate\"", "DATETIME",
        //            "\"SequentialDayNumber\"", "INTEGER",
        //            "\"Year\"","INTEGER",
        //            "\"YearName\"","STRING",
        //            "\"Month\"","INTEGER",
        //            "\"MonthName\"","STRING",
        //            "\"Quarter\"","INTEGER",
        //            "\"QuarterName\"","STRING",
        //            "\"HalfYear\"","INTEGER",
        //            "\"HalfYearName\"","STRING",
        //            "\"DayOfMonth\"","INTEGER",
        //            "\"DayOfMonthName\"","STRING",
        //            "\"DayOfWeek\"","INTEGER",
        //            "\"DayOfWeekName\"","STRING",
        //            "\"DayOfYear\"","INTEGER",
        //            "\"DayOfQuarter\"","INTEGER",
        //            "\"MonthOfYear\"","INTEGER",
        //            "\"MonthOfYearName\"","STRING",
        //            "\"MonthTotalDays\"","INTEGER",
        //            "\"QuarterOfYear\"","INTEGER",
        //            "\"QuarterOfYearName\"","STRING",
        //            "\"QuarterTotalDays\"","INTEGER",
        //            "\"HalfYearOfYear\"","INTEGER",
        //            "\"HalfYearOfYearName\"","STRING",
        //            "\"LunarDate\"","INTEGER",
        //            "\"LunarDateName\"","STRING",
        //            "\"LunarMonth\"","INTEGER",
        //            "\"LunarMonthName\"","STRING",
        //            "\"LunarQuarter\"","INTEGER",
        //            "\"LunarQuarterName\"","STRING",
        //            "\"LunarYear\"","INTEGER",
        //            "\"LunarYearName\"","STRING",
        //            "\"LunarDayOfWeek\"","INTEGER",
        //            "\"LunarDayOfWeekName\"","STRING",
        //            "\"LunarDayOfMonth\"","INTEGER",
        //            "\"LunarDayOfMonthName\"","STRING",
        //            "\"LunarMonthOfYear\"","INTEGER",
        //            "\"LunarMonthOfYearName\"","STRING",
        //            "\"LunarQuarterOfYear\"","INTEGER",
        //            "\"LunarQuarterOfYearName\"","STRING",
        //            "\"EventName\"","STRING"
        //        };
        //    var dataExpr = BuildDateTableExpression(model.FromDate, model.ToDate);
        //    var colExpr = string.Join(", ", tmp);
        //    var par = new Partition
        //    {
        //        Name = "DefaultPartition",
        //        Expression = $"DATATABLE ( {colExpr}, {dataExpr} )",
        //        SourceType = PartitionSourceType.Calculated
        //    };
        //    table.Partitions.Add(par);

        //    using (var server = new AS.Server())
        //    {
        //        server.Connect(_asConnectionString);
        //        var database = server.Databases.FindByName(model.ModelId.ToString());
        //        if (database == null)
        //        {
        //            throw new ArgumentException("Database not found");
        //        }
        //        var tb = AddCalculatedTable(database, model.ModelId, table);
        //        tb.RequestRefresh(AS.RefreshType.Full);
        //        database.Update(AN.UpdateOptions.ExpandFull);
        //        foreach (var column in tb.Columns)
        //        {
        //            if (column is AS.CalculatedTableColumn col)
        //            {
        //                var c = new Column()
        //                {
        //                    Name = col.Name,
        //                    DataType = (ColumnDataType)col.DataType,
        //                    SourceColumn = col.SourceColumn,
        //                    ColumnType = ColumnType.CalculatedTableColumn
        //                };
        //                if (c.Name == "Date")
        //                {
        //                    c.IsKey = true;
        //                }
        //                table.Columns.Add(c);
        //            }
        //        }
        //        Context.SaveChanges();
        //    }
        ////}

        public string ExportModelTemplate(int modelId, string exportPath)
        {
            var model = Context.Models.Where(x => x.Id == modelId)
                .Include(x => x.Tables.Select(y => y.Columns))
                .Include(x => x.Tables.Select(y => y.Partitions))
                .Include(x => x.Tables.Select(y => y.Measures))
                .Include(x => x.DataSources)
                .Include(x => x.Relationships).FirstOrDefault();

            if (model == null)
            {
                throw new Exception("Model not found");
            }

            var json = JsonConvert.SerializeObject(model, Formatting.Indented,
                            new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

                            });
            if (!Directory.Exists(exportPath))
            {
                Directory.CreateDirectory(exportPath);
            }
            var fileName = $"{model.Id}-{model.Name}-{DateTime.Now:yyyyMMddhhmmss}.json";
            var filePath = Path.Combine(exportPath, fileName);
            using (var file = new StreamWriter(filePath))
            {
                file.Write(json);
            }
            var fileInfo = new FileInfo(filePath);
            var cFilePath = fileInfo.FullName + ".psm";
            using (var originalFileStream = fileInfo.OpenRead())
            {
                using (var compressedFileStream = File.Create(cFilePath))
                {
                    using (var compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                    {
                        originalFileStream.CopyTo(compressionStream);
                        return cFilePath;
                    }
                }
            }
        }
    }
}