namespace EvidenceProject.Controllers.RequestClasses;
public class ProjectCreateData
{
    public string ProjectName { get; set; }

    public string ProjectState { get; set; }

    public string ProjectType { get; set; }

    public string Technology { get; set; }

    public IFormFileCollection Photos { get; set; }

    public string Achievements { get; set; }

}
