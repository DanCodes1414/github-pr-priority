using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using PriorityPR.Common.Settings;

namespace PriorityPR.Integrations.Jira.Helpers;

public class JiraAuthenticationHandler(IOptionsMonitor<JiraSettings> jiraSettings) : DelegatingHandler
{
    private readonly JiraSettings _jiraSettings = jiraSettings.CurrentValue;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var accessToken = JiraTokenGenerator.GenerateAccessToken(_jiraSettings.Email, _jiraSettings.Secret);
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", accessToken);

        return base.SendAsync(request, cancellationToken);
    }
}