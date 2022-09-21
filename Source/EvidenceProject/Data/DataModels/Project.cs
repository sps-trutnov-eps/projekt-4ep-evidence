using System.ComponentModel.DataAnnotations;

namespace EvidenceProject.Data.DataModels;

public class Project
{
    [Key] public Guid id { init; get; }
    [Required] public string? name { get; set; }
    [Required] public DialCode? projectState { get; set; }
    [Required] public DialCode? projectType { get; set; }
    [Required] public DialCode? projectTechnology { get; set; }
    [Required] public List<DialCode>? projectAchievements { get; set; }

    [Required] public List<User>? assignees { get; set; }
    public AuthUser? projectManager;

    //TODO Artefacts(Files, text)
    //TODO Technology(List<string>???)
    //TODO Showcase??(Files?, Photos?)
    //TODO Accomplishments(Text tied with Photos??)
    
    public string? github { get; set; }
    public string? slack { get; set; }
}