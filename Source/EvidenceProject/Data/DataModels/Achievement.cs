using System.ComponentModel.DataAnnotations;

namespace EvidenceProject.Data.DataModels
{
    public class Achievement
    {
        [Key, Required]
        public string? name { get; set; }
        [Required]
        public Project? project { get; set; }
    }
}
