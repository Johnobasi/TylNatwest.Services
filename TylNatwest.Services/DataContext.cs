using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TylNatwest.Services.Models;

namespace TylNatwest.Services
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public DbSet<Broker> Brokers { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<TradeTransaction> TradeTransactions { get; set; }
    }
}
