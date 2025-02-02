using System.ComponentModel.DataAnnotations;

namespace PriorityPR.Common.Settings;

public class MemberSettings
{
    public const string Section = "Members";

    [Required] public string MemberIdMapping { get; set; }
}