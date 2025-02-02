using System.Text.Json.Serialization;

namespace PriorityPR.Integrations.Github.Models;

public abstract class GithubLabel
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}