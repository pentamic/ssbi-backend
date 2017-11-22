using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Ciloci.Flee;
using Microsoft.AnalysisServices.AdomdClient;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Services.SSAS.Query
{
    public class QueryService
    {
        private readonly string _connectionString;

        public QueryService(string connectionString)
        {
            _connectionString = connectionString;
        }

        //public List<Dictionary<string, object>> GetReportTileData(int reportTileId)
        //{
        //    var tile = Context.ReportTiles.Where(x => x.Id == reportTileId)
        //        .Include(x => x.ReportPage.Report)
        //        .FirstOrDefault();
        //    var modelService = new DataModelService();
        //    var model = modelService.Models.Where(x => x.Id == tile.ReportPage.Report.ModelId)
        //        .FirstOrDefault();
        //    var conStr = @"DataSource=.\astab16;Catalog=" + model.DatabaseName;
        //    using (var conn = new AdomdConnection(conStr))
        //    {
        //        try
        //        {
        //            conn.Open();
        //            var command = conn.CreateCommand();
        //            command.CommandText = BuildReportTileQuery(tile);
        //            using (var reader = command.ExecuteReader())
        //            {
        //                var result = new List<Dictionary<string, object>>();
        //                while (reader.Read())
        //                {
        //                    var row = new Dictionary<string, object>();
        //                    var columns = new List<string>();
        //                    for (var i = 0; i < reader.FieldCount; ++i)
        //                    {
        //                        columns.Add(reader.GetName(i));
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

        //public string BuildReportTileQuery(ReportTile tile)
        //{
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

        public List<Dictionary<string, object>> Query(QueryModel queryModel)
        {
            var query = queryModel.Filters2.Count > 0 ?
                $" EVALUATE ( FILTER (  SUMMARIZECOLUMNS ( {string.Join(",", queryModel.Columns.Concat(queryModel.Filters1).Concat(queryModel.Values))} ), {string.Join(" && ", queryModel.Filters2)} ) ) "
                : $" EVALUATE ( SUMMARIZECOLUMNS ( {string.Join(",", queryModel.Columns.Concat(queryModel.Filters1).Concat(queryModel.Values))} ) ) ";
            if (queryModel.OrderBy.Count > 0)
            {
                query += $" ORDER BY {string.Join(",", queryModel.OrderBy)} ";
            }
            //var conStrBuilder = new OleDbConnectionStringBuilder(_asConnectionString)
            //{
            //    ["Catalog"] = queryModel.ModelId.ToString()
            //};
            using (var conn = new AdomdConnection(GetAnalysisServiceConnectionString(queryModel.ModelId)))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = query;
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
        }


        public List<ReportTileRowQueryResult> QueryRowTile(List<ReportTileRow> tileRows, QueryModel2 queryModel)
        {
            if (tileRows.Count == 0)
            {
                return new List<ReportTileRowQueryResult>();
            }
            var rowExprList = new List<string>();
            var allRowExpr = "";
            foreach (var r in tileRows.Where(x => !x.IsFormula))
            {
                if (!string.IsNullOrEmpty(r.ValueExpression))
                {
                    var rowExpr = BuildTileRowQuery(r);
                    rowExprList.Add(rowExpr);
                }
            }
            if (rowExprList.Count > 1)
            {
                allRowExpr = $"UNION({string.Join(",", rowExprList)})";
            }
            else if (rowExprList.Count > 0)
            {
                allRowExpr = rowExprList.First();
            }
            var query = "";
            if (string.IsNullOrEmpty(queryModel.FilterExpression))
            {
                query = $"EVALUATE( {allRowExpr} )";
            }
            else
            {
                query = $"EVALUATE( CALCULATETABLE ( {allRowExpr}, {queryModel.FilterExpression}) )";
            }
            //var conStrBuilder = new OleDbConnectionStringBuilder(_asConnectionString)
            //{
            //    ["Catalog"] = tile.ModelId.ToString()
            //};
            var result = tileRows.Select(x => new ReportTileRowQueryResult(x)).ToList();
            using (var conn = new AdomdConnection(GetAnalysisServiceConnectionString(queryModel.ModelId)))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = query;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        var pmtdVal = reader.GetValue(1);
                        var mtdVal = reader.GetValue(2);
                        var pytdVal = reader.GetValue(3);
                        var ytdVal = reader.GetValue(4);
                        for (var i = 0; i < result.Count; ++i)
                        {
                            if (result[i].Id == id)
                            {
                                result[i].MTD = (decimal?)mtdVal ?? 0;
                                result[i].YTD = (decimal?)ytdVal ?? 0;
                                result[i].PMTD = (decimal?)pmtdVal ?? 0;
                                result[i].PYTD = (decimal?)pytdVal ?? 0;
                                result[i].MTDRate = result[i].PMTD == 0 ? 0 : result[i].MTD / result[i].PMTD;
                                result[i].YTDRate = result[i].PYTD == 0 ? 0 : result[i].YTD / result[i].PYTD;
                                result[i].IsCalculated = true;
                                break;
                            }
                        }
                    }
                }
            }
            foreach (var r in result)
            {
                if (r.IsFormula)
                {
                    CalculateEntryFormula(r, result);
                }
            }
            return result.OrderBy(x => x.Ordinal).ToList();
        }

        public List<Dictionary<string, object>> QueryCustom(QueryModel3 queryModel)
        {
            //var conStrBuilder = new OleDbConnectionStringBuilder(_asConnectionString)
            //{
            //    ["Catalog"] = queryModel.ModelId.ToString()
            //};
            using (var conn = new AdomdConnection(GetAnalysisServiceConnectionString(queryModel.ModelId)))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = queryModel.Query;
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
        }

        public string BuildTileRowQuery(ReportTileRow row)
        {
            var mtd = "FILTER ( ALL(DimDate), DimDate[Month] = MAX(DimDate[Month]) && DimDate[Date] <= MAX(DimDate[Date]))";
            var ytd = "FILTER ( ALL(DimDate), DimDate[Year] = MAX(DimDate[Year]) && DimDate[Date] <= MAX(DimDate[Date]))";
            var pmtd = "FILTER ( ALL(DimDate), DimDate[Month] = MAX(DimDate[Month]) - 1 && DimDate[Date] <= MAX(DimDate[PreviousMonthDate]))";
            var pytd = "FILTER ( ALL(DimDate), DimDate[Year] = MAX(DimDate[Year]) - 1 && DimDate[Date] <= MAX(DimDate[PreviousYearDate]))";
            if (string.IsNullOrEmpty(row.FilterExpression))
            {
                var res =
                    $"\"Id\", {row.Id}," +
                    $"\"PMTD\", CALCULATE ( {row.ValueExpression}, {pmtd})," +
                    $"\"MTD\", CALCULATE ( {row.ValueExpression}, {mtd})," +
                    $"\"PYTD\", CALCULATE ( {row.ValueExpression}, {pytd})," +
                    $"\"YTD\", CALCULATE ( {row.ValueExpression}, {ytd})";
                return $"ROW ({res})";
            }
            else
            {
                var res =
                    $"\"Id\", {row.Id}," +
                    $"\"PMTD\", CALCULATE ( {row.ValueExpression}, {pmtd}, {row.FilterExpression})," +
                    $"\"MTD\", CALCULATE ( {row.ValueExpression}, {mtd}, {row.FilterExpression})," +
                    $"\"PYTD\", CALCULATE ( {row.ValueExpression}, {pytd}, {row.FilterExpression})," +
                    $"\"YTD\", CALCULATE ( {row.ValueExpression}, {ytd}, {row.FilterExpression})";
                return $"ROW ({res})";
            }
        }

        public void CalculateEntryFormula(ReportTileRowQueryResult e, List<ReportTileRowQueryResult> r)
        {
            var formula = e.FormulaExpression;
            try
            {
                var codes = new List<string>();
                var regex = new Regex(@"(\[[^]]*\])");
                var match = regex.Match(formula);
                while (match.Success)
                {
                    var val = match.Groups[1].Value;
                    if (!codes.Contains(val))
                    {
                        codes.Add(val);
                    }
                    match = match.NextMatch();
                }
                var context = new ExpressionContext();
                //context.Imports.AddType(typeof(FinancialEntryResult));
                for (var i = 0; i < codes.Count; ++i)
                {
                    var tCode = codes[i].TrimStart('[').TrimEnd(']');
                    formula = formula.Replace(codes[i], "v" + i);
                    var eo = r.FirstOrDefault(x => x.Code == tCode);
                    if (eo != null && (eo.IsFormula && !eo.IsCalculated))
                    {
                        CalculateEntryFormula(eo, r);
                    }
                    context.Variables.Add("v" + i, eo);
                }
                var eDynamic = context.CompileGeneric<ReportTileRowQueryResult>(formula);
                var entryVal = eDynamic.Evaluate();
                e.MTD = entryVal.MTD;
                e.PMTD = entryVal.PMTD;
                e.YTD = entryVal.YTD;
                e.PYTD = entryVal.PYTD;
                e.MTDRate = e.PMTD == 0 ? 0 : e.MTD / e.PMTD;
                e.YTDRate = e.PYTD == 0 ? 0 : e.YTD / e.PYTD;
                e.IsCalculated = true;
            }
            catch (Exception ex)
            {
                var x = ex;
            }
        }

        public List<Dictionary<string, object>> GetFieldValues(FieldQueryModel queryModel)
        {
            var query = $"EVALUATE(VALUES({queryModel.FieldName})) ORDER BY {queryModel.FieldName}";
            //var conStrBuilder = new OleDbConnectionStringBuilder(_asConnectionString)
            //{
            //    ["Catalog"] = queryModel.ModelId.ToString()
            //};
            using (var conn = new AdomdConnection(GetAnalysisServiceConnectionString(queryModel.ModelId)))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = query;
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
        }

        public List<Dictionary<string, object>> GetFieldValuesRange(FieldQueryModel queryModel)
        {
            var minQuery = $"ROW(\"{queryModel.FieldName}\", CALCULATE(MIN({queryModel.FieldName})))";
            var maxQuery = $"ROW(\"{queryModel.FieldName}\", CALCULATE(MAX({queryModel.FieldName})))";
            var query = $"EVALUATE UNION({minQuery},{maxQuery})";
            //var conStrBuilder = new OleDbConnectionStringBuilder(_asConnectionString)
            //{
            //    ["Catalog"] = queryModel.ModelId.ToString()
            //};
            using (var conn = new AdomdConnection(GetAnalysisServiceConnectionString(queryModel.ModelId)))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = query;
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
        }


        public void ProcessAlert(List<Alert> alerts)
        {
            foreach (var alert in alerts)
            {
                var val = CalculateAlertValue(alert);
                var res = false;
                switch (alert.Condition)
                {
                    case AlertCondition.Above:
                        res = val[0] > val[1];
                        break;
                    case AlertCondition.AboveOrEqual:
                        res = val[0] >= val[1];
                        break;
                    case AlertCondition.Below:
                        res = val[0] < val[1];
                        break;
                    case AlertCondition.BelowOrEqual:
                        res = val[0] <= val[1];
                        break;
                }
            }
        }

        public List<decimal> CalculateAlertValue(Alert alert)
        {
            string query = "";
            if (alert.UseThresold)
            {
                query = $"EVALUATE(ROW(\"MainValue\", {BuildAlertValueQuery(alert.MainValueField, alert.MainValueModification, alert.MainFilterExpression)}, \"TargetValue\", {alert.Thresold}))";
            }
            else
            {
                query = $"EVALUATE(ROW(\"MainValue\", {BuildAlertValueQuery(alert.MainValueField, alert.MainValueModification, alert.MainFilterExpression)}, \"TargetValue\", {BuildAlertValueQuery(alert.TargetValueField, alert.TargetValueModification, alert.TargetFilterExpression)}))";
            }
            using (var conn = new AdomdConnection(GetAnalysisServiceConnectionString(alert.ModelId)))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = query;
                using (var reader = command.ExecuteReader())
                {
                    var result = new List<decimal>();
                    reader.Read();
                    result.Add(reader.GetDecimal(0));
                    result.Add(reader.GetDecimal(1));
                    return result;
                }
            }
        }

        public string BuildAlertValueQuery(string valueField, string valueModification, string filterExpression)
        {
            return $"CALCULATE({valueField},{filterExpression})";
        }

        public string GetAnalysisServiceConnectionString(int modelId, List<string> roles = null)
        {
            var conStrBuilder = new OleDbConnectionStringBuilder(_connectionString)
            {
                ["Catalog"] = modelId.ToString()
            };
            if (roles != null && roles.Count > 0)
            {
                conStrBuilder["Roles"] = string.Join(",", roles);
            }
            return conStrBuilder.ConnectionString;
        }

        public List<Dictionary<string, object>> GetTableData(TableQueryModel queryModel)
        {
            var query = $" EVALUATE TOPN(50,'{queryModel.TableName}' ";
            if (!string.IsNullOrEmpty(queryModel.OrderBy))
            {
                query += $",[{queryModel.OrderBy}]";
                if (queryModel.OrderDesc)
                {
                    query += ", 0";
                }
                else
                {
                    query += ", 1";
                }
            }
            query += ")";

            using (var conn = new AdomdConnection(GetAnalysisServiceConnectionString(queryModel.ModelId)))
            {
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = query;
                using (var reader = command.ExecuteReader())
                {
                    var result = new List<Dictionary<string, object>>();
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();
                        var columns = new List<string>();
                        for (var i = 0; i < reader.FieldCount; ++i)
                        {
                            var name = reader.GetName(i);
                            var si = name.IndexOf("[", StringComparison.Ordinal) + 1;
                            var ei = name.IndexOf("]", StringComparison.Ordinal);
                            columns.Add(name.Substring(si, ei - si));
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

        }

    }
}