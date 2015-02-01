using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FolioBot
{
    public class StatsFile
    {
        public static Int16 Actual_Idx = 0;
        public static Int16 Stdt_Idx = 1;
        public static Int16 Enddt_Idx = 2;
        public static Int16 Average_Idx = 3;
        public static Int16 Volty_Idx = 4;
        public static Int16 Sortino_Idx = 5;
        public static Int16 Corr_Idx = 6;
        public static Int16 VIXCorr_Idx = 7;
        public static Int16 CTACorr_Idx = 8;

        private string m_fileName;
        public string FileName { get { return m_fileName; } }

        private string[] m_lines;
        public string[] Lines { get { return m_lines; } }

        public StatsFile(string fileName)
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
                    throw new Exception("StatsFile.Load() : Could not open input file " + m_fileName + ".", ex);
                }
                string s = sr.ReadToEnd();

                // get file lines
                m_lines = s.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            }

        public List<StatsRecord> StatsRecords()
        {
            List<StatsRecord> recs = new List<StatsRecord>();

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
                StatsRecord rec = new StatsRecord();

                field = fields[Actual_Idx];
                field = field.Replace("\"", "");
                rec.ManagerName = field;

                field = fields[Stdt_Idx];
                field = field.Replace("\"", "");
                rec.StartDate = DateTime.Parse(field);

                field = fields[Enddt_Idx];
                field = field.Replace("\"", "");
                rec.EndDate = DateTime.Parse(field);

                field = fields[Average_Idx];
                field = field.Replace("\"", "");
                if (field != "NA")
                {
                    rec.Average = Decimal.Parse(field);
                }
                else
                    rec.Average = 0;

                field = fields[Volty_Idx];
                field = field.Replace("\"", "");
                if (field != "NA")
                {
                    rec.Volatility = Decimal.Parse(field);
                }
                else
                    rec.Volatility = 0;

                field = fields[Sortino_Idx];
                field = field.Replace("\"", "");
                if (field != "NA")
                {
                    rec.Sortino = Decimal.Parse(field);
                }
                else
                    rec.Sortino = 0;

                field = fields[Corr_Idx];
                field = field.Replace("\"", "");
                if (field != "NA")
                {
                    rec.Correlation = Decimal.Parse(field);
                }
                else
                    rec.Correlation = 0;

                field = fields[VIXCorr_Idx];
                field = field.Replace("\"", "");
                if (field != "NA")
                {
                    rec.VIXCorrelation = Decimal.Parse(field);
                }
                else
                    rec.VIXCorrelation = 0;

                field = fields[CTACorr_Idx];
                field = field.Replace("\"", "");
                if (field != "NA")
                {
                    rec.CTACorrelation = Decimal.Parse(field);
                }
                else
                    rec.CTACorrelation = 0;

                recs.Add(rec);
            }

            return recs;
        }
    }
}
