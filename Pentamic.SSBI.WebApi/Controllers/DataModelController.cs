using System.IO;
using System.Linq;
using Breeze.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Pentamic.SSBI.Entities;
using Pentamic.SSBI.Services.Breeze;

namespace Pentamic.SSBI.WebApi.Controllers
{
    [Route("breeze/[controller]/[action]")]
    [BreezeQueryFilter]
    public class DataModelController : Controller
    {
        private readonly DataModelEntityService _dataModelEntityService;

        public DataModelController(DataModelEntityService dataModelEntityService)
        {
            _dataModelEntityService = dataModelEntityService;
        }

        [HttpGet]
        public IQueryable<Model> Models()
        {
            return _dataModelEntityService.Models;
        }
        [HttpGet]
        public IQueryable<ModelRole> Roles()
        {
            return _dataModelEntityService.Roles;
        }
        [HttpGet]
        public IQueryable<ModelRoleTablePermission> RoleTablePermissions()
        {
            return _dataModelEntityService.RoleTablePermissions;
        }
        [HttpGet]
        public IQueryable<DataSource> DataSources()
        {
            return _dataModelEntityService.DataSources;
        }
        [HttpGet]
        public IQueryable<Table> Tables()
        {
            return _dataModelEntityService.Tables;
        }
        [HttpGet]
        public IQueryable<Column> Columns()
        {
            return _dataModelEntityService.Columns;
        }
        [HttpGet]
        public IQueryable<Measure> Measures()
        {
            return _dataModelEntityService.Measures;
        }
        [HttpGet]
        public IQueryable<Partition> Partitions()
        {
            return _dataModelEntityService.Partitions;
        }
        [HttpGet]
        public IQueryable<Relationship> Relationships()
        {
            return _dataModelEntityService.Relationships;
        }
        [HttpGet]
        public IQueryable<SourceFile> SourceFiles()
        {
            return _dataModelEntityService.SourceFiles;
        }
        [HttpGet]
        public IQueryable<ModelSharing> ModelSharings()
        {
            return _dataModelEntityService.ModelSharings;
        }
        [HttpGet]
        public IQueryable<UserModelActivity> UserRecentModels()
        {
            return _dataModelEntityService.GetUserRecentModels();
        }
        [HttpGet]
        public IQueryable<UserModelRole> UserRoles()
        {
            return _dataModelEntityService.UserRoles;
        }
        [HttpGet]
        public IQueryable<UserFavoriteModel> UserFavoriteModels()
        {
            return _dataModelEntityService.UserFavoriteModels;
        }


        //[HttpPost]
        //public IActionResult TableData(TableQueryModel queryModel)
        //{
        //    try
        //    {
        //        var res = _dataModelEntityService.GetTableData(queryModel);
        //        return Ok(res);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}

        //[HttpPost]
        //public SaveResult SaveChanges(JObject saveBundle)
        //{
        //    return _dataModelEntityService.SaveChanges(saveBundle);
        //}

        //[HttpPost]
        //public SaveResult SaveImport(JObject saveBundle)
        //{
        //    return _dataModelEntityService.SaveImport(saveBundle);
        //}


        //[HttpPost]
        //public async Task<IActionResult> Import()
        //{
        //    if (!Request.Content.IsMimeMultipartContent())
        //    {
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //    }
        //    var basePath = System.Configuration.ConfigurationManager.AppSettings["UploadBasePath"];
        //    if (string.IsNullOrEmpty(basePath))
        //    {
        //        basePath = HttpContext.Current.Server.MapPath("~/Uploads");
        //    }
        //    if (!Directory.Exists(basePath))
        //    {
        //        Directory.CreateDirectory(basePath);
        //    }
        //    var provider = new MultipartFormDataStreamProvider(basePath);
        //    await Request.Content.ReadAsMultipartAsync(provider);
        //    var sourceFile = await _dataModelEntityService.HandleFileUpload(provider);
        //    return Ok(sourceFile);
        //}

        //[HttpPost]
        //[Route("breeze/datamodel/deploy/{id}")]
        //public IActionResult Deploy(int id)
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

        //[HttpPost]
        //[Route("breeze/datamodel/{modelId}/refresh")]
        //public async Task<IActionResult> RefreshModel(int modelId)
        //{
        //    try
        //    {
        //        _dataModelEntityService.RefreshModel(modelId);
        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        Serilog.Log.Logger.Error(e, e.Message);
        //        return BadRequest(e.Message);
        //    }
        //}

        //[HttpPost]
        //[Route("breeze/datamodel/refreshtable/{tableId}")]
        //public IActionResult RefreshTable(int tableId)
        //{
        //    try
        //    {
        //        _dataModelEntityService.RefreshTable(tableId);
        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        Serilog.Log.Logger.Error(e, e.Message);
        //        return BadRequest(e.Message);
        //    }
        //}


        //[HttpPost]
        //[Route("breeze/datamodel/createdatetable")]
        //public IActionResult CreateDateTable(DateTableCreateModel model)
        //{
        //    try
        //    {
        //        _dataModelEntityService.CreateDateTable(model);
        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        Serilog.Log.Logger.Error(e, e.Message);
        //        return BadRequest(e.Message);
        //    }

        //}

        [HttpPost]
        [Route("breeze/datamodel/export/{modelId}")]
        public IActionResult ExportModel(int modelId)
        {
            var basePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot");
            var path = _dataModelEntityService.ExportModelTemplate(modelId, basePath);
            using (var stream = new FileStream(path, FileMode.Open))
            {
                return File(stream, "");
            }
        }


        //[HttpPost]
        //public async Task<IActionResult> ImportModel(IFormFile file)
        //{
        //    if (file == null) throw new Exception("File is null");
        //    if (file.Length == 0) throw new Exception("File is empty");
        //    //var basePath = Path.Combine(
        //    //    Directory.GetCurrentDirectory(),
        //    //    "wwwroot");
        //    //using (var stream = file.OpenReadStream())
        //    //{
        //    //    using (var binaryReader = new BinaryReader(stream))
        //    //    {
        //    //        var fileContent = binaryReader.ReadBytes((int)file.Length);
        //    //        await _uploadService.AddFile(fileContent, file.FileName, file.ContentType);
        //    //    }
        //    //}

        //    //var basePath = System.Configuration.ConfigurationManager.AppSettings["ImportBasePath"];
        //    //if (string.IsNullOrEmpty(basePath))
        //    //{
        //    //    basePath = HttpContext.Current.Server.MapPath("~/Imports");
        //    //}
        //    //if (!Directory.Exists(basePath))
        //    //{
        //    //    Directory.CreateDirectory(basePath);
        //    //}
        //    //var provider = new MultipartFormDataStreamProvider(basePath);
        //    //await Request.Content.ReadAsMultipartAsync(provider);
        //    //var model = _dataModelEntityService.ImportModel(provider);
        //    //return Ok(model);
        //}


    }
}
