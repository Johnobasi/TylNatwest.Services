using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TylNatwest.Services.Models
{
    public class Broker
    {
        public string BrokerId { get; set; }
        public string BrokerName { get; set; }
        public string BrokerAddress { get; set; }
        public List<Stock> Stocks { get; set; }
    }
}
