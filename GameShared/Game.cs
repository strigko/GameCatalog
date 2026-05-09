using System.Text.Json.Serialization;

namespace GameShared
{
    public class Game
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("yearOfRelease")]
        public int YearOfRelease { get; set; }

        [JsonPropertyName("pictureFilePath")]
        public string PictureFilePath { get; set; } = string.Empty;
    }
}
