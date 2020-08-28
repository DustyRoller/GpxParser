using System;
using System.Text;
using System.Xml.Serialization;

namespace GPXParser
{
    /// <summary>
    /// Partial class to provide some analysis on the GPX file
    /// parsed by gpxType.Generated.cs.
    /// </summary>
    public partial class gpxType
    {
        /// <summary>
        /// The duration of the activity defined in the GPX file.
        /// <summary>
        /// Gets the duration of the activity defined in the GPX file.
        /// </summary>
        [XmlIgnoreAttribute]
        public TimeSpan Duration { get; private set; } = default;

        /// <summary>
        /// Gets the activity's elevation gain in metres as defined by the schema.
        /// </summary>
        [XmlIgnoreAttribute]
        public decimal ElevationGain { get; private set; } = 0.0M;

        /// <summary>
        /// Gets the activity's elevation loss in metres as defined by the schema.
        /// </summary>
        [XmlIgnoreAttribute]
        public decimal ElevationLoss { get; private set; } = 0.0M;

        /// <summary>
        /// Calculate the stats for the GPX file that has been parsed.
        /// </summary>
        public void CalculateStats()
        {
            CalculateDuration();
            CalculateElevationGain();
            CalculateElevationLoss();
        }

        /// <summary>
        /// Get a string representation of this gpxType object.
        /// </summary>
        /// <returns>String representation of this gpxType object.</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Duration: {Duration}");
            sb.AppendLine($"Elevation gain: {ElevationGain}");
            sb.AppendLine($"Elevation Loss {ElevationLoss}");
            return sb.ToString();
        }

        /// <summary>
        /// Calculate duration of the activity.
        /// </summary>
        private void CalculateDuration()
        {
            if (trkField != null)
            {
                // Use the first and last track points to determine the duration.
                var startTime = trkField[0].trkseg[0].trkpt[0].time;
                var endTime = trkField[0].trkseg[0].trkpt[^1].time;

                Duration = endTime.Subtract(startTime);
            }
        }

        /// <summary>
        /// Calculate the total elevation gain of the activity..
        /// </summary>
        private void CalculateElevationGain()
        {
            if (trk != null)
            {
                var trkPts = trkField[0].trkseg[0].trkpt;

                for (int i = 1; i < trkPts.Length; ++i)
                {
                    // Only add gain if elevation of track point is
                    // greater than that of the previous track point.
                    if (trkPts[i].ele > trkPts[i - 1].ele)
                    {
                        ElevationGain += trkPts[i].ele - trkPts[i - 1].ele;
                    }
                }
            }
        }

        /// <summary>
        /// Calculate the total elevation loss of the activity.
        /// </summary>
        private void CalculateElevationLoss()
        {
            if (trk != null)
            {
                var trkPts = trkField[0].trkseg[0].trkpt;

                for (int i = 1; i < trkPts.Length; ++i)
                {
                    // Only add gain if elevation of track point is
                    // greater than that of the previous track point.
                    if (trkPts[i].ele < trkPts[i - 1].ele)
                    {
                        ElevationLoss += trkPts[i - 1].ele - trkPts[i].ele;
                    }
                }
            }
        }
    }
}
