using System.Text.Json.Serialization;

namespace PriorityPR.Integrations.Jira.Models;

public class GetCurrentSprintTicketsResponse
{
    // TODO: Implement Pagination
    [JsonPropertyName("startAt")]
    public int StartAt { get; set; }

    [JsonPropertyName("maxResults")]
    public int MaxResults { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("issues")]
    public IEnumerable<JiraTicket> Tickets { get; set; }
}