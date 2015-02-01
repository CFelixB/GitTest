using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace FolioBot
{
    class TrackProFormaInputFile
    {
        public static Int16 MgrMgmtFees_Idx = 0;
        public static Int16 FixedFees_Idx = 1;
        public static Int16 initial_Idx = 2;
        public static Int16 FinalDt_Idx = 4;
        public static Int16 incentive_Idx = 5;
        public static Int16 MinAlloc_Idx = 6;
        public static Int16 MgrIncentiveFees_Idx = 7;
        public static Int16 freq_Idx = 8;

        private string m_fileName;
        public string FileName { get { return m_fileName; } }

        private string[] m_lines;
        public string[] Lines { get { return m_lines; } }

        private List<TrackProFormaInputRecord> m_trackProFormaInputRecords;

        public TrackProFormaInputFile(string fileName)
        {
            m_fileName = fileName;
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
                MessageBox.Show("TrackProFormaInputFile.Load() : Could not open input file " + m_fileName + ".");
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
            TrackProFormaInputRecord rec;

            if (m_trackProFormaInputRecords == null)
            {
                m_trackProFormaInputRecords = new List<TrackProFormaInputRecord>();
            }


            for (int i = 1; i < m_lines.Length; i++)
            {
                line = m_lines[i];

                // skip blank lines
                if (line.Trim().Length == 0)
                    continue;

                fields = line.Split(',');

                // create new record object
                rec = new TrackProFormaInputRecord();
                
                //MgrMgmtFees
                field = fields[MgrMgmtFees_Idx];
                field = field.Replace("\"", "");
                if (field != "NA")
                {
                    rec.MgrMgmtFees = Decimal.Parse(field);
                }
                else
                    rec.MgrMgmtFees = 0;

                //FixedFees
                field = fields[FixedFees_Idx];
                field = field.Replace("\"", "");
                if (field != "NA")
                {
                    rec.FixedFees = Decimal.Parse(field);
                }
                else
                    rec.FixedFees = 0;

                //initial
                field = fields[initial_Idx];
                field = field.Replace("\"", "");
                if (field != "NA")
                {
                    rec.initial = Decimal.Parse(field);
                }
                else
                    rec.initial = 0;

                //FinalDt
                field = fields[FinalDt_Idx];
                field = field.Replace("\"", "");
                rec.FinalDt = DateTime.Parse(field);

                //incentive
                field = fields[incentive_Idx];
                field = field.Replace("\"", "");
                if (field != "NA")
                {
                    rec.incentive = Decimal.Parse(field);
                }
                else
                    rec.incentive = 0;
                
                //MinAlloc
                field = fields[MinAlloc_Idx];
                field = field.Replace("\"", "");
                if (field != "NA")
                {
                    rec.MinAlloc = Decimal.Parse(field);
                }
                else
                    rec.MinAlloc = 0;
                
                //MgrIncentiveFees
                field = fields[MgrIncentiveFees_Idx];
                field = field.Replace("\"", "");
                if (field != "NA")
                {
                    rec.MgrIncentiveFees = Decimal.Parse(field);
                }
                else
                    rec.MgrIncentiveFees = 0;

                //freq
                field = fields[freq_Idx];
                field = field.Replace("\"", "");
                rec.freq = field;

                m_trackProFormaInputRecords.Add(rec);
            }
        }

        public List<TrackProFormaInputRecord> PortfolioBuildingRecords()
        {
            return m_trackProFormaInputRecords;
        }

        public void WriteToFile(List<TrackProFormaInputRecord> recs, string path)
        {
            // example
            //   1            2          3        4        5          6         7                 8
            //   MgrMgmtFees, FixedFees, initial, FinalDt, incentive, MinAlloc, MgrIncentiveFees, freq
            //   0.0004365079, 0.0001984127, 100, 2013-07-31, 0.05, 0.68, 0.2, Yearly

            string fileName = path + @"\R Files\TrackProFormaInput.csv";
            StreamWriter sw = new StreamWriter(fileName, false);
            string header = @"MgrMgmtFees, FixedFees, initial, FinalDt, incentive, MinAlloc, MgrIncentiveFees, freq";
            sw.WriteLine(header);

            // write detail
            string detail = String.Empty;
            for (int i = 0; i < recs.Count; i++)
            {
                detail =
                    //MgrMgmtFees
                    recs[i].MgrMgmtFees.ToString() + "," +
                    //FixedFees
                    recs[i].FixedFees.ToString() + "," +
                    //initial
                    recs[i].initial.ToString() + "," +
                    //FinalDt
                    recs[i].FinalDt.ToString("yyyy-MM-dd") + "," +
                    //incentive
                    recs[i].incentive.ToString() + "," +
                    //MinAlloc
                    recs[i].MinAlloc.ToString() + "," +
                    //MgrIncentiveFees
                    recs[i].MgrIncentiveFees.ToString() + "," +
                    //freq
                    recs[i].freq + ","
                    ;
                sw.WriteLine(detail);
            }
            
            sw.Close();

            // if all went well, 
            m_fileName = fileName;
            m_trackProFormaInputRecords = recs;
        }
    }
}
