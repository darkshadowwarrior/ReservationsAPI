namespace Reservations.Api.Managers
{

    public interface IPricingManager
    {
        decimal GetPrice(DateTime date);
    }

    public class PricingManager : IPricingManager
    {
        public decimal GetPrice(DateTime date)
        {
            var weekDayPrice = 10.0M;
            var weekendPrice = 15.0M;
            var summerPriceIncrease = 12.0M;
            var winterPriceIncrease = 8.0M;

            var pricePerDay = 0.0M;

            if (IsSummer(date))
            {
                pricePerDay += summerPriceIncrease;
            }
            else if (IsWinter(date))
            {
                pricePerDay += winterPriceIncrease;
            }

            if (IsWeekend(date))
            {
                pricePerDay += weekendPrice;
                return pricePerDay;
            }

            pricePerDay += weekDayPrice;

            return pricePerDay;
        }

        private bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
        }

        private bool IsSummer(DateTime date)
        {
            return date.Month is >= 6 and <= 8;
        }

        private bool IsWinter(DateTime date)
        {
            return date.Month is 12 or <= 2;
        }
    }
}
