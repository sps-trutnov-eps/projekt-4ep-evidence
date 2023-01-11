namespace EvidenceProject.Controllers.ActionData;
public class ProfileData : ModelBase
{
    public List<Project>? Projects { get; set; }
    public AuthUser? AuthUser { get; set; }
}
