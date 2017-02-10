using System;
using Pentamic.SSBI.Models;
using Microsoft.AnalysisServices.AdomdClient;
using System.Text;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using System.Web.Http.ModelBinding;
using System.Collections.Generic;

namespace Pentamic.SSBI.Controllers
{
    [RoutePrefix("api/olap")]
    public class OlapController : ApiController
    {

        [HttpPost]
        [Route("query")]
        public HttpResponseMessage Query(
            [ModelBinder(typeof(PivotDataSourceRequestModelBinder))] PivotDataSourceRequest request)
        {
            using (var conn = new AdomdConnection(@"DataSource=.\astab16;Catalog=27be1e75-d20b-409d-8f46-47e3d7fef2d3"))
            {
                try
                {
                    conn.Open();
                    if (request.Discover)
                    {
                        var restrictions = new AdomdRestrictionCollection();
                        foreach (var restriction in request.Restrictions)
                        {
                            restrictions.Add(restriction.Key, restriction.Value);
                        }
                        var result = conn.GetSchemaDataSet(request.Command, restrictions);
                        return new HttpResponseMessage
                        {
                            Content = new StringContent(result.ToXmlaDiscoverResult(), Encoding.UTF8, "application/xml")
                        };
                    }
                    var command = conn.CreateCommand();
                    command.CommandText = request.Statement;
                    using (var reader = command.ExecuteXmlReader())
                    {
                        return new HttpResponseMessage
                        {
                            Content = new StringContent(reader.ToXmlaResult(), Encoding.UTF8, "application/xml")
                        };
                    }
                }
                catch (Exception e)
                {
                    return new HttpResponseMessage
                    {
                        Content = new StringContent(e.Message),
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }
            }
        }

        [HttpPost]
        [Route("dax")]
        public IHttpActionResult Dax([FromBody]string query)
        {
            using (var conn = new AdomdConnection(@"DataSource=.\astab16;Catalog=27be1e75-d20b-409d-8f46-47e3d7fef2d3"))
            {
                try
                {
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = query;
                    using (var reader = command.ExecuteReader())
                    {
                        var result = new List<Dictionary<string, object>>();
                        while (reader.Read())
                        {
                            var row = new Dictionary<string, object>();
                            var columns = new List<string>();
                            for (var i = 0; i < reader.FieldCount; ++i)
                            {
                                columns.Add(reader.GetName(i));
                            }
                            for (var i = 0; i < reader.FieldCount; ++i)
                            {
                                row[columns[i]] = reader.GetValue(i);
                            }
                            result.Add(row);
                        }
                        return Ok(result);
                    }
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
            }
        }
    }
}
