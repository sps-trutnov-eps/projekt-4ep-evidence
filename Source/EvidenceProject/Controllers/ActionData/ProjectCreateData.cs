namespace EvidenceProject.Controllers.RequestClasses;

public class ProjectCreateData
{
    public string? projectName { get; set; }

    public string? description { get; set; }

    public string? stavit { get; set; }

    public string? typy { get; set; }

    //public int? projectManager { get; set; }

    public string[]? tech { get; set; }

    public IFormFileCollection? photos { get; set; }

    public string? achievements { get; set; }

    public string? github { get; set; }

    public string? slack { get; set; }
}