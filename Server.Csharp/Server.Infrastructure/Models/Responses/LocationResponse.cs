namespace Server.Infrastructure.Models.Responses
{
    public class LocationResponse
    {
        public string? Status { get; set; }
        public string? Continent { get; set; }
        public string? Country { get; set; }
        public string? RegionName { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Zip { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public string? Isp { get; set; }
        public string? Query { get; set; }
    }
}
