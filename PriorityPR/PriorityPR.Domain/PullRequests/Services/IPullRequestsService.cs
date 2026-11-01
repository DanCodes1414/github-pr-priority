namespace PriorityPR.Domain.PullRequests.Services;

public interface IPullRequestsService
{
    Task SendPullRequestPriorityNotification();
}