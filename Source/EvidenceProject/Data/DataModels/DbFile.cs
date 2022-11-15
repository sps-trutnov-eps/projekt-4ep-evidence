using System.ComponentModel.DataAnnotations;

namespace EvidenceProject.Data.DataModels;

public class DbFile
{
    /// <summary>
    ///     Unikátní identifikátor souboru
    /// </summary>
    [Key] public Guid id { get; set; }

    /// <summary>
    ///     Název souboru
    /// </summary>
    [Required]
    public string fileName { get; set; }

    /// <summary>
    ///     Data souboru
    /// </summary>
    [Required]
    public byte[] fileData { get; set; }
    
    /// <summary>
    ///     Mime type souboru
    /// </summary>
    [Required] public string mimeType { get; set; }
}

public static class FileExtension
{
    /// <summary>
    ///     Čtení souboru z databáze
    /// </summary>
    /// <returns>
    ///     <see cref="MemoryStream" /> from <see cref="DbFile" />
    /// </returns>
    public static MemoryStream ReadFile(this DbFile file)
    {
        var memoryStream = new MemoryStream();
        memoryStream.Write(file.fileData, 0, file.fileData.Length);
        return memoryStream;
    }
    
    /// <summary>
    ///     Write file to database
    /// </summary>
    /// <param name="file"></param>
    /// <param name="formFile"><see cref="IFormFile"/> to write</param>
    public static void WriteFile(this DbFile file, IFormFile formFile)
    {
        using var memoryStream = new MemoryStream();
        formFile.CopyTo(memoryStream);
        file.fileData = memoryStream.ToArray();
        file.mimeType = formFile.ContentType;
    }
}