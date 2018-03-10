using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using dataquery.Web;

namespace dataquery
{
    /// <summary>
    /// 个股资讯
    /// </summary>
    public partial class SingleInfoForm : Form
    {
        /// <summary>
        /// 创建窗体
        /// </summary>
        public SingleInfoForm()
        {
            InitializeComponent();
        }

        [DllImport("news.dll", EntryPoint = "GetNews", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetNews(String code, StringBuilder str);

        /// <summary>
        /// 单元格双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            String type = dgvData.Rows[e.RowIndex].Cells[2].Value.ToString();
            String infoCode = dgvData.Rows[e.RowIndex].Cells[3].Value.ToString();
            if (rbViewText.Checked)
            {
                String text = "";
                if (type == "1")
                {
                    text = StockNewsDataHelper.GetRealTimeInfoByCode(infoCode);
                }
                else if (type == "2")
                {
                    text = NoticeDataHelper.GetRealTimeInfoByCode(infoCode);
                }
                else if (type == "3")
                {
                    text = ReportDataHelper.GetRealTimeInfoByCode(infoCode);
                }
                MessageBox.Show(text);
            }
            else
            {
                string url = "";
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
                    url = "http://pdf.dfcfw.com/pdf/H301_"+ infoCode +"_1.pdf";
                }
                
                Process.Start(url);
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StockNewsForm_Load(object sender, EventArgs e)
        {

        }

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
            String[] strs = str.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
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
        /// 简单搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, EventArgs e)
        {
            dgvData.DataSource = GetSingleInfos(txtCodes.Text);
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
