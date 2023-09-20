using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using TylNatwest.Services.Contracts;
using TylNatwest.Services.Models;

namespace TylNatWest.API.Controllers
{
    public class NotifyTradeController : ControllerBase
    {
        private readonly INotifyTrade _tradeNotify;
        private readonly ILogger<NotifyTradeController> _logger;
        public NotifyTradeController(INotifyTrade tradeNotify, ILogger<NotifyTradeController> logger)
        {
            _tradeNotify = tradeNotify;
            _logger = logger;
        }

        [HttpPost("trade")]
        public IActionResult NotifyTrade([FromBody] TradeTransaction request)
        {
          
                if (request == null)
                {
                        return BadRequest("Invalid trade data");
                }
                
                if (string.IsNullOrEmpty(request.TickerSymbol))
                {
                    return BadRequest("TickerSymbol cannot be empty");
                }

                string result = _tradeNotify.ReceiveTradeNotification(request);
                if (result == "Invalid trade data")
                {
                    _logger.LogInformation($"Stock price for {request.TickerSymbol} not found");
                      return BadRequest();
                }

                return Ok(result);
        }
    }
}
