using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EvidenceProject.Data.DataModels;

public class User
{
    [Required] [Key] public int id { get; set; }
    [Required] public string? fullName { get; set; }
    public string? studyField { get; set; }
    public string? contactDetails { get; set; }
    public virtual List<Project>? Projects { get; set; }
    
}

public class AuthUser : User
{
    [Required] public string? username { init; get; }
    public string? password { set; get; }
    public bool? globalAdmin { get; init; }
}

