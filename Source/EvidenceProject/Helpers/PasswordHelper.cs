using Konscious.Security.Cryptography;
using System.Text;

namespace EvidenceProject.Helpers;
public class PasswordHelper 
{
    static public string CreateHash(string input)
    {
        var argon = new Argon2id(Encoding.UTF8.GetBytes(input));
        argon.Salt = Encoding.UTF8.GetBytes("00000000");
        argon.DegreeOfParallelism = 8;
        argon.Iterations = 4;
        argon.MemorySize = 1024;

        return Convert.ToHexString(argon.GetBytes(16));
    }

    static public bool VerifyHash(string input, string hash)
    {
        var novejHash = CreateHash(input);
        return novejHash == hash;
    }
}