﻿using Breeze.ContextProvider;
using Breeze.ContextProvider.EF6;
using Ciloci.Flee;
using Microsoft.AnalysisServices.AdomdClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pentamic.SSBI.Models;
using Pentamic.SSBI.Models.DataModel;
using Pentamic.SSBI.Models.Reporting;
using Pentamic.SSBI.Models.Reporting.Query;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.OleDb;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web;

namespace Pentamic.SSBI.Services
{
    public class ReportingService
    {
        private readonly EFContextProvider<ReportingContext> _contextProvider;
        private readonly string _asConnectionString = System.Configuration.ConfigurationManager
                .ConnectionStrings["AnalysisServiceConnection"]
                .ConnectionString;
        private string _userId;
        private string _userName;

        private string UserId
        {
            get
            {
                return _userId ?? (_userId = (HttpContext.Current.User.Identity as ClaimsIdentity)?.Claims
                           .First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                           .Value);
            }
        }

        private string UserName
        {
            get
            {
                if (_userName == null)
                {
                    _userName = HttpContext.Current.User.Identity.Name;
                }
                return _userName;
            }
        }

        public ReportingService()
        {
            _contextProvider = new EFContextProvider<ReportingContext>();
        }

        private ReportingContext Context => _contextProvider.Context;

        public string Metadata => _contextProvider.Metadata();

        public IQueryable<Dashboard> Dashboards => Context.Dashboards.Where(x => x.CreatedBy == UserId)
               .Concat(Context.DashboardSharings.Where(x => x.UserId == UserId).Select(x => x.Dashboard));
        public IQueryable<DashboardTile> DashboardTiles => Context.DashboardTiles;
        public IQueryable<Report> Reports => Context.Reports.Where(x => x.CreatedBy == UserId)
                    .Concat(Context.ReportSharings.Where(x => x.UserId == UserId).Select(x => x.Report));
        public IQueryable<ReportPage> ReportPages => Context.ReportPages;
        public IQueryable<ReportTile> ReportTiles => Context.ReportTiles.Where(x => Reports.Select(y => y.Id).Contains(x.ReportId));
        public IQueryable<ReportTileRow> ReportTileRows => Context.ReportTileRows.Where(x => ReportTiles.Select(y => y.Id).Contains(x.ReportTileId));
        public IQueryable<DisplayType> DisplayTypes => Context.DisplayTypes;
        public IQueryable<ReportSharing> ReportSharings => Context.ReportSharings;
        public IQueryable<DashboardSharing> DashboardSharings => Context.DashboardSharings;
        public IQueryable<ReportComment> ReportComments => Context.ReportComments;
        public IQueryable<ReportView> ReportViews => Context.ReportViews;
        public IQueryable<DashboardComment> DashboardComments => Context.DashboardComments;
        public IQueryable<DashboardView> DashboardViews => Context.DashboardViews;
        public IQueryable<UserReportActivity> UserReportActivities => Context.UserReportActivities.Where(x => x.UserId == UserId);
        public IQueryable<UserDashboardActivity> UserDashboardActivities => Context.UserDashboardActivities.Where(x => x.UserId == UserId);
        public IQueryable<UserFavoriteReport> UserFavoriteReports => Context.UserFavoriteReports.Where(x => x.UserId == UserId);
        public IQueryable<UserFavoriteDashboard> UserFavoriteDashboards => Context.UserFavoriteDashboards.Where(x => x.UserId == UserId);
        public IQueryable<Alert> Alerts => Context.Alerts;
        public IQueryable<Notification> Notifications => Context.Notifications;
        public IQueryable<NotificationSubscription> NotificationSubscriptions => Context.NotificationSubscriptions
            .Where(x => x.UserId == UserId);
        public IQueryable<UserNotification> UserNotifications => Context.UserNotifications
            .Where(x => x.UserId == UserId);

        public IQueryable<UserReportActivity> GetUserRecentReports()
        {
            return Context.UserReportActivities
                .GroupBy(x => x.ReportId)
                .OrderByDescending(x => x.Max(y => y.CreatedAt))
                .Take(10)
                .Select(x => x.OrderByDescending(y => y.CreatedAt).FirstOrDefault())
                .Include(x => x.Report);
        }
        public IQueryable<UserDashboardActivity> GetUserRecentDashboards()
        {
            return Context.UserDashboardActivities
                .GroupBy(x => x.DashboardId)
                .OrderByDescending(x => x.Max(y => y.CreatedAt))
                .Take(10)
                .Select(x => x.OrderByDescending(y => y.CreatedAt).FirstOrDefault())
                .Include(x => x.Dashboard);
        }


