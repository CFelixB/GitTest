using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;

namespace FolioBot
{
    public partial class frmMain : Form
    {
        public static frmMain MainForm;

        private string m_rootFolder = Properties.Settings.Default.MainFolder; //@"I:\jon\Jon Data Project 20120622";
        private string m_rScriptPath = Properties.Settings.Default.RScriptPath; //@"C:\Program Files\R\R-3.0.1\bin\rscript.exe";

        private List<UIManager> m_managers;

        // files
        private AliasNamesFile m_aliasNamesFile;
        private StatsFile m_dbStatsFile;
        private StatsFile m_citiStatsFile;
        private StatsFile m_jlsStatsFile;
        private StitchStatsFile m_stitchStatsFile;
        private PortfolioBuildingFile m_portfolioBuildingFile;
        private ManagerFile m_managerFile;
        private StitchDatabaseFile m_stitchDatabaseFile;

        public StitchDatabaseFile StitchDatabaseFile { get { return m_stitchDatabaseFile; } }

        // temp files
        private AliasNamesFile tmp_aliasNamesFile;
        private StatsFile tmp_dbStatsFile;
        private StatsFile tmp_citiStatsFile;
        private StatsFile tmp_jlsStatsFile;
        private StitchStatsFile tmp_stitchStatsFile;
        private PortfolioBuildingFile tmp_portfolioBuildingFile;
        private ManagerFile tmp_managerFile;
        private StitchDatabaseFile tmp_stitchDatabaseFile;

        List<DateDelta> m_spMonthlyDeltas;
        List<DateDelta> m_vixMonthlyDeltas;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            tbInputDir.Text = m_rootFolder;
            txtRScriptPath.Text = m_rScriptPath;

            dgvManagersNew.DoubleBuffered(true);

            cbRebalanceFrequency.SelectedIndex = 1; //"Quarterly";
            cbOutputFormat.SelectedIndex = 0; //"PDF";
            cbBenchmark.SelectedIndex = 0; //"S&P 500";
            rbPortfolio.Select();

            SetFullManagerVisible();
            SetAnalyzeVisible();
            SetStatisticsVisible();
            

            LoadInfoFromRootFolder();
        }

        private void LoadInfoFromRootFolder()
        {
            // abandon changes if all is not well

            // Load Managers
            bool ableToLoad = true;
            if (!LoadManagerDataObjects())
                ableToLoad = false;
            if (!ableToLoad)
            {
                MessageBox.Show("Unable to initialize data from folder. Data displayed in UI does not reflect data in folder.");
                return;
            }


            // read existing portfolio building info
            if (!LoadPortfolioBuildingObject())
                ableToLoad = false;
            if (!ableToLoad)
            {
                MessageBox.Show("Unable to initialize data from folder. Data displayed in UI does not reflect data in folder.");
                return;
            }

            // save main
            Properties.Settings.Default.MainFolder = this.tbInputDir.Text.Trim();
            Properties.Settings.Default.Save();

            // managers
            // if all is well, assign temp file objects to globals
            // for display in UI
            m_aliasNamesFile = tmp_aliasNamesFile;
            m_dbStatsFile = tmp_dbStatsFile;
            m_citiStatsFile = tmp_citiStatsFile;
            m_jlsStatsFile = tmp_jlsStatsFile;
            m_stitchStatsFile = tmp_stitchStatsFile;
            m_managerFile = tmp_managerFile;
            m_stitchDatabaseFile = tmp_stitchDatabaseFile;
            m_portfolioBuildingFile = tmp_portfolioBuildingFile;


            //CreateManagerObjects_FromAliasNamesAndStitch();
            //ShowManagerData();
            //ShowPortfolioBuilding();


            CreateManagerObjects_ManagerFile();
            
            ShowManagerData();
            ShowPortfolioBuilding();

        
            // benchmarks
            LoadBenchmarks();

            // manager
            string path = Properties.Settings.Default.MainFolder + @"\Output Files\Stitchdata Brads Format.csv";
        }

        private void LoadBenchmarks()
        {
            // benchmarks
            string path = Properties.Settings.Default.MainFolder + @"\Benchmark\SP-Index.csv";
            BenchmarkIndexFile benchmarkFile = new BenchmarkIndexFile(path);
            m_spMonthlyDeltas = benchmarkFile.MonthlyDeltas();

            /*
            MessageBox.Show(
                spMonthlyDeltas[0].Date.ToShortDateString() + "  :  " + spMonthlyDeltas[0].Delta.ToString() + "\n" +
                spMonthlyDeltas[1].Date.ToShortDateString() + "  :  " + spMonthlyDeltas[1].Delta.ToString() + "\n" +
                spMonthlyDeltas[2].Date.ToShortDateString() + "  :  " + spMonthlyDeltas[2].Delta.ToString()
                );
            */

            path = Properties.Settings.Default.MainFolder + @"\Benchmark\VIX Index.csv";
            benchmarkFile = new BenchmarkIndexFile(path);
            m_vixMonthlyDeltas = benchmarkFile.MonthlyDeltas();
        }

