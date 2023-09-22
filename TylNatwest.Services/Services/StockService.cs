using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq.Expressions;
using TylNatwest.Services.Contracts;
using TylNatwest.Services.Models;

namespace TylNatwest.Services.Services
{
    public class StockService : IStock
    {
        private readonly ILogger<StockService> _logger;
        private readonly DataContext _dataContext;
        public StockService(ILogger<StockService> logger, DataContext dataContext)
        {
            _logger = logger;    
            _dataContext = dataContext;
        }


        public Stock GetStockPrice(string tickerSymbol)
        {

            try
            {
                var tradeResult = _dataContext.Stocks
                    .Include(x=>x.Broker)
                        .Include(C=>C.TradeTransaction)
                            .Where(x => x.TickerSymbol == tickerSymbol.ToUpper())
                                .FirstOrDefault();
                if (tradeResult == null)
                {
                    throw new InvalidOperationException("No trade found");
                }

                 var stocks = new Stock
                 {
                    TickerSymbol = tickerSymbol,
                    CurrentPrice = tradeResult.CurrentPrice,
                        Broker = tradeResult.Broker != null ? new Broker
                        {
                            BrokerId  = "T3",
                            BrokerName = "TESTING",
                            BrokerAddress = "London"
                        } : null, 
                    TradeTransaction = tradeResult.TradeTransaction != null && tradeResult.TradeTransaction.Any()? tradeResult.TradeTransaction.Select(trade => new TradeTransaction
                    {
                        BrokerID = "T3",
                        PriceInPound = tradeResult.CurrentPrice, 
                        Timestamp = DateTime.UtcNow,
                        Id = 90,
                        Shares = 20,
                        TickerSymbol = tradeResult.TickerSymbol,
                        TradeID = tradeResult.Id.ToString()
                    }).ToList()
                    : null // Handle null TradeTransaction
                    
                 };

                return stocks;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting stock info for {tickerSymbol}: {ex.Message}");
                return null; // I return null here to indicate an error
            }

        }

        public List<Stock> GetStockValuesInRange(List<string> tickerSymbols)
        {
            try
            {
                var selectedStocksInfo = new List<Stock>();
                var stockResult = _dataContext.Stocks.Include(x=>x.Broker).Where(x=> tickerSymbols.Contains(x.TickerSymbol)).ToList();
                if (stockResult != null)
                {
                    foreach (var stock in stockResult)
                    {

                        decimal currentPrice = 0;
                        // Assuming that stock has a collection of trade transactions
                        if (stock.TradeTransaction != null && stock.TradeTransaction.Any())
                        {
                            // Calculate the current price based on the most recent trade
                            currentPrice = (decimal)stock.TradeTransaction
                                .OrderByDescending(transaction => transaction.Timestamp)
                                .First().PriceInPound;
                        }

                        var stockInfo = new Stock
                        {
                            TickerSymbol = stock.TickerSymbol,
                            CurrentPrice = (double)currentPrice,
                            TradeTransaction = new List<TradeTransaction>
                            {
                              new TradeTransaction
                              {
                                Shares = 10,
                                BrokerID = stock.Broker.BrokerId,
                                Timestamp = DateTime.UtcNow,
                                PriceInPound = (double)currentPrice,
                                TickerSymbol = stock.TickerSymbol
                              }
                            },
                            Broker = stock.Broker,
                            Id = stock.Id
                        };

                        selectedStocksInfo.Add(stockInfo);

                    }
                }
                return selectedStocksInfo;

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting range of stock prices for {string.Join(", ", tickerSymbols)}: {ex.Message}");
                return new List<Stock>();
            }

        }

        public List<Stock> GetAllStockValues()
        {
            try
            {
                var allStocksInfo = new List<Stock>();
                var stockResult = _dataContext.Stocks.Include(x => x.Broker).Include(c=>c.TradeTransaction).ToList();
                foreach (var tickerSymbol in stockResult)
                {

                    // Calculate the current price if there are trade transactions
                    decimal currentPrice = 0;
                    if (tickerSymbol.TradeTransaction != null && tickerSymbol.TradeTransaction.Count > 0)
                    {
                        //get the most recent trade's price
                        currentPrice = (decimal)tickerSymbol.TradeTransaction.OrderByDescending(t => t.Timestamp).First().PriceInPound;
                    }

                    // Create a new Stock object with the calculated current price
                    var stockInfo = new Stock
                    {
                        TickerSymbol = tickerSymbol.TickerSymbol,
                        CurrentPrice = (double)currentPrice,
                        TradeTransaction = new List<TradeTransaction>
                        {
                              new TradeTransaction
                              {
                                Shares = 10,
                                BrokerID = tickerSymbol.Broker.BrokerId,
                                Timestamp = DateTime.UtcNow,
                                PriceInPound = (double)currentPrice,
                                TickerSymbol = tickerSymbol.TickerSymbol
                              }
                        },
                        Broker = tickerSymbol.Broker,
                        Id = tickerSymbol.Id                       

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
