using System.Text.Json.Serialization;

namespace PriorityPR.Integrations.Jira.Models;

public class JiraTicketType
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
}