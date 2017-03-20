﻿namespace Pentamic.SSBI.Models.Reporting
{
    public class ReportTile
    {
        public int Id { get; set; }
        public int ReportPageId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
        public int SizeX { get; set; }
        public int SizeY { get; set; }
        public string DisplayConfig { get; set; }
        public string DataConfig { get; set; }

        public ReportPage ReportPage { get; set; }
    }
}