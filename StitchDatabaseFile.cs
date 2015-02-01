using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FolioBot
{
    public class StitchDatabaseFile
    {
        public static Int16 Number_Idx = 0;
        public static Int16 ManagerName_Idx = 1;
        public static Int16 MgrCode_Idx = 2;
        public static Int16 ProgramCode_Idx = 3;
        public static Int16 Strategy_Idx = 4;
        public static Int16 SubStrat_Idx = 5;
        public static Int16 MktsFocus_Idx = 6;
        public static Int16 Switch_Idx = 7;
        public static Int16 DB1_Idx = 8;
        public static Int16 StDt1_Idx = 9;
        public static Int16 EnDt1_Idx = 10;
        public static Int16 DB2_Idx = 11;
        public static Int16 StDt2_Idx = 12;
        public static Int16 EnDt2_Idx = 13;

        private string m_fileName;
        public string FileName { get { return m_fileName; } }

        private string[] m_lines;
        public string[] Lines { get { return m_lines; } }

        private List<StitchDatabaseFileRecord> m_recs;

        public StitchDatabaseFile(string fileName)
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
                throw new Exception("StitchFile.() : Could not open input file " + m_fileName + ".", ex);
            }
            string s = sr.ReadToEnd();

            // get file lines
            m_lines = s.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        }

        public List<StitchDatabaseFileRecord> StitchRecords()
        {
            List<StitchDatabaseFileRecord> recs = new List<StitchDatabaseFileRecord>();

            string line;
            string[] fields = null;
            int CODE_FIRM = 0;
            string field = string.Empty;
            for (int i = 1; i < m_lines.Length; i++)
            {
                line = m_lines[i];

                // skip blank lines
                if (line.Trim().Length == 0)
                    continue;

                fields = line.Split(',');

                // create new record object
                StitchDatabaseFileRecord rec = new StitchDatabaseFileRecord();

                field = fields[Number_Idx];
                field = field.Replace("\"", "");
                rec.Number = field;

                field = fields[ManagerName_Idx];
                field = field.Replace("\"", "");
                rec.ManagerName = field;

                field = fields[MgrCode_Idx];
                field = field.Replace("\"", "");
                rec.MgrCode = field;

                field = fields[ProgramCode_Idx];
                field = field.Replace("\"", "");
                rec.ProgramCode = field;

                field = fields[Strategy_Idx];
                field = field.Replace("\"", "");
                rec.Strategy = field;

                field = fields[SubStrat_Idx];
                field = field.Replace("\"", "");
                rec.SubStrat = field;

                field = fields[MktsFocus_Idx];
                field = field.Replace("\"", "");
                rec.MktsFocus = field;

                field = fields[Switch_Idx];
                field = field.Replace("\"", "");
                rec.Switch = field;

                if (fields.Length < 14)
                {
                    continue;
                }

                field = fields[DB1_Idx];
                field = field.Replace("\"", "");
                rec.DB1 = field;

                field = fields[StDt1_Idx];
                field = field.Replace("\"", "");
                if (field.Trim() != String.Empty)
                    rec.StDt1 = DateTime.Parse(field);
                
                field = fields[EnDt1_Idx];
                field = field.Replace("\"", "");
                if (field.Trim() != String.Empty)
                    rec.EnDt1 = DateTime.Parse(field);

                field = fields[DB2_Idx];
                field = field.Replace("\"", "");
                rec.DB2 = field;

                field = fields[StDt2_Idx];
                field = field.Replace("\"", "");
                if (field.Trim() != String.Empty)
                    rec.StDt2 = DateTime.Parse(field);

                field = fields[EnDt2_Idx];
                field = field.Replace("\"", "");
                if (field.Trim() != String.Empty)
                    rec.EnDt2 = DateTime.Parse(field);
                recs.Add(rec);
            }

            return recs;
        }

        public void WriteToFile(List<StitchDatabaseFileRecord> recs, string path)
        {
            string fileName = path + @"\Manager.csv";
            StreamWriter sw = new StreamWriter(fileName, false);
            string header = @"Number, ManagerName, MgrCode, ProgramCode, Strategy, SubStrat, MktsFocus, Switch, DB1, StDt1, EnDt1, DB2, StDt2, EnDt2";
            sw.WriteLine(header);

            // write detail
            string detail = String.Empty;
            for (int i = 0; i < recs.Count; i++)
            {
                detail =
                    recs[i].Number + "," +
                    recs[i].ManagerName + "," +
                    recs[i].MgrCode + "," +
                    recs[i].ProgramCode + "," +
                    recs[i].Strategy + "," +
                    recs[i].SubStrat + "," +
                    recs[i].MktsFocus + "," +
                    recs[i].Switch + "," +
                    recs[i].DB1 + "," +
                    recs[i].StDt1.ToString("M/d/yyyy") + "," +
                    recs[i].EnDt1.ToString("M/d/yyyy") + "," +
                    recs[i].DB2 + "," +
                    recs[i].StDt2.ToString("M/d/yyyy") + "," +
                    recs[i].EnDt2.ToString("M/d/yyyy");
                sw.WriteLine(detail);
            }

            sw.Close();

            // if all went well, 
            m_fileName = fileName;
            m_recs = recs;
        }

        public Dictionary<String, StitchDatabaseFileRecord> GetDictionary()
        {
            Dictionary<String, StitchDatabaseFileRecord> dict = new Dictionary<String, StitchDatabaseFileRecord>();

            List<StitchDatabaseFileRecord> list = StitchRecords();

            foreach (StitchDatabaseFileRecord rec in list)
            {
                dict.Add(rec.ManagerName, rec);
            }
            return dict;
        }
    }
}