using GPXParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GPXParserUnitTests
{
    [TestClass]
    public class LocationUnitTests
    {
        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(ArgumentNullException), "Value cannot be null. (Parameter 'firstLocation')")]
        public void Location_CalculateDistance_ThrowsExceptionWithNullFirstLocation()
        {
            Location firstLocation = null;

            var secondLocation = new Location
            {
                Latitude = 51.1d,
                Longitude = 0.1d
            };

            Location.CalculateDistance(firstLocation, secondLocation);
        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(ArgumentNullException), "Value cannot be null. (Parameter 'secondLocation')")]
        public void Location_CalculateDistance_ThrowsExceptionWithNullSecondLocation()
        {
            var firstLocation = new Location
            {
                Latitude = 51.0d,
                Longitude = 0.0d
            };

            Location secondLocation = null;

            Location.CalculateDistance(firstLocation, secondLocation);
        }

        [TestMethod]
        public void Location_CalculateDistance_Successful()
        {
            var firstLocation = new Location
            {
                Latitude = 51.0d,
                Longitude = 0.0d
            };

            var secondLocation = new Location
            {
                Latitude = 51.1d,
                Longitude = 0.1d
            };

            var distance = Location.CalculateDistance(firstLocation, secondLocation);

            Assert.AreEqual(13.15, distance);
        }
    }
}
