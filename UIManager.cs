using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FolioBot
{
    public class UIManager
    {
        private ManagerFileRecord m_managerFileRecord;
        public ManagerFileRecord ManagerFileRecord { get { return m_managerFileRecord; } }

        private StatsRecord m_dbStats;
        public StatsRecord DBStats { get { return m_dbStats; } }

        private StatsRecord m_citiStats;
        public StatsRecord CITIStats { get { return m_citiStats; } }

        private StatsRecord m_jlsStats;
        public StatsRecord JLSStats { get { return m_citiStats; } }

        private bool m_existsInStitch;
        public bool ExistsInStitch { get { return m_existsInStitch; } }

        private bool m_dontShow;
        public bool DontShow { get { return m_dontShow; } set { m_dontShow = value; } }

        public UIManager(ManagerFileRecord managerFileRecord, StatsRecord dbStats, StatsRecord citiStats, StatsRecord jlsStats)
        {
            // create a new manager file record
            // in the UI for comparison with
            // disk file data
            m_managerFileRecord = new ManagerFileRecord();

            m_managerFileRecord.ManagerName = managerFileRecord.ManagerName;
            m_managerFileRecord.DBManagerName = managerFileRecord.DBManagerName;
            m_managerFileRecord.CITIManagerName = managerFileRecord.CITIManagerName;
            m_managerFileRecord.MorganStanleyManagerName = managerFileRecord.MorganStanleyManagerName;
            m_managerFileRecord.JLSName = managerFileRecord.JLSName;
            m_managerFileRecord.Location = managerFileRecord.Location;
            m_managerFileRecord.ContactName = managerFileRecord.ContactName;
            m_managerFileRecord.ContactPhone = managerFileRecord.ContactPhone;
            m_managerFileRecord.FirmCode = managerFileRecord.FirmCode;
            m_managerFileRecord.ProgramCode = managerFileRecord.ProgramCode;
            m_managerFileRecord.Strategy = managerFileRecord.Strategy;
            m_managerFileRecord.SubStrat = managerFileRecord.SubStrat;
            m_managerFileRecord.MktsFocus = managerFileRecord.MktsFocus;

            if (Utility.FindManagerInStitch(managerFileRecord.ManagerName, frmMain.MainForm.StitchDatabaseFile) != null)
                m_existsInStitch = true;
            else
                m_existsInStitch = false;


            m_dbStats = dbStats;
            m_citiStats = citiStats;
            m_jlsStats = jlsStats;
        }

        public UIManager(AliasNamesRecord aliasNamesRecord, StitchDatabaseFileRecord stitchDatabaseFileRecord, StatsRecord dbStats, StatsRecord citiStats, StatsRecord jlsStats)
        {
            // create a new manager file record
            // in the UI for comparison with
            // disk file data
            m_managerFileRecord = new ManagerFileRecord();
            
            // alias names
            m_managerFileRecord.ManagerName = aliasNamesRecord.ManagerName;
            m_managerFileRecord.DBManagerName = aliasNamesRecord.DBManagerName;
            m_managerFileRecord.CITIManagerName = aliasNamesRecord.CITIManagerName;
            m_managerFileRecord.MorganStanleyManagerName = aliasNamesRecord.MorganStanleyManagerName;
            m_managerFileRecord.JLSName = aliasNamesRecord.JLS_Has_Data_Sent_directly_from_Mgr;
            m_managerFileRecord.FirmCode = aliasNamesRecord.FirmCode;
            m_managerFileRecord.ProgramCode = aliasNamesRecord.ProgramCode;

            // stitch database
            if (stitchDatabaseFileRecord != null)
            {
                m_managerFileRecord.Strategy = stitchDatabaseFileRecord.Strategy;
                m_managerFileRecord.SubStrat = stitchDatabaseFileRecord.SubStrat;
                m_managerFileRecord.MktsFocus = stitchDatabaseFileRecord.MktsFocus;
                m_existsInStitch = true;
            }
            else
            {
                m_managerFileRecord.Strategy = String.Empty;
                m_managerFileRecord.SubStrat = String.Empty;
                m_managerFileRecord.MktsFocus = String.Empty;
                m_existsInStitch = false;
            }

            // these only exist in manager.csv file
            m_managerFileRecord.Location = "";
            m_managerFileRecord.ContactName = "";
            m_managerFileRecord.ContactPhone = "";


            //m_aliasNamesRecord = aliasNamesRecord;
            m_dbStats = dbStats;
            m_citiStats = citiStats;
            m_jlsStats = jlsStats;
        }
    }
}
