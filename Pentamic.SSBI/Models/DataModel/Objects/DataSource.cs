﻿using System;
using System.Collections.Generic;

namespace Pentamic.SSBI.Models.DataModel.Objects
{
    public class DataSource : IDataModelObject,IAuditable
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string Description { get; set; }
        public DataSourceType Type { get; set; }
        public string Source { get; set; }
        public string Catalog { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool IntegratedSecurity { get; set; }
        public int? SourceFileId { get; set; }
        public DataModelObjectState State { get; set; }
        public Model Model { get; set; }
        public List<Table> Tables { get; set; }
        public SourceFile SourceFile { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
    }
}