using System.ComponentModel;

namespace PriorityPR.Common.Models;

public enum TicketType
{
    [Description("Story")] Story,
    [Description("Task")] Task,
    [Description("Bug")] Bug,
}