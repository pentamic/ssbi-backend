using Breeze.ContextProvider;
using Breeze.ContextProvider.EF6;
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
using System.Linq;
using System.Security.Claims;
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

        public IQueryable<Dashboard> Dashboards
        {
            get
            {
                return Context.Dashboards.Where(x => x.CreatedBy == UserId)
               .Concat(Context.DashboardSharings.Where(x => x.UserId == UserId).Select(x => x.Dashboard));
            }
        }
        public IQueryable<DashboardTile> DashboardTiles => Context.DashboardTiles;

        public IQueryable<Report> Reports
        {
            get
            {
                return Context.Reports.Where(x => x.CreatedBy == UserId)
                    .Concat(Context.ReportSharings.Where(x => x.UserId == UserId).Select(x => x.Report));
            }
        }
        public IQueryable<ReportPage> ReportPages => Context.ReportPages;

        public IQueryable<ReportTile> ReportTiles
        {
            get { return Context.ReportTiles.Where(x => Reports.Select(y => y.Id).Contains(x.ReportId)); }
        }
        public IQueryable<DisplayType> DisplayTypes => Context.DisplayTypes;

        public IQueryable<ReportSharing> ReportSharings => Context.ReportSharings;

        public IQueryable<DashboardSharing> DashboardSharings => Context.DashboardSharings;

        public IQueryable<ReportComment> ReportComments => Context.ReportComments;

        public IQueryable<ReportView> ReportViews => Context.ReportViews;

        public IQueryable<DashboardComment> DashboardComments => Context.DashboardComments;

        public IQueryable<DashboardView> DashboardViews => Context.DashboardViews;

        public IQueryable<UserReportActivity> UserReportActivities
        {
            get { return Context.UserReportActivities.Where(x => x.UserId == UserId); }
        }
        public IQueryable<UserDashboardActivity> UserDashboardActivities
        {
            get { return Context.UserDashboardActivities.Where(x => x.UserId == UserId); }
        }
        public IQueryable<UserFavoriteReport> UserFavoriteReports
        {
            get { return Context.UserFavoriteReports.Where(x => x.UserId == UserId); }
        }
        public IQueryable<UserFavoriteDashboard> UserFavoriteDashboards
        {
            get { return Context.UserFavoriteDashboards.Where(x => x.UserId == UserId); }
        }

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
            var conStrBuilder = new OleDbConnectionStringBuilder(_asConnectionString)
            {
                ["Catalog"] = model.DatabaseName
            };
            using (var conn = new AdomdConnection(conStrBuilder.ToString()))
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

        //protected Dictionary<Type, List<EntityInfo>> BeforeSaveEntities(Dictionary<Type, List<EntityInfo>> saveMap)
        //{    
        //    return saveMap;
        //}


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
    }
}