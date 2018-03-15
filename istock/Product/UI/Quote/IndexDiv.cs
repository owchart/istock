/*****************************************************************************\
*                                                                             *
* IndexDiv.cs - Index div functions, types, and definitions.                  *
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

namespace OwLib
{
    /// <summary>
    /// ָ����
    /// </summary>
    public class IndexDiv : ControlA
    {
        #region Lord 2016/3/21
        /// <summary>
        /// �����ؼ�
        /// </summary>
        public IndexDiv()
        {
            BackColor = CDraw.PCOLORS_BACKCOLOR4;
            BorderColor = COLOR.EMPTY;
        }

        /// <summary>
        /// ��ҵ��ָ������
        /// </summary>
        private SecurityLatestData m_cyLatestData = new SecurityLatestData();

        /// <summary>
        /// ������
        /// </summary>
        private int m_requestID = BaseService.GetRequestID();

        /// <summary>
        /// ��ָ֤������
        /// </summary>
        private SecurityLatestData m_ssLatestData = new SecurityLatestData();

        /// <summary>
        /// ��ָ֤������
        /// </summary>
        private SecurityLatestData m_szLatestData = new SecurityLatestData();

        /// <summary>
        /// ���ID
        /// </summary>
        private int m_timerID = ControlA.GetNewTimerID();

        private MainFrame m_mainFrame;

        /// <summary>
        /// ��ȡ������������
        /// </summary>
        public MainFrame MainFrame
        {
            get { return m_mainFrame; }
            set { m_mainFrame = value; }
        }

        /// <summary>
        /// �ؼ����ط���
        /// </summary>
        public override void OnLoad()
        {
            base.OnLoad();
            StartTimer(m_timerID, 1000);
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
            int width = Width;
            String code = "";
            if (mp.x < width / 3)
            {
                code = m_ssLatestData.m_code;
            }
            else if (mp.x < width * 2 / 3)
            {
                code = m_szLatestData.m_code;
            }
            else
            {
                code = m_cyLatestData.m_code;
            }
            m_mainFrame.SearchSecurity(code);
        }

        /// <summary>
        /// ����ǰ������
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="clipRect">�ü�����</param>
        public override void OnPaintForeground(CPaint paint, RECT clipRect)
        {
            RECT bounds = Bounds;
            int width = bounds.right - bounds.left;
            int height = bounds.bottom - bounds.top;
            if (width > 0 && height > 0)
            {
                if (m_ssLatestData != null && m_szLatestData != null && m_cyLatestData != null)
                {
                    long titleColor = COLOR.ARGB(255, 255, 80);
                    FONT font = new FONT("SimSun", 16, false, false, false);
                    FONT indexFont = new FONT("Arial", 14, true, false, false);
                    long grayColor = CDraw.PCOLORS_FORECOLOR4;
                    //��ָ֤��
                    long indexColor = CDraw.GetPriceColor(m_ssLatestData.m_close, m_ssLatestData.m_lastClose);
                    int left = 1;
                    CDraw.DrawText(paint, "��֤", titleColor, font, left, 3);
                    left += 40;
                    paint.DrawLine(grayColor, 1, 0, left, 0, left, height);
                    String amount = (m_ssLatestData.m_amount / 100000000).ToString("0.0") + "��";
                    SIZE amountSize = paint.TextSize(amount, indexFont);
                    CDraw.DrawText(paint, amount, titleColor, indexFont, width / 3 - amountSize.cx, 3);
                    left += (width / 3 - 40 - amountSize.cx) / 4;
                    int length = CDraw.DrawUnderLineNum(paint, m_ssLatestData.m_close, 2, indexFont, indexColor, false, left, 3);
                    left += length + (width / 3 - 40 - amountSize.cx) / 4;
                    length = CDraw.DrawUnderLineNum(paint, m_ssLatestData.m_close - m_ssLatestData.m_lastClose, 2, indexFont, indexColor, false, left, 3);
                    //��ָ֤��
                    left = width / 3;
                    paint.DrawLine(grayColor, 1, 0, left, 0, left, height);
                    indexColor = CDraw.GetPriceColor(m_szLatestData.m_close, m_szLatestData.m_lastClose);
                    CDraw.DrawText(paint, "��֤", titleColor, font, left, 3);
                    left += 40;
                    paint.DrawLine(grayColor, 1, 0, left, 0, left, height);
                    amount = (m_szLatestData.m_amount / 100000000).ToString("0.0") + "��";
                    amountSize = paint.TextSize(amount, indexFont);
                    CDraw.DrawText(paint, amount, titleColor, indexFont, width * 2 / 3 - amountSize.cx, 3);
                    left += (width / 3 - 40 - amountSize.cx) / 4;
                    length = CDraw.DrawUnderLineNum(paint, m_szLatestData.m_close, 2, indexFont, indexColor, false, left, 3);
                    left += length + (width / 3 - 40 - amountSize.cx) / 4;
                    length = CDraw.DrawUnderLineNum(paint, m_szLatestData.m_close - m_szLatestData.m_lastClose, 2, indexFont, indexColor, false, left, 3);
                    //��ҵָ��
                    left = width * 2 / 3;
                    paint.DrawLine(grayColor, 1, 0, left, 0, left, height);
                    indexColor = CDraw.GetPriceColor(m_cyLatestData.m_close, m_cyLatestData.m_lastClose);
                    CDraw.DrawText(paint, "��ҵ", titleColor, font, left, 3);
                    left += 40;
                    paint.DrawLine(grayColor, 1, 0, left, 0, left, height);
                    amount = (m_cyLatestData.m_amount / 100000000).ToString("0.0") + "��";
                    amountSize = paint.TextSize(amount, indexFont);
                    CDraw.DrawText(paint, amount, titleColor, indexFont, width - amountSize.cx, 3);
                    left += (width / 3 - 40 - amountSize.cx) / 4;
                    length = CDraw.DrawUnderLineNum(paint, m_cyLatestData.m_close, 2, indexFont, indexColor, false, left, 3);
                    left += (width / 3 - 40 - amountSize.cx) / 4 + length;
                    length = CDraw.DrawUnderLineNum(paint, m_cyLatestData.m_close - m_cyLatestData.m_lastClose, 2, indexFont, indexColor, false, left, 3);
                    paint.DrawRect(grayColor, 1, 0, new RECT(0, 0, width - 1, height - 1));
                }
            }
        }

        /// <summary>
        /// �����
        /// </summary>
        /// <param name="timerID">���ID</param>
        public override void OnTimer(int timerID)
        {
            if (m_timerID == timerID)
            {
                SecurityService.GetLatestData("000001.SH", ref m_ssLatestData);
                SecurityService.GetLatestData("399001.SZ", ref m_szLatestData);
                SecurityService.GetLatestData("399006.SZ", ref m_cyLatestData);
                Invalidate();
            }
        }
        #endregion
    }
}