        private void CreateManagerObjects_ManagerFile()
        {
            m_managers = new List<UIManager>();
            UIManager manager = null;
            StatsRecord dbStats = null;
            StatsRecord citiStats = null;
            StatsRecord jlsStats = null;
            StitchDatabaseFileRecord stitchRec = null;
            foreach (ManagerFileRecord managerFileRecord in m_managerFile.GetList())
            {
                dbStats = Utility.FindManagerInStats(managerFileRecord.ManagerName, m_dbStatsFile);
                citiStats = Utility.FindManagerInStats(managerFileRecord.ManagerName, m_citiStatsFile);
                jlsStats = Utility.FindManagerInStats(managerFileRecord.ManagerName, m_jlsStatsFile);
                stitchRec = Utility.FindManagerInStitch(managerFileRecord.ManagerName, m_stitchDatabaseFile);
                manager = new UIManager(managerFileRecord, dbStats, citiStats, jlsStats);
                m_managers.Add(manager);
            }
        }

        private void CreateManagerObjects_FromAliasNamesAndStitch()
        {
            m_managers = new List<UIManager>();
            UIManager manager = null;
            StatsRecord dbStats = null;
            StatsRecord citiStats = null;
            StatsRecord jlsStats = null;
            StitchDatabaseFileRecord stitchRec = null;
            foreach (AliasNamesRecord aliasNamesRecord in m_aliasNamesFile.GetList())
            {
                dbStats = Utility.FindManagerInStats(aliasNamesRecord.ManagerName, m_dbStatsFile);
                citiStats = Utility.FindManagerInStats(aliasNamesRecord.ManagerName, m_citiStatsFile);
                jlsStats = Utility.FindManagerInStats(aliasNamesRecord.ManagerName, m_jlsStatsFile);
                stitchRec = Utility.FindManagerInStitch(aliasNamesRecord.ManagerName, m_stitchDatabaseFile);
                manager = new UIManager(aliasNamesRecord, stitchRec, dbStats, citiStats, jlsStats);
                m_managers.Add(manager);
            }
        }


