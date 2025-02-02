using System.Text.Json.Serialization;

namespace PriorityPR.Integrations.Github.Models;

public abstract class GithubPullRequest
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("html_url")]
    public string Url { get; set; }

    [JsonPropertyName("labels")]
    public IEnumerable<GithubLabel> Labels { get; set; }

    [JsonPropertyName("base.ref")]
    public string TargetBranch { get; set; }

    [JsonPropertyName("user")]
    public GithubUser Author { get; set; }

    [JsonPropertyName("requested_reviewers")]
    public IEnumerable<GithubUser> Reviewers { get; set; } = new List<GithubUser>();

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    // Probably not needed
    // public bool Draft { get; set; }
}