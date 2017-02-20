using Breeze.ContextProvider;
using Breeze.WebApi2;
using Newtonsoft.Json.Linq;
using Pentamic.SSBI.Models.DataModel;
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
    public class DataModelController : ApiController
    {
        private DataModelService _dataModelService;

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
            return _dataModelService.Models;
        }
        [HttpGet]
        public IQueryable<DataSource> DataSources()
        {
            return _dataModelService.DataSources;
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
            string root = HttpContext.Current.Server.MapPath("~/Uploads");
            var provider = new MultipartFormDataStreamProvider(root);
            await Request.Content.ReadAsMultipartAsync(provider);
            var sourceFileId = await _dataModelService.HandleFileUpload(provider);
            return Ok(sourceFileId);
        }

        [HttpPost]
        [Route("breeze/datamodel/deploy/{id}")]
        public IHttpActionResult Deploy(int id)
        {
            _dataModelService.DeployModel(id);
            return Ok();
        }

        [HttpPost]
        [Route("breeze/datamodel/refresh/{id}")]
        public IHttpActionResult Refresh(int id)
        {
            _dataModelService.RefreshModel(id);
            return Ok();
        }
    }
}
