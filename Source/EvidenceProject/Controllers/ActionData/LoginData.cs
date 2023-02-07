using EvidenceProject.Controllers.ActionData;

namespace EvidenceProject.Controllers.RequestClasses;

public class LoginData : ModelBase
{
    public string? Username { get; set; }
    public string? Password { get; set; }

}