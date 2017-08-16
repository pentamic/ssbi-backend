using Breeze.ContextProvider;
using Breeze.WebApi2;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using Pentamic.SSBI.Models.Reporting;
using Pentamic.SSBI.Models.Reporting.Query;
using Pentamic.SSBI.Services;
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace Pentamic.SSBI.Controllers
{
    //[Authorize]
    [BreezeController]
    public class ReportingController : ApiController
    {
        private ReportingService _reportingService;

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
        public IQueryable<DisplayType> DisplayTypes()
        {
            return _reportingService.DisplayTypes;
        }

        [HttpPost]
        public IHttpActionResult Data(QueryModel queryModel)
        {
            var result = _reportingService.Query(queryModel);
            return Ok(result);
        }

        [HttpPost]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return _reportingService.SaveChanges(saveBundle);
        }

    }
}
