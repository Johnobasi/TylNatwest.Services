using Microsoft.Extensions.Logging;
using Moq;
using TylNatWest.API.Controllers;
using TylNatwest.Services.Contracts;
using TylNatwest.Services.Models;
using Microsoft.AspNetCore.Mvc;

namespace TylNatWest.Test
{
    public class NotifyTradeControllerTest
    {
        private readonly Mock<INotifyTrade> _mockNotifyService;
        private readonly Mock<ILogger<NotifyTradeController>> _mockLogger;
        private readonly NotifyTradeController _tradeController;
        public NotifyTradeControllerTest()
        {
            _mockNotifyService = new Mock<INotifyTrade>();
            _mockLogger = new Mock<ILogger<NotifyTradeController>>();
            _tradeController = new NotifyTradeController(_mockNotifyService.Object, _mockLogger.Object);
        }

        [Fact]
        public void NotifyTrade_WithValidTickerSymbol_ReturnsStockPrice()
        {
            // Arrange
            var tickerSymbol = "AAPL";
            var tradeRequest = new TradeTransaction { TickerSymbol = tickerSymbol, BrokerID = "test", PriceInPound = 10.1, Shares = 30.00, TradeID = "qwe2637u" };
            var stockPrice = new Stock { TickerSymbol = tickerSymbol, CurrentPrice = 150.50 };
            _mockNotifyService.Setup(x => x.ReceiveTradeNotification(tradeRequest)).Returns("return ok");

            // Act
            var result = _tradeController.NotifyTrade(tradeRequest) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(stockPrice, result.Value);
        }

        [Fact]
        public void NotifyTrade_WithInvalidTickerSymbol_ReturnsNotFound()
        {
            // Arrange
            var tickerSymbol = "1234";
            var tradeRequest = new TradeTransaction { TickerSymbol = tickerSymbol, BrokerID = "test", PriceInPound = 10.1, Shares = 30.00, TradeID = "qwe2637u" };
            _mockNotifyService.Setup(x => x.ReceiveTradeNotification(tradeRequest)).Returns("returned ok");

            // Act
            var result = _tradeController.NotifyTrade(tradeRequest) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }   
    }
}
