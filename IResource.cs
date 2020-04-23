namespace CliniaApi
{
    public interface IResource
    {
        string Id { get; set; }
        string Name { get; set; }
        double Lat { get; set; }
        double Lng { get; set; }
        string Address { get; set; }
    }
}