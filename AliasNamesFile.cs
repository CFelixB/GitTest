using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FolioBot
{
    public class AliasNamesFile
    {
        public static Int16 Proprietary_Manager_Names_Idx = 0;
        public static Int16 DB_Consolidated_Manager_Names_Idx = 1;
        public static Int16 Alias_for_Citi_Managers_Idx = 2;
        public static Int16 Alias_for_Morgan_Stanley_Idx = 3;
        public static Int16 JLS_Has_Data_Sent_directly_from_Mgr_Idx = 4;
        public static Int16 CODE_FIRM_Idx = 5;
        public static Int16 CODE_PROG_Idx = 6;

        private string m_fileName;
        public string FileName { get { return m_fileName; } }

        private string[] m_lines;
        public string[] Lines { get { return m_lines; } }

        public AliasNamesFile(string fileName)
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
                sr = new StreamReader(m_fileName);
            }
            catch (Exception ex)
            {
                throw new Exception("AliasNamesFile.Load() : Could not open input file " + m_fileName + ".", ex);
            }
            string s = sr.ReadToEnd();

            // get file lines
            m_lines = s.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        }

        public List<AliasNamesRecord> GetList()
        {
            List<AliasNamesRecord> recs = new List<AliasNamesRecord>();

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
                AliasNamesRecord rec = new AliasNamesRecord();

                rec.ManagerName = fields[Proprietary_Manager_Names_Idx];
                rec.DBManagerName = fields[DB_Consolidated_Manager_Names_Idx];
                rec.CITIManagerName = fields[Alias_for_Citi_Managers_Idx];
                rec.MorganStanleyManagerName = fields[Alias_for_Morgan_Stanley_Idx];
                rec.JLS_Has_Data_Sent_directly_from_Mgr = fields[JLS_Has_Data_Sent_directly_from_Mgr_Idx];
                rec.FirmCode = fields[CODE_FIRM_Idx];
                rec.ProgramCode = fields[CODE_PROG_Idx];

                /*
                // convert the fields
                try
                {
                    sRate = fields[rateIdx];
                    rate = Decimal.Parse(sRate);
                }
                catch (Exception ex)
                {
                    throw new Exception("Bad rate in file " + file + " at line " + (i + 1).ToString() + "  :  " + line, ex);
                }
                */

                recs.Add(rec);
            }

            return recs;
        }

        public Dictionary<String, AliasNamesRecord> GetDictionary()
        {
            Dictionary<String, AliasNamesRecord> dict = new Dictionary<String, AliasNamesRecord>();

            List<AliasNamesRecord> list = GetList();

            foreach(AliasNamesRecord rec in list)
            {
                dict.Add(rec.ManagerName, rec);
            }
            return dict;
        }
    }    
}
