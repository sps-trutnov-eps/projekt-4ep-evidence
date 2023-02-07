using System.ComponentModel.DataAnnotations;
using bcrypt = BCrypt.Net.BCrypt;

namespace EvidenceProject.Data.DataModels;

public class User
{
    /// <summary>
    ///     Id uživatele
    /// </summary>
    [Required]
    [Key]
    public int id { get; private set; }

    /// <summary>
    ///     Celé jméno uživatele
    /// </summary>
    [Required]
    public string? fullName { get; set; }

    /// <summary>
    ///     Studijní obor uživatele
    /// </summary>
    public string? studyField { get; set; }

    /// <summary>
    ///     Ročník žáka
    /// </summary>
    public byte? schoolYear { get; set; }

    /// <summary>
    ///     Kontaktní údaje uživatele
    /// </summary>
    public string? contactDetails { get; set; }

    /// <summary>
    ///     Projekty, ve kterých je zapojen
    /// </summary>
    public virtual List<Project>? Projects { get; set; }
}

public class AuthUser : User
{

    /// <summary>
    ///     Login ověřeného uživatele - je unikátní
    /// </summary>
    [Required]
    public string? username { get; set; }

    /// <summary>
    ///     Heslo ověřeného uživatele
    /// </summary>
    [Required]
    public string? password { set; get; }

    /// <summary>
    ///     Pakliže je globální admin = true
    /// </summary>
    public bool? globalAdmin { get; init; }

    /// <summary>
    ///     Uživatelův unikátní klíč -> Je jím jednoznačně identifikován v session
    /// </summary>
    [Required]
    public string? id_key { get; private set; }

    /// <summary>
    ///     Konstruktor - vytvoří a zkontroluje ID_Key podle <paramref name="context" />.
    /// </summary>
    /// <param name="context">DBContext, pakliže je null hodnota ID_key se nekontroluje.</param>
    public AuthUser(ProjectContext? context = null)
    {
        GenerateIdKey(context);
    }

    /// <summary>
    ///     Konstruktor - Nastaví Obj. dle vložených parametrů, automaticky hashuje heslo (bcrypt).
    /// </summary>
    /// <param name="login">Username ověřeného uživatele.</param>
    /// <param name="pass">Heslo - Hashuje se automaticky.</param>
    /// <param name="fullname">Celé jméno.</param>
    /// <param name="studyfield">Studijní obor.</param>
    /// <param name="contact">Kontaktní údaje.</param>
    /// <param name="admin">Je global admin?</param>
    /// <param name="context">DB context - pro kontrolu unikátnosti ID_Key v rámci Db.</param>
    public AuthUser(string login, string pass, string fullname = "", string studyfield = "", string contact = "", byte year = 0,
        bool admin = false, ProjectContext? context = null)
    {
        username = login;
        password = pass;
        GenerateIdKey(context);

        fullName = fullname;
        studyField = studyfield;
        contactDetails = contact;

        schoolYear = year;
        globalAdmin = admin;
    }

    /// <summary>
    ///     Knstruktor Admina !!!
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="context"></param>
    public AuthUser(string username, string password, ProjectContext context = null)
    {
        this.username = username;
        this.password = bcrypt.HashPassword(password);
        this.GenerateIdKey(context);
        this.globalAdmin = true;

        fullName = "Admin";
        studyField = "Admin";
        contactDetails = "Admin";

        schoolYear = 0;
    }

    /// <summary>
    ///     Získání uživatele dle ID - najde uživatele v dodaném <paramref name="context" />, pokud uživatel s daným id neexistuje vrátí null.
    /// </summary>
    /// <param name="context">DBContext/param>
    /// <param name="user_id">ID uživatele kterého hledáme</param>
    static public AuthUser? FindUser(ProjectContext context, int user_id)
    {
        return context?.globalUsers?.FirstOrDefault(user => user.id == user_id);
    }

    /// <summary>
    ///     Metoda nastaví obj. vygenerovaný a ověřený <see cref="id_key" />
    /// </summary>
    /// <param name="context">DB pro zkontrolování unikátnosti vygenerovaného ID_key.</param>
    /// <param name="keyLength">Délka Id_key ve znacích.</param>
    public void GenerateIdKey(ProjectContext? context = null, int keyLength = 32)
    {
        id_key = ReturnIdKey(keyLength, context);
    }

    /// <summary>
    ///     Metoda vrátí vygenerovaný <see cref="id_key" />
    /// </summary>
    /// <param name="keyLength">Délka řetězce v char.</param>
    /// <param name="context">DB pro zkontrolování unikátnosti vygenerovaného ID_key.</param>
    /// <returns>Vrátí náhodně vygenerovaný řetězec znaků.</returns>
    private string ReturnIdKey(int keyLength, ProjectContext? context = null)
    {
        var key = Guid.NewGuid().ToString();
        return key;
    }
}