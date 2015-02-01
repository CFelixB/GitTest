using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FolioBot
{
    class BenchmarkIndexFile
    {
        public static Int16 Date_Idx = 0;
        public static Int16 Open_Idx = 1;
        public static Int16 High_Idx = 2;
        public static Int16 Low_Idx = 3;
        public static Int16 Close_Idx = 4;
        public static Int16 Volume_Idx = 5;
        public static Int16 AdjustedClose_Idx = 6;

        private string m_fileName;
        public string FileName { get { return m_fileName; } }

        private string[] m_lines;
        public string[] Lines { get { return m_lines; } }

        private List<BenchmarkIndexFileRecord> m_recs;

        public BenchmarkIndexFile(string fileName)
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
                throw new Exception("BenchmarkIndexFile.ReadFile() : Could not open input file " + m_fileName + ".", ex);
            }
            string s = sr.ReadToEnd();

            // get file lines
            m_lines = s.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            Records();
        }


        public List<BenchmarkIndexFileRecord> Records()
        {
            // return list to be used outside of this class
            // ? save m_recs at end of proc
            // ? so we can compare?
            List<BenchmarkIndexFileRecord> recs = new List<BenchmarkIndexFileRecord>();

            string line;
            string[] fields = null;
            string field = string.Empty;
            for (int i = 1; i < m_lines.Length; i++)
            {
                line = m_lines[i];

                // skip blank lines
                if (line.Trim().Length == 0)
                    continue;

                fields = line.Split(',');

                // create new record object
                BenchmarkIndexFileRecord rec = new BenchmarkIndexFileRecord();

                field = fields[Date_Idx];
                field = field.Replace("\"", "");
                rec.Date = DateTime.Parse(field);

                field = fields[Open_Idx];
                field = field.Replace("\"", "");
                rec.Open = Decimal.Parse(field);

                field = fields[High_Idx];
                field = field.Replace("\"", "");
                rec.High = Decimal.Parse(field);

                field = fields[Low_Idx];
                field = field.Replace("\"", "");
                rec.Low = Decimal.Parse(field);

                field = fields[Close_Idx];
                field = field.Replace("\"", "");
                rec.Close = Decimal.Parse(field);

                field = fields[Volume_Idx];
                field = field.Replace(".0", "");
                field = field.Replace("\"", "");
                rec.Volume = Int64.Parse(field);

                field = fields[AdjustedClose_Idx];
                field = field.Replace("\"", "");
                rec.AdjustedClose = Decimal.Parse(field);

                /*
                field = fields[StDt1_Idx];
                field = field.Replace("\"", "");
                if (field.Trim() != String.Empty)
                    rec.StDt1 = DateTime.Parse(field);
                */

                recs.Add(rec);
            }

            m_recs = recs;
            return recs;
        }

        public List<DateDelta> MonthlyDeltas()
        {
            List<DateDelta> monthlyDeltas = new List<DateDelta>();

            // if no records, return empty list
            if ((m_recs == null) || (m_recs.Count == 0))
                return monthlyDeltas;
           

            BenchmarkIndexFileRecord monthBeginRec = m_recs[0];
            BenchmarkIndexFileRecord currRec = m_recs[0];
            
            monthlyDeltas.Add(new DateDelta(monthBeginRec.Date));
            monthlyDeltas[monthlyDeltas.Count - 1].Delta = 0;

            for (int i = 0; i < m_recs.Count; i++)
            { 
                // new month
                if ((m_recs[i].Date.Month != monthBeginRec.Date.Month) ||
                    (m_recs[i].Date.Year != monthBeginRec.Date.Year))
                {
                    monthBeginRec = m_recs[i];
                    monthlyDeltas.Add(new DateDelta(monthBeginRec.Date));
                }

                // calculate current delta
                currRec = m_recs[i];
                decimal diff = currRec.AdjustedClose - monthBeginRec.AdjustedClose;
                decimal delta = diff / monthBeginRec.AdjustedClose;

                monthlyDeltas[monthlyDeltas.Count - 1].Date = currRec.Date;
                monthlyDeltas[monthlyDeltas.Count - 1].Delta = delta;
            }

            return monthlyDeltas;
        }
    }
}
