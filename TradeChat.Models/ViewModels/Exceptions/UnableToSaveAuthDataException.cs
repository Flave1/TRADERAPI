using System;

namespace TradeChat.Models.ViewModels.Exceptions
{
    public class UnableToSaveAuthDataException : Exception
    {
        public string result { get; set; }
        public UnableToSaveAuthDataException() { }
        public UnableToSaveAuthDataException(string result) : base(result) { }
        public UnableToSaveAuthDataException(string result, Exception exception) : base(result, exception) { }
    }
}
