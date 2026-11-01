using System.Net.Http.Headers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using PriorityPR.Common.Settings;

namespace PriorityPR.Integrations.Github.Helpers;

public class GithubAuthenticationHandler(
    IHttpClientFactory httpClientFactory,
    IMemoryCache memoryCache,
    IOptionsMonitor<GithubSettings> githubSettings)
    : DelegatingHandler
{
    private const string InstallationAccessToken = "InstallationAccessToken";
    private readonly TimeSpan _cacheExpiry = new(0, 55, 0);
    private readonly GithubSettings _githubSettings = githubSettings.CurrentValue;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string installationAccessToken;
        if (memoryCache.TryGetValue(InstallationAccessToken, out string cachedAccessToken))
        {
            installationAccessToken = cachedAccessToken;
        }
        else
        {
            var authClient = httpClientFactory.CreateClient();
            installationAccessToken = await authClient.GenerateInstallationAccessToken(_githubSettings);
            memoryCache.Set(InstallationAccessToken, installationAccessToken, new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = _cacheExpiry });
        }

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", installationAccessToken);
        request.Headers.Add("X-GitHub-Api-Version", githubSettings.CurrentValue.ApiVersion);
        request.Headers.Add("User-Agent", "Priority PR");

        return await base.SendAsync(request, cancellationToken);
    }
}