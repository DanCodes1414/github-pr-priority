using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace PriorityPR.Integrations.Jira.Models;

public class JiraTicketFields
{
    [JsonPropertyName("issuetype")]
    public JiraTicketType Type { get; set; }

    [JsonPropertyName("customfield_10014")]
    public double? StoryPoints { get; set; }

    [JsonPropertyName("fixVersions")]
    public IEnumerable<JiraFixVersion> FixVersions { get; set; }

    [JsonPropertyName("priority")]
    public JiraTicketPriority Priority { get; set; }

    [JsonPropertyName("status")]
    public JiraTicketStatus Status { get; set; }

    [JsonPropertyName("customfield_10000")]
    public string PullRequestsUnformatted { get; set; }

    public JiraPullRequestMetaData PullRequestMetaData => FormatPullRequests(PullRequestsUnformatted);

    [JsonPropertyName("created")]
    public string CreatedAt { get; set; }

    private static JiraPullRequestMetaData FormatPullRequests(string unformattedString)
    {
        // Extract "pullrequest" section of the unformatted string
        var match = Regex.Match(unformattedString, @"pullrequest=\{(.*?)\}, json=", RegexOptions.Singleline);
        var pullRequestData = match.Success ? match.Groups[1].Value : null;
        if (pullRequestData is null)
        {
            return new JiraPullRequestMetaData();
        }

        // Convert unformatted string to valid JSON
        var pullRequestJson = Regex.Replace(pullRequestData, @"(\w+)=([\w\d\-:]+)", "\"$1\":\"$2\"");
        pullRequestJson = "{" + pullRequestJson + "}";
        return JsonSerializer.Deserialize<JiraPullRequestMetaData>(pullRequestJson);
    }
}