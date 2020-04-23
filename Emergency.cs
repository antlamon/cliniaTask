namespace CliniaApi
{
    public class Emergency : IResource
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Address { get; set; }
        public string EmergencySlugId { get; set; }
    }
}