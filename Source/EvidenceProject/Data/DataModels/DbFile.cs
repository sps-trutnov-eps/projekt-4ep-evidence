using System.ComponentModel.DataAnnotations;

namespace EvidenceProject.Data.DataModels;

public class DbFile // https://stackoverflow.com/questions/2579373/saving-any-file-to-in-the-database-just-convert-it-to-a-byte-array
{
    [Key]
    public Guid id { get; set; }
    [Required]
    public string fileName { get; set; }
    [Required]
    public byte[] fileData { get; set; }
}