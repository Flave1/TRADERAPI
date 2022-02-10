using System;

namespace TradeChat.Models.ViewModels.Exceptions
{
    public class UnAuthorizedBrokerAccessException : Exception
    {
        public string result { get; set; }
        public UnAuthorizedBrokerAccessException() { }
        public UnAuthorizedBrokerAccessException(string result) : base(result) { }
        public UnAuthorizedBrokerAccessException(string result, Exception exception) : base(result, exception) { }
    }
}
