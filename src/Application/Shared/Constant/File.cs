namespace art_tattoo_be.Application.Shared.Constant;

public static class FileConst
{
  public const string IMAGE = "image";
  public const string VIDEO = "video";
  public const string CERT = "cert";

  public const int MAX_IMAGE_SIZE = 5 * 1024 * 1024; // 5MB
  public const int MAX_VIDEO_SIZE = 20 * 1024 * 1024; // 20MB
  public const int MAX_CERT_SIZE = 5 * 1024 * 1024; // 5MB

  public static readonly string[] IMAGE_CONTENT_TYPES = { "image/jpeg", "image/png", "image/gif", "image/webp" };
  public static readonly string[] VIDEO_CONTENT_TYPES = { "video/mp4", "video/avi", "video/quicktime", "video/x-ms-wmv", "video/x-flv", "video/x-matroska" };
  public static readonly string[] CERT_CONTENT_TYPES = { "application/pdf", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" };
}
