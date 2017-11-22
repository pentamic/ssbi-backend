using System.Threading.Tasks;
using Pentamic.SSBI.Services;
using System;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pentamic.SSBI.Services.Breeze;

namespace Pentamic.SSBI.WebApi.Controllers
{
    [Route("breeze/[controller]/[action]")]
    [Authorize]
    public class DiscoverController : Controller
    {
        private readonly DiscoverService _discoverService;
        private readonly DataModelEntityService _dataModelEntityService;

        public DiscoverController(DiscoverService discoverService, DataModelEntityService dataModelEntityService)
        {
            _discoverService = discoverService;
            _dataModelEntityService = dataModelEntityService;
        }

        [HttpPost]
        public async Task<IActionResult> Catalogs([FromBody]TableDiscoverModel model)
        {
            var dataSource = _dataModelEntityService.DataSources
                .Where(x => x.Id == model.DataSourceId)
                .Include(x => x.SourceFile).FirstOrDefault();
            if (dataSource == null)
            {
                return BadRequest();
            }
            var result = await _discoverService.DiscoverCatalogs(dataSource);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Tables([FromBody]TableDiscoverModel model)
        {
            try
            {
                var dataSource = _dataModelEntityService.DataSources
                    .Where(x => x.Id == model.DataSourceId)
                    .Include(x => x.SourceFile).FirstOrDefault();
                if (dataSource == null)
                {
                    return BadRequest();
                }
                var result = await _discoverService.DiscoverTables(dataSource);
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
        public async Task<IActionResult> Columns([FromBody]ColumnDiscoverModel model)
        {
            try
            {
                var dataSource = _dataModelEntityService.DataSources.Where(x => x.Id == model.DataSourceId)
                    .Include(x => x.SourceFile).FirstOrDefault();
                if (dataSource == null)
                {
                    return BadRequest();
                }
                var result = await _discoverService.DiscoverColumns(dataSource, model.TableSchema, model.TableName);
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
        public async Task<IActionResult> Data([FromBody]DataDiscoverModel model)
        {
            try
            {
                var dataSource = _dataModelEntityService.DataSources.Where(x => x.Id == model.DataSourceId)
                    .Include(x => x.SourceFile).FirstOrDefault();
                if (dataSource == null)
                {
                    return BadRequest();
                }
                var result = await _discoverService.DiscoverTable(dataSource, model.TableSchema, model.TableName, model.Query);
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
        public async Task<IActionResult> Relationships([FromBody]RelationshipDiscoverModel model)
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
