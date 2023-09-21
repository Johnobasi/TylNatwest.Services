using Microsoft.Extensions.DependencyInjection;
using TylNatwest.Services.Contracts;
using TylNatwest.Services.Models;
using TylNatwest.Services.Services;

namespace TylNatwest.Services
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddSingleton<StockExchangeDataManager>();
            services.AddScoped<IStock, StockService>();
            services.AddScoped<INotifyTrade, TradeNotificationService>();
            return services;
        }
    }
}