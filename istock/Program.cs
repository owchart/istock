using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OwLib;

namespace OwLib
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            DataCenter.StartService();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm mainFrom = new MainForm();
            mainFrom.LoadXml("MainFrame");
            Application.Run(mainFrom);
        }
    }
}