using System.Threading.Tasks;
using Pentamic.SSBI.Services;
using System.Web.Http;
using Pentamic.SSBI.Models.Discover;
using Pentamic.SSBI.Models.DataModel.Objects;
using System;

namespace Pentamic.SSBI.Controllers
{
    [Authorize]
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
        [Route("catalogs/")]
        public async Task<IHttpActionResult> Catalogs([FromBody]TableDiscoverModel model)
        {
            var result = await _discoverService.DiscoverCatalogs(model.DataSourceId);
            return Ok(result);
        }

        [HttpPost]
        [Route("tables")]
        public async Task<IHttpActionResult> Tables([FromBody]TableDiscoverModel model)
        {
            try
            {
                var result = await _discoverService.DiscoverTables(model.DataSourceId);
                return Ok(result);
            }
            catch (Exception e)
            {
                var message = e.Message;
                var ie = e.InnerException;
                while (ie != null)
                {
                    message += " | " + ie.Message;
                    ie = ie.InnerException;
                }
                return BadRequest(message);
            }
        }

        [HttpPost]
        [Route("columns")]
        public async Task<IHttpActionResult> Columns([FromBody]ColumnDiscoverModel model)
        {
            try
            {
                var result = await _discoverService.DiscoverColumns(model.DataSourceId, model.TableSchema, model.TableName);
                return Ok(result);
            }
            catch (Exception e)
            {
                var message = e.Message;
                var ie = e.InnerException;
                while (ie != null)
                {
                    message += " | " + ie.Message;
                    ie = ie.InnerException;
                }
                return BadRequest(message);
            }
        }

        [HttpPost]
        [Route("data")]
        public async Task<IHttpActionResult> Data([FromBody]DataDiscoverModel model)
        {
            try
            {
                var result = await _discoverService.DiscoverTable(model.DataSourceId, model.TableSchema, model.TableName, model.Query);
                return Ok(result);
            }
            catch (Exception e)
            {
                var message = e.Message;
                var ie = e.InnerException;
                while (ie != null)
                {
                    message += " | " + ie.Message;
                    ie = ie.InnerException;
                }
                return BadRequest(message);
            }
        }

        [HttpPost]
        [Route("relationships")]
        public async Task<IHttpActionResult> Relationships([FromBody]RelationshipDiscoverModel model)
        {
            try
            {
                var result = await _discoverService.DiscoverRelationships(model.DataSource, model.FkTableSchema, model.FkTableName);
                return Ok(result);
            }
            catch (Exception e)
            {
                var message = e.Message;
                var ie = e.InnerException;
                while (ie != null)
                {
                    message += " | " + ie.Message;
                    ie = ie.InnerException;
                }
                return BadRequest(message);
            }
        }
    }
}