        private bool LoadManagerDataObjects()
        {
            // check root path
            if (!Directory.Exists(this.tbInputDir.Text.Trim()))
            {
                MessageBox.Show("Main Folder " + this.tbInputDir.Text.Trim() + " does not exist. Please enter a valid folder name.");
                return false;
            }

            // check output path
            if (!Directory.Exists(this.tbInputDir.Text.Trim() + @"\Output Files"))
            {
                MessageBox.Show(@"Output Folder " + this.tbInputDir.Text.Trim() + @"\Output Files" + " does not exist. Please make sure this folder exists.");
                return false;
            }

            try
            {
                string rootPath = this.tbInputDir.Text.Trim();
                string sAliasNamesFilePath = rootPath + @"\Alias Names.csv";

                // temp file objects
                tmp_aliasNamesFile = new AliasNamesFile(sAliasNamesFilePath);

                string sDBStatsFilePath = rootPath + @"\Output Files\DB Stats.csv";
                tmp_dbStatsFile = new StatsFile(sDBStatsFilePath);

                string sCITIStatsFilePath = rootPath + @"\Output Files\CITI Stats.csv";
                tmp_citiStatsFile = new StatsFile(sCITIStatsFilePath);

                string sJLSStatsFilePath = rootPath + @"\Output Files\JLS Stats.csv";
                tmp_jlsStatsFile = new StatsFile(sJLSStatsFilePath);

                string sStitchStatsFilePath = rootPath + @"\Output Files\Stitch Stats.csv";
                tmp_stitchStatsFile = new StitchStatsFile(sStitchStatsFilePath);

                // if manager file doesn't exist, create it
                // then open it
                string sManagerFilePath = rootPath + @"\Manager.csv";
                if (File.Exists(sManagerFilePath) == false)
                {
                    ManagerFile.CreateFile(rootPath);
                }
                tmp_managerFile = new ManagerFile(rootPath);

                string sStitchDatabaseFilePath = rootPath + @"\Stitch Database.csv";
                tmp_stitchDatabaseFile = new StitchDatabaseFile(sStitchDatabaseFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading manager data./n" + ex.Message);
                return false;
            }

            return true;
        }

        private bool LoadPortfolioBuildingObject()
        {
            // check root path
            if (!Directory.Exists(this.tbInputDir.Text.Trim()))
            {
                MessageBox.Show("Main Folder " + this.tbInputDir.Text.Trim() + " does not exist. Please enter a valid folder name.");
                return false;
            }

            // check output path
            if (!Directory.Exists(this.tbInputDir.Text.Trim() + @"\Output Files"))
            {
                MessageBox.Show(@"Output Folder " + this.tbInputDir.Text.Trim() + @"\Output Files" + " does not exist. Please make sure this folder exists.");
                return false;
            }

            try
            {
                tmp_portfolioBuildingFile = new PortfolioBuildingFile(m_rootFolder + @"\PortfolioBuilding.csv");
                tmp_portfolioBuildingFile.ReadFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading PortfolioBuilding.csv");
                return false;
            }

            return true;
        }

        private void ClearManagerFilter()
        {
            for (int i = 0; i < m_managers.Count; i++)
            {
                m_managers[i].DontShow = false;
            }
        }

        private void ShowManagerData()
        {
            dgvManagersNew.Rows.Clear();

            UIManager uiManager = null;
            for (int i = 0; i < m_managers.Count; i++)
            {
                uiManager = m_managers[i];

                // Don't show filtered managers
                if (uiManager.DontShow == true)
                    continue;

                dgvManagersNew.Rows.Add();

                // and add manager info to row
                dgvManagersNew.Rows[i].Cells[(int)ManagerCols.Manager].Value = uiManager.ManagerFileRecord.ManagerName;

                dgvManagersNew.Rows[i].Cells[(int)ManagerCols.InStitch].Value = uiManager.ExistsInStitch;
                dgvManagersNew.Rows[i].Cells[(int)ManagerCols.DBManagerName].Value = uiManager.ManagerFileRecord.DBManagerName;
                dgvManagersNew.Rows[i].Cells[(int)ManagerCols.CITIManagerName].Value = uiManager.ManagerFileRecord.CITIManagerName;
                dgvManagersNew.Rows[i].Cells[(int)ManagerCols.JLSName].Value = uiManager.ManagerFileRecord.JLSName;
                dgvManagersNew.Rows[i].Cells[(int)ManagerCols.Location].Value = uiManager.ManagerFileRecord.Location;
                dgvManagersNew.Rows[i].Cells[(int)ManagerCols.ContactName].Value = uiManager.ManagerFileRecord.ContactName;
                dgvManagersNew.Rows[i].Cells[(int)ManagerCols.ContactPhone].Value = uiManager.ManagerFileRecord.ContactPhone;

                dgvManagersNew.Rows[i].Cells[(int)ManagerCols.ManagerCode].Value = uiManager.ManagerFileRecord.FirmCode;
                dgvManagersNew.Rows[i].Cells[(int)ManagerCols.ProgramCode].Value = uiManager.ManagerFileRecord.ProgramCode.ToString();

                dgvManagersNew.Rows[i].Cells[(int)ManagerCols.Strategy].Value = uiManager.ManagerFileRecord.Strategy;
                dgvManagersNew.Rows[i].Cells[(int)ManagerCols.SubStrategy].Value = uiManager.ManagerFileRecord.SubStrat;
                dgvManagersNew.Rows[i].Cells[(int)ManagerCols.MarketFocus].Value = uiManager.ManagerFileRecord.MktsFocus;

                // stats
                StatsRecord stats = null;
                if (uiManager.JLSStats != null)
                {
                    stats = uiManager.JLSStats;
                    dgvManagersNew.Rows[i].Cells[(int)ManagerCols.JS].Value = true;
                    dgvManagersNew.Rows[i].Cells[(int)ManagerCols.CL].Value = false;
                    dgvManagersNew.Rows[i].Cells[(int)ManagerCols.DB].Value = false;
                }
                else if (uiManager.CITIStats != null) 
                {
                    stats = uiManager.CITIStats;
                    dgvManagersNew.Rows[i].Cells[(int)ManagerCols.JS].Value = false;
                    dgvManagersNew.Rows[i].Cells[(int)ManagerCols.CL].Value = true;
                    dgvManagersNew.Rows[i].Cells[(int)ManagerCols.DB].Value = false;
                }
                else if (uiManager.DBStats != null)
                {
                    stats = uiManager.DBStats;
                    dgvManagersNew.Rows[i].Cells[(int)ManagerCols.JS].Value = false;
                    dgvManagersNew.Rows[i].Cells[(int)ManagerCols.CL].Value = false;
                    dgvManagersNew.Rows[i].Cells[(int)ManagerCols.DB].Value = true;
                }
                //
                if (stats != null)
                {
                    dgvManagersNew.Rows[i].Cells[(int)ManagerCols.Volatility].Value = stats.Volatility;
                    dgvManagersNew.Rows[i].Cells[(int)ManagerCols.Sortino].Value = stats.Sortino;
                    dgvManagersNew.Rows[i].Cells[(int)ManagerCols.CTACorrelation].Value = stats.CTACorrelation;
                    dgvManagersNew.Rows[i].Cells[(int)ManagerCols.VIXCorrelation].Value = stats.VIXCorrelation;
                    dgvManagersNew.Rows[i].Cells[(int)ManagerCols.StartDate].Value = stats.StartDate;
                    dgvManagersNew.Rows[i].Cells[(int)ManagerCols.EndDate].Value = stats.EndDate;
                }
            }

            // manager filter list
            // supports filtering of managers
            // and needs to be updated anytime
            // managers change
            //CreateManagerFilterList();

            //SetManagersGridColors();
        }

        private void SetManagersGridColors()
        {
            for (int i = 1; i < dgvManagersNew.Rows.Count; i = i + 2)
            {
                DataGridViewRow row = dgvManagersNew.Rows[i];
                //MessageBox.Show(row.DefaultCellStyle.BackColor.ToString());
                row.DefaultCellStyle.BackColor = Color.Red; //SystemColors.Control;
            }
        }

        private List<ManagerFilterRecord> CreateManagerFilterList()
        {
            List<ManagerFilterRecord> filterRecs = new List<ManagerFilterRecord>();

            ManagerFilterRecord rec = null;

            for (int j = 0; j < this.dgvManagersNew.Rows.Count; j++)
            {
                // only take managers that are in the stitch database
                if ((dgvManagersNew.Rows[j].Cells[(int)ManagerCols.InStitch].Value == null) ||
                    (bool)dgvManagersNew.Rows[j].Cells[(int)ManagerCols.InStitch].Value == true)
                    continue;

                // make sure we have stats for this manager
                if (dgvManagersNew.Rows[j].Cells[(int)ManagerCols.Volatility].Value == null)
                    continue;

                rec = new ManagerFilterRecord();

                rec.ManagerCode = dgvManagersNew.Rows[j].Cells[(int)ManagerCols.ManagerCode].Value.ToString();
                rec.Manager = dgvManagersNew.Rows[j].Cells[(int)ManagerCols.Manager].Value.ToString();
                rec.ProgramCode = dgvManagersNew.Rows[j].Cells[(int)ManagerCols.ProgramCode].Value.ToString();
                rec.Strategy = dgvManagersNew.Rows[j].Cells[(int)ManagerCols.Strategy].Value.ToString();
                rec.SubStrategy = dgvManagersNew.Rows[j].Cells[(int)ManagerCols.SubStrategy].Value.ToString();
                rec.MarketFocus = dgvManagersNew.Rows[j].Cells[(int)ManagerCols.MarketFocus].Value.ToString();
                rec.Volatility = (decimal)dgvManagersNew.Rows[j].Cells[(int)ManagerCols.Volatility].Value;
                rec.Sortino = (decimal)dgvManagersNew.Rows[j].Cells[(int)ManagerCols.Sortino].Value;
                rec.CTACorrelation = (decimal)dgvManagersNew.Rows[j].Cells[(int)ManagerCols.CTACorrelation].Value;
                rec.VIXCorrelation = (decimal)dgvManagersNew.Rows[j].Cells[(int)ManagerCols.VIXCorrelation].Value;

                filterRecs.Add(rec);
            }
            
            return filterRecs;
        }

        private void ShowPortfolioBuilding()
        { 
            // display portfolio building

            List<PortfolioBuildingRecord> pbrs = m_portfolioBuildingFile.PortfolioBuildingRecords();
            for (int i = 0; i < pbrs.Count(); i++)
            {
                for (int j = 0; j < this.dgvManagersNew.Rows.Count; j++)
                {
                    if (dgvManagersNew.Rows[j].Cells[(int)ManagerCols.Manager].Value == null)
                        continue;
                    if (pbrs[i].ManagerName == dgvManagersNew.Rows[j].Cells[(int)ManagerCols.Manager].Value.ToString())
                    {
                        dgvManagersNew.Rows[j].Cells[(int)ManagerCols.Analyze].Value = true;
                        dgvManagersNew.Rows[j].Cells[(int)ManagerCols.Number].Value = pbrs[i].Number;
                        dgvManagersNew.Rows[j].Cells[(int)ManagerCols.PercentageWeight].Value = pbrs[i].Percentage;
                        dgvManagersNew.EndEdit();
                    }
                }
            }
        }

        private void ReadAliasNames_Test()
        {
            // OLD Do NOT Use
            dgvManagersNew.Rows.Clear();

            string path = this.tbInputDir.Text + @"\Alias Names.csv"; //@"C:\jon\Jon Data Project 20120622\Alias Names.csv";
            string line;
            int lineCount = 0;
            string[] cols;
            StreamReader sr = new StreamReader(path);
            // skip header line
            line = sr.ReadLine();
            line = sr.ReadLine();
            while (line != null)
            {
                if (line.Length < 1)
                    break;

                // add a row to the grid for this manager
                dgvManagersNew.Rows.Add();
                cols = line.Split(',');

                int colManager_Idx = 1;
                int colManagerCode_Idx = 2;
                int colProgramCode_Idx = 3;
                int colStrategy_Idx = 6;
                int colSubStrategy_Idx = 7;
                int colMarketFocus_Idx = 8;

                // and add manager info to row
                dgvManagersNew.Rows[lineCount].Cells[(int)ManagerCols.Manager].Value = cols[0];
                dgvManagersNew.Rows[lineCount].Cells[(int)ManagerCols.ManagerCode].Value = cols[5];
                dgvManagersNew.Rows[lineCount].Cells[(int)ManagerCols.ProgramCode].Value = cols[6];
                // ASSIGN REAL VALUES
                dgvManagersNew.Rows[lineCount].Cells[(int)ManagerCols.Strategy].Value = "";
                dgvManagersNew.Rows[lineCount].Cells[(int)ManagerCols.SubStrategy].Value = "";
                dgvManagersNew.Rows[lineCount].Cells[(int)ManagerCols.MarketFocus].Value = "";
                //dgvManagers.Rows[lineCount].Cells[(int)ManagerCols.AllocationPercent].Value = "";
                // next manager line
                line = sr.ReadLine();
                lineCount++;
            }
        }

        private void fillNewGridTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadAliasNames_Test();
        }

