using System;

namespace GPXParser
{
    /// <summary>
    /// Class defining a location with latitude and longitude values.
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Gets or sets and sets the latitude of the location.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the location.
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Calculate the distance between two Locations, in kilometres.
        /// </summary>
        /// <remarks>
        /// Formula taken from the internet.
        /// </remarks>
        /// <param name="firstLocation">The first Location.</param>
        /// <param name="secondLocation">The second Location.</param>
        /// <returns>The distance between the two Locations in kilometres.</returns>
        public static double CalculateDistance(Location firstLocation, Location secondLocation)
        {
            if (firstLocation == null)
            {
                throw new ArgumentNullException(nameof(firstLocation));
            }

            if (secondLocation == null)
            {
                throw new ArgumentNullException(nameof(secondLocation));
            }

            var d1 = firstLocation.Latitude * (Math.PI / 180.0);
            var num1 = firstLocation.Longitude * (Math.PI / 180.0);
            var d2 = secondLocation.Latitude * (Math.PI / 180.0);
            var num2 = (secondLocation.Longitude * (Math.PI / 180.0)) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            var distanceMetres = 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));

            // Get the distance in kilometres and also only allow 2 decimal places.
            var distanceKm = distanceMetres / 1000;
            return Math.Round(distanceKm, 2);
        }
    }
}
