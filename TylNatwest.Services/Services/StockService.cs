using Microsoft.Extensions.Logging;
using System.Diagnostics;
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
                var selectedStocksInfo = new List<Stock>();
                var tradeResult = _dataContext.Stocks.Where(x => x.TickerSymbol == tickerSymbol.ToUpper()).FirstOrDefault();
                if (tradeResult == null)
                {
                    throw new InvalidOperationException("No trade found");
                }

                var stock = new Stock
                {
                    TickerSymbol = tickerSymbol,
                    CurrentPrice = tradeResult.CurrentPrice,
                    TradeTransaction = tradeResult.TradeTransaction
                };

                return stock;
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
                var stockResult = _dataContext.Stocks.Where(x=> tickerSymbols.Contains(x.TickerSymbol)).ToList();
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
                            CurrentPrice = (double)currentPrice
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
                var stockResult = _dataContext.Stocks.ToList();
                foreach (var tickerSymbol in stockResult)
                {
                    var stock = GetStockPrice(tickerSymbol.TickerSymbol);

                    // Calculate the current price if there are trade transactions
                    decimal currentPrice = 0;
                    if (stock.TradeTransaction.Count > 0)
                    {
                        //get the most recent trade's price
                        currentPrice = (decimal)stock.TradeTransaction.OrderByDescending(t => t.Timestamp).First().PriceInPound;
                    }

                    // Create a new Stock object with the calculated current price
                    var stockInfo = new Stock
                    {
                        TickerSymbol = tickerSymbol.TickerSymbol,
                        CurrentPrice = (double)currentPrice,
                        TradeTransaction = stock.TradeTransaction
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
