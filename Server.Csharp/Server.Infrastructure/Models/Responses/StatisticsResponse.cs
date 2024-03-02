namespace Server.Infrastructure.Models.Responses
{
    public class StatisticsItem
    {
        public required string Name { get; set; }
        public int Count { get; set; }
    }

    public class StatisticsResponse
    {
        public int NavigationCount { get; set; }
        public List<StatisticsItem> Countries { get; set; } = new List<StatisticsItem>();
        public List<StatisticsItem> Platforms { get; set; } = new List<StatisticsItem>();
        public List<StatisticsItem> Browsers { get; set; } = new List<StatisticsItem>();

    }
}
