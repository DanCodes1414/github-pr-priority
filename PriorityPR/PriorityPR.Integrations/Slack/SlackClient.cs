using System.Net.Http.Json;
using PriorityPR.Common.Clients;
using PriorityPR.Common.Extensions;
using PriorityPR.Common.Models;
using PriorityPR.Integrations.Slack.Helpers;
using PriorityPR.Integrations.Slack.Models;

namespace PriorityPR.Integrations.Slack;

public class SlackClient(HttpClient client) : ISlackClient
{
    public async Task SendUrgentPullRequest(PullRequest pullRequest)
    {
        throw new NotImplementedException();
    }

    public async Task SendPullRequestsToPrioritise(IEnumerable<PullRequest> pullRequests)
    {
        var slackMessage = ":panda_face: *Daily PR Priority List!* Reviewing PRs at the top should be prioritised.\n";
        foreach (var pullRequest in pullRequests)
        {
            var ticket = pullRequest.LinkedTicket;
            var ticketMessage = string.Empty;
            if (ticket is not null)
            {
                ticketMessage = $"({ticket.Url.CreateLink($"{ticket.Key} - {ticket.Type.GetEnumDescription()} - {ticket.FixVersion} - {ticket.StoryPoints}SP")}). ";
            }

            var relatedPullRequestsMessage = string.Empty;
            if (pullRequest.LinkedPullRequestUrls.Any())
            {
                relatedPullRequestsMessage = $"Related PRs: {pullRequest.LinkedPullRequestUrls.Select(linked => linked.Url.CreateLink(linked.Id))}";
            }

            slackMessage += TextFormatter.CreateListItem(
                $"Reviewers: {TextFormatter.TagMembers(pullRequest.ReviewerIds)}" +
                $"{pullRequest.Url.CreateLink(pullRequest.Title)}" +
                ticketMessage +
                relatedPullRequestsMessage +
                $"Author: {TextFormatter.TagMembers([pullRequest.AuthorId])}");
        }

        await client.PostAsJsonAsync(string.Empty, new SlackMessage(slackMessage));
    }
}