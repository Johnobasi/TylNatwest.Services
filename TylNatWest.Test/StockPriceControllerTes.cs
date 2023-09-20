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
            // Arrange
            var tickerSymbol = "AAPL";
            var expectedStockPrice = new Stock { TickerSymbol = tickerSymbol, CurrentPrice = 150.50 };

            _mockStockService.Setup(x => x.GetStockPrice(tickerSymbol)).Returns((decimal)expectedStockPrice.CurrentPrice);

            // Act
            var result = _stockController.GetCurrentStockValue(tickerSymbol) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal((decimal)expectedStockPrice.CurrentPrice, (decimal)result.Value);
        }

        [Fact]
        public void GetStockPrice_WithInvalidTickerSymbol_ReturnsNotFound()
        {
            // Arrange
            _mockStockService.Setup(x => x.GetStockPrice(It.IsAny<string>())).Returns(0m);

            // Act
            var result = _stockController.GetCurrentStockValue(It.IsAny<string>()) as NotFoundResult;

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetAllStockValues_ReturnsStockPrices()
        {
            // Arrange
            var stockPrices = new List<Stock>
            {
                new Stock { TickerSymbol = "AAPL", CurrentPrice = 150.50 },
                new Stock { TickerSymbol = "MSFT", CurrentPrice = 250.50 },
                new Stock { TickerSymbol = "GOOG", CurrentPrice = 350.50 }
            };
            _mockStockService.Setup(x => x.GetAllStockValues()).Returns(stockPrices);

            // Act
            var result = _stockController.GetAll();

            // Assert
            Assert.IsType<ActionResult<List<Stock>>>(result);
        }

        [Fact]
        public void GetAllStockValues_ReturnsStockPrices_WithNoStockPrices()
        {
            // Arrange

            _mockStockService.Setup(x => x.GetAllStockValues()).Returns(new List<Stock>
            {
                new Stock { TickerSymbol = "AAPL", CurrentPrice = 150.50 },
            });

            // Act
            var result = _stockController.GetAll();

            // Assert
            Assert.IsType<ActionResult<List<Stock>>>(result);
        }

        [Fact]
        public void GetStockValuesInRange_ReturnsStockPrices()
        {
            // Arrange
            var tickerSymbols = new List<string> { "AAPL", "MSFT", "GOOG" };
            var stockPrices = new List<Stock>
            {
                new Stock { TickerSymbol = "AAPL", CurrentPrice = 150.50 },
                new Stock { TickerSymbol = "MSFT", CurrentPrice = 250.50 },
                new Stock { TickerSymbol = "GOOG", CurrentPrice = 350.50 }
            };
            _mockStockService.Setup(x => x.GetStockValuesInRange(tickerSymbols)).Returns(stockPrices);

            // Act
            var result = _stockController.GetStockByStickerSymbol(tickerSymbols);

            // Assert
            Assert.IsType<ActionResult<List<Stock>>>(result);
        }

        [Fact]
        public void GetStockValuesInRange_ReturnsStockPrices_WithNoStockPrices()
        {
            // Arrange
            _mockStockService.Setup(x => x.GetStockValuesInRange(It.IsAny<List<string>>())).Returns(new List<Stock>
            {
                new Stock { TickerSymbol = "AAPL", CurrentPrice = 150.50 },
            });

            // Act
            var result = _stockController.GetStockByStickerSymbol(It.IsAny<List<string>>());

            // Assert
            Assert.IsType<ActionResult<List<Stock>>>(result);
        }
    }
}