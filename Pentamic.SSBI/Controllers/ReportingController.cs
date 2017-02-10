using Breeze.ContextProvider;
using Breeze.WebApi2;
using Newtonsoft.Json.Linq;
using Pentamic.SSBI.Models.DataModel;
using Pentamic.SSBI.Models.Reporting;
using Pentamic.SSBI.Services;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
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
        [Route("breeze/reporting/data/{id}")]
        public IHttpActionResult Data(int id)
        {
            var result = _reportingService.GetReportTileData(id);
            return Ok(result);
        }

        [HttpPost]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return _reportingService.SaveChanges(saveBundle);
        }

    }
}
