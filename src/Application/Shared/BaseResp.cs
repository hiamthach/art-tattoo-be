namespace art_tattoo_be.Application.Shared;

public class BaseResp
{
  public string Message { get; set; }
}

public class ErrResp
{
  public string Error { get; set; } = "Bad Request";
  public int Code { get; set; }
}