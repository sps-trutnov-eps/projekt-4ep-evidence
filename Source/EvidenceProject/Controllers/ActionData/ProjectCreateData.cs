namespace EvidenceProject.Controllers.RequestClasses;

public class ProjectCreateData
{
    public string? projectName { get; set; }

    public string? description { get; set; }

    public string? stavit { get; set; }

    [NotRequired]
    public string? typy { get; set; }

    public string? projectManager { get; set; }

    [NotRequired]
    public string[]? tech { get; set; }

    [NotRequired]
    public IFormFileCollection? photos { get; set; }

    [NotRequired]
    public string? achievements { get; set; }

    [NotRequired]
    public string? github { get; set; }

    [NotRequired]
    public string? slack { get; set; }

    [NotRequired]
    public string[]? assignees { get; set; }
}