using GPXParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GPXParserUnitTests
{
    [TestClass]
    public class GPXFileUnitTests
    {
        [TestMethod]
        public void GPXFile_Duration_ReturnsZeroWithSingleTrackPoint()
        {
            var startTime = DateTime.Now;

            var trackPoints = new wptType[]
            {
                new wptType()
                {
                    time = startTime
                }
            };

            var gpxFile = CreateGPXFile(trackPoints);

            var duration = gpxFile.Duration;

            Assert.AreEqual(new TimeSpan(), duration);
        }

        [TestMethod]
        public void GPXFile_Duration_ReturnsNegativeTimespanWithNegativeDuration()
        {
            var startTime = new DateTime(2000, 1, 1, 1, 0, 0);
            // Set the end time to be before the start time.
            var endTime = startTime.Subtract(new TimeSpan(2, 2, 2));

            var trackPoints = new wptType[]
            {
                new wptType()
                {
                    time = startTime
                },
                new wptType()
                {
                    time = endTime
                }
            };

            var gpxFile = CreateGPXFile(trackPoints);

            var duration = gpxFile.Duration;

            Assert.AreEqual(endTime.Subtract(startTime), duration);
        }

        [TestMethod]
        public void GPXFile_Duration_Successful()
        {
            var startTime = new DateTime(2000, 1, 1, 1, 0, 0);
            var endTime = startTime.Add(new TimeSpan(2, 2, 2));

            var trackPoints = new wptType[]
            {
                new wptType()
                {
                    time = startTime
                },
                new wptType()
                {
                    time = endTime
                }
            };

            var gpxFile = CreateGPXFile(trackPoints);

            var duration = gpxFile.Duration;

            Assert.AreEqual(endTime.Subtract(startTime), duration);
        }

        [TestMethod]
        public void GPXFile_ElevationGain_Successful()
        {
            var elevation1 = 10.1M;
            var elevation2 = 15.6M;
            var elevation3 = 22.5M;

            var trackPoints = new wptType[]
            {
                new wptType()
                {
                    ele = elevation1
                },
                new wptType()
                {
                    ele = elevation2
                },
                new wptType()
                {
                    ele = elevation3
                }
            };

            var gpxFile = CreateGPXFile(trackPoints);

            var expectedElevationGain = (elevation2 - elevation1) + (elevation3 - elevation2);
            var elevationGain = gpxFile.ElevationGain;

            Assert.AreEqual(expectedElevationGain, elevationGain);
        }

        [TestMethod]
        public void GPXFile_ElevationGain_SuccessfulWithNoElevationGain()
        {
            var elevation1 = 10.1M;
            var elevation2 = 9.0M;
            var elevation3 = 8.0M;

            var trackPoints = new wptType[]
            {
                new wptType()
                {
                    ele = elevation1
                },
                new wptType()
                {
                    ele = elevation2
                },
                new wptType()
                {
                    ele = elevation3
                }
            };

            var gpxFile = CreateGPXFile(trackPoints);

            var elevationGain = gpxFile.ElevationGain;

            Assert.AreEqual(0.0M, elevationGain);
        }

        [TestMethod]
        public void GPXFile_ElevationGain_SuccessfulWithElevationLoss()
        {
            var elevation1 = 10.1M;
            var elevation2 = 9.0M;
            var elevation3 = 12.6M;

            var trackPoints = new wptType[]
            {
                new wptType()
                {
                    ele = elevation1
                },
                new wptType()
                {
                    ele = elevation2
                },
                new wptType()
                {
                    ele = elevation3
                }
            };

            var gpxFile = CreateGPXFile(trackPoints);

            var expectedElevationGain = (elevation3 - elevation2);
            var elevationGain = gpxFile.ElevationGain;

            Assert.AreEqual(expectedElevationGain, elevationGain);
        }

        [TestMethod]
        public void GPXFile_ElevationLoss_Successful()
        {
            var elevation1 = 22.5M;
            var elevation2 = 15.6M;
            var elevation3 = 10.1M;

            var trackPoints = new wptType[]
            {
                new wptType()
                {
                    ele = elevation1
                },
                new wptType()
                {
                    ele = elevation2
                },
                new wptType()
                {
                    ele = elevation3
                }
            };

            var gpxFile = CreateGPXFile(trackPoints);

            var expectedElevationLoss = (elevation1 - elevation2) + (elevation2 - elevation3);
            var elevationLoss = gpxFile.ElevationLoss;

            Assert.AreEqual(expectedElevationLoss, elevationLoss);
        }

        [TestMethod]
        public void GPXFile_ElevationLoss_SuccessfulWithNoElevationLoss()
        {
            var elevation1 = 8.5M;
            var elevation2 = 9.0M;
            var elevation3 = 10.1M;

            var trackPoints = new wptType[]
            {
                new wptType()
                {
                    ele = elevation1
                },
                new wptType()
                {
                    ele = elevation2
                },
                new wptType()
                {
                    ele = elevation3
                }
            };

            var gpxFile = CreateGPXFile(trackPoints);

            var elevationLoss = gpxFile.ElevationLoss;

            Assert.AreEqual(0.0M, elevationLoss);
        }

        [TestMethod]
        public void GPXFile_ElevationLoss_SuccessfulWithElevationGain()
        {
            var elevation1 = 9.0M;
            var elevation2 = 10.1M;
            var elevation3 = 8.3M;

            var trackPoints = new wptType[]
            {
                new wptType()
                {
                    ele = elevation1
                },
                new wptType()
                {
                    ele = elevation2
                },
                new wptType()
                {
                    ele = elevation3
                }
            };

            var gpxFile = CreateGPXFile(trackPoints);

            var expectedElevationLoss = (elevation2 - elevation3);
            var elevationLoss = gpxFile.ElevationLoss;

            Assert.AreEqual(expectedElevationLoss, elevationLoss);
        }

        /// <summary>
        /// Create a gpxType with the given TrackPoints.
        /// </summary>
        /// <param name="trackPoints">The TrackPoints to use.</param>
        /// <returns>A gpxType.</returns>
        private gpxType CreateGPXFile(wptType[] trackPoints)
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
                                trkpt = trackPoints
                            }
                        }
                    }
                }
            };

            // Make sure to calculate the stats for this file.
            gpxFile.CalculateStats();

            return gpxFile;
        }
    }
}
