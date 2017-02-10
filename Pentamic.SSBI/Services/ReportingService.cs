using Breeze.ContextProvider;
using Breeze.ContextProvider.EF6;
using Microsoft.AnalysisServices.AdomdClient;
using Newtonsoft.Json.Linq;
using Pentamic.SSBI.Models.Reporting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Services
{
    public class ReportingService
    {
        private EFContextProvider<ReportingContext> _contextProvider;

        public ReportingService()
        {
            _contextProvider = new EFContextProvider<ReportingContext>();
        }

        private ReportingContext Context
        {
            get
            {
                return _contextProvider.Context;
            }
        }

        public string Metadata
        {
            get { return _contextProvider.Metadata(); }
        }

        public IQueryable<Report> Reports
        {
            get { return Context.Reports; }
        }
        public IQueryable<ReportPage> ReportPages
        {
            get { return Context.ReportPages; }
        }
        public IQueryable<ReportTile> ReportTiles
        {
            get { return Context.ReportTiles; }
        }

        public List<Dictionary<string, object>> GetReportTileData(int reportTileId)
        {
            var tile = Context.ReportTiles.Where(x => x.Id == reportTileId)
                .Include(x => x.ReportPage.Report)
                .FirstOrDefault();
            var modelService = new DataModelService();
            var model = modelService.Models.Where(x => x.Id == tile.ReportPage.Report.ModelId)
                .FirstOrDefault();
            var conStr = @"DataSource=.\astab16;Catalog=" + model.DatabaseName;
            using (var conn = new AdomdConnection(conStr))
            {
                try
                {
                    conn.Open();
                    var command = conn.CreateCommand();
                    command.CommandText = BuildReportTileQuery(tile);
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
                        return result;
                    }
                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }
        }

        public string BuildReportTileQuery(ReportTile tile)
        {
            var query = "EVALUATE ( SUMMARIZECOLUMNS ( {0} ) ) ";
            var columns = new List<string>();
            if (!string.IsNullOrEmpty(tile.ColumnFields))
            {
                columns.AddRange(tile.ColumnFields.Split(','));
            }
            if (!string.IsNullOrEmpty(tile.RowFields))
            {
                columns.AddRange(tile.RowFields.Split(','));
            }
            if (!string.IsNullOrEmpty(tile.ValueFields))
            {
                var valueFields = tile.ValueFields.Split(',');
                foreach (var field in valueFields)
                {
                    var sIdx = field.IndexOf('[') + 1;
                    var name = field.Substring(field.IndexOf('[') + 1, field.Length - 1 - sIdx);
                    columns.Add("\"" + field + "\"");
                    columns.Add(field);
                }
            }
            query = string.Format(query, string.Join(",", columns));

            return query;
        }

        public SaveResult SaveChanges(JObject saveBundle)
        {
            var txSettings = new TransactionSettings { TransactionType = TransactionType.TransactionScope };
            return _contextProvider.SaveChanges(saveBundle, txSettings);
        }

    }
}