using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FolioBot
{
    public class ManagerFile
    {
        // used for creating file if necessary
        public static readonly string BaseFileName = @"\Manager.csv";
        protected static readonly string header = @"ManagerName, DBManagerName, CITIManagerName, MorganStanleyManagerName, SendsDataDirect, Location, ContactName, ContactPhone, FirmCode, ProgramCode";
        // create and empty manager file
        public static void CreateFile(string path)
        {
            string fileName = path + BaseFileName;
            StreamWriter sw = new StreamWriter(fileName, false);
            sw.WriteLine(header);
            sw.Close();
        }


        public static Int16 Proprietary_Manager_Names_Idx = 0;
        public static Int16 DB_Consolidated_Manager_Names_Idx = 1;
        public static Int16 Alias_for_Citi_Managers_Idx = 2;
        public static Int16 Alias_for_Morgan_Stanley_Idx = 3;
        public static Int16 JLSName_Idx = 4;
        public static Int16 Location_Idx = 5;
        public static Int16 ContactName_Idx = 6;
        public static Int16 ContactPhone_Idx = 7;
        public static Int16 CODE_FIRM_Idx = 9;
        public static Int16 CODE_PROG_Idx = 9;
        public static Int16 Strategy_Idx = 10;
        public static Int16 SubStrat_Idx = 11;
        public static Int16 MktsFocus_Idx = 12;

        private string m_fileName;
        public string FileName { get { return m_fileName; } }

        private string[] m_lines;
        public string[] Lines { get { return m_lines; } }

        private List<ManagerFileRecord> m_recs;

        public ManagerFile(string fileName)
        {
            m_fileName = fileName;
            ReadFile();
        }
        private void ReadFile()
        {
            // read file
            StreamReader sr = null;
            try
            {
                string fname = m_fileName + BaseFileName;
                sr = new StreamReader(fname);
            }
            catch (Exception ex)
            {
                throw new Exception("ManagerFile.Load() : Could not open input file " + m_fileName + ".", ex);
            }
            string s = sr.ReadToEnd();

            // get file lines
            m_lines = s.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        }

        public List<ManagerFileRecord> GetList()
        {
            List<ManagerFileRecord> recs = new List<ManagerFileRecord>();

            string line;
            string[] fields = null;
            int CODE_FIRM = 0;
            for (int i = 1; i < m_lines.Length; i++)
            {
                line = m_lines[i];

                // skip blank lines
                if (line.Trim().Length == 0)
                    continue;

                fields = line.Split(',');

                // create new record object
                ManagerFileRecord rec = new ManagerFileRecord();

                // alias names
                rec.ManagerName = fields[Proprietary_Manager_Names_Idx];
                rec.DBManagerName = fields[DB_Consolidated_Manager_Names_Idx];
                rec.CITIManagerName = fields[Alias_for_Citi_Managers_Idx];
                rec.MorganStanleyManagerName = fields[Alias_for_Morgan_Stanley_Idx];
                rec.JLSName = fields[JLSName_Idx];

                rec.Location = fields[Location_Idx];
                rec.ContactName = fields[ContactName_Idx];
                rec.ContactPhone = fields[ContactPhone_Idx];

                rec.FirmCode = fields[CODE_FIRM_Idx];
                rec.ProgramCode = fields[CODE_PROG_Idx];

                // stitch
                rec.Strategy = fields[Strategy_Idx];
                rec.SubStrat = fields[SubStrat_Idx];
                rec.MktsFocus = fields[MktsFocus_Idx];

                recs.Add(rec);
            }

            return recs;
        }

        public void WriteToFile(List<ManagerFileRecord> recs, string path)
        {
            string fileName = path + BaseFileName;
            StreamWriter sw = new StreamWriter(fileName, false);
            sw.WriteLine(header);

            // write detail
            string detail = String.Empty;
            for (int i = 0; i < recs.Count; i++)
            {
                detail =
                    recs[i].ManagerName + "," +
                    recs[i].DBManagerName + "," +
                    recs[i].CITIManagerName + "," +
                    recs[i].MorganStanleyManagerName + "," +
                    recs[i].JLSName + "," +

                    recs[i].Location + "," +
                    recs[i].ContactName + "," +
                    recs[i].ContactPhone + "," +

                    recs[i].FirmCode + "," +
                    recs[i].ProgramCode + "," +
                    recs[i].Strategy + "," +
                    recs[i].SubStrat + "," +
                    recs[i].MktsFocus;
                sw.WriteLine(detail);
            }

            sw.Close();

            // if all went well, 
            m_fileName = fileName;
            m_recs = recs;
        }

        public Dictionary<String, ManagerFileRecord> GetDictionary()
        {
            Dictionary<String, ManagerFileRecord> dict = new Dictionary<String, ManagerFileRecord>();

            List<ManagerFileRecord> list = GetList();

            foreach (ManagerFileRecord rec in list)
            {
                dict.Add(rec.ManagerName, rec);
            }
            return dict;
        }
    }
}
