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
    public virtual DialCode? projectState { get; set; }

    /// <summary>
    ///     Typ projektu - položka z předdefinovaného výběru.
    /// </summary>
    [Required]
    [ForeignKey("Type")]
    public virtual DialCode? projectType { get; set; }

    /// <summary>
    ///     Technologie projektu - položka z předdefinovaného výběru.
    /// </summary>
    [Required]
    [ForeignKey("Technology")]
    public virtual List<DialCode>? projectTechnology { get; set; }

    /// <summary>
    ///     Úspechy projektu.
    /// </summary>
    [Required]
    public virtual List<Achievement>? projectAchievements { get; set; }

    /// <summary>
    ///     Žáci přiřazení k projektu.
    /// </summary>
    [Required]
    [Obsolete("Nebude potřeba, uživatelé se nemusí regstrovat, list<string> ? nebo se to bude dávat do desc")]
    public virtual List<User>? assignees { get; set; }

    /// <summary>
    ///     Subory projektu.
    /// </summary>
    [Required]
    public virtual List<DbFile>? files { get; set; }

    /// <summary>
    ///     Url adresa Github repozitáře.
    /// </summary>
    [StringLength(100)]
    public string? github { get; set; }

    /// <summary>
    ///     Url adresa slack kanálu.
    /// </summary>
    [StringLength(100)]
    public string? slack { get; set; }
}