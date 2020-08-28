using GPXParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace GPXParserUnitTests
{
    [TestClass]
    public class ParserUnitTests
    {
        /// <summary>
        /// Directory containing test data.
        /// </summary>
        private const string TestDataDir = "TestData";

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(ArgumentException), "File path is invalid")]
        public void Parser_Parse_FailsWithNullFilePath()
        {
            Parser.Parse(null);
        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(ArgumentException), "File path is invalid")]
        public void Parser_Parse_FailsWithInvalidFilePath()
        {
            Parser.Parse("");
        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(ArgumentException), "File Doesntexist.gpx doesn't exist")]
        public void Parser_Parse_FailsIfFileDoesNotExist()
        {
            Parser.Parse("Doesntexist.gpx");
        }

        [TestMethod]
        [ExpectedExceptionWithMessage(typeof(ParserException), "Failed to parse TestData\\NotGPX.xml, ensure it is in the correct format.")]
        public void Parser_Parse_FailsWithInvalidFile()
        {
            Parser.Parse(Path.Combine(TestDataDir, "NotGPX.xml"));
        }

        [TestMethod]
        public void Parser_Parse_LoadValidFile()
        {
            var gpxFile = Parser.Parse(Path.Combine(TestDataDir, "SimpleGPX.gpx"));

            Assert.AreEqual("connect.garmin.com", gpxFile.metadata.link[0].href);
            Assert.AreEqual("Garmin Connect", gpxFile.metadata.link[0].text);
            Assert.AreEqual(new DateTime(2020, 01, 01, 12, 00, 00), gpxFile.metadata.time);
            Assert.AreEqual("Sample Running", gpxFile.trk[0].name);
            Assert.AreEqual("running", gpxFile.trk[0].type);
            Assert.AreEqual(2, gpxFile.trk[0].trkseg[0].trkpt.Length);
            // First track point.
            Assert.AreEqual(10.0M, gpxFile.trk[0].trkseg[0].trkpt[0].ele);
            Assert.AreEqual(52.0M, gpxFile.trk[0].trkseg[0].trkpt[0].lat);
            Assert.AreEqual(0.0M, gpxFile.trk[0].trkseg[0].trkpt[0].lon);
            Assert.AreEqual(new DateTime(2020, 01, 01, 12, 00, 00), gpxFile.trk[0].trkseg[0].trkpt[0].time);
            // Second track point.
            Assert.AreEqual(10.1M, gpxFile.trk[0].trkseg[0].trkpt[1].ele);
            Assert.AreEqual(52.1M, gpxFile.trk[0].trkseg[0].trkpt[1].lat);
            Assert.AreEqual(0.1M, gpxFile.trk[0].trkseg[0].trkpt[1].lon);
            Assert.AreEqual(new DateTime(2020, 01, 01, 12, 01, 00), gpxFile.trk[0].trkseg[0].trkpt[1].time);
        }

        [TestMethod]
        public void Parser_Parse_LoadValidFile_WithWPTData()
        {
            var gpxFile = Parser.Parse(Path.Combine(TestDataDir, "wpt.gpx"));

            // No metadata or trk data.
            Assert.IsNull(gpxFile.metadata);
            Assert.IsNull(gpxFile.trk);

            // Should have 3 WPT entries.
            Assert.AreEqual(3, gpxFile.wpt.Length);

            // First wpt.
            Assert.AreEqual(46.0M, gpxFile.wpt[0].lat);
            Assert.AreEqual(9.0M, gpxFile.wpt[0].lon);
            Assert.AreEqual(46.1M, gpxFile.wpt[0].ele);
            Assert.AreEqual("WPT data 1", gpxFile.wpt[0].name);
            Assert.AreEqual("Description", gpxFile.wpt[0].desc);

            // Second wpt.
            Assert.AreEqual(42.5M, gpxFile.wpt[1].lat);
            Assert.AreEqual(9.5M, gpxFile.wpt[1].lon);
            Assert.AreEqual(46.2M, gpxFile.wpt[1].ele);
            Assert.AreEqual("WPT data 2", gpxFile.wpt[1].name);
            Assert.AreEqual("Description", gpxFile.wpt[1].desc);

            // Third wpt.
            Assert.AreEqual(43.3M, gpxFile.wpt[2].lat);
            Assert.AreEqual(-1.1M, gpxFile.wpt[2].lon);
            Assert.AreEqual(46.3M, gpxFile.wpt[2].ele);
            Assert.AreEqual("WPT data 3", gpxFile.wpt[2].name);
            Assert.AreEqual("Description", gpxFile.wpt[2].desc);
        }
    }
}
