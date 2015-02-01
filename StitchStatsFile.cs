using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FolioBot
{
    public class StitchStatsFile
    {
        public static Int16 Actual_Idx = 0;
        public static Int16 Stdt_Idx = 1;
        public static Int16 Enddt_Idx = 2;
        public static Int16 Average_Idx = 3;
        public static Int16 RoR_Idx = 4;
        public static Int16 Volty_Idx = 5;
        public static Int16 Sortino_Idx = 6;
        public static Int16 Corr_Idx = 7;
        public static Int16 VIXCorr_Idx = 8;
        public static Int16 CTACorr_Idx = 9;

        private string m_fileName;
        public string FileName { get { return m_fileName; } }

        private string[] m_lines;
        public string[] Lines { get { return m_lines; } }

        public StitchStatsFile(string fileName)
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
                throw new Exception("StitchStatsFile.Load() : Could not open input file " + m_fileName + ".", ex);
            }
            string s = sr.ReadToEnd();

            // get file lines
            m_lines = s.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        }

        public List<StitchStatsRecord> StatsRecords()
        {
            List<StitchStatsRecord> recs = new List<StitchStatsRecord>();

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
                StitchStatsRecord rec = new StitchStatsRecord();

                rec.ManagerName = fields[Actual_Idx];
                rec.StartDate = DateTime.Parse(fields[Stdt_Idx]);
                rec.EndDate = DateTime.Parse(fields[Enddt_Idx]);
                rec.Average = Decimal.Parse(fields[Average_Idx]);
                rec.RoR = Decimal.Parse(fields[RoR_Idx]);
                rec.Volatility = Decimal.Parse(fields[Volty_Idx]);
                rec.Sortino = Decimal.Parse(fields[Sortino_Idx]);
                rec.Correlation = Decimal.Parse(fields[Corr_Idx]);
                rec.VIXCorrelation = Decimal.Parse(fields[VIXCorr_Idx]);
                rec.CTACorrelation = Decimal.Parse(fields[CTACorr_Idx]);

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
    }
}
