using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace dataquery
{
    /// <summary>
    /// 自选股窗体
    /// </summary>
    public partial class OtherForm : Form
    {
        /// <summary>
        /// 创建窗体
        /// </summary>
        public OtherForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 行情序列按钮
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private void btnQuoteSequenc_Click(object sender, EventArgs e)
        {
            DataCenter dc = new DataCenter();
            SecurityService securityService = DataCenter.SecurityService;
            Dictionary<String, KwItem> availableItems = SecurityService.KwItems;
            int availableItemsSize = availableItems.Count;
            int size = availableItemsSize / 50;
            for (int i = 0; i <= size; i++)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(DownLoadSecurityDatas));
                thread.Start(i);
            }
        }

        /// <summary>
        /// 自选股按钮
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private void btnUserSecurity_Click(object sender, EventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(UserSecurityService.StartService));
            thread.Start();
        }

        /// <summary>
        /// 下载证券
        /// </summary>
        /// <param name="args"></param>
        private void DownLoadSecurityDatas(object args)
        {
            int index = (int)args;
            QuoteSequencService.DownAllStockHistory(index);
        }
    }
}
