﻿using System;
using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Data.OleDb;
using System.Dynamic;
using System.Data.Common;
using Pentamic.SSBI.Data;
using Pentamic.SSBI.Entities;
using Pentamic.SSBI.Services.Common;
using Pentamic.SSBI.Services.SSAS.Metadata;
using AS = Microsoft.AnalysisServices.Tabular;

namespace Pentamic.SSBI.Services
{
    public class DiscoverService
    {
        private readonly DataSourceHelper _dataSourceHelper;

        public DiscoverService(DataSourceHelper dataSourceHelper)
        {
            _dataSourceHelper = dataSourceHelper;
        }

        public async Task<List<CatalogDiscoverResult>> DiscoverCatalogs(DataSource dataSourceInfo)
        {
            var conStr = _dataSourceHelper.GetDataSourceConnectionString(dataSourceInfo);
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

        public async Task<List<TableDiscoverResult>> DiscoverTables(DataSource dataSourceInfo)
        {
            var conStr = _dataSourceHelper.GetDataSourceConnectionString(dataSourceInfo);
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

        public async Task<List<ColumnDiscoverResult>> DiscoverColumns(DataSource dataSourceInfo, string tableSchema, string tableName)
        {
            var conStr = _dataSourceHelper.GetDataSourceConnectionString(dataSourceInfo);
            if (dataSourceInfo.Type == DataSourceType.Excel)
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
                        DataType = ((OleDbType)Convert.ToInt32(row["DATA_TYPE"])).ToDataType().ToString(),
                        TableName = row["TABLE_NAME"].ToString(),
                        TableSchema = row["TABLE_SCHEMA"].ToString()
                    });
                }
                return result;
            }
        }

        public async Task<List<RelationshipDiscoverResult>> DiscoverRelationships(DataSource dataSourceInfo, string fkTableSchema, string fkTableName)
        {
            var conStr = _dataSourceHelper.GetDataSourceConnectionString(dataSourceInfo);
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

        public async Task<TableDetailResult> DiscoverTable(DataSource dataSourceInfo, string tableSchema, string tableName, string query)
        {
            var conStr = _dataSourceHelper.GetDataSourceConnectionString(dataSourceInfo);
            if (dataSourceInfo.Type == DataSourceType.Excel)
            {
                tableSchema = null;
            }
            using (var con = new OleDbConnection(conStr))
            {
                using (var cmd = con.CreateCommand())
                {
                    if (query == null)
                    {
                        cmd.CommandText = string.IsNullOrEmpty(tableSchema) ?
                            $"SELECT TOP 50 * FROM [{tableName}]" :
                            $"SELECT TOP 50 * FROM [{tableSchema}].[{tableName}]";
                    }
                    else
                    {
                        cmd.CommandText = $"SELECT TOP 50 * FROM ( {query} ) tmp{DateTime.Now:yyyyMMddhhmmss}";
                    }
                    await con.OpenAsync();
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

                        foreach (DataRow sr in schema.Rows)
                        {
                            columns.Add(new ColumnDiscoverResult
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

        public ColumnDataType GetDataType(string typeName)
        {
            switch (typeName)
            {
                case "Int16":
                case "Int32":
                case "Int64":
                case "Byte":
                    return ColumnDataType.Int64;
                case "String":
                    return ColumnDataType.String;
                case "DateTime":
                    return ColumnDataType.DateTime;
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

        public DataTable GetProviderFactoryClasses()
        {
            var table = DbProviderFactories.GetFactoryClasses();
            return table;
        }

    }
}
