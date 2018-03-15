/*****************************************************************************\
*                                                                             *
* LatestDiv.cs - Latest div functions, types, and definitions.                *
*                                                                             *
*               Version 1.00  ����                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/3/21.                                    *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace OwLib
{
    /// <summary>
    /// ʵʱ����
    /// </summary>
    public class LatestDiv : ControlA
    {
        #region Lord 2016/3/21
        /// <summary>
        /// �����ؼ�
        /// </summary>
        public LatestDiv()
        {
            BackColor = CDraw.PCOLORS_BACKCOLOR4;
            BorderColor = CDraw.PCOLORS_LINECOLOR4;
        }

        /// <summary>
        /// ��������
        /// </summary>
        private List<String> m_buySellStrs = new List<String>();

        /// <summary>
        /// �ɽ���¼��
        /// </summary>
        private GridA m_gridTransaction;

        /// <summary>
        /// ��������������
        /// </summary>
        private int m_requestID = BaseService.GetRequestID();

        private KLine m_chart;

        /// <summary>
        /// ��ȡ������
        /// </summary>
        public KLine Chart
        {
            get { return m_chart; }
            set { m_chart = value; }
        }

        private int m_digit = 2;

        /// <summary>
        /// ��ȡ�����ñ���С����λ��
        /// </summary>
        public int Digit
        {
            get { return m_digit; }
            set { m_digit = value; }
        }

        private SecurityLatestData m_latestData = new SecurityLatestData();

        /// <summary>
        /// ��ȡ������ʵʱ����
        /// </summary>
        public SecurityLatestData LatestData
        {
            get { return m_latestData; }
            set
            {
                m_latestData.Copy(value);
                if (m_chart != null)
                {
                    List<SecurityLatestData> datas = new List<SecurityLatestData>();
                    datas.Add(value);
                    m_chart.m_refreshKLineFlag = true;
                    m_chart.RefreshKLineData(datas);
                }
                Invalidate();
            }
        }

        private bool m_lV2;

        /// <summary>
        /// ��ȡ�������Ƿ�LV2
        /// </summary>
        public bool LV2
        {
            get { return m_lV2; }
            set { m_lV2 = value; }
        }

        private String m_securityCode;

        /// <summary>
        /// ��ȡ�����ù�Ʊ����
        /// </summary>
        public String SecurityCode
        {
            get { return m_securityCode; }
            set
            {
                m_latestData = new SecurityLatestData();
                m_lV2 = m_type != 17;
                m_securityCode = value;
                m_gridTransaction.ClearRows();
                m_gridTransaction.Update();
                Update();
            }
        }

        private String m_securityName;

        /// <summary>
        /// ��ȡ�����ù�Ʊ����
        /// </summary>
        public String SecurityName
        {
            get { return m_securityName; }
            set { m_securityName = value; }
        }

        private int m_type;

        /// <summary>
        /// ��ȡ����������
        /// </summary>
        public int Type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        /// <summary>
        /// ��ȡ���ֵ
        /// </summary>
        /// <param name="list">����</param>
        /// <returns>���ֵ</returns>
        public double Max(List<double> list)
        {
            double max = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    max = list[i];
                }
                else
                {
                    if (list[i] > max)
                    {
                        max = list[i];
                    }
                }
            }
            return max;
        }

        /// <summary>
        /// �ؼ���ӷ���
        /// </summary>
        public override void OnAdd()
        {
            base.OnAdd();
            if (m_gridTransaction == null)
            {
                m_latestData = new SecurityLatestData();
                m_gridTransaction = new GridA();
                m_gridTransaction.BackColor = COLOR.EMPTY;
                m_gridTransaction.BorderColor = COLOR.EMPTY;
                m_gridTransaction.GridLineColor = COLOR.EMPTY;
                m_gridTransaction.HeaderVisible = false;
                m_gridTransaction.SelectionMode = GridSelectionMode.SelectNone;
                AddControl(m_gridTransaction);
                GridColumn dateColumn = new GridColumn();
                dateColumn.Width = 80;
                m_gridTransaction.AddColumn(dateColumn);
                GridColumn priceColumn = new GridColumn();
                priceColumn.Width = 70;
                m_gridTransaction.AddColumn(priceColumn);
                GridColumn volumeColumn = new GridColumn();
                volumeColumn.Width = 100;
                m_gridTransaction.AddColumn(volumeColumn);
                m_gridTransaction.Update();
            }
        }

        /// <summary>
        /// ������¼�
        /// </summary>
        /// <param name="mp"></param>
        /// <param name="button"></param>
        /// <param name="clicks"></param>
        /// <param name="delta"></param>
        public override void OnClick(POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            base.OnClick(mp, button, clicks, delta);
            if (mp.y < 30)
            {
                Process.Start("http://app.jg.eastmoney.com/html_F9/index.html?securityCode=" + m_latestData.m_code);
            }
        }

        /// <summary>
        /// ����ǰ������
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="clipRect">�ü�����</param>
        public override void OnPaintForeground(CPaint paint, RECT clipRect)
        {
            int width = Width;
            int height = Height;
            if (width > 0 && height > 0)
            {
                FONT font = new FONT("SimSun", 16, false, false, false);
                FONT lfont = new FONT("SimSun", 14, false, false, false);
                long wordColor = CDraw.PCOLORS_FORECOLOR5;
                int top = 32, step = 20;
                //��������
                CDraw.DrawText(paint, "��", wordColor, font, 1, (m_lV2 ? 87 : 47));
                CDraw.DrawText(paint, "��", wordColor, font, 1, (m_lV2 ? 140 : 100));
                CDraw.DrawText(paint, "��", wordColor, font, 1, (m_lV2 ? 267 : 147));
                CDraw.DrawText(paint, "��", wordColor, font, 1, (m_lV2 ? 310 : 200));
                String buySellStr = "5,4,3,2,1,1,2,3,4,5";
                if (m_lV2)
                {
                    step = 16;
                    buySellStr = "������,10,9,8,7,6," + buySellStr + ",6,7,8,9,10,������";
                    font.m_fontSize = 14;
                }
                String[] buySellStrs = buySellStr.Split(',');
                int strsSize = buySellStrs.Length;
                for (int i = 0; i < strsSize; i++)
                {
                    CDraw.DrawText(paint, buySellStrs[i], wordColor, font, 25, top);
                    top += step;
                }
                font.m_fontSize = 16;
                top = m_lV2 ? 390 : 232;
                CDraw.DrawText(paint, "����", wordColor, font, 1, top);
                CDraw.DrawText(paint, "����", wordColor, font, 1, top + 20);
                CDraw.DrawText(paint, "����", wordColor, font, 1, top + 40);
                CDraw.DrawText(paint, "����", wordColor, font, 1, top + 60);
                CDraw.DrawText(paint, "��ͣ", wordColor, font, 1, top + 80);
                CDraw.DrawText(paint, "����", wordColor, font, 1, top + 100);
                CDraw.DrawText(paint, "����", wordColor, font, 110, top);
                CDraw.DrawText(paint, "���", wordColor, font, 110, top + 20);
                CDraw.DrawText(paint, "���", wordColor, font, 110, top + 40);
                CDraw.DrawText(paint, "����", wordColor, font, 110, top + 60);
                CDraw.DrawText(paint, "��ͣ", wordColor, font, 110, top + 80);
                CDraw.DrawText(paint, "����", wordColor, font, 110, top + 100);
                font.m_bold = true;
                //����Ʊ����
                long yellowColor = COLOR.ARGB(255, 255, 80);
                if (m_latestData.m_code!=null && m_latestData.m_code.Length > 0)
                {
                    double close = m_latestData.m_close, open = m_latestData.m_open, high = m_latestData.m_high, low = m_latestData.m_low, lastClose = m_latestData.m_lastClose;
                    if (close == 0)
                    {
                        if (m_latestData.m_buyPrice1 > 0)
                        {
                            close = m_latestData.m_buyPrice1;
                            open = m_latestData.m_buyPrice1;
                            high = m_latestData.m_buyPrice1;
                            low = m_latestData.m_buyPrice1;
                        }
                        else if (m_latestData.m_sellPrice1 > 0)
                        {
                            close = m_latestData.m_sellPrice1;
                            open = m_latestData.m_sellPrice1;
                            high = m_latestData.m_sellPrice1;
                            low = m_latestData.m_sellPrice1;
                        }
                    }
                    if (lastClose == 0)
                    {
                        lastClose = close;
                    }
                    List<double> plist = new List<double>();
                    List<double> vlist = new List<double>();
                    if (m_lV2)
                    {
                        plist.Add(m_latestData.m_sellPrice10);
                        plist.Add(m_latestData.m_sellPrice9);
                        plist.Add(m_latestData.m_sellPrice8);
                        plist.Add(m_latestData.m_sellPrice7);
                        plist.Add(m_latestData.m_sellPrice6);
                        vlist.Add(m_latestData.m_sellVolume10);
                        vlist.Add(m_latestData.m_sellVolume9);
                        vlist.Add(m_latestData.m_sellVolume8);
                        vlist.Add(m_latestData.m_sellVolume7);
                        vlist.Add(m_latestData.m_sellVolume6);
                    }
                    plist.Add(m_latestData.m_sellPrice5);
                    plist.Add(m_latestData.m_sellPrice4);
                    plist.Add(m_latestData.m_sellPrice3);
                    plist.Add(m_latestData.m_sellPrice2);
                    plist.Add(m_latestData.m_sellPrice1);
                    vlist.Add(m_latestData.m_sellVolume5);
                    vlist.Add(m_latestData.m_sellVolume4);
                    vlist.Add(m_latestData.m_sellVolume3);
                    vlist.Add(m_latestData.m_sellVolume2);
                    vlist.Add(m_latestData.m_sellVolume1);
                    plist.Add(m_latestData.m_buyPrice1);
                    plist.Add(m_latestData.m_buyPrice2);
                    plist.Add(m_latestData.m_buyPrice3);
                    plist.Add(m_latestData.m_buyPrice4);
                    plist.Add(m_latestData.m_buyPrice5);
                    vlist.Add(m_latestData.m_buyVolume1);
                    vlist.Add(m_latestData.m_buyVolume2);
                    vlist.Add(m_latestData.m_buyVolume3);
                    vlist.Add(m_latestData.m_buyVolume4);
                    vlist.Add(m_latestData.m_buyVolume5);
                    if (m_lV2)
                    {
                        plist.Add(m_latestData.m_buyPrice6);
                        plist.Add(m_latestData.m_buyPrice7);
                        plist.Add(m_latestData.m_buyPrice8);
                        plist.Add(m_latestData.m_buyPrice9);
                        plist.Add(m_latestData.m_buyPrice10);
                        vlist.Add(m_latestData.m_buyVolume6);
                        vlist.Add(m_latestData.m_buyVolume7);
                        vlist.Add(m_latestData.m_buyVolume8);
                        vlist.Add(m_latestData.m_buyVolume9);
                        vlist.Add(m_latestData.m_buyVolume10);
                    }
                    long color = 0;
                    double max = Max(vlist);
                    font.m_fontSize = m_lV2 ? 14 : 16;
                    if (max > 0)
                    {
                        //����������
                        int pLength = plist.Count;
                        top = 32;
                        if (m_lV2)
                        {
                            color = CDraw.GetPriceColor(m_latestData.m_avgSellPrice, lastClose);
                            CDraw.DrawUnderLineNum(paint, m_latestData.m_avgSellPrice, m_digit, font, color, false, 80, top);
                            color = CDraw.GetPriceColor(0, m_latestData.m_allSellVol);
                            CDraw.DrawUnderLineNum(paint, m_latestData.m_allSellVol, 0, font, yellowColor, false, 130, top);
                            top += step;
                        }
                        for (int i = 0; i < pLength; i++)
                        {
                            color = CDraw.GetPriceColor(plist[i], lastClose);
                            CDraw.DrawUnderLineNum(paint, plist[i], m_digit, font, color, true, m_lV2 ? 80 : 60, top);
                            CDraw.DrawUnderLineNum(paint, vlist[i], 0, font, yellowColor, false, m_lV2 ? 130 : 110, top);
                            paint.FillRect(color, new RECT(width - (int)(vlist[i] / max * 50), top + step / 2 - 2, width, top + step / 2 + 2));
                            top += step;
                        }
                        if (m_lV2)
                        {
                            color = CDraw.GetPriceColor(m_latestData.m_avgBuyPrice, lastClose);
                            CDraw.DrawUnderLineNum(paint, m_latestData.m_avgBuyPrice, m_digit, font, color, false, 80, top);
                            color = CDraw.GetPriceColor(m_latestData.m_allBuyVol, 0);
                            CDraw.DrawUnderLineNum(paint, m_latestData.m_allBuyVol, 0, font, yellowColor, false, 130, top);
                            top += step;
                        }
                    }
                    vlist.Clear();
                    plist.Clear();
                    top = m_lV2 ? 390 : 232;
                    //�ɽ�
                    color = CDraw.GetPriceColor(close, lastClose);
                    CDraw.DrawUnderLineNum(paint, close, m_digit, font, color, true, 45, top);
                    //����
                    double sub = 0;
                    if (close == 0)
                    {
                        sub = m_latestData.m_buyPrice1 - lastClose;
                        double rate = 100 * (m_latestData.m_buyPrice1 - lastClose) / lastClose;
                        int pleft = CDraw.DrawUnderLineNum(paint, rate, 2, font, color, false, 45, top + 40);
                        CDraw.DrawText(paint, "%", color, font, pleft + 47, top + 40);
                    }
                    else
                    {
                        sub = close - m_latestData.m_lastClose;
                        double rate = 100 * (close - lastClose) / lastClose;
                        int pleft = CDraw.DrawUnderLineNum(paint, rate, 2, font, color, false, 45, top + 40);
                        CDraw.DrawText(paint, "%", color, font, pleft + 47, top + 40);
                    }
                    CDraw.DrawUnderLineNum(paint, sub, m_digit, font, color, false, 45, top + 20);
                    double volume = m_latestData.m_volume / 100;
                    String unit = "";
                    if (volume > 100000000)
                    {
                        volume /= 100000000;
                        unit = "��";
                    }
                    else if (volume > 10000)
                    {
                        volume /= 10000;
                        unit = "��";
                    }
                    //����
                    int cleft = CDraw.DrawUnderLineNum(paint, volume, unit.Length > 0 ? m_digit : 0, font, yellowColor, true, 45, top + 60);
                    if (unit.Length > 0)
                    {
                        CDraw.DrawText(paint, unit, yellowColor, font, cleft + 47, top + 60);
                    }
                    //����
                    double turnoverRate = m_latestData.m_turnoverRate;
                    cleft = CDraw.DrawUnderLineNum(paint, turnoverRate, 2, font, yellowColor, true, 155, top + 60);
                    if (turnoverRate > 0)
                    {
                        CDraw.DrawText(paint, "%", yellowColor, font, cleft + 157, top + 60);
                    }
                    //����
                    color = CDraw.GetPriceColor(open, lastClose);
                    CDraw.DrawUnderLineNum(paint, open, m_digit, font, color, true, 155, top);
                    //���
                    color = CDraw.GetPriceColor(high, lastClose);
                    CDraw.DrawUnderLineNum(paint, high, m_digit, font, color, true, 155, top + 20);
                    //���
                    color = CDraw.GetPriceColor(low, lastClose);
                    CDraw.DrawUnderLineNum(paint, low, m_digit, font, color, true, 155, top + 40);
                    //��ͣ
                    double upPrice = lastClose * 1.1;
                    if (m_securityName != null && m_securityName.Length > 0)
                    {
                        if (m_securityName.StartsWith("ST") || m_securityName.StartsWith("*ST"))
                        {
                            upPrice = lastClose * 1.05;
                        }
                    }
                    CDraw.DrawUnderLineNum(paint, upPrice, m_digit, font, CDraw.PCOLORS_UPCOLOR, true, 45, top + 80);
                    //��ͣ
                    double downPrice = lastClose * 0.9;
                    if (m_securityName != null && m_securityName.Length > 0)
                    {
                        if (m_securityName.StartsWith("ST") || m_securityName.StartsWith("*ST"))
                        {
                            downPrice = lastClose * 0.95;
                        }
                    }
                    CDraw.DrawUnderLineNum(paint, downPrice, m_digit, font, CDraw.PCOLORS_DOWNCOLOR, true, 155, top + 80);
                    //����
                    double outerVol = m_latestData.m_outerVol;
                    unit = "";
                    if (outerVol > 100000000)
                    {
                        outerVol /= 100000000;
                        unit = "��";
                    }
                    else if (outerVol > 10000)
                    {
                        outerVol /= 10000;
                        unit = "��";
                    }
                    cleft = CDraw.DrawUnderLineNum(paint, outerVol, unit.Length > 0 ? m_digit : 0, font, CDraw.PCOLORS_UPCOLOR, false, 45, top + 100);
                    if (unit.Length > 0)
                    {
                        CDraw.DrawText(paint, unit, CDraw.PCOLORS_UPCOLOR, font, cleft + 47, top + 100);
                    }
                    unit = "";
                    double innerVol = m_latestData.m_innerVol;
                    if (innerVol > 100000000)
                    {
                        innerVol /= 100000000;
                        unit = "��";
                    }
                    else if (innerVol > 10000)
                    {
                        innerVol /= 10000;
                        unit = "��";
                    }
                    //����
                    cleft = CDraw.DrawUnderLineNum(paint, innerVol, unit.Length > 0 ? m_digit : 0, font, CDraw.PCOLORS_DOWNCOLOR, true, 155, top + 100);
                    if (unit.Length > 0)
                    {
                        CDraw.DrawText(paint, unit, CDraw.PCOLORS_DOWNCOLOR, font, cleft + 157, top + 100);
                    }
                }
                font.m_bold = false;
                font.m_fontSize = 20;
                //��Ʊ����
                if (m_securityCode != null && m_securityCode.Length > 0)
                {
                    CDraw.DrawText(paint, m_securityCode, CDraw.PCOLORS_FORECOLOR3, font, 2, 4);
                }
                //��Ʊ����
                if (m_securityName != null && m_securityName.Length > 0)
                {         
                    CDraw.DrawText(paint, m_securityName, yellowColor, font, 100, 4);
                }
                //���߿�
                long frameColor = CDraw.PCOLORS_LINECOLOR4;
                paint.DrawLine(frameColor, 1, 0, 0, 0, 0, height);
                paint.DrawLine(frameColor, 1, 0, 0, 30, width, 30);
                paint.DrawLine(frameColor, 1, 0, 24, 30, 24, top - 2);
                paint.DrawLine(frameColor, 1, 0, 0, m_lV2 ? 208 : 130, width, m_lV2 ? 208 : 130);
                paint.DrawLine(frameColor, 1, 0, 0, top - 2, width, top - 2);
                paint.DrawLine(frameColor, 1, 0, width, 0, width, height);
                paint.DrawLine(frameColor, 1, 0, 0, top + 120, width, top + 120);
            }
        }

        /// <summary>
        /// ���²��ַ���
        /// </summary>
        public override void Update()
        {
            base.Update();
            if (m_gridTransaction != null)
            {
                int top = m_lV2 ? 510 : 352;
                m_gridTransaction.Top = top;
                int width = Width, height = Height;
                SIZE gridSize = new SIZE(width, height - top);
                if (gridSize.cy > 0)
                {
                    m_gridTransaction.Size = gridSize;
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// �ɽ���¼���ڵ�Ԫ��
    /// </summary>
    public class TransactionDateCell : GridStringCell
    {
        /// <summary>
        /// ������Ԫ��
        /// </summary>
        public TransactionDateCell()
        {
        }

        /// <summary>
        /// �ػ淽��
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="rect">����</param>
        /// <param name="clipRect">�ü�����</param>
        /// <param name="isAlternate">�Ƿ�����</param>
        public override void OnPaint(CPaint paint, RECT rect, RECT clipRect, bool isAlternate)
        {
            int clipW = clipRect.right - clipRect.left;
            int clipH = clipRect.bottom - clipRect.top;
            if (clipW > 0 && clipH > 0)
            {
                GridA grid = Grid;
                GridRow row = Row;
                GridColumn column = Column;
                if (grid != null && row != null && column != null)
                {
                    double value = GetDouble();
                    String text = Text;
                    GridCellStyle style = Style;
                    FONT font = style.Font;
                    long foreColor = style.ForeColor;
                    SIZE tSize = paint.TextSize(text, font);
                    POINT tPoint = new POINT(rect.left, rect.top + clipH / 2 - tSize.cy / 2);
                    RECT tRect = new RECT(tPoint.x, tPoint.y, tPoint.x + tSize.cx, tPoint.y + tSize.cy);
                    paint.DrawText(text, foreColor, font, tRect);
                }
            }
        }
    }

    /// <summary>
    /// �ɽ���¼�����͵�Ԫ��
    /// </summary>
    public class TransactionDataDoubleCell : GridDoubleCell
    {
        /// <summary>
        /// ������Ԫ��
        /// </summary>
        public TransactionDataDoubleCell()
        {
        }

        private int m_digit;

        /// <summary>
        /// ��ȡ��������ʾ����
        /// </summary>
        public int Digit
        {
            get { return m_digit; }
            set { m_digit = value; }
        }

        /// <summary>
        /// �ػ淽��
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="rect">����</param>
        /// <param name="clipRect">�ü�����</param>
        /// <param name="isAlternate">�Ƿ�����</param>
        public override void OnPaint(CPaint paint, RECT rect, RECT clipRect, bool isAlternate)
        {
            int clipW = clipRect.right - clipRect.left;
            int clipH = clipRect.bottom - clipRect.top;
            if (clipW > 0 && clipH > 0)
            {
                GridA grid = Grid;
                GridRow row = Row;
                GridColumn column = Column;
                if (grid != null && row != null && column != null)
                {
                    double value = GetDouble();
                    String text = " ";
                    GridCellStyle style = Style;
                    FONT font = style.Font;
                    long foreColor = style.ForeColor;
                    SIZE tSize = paint.TextSize(text, font);
                    POINT tPoint = new POINT(rect.left, rect.top + clipH / 2 - tSize.cy / 2);
                    CDraw.DrawUnderLineNum(paint, value, m_digit, font, foreColor, false, tPoint.x, tPoint.y);
                }
            }
        }
    }
}
