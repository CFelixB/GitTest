using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FolioBot
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            frmMain mainForm = new frmMain();
            frmMain.MainForm = mainForm;
            Application.Run(mainForm);
        }
    }
}
