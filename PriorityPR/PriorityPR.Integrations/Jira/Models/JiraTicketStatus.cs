using System.Text.Json.Serialization;

namespace PriorityPR.Integrations.Jira.Models;

public abstract class JiraTicketStatus
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}