using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace EvidenceProject.Data.DataModels;
// item v Kategorii
public class DialCode
{
    /// <summary>
    ///     Konstruktor vytvoří záznam v číselníku
    /// </summary>
    /// <param name="name"></param>
    /// <param name="dialInfo"></param>
    /// <param name="color"></param>
    /// <param name="description"></param>
    public DialCode(string name, DialInfo? dialInfo, Color? color = null, string? description = "")
    {
        this.name = name;
        this.color = color ?? Color.White;
        this.description = description;
        this.dialInfo = dialInfo;
    }

    /// <summary>
    ///     Prázdný konstruktor záznamu
    /// </summary>
    public DialCode()
    {
        color = Color.White;
    }

    /// <summary>
    ///     Unikátní identifikátor záznamu.
    /// </summary>
    [Key]
    public int id { get; private set; }

    /// <summary>
    ///     Název/obsah číselníkového záznamu - max 50 znaků
    /// </summary>
    [Required]
    public string? name { get; set; }

    /// <summary>
    ///     Barevnost vizualizace záznamu.
    /// </summary>
    [NotMapped]
    public Color? color
    {
        get => Color.FromArgb(_color);
        set => _color = value?.ToArgb() ?? 0;
    }

    /// <summary>
    ///     Číselný záznam barvy pro uložení do DB.
    /// </summary>
    [Required]
    public int _color { get; set; }

    /// <summary>
    ///     Popis záznamu číselníku. Max 200 znaků.
    /// </summary>
    public string? description { get; set; }

    /// <summary>
    ///     Kategorie, číselníků do které záznam zapadá.
    /// </summary>
    [Required]
    public virtual DialInfo? dialInfo { get; set; }
}