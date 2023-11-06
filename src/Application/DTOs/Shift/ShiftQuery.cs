namespace art_tattoo_be.Application.DTOs.Shift;

public class ShiftQueryReq
{
  public DateTime Start { get; set; }
  public DateTime End { get; set; }
  public Guid StudioId { get; set; }
  public Guid? ArtistId { get; set; }
}

public class ShiftQuery : ShiftQueryReq
{
  public bool IsStudio { get; set; }
}

public class ShiftQueryDate
{
  public DateTime Start { get; set; }
  public DateTime End { get; set; }
}
