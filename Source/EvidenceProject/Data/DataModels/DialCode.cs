using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace EvidenceProject.Data.DataModels
{
    public class DialCode
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public Color? Color { get; set; }

        public string? Description { get; set; }
    }
}
