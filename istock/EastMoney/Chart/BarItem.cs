/*****************************************************************************\
*                                                                             *
* BarItem.cs -  Bar item  functions, types, and definitions                   *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OwLib
{
    /// <summary>
    /// ��״ͼ��
    /// </summary>
    [Serializable]
    public class BarItem : CurveItem
    {
        #region �յ� 2016/5/31
        /// <summary>
        /// ������״ͼ��
        /// </summary>
        /// <param name="label">��ǩ</param>
        public BarItem(String label)
            : base(label)
        {
            m_bar = new Bar();
        }

        /// <summary>
        /// ������״ͼ��
        /// </summary>
        /// <param name="label">��ǩ</param>
        /// <param name="x">X����ֵ</param>
        /// <param name="y">Y����ֵ</param>
        /// <param name="color">��ɫ</param>
        public BarItem(String label, double[] x, double[] y, Color color)
            : this(label, new PointPairList(x, y), color)
        {
        }

        /// <summary>
        /// ������״ͼ��
        /// </summary>
        /// <param name="label">��ǩ</param>
        /// <param name="points">��ֵ</param>
        /// <param name="color">��ɫ</param>
        public BarItem(String label, IPointList points, Color color)
            : base(label, points)
        {
            m_bar = new Bar(color);
        }

        protected Bar m_bar;

        /// <summary>
        /// ��ȡ��������״ͼ
        /// </summary>
        public Bar Bar
        {
            get { return m_bar; }
        }

        /// <summary>
        /// ������ǩ
        /// </summary>
        /// <param name="pane">ͼ��</param>
        /// <param name="isBarCenter">�Ƿ����м�</param>
        /// <param name="valueFormat">��ʽ��</param>
        public static void CreateBarLabels(GraphPane pane, bool isBarCenter, String valueFormat)
        {
            CreateBarLabels(pane, isBarCenter, valueFormat, TextObj.Default.FontFamily,
                    TextObj.Default.FontSize, TextObj.Default.FontColor, TextObj.Default.FontBold,
                    TextObj.Default.FontItalic, TextObj.Default.FontUnderline);
        }

        /// <summary>
        /// ������ǩ
        /// </summary>
        /// <param name="pane">ͼ��</param>
        /// <param name="isBarCenter">�Ƿ����м�</param>
        /// <param name="valueFormat">��ʽ��</param>
        /// <param name="fontFamily">����</param>
        /// <param name="fontSize">���ִ�С</param>
        /// <param name="fontColor">������ɫ</param>
        /// <param name="isBold">�Ƿ����</param>
        /// <param name="isItalic">�Ƿ�б��</param>
        /// <param name="isUnderline">�Ƿ��»���</param>
        public static void CreateBarLabels(GraphPane pane, bool isBarCenter, String valueFormat,
            String fontFamily, float fontSize, Color fontColor, bool isBold, bool isItalic,
            bool isUnderline)
        {
            bool isVertical = pane.BarSettings.Base == BarBase.X;
            int curveIndex = 0;
            ValueHandler valueHandler = new ValueHandler(pane, true);
            foreach (CurveItem curve in pane.CurveList)
            {
                BarItem bar = curve as BarItem;
                if (bar != null)
                {
                    IPointList points = curve.Points;
                    float labelOffset;
                    Scale scale = curve.ValueAxis(pane).Scale;
                    labelOffset = (float)(scale.m_max - scale.m_min) * 0.015f;
                    for (int i = 0; i < points.Count; i++)
                    {
                        double baseVal, lowVal, hiVal;
                        valueHandler.GetValues(curve, i, out baseVal, out lowVal, out hiVal);
                        float centerVal = (float)valueHandler.BarCenterValue(bar,
                            bar.GetBarWidth(pane), i, baseVal, curveIndex);
                        String barLabelText = (isVertical ? points[i].Y : points[i].X).ToString(valueFormat);
                        float position;
                        if (isBarCenter)
                            position = (float)(hiVal + lowVal) / 2.0f;
                        else if (hiVal >= 0)
                            position = (float)hiVal + labelOffset;
                        else
                            position = (float)hiVal - labelOffset;
                        TextObj label;
                        if (isVertical)
                            label = new TextObj(barLabelText, centerVal, position);
                        else
                            label = new TextObj(barLabelText, position, centerVal);
                        label.FontSpec.Family = fontFamily;
                        label.Location.CoordinateFrame =
                            (isVertical && curve.IsY2Axis) ? CoordType.AxisXY2Scale : CoordType.AxisXYScale;
                        label.FontSpec.Size = fontSize;
                        label.FontSpec.FontColor = fontColor;
                        label.FontSpec.IsItalic = isItalic;
                        label.FontSpec.IsBold = isBold;
                        label.FontSpec.IsUnderline = isUnderline;
                        label.FontSpec.Angle = isVertical ? 90 : 0;
                        label.Location.AlignH = isBarCenter ? AlignH.Center :
                                    (hiVal >= 0 ? AlignH.Left : AlignH.Right);
                        label.Location.AlignV = AlignV.Center;
                        label.FontSpec.Border.IsVisible = false;
                        label.FontSpec.Fill.IsVisible = false;
                        pane.GraphObjList.Add(label);
                    }
                    curveIndex++;
                }
            }
        }

        /// <summary>
        /// ��ͼ����
        /// </summary>
        /// <param name="g">��ͼ����</param>
        /// <param name="pane">ͼ��</param>
        /// <param name="pos">λ��</param>
        /// <param name="scaleFactor">�̶�����</param>
        public override void Draw(Graphics g, GraphPane pane, int pos,
                                    float scaleFactor)
        {
            if (m_isVisible)
                m_bar.DrawBars(g, pane, this, BaseAxis(pane), ValueAxis(pane),
                                this.GetBarWidth(pane), pos, scaleFactor);
        }

        /// <summary>
        /// ����ͼ��
        /// </summary>
        /// <param name="g">��ͼ����</param>
        /// <param name="pane">ͼ��</param>
        /// <param name="rect">����</param>
        /// <param name="scaleFactor">�̶�����</param>
        public override void DrawLegendKey(Graphics g, GraphPane pane, RectangleF rect, float scaleFactor)
        {
            m_bar.Draw(g, pane, rect, scaleFactor, true, false, null);
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="pane">ͼ��</param>
        /// <param name="i">����</param>
        /// <param name="coords">����</param>
        /// <returns>�Ƿ��ȡ�ɹ�</returns>
        public override bool GetCoords(GraphPane pane, int i, out String coords)
        {
            coords = String.Empty;
            if (i < 0 || i >= m_points.Count)
                return false;
            Axis valueAxis = ValueAxis(pane);
            Axis baseAxis = BaseAxis(pane);
            float pixBase, pixHiVal, pixLowVal;
            float clusterWidth = pane.BarSettings.GetClusterWidth();
            float barWidth = GetBarWidth(pane);
            float clusterGap = pane.m_barSettings.MinClusterGap * barWidth;
            float barGap = barWidth * pane.m_barSettings.MinBarGap;
            double curBase, curLowVal, curHiVal;
            ValueHandler valueHandler = new ValueHandler(pane, false);
            valueHandler.GetValues(this, i, out curBase, out curLowVal, out curHiVal);
            if (!m_points[i].IsInvalid3D)
            {
                pixLowVal = valueAxis.Scale.Transform(m_isOverrideOrdinal, i, curLowVal);
                pixHiVal = valueAxis.Scale.Transform(m_isOverrideOrdinal, i, curHiVal);
                pixBase = baseAxis.Scale.Transform(m_isOverrideOrdinal, i, curBase);
                float pixSide = pixBase - clusterWidth / 2.0F + clusterGap / 2.0F +
                                pane.CurveList.GetBarItemPos(pane, this) * (barWidth + barGap);
                if (baseAxis is XAxis)
                    coords = String.Format("{0:f0},{1:f0},{2:f0},{3:f0}",
                                pixSide, pixLowVal,
                                pixSide + barWidth, pixHiVal);
                else
                    coords = String.Format("{0:f0},{1:f0},{2:f0},{3:f0}",
                                pixLowVal, pixSide,
                                pixHiVal, pixSide + barWidth);
                return true;
            }
            return false;
        }

        /// <summary>
        /// �Ƿ�X�����
        /// </summary>
        /// <param name="pane">ͼ��</param>
        /// <returns>�Ƿ����</returns>
        internal override bool IsXIndependent(GraphPane pane)
        {
            return pane.m_barSettings.Base == BarBase.X;
        }
        #endregion
    }
}
