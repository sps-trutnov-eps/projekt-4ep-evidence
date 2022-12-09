using EvidenceProject.Data.DataModels;

namespace EvidenceProject.Controllers.ActionData;
public class ProfileData
{
    public List<Project>? Projects { get; set; }
    public AuthUser? AuthUser { get; set; }

    public List<AuthUser>? Users { get; set; }
    public List<User>? NonAuthUsers { get; set; }
    public List<DialInfo>? Categories { get; set; }
}
