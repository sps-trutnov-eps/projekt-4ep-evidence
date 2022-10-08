using EvidenceProject.Helpers;

namespace Tests.HelperTests;

public class PasswordHelperTests
{
    [Test]
    [TestCase("hello", "248a255ef47d77649bebd5a2cd43a05e")]
    [TestCase("google", "88942e9114d959a728f11c7bc06f83a4")]
    [TestCase("VeryHardPassword#%@!&%&", "b65685d89e1f81fa7713f80b7c6e8ef1")]
    public void HashTest(string password, string hash)
    {
        hash = hash.ToUpper();
        var hashed = PasswordHelper.CreateHash(password);
        Assert.AreEqual(hashed, hash);
    }

    [Test]
    [TestCase("heslo", "ee5074a577baa9ff3356f04a08218661", true)]
    [TestCase("password", "88942e9114d959a728f11c7bc06f83a4", false)]
    [TestCase("VeryHardPassword#%@!&%&", "b65685d89e1f81fa7713f80b7c6e8ef1", true)]
    public void VerifyHashTest(string password, string passwordHash, bool expected)
    {
        passwordHash = passwordHash.ToUpper();
        bool result = PasswordHelper.VerifyHash(password, passwordHash);
        Assert.AreEqual(expected, result);
    }
}

