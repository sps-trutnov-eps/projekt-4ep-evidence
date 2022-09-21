using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace EvidenceProject.Data.DataModels
{
    public class DialCode
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public Color? Color { get; set; }

        public string? Description { get; set; }

        [Required]
        public virtual DialInfo DialInfo { get; set; }
    }
}
