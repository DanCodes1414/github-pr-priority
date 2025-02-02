using System.Text.Json.Serialization;

namespace PriorityPR.Integrations.Jira.Models;

public class JiraFixVersion
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("released")]
    public bool IsReleased { get; set; }

    [JsonPropertyName("releaseDate")]
    public string ReleaseDate { get; set; }
}