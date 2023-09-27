namespace art_tattoo_be.Core.Crypto;

using System.Security.Cryptography;

public class CryptoService
{
  public static string HashPassword(string password)
  {
    using var sha256 = SHA256.Create();
    var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
  }

  public static bool VerifyPassword(string password, string hashedPassword)
  {
    return HashPassword(password) == hashedPassword;
  }
}
