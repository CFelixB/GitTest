using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace FolioBot
{
    class PortfolioBuildingFile
    {
        public static Int16 Number_Idx = 0;
        public static Int16 Manager_Idx = 1;
        public static Int16 ManagerCode_Idx = 2;
        public static Int16 ProgramCode_Idx = 3;
        public static Int16 Strategy_Idx = 4;
        public static Int16 SubStrategy_Idx = 5;
        public static Int16 MarketsFocus_Idx = 6;
        public static Int16 Percentage_Idx = 7;
        public static Int16 Analyze_Idx = 8;
        public static Int16 Correlation_Idx = 9;
        public static Int16 TargetVol_Idx = 10;

        private string m_fileName;
        public string FileName { get { return m_fileName; } }

        private string[] m_lines;
        public string[] Lines { get { return m_lines; } }

        private List<PortfolioBuildingRecord> m_recs;

        public PortfolioBuildingFile(string fileName)
        {
            m_fileName = fileName;
            //ReadFile();
            //m_portfolioBuildingRecords = new List<PortfolioBuildingRecord>();
        }

        public void Clear()
        {
            m_lines = new string[0];
        }

        public bool ReadFile()
        {
            // read file
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(m_fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("PortfolioBuildingFile.Load() : Could not open input file " + m_fileName + ".");
                return false;
            }
            string s = sr.ReadToEnd();

            // get file lines
            m_lines = s.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            // create records from lines
            LinesToRecords();

            return true;
        }

        private void LinesToRecords()
        { 
            string line;
            string[] fields = null;
            int CODE_FIRM = 0;
            string field = string.Empty;
            PortfolioBuildingRecord rec;

            if (m_recs == null)
            {
                m_recs = new List<PortfolioBuildingRecord>();
            }


            for (int i = 1; i < m_lines.Length; i++)
            {
                line = m_lines[i];

                // skip blank lines
                if (line.Trim().Length == 0)
                    continue;

                fields = line.Split(',');

                // create new record object
                rec = new PortfolioBuildingRecord();

                field = fields[Number_Idx];
                field = field.Replace("\"", "");
                rec.Number = field;

                field = fields[Manager_Idx];
                field = field.Replace("\"", "");
                rec.ManagerName = field;

                field = fields[ManagerCode_Idx];
                field = field.Replace("\"", "");
                rec.ManagerCode = field;

                field = fields[ProgramCode_Idx];
                field = field.Replace("\"", "");
                rec.ProgramCode = field;

                field = fields[Strategy_Idx];
                field = field.Replace("\"", "");
                rec.Strategy = field;

                field = fields[SubStrategy_Idx];
                field = field.Replace("\"", "");
                rec.SubStrategy = field;

                field = fields[MarketsFocus_Idx];
                field = field.Replace("\"", "");
                rec.MarketsFocus = field;

                field = fields[Percentage_Idx];
                field = field.Replace("\"", "");
                rec.Percentage = field;

                field = fields[Analyze_Idx];
                field = field.Replace("\"", "");
                rec.Analyze = field;

                field = fields[Correlation_Idx];
                field = field.Replace("\"", "");
                rec.Correlation = field;

                field = fields[TargetVol_Idx];
                field = field.Replace("\"", "");
                rec.TargetVol = field;
                
                m_recs.Add(rec);
            }
        }

        public List<PortfolioBuildingRecord> PortfolioBuildingRecords()
        {
            return m_recs;
        }

        public void WriteToFile(List<PortfolioBuildingRecord> recs, string path)
        {
            string fileName = path + @"\PortfolioBuilding.csv";
            StreamWriter sw = new StreamWriter(fileName, false);
            string header = @"NO,MANAGER,MGR CODE,Program Code,Strategy,Sub-Strat,Mkts Focus,ALLOC %,""PRO FORMA BUILDER"",""CORRELATION MATRIX"",""TARGET VOL for EA MGR"",,,,";
            sw.WriteLine(header);

            // write detail
            string detail = String.Empty;
            for (int i = 0; i < recs.Count; i++)
            {
                //if (i > 0)
                //    sw.WriteLine("");
                detail =
                    recs[i].Number + "," +
                    recs[i].ManagerName + "," +
                    recs[i].ManagerCode + "," +
                    recs[i].ProgramCode + "," +
                    recs[i].Strategy + "," +
                    recs[i].SubStrategy + "," +
                    recs[i].MarketsFocus + "," +
                    recs[i].Percentage + "," +
                    "JLS" + "," + // proforma
                    "X" + "," + // correlation
                    ",,,,";
                sw.WriteLine(detail);
            }
            
            sw.Close();

            // if all went well, 
            m_fileName = fileName;
            m_recs = recs;
        }
    }
}
