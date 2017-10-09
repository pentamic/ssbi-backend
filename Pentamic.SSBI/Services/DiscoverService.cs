using System;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using Pentamic.SSBI.Models.DataModel;
using Pentamic.SSBI.Models.Discover;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Data.OleDb;
using Pentamic.SSBI.Models.DataModel.Objects;
using System.Dynamic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using AS = Microsoft.AnalysisServices.Tabular;
using System.Web;
using Pentamic.SSBI.Models.DataModel.Connections;

namespace Pentamic.SSBI.Services
{
    public class DiscoverService
    {
        private readonly DataModelContext _dataModelContext;

        private readonly string _asConnectionString = "";
        //System.Configuration.ConfigurationManager
        //    .ConnectionStrings["AnalysisServiceConnection"]
        //    .ConnectionString;

        public DiscoverService()
        {
            _dataModelContext = new DataModelContext();
        }

        public JArray DiscoverModel(int modelId, string perspective)
        {
            var model = _dataModelContext.Models.Find(modelId);
            if (model == null)
            {
                throw new ArgumentException("Model not found");
            }
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var db = server.Databases.FindByName(model.DatabaseName);
                if (string.IsNullOrEmpty(perspective))
                {
                    var tables = new JArray();
                    foreach (var tb in db.Model.Tables)
                    {
                        if (tb.IsHidden) continue;
                        var table = new JObject
                        {
                            ["name"] = tb.Name
                        };
                        var fields = new JArray();
                        foreach (var co in tb.Columns)
                        {
                            if (co.IsHidden) continue;
                            var field = new JObject
                            {
                                ["tableName"] = tb.Name,
                                ["name"] = co.Name,
                                ["dataType"] = (int)co.DataType,
                                ["displayFolder"] = co.DisplayFolder
                            };
                            fields.Add(field);
                        }
                        foreach (var me in tb.Measures)
                        {
                            if (me.IsHidden) continue;
                            var field = new JObject
                            {
                                ["tableName"] = tb.Name,
                                ["name"] = me.Name,
                                ["dataType"] = (int)me.DataType,
                                ["displayFolder"] = me.DisplayFolder,
                                ["isMeasure"] = true
                            };
                            fields.Add(field);
                        }
                        foreach (var hi in tb.Hierarchies)
                        {
                            if (hi.IsHidden) continue;
                            var field = new JObject
                            {
                                ["tableName"] = tb.Name,
                                ["name"] = hi.Name,
                                ["displayFolder"] = hi.DisplayFolder,
                                ["isHierarchy"] = true
                            };
                            var levels = new JArray();
                            foreach (var le in hi.Levels)
                            {
                                var level = new JObject
                                {
                                    ["name"] = le.Name,
                                    ["ordinal"] = le.Ordinal
                                };
                                levels.Add(level);
                            }
                            field["levels"] = levels;
                            fields.Add(field);
                        }
                        table["fields"] = fields;
                        tables.Add(table);
                    }
                    return tables;
                }
                else
                {
                    var per = db.Model.Perspectives.Find(perspective);
                    var tables = new JArray();
                    foreach (var tb in per.PerspectiveTables)
                    {
                        if (tb.Table.IsHidden) continue;
                        var table = new JObject
                        {
                            ["name"] = tb.Name
                        };
                        var fields = new JArray();
                        foreach (var co in tb.PerspectiveColumns)
                        {
                            if (co.Column.IsHidden) continue;
                            var field = new JObject
                            {
                                ["tableName"] = tb.Name,
                                ["name"] = co.Name,
                                ["dataType"] = (int)co.Column.DataType,
                                ["displayFolder"] = co.Column.DisplayFolder
                            };
                            fields.Add(field);
                        }
                        foreach (var me in tb.PerspectiveMeasures)
                        {
                            if (me.Measure.IsHidden) continue;
                            var field = new JObject
                            {
                                ["tableName"] = tb.Name,
                                ["name"] = me.Name,
                                ["dataType"] = (int)me.Measure.DataType,
                                ["displayFolder"] = me.Measure.DisplayFolder,
                                ["isMeasure"] = true
                            };
                            fields.Add(field);
                        }
                        foreach (var hi in tb.PerspectiveHierarchies)
                        {
                            if (hi.Hierarchy.IsHidden) continue;
                            var field = new JObject
                            {
                                ["tableName"] = tb.Name,
                                ["name"] = hi.Name,
                                ["displayFolder"] = hi.Hierarchy.DisplayFolder,
                                ["isHierarchy"] = true
                            };
                            var levels = new JArray();
                            foreach (var le in hi.Hierarchy.Levels)
                            {
                                var level = new JObject
                                {
                                    ["name"] = le.Name,
                                    ["ordinal"] = le.Ordinal
                                };
                                levels.Add(level);
                            }
                            field["levels"] = levels;
                            fields.Add(field);
                        }
                        table["fields"] = fields;
                        tables.Add(table);
                    }
                    return tables;
                }

            }
        }

