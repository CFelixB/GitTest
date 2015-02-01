using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FolioBot
{
    class Utility
    {
        public static StitchDatabaseFileRecord FindManagerInStitch(string managerName, StitchDatabaseFile stitchFile)
        {
            StitchDatabaseFileRecord rtn = null;

            List<StitchDatabaseFileRecord> recs = stitchFile.StitchRecords();

            foreach (StitchDatabaseFileRecord rec in recs)
            {
                if (rec.ManagerName == managerName)
                {
                    rtn = rec;
                    break;
                }
            }

            return rtn;
        }

        public static StatsRecord FindManagerInStats(string managerName, StatsFile statsFile)
        {
            StatsRecord statsRecord = null;

            List<StatsRecord> statsRecords = statsFile.StatsRecords();

            foreach (StatsRecord rec in statsRecords)
            {
                if (rec.ManagerName == managerName)
                {
                    statsRecord = rec;
                    break;
                }
            }

            return statsRecord;
        }
    }
}
