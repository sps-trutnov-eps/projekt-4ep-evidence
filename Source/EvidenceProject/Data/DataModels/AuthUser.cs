using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EvidenceProject.Data.DataModels;

public class User
{
    [Required] [Key] public int id { get; set; }
    [Required] [StringLength(35)] public string? fullName { get; set; }
    [StringLength(50)] public string? studyField { get; set; }
    [StringLength(100)] public string? contactDetails { get; set; }
    public virtual List<Project>? Projects { get; set; }
    
}

public class AuthUser : User
{
    [Required] [StringLength(25)] public string? username { init; get; }
    public string? password { set; get; }
    public bool? globalAdmin { get; init; }
}

