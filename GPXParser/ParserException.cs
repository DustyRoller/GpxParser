using System;
using System.Runtime.Serialization;

namespace GPXParser
{
    /// <summary>
    /// Exception class for GPX Parser related exceptions.
    /// </summary>
    [Serializable]
    public class ParserException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParserException"/> class.
        /// </summary>
        public ParserException()
            : base()
        {
            // Nothing to do.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParserException"/> class.
        /// </summary>
        /// <param name="message">The message for this exception.</param>
        public ParserException(string message)
            : base(message)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParserException"/> class.
        /// </summary>
        /// <param name="message">The message for this exception.</param>
        /// <param name="innerException">The inner exception for this exception.</param>
        public ParserException(string message, Exception innerException)
            : base(message, innerException)
        {
            // Nothing to do.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParserException"/> class.
        /// </summary>
        /// <param name="info">The information for serializing.</param>
        /// <param name="context">The context for serializing.</param>
        protected ParserException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
