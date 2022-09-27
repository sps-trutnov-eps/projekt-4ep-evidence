using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace EvidenceProject.Helpers;

public class PasswordHelper {
    // hesla nejsou posolena
    static public string CreateHash(string input)
    {
        var argon = new Argon2id(Encoding.UTF8.GetBytes(input));
        argon.DegreeOfParallelism = 8;
        argon.Iterations = 4;
        argon.MemorySize = 1024;

        return Convert.ToBase64String(argon.GetBytes(16));
    }

    static public bool VerifyHash(string input, string hash)
    {
        var novejHash = CreateHash(input);
        return novejHash == hash;
    }

}