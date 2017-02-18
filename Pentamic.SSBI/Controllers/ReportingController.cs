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
