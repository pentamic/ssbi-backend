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
using AS = Microsoft.AnalysisServices.Tabular;


namespace Pentamic.SSBI.Services
{
    public class DiscoverService
    {
        private DataModelContext _dataModelContext;
        private string _asConnectionString = System.Configuration.ConfigurationManager
                .ConnectionStrings["AnalysisServiceConnection"]
                .ConnectionString;

        public DiscoverService()
        {
            _dataModelContext = new DataModelContext();
        }

        public JArray DiscoverModel(int modelId, string perspective)
        {
            var model = _dataModelContext.Models.Find(modelId);
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
                        var table = new JObject();
                        table["name"] = tb.Name;
                        var fields = new JArray();
                        foreach (var co in tb.Columns)
                        {
                            if (co.IsHidden) continue;
                            var field = new JObject();
                            field["tableName"] = tb.Name;
                            field["name"] = co.Name;
                            field["dataType"] = (int)co.DataType;
                            field["displayFolder"] = co.DisplayFolder;
                            fields.Add(field);
                        }
                        foreach (var me in tb.Measures)
                        {
                            if (me.IsHidden) continue;
                            var field = new JObject();
                            field["tableName"] = tb.Name;
                            field["name"] = me.Name;
                            field["dataType"] = (int)me.DataType;
                            field["displayFolder"] = me.DisplayFolder;
                            field["isMeasure"] = true;
                            fields.Add(field);
                        }
                        foreach (var hi in tb.Hierarchies)
                        {
                            if (hi.IsHidden) continue;
                            var field = new JObject();
                            field["tableName"] = tb.Name;
                            field["name"] = hi.Name;
                            field["displayFolder"] = hi.DisplayFolder;
                            field["isHierarchy"] = true;
                            var levels = new JArray();
                            foreach (var le in hi.Levels)
                            {
                                var level = new JObject();
                                level["name"] = le.Name;
                                level["ordinal"] = le.Ordinal;
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
                        var table = new JObject();
                        table["name"] = tb.Name;
                        var fields = new JArray();
                        foreach (var co in tb.PerspectiveColumns)
                        {
                            if (co.Column.IsHidden) continue;
                            var field = new JObject();
                            field["tableName"] = tb.Name;
                            field["name"] = co.Name;
                            field["dataType"] = (int)co.Column.DataType;
                            field["displayFolder"] = co.Column.DisplayFolder;
                            fields.Add(field);
                        }
                        foreach (var me in tb.PerspectiveMeasures)
                        {
                            if (me.Measure.IsHidden) continue;
                            var field = new JObject();
                            field["tableName"] = tb.Name;
                            field["name"] = me.Name;
                            field["dataType"] = (int)me.Measure.DataType;
                            field["displayFolder"] = me.Measure.DisplayFolder;
                            field["isMeasure"] = true;
                            fields.Add(field);
                        }
                        foreach (var hi in tb.PerspectiveHierarchies)
                        {
                            if (hi.Hierarchy.IsHidden) continue;
                            var field = new JObject();
                            field["tableName"] = tb.Name;
                            field["name"] = hi.Name;
                            field["displayFolder"] = hi.Hierarchy.DisplayFolder;
                            field["isHierarchy"] = true;
                            var levels = new JArray();
                            foreach (var le in hi.Hierarchy.Levels)
                            {
                                var level = new JObject();
                                level["name"] = le.Name;
                                level["ordinal"] = le.Ordinal;
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
            var conStr = GetDataSourceConnectionString(ds);
            using (var con = new OleDbConnection(conStr))
            {
                var result = new List<CatalogDiscoverResult>();
                await con.OpenAsync();
                var dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Catalogs, null);
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
            // var conStr = GetDataSourceConnectionString(ds);
            var conStr = $"Provider=SQLOLEDB;Data Source={ds.Source};Integrated Security=SSPI;Persist Security Info=false";
            using (var con = new OleDbConnection(conStr))
            {
                var result = new List<CatalogDiscoverResult>();
                await con.OpenAsync();
                var dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Catalogs, null);
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
            var conStr = GetDataSourceConnectionString(ds);
            using (var con = new OleDbConnection(conStr))
            {
                var result = new List<TableDiscoverResult>();
                await con.OpenAsync();
                var dt1 = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new[] { null, null, null, "TABLE" });
                var dt2 = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new[] { null, null, null, "VIEW" });
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
                foreach (DataRow row in dt2.Rows)
                {
                    var obj = new TableDiscoverResult
                    {
                        TableName = row["TABLE_NAME"].ToString(),
                        TableSchema = row["TABLE_SCHEMA"].ToString(),
                        TableType = row["TABLE_TYPE"].ToString()
                    };
                    result.Add(obj);
                }
                return result;
            }
        }

        public async Task<List<TableDiscoverResult>> DiscoverTables(DataSource ds)
        {
            var providers = GetProviderFactoryClasses();
            var conStr = GetDataSourceConnectionString(ds);
            using (var con = new OleDbConnection(conStr))
            {
                var result = new List<TableDiscoverResult>();
                await con.OpenAsync();
                var dt1 = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new[] { null, null, null, "TABLE" });
                var dt2 = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new[] { null, null, null, "VIEW" });
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
                foreach (DataRow row in dt2.Rows)
                {
                    var obj = new TableDiscoverResult
                    {
                        TableName = row["TABLE_NAME"].ToString(),
                        TableSchema = row["TABLE_SCHEMA"].ToString(),
                        TableType = row["TABLE_TYPE"].ToString()
                    };
                    result.Add(obj);
                }
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
            var conStr = GetDataSourceConnectionString(ds);
            if (ds.Type == DataSourceType.Excel)
            {
                tableSchema = null;
            }
            using (var con = new OleDbConnection(conStr))
            {
                var result = new List<ColumnDiscoverResult>();
                await con.OpenAsync();
                var dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new[] { null, tableSchema, tableName, null });
                foreach (DataRow row in dt.Rows)
                {
                    result.Add(new ColumnDiscoverResult
                    {
                        ColumnName = row["COLUMN_NAME"].ToString(),
                        DataType = ((OleDbType)Convert.ToInt32(row["DATA_TYPE"])).ToDataType(),
                        TableName = row["TABLE_NAME"].ToString(),
                        TableSchema = row["TABLE_SCHEMA"].ToString()
                    });
                }
                return result;
            }
        }

