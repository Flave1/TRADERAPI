namespace TradeChat.Data.Enums
{
    public enum TradeType
    {
        // unknown, fee, cash and transfer trade types are not shared and are only for analytics.
        Unknown, Buy, Sell, Cancel, Fee, Cash, Transfer
    }
}
