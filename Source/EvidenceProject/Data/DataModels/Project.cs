using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvidenceProject.Data.DataModels;

public sealed class Project
{
    /// <summary> Unikátní identifikátor záznamu.
    /// </summary>
    [Key] public int _id { init; get; }
    /// <summary> Název projektu. Max 50 znaků.
    /// </summary>
    [Required] [StringLength(50)] public string? _name { get; set; }
    /// <summary> Stav projektu - položka z číselníku (předdefinovaného výběru).
    /// </summary>
    [Required] [ForeignKey("State")] public DialCode? _projectState { get; set; }
    /// <summary> Typ projektu - položka z předdefinovaného výběru.
    /// </summary>
    [Required] [ForeignKey("Type")] public DialCode? _projectType { get; set; }
    /// <summary> Technologie projektu - položka z předdefinovaného výběru.
    /// </summary>
    [Required] [ForeignKey("Technology")] public DialCode? _projectTechnology { get; set; }
    /// <summary> Úspechy projektu.
    /// </summary>
    [Required] public List<Achievement>? _projectAchievements { get; set; }

    /// <summary> Žáci přiřazení k projektu.
    /// </summary>
    [Required] public List<User>? _assignees { get; set; }
    /// <summary> Ověřený uživatel spravující projekt.
    /// </summary>
    [ForeignKey("AuthUser")] public AuthUser? _projectManager;

    //TODO Artefacts(Files, text) 
    //TODO Showcase??(Files?, Photos?)

    /// <summary> Url adresa Github repozitáře.
    /// </summary>
    [StringLength(100)]
    public string? _github { get; set; }
    /// <summary> Url adresa slack kanálu.
    /// </summary>
    [StringLength(100)]
    public string? _slack { get; set; }

    public Project(string name, DialCode projectState, DialCode projectTechnology, List<Achievement> projectAchievements, List<User> assignees, AuthUser projectManager, string github, string slack) {
        _name = name;
        _projectState = projectState;
        _projectTechnology = projectTechnology;
        _projectAchievements = projectAchievements;
        _assignees = assignees;
        _projectManager = projectManager;
        _github = github;
        _slack = slack;
    }
}