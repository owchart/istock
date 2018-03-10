using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using EmQDS.Data;
using EmQComm;
using EmQTCP;
using EmQDataCore;
using Newtonsoft.Json;
using EmSocketClient;

namespace dataquery
{
    /// <summary>
    /// 行情窗体
    /// </summary>
    public partial class QuoteForm : Form
    {
        /// <summary>
        /// 创建窗体
        /// </summary>
        public QuoteForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 日线查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOneDayHis_Click(object sender, EventArgs e)
        {
            String code = txtCode.Text;
            if (SecurityService.KwItems.ContainsKey(code))
            {
                KwItem item = SecurityService.KwItems[code];
                int innerCode = item.Innercode;
                //财富通历史数据
                OneStockHisKLineData oneStockHisKLineData = new OneStockHisKLineData(innerCode,
                    KLineCycle.CycleDay, 0, 0);
                oneStockHisKLineData._reqKLineDataRange = ReqKLineDataRange.All;
                oneStockHisKLineData.Start();
            }      
        }

        /// <summary>
        /// 指数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIndexDetail_Click(object sender, EventArgs e)
        {
            List<int> codes = new List<int>();
            codes.Add(SecurityService.KwItems["000001.SH"].Innercode);
            IndexDetailData indexDetailData = new IndexDetailData(codes);
            indexDetailData.Start();
        }

        /// <summary>
        /// 短线精灵
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShortLineStrategy_Click(object sender, EventArgs e)
        {
            ReqShortLineStrategyDataPacket reqShortLine = new ReqShortLineStrategyDataPacket();
            ConnectManager2.CreateInstance().Request(reqShortLine);
        }

        /// <summary>
        /// 趋势线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTrendLine_Click(object sender, EventArgs e)
        {
            String code = txtCode.Text;
            if (SecurityService.KwItems.ContainsKey(code))
            {
                KwItem item = SecurityService.KwItems[code];
                int innerCode = item.Innercode;
                TrendData trendData = new TrendData();
                trendData.Code = innerCode;
                trendData.Start();
            }
        }

        /// <summary>
        /// N档行情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnN_Click(object sender, EventArgs e)
        {
            String code = txtCode.Text;
            if (SecurityService.KwItems.ContainsKey(code))
            {
                KwItem item = SecurityService.KwItems[code];
                int innerCode = item.Innercode;
                ReqNOrderStockDetailLevel2DataPacket req = new ReqNOrderStockDetailLevel2DataPacket();
                req.Code = innerCode;
                ConnectManager2.CreateInstance().Request(req);
            }
        }

        /// <summary>
        /// 最新数据
        /// </summary>
        private static String dataText = "";

        private static String hisData = "";

        private static FuncTypeRealTime funcType;
        private static FuncTypeRealTime funcType2;

        public static void SetData(FuncTypeRealTime type, String data)
        {
            if (type == FuncTypeRealTime.StockTrend || type == FuncTypeRealTime.HisKLine)
            {
                hisData = data;
                funcType2 = type;
            }
            else
            {
                dataText = data;
                funcType = type;
            }
        }

        /// <summary>
        /// 秒表事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            if (dataText.Length > 0)
            {
                rtbText.Text = dataText;
                if (funcType == FuncTypeRealTime.HisKLine)
                {
                    UpdateDataToGraph(JsonConvert.DeserializeObject <List<OneDayDataRec>> (dataText));
                }
                else if (funcType == FuncTypeRealTime.StockTrend)
                {
                    UpdateDataToGraph(JsonConvert.DeserializeObject<OneMinuteDataRec[]>(dataText));
                }
                dataText = "";
            }
            if (hisData.Length > 0)
            {
                rtbText.Text = hisData;
                if (funcType2 == FuncTypeRealTime.HisKLine)
                {
                    UpdateDataToGraph(JsonConvert.DeserializeObject<List<OneDayDataRec>>(hisData));
                }
                else if (funcType2 == FuncTypeRealTime.StockTrend)
                {
                    UpdateDataToGraph(JsonConvert.DeserializeObject<OneMinuteDataRec[]>(hisData));
                }
                hisData = "";
            }
        }

        /// <summary>
        /// LV2数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLv2Details_Click(object sender, EventArgs e)
        {
            String code = txtCode.Text;
            if (SecurityService.KwItems.ContainsKey(code))
            {
                KwItem item = SecurityService.KwItems[code];
                int innerCode = item.Innercode;
                OneStockDetailData detailDate = new OneStockDetailData();
                detailDate.Code = innerCode;
                detailDate.Start();
            }
        }

        /// <summary>
        /// 百档行情
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnN100_Click(object sender, EventArgs e)
        {
            String code = txtCode.Text;
            if (SecurityService.KwItems.ContainsKey(code))
            {
                KwItem item = SecurityService.KwItems[code];
                int innerCode = item.Innercode;
                ReqAllOrderStockDetailLev2DataPacket req = new ReqAllOrderStockDetailLev2DataPacket();
                req.Code = innerCode;
                ConnectManager2.CreateInstance().Request(req);
            }
        }

        /// <summary>
        /// LV1数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLv1_Click(object sender, EventArgs e)
        {
            String code = txtCode.Text;
            if (SecurityService.KwItems.ContainsKey(code))
            {
                KwItem item = SecurityService.KwItems[code];
                int innerCode = item.Innercode;
                OneStockDetailData detailDate = new OneStockDetailData();
                detailDate.Code = innerCode;
                detailDate.Start();
            }
        }

        private void QuoteForm_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 更新数据到图像
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateDataToGraph(OneMinuteDataRec[] list)
        {
        }

        /// <summary>
        /// 更新数据到图像
        /// </summary>
        /// <param name="obj"></param>
        public void UpdateDataToGraph(List<OneDayDataRec> OneDayDataList)
        {
        }

        int mainPanelID = -1;
        int volumePanelID = -1;
        int kdjPanelID = -1;
        int macdPanelID = -1;

        /// <summary>
        /// 报价列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCustomReport_Click(object sender, EventArgs e)
        {
            ReqCustomReportOrgDataPacket reqCustom = new ReqCustomReportOrgDataPacket();
            reqCustom.FieldFlag = 1;
            reqCustom.FieldIndexList = new List<short>(2);
            reqCustom.FieldIndexList.Add(0x1f42);
            reqCustom.FieldIndexList.Add(0x1f6c);
            reqCustom.CustomCodeList = new List<int>();
            int count = 0;
            foreach (int key in SecurityService.KwItems2.Keys)
            {
                reqCustom.CustomCodeList.Add(key);
                count++;
                if (count > 100)
                {
                    break;
                }
            }
            ConnectManager2.CreateInstance().Request(reqCustom);
        }
    }
}
