using System.Globalization;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using PriorityPR.Common.Clients;
using PriorityPR.Common.Extensions;
using PriorityPR.Common.Models;
using PriorityPR.Common.Settings;
using PriorityPR.Integrations.Jira.Models;

namespace PriorityPR.Integrations.Jira;

public class JiraClient(HttpClient client, IOptionsMonitor<JiraSettings> jiraSettings) : IJiraClient
{
    private readonly JiraSettings _jiraSettings = jiraSettings.CurrentValue;

    public async Task<Dictionary<string, Ticket>> GetCurrentSprintTickets()
    {
        var projects = _jiraSettings.Projects;
        const string fields = "fixVersions,key,priority,issuetype,created,status,customfield_10014,customfield_10000";
        var endpoint = $"search?fields={fields}&maxResults=100&jql=project in ({projects}) AND Sprint in openSprints()";
        var response = await client.GetFromJsonAsync<GetCurrentSprintTicketsResponse>(endpoint);

        if (response?.Tickets is null || !response.Tickets.Any())
        {
            return new Dictionary<string, Ticket>();
        }

        return response.Tickets
            .Select(t => new Ticket
            {
                Key = t.Key,
                Url = Path.Combine(_jiraSettings.WebUrl, t.Key),
                Type = t.Fields.Type.Name.GetEnumFromDescription<TicketType>(),
                Priority = t.Fields.Priority.Name.GetEnumFromDescription<TicketPriority>(),
                Status = t.Fields.Status.Name.GetEnumFromDescription<TicketStatus>(),
                StoryPoints = t.Fields.StoryPoints ?? 0,
                FixVersion = GetRelevantFixVersion(t.Fields.FixVersions?.ToList()),
                PullRequestStatus = t.Fields.PullRequestMetaData?.Status?.GetEnumFromDescription<PullRequestStatus>(),
            })
            .ToDictionary(k => k.Key, v => v);
    }

    private static FixVersion GetRelevantFixVersion(IList<JiraFixVersion> fixVersions)
    {
        if (fixVersions is null || !fixVersions.Any())
        {
            return null;
        }

        var fixVersion = fixVersions.MinBy(fv => fv.Name.Replace(".", string.Empty));
        var releaseDate = fixVersion.ReleaseDate is null
            ? null
            : (DateTime?) DateTime.ParseExact(fixVersion.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        return new FixVersion { Name = fixVersion.Name, ReleaseDate = releaseDate };
    }
}