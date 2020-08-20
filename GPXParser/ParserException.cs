using System;

namespace GPXParser
{
    public class ParserException : Exception
    {
        public ParserException() : base()
        {
            // Nothing to do.
        }

        public ParserException(string message) : base(message)
        {
            // Nothing to do.
        }

        public ParserException(string message, Exception innerException)
            : base(message, innerException)
        {
            // Nothing to do.
        }
    }
}
