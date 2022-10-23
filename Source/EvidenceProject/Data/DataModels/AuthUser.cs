using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using bcrypt = BCrypt.Net.BCrypt;

namespace EvidenceProject.Data.DataModels;

public class User
{
    [Required] [Key] public int id { get; set; }
    [Required] [StringLength(35)] public string? fullName { get; set; }
    [StringLength(50)] public string? studyField { get; set; }
    [StringLength(100)] public string? contactDetails { get; set; }
    public virtual List<Project>? Projects { get; set; }
    
}

public class AuthUser : User
{
    [Required] [StringLength(25)] public string? username { init; get; }
    [Required] public string? password { set; get; }
    public bool? globalAdmin { get; init; }
    [Required] public string? id_key { get; private set; }

    [NotMapped]
    private bool notHashedPass = false;

    public AuthUser(ProjectContext? context = null)
    {
        notHashedPass = true;
        GenerateIdKey(context);
    }

    public AuthUser(string login, string pass, string fullname = "", string studyfield = "", string contact = "", bool admin = false, ProjectContext? context = null)
    {
        username = login;
        HashPassword(pass);
        GenerateIdKey(context);

        fullName = fullname;
        studyField = studyfield;
        contactDetails = contact;

        globalAdmin = admin;
    }

    ~AuthUser()
    {
        if (notHashedPass)
        {
            HashPassword();
        }
    }

    public void HashPassword(string? pass = null)
    {
        password = bcrypt.HashPassword(pass ?? password);

        notHashedPass = false;
    }

    public bool VerifyPassword(string passToCompare)
    {
        if (bcrypt.Verify(passToCompare, password)) { return true; }

        return false;
    }

    public void GenerateIdKey(ProjectContext? context = null, int keyLength = 32)
    {
        id_key = ReturnIdKey(keyLength, context);
    }

    private string ReturnIdKey(int keyLength, ProjectContext? context = null)
    {
        string key = "";

        do
        {
            key = "";
            Random rand = new Random();
            for (int i = 0; i < keyLength; i++)
            {
                int cislo = rand.Next(48, 122);
                key += Convert.ToChar(cislo).ToString();
            }
        } while (context?.globalUsers?.Where(a => a.id_key == key).FirstOrDefault() != null);

        return key;
    }
}

