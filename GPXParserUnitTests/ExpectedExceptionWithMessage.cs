using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace GPXParserUnitTests
{
    /// <summary>
    /// Allows the validation of an Exception's error message
    /// within a unit test.
    /// </summary>
    public class ExpectedExceptionWithMessage : ExpectedExceptionBaseAttribute
    {
        public Type ExceptionType { get; private set; }

        public string ExpectedMessage { get; private set; }

        public ExpectedExceptionWithMessage(Type exceptionType, string expectedMessage)
        {
            ExceptionType = exceptionType;
            ExpectedMessage = expectedMessage;
        }

        protected override void Verify(Exception ex)
        {
            if (ex.GetType() != ExceptionType)
            {
                Assert.Fail($"ExpectedExceptionWithMessage failed. Expected exception type: {ExceptionType.FullName}. " +
                    $"Actual exception type: {ex.GetType().FullName}. Exception message: {ex.Message}");
            }

            var actualMessage = ex.Message.Trim();
            Assert.AreEqual(ExpectedMessage, actualMessage);
        }
    }
}
