﻿using System;
using System.Data;
using System.Data.OleDb;
using System.Threading.Tasks;
using System.Collections.Generic;
using Pentamic.SSBI.Models.DataModel;
using Pentamic.SSBI.Models.Discover;
using System.IO;
using AS = Microsoft.AnalysisServices.Tabular;
using Newtonsoft.Json.Linq;

namespace Pentamic.SSBI.Services
{
    public class DiscoverService
    {
        private DataModelContext _dataModelContext;

        public DiscoverService()
        {
            _dataModelContext = new DataModelContext();
        }

        public JArray DiscoverModel(ModelDiscoverRestriction restrictions)
        {
            var model = _dataModelContext.Models.Find(restrictions.ModelId);
            using (var server = new AS.Server())
            {
                server.Connect(@".\astab16");
                var db = server.Databases.FindByName(model.DatabaseName);
                if (string.IsNullOrEmpty(restrictions.Perspective))
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
                    var per = db.Model.Perspectives.Find(restrictions.Perspective);
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

        public DataTable Discover(string conStr, Guid collection, object[] restrictionValues)
        {
            using (var con = new OleDbConnection(conStr))
            {
                con.Open();
                return con.GetOleDbSchemaTable(collection, restrictionValues);
            }
        }

        public async Task<List<CatalogDiscoverResult>> DiscoverCatalogs(CatalogSchemaRestriction restrictions)
        {
            var ds = _dataModelContext.DataSources.Find(restrictions.DataSourceId);
            var conStr = GetDataSourceConnectionString(ds);
            using (var con = new OleDbConnection(conStr))
            {
                var result = new List<CatalogDiscoverResult>();
                await con.OpenAsync();
                var dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Catalogs, restrictions.Restrictions);
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

        public async Task<List<TableDiscoverResult>> DiscoverTables(TableSchemaRestriction restrictions)
        {
            var ds = _dataModelContext.DataSources.Find(restrictions.DataSourceId);
            var conStr = GetDataSourceConnectionString(ds);
            using (var con = new OleDbConnection(conStr))
            {
                var result = new List<TableDiscoverResult>();
                await con.OpenAsync();
                restrictions.TableType = "TABLE";
                var dt1 = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, restrictions.Restrictions);
                restrictions.TableType = "VIEW";
                var dt2 = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, restrictions.Restrictions);
                foreach (DataRow row in dt1.Rows)
                {
                    result.Add(new TableDiscoverResult
                    {
                        Name = row["TABLE_NAME"].ToString(),
                        Schema = row["TABLE_SCHEMA"].ToString(),
                        Type = row["TABLE_TYPE"].ToString()
                    });
                }
                foreach (DataRow row in dt2.Rows)
                {
                    result.Add(new TableDiscoverResult
                    {
                        Name = row["TABLE_NAME"].ToString(),
                        Schema = row["TABLE_SCHEMA"].ToString(),
                        Type = row["TABLE_TYPE"].ToString()
                    });
                }
                return result;
            }
        }

        public async Task<JArray> DiscoverTables(DataSource ds)
        {
            var conStr = GetDataSourceConnectionString(ds);
            using (var con = new OleDbConnection(conStr))
            {
                var result = new JArray();
                await con.OpenAsync();
                var dt1 = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new[] { null, null, null, "TABLE" });
                var dt2 = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new[] { null, null, null, "VIEW" });
                foreach (DataRow row in dt1.Rows)
                {
                    var obj = new JObject();
                    obj["TableName"] = row["TABLE_NAME"].ToString();
                    obj["TableSchema"] = row["TABLE_SCHEMA"].ToString();
                    obj["TableType"] = row["TABLE_TYPE"].ToString();
                    result.Add(obj);
                }
                foreach (DataRow row in dt2.Rows)
                {
                    var obj = new JObject();
                    obj["TableName"] = row["TABLE_NAME"].ToString();
                    obj["TableSchema"] = row["TABLE_SCHEMA"].ToString();
                    obj["TableType"] = row["TABLE_TYPE"].ToString();
                    result.Add(obj);
                }
                return result;
            }
        }

        public async Task<List<ColumnDiscoverResult>> DiscoverColumns(ColumnSchemaRestriction restrictions)
        {
            var ds = _dataModelContext.DataSources.Find(restrictions.DataSourceId);
            var conStr = GetDataSourceConnectionString(ds);
            using (var con = new OleDbConnection(conStr))
            {
                var result = new List<ColumnDiscoverResult>();
                await con.OpenAsync();
                var dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, restrictions.Restrictions);
                foreach (DataRow row in dt.Rows)
                {
                    result.Add(new ColumnDiscoverResult
                    {
                        Name = row["COLUMN_NAME"].ToString(),
                        DataType = ((OleDbType)Convert.ToInt32(row["DATA_TYPE"])).ToDataType(),
                        TableName = row["TABLE_NAME"].ToString(),
                        TableSchema = row["TABLE_SCHEMA"].ToString()
                    });
                }
                return result;
            }
        }

        public async Task<DataTable> DiscoverRelationships(ForeignKeySchemaRestriction restrictions)
        {
            var ds = _dataModelContext.DataSources.Find(restrictions.DataSourceId);
            var conStr = GetDataSourceConnectionString(ds);
            using (var con = new OleDbConnection(conStr))
            {
                await con.OpenAsync();
                return con.GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, restrictions.Restrictions);
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

    }
}