        //public bool CheckUserModelPermission(int modelId)
        //{
        //    var dataModelContext = new DataModelContext();
        //    var model = dataModelContext.Models.Find(modelId);
        //    if (model == null)
        //    {
        //        throw new Exception("Model not found");
        //    }
        //    if (model.CreatedBy == UserId)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return dataModelContext.ModelSharings.Any(x => x.UserId == UserId);
        //    }
        //}
        //public bool CheckUserModelPermission(Models.DataModel.Objects.Model model)
        //{
        //    if (model.CreatedBy == UserId)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        var dataModelContext = new DataModelContext();
        //        return dataModelContext.ModelSharings.Any(x => x.UserId == UserId);
        //    }
        //}

        public void UpdateReportDataConfigColumnName(int modelId, string oldName, string newName)
        {
            var reportTiles = Context.ReportTiles.Where(x => x.ModelId == modelId).ToList();
            foreach (var tile in reportTiles)
            {
                if (!string.IsNullOrEmpty(tile.DataConfig))
                {
                    var dataConfig = JArray.Parse(tile.DataConfig);
                    foreach (JObject field in dataConfig)
                    {
                        if (field.GetValue("name").ToString() == oldName)
                        {
                            field["name"] = newName;
                        }
                    }
                    tile.DataConfig = JsonConvert.SerializeObject(dataConfig);
                }
            }
            Context.SaveChanges();
        }
        public void UpdateReportDataConfigTableName(int modelId, string oldName, string newName)
        {
            var reportTiles = Context.ReportTiles.Where(x => x.ModelId == modelId).ToList();
            foreach (var tile in reportTiles)
            {
                if (!string.IsNullOrEmpty(tile.DataConfig))
                {
                    var dataConfig = JArray.Parse(tile.DataConfig);
                    foreach (JObject field in dataConfig)
                    {
                        if (field.GetValue("tableName").ToString() == oldName)
                        {
                            field["tableName"] = newName;
                        }
                    }
                    tile.DataConfig = JsonConvert.SerializeObject(dataConfig);
                }
            }
            Context.SaveChanges();
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

        public SaveResult SaveChanges(JObject saveBundle)
        {
            var txSettings = new TransactionSettings { TransactionType = TransactionType.TransactionScope };
            _contextProvider.BeforeSaveEntityDelegate += BeforeSaveEntity;
            _contextProvider.AfterSaveEntitiesDelegate += AfterSaveEntities;
            return _contextProvider.SaveChanges(saveBundle, txSettings);
        }

        protected bool BeforeSaveEntity(EntityInfo info)
        {
            if (info.Entity is IAuditable)
            {
                var entity = info.Entity as IAuditable;
                switch (info.EntityState)
                {
                    case Breeze.ContextProvider.EntityState.Added:
                        entity.CreatedAt = DateTimeOffset.Now;
                        entity.CreatedBy = UserId;
                        entity.ModifiedAt = DateTimeOffset.Now;
                        entity.ModifiedBy = UserId;
                        break;
                    case Breeze.ContextProvider.EntityState.Modified:
                        entity.ModifiedAt = DateTimeOffset.Now;
                        entity.ModifiedBy = UserId;
                        break;
                    default:
                        break;
                }
            }
            if (info.Entity is IShareInfo)
            {
                var entity = info.Entity as IShareInfo;
                switch (info.EntityState)
                {
                    case Breeze.ContextProvider.EntityState.Added:
                        entity.SharedAt = DateTimeOffset.Now;
                        entity.SharedBy = UserId;
                        break;
                    case Breeze.ContextProvider.EntityState.Modified:
                        entity.SharedAt = DateTimeOffset.Now;
                        break;
                    default:
                        break;
                }
            }
            if (info.Entity is UserFavoriteDashboard && info.EntityState == Breeze.ContextProvider.EntityState.Added)
            {
                var entity = info.Entity as UserFavoriteDashboard;
                entity.UserId = UserId;
            }
            if (info.Entity is UserFavoriteReport && info.EntityState == Breeze.ContextProvider.EntityState.Added)
            {
                var entity = info.Entity as UserFavoriteReport;
                entity.UserId = UserId;
            }
            if (info.Entity is UserReportActivity && info.EntityState == Breeze.ContextProvider.EntityState.Added)
            {
                var entity = info.Entity as UserReportActivity;
                entity.UserId = UserId;
            }
            return true;
        }

        protected Dictionary<Type, List<EntityInfo>> BeforeSaveEntities(Dictionary<Type, List<EntityInfo>> saveMap)
        {
            if (saveMap.TryGetValue(typeof(Alert), out List<EntityInfo> eis))
            {
                foreach (var ei in eis)
                {
                    if (ei.EntityState == Breeze.ContextProvider.EntityState.Added)
                    {
                        var e = ei.Entity as Alert;
                        var n = new Notification
                        {
                            Title = e.Name,
                            Content = e.Name,
                            CreatedAt = DateTimeOffset.Now,
                            CreatedBy = UserId,
                            ModifiedAt = DateTimeOffset.Now,
                            ModifiedBy = UserId
                        };
                        e.Notification = n;
                        var notificationEntity = _contextProvider.CreateEntityInfo(n, Breeze.ContextProvider.EntityState.Added);
                        if (!saveMap.TryGetValue(typeof(Notification), out List<EntityInfo> notifications))
                        {
                            notifications = new List<EntityInfo>();
                            saveMap.Add(typeof(Notification), notifications);
                        }
                        notifications.Add(notificationEntity);
                    }
                    if (ei.EntityState == Breeze.ContextProvider.EntityState.Modified)
                    {
                        var e = ei.Entity as Alert;
                        var n = e.Notification ?? new Notification
                        {

                        };
                        var ne = _contextProvider.CreateEntityInfo(n, Breeze.ContextProvider.EntityState.Modified);
                        if (!saveMap.TryGetValue(typeof(Notification), out List<EntityInfo> notifications))
                        {
                            notifications = new List<EntityInfo>();
                            saveMap.Add(typeof(Notification), notifications);
                        }
                        notifications.Add(ne);
                    }
                    if (ei.EntityState == Breeze.ContextProvider.EntityState.Deleted)
                    {
                        var e = ei.Entity as Alert;
                        var n = e.Notification ?? new Notification
                        {
                            Id = e.NotificationId
                        };
                        var ne = _contextProvider.CreateEntityInfo(n, Breeze.ContextProvider.EntityState.Deleted);
                        if (!saveMap.TryGetValue(typeof(Notification), out List<EntityInfo> notifications))
                        {
                            notifications = new List<EntityInfo>();
                            saveMap.Add(typeof(Notification), notifications);
                        }
                        notifications.Add(ne);
                    }
                }
            }
            return saveMap;
        }


        public List<Dictionary<string, object>> Query(QueryModel queryModel)
        {
            var dmContext = new DataModelContext();
            var model = dmContext.Models.Find(queryModel.ModelId);
            if (model == null)
            {
                throw new Exception("Model not found");
            }
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


        public List<ReportTileRowQueryResult> QueryRowTile(QueryModel2 queryModel)
        {
            var tile = Context.ReportTiles.Find(queryModel.TileId);
            if (tile == null)
            {
                throw new Exception("Report Tile not found");
            }
            var dms = new DataModelService();
            var tileRows = Context.ReportTileRows.Where(x => x.ReportTileId == queryModel.TileId).OrderBy(x => x.Ordinal).ToList();
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
            using (var conn = new AdomdConnection(GetAnalysisServiceConnectionString(tile.ModelId)))
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
                                result[i].MTD = mtdVal == null ? 0 : (decimal)mtdVal;
                                result[i].YTD = ytdVal == null ? 0 : (decimal)ytdVal;
                                result[i].PMTD = pmtdVal == null ? 0 : (decimal)pmtdVal;
                                result[i].PYTD = pytdVal == null ? 0 : (decimal)pytdVal;
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
            var dmContext = new DataModelContext();
            var model = dmContext.Models.Find(queryModel.ModelId);
            if (model == null)
            {
                throw new Exception("Model not found");
            }
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
                    if (eo.IsFormula && !eo.IsCalculated)
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
            var dmContext = new DataModelContext();
            var model = dmContext.Models.Find(queryModel.ModelId);
            if (model == null)
            {
                throw new Exception("Model not found");
            }
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
            var dmContext = new DataModelContext();
            var model = dmContext.Models.Find(queryModel.ModelId);
            if (model == null)
            {
                throw new Exception("Model not found");
            }
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


        protected void AfterSaveEntities(Dictionary<Type, List<EntityInfo>> saveMap, List<KeyMapping> keyMappings)
        {
            List<EntityInfo> eis;
            if (saveMap.TryGetValue(typeof(Report), out eis))
            {
                foreach (var ei in eis)
                {
                    var e = ei.Entity as Report;
                    if (ei.EntityState == Breeze.ContextProvider.EntityState.Modified && ei.OriginalValuesMap.ContainsKey("ModelId"))
                    {
                        var reportTiles = Context.ReportTiles.Where(x => x.ReportPage.ReportId == e.Id).ToList();
                        foreach (var tile in reportTiles)
                        {
                            tile.ModelId = e.ModelId;
                        }
                        Context.SaveChanges();
                    }
                }
            }
        }


        public void ProcessAlert()
        {
            var alerts = Context.Alerts.Where(x => x.IsActive).ToList();
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

        public string GetAnalysisServiceConnectionString(int modelId)
        {
            var conStrBuilder = new OleDbConnectionStringBuilder(_asConnectionString)
            {
                ["Catalog"] = modelId.ToString()
            };
            var context = new DataModelContext();
            var userRoles = context.UserRoles.Where(x => x.UserId == UserId).Select(x => x.RoleId);
            var roles = context.Roles.Where(x => x.ModelId == modelId && userRoles.Contains(x.Id)).Select(x => x.Id).ToList();
            if (roles != null && roles.Count > 0)
            {
                conStrBuilder["Roles"] = string.Join(",", roles);
            }
            return conStrBuilder.ConnectionString;
        }
    }
}