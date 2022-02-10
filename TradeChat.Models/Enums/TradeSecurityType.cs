namespace TradeChat.Data.Enums
{
    //cash - Cash, currency, and money market funds
    //crypto - crypto currencies
    //options - derivative: Options, warrants, and other derivative instruments
    //equity - Domestic and foreign equities (stocks)
    //etf: Multi-asset exchange-traded investment funds
    //mutual fund: Open- and closed-end vehicles pooling funds of multiple investors.
    //Unknown: other unknown security classifications

    public enum TradeSecurityType
    {
        Cash, Crypto, Options, Equity, ETF, MutualFund, Unknown
    }
}
