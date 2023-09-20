using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TylNatwest.Services.Contracts;
using TylNatwest.Services.Models;
using TylNatWest.API.Controllers;

namespace TylNatWest.Test
{
    public class StockPriceControllerTes
    {
        private readonly Mock<IStock> _mockStockService;
        private readonly Mock<ILogger<StockController>> _mockLogger;
        private readonly StockController _stockController;
        public StockPriceControllerTes()
        {
            _mockStockService = new Mock<IStock>();
            _mockLogger = new Mock<ILogger<StockController>>();
            _stockController = new StockController(_mockStockService.Object, _mockLogger.Object);
        }   

        [Fact]
        public void GetStockPrice_WithValidTickerSymbol_ReturnsStockPrice()
        {
            var tickerSymbol = "AAPL";
            _mockStockService.Setup(x => x.GetStockPrice(tickerSymbol)).Returns(new Stock
            {
                TickerSymbol = tickerSymbol,
                CurrentPrice = 150.50,
                Trades = new List<TradeTransaction>
                {
                    new TradeTransaction { TickerSymbol = tickerSymbol, BrokerID = "test", PriceInPound = 10.1, Shares = 30.00, TradeID = "qwe2637u" }
                }
            });
            var result = _stockController.GetCurrentStockValue(tickerSymbol) as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void GetStockPrice_WithInvalidTickerSymbol_ReturnsNotFound()
        {
            _mockStockService.Setup(x => x.GetStockPrice(It.IsAny<string>())).Returns(new Stock());

            var result = _stockController.GetCurrentStockValue(It.IsAny<string>()) as NotFoundResult;
            Assert.Null(result);
        }

        [Fact]
        public void GetAllStockValues_ReturnsStockPrices()
        {
            var stockPrices = new List<Stock>
            {
                new Stock { TickerSymbol = "AAPL", CurrentPrice = 150.50 },
                new Stock { TickerSymbol = "MSFT", CurrentPrice = 250.50 },
                new Stock { TickerSymbol = "GOOG", CurrentPrice = 350.50 }
            };
            _mockStockService.Setup(x => x.GetAllStockValues()).Returns(stockPrices);
            var result = _stockController.GetAll();
            Assert.IsType<ActionResult<List<Stock>>>(result);
        }

        [Fact]
        public void GetAllStockValues_ReturnsStockPrices_WithNoStockPrices()
        {
            _mockStockService.Setup(x => x.GetAllStockValues()).Returns(new List<Stock>
            {
                new Stock { TickerSymbol = "AAPL", CurrentPrice = 150.50 },
            });
            var result = _stockController.GetAll();
            Assert.IsType<ActionResult<List<Stock>>>(result);
        }

        [Fact]
        public void GetStockValuesInRange_ReturnsStockPrices()
        {
            var tickerSymbols = new List<string> { "AAPL", "MSFT", "GOOG" };
            var stockPrices = new List<Stock>
            {
                new Stock { TickerSymbol = "AAPL", CurrentPrice = 150.50 },
                new Stock { TickerSymbol = "MSFT", CurrentPrice = 250.50 },
                new Stock { TickerSymbol = "GOOG", CurrentPrice = 350.50 }
            };
            _mockStockService.Setup(x => x.GetStockValuesInRange(tickerSymbols)).Returns(stockPrices);

            var result = _stockController.GetStockByStickerSymbol(tickerSymbols);

            Assert.IsType<ActionResult<List<Stock>>>(result);
        }

        [Fact]
        public void GetStockValuesInRange_ReturnsStockPrices_WithNoStockPrices()
        {
            _mockStockService.Setup(x => x.GetStockValuesInRange(It.IsAny<List<string>>())).Returns(new List<Stock>
            {
                new Stock { TickerSymbol = "AAPL", CurrentPrice = 150.50 },
            });
            var result = _stockController.GetStockByStickerSymbol(It.IsAny<List<string>>());
            Assert.IsType<ActionResult<List<Stock>>>(result);
        }
    }
}