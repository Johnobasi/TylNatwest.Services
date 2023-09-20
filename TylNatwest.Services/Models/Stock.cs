namespace TylNatwest.Services.Models
{
    public class Stock
    {
        public string TickerSymbol { get; set; }
        public double CurrentPrice { get; set; }
        public List<TradeTransaction> Trades { get; set; }
    }
}
