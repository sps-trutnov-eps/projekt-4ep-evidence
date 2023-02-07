namespace EvidenceProject.Controllers.ActionData;
public class RegisterData: LoginData
{
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Contact { get; set; }
    public string? SchoolYear { get; set; }
    public string? StudyField { get; set; }
    [NotRequired]
    public string? Caller { get; set; }
}
