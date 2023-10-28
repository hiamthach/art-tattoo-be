using System.Globalization;

namespace art_tattoo_be.Application.Shared.Helper;

public static class DateHelper
{
  public static int GetWeekNumber(DateTime date)
  {
    Calendar calendar = CultureInfo.CurrentCulture.Calendar;
    int weekNumber = calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    return weekNumber;
  }
}
