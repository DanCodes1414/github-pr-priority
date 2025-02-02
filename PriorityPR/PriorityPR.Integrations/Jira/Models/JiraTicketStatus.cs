using System.Text.Json.Serialization;

namespace PriorityPR.Integrations.Jira.Models;

public class JiraTicketStatus
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}