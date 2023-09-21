using TylNatwest.Services.Contracts;
using TylNatwest.Services.Models;

namespace TylNatwest.Services.Services
{
    public class TradeNotificationService : INotifyTrade
    {

        private readonly DataContext _context;
        public TradeNotificationService(DataContext context)
        {
            _context = context;   
        }
        public string ReceiveTradeNotification(TradeTransaction request)
        {

            if (request == null)
            {
                throw new ArgumentException("Invalid trade data");
            }

            // Check if the stock and broker exist, create them if not
            var stock = _context.Stocks.FirstOrDefault(x => x.TickerSymbol == request.TickerSymbol);
            if (stock == null)
            {
                stock = new Stock
                {
                    TickerSymbol = request.TickerSymbol,
                    CurrentPrice = 0,
                    TradeTransaction = new List<TradeTransaction>()
                };
                _context.Stocks.Add(stock);
            }

            // Create a new trade entry and associate it with the stock and broker
            var newTrade = new TradeTransaction
            {
                TradeID = Guid.NewGuid().ToString(), // Generate a unique trade ID
                TickerSymbol = request.TickerSymbol,
                PriceInPound = request.PriceInPound,
                Shares = request.Shares,
                BrokerID = request.BrokerID,
                Timestamp = DateTime.UtcNow
            };

            _context.TradeTransactions.Add(newTrade);

            // Update the stock's current price
            stock.CurrentPrice = request.PriceInPound;

            // Save changes to the database
            _context.SaveChanges();

            return "Trade notification received and processed";

        }
    }
}
