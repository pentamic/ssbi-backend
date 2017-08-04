﻿using Breeze.ContextProvider;
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

namespace Pentamic.SSBI.Controllers
{
    //[Authorize]
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
        [HttpGet]
        public IQueryable<SourceFile> SourceFiles()
        {
            return _dataModelService.SourceFiles;
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
        [Route("breeze/datamodel/generate/{id}")]
        public IHttpActionResult Generate(int id)
        {
            try
            {
                _dataModelService.GenerateModelFromDatabase(id);
                return Ok();
            }
            catch (Exception e)
            {
                Serilog.Log.Logger.Error(e, e.Message);
                return BadRequest(e.Message);
            }

        }


        [HttpPost]
        [Route("breeze/datamodel/update/{id}")]
        public IHttpActionResult Update(int id)
        {
            try
            {
                _dataModelService.Update(id);
                return Ok();
            }
            catch (Exception e)
            {
                Serilog.Log.Logger.Error(e, e.Message);
                return BadRequest(e.Message);
            }

        }

        [HttpPost]
        [Route("breeze/datamodel/refresh/{id}")]
        public IHttpActionResult Refresh(int id)
        {
            try
            {
                _dataModelService.RefreshModel(id);
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
