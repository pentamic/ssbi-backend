using System.Collections.Generic;
using System.Linq;
using Breeze.AspNetCore;
using Breeze.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Pentamic.SSBI.Entities;
using Pentamic.SSBI.Services.Breeze;
using Pentamic.SSBI.Services.SSAS.Query;

namespace Pentamic.SSBI.WebApi.Controllers
{
    [Route("breeze/[controller]/[action]")]
    [BreezeQueryFilter]
    [Authorize]
    public class ReportingController : Controller
    {
        private readonly ReportingEntityService _reportingEntityService;
        private readonly QueryService _queryService;

        public ReportingController(ReportingEntityService reportingEntityService, QueryService queryService)
        {
            _reportingEntityService = reportingEntityService;
            _queryService = queryService;
        }

        [HttpGet]
        public IQueryable<Dashboard> Dashboards()
        {
            return _reportingEntityService.Dashboards;
        }
        [HttpGet]
        public IQueryable<DashboardTile> DashboardTiles()
        {
            return _reportingEntityService.DashboardTiles;
        }
        [HttpGet]
        public IQueryable<Report> Reports()
        {
            return _reportingEntityService.Reports;
        }
        [HttpGet]
        public IQueryable<ReportPage> ReportPages()
        {
            return _reportingEntityService.ReportPages;
        }
        [HttpGet]
        public IQueryable<ReportTile> ReportTiles()
        {
            return _reportingEntityService.ReportTiles;
        }
        [HttpGet]
        public IQueryable<ReportTileRow> ReportTileRows()
        {
            return _reportingEntityService.ReportTileRows;
        }
        [HttpGet]
        public IQueryable<DisplayType> DisplayTypes()
        {
            return _reportingEntityService.DisplayTypes;
        }
        [HttpGet]
        public IQueryable<ReportSharing> ReportSharings()
        {
            return _reportingEntityService.ReportSharings;
        }
        [HttpGet]
        public IQueryable<DashboardSharing> DashboardSharings()
        {
            return _reportingEntityService.DashboardSharings;
        }
        [HttpGet]
        public IQueryable<DashboardComment> DashboardComments()
        {
            return _reportingEntityService.DashboardComments;
        }
        [HttpGet]
        public IQueryable<DashboardView> DashboardViews()
        {
            return _reportingEntityService.DashboardViews;
        }
        [HttpGet]
        public IQueryable<ReportComment> ReportComments()
        {
            return _reportingEntityService.ReportComments;
        }
        [HttpGet]
        public IQueryable<ReportView> ReportViews()
        {
            return _reportingEntityService.ReportViews;
        }
        [HttpGet]
        public IQueryable<UserReportActivity> UserReportActivities()
        {
            return _reportingEntityService.UserReportActivities;
        }
        [HttpGet]
        public IQueryable<UserDashboardActivity> UserDashboardActivities()
        {
            return _reportingEntityService.UserDashboardActivities;
        }
        [HttpGet]
        public IQueryable<Alert> Alerts()
        {
            return _reportingEntityService.Alerts;
        }
        [HttpGet]
        public IQueryable<Notification> Notifications()
        {
            return _reportingEntityService.Notifications;
        }
        [HttpGet]
        public IQueryable<NotificationSubscription> NotificationSubscriptions()
        {
            return _reportingEntityService.NotificationSubscriptions;
        }
        [HttpGet]
        public IQueryable<UserNotification> UserNotifications()
        {
            return _reportingEntityService.UserNotifications;
        }
        [HttpGet]
        public IQueryable<UserReportActivity> UserRecentReports()
        {
            return _reportingEntityService.GetUserRecentReports();
        }

        [HttpGet]
        public IQueryable<UserDashboardActivity> UserRecentDashboards()
        {
            return _reportingEntityService.GetUserRecentDashboards();
        }

        [HttpGet]
        public IQueryable<UserFavoriteReport> UserFavoriteReports()
        {
            return _reportingEntityService.UserFavoriteReports;
        }

        [HttpGet]
        public IQueryable<UserFavoriteDashboard> UserFavoriteDashboards()
        {
            return _reportingEntityService.UserFavoriteDashboards;
        }

        [HttpPost]
        public IActionResult FieldValues([FromBody]FieldQueryModel queryModel)
        {
            var result = _queryService.GetFieldValues(queryModel);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult ProcessAlert()
        {
            _queryService.ProcessAlert(new List<Alert>());
            return Ok();
        }

        [HttpPost]
        public IActionResult Data([FromBody]QueryModel queryModel)
        {
            var result = _queryService.Query(queryModel);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Data2([FromBody]QueryModel2 queryModel)
        {
            var result = _queryService.QueryRowTile(new List<ReportTileRow>(), queryModel);
            return Ok(result);
        }

        [HttpPost]
        public SaveResult SaveChanges([FromBody]JObject saveBundle)
        {
            return _reportingEntityService.SaveChanges(saveBundle);
        }

    }
}
