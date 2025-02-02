using System.Text.Json.Serialization;

namespace PriorityPR.Integrations.Jira.Models;

public class JiraPullRequestMetaData
{
    [JsonPropertyName("state")]
    public string Status { get; init; }

    [JsonPropertyName("stateCount")]
    public string Count { get; init; }
}