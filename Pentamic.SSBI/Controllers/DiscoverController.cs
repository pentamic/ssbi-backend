using System.Threading.Tasks;
using Pentamic.SSBI.Services;
using System.Web.Http;
using Pentamic.SSBI.Models.Discover;
using System;
using Breeze.WebApi2;

namespace Pentamic.SSBI.Controllers
{
    [Authorize]
    [BreezeController]
    public class DiscoverController : ApiController
    {
        private readonly DiscoverService _discoverService;

        public DiscoverController()
        {
            _discoverService = new DiscoverService();
        }

        [HttpPost]
        public IHttpActionResult Providers()
        {
            try
            {
                var result = _discoverService.GetDataProviders();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpPost]
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
        public async Task<IHttpActionResult> Catalogs(CatalogDiscoverModel model)
        {
            var result = await _discoverService.GetCatalogs(model.ConnectionId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Tables(TableDiscoverModel model)
        {
            var result = await _discoverService.GetTables(model.ConnectionId, model.CatalogName);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Columns(ColumnDiscoverModel model)
        {
            var result = await _discoverService.DiscoverColumns(model.DataSourceId, model.TableSchema, model.TableName);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Data(DataDiscoverModel model)
        {
            var result = await _discoverService.GetTableInfo(model.DataSourceId, model.TableSchema, model.TableName, model.Query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Relationships(RelationshipDiscoverModel model)
        {
            //var result = await _discoverService.DiscoverRelationships(model.DataSource, model.FkTableSchema, model.FkTableName);
            return Ok();
        }
    }
}
