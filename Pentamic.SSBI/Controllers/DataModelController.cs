using Breeze.ContextProvider;
using Breeze.WebApi2;
using Newtonsoft.Json.Linq;
using Pentamic.SSBI.Models.DataModel;
using Pentamic.SSBI.Models.DataModel.Objects;
using Pentamic.SSBI.Services;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Pentamic.SSBI.Models.DataModel.Connections;

namespace Pentamic.SSBI.Controllers
{
    [Authorize]
    [BreezeController]
    public class DataModelController : ApiController
    {
        private readonly DataModelService _dataModelService;

        public DataModelController()
        {
            _dataModelService = new DataModelService();
        }

        [HttpGet]
        public string Metadata()
        {
            return _dataModelService.Metadata;
        }

        [HttpGet]
        public IQueryable<Model> Models()
        {
            //_dataModelService.FixModelColumns(1068);
            return _dataModelService.Models;
        }
        [HttpGet]
        public IQueryable<Role> Roles()
        {
            return _dataModelService.Roles;
        }
        [HttpGet]
        public IQueryable<RoleTablePermission> RoleTablePermissions()
        {
            return _dataModelService.RoleTablePermissions;
        }
        [HttpGet]
        public IQueryable<DataSource> DataSources()
        {
            return _dataModelService.DataSources;
        }
        [HttpGet]
        public IQueryable<Connection> Connections()
        {
            return _dataModelService.Connections;
        }
        [HttpGet]
        public IQueryable<ModelConnection> ModelConnections()
        {
            return _dataModelService.ModelConnections;
        }
        [HttpGet]
        public IQueryable<SqlServerConnection> SqlServerConnections()
        {
            return _dataModelService.SqlServerConnections;
        }
        [HttpGet]
        public IQueryable<ExcelConnection> ExcelConnections()
        {
            return _dataModelService.ExcelConnections;
        }
        [HttpGet]
        public IQueryable<Table> Tables()
        {
            return _dataModelService.Tables;
        }
        [HttpGet]
        public IQueryable<Column> Columns()
        {
            return _dataModelService.Columns;
        }
        [HttpGet]
        public IQueryable<Measure> Measures()
        {
            return _dataModelService.Measures;
        }
        [HttpGet]
        public IQueryable<Partition> Partitions()
        {
            return _dataModelService.Partitions;
        }
        [HttpGet]
        public IQueryable<Relationship> Relationships()
        {
            return _dataModelService.Relationships;
        }
        [HttpGet]
        public IQueryable<SourceFile> SourceFiles()
        {
            return _dataModelService.SourceFiles;
        }
        [HttpGet]
        public IQueryable<ModelSharing> ModelSharings()
        {
            return _dataModelService.ModelSharings;
        }
        [HttpGet]
        public IQueryable<UserModelActivity> UserRecentModels()
        {
            return _dataModelService.GetUserRecentModels();
        }
        [HttpGet]
        public IQueryable<UserFavoriteModel> UserFavoriteModels()
        {
            return _dataModelService.UserFavoriteModels;
        }


        [HttpPost]
        public IHttpActionResult TableData(TableQueryModel queryModel)
        {
            try
            {
                var res = _dataModelService.GetTableData(queryModel);
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return _dataModelService.SaveChanges(saveBundle);
        }

        [HttpPost]
        public async Task<IHttpActionResult> Import()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            var basePath = System.Configuration.ConfigurationManager.AppSettings["UploadBasePath"];
            if (string.IsNullOrEmpty(basePath))
            {
                basePath = HttpContext.Current.Server.MapPath("~/Uploads");
            }
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            var provider = new MultipartFormDataStreamProvider(basePath);
            await Request.Content.ReadAsMultipartAsync(provider);
            var sourceFile = await _dataModelService.HandleFileUpload(provider);
            return Ok(sourceFile);
        }

        //[HttpPost]
        //[Route("breeze/datamodel/deploy/{id}")]
        //public IHttpActionResult Deploy(int id)
        //{
        //    try
        //    {
        //        _dataModelService.DeployModel(id);
        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        Serilog.Log.Logger.Error(e, e.Message);
        //        return BadRequest(e.Message);
        //    }

        //}

        [HttpPost]
        [Route("breeze/datamodel/{modelId}/refresh")]
        public IHttpActionResult RefreshModel(int modelId)
        {
            try
            {
                _dataModelService.EnqueueRefreshModel(modelId);
                return Ok();
            }
            catch (Exception e)
            {
                Serilog.Log.Logger.Error(e, e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("breeze/datamodel/createdatetable")]
        public IHttpActionResult CreateDateTable(DateTableCreateModel model)
        {
            try
            {
                _dataModelService.CreateDateTable(model);
                return Ok();
            }
            catch (Exception e)
            {
                Serilog.Log.Logger.Error(e, e.Message);
                return BadRequest(e.Message);
            }

        }

    }
}
