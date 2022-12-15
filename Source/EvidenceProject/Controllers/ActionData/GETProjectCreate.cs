namespace EvidenceProject.Controllers.ActionData;
/// <summary>
/// Data pro Vytvoření projektu
/// </summary>
public class GETProjectCreate : ModelBase
{
    public List<DialCode>? DialCodes { get; set; }
    public List<DialInfo>? DialInfos { get; set; }
    public List<User>? Users { get; set; }
    public Project? CurrentProject { get; set; }
}
