namespace Server.Infrastructure.Models.Responses
{
    public class StatisticsResponse
    {
        public int NavigationCount { get; set; }
        public IEnumerable<KeyValuePair<string,int>> Countries { get; set; } = new List<KeyValuePair<string, int>>();
        public IEnumerable<KeyValuePair<string, int>> Platforms { get; set; } = new List<KeyValuePair<string, int>>();
        public IEnumerable<KeyValuePair<string, int>> Browsers { get; set; } = new List<KeyValuePair<string, int>>();

    }
}
