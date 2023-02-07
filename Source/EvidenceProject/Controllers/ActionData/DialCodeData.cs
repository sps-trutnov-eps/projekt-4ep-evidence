namespace EvidenceProject.Controllers.ActionData;

/// <summary>
/// <see cref="DialStuffController.AddDialCode(DialCodeData?)"/>
/// </summary>
public class DialCodeData
{
    public string? Name { get; set; } = "";
    public string? Description { get; set; } = "";
    public string? DialInfoName { get; set; }
    public string? Color { get; set; }
}