using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pentamic.SSBI.Models.Reporting
{
    public class ReportTile : IAuditable
    {
        public int Id { get; set; }
        public int ReportPageId { get; set; }
        public int ReportId { get; set; }
        public int ModelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PositionConfig { get; set; }
        public string DisplayConfig { get; set; }
        public string DataConfig { get; set; }
        public string DisplayTypeId { get; set; }
        public int SourceType { get; set; } //0-Drag&drop.1-BuildRow

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }

        public ReportPage ReportPage { get; set; }
        public DisplayType DisplayType { get; set; }
    }
}