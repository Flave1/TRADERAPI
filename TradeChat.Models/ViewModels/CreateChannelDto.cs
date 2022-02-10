using System.ComponentModel.DataAnnotations;

namespace TradeChat.Data.ViewModels
{
    public class CreateChannelDto
    {
        [Required]
        public string Name { get; set; }
        public string Logo { get; set; }
    }
}
