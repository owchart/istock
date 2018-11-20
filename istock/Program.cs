using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OwLib;
using System.Runtime.InteropServices;

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
            FormulaProxy.FormulaInit();
            IList<Formula> formulas = FormulaProxy.GetSystemFormulas();
            int id = 0;
            foreach (Formula la in formulas)
            {
                Formula formula = new Formula();
                FormulaProxy.GetDbFormula(id, ref formula);
                String text = Marshal.PtrToStringAnsi(formula.src);
                id++;
            }
            DataCenter.StartService();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm mainFrom = new MainForm();
            mainFrom.LoadXml("MainFrame");
            Application.Run(mainFrom);
        }
    }
}