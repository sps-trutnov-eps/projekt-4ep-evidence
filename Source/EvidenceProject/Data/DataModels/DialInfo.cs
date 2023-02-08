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

public static class DialInfoExtencion
{
    public static bool WasChecked = false;

    public static void CreateIFNotDefaults(ProjectContext context)
    {
        var dials = context.dialInfos;

        if (dials?.FirstOrDefault(i => i.name == "Stav") == null)
            dials?.Add(new DialInfo() { name = "Stav", desc = "Stav projektu." });
        if (dials?.FirstOrDefault(i => i.name == "Technologie") == null)
            dials?.Add(new DialInfo() { name = "Technologie", desc = "Technologie projektu." });
        if (dials?.FirstOrDefault(i => i.name == "Typ") == null)
            dials?.Add(new DialInfo() { name = "Typ", desc = "Typ projektu." });

        WasChecked = true;

        context.SaveChanges();
    }
}