using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace TylNatwest.Services.Models
{
    public class StockExchangeDataManager
    {
        private readonly IServiceProvider _serviceProvider;

        public StockExchangeDataManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }



        public void SeedData()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DataContext>();
                var existingBroker = context.Brokers.FirstOrDefault(b => b.BrokerId == "B1");
                if (existingBroker == null)
                {
                        var broker1 = new Broker
                        {
                            BrokerId = "B1",
                            BrokerName = "Broker One",
                            BrokerAddress = "123 Main Street, New York",
                            Stocks = new List<Stock>
                            {
                                new Stock
                                {
                                    TickerSymbol = "AAPL",
                                    CurrentPrice = 150.50
                                },
                                new Stock
                                {
                                    TickerSymbol = "GOOGL",
                                    CurrentPrice = 2700.75
                                },
                                new Stock
                                {
                                    TickerSymbol = "MSFT",
                                    CurrentPrice = 300.25
                                }
                            },
                            TradeTransactions = new List<TradeTransaction>
                            {
                                new TradeTransaction
                                {
                                    BrokerID = "B1",
                                    TickerSymbol = "AAPL",
                                    Id = 1,
                                    PriceInPound = 150.50,
                                    Shares = 100,
                                    Timestamp = DateTime.UtcNow,
                                },
                                new TradeTransaction
                                {
                                    BrokerID = "B1",
                                    TickerSymbol = "GOOGL",
                                    Id = 2,
                                    PriceInPound = 2700.75,
                                    Shares = 100,
                                    Timestamp = DateTime.UtcNow,
                                },
                            }
                        };
                    var broker2 = new Broker
                    {
                        BrokerAddress = "456 Main Street, New York",
                        BrokerId = "B2",
                        BrokerName = "Broker Two",
                        Stocks = new List<Stock>
                        {
                            new Stock
                            {
                                TickerSymbol = "AMZN",
                                CurrentPrice = 3500.50
                            },
                            new Stock
                            {
                                TickerSymbol = "TSLA",
                                CurrentPrice = 750.00
                            }
                        },
                        TradeTransactions = new List<TradeTransaction>
                        {
                            new TradeTransaction
                            {
                                BrokerID = "B2",
                                TickerSymbol = "AMZN",
                                Id = 3,
                                PriceInPound = 3500.50,
                                Shares = 100,
                                Timestamp = DateTime.UtcNow,
                            },
                            new TradeTransaction
                            {
                                BrokerID = "B2",
                                TickerSymbol = "TSLA",
                                Id = 4,
                                PriceInPound = 750.00,
                                Shares = 100,
                                Timestamp = DateTime.UtcNow,
                            },
                        }
                    };
                    context.Brokers.AddRange(broker1, broker2);
                }

                

                context.SaveChanges();
            }
        }
    }

}

