/*****************************************************************************\
*                                                                             *
* Bar.cs -      Bar functions, types, and definitions                         *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Runtime.Serialization;
using System.Security.Permissions;
namespace OwLib
{
    /// <summary>
    /// ��״ͼ
    /// </summary>
    [Serializable]
    public class Bar
    {
        #region �յ� 2016/5/31
        /// <summary>
        /// ������״ͼ
        /// </summary>
        public Bar()
            : this(Color.Empty)
        {
        }

        /// <summary>
        /// ������״ͼ
        /// </summary>
        /// <param name="color">��ɫ</param>
        public Bar(Color color)
        {
            m_border = new Border(Default.IsBorderVisible, Default.BorderColor, Default.BorderWidth);
            m_fill = new Fill(color.IsEmpty ? Default.FillColor : color,
                                    Default.FillBrush, Default.FillType);
        }

        /// <summary>
        /// ������״ͼ
        /// </summary>
        /// <param name="rhs">������״ͼ</param>
        public Bar(Bar rhs)
        {
            m_border = new Border(rhs.Border);
            m_fill = new Fill(rhs.Fill);
        }

        /// <summary>
        /// Ĭ������
        /// </summary>
        public struct Default
        {
            public static float BorderWidth = 1.0F;
            public static FillType FillType = FillType.Brush;
            public static bool IsBorderVisible = true;
            public static Color BorderColor = Color.FromArgb(30, 30, 30);
            public static Color FillColor = Color.Red;
            public static Brush FillBrush = null;
        }

        private Fill m_fill;

        /// <summary>
        /// ��ȡ���������
        /// </summary>
        public Fill Fill
        {
            get { return m_fill; }
            set { m_fill = value; }
        }

        private Border m_border;

        /// <summary>
        /// ��ȡ�����ñ���
        /// </summary>
        public Border Border
        {
            get { return m_border; }
            set { m_border = value; }
        }

        /// <summary>
        /// ��ͼ����
        /// </summary>
        /// <param name="g">��ͼ����</param>
        /// <param name="pane">ͼ��</param>
        /// <param name="left">������</param>
        /// <param name="right">�Ҳ����</param>
        /// <param name="top">�ϲ�����</param>
        /// <param name="bottom">�ײ�����</param>
        /// <param name="scaleFactor">�̶�����</param>
        /// <param name="fullFrame">�Ƿ�ȫ���</param>
        /// <param name="isSelected">�Ƿ�ѡ��</param>
        /// <param name="dataValue">��ֵ</param>
        public void Draw(Graphics g, GraphPane pane, float left, float right, float top,
                            float bottom, float scaleFactor, bool fullFrame, bool isSelected,
                            PointPair dataValue)
        {
            if (top > bottom)
            {
                float junk = top;
                top = bottom;
                bottom = junk;
            }
            if (left > right)
            {
                float junk = right;
                right = left;
                left = junk;
            }
            if (top < -10000)
                top = -10000;
            else if (top > 10000)
                top = 10000;
            if (left < -10000)
                left = -10000;
            else if (left > 10000)
                left = 10000;
            if (right < -10000)
                right = -10000;
            else if (right > 10000)
                right = 10000;
            if (bottom < -10000)
                bottom = -10000;
            else if (bottom > 10000)
                bottom = 10000;
            RectangleF rect = new RectangleF(left, top, right - left, bottom - top);
            Draw(g, pane, rect, scaleFactor, fullFrame, isSelected, dataValue);
        }

        /// <summary>
        /// ��ͼ����
        /// </summary>
        /// <param name="g">��ͼ����</param>
        /// <param name="pane">ͼ��</param>
        /// <param name="rect">����</param>
        /// <param name="scaleFactor">�̶�����</param>
        /// <param name="fullFrame">�Ƿ�ȫ���</param>
        /// <param name="isSelected">�Ƿ�ѡ��</param>
        /// <param name="dataValue">��ֵ</param>
        public void Draw(Graphics g, GraphPane pane, RectangleF rect, float scaleFactor,
                            bool fullFrame, bool isSelected, PointPair dataValue)
        {
            if (isSelected)
            {
                Selection.Fill.Draw(g, rect, dataValue);
            }
            else
            {
                m_fill.Draw(g, rect, dataValue);
            }
        }

        /// <summary>
        /// ������״ͼ
        /// </summary>
        /// <param name="g">��ͼ����</param>
        /// <param name="pane">ͼ��</param>
        /// <param name="curve">��</param>
        /// <param name="baseAxis">��������</param>
        /// <param name="valueAxis">��ֵ����</param>
        /// <param name="barWidth">��</param>
        /// <param name="pos">λ��</param>
        /// <param name="scaleFactor">�̶�����</param>
        public void DrawBars(Graphics g, GraphPane pane, CurveItem curve,
                                Axis baseAxis, Axis valueAxis,
                                float barWidth, int pos, float scaleFactor)
        {
            BarType barType = pane.m_barSettings.Type;
            if (barType == BarType.Overlay || barType == BarType.Stack || barType == BarType.PercentStack ||
                    barType == BarType.SortedOverlay)
                pos = 0;
            for (int i = 0; i < curve.Points.Count; i++)
                DrawSingleBar(g, pane, curve, i, pos, baseAxis, valueAxis, barWidth, scaleFactor);
        }

        /// <summary>
        /// ���Ƶ�����״ͼ
        /// </summary>
        /// <param name="g">��ͼ����</param>
        /// <param name="pane">ͼ��</param>
        /// <param name="curve">��</param>
        /// <param name="baseAxis">��������</param>
        /// <param name="valueAxis">��ֵ����</param>
        /// <param name="pos">λ��</param>
        /// <param name="index">����</param>
        /// <param name="barWidth">���</param>
        /// <param name="scaleFactor">�̶�����</param>
        public void DrawSingleBar(Graphics g, GraphPane pane, CurveItem curve,
                                    Axis baseAxis, Axis valueAxis,
                                    int pos, int index, float barWidth, float scaleFactor)
        {
            if (index >= curve.Points.Count)
                return;
            if (pane.m_barSettings.Type == BarType.Overlay || pane.m_barSettings.Type == BarType.Stack ||
                    pane.m_barSettings.Type == BarType.PercentStack)
                pos = 0;
            DrawSingleBar(g, pane, curve, index, pos, baseAxis, valueAxis, barWidth, scaleFactor);
        }

        /// <summary>
        /// ���Ƶ�����״ͼ
        /// </summary>
        /// <param name="g">��ͼ����</param>
        /// <param name="pane">ͼ��</param>
        /// <param name="curve">��</param>
        /// <param name="index">����</param>
        /// <param name="pos">λ��</param>
        /// <param name="baseAxis">��������</param>
        /// <param name="valueAxis">��ֵ����</param>
        /// <param name="barWidth">���</param>
        /// <param name="scaleFactor">�̶�����</param>
        protected virtual void DrawSingleBar(Graphics g, GraphPane pane,
                                        CurveItem curve,
                                        int index, int pos, Axis baseAxis, Axis valueAxis,
                                        float barWidth, float scaleFactor)
        {
            float pixBase, pixHiVal, pixLowVal;
            float clusterWidth = pane.BarSettings.GetClusterWidth();
            float clusterGap = pane.m_barSettings.MinClusterGap * barWidth;
            float barGap = barWidth * pane.m_barSettings.MinBarGap;
            if (curve.Points.Count == 1)
            {
                clusterWidth = 100;
                clusterGap = 0;
            }
            double curBase, curLowVal, curHiVal;
            ValueHandler valueHandler = new ValueHandler(pane, false);
            valueHandler.GetValues(curve, index, out curBase, out curLowVal, out curHiVal);
            if (!curve.Points[index].IsInvalid)
            {
                pixLowVal = valueAxis.Scale.Transform(curve.IsOverrideOrdinal, index, curLowVal);
                pixHiVal = valueAxis.Scale.Transform(curve.IsOverrideOrdinal, index, curHiVal);
                pixBase = baseAxis.Scale.Transform(curve.IsOverrideOrdinal, index, curBase);
                float pixSide = pixBase - clusterWidth / 2.0F + clusterGap / 2.0F +
                                pos * (barWidth + barGap);
                if (pane.m_barSettings.Base == BarBase.X)
                    this.Draw(g, pane, pixSide, pixSide + barWidth, pixLowVal,
                            pixHiVal, scaleFactor, true, curve.IsSelected,
                            curve.Points[index]);
                else
                    this.Draw(g, pane, pixLowVal, pixHiVal, pixSide, pixSide + barWidth,
                            scaleFactor, true, curve.IsSelected,
                            curve.Points[index]);
            }
        }
        #endregion
    }
}
