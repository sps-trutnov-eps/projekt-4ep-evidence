using System.ComponentModel.DataAnnotations;
using System.Drawing;
using Microsoft.EntityFrameworkCore;

namespace EvidenceProject.Data.DataModels;

public class ProjectState
{
    [Key] public string name;
    public Color color;
}