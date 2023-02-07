using System.ComponentModel.DataAnnotations;

namespace EvidenceProject.Data.DataModels;

public class Achievement
{
    /// <summary>
    ///     Název/obsah úspěchu
    /// </summary>
    [Key]
    [Required]
    public string? name { get; set; }

    /// <summary>
    ///     Přiřazený projekt
    /// </summary>
    [Required]
    public Project? project { get; set; }

    //TODO Files/Photos
}