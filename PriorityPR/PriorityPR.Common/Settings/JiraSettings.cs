using System.ComponentModel.DataAnnotations;

namespace PriorityPR.Common.Settings;

public class JiraSettings
{
    public const string Section = "Jira";

    [Required] public string Projects { get; set; }
    [Required] public string WebUrl { get; set; }
    [Required] public string ApiUrl { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string Secret { get; set; }
}