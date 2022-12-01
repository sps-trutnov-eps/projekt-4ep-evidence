using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EvidenceProject.Data.DataModels;

public class Project
{
    /// <summary>
    ///     Ověřený uživatel spravující projekt.
    /// </summary>
    [ForeignKey("AuthUser")] public AuthUser? projectManager;

    /// <summary>
    ///     Unikátní identifikátor záznamu.
    /// </summary>
    [Key]
    public int id { init; get; }

    /// <summary>
    ///     Název projektu. 
    /// </summary>
    [Required]
    public string? name { get; set; }
    
    /// <summary>
    ///     Popis projektu. 
    /// </summary>
    [ForeignKey("Description")]
    public string? projectDescription { get; set; }

    /// <summary>
    ///     Stav projektu - položka z číselníku (předdefinovaného výběru).
    /// </summary>
    [Required]
    [ForeignKey("State")]
    virtual public DialCode? projectState { get; set; }

    /// <summary>
    ///     Typ projektu - položka z předdefinovaného výběru.
    /// </summary>
    [Required]
    [ForeignKey("Type")]
    virtual public DialCode? projectType { get; set; }

    /// <summary>
    ///     Technologie projektu - položka z předdefinovaného výběru.
    /// </summary>
    [Required]
    [ForeignKey("Technology")]
    virtual public List<DialCode>? projectTechnology { get; set; }

    /// <summary>
    ///     Úspechy projektu.
    /// </summary>
    [Required]
    virtual public List<Achievement>? projectAchievements { get; set; }

    /// <summary>
    ///     Žáci přiřazení k projektu.
    /// </summary>
    [Required]
    [Obsolete("Nebude potřeba, uživatelé se nemusí regstrovat, list<string> ? nebo se to bude dávat do desc")]
    virtual public List<User>? assignees { get; set; }

    /// <summary>
    ///     Subory projektu.
    /// </summary>
    [Required]
    virtual public List<DbFile>? files { get; set; }

    /// <summary>
    ///     Url adresa Github repozitáře.
    /// </summary>
    public string? github { get; set; }

    /// <summary>
    ///     Url adresa slack kanálu.
    /// </summary>
    public string? slack { get; set; }
}