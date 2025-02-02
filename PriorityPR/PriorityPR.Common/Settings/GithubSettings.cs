using System.ComponentModel.DataAnnotations;

namespace PriorityPR.Common.Settings;

public class GithubSettings
{
    public const string Section = "Github";

    [Required] public string Organisation { get; set; }
    [Required] public string Repositories { get; set; }
    [Required] public string InstallationId { get; set; }
    [Required] public string ClientId { get; set; }
    [Required] public string Secret { get; set; }
    public string ApiVersion { get; set; } = "2022-11-28";
}