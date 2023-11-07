namespace art_tattoo_be.Application.DTOs.Shift;

public class CreateShift
{
  public DateTime Start { get; set; }
  public DateTime End { get; set; }
}

public class GenerateShift
{
  public TimeSpan ShiftDuration { get; set; }
}
