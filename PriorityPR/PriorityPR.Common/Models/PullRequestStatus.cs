using System.ComponentModel;

namespace PriorityPR.Common.Models;

public enum PullRequestStatus
{
    [Description("OPEN")] Open,
    [Description("MERGED")] Merged,
    [Description("DECLINED")] Declined,
}