using Microsoft.Extensions.Logging;
using TylNatwest.Services.Contracts;
using TylNatwest.Services.Models;

namespace TylNatwest.Services.Services
{
    public class StockService : IStock
    {
        private readonly ILogger<StockService> _logger;
        public StockService(ILogger<StockService> logger)
        {
            _logger = logger;    
        }

        List<TradeTransaction> tradeTransactions = new List<TradeTransaction>
        {
            new TradeTransaction { TickerSymbol = "AAPL", PriceInPound = 150.50, Shares = 14 , BrokerID = "B1", Timestamp = DateTime.Now.AddMinutes(-10), TradeID = "1" },
            new TradeTransaction { TickerSymbol = "GOOGL", PriceInPound = 2700.75, Shares = 60, BrokerID = "B2", TradeID = "2", Timestamp = DateTime.Now.AddMinutes(-5) },
            new TradeTransaction { TickerSymbol = "MSFT", PriceInPound = 300.25, Shares = 34, Timestamp = DateTime.Now.AddMinutes(-3), TradeID = "3", BrokerID = "B1" },
            new TradeTransaction { TickerSymbol = "AMZN", PriceInPound = 3500.50, Shares = 22, BrokerID = "B3", TradeID = "4", Timestamp = DateTime.Now.AddMinutes(-8) },
            new TradeTransaction { TickerSymbol = "TSLA", PriceInPound = 750.00, Shares = 79, Timestamp = DateTime.Now.AddMinutes(-14), TradeID = "5", BrokerID = "B3" }
        };
        public decimal GetStockPrice(string tickerSymbol)
        {
            try
            {
                var tradeResult = tradeTransactions.Where(x => x.TickerSymbol == tickerSymbol.ToUpper())
                                                   .OrderByDescending(x => x.Timestamp)
                                                   .FirstOrDefault();
                if (tradeResult == null)
                {
                    throw new InvalidOperationException("No trade found");
                }

                return (decimal)tradeResult.PriceInPound;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting stock price for {tickerSymbol}: {ex.Message}");
                return 0;
            }

        }

        public List<Stock> GetStockValuesInRange(List<string> tickerSymbols)
        {
            try
            {
                var stockValues = tradeTransactions.Where(x => tickerSymbols.Contains(x.TickerSymbol))
                                                    .GroupBy(x => x.TickerSymbol)
                                                    .Select(x =>
                                                    {
                                                        var latestTrade = x.OrderByDescending(y => y.Timestamp).FirstOrDefault();
                                                        return new Stock
                                                        {
                                                            TickerSymbol = x.Key,
                                                            CurrentPrice = latestTrade?.PriceInPound ?? 0
                                                        };
                                                    }).ToList();
                return stockValues;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting range of stock price for {tickerSymbols}: {ex.Message}");
                return null;
            }

        }

        public List<Stock> GetAllStockValues()
        {
            try
            {
                var stockValues = tradeTransactions.GroupBy(x => x.TickerSymbol)
                                                    .Select(x => 
                                                    {
                                                        var latestTrade = x.OrderByDescending(y => y.Timestamp).FirstOrDefault();
                                                        return new Stock
                                                        {
                                                            TickerSymbol = x.Key,
                                                            CurrentPrice = latestTrade?.PriceInPound ?? 0
                                                        };
                                                    }).ToList();
              return stockValues;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting stock prices: {ex.Message}");
                return null;
            }
      
        }
    }
}
