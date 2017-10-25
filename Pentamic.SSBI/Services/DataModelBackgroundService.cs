using Breeze.ContextProvider;
using Breeze.ContextProvider.EF6;
using Newtonsoft.Json.Linq;
using Pentamic.SSBI.Models.DataModel;
using Pentamic.SSBI.Models.DataModel.Objects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AS = Microsoft.AnalysisServices.Tabular;
using AC = Microsoft.AnalysisServices.AdomdClient;
using System.Data.SqlClient;
using System.Web;
using AN = Microsoft.AnalysisServices;
using System.Diagnostics;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Data;
using Pentamic.SSBI.Models;
using System.Security.Claims;
using Hangfire;

namespace Pentamic.SSBI.Services
{
    public class DataModelBackgroundService
    {
        private string _asConnectionString = System.Configuration.ConfigurationManager
                .ConnectionStrings["AnalysisServiceConnection"]
                .ConnectionString;

        public void RefreshModel(int modelId)
        {
            using (var context = new DataModelContext())
            {
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases[modelId.ToString()];
                    if (database == null)
                    {
                        throw new Exception("Database not found");
                    }
                    database.Model.RequestRefresh(AS.RefreshType.Full);
                    database.Update(AN.UpdateOptions.ExpandFull);
                }
            }
        }

        public void RefreshTable(int tableId)
        {
            using (var context = new DataModelContext())
            {
                var tb = context.Tables.Where(x => x.Id == tableId).FirstOrDefault();
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases[tb.ModelId.ToString()];
                    var table = database.Model.Tables[tb.Name];
                    table.RequestRefresh(AS.RefreshType.Full);
                    database.Update(Microsoft.AnalysisServices.UpdateOptions.ExpandFull);
                }
            }
        }

        public void RefreshPartition(int partitionId)
        {
            using (var context = new DataModelContext())
            {
                var pa = context.Partitions.Where(x => x.Id == partitionId)
                    .FirstOrDefault();
                using (var server = new AS.Server())
                {
                    server.Connect(_asConnectionString);
                    var database = server.Databases[pa.ModelId.ToString()];
                    var partition = database.Model.Tables[pa.TableId.ToString()].Partitions[pa.IdStr];
                    partition.RequestRefresh(AS.RefreshType.Full);
                    database.Update(Microsoft.AnalysisServices.UpdateOptions.ExpandFull);
                }
            }
        }

        public void RunModelRefreshQueue()
        {
            using (var context = new DataModelContext())
            {
                var queueEntries = context.ModelRefreshQueues
                    .Where(x => x.StartedAt == null)
                    .GroupBy(x => x.ModelId)
                    .Select(x => x.OrderBy(y => y.CreatedAt).FirstOrDefault()).ToList();
                foreach (var e in queueEntries)
                {
                    e.StartedAt = DateTimeOffset.Now;
                    context.SaveChanges();
                    RefreshModel(e.ModelId);
                    e.EndedAt = DateTimeOffset.Now;
                    context.SaveChanges();
                }
            }

        }

    }
}