using System.Text.Json.Serialization;

namespace TylNatwest.Services.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public string TickerSymbol { get; set; }
        public double CurrentPrice { get; set; }
        [JsonIgnore]
        public Broker Broker { get; set; }
        public List<TradeTransaction> TradeTransaction { get; set; } = new List<TradeTransaction>();
    }
}
