﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Pentamic.SSBI.Models
{
    public class FileActionResult : IHttpActionResult
    {
        public FileActionResult(string path)
        {
            StreamContent = new StreamContent(File.OpenRead(path));
        }

        public FileActionResult(Stream stream)
        {
            StreamContent = new StreamContent(stream);
        }

        public StreamContent StreamContent { get; set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage
            {
                Content = StreamContent
            };
            response.Content.Headers.ContentLength = StreamContent.Headers.ContentLength;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            return Task.FromResult(response);
        }
    }
}