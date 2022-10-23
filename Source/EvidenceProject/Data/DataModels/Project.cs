using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvidenceProject.Data.DataModels;

public class Project
{
    [Key] public int id { init; get; }
    [Required] [StringLength(50)] public string? name { get; set; }
    [Required] [ForeignKey("State")] public virtual DialCode? projectState { get; set; }
    [Required] [ForeignKey("Type")] public virtual DialCode? projectType { get; set; }
    [Required] [ForeignKey("Technology")] public virtual DialCode? projectTechnology { get; set; }
    [Required] public virtual List<Achievement>? projectAchievements { get; set; }

    [Required] public virtual List<User>? assignees { get; set; }
    [ForeignKey("AuthUser")] public AuthUser? projectManager;

    //TODO Artefacts(Files, text) 
    //TODO Showcase??(Files?, Photos?)

    [StringLength(100)]
    public string? github { get; set; }
    [StringLength(100)]
    public string? slack { get; set; }
}