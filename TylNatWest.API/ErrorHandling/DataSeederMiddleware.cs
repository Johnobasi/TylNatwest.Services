using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TylNatwest.Services.Models;

namespace TylNatWest.API.ErrorHandling
{
    public class DataSeederMiddleware
    {
        private readonly RequestDelegate _next;

        public DataSeederMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, StockExchangeDataManager dataSeeder)
        {
            dataSeeder.SeedData();
            await _next(context);
        }
    }

    public static class DataSeederMiddlewareExtensions
    {
        public static IApplicationBuilder UseDataSeeder(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DataSeederMiddleware>();
        }
    }
}
