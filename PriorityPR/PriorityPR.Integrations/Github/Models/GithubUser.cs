using System.Text.Json.Serialization;

namespace PriorityPR.Integrations.Github.Models;

public abstract class GithubUser
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("login")]
    public string Login { get; set; }
}