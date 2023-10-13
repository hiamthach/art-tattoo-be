namespace art_tattoo_be.Core.Crypto;

using BCrypt.Net;

public interface ICryptoService
{
  string HashPassword(string password);
  bool VerifyPassword(string password, string hashedPassword);
}

public class CryptoService
{
  public static string HashPassword(string password)
  {
    return BCrypt.HashPassword(password);
  }

  public static bool VerifyPassword(string password, string hashedPassword)
  {
    return BCrypt.Verify(password, hashedPassword);
  }
}
