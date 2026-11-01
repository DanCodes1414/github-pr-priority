namespace PriorityPR.Common.Models;

public class Ticket
{
    public string Key { get; init; }
    public string Url { get; init; }
    public TicketType Type { get; init; }
    public TicketPriority Priority { get; init; }
    public TicketStatus Status { get; set; }
    public double StoryPoints { get; init; }
    public FixVersion FixVersion { get; init; }
    public PullRequestStatus? PullRequestStatus { get; set; }
}