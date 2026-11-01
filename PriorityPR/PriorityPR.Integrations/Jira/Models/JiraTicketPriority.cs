using System.Text.Json.Serialization;

namespace PriorityPR.Integrations.Jira.Models;

public class JiraTicketPriority
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}