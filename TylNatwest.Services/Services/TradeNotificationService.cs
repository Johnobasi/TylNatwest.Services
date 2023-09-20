using TylNatwest.Services.Contracts;
using TylNatwest.Services.Models;

namespace TylNatwest.Services.Services
{
    public class TradeNotificationService : INotifyTrade
    {

        private readonly List<Stock> stocks;
        private readonly List<Broker> brokers;
        public TradeNotificationService()
        {
            stocks = new List<Stock>();
            brokers = new List<Broker>();
        }
        public string ReceiveTradeNotification(TradeTransaction request)
        {

            if (request == null)
            {
                throw new ArgumentException("Invalid trade data");
            }

            // Check if the stock and broker exist, create them if not
            Stock stock = stocks.Find(c=>c.TickerSymbol == request.TickerSymbol);
            if (stock == null)
            {
                stock = new Stock
                {
                    TickerSymbol = request.TickerSymbol,
                    CurrentPrice = 0,
                    Trades = new List<TradeTransaction>()
                    {
                        new TradeTransaction
                        {
                            BrokerID = "1",
                            TickerSymbol = "GOOGL",
                            PriceInPound = 100,
                            Timestamp = DateTime.UtcNow
                        }
                    }
                };
                stocks.Add(stock);
            }

            Broker broker = brokers.Find(b => b.BrokerId == request.BrokerID);
            if (broker == null)
            {
                broker = new Broker
                {
                    BrokerId = request.BrokerID,
                    BrokerName = "B1", 
                    BrokerAddress = "London",
                    Trades = new List<TradeTransaction>() { new TradeTransaction
                                           {
                            BrokerID = "1",
                            TickerSymbol = "TSLA",
                            PriceInPound = 100,
                            Timestamp = DateTime.UtcNow
                        }
                    }
                };
                brokers.Add(broker);
            }

            // add the trade to the stock and broker
            request.Stock = stock;
            request.Timestamp = DateTime.UtcNow;
            stock.Trades.Add(request);
            broker.Trades.Add(request);
            
            // update the stock price
            stock.CurrentPrice = request.PriceInPound;
            return "Trade notification received and processed";
        }
    }
}
