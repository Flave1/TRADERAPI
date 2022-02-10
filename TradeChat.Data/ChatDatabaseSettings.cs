namespace TradeChat.Data
{
    public class ChatDatabaseSettings : IChatDatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