        public async Task<List<ColumnDiscoverResult>> DiscoverColumns(DataSource ds, string tableSchema, string tableName)
        {
            var conStr = GetDataSourceConnectionString(ds);
            if (ds.Type == DataSourceType.Excel)
            {
                tableSchema = null;
            }
            using (var con = new OleDbConnection(conStr))
            {
                var result = new List<ColumnDiscoverResult>();
                await con.OpenAsync();
                var dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new[] { null, tableSchema, tableName, null });
                foreach (DataRow row in dt.Rows)
                {
                    result.Add(new ColumnDiscoverResult
                    {
                        ColumnName = row["COLUMN_NAME"].ToString(),
                        DataType = ((OleDbType)Convert.ToInt32(row["DATA_TYPE"])).ToDataType(),
                        TableName = row["TABLE_NAME"].ToString(),
                        TableSchema = row["TABLE_SCHEMA"].ToString()
                    });
                }
                return result;
            }
        }

        public async Task<List<RelationshipDiscoverResult>> DiscoverRelationships(DataSource ds, string fkTableSchema, string fkTableName)
        {
            var conStr = GetDataSourceConnectionString(ds);
            using (var con = new OleDbConnection(conStr))
            {
                var result = new List<RelationshipDiscoverResult>();
                await con.OpenAsync();
                var dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, new[] { null, null, null, null, fkTableSchema, fkTableName });
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

        public async Task<List<dynamic>> DiscoverTableData(DataSource ds, string tableSchema, string tableName)
        {
            var conStr = GetDataSourceConnectionString(ds);
            if (ds.Type == DataSourceType.Excel)
            {
                tableSchema = null;
            }
            OleDbConnection con = null;
            DbDataReader reader = null;
            try
            {
                using (con = new OleDbConnection(conStr))
                {
                    using (var cmd = con.CreateCommand())
                    {
                        cmd.CommandText = string.IsNullOrEmpty(tableSchema) ? $"SELECT TOP 100 * FROM [{tableName}]"
                        : $"SELECT TOP 100 * FROM [{tableSchema}].[{tableName}]";
                        await con.OpenAsync();
                        var result = new List<dynamic>();
                        reader = await cmd.ExecuteReaderAsync();
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
            finally
            {
                if (con != null)
                {
                    con.Close();
                }
                if (reader != null)
                {
                    reader.Close();
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
                        cs = $"Provider=SQLOLEDB;Data Source={ds.Source};Initial Catalog={ds.Catalog};Integrated Security=SSPI;Persist Security Info=false";
                    }
                    else
                    {
                        cs = $"Provider=SQLOLEDB;Data Source={ds.Source};Initial Catalog={ds.Catalog};User ID={ds.User};Password={ds.Password};Persist Security Info=true";
                    }
                    break;
                case DataSourceType.Excel:
                    if (ds.SourceFileId != null && ds.SourceFile == null)
                    {
                        ds.SourceFile = _dataModelContext.SourceFiles.Find(ds.SourceFileId);
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

        public DataTable GetProviderFactoryClasses()
        {
            var table = DbProviderFactories.GetFactoryClasses();
            return table;
        }

    }
}
