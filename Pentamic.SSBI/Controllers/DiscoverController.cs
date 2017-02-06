using System.Threading.Tasks;
using Pentamic.SSBI.Services;
using System.Web.Http;
using Pentamic.SSBI.Models.Discover;

namespace Pentamic.SSBI.Controllers
{
    [RoutePrefix("api/discover")]
    public class DiscoverController : ApiController
    {
        private DiscoverService _discoverService;

        public DiscoverController()
        {
            _discoverService = new DiscoverService();
        }

        [HttpPost]
        [Route("tables")]
        public async Task<IHttpActionResult> Tables([FromBody]TableSchemaRestriction restrictions)
        {
            var result = await _discoverService.DiscoverTables(restrictions);
            return Ok(result);
        }

        [HttpPost]
        [Route("columns")]
        public async Task<IHttpActionResult> Columns([FromBody]ColumnSchemaRestriction restrictions)
        {
            var result = await _discoverService.DiscoverColumns(restrictions);
            return Ok(result);
        }

        [HttpPost]
        [Route("relationships")]
        public async Task<IHttpActionResult> Relationships([FromBody]ForeignKeySchemaRestriction restrictions)
        {
            var result = await _discoverService.DiscoverRelationships(restrictions);
            return Ok(result);
        }
    }
}
