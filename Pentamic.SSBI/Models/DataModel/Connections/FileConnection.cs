namespace Pentamic.SSBI.Models.DataModel.Connections
{
    public class FileConnection : Connection
    {
        public int SourceFileId { get; set; }
        public SourceFile SourceFile { get; set; }
    }
}
