using System.Text.Json.Serialization;

namespace BacklogTracker.Models
{
    public class IGDBGame
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }

    public class IGDBResponse
    {
        public List<IGDBGame>? Games { get; set; }
    }
}
