﻿using Pentamic.SSBI.Models.DataModel.Objects;

namespace Pentamic.SSBI.Models.Discover
{
    public class RelationshipDiscoverModel
    {
        public DataSource DataSource { get; set; }
        public string FkTableSchema { get; set; }
        public string FkTableName { get; set; }
    }
}
