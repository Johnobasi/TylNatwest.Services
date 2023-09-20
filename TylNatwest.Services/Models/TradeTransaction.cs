namespace TylNatwest.Services.Models
{
    public class TradeTransaction
    {
        public string TradeID { get; set; }
        public string TickerSymbol { get; set; }
        public double PriceInPound { get; set; }
        public double Shares { get; set; }
        public string BrokerID { get; set; }
        public DateTime Timestamp { get; set; }
        public Stock Stock { get; set; }
    }
}
