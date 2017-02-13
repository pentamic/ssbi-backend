using Breeze.ContextProvider;
using Breeze.ContextProvider.EF6;
using Microsoft.AnalysisServices.AdomdClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pentamic.SSBI.Models.Reporting;
using Pentamic.SSBI.Models.Reporting.Query;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Services
{
    public class ReportingService
    {
        private EFContextProvider<ReportingContext> _contextProvider;

        public ReportingService()
        {
            _contextProvider = new EFContextProvider<ReportingContext>();
        }

        private ReportingContext Context
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

        public IQueryable<Report> Reports
        {
            get { return Context.Reports; }
        }
        public IQueryable<ReportPage> ReportPages
        {
            get { return Context.ReportPages; }
        }
        public IQueryable<ReportTile> ReportTiles
        {
            get { return Context.ReportTiles; }
        }

        public List<Dictionary<string, object>> GetReportTileData(int reportTileId)
        {
            var tile = Context.ReportTiles.Where(x => x.Id == reportTileId)
                .Include(x => x.ReportPage.Report)
                .FirstOrDefault();
            var modelService = new DataModelService();
            var model = modelService.Models.Where(x => x.Id == tile.ReportPage.Report.ModelId)
                .FirstOrDefault();
            var conStr = @"DataSource=.\astab16;Catalog=" + model.DatabaseName;
            using (var conn = new AdomdConnection(conStr))
            {
                try
                {
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = BuildReportTileQuery(tile);
                    using (var reader = command.ExecuteReader())
                    {
                        var result = new List<Dictionary<string, object>>();
                        while (reader.Read())
                        {
                            var row = new Dictionary<string, object>();
                            var columns = new List<string>();
                            for (var i = 0; i < reader.FieldCount; ++i)
                            {
                                columns.Add(reader.GetName(i));
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

        public string BuildReportTileQuery(ReportTile tile)
        {
            var query = "EVALUATE ( SUMMARIZECOLUMNS ( {0} ) ) ";
            var columns = new List<string>();
            var filterAfter = new List<string>();
            if (!string.IsNullOrEmpty(tile.ColumnFields))
            {
                columns.AddRange(tile.ColumnFields.Split(','));
            }
            if (!string.IsNullOrEmpty(tile.RowFields))
            {
                columns.AddRange(tile.RowFields.Split(','));
            }
            if (!string.IsNullOrEmpty(tile.FilterFields))
            {
                var filterFields = JsonConvert.DeserializeObject<List<FilterField>>(tile.FilterFields);
                foreach (var field in filterFields)
                {
                    //var filter = "FILTER({0},{1})";
                    string filterExpr = "";
                    switch (field.Operator)
                    {
                        case FilterOperator.Equals:
                            {
                                filterExpr = $"{field.Name} = \"{field.Value}\"";
                            }
                            break;
                        case FilterOperator.NotEquals:
                            {
                                filterExpr = $"{field.Name} <> \"{field.Value}\"";
                            }
                            break;
                        case FilterOperator.In:
                            {
                                filterExpr = string.Join("||", field.Value.Split(',').Select(x => $"{field.Name} = \"{x}\""));
                            }
                            break;
                        case FilterOperator.Contains:
                            {
                                filterExpr = $"SEARCH(\"{field.Value}\",{field.Name},1,0) > 0";
                            }
                            break;
                        case FilterOperator.StartsWith:
                            {
                                filterExpr = $"LEFT({field.Name},{field.Value.Length}) = \"{field.Value}\"";
                            }
                            break;
                        case FilterOperator.EndsWith:
                            {
                                filterExpr = $"RIGHT({field.Name},{field.Value.Length}) = \"{field.Value}\"";
                            }
                            break;
                        case FilterOperator.GreaterThan:
                            {
                                filterExpr = $"{field.Name} > {field.Value}";
                            }
                            break;
                        case FilterOperator.GreaterThanOrEqual:
                            {
                                filterExpr = $"{field.Name} >= {field.Value}";
                            }
                            break;
                        case FilterOperator.LessThan:
                            {
                                filterExpr = $"{field.Name} < {field.Value}";
                            }
                            break;
                        case FilterOperator.LessThanOrEqual:
                            {
                                filterExpr = $"{field.Name} <= {field.Value}";
                            }
                            break;
                        default: continue;
                    }
                    //filterAfter.Add(filterExpr);
                    if (field.IsMeasure)
                    {
                        filterAfter.Add(filterExpr);
                    }
                    else
                    {
                        columns.Add($"FILTER({field.TableName},{filterExpr})");
                    }
                }

            }
            if (!string.IsNullOrEmpty(tile.ValueFields))
            {
                var valueFields = tile.ValueFields.Split(',');
                foreach (var field in valueFields)
                {
                    var sIdx = field.IndexOf('[') + 1;
                    var name = field.Substring(field.IndexOf('[') + 1, field.Length - 1 - sIdx);
                    columns.Add("\"" + field + "\"");
                    columns.Add(field);
                }
            }
            if (filterAfter.Count > 0)
            {
                query = "EVALUATE ( FILTER (  SUMMARIZECOLUMNS ( {0} ), {1} ) ) ";
                query = string.Format(query, string.Join(",", columns), string.Join(" && ", filterAfter));
            }
            else
            {
                query = string.Format(query, string.Join(",", columns));
            }

            return query;
        }

        //public string BuildReportTileQuery1(List<QueryField> fields)
        //{
        //    var groupByType = fields.GroupBy(x => x.Type).Select(x => new
        //    {
        //        Type = x.Key,
        //        Fields = x
        //    });
        //    var query = "EVALUATE ( SUMMARIZECOLUMNS ( {0} ) ) ";
        //    var columns = new List<string>();
        //    var filterAfter = new List<string>();
        //    if (!string.IsNullOrEmpty(tile.ColumnFields))
        //    {
        //        columns.AddRange(tile.ColumnFields.Split(','));
        //    }
        //    if (!string.IsNullOrEmpty(tile.RowFields))
        //    {
        //        columns.AddRange(tile.RowFields.Split(','));
        //    }
        //    if (!string.IsNullOrEmpty(tile.FilterFields))
        //    {
        //        var filterFields = JsonConvert.DeserializeObject<List<FilterField>>(tile.FilterFields);
        //        foreach (var field in filterFields)
        //        {
        //            //var filter = "FILTER({0},{1})";
        //            string filterExpr = "";
        //            switch (field.Operator)
        //            {
        //                case FilterOperator.Equals:
        //                    {
        //                        filterExpr = $"{field.Name} = \"{field.Value}\"";
        //                    }
        //                    break;
        //                case FilterOperator.NotEquals:
        //                    {
        //                        filterExpr = $"{field.Name} <> \"{field.Value}\"";
        //                    }
        //                    break;
        //                case FilterOperator.In:
        //                    {
        //                        filterExpr = string.Join("||", field.Value.Split(',').Select(x => $"{field.Name} = \"{x}\""));
        //                    }
        //                    break;
        //                case FilterOperator.Contains:
        //                    {
        //                        filterExpr = $"SEARCH(\"{field.Value}\",{field.Name},1,0) > 0";
        //                    }
        //                    break;
        //                case FilterOperator.StartsWith:
        //                    {
        //                        filterExpr = $"LEFT({field.Name},{field.Value.Length}) = \"{field.Value}\"";
        //                    }
        //                    break;
        //                case FilterOperator.EndsWith:
        //                    {
        //                        filterExpr = $"RIGHT({field.Name},{field.Value.Length}) = \"{field.Value}\"";
        //                    }
        //                    break;
        //                case FilterOperator.GreaterThan:
        //                    {
        //                        filterExpr = $"{field.Name} > {field.Value}";
        //                    }
        //                    break;
        //                case FilterOperator.GreaterThanOrEqual:
        //                    {
        //                        filterExpr = $"{field.Name} >= {field.Value}";
        //                    }
        //                    break;
        //                case FilterOperator.LessThan:
        //                    {
        //                        filterExpr = $"{field.Name} < {field.Value}";
        //                    }
        //                    break;
        //                case FilterOperator.LessThanOrEqual:
        //                    {
        //                        filterExpr = $"{field.Name} <= {field.Value}";
        //                    }
        //                    break;
        //                default: continue;
        //            }
        //            //filterAfter.Add(filterExpr);
        //            if (field.IsMeasure)
        //            {
        //                filterAfter.Add(filterExpr);
        //            }
        //            else
        //            {
        //                columns.Add($"FILTER({field.TableName},{filterExpr})");
        //            }
        //        }

        //    }
        //    if (!string.IsNullOrEmpty(tile.ValueFields))
        //    {
        //        var valueFields = tile.ValueFields.Split(',');
        //        foreach (var field in valueFields)
        //        {
        //            var sIdx = field.IndexOf('[') + 1;
        //            var name = field.Substring(field.IndexOf('[') + 1, field.Length - 1 - sIdx);
        //            columns.Add("\"" + field + "\"");
        //            columns.Add(field);
        //        }
        //    }
        //    if (filterAfter.Count > 0)
        //    {
        //        query = "EVALUATE ( FILTER (  SUMMARIZECOLUMNS ( {0} ), {1} ) ) ";
        //        query = string.Format(query, string.Join(",", columns), string.Join(" && ", filterAfter));
        //    }
        //    else
        //    {
        //        query = string.Format(query, string.Join(",", columns));
        //    }

        //    return query;
        //}

        public SaveResult SaveChanges(JObject saveBundle)
        {
            var txSettings = new TransactionSettings { TransactionType = TransactionType.TransactionScope };
            return _contextProvider.SaveChanges(saveBundle, txSettings);
        }

    }
}