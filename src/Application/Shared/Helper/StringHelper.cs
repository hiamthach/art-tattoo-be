namespace art_tattoo_be.Application.Shared.Helper;

using BinaryAnalysis.UnidecodeSharp;

public static class StringHelper
{
  public static string ConvertVietnamese(string text)
  {
    return text.Unidecode();
  }
}
