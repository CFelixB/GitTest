using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FolioBot
{
    public class StatsRecord
    {
        public string ManagerName;
        public DateTime StartDate;
        public DateTime EndDate;
        public decimal Average;
        public decimal Volatility;
        public decimal Sortino;
        public decimal Correlation;
        public decimal VIXCorrelation;
        public decimal CTACorrelation;

    }
}
