using Microsoft.Extensions.Options;
using PriorityPR.Common.Clients;
using PriorityPR.Common.Models;
using PriorityPR.Common.Settings;

namespace PriorityPR.Domain.PullRequests.Services;

public class PullRequestsService(
    IGithubClient githubClient,
    ISlackClient slackClient,
    IJiraClient jiraClient,
    IOptionsMonitor<MemberSettings> memberSettings)
    : IPullRequestsService
{
    private readonly string[] _priorityLabels = ["hotfix", "urgent"];
    private readonly MemberSettings _memberSettings = memberSettings.CurrentValue;

    public async Task SendPullRequestPriorityNotification()
    {
        var pullRequests = await githubClient.GetPullRequests();
        if (pullRequests is null || !pullRequests.Any())
        {
            return;
        }

        var currentSprintTickets = await jiraClient.GetCurrentSprintTickets();
        foreach (var pullRequest in pullRequests)
        {
            pullRequest.LinkedTicket = currentSprintTickets.GetValueOrDefault(pullRequest.TicketKey ?? string.Empty);
        }

        var groupedPullRequestsByPriority = pullRequests
            .OrderBy(pr => pr.Labels.Any(l => _priorityLabels.Contains(l)))
            .ThenBy(pr => pr.LinkedTicket?.FixVersion?.Name)
            .ThenByDescending(pr => pr.LinkedTicket?.Priority)
            .ThenByDescending(pr => pr.LinkedTicket?.Type)
            .ThenBy(pr => pr.CreatedAt)
            // Group related FE & BE PRs
            .GroupBy(pr => pr.TicketKey + pr.TargetBranch)
            .Take(5)
            .ToList();

        var memberMap = CreateMemberMapping();
        var priorityPullRequests = new List<PullRequest>();
        foreach (var groupedPullRequest in groupedPullRequestsByPriority)
        {
            var priorityPullRequest = groupedPullRequest.First();
            priorityPullRequest.LinkedPullRequestUrls = groupedPullRequest
                .Skip(1)
                .Select(pr => (GetPullRequestId(pr.Url), pr.Url))
                .ToList();

            // Map GithubMember IDs to SlackMember IDs
            priorityPullRequest.AuthorId = memberMap.GetValueOrDefault(priorityPullRequest.AuthorId);
            priorityPullRequest.ReviewerIds = groupedPullRequest
                .SelectMany(pr => pr.ReviewerIds)
                .Distinct()
                .Select(id => memberMap.GetValueOrDefault(id))
                .Where(id => !string.IsNullOrEmpty(id))
                .ToList();

            priorityPullRequests.Add(priorityPullRequest);
        }

        await slackClient.SendPullRequestsToPrioritise(priorityPullRequests);
    }

    private Dictionary<string, string> CreateMemberMapping()
    {
        return _memberSettings
            .MemberIdMapping
            .Split(";")
            .ToDictionary(k => k.Split(":")[0], v => v.Split(":")[1]);
    }

    private static string GetPullRequestId(string url)
    {
        return url.Split("/").Last();
    }
}