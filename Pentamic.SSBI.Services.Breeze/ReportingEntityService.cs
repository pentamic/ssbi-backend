using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Breeze.Persistence;
using Pentamic.SSBI.Data;
using Pentamic.SSBI.Entities;
using EntityState = Breeze.Persistence.EntityState;

namespace Pentamic.SSBI.Services.Breeze
{
    public class ReportingEntityService
    {
        private readonly DbPersistenceManager<AppDbContext> _persistenceManager;

        private string UserId { get; }

        private string UserName { get; }

        public ReportingEntityService(DbPersistenceManager<AppDbContext> persistenceManager)
        {
            _persistenceManager = persistenceManager;
        }

        private AppDbContext Context => _persistenceManager.Context;

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
        public IQueryable<NotificationSubscription> NotificationSubscriptions => Context.NotificationSubscriptions;
        public IQueryable<UserNotification> UserNotifications => Context.UserNotifications;

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

        public SaveResult SaveChanges(JObject saveBundle)
        {
            var txSettings = new TransactionSettings { TransactionType = TransactionType.TransactionScope };
            _persistenceManager.BeforeSaveEntityDelegate += BeforeSaveEntity;
            _persistenceManager.AfterSaveEntitiesDelegate += AfterSaveEntities;
            return _persistenceManager.SaveChanges(saveBundle, txSettings);
        }

        protected bool BeforeSaveEntity(EntityInfo info)
        {
            if (info.Entity is IAuditable)
            {
                var entity = info.Entity as IAuditable;
                switch (info.EntityState)
                {
                    case EntityState.Added:
                        entity.CreatedAt = DateTimeOffset.Now;
                        entity.CreatedBy = UserId;
                        entity.ModifiedAt = DateTimeOffset.Now;
                        entity.ModifiedBy = UserId;
                        break;
                    case EntityState.Modified:
                        entity.ModifiedAt = DateTimeOffset.Now;
                        entity.ModifiedBy = UserId;
                        break;
                }
            }
            if (info.Entity is IShareInfo)
            {
                var entity = info.Entity as IShareInfo;
                switch (info.EntityState)
                {
                    case EntityState.Added:
                        entity.SharedAt = DateTimeOffset.Now;
                        entity.SharedBy = UserId;
                        break;
                    case EntityState.Modified:
                        entity.SharedAt = DateTimeOffset.Now;
                        break;
                }
            }
            if (info.Entity is UserFavoriteDashboard && info.EntityState == EntityState.Added)
            {
                var entity = info.Entity as UserFavoriteDashboard;
                entity.UserId = UserId;
            }
            if (info.Entity is UserFavoriteReport && info.EntityState == EntityState.Added)
            {
                var entity = info.Entity as UserFavoriteReport;
                entity.UserId = UserId;
            }
            if (info.Entity is UserReportActivity && info.EntityState == EntityState.Added)
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
            if (saveMap.TryGetValue(typeof(Report), out var eis))
            {
                foreach (var ei in eis)
                {
                    if (ei.EntityState == EntityState.Modified && ei.OriginalValuesMap.ContainsKey("ModelId"))
                    {
                        var e = ei.Entity as Report;
                        var reportTiles = Context.ReportTiles.Where(x => x.ReportPage.ReportId == e.Id).ToList();
                        foreach (var tile in reportTiles)
                        {
                            if (e != null) tile.ModelId = e.ModelId;
                        }
                        Context.SaveChanges();
                    }
                }
            }
        }
    }
}