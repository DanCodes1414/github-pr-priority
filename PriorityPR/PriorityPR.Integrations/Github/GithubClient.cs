using System.Net.Http.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using PriorityPR.Common.Clients;
using PriorityPR.Common.Models;
using PriorityPR.Common.Settings;
using PriorityPR.Integrations.Github.Models;

namespace PriorityPR.Integrations.Github;

public class GithubClient(HttpClient client, IOptionsMonitor<GithubSettings> githubSettings) : IGithubClient
{
    private readonly GithubSettings _githubSettings = githubSettings.CurrentValue;

    public async Task<IList<PullRequest>> GetPullRequests()
    {
        var pullRequests = new List<PullRequest>();
        foreach (var repository in _githubSettings.Repositories.Split(","))
        {
            var endpoint = $"repos/{_githubSettings.Organisation}/{repository}/pulls?per_page=100";
            var response = await client.GetFromJsonAsync<IList<GithubPullRequest>>(endpoint);
            if (response is null || !response.Any())
            {
                return new List<PullRequest>();
            }

            var repoPullRequests = response
                .Select(pr => new PullRequest
                {
                    Title = pr.Title,
                    Url = pr.Url,
                    TargetBranch = pr.TargetBranch,
                    AuthorId = pr.Author.Id.ToString(),
                    CreatedAt = pr.CreatedAt,
                    Labels = pr.Labels.Select(l => l.Name).ToList(),
                    ReviewerIds = pr.Reviewers.Select(r => r.Id.ToString()),
                    TicketKey = GetTicketCode(pr.Title),
                })
                .ToList();
            pullRequests.AddRange(repoPullRequests);
        }

        return pullRequests;
    }

    private static string GetTicketCode(string title)
    {
        var ticketCodeMatch = Regex.Match(title, @"\bPP[A-Za-z]{1,2}-\d{2,5}\b");
        return ticketCodeMatch.Success ? ticketCodeMatch.Value : null;
    }
}