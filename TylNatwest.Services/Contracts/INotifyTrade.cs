using TylNatwest.Services.Models;

namespace TylNatwest.Services.Contracts
{
    public interface INotifyTrade
    {
        string ReceiveTradeNotification(TradeTransaction request);
    }
}
