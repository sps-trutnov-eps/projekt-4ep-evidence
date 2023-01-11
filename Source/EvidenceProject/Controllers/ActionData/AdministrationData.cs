namespace EvidenceProject.Controllers.ActionData
{
    public class AdministrationData : ModelBase
    {
        public List<Project>? Projects { get; set; }
        public List<AuthUser>? Users { get; set; }
        public List<User>? NonAuthUsers { get; set; }
        public List<DialInfo>? Categories { get; set; }
    }
}
