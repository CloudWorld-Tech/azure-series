using System.Text.Json.Serialization;

namespace CloudWorld.Functions.Models;

public record ImageAnalysisDocument
{
    [JsonPropertyName("id")] public required string Id { get; set; }

    [JsonPropertyName("imageId")] public required string ImageId { get; set; }

    [JsonPropertyName("tags")] public required List<string> Tags { get; set; }

    [JsonPropertyName("objects")] public required List<string> Objects { get; set; }

    [JsonPropertyName("description")] public required List<string> Description { get; set; }
}