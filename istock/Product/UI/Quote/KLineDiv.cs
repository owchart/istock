/*****************************************************************************\
*                                                                             *
* OwChart.cs -  Chart functions, types, and definitions.                      *
*                                                                             *
*               Version 1.00  ★★★                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/12/24.                                   *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Windows.Forms;
using System.Threading;

namespace OwLib
{
    /// <summary>
    /// 行情系统
    /// </summary>
    public class KLineDiv
    {
        #region Lord 2016/12/24
        /// <summary>
        /// 创建行情系统
        /// </summary>
        public KLineDiv(MainFrame mainFrame)
        {
            m_mainFrame = mainFrame;
            InitInterface();
            if (m_rightMenu == null)
            {
                //创建右键菜单
                m_rightMenu = m_mainFrame.GetMenu("rightMenu");
                MenuItemMouseEvent menuItemClickEvent = new MenuItemMouseEvent(MenuItemClick);
                m_rightMenu.RegisterEvent(menuItemClickEvent, EVENTID.MENUITEMCLICK);
                m_rightMenu.Visible = false;
                //画线
                MenuItemA plotItemRoot = m_mainFrame.GetMenuItem("ADDPLOT");
                m_rightMenu.Update();
                Dictionary<String, String> plots = DataCenter.Plots;
                foreach (String plotType in plots.Keys)
                {
                    MenuItemA plotItem = new MenuItemA(plots[plotType]);
                    plotItem.Name = "PLOT_ADDPLOT_" + plotType;
                    plotItemRoot.AddItem(plotItem);
                }
            }
            m_plotRightMenu = m_mainFrame.GetMenu("plotRightMenu");
            MenuItemMouseEvent menuItemClickEvent2 = new MenuItemMouseEvent(MenuItemClick);
            m_plotRightMenu.RegisterEvent(menuItemClickEvent2, EVENTID.MENUITEMCLICK);

            Indicator ma = new Indicator();
            SecurityDataHelper.GetIndicatorByName("BOLL", ma);
            Indicator macd = new Indicator();
            SecurityDataHelper.GetIndicatorByName("KDJ", macd);
            AddMainIndicator(ma.m_name, ma.m_name, ma.m_text, ma.m_parameters, m_candleDiv, true);
            AddMainIndicator(macd.m_name, macd.m_name, macd.m_text, macd.m_parameters, m_divs[2], true);
        }

        /// <summary>
        /// 正要添加的画线工具
        /// </summary>
        private String m_addingPlotType;

        /// <summary>
        /// 成交量线
        /// </summary>
        private BarShape m_bar;

        /// <summary>
        /// 成交量预测线
        /// </summary>
        private BarShape m_barForecast;

        /// <summary>
        /// K线
        /// </summary>
        private CandleShape m_candle;

        /// <summary>
        /// 主div的编号
        /// </summary>
        private CDiv m_candleDiv;

        /// <summary>
        /// K线的横轴间隔
        /// </summary>
        private double m_candleHScalePixel;

        /// <summary>
        /// 当前操作的图层
        /// </summary>
        private CDiv m_currentDiv;

        /// <summary>
        /// 所有的层
        /// </summary>
        private List<CDiv> m_divs = new List<CDiv>();

        /// <summary>
        /// 浮动层
        /// </summary>
        private FloatDiv m_floatDiv;

        /// <summary>
        /// 横轴的步长
        /// </summary>
        private List<double> m_hScaleSteps = new List<double>();

        /// <summary>
        /// 分时线的平均线
        /// </summary>
        private PolylineShape m_minuteAvgLine;

        /// <summary>
        /// 分时线
        /// </summary>
        private PolylineShape m_minuteLine;

        /// <summary>
        /// 画线工具的右键菜单
        /// </summary>
        private MenuA m_plotRightMenu;

        /// <summary>
        /// 昨收
        /// </summary>
        public double m_preClosePrice;

        /// <summary>
        /// 刷新K线的标识
        /// </summary>
        public bool m_refreshKLineFlag;

        /// <summary>
        /// 是否反转纵坐标
        /// </summary>
        private bool m_reverseVScale = false;

        /// <summary>
        /// 右键菜单
        /// </summary>
        private MenuA m_rightMenu;

        /// <summary>
        /// 正在查询的证券代码
        /// </summary>
        private GSecurity m_searchSecurity;

        /// <summary>
        /// 行情数据
        /// </summary>
        private List<SecurityLatestData> m_securityDatas = new List<SecurityLatestData>();

        /// <summary>
        /// 成交量层
        /// </summary>
        private CDiv m_volumeDiv;

        private ChartAEx m_chart;

        /// <summary>
        /// 获取或设置图形控件
        /// </summary>
        public ChartAEx Chart
        {
            get { return m_chart; }
            set { m_chart = value; }
        }

        private int m_cycle = 0;

        /// <summary>
        /// 获取或设置周期
        /// </summary>
        public int Cycle
        {
            get
            {
                if (m_showMinuteLine)
                {
                    return 0;
                }
                else
                {
                    return m_cycle;
                }
            }
            set { m_cycle = value; }
        }

        private int m_digit = 2;

        /// <summary>
        /// 获取或设置价格保留小数位数
        /// </summary>
        public int Digit
        {
            get { return m_digit; }
            set { m_digit = value; }
        }

        private int m_index = -1;

        /// <summary>
        /// 获取当前实际的数据索引
        /// </summary>
        public int Index
        {
            get { return m_index; }
        }

        private List<CIndicator> m_indicators = new List<CIndicator>();

        /// <summary>
        /// 获取或设置所有指标
        /// </summary>
        public List<CIndicator> Indicators
        {
            get { return m_indicators; }
            set { m_indicators = value; }
        }

        private LatestDiv m_latestDiv;

        /// <summary>
        /// 获取或设置实时数据面板
        /// </summary>
        public LatestDiv LatestDiv
        {
            get { return m_latestDiv; }
            set { m_latestDiv = value; }
        }

        /// <summary>
        /// 获取最新数据
        /// </summary>
        public SecurityLatestData LatestData
        {
            get { return m_latestDiv.LatestData; }
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

        private SearchDiv m_searchDiv;

        /// <summary>
        /// 获取或设置键盘精灵
        /// </summary>
        public SearchDiv SearchDiv
        {
            get { return m_searchDiv; }
            set { m_searchDiv = value; }
        }

        private bool m_showMinuteLine = true;

        /// <summary>
        /// 获取或设置是否分时图
        /// </summary>
        public bool ShowMinuteLine
        {
            get { return m_showMinuteLine; }
            set { m_showMinuteLine = value; }
        }

        private int m_subscription = 1;

        /// <summary>
        /// 获取或设置复权方式
        /// </summary>
        public int Subscription
        {
            get { return m_subscription; }
            set { m_subscription = value; }
        }

        /// <summary>
        /// 添加空白层
        /// </summary>
        public void AddBlankDiv()
        {
            //隐藏的x轴
            int divSize = m_divs.Count;
            for (int i = 0; i < divSize; i++)
            {
                m_divs[i].HScale.Visible = false;
                m_divs[i].HScale.Height = 0;
            }
            CDiv div = m_chart.AddDiv();
            div.BackColor = CDraw.PCOLORS_BACKCOLOR4;
            m_divs.Add(div);
            div.HScale.Height = 22;
            div.VGrid.Distance = 40;
            div.LeftVScale.ForeColor = CDraw.PCOLORS_FORECOLOR;
            div.LeftVScale.Font = new FONT("Arial", 14, false, false, false);
            div.RightVScale.ForeColor = CDraw.PCOLORS_FORECOLOR;
            div.RightVScale.Font = new FONT("Arial", 14, false, false, false);
            RefreshData();
        }

        /// <summary>
        /// 设置主图指标
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="title">标题</param>
        /// <param name="script">脚本</param>
        /// <param name="parameters">参数</param>
        /// <param name="div">图层</param>
        /// <param name="update">是否更新</param>
        /// <returns>指标对象</returns>
        public CIndicator AddMainIndicator(String name, String title, String script, String parameters, CDiv div, bool update)
        {
            //计算数据
            CIndicator indicator = SecurityDataHelper.CreateIndicator(m_chart, m_chart.DataSource, script, parameters);
            indicator.Name = name;
            //indicator.FullName = title;
            indicator.AttachVScale = AttachVScale.Left;
            m_indicators.Add(indicator);
            indicator.Div = div;
            indicator.OnCalculate(0);
            if (div != m_candleDiv && div != m_volumeDiv)
            {
                div.TitleBar.Text = title;
            }
            //刷新图像
            if (update)
            {
                m_chart.Update();
                m_chart.Invalidate();
            }
            return indicator;
        }

        /// <summary>
        /// 添加辅图指标
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="script">脚本</param>
        /// <param name="param">参数</param>
        /// <param name="div">图层</param>
        /// <param name="update">是否更新布局</param>
        /// <returns>指标对象</returns>
        public CIndicator AddViceIndicator(String name, String script, String parameters, CDiv div, bool update)
        {
            CIndicator indicator = SecurityDataHelper.CreateIndicator(m_chart, m_chart.DataSource, script, parameters);
            indicator.AttachVScale = AttachVScale.Right;
            m_indicators.Add(indicator);
            indicator.Div = div;
            indicator.Name = name;
            //计算数据
            indicator.OnCalculate(0);
            if (update)
            {
                m_chart.Update();
                m_chart.Invalidate();
            }
            return indicator;
        }

        public void BindHistoryData(HistoryDataInfo dataInfo, List<SecurityData> historyDatas)
        {
            int size = historyDatas.Count;
            CTable dataSource = m_chart.DataSource;
            int[] fields = new int[7];
            fields[0] = KeyFields.CLOSE_INDEX;
            fields[1] = KeyFields.HIGH_INDEX;
            fields[2] = KeyFields.LOW_INDEX;
            fields[3] = KeyFields.OPEN_INDEX;
            fields[4] = KeyFields.VOL_INDEX;
            fields[5] = KeyFields.AMOUNT_INDEX;
            fields[6] = KeyFields.AVGPRICE_INDEX;
            if (dataInfo.m_cycle == m_cycle
               && dataInfo.m_code == m_latestDiv.SecurityCode
               && dataInfo.m_subscription == m_subscription)
            {
                SecurityDataHelper.BindHistoryDatas(m_chart, dataSource, m_indicators, fields, historyDatas, m_showMinuteLine);
                //if (m_cycle == 0)
                //{
                //    int lastIndex = dataSource.GetRowIndex(historyDatas[size - 1].m_date);
                //    SecurityDataHelper.InsertLatestData(m_chart, dataSource, m_indicators, fields, m_preClosePrice, lastIndex);
                //}
                m_hScaleSteps.Clear();
                if (m_showMinuteLine)
                {
                    int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, ms = 0;
                    double date = dataSource.GetXValue(0);
                    CStrA.M130(date, ref year, ref month, ref day, ref hour, ref minute, ref second, ref ms);
                    m_hScaleSteps.Add(CStrA.M129(year, month, day, 10, 0, 0, 0));
                    m_hScaleSteps.Add(CStrA.M129(year, month, day, 10, 30, 0, 0));
                    m_hScaleSteps.Add(CStrA.M129(year, month, day, 11, 0, 0, 0));
                    m_hScaleSteps.Add(CStrA.M129(year, month, day, 11, 30, 0, 0));
                    m_hScaleSteps.Add(CStrA.M129(year, month, day, 13, 30, 0, 0));
                    m_hScaleSteps.Add(CStrA.M129(year, month, day, 14, 0, 0, 0));
                    m_hScaleSteps.Add(CStrA.M129(year, month, day, 14, 30, 0, 0));
                }
                else
                {
                    int rowsSize = dataSource.RowsCount;
                    for (int i = 0; i < rowsSize; i++)
                    {
                        double volume = dataSource.Get2(i, KeyFields.VOL_INDEX);
                        if (!double.IsNaN(volume))
                        {
                            m_index = i;
                        }
                        if (!m_showMinuteLine)
                        {
                            double close = dataSource.Get2(i, KeyFields.CLOSE_INDEX);
                            double open = dataSource.Get2(i, KeyFields.OPEN_INDEX);
                            if (close >= open)
                            {
                                dataSource.Set2(i, m_bar.StyleField, 1);
                                dataSource.Set2(i, m_bar.ColorField, CDraw.PCOLORS_UPCOLOR);
                            }
                            else
                            {
                                dataSource.Set2(i, m_bar.StyleField, 0);
                                dataSource.Set2(i, m_bar.ColorField, CDraw.PCOLORS_DOWNCOLOR2);
                            }
                        }
                    }
                }
                RefreshData();
                m_chart.Update();
                m_chart.Invalidate();
            }
        }

        public void BindHistoryVolAndAmountDatas(HistoryDataInfo dataInfo, List<SecurityData> historyDatas)
        {
            if (dataInfo.m_cycle == m_cycle
               && dataInfo.m_code == m_latestDiv.SecurityCode
               && dataInfo.m_subscription == m_subscription)
            {
                CTable dataSource = m_chart.DataSourceHistory;
                int[] fields = new int[2];
                fields[0] = KeyFields.VOLHISTORY_INDEX;
                fields[1] = KeyFields.AMOUNTHISTORY_INDEX;
                SecurityDataHelper.BindHistoryVolAndAmountDatas(m_chart, dataSource, fields, historyDatas);
            }
        }

        /// <summary>
        /// 修改周期
        /// </summary>
        /// <param name="cycle">周期</param>
        public void ChangeCycle(int cycle)
        {
            int oldCycle = Cycle;
            if (cycle > 0)
            {
                if (oldCycle > 0 && oldCycle != cycle)
                {
                    m_candleHScalePixel = m_chart.HScalePixel;
                }
                Cycle = cycle;
                m_showMinuteLine = false;
            }
            else
            {
                Cycle = cycle;
                m_showMinuteLine = true;
            }
            String securityCode = m_latestDiv.SecurityCode;
            if (securityCode != null && securityCode.Length > 0)
            {
                GSecurity security = new GSecurity();
                SecurityService.GetSecurityByCode(securityCode, ref security);
                SearchSecurity(security);
            }
        }

        /// <summary>
        /// 鼠标点击事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚动值</param>
        private void ChartMouseDown(object sender, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            ControlA control = sender as ControlA;
            mp = control.PointToNative(mp);
            if (m_addingPlotType != null && m_addingPlotType.Length > 0)
            {
                if (button == MouseButtonsA.Left && clicks == 1)
                {
                    //添加画线工具
                    long plotColor = CDraw.PCOLORS_LINECOLOR;
                    CPlot plot = null;
                    //自定义画线
                    plot = m_mainFrame.Native.CreatePlot(m_addingPlotType);
                    m_chart.ShowCrossLine = false;
                    plot.Color = plotColor;
                    plot.SelectedColor = plotColor;
                    CDiv mouseOverDiv = m_chart.GetMouseOverDiv();
                    m_chart.AddPlot(plot, new POINT(mp.x - m_chart.Left - mouseOverDiv.Bounds.left,
                        mp.y - m_chart.Top - mouseOverDiv.Bounds.top - mouseOverDiv.TitleBar.Height), mouseOverDiv);
                    m_chart.Cursor = CursorsA.Arrow;
                    m_chart.Invalidate();
                }
            }
            else
            {
                if (button == MouseButtonsA.Right && clicks == 1)
                {
                    if (m_chart.SelectedPlot != null)
                    {
                        if (m_plotRightMenu != null)
                        {
                            //弹出画线工具的右键菜单
                            m_plotRightMenu.Focused = true;
                            m_plotRightMenu.Visible = true;
                            m_plotRightMenu.Location = mp;
                            m_plotRightMenu.BringToFront();
                            m_mainFrame.Native.Invalidate();
                        }
                    }
                    else
                    {
                        if (m_rightMenu != null)
                        {
                            m_currentDiv = m_chart.GetMouseOverDiv();
                            //弹出右键菜单
                            SIZE nativeSize = m_mainFrame.Native.DisplaySize;
                            int rightMenuHeight = m_rightMenu.Height;
                            if (mp.y + rightMenuHeight > nativeSize.cy)
                            {
                                mp.y = nativeSize.cy - rightMenuHeight;
                            }
                            m_rightMenu.Location = mp;
                            m_rightMenu.Focused = true;
                            m_rightMenu.Update();
                            m_rightMenu.Visible = true;
                            m_rightMenu.BringToFront();
                            m_mainFrame.Native.Invalidate();
                        }
                    }
                    return;
                }
            }
            m_addingPlotType = String.Empty;
        }

        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚动值</param>
        private void ChartMouseMove(object sender, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            if (m_addingPlotType != null && m_addingPlotType.Length > 0)
            {
                m_chart.Cursor = CursorsA.Hand;
                m_chart.Invalidate();
            }
        }

        /// <summary>
        /// 鼠标抬起事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚动值</param>
        private void ChartMouseUp(object sender, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            m_floatDiv.Visible = m_chart.ShowCrossLine;
            m_mainFrame.Native.Invalidate();
        }

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚动值</param>
        private void ClickButton(object sender, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            ButtonA closeButton = sender as ButtonA;
            WindowA window = closeButton.Parent as WindowA;
            window.Close();
            window.Dispose();
        }

        /// <summary>
        /// 删除指标
        /// </summary>
        /// <param name="indicator">指标</param>
        public void DeleteIndicator(CIndicator indicator)
        {
            indicator.Clear();
            m_indicators.Remove(indicator);
            indicator.Dispose();
            m_chart.Update();
            m_mainFrame.Native.Invalidate();
        }

        /// <summary>
        /// 删除所有指标
        /// </summary>
        /// <param name="update">是否更新</param>
        public void DeleteIndicators(bool update)
        {
            int m_indicatorsSize = m_indicators.Count;
            for (int i = 0; i < m_indicatorsSize; i++)
            {
                CIndicator indicator = m_indicators[i];
                indicator.Clear();
                indicator.Dispose();
            }
            m_indicators.Clear();
            if (update)
            {
                m_chart.Update();
                m_mainFrame.Native.Invalidate();
            }
        }

        /// <summary>
        /// 删除选中的指标
        /// </summary>
        public void DeleteSelectedIndicator()
        {
            CIndicator indicator = GetSelectedIndicator();
            if (indicator != null)
            {
                indicator.Clear();
                m_indicators.Remove(indicator);
                indicator.Dispose();
                m_chart.Update();
                m_mainFrame.Native.Invalidate();
            }
        }

        /// <summary>
        /// 删除选中的画线
        /// </summary>
        public void DeleteSelectedPlot()
        {
            CPlot selectedPlot = m_chart.SelectedPlot;
            if (selectedPlot != null)
            {
                selectedPlot.Div.RemovePlot(selectedPlot);
                selectedPlot.Dispose();
                m_chart.Update();
                m_mainFrame.Native.Invalidate();
            }
        }

        /// <summary>
        /// 获取选中的指标
        /// </summary>
        /// <returns>指标</returns>
        private CIndicator GetSelectedIndicator()
        {
            BaseShape shape = m_chart.SelectedShape;
            if (shape != null)
            {
                foreach (CIndicator indicator in m_indicators)
                {
                    List<BaseShape> shapes = indicator.GetShapes();
                    if (shapes.Contains(shape))
                    {
                        return indicator;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 初始化图形界面
        /// </summary>
        private void InitInterface()
        {
            m_chart = m_mainFrame.GetChart("divKLine") as ChartAEx;
            CTable dataSource = m_chart.DataSource;
            m_chart.RegisterEvent(new ControlMouseEvent(ChartMouseDown), EVENTID.MOUSEDOWN);
            m_chart.RegisterEvent(new ControlMouseEvent(ChartMouseMove), EVENTID.MOUSEMOVE);
            m_chart.RegisterEvent(new ControlMouseEvent(ChartMouseUp), EVENTID.MOUSEUP);
            m_chart.BackColor = CDraw.PCOLORS_BACKCOLOR4;
            m_chart.BorderColor = CDraw.PCOLORS_LINECOLOR4;
            //设置可以拖动K线，成交量，线及标记
            m_chart.CanMoveShape = true;
            //设置滚动加速
            m_chart.ScrollAddSpeed = true;
            //设置左右Y轴的宽度
            m_chart.LeftVScaleWidth = 85;
            m_chart.RightVScaleWidth = 85;
            //设置X轴刻度间距
            m_chart.HScalePixel = 3;
            //设置X轴
            m_chart.HScaleFieldText = "日期";
            //添加k线层
            m_candleDiv = m_chart.AddDiv(60);
            m_candleDiv.BackColor = CDraw.PCOLORS_BACKCOLOR4;
            m_candleDiv.TitleBar.Text = "分时线";
            //设置主Div左右Y轴数值带下划线
            m_candleDiv.VGrid.Visible = true;
            m_candleDiv.LeftVScale.NumberStyle = NumberStyle.UnderLine;
            m_candleDiv.LeftVScale.PaddingTop = 2;
            m_candleDiv.VGrid.Distance = 40;
            m_candleDiv.LeftVScale.PaddingBottom = 2;
            m_candleDiv.LeftVScale.Font = new FONT("Arial", 14, false, false, false);
            m_candleDiv.RightVScale.NumberStyle = NumberStyle.UnderLine;
            m_candleDiv.RightVScale.Font = new FONT("Arial", 14, false, false, false);
            m_candleDiv.RightVScale.PaddingTop = 2;
            m_candleDiv.RightVScale.PaddingBottom = 2;
            CTitle priceTitle = new CTitle(KeyFields.CLOSE_INDEX, "", CDraw.PCOLORS_FORECOLOR9, 2, true);
            priceTitle.FieldTextMode = TextMode.Value;
            m_candleDiv.TitleBar.Titles.Add(priceTitle);
            //添加K线图
            m_candle = new CandleShape();
            m_candleDiv.AddShape(m_candle);
            m_candle.CloseField = KeyFields.CLOSE_INDEX;
            m_candle.HighField = KeyFields.HIGH_INDEX;
            m_candle.LowField = KeyFields.LOW_INDEX;
            m_candle.OpenField = KeyFields.OPEN_INDEX;
            m_candle.CloseFieldText = "收盘";
            m_candle.HighFieldText = "最高";
            m_candle.LowFieldText = "最低";
            m_candle.OpenFieldText = "开盘";
            m_candle.Visible = false;
            //分时线
            m_minuteLine = new PolylineShape();
            m_candleDiv.AddShape(m_minuteLine);
            m_minuteLine.Color = COLOR.ARGB(255, 255, 255);
            m_minuteLine.FieldName = KeyFields.CLOSE_INDEX;
            //分时线的平均线
            m_minuteAvgLine = new PolylineShape();
            m_candleDiv.AddShape(m_minuteAvgLine);
            m_minuteAvgLine.Color = COLOR.ARGB(255, 255, 80);
            m_minuteAvgLine.FieldName = KeyFields.AVGPRICE_INDEX;
            //添加成交量层
            m_volumeDiv = m_chart.AddDiv(15);
            m_volumeDiv.BackColor = CDraw.PCOLORS_BACKCOLOR4;
            //设置成交量的单位
            m_volumeDiv.LeftVScale.Digit = 0;
            m_volumeDiv.LeftVScale.Font = new FONT("Arial", 14, false, false, false);
            m_volumeDiv.VGrid.Distance = 30;
            m_volumeDiv.RightVScale.Digit = 0;
            m_volumeDiv.RightVScale.Font = new FONT("Arial", 14, false, false, false);
            //添加成交量
            m_bar = new BarShape();
            m_bar.ColorField = CTable.AutoField;
            m_bar.StyleField = CTable.AutoField;
            m_bar.UpColor = CDraw.PCOLORS_LINECOLOR2;
            m_volumeDiv.AddShape(m_bar);
            m_bar.FieldName = KeyFields.VOL_INDEX;
            //添加成交量预测
            m_barForecast = new BarShape();
            m_barForecast.FieldText = "成交量预测";
            m_barForecast.ZOrder = -1;
            m_volumeDiv.AddShape(m_barForecast);
            m_barForecast.FieldName = CTable.AutoField;
            //设置标题
            m_volumeDiv.TitleBar.Text = "成交量";
            //设置成交量显示名称
            m_bar.FieldText = "成交量";
            //设置成交量标题只显示值
            CTitle barTitle = new CTitle(KeyFields.VOL_INDEX, "成交量", m_bar.DownColor, 0, true);
            barTitle.FieldTextMode = TextMode.Value;
            m_volumeDiv.TitleBar.Titles.Add(barTitle);
            //添加指标层
            CDiv indDiv = m_chart.AddDiv(25);
            indDiv.BackColor = CDraw.PCOLORS_BACKCOLOR4;
            indDiv.VGrid.Distance = 30;
            indDiv.LeftVScale.PaddingTop = 2;
            indDiv.LeftVScale.PaddingBottom = 2;
            indDiv.LeftVScale.Font = new FONT("Arial", 14, false, false, false);
            indDiv.RightVScale.PaddingTop = 2;
            indDiv.RightVScale.PaddingBottom = 2;
            indDiv.RightVScale.Font = new FONT("Arial", 14, false, false, false);
            //设置X轴不可见
            m_candleDiv.HScale.Visible = false;
            m_candleDiv.HScale.Height = 0;
            m_volumeDiv.HScale.Visible = false;
            m_volumeDiv.HScale.Height = 0;
            indDiv.HScale.Visible = true;
            indDiv.HScale.Height = 22;
            //设置坐标轴的颜色
            m_volumeDiv.LeftVScale.ForeColor = CDraw.PCOLORS_FORECOLOR11;
            m_volumeDiv.RightVScale.ForeColor = CDraw.PCOLORS_FORECOLOR11;
            indDiv.LeftVScale.ForeColor = CDraw.PCOLORS_FORECOLOR6;
            indDiv.RightVScale.ForeColor = CDraw.PCOLORS_FORECOLOR6;
            //设置坐标轴的网格线间隔
            //添加到集合
            m_divs.AddRange(new CDiv[] { m_candleDiv, m_volumeDiv, indDiv });
            //添加用户自定义层
            m_floatDiv = m_mainFrame.FindControl("divFloat") as FloatDiv;
            m_floatDiv.Chart = this;
            //当前数据层
            m_latestDiv = m_mainFrame.FindControl("divLatest") as LatestDiv;
            m_latestDiv.Chart = this;
            dataSource.AddColumn(KeyFields.CLOSE_INDEX);
            dataSource.AddColumn(KeyFields.HIGH_INDEX);
            dataSource.AddColumn(KeyFields.LOW_INDEX);
            dataSource.AddColumn(KeyFields.OPEN_INDEX);
            dataSource.AddColumn(KeyFields.VOL_INDEX);
            dataSource.AddColumn(KeyFields.AMOUNT_INDEX);
            dataSource.AddColumn(KeyFields.AVGPRICE_INDEX);
            dataSource.AddColumn(m_bar.ColorField);
            dataSource.AddColumn(m_bar.StyleField);
            dataSource.AddColumn(m_barForecast.FieldName);
            dataSource.SetColsCapacity(16);
            dataSource.SetColsGrowStep(4);
        }

        /// <summary>
        /// 点击
        /// </summary>
        /// <param name="sender">控件</param>
        /// <param name="item">菜单项</param>
        public void MenuItemClick(object sender, MenuItemA item, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            String name = item.Name;
            if (name != null && name.Length > 0)
            {
                bool setChecked = false;
                //主图类型
                if (name.StartsWith("CANDLE_"))
                {
                    if (!m_showMinuteLine)
                    {
                        String candleStye = name.Substring(7);
                        switch (candleStye)
                        {
                            case "STANDARD":
                                m_candle.Style = CandleStyle.Rect;
                                break;
                            case "TOWER":
                                m_candle.Style = CandleStyle.Tower;
                                break;
                            case "AMERICAN":
                                m_candle.Style = CandleStyle.American;
                                break;
                            case "CLOSE":
                                m_candle.Style = CandleStyle.CloseLine;
                                break;
                        }
                        setChecked = true;
                    }
                }
                //坐标轴类型
                else if (name.StartsWith("SCALE_"))
                {
                    String scaleStyle = name.Substring(6);
                    switch (scaleStyle)
                    {
                        case "STANDARD":
                            m_candleDiv.LeftVScale.System = VScaleSystem.Standard;
                            break;
                        case "LOG":
                            m_candleDiv.LeftVScale.System = VScaleSystem.Logarithmic;
                            break;
                        case "DIFF":
                            m_candleDiv.LeftVScale.Type = VScaleType.EqualDiff;
                            break;
                        case "EQUALRATIO":
                            m_candleDiv.LeftVScale.Type = VScaleType.EqualRatio;
                            break;
                        case "DIVIDE":
                            m_candleDiv.LeftVScale.Type = VScaleType.Divide;
                            break;
                        case "PERCENT":
                            m_candleDiv.LeftVScale.Type = VScaleType.Percent;
                            break;
                        case "GOLDENRATIO":
                            m_candleDiv.LeftVScale.Type = VScaleType.GoldenRatio;
                            break;
                        case "REVERSEH":
                            m_chart.ReverseHScale = !m_chart.ReverseHScale;
                            break;
                        case "REVERSEV":
                            m_reverseVScale = !m_reverseVScale;
                            List<CDiv> divs = m_chart.GetDivs();
                            int divsSize = divs.Count;
                            for (int i = 0; i < divsSize; i++)
                            {
                                CDiv div = divs[i];
                                if (div != m_volumeDiv)
                                {
                                    div.LeftVScale.Reverse = m_reverseVScale;
                                    div.RightVScale.Reverse = m_reverseVScale;
                                }
                            }
                            break;
                    }
                }
                //切换周期
                else if (name.StartsWith("CYCLE_"))
                {
                    String type = name.Substring(6);
                    int cycle = 0;
                    switch (type)
                    {
                        case "MINUTELINE":
                            m_showMinuteLine = true;
                            cycle = 0;
                            break;
                        case "1MINUTE":
                            cycle = SecurityDataHelper.GetRealPeriodCount(SecurityDataHelper.CYCLE_MINUTE_1);
                            break;
                        case "5MINUTE":
                            cycle = SecurityDataHelper.GetRealPeriodCount(SecurityDataHelper.CYCLE_MINUTE_5);
                            break;
                        case "15MINUTE":
                            cycle = SecurityDataHelper.GetRealPeriodCount(SecurityDataHelper.CYCLE_MINUTE_15);
                            break;
                        case "30MINUTE":
                            cycle = SecurityDataHelper.GetRealPeriodCount(SecurityDataHelper.CYCLE_MINUTE_30);
                            break;
                        case "60MINUTE":
                            cycle = SecurityDataHelper.GetRealPeriodCount(SecurityDataHelper.CYCLE_MINUTE_60);
                            break;
                        case "DAY":
                            cycle = SecurityDataHelper.GetRealPeriodCount(SecurityDataHelper.CYCLE_DAY);
                            break;
                        case "WEEK":
                            cycle = SecurityDataHelper.GetRealPeriodCount(SecurityDataHelper.CYCLE_WEEK);
                            break;
                        case "MONTH":
                            cycle = SecurityDataHelper.GetRealPeriodCount(SecurityDataHelper.CYCLE_MONTH);
                            break;
                    }
                    ChangeCycle(cycle);
                    setChecked = true;
                }
                //复权方式
                else if (name.StartsWith("SUBSCRIPTION_"))
                {
                    if (!m_showMinuteLine)
                    {
                        String type = name.Substring(13);
                        switch (type)
                        {
                            case "NONE":
                                m_subscription = 0;
                                break;
                            case "FRONT":
                                m_subscription = 1;
                                break;
                            case "BACK":
                                m_subscription = 2;
                                break;
                        }
                        String securityCode = m_latestDiv.SecurityCode;
                        if (securityCode != null && securityCode.Length > 0)
                        {
                            GSecurity security = new GSecurity();
                            SecurityService.GetSecurityByCode(securityCode, ref security);
                            SearchSecurity(security);
                        }
                        setChecked = true;
                    }
                }
                //画线工具
                else if (name.StartsWith("PLOT_"))
                {
                    String type = name.Substring(5);
                    if (type.StartsWith("ADDPLOT_"))
                    {
                        m_addingPlotType = type.Substring(8);
                    }
                    else if (type == "DELETEPLOT")
                    {
                        DeleteSelectedPlot();
                    }
                }
                if (setChecked)
                {
                    List<MenuItemA> items = item.ParentItem.GetItems();
                    int itemsSize = items.Count;
                    for (int i = 0; i < itemsSize; i++)
                    {
                        items[i].Checked = items[i] == item;
                    }
                }
                m_mainFrame.Native.Update();
                m_mainFrame.Native.Invalidate();
            }
        }


        /// <summary>
        /// 移除新添加的空白层
        /// </summary>
        /// <param name="update">是否更新布局</param>
        public void RemoveBlankDivs(bool update)
        {
            List<CDiv> removeDivs = new List<CDiv>();
            //获取要移除的层
            foreach (CDiv div in m_chart.GetDivs())
            {
                if (div != m_candleDiv && div != m_volumeDiv)
                {
                    if (div.GetShapes(SortType.NONE).Count == 0)
                    {
                        removeDivs.Add(div);
                        m_divs.Remove(div);
                    }
                }
            }
            //移除层
            int removeDivSize = removeDivs.Count;
            for (int i = 0; i < removeDivSize; i++)
            {
                m_chart.RemoveDiv(removeDivs[i]);
            }
            //重新设置X轴
            List<CDiv> divsCopy = m_chart.GetDivs();
            int divSize = divsCopy.Count;
            for (int i = 0; i < divSize; i++)
            {
                if (i == divSize - 1)
                {
                    divsCopy[i].HScale.Visible = true;
                    divsCopy[i].HScale.Height = 22;
                }
                else
                {
                    divsCopy[i].HScale.Visible = false;
                    divsCopy[i].HScale.Height = 0;
                }
            }
            if (update)
            {
                m_chart.Update();
                m_mainFrame.Native.Invalidate();
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        public void RefreshData()
        {
            if (m_showMinuteLine)
            {
                m_candleDiv.LeftVScale.ForeColor2 = CDraw.PCOLORS_DOWNCOLOR;
                m_candleDiv.RightVScale.ForeColor2 = CDraw.PCOLORS_DOWNCOLOR;
                m_candleDiv.RightVScale.Type = VScaleType.Percent;
                m_candle.DownColor = CDraw.PCOLORS_LINECOLOR;
                m_candle.Style = CandleStyle.CloseLine;
                m_candle.TagColor = COLOR.EMPTY;
                m_candle.UpColor = CDraw.PCOLORS_LINECOLOR;
                m_bar.Style = BarStyle.Line;
                m_minuteLine.Visible = true;
                m_minuteAvgLine.Visible = true;
                m_candle.Visible = false;
                m_volumeDiv.LeftVScale.Magnitude = 1;
                m_volumeDiv.RightVScale.Magnitude = 1;
            }
            else
            {
                m_candleDiv.LeftVScale.ForeColor2 = COLOR.EMPTY;
                m_candleDiv.RightVScale.ForeColor2 = CDraw.PCOLORS_DOWNCOLOR;
                m_candleDiv.RightVScale.Type = VScaleType.Percent;
                m_candle.DownColor = CDraw.PCOLORS_DOWNCOLOR2;
                m_candle.Style = CandleStyle.Rect;
                m_candle.TagColor = COLOR.ARGB(255, 255, 255);
                m_candle.UpColor = CDraw.PCOLORS_UPCOLOR;
                m_bar.Style = BarStyle.Rect;
                m_minuteLine.Visible = false;
                m_candle.Visible = true;
                m_minuteAvgLine.Visible = false;
                if (m_cycle >= 1 && m_cycle < 30)
                {
                    m_volumeDiv.LeftVScale.Magnitude = 1;
                    m_volumeDiv.RightVScale.Magnitude = 1;
                }
                else
                {
                    m_volumeDiv.LeftVScale.Magnitude = 1000;
                    m_volumeDiv.RightVScale.Magnitude = 1000;
                }
            }
            int indicatorSize = m_indicators.Count;
            for (int i = 0; i < indicatorSize; i++)
            {
                CIndicator indicator = m_indicators[i];
                CDiv div = indicator.Div;
                if (div == m_candleDiv)
                {
                    //隐藏显示线条
                    List<BaseShape> shapes = indicator.GetShapes();
                    int shapesSize = shapes.Count;
                    for (int j = 0; j < shapesSize; j++)
                    {
                        BaseShape shape = shapes[j];
                        shape.Visible = !m_showMinuteLine;
                    }
                    //隐藏显示标题
                    List<CTitle> titles = div.TitleBar.Titles;
                    int titlesSize = titles.Count;
                    for (int j = 0; j < titlesSize; j++)
                    {
                        CTitle title = titles[j];
                        if (title.FieldName == KeyFields.CLOSE_INDEX)
                        {
                            title.Visible = m_showMinuteLine;
                        }
                        else
                        {
                            title.Visible = !m_showMinuteLine;
                        }
                    }
                }
            }
            m_latestDiv.Digit = m_digit;
            SecurityLatestData latestData = m_latestDiv.LatestData;
            CTable dataSource = m_chart.DataSource;
            foreach (CDiv div in m_chart.GetDivs())
            {

                if (div == m_candleDiv)
                {
                    double lastClose = 0;
                    if (latestData != null && latestData.m_code != null && latestData.m_code.Length > 0)
                    {
                        lastClose = latestData.m_lastClose;
                    }
                    else
                    {
                        int rowsSize = dataSource.RowsCount;
                        if (rowsSize > 0)
                        {
                            if (m_showMinuteLine)
                            {
                                lastClose = dataSource.Get2(0, KeyFields.CLOSE_INDEX);
                            }
                            else
                            {
                                if (rowsSize == 1)
                                {
                                    lastClose = dataSource.Get2(0, KeyFields.OPEN_INDEX);
                                }
                                else
                                {
                                    lastClose = dataSource.Get2(rowsSize - 2, KeyFields.CLOSE_INDEX);
                                }
                            }
                        }
                    }
                    if (m_showMinuteLine)
                    {
                        div.LeftVScale.MidValue = lastClose;
                    }
                    else
                    {
                        div.LeftVScale.MidValue = 0;
                    }
                    div.RightVScale.MidValue = lastClose;
                }
                if (div != m_volumeDiv)
                {
                    div.LeftVScale.Digit = m_digit;
                    div.RightVScale.Digit = m_digit;
                }
                div.HScale.SetScaleSteps(m_hScaleSteps);
                div.VGrid.Visible = m_showMinuteLine;
            }
        }

        public void RefreshKLineData(List<SecurityLatestData> datas)
        {
            if (m_cycle != SecurityDataHelper.CYCLE_DAY)
            {
                return;
            }
            int dataSize = datas.Count;
            if (dataSize == 0)
            {
                return;
            }
            if (!m_refreshKLineFlag)
            {
                for (int i = 0; i < dataSize; i++)
                {
                    SecurityLatestData newData = new SecurityLatestData();
                    newData.Copy(datas[i]);
                    if (newData.m_close > 0)
                    {
                        m_securityDatas.Add(newData);
                    }
                }
                return;
            }
            SecurityLatestData data = datas[0];
            int[] fields = new int[7];
            fields[0] = KeyFields.CLOSE_INDEX;
            fields[1] = KeyFields.HIGH_INDEX;
            fields[2] = KeyFields.LOW_INDEX;
            fields[3] = KeyFields.OPEN_INDEX;
            fields[4] = KeyFields.VOL_INDEX;
            fields[5] = KeyFields.AMOUNT_INDEX;
            fields[6] = KeyFields.AVGPRICE_INDEX;
            CTable dataSource = m_chart.DataSource;
            double sumAmount = 0, sumVol = 0;
            double amount = 0, vol = 0;
            if (m_cycle == SecurityDataHelper.CYCLE_WEEK)
            {
                sumAmount = SecurityDataHelper.SumHistoryData(m_chart.DataSourceHistory, data.m_date, SecurityDataHelper.CYCLE_WEEK, KeyFields.AMOUNTHISTORY_INDEX);
                sumVol = SecurityDataHelper.SumHistoryData(m_chart.DataSourceHistory, data.m_date, SecurityDataHelper.CYCLE_WEEK, KeyFields.VOLHISTORY_INDEX);
            }
            else if (m_cycle == SecurityDataHelper.CYCLE_MONTH)
            {
                sumAmount = SecurityDataHelper.SumHistoryData(m_chart.DataSourceHistory, data.m_date, SecurityDataHelper.CYCLE_MONTH, KeyFields.AMOUNTHISTORY_INDEX);
                sumVol = SecurityDataHelper.SumHistoryData(m_chart.DataSourceHistory, data.m_date, SecurityDataHelper.CYCLE_MONTH, KeyFields.VOLHISTORY_INDEX);
            }
            else if (m_cycle == 0)
            {
                sumAmount = SecurityDataHelper.SumHistoryData(m_chart.DataSource, data.m_date, 0, KeyFields.AMOUNT_INDEX);
                sumVol = SecurityDataHelper.SumHistoryData(m_chart.DataSource, data.m_date, 0, KeyFields.VOL_INDEX);
            }
            for (int i = 0; i < dataSize; i++)
            {
                SecurityData latestData = new SecurityData();
                data = datas[i];
                if (data.m_dVolume <= 0)
                {
                    continue;
                }
                MinuteKLineDate minuteKLineDate = new MinuteKLineDate(); ;
                double date = 0;
                int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, ms = 0;
                CStrA.M130(data.m_date, ref year, ref month, ref day, ref hour, ref minute, ref second, ref ms);
                minuteKLineDate.m_cycle = m_cycle;
                minuteKLineDate.m_day = day;
                minuteKLineDate.m_hour = hour;
                minuteKLineDate.m_minute = minute;
                minuteKLineDate.m_month = month;
                minuteKLineDate.m_year = year;
                SecurityDataHelper.CalculateMinuteKLineDate(minuteKLineDate, 0, m_cycle);
                if (m_cycle == SecurityDataHelper.CYCLE_DAY)
                {
                    if (hour >= 0 && hour <= 5)
                    {
                        day = day - 1;
                    }
                    date = CStrA.M129(year, month, day, 0, 0, 0, 0);
                }
                else if (m_cycle == SecurityDataHelper.CYCLE_WEEK)
                {
                    if (hour >= 0 && hour <= 5)
                    {
                        day = day - 1;
                    }
                    int dayOfWeek = SecurityDataHelper.DayOfWeek(year, month, day);
                    if (dayOfWeek >= 5)
                    {
                        dayOfWeek = 4;
                    }
                    date = CStrA.M129(year, month, day - dayOfWeek, 0, 0, 0, 0);
                }
                else if (m_cycle == SecurityDataHelper.CYCLE_MONTH)
                {
                    date = CStrA.M129(year, month, 1, 0, 0, 0, 0);
                }
                else if (m_cycle == SecurityDataHelper.CYCLE_MINUTE_1)
                {
                    date = CStrA.M129(year, month, day, hour, minute + 1, 0, 0);
                }
                else if (m_cycle > SecurityDataHelper.CYCLE_MINUTE_1 && m_cycle <= SecurityDataHelper.CYCLE_MINUTE_60)
                {
                    date = CStrA.M129(year, month, day, minuteKLineDate.m_hour_cycle, (minuteKLineDate.m_minute_cycle + 1) * m_cycle, 0, 0);
                }
                else if (m_cycle == 0)
                {
                    date = CStrA.M129(year, month, day, hour, minute + 1, 0, 0);
                }
                else
                {
                    date = data.m_date;
                }
                if (date != 0)
                {
                    latestData.m_date = date;
                    int index = dataSource.GetRowIndex(date);
                    double open = 0, low = 0, high = 0;
                    if (m_cycle >= SecurityDataHelper.CYCLE_MINUTE_1 && m_cycle <= SecurityDataHelper.CYCLE_MINUTE_60)
                    {
                        if (index == -1)
                        {
                            latestData.m_open = data.m_close;
                            latestData.m_high = data.m_close;
                            latestData.m_low = data.m_close;
                            latestData.m_close = data.m_close;
                            latestData.m_volume = data.m_dVolume;
                        }
                        else
                        {
                            high = dataSource.Get2(index, KeyFields.HIGH_INDEX);
                            low = dataSource.Get2(index, KeyFields.LOW_INDEX);
                            open = dataSource.Get2(index, KeyFields.OPEN_INDEX);
                            vol = dataSource.Get2(index, KeyFields.VOL_INDEX);
                            latestData.m_open = open;
                            latestData.m_high = high;
                            latestData.m_low = low;
                            if (data.m_close > high)
                            {
                                latestData.m_high = data.m_close;
                            }
                            if (data.m_close < low)
                            {
                                latestData.m_low = data.m_close;
                            }
                            latestData.m_close = data.m_close;
                            latestData.m_amount = data.m_dVolume * data.m_close + amount;
                            latestData.m_volume = data.m_dVolume + vol;
                        }
                    }
                    else if (m_cycle >= SecurityDataHelper.CYCLE_DAY)
                    {
                        if (index == -1)
                        {
                            latestData.m_open = data.m_open;
                            latestData.m_high = data.m_high;
                            latestData.m_low = data.m_low;
                            latestData.m_close = data.m_close;
                            latestData.m_volume = data.m_dVolume;
                        }
                        else
                        {
                            high = dataSource.Get2(index, KeyFields.HIGH_INDEX);
                            low = dataSource.Get2(index, KeyFields.LOW_INDEX);
                            open = dataSource.Get2(index, KeyFields.OPEN_INDEX);
                            vol = dataSource.Get2(index, KeyFields.VOL_INDEX);
                            latestData.m_open = open;
                            latestData.m_high = high;
                            latestData.m_low = low;
                            if (data.m_high > high)
                            {
                                latestData.m_high = data.m_high;
                            }
                            if (data.m_low < low)
                            {
                                latestData.m_low = data.m_low;
                            }
                            latestData.m_close = data.m_close;
                            latestData.m_amount = data.m_dVolume * data.m_close + amount;
                            latestData.m_volume = data.m_dVolume + vol;
                        }
                    }
                    else if (m_cycle == 0)
                    {
                        if (index == -1)
                        {
                            latestData.m_open = data.m_open;
                            latestData.m_high = data.m_high;
                            latestData.m_low = data.m_low;
                            latestData.m_close = data.m_close;
                            latestData.m_volume = data.m_dVolume;
                        }
                        else
                        {
                            high = dataSource.Get2(index, KeyFields.HIGH_INDEX);
                            low = dataSource.Get2(index, KeyFields.LOW_INDEX);
                            open = dataSource.Get2(index, KeyFields.OPEN_INDEX);
                            vol = dataSource.Get2(index, KeyFields.VOL_INDEX);
                            amount = dataSource.Get2(index, KeyFields.AMOUNT_INDEX);
                            if (double.IsNaN(high))
                            {
                                high = latestData.m_high;
                            }
                            if (double.IsNaN(low))
                            {
                                low = latestData.m_low;
                            }
                            if (double.IsNaN(open))
                            {
                                open = latestData.m_open;
                            }
                            if (double.IsNaN(vol))
                            {
                                vol = 0;
                            }
                            latestData.m_open = open;
                            latestData.m_high = high;
                            latestData.m_low = low;
                            if (data.m_high > high)
                            {
                                latestData.m_high = data.m_high;
                            }
                            if (data.m_low < low)
                            {
                                latestData.m_low = data.m_low;
                            }
                            latestData.m_close = data.m_close;
                            latestData.m_amount = amount + data.m_close * data.m_dVolume;
                            latestData.m_volume = data.m_dVolume + vol;
                        }
                        sumAmount += data.m_dVolume * data.m_close;
                        sumVol += data.m_dVolume;
                        latestData.m_avgPrice = sumAmount / sumVol;
                    }
                    SecurityDataHelper.InsertLatestData(m_chart, dataSource, m_indicators, fields, latestData);
                    index = dataSource.GetRowIndex(date);
                    if (m_cycle == 0)
                    {
                        SecurityDataHelper.InsertLatestData(m_chart, dataSource, m_indicators, fields, m_preClosePrice, index);
                    }
                    if (!m_showMinuteLine && index >= 0)
                    {
                        double close = dataSource.Get2(index, KeyFields.CLOSE_INDEX);
                        open = dataSource.Get2(index, KeyFields.OPEN_INDEX);
                        if (close >= open)
                        {
                            dataSource.Set2(index, m_bar.StyleField, 1);
                            dataSource.Set2(index, m_bar.ColorField, CDraw.PCOLORS_UPCOLOR);
                        }
                        else
                        {
                            dataSource.Set2(index, m_bar.StyleField, 0);
                            dataSource.Set2(index, m_bar.ColorField, CDraw.PCOLORS_DOWNCOLOR2);
                        }
                    }
                }
            }
            RefreshData();
            m_chart.Update();
            m_chart.Invalidate();
        }

        /// <summary>
        /// 查询股票
        /// </summary>
        /// <param name="security">股票</param>
        public void SearchSecurity(GSecurity security)
        {
            if (m_showMinuteLine)
            {
                m_chart.AutoFillHScale = true;
                if (m_candleHScalePixel > 0)
                {
                    m_candleHScalePixel = m_chart.HScalePixel;
                }
            }
            else
            {
                m_chart.AutoFillHScale = false;
                if (m_candleHScalePixel == 0)
                {
                    m_candleHScalePixel = 9;
                }
                m_chart.HScalePixel = m_candleHScalePixel;
            }
            bool showCrossLine = m_chart.ShowCrossLine;
            m_index = -1;
            m_chart.Clear();
            m_chart.ShowCrossLine = showCrossLine;
            System.GC.Collect();
            m_searchSecurity = security;
            m_latestDiv.Type = security.m_type;
            CFTService.QuitLV2(m_latestDiv.SecurityCode);
            m_latestDiv.SecurityCode = security.m_code;
            m_latestDiv.SecurityName = security.m_name;
            HistoryDataInfo dataInfo = new HistoryDataInfo();
            dataInfo.m_code = security.m_code;
            int cycle = Cycle;
            if (cycle <= SecurityDataHelper.CYCLE_MINUTE_60)
            {
                dataInfo.m_cycle = cycle;
                if (m_showMinuteLine)
                {
                    m_candleDiv.TitleBar.Text = "分时线";
                }
                else
                {
                    m_candleDiv.TitleBar.Text = dataInfo.m_cycle.ToString() + "分钟线";
                }
            }
            else
            {
                if (cycle == SecurityDataHelper.CYCLE_DAY)
                {
                    m_candleDiv.TitleBar.Text = "日线";
                }
                else if (cycle == SecurityDataHelper.CYCLE_WEEK)
                {
                    m_candleDiv.TitleBar.Text = "周线";
                }
                else if (cycle == SecurityDataHelper.CYCLE_MONTH)
                {
                    m_candleDiv.TitleBar.Text = "月线";
                }
                dataInfo.m_cycle = cycle;
            }
            m_refreshKLineFlag = false;
            dataInfo.m_pushData = true;
            dataInfo.m_subscription = m_subscription;
            dataInfo.m_type = security.m_type;
            KLineCycle klineCycle = KLineCycle.CycleDay;
            if (dataInfo.m_cycle == 0)
            {
                klineCycle = KLineCycle.CycleMint1;
            }
            else if (dataInfo.m_cycle == 1)
            {
                klineCycle = KLineCycle.CycleMint1;
            }
            else if (dataInfo.m_cycle == 5)
            {
                klineCycle = KLineCycle.CycleMint5;
            }
            else if (dataInfo.m_cycle == 15)
            {
                klineCycle = KLineCycle.CycleMint15;
            }
            else if (dataInfo.m_cycle == 30)
            {
                klineCycle = KLineCycle.CycleMint30;
            }
            else if (dataInfo.m_cycle == 60)
            {
                klineCycle = KLineCycle.CycleMint60;
            }
            else if (dataInfo.m_cycle == 1440)
            {
                klineCycle = KLineCycle.CycleDay;
            }
            else if (dataInfo.m_cycle == 10080)
            {
                klineCycle = KLineCycle.CycleWeek;
            }
            else if (dataInfo.m_cycle == 43200)
            {
                klineCycle = KLineCycle.CycleMonth;
            }
            SecurityLatestData data = new SecurityLatestData();
            SecurityService.GetLatestData(security.m_code, ref data);
            m_preClosePrice = data.m_lastClose;
            double min = m_preClosePrice * 0.999;
            double max = m_preClosePrice * 1.001;
            m_candleDiv.LeftVScale.VisibleMin = min;
            m_candleDiv.LeftVScale.VisibleMax = max;
            m_candleDiv.RightVScale.VisibleMin = min;
            m_candleDiv.RightVScale.VisibleMax = max;
            if (m_showMinuteLine)
            {
                m_candleDiv.LeftVScale.AutoMaxMin = true;
                m_candleDiv.RightVScale.AutoMaxMin = true;
            }
            else
            {
                m_candleDiv.LeftVScale.AutoMaxMin = true;
                m_candleDiv.RightVScale.AutoMaxMin = true;
            }
            if (m_cycle == 0)
            {
                CFTService.QueryTrendLine(dataInfo.m_code);
            }
            else
            {
                CFTService.QueryHistoryDatas(dataInfo.m_code, klineCycle);
            }
            m_chart.Update();
            m_mainFrame.Native.Invalidate();
        }
        #endregion
    }
}
