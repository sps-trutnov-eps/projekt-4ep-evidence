namespace EvidenceProject.Controllers.ActionData;

public class LoginDataUpdate : ModelBase
{
    public string? new_password { get; set; }
    public string? new_password_again { get; set; }
}
