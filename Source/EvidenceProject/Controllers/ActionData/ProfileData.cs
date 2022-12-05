using EvidenceProject.Data.DataModels;

namespace EvidenceProject.Controllers.ActionData;
public class ProfileData
{
    public List<Project>? Projects { get; set; }
    public AuthUser? AuthUser { get; set; }

    // Pro admina, aby z user mohl udělat authUser
    public List<AuthUser>? Users { get; set; }
    // Pro Admina
    public List<DialInfo>? Categories { get; set; }
}
