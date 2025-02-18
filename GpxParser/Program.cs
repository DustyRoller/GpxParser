using System;

namespace GpxParser
{
    /// <summary>
    /// The class containing the entry point for the application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The GpxParser main function.
        /// </summary>
        /// <param name="args">The arguments provided to the project.</param>
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Error.WriteLine("Expected 1 argument - Path to GPX file");
                Environment.Exit(1);
            }

            var gpxFile = Parser.Parse(args[0]);

            Console.WriteLine(gpxFile);
        }
    }
}
