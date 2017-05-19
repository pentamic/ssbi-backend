using System.Threading.Tasks;
using Pentamic.SSBI.Services;
using System.Web.Http;
using Pentamic.SSBI.Models.Discover;
using Pentamic.SSBI.Models.DataModel.Objects;
using System;

namespace Pentamic.SSBI.Controllers
{
    //[Authorize]
    [RoutePrefix("api/discover")]
    public class DiscoverController : ApiController
    {
        private DiscoverService _discoverService;

        public DiscoverController()
        {
            _discoverService = new DiscoverService();
        }

        [HttpGet]
        [Route("model")]
        public IHttpActionResult Model(int modelId, string perspective = null)
        {
            try
            {
                var result = _discoverService.DiscoverModel(modelId, perspective);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPost]
        [Route("catalogs")]
        public async Task<IHttpActionResult> Catalogs([FromBody]DataSource ds)
        {
            var result = await _discoverService.DiscoverCatalogs(ds);
            return Ok(result);
        }

        [HttpPost]
        [Route("tables")]
        public async Task<IHttpActionResult> Tables([FromBody]DataSource ds)
        {
            var result = await _discoverService.DiscoverTables(ds);
            return Ok(result);
        }

        [HttpPost]
        [Route("data")]
        public async Task<IHttpActionResult> Data([FromBody]DataDiscoverModel model)
        {
            var result = await _discoverService.DiscoverTableData(model.DataSource, model.TableSchema, model.TableName);
            return Ok(result);
        }

        [HttpPost]
        [Route("columns")]
        public async Task<IHttpActionResult> Columns([FromBody]ColumnDiscoverModel model)
        {
            var result = await _discoverService.DiscoverColumns(model.DataSource, model.TableSchema, model.TableName);
            return Ok(result);
        }

        [HttpPost]
        [Route("relationships")]
        public async Task<IHttpActionResult> Relationships([FromBody]RelationshipDiscoverModel model)
        {
            var result = await _discoverService.DiscoverRelationships(model.DataSource, model.FkTableSchema, model.FkTableName);
            return Ok(result);
        }
    }
}
