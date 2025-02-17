using System;
using System.IO;
using System.Xml.Serialization;

namespace GpxParser
{
    /// <summary>
    /// Class to provide functionality to parse a GPX file.
    /// </summary>
    public static class Parser
    {
        /// <summary>
        /// Deserialize the data within the given XML file in object of type T.
        /// </summary>
        /// <typeparam name="T">The type of object to be created.</typeparam>
        /// <param name="filePath">The path to the XML file to be read in.</param>
        /// <returns>If successful an object of type T, otherwise null.</returns>
        public static T? DeserializeToObject<T>(string filePath)
            where T : class
        {
            var xmlSerializer = new XmlSerializer(typeof(T), "http://www.topografix.com/GPX/1/1");

            using (var streamReader = new StreamReader(filePath))
            {
                return xmlSerializer.Deserialize(streamReader) as T;
            }
        }

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
                throw new FileNotFoundException($"File {filePath} doesn't exist");
            }

            try
            {
                var gpxFile = DeserializeToObject<gpxType>(filePath);

                if (gpxFile == null)
                {
                    throw new ParserException($"Failed to parse {filePath}");
                }

                gpxFile.CalculateStats();

                return gpxFile;
            }
            catch (InvalidOperationException ex)
            {
                throw new ParserException($"Failed to parse {filePath}, ensure it is in the correct format.", ex);
            }
        }
    }
}
