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
            new TradeTransaction { TickerSymbol = "AAPL", PriceInPound = 150.50, Shares = 14 , BrokerID = "B1", Timestamp = DateTime.Now.AddMinutes(-10), TradeID = "1", Stock = new Stock() },
            new TradeTransaction { TickerSymbol = "GOOGL", PriceInPound = 2700.75, Shares = 60, BrokerID = "B2", TradeID = "2", Timestamp = DateTime.Now.AddMinutes(-5), Stock = new Stock() },
            new TradeTransaction { TickerSymbol = "MSFT", PriceInPound = 300.25, Shares = 34, Timestamp = DateTime.Now.AddMinutes(-3), TradeID = "3", BrokerID = "B1", Stock = new Stock() },
            new TradeTransaction { TickerSymbol = "AMZN", PriceInPound = 3500.50, Shares = 22, BrokerID = "B3", TradeID = "4", Timestamp = DateTime.Now.AddMinutes(-8), Stock = new Stock() },
            new TradeTransaction { TickerSymbol = "TSLA", PriceInPound = 750.00, Shares = 79, Timestamp = DateTime.Now.AddMinutes(-14), TradeID = "5", BrokerID = "B3", Stock = new Stock() }
        };

        public Stock GetStockPrice(string tickerSymbol)
        {
            try
            {
                var tradeResult = tradeTransactions.Where(x => x.TickerSymbol == tickerSymbol.ToUpper())
                                                   .OrderByDescending(x => x.Timestamp)
                                                   .Select(s=> new TradeTransaction
                                                   {
                                                       PriceInPound = s.PriceInPound,
                                                       TickerSymbol = s.TickerSymbol,
                                                       Shares = s.Shares,
                                                       Timestamp = s.Timestamp                                                      
                                                   }).ToList();
                if (tradeResult.Count == 0)
                {
                    throw new InvalidOperationException("No trade found");
                }

                var currentPrice = tradeResult[0].PriceInPound;
                var stock = new Stock
                {
                    TickerSymbol = tickerSymbol,
                    CurrentPrice = currentPrice,
                    Trades = tradeResult
                };

                return stock;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting stock info for {tickerSymbol}: {ex.Message}");
                return new Stock();
            }

        }

        public List<Stock> GetStockValuesInRange(List<string> tickerSymbols)
        {
            try
            {
                var selectedStocksInfo = new List<Stock>();
                foreach (var tickedSymbol in tradeTransactions)
                {
                    var stockValues = tradeTransactions.Where(x => tickerSymbols.Contains(x.TickerSymbol))
                                                .OrderByDescending(x => x.Timestamp)
                                                .FirstOrDefault();

                    decimal currentPrice = 0;
                    if (stockValues != null)
                    {
                        currentPrice = (decimal)stockValues.PriceInPound;
                    }
                    var stockInfo = new Stock
                    {
                        TickerSymbol = tickedSymbol.TickerSymbol,
                        CurrentPrice = (double)currentPrice
                    };

                    selectedStocksInfo.Add(stockInfo);
        
                }
            
                return selectedStocksInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting range of stock price for {tickerSymbols}: {ex.Message}");
                return new List<Stock>();
            }

        }

        public List<Stock> GetAllStockValues()
        {
            try
            {
                var allStocksInfo = new List<Stock>();
                foreach (var tickerSymbol in tradeTransactions)
                {
                    var stock = GetStockPrice(tickerSymbol.TickerSymbol);
                    allStocksInfo.Add(stock);

                    decimal currentPrice = 0;
                    if (stock.Trades.Count > 0)
                    {
                        currentPrice = (decimal)stock.Trades[0].PriceInPound;
                    }

                    var stockInfo = new Stock
                    {
                        TickerSymbol = tickerSymbol.TickerSymbol,
                        CurrentPrice = (double)currentPrice,
                        Trades = stock.Trades
                    };

                    allStocksInfo.Add(stockInfo);
                }
                
                return allStocksInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting stock prices: {ex.Message}");
                return new List<Stock>();
            }
      
        }
    }
}
