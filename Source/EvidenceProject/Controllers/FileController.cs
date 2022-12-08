namespace EvidenceProject.Controllers;

public class FileController : Controller
{
    private readonly ProjectContext _context;

    public FileController(ProjectContext context) => _context = context;


    [HttpPost("/file/edit/{folder}/{filename}")]
    public ActionResult Edit(string folder, string filename, string fileData)
    {
        string path = $"./wwwroot/{folder}/{filename}";

        try
        {
            System.IO.File.WriteAllText(path, fileData);
        }
        catch { }

        return Redirect("/user/profile");
    }

    [HttpGet("/file/{generatedFileName}")]
    public ActionResult GetFile(string generatedFileName)
    {
        var file = _context.files.FirstOrDefault(x => x.generatedFileName == generatedFileName);
        if (file == null) return NotFound();

        return File(file.fileData, file.mimeType, fileDownloadName: file.originalFileName);
    }
}
