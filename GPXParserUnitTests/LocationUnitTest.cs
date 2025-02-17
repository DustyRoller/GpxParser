using GpxParser;
using NUnit.Framework;
using System;

namespace GpxParserUnitTests
{
    [TestFixture]
    public class LocationUnitTests
    {
        [TestCase]
        public void Location_CalculateDistance_ThrowsExceptionWithNullFirstLocation()
        {
            // Converting null literal or possible null value to non-nullable type.
            // Possible null reference argument.
#pragma warning disable CS8600, CS8604
            Location firstLocation = null;

            var secondLocation = new Location
            {
                Latitude = 51.1d,
                Longitude = 0.1d,
            };

            var ex = Assert.Throws<ArgumentNullException>(() => Location.CalculateDistance(firstLocation, secondLocation));
#pragma warning restore CS8604, CS8600

            Assert.That(ex?.Message, Is.EqualTo("Value cannot be null. (Parameter 'firstLocation')"));
        }

        [TestCase]
        public void Location_CalculateDistance_ThrowsExceptionWithNullSecondLocation()
        {
            var firstLocation = new Location
            {
                Latitude = 51.0d,
                Longitude = 0.0d,
            };

            // Converting null literal or possible null value to non-nullable type.
            // Possible null reference argument.
#pragma warning disable CS8600, CS8604
            Location secondLocation = null;

            var ex = Assert.Throws<ArgumentNullException>(() => Location.CalculateDistance(firstLocation, secondLocation));
#pragma warning restore CS8604, CS8600

            Assert.That(ex?.Message, Is.EqualTo("Value cannot be null. (Parameter 'secondLocation')"));
        }

        [TestCase]
        public void Location_CalculateDistance_Successful()
        {
            var firstLocation = new Location
            {
                Latitude = 51.0d,
                Longitude = 0.0d,
            };

            var secondLocation = new Location
            {
                Latitude = 51.1d,
                Longitude = 0.1d,
            };

            var distance = Location.CalculateDistance(firstLocation, secondLocation);

            Assert.That(distance, Is.EqualTo(13145.5).Within(0.1));
        }

        [TestCase]
        public void Location_CalculateDistance_Successful_HighPrecision()
        {
            var firstLocation = new Location
            {
                Latitude = 51.0d,
                Longitude = 0.0d,
            };

            var secondLocation = new Location
            {
                Latitude = 51.000005d,
                Longitude = 0.000005d,
            };

            var distance = Location.CalculateDistance(firstLocation, secondLocation);

            Assert.That(distance, Is.EqualTo(0.657).Within(0.001));
        }
    }
}
