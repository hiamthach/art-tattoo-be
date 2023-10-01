namespace art_tattoo_be.Core.Crypto;

using Xunit;

public class CryptoServiceTests
{
  [Fact]
  public void HashPassword_ReturnsValidHash()
  {
    // Arrange
    var password = "password";

    // Act
    var hashedPassword = CryptoService.HashPassword(password);

    // Assert
    Assert.NotNull(hashedPassword);
    Assert.NotEqual(password, hashedPassword);
  }

  [Fact]
  public void VerifyPassword_ReturnsTrueForValidPassword()
  {
    // Arrange
    var password = "password";
    var hashedPassword = CryptoService.HashPassword(password);

    // Act
    var result = CryptoService.VerifyPassword(password, hashedPassword);

    // Assert
    Assert.True(result);
  }

  [Fact]
  public void VerifyPassword_ReturnsFalseForInvalidPassword()
  {
    // Arrange
    var password = "password";
    var hashedPassword = CryptoService.HashPassword(password);
    var invalidPassword = "invalid";

    // Act
    var result = CryptoService.VerifyPassword(invalidPassword, hashedPassword);

    // Assert
    Assert.False(result);
  }
}
