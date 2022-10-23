using System.ComponentModel.DataAnnotations;

namespace EvidenceProject.Data.DataModels
{
    public class DialInfo
    {
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(50)]
        public string? name { get; set; }
        [StringLength(200)]
        public string? desc { get; set; }

        public virtual List<DialCode>? dialCodes { get; set; }
    }
}
