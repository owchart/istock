/*****************************************************************************\
*                                                                             *
* MainFrame.cs -  MainFrame functions, types, and definitions.                *
*                                                                             *
*               Version 1.00  ★★★                                          *
*                                                                             *
*               Copyright (c) 2016-2016, iTeam. All rights reserved.      *
*               Created by Lord 2016/12/24.                                   *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using Newtonsoft.Json;

namespace OwLib
{
    /// <summary>
    /// 管理系统
    /// </summary>
    public class MainFrame : UIXmlEx, IDisposable
    {
        /// <summary>
        /// 创建行情系统
        /// </summary>
        public MainFrame()
        {
        }

        /// <summary>
        /// K线控件
        /// </summary>
        private ChartEx m_chartEx;

        /// <summary>
        /// 自选股表格
        /// </summary>
        private GridA m_gridUserSecurities;

        /// <summary>
        /// 秒表ID
        /// </summary>
        private int m_timerID = ControlA.GetNewTimerID();

        private AllStockNews m_allStockNews;

        /// <summary>
        /// 获取或设置所有股票新闻
        /// </summary>
        public AllStockNews AllStockNews
        {
            get { return m_allStockNews; }
            set { m_allStockNews = value; }
        }

        private AllStockNotices m_allStockNotices;

        /// <summary>
        /// 获取或设置所有股票公告
        /// </summary>
        public AllStockNotices AllStockNotices
        {
            get { return m_allStockNotices; }
            set { m_allStockNotices = value; }
        }

        private AllStockReports m_allStockReports;

        /// <summary>
        /// 获取或设置所有股票研报
        /// </summary>
        public AllStockReports AllStockReports
        {
            get { return m_allStockReports; }
            set { m_allStockReports = value; }
        }

        private NewStocks m_newStocks;

        /// <summary>
        /// 获取或设置新股申购
        /// </summary>
        public NewStocks NewStocks
        {
            get { return m_newStocks; }
            set { m_newStocks = value; }
        }

        private OrderTrade m_orderTrade;

        /// <summary>
        /// 获取或设置同花顺交易
        /// </summary>
        public OrderTrade OrderTrade
        {
            get { return m_orderTrade; }
            set { m_orderTrade = value; }
        }

        private SearchDiv m_searchDiv;

        /// <summary>
        /// 获取或设置搜索框
        /// </summary>
        public SearchDiv SearchDiv
        {
            get { return m_searchDiv; }
            set { m_searchDiv = value; }
        }

        private StockNews m_stockNews;

        /// <summary>
        /// 获取或设置个股新闻
        /// </summary>
        public StockNews StockNews
        {
            get { return m_stockNews; }
            set { m_stockNews = value; }
        }

        /// <summary>
        /// 添加自选股
        /// </summary>
        /// <param name="code">代码</param>
        public void AddUserSecurity(UserSecurity userSecurity)
        {
            List<GridRow> rows = m_gridUserSecurities.m_rows;
            int rowsSize = rows.Count;
            for (int i = 0; i < rowsSize; i++)
            {
                GridRow findRow = rows[i];
                if (findRow.GetCell("colP1").GetString() == userSecurity.m_code)
                {
                    findRow.GetCell("colP11").SetDouble(userSecurity.m_up);
                    findRow.GetCell("colP12").SetDouble(userSecurity.m_down);
                    return;
                }
            }
            GridRow row = new GridRow();
            row.AllowEdit = true;
            row.Height = 30;
            m_gridUserSecurities.AddRow(row);
            ButtonA editButton = new ButtonA();
            editButton.RegisterEvent(new ControlMouseEvent(ClickEvent), EVENTID.CLICK);
            editButton.Font = new FONT("微软雅黑", 16, true, false, false);
            editButton.BackColor = COLOR.ARGB(255, 80, 80);
            editButton.ForeColor = COLOR.ARGB(255, 255, 255);
            editButton.Text = "删除";
            editButton.Name = "btnGridRowEdit";
            editButton.Size = new SIZE(100, 30);
            row.EditButton = editButton;
            row.AddCell("colP1", new GridStringCell(userSecurity.m_code));
            row.AddCell("colP2", new GridStringCell());
            GridDoubleCellEx cellP3 = new GridDoubleCellEx();
            cellP3.Digit = 2;
            row.AddCell("colP3", cellP3);
            GridDoubleCellEx cellP4 = new GridDoubleCellEx();
            cellP4.Digit = 2;
            row.AddCell("colP4", cellP4);
            GridDoubleCellEx cellP5 = new GridDoubleCellEx();
            cellP5.Digit = 2;
            cellP5.IsPercent = true;
            row.AddCell("colP5", cellP5);
            GridDoubleCellEx cellP6 = new GridDoubleCellEx();
            cellP6.Digit = 2;
            row.AddCell("colP6", cellP6);
            GridDoubleCellEx cellP7 = new GridDoubleCellEx();
            cellP7.Digit = 2;
            row.AddCell("colP7", cellP7);
            GridDoubleCellEx cellP8 = new GridDoubleCellEx();
            cellP8.Digit = 2;
            row.AddCell("colP8", cellP8);
            row.AddCell("colP9", new GridDoubleCellEx());
            row.AddCell("colP10", new GridDoubleCellEx());
            GridDoubleCellEx cellP11 = new GridDoubleCellEx();
            cellP11.Digit = 2;
            cellP11.SetDouble(userSecurity.m_up);
            row.AddCell("colP11", cellP11);
            GridDoubleCellEx cellP12 = new GridDoubleCellEx();
            cellP12.Digit = 2;
            cellP12.SetDouble(userSecurity.m_down);
            row.AddCell("colP12", cellP12);
            List<GridCell> cells = row.GetCells();
            int cellsSize = cells.Count;
            for (int i = 0; i < cellsSize; i++)
            {
                cells[i].Style = new GridCellStyle();
                cells[i].Style.Font = new FONT("微软雅黑", 14, true, false, false);
                if (i >= 10)
                {
                    cells[i].AllowEdit = true;
                }
            }
            m_gridUserSecurities.Update();
            m_gridUserSecurities.Invalidate();
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮值</param>
        private void ClickEvent(object sender, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            if (button == MouseButtonsA.Left && clicks == 1)
            {
                ControlA control = sender as ControlA;
                String name = control.Name;
                if (name == "btnAdd")
                {
                    String code = FindControl("txtCode").Text;
                    GSecurity security = new GSecurity();
                    if (SecurityService.GetSecurityByCode(code, ref security) > 0)
                    {
                        UserSecurity userSecurity = new UserSecurity();
                        userSecurity.m_code = code;
                        AddUserSecurity(userSecurity);
                        DataCenter.UserSecurityService.Add(userSecurity);
                    }
                }
                else if (name == "btnGridRowEdit")
                {
                    List<GridRow> selectedRows = m_gridUserSecurities.SelectedRows;
                    int selectedRowsSize = selectedRows.Count;
                    if (selectedRowsSize > 0)
                    {
                        DataCenter.UserSecurityService.Delete(selectedRows[0].GetCell("colP1").GetString());
                        m_gridUserSecurities.AnimateRemoveRow(selectedRows[0]);
                        m_gridUserSecurities.OnRowEditEnd();
                    }
                }
                else if (name == "btnMergeHistoryDatas")
                {
                    SecurityService.Start3();
                }
                else if (name == "btnContract")
                {
                    Process.Start("LordALike.exe");
                }
                else if (name == "btnActiveStock" || name == "btnSecondNewStock"
                    || name == "btnUpperLimitStock" || name == "btnDownLimitStock"
                    || name == "btnUnTradeStock" || name == "btnSwingStock"
                    || name == "btnLowPriceStock" || name == "btnAmountsStock")
                {
                    m_gridUserSecurities.ClearRows();
                    List<String> codes = new List<String>();
                    if (name == "btnActiveStock")
                    {
                        SecurityService.GetActiveCodes(codes);
                    }
                    else if (name == "btnSecondNewStock")
                    {
                        SecurityService.GetSecondNewCodes(codes);
                    }
                    else if (name == "btnUpperLimitStock")
                    {
                        SecurityService.GetLimitUp(codes);
                    }
                    else if (name == "btnDownLimitStock")
                    {
                        SecurityService.GetLimitDown(codes);
                    }
                    else if (name == "btnUnTradeStock")
                    {
                        SecurityService.GetNotTradedCodes(codes);
                    }
                    else if (name == "btnSwingStock")
                    {
                        SecurityService.GetCodesBySwing(codes);
                    }
                    else if (name == "btnLowPriceStock")
                    {
                        SecurityService.GetCodesByPrice(codes);
                    }
                    else if (name == "btnAmountsStock")
                    {
                        SecurityService.GetCodesByAmount(codes);
                    }
                    int codesSize = codes.Count;
                    for (int i = 0; i < codesSize; i++)
                    {
                        UserSecurity userSecurity = new UserSecurity();
                        userSecurity.m_code = codes[i];
                        AddUserSecurity(userSecurity);
                    }
                    GetTabControl("tabMain").SelectedIndex = 0;
                    Native.Invalidate();
                }
                else if (name == "btnExport")
                {
                    ExportToTxt("Stocks.txt", m_gridUserSecurities);
                }
                else if (name == "btnStart")
                {
                    LoadData();
                }
                else if (name == "btnSetStrategy")
                {
                    m_orderTrade.ShowStrategySettingWindow();
                }     
            }
        }

        /// <summary>
        /// 销毁资源方法
        /// </summary>
        public override void Dispose()
        {
        }

        /// <summary>
        /// 下载证券
        /// </summary>
        /// <param name="args"></param>
        private void DownloadHistoryDatas(object args)
        {
            int index = (int)args;
            QuoteSequencService.DownAllStockHistory(index);
        }

        /// <summary>
        /// 退出程序
        /// </summary>
        public override void Exit()
        {
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
                if (cell.Column.Name != "colP11" && cell.Column.Name != "colP12")
                {
                    String code = cell.Row.GetCell("colP1").GetString();
                    if (m_chartEx != null)
                    {
                        GSecurity security = new GSecurity();
                        SecurityService.GetSecurityByCode(code, ref security);
                        m_chartEx.SearchSecurity(security);
                        GetTabControl("tabMain").SelectedIndex = 1;
                        m_stockNews.Code = code; 
                    }
                }
            }
        }

        /// <summary>
        /// 单元格编辑结束事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="cell">单元格</param>
        private void GridCellEditEnd(object sender, GridCell cell)
        {
            if (cell != null)
            {
                String colName = cell.Column.Name;
                String cellValue = cell.GetString();
                UserSecurity userSecurity = DataCenter.UserSecurityService.Get(cell.Row.GetCell("colP1").GetString());
                if (userSecurity == null)
                {
                    userSecurity = new UserSecurity();
                    userSecurity.m_code = cell.Row.GetCell("colP1").GetString();
                }
                if (colName == "colP11")
                {
                    userSecurity.m_up = cell.GetDouble();
                    DataCenter.UserSecurityService.Add(userSecurity);
                    TimerEvent(null, m_timerID);
                }
                else if (colName == "colP12")
                {
                    userSecurity.m_down = cell.GetDouble();
                    DataCenter.UserSecurityService.Add(userSecurity);
                    TimerEvent(null, m_timerID);
                }
            }
        }

        /// <summary>
        /// 历史数据回调
        /// </summary>
        public void HistoryDatasCallBack(OneStockKLineDataRec oneStockKLineDataRec)
        {
            List<SecurityData> securityDatas = new List<SecurityData>();
            int size = oneStockKLineDataRec.OneDayDataList.Count;
            for (int i = 0; i < size; i++)
            {
                OneDayDataRec oneDayDataRec = oneStockKLineDataRec.OneDayDataList[i];
                SecurityData securityData = new SecurityData();
                securityData.m_close = oneDayDataRec.Close;
                securityData.m_high = oneDayDataRec.High;
                securityData.m_low = oneDayDataRec.Low;
                securityData.m_open = oneDayDataRec.Open;
                securityData.m_volume = oneDayDataRec.Volume;
                securityData.m_amount = oneDayDataRec.Amount;               
                int year = oneDayDataRec.Date / 10000;
                int month = (oneDayDataRec.Date  - year * 10000) / 100;
                int day = oneDayDataRec.Date - year * 10000 - month * 100;
                int hour = oneDayDataRec.Time / 10000;
                int minute = (oneDayDataRec.Time - hour * 10000) / 100;
                securityData.m_date = CStrA.M129(year, month, day, hour, minute, 0, 0);
                securityDatas.Add(securityData);
            }
            HistoryDataInfo historyDataInfo = new HistoryDataInfo();
            historyDataInfo.m_code = EMSecurityService.GetKwItemByInnerCode(oneStockKLineDataRec.Code).Code;    
            if(oneStockKLineDataRec.Cycle == KLineCycle.CycleMint1)
            {
                historyDataInfo.m_cycle = 1;
            }
            else if(oneStockKLineDataRec.Cycle == KLineCycle.CycleMint5)
            {
                historyDataInfo.m_cycle = 5;
            }
            else if(oneStockKLineDataRec.Cycle == KLineCycle.CycleMint15)
            {
                historyDataInfo.m_cycle = 15;
            }
            else if(oneStockKLineDataRec.Cycle == KLineCycle.CycleMint30)
            {
                historyDataInfo.m_cycle = 30;
            }
            else if(oneStockKLineDataRec.Cycle == KLineCycle.CycleMint60)
            {
                historyDataInfo.m_cycle = 60;
            }
            else if(oneStockKLineDataRec.Cycle == KLineCycle.CycleDay)
            {
                historyDataInfo.m_cycle = 1440;
            }
            else if(oneStockKLineDataRec.Cycle == KLineCycle.CycleWeek)
            {
                historyDataInfo.m_cycle = 10080;
            }
            else if(oneStockKLineDataRec.Cycle == KLineCycle.CycleMonth)
            {
                historyDataInfo.m_cycle = 43200;
            }
            historyDataInfo.m_subscription = 1;
            m_chartEx.BindHistoryData(historyDataInfo, securityDatas);
        }

        /// <summary>
        /// 是否有窗体显示
        /// </summary>
        /// <returns>是否显示</returns>
        public bool IsWindowShowing()
        {
            List<ControlA> controls = Native.GetControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++)
            {
                WindowFrameA frame = controls[i] as WindowFrameA;
                if (frame != null)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// LV2数据回调
        /// </summary>
        /// <param name="fieldInt32"></param>
        /// <param name="fieldSingle"></param>
        /// <param name="fieldInt64"></param>
        /// <param name="fieldDouble"></param>
        public void LV2DatasCallBack(int code, Dictionary<FieldIndex, int> fieldInt32,
                Dictionary<FieldIndex, float> fieldSingle,
                Dictionary<FieldIndex, long> fieldInt64,
                Dictionary<FieldIndex, double> fieldDouble)
        {
            SecurityLatestData latestData = new SecurityLatestData();
            latestData.m_code = EMSecurityService.GetKwItemByInnerCode(code).Code;
            latestData.m_high = fieldSingle[FieldIndex.High];
            latestData.m_low = fieldSingle[FieldIndex.Low];
            latestData.m_open = fieldSingle[FieldIndex.Open];
            latestData.m_close = fieldSingle[FieldIndex.Now];
            latestData.m_lastClose = fieldSingle[FieldIndex.PreClose];
            latestData.m_sellPrice1 = fieldSingle[FieldIndex.SellPrice1];
            latestData.m_sellPrice2 = fieldSingle[FieldIndex.SellPrice2];
            latestData.m_sellPrice3 = fieldSingle[FieldIndex.SellPrice3];
            latestData.m_sellPrice4 = fieldSingle[FieldIndex.SellPrice4];
            latestData.m_sellPrice5 = fieldSingle[FieldIndex.SellPrice5];
            latestData.m_sellPrice6 = fieldSingle[FieldIndex.SellPrice6];
            latestData.m_sellPrice7 = fieldSingle[FieldIndex.SellPrice7];
            latestData.m_sellPrice8 = fieldSingle[FieldIndex.SellPrice8];
            latestData.m_sellPrice9 = fieldSingle[FieldIndex.SellPrice9];
            latestData.m_sellPrice10 = fieldSingle[FieldIndex.SellPrice10];
            latestData.m_sellVolume1 = fieldInt32[FieldIndex.SellVolume1];
            latestData.m_sellVolume2 = fieldInt32[FieldIndex.SellVolume2];
            latestData.m_sellVolume3 = fieldInt32[FieldIndex.SellVolume3];
            latestData.m_sellVolume4 = fieldInt32[FieldIndex.SellVolume4];
            latestData.m_sellVolume5 = fieldInt32[FieldIndex.SellVolume5];
            latestData.m_sellVolume6 = fieldInt32[FieldIndex.SellVolume6];
            latestData.m_sellVolume7 = fieldInt32[FieldIndex.SellVolume7];
            latestData.m_sellVolume8 = fieldInt32[FieldIndex.SellVolume8];
            latestData.m_sellVolume9 = fieldInt32[FieldIndex.SellVolume9];
            latestData.m_sellVolume10 = fieldInt32[FieldIndex.SellVolume10];
            latestData.m_buyPrice1 = fieldSingle[FieldIndex.BuyPrice1];
            latestData.m_buyPrice2 = fieldSingle[FieldIndex.BuyPrice2];
            latestData.m_buyPrice3 = fieldSingle[FieldIndex.BuyPrice3];
            latestData.m_buyPrice4 = fieldSingle[FieldIndex.BuyPrice4];
            latestData.m_buyPrice5 = fieldSingle[FieldIndex.BuyPrice5];
            latestData.m_buyPrice6 = fieldSingle[FieldIndex.BuyPrice6];
            latestData.m_buyPrice7 = fieldSingle[FieldIndex.BuyPrice7];
            latestData.m_buyPrice8 = fieldSingle[FieldIndex.BuyPrice8];
            latestData.m_buyPrice9 = fieldSingle[FieldIndex.BuyPrice9];
            latestData.m_buyPrice10 = fieldSingle[FieldIndex.BuyPrice10];
            latestData.m_buyVolume1 = fieldInt32[FieldIndex.BuyVolume1];
            latestData.m_buyVolume2 = fieldInt32[FieldIndex.BuyVolume2];
            latestData.m_buyVolume3 = fieldInt32[FieldIndex.BuyVolume3];
            latestData.m_buyVolume4 = fieldInt32[FieldIndex.BuyVolume4];
            latestData.m_buyVolume5 = fieldInt32[FieldIndex.BuyVolume5];
            latestData.m_buyVolume6 = fieldInt32[FieldIndex.BuyVolume6];
            latestData.m_buyVolume7 = fieldInt32[FieldIndex.BuyVolume7];
            latestData.m_buyVolume8 = fieldInt32[FieldIndex.BuyVolume8];
            latestData.m_buyVolume9 = fieldInt32[FieldIndex.BuyVolume9];
            latestData.m_buyVolume10 = fieldInt32[FieldIndex.BuyVolume10];
            latestData.m_allBuyVol = fieldInt64[FieldIndex.GreenVolume];
            latestData.m_allSellVol = fieldInt64[FieldIndex.RedVolume];
            int time = fieldInt32[FieldIndex.Time];
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            int hour = time / 10000;
            int minute = (time - hour * 10000) / 100;
            int second = time - hour * 10000 - minute * 100;
            latestData.m_date = CStrA.M129(year, month, day, hour, 0, 0, 0);
            m_chartEx.LatestDiv.LatestData = latestData;
        }

        /// <summary>
        /// 加载XML
        /// </summary>
        /// <param name="xmlPath">XML路径</param>
        public override void Load(String xmlPath)
        {
            LoadFile(xmlPath, null);
            ControlA control = Native.GetControls()[0];
            control.BackColor = COLOR.CONTROL;
            RegisterEvents(control);
            m_gridUserSecurities = GetGrid("gridUserSecurities");
            m_gridUserSecurities.UseAnimation = true;
            m_gridUserSecurities.GridLineColor = COLOR.EMPTY;
            m_gridUserSecurities.BackColor = COLOR.ARGB(0, 0, 0);
            m_gridUserSecurities.RowStyle = new GridRowStyle();
            m_gridUserSecurities.RowStyle.BackColor = COLOR.ARGB(0, 0, 0);
            m_gridUserSecurities.RegisterEvent(new ControlTimerEvent(TimerEvent), EVENTID.TIMER);
            m_gridUserSecurities.StartTimer(m_timerID, 1000);
            m_gridUserSecurities.RegisterEvent(new GridCellMouseEvent(GridCellClick), EVENTID.GRIDCELLCLICK);
            m_gridUserSecurities.RegisterEvent(new GridCellEvent(GridCellEditEnd), EVENTID.GRIDCELLEDITEND);
            m_chartEx = new ChartEx(this);
            DataCenter.MainUI = this;
            //m_tradePlugIn = new TradePlugIn(this);
            m_stockNews = new StockNews(this);
            m_allStockNews = new AllStockNews(this);
            m_allStockNotices = new AllStockNotices(this);
            m_allStockReports = new AllStockReports(this);
            m_newStocks = new NewStocks(this);
            List<UserSecurity> codes = DataCenter.UserSecurityService.m_codes;
            int codesSize = codes.Count;
            if (codesSize > 0)
            {
                for (int i = 0; i < codesSize; i++)
                {
                    AddUserSecurity(codes[i]);
                }
                GSecurity security = new GSecurity();
                SecurityService.GetSecurityByCode(codes[0].m_code, ref security);
                m_chartEx.SearchSecurity(security);
                m_stockNews.Code = codes[0].m_code;
            }
        }

        /// <summary>
        /// 重绘菜单布局
        /// </summary>
        /// <param name="sender">调用对象</param>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        private void PaintLayoutDiv(object sender, CPaint paint, OwLib.RECT clipRect)
        {
            ControlA control = sender as ControlA;
            int width = control.Width, height = control.Height;
            OwLib.RECT drawRect = new OwLib.RECT(0, 0, width, height);
            paint.FillGradientRect(CDraw.PCOLORS_BACKCOLOR, CDraw.PCOLORS_BACKCOLOR2, drawRect, 0, 90);
        }

        /// 注册事件
        /// </summary>
        /// <param name="control">控件</param>
        private void RegisterEvents(ControlA control)
        {
            ControlMouseEvent clickButtonEvent = new ControlMouseEvent(ClickEvent);
            List<ControlA> controls = control.GetControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++)
            {
                ControlA subControl = controls[i];
                ButtonA button = subControl as ButtonA;
                if (button != null)
                {
                    button.RegisterEvent(clickButtonEvent, EVENTID.CLICK);
                }
                RegisterEvents(controls[i]);
            }
        }

        /// <summary>
        /// 显示提示窗口
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="caption">标题</param>
        /// <param name="uType">格式</param>
        /// <returns>结果</returns>
        public int ShowMessageBox(String text, String caption, int uType)
        {
            MessageBox.Show(text, caption);
            return 1;
        }

        /// <summary>
        /// 显示键盘精灵层
        /// </summary>
        /// <param name="key">按键</param>
        public void ShowSearchDiv(char key)
        {
            ControlA focusedControl = Native.FocusedControl;
            if (focusedControl != null)
            {
                String name = focusedControl.Name;
                if (IsWindowShowing() && name != "txtCode")
                {
                    return;
                }
                if (!(focusedControl is TextBoxA) || (m_searchDiv != null && focusedControl == m_searchDiv.SearchTextBox)
                    || name == "txtCode")
                {
                    Keys keyData = (Keys)key;
                    //创建键盘精灵
                    if (m_searchDiv == null)
                    {
                        m_searchDiv = new SearchDiv();
                        m_searchDiv.Popup = true;
                        m_searchDiv.Size = new SIZE(240, 200);
                        m_searchDiv.Visible = false;
                        Native.AddControl(m_searchDiv);
                        m_searchDiv.BringToFront();
                        m_searchDiv.MainFrame = this;
                    }
                    //退出
                    if (keyData == Keys.Escape)
                    {
                        m_searchDiv.Visible = false;
                        m_searchDiv.Invalidate();
                    }
                    //输入
                    else
                    {
                        if (!m_searchDiv.Visible)
                        {
                            char ch = '\0';
                            if ((keyData >= Keys.D0) && (keyData <= Keys.D9))
                            {
                                ch = (char)((0x30 + keyData) - 0x30);
                            }
                            else if ((keyData >= Keys.A) && (keyData <= Keys.Z))
                            {
                                ch = (char)((0x41 + keyData) - 0x41);
                            }
                            else if ((keyData >= Keys.NumPad0) && (keyData <= Keys.NumPad9))
                            {
                                ch = (char)((0x30 + keyData) - 0x60);
                            }
                            if (ch != '\0')
                            {
                                SIZE size = Native.Host.GetSize();
                                POINT location = new POINT(size.cx - m_searchDiv.Width, size.cy - m_searchDiv.Height);
                                if (name == "txtCode")
                                {
                                    POINT fPoint = new POINT(0, 0);
                                    fPoint = focusedControl.PointToNative(fPoint);
                                    location = new POINT(fPoint.x, fPoint.y + focusedControl.Height);
                                    m_searchDiv.Location = location;
                                    m_searchDiv.SearchTextBox.Text = "";
                                    m_searchDiv.FilterSearch();
                                    m_searchDiv.Visible = true;
                                    m_searchDiv.SearchTextBox.Focused = true;
                                    m_searchDiv.Update();
                                    m_searchDiv.Invalidate();
                                }           
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 开始下载历史数据
        /// </summary>
        public void StartDownloadHistoryDatas()
        {
            DataCenter dc = new DataCenter();
            Dictionary<String, KwItem> availableItems = EMSecurityService.KwItems;
            int availableItemsSize = availableItems.Count;
            int size = availableItemsSize / 50;
            for (int i = 0; i <= size; i++)
            {
                Thread thread = new Thread(new ParameterizedThreadStart(DownloadHistoryDatas));
                thread.Start(i);
            }
        }

        /// <summary>
        /// 秒表事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="sender">秒表ID</param>
        private void TimerEvent(object sender, int timerID)
        {
            if (timerID == m_timerID)
            {
                List<GridRow> rows = m_gridUserSecurities.m_rows;
                int rowsSize = rows.Count;
                for (int i = 0; i < rowsSize; i++)
                {
                    GridRow row = rows[i];
                    String code = row.GetCell("colP1").GetString();
                    GSecurity security = new GSecurity();
                    row.GetCell("colP1").Style.ForeColor = COLOR.ARGB(255, 255, 255);
                    SecurityLatestData latestData = new SecurityLatestData();
                    SecurityService.GetSecurityByCode(code, ref security);
                    SecurityService.GetLatestData(code, ref latestData);
                    row.GetCell("colP2").SetString(security.m_name);
                    row.GetCell("colP2").Style.ForeColor = COLOR.ARGB(255, 255, 80);
                    double diff = 0, diffRange = 0;
                    if (latestData.m_lastClose != 0)
                    {
                        diff = latestData.m_close - latestData.m_lastClose;
                        diffRange = 100 * (latestData.m_close - latestData.m_lastClose) / latestData.m_lastClose;
                    }
                    row.GetCell("colP3").SetDouble(latestData.m_close);
                    row.GetCell("colP3").Style.ForeColor = CDraw.GetPriceColor(latestData.m_close, latestData.m_lastClose);
                    row.GetCell("colP4").SetDouble(diff);
                    row.GetCell("colP4").Style.ForeColor = CDraw.GetPriceColor(latestData.m_close, latestData.m_lastClose);
                    row.GetCell("colP5").SetDouble(diffRange);
                    row.GetCell("colP5").Style.ForeColor = CDraw.GetPriceColor(latestData.m_close, latestData.m_lastClose);
                    row.GetCell("colP6").SetDouble(latestData.m_high);
                    row.GetCell("colP6").Style.ForeColor = CDraw.GetPriceColor(latestData.m_high, latestData.m_lastClose);
                    row.GetCell("colP7").SetDouble(latestData.m_low);
                    row.GetCell("colP7").Style.ForeColor = CDraw.GetPriceColor(latestData.m_low, latestData.m_lastClose);
                    row.GetCell("colP8").SetDouble(latestData.m_open);
                    row.GetCell("colP8").Style.ForeColor = CDraw.GetPriceColor(latestData.m_open, latestData.m_lastClose);
                    row.GetCell("colP9").SetDouble(latestData.m_volume);
                    row.GetCell("colP9").Style.ForeColor = COLOR.ARGB(80, 255, 255);
                    row.GetCell("colP10").SetDouble(latestData.m_amount);
                    row.GetCell("colP10").Style.ForeColor = COLOR.ARGB(80, 255, 255);
                    row.GetCell("colP11").Style.ForeColor = COLOR.ARGB(255, 80, 80);
                    row.GetCell("colP12").Style.ForeColor = COLOR.ARGB(80, 255, 80);
                    int isAlarm = 0;
                    if (latestData.m_close > 0)
                    {
                        double up = row.GetCell("colP11").GetDouble(), down = row.GetCell("colP12").GetDouble();
                        if (up != 0)
                        {
                            if (latestData.m_close > up)
                            {
                                Sound.Play("alarm.wav");
                                isAlarm = 1;
                            }
                        }
                        if (down != 0)
                        {
                            if (latestData.m_close < down)
                            {
                                Sound.Play("alarm.wav");
                                isAlarm = 2;
                            }
                        }
                    }
                    List<GridCell> cells = row.GetCells();
                    int cellsSize = cells.Count;
                    for (int j = 0; j < cellsSize; j++)
                    {
                        GridCell cCell = cells[j];
                        if (isAlarm == 2)
                        {
                            cCell.Style.BackColor = COLOR.ARGB(100, 80, 255, 80);
                        }
                        else if (isAlarm == 1)
                        {
                            cCell.Style.BackColor = COLOR.ARGB(100, 255, 80, 80);
                        }
                        else
                        {
                            cCell.Style.BackColor = COLOR.EMPTY;
                        }
                    }
                }
                m_gridUserSecurities.Invalidate();
            }
        }
    }

    public class GridDoubleCellEx : GridDoubleCell
    {
        private int m_digit;

        /// <summary>
        /// 获取或设置保留位数
        /// </summary>
        public int Digit
        {
            get { return m_digit; }
            set { m_digit = value; }
        }

        private bool m_isPercent;

        /// <summary>
        /// 获取或设置是否百分比
        /// </summary>
        public bool IsPercent
        {
            get { return m_isPercent; }
            set { m_isPercent = value; }
        }

        /// <summary>
        /// 获取绘制文字
        /// </summary>
        /// <returns>绘制文字</returns>
        public override string GetPaintText()
        {
            String text = CStr.GetValueByDigit(m_value, m_digit);
            if (m_isPercent)
            {
                text += "%";
            }
            return text;
        }
    }
}
