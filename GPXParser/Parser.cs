using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace GPXParser
{
    public static class Parser
    {
        /// <summary>
        /// Parse the provided GPX file and convert it into a GPXFile object.
        /// </summary>
        /// <param name="filePath">The path to the GPX file to parse.</param>
        /// <returns>A gpxType object representing the given GPX file.</returns>
        public static gpxType Parse(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path is invalid");
            }

            // Make sure the file exists.
            if (!File.Exists(filePath))
            {
                throw new ArgumentException($"File {filePath} doesn't exist");
            }

            try
            {
                using (var fileStream = File.Open(filePath, FileMode.Open))
                {
                    var xmlSerializer = new XmlSerializer(typeof(gpxType), "http://www.topografix.com/GPX/1/1");

                    using (var reader = XmlReader.Create(fileStream))
                    {

                        var gpxFile = (gpxType)xmlSerializer.Deserialize(reader);

                        gpxFile.CalculateStats();

                        return gpxFile;
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                throw new ParserException($"Failed to parse {filePath}, ensure it is in the correct format.", ex);
            }
        }
    }
}