        private void WriteRunItBat()
        {
            string runitFile = "";
            //@"\R Files\TrackProForma Rebal JLS.R"
            if (_managerCount > 1)
                runitFile = @"\R Files\TrackProForma Rebal JLS - MULTI.R";
            else
                runitFile = @"\R Files\TrackProForma Rebal JLS - SINGLE.R";

            // full RScript.exe path
            // full TrackProForma Rebal JLS.R path
            // params
            string path = "\"" + txtRScriptPath.Text.Trim() + "\" " +
                          "\"" + tbInputDir.Text.Trim() + runitFile + "\" " +
                          "\"" + tbInputDir.Text.Trim().Replace("\\", "/") + "\"";

            // write header
            StreamWriter sw = new StreamWriter(this.tbInputDir.Text.Trim() + @"\RunIt.bat", false);
            sw.WriteLine(path);
            sw.Close();
        }

        private int _managerCount = 0;
        private void tsmiRun_Click(object sender, EventArgs e)
        {
            // determine whether run is single or multiple
            for (int i = 0; i < dgvManagersNew.Rows.Count; i++)
            {
                dgvManagersNew.Rows[i].Cells[(int)ManagerCols.Number].Value = null;
                DataGridViewCheckBoxCell CbxCell = dgvManagersNew.Rows[i].Cells[(int)ManagerCols.Analyze] as DataGridViewCheckBoxCell;
                if (CbxCell.Value == null) continue;
                if (CbxCell != null && !DBNull.Value.Equals(CbxCell.Value) && (bool)CbxCell.Value == true)
                {
                    _managerCount++;
                }
            }

            WriteRunItBat();

            // write the input files
            WriteTrackProForma_Input();

            dgvManagersNew.EndEdit();
            WritePortfolioBuilding();

            // run the R process
            string path = tbInputDir.Text.Trim() + @"\RunIt.bat";
            //if (_managerCount > 1)
            //    path = path + @"\RunItMULTI.bat";
            //else
            //    path = path + @"\RunItSINGLE.bat";

            
          //ProcessStartInfo info = new ProcessStartInfo(this.txtRScriptPath.Text);
            ProcessStartInfo info = new ProcessStartInfo(path);

            info.UseShellExecute = false;
            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;
            //info.Arguments = @"""" + tbInputDir.Text + @"\R Files\TrackProForma Rebal JLS.R""";
            Process process = Process.Start(info);
            process.WaitForExit();

            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            int exitCode = process.ExitCode;

            txtROutput.Clear();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ERRORS/WARNINGS");
            sb.AppendLine();
            sb.AppendLine(error);
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("Output");
            sb.AppendLine();
            sb.AppendLine(output);

            txtROutput.Text = sb.ToString();
        }

