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
    /// 柱状图项
    /// </summary>
    [Serializable]
    public class BarItem : CurveItem
    {
        #region 陶德 2016/5/31
        /// <summary>
        /// 创建柱状图项
        /// </summary>
        /// <param name="label">标签</param>
        public BarItem(String label)
            : base(label)
        {
            m_bar = new Bar();
        }

        /// <summary>
        /// 创建柱状图项
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="x">X轴数值</param>
        /// <param name="y">Y轴数值</param>
        /// <param name="color">颜色</param>
        public BarItem(String label, double[] x, double[] y, Color color)
            : this(label, new PointPairList(x, y), color)
        {
        }

        /// <summary>
        /// 创建柱状图项
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="points">数值</param>
        /// <param name="color">颜色</param>
        public BarItem(String label, IPointList points, Color color)
            : base(label, points)
        {
            m_bar = new Bar(color);
        }

        protected Bar m_bar;

        /// <summary>
        /// 获取或设置柱状图
        /// </summary>
        public Bar Bar
        {
            get { return m_bar; }
        }

        /// <summary>
        /// 创建标签
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="isBarCenter">是否在中间</param>
        /// <param name="valueFormat">格式化</param>
        public static void CreateBarLabels(GraphPane pane, bool isBarCenter, String valueFormat)
        {
            CreateBarLabels(pane, isBarCenter, valueFormat, TextObj.Default.FontFamily,
                    TextObj.Default.FontSize, TextObj.Default.FontColor, TextObj.Default.FontBold,
                    TextObj.Default.FontItalic, TextObj.Default.FontUnderline);
        }

        /// <summary>
        /// 创建标签
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="isBarCenter">是否在中间</param>
        /// <param name="valueFormat">格式化</param>
        /// <param name="fontFamily">字体</param>
        /// <param name="fontSize">文字大小</param>
        /// <param name="fontColor">文字颜色</param>
        /// <param name="isBold">是否粗体</param>
        /// <param name="isItalic">是否斜体</param>
        /// <param name="isUnderline">是否下划线</param>
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
        /// 绘图方法
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="pos">位置</param>
        /// <param name="scaleFactor">刻度因子</param>
        public override void Draw(Graphics g, GraphPane pane, int pos,
                                    float scaleFactor)
        {
            if (m_isVisible)
                m_bar.DrawBars(g, pane, this, BaseAxis(pane), ValueAxis(pane),
                                this.GetBarWidth(pane), pos, scaleFactor);
        }

        /// <summary>
        /// 绘制图例
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="rect">区域</param>
        /// <param name="scaleFactor">刻度因子</param>
        public override void DrawLegendKey(Graphics g, GraphPane pane, RectangleF rect, float scaleFactor)
        {
            m_bar.Draw(g, pane, rect, scaleFactor, true, false, null);
        }

        /// <summary>
        /// 获取坐标
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="i">索引</param>
        /// <param name="coords">坐标</param>
        /// <returns>是否获取成功</returns>
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
        /// 是否X轴独立
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns>是否独立</returns>
        internal override bool IsXIndependent(GraphPane pane)
        {
            return pane.m_barSettings.Base == BarBase.X;
        }
        #endregion
    }
}
