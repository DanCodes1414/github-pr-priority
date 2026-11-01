using Microsoft.Extensions.Options;
using PriorityPR.Domain.PullRequests.Services;
using PriorityPR.Common.Clients;
using PriorityPR.Common.Settings;
using PriorityPR.Integrations.Github;
using PriorityPR.Integrations.Github.Helpers;
using PriorityPR.Integrations.Jira;
using PriorityPR.Integrations.Jira.Helpers;
using PriorityPR.Integrations.Slack;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();
builder.Services.AddControllers();

builder.Services.AddOptions<GithubSettings>()
    .Bind(builder.Configuration.GetSection(GithubSettings.Section))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddOptions<JiraSettings>()
    .Bind(builder.Configuration.GetSection(JiraSettings.Section))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddOptions<SlackSettings>()
    .Bind(builder.Configuration.GetSection(SlackSettings.Section))
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddOptions<MemberSettings>()
    .Bind(builder.Configuration.GetSection(MemberSettings.Section))
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Integrations
builder.Services.AddTransient<GithubAuthenticationHandler>();
builder.Services.AddTransient<JiraAuthenticationHandler>();
builder.Services.AddTransient<IGithubClient, GithubClient>();
builder.Services.AddTransient<IJiraClient, JiraClient>();
builder.Services.AddTransient<ISlackClient, SlackClient>();

builder.Services
    .AddHttpClient<IGithubClient, GithubClient>(httpClient =>
    {
        httpClient.BaseAddress = new Uri("https://api.github.com");
    })
    .AddHttpMessageHandler<GithubAuthenticationHandler>();
builder.Services
    .AddHttpClient<IJiraClient, JiraClient>((serviceProvider, httpClient) =>
    {
        var jiraSettings = serviceProvider.GetRequiredService<IOptionsMonitor<JiraSettings>>().CurrentValue;
        httpClient.BaseAddress = new Uri($"{jiraSettings.ApiUrl}/rest/api/3/");
    })
    .AddHttpMessageHandler<JiraAuthenticationHandler>();
builder.Services
    .AddHttpClient<ISlackClient, SlackClient>((serviceProvider, httpClient) =>
    {
        var slackSettings = serviceProvider.GetRequiredService<IOptionsMonitor<SlackSettings>>().CurrentValue;
        httpClient.BaseAddress = new Uri(slackSettings.WebhookUrl);
    });

builder.Services.AddTransient<IPullRequestsService, PullRequestsService>();

var app = builder.Build();

app.MapControllers();
app.UseHttpsRedirection();

// TODO: Remove
await new PullRequestsService(
        app.Services.GetRequiredService<IGithubClient>(),
        app.Services.GetRequiredService<ISlackClient>(),
        app.Services.GetRequiredService<IJiraClient>(),
        app.Services.GetRequiredService<IOptionsMonitor<MemberSettings>>())
    .SendPullRequestPriorityNotification();
app.Run();