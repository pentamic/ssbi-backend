using System;
using Pentamic.SSBI.Data;
using AS = Microsoft.AnalysisServices.Tabular;
using AC = Microsoft.AnalysisServices.AdomdClient;
using AN = Microsoft.AnalysisServices;

namespace Pentamic.SSBI.Services
{
    public class DataModelBackgroundService
    {
        private string _asConnectionString = "";

        public void RefreshModel(int modelId)
        {
            using (var context = new AppDbContext())
            {
                var mo = context.Models.Find(modelId);
                if (mo == null)
                {
                    throw new Exception("Model not found");
                }
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

        //public void RunModelRefreshQueue()
        //{
        //    using (var context = new DataModelContext())
        //    {
        //        var queueEntries = context.ModelRefreshQueues
        //            .Where(x => x.StartedAt == null)
        //            .GroupBy(x => x.ModelId)
        //            .Select(x => x.OrderBy(y => y.CreatedAt).FirstOrDefault()).ToList();
        //        foreach (var e in queueEntries)
        //        {
        //            e.StartedAt = DateTimeOffset.Now;
        //            context.SaveChanges();
        //            RefreshModel(e.ModelId);
        //            e.EndedAt = DateTimeOffset.Now;
        //            context.SaveChanges();
        //        }
        //    }

        //}

    }
}