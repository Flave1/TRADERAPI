namespace TradeChat.Models.ViewModels
{
    public class APIResponse<T>
    {
        public APIResponse(T s) => result = s;
        public T result { get; set; }
    }

}
