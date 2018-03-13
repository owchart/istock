using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace OwLib
{
    /// <summary>
    /// 新股申购
    /// </summary>
    public class NewStocks
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mainFrame"></param>
        public NewStocks(MainFrame mainFrame)
        {
            m_mainFrame = mainFrame;
            BindNewStockCalendar();
        }

        private MainFrame m_mainFrame;

        /// <summary>
        /// 获取或设置主框架
        /// </summary>
        public MainFrame MainFrame
        {
            get { return m_mainFrame; }
            set { m_mainFrame = value; }
        }

        /// <summary>
        /// 绑定新股名称表格
        /// </summary>
        private void BindNewStockCalendar()
        {
            GridA gridNewStockCalendar = m_mainFrame.GetGrid("gridNewStockCalendar");
            gridNewStockCalendar.GridLineColor = COLOR.EMPTY;
            gridNewStockCalendar.BackColor = COLOR.ARGB(0, 0, 0);
            gridNewStockCalendar.RowStyle = new GridRowStyle();
            gridNewStockCalendar.RowStyle.BackColor = COLOR.ARGB(0, 0, 0);
            DataSet ds = NewStockDataHelper.GetNewStockCalendar(0);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                GridRow row = new GridRow();
                gridNewStockCalendar.AddRow(row);
                row.AddCell("colN1", new GridStringCell(dr[1].ToString()));
                row.AddCell("colN2", new GridStringCell(dr[2].ToString()));
                row.AddCell("colN3", new GridStringCell(dr[3].ToString()));
                row.AddCell("colN4", new GridStringCell(dr[4].ToString()));
                row.GetCell("colN1").Style = new GridCellStyle();
                row.GetCell("colN1").Style.ForeColor = COLOR.ARGB(255, 80, 80);
                row.GetCell("colN2").Style = new GridCellStyle();
                row.GetCell("colN2").Style.ForeColor = COLOR.ARGB(80, 255, 255);
                row.GetCell("colN3").Style = new GridCellStyle();
                row.GetCell("colN3").Style.ForeColor = COLOR.ARGB(255, 80, 255);
                row.GetCell("colN4").Style = new GridCellStyle();
                row.GetCell("colN4").Style.ForeColor = COLOR.ARGB(255, 255, 80);
            }
        }
    }
}
