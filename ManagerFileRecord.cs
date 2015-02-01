using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FolioBot
{
    public class ManagerFileRecord
    {
        // alias names
        public string ManagerName;
        public string DBManagerName;
        public string CITIManagerName;
        public string MorganStanleyManagerName;
        public string JLSName;

        public string Location;
        public string ContactName;
        public string ContactPhone;

        public string FirmCode; // MgrCode in stitch database
        public string ProgramCode;

        // stitch database
        public string Strategy;
        public string SubStrat;
        public string MktsFocus;

        public ManagerFileRecord()
        {
            ManagerName = String.Empty;
            DBManagerName = String.Empty;
            CITIManagerName = String.Empty;
            MorganStanleyManagerName = String.Empty;
            JLSName = String.Empty;
            FirmCode = String.Empty;
            ProgramCode = String.Empty;
            Strategy = String.Empty;
            SubStrat = String.Empty;
            MktsFocus = String.Empty;
        }
    }
}
