using System.Diagnostics;

namespace TylNatwest.Services.Models
{
    public class StockExchangeDataManager
    {
        private Dictionary<string, Stock> Stocks { get; }
        private Dictionary<string, Broker> Brokers { get; }

        public StockExchangeDataManager()
        {
            Stocks = new Dictionary<string, Stock>();
            Brokers = new Dictionary<string, Broker>();
            InitializeTestData();
        }

        private void InitializeTestData()
        {
            // Initialize some test data for test purposes
            var stock1 = new Stock { TickerSymbol = "AAPL", CurrentPrice = 150.00, Trades = new List<TradeTransaction>() };
            var stock2 = new Stock { TickerSymbol = "GOOGL", CurrentPrice = 2800.00, Trades = new List<TradeTransaction>() };

            var broker1 = new Broker { BrokerId = "B001", BrokerName = "Broker1", BrokerAddress = "123 Main St", Trades = new List<TradeTransaction>() };
            var broker2 = new Broker { BrokerId = "B002", BrokerName = "Broker2", BrokerAddress = "456 Elm St", Trades = new List<TradeTransaction>() };

            Stocks.Add(stock1.TickerSymbol, stock1);
            Stocks.Add(stock2.TickerSymbol, stock2);

            Brokers.Add(broker1.BrokerId, broker1);
            Brokers.Add(broker2.BrokerId, broker2);
        }
    }
}
