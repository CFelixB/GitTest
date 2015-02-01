using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FolioBot
{
    public enum RebalanceFrequencyType {None, Yearly, Monthly, Weekly, Daily}

    public class Analysis
    {
        public decimal MgrMgmtFees;
        public decimal FixedFees;
        public decimal Initial;
        public DateTime FinalDt;
        public decimal MgrIncentiveFees;
        public decimal Incentive;
        public decimal MinAlloc;
        public RebalanceFrequencyType RebalFreq;
        public List<ManagerSettings> ManagerSettings;

        public Analysis()
        {
            ManagerSettings = new List<ManagerSettings>();
        }
    }
}
