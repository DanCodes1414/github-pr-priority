namespace PriorityPR.Integrations.Slack.Helpers;

public static class TextFormatter
{
    public static string CreateListItem(string text)
    {
        return $"  *•*   {text}\n";
    }

    public static string TagMembers(IEnumerable<string> memberIds)
    {
        return memberIds.Aggregate(string.Empty, (taggedMembers, memberId) => taggedMembers + $"<@{memberId}> ");
    }

    public static string CreateLink(this string url, string displayText)
    {
        return $"<{url}|{displayText}> ";
    }
}