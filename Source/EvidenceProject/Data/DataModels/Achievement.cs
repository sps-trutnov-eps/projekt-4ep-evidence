using System.ComponentModel.DataAnnotations;

namespace EvidenceProject.Data.DataModels
{
    public class Achievement
    {
        [Key, Required]
        [StringLength(50)]
        public string? name { get; set; }
        [Required]
        public Project? project { get; set; }
        
        //TODO Files/Photos
    }
}
