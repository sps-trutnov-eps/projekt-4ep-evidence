using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EvidenceProject.Data.DataModels;

[Keyless]
public class User
{
    [Required] public string? fullName { get; set; }
    public string? studyField { get; set; }
    public string? contactDetails { get; set; }
    
}

public class AuthUser : User
{
    [Required] [Key] public string? username { init; get; }
    [Required] public string? password { set; get; }
    public bool? globalAdmin { get; set; }
}

