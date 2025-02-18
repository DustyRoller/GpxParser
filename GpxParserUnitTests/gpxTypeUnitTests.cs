using GpxParser;
using NUnit.Framework;
using System;

namespace GpxParserUnitTests
{
    [TestFixture]
    public class gpxTypeUnitTests
    {
        [TestCase]
        public void gpxType_Distance_CalculatedSuccessfully()
        {
            var trackPoints = new wptType[]
            {
                new wptType()
                {
                    lat = 51.0M,
                    lon = 0.0M,
                },
                new wptType()
                {
                    lat = 51.1M,
                    lon = 0.1M,
                },
                new wptType()
                {
                    lat = 51.0M,
                    lon = 0.0M,
                },
            };

            var gpxFile = CreateGPXFile(trackPoints);

            var distance = gpxFile.Distance;

            Assert.That(distance, Is.EqualTo(26.3).Within(0.1));
        }

        [TestCase]
        public void gpxType_Duration_ReturnsZeroWithSingleTrackPoint()
        {
            var startTime = DateTime.Now;

            var trackPoints = new wptType[]
            {
                new wptType()
                {
                    time = startTime,
                },
            };

            var gpxFile = CreateGPXFile(trackPoints);

            var duration = gpxFile.Duration;

            Assert.That(duration, Is.EqualTo(new TimeSpan()));
        }

        [TestCase]
        public void gpxType_Duration_ReturnsNegativeTimespanWithNegativeDuration()
        {
            var startTime = new DateTime(2000, 1, 1, 1, 0, 0, DateTimeKind.Utc);

            // Set the end time to be before the start time.
            var endTime = startTime.Subtract(new TimeSpan(2, 2, 2));

            var trackPoints = new wptType[]
            {
                new wptType()
                {
                    time = startTime,
                },
                new wptType()
                {
                    time = endTime,
                },
            };

            var gpxFile = CreateGPXFile(trackPoints);

            var duration = gpxFile.Duration;

            Assert.That(duration, Is.EqualTo(endTime.Subtract(startTime)));
        }

        [TestCase]
        public void gpxType_Duration_CalculatedSuccessfully()
        {
            var startTime = new DateTime(2000, 1, 1, 1, 0, 0, DateTimeKind.Utc);
            var endTime = startTime.Add(new TimeSpan(2, 2, 2));

            var trackPoints = new wptType[]
            {
                new wptType()
                {
                    time = startTime,
                },
                new wptType()
                {
                    time = endTime,
                },
            };

            var gpxFile = CreateGPXFile(trackPoints);

            var duration = gpxFile.Duration;

            Assert.That(duration, Is.EqualTo(endTime.Subtract(startTime)));
        }

        [TestCase]
        public void gpxType_ElevationGain_CalculatedSuccessfully()
        {
            var elevation1 = 10.1M;
            var elevation2 = 15.6M;
            var elevation3 = 22.5M;

            var trackPoints = new wptType[]
            {
                new wptType()
                {
                    ele = elevation1,
                },
                new wptType()
                {
                    ele = elevation2,
                },
                new wptType()
                {
                    ele = elevation3,
                },
            };

            var gpxFile = CreateGPXFile(trackPoints);

            var expectedElevationGain = (elevation2 - elevation1) + (elevation3 - elevation2);
            var elevationGain = gpxFile.ElevationGain;

            Assert.That(elevationGain, Is.EqualTo(expectedElevationGain));
        }

        [TestCase]
        public void gpxType_ElevationGain_CalculatedSuccessfullyWithNoElevationGain()
        {
            var elevation1 = 10.1M;
            var elevation2 = 9.0M;
            var elevation3 = 8.0M;

            var trackPoints = new wptType[]
            {
                new wptType()
                {
                    ele = elevation1,
                },
                new wptType()
                {
                    ele = elevation2,
                },
                new wptType()
                {
                    ele = elevation3,
                },
            };

            var gpxFile = CreateGPXFile(trackPoints);

            var elevationGain = gpxFile.ElevationGain;

            Assert.That(elevationGain, Is.EqualTo(0.0M));
        }

        [TestCase]
        public void gpxType_ElevationGain_CalculatedSuccessfullyWithElevationLoss()
        {
            var elevation1 = 10.1M;
            var elevation2 = 9.0M;
            var elevation3 = 12.6M;

            var trackPoints = new wptType[]
            {
                new wptType()
                {
                    ele = elevation1,
                },
                new wptType()
                {
                    ele = elevation2,
                },
                new wptType()
                {
                    ele = elevation3,
                },
            };

            var gpxFile = CreateGPXFile(trackPoints);

            var expectedElevationGain = (elevation3 - elevation2);
            var elevationGain = gpxFile.ElevationGain;

            Assert.That(elevationGain, Is.EqualTo(expectedElevationGain));
        }

        [TestCase]
        public void gpxType_ElevationLoss_CalculatedSuccessfully()
        {
            var elevation1 = 22.5M;
            var elevation2 = 15.6M;
            var elevation3 = 10.1M;

            var trackPoints = new wptType[]
            {
                new wptType()
                {
                    ele = elevation1,
                },
                new wptType()
                {
                    ele = elevation2,
                },
                new wptType()
                {
                    ele = elevation3,
                },
            };

            var gpxFile = CreateGPXFile(trackPoints);

            var expectedElevationLoss = (elevation1 - elevation2) + (elevation2 - elevation3);
            var elevationLoss = gpxFile.ElevationLoss;

            Assert.That(elevationLoss, Is.EqualTo(expectedElevationLoss));
        }

        [TestCase]
        public void gpxType_ElevationLoss_CalculatedSuccessfullyWithNoElevationLoss()
        {
            var elevation1 = 8.5M;
            var elevation2 = 9.0M;
            var elevation3 = 10.1M;

            var trackPoints = new wptType[]
            {
                new wptType()
                {
                    ele = elevation1,
                },
                new wptType()
                {
                    ele = elevation2,
                },
                new wptType()
                {
                    ele = elevation3,
                },
            };

            var gpxFile = CreateGPXFile(trackPoints);

            var elevationLoss = gpxFile.ElevationLoss;

            Assert.That(elevationLoss, Is.EqualTo(0.0M));
        }

        [TestCase]
        public void gpxType_ElevationLoss_CalculatedSuccessfullyWithElevationGain()
        {
            var elevation1 = 9.0M;
            var elevation2 = 10.1M;
            var elevation3 = 8.3M;

            var trackPoints = new wptType[]
            {
                new wptType()
                {
                    ele = elevation1,
                },
                new wptType()
                {
                    ele = elevation2,
                },
                new wptType()
                {
                    ele = elevation3,
                },
            };

            var gpxFile = CreateGPXFile(trackPoints);

            var expectedElevationLoss = (elevation2 - elevation3);
            var elevationLoss = gpxFile.ElevationLoss;

            Assert.That(elevationLoss, Is.EqualTo(expectedElevationLoss));
        }

        /// <summary>
        /// Create a gpxType with the given TrackPoints.
        /// </summary>
        /// <param name="trackPoints">The TrackPoints to use.</param>
        /// <returns>A gpxType.</returns>
        private static gpxType CreateGPXFile(wptType[] trackPoints)
        {
            var gpxFile = new gpxType()
            {
                trk = new trkType[]
                {
                    new trkType()
                    {
                        trkseg = new trksegType[]
                        {
                            new trksegType
                            {
                                trkpt = trackPoints,
                            },
                        },
                    },
                },
            };

            // Make sure to calculate the stats for this file.
            gpxFile.CalculateStats();

            return gpxFile;
        }
    }
}