        private void rbPortfolio_CheckedChanged(object sender, EventArgs e)
        {
            txtFixedFees.Enabled = true;
            txtIncentive.Enabled = true;
            txtMinimumAllocation.Enabled = true;
            cbRebalanceFrequency.Enabled = true;

            //dgvManagersNew.Columns[ManagerCols.].Visible = true; // 3
        }

        private void rbSingleManager_CheckedChanged(object sender, EventArgs e)
        {
            txtFixedFees.Enabled = false;
            txtIncentive.Enabled = false;
            txtMinimumAllocation.Enabled = false;
            cbRebalanceFrequency.Enabled = false;

            //dgvManagersNew.Columns[3].Visible = false;
        }

        private void testToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            WriteTrackProForma_Input();

            dgvManagersNew.EndEdit();
            WritePortfolioBuilding();
        }

        private void WriteTrackProForma_Input()
        {
            // write header
            StreamWriter sw = new StreamWriter(this.tbInputDir.Text.Trim() + @"\R Files\TrackProFormaInput.csv", false);
            string header = 
                //"InputDir, " +
                "MgrMgmtFees, " +
                "FixedFees, " +
                "initial, " +
                "Initialdt, " +
                "FinalDt, " +
                "incentive, " +
                "MinAlloc, " +
                "MgrIncentiveFees, " +
                "freq";
            sw.WriteLine(header);
            // write detail
            string detail =
                //@"""" + m_rootFolder + @"""" + ", " + 
                txtManagerManagementFees.Text + ", " +
                txtFixedFees.Text + ", " +
                txtInitial.Text + ", " +
                dtInitialDate.Value.ToString("yyyy-MM-dd") + ", " +
                dtFinalDate.Value.ToString("yyyy-MM-dd") + ", " +
                txtIncentive.Text + ", " +
                txtMinimumAllocation.Text + ", " +
                txtManagerIncentiveFees.Text + ", " +
                cbRebalanceFrequency.Text;
            sw.WriteLine(detail);
            sw.Close();
        }

        private void WritePortfolioBuilding()
        {
            // first check the percentages
            decimal pctWt = 0.0m;
            decimal totalWt = 0.0m;
            bool ok = true;
            string message = String.Empty;
            string sPctWt;
            //int mgrCount = 0;
            for (int i = 0; i < dgvManagersNew.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell CbxCell = dgvManagersNew.Rows[i].Cells[(int)ManagerCols.Analyze] as DataGridViewCheckBoxCell;
                if (CbxCell.Value == null) continue;
                if (CbxCell != null && !DBNull.Value.Equals(CbxCell.Value) && (bool)CbxCell.Value == true)
                {
                    //mgrCount++;

                    // %WT
                    if (dgvManagersNew.Rows[i].Cells[(int)ManagerCols.PercentageWeight].Value == null)
                    {
                        ok = false;
                        message = "Percentage";
                        break;
                    }
                    sPctWt = dgvManagersNew.Rows[i].Cells[(int)ManagerCols.PercentageWeight].Value.ToString();
                    if (!Decimal.TryParse(sPctWt, out pctWt))
                    {
                        ok = false;
                        message = "Every selected manager must have a Percentage value.";
                        break;
                    }
                    totalWt += pctWt;
                }
            }
            if ((totalWt < 0.7m) | (totalWt > 1.2m))
            {
                ok = false;
                message = "Total percentage weight = " + totalWt.ToString() + ", which is outside the acceptable range of 0.7 to 1.2.\n\nRun anyway?";
            }
            if (ok == false)
            {
                DialogResult rslt = MessageBox.Show(message, "Percentage Weight Out of Range", MessageBoxButtons.YesNo);
                if (rslt == DialogResult.No)
                    return;
            }

            // check other values
            ok = true;
            message = String.Empty;
            for (int i = 0; i < dgvManagersNew.Rows.Count; i++)
            {
                DataGridViewCheckBoxCell CbxCell = dgvManagersNew.Rows[i].Cells[(int)ManagerCols.Analyze] as DataGridViewCheckBoxCell;
                if (CbxCell.Value == null) continue;
                if (CbxCell != null && !DBNull.Value.Equals(CbxCell.Value) && (bool)CbxCell.Value == true)
                {
                    // strategy
                    if ((dgvManagersNew.Rows[i].Cells[(int)ManagerCols.Strategy].Value == null) ||
                        (dgvManagersNew.Rows[i].Cells[(int)ManagerCols.Strategy].Value.ToString() == String.Empty))
                    {
                        ok = false;
                        message = "Strategy";
                        break;
                    }
                    // sub strategy
                    if ((dgvManagersNew.Rows[i].Cells[(int)ManagerCols.SubStrategy].Value == null) ||
                        (dgvManagersNew.Rows[i].Cells[(int)ManagerCols.SubStrategy].Value.ToString() == String.Empty))
                    {
                        ok = false;
                        message = "Sub Strategy";
                        break;
                    }
                    // market focus
                    if ((dgvManagersNew.Rows[i].Cells[(int)ManagerCols.MarketFocus].Value == null) ||
                        (dgvManagersNew.Rows[i].Cells[(int)ManagerCols.MarketFocus].Value.ToString() == String.Empty))
                    {
                        ok = false;
                        message = "Market Focus";
                        break;
                    }
                }
            }
            if (ok == false)
            {
                MessageBox.Show("Every selected manager must have a " + message + " value.", "Missing " + message + " Value");
                return;
            }

            // save UI to PortfolioBuildingFileObject
            int selectedCount = 0;
            List<PortfolioBuildingRecord> pbrs = new List<PortfolioBuildingRecord>();
            for (int i = 0; i < this.dgvManagersNew.Rows.Count; i++)
            {
                dgvManagersNew.Rows[i].Cells[(int)ManagerCols.Number].Value = null;
                DataGridViewCheckBoxCell CbxCell = dgvManagersNew.Rows[i].Cells[(int)ManagerCols.Analyze] as DataGridViewCheckBoxCell;
                if (CbxCell.Value == null) continue;
                if (CbxCell!=null && !DBNull.Value.Equals(CbxCell.Value) && (bool)CbxCell.Value == true)
                {
                    PortfolioBuildingRecord rec = new PortfolioBuildingRecord();
                    selectedCount++;

                    // set number column in grid
                    dgvManagersNew.Rows[i].Cells[(int)ManagerCols.Number].Value = selectedCount.ToString();

                    // set namager record values
                    rec.Number = dgvManagersNew.Rows[i].Cells[(int)ManagerCols.Number].Value.ToString();
                    rec.ManagerName = dgvManagersNew.Rows[i].Cells[(int)ManagerCols.Manager].Value.ToString();
                    rec.ManagerCode = dgvManagersNew.Rows[i].Cells[(int)ManagerCols.ManagerCode].Value.ToString();
                    rec.ProgramCode = dgvManagersNew.Rows[i].Cells[(int)ManagerCols.ProgramCode].Value.ToString();
                    rec.Strategy = dgvManagersNew.Rows[i].Cells[(int)ManagerCols.Strategy].Value.ToString();
                    rec.SubStrategy = dgvManagersNew.Rows[i].Cells[(int)ManagerCols.SubStrategy].Value.ToString();
                    rec.MarketsFocus = dgvManagersNew.Rows[i].Cells[(int)ManagerCols.MarketFocus].Value.ToString();
                    rec.Percentage = dgvManagersNew.Rows[i].Cells[(int)ManagerCols.PercentageWeight].Value.ToString();
                    pbrs.Add(rec);
                }
            }
            // now update the file object and write to file
            m_portfolioBuildingFile.WriteToFile(pbrs, tbInputDir.Text.Trim());
        }

        private void WriteManagerFile_FromInputGrid()
        {
            this.dgvManagersNew.EndEdit();
            // save UI to ManagerFileObject
            int managerCount = 0;
            List<ManagerFileRecord> recs = new List<ManagerFileRecord>();
            for (int i = 0; i < this.dgvManagersNew.Rows.Count -1; i++)
            {
                ManagerFileRecord rec = new ManagerFileRecord();
                managerCount++;

                rec.ManagerName = dgvManagersNew.Rows[i].Cells[(int)ManagerCols.Manager].Value.ToString();
                rec.DBManagerName = dgvManagersNew.Rows[i].Cells[(int)ManagerCols.DBManagerName].Value.ToString();
                rec.CITIManagerName = dgvManagersNew.Rows[i].Cells[(int)ManagerCols.CITIManagerName].Value.ToString();
                rec.JLSName = dgvManagersNew.Rows[i].Cells[(int)ManagerCols.JLSName].Value.ToString();
                rec.Location = dgvManagersNew.Rows[i].Cells[(int)ManagerCols.Location].Value.ToString();
                 rec.ContactName = dgvManagersNew.Rows[i].Cells[(int)ManagerCols.ContactName].Value.ToString();
                rec.ContactPhone = dgvManagersNew.Rows[i].Cells[(int)ManagerCols.ContactPhone].Value.ToString();
                rec.FirmCode = dgvManagersNew.Rows[i].Cells[(int)ManagerCols.ManagerCode].Value.ToString();
                rec.ProgramCode = dgvManagersNew.Rows[i].Cells[(int)ManagerCols.ProgramCode].Value.ToString();
                rec.Strategy = dgvManagersNew.Rows[i].Cells[(int)ManagerCols.Strategy].Value.ToString();
                rec.SubStrat = dgvManagersNew.Rows[i].Cells[(int)ManagerCols.SubStrategy].Value.ToString();
                rec.MktsFocus = dgvManagersNew.Rows[i].Cells[(int)ManagerCols.MarketFocus].Value.ToString();
                recs.Add(rec);
            }
            // now update the file object and write to file
            m_managerFile.WriteToFile(recs, tbInputDir.Text.Trim());

        }

        private void tbInputDir_Validated(object sender, EventArgs e)
        {
            LoadInfoFromRootFolder();
        }

        private void txtRScriptPath_Validated(object sender, EventArgs e)
        {
            Properties.Settings.Default.RScriptPath = txtRScriptPath.Text.Trim();
            Properties.Settings.Default.Save();
        }

        private void SetFullManagerVisible()
        {
            if (chkShowFullManagerInfo.Checked == true)
            {
                dgvManagersNew.Columns[(int)ManagerCols.Location].Visible = true;
                dgvManagersNew.Columns[(int)ManagerCols.ContactName].Visible = true;
                dgvManagersNew.Columns[(int)ManagerCols.ContactPhone].Visible = true;
                dgvManagersNew.Columns[(int)ManagerCols.CITIManagerName].Visible = true;
                dgvManagersNew.Columns[(int)ManagerCols.DBManagerName].Visible = true;
                dgvManagersNew.Columns[(int)ManagerCols.JLSName].Visible = true;
            }
            else
            {
                dgvManagersNew.Columns[(int)ManagerCols.Location].Visible = false;
                dgvManagersNew.Columns[(int)ManagerCols.ContactName].Visible = false;
                dgvManagersNew.Columns[(int)ManagerCols.ContactPhone].Visible = false;
                dgvManagersNew.Columns[(int)ManagerCols.CITIManagerName].Visible = false;
                dgvManagersNew.Columns[(int)ManagerCols.DBManagerName].Visible = false;
                dgvManagersNew.Columns[(int)ManagerCols.JLSName].Visible = false;
            }
        }

        private void SetAnalyzeVisible()
        {
            if (chkShowAnalysisColumns.Checked == true)
            {
                dgvManagersNew.Columns[(int)ManagerCols.Analyze].Visible = true;
                dgvManagersNew.Columns[(int)ManagerCols.Compare].Visible = true;
                dgvManagersNew.Columns[(int)ManagerCols.PercentageWeight].Visible = true;
            }
            else
            {
                dgvManagersNew.Columns[(int)ManagerCols.Analyze].Visible = false;
                dgvManagersNew.Columns[(int)ManagerCols.Compare].Visible = false;
                dgvManagersNew.Columns[(int)ManagerCols.PercentageWeight].Visible = false;
            }
        }

        private void SetStatisticsVisible()
        {
            if (chkShowStatistics.Checked == true)
            {
                dgvManagersNew.Columns[(int)ManagerCols.AccessedHydra].Visible = true;
                dgvManagersNew.Columns[(int)ManagerCols.AccessedDBS].Visible = true;
                dgvManagersNew.Columns[(int)ManagerCols.AccessedCITI].Visible = true;
                dgvManagersNew.Columns[(int)ManagerCols.AccessedMA].Visible = true;
                dgvManagersNew.Columns[(int)ManagerCols.DB].Visible = true;
                dgvManagersNew.Columns[(int)ManagerCols.CL].Visible = true;
                dgvManagersNew.Columns[(int)ManagerCols.JS].Visible = true;
                dgvManagersNew.Columns[(int)ManagerCols.Volatility].Visible = true;
                dgvManagersNew.Columns[(int)ManagerCols.Sortino].Visible = true;
                dgvManagersNew.Columns[(int)ManagerCols.CTACorrelation].Visible = true;
                dgvManagersNew.Columns[(int)ManagerCols.VIXCorrelation].Visible = true;
                dgvManagersNew.Columns[(int)ManagerCols.StartDate].Visible = true;
                dgvManagersNew.Columns[(int)ManagerCols.EndDate].Visible = true;
                dgvManagersNew.Columns[(int)ManagerCols.AutoCorrelation].Visible = true;
            }
            else
            {
                dgvManagersNew.Columns[(int)ManagerCols.AccessedHydra].Visible = false;
                dgvManagersNew.Columns[(int)ManagerCols.AccessedDBS].Visible = false;
                dgvManagersNew.Columns[(int)ManagerCols.AccessedCITI].Visible = false;
                dgvManagersNew.Columns[(int)ManagerCols.AccessedMA].Visible = false;
                dgvManagersNew.Columns[(int)ManagerCols.DB].Visible = false;
                dgvManagersNew.Columns[(int)ManagerCols.CL].Visible = false;
                dgvManagersNew.Columns[(int)ManagerCols.JS].Visible = false;
                dgvManagersNew.Columns[(int)ManagerCols.Volatility].Visible = false;
                dgvManagersNew.Columns[(int)ManagerCols.Sortino].Visible = false;
                dgvManagersNew.Columns[(int)ManagerCols.CTACorrelation].Visible = false;
                dgvManagersNew.Columns[(int)ManagerCols.VIXCorrelation].Visible = false;
                dgvManagersNew.Columns[(int)ManagerCols.StartDate].Visible = false;
                dgvManagersNew.Columns[(int)ManagerCols.EndDate].Visible = false;
                dgvManagersNew.Columns[(int)ManagerCols.AutoCorrelation].Visible = false;
            }
        }

        private void chkShowFullManagerInfo_CheckedChanged(object sender, EventArgs e)
        {
            SetFullManagerVisible();
        }

        private void chkShowAnalysisColumns_CheckedChanged(object sender, EventArgs e)
        {
            SetAnalyzeVisible();
        }

        private void chkShowStatisticsColumns_CheckedChanged(object sender, EventArgs e)
        {
            SetStatisticsVisible();
        }

        private void tsmiSaveManagerInfo_Click(object sender, EventArgs e)
        {
            WriteManagerFile_FromInputGrid();
        }

        private void tsmiLoadManagerFromAliasAndStitch_Click(object sender, EventArgs e)
        {
            CreateManagerObjects_FromAliasNamesAndStitch();
            ShowManagerData();
            ShowPortfolioBuilding();
        }

        private void LoadManagerFromAliasAndStitch()
        {
            CreateManagerObjects_FromAliasNamesAndStitch();
            ShowManagerData();
            ShowPortfolioBuilding();
        }

        private void testToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            LoadBenchmarks();
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<ManagerFilterRecord> recs = CreateManagerFilterList();

            ManagerSearch ms = new ManagerSearch(recs);
            ms.cbSearchBy.SelectedIndex = 0;
            DialogResult rslt = ms.ShowDialog();
            //return;
            if (rslt == DialogResult.OK)
            {
                for (int i = 0; i < recs.Count; i++)
                {
                    if (recs[i].Selected == true)
                    {
                        dgvManagersNew.Rows[i].Cells[(int)ManagerCols.Compare].Value = true;
                        dgvManagersNew.Rows[i].Cells[(int)ManagerCols.Analyze].Value = true;
                    }
                }
            }
        }

        private void dgvManagersNew_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //SetManagersGridColors();
        }

        private void txtRScriptPath_Validated_1(object sender, EventArgs e)
        {
            Properties.Settings.Default.RScriptPath = txtRScriptPath.Text.Trim();
            Properties.Settings.Default.Save();
        }

   
    }


    public enum ManagerCols
    {
        Number = 0,
        Manager = 1,
        InStitch = 2,
        Location = 3,
        ContactName = 4,
        ContactPhone = 5,
        DBManagerName = 6,
        CITIManagerName = 7,
        JLSName = 8,
        ManagerCode = 9,
        ProgramCode = 10,
        Analyze = 11,
        Compare = 12,
        PercentageWeight = 13,
        Strategy = 14,
        SubStrategy = 15,
        MarketFocus = 16,
        MRACFactor = 17,

        AccessedHydra = 18,
        AccessedDBS = 19,
        AccessedCITI = 20,
        AccessedMA = 21,

        DB = 22,
        CL = 23,
        JS = 24,
        Volatility = 25,
        Sortino = 26,
        CTACorrelation = 27,
        VIXCorrelation = 28,
        StartDate = 29,
        EndDate = 30,
        AutoCorrelation = 31,
        Short = 32
    }
}


