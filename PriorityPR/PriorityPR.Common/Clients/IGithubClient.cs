using PriorityPR.Common.Models;

namespace PriorityPR.Common.Clients;

public interface IGithubClient
{
    Task<IList<PullRequest>> GetPullRequests();
}