using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace EvidenceProject.Data.DataModels
{
    public class DialCode
    {
        [Key]  
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string? name { get; set; }

        [NotMapped]
        public Color? color { get => Color.FromArgb(_color); set => _color = value?.ToArgb() ?? 0; }

        [Required]
        public int _color { get; set; }

        [StringLength(200)]
        public string? description { get; set; }

        [Required]
        public virtual DialInfo? dialInfo { get; set; }
    }
}
