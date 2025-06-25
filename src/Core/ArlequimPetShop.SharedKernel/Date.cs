using System.Globalization;

namespace ArlequimPetShop.SharedKernel
{
    public static class Date
    {
        public static DateTime? LastWorkingDay(DateTime date, List<DateTime> holidays)
        {
            for (int i = -1; i >= -7; i--)
            {
                if (IsWorkingDay(date, holidays.Any(h => h.Date.Date == date.Date)))
                    return date.Date;

                date = date.Date.AddDays(-1);
            }

            return null;
        }

        public static DateTime? NextWorkingDay(DateTime date, List<DateTime> holidays)
        {
            for (int i = 1; i <= 7; i++)
            {
                if (IsWorkingDay(date, holidays.Any(h => h.Date.Date == date.Date)))
                    return date.Date;

                date = date.Date.AddDays(1);
            }

            return null;
        }

        public static bool IsWorkingDay(DateTime date, bool isHoliday)
        {
            var isWorkingDay = date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday && !isHoliday;

            return isWorkingDay;
        }

        public static (bool, DateTime?) IsValid(string date)
        {
            if (DateTime.TryParse(date, new CultureInfo("pt-BR"), DateTimeStyles.None, out DateTime dateValue))
                return (true, dateValue);

            return (false, null);
        }

        public static int CalculateAge(DateTime date, DateTime birthDate)
        {
            var age = date.Date.Year - birthDate.Date.Year;

            if (date < birthDate.AddYears(age))
                age--;

            return age;
        }

        public static (int, int, int) CalculateCompletedAge(DateTime date, DateTime birthDate)
        {
            int years = date.Year - birthDate.Year;
            int months = date.Month - birthDate.Month;
            int days = date.Day - birthDate.Day;

            if (days < 0)
            {
                months--;
                var ultimoDiaMesAnterior = DateTime.DaysInMonth(date.Year, date.Month - 1);
                days += ultimoDiaMesAnterior;
            }

            if (months < 0)
            {
                years--;
                months += 12;
            }

            return (years, months, days);
        }

        public static DateTime FormatToBrazil(DateTime date)
        {
            var newDate = new DateTime(date.Year, date.Month, date.Day);

            var formateDate = newDate.ToString("dd/MM/yyyy", CultureInfo.GetCultureInfo("pt-BR"));

            return Convert.ToDateTime(formateDate);
        }

        public static DateTime LastHourOfTheDay(DateTime date)
        {
            var dateAndOfTheDay = date.AddDays(1).AddMilliseconds(-1);

            return dateAndOfTheDay;
        }

        public static DateTime FirstHourOfTheDay(DateTime date)
        {
            var dateAndOfTheDay = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            return dateAndOfTheDay;
        }

        public static int CalculateWorkingDaysBetweenTwoDates(DateTime startDate, DateTime endDate, DateTime[] holidays, int subtractDays = 0)
        {
            var withHoliday = true;

            var dates = new List<DateTime>();

            for (var i = startDate.Date; i <= endDate.Date; i = i.AddDays(1))
                dates.Add(i);

            var totalBusinessDays = dates.Count(d => d.DayOfWeek != DayOfWeek.Saturday
                                                  && d.DayOfWeek != DayOfWeek.Sunday
                                                  && (withHoliday
                                                      && (holidays.Any()
                                                          && !holidays.Contains(d))));

            return totalBusinessDays - subtractDays;
        }

        public static int CalculateDays30(DateTime start, DateTime end)
        {
            if (end < start)
                return 0;

            // Calcula diferença de anos e converte para meses
            int yearDiff = end.Year - start.Year;
            int monthDiff = end.Month - start.Month;
            int totalMonths = (yearDiff * 12) + monthDiff;

            // Calcula diferença de dias dentro do mês (ajustando para 30 dias fixos)
            int dayDiff = (end.Day - start.Day);

            // Normaliza a diferença de dias
            if (dayDiff < 0)
            {
                totalMonths -= 1;
                dayDiff += 30; // Compensa o mês anterior fixando em 30 dias
            }

            // Retorna total em dias, considerando meses de 30 dias
            return (totalMonths * 30) + dayDiff;
        }

        public static int CalculateCalendarDaysBetweenTwoDates(DateTime start, DateTime end)
        {
            var consecutiveDays = end.Date - start.Date;

            return consecutiveDays.Days;
        }

        public static int TimeOut()
        {
            return 240000; // 4 minutos
        }

        public static TimeSpan TimeSpanOut()
        {
            return TimeSpan.FromMilliseconds(240000); // 4 minutos
        }

        public static int CalculateWorkingDays(DateTime start, DateTime end, List<DateTime> holidays)
        {
            int count = 0;

            for (DateTime date = start; date <= end; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Saturday
                    && date.DayOfWeek != DayOfWeek.Sunday
                    && !holidays.Any(h => h.Date == date))
                    count++;
            }

            if (end.DayOfWeek == DayOfWeek.Saturday
                || end.DayOfWeek == DayOfWeek.Sunday
                || holidays.Any(h => h.Date == end))
                return count;

            return count > 0 ? count - 1 : count;
        }

        public static int MonthByCode(string code)
        {
            var month = code.ToUpper()[0];

            switch (month)
            {
                default:
                case 'F':
                    return 1;
                case 'G':
                    return 2;
                case 'H':
                    return 3;
                case 'J':
                    return 4;
                case 'K':
                    return 5;
                case 'M':
                    return 6;
                case 'N':
                    return 7;
                case 'Q':
                    return 8;
                case 'U':
                    return 9;
                case 'V':
                    return 10;
                case 'X':
                    return 11;
                case 'Z':
                    return 12;
            }
        }

        public static int YearByCode(string code)
        {
            var year = code.ToUpper()[1..3];
            var start = "20";

            return Convert.ToInt32($"{start}{year}");
        }

        public static DateTime FirstWorkingDayByMonthAndYear(int month, int year, List<DateTime> holidays)
        {
            var firtDayOfDateMonth = new DateTime(year, month, 1);
            var date = NextWorkingDay(firtDayOfDateMonth, holidays); ;

            return date.Value.Date;
        }

        public static DateTime LastWorkingDayByDate(DateTime date, List<DateTime> holidays)
        {
            var lastDay = DateTime.DaysInMonth(date.Year, date.Month);
            var lastDayOfDateMonth = new DateTime(date.Year, date.Month, lastDay);

            var newDate = LastWorkingDay(lastDayOfDateMonth, holidays);

            return newDate.Value.Date;
        }

        public static DateTime NextWorkingDayOfNextMonth(DateTime date, List<DateTime> holidays)
        {
            var firstDayOfNextMonth = new DateTime(date.Month == 12
                ? date.Year + 1
                : date.Year,
                date.Month == 12
                ? 1
                : date.Month + 1,
                1);

            var newDate = NextWorkingDay(firstDayOfNextMonth, holidays);

            return newDate.Value.Date;
        }
    }
}