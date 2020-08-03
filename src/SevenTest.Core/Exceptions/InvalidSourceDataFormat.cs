using System;
using System.Collections.Generic;
using System.Text;

namespace SevenTest.Core.Exceptions
{
    public class InvalidSourceDataFormat : Exception
    {
        public string SourceType { get; set; }
        public string SourceUrl { get; set; }
        public InvalidSourceDataFormat(string message, string sourceType, string sourceURL, Exception innerException) : base(message,innerException)
        {
            SourceType = sourceType;
            SourceUrl = sourceURL;
        }
    }
}
