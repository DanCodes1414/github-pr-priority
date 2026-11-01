using PriorityPR.Common.Models;

namespace PriorityPR.Common.Clients;

public interface ISlackClient
{
    Task SendUrgentPullRequest(PullRequest pullRequest);
    Task SendPullRequestsToPrioritise(IEnumerable<PullRequest> pullRequests);
}