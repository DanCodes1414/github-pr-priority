using System.ComponentModel.DataAnnotations;

namespace PriorityPR.Common.Settings;

public class SlackSettings
{
    public const string Section = "Slack";

    [Required] public string WebhookUrl { get; set; }
}