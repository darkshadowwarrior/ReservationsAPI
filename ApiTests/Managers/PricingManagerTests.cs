using Reservations.Api.Managers;

namespace ApiTests.Managers
{
    public class PricingManagerTests
    {
        private readonly PricingManager _manager;

        public PricingManagerTests()
        {
            _manager = new PricingManager();
        }

        [Fact]
        public void GivenAWeekdayDate_GetPrice_ReturnsPrice()
        {
            var date = new DateTime(2023, 7, 1);

            var actualPrice = _manager.GetPrice(date);

            var expectedPrice = 27.0M;
            Assert.Equal(expectedPrice, actualPrice);
        }

        [Fact]
        public void GivenAWeekendDate_GetPrice_ReturnsPrice()
        {
            var date = new DateTime(2023, 7, 1);

            var actualPrice = _manager.GetPrice(date);

            var expectedPrice = 27.0M;
            Assert.Equal(expectedPrice, actualPrice);
        }

        [Fact]
        public void GivenAWeekdayDateInWinter_GetPrice_ReturnsPrice()
        {
            var date = new DateTime(2023, 1, 1);

            var actualPrice = _manager.GetPrice(date);

            var expectedPrice = 23.0M;
            Assert.Equal(expectedPrice, actualPrice);
        }

        [Fact]
        public void GivenAWeekendDateAndInWinter_GetPrice_ReturnsPrice()
        {
            var date = new DateTime(2023, 2, 1);

            var actualPrice = _manager.GetPrice(date);

            var expectedPrice = 18.0M;
            Assert.Equal(expectedPrice, actualPrice);
        }

        [Fact]
        public void GivenAWeekdayDateNotInWinterOrSummer_GetPrice_ReturnsPrice()
        {
            var date = new DateTime(2023, 10, 1);

            var actualPrice = _manager.GetPrice(date);

            var expectedPrice = 15.0M;
            Assert.Equal(expectedPrice, actualPrice);
        }

        [Fact]
        public void GivenAWeekendDateNotInWinterOrSummer_GetPrice_ReturnsPrice()
        {
            var date = new DateTime(2023, 4, 1);
            var expectedPrice = 15.0M;

            var actualPrice = _manager.GetPrice(date);

            Assert.Equal(expectedPrice, actualPrice);
        }
    }
}
