using Breeze.ContextProvider;
using Breeze.WebApi2;
using Newtonsoft.Json.Linq;
using Pentamic.SSBI.Models.Reporting;
using Pentamic.SSBI.Models.Reporting.Query;
using Pentamic.SSBI.Services;
using System.Linq;
using System.Web.Http;

namespace Pentamic.SSBI.Controllers
{
    [Authorize]
    [BreezeController]
    public class ReportingController : ApiController
    {
        private readonly ReportingService _reportingService;

        public ReportingController()
        {
            _reportingService = new ReportingService();
        }

        [HttpGet]
        public string Metadata()
        {
            return _reportingService.Metadata;
        }
        [HttpGet]
        public IQueryable<Dashboard> Dashboards()
        {
            return _reportingService.Dashboards;
        }
        [HttpGet]
        public IQueryable<DashboardTile> DashboardTiles()
        {
            return _reportingService.DashboardTiles;
        }
        [HttpGet]
        public IQueryable<Report> Reports()
        {
            return _reportingService.Reports;
        }
        [HttpGet]
        public IQueryable<ReportPage> ReportPages()
        {
            return _reportingService.ReportPages;
        }
        [HttpGet]
        public IQueryable<ReportTile> ReportTiles()
        {
            return _reportingService.ReportTiles;
        }
        [HttpGet]
        public IQueryable<ReportTileRow> ReportTileRows()
        {
            return _reportingService.ReportTileRows;
        }
        [HttpGet]
        public IQueryable<DisplayType> DisplayTypes()
        {
            return _reportingService.DisplayTypes;
        }
        [HttpGet]
        public IQueryable<ReportSharing> ReportSharings()
        {
            return _reportingService.ReportSharings;
        }
        [HttpGet]
        public IQueryable<DashboardSharing> DashboardSharings()
        {
            return _reportingService.DashboardSharings;
        }
        [HttpGet]
        public IQueryable<DashboardComment> DashboardComments()
        {
            return _reportingService.DashboardComments;
        }
        [HttpGet]
        public IQueryable<DashboardView> DashboardViews()
        {
            return _reportingService.DashboardViews;
        }
        [HttpGet]
        public IQueryable<ReportComment> ReportComments()
        {
            return _reportingService.ReportComments;
        }
        [HttpGet]
        public IQueryable<ReportView> ReportViews()
        {
            return _reportingService.ReportViews;
        }
        [HttpGet]
        public IQueryable<UserReportActivity> UserReportActivities()
        {
            return _reportingService.UserReportActivities;
        }
        [HttpGet]
        public IQueryable<UserDashboardActivity> UserDashboardActivities()
        {
            return _reportingService.UserDashboardActivities;
        }

        [HttpGet]
        public IQueryable<UserReportActivity> UserRecentReports()
        {
            return _reportingService.GetUserRecentReports();
        }

        [HttpGet]
        public IQueryable<UserDashboardActivity> UserRecentDashboards()
        {
            return _reportingService.GetUserRecentDashboards();
        }

        [HttpGet]
        public IQueryable<UserFavoriteReport> UserFavoriteReports()
        {
            return _reportingService.UserFavoriteReports;
        }

        [HttpGet]
        public IQueryable<UserFavoriteDashboard> UserFavoriteDashboards()
        {
            return _reportingService.UserFavoriteDashboards;
        }

        [HttpPost]
        public IHttpActionResult FieldValues(FieldQueryModel queryModel)
        {
            var result = _reportingService.GetFieldValues(queryModel);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult Data(QueryModel queryModel)
        {
            var result = _reportingService.Query(queryModel);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult Data2(QueryModel2 queryModel)
        {
            var result = _reportingService.QueryRowTile(queryModel);
            return Ok(result);
        }


        [HttpPost]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return _reportingService.SaveChanges(saveBundle);
        }

    }
}
