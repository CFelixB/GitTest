using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FolioBot
{
    class Benchmark10YearFile
    {
        public static Int16 YieldDate_Idx = 0;
        public static Int16 Yield_Idx = 1;
        public static Int16 unknown_Idx = 2;
        public static Int16 PriceDate_Idx = 3;
        public static Int16 Price_Idx = 4;

        private string m_fileName;
        public string FileName { get { return m_fileName; } }

        private string[] m_lines;
        public string[] Lines { get { return m_lines; } }

        private List<Benchmark10YearFileRecord> m_recs;

        public Benchmark10YearFile(string fileName)
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
                throw new Exception("Benchmark10YearFile.ReadFile() : Could not open input file " + m_fileName + ".", ex);
            }
            string s = sr.ReadToEnd();

            // get file lines
            m_lines = s.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        }


        public List<Benchmark10YearFileRecord> StitchRecords()
        {
            // return list to be used outside of this class
            // ? save m_recs at end of proc
            // ? so we can compare?
            List<Benchmark10YearFileRecord> recs = new List<Benchmark10YearFileRecord>();

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
                Benchmark10YearFileRecord rec = new Benchmark10YearFileRecord();

                field = fields[YieldDate_Idx];
                field = field.Replace("\"", "");
                rec.YieldDate = DateTime.Parse(field);

                field = fields[Yield_Idx];
                field = field.Replace("\"", "");
                rec.Yield = Decimal.Parse(field);

                field = fields[PriceDate_Idx];
                field = field.Replace("\"", "");
                rec.PriceDate = DateTime.Parse(field);

                field = fields[Price_Idx];
                field = field.Replace("\"", "");
                rec.Price = Decimal.Parse(field);

                /*
                field = fields[StDt1_Idx];
                field = field.Replace("\"", "");
                if (field.Trim() != String.Empty)
                    rec.StDt1 = DateTime.Parse(field);
                */

                recs.Add(rec);
            }

            return recs;
        }
    }
}
