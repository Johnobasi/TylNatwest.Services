using TylNatwest.Services.Models;

namespace TylNatwest.Services.Contracts
{
    public interface IStock
    {
        decimal GetStockPrice(string tickerSymbol);
        List<Stock> GetAllStockValues();
        List<Stock> GetStockValuesInRange(List<string> tickerSymbols);
    }
}
