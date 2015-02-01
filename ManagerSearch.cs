using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FolioBot
{
    public  enum SortBy
    { 
        Volatility,
        Sortino,
        CTACorrelation,
        VIXCorrelation
    }
    
    public enum SearchCols
    {
        ManagerCode = 0,
        Manager = 1,
        ProgramCode = 2,
        Strategy = 3,
        SubStrategy = 4,
        MarketFocus = 5,
        Volatility = 6,
        Sortino = 7,
        CTACorrelation = 8,
        VIXCorrelation = 9,
        Selected = 10
    }


    public partial class ManagerSearch : Form
    {
        private List<ManagerFilterRecord> m_displayList;
        private List<ManagerFilterRecord> m_originalList;

        public ManagerSearch(List<ManagerFilterRecord> filterRecs)
        {
            InitializeComponent();


            m_originalList = filterRecs;

            // Display list allows items to be reordered
            // without changing the order of the original list.
            // Both lists have references to the same items
            // but in different orders
            // This allows easier matching of the list items
            // to the main form grid items when the search is complete
            // and the main form grid items need to be marked
            m_displayList = new List<ManagerFilterRecord>();
            for (int i = 0; i < m_originalList.Count; i++)
            {
                m_displayList.Add(m_originalList[i]);
            }

            cbSortDirection.SelectedIndex = 0;
            SortManagerList();
            DisplayManagerList();
        }

        private void DisplayManagerList()
        {
            dgvManagerFilter.Rows.Clear();

            for (int i = 0; i < m_displayList.Count; i++)
            {
                dgvManagerFilter.Rows.Add();

                // and add manager info to row
                dgvManagerFilter.Rows[i].Cells[(int)SearchCols.ManagerCode].Value = m_displayList[i].ManagerCode;
                dgvManagerFilter.Rows[i].Cells[(int)SearchCols.Manager].Value = m_displayList[i].Manager;
                dgvManagerFilter.Rows[i].Cells[(int)SearchCols.ProgramCode].Value = m_displayList[i].ProgramCode;
                dgvManagerFilter.Rows[i].Cells[(int)SearchCols.Strategy].Value = m_displayList[i].Strategy;
                dgvManagerFilter.Rows[i].Cells[(int)SearchCols.SubStrategy].Value = m_displayList[i].SubStrategy;
                dgvManagerFilter.Rows[i].Cells[(int)SearchCols.MarketFocus].Value = m_displayList[i].MarketFocus;
                dgvManagerFilter.Rows[i].Cells[(int)SearchCols.Volatility].Value = m_displayList[i].Volatility;
                dgvManagerFilter.Rows[i].Cells[(int)SearchCols.Sortino].Value = m_displayList[i].Sortino;
                dgvManagerFilter.Rows[i].Cells[(int)SearchCols.CTACorrelation].Value = m_displayList[i].CTACorrelation;
                dgvManagerFilter.Rows[i].Cells[(int)SearchCols.VIXCorrelation].Value = m_displayList[i].VIXCorrelation;
                dgvManagerFilter.Rows[i].Cells[(int)SearchCols.Selected].Value = m_displayList[i].Selected;

            }
        }

        private void SortManagerList()
        {
            // objListOrder.Sort((x, y) => x.OrderDate.CompareTo(y.OrderDate));
            if (cbSearchBy.SelectedIndex == 0) // Volatility
            {
                m_displayList.Sort((x, y) => x.Volatility.CompareTo(y.Volatility));
                if (cbSortDirection.SelectedIndex == 1)
                    m_displayList.Reverse();
            }
            if (cbSearchBy.SelectedIndex == 1) // sortino
            {
                m_displayList.Sort((x, y) => x.Sortino.CompareTo(y.Sortino));
                if (cbSortDirection.SelectedIndex == 1)
                    m_displayList.Reverse();
            }
            if (cbSearchBy.SelectedIndex == 2) // CTACorrelation
            {
                m_displayList.Sort((x, y) => x.CTACorrelation.CompareTo(y.CTACorrelation));
                if (cbSortDirection.SelectedIndex == 1)
                    m_displayList.Reverse();
            }
            if (cbSearchBy.SelectedIndex == 3) // VIXCorrelation
            {
                m_displayList.Sort((x, y) => x.VIXCorrelation.CompareTo(y.VIXCorrelation));
                if (cbSortDirection.SelectedIndex == 1)
                    m_displayList.Reverse();
            }

            /*
            if (cbSearchBy.SelectedText == "Volatility")
            {
                if (cbSortDirection.SelectedText == "Ascending")
                    m_displayList = m_displayList.OrderBy(o => o.Volatility).ToList();
                else
                    m_displayList = m_displayList.OrderByDescending(o => o.Volatility).ToList();
            }
            if (cbSearchBy.SelectedText == "Sortino")
            {
                if (cbSortDirection.SelectedText == "Ascending")
                    m_displayList = m_displayList.OrderBy(o => o.Sortino).ToList();
                else
                    m_displayList = m_displayList.OrderByDescending(o => o.Sortino).ToList();
            }
            if (cbSearchBy.SelectedText == "CTACorrelation")
            {
                if (cbSortDirection.SelectedText == "Ascending")
                    m_displayList = m_displayList.OrderBy(o => o.CTACorrelation).ToList();
                else
                    m_displayList = m_displayList.OrderByDescending(o => o.CTACorrelation).ToList();
            }
            if (cbSearchBy.SelectedText == "VIXCorrelation")
            {
                if (cbSortDirection.SelectedText == "Ascending")
                    m_displayList = m_displayList.OrderBy(o => o.VIXCorrelation).ToList();
                else
                    m_displayList = m_displayList.OrderByDescending(o => o.VIXCorrelation).ToList();
            }
            */
            DisplayManagerList();
        }

        private void cbSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortManagerList();
        }

        private void cbSortDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortManagerList();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            // this assumes that the display list and grid
            // are in the same order
            for (int i = 0; i < m_displayList.Count; i++)
            {
                if (m_displayList[i].ManagerCode != dgvManagerFilter.Rows[i].Cells[(int)SearchCols.ManagerCode].Value)
                    MessageBox.Show("Mismatch between list and grid.");
                if ((bool)dgvManagerFilter.Rows[i].Cells[(int)SearchCols.Selected].Value == true)
                {
                    m_displayList[i].Selected = true;
                }
            }
        }

        
    }

}
