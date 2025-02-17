using GpxParser;
using NUnit.Framework;
using System;
using System.IO;

namespace GpxParserUnitTests
{
    [TestFixture]
    public class ParserUnitTests
    {
        /// <summary>
        /// Directory containing test data.
        /// </summary>
        private const string TestDataDir = "TestData";

        [TestCase]
        public void Parser_Parse_FailsWithNullFilePath()
        {
            // Converting null literal or possible null value to non-nullable type.
            // Possible null reference argument.
#pragma warning disable CS8600, CS8604
            string filePath = null;
            var ex = Assert.Throws<ArgumentException>(() => Parser.Parse(filePath));
#pragma warning restore CS8604, CS8600

            Assert.That(ex?.Message, Is.EqualTo("File path is invalid"));
        }

        [TestCase]
        public void Parser_Parse_FailsWithInvalidFilePath()
        {
            string filePath = "";
            var ex = Assert.Throws<ArgumentException>(() => Parser.Parse(filePath));

            Assert.That(ex?.Message, Is.EqualTo("File path is invalid"));
        }

        [TestCase]
        public void Parser_Parse_FailsIfFileDoesNotExist()
        {
            string filePath = "Doesntexist.gpx";
            var ex = Assert.Throws<FileNotFoundException>(() => Parser.Parse(filePath));

            Assert.That(ex?.Message, Is.EqualTo("File Doesntexist.gpx doesn't exist"));
        }

        [TestCase]
        public void Parser_Parse_FailsWithInvalidFile()
        {
            string filePath = Path.Combine(TestDataDir, "NotGPX.xml");
            var ex = Assert.Throws<ParserException>(() => Parser.Parse(filePath));

            Assert.That(ex?.Message, Is.EqualTo($"Failed to parse {filePath}, ensure it is in the correct format."));
        }

        [TestCase]
        public void Parser_Parse_LoadValidFile()
        {
            var gpxFile = Parser.Parse(Path.Combine(TestDataDir, "SimpleGPX.gpx"));

            Assert.That(gpxFile.metadata.link[0].href, Is.EqualTo("connect.garmin.com"));
            Assert.That(gpxFile.metadata.link[0].text, Is.EqualTo("Garmin Connect"));
            Assert.That(gpxFile.metadata.time, Is.EqualTo(new DateTime(2020, 1, 1, 12, 0, 0, DateTimeKind.Utc)));
            Assert.That(gpxFile.trk[0].name, Is.EqualTo("Sample Running"));
            Assert.That(gpxFile.trk[0].type, Is.EqualTo("running"));
            Assert.That(gpxFile.trk[0].trkseg[0].trkpt.Length, Is.EqualTo(2));

            // First track point.
            Assert.That(gpxFile.trk[0].trkseg[0].trkpt[0].ele, Is.EqualTo(10.0M));
            Assert.That(gpxFile.trk[0].trkseg[0].trkpt[0].lat, Is.EqualTo(52.0M));
            Assert.That(gpxFile.trk[0].trkseg[0].trkpt[0].lon, Is.EqualTo(0.0M));
            Assert.That(gpxFile.trk[0].trkseg[0].trkpt[0].time, Is.EqualTo(new DateTime(2020, 1, 1, 12, 0, 0, DateTimeKind.Utc)));

            // Second track point.
            Assert.That(gpxFile.trk[0].trkseg[0].trkpt[1].ele, Is.EqualTo(10.1M));
            Assert.That(gpxFile.trk[0].trkseg[0].trkpt[1].lat, Is.EqualTo(52.1M));
            Assert.That(gpxFile.trk[0].trkseg[0].trkpt[1].lon, Is.EqualTo(0.1M));
            Assert.That(gpxFile.trk[0].trkseg[0].trkpt[1].time, Is.EqualTo(new DateTime(2020, 1, 1, 12, 1, 0, DateTimeKind.Utc)));
        }

        [TestCase]
        public void Parser_Parse_LoadValidFile_WithWptData()
        {
            var filePath = Path.Combine(TestDataDir, "WPT.gpx");

            Assert.That(File.Exists(filePath), $"Test file {Path.GetFullPath(filePath)} does not exist");

            var gpxFile = Parser.Parse(filePath);

            // No metadata or trk data.
            Assert.That(gpxFile.metadata, Is.Null);
            Assert.That(gpxFile.trk, Is.Null);

            // Should have 3 WPT entries.
            Assert.That(gpxFile.wpt.Length, Is.EqualTo(3));

            // First wpt.
            Assert.That(gpxFile.wpt[0].lat, Is.EqualTo(46.0M));
            Assert.That(gpxFile.wpt[0].lon, Is.EqualTo(9.0M));
            Assert.That(gpxFile.wpt[0].ele, Is.EqualTo(46.1M));
            Assert.That(gpxFile.wpt[0].name, Is.EqualTo("WPT data 1"));
            Assert.That(gpxFile.wpt[0].desc, Is.EqualTo("Description"));

            // Second wpt.
            Assert.That(gpxFile.wpt[1].lat, Is.EqualTo(42.5M));
            Assert.That(gpxFile.wpt[1].lon, Is.EqualTo(9.5M));
            Assert.That(gpxFile.wpt[1].ele, Is.EqualTo(46.2M));
            Assert.That(gpxFile.wpt[1].name, Is.EqualTo("WPT data 2"));
            Assert.That(gpxFile.wpt[1].desc, Is.EqualTo("Description"));

            // Third wpt.
            Assert.That(gpxFile.wpt[2].lat, Is.EqualTo(43.3M));
            Assert.That(gpxFile.wpt[2].lon, Is.EqualTo(-1.1M));
            Assert.That(gpxFile.wpt[2].ele, Is.EqualTo(46.3M));
            Assert.That(gpxFile.wpt[2].name, Is.EqualTo("WPT data 3"));
            Assert.That(gpxFile.wpt[2].desc, Is.EqualTo("Description"));
        }
    }
}
