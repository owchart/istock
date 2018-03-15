/*****************************************************************************\
*                                                                             *
* OwChart.cs -  Chart functions, types, and definitions.                      *
*                                                                             *
*               Version 1.00  ����                                          *
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
    /// ����ϵͳ
    /// </summary>
    public class KLineDiv
    {
        #region Lord 2016/12/24
        /// <summary>
        /// ��������ϵͳ
        /// </summary>
        public KLineDiv(MainFrame mainFrame)
        {
            m_mainFrame = mainFrame;
            InitInterface();
            if (m_rightMenu == null)
            {
                //�����Ҽ��˵�
                m_rightMenu = m_mainFrame.GetMenu("rightMenu");
                MenuItemMouseEvent menuItemClickEvent = new MenuItemMouseEvent(MenuItemClick);
                m_rightMenu.RegisterEvent(menuItemClickEvent, EVENTID.MENUITEMCLICK);
                m_rightMenu.Visible = false;
                //����
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
        /// ��Ҫ��ӵĻ��߹���
        /// </summary>
        private String m_addingPlotType;

        /// <summary>
        /// �ɽ�����
        /// </summary>
        private BarShape m_bar;

        /// <summary>
        /// �ɽ���Ԥ����
        /// </summary>
        private BarShape m_barForecast;

        /// <summary>
        /// K��
        /// </summary>
        private CandleShape m_candle;

        /// <summary>
        /// ��div�ı��
        /// </summary>
        private CDiv m_candleDiv;

        /// <summary>
        /// K�ߵĺ�����
        /// </summary>
        private double m_candleHScalePixel;

        /// <summary>
        /// ��ǰ������ͼ��
        /// </summary>
        private CDiv m_currentDiv;

        /// <summary>
        /// ���еĲ�
        /// </summary>
        private List<CDiv> m_divs = new List<CDiv>();

        /// <summary>
        /// ������
        /// </summary>
        private FloatDiv m_floatDiv;

        /// <summary>
        /// ����Ĳ���
        /// </summary>
        private List<double> m_hScaleSteps = new List<double>();

        /// <summary>
        /// ��ʱ�ߵ�ƽ����
        /// </summary>
        private PolylineShape m_minuteAvgLine;

        /// <summary>
        /// ��ʱ��
        /// </summary>
        private PolylineShape m_minuteLine;

        /// <summary>
        /// ���߹��ߵ��Ҽ��˵�
        /// </summary>
        private MenuA m_plotRightMenu;

        /// <summary>
        /// ����
        /// </summary>
        public double m_preClosePrice;

        /// <summary>
        /// ˢ��K�ߵı�ʶ
        /// </summary>
        public bool m_refreshKLineFlag;

        /// <summary>
        /// �Ƿ�ת������
        /// </summary>
        private bool m_reverseVScale = false;

        /// <summary>
        /// �Ҽ��˵�
        /// </summary>
        private MenuA m_rightMenu;

        /// <summary>
        /// ���ڲ�ѯ��֤ȯ����
        /// </summary>
        private GSecurity m_searchSecurity;

        /// <summary>
        /// ��������
        /// </summary>
        private List<SecurityLatestData> m_securityDatas = new List<SecurityLatestData>();

        /// <summary>
        /// �ɽ�����
        /// </summary>
        private CDiv m_volumeDiv;

        private ChartAEx m_chart;

        /// <summary>
        /// ��ȡ������ͼ�οؼ�
        /// </summary>
        public ChartAEx Chart
        {
            get { return m_chart; }
            set { m_chart = value; }
        }

        private int m_cycle = 0;

        /// <summary>
        /// ��ȡ����������
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
        /// ��ȡ�����ü۸���С��λ��
        /// </summary>
        public int Digit
        {
            get { return m_digit; }
            set { m_digit = value; }
        }

        private int m_index = -1;

        /// <summary>
        /// ��ȡ��ǰʵ�ʵ���������
        /// </summary>
        public int Index
        {
            get { return m_index; }
        }

        private List<CIndicator> m_indicators = new List<CIndicator>();

        /// <summary>
        /// ��ȡ����������ָ��
        /// </summary>
        public List<CIndicator> Indicators
        {
            get { return m_indicators; }
            set { m_indicators = value; }
        }

        private LatestDiv m_latestDiv;

        /// <summary>
        /// ��ȡ������ʵʱ�������
        /// </summary>
        public LatestDiv LatestDiv
        {
            get { return m_latestDiv; }
            set { m_latestDiv = value; }
        }

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        public SecurityLatestData LatestData
        {
            get { return m_latestDiv.LatestData; }
        }

        private MainFrame m_mainFrame;

        /// <summary>
        /// ��ȡ�����������
        /// </summary>
        public MainFrame MainFrame
        {
            get { return m_mainFrame; }
            set { m_mainFrame = value; }
        }

        private SearchDiv m_searchDiv;

        /// <summary>
        /// ��ȡ�����ü��̾���
        /// </summary>
        public SearchDiv SearchDiv
        {
            get { return m_searchDiv; }
            set { m_searchDiv = value; }
        }

        private bool m_showMinuteLine = true;

        /// <summary>
        /// ��ȡ�������Ƿ��ʱͼ
        /// </summary>
        public bool ShowMinuteLine
        {
            get { return m_showMinuteLine; }
            set { m_showMinuteLine = value; }
        }

        private int m_subscription = 1;

        /// <summary>
        /// ��ȡ�����ø�Ȩ��ʽ
        /// </summary>
        public int Subscription
        {
            get { return m_subscription; }
            set { m_subscription = value; }
        }

        /// <summary>
        /// ��ӿհײ�
        /// </summary>
        public void AddBlankDiv()
        {
            //���ص�x��
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
        /// ������ͼָ��
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="title">����</param>
        /// <param name="script">�ű�</param>
        /// <param name="parameters">����</param>
        /// <param name="div">ͼ��</param>
        /// <param name="update">�Ƿ����</param>
        /// <returns>ָ�����</returns>
        public CIndicator AddMainIndicator(String name, String title, String script, String parameters, CDiv div, bool update)
        {
            //��������
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
            //ˢ��ͼ��
            if (update)
            {
                m_chart.Update();
                m_chart.Invalidate();
            }
            return indicator;
        }

        /// <summary>
        /// ��Ӹ�ͼָ��
        /// </summary>
        /// <param name="name">����</param>
        /// <param name="script">�ű�</param>
        /// <param name="param">����</param>
        /// <param name="div">ͼ��</param>
        /// <param name="update">�Ƿ���²���</param>
        /// <returns>ָ�����</returns>
        public CIndicator AddViceIndicator(String name, String script, String parameters, CDiv div, bool update)
        {
            CIndicator indicator = SecurityDataHelper.CreateIndicator(m_chart, m_chart.DataSource, script, parameters);
            indicator.AttachVScale = AttachVScale.Right;
            m_indicators.Add(indicator);
            indicator.Div = div;
            indicator.Name = name;
            //��������
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
        /// �޸�����
        /// </summary>
        /// <param name="cycle">����</param>
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
        /// ������¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">����ֵ</param>
        private void ChartMouseDown(object sender, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            ControlA control = sender as ControlA;
            mp = control.PointToNative(mp);
            if (m_addingPlotType != null && m_addingPlotType.Length > 0)
            {
                if (button == MouseButtonsA.Left && clicks == 1)
                {
                    //��ӻ��߹���
                    long plotColor = CDraw.PCOLORS_LINECOLOR;
                    CPlot plot = null;
                    //�Զ��廭��
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
                            //�������߹��ߵ��Ҽ��˵�
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
                            //�����Ҽ��˵�
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
        /// ����ƶ��¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">����ֵ</param>
        private void ChartMouseMove(object sender, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            if (m_addingPlotType != null && m_addingPlotType.Length > 0)
            {
                m_chart.Cursor = CursorsA.Hand;
                m_chart.Invalidate();
            }
        }

        /// <summary>
        /// ���̧���¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">����ֵ</param>
        private void ChartMouseUp(object sender, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            m_floatDiv.Visible = m_chart.ShowCrossLine;
            m_mainFrame.Native.Invalidate();
        }

        /// <summary>
        /// ��ť����¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">����ֵ</param>
        private void ClickButton(object sender, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            ButtonA closeButton = sender as ButtonA;
            WindowA window = closeButton.Parent as WindowA;
            window.Close();
            window.Dispose();
        }

        /// <summary>
        /// ɾ��ָ��
        /// </summary>
        /// <param name="indicator">ָ��</param>
        public void DeleteIndicator(CIndicator indicator)
        {
            indicator.Clear();
            m_indicators.Remove(indicator);
            indicator.Dispose();
            m_chart.Update();
            m_mainFrame.Native.Invalidate();
        }

        /// <summary>
        /// ɾ������ָ��
        /// </summary>
        /// <param name="update">�Ƿ����</param>
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
        /// ɾ��ѡ�е�ָ��
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
        /// ɾ��ѡ�еĻ���
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
        /// ��ȡѡ�е�ָ��
        /// </summary>
        /// <returns>ָ��</returns>
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
        /// ��ʼ��ͼ�ν���
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
            //���ÿ����϶�K�ߣ��ɽ������߼����
            m_chart.CanMoveShape = true;
            //���ù�������
            m_chart.ScrollAddSpeed = true;
            //��������Y��Ŀ��
            m_chart.LeftVScaleWidth = 85;
            m_chart.RightVScaleWidth = 85;
            //����X��̶ȼ��
            m_chart.HScalePixel = 3;
            //����X��
            m_chart.HScaleFieldText = "����";
            //���k�߲�
            m_candleDiv = m_chart.AddDiv(60);
            m_candleDiv.BackColor = CDraw.PCOLORS_BACKCOLOR4;
            m_candleDiv.TitleBar.Text = "��ʱ��";
            //������Div����Y����ֵ���»���
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
            //���K��ͼ
            m_candle = new CandleShape();
            m_candleDiv.AddShape(m_candle);
            m_candle.CloseField = KeyFields.CLOSE_INDEX;
            m_candle.HighField = KeyFields.HIGH_INDEX;
            m_candle.LowField = KeyFields.LOW_INDEX;
            m_candle.OpenField = KeyFields.OPEN_INDEX;
            m_candle.CloseFieldText = "����";
            m_candle.HighFieldText = "���";
            m_candle.LowFieldText = "���";
            m_candle.OpenFieldText = "����";
            m_candle.Visible = false;
            //��ʱ��
            m_minuteLine = new PolylineShape();
            m_candleDiv.AddShape(m_minuteLine);
            m_minuteLine.Color = COLOR.ARGB(255, 255, 255);
            m_minuteLine.FieldName = KeyFields.CLOSE_INDEX;
            //��ʱ�ߵ�ƽ����
            m_minuteAvgLine = new PolylineShape();
            m_candleDiv.AddShape(m_minuteAvgLine);
            m_minuteAvgLine.Color = COLOR.ARGB(255, 255, 80);
            m_minuteAvgLine.FieldName = KeyFields.AVGPRICE_INDEX;
            //��ӳɽ�����
            m_volumeDiv = m_chart.AddDiv(15);
            m_volumeDiv.BackColor = CDraw.PCOLORS_BACKCOLOR4;
            //���óɽ����ĵ�λ
            m_volumeDiv.LeftVScale.Digit = 0;
            m_volumeDiv.LeftVScale.Font = new FONT("Arial", 14, false, false, false);
            m_volumeDiv.VGrid.Distance = 30;
            m_volumeDiv.RightVScale.Digit = 0;
            m_volumeDiv.RightVScale.Font = new FONT("Arial", 14, false, false, false);
            //��ӳɽ���
            m_bar = new BarShape();
            m_bar.ColorField = CTable.AutoField;
            m_bar.StyleField = CTable.AutoField;
            m_bar.UpColor = CDraw.PCOLORS_LINECOLOR2;
            m_volumeDiv.AddShape(m_bar);
            m_bar.FieldName = KeyFields.VOL_INDEX;
            //��ӳɽ���Ԥ��
            m_barForecast = new BarShape();
            m_barForecast.FieldText = "�ɽ���Ԥ��";
            m_barForecast.ZOrder = -1;
            m_volumeDiv.AddShape(m_barForecast);
            m_barForecast.FieldName = CTable.AutoField;
            //���ñ���
            m_volumeDiv.TitleBar.Text = "�ɽ���";
            //���óɽ�����ʾ����
            m_bar.FieldText = "�ɽ���";
            //���óɽ�������ֻ��ʾֵ
            CTitle barTitle = new CTitle(KeyFields.VOL_INDEX, "�ɽ���", m_bar.DownColor, 0, true);
            barTitle.FieldTextMode = TextMode.Value;
            m_volumeDiv.TitleBar.Titles.Add(barTitle);
            //���ָ���
            CDiv indDiv = m_chart.AddDiv(25);
            indDiv.BackColor = CDraw.PCOLORS_BACKCOLOR4;
            indDiv.VGrid.Distance = 30;
            indDiv.LeftVScale.PaddingTop = 2;
            indDiv.LeftVScale.PaddingBottom = 2;
            indDiv.LeftVScale.Font = new FONT("Arial", 14, false, false, false);
            indDiv.RightVScale.PaddingTop = 2;
            indDiv.RightVScale.PaddingBottom = 2;
            indDiv.RightVScale.Font = new FONT("Arial", 14, false, false, false);
            //����X�᲻�ɼ�
            m_candleDiv.HScale.Visible = false;
            m_candleDiv.HScale.Height = 0;
            m_volumeDiv.HScale.Visible = false;
            m_volumeDiv.HScale.Height = 0;
            indDiv.HScale.Visible = true;
            indDiv.HScale.Height = 22;
            //�������������ɫ
            m_volumeDiv.LeftVScale.ForeColor = CDraw.PCOLORS_FORECOLOR11;
            m_volumeDiv.RightVScale.ForeColor = CDraw.PCOLORS_FORECOLOR11;
            indDiv.LeftVScale.ForeColor = CDraw.PCOLORS_FORECOLOR6;
            indDiv.RightVScale.ForeColor = CDraw.PCOLORS_FORECOLOR6;
            //����������������߼��
            //��ӵ�����
            m_divs.AddRange(new CDiv[] { m_candleDiv, m_volumeDiv, indDiv });
            //����û��Զ����
            m_floatDiv = m_mainFrame.FindControl("divFloat") as FloatDiv;
            m_floatDiv.Chart = this;
            //��ǰ���ݲ�
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
        /// ���
        /// </summary>
        /// <param name="sender">�ؼ�</param>
        /// <param name="item">�˵���</param>
        public void MenuItemClick(object sender, MenuItemA item, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            String name = item.Name;
            if (name != null && name.Length > 0)
            {
                bool setChecked = false;
                //��ͼ����
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
                //����������
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
                //�л�����
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
                //��Ȩ��ʽ
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
                //���߹���
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
        /// �Ƴ�����ӵĿհײ�
        /// </summary>
        /// <param name="update">�Ƿ���²���</param>
        public void RemoveBlankDivs(bool update)
        {
            List<CDiv> removeDivs = new List<CDiv>();
            //��ȡҪ�Ƴ��Ĳ�
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
            //�Ƴ���
            int removeDivSize = removeDivs.Count;
            for (int i = 0; i < removeDivSize; i++)
            {
                m_chart.RemoveDiv(removeDivs[i]);
            }
            //��������X��
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
        /// ˢ������
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
                    //������ʾ����
                    List<BaseShape> shapes = indicator.GetShapes();
                    int shapesSize = shapes.Count;
                    for (int j = 0; j < shapesSize; j++)
                    {
                        BaseShape shape = shapes[j];
                        shape.Visible = !m_showMinuteLine;
                    }
                    //������ʾ����
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
        /// ��ѯ��Ʊ
        /// </summary>
        /// <param name="security">��Ʊ</param>
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
                    m_candleDiv.TitleBar.Text = "��ʱ��";
                }
                else
                {
                    m_candleDiv.TitleBar.Text = dataInfo.m_cycle.ToString() + "������";
                }
            }
            else
            {
                if (cycle == SecurityDataHelper.CYCLE_DAY)
                {
                    m_candleDiv.TitleBar.Text = "����";
                }
                else if (cycle == SecurityDataHelper.CYCLE_WEEK)
                {
                    m_candleDiv.TitleBar.Text = "����";
                }
                else if (cycle == SecurityDataHelper.CYCLE_MONTH)
                {
                    m_candleDiv.TitleBar.Text = "����";
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
