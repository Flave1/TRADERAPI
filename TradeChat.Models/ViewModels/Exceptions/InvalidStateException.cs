using System;

namespace TradeChat.Models.ViewModels.Exceptions
{
    public class InvalidStateException : Exception
    {
        public string result { get; set; }
        public InvalidStateException() { }
        public InvalidStateException(string result) : base(result) { }
        public InvalidStateException(string result, Exception exception) : base(result, exception) { }
    }
}
