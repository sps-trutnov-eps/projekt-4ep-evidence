using System.ComponentModel.DataAnnotations;

namespace EvidenceProject.Data.DataModels
{
    public class DialInfo
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string? name { get; set; }
        public string? desc { get; set; }

        public virtual List<DialCode>? dialCodes { get; set; }
    }
}
