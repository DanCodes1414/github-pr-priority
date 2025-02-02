using System.ComponentModel;

namespace PriorityPR.Common.Models;

public enum TicketStatus
{
    [Description("Open")] Open,
    [Description("In Progress")] InProgress,
    [Description("PR Review")] PrReview,
    [Description("PR Accepted")] PrAccepted,
    [Description("QA Ready")] QaReady,
    [Description("QA Review")] QaReview,
    [Description("Done")] Done,
}