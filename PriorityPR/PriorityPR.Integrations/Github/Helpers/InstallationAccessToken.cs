using System.Text.Json.Serialization;

namespace PriorityPR.Integrations.Github.Helpers;

public class InstallationAccessToken
{
    [JsonPropertyName("token")]
    public string Token { get; set; }

    [JsonPropertyName("expires_at")]
    public DateTime ExpiresAt { get; set; }
}