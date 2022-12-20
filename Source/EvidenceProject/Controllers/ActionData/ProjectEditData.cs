
namespace EvidenceProject.Controllers.RequestClasses;

public class ProjectEditData : ProjectCreateData
{
    [NotRequired]
    public string[]? oldFile { get; set; }
    [NotRequired]
    public string[]? oldTech { get; set; }
    [NotRequired]
    public string[]? oldAssignees { get; set; }
}