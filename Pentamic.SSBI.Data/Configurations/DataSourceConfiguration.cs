﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class DataSourceConfiguration : EntityTypeConfiguration<DataSource>
    {
        public DataSourceConfiguration()
        {
            HasRequired(x => x.Model).WithMany(x => x.DataSources);
            HasOptional(x => x.SourceFile);
        }
        //public int Id { get; set; }
        //public int ModelId { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public DataSourceType Type { get; set; }
        //public string ConnectionString { get; set; }
        //public string Source { get; set; }
        //public string Catalog { get; set; }
        //public string User { get; set; }
        //public string Password { get; set; }
        //public bool IntegratedSecurity { get; set; }
        //public int? SourceFileId { get; set; }

        //public List<PartitionConfiguration> Partitions { get; set; }
        //public ModelConfiguration Model { get; set; }
        //public SourceFileConfiguration SourceFile { get; set; }

        //public string CreatedBy { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }
    }
}
