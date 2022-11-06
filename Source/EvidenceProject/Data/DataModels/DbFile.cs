using System.ComponentModel.DataAnnotations;
using System.Data;
using Microsoft.Data.SqlClient;

namespace EvidenceProject.Data.DataModels;

public class DbFile
{
    //Any file - https://stackoverflow.com/questions/2579373/saving-any-file-to-in-the-database-just-convert-it-to-a-byte-array
    //Image - http://www.binaryintellect.net/articles/2f55345c-1fcb-4262-89f4-c4319f95c5bd.aspx
    [Key]
    public Guid id { get; set; }
    
    /// <summary> Název souboru
    /// </summary>
    [Required]
    public string fileName { get; set; }
    
    /// <summary> Data souboru
    /// </summary>
    [Required]
    public byte[] fileData { get; set; }
    
}

public static class FileExtension
{
    /// <summary>
    /// Read file from database
    /// </summary>
    /// <returns>
    /// <see cref="MemoryStream"/> from <see cref="DbFile"/>
    /// </returns>
    public static MemoryStream ReadFile(this DbFile file) {
        MemoryStream memoryStream = new MemoryStream();
        memoryStream.Write(file.fileData, 0, file.fileData.Length);
        return memoryStream;
    }

    /// <summary>
    /// Write file to database
    /// </summary>
    /// <param name="file"></param>
    /// <param name="stream"><see cref="MemoryStream"/> to write</param>
    public static void WriteFile(this DbFile file, MemoryStream stream) {
        file.fileData = new byte[stream.Length];
        stream.Read(file.fileData, 0, (int)stream.Length);
    }
}

