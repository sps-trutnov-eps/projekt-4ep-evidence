using System.ComponentModel.DataAnnotations;

namespace EvidenceProject.Data.DataModels;

public class Project
{
    [Key] public Guid id { init; get; }
    [Required] public string name { get; set; }
    [Required] public ProjectState projectState { get; set; }
    [Required] public List<Person> assignees { get; set; }
    
    
}