using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvidenceProject.Data.DataModels;

public sealed class Project
{
    /// <summary> Unikátní identifikátor záznamu.
    /// </summary>
    [Key] public int id { init; get; }
    /// <summary> Název projektu. Max 50 znaků.
    /// </summary>
    [Required] [StringLength(50)] public string? name { get; set; }
    /// <summary> Stav projektu - položka z číselníku (předdefinovaného výběru).
    /// </summary>
    [Required] [ForeignKey("State")] public DialCode? projectState { get; set; }
    /// <summary> Typ projektu - položka z předdefinovaného výběru.
    /// </summary>
    [Required] [ForeignKey("Type")] public DialCode? projectType { get; set; }
    /// <summary> Technologie projektu - položka z předdefinovaného výběru.
    /// </summary>
    [Required] [ForeignKey("Technology")] public DialCode? projectTechnology { get; set; }
    /// <summary> Úspechy projektu.
    /// </summary>
    [Required] public List<Achievement>? projectAchievements { get; set; }

    /// <summary> Žáci přiřazení k projektu.
    /// </summary>
    [Required] public List<User>? assignees { get; set; }
    /// <summary> Ověřený uživatel spravující projekt.
    /// </summary>
    [ForeignKey("AuthUser")] public AuthUser? projectManager;

    //TODO Artefacts(Files, text) 
    //TODO Showcase??(Files?, Photos?)

    /// <summary> Url adresa Github repozitáře.
    /// </summary>
    [StringLength(100)]
    public string? github { get; set; }
    /// <summary> Url adresa slack kanálu.
    /// </summary>
    [StringLength(100)]
    public string? slack { get; set; }

    public Project(string name, DialCode projectState, DialCode projectTechnology, List<Achievement> projectAchievements, List<User> assignees, AuthUser projectManager, string github, string slack) {
        this.name = name;
        this.projectState = projectState;
        this.projectTechnology = projectTechnology;
        this.projectAchievements = projectAchievements;
        this.assignees = assignees;
        this.projectManager = projectManager;
        this.github = github;
        this.slack = slack;
    }
}