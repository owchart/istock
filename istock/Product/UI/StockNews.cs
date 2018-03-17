using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace OwLib
{
    /// <summary>
    /// 个股新闻
    /// </summary>
    public class StockNews
    {
        /// <summary>
        /// 创建新闻
        /// </summary>
        public StockNews(MainFrame mainFrame)
        {
            m_mainFrame = mainFrame;
            m_gridNews = mainFrame.GetGrid("gridStockNews");
            m_gridNews.GridLineColor = COLOR.EMPTY;
            m_gridNews.BackColor = COLOR.ARGB(0, 0, 0);
            m_gridNews.RowStyle = new GridRowStyle();
            m_gridNews.RowStyle.BackColor = COLOR.ARGB(0, 0, 0);
            m_gridNews.RegisterEvent(new GridCellMouseEvent(GridCellClick), EVENTID.GRIDCELLCLICK);
        }

        private GridA m_gridNews;

        private String m_code;

        /// <summary>
        /// 获取或设置代码
        /// </summary>
        public String Code
        {
            get { return m_code; }
            set 
            { 
                m_code = value;
                List<SingleInfo> singleInfos = GetSingleInfos(m_code);
                int singleInfosSize = singleInfos.Count;
                m_gridNews.ClearRows();
                for (int i = 0; i < singleInfosSize; i++)
                {
                    SingleInfo singleInfo = singleInfos[i];
                    GridRow row = new GridRow();
                    m_gridNews.AddRow(row);
                    row.Tag = singleInfo;
                    int date = CStr.ConvertStrToInt(singleInfo.Date);
                    int time = CStr.ConvertStrToInt(singleInfo.Time);
                    int year = date / 10000;
                    int month = (date - year * 10000) / 100;
                    int day = date - year * 10000 - month * 100;
                    DateTime dt = new DateTime(year, month, day);
                    if (singleInfo.Time == "0")
                    {
                        row.AddCell("colN1", new GridStringCell(dt.ToString("yyyy-MM-dd")));
                    }
                    else
                    {
                        int hour = time / 10000;
                        int minute = (time - hour * 10000) / 100;
                        int second = time - hour * 10000 - minute * 100;
                        dt = new DateTime(year, month, day, hour, minute, second);
                        row.AddCell("colN1", new GridStringCell(dt.ToString("yyyy-MM-dd hh:MM:ss")));
                    }
                    String strType = "新闻";
                    if (singleInfo.Type == "2")
                    {
                        strType = "公告";
                    }
                    else if (singleInfo.Type == "3")
                    {
                        strType = "研报";
                    }
                    row.AddCell("colN2", new GridStringCell(strType));
                    row.AddCell("colN3", new GridStringCell(singleInfo.Title));
                    row.GetCell("colN1").Style = new GridCellStyle();
                    row.GetCell("colN1").Style.ForeColor = COLOR.ARGB(255, 255, 80);
                    row.GetCell("colN2").Style = new GridCellStyle();
                    row.GetCell("colN2").Style.ForeColor = COLOR.ARGB(80, 255, 255);
                    row.GetCell("colN3").Style = new GridCellStyle();
                    row.GetCell("colN3").Style.ForeColor = COLOR.ARGB(255, 80, 80);
                }
                m_gridNews.Update();
                m_gridNews.Invalidate();
            }
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

        [DllImport("news.dll", EntryPoint = "GetNews", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetNews(String code, StringBuilder str);

        /// <summary>
        /// 获取单个证券的信息
        /// </summary>
        /// <param name="codes"></param>
        /// <returns></returns>
        public static List<SingleInfo> GetSingleInfos(String codes)
        {
            StringBuilder sb = new StringBuilder(102400);
            int len = GetNews(codes, sb);
            String str = sb.ToString();
            String[] strs = str.Split(new String[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            int strsSize = strs.Length;
            List<SingleInfo> infos = new List<SingleInfo>();
            for (int i = 0; i < strsSize; i++)
            {
                String strInfo = strs[i];
                SingleInfo singleInfo = new SingleInfo();
                int index = strInfo.IndexOf(",");
                singleInfo.Date = strInfo.Substring(0, index);
                strInfo = strInfo.Substring(index + 1);
                index = strInfo.IndexOf(",");
                singleInfo.Time = strInfo.Substring(0, index);
                strInfo = strInfo.Substring(index + 1);
                index = strInfo.IndexOf(",");
                singleInfo.Type = strInfo.Substring(0, index);
                strInfo = strInfo.Substring(index + 1);
                index = strInfo.IndexOf(",");
                singleInfo.InfoCode = strInfo.Substring(0, index);
                singleInfo.Title = strInfo.Substring(index + 1);
                infos.Add(singleInfo);
            }
            return infos;
        }

        /// <summary>
        /// 单元格双击事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="cell">单元格</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮值</param>
        private void GridCellClick(object sender, GridCell cell, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            if (clicks == 2)
            {
                SingleInfo singleInfo = cell.Row.Tag as SingleInfo;
                String type = singleInfo.Type;
                String infoCode = singleInfo.InfoCode;
                String url = "";
                if (type == "1")
                {
                    url = "http://app.jg.eastmoney.com/html_News/DetailOnly.html?infoCode=" + infoCode;
                }
                else if (type == "2")
                {
                    url = "http://pdf.dfcfw.com/pdf/H2_" + infoCode + "_1.pdf";
                }
                else if (type == "3")
                {
                    url = "http://pdf.dfcfw.com/pdf/H301_" + infoCode + "_1.pdf";
                }
                Process.Start(url);
                //String text = "";
                //if (type == "1")
                //{
                //    text = StockNewsDataHelper.GetRealTimeInfoByCode(infoCode);
                //}
                //else if (type == "2")
                //{
                //    text = NoticeDataHelper.GetRealTimeInfoByCode(infoCode);
                //}
                //else if (type == "3")
                //{
                //    text = ReportDataHelper.GetRealTimeInfoByCode(infoCode);
                //}
                //m_mainFrame.ShowMessageBox(text, "提示", 0);
            }
        }
    }

    public class SingleInfo
    {
        private String m_date;

        public String Date
        {
            get { return m_date; }
            set { m_date = value; }
        }

        private String m_time;

        public String Time
        {
            get { return m_time; }
            set { m_time = value; }
        }

        private String m_type;

        public String Type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        private String m_infoCode;

        public String InfoCode
        {
            get { return m_infoCode; }
            set { m_infoCode = value; }
        }

        private String m_title;

        public String Title
        {
            get { return m_title; }
            set { m_title = value; }
        }
    }
}
