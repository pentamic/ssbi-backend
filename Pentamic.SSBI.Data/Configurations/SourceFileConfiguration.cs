using System;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class SourceFileConfiguration : EntityTypeConfiguration<SourceFile>
    {
        public SourceFileConfiguration() { }
        //public int Id { get; set; }
        //public string FileName { get; set; }
        //public string FilePath { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }
    }
}