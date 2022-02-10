namespace TradeChat.Data
{
    public interface IChatDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
