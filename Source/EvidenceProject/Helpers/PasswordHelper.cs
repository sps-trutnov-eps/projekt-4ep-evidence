using System.Text;
using Konscious.Security.Cryptography;

namespace EvidenceProject.Helpers;

public class PasswordHelper
{
    //  Zm�na argonu na bcrypt?
    //  Je to p�ipraveno v modelech
    public static string CreateHash(string input)
    {
        var argon = new Argon2id(Encoding.UTF8.GetBytes(input));
        argon.Salt = Encoding.UTF8.GetBytes("00000000");
        argon.DegreeOfParallelism = 8;
        argon.Iterations = 4;
        argon.MemorySize = 1024;

        return Convert.ToHexString(argon.GetBytes(16));
    }

    public static bool VerifyHash(string input, string hash)
    {
        var novejHash = CreateHash(input);
        return novejHash == hash;
    }
}