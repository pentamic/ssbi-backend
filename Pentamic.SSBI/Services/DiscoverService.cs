using System;
using System.Data;
using System.Data.OleDb;
using System.Threading.Tasks;
using System.Collections.Generic;
using Pentamic.SSBI.Models.DataModel;
using Pentamic.SSBI.Models.Discover;
using System.IO;

namespace Pentamic.SSBI.Services
{
    public class DiscoverService
    {
        private DataModelContext _dataModelContext;

        public DiscoverService()
        {
            _dataModelContext = new DataModelContext();
        }

        public DataTable Discover(string conStr, Guid collection, object[] restrictionValues)
        {
            using (var con = new OleDbConnection(conStr))
            {
                con.Open();
                return con.GetOleDbSchemaTable(collection, restrictionValues);
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