        public async Task<List<CatalogDiscoverResult>> DiscoverCatalogs(int dsId)
        {
            var ds = _dataModelContext.DataSources.Find(dsId);
            if (ds == null)
            {
                throw new ArgumentException("Data Source not found");
            }
            var conStr = GetDataSourceConnectionString(ds.Connection);
            using (var con = new OleDbConnection(conStr))
            {
                var result = new List<CatalogDiscoverResult>();
                await con.OpenAsync();
                var dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Catalogs, null);
                if (dt != null)
                    foreach (DataRow row in dt.Rows)
                    {
                        result.Add(new CatalogDiscoverResult
                        {
                            CatalogName = row["CATALOG_NAME"].ToString(),
                        });
                    }
                return result;
            }
        }

        public async Task<List<CatalogDiscoverResult>> DiscoverCatalogs(DataSource ds)
        {
            var conStr = GetDataSourceConnectionString(ds.Connection);
            using (var con = new OleDbConnection(conStr))
            {
                var result = new List<CatalogDiscoverResult>();
                await con.OpenAsync();
                var dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Catalogs, null);
                if (dt != null)
                    foreach (DataRow row in dt.Rows)
                    {
                        result.Add(new CatalogDiscoverResult
                        {
                            CatalogName = row["CATALOG_NAME"].ToString(),
                        });
                    }
                return result;
            }
        }

        public async Task<List<TableDiscoverResult>> DiscoverTables(int dsId)
        {
            var ds = _dataModelContext.DataSources.Find(dsId);
            if (ds == null)
            {
                throw new ArgumentException("Data Source not found");
            }
            var conStr = GetDataSourceConnectionString(ds.Connection);
            using (var con = new OleDbConnection(conStr))
            {
                var result = new List<TableDiscoverResult>();
                await con.OpenAsync();
                var dt1 = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new[] { null, null, null, "TABLE" });
                var dt2 = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new[] { null, null, null, "VIEW" });
                if (dt1 != null)
                    foreach (DataRow row in dt1.Rows)
                    {
                        var obj = new TableDiscoverResult
                        {
                            TableName = row["TABLE_NAME"].ToString(),
                            TableSchema = row["TABLE_SCHEMA"].ToString(),
                            TableType = row["TABLE_TYPE"].ToString()
                        };
                        result.Add(obj);
                    }
                if (dt2 != null)
                {
                    result.AddRange(from DataRow row in dt2.Rows
                        select new TableDiscoverResult
                        {
                            TableName = row["TABLE_NAME"].ToString(),
                            TableSchema = row["TABLE_SCHEMA"].ToString(),
                            TableType = row["TABLE_TYPE"].ToString()
                        });
                }
                return result;
            }
        }

        public async Task<List<TableDiscoverResult>> DiscoverTables(DataSource ds)
        {
            var conStr = GetDataSourceConnectionString(ds.Connection);
            using (var con = new OleDbConnection(conStr))
            {
                await con.OpenAsync();
                var dt1 = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new[] { null, null, null, "TABLE" });
                var dt2 = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new[] { null, null, null, "VIEW" });
                var result = (from DataRow row in dt1?.Rows
                    select new TableDiscoverResult
                    {
                        TableName = row["TABLE_NAME"].ToString(),
                        TableSchema = row["TABLE_SCHEMA"].ToString(),
                        TableType = row["TABLE_TYPE"].ToString()
                    }).ToList();
                result.AddRange(from DataRow row in dt2?.Rows
                    select new TableDiscoverResult
                    {
                        TableName = row["TABLE_NAME"].ToString(),
                        TableSchema = row["TABLE_SCHEMA"].ToString(),
                        TableType = row["TABLE_TYPE"].ToString()
                    });
                return result;
            }
        }

        public async Task<List<ColumnDiscoverResult>> DiscoverColumns(int dsId, string tableSchema, string tableName)
        {
            var ds = _dataModelContext.DataSources.Find(dsId);
            if (ds == null)
            {
                throw new ArgumentException("Data Source not found");
            }
            var conStr = GetDataSourceConnectionString(ds.Connection);
            using (var con = new OleDbConnection(conStr))
            {
                await con.OpenAsync();
                var dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new[] { null, tableSchema, tableName, null });
                return (from DataRow row in dt.Rows
                    select new ColumnDiscoverResult
                    {
                        ColumnName = row["COLUMN_NAME"].ToString(),
                        DataType = ((OleDbType) Convert.ToInt32(row["DATA_TYPE"])).ToDataType().ToString(),
                        TableName = row["TABLE_NAME"].ToString(),
                        TableSchema = row["TABLE_SCHEMA"].ToString()
                    }).ToList();
            }
        }

        public async Task<List<ColumnDiscoverResult>> DiscoverColumns(DataSource ds, string tableSchema, string tableName)
        {
            var conStr = GetDataSourceConnectionString(ds.Connection);
            using (var con = new OleDbConnection(conStr))
            {
                await con.OpenAsync();
                var dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new[] { null, tableSchema, tableName, null });
                return (from DataRow row in dt.Rows
                    select new ColumnDiscoverResult
                    {
                        ColumnName = row["COLUMN_NAME"].ToString(),
                        DataType = ((OleDbType) Convert.ToInt32(row["DATA_TYPE"])).ToDataType().ToString(),
                        TableName = row["TABLE_NAME"].ToString(),
                        TableSchema = row["TABLE_SCHEMA"].ToString()
                    }).ToList();
            }
        }

        public async Task<List<RelationshipDiscoverResult>> DiscoverRelationships(DataSource ds, string fkTableSchema, string fkTableName)
        {
            var conStr = GetDataSourceConnectionString(ds.Connection);
            using (var con = new OleDbConnection(conStr))
            {
                await con.OpenAsync();
                var dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, new[] { null, null, null, null, fkTableSchema, fkTableName });
                return (from DataRow row in dt.Rows
                    select new RelationshipDiscoverResult
                    {
                        PkTableSchema = row["PK_TABLE_SCHEMA"].ToString(),
                        PkTableName = row["PK_TABLE_NAME"].ToString(),
                        FkTableSchema = row["FK_TABLE_SCHEMA"].ToString(),
                        FkTableName = row["FK_TABLE_NAME"].ToString()
                    }).ToList();
            }
        }

        public async Task<TableDetailResult> DiscoverTable(int dsId, string tableSchema, string tableName, string query)
        {
            var ds = _dataModelContext.DataSources.Find(dsId);
            if (ds == null)
            {
                throw new ArgumentException("Data Source not found");
            }
            var conStr = GetDataSourceConnectionString(ds.Connection);
            using (var con = new OleDbConnection(conStr))
            {
                using (var cmd = con.CreateCommand())
                {
                    if (query == null)
                    {
                        cmd.CommandText = string.IsNullOrEmpty(tableSchema) ?
                            $"SELECT TOP 100 * FROM [{tableName}]" :
                            $"SELECT TOP 100 * FROM [{tableSchema}].[{tableName}]";
                    }
                    else
                    {
                        cmd.CommandText = query;
                    }
                    await con.OpenAsync();
                    var data = new List<dynamic>();
                    var reader = await cmd.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        var row = new ExpandoObject() as IDictionary<string, object>;
                        for (var i = 0; i < reader.FieldCount; ++i)
                        {
                            row.Add(reader.GetName(i), reader.GetValue(i));
                        }
                        data.Add(row);
                    }
                    var schema = reader.GetSchemaTable();
                    var columns = (from DataRow sr in schema?.Rows
                        select new ColumnDiscoverResult
                        {
                            ColumnName = sr.Field<string>("ColumnName"),
                            DataType = GetDataType(sr.Field<Type>("DataType").Name).ToString()
                        }).ToList();

                    return new TableDetailResult
                    {
                        Data = data,
                        Columns = columns
                    };
                }
            }

        }

        public ColumnDataType GetDataType(string typeName)
        {
            switch (typeName)
            {
                case "Int16":
                case "Int32":
                case "Int64":
                    return ColumnDataType.Int64;
                case "String":
                    return ColumnDataType.String;
                case "DateTime":
                    return ColumnDataType.DateTime;
                case "Byte":
                case "Byte[]":
                    return ColumnDataType.Binary;
                case "Decimal":
                    return ColumnDataType.Decimal;
                case "Double":
                case "Single":
                    return ColumnDataType.Double;
                case "Boolean":
                    return ColumnDataType.Boolean;
                default:
                    return ColumnDataType.Unknown;
            }
        }

        public async Task<List<dynamic>> DiscoverTable(DataSource ds, string tableSchema, string tableName)
        {
            var conStr = GetDataSourceConnectionString(ds.Connection);
            using (var con = new OleDbConnection(conStr))
            {
                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = string.IsNullOrEmpty(tableSchema) ? $"SELECT TOP 100 * FROM [{tableName}]"
                        : $"SELECT TOP 100 * FROM [{tableSchema}].[{tableName}]";
                    await con.OpenAsync();
                    var result = new List<dynamic>();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var row = new ExpandoObject() as IDictionary<string, object>;
                            for (var i = 0; i < reader.FieldCount; ++i)
                            {
                                row.Add(reader.GetName(i), reader.GetValue(i));
                            }
                            result.Add(row);
                        }
                        return result;
                    }
                }
            }

        }

        public string GetDataSourceConnectionString(Connection connection)
        {
            string cs = null;
            switch (connection)
            {
                case SqlServerConnection sqlCon:
                    cs = sqlCon.IntegratedSecurity ? $"Provider=SQLNCLI11;Data Source={sqlCon.Server};Initial Catalog={sqlCon.Database};Integrated Security=SSPI;Persist Security Info=false" :
                        $"Provider=SQLNCLI11;Data Source={sqlCon.Server};Initial Catalog={sqlCon.Database};User ID={sqlCon.User};Password={sqlCon.Password};Persist Security Info=true";
                    break;
                case ExcelConnection excelCon:
                    var context = new DataModelContext();
                    if (excelCon.SourceFile == null)
                    {
                        excelCon.SourceFile = context.SourceFiles.Find(excelCon.SourceFileId);
                    }
                    var basePath = System.Configuration.ConfigurationManager.AppSettings["UploadBasePath"];
                    if (string.IsNullOrEmpty(basePath))
                    {
                        basePath = HttpContext.Current.Server.MapPath("~/Uploads");
                    }
                    if (excelCon.SourceFile != null)
                    {
                        var builder = new OleDbConnectionStringBuilder
                        {
                            Provider = "Microsoft.ACE.OLEDB.12.0",
                            DataSource = Path.Combine(basePath, excelCon.SourceFile.FilePath),
                            PersistSecurityInfo = false,
                            ["Mode"] = "Read"
                        };
                        var extension = Path.GetExtension(excelCon.SourceFile.FileName)?.ToUpper();
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
                    }
                    break;
            }
            return cs;
        }

        //--------------------------------------------

        //public async Task<List<TableDiscoverResult>> GetTables(int connectionId)
        //{
        //    var conn = _dataModelContext.Connections.Find(connectionId);
        //    if (conn == null)
        //    {
        //        throw new ArgumentException("Connection not found");
        //    }
        //    var conStr = GetDataSourceConnectionString(conn);
        //    using (var con = new OleDbConnection(conStr))
        //    {
        //        var result = new List<TableDiscoverResult>();
        //        await con.OpenAsync();
        //        var dt1 = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new[] { (object)null, (object)null, (object)null, "TABLE" });
        //        var dt2 = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new[] { (object)null, (object)null, (object)null, "VIEW" });
        //        if (dt1 != null)
        //        {
        //            result.AddRange(from DataRow row in dt1.Rows
        //                            select new TableDiscoverResult
        //                            {
        //                                TableName = row["TABLE_NAME"].ToString(),
        //                                TableSchema = row["TABLE_SCHEMA"].ToString(),
        //                                TableType = row["TABLE_TYPE"].ToString()
        //                            });
        //        }
        //        if (dt2 != null)
        //        {
        //            result.AddRange(from DataRow row in dt2.Rows
        //                            select new TableDiscoverResult
        //                            {
        //                                TableName = row["TABLE_NAME"].ToString(),
        //                                TableSchema = row["TABLE_SCHEMA"].ToString(),
        //                                TableType = row["TABLE_TYPE"].ToString()
        //                            });
        //        }
        //        return result;
        //    }
        //}

        //public async Task<TableDetailResult> GetTableInfo(int dsId, string tableSchema, string tableName, string query)
        //{
        //    var ds = _dataModelContext.DataSources.Find(dsId);
        //    if (ds == null)
        //    {
        //        throw new ArgumentException("Data Source not found");
        //    }
        //    var conStr = GetDataSourceConnectionString(ds.Connection);
        //    OleDbConnection con = null;
        //    DbDataReader reader = null;
        //    try
        //    {
        //        using (con = new OleDbConnection(conStr))
        //        {
        //            using (var cmd = con.CreateCommand())
        //            {
        //                if (query == null)
        //                {
        //                    cmd.CommandText = string.IsNullOrEmpty(tableSchema) ?
        //                        $"SELECT TOP 100 * FROM [{tableName}]" :
        //                        $"SELECT TOP 100 * FROM [{tableSchema}].[{tableName}]";
        //                }
        //                else
        //                {
        //                    cmd.CommandText = query;
        //                }
        //                await con.OpenAsync();
        //                var data = new List<dynamic>();
        //                reader = await cmd.ExecuteReaderAsync();
        //                while (reader.Read())
        //                {
        //                    var row = new ExpandoObject() as IDictionary<string, object>;
        //                    for (var i = 0; i < reader.FieldCount; ++i)
        //                    {
        //                        row.Add(reader.GetName(i), reader.GetValue(i));
        //                    }
        //                    data.Add(row);
        //                }
        //                var columns = new List<ColumnDiscoverResult>();
        //                var schema = reader.GetSchemaTable();

        //                foreach (DataRow sr in schema.Rows)
        //                {
        //                    columns.Add(new ColumnDiscoverResult
        //                    {
        //                        ColumnName = sr.Field<string>("ColumnName"),
        //                        DataType = GetDataType(sr.Field<Type>("DataType").Name).ToString()
        //                    });
        //                }

        //                return new TableDetailResult
        //                {
        //                    Data = data,
        //                    Columns = columns
        //                };
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        if (con != null)
        //        {
        //            con.Close();
        //        }
        //        if (reader != null)
        //        {
        //            reader.Close();
        //        }
        //    }
        //}


        public async Task<List<CatalogDiscoverResult>> GetCatalogs(int connectionId)
        {
            var connection = _dataModelContext.Connections.Find(connectionId);
            if (connection == null)
            {
                throw new ArgumentException("Data Source not found");
            }
            string schema = null;
            switch (connection)
            {
                case SqlServerConnection _:
                    schema = "databases";
                    break;
                case ExcelConnection _:
                    return null;
            }
            if (schema == null)
            {
                return null;
            }
            using (var dbCon = GetDbConnection(connection))
            {
                await dbCon.OpenAsync();
                var dt = dbCon.GetSchema(schema);
                return (from DataRow row in dt.Rows
                        select new CatalogDiscoverResult()
                        {
                            CatalogName = row["database_name"].ToString()
                        }).ToList();
            }
        }

        public async Task<List<TableDiscoverResult>> GetTables(int connectionId, string catalogName)
        {
            var connection = _dataModelContext.Connections.Find(connectionId);
            if (connection == null)
            {
                throw new ArgumentException("Connection not found");
            }
            if (connection is SqlServerConnection con)
            {
                if (!string.IsNullOrEmpty(catalogName))
                {
                    con.Database = catalogName;
                }
            }
            using (var dbCon = GetDbConnection(connection))
            {
                await dbCon.OpenAsync();
                var dt = dbCon.GetSchema("tables", new[] { catalogName, null, null, null });
                return (from DataRow dataRow in dt.Rows
                        select new TableDiscoverResult()
                        {
                            TableName = dataRow["TABLE_NAME"].ToString(),
                            TableSchema = dataRow["TABLE_SCHEMA"].ToString(),
                            TableType = dataRow["TABLE_TYPE"].ToString(),
                        }).ToList();
            }
        }

        public async Task<List<TableDiscoverResult>> GetRelatedTables(int connectionId, string catalogName, string tableSchema, string tableName)
        {
            var connection = _dataModelContext.Connections.Find(connectionId);
            if (connection == null)
            {
                throw new ArgumentException("Connection not found");
            }
            if (connection is SqlServerConnection con)
            {
                if (!string.IsNullOrEmpty(catalogName))
                {
                    con.Database = catalogName;
                }
            }
            using (var dbCon = GetDbConnection(connection))
            {
                await dbCon.OpenAsync();
                var dt = dbCon.GetSchema("tables", new[] { catalogName, null, null, null });
                return (from DataRow dataRow in dt.Rows
                        select new TableDiscoverResult()
                        {
                            TableName = dataRow["TABLE_NAME"].ToString(),
                            TableSchema = dataRow["TABLE_SCHEMA"].ToString(),
                            TableType = dataRow["TABLE_TYPE"].ToString(),
                        }).ToList();
            }
        }

        public async Task<List<RelationshipDiscoverResult>> DiscoverRelationships(int connectionId, string catalogName, string fkTableSchema, string fkTableName)
        {
            var connection = _dataModelContext.Connections.Find(connectionId);
            if (connection == null)
            {
                throw new ArgumentException("Connection not found");
            }
            if (connection is SqlServerConnection con)
            {
                if (!string.IsNullOrEmpty(catalogName))
                {
                    con.Database = catalogName;
                }
            }
            using (var dbCon = GetDbConnection(connection))
            {
                var result = new List<RelationshipDiscoverResult>();
                await dbCon.OpenAsync();
                var dt = dbCon.GetSchema("ForeignKeys", new[] { catalogName, fkTableSchema, fkTableName, null });
                foreach (DataRow row in dt.Rows)
                {
                    result.Add(new RelationshipDiscoverResult
                    {
                        PkTableSchema = row["PK_TABLE_SCHEMA"].ToString(),
                        PkTableName = row["PK_TABLE_NAME"].ToString(),
                        FkTableSchema = row["FK_TABLE_SCHEMA"].ToString(),
                        FkTableName = row["FK_TABLE_NAME"].ToString()
                    });
                }
                return result;
            }
        }


        public async Task<TableDetailResult> GetTableInfo(int connectionId, string tableSchema, string tableName, string query)
        {
            var connection = _dataModelContext.Connections.Find(connectionId);
            if (connection == null)
            {
                throw new ArgumentException("Connection not found");
            }
            using (var dbCon = GetDbConnection(connection))
            {
                using (var cmd = dbCon.CreateCommand())
                {
                    if (query == null)
                    {
                        cmd.CommandText = string.IsNullOrEmpty(tableSchema) ?
                            $"SELECT TOP 100 * FROM [{tableName}]" :
                            $"SELECT TOP 100 * FROM [{tableSchema}].[{tableName}]";
                    }
                    else
                    {
                        cmd.CommandText = query;
                    }
                    await dbCon.OpenAsync();
                    var data = new List<dynamic>();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            var row = new ExpandoObject() as IDictionary<string, object>;
                            for (var i = 0; i < reader.FieldCount; ++i)
                            {
                                row.Add(reader.GetName(i), reader.GetValue(i));
                            }
                            data.Add(row);
                        }
                        var columns = new List<ColumnDiscoverResult>();
                        var schema = reader.GetSchemaTable();
                        if (schema != null)
                        {
                            columns.AddRange(from DataRow sr in schema.Rows
                                             select new ColumnDiscoverResult
                                             {
                                                 ColumnName = sr.Field<string>("ColumnName"),
                                                 DataType = GetDataType(sr.Field<Type>("DataType").Name).ToString()
                                             });
                        }
                        return new TableDetailResult
                        {
                            Data = data,
                            Columns = columns
                        };
                    }
                }
            }
        }

        public DbConnection GetDbConnection(Connection connection)
        {
            var providerName = "";
            switch (connection)
            {
                case SqlServerConnection _:
                    providerName = "System.Data.SqlClient";
                    break;
                case ExcelConnection _:
                    providerName = "System.Data.OleDb";
                    break;
            }
            if (!IsDataProviderInstalled(providerName))
            {
                throw new Exception("Provider is not installed");
            }
            var factory = DbProviderFactories.GetFactory(providerName);
            var connectionString = GetConnectionString(connection);
            //var csb = factory.CreateConnectionStringBuilder();
            //make sure it got created
            //if (csb != null)
            //{
            //    csb.ConnectionString = connectionString;
            //    if (csb.ContainsKey("MultipleActiveResultSets"))
            //    {
            //        csb["MultipleActiveResultSets"] = true;
            //    }
            //    connectionString = csb.ConnectionString;
            //}
            var dbConnection = factory.CreateConnection();
            if (dbConnection != null)
            {
                dbConnection.ConnectionString = connectionString;
            }
            return dbConnection;
        }

        public string GetConnectionString(Connection connection)
        {
            string cs = null;
            switch (connection)
            {
                case SqlServerConnection sqlCon:
                    {
                        var builder = new SqlConnectionStringBuilder()
                        {
                            DataSource = sqlCon.Server,
                            IntegratedSecurity = sqlCon.IntegratedSecurity
                        };
                        if (!string.IsNullOrEmpty(sqlCon.Database))
                        {
                            builder.InitialCatalog = sqlCon.Database;
                        }
                        if (!sqlCon.IntegratedSecurity)
                        {
                            builder.UserID = sqlCon.User;
                            builder.Password = sqlCon.Password;
                        }
                        cs = builder.ConnectionString;
                    }
                    break;
                case ExcelConnection excelCon:
                    {
                        var context = new DataModelContext();
                        if (excelCon.SourceFile == null)
                        {
                            excelCon.SourceFile = context.SourceFiles.Find(excelCon.SourceFileId);
                        }
                        var basePath = System.Configuration.ConfigurationManager.AppSettings["UploadBasePath"];
                        if (string.IsNullOrEmpty(basePath))
                        {
                            basePath = HttpContext.Current.Server.MapPath("~/Uploads");
                        }
                        if (excelCon.SourceFile != null)
                        {
                            var builder = new OleDbConnectionStringBuilder
                            {
                                Provider = "Microsoft.ACE.OLEDB.12.0",
                                DataSource = Path.Combine(basePath, excelCon.SourceFile.FilePath),
                                PersistSecurityInfo = false,
                                ["Mode"] = "Read"
                            };
                            var extension = Path.GetExtension(excelCon.SourceFile.FileName)?.ToUpper();
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
                        }
                    }
                    break;
            }
            return cs;
        }

        public List<DataProvider> GetDataProviders()
        {
            var reader = OleDbEnumerator.GetRootEnumerator();
            var res = new List<string>();
            while (reader.Read())
            {
                res.Add(reader["SOURCES_NAME"].ToString());
            }
            return (from DataRow row in DbProviderFactories.GetFactoryClasses().Rows
                    select new DataProvider()
                    {
                        Name = row["Name"].ToString(),
                        AssemblyQualifiedName = row["AssemblyQualifiedName"].ToString(),
                        Description = row["Description"].ToString(),
                        InvariantName = row["InvariantName"].ToString(),
                    }).ToList();
        }

        public bool IsDataProviderInstalled(string providerName)
        {
            return GetDataProviders().Any(x => x.InvariantName == providerName);
        }
    }
}
