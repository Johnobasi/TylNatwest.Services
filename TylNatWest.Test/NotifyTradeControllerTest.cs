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
            _mockNotifyService.Setup(x => x.ReceiveTradeNotification(tradeRequest)).Returns("return ok");

            // Act
            var result = _tradeController.NotifyTrade(tradeRequest) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void NotifyTrade_WithInvalidTickerSymbol_ReturnsNotFound()
        {
            _mockNotifyService.Setup(x => x.ReceiveTradeNotification(It.IsAny<TradeTransaction>())).Returns("Failed");
            var result = _tradeController.NotifyTrade(It.IsAny<TradeTransaction>()) as NotFoundResult;
            Assert.Null(result);
        }   
    }
}
