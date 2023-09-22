using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TylNatwest.Services.Contracts;
using TylNatwest.Services.Models;

namespace TylNatWest.API.Controllers
{
    public class StockController : ControllerBase
    {
        private readonly IStock _stockService;
        private readonly ILogger<StockController> _logger;
        public StockController(IStock stockService, ILogger<StockController> logger)
        {
            _stockService = stockService;
            _logger = logger;   
        }


        [HttpGet("stock/{tickerSymbol}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult GetCurrentStockValue(string tickerSymbol)
        {

            if (string.IsNullOrWhiteSpace(tickerSymbol))
            {
               return BadRequest();
            }

            var stockPrice = _stockService.GetStockPrice(tickerSymbol);

            if (stockPrice == null)
            {
                _logger.LogInformation($"Stock price for {tickerSymbol} not found");
            }
            return Ok(stockPrice);

        }

        [HttpGet("stock")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult<List<Stock>> GetAll()
        {

            var stockPrices = _stockService.GetAllStockValues();
            if (stockPrices == null)
            {
                _logger.LogInformation($"Stock prices not found");
                return NotFound();
            }
            return Ok(stockPrices);

        }

        [HttpGet("stock/range")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult<List<Stock>> GetStockByStickerSymbol([FromQuery] List<string> tickerSymbols)
        {
           
                if (tickerSymbols == null || !tickerSymbols.Any())
                {
                    return BadRequest();
                }
                var stockPrices = _stockService.GetStockValuesInRange(tickerSymbols);
                if (stockPrices == null)
                {
                    _logger.LogInformation($"Stock prices not found");
                    return NotFound();
                }
                return Ok(stockPrices);
        }   

    }
}
