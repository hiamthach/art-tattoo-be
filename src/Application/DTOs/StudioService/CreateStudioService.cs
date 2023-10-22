namespace art_tattoo_be.src.Application.DTOs.StudioService;

using System;

public class CreateStuioServiceReq
{
  public Guid StudioId { get; set; }
  public int CategoryId { get; set; }
  public string Name { get; set; } = null!;
  public string Description { get; set; } = null!;
  public double MinPrice { get; set; }
  public double MaxPrice { get; set; }
  public double Discount { get; set; }
}
