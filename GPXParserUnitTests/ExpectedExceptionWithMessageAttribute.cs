using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GPXParserUnitTests
{
    /// <summary>
    /// Allows the validation of an Exception's error message
    /// within a unit test.
    /// </summary>
    public class ExpectedExceptionWithMessageAttribute : ExpectedExceptionBaseAttribute
    {
        public Type ExceptionType { get; private set; }

        public string ExpectedMessage { get; private set; }

        public ExpectedExceptionWithMessageAttribute(Type exceptionType, string expectedMessage)
        {
            ExceptionType = exceptionType;
            ExpectedMessage = expectedMessage;
        }

        protected override void Verify(Exception exception)
        {
            if (exception.GetType() != ExceptionType)
            {
                Assert.Fail($"ExpectedExceptionWithMessage failed. Expected exception type: {ExceptionType.FullName}. " +
                    $"Actual exception type: {exception.GetType().FullName}. Exception message: {exception.Message}");
            }

            var actualMessage = exception.Message.Trim();
            Assert.AreEqual(ExpectedMessage, actualMessage);
        }
    }
}
