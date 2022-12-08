namespace EvidenceProject.Controllers;

public class FileController : Controller
{
    private readonly ProjectContext _context;

    public FileController(ProjectContext context) => _context = context;

    [HttpGet("/file/{generatedFileName}")]
    public ActionResult GetFile(string generatedFileName)
    {
        var file = _context.files.FirstOrDefault(x => x.generatedFileName == generatedFileName);
        if (file == null) return NotFound();

        return File(file.fileData, file.mimeType, fileDownloadName: file.originalFileName);
    }
}
