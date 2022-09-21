using System.ComponentModel.DataAnnotations;

namespace EvidenceProject.Data.DataModels
{
    public class DialInfo
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Desc { get; set; }

        public virtual List<DialCode> DialCodes { get; set; }
    }
}
