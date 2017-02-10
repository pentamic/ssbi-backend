namespace Pentamic.SSBI.Models.DataModel
{
    public interface IDataModelObject
    {
        int Id { get; set; }
        string Name { get; set; }
        string OriginalName { get; set; }
        DataModelObjectState State { get; set; }
    }
}