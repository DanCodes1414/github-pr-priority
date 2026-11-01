using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using PriorityPR.Common.Settings;

namespace PriorityPR.Integrations.Github.Helpers;

public static class GithubTokenGenerator
{
    public static async Task<string> GenerateInstallationAccessToken(this HttpClient client, GithubSettings githubSettings)
    {
        // TODO: Find a better way
        var jwt = GenerateJwtAccessToken(githubSettings.ClientId, githubSettings.Secret);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        client.DefaultRequestHeaders.Add("X-GitHub-Api-Version", githubSettings.ApiVersion);
        client.DefaultRequestHeaders.Add("User-Agent", "Priority PR");

        var url = $"app/installations/{githubSettings.InstallationId}/access_tokens";
        client.BaseAddress = new Uri("https://api.github.com");
        var response = await client.PostAsync(url, null);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Unable to generate Github Access Token");
        }

        var accessToken = await response.Content.ReadFromJsonAsync<InstallationAccessToken>();
        if (string.IsNullOrEmpty(accessToken.Token) || accessToken.ExpiresAt < DateTime.UtcNow)
        {
            throw new Exception("Invalid Github Access Token");
        }

        return accessToken.Token;
    }

    private static string GenerateJwtAccessToken(string clientId, string secret)
    {
        using var rsa = RSA.Create();
        rsa.ImportFromPem(secret);
        var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

        return new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
        {
            Issuer = clientId,
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(10),
            SigningCredentials = signingCredentials,
        });
    }
}