namespace art_tattoo_be.Core.Crypto;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

public interface ICryptoService
{
  string HashPassword(string password);
  bool VerifyPassword(string password, string hashedPassword);
}

public class CryptoService
{
  public static string HashPassword(string password)
  {
    byte[] salt = new byte[128 / 8];
    using (var rng = RandomNumberGenerator.Create())
    {
      rng.GetBytes(salt);
    }

    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
      password: password!,
      salt: salt,
      prf: KeyDerivationPrf.HMACSHA256,
      iterationCount: 100000,
      numBytesRequested: 256 / 8));

    return hashed;
  }

  public static bool VerifyPassword(string password, string hashedPassword)
  {
    return HashPassword(password) == hashedPassword;
  }
}
