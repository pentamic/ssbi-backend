using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using AS = Microsoft.AnalysisServices.Tabular;
using AN = Microsoft.AnalysisServices;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Services.SSAS.Metadata
{
    public class MetadataService
    {
        private readonly string _asConnectionString = "";

        //public JArray GetModelMetadata(int modelId)
        //{
        //    using (var server = new AS.Server())
        //    {
        //        server.Connect(_asConnectionString);
        //        var db = server.Databases.FindByName(modelId.ToString());
        //        if (db == null)
        //        {
        //            throw new Exception("Database not found");
        //        }
        //        if (string.IsNullOrEmpty(perspective))
        //        {
        //            var tables = new JArray();
        //            foreach (var tb in db.Model.Tables)
        //            {
        //                if (tb.IsHidden) continue;
        //                var table = new JObject
        //                {
        //                    ["name"] = tb.Name
        //                };
        //                var fields = new JArray();
        //                foreach (var co in tb.Columns)
        //                {
        //                    if (co.IsHidden) continue;
        //                    var field = new JObject
        //                    {
        //                        ["tableName"] = tb.Name,
        //                        ["name"] = co.Name,
        //                        ["dataType"] = (int)co.DataType,
        //                        ["displayFolder"] = co.DisplayFolder
        //                    };
        //                    fields.Add(field);
        //                }
        //                foreach (var me in tb.Measures)
        //                {
        //                    if (me.IsHidden) continue;
        //                    var field = new JObject
        //                    {
        //                        ["tableName"] = tb.Name,
        //                        ["name"] = me.Name,
        //                        ["dataType"] = (int)me.DataType,
        //                        ["displayFolder"] = me.DisplayFolder,
        //                        ["isMeasure"] = true
        //                    };
        //                    fields.Add(field);
        //                }
        //                foreach (var hi in tb.Hierarchies)
        //                {
        //                    if (hi.IsHidden) continue;
        //                    var field = new JObject
        //                    {
        //                        ["tableName"] = tb.Name,
        //                        ["name"] = hi.Name,
        //                        ["displayFolder"] = hi.DisplayFolder,
        //                        ["isHierarchy"] = true
        //                    };
        //                    var levels = new JArray();
        //                    foreach (var le in hi.Levels)
        //                    {
        //                        var level = new JObject
        //                        {
        //                            ["name"] = le.Name,
        //                            ["ordinal"] = le.Ordinal
        //                        };
        //                        levels.Add(level);
        //                    }
        //                    field["levels"] = levels;
        //                    fields.Add(field);
        //                }
        //                table["fields"] = fields;
        //                tables.Add(table);
        //            }
        //            return tables;
        //        }
        //        else
        //        {
        //            var per = db.Model.Perspectives.Find(perspective);
        //            var tables = new JArray();
        //            foreach (var tb in per.PerspectiveTables)
        //            {
        //                if (tb.Table.IsHidden) continue;
        //                var table = new JObject
        //                {
        //                    ["name"] = tb.Name
        //                };
        //                var fields = new JArray();
        //                foreach (var co in tb.PerspectiveColumns)
        //                {
        //                    if (co.Column.IsHidden) continue;
        //                    var field = new JObject
        //                    {
        //                        ["tableName"] = tb.Name,
        //                        ["name"] = co.Name,
        //                        ["dataType"] = (int)co.Column.DataType,
        //                        ["displayFolder"] = co.Column.DisplayFolder
        //                    };
        //                    fields.Add(field);
        //                }
        //                foreach (var me in tb.PerspectiveMeasures)
        //                {
        //                    if (me.Measure.IsHidden) continue;
        //                    var field = new JObject
        //                    {
        //                        ["tableName"] = tb.Name,
        //                        ["name"] = me.Name,
        //                        ["dataType"] = (int)me.Measure.DataType,
        //                        ["displayFolder"] = me.Measure.DisplayFolder,
        //                        ["isMeasure"] = true
        //                    };
        //                    fields.Add(field);
        //                }
        //                foreach (var hi in tb.PerspectiveHierarchies)
        //                {
        //                    if (hi.Hierarchy.IsHidden) continue;
        //                    var field = new JObject
        //                    {
        //                        ["tableName"] = tb.Name,
        //                        ["name"] = hi.Name,
        //                        ["displayFolder"] = hi.Hierarchy.DisplayFolder,
        //                        ["isHierarchy"] = true
        //                    };
        //                    var levels = new JArray();
        //                    foreach (var le in hi.Hierarchy.Levels)
        //                    {
        //                        var level = new JObject
        //                        {
        //                            ["name"] = le.Name,
        //                            ["ordinal"] = le.Ordinal
        //                        };
        //                        levels.Add(level);
        //                    }
        //                    field["levels"] = levels;
        //                    fields.Add(field);
        //                }
        //                table["fields"] = fields;
        //                tables.Add(table);
        //            }
        //            return tables;
        //        }
        //    }
        //}

        public ModelResult GetModelMetadata(int modelId)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(modelId.ToString());
                if (database == null)
                {
                    throw new Exception("Database not found");
                }
                var result = new ModelResult
                {
                    Name = database.Model.Name,
                    Tables = database.Model.Tables.Where(x => !x.IsHidden).Select(table => new TableResult
                    {
                        Name = table.Name,
                        Columns = table.Columns.Select(column => new ColumnResult
                        {
                            Name = column.Name,
                            DataType = (int)column.DataType
                        }).ToList(),
                        Measures = table.Measures.Select(measure => new MeasureResult
                        {
                            Name = measure.Name
                        }).ToList()
                    }).ToList()
                };
                return result;
            }
        }

        public void RefreshDatabase(string databaseName)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(databaseName);
                if (database == null)
                {
                    throw new Exception("Database not found");
                }
                database.Model.RequestRefresh(AS.RefreshType.Full);
                database.Update(AN.UpdateOptions.ExpandFull);
            }
        }

        public void RefreshTable(string databaseName, string tableName)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(databaseName);
                if (database == null)
                {
                    throw new Exception("Database not found");
                }
                var table = database.Model.Tables.Find(tableName);
                if (table == null)
                {
                    throw new Exception("Table not found");
                }
                table.RequestRefresh(AS.RefreshType.Full);
                database.Update(Microsoft.AnalysisServices.UpdateOptions.ExpandFull);
            }
        }

        public void RefreshPartition(string databaseName, string tableName, string partitionName)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(databaseName);
                if (database == null)
                {
                    throw new Exception("Database not found");
                }
                var table = database.Model.Tables.Find(tableName);
                if (table == null)
                {
                    throw new Exception("Table not found");
                }
                var partition = table.Partitions.Find(partitionName);
                partition.RequestRefresh(AS.RefreshType.Full);
                database.Update(Microsoft.AnalysisServices.UpdateOptions.ExpandFull);
            }
        }

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

        //public void CloneModel(Model mo, int modelId)
        //{
        //    var fromModel = Context.Models.Find(modelId);
        //    if (fromModel == null)
        //    {
        //        throw new Exception("Clone model not found");
        //    }
        //    using (var server = new AS.Server())
        //    {
        //        server.Connect(_asConnectionString);
        //        AS.Database database = server.Databases.Find(fromModel.Id.ToString());
        //        if (database == null)
        //        {
        //            throw new Exception("Database not found");
        //        }
        //        if (database.CompatibilityLevel < 1200)
        //        {
        //            throw new Exception("Database not supported");
        //        }
        //        var newDb = database.Clone();
        //        newDb.ID = mo.Id.ToString();
        //        newDb.Name = mo.Id.ToString();
        //        newDb.Model = database.Model.Clone();
        //        newDb.Model.Name = mo.Name;
        //        newDb.Model.Description = mo.Description;
        //        server.Databases.Add(newDb);
        //        mo.DataSources = new List<DataSource>();
        //        mo.Tables = new List<Table>();
        //        mo.Relationships = new List<Relationship>();
        //        mo.DefaultMode = (ModeType)database.Model.DefaultMode;
        //        mo.Description = database.Model.Description;

        //        //Data Sources
        //        var dataSources = database.Model.DataSources;
        //        foreach (AS.ProviderDataSource ds in dataSources)
        //        {
        //            ds.ImpersonationMode = AS.ImpersonationMode.ImpersonateServiceAccount;
        //            var conStrBuilder = new OleDbConnectionStringBuilder(ds.ConnectionString);
        //            var dataSource = new DataSource
        //            {
        //                Name = ds.Name,
        //                Description = ds.Description,
        //                ConnectionString = ds.ConnectionString,
        //                Source = conStrBuilder.DataSource
        //            };
        //            conStrBuilder.TryGetValue("Integrated Security", out object val);
        //            if (val != null)
        //            {
        //                if (val is bool)
        //                {
        //                    dataSource.IntegratedSecurity = (bool)val;
        //                }
        //                if (val is string)
        //                {
        //                    if ((string)val == "SSPI" || (string)val == "true")
        //                    {
        //                        dataSource.IntegratedSecurity = true;
        //                    }
        //                }
        //            }
        //            conStrBuilder.TryGetValue("Initial Catalog", out val);
        //            if (val != null)
        //            {
        //                dataSource.Catalog = (string)val;
        //            }
        //            conStrBuilder.TryGetValue("User ID", out val);
        //            if (val != null)
        //            {
        //                dataSource.User = (string)val;
        //            }
        //            conStrBuilder.TryGetValue("Password", out val);
        //            if (val != null)
        //            {
        //                dataSource.Password = (string)val;
        //            }
        //            mo.DataSources.Add(dataSource);
        //        }

        //        //Tables
        //        var tables = database.Model.Tables;
        //        foreach (AS.Table tb in tables)
        //        {
        //            var table = new Table
        //            {
        //                Name = tb.Name,
        //                Description = tb.Description,
        //                Partitions = new List<Partition>(),
        //                Measures = new List<Measure>(),
        //                Columns = new List<Column>()
        //            };
        //            mo.Tables.Add(table);

        //            foreach (AS.Partition pa in tb.Partitions)
        //            {
        //                if (pa.Source is AS.CalculatedPartitionSource)
        //                {
        //                    var source = pa.Source as AS.CalculatedPartitionSource;
        //                    table.Partitions.Add(new Partition
        //                    {
        //                        Name = pa.Name,
        //                        Description = pa.Description,
        //                        Query = source.Expression,
        //                        SourceType = PartitionSourceType.Calculated
        //                    });
        //                }
        //                else if (pa.Source is AS.QueryPartitionSource)
        //                {
        //                    var source = pa.Source as AS.QueryPartitionSource;
        //                    table.Partitions.Add(new Partition
        //                    {
        //                        Name = pa.Name,
        //                        Description = pa.Description,
        //                        Query = source.Query,
        //                        SourceType = PartitionSourceType.Query
        //                    });
        //                }
        //            }

        //            foreach (AS.Column co in tb.Columns)
        //            {

        //                if (co.Type == AS.ColumnType.RowNumber)
        //                {
        //                    continue;
        //                }
        //                table.Columns.Add(new Column
        //                {
        //                    Name = co.Name,
        //                    DataType = (ColumnDataType)co.DataType,
        //                    Description = co.Description,
        //                    IsHidden = co.IsHidden,
        //                    DisplayFolder = co.DisplayFolder,
        //                    FormatString = co.FormatString
        //                });
        //            }

        //            foreach (AS.Measure me in tb.Measures)
        //            {
        //                table.Measures.Add(new Measure
        //                {
        //                    Name = me.Name,
        //                    Description = me.Description,
        //                    Expression = me.Expression
        //                });
        //            }
        //        }

        //        //Relationships
        //        var relationships = database.Model.Relationships;
        //        foreach (AS.SingleColumnRelationship re in relationships)
        //        {
        //            var relationship = new Relationship
        //            {
        //                Name = re.Name
        //            };
        //            foreach (var table in mo.Tables)
        //            {
        //                if (re.FromTable.Name == table.Name)
        //                {
        //                    foreach (var col in table.Columns)
        //                    {
        //                        if (re.FromColumn.Name == col.Name)
        //                        {
        //                            relationship.FromColumn = col;
        //                        }
        //                    }
        //                }
        //                if (re.ToTable.Name == table.Name)
        //                {
        //                    foreach (var col in table.Columns)
        //                    {
        //                        if (re.ToColumn.Name == col.Name)
        //                        {
        //                            relationship.ToColumn = col;
        //                        }
        //                    }
        //                }
        //            }
        //            mo.Relationships.Add(relationship);
        //        }
        //        newDb.Update(AN.UpdateOptions.ExpandFull);
        //        Context.SaveChanges();
        //    }
        //}

        //public string GetDataSourceConnectionString(DataSource dataSourceInfo, string serverBasePath)
        //{
        //    string cs;
        //    switch (dataSourceInfo.Type)
        //    {
        //        case DataSourceType.SqlServer:
        //            cs = dataSourceInfo.IntegratedSecurity ? $"Provider=SQLNCLI11;Data Source={dataSourceInfo.Source};Initial Catalog={dataSourceInfo.Catalog};Integrated Security=SSPI;Persist Security Info=false" :
        //                $"Provider=SQLNCLI11;Data Source={dataSourceInfo.Source};Initial Catalog={dataSourceInfo.Catalog};User ID={dataSourceInfo.User};Password={dataSourceInfo.Password};Persist Security Info=true";
        //            break;
        //        case DataSourceType.Excel:
        //            var builder = new OleDbConnectionStringBuilder()
        //            {
        //                Provider = "Microsoft.ACE.OLEDB.12.0",
        //                DataSource = Path.Combine(serverBasePath, dataSourceInfo.SourceFile.FilePath),
        //                PersistSecurityInfo = false
        //            };
        //            builder["Mode"] = "Read";
        //            var extension = Path.GetExtension(dataSourceInfo.SourceFile.FileName)?.ToUpper();
        //            switch (extension)
        //            {
        //                case ".XLS":
        //                    builder["Extended Properties"] = "Excel 8.0;HDR=Yes";
        //                    break;
        //                case ".XLSB":
        //                    builder["Extended Properties"] = "Excel 12.0;HDR=Yes";
        //                    break;
        //                case ".XLSX":
        //                    builder["Extended Properties"] = "Excel 12.0 Xml;HDR=Yes";
        //                    break;
        //                case ".XLSM":
        //                    builder["Extended Properties"] = "Excel 12.0 Macro;HDR=Yes";
        //                    break;
        //                default:
        //                    builder["Extended Properties"] = "Excel 12.0;HDR=Yes";
        //                    break;
        //            }
        //            cs = builder.ToString();
        //            break;
        //        default: return null;
        //    }
        //    return cs;
        //}

        public void RenameTable(string databaseName, string oldTableName, string newTableName)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(databaseName);
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                var table = database.Model.Tables.Find(oldTableName);
                if (table == null)
                {
                    throw new ArgumentException("Table not found");
                }
                table.RequestRename(newTableName);
                database.Update(AN.UpdateOptions.ExpandFull);
            }
        }

        public void RenameColumn(string databaseName, string tableName, string oldColumnName, string newColumnName)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(databaseName);
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                var table = database.Model.Tables.Find(tableName);
                if (table == null)
                {
                    throw new ArgumentException("Table not found");
                }
                var column = table.Columns.Find(oldColumnName);
                if (column == null)
                {
                    throw new ArgumentException("Column not found");
                }
                column.RequestRename(column.Name);
                database.Update(AN.UpdateOptions.ExpandFull);
            }
        }

        public void RenameMeasure(string databaseName, string tableName, string oldMeasureName, string newMeasureName)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(databaseName);
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                var table = database.Model.Tables.Find(tableName);
                if (table == null)
                {
                    throw new ArgumentException("Table not found");
                }
                var measure = table.Measures.Find(oldMeasureName);
                if (measure == null)
                {
                    throw new ArgumentException("Measure not found");
                }
                measure.RequestRename(newMeasureName);
                database.Update(AN.UpdateOptions.ExpandFull);
            }
        }


        //Model-----------------------------------------------------------

        public void CreateModel(Model modelInfo)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = new AS.Database
                {
                    Name = modelInfo.Id.ToString(),
                    ID = modelInfo.Id.ToString(),
                    CompatibilityLevel = 1200,
                    StorageEngineUsed = AN.StorageEngineUsed.TabularMetadata,
                    Model = new AS.Model
                    {
                        Name = modelInfo.Id.ToString(),
                        Description = modelInfo.Description,
                        DefaultMode = modelInfo.DefaultMode.ToModeType()
                    },
                };
                server.Databases.Add(database);
                database.Update(AN.UpdateOptions.ExpandFull);
            }
        }

        public void UpdateModel(Model modelInfo)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(modelInfo.Id.ToString());
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                database.Model.Description = modelInfo.Description;
                database.Model.DefaultMode = modelInfo.DefaultMode.ToModeType();
                database.Update(AN.UpdateOptions.ExpandFull);
            }
        }

        public void DeleteModel(Model modelInfo)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(modelInfo.Id.ToString());
                database?.Drop();
            }
        }

        //Data Source---------------------------------------------------

        public void CreateDataSource(DataSource dataSourceInfo, string serverBasePath)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(dataSourceInfo.ModelId.ToString());
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                database.Model.DataSources.Add(new AS.ProviderDataSource
                {
                    Name = dataSourceInfo.Id.ToString(),
                    Description = dataSourceInfo.Description,
                    ConnectionString = dataSourceInfo.ConnectionString,
                    ImpersonationMode = AS.ImpersonationMode.ImpersonateServiceAccount
                });
                database.Update(AN.UpdateOptions.ExpandFull);
            }
        }

        public void AddDataSource(AS.Database database, DataSource dataSourceInfo)
        {
            database.Model.DataSources.Add(new AS.ProviderDataSource
            {
                Name = dataSourceInfo.Id.ToString(),
                Description = dataSourceInfo.Description,
                ConnectionString = dataSourceInfo.ConnectionString,
                ImpersonationMode = AS.ImpersonationMode.ImpersonateServiceAccount
            });
        }

        public void UpdateDataSource(DataSource dataSourceInfo)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(dataSourceInfo.ModelId.ToString());
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                if (!(database.Model.DataSources.Find(dataSourceInfo.Id.ToString()) is AS.ProviderDataSource ds))
                {
                    throw new ArgumentException("Data Source not found");
                }
                ds.Description = dataSourceInfo.Description;
                ds.ImpersonationMode = AS.ImpersonationMode.ImpersonateServiceAccount;
                ds.ConnectionString = dataSourceInfo.ConnectionString;
                database.Update(AN.UpdateOptions.ExpandFull);
            }
        }

        public void DeleteDataSource(DataSource dataSource)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(dataSource.ModelId.ToString());
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                var ds = database.Model.DataSources.Find(dataSource.Id.ToString());
                if (ds != null)
                {
                    database.Model.DataSources.Remove(ds);
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
        }

        //Table--------------------------------------------------------

        public void CreateTable(string databaseName, Table tableInfo)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(databaseName);
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                var table = new AS.Table
                {
                    Name = tableInfo.Name,
                    Description = tableInfo.Description
                };
                if (tableInfo.Columns != null)
                {
                    foreach (var columnInfo in tableInfo.Columns)
                    {
                        switch (columnInfo.ColumnType)
                        {
                            case ColumnType.Data:
                                table.Columns.Add(new AS.DataColumn
                                {
                                    Name = columnInfo.Name,
                                    DataType = columnInfo.DataType.ToDataType(),
                                    SourceColumn = columnInfo.SourceColumn,
                                    IsHidden = columnInfo.IsHidden,
                                    DisplayFolder = columnInfo.DisplayFolder,
                                    FormatString = columnInfo.FormatString
                                });
                                break;
                            case ColumnType.Calculated:
                                table.Columns.Add(new AS.CalculatedColumn
                                {
                                    Name = columnInfo.Name,
                                    Expression = columnInfo.Expression,
                                    DataType = columnInfo.DataType.ToDataType(),
                                    IsHidden = columnInfo.IsHidden,
                                    DisplayFolder = columnInfo.DisplayFolder,
                                    FormatString = columnInfo.FormatString
                                });
                                break;
                            case ColumnType.RowNumber:
                                break;
                            case ColumnType.CalculatedTableColumn:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
                if (tableInfo.Partitions != null)
                {
                    foreach (var partitionInfo in tableInfo.Partitions)
                    {
                        switch (partitionInfo.SourceType)
                        {
                            case PartitionSourceType.Query:
                                var ds = database.Model.DataSources.Find(partitionInfo.DataSourceId.ToString());
                                if (ds == null)
                                {
                                    throw new Exception("Data Source not found");
                                }
                                table.Partitions.Add(new AS.Partition
                                {
                                    Name = partitionInfo.Id.ToString(),
                                    Source = new AS.QueryPartitionSource()
                                    {
                                        DataSource = ds,
                                        Query = partitionInfo.Query
                                    }
                                });
                                break;
                            case PartitionSourceType.Calculated:
                                table.Partitions.Add(new AS.Partition
                                {
                                    Name = partitionInfo.Id.ToString(),
                                    Source = new AS.CalculatedPartitionSource()
                                    {
                                        Expression = partitionInfo.Expression
                                    }
                                });
                                break;
                            case PartitionSourceType.None:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
                if (tableInfo.Measures != null)
                {
                    foreach (var mes in tableInfo.Measures)
                    {
                        table.Measures.Add(new AS.Measure
                        {
                            Name = mes.Name,
                            Expression = mes.Expression,
                            Description = mes.Description,
                            DisplayFolder = mes.DisplayFolder,
                            FormatString = mes.FormatString
                        });
                    }
                }
                database.Model.Tables.Add(table);
                database.Update(AN.UpdateOptions.ExpandFull);
            }

        }

        public void CreateTables(int modelId, List<Table> tables)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(modelId.ToString());
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
                            if (col.ColumnType == ColumnType.Data)
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
                            if (col.ColumnType == ColumnType.Calculated)
                            {
                                tb.Columns.Add(new AS.CalculatedColumn
                                {
                                    Name = col.Name,
                                    DataType = col.DataType.ToDataType(),
                                    Expression = col.Expression,
                                    IsHidden = col.IsHidden,
                                    DisplayFolder = col.DisplayFolder,
                                    FormatString = col.FormatString
                                });
                            }
                        }
                    }
                    if (table.Partitions != null)
                    {
                        foreach (var par in table.Partitions)
                        {
                            if (par.SourceType == PartitionSourceType.Query)
                            {
                                var ds = database.Model.DataSources.Find(par.DataSourceId.ToString());
                                if (ds == null)
                                {
                                    throw new Exception("Data source not found");
                                }
                                tb.Partitions.Add(new AS.Partition
                                {
                                    Name = par.Id.ToString(),
                                    Source = new AS.QueryPartitionSource()
                                    {
                                        DataSource = ds,
                                        Query = par.Query
                                    }
                                });
                            }
                            if (par.SourceType == PartitionSourceType.Calculated)
                            {
                                tb.Partitions.Add(new AS.Partition
                                {
                                    Name = par.Id.ToString(),
                                    Source = new AS.CalculatedPartitionSource()
                                    {
                                        Expression = par.Expression
                                    }
                                });
                            }
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
                        if (par.SourceType == PartitionSourceType.Query)
                        {
                            var ds = database.Model.DataSources.Find(par.DataSourceId.ToString());
                            if (ds == null)
                            {
                                throw new Exception("Data source not found");
                            }
                            tb.Partitions.Add(new AS.Partition
                            {
                                Name = par.Id.ToString(),
                                Source = new AS.QueryPartitionSource()
                                {
                                    DataSource = database.Model.DataSources[ds.Name],
                                    Query = par.Query
                                }
                            });
                        }
                        if (par.SourceType == PartitionSourceType.Calculated)
                        {
                            tb.Partitions.Add(new AS.Partition
                            {
                                Name = par.Id.ToString(),
                                Source = new AS.CalculatedPartitionSource()
                                {
                                    Expression = par.Expression
                                }
                            });
                        }
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
                    if (par.SourceType == PartitionSourceType.Query)
                    {
                        var ds = database.Model.DataSources.Find(par.DataSourceId.ToString());
                        if (ds == null)
                        {
                            throw new Exception("Data Source not found");
                        }
                        tb.Partitions.Add(new AS.Partition
                        {
                            Name = par.Id.ToString(),
                            Source = new AS.QueryPartitionSource()
                            {
                                DataSource = ds,
                                Query = par.Query
                            }
                        });
                    }
                    if (par.SourceType == PartitionSourceType.Calculated)
                    {
                        tb.Partitions.Add(new AS.Partition
                        {
                            Name = par.Id.ToString(),
                            Source = new AS.CalculatedPartitionSource()
                            {
                                Expression = par.Expression
                            }
                        });
                    }
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
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(table.ModelId.ToString());
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                var tb = database.Model.Tables.Find(table.Name);
                tb.Description = table.Description;
                database.Update(AN.UpdateOptions.ExpandFull);
            }
        }

        public void DeleteTable(Table table)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(table.ModelId.ToString());
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                var tb = database.Model.Tables.Find(table.Name);
                if (tb != null)
                {
                    database.Model.Tables.Remove(tb);
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }

        }

        //Partition-------------------------------------------------

        public void CreatePartition(string databaseName, string tableName, Partition partitionInfo)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(databaseName);
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                var tb = database.Model.Tables.Find(tableName);
                if (tb == null)
                {
                    throw new ArgumentException("Table not found");
                }
                var pa = new AS.Partition
                {
                    Name = partitionInfo.Id.ToString(),
                    Description = partitionInfo.Description
                };
                if (partitionInfo.SourceType == PartitionSourceType.Query)
                {
                    var dataSource = database.Model.DataSources.Find(partitionInfo.DataSourceId.ToString());
                    if (dataSource == null)
                    {
                        throw new ArgumentException("Data Source not found");
                    }
                    pa.Source = new AS.QueryPartitionSource
                    {
                        Query = partitionInfo.Query,
                        DataSource = dataSource
                    };
                }
                if (partitionInfo.SourceType == PartitionSourceType.Calculated)
                {
                    pa.Source = new AS.CalculatedPartitionSource
                    {
                        Expression = partitionInfo.Expression
                    };
                }
                database.Update(AN.UpdateOptions.ExpandFull);
            }

        }

        public void UpdatePartition(string databaseName, string tableName, Partition partitionInfo)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(databaseName);
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                var table = database.Model.Tables.Find(tableName);
                if (table == null)
                {
                    throw new ArgumentException("Table not found");
                }
                var partition = table.Partitions.Find(partitionInfo.Id.ToString());
                if (partition == null)
                {
                    throw new ArgumentException("Partition not found");
                }
                partition.Description = partitionInfo.Description;
                switch (partition.SourceType)
                {
                    case AS.PartitionSourceType.Query:
                        {
                            if (partition.Source is AS.QueryPartitionSource source)
                            {
                                source.Query = partitionInfo.Query;
                            }
                            break;
                        }
                    case AS.PartitionSourceType.Calculated:
                        {
                            if (partition.Source is AS.CalculatedPartitionSource source)
                            {
                                source.Expression = partitionInfo.Expression;
                            }
                            break;
                        }
                    case AS.PartitionSourceType.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                database.Update(AN.UpdateOptions.ExpandFull);
            }
        }

        public void DeletePartition(string databaseName, string tableName, Partition partitionInfo)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(databaseName);
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                var table = database.Model.Tables.Find(tableName);
                if (table == null)
                {
                    throw new ArgumentException("Table not found");
                }
                var partition = table.Partitions.Find(partitionInfo.Id.ToString());
                if (partition == null) return;
                table.Partitions.Remove(partition);
                database.Update(AN.UpdateOptions.ExpandFull);
            }

        }

        //Relationship---------------------------------------------

        public void CreateRelationship(string databaseName, Relationship relationshipInfo)
        {
            if (relationshipInfo.FromColumn?.Table == null || relationshipInfo.ToColumn?.Table == null)
            {
                return;
            }
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(databaseName);
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                var relationship = new AS.SingleColumnRelationship
                {
                    Name = relationshipInfo.Id.ToString(),
                    FromColumn = database.Model.Tables[relationshipInfo.FromColumn.Table.Name].Columns[relationshipInfo.FromColumn.Name],
                    ToColumn = database.Model.Tables[relationshipInfo.ToColumn.Table.Name].Columns[relationshipInfo.ToColumn.Name],
                    CrossFilteringBehavior = (AS.CrossFilteringBehavior)relationshipInfo.CrossFilteringBehavior,
                    SecurityFilteringBehavior = (AS.SecurityFilteringBehavior)relationshipInfo.SecurityFilteringBehavior,
                    ToCardinality = AS.RelationshipEndCardinality.One,
                    IsActive = relationshipInfo.IsActive
                };
                if (relationshipInfo.DateBehavior != null)
                {
                    relationship.JoinOnDateBehavior = (AS.DateTimeRelationshipBehavior)relationshipInfo.DateBehavior;
                }
                relationship.FromCardinality = relationshipInfo.Cardinality == RelationshipCardinality.OneToOne ?
                    AS.RelationshipEndCardinality.One : AS.RelationshipEndCardinality.Many;
                database.Model.Relationships.Add(relationship);
                database.Update(AN.UpdateOptions.ExpandFull);
            }

        }

        public void UpdateRelationship(string databaseName, Relationship relationshipInfo)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(databaseName);
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                if (!(database.Model.Relationships.Find(relationshipInfo.Id.ToString()) is AS.SingleColumnRelationship relationship))
                {
                    throw new ArgumentException("Relationship not found");
                }
                relationship.FromColumn = database.Model.Tables[relationshipInfo.FromColumn.Table.Name].Columns[relationshipInfo.FromColumn.Name];
                relationship.ToColumn = database.Model.Tables[relationshipInfo.ToColumn.Table.Name].Columns[relationshipInfo.ToColumn.Name];
                relationship.CrossFilteringBehavior = (AS.CrossFilteringBehavior)relationshipInfo.CrossFilteringBehavior;
                relationship.SecurityFilteringBehavior = (AS.SecurityFilteringBehavior)relationshipInfo.SecurityFilteringBehavior;
                relationship.ToCardinality = AS.RelationshipEndCardinality.One;
                relationship.IsActive = relationshipInfo.IsActive;
                if (relationshipInfo.DateBehavior != null)
                {
                    relationship.JoinOnDateBehavior = (AS.DateTimeRelationshipBehavior)relationshipInfo.DateBehavior;
                }
                relationship.FromCardinality = relationshipInfo.Cardinality == RelationshipCardinality.OneToOne ? AS.RelationshipEndCardinality.One : AS.RelationshipEndCardinality.Many;
                relationship.ToColumn = database.Model.Tables[relationshipInfo.ToColumn.Table.Name].Columns[relationshipInfo.ToColumn.Name];
                relationship.FromColumn = database.Model.Tables[relationshipInfo.FromColumn.Table.Name].Columns[relationshipInfo.FromColumn.Name];
                database.Update(AN.UpdateOptions.ExpandFull);
            }
        }

        public void DeleteRelationship(string databaseName, Relationship relationshipInfo)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(databaseName);
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                var re = database.Model.Relationships.Find(relationshipInfo.Id.ToString());
                if (re == null) return;
                database.Model.Relationships.Remove(re);
                database.Update(AN.UpdateOptions.ExpandFull);
            }
        }

        //Measure--------------------------------------------------

        public void CreateMeasure(string databaseName, string tableName, Measure measureInfo)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(databaseName);
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                var table = database.Model.Tables[tableName];
                if (table == null)
                {
                    throw new ArgumentException("Table not found");
                }
                var measure = new AS.Measure
                {
                    Name = measureInfo.Name,
                    Expression = measureInfo.Expression
                };
                table.Measures.Add(measure);
                database.Update(AN.UpdateOptions.ExpandFull);
            }

        }

        public void UpdateMeasure(string databaseName, string tableName, Measure measureInfo)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(databaseName);
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                var table = database.Model.Tables[tableName];
                if (table == null)
                {
                    throw new ArgumentException("Table not found");
                }
                var measure = table.Measures[measureInfo.Name];
                if (measure == null)
                {
                    throw new ArgumentException("Measure not found");
                }
                measure.Expression = measureInfo.Expression;
                database.Update(AN.UpdateOptions.ExpandFull);
            }
        }

        public void DeleteMeasure(string databaseName, string tableName, Measure measureInfo)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(databaseName);
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                var table = database.Model.Tables[tableName];
                var measure = table.Measures[measureInfo.Name];
                if (measure == null) return;
                table.Measures.Remove(measure);
                database.Update(AN.UpdateOptions.ExpandFull);
            }
        }

        //Column--------------------------------------------------

        public void CreateColumn(string databaseName, string tableName, Column columnInfo)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(databaseName);
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                var table = database.Model.Tables.Find(tableName);
                if (table == null)
                {
                    throw new ArgumentException("Table not found");
                }
                switch (columnInfo.ColumnType)
                {
                    case ColumnType.Calculated:
                        {
                            var column = new AS.CalculatedColumn
                            {
                                Name = columnInfo.Name,
                                Description = columnInfo.Description,
                                DataType = columnInfo.DataType.ToDataType(),
                                DisplayFolder = columnInfo.DisplayFolder,
                                FormatString = columnInfo.FormatString,
                                IsHidden = columnInfo.IsHidden,
                                Expression = columnInfo.Expression
                            };
                            table.Columns.Add(column);
                            break;
                        }
                    case ColumnType.Data:
                        {
                            var column = new AS.DataColumn
                            {
                                Name = columnInfo.Name,
                                Description = columnInfo.Description,
                                DataType = columnInfo.DataType.ToDataType(),
                                DisplayFolder = columnInfo.DisplayFolder,
                                FormatString = columnInfo.FormatString,
                                IsHidden = columnInfo.IsHidden,
                                SourceColumn = columnInfo.SourceColumn
                            };
                            table.Columns.Add(column);
                            break;
                        }
                    case ColumnType.RowNumber:
                        break;
                    case ColumnType.CalculatedTableColumn:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                database.Update(AN.UpdateOptions.ExpandFull);
            }

        }

        public void UpdateColumn(string databaseName, string tableName, Column columnInfo)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(databaseName);
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                var table = database.Model.Tables.Find(tableName);
                if (table == null)
                {
                    throw new ArgumentException("Table not found");
                }
                switch (columnInfo.ColumnType)
                {
                    case ColumnType.Data:
                        {
                            if (!(table.Columns.Find(columnInfo.Name) is AS.DataColumn column))
                            {
                                throw new ArgumentException("Column not found");
                            }
                            column.Description = columnInfo.Description;
                            column.DataType = columnInfo.DataType.ToDataType();
                            column.DisplayFolder = columnInfo.DisplayFolder;
                            column.FormatString = columnInfo.FormatString;
                            column.IsHidden = columnInfo.IsHidden;
                            column.SourceColumn = columnInfo.SourceColumn;
                            break;
                        }
                    case ColumnType.Calculated:
                        {
                            if (!(table.Columns.Find(columnInfo.Name) is AS.CalculatedColumn column))
                            {
                                throw new ArgumentException("Column not found");
                            }
                            column.Description = columnInfo.Description;
                            column.DataType = columnInfo.DataType.ToDataType();
                            column.DisplayFolder = columnInfo.DisplayFolder;
                            column.FormatString = columnInfo.FormatString;
                            column.IsHidden = columnInfo.IsHidden;
                            column.Expression = columnInfo.Expression;
                            break;
                        }
                    case ColumnType.RowNumber:
                        break;
                    case ColumnType.CalculatedTableColumn:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                database.Update(AN.UpdateOptions.ExpandFull);
            }

        }

        public void DeleteColumn(string databaseName, string tableName, Column columnInfo)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(databaseName);
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                var table = database.Model.Tables.Find(tableName);
                if (table == null)
                {
                    throw new ArgumentException("Table not found");
                }
                var column = table.Columns.Find(columnInfo.Name);
                if (column == null) return;
                table.Columns.Remove(column);
                database.Update(AN.UpdateOptions.ExpandFull);
            }
        }

        //Role----------------------------------------------------

        public void CreateRole(string databaseName, ModelRole modelRoleInfo)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(databaseName);
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                var modelRole = new AS.ModelRole
                {
                    Name = modelRoleInfo.Id.ToString(),
                    Description = modelRoleInfo.Description,
                    ModelPermission = (AS.ModelPermission)modelRoleInfo.ModelPermission
                };
                if (modelRoleInfo.TablePermissions != null && modelRoleInfo.TablePermissions.Count > 0)
                {
                    foreach (var tablePermission in modelRoleInfo.TablePermissions)
                    {
                        modelRole.TablePermissions.Add(new AS.TablePermission
                        {
                            Name = modelRole.TablePermissions.GetNewName(),
                            FilterExpression = tablePermission.FilterExpression
                        });
                    }
                }
                database.Model.Roles.Add(modelRole);
                database.Update(AN.UpdateOptions.ExpandFull);
            }
        }

        public void UpdateRole(string databaseName, ModelRole modelRoleInfo)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(databaseName);
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                var modelRole = database.Model.Roles.Find(modelRoleInfo.Id.ToString());
                modelRole.Description = modelRoleInfo.Description;
                database.Model.Roles.Add(modelRole);
                database.Update(AN.UpdateOptions.ExpandFull);
            }
        }

        public void DeleteRole(string databaseName, ModelRole modelRoleInfo)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(modelRoleInfo.ModelId.ToString());
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                var modelRole = database.Model.Roles.Find(modelRoleInfo.Id.ToString());
                if (modelRole == null) return;
                database.Model.Roles.Remove(modelRole);
                database.Update(AN.UpdateOptions.ExpandFull);
            }
        }

        //Role table permission-----------------------------------

        public void CreateRoleTablePermission(string databaseName, string modelRoleName, string tableName, ModelRoleTablePermission tablePermissionInfo)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(databaseName);
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                var modelRole = database.Model.Roles.Find(tablePermissionInfo.RoleId.ToString());
                if (modelRole == null)
                {
                    throw new Exception("Role not found");
                }
                var table = database.Model.Tables.Find(tableName);
                if (table == null)
                {
                    throw new Exception("Table not found");
                }
                modelRole.TablePermissions.Add(new AS.TablePermission
                {
                    Table = table,
                    FilterExpression = tablePermissionInfo.FilterExpression
                });
                database.Update(AN.UpdateOptions.ExpandFull);
            }
        }

        public void UpdateRoleTablePermission(string databaseName, string modelRoleName, string tableName, ModelRoleTablePermission tablePermissionInfo)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(databaseName);
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                var modelRole = database.Model.Roles.Find(tablePermissionInfo.RoleId.ToString());
                if (modelRole == null)
                {
                    throw new Exception("Role not found");
                }
                var tablePermission = modelRole.TablePermissions.Find(tableName);
                if (tablePermission == null)
                {
                    throw new Exception("Table permission not found");
                }
                tablePermission.FilterExpression = tablePermissionInfo.FilterExpression;
                database.Update(AN.UpdateOptions.ExpandFull);
            }
        }

        public void DeleteRoleTablePermission(string databaseName, string modelRoleName, ModelRoleTablePermission tablePermissionInfo)
        {
            using (var server = new AS.Server())
            {
                server.Connect(_asConnectionString);
                var database = server.Databases.FindByName(databaseName);
                if (database == null)
                {
                    throw new ArgumentException("Database not found");
                }
                var modelRole = database.Model.Roles.Find(tablePermissionInfo.RoleId.ToString());
                if (modelRole == null)
                {
                    throw new Exception("Role not found");
                }
                var tablePermission = modelRole.TablePermissions.Find(tablePermissionInfo.TableId.ToString());
                if (tablePermission == null) return;
                modelRole.TablePermissions.Remove(tablePermission);
                database.Update(AN.UpdateOptions.ExpandFull);
            }
        }

        public AS.Table AddCalculatedTable(AS.Database database, int modelId, Table tableInfo)
        {
            var table = new AS.Table
            {
                Name = tableInfo.Name,
                Description = tableInfo.Description,
            };
            if (tableInfo.Partitions != null)
            {
                foreach (var par in tableInfo.Partitions)
                {
                    if (par.SourceType == PartitionSourceType.Calculated)
                    {
                        table.Partitions.Add(new AS.Partition
                        {
                            Name = par.Name,
                            Source = new AS.CalculatedPartitionSource()
                            {
                                Expression = par.Expression
                            }
                        });
                    }
                }
            }
            database.Model.Tables.Add(table);
            return table;
        }

        //public void FixModelColumns(int modelId)
        //{
        //    var model = Context.Models.Find(modelId);
        //    if (model == null)
        //    {
        //        throw new ArgumentException("Model not found");
        //    }
        //    try
        //    {
        //        using (var server = new AS.Server())
        //        {
        //            server.Connect(_asConnectionString);
        //            var database = server.Databases.FindByName(modelId.ToString());
        //            if (database == null)
        //            {
        //                throw new ArgumentException("Database not found");
        //            }
        //            //List<KeyValuePair<string, AS.DataColumn>> newCols = new List<KeyValuePair<string, AS.DataColumn>>();
        //            foreach (var tb in database.Model.Tables)
        //            {
        //                foreach (var co in tb.Columns)
        //                {
        //                    if (co.Type == AS.ColumnType.Data)
        //                    {
        //                        if (string.IsNullOrEmpty(((AS.DataColumn)co).SourceColumn))
        //                        {
        //                            ((AS.DataColumn)co).SourceColumn = ((AS.DataColumn)co).Name;
        //                        }
        //                    }
        //                    //if (co.DataType == AS.DataType.Binary)
        //                    //{
        //                    //    var nv = new AS.DataColumn
        //                    //    {
        //                    //        Name = co.Name,
        //                    //        Description = co.Description,
        //                    //        DataType = AS.DataType.Int64,
        //                    //        DisplayFolder = co.DisplayFolder,
        //                    //        FormatString = co.FormatString,
        //                    //        IsHidden = co.IsHidden
        //                    //    };
        //                    //    tb.Columns.Remove(co);
        //                    //    newCols.Add(new KeyValuePair<string, AS.DataColumn>(tb.Name, nv));
        //                    //}

        //                }
        //            }
        //            database.Update(AN.UpdateOptions.ExpandFull);
        //            //foreach (var c in newCols)
        //            //{
        //            //    database.Model.Tables[c.Key].Columns.Add(c.Value);
        //            //}
        //            //database.Update(AN.UpdateOptions.ExpandFull);
        //        }
        //    }
        //    catch (AN.OperationException ex)
        //    {
        //        foreach (AN.XmlaError err in ex.Results.OfType<AN.XmlaError>().Cast<AN.XmlaError>())
        //        {
        //        }
        //        throw;
        //    }
        //}

        //public string GetModelDateColumn(int modelId)
        //{
        //    var model = Context.Models.Find(modelId);
        //    if (model == null)
        //    {
        //        throw new Exception("Model not found");
        //    }
        //    using (var server = new AS.Server())
        //    {
        //        server.Connect(_asConnectionString);
        //        var database = server.Databases.FindByName(modelId.ToString());
        //        if (database == null)
        //        {
        //            throw new ArgumentException("Database not found");
        //        }
        //        foreach (var t in database.Model.Tables)
        //        {
        //            if (t.DataCategory == "Time")
        //            {
        //                foreach (var c in t.Columns)
        //                {
        //                    if (c.IsKey)
        //                    {
        //                        return $"'{t.Name}'[{c.Name}]";
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return null;
        //}

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

        //public string BuildDateTableExpression(DateTime fromDate, DateTime toDate)
        //{
        //    var dataExpr = new List<string>();
        //    var i = 0;
        //    var sdn = 0;
        //    var smn = 0; var cm = 0;
        //    var sqn = 0; var cq = 0;
        //    while (true)
        //    {
        //        var currentDate = fromDate.AddDays(i);
        //        if (currentDate > toDate) { break; }
        //        if (!(currentDate.Month == 2 && currentDate.Day == 29))
        //        {
        //            sdn++;
        //        }
        //        if (currentDate.Month != cm)
        //        {
        //            smn++;
        //            cm = currentDate.Month;
        //        }
        //        if (currentDate.Quarter() != cq)
        //        {
        //            sqn++;
        //            cq = currentDate.Quarter();
        //        }
        //        var date = new DateData(currentDate)
        //        {
        //            SequentialDayNumber = sdn
        //        };
        //        var tmp = new[]
        //        {
        //            date.DateKey.ToString(),
        //            "\"" + date.Date.ToString("yyyy-MM-dd") + "\"",
        //            "\"" + date.DateName + "\"",
        //            "\"" + date.PreviousMonthDate.ToString("yyyy-MM-dd") + "\"",
        //            "\"" + date.PreviousQuarterDate.ToString("yyyy-MM-dd") + "\"",
        //            "\"" + date.PreviousYearDate.ToString("yyyy-MM-dd") + "\"",
        //            date.SequentialDayNumber.ToString(),
        //            date.Year.ToString(),
        //            "\"" +date.YearName + "\"",
        //            date.Month.ToString(),
        //            "\"" +date.MonthName + "\"",
        //            date.Quarter.ToString(),
        //            "\"" +date.QuarterName + "\"",
        //            date.HalfYear.ToString(),
        //            "\"" +date.HalfYearName + "\"",
        //            date.DayOfMonth.ToString(),
        //            "\"" +date.DayOfMonthName + "\"",
        //            date.DayOfWeek.ToString(),
        //            "\"" +date.DayOfWeekName + "\"",
        //            date.DayOfYear.ToString(),
        //            date.DayOfQuarter.ToString(),
        //            date.MonthOfYear.ToString(),
        //            "\"" +date.MonthOfYearName + "\"",
        //            date.MonthTotalDays.ToString(),
        //            date.QuarterOfYear.ToString(),
        //            "\"" +date.QuarterOfYearName + "\"",
        //            date.QuarterTotalDays.ToString(),
        //            date.HalfYearOfYear.ToString(),
        //            "\"" +date.HalfYearOfYearName + "\"",
        //            date.LunarDate.ToString(),
        //            "\"" +date.LunarDateName + "\"",
        //            date.LunarMonth.ToString(),
        //            "\"" +date.LunarMonthName + "\"",
        //            date.LunarQuarter.ToString(),
        //            "\"" +date.LunarQuarterName + "\"",
        //            date.LunarYear.ToString(),
        //            "\"" +date.LunarYearName + "\"",
        //            date.LunarDayOfWeek.ToString(),
        //            "\"" +date.LunarDayOfWeekName + "\"",
        //            date.LunarDayOfMonth.ToString(),
        //            "\"" +date.LunarDayOfMonthName + "\"",
        //            date.LunarMonthOfYear.ToString(),
        //            "\"" +date.LunarMonthOfYearName + "\"",
        //            date.LunarQuarterOfYear.ToString(),
        //            "\"" +date.LunarQuarterOfYearName + "\"",
        //            "\"" +date.EventName + "\""
        //        };
        //        dataExpr.Add(" { " + string.Join(", ", tmp) + " } ");
        //        i++;
        //    }
        //    return "{" + string.Join(", ", dataExpr) + "}";
        //}

        //public string GetAnalysisServiceConnectionString(int modelId)
        //{
        //    var conStrBuilder = new OleDbConnectionStringBuilder(_asConnectionString)
        //    {
        //        ["Catalog"] = modelId.ToString()
        //    };
        //    var userRoles = Context.UserRoles.Where(x => x.UserId == UserId).Select(x => x.RoleId);
        //    var roles = Context.Roles.Where(x => x.ModelId == modelId && userRoles.Contains(x.Id)).Select(x => x.Id).ToList();
        //    if (roles != null && roles.Count > 0)
        //    {
        //        conStrBuilder["Roles"] = string.Join(",", roles);
        //    }
        //    return conStrBuilder.ConnectionString;
        //}

    }
}