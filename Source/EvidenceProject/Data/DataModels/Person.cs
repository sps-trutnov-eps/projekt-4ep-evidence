using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EvidenceProject.Data.DataModels;

[Keyless]
public class Person
{
    [Required] public string name;
    public string studyField;
    public string contactDetails;
    
}
