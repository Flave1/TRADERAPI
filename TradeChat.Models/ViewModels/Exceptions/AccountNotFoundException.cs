using System;

namespace TradeChat.Models.ViewModels.Exceptions
{

    public class AccountNotFoundException : Exception
    {
        public string result { get; set; }
        public AccountNotFoundException() { }
        public AccountNotFoundException(string result) : base(result) { }
        public AccountNotFoundException(string result, Exception exception) : base(result, exception) { }
    }
}
