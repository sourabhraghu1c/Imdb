using System;

namespace IMDBSample.Exceptions
{
    public class InvalidRequestDataException : Exception
    {
        public InvalidRequestDataException(string message) : base(message) { }
    }
}
