/*****************************************************************************\
*                                                                             *
* FloatDiv.cs - Float div functions, types, and definitions.                  *
*                                                                             *
*               Version 1.00  ����                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/3/22.                                    *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    /// <summary>
    /// ������ָ��
    /// </summary>
    public class FloatDiv : ControlA
    {
        #region Lord 2016/3/22
        /// <summary>
        /// �����ؼ�
        /// </summary>
        public FloatDiv()
        {
            BackColor = CDraw.PCOLORS_BACKCOLOR3;
            BorderColor = CDraw.PCOLORS_LINECOLOR4;
            Cursor = CursorsA.SizeAll;
        }

        private ChartEx m_chart;

        /// <summary>
        /// ��ȡ������
        /// </summary>
        public ChartEx Chart
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

        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="click">�������</param>
        /// <param name="delta">���ֹ���ֵ</param>
        public override void OnClick(POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            base.OnClick(mp, button, clicks, delta);
            if (button == MouseButtonsA.Left && clicks == 1)
            {
                int width = Width;
                if (mp.x >= width - 14 && mp.y <= 14)
                {
                    Visible = false;
                    Native.Invalidate();
                }
            }
        }

        /// <summary>
        /// ����ƶ�����
        /// </summary>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="click">�������</param>
        /// <param name="delta">���ֹ���ֵ</param>
        public override void OnMouseMove(POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            base.OnMouseMove(mp, button, clicks, delta);
            int width = Width;
            if (mp.x >= width - 14 && mp.y <= 14)
            {
                Cursor = CursorsA.Arrow;
            }
            else
            {
                Cursor = CursorsA.SizeAll;
            }
            Invalidate();
        }

        /// <summary>
        /// �ػ汳������
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="clipRect">�ü�����</param>
        public override void OnPaintBackground(CPaint paint, RECT clipRect)
        {
            int width = Width;
            int height = Height;
            if (width > 0 && height > 0)
            {
                ChartA chart = m_chart.Chart;
                //ʮ���߳���ʱ���л���
                if (chart.ShowCrossLine)
                {
                    CTable dataSource = chart.DataSource;
                    //��ȡ���ͣ������
                    int crossStopIndex = chart.CrossStopIndex;
                    if (dataSource.RowsCount > 0)
                    {
                        if (crossStopIndex < 0)
                        {
                            crossStopIndex = chart.FirstVisibleIndex;
                        }
                        if (crossStopIndex > chart.LastVisibleIndex)
                        {
                            crossStopIndex = chart.LastVisibleIndex;
                        }
                    }
                    else
                    {
                        crossStopIndex = -1;
                    }
                    //��ȡK�ߺͳɽ���
                    RECT rectangle = new RECT(0, 0, width, height);
                    long win32Color = COLOR.EMPTY;
                    paint.FillRect(GetPaintingBackColor(), rectangle);
                    paint.DrawRect(GetPaintingBorderColor(), 1, 0, rectangle);
                    //���رհ�ť
                    long lineColor = CDraw.PCOLORS_LINECOLOR;
                    paint.DrawLine(lineColor, 2, 0, width - 6, 4, width - 14, 12);
                    paint.DrawLine(lineColor, 2, 0, width - 6, 12, width - 14, 4);
                    //��������
                    FONT font = new FONT("SimSun", 14, false, false, false);
                    FONT lfont = new FONT("Arial", 12, false, false, false);
                    FONT nfont = new FONT("Arial", 14, true, false, false);
                    //������
                    CDraw.DrawText(paint, "ʱ ��", CDraw.PCOLORS_FORECOLOR4, font, rectangle.left + 25, rectangle.top + 2);
                    DateTime date = DateTime.Now;
                    if (crossStopIndex >= 0)
                    {
                        double dateNum = dataSource.GetXValue(crossStopIndex);
                        if (dateNum != 0)
                        {
                            date = CStr.ConvertNumToDate(dateNum);
                        }
                        String dateStr = "";
                        int cycle = m_chart.Cycle;
                        if (cycle <= 1)
                        {
                            dateStr = date.ToString("hh:mm");
                        }
                        else if (cycle >= 5 && cycle <= 60)
                        {
                            dateStr = date.ToString("MM-dd hh:mm");
                        }
                        else
                        {
                            dateStr = date.ToString("yyyy-MM-dd");
                        }
                        SIZE dtSize = paint.TextSize(dateStr, lfont);
                        CDraw.DrawText(paint, dateStr, CDraw.PCOLORS_FORECOLOR3,
                        lfont, rectangle.left + width / 2 - dtSize.cx / 2, rectangle.top + 20);
                        //��ȡֵ
                        double close = 0, high = 0, low = 0, open = 0, amount = 0;
                        if (crossStopIndex >= 0)
                        {
                            close = dataSource.Get2(crossStopIndex, KeyFields.CLOSE_INDEX);
                            high = dataSource.Get2(crossStopIndex, KeyFields.HIGH_INDEX);
                            low = dataSource.Get2(crossStopIndex, KeyFields.LOW_INDEX);
                            open = dataSource.Get2(crossStopIndex, KeyFields.OPEN_INDEX);
                            amount = dataSource.Get2(crossStopIndex, KeyFields.AMOUNT_INDEX);
                        }
                        if (double.IsNaN(close))
                        {
                            close = 0;
                        }
                        if (double.IsNaN(high))
                        {
                            high = 0;
                        }
                        if (double.IsNaN(low))
                        {
                            low = 0;
                        }
                        if (double.IsNaN(open))
                        {
                            open = 0;
                        }
                        if (double.IsNaN(amount))
                        {
                            amount = 0;
                        }
                        double rate = 1;
                        double lastClose = 0;
                        if (crossStopIndex > 1)
                        {
                            lastClose = dataSource.Get2(crossStopIndex - 1, KeyFields.CLOSE_INDEX);
                            if (cycle == 0)
                            {
                                lastClose = m_chart.LatestData.m_lastClose;
                            }
                            if (!double.IsNaN(lastClose))
                            {
                                if (lastClose != 0)
                                {
                                    rate = (close - lastClose) / lastClose;
                                }
                            }
                        }
                        //���̼�
                        String openStr = double.IsNaN(open) ? "" : CStr.GetValueByDigit(open, m_digit).ToString();
                        SIZE tSize = paint.TextSize(openStr, nfont);
                        CDraw.DrawText(paint, openStr, CDraw.GetPriceColor(open, lastClose), nfont, rectangle.left + width / 2 - tSize.cx / 2, rectangle.top + 60);
                        //��߼�
                        String highStr = double.IsNaN(high) ? "" : CStr.GetValueByDigit(high, m_digit).ToString();
                        tSize = paint.TextSize(highStr, nfont);
                        CDraw.DrawText(paint, highStr, CDraw.GetPriceColor(high, lastClose), nfont, rectangle.left + width / 2 - tSize.cx / 2, rectangle.top + 100);
                        //��ͼ�
                        String lowStr = double.IsNaN(low) ? "" : CStr.GetValueByDigit(low, m_digit).ToString();
                        tSize = paint.TextSize(lowStr, nfont);
                        CDraw.DrawText(paint, lowStr, CDraw.GetPriceColor(low, lastClose), nfont, rectangle.left + width / 2 - tSize.cx / 2, rectangle.top + 140);
                        //��ͼ�
                        String closeStr = double.IsNaN(close) ? "" : CStr.GetValueByDigit(close, m_digit).ToString();
                        tSize = paint.TextSize(closeStr, nfont);
                        CDraw.DrawText(paint, closeStr, CDraw.GetPriceColor(close, lastClose), nfont, rectangle.left + width / 2 - tSize.cx / 2, rectangle.top + 180);
                        //�ɽ���
                        String unit = "";
                        if (amount > 100000000)
                        {
                            amount /= 100000000;
                            unit = "��";
                        }
                        else if (amount > 10000)
                        {
                            amount /= 10000;
                            unit = "��";
                        }
                        String amountStr = CStr.GetValueByDigit(amount, 2) + unit;
                        tSize = paint.TextSize(amountStr, lfont);
                        CDraw.DrawText(paint, amountStr, CDraw.PCOLORS_FORECOLOR3, lfont, rectangle.left + width / 2 - tSize.cx / 2, rectangle.top + 220);
                        //�Ƿ�
                        String rangeStr = double.IsNaN(rate) ? "0.00%" : rate.ToString("0.00%");
                        tSize = paint.TextSize(rangeStr, nfont);
                        CDraw.DrawText(paint, rangeStr, CDraw.GetPriceColor(close, lastClose), lfont, rectangle.left + width / 2 - tSize.cx / 2, rectangle.top + 260);
                    }
                    long whiteColor = CDraw.PCOLORS_FORECOLOR4;
                    CDraw.DrawText(paint, "�� ��", whiteColor, font, rectangle.left + 25, rectangle.top + 40);
                    CDraw.DrawText(paint, "�� ��", whiteColor, font, rectangle.left + 25, rectangle.top + 80);
                    CDraw.DrawText(paint, "�� ��", whiteColor, font, rectangle.left + 25, rectangle.top + 120);
                    CDraw.DrawText(paint, "�� ��", whiteColor, font, rectangle.left + 25, rectangle.top + 160);
                    CDraw.DrawText(paint, "�� ��", whiteColor, font, rectangle.left + 25, rectangle.top + 200);
                    CDraw.DrawText(paint, "�� ��", whiteColor, font, rectangle.left + 25, rectangle.top + 240);
                }
            }
        }

        /// <summary>
        /// �ػ���߷���
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="clipRect">�ü�����</param>
        public override void OnPaintBorder(CPaint paint, RECT clipRect)
        {
        }
        #endregion
    }
}
