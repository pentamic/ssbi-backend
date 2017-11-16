using System;

namespace Pentamic.SSBI.Entities
{
    public class SourceFile : IAuditable
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
    }
}