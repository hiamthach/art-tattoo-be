namespace art_tattoo_be.Application.DTOs.Analytics;

public class AdminDashboard
{
  public required StudioAdminDashboard StudioData { get; set; }
  public required UserAdminDashboard UserData { get; set; }
  public required TestimonialAdminDashboard TestimonialData { get; set; }
  public required BookingAdminDashboard BookingData { get; set; }
}

public class StudioAdminDashboard
{
  public int TotalStudio { get; set; }
  public int TotalStudioThisMonth { get; set; }
  public int TotalStudioLastMonth { get; set; }
}

public class UserAdminDashboard
{
  public int TotalUser { get; set; }
  public int TotalUserThisMonth { get; set; }
  public int TotalUserLastMonth { get; set; }
}

public class TestimonialAdminDashboard
{
  public double AvgTestimonial { get; set; }
  public int TotalTestimonial { get; set; }
}

public class BookingAdminDashboard
{
  public int TotalBooking { get; set; }
  public int TotalBookingThisMonth { get; set; }
  public int TotalBookingLastMonth { get; set; }
}
