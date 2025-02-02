using System.Text;

namespace PriorityPR.Integrations.Jira.Helpers;

public static class JiraTokenGenerator
{
    public static string GenerateAccessToken(string email, string secret)
    {
        return Convert.ToBase64String(Encoding.ASCII.GetBytes($"{email}:{secret}"));
    }
}