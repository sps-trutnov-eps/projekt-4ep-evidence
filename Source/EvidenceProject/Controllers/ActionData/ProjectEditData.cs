namespace EvidenceProject.Controllers.RequestClasses;

public class ProjectEditData : ProjectCreateData
{
    public string[]? oldFile { get; set; }
    public string[]? oldTech { get; set; }
    public string[]? oldAssignees { get; set; }
}