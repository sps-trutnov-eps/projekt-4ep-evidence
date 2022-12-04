namespace EvidenceProject.Controllers.ActionData;
/// <summary>
/// Data pro Vytvoření projektu
/// </summary>
public class GETProjectCreate
{
    public List<DialCode>? DialCodes { get; set; }
    public List<DialInfo>? DialInfos { get; set; }

    public List<AuthUser>? Users { get; set; }
}
