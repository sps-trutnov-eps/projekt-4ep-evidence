namespace EvidenceProject.Controllers.RequestClasses;

public class ProjectCreateData
{
    public string? projectName { get; set; }

    public string? projectState { get; set; }

    public string? projectType { get; set; }

    public string? technology { get; set; }

    public IFormFileCollection? photos { get; set; }

    public string? achievements { get; set; }
}