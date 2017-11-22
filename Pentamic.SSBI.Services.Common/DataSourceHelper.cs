using System;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Services.Common
{
    public class DataSourceHelper
    {
        private readonly string _serverBasePath;

        public DataSourceHelper(string serverBasePath)
        {
            _serverBasePath = serverBasePath;
        }

        public string GetDataSourceConnectionString(DataSource dataSource)
        {
            string cs;
            switch (dataSource.Type)
            {
                case DataSourceType.SqlServer:
                    if (dataSource.IntegratedSecurity)
                    {
                        cs = $"Provider=SQLNCLI11;Data Source={dataSource.Source};Initial Catalog={dataSource.Catalog};Integrated Security=SSPI;Persist Security Info=false";
                    }
                    else
                    {
                        cs = $"Provider=SQLNCLI11;Data Source={dataSource.Source};Initial Catalog={dataSource.Catalog};User ID={dataSource.User};Password={dataSource.Password};Persist Security Info=true";
                    }
                    break;
                case DataSourceType.Excel:
                    if (dataSource.SourceFile == null)
                    {
                        throw new ArgumentException("Source file not found");
                    }
                    var builder = new OleDbConnectionStringBuilder()
                    {
                        Provider = "Microsoft.ACE.OLEDB.12.0",
                        DataSource = Path.Combine(_serverBasePath, dataSource.SourceFile.FilePath),
                        PersistSecurityInfo = false
                    };
                    builder["Mode"] = "Read";
                    var extension = Path.GetExtension(dataSource.SourceFile.FileName)?.ToUpper();
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
