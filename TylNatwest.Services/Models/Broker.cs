namespace TylNatwest.Services.Models
{
    public class Broker
    {
        public string BrokerId { get; set; }
        public string BrokerName { get; set; }
        public string BrokerAddress { get; set; }
        public List<Stock> Stocks { get; set; }
        public List<TradeTransaction> TradeTransactions { get; set; } = new List<TradeTransaction>();
    }
}
