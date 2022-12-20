using System.ComponentModel.DataAnnotations;

namespace EvidenceProject.Data.DataModels;

// "Kategorie"
public class DialInfo
{
    /// <summary>
    ///     Unikátní identifikátor kategorie číselníkových záznamů.
    /// </summary>
    [Key]
    public int id { get; private set; }

    /// <summary>
    ///     Název/obsah kategrie. Max 50 znaků.
    /// </summary>
    [Required]
    public string? name { get; set; }

    /// <summary>
    ///     Popis kategorie záznamu. Max 200 znaků.
    /// </summary>
    public string? desc { get; set; }

    /// <summary>
    ///     Obsah kategorie -> stavy v options.
    /// </summary>
    public virtual List<DialCode>? dialCodes { get; set; } = new();
}