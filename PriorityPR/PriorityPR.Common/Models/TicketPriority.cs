using System.ComponentModel;

namespace PriorityPR.Common.Models;

public enum TicketPriority
{
    [Description("Lowest")] Lowest,
    [Description("Low")] Low,
    [Description("Medium")] Medium,
    [Description("High")] High,
    [Description("Highest")] Highest,
}