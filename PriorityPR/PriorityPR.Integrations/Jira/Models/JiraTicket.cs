using System.Text.Json.Serialization;

namespace PriorityPR.Integrations.Jira.Models;

public class JiraTicket
{
    [JsonPropertyName("key")]
    public string Key { get; set; }

    [JsonPropertyName("fields")]
    public JiraTicketFields Fields { get; set; }
}