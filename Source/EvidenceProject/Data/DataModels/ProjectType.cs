using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace EvidenceProject.Data.DataModels;

public class ProjectType
{
    [Key] private string? name { get; set; }
    public Color color { get; set; }
}