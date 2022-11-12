using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace EvidenceProject.Data.DataModels
{
    public class DialCode
    {
        /// <summary> Unikátní identifikátor záznamu.
        /// </summary>
        [Key]  
        public int id { get; set; }

        /// <summary> Název/obsah číselníkového záznamu - max 50 znaků.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string? name { get; set; }

        /// <summary> Barevnost vizualizace záznamu.
        /// </summary>
        [NotMapped]
        public Color? color { get => Color.FromArgb(_color); set => _color = value?.ToArgb() ?? 0; }

        /// <summary> Číselný záznam barvy pro uložení do DB.
        /// </summary>
        [Required]
        private int _color { get; set; }

        /// <summary> Popis záznamu číselníku. Max 200 znaků.
        /// </summary>
        [StringLength(200)]
        public string? description { get; set; }

        /// <summary> Kategorie, číselníků do které záznam zapadá.
        /// </summary>
        [Required]
        public virtual DialInfo? dialInfo { get; set; }
    }
}
