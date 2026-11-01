namespace PriorityPR.Common.Models;

public class PullRequest
{
    public string Title { get; init; }
    public string Url { get; init; }
    public string TargetBranch { get; init; }
    public string AuthorId { get; set; }
    public IEnumerable<string> ReviewerIds { get; set; }
    public IEnumerable<string> Labels { get; init; }
    public DateTime CreatedAt { get; init; }
    public string TicketKey { get; init; }
    public Ticket LinkedTicket { get; set; }
    public IEnumerable<(string Id, string Url)> LinkedPullRequestUrls { get; set; }
}