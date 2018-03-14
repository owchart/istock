/*****************************************************************************\
*                                                                             *
* PieItem.cs -   PieItem functions, types, and definitions                    *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using System.Drawing;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Collections.Generic;
namespace OwLib
{
    /// <summary>
    /// 饼图切片
    /// </summary>
    [Serializable]
    public class PieItem : CurveItem
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建饼图切片
        /// </summary>
        /// <param name="pieValue">数值</param>
        /// <param name="color1">颜色1</param>
        /// <param name="color2">颜色2</param>
        /// <param name="fillAngle">填充角度</param>
        /// <param name="displacement">位移</param>
        /// <param name="label">标签</param>
        public PieItem(double pieValue, Color color1, Color color2, float fillAngle,
                        double displacement, String label)
            :
                        this(pieValue, color1, displacement, label)
        {
            if (!color1.IsEmpty && !color2.IsEmpty)
                m_fill = new Fill(color1, color2, fillAngle);
        }

        /// <summary>
        /// 创建饼图切片
        /// </summary>
        /// <param name="pieValue">数值</param>
        /// <param name="color">颜色</param>
        /// <param name="displacement">位移</param>
        /// <param name="label">标签</param>
        public PieItem(double pieValue, Color color, double displacement, String label)
            : base(label)
        {
            m_pieValue = pieValue;
            m_fill = new Fill(color.IsEmpty ? m_rotator.NextColor : color);
            m_displacement = displacement;
            m_border = new Border(Default.BorderColor, Default.BorderWidth);
            m_labelDetail = new TextObj();
            m_labelDetail.FontSpec.Size = Default.FontSize;
            m_labelType = Default.LabelType;
            m_valueDecimalDigits = Default.ValueDecimalDigits;
            m_percentDecimalDigits = Default.PercentDecimalDigits;
            m_slicePath = null;
        }

        /// <summary>
        /// 创建饼图切片
        /// </summary>
        /// <param name="pieValue">数值</param>
        /// <param name="label">标签</param>
        public PieItem(double pieValue, String label)
            :
            this(pieValue, m_rotator.NextColor, Default.Displacement, label)
        {
        }

        /// <summary>
        /// 创建饼图切片
        /// </summary>
        /// <param name="rhs">其他对象</param>
        public PieItem(PieItem rhs)
            : base(rhs)
        {
            m_pieValue = rhs.m_pieValue;
            m_fill = new Fill(rhs.m_fill);
            this.Border = new Border(rhs.m_border);
            m_displacement = rhs.m_displacement;
            m_labelDetail = new TextObj(rhs.m_labelDetail);
            m_labelType = rhs.m_labelType;
            m_valueDecimalDigits = rhs.m_valueDecimalDigits;
            m_percentDecimalDigits = rhs.m_percentDecimalDigits;
        }

        /// <summary>
        /// 默认属性
        /// </summary>
        public struct Default
        {
            public static double Displacement = 0;
            public static float BorderWidth = 1.0F;
            public static FillType FillType = FillType.Brush;
            public static bool IsBorderVisible = true;
            public static Color BorderColor = Color.FromArgb(30, 30, 30);
            public static Color FillColor = Color.Red;
            public static Brush FillBrush = null;
            public static bool isVisible = true;
            public static PieLabelType LabelType = PieLabelType.Name;
            public static float FontSize = 10;
            public static int ValueDecimalDigits = 0;
            public static int PercentDecimalDigits = 2;
        }

        private PointF m_beginPoint;
        private RectangleF m_boundingRectangle;
        private PointF m_endPoint;
        public int Index;
        private PointF m_intersectionPoint;
        public RectangleF m_labelRect;
        private String m_labelStr;
        private PointF m_pivotPoint;
        private static ColorSymbolRotator m_rotator = new ColorSymbolRotator();
        public bool IsLeftSlice;

        private Border m_border;

        /// <summary>
        /// 
        /// </summary>
        public Border Border
        {
            get { return (m_border); }
            set { m_border = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public new Color Color
        {
            get { return m_fill.Color; }
            set { m_fill.Color = value; }
        }

        private double m_displacement;

        /// <summary>
        /// 
        /// </summary>
        public double Displacement
        {
            get { return (m_displacement); }
            set { m_displacement = value > .5 ? .5 : value; }
        }

        private Fill m_fill;

        /// <summary>
        /// 
        /// </summary>
        public Fill Fill
        {
            get { return m_fill; }
            set { m_fill = value; }
        }

        private TextObj m_labelDetail;

        /// <summary>
        /// 
        /// </summary>
        public TextObj LabelDetail
        {
            get { return m_labelDetail; }
            set { m_labelDetail = value; }
        }

        private PieLabelType m_labelType;

        /// <summary>
        /// 
        /// </summary>
        public PieLabelType LabelType
        {
            get { return (m_labelType); }
            set
            {
                m_labelType = value;
                if (value == PieLabelType.None)
                    this.LabelDetail.IsVisible = false;
                else
                    this.LabelDetail.IsVisible = true;
            }
        }

        private float m_midAngle;

        /// <summary>
        /// 
        /// </summary>
        private float MidAngle
        {
            get { return (m_midAngle); }
            set { m_midAngle = value; }
        }

        private int m_percentDecimalDigits;

        /// <summary>
        /// 
        /// </summary>
        public int PercentDecimalDigits
        {
            get { return (m_percentDecimalDigits); }
            set { m_percentDecimalDigits = value; }
        }

        private Pie pie;

        /// <summary>
        /// 
        /// </summary>
        public Pie Pie
        {
            get { return pie; }
            set { pie = value; }
        }

        private GraphicsPath m_slicePath;

        /// <summary>
        /// 
        /// </summary>
        public GraphicsPath SlicePath
        {
            get { return m_slicePath; }
        }

        private float m_startAngle;

        /// <summary>
        /// 
        /// </summary>
        private float StartAngle
        {
            get { return (m_startAngle); }
            set { m_startAngle = value; }
        }

        private float m_sweepAngle;

        /// <summary>
        /// 
        /// </summary>
        private float SweepAngle
        {
            get { return m_sweepAngle; }
            set { m_sweepAngle = value; }
        }


        private double m_pieValue;

        /// <summary>
        /// 
        /// </summary>
        public double Value
        {
            get { return (m_pieValue); }
            set { m_pieValue = value > 0 ? value : 0; }
        }

        private int m_valueDecimalDigits;

        /// <summary>
        /// 
        /// </summary>
        public int ValueDecimalDigits
        {
            get { return (m_valueDecimalDigits); }
            set { m_valueDecimalDigits = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="curve">图形</param>
        /// <param name="value1">数值</param>
        /// <returns></returns>
        private static int BuildLabelString(PieItem curve, int value1)
        {
            NumberFormatInfo labelFormat = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
            labelFormat.NumberDecimalDigits = curve.m_valueDecimalDigits;
            labelFormat.PercentPositivePattern = 1;
            labelFormat.PercentDecimalDigits = curve.m_percentDecimalDigits;
            double value;
            switch (curve.m_labelType)
            {
                case PieLabelType.Value:
                    curve.m_labelStr = curve.m_pieValue.ToString("F", labelFormat);
                    break;
                case PieLabelType.Percent:
                    curve.m_labelStr = (curve.m_sweepAngle / 360).ToString("P", labelFormat);
                    break;
                case PieLabelType.Name_Value:
                    curve.m_labelStr = curve.m_label.m_text + ": " + curve.m_pieValue.ToString("F", labelFormat);
                    break;
                case PieLabelType.Name_Percent:
                    if (value1 < 0)
                    {
                        value = curve.m_sweepAngle / 360;
                    }
                    else
                    {
                        value = Convert.ToDouble(value1) / 10000;
                    }
                    String tmp = value.ToString("P", labelFormat);
                    curve.m_labelStr = curve.m_label.m_text + ": " + tmp;
                    value1 = Convert.ToInt32((Convert.ToDouble(tmp.Replace("%", "")) * 100));
                    break;
                case PieLabelType.Name_Value_Percent:
                    curve.m_labelStr = curve.m_label.m_text + ": " + curve.m_pieValue.ToString("F", labelFormat) +
                        " (" + (curve.m_sweepAngle / 360).ToString("P", labelFormat) + ")";
                    value = curve.m_sweepAngle / 360;
                    break;
                case PieLabelType.Name:
                    curve.m_labelStr = curve.m_label.m_text;
                    break;
                case PieLabelType.None:
                default:
                    break;
            }
            if (value1 < 0)
                value1 = 0;
            return value1; ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="chartRect">区域</param>
        /// <returns></returns>
        public static RectangleF CalcPieSliceRect(Graphics g, GraphPane pane, float scaleFactor, RectangleF chartRect)
        {
            double maxDisplacement = 0;
            RectangleF tempRect;
            RectangleF nonExplRect = chartRect;
            if (pane.CurveList.IsPieOnly)
            {
                if (nonExplRect.Width < nonExplRect.Height)
                {
                    nonExplRect.Inflate(-(float)0.05F * nonExplRect.Height, -(float)0.05F * nonExplRect.Width);
                    float delta = (nonExplRect.Height - nonExplRect.Width) / 2;
                    nonExplRect.Height = nonExplRect.Width;
                    nonExplRect.Y += delta;
                }
                else
                {
                    nonExplRect.Inflate(-(float)0.05F * nonExplRect.Height, -(float)0.05F * nonExplRect.Width);
                    float delta = (nonExplRect.Width - nonExplRect.Height) / 2;
                    nonExplRect.Width = nonExplRect.Height;
                    nonExplRect.X += delta;
                }
                double aspectRatio = chartRect.Width / chartRect.Height;
                if (aspectRatio < 1.5)
                    nonExplRect.Inflate(-(float)(.1 * (1.5 / aspectRatio) * nonExplRect.Width),
                                            -(float)(.1 * (1.5 / aspectRatio) * nonExplRect.Width));
                PieItem.CalculateSliceChartParams(pane, ref maxDisplacement);
                if (maxDisplacement != 0)
                    CalcNewBaseRect(maxDisplacement, ref nonExplRect);
                foreach (PieItem slice in pane.CurveList)
                {
                    slice.m_boundingRectangle = nonExplRect;
                    if (slice.Displacement != 0)
                    {
                        tempRect = nonExplRect;
                        slice.CalcExplodedRect(ref tempRect);
                        slice.m_boundingRectangle = tempRect;
                    }
                    slice.DesignLabel(g, pane, slice.m_boundingRectangle, scaleFactor);
                }
            }
            return nonExplRect;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="explRect">区域</param>
        private void CalcExplodedRect(ref RectangleF explRect)
        {
            explRect.X += (float)(this.Displacement * explRect.Width / 2 * Math.Cos(m_midAngle * Math.PI / 180));
            explRect.Y += (float)(this.Displacement * explRect.Height / 2 * Math.Sin(m_midAngle * Math.PI / 180));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="maxDisplacement">最小位移</param>
        private static void CalculateSliceChartParams(GraphPane pane, ref double maxDisplacement)
        {
            String lblStr = " ";
            double pieTotalValue = 0;
            foreach (PieItem curve in pane.CurveList)
                if (curve.IsPie)
                {
                    pieTotalValue += curve.m_pieValue;
                    if (curve.Displacement > maxDisplacement)
                        maxDisplacement = curve.Displacement;
                }
            double nextStartAngle = 0;
            foreach (PieItem curve in pane.CurveList)
            {
                lblStr = curve.m_labelStr;
                curve.StartAngle = (float)nextStartAngle;
                curve.SweepAngle = (float)(360 * curve.Value / pieTotalValue);
                curve.MidAngle = curve.StartAngle + curve.SweepAngle / 2;
                nextStartAngle = curve.m_startAngle + curve.m_sweepAngle;
                PieItem.BuildLabelString(curve, -1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="pie">饼图</param>
        /// <param name="maxDisplacement">最大位移</param>
        /// <returns></returns>
        public static float CalculateAngleAndLabelLength(Graphics g, float scaleFactor, Pie pie, ref double maxDisplacement)
        {
            String lblStr = " ";
            double pieTotalValue = 0;
            foreach (PieItem curve in pie.Slices)
            {
                pieTotalValue += curve.m_pieValue;
                if (curve.Displacement > maxDisplacement)
                    maxDisplacement = curve.Displacement;
            }
            double nextStartAngle = -90;
            float MaxWith = 0F;
            int value = 0;
            int count = pie.Slices.Count;
            for (int i = 0; i < count; i++)
            {
                PieItem curve = pie.Slices[i];
                lblStr = curve.m_labelStr;
                curve.StartAngle = (float)nextStartAngle;
                curve.SweepAngle = (float)(360 * curve.Value / pieTotalValue);
                curve.MidAngle = curve.StartAngle + curve.SweepAngle / 2;
                nextStartAngle = curve.m_startAngle + curve.m_sweepAngle;
                if (count == 1)
                    value = -1;
                if (i < count - 1)
                    value = value + PieItem.BuildLabelString(curve, -1);
                else
                    PieItem.BuildLabelString(curve, 10000 - value);
                if (curve.IsVisible)
                {
                    float widthTemp = curve.m_labelDetail.FontSpec.MeasureString(g, curve.m_labelStr, scaleFactor).Width;
                    if (MaxWith < widthTemp)
                    {
                        MaxWith = widthTemp;
                    }
                }
            }
            return MaxWith;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="rectPie">区域</param>
        /// <param name="rectOut">外部区域</param>
        /// <param name="extensionLine">扩展线</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="preRects">饼切片</param>
        public void CalcLabel2(Graphics g, GraphPane pane, RectangleF rectPie, RectangleF rectOut, float extensionLine, float scaleFactor, List<PieItem> preRects)
        {
            try
            {
                if (!m_labelDetail.IsVisible)
                    return;
                PointF rectCenter = new PointF((rectPie.X + rectPie.Width / 2), (rectPie.Y + rectPie.Height / 2));
                m_intersectionPoint = new PointF((float)(rectCenter.X + (rectPie.Width / 2 * Math.Cos((m_midAngle) * Math.PI / 180))),
                    (float)(rectCenter.Y + (rectPie.Height / 2 * Math.Sin((m_midAngle) * Math.PI / 180))));
                m_pivotPoint = new PointF((float)(m_intersectionPoint.X + (extensionLine * Math.Cos(m_midAngle * Math.PI / 180))),
                    (float)(m_intersectionPoint.Y + (extensionLine * Math.Sin((m_midAngle) * Math.PI / 180))));
                SizeF size = m_labelDetail.FontSpec.MeasureString(g, m_labelStr, scaleFactor);
                m_labelRect = new RectangleF(0F, 0F, size.Width, size.Height);
                if (IsLeftSlice)
                {
                    m_beginPoint.X = rectOut.X;
                    m_beginPoint.Y = m_pivotPoint.Y;
                    m_endPoint.X = m_beginPoint.X + size.Width;
                    m_endPoint.Y = m_beginPoint.Y;
                }
                else
                {
                    m_beginPoint.X = rectOut.Right;
                    m_beginPoint.Y = m_pivotPoint.Y;
                    m_endPoint.X = m_beginPoint.X - size.Width;
                    m_endPoint.Y = m_beginPoint.Y;
                }
                if (m_intersectionPoint.X >= rectCenter.X)
                {
                    m_labelDetail.Location.AlignH = AlignH.Right;
                }
                else
                {
                    m_labelDetail.Location.AlignH = AlignH.Left;
                }
                CollisionClockWise(ref rectOut, preRects, ref rectCenter, ref size);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rectOut">外部区域</param>
        /// <param name="preRects">切片</param>
        /// <param name="rectCenter">中心点</param>
        /// <param name="size">尺寸</param>
        private void CollisionClockWise(ref RectangleF rectOut, List<PieItem> preRects, ref PointF rectCenter, ref SizeF size)
        {
            m_labelRect.X = m_beginPoint.X;
            m_labelRect.Y = m_beginPoint.Y;
            if (preRects.Count > 0)
            {
                PieItem pi = preRects[preRects.Count - 1];
                if (this.IsLeftSlice == pi.IsLeftSlice)
                {
                    RectangleF rect = pi.m_labelRect;
                    if (this.m_midAngle > -90 && this.m_midAngle < 90)
                    {
                        if (m_labelRect.Y < rect.Y + m_labelRect.Height)
                        {
                            m_labelRect.Y = rect.Y + m_labelRect.Height;
                        }
                    }
                    else
                    {
                        if (m_labelRect.Y > rect.Y - m_labelRect.Height)
                        {
                            m_labelRect.Y = rect.Y - m_labelRect.Height;
                        }
                    }
                }
            }
            if (m_labelRect.Y > rectOut.Bottom - m_labelRect.Height / 2F)
            {
                m_labelRect.Y = rectOut.Bottom - m_labelRect.Height / 2F;
            }
            else if (m_labelRect.Y < rectOut.Top + m_labelRect.Height / 2F)
            {
                m_labelRect.Y = rectOut.Top + m_labelRect.Height / 2F;
            }
            preRects.Add(this);
            m_beginPoint.X = m_labelRect.X;
            m_beginPoint.Y = m_labelRect.Y;
            if (IsLeftSlice)
            {
                m_endPoint.X = m_beginPoint.X + size.Width;
                m_endPoint.Y = m_beginPoint.Y;
            }
            else
            {
                m_endPoint.X = m_beginPoint.X - size.Width;
                m_endPoint.Y = m_beginPoint.Y;
            }
            if (m_midAngle > -90 && m_midAngle <= 0)
            {
                if (m_endPoint.Y < m_intersectionPoint.Y)
                {
                    m_pivotPoint.Y = m_endPoint.Y;
                    m_pivotPoint.X = rectCenter.X - (rectCenter.Y - m_endPoint.Y) / (float)Math.Tan(m_midAngle * Math.PI / 180);
                    if (m_pivotPoint.X > m_endPoint.X)
                    {
                        m_pivotPoint.X = m_endPoint.X;
                    }
                }
                else
                {
                    m_pivotPoint = m_intersectionPoint;
                }
            }
            else if (m_midAngle > 0 && m_midAngle <= 90)
            {
                if (m_endPoint.Y > m_intersectionPoint.Y)
                {
                    m_pivotPoint.Y = m_endPoint.Y;
                    m_pivotPoint.X = rectCenter.X + (m_endPoint.Y - rectCenter.Y) / (float)Math.Tan(m_midAngle * Math.PI / 180);
                    if (m_pivotPoint.X > m_endPoint.X)
                    {
                        m_pivotPoint.X = m_endPoint.X;
                    }
                }
                else
                {
                    m_pivotPoint = m_intersectionPoint;
                }
            }
            else if (m_midAngle > 90 && m_midAngle <= 180)
            {
                if (m_endPoint.Y > m_intersectionPoint.Y)
                {
                    m_pivotPoint.Y = m_endPoint.Y;
                    m_pivotPoint.X = rectCenter.X + (m_endPoint.Y - rectCenter.Y) / (float)Math.Tan(m_midAngle * Math.PI / 180);
                    if (m_pivotPoint.X < m_endPoint.X)
                    {
                        m_pivotPoint.X = m_endPoint.X;
                    }
                }
                else
                {
                    m_pivotPoint = m_intersectionPoint;
                }
            }
            else if (m_midAngle > 180 && m_midAngle <= 270)
            {
                if (m_endPoint.Y < m_intersectionPoint.Y)
                {
                    m_pivotPoint.Y = m_endPoint.Y;
                    m_pivotPoint.X = rectCenter.X - (rectCenter.Y - m_endPoint.Y) / (float)Math.Tan(m_midAngle * Math.PI / 180);
                    if (m_pivotPoint.X < m_endPoint.X)
                    {
                        m_pivotPoint.X = m_endPoint.X;
                    }
                }
                else
                {
                    m_pivotPoint = m_intersectionPoint;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="rectOut">外部区域</param>
        /// <param name="preRects">切片</param>
        /// <param name="rectCenter">中心点</param>
        /// <param name="scaleFactor">刻度因子</param>
        public void CollisionCounterClockWise(Graphics g, ref RectangleF rectOut, List<PieItem> preRects, ref PointF rectCenter, float scaleFactor)
        {
            SizeF size = m_labelDetail.FontSpec.MeasureString(g, m_labelStr, scaleFactor);
            m_labelRect.X = m_beginPoint.X;
            m_labelRect.Y = m_beginPoint.Y;
            if (preRects.Count > 0)
            {
                PieItem pi = preRects[preRects.Count - 1];
                if (this.IsLeftSlice == pi.IsLeftSlice)
                {
                    RectangleF rect = pi.m_labelRect;
                    if (this.m_midAngle > -90 && this.m_midAngle < 90)
                    {
                        if (m_labelRect.Y > rect.Y - m_labelRect.Height)
                        {
                            m_labelRect.Y = rect.Y - m_labelRect.Height;
                        }
                    }
                    else
                    {
                        if (m_labelRect.Y < rect.Y + m_labelRect.Height)
                        {
                            m_labelRect.Y = rect.Y + m_labelRect.Height;
                        }
                    }
                }
            }
            if (m_labelRect.Y > rectOut.Bottom - m_labelRect.Height / 2F)
            {
                m_labelRect.Y = rectOut.Bottom - m_labelRect.Height / 2F;
            }
            else if (m_labelRect.Y < rectOut.Top + m_labelRect.Height / 2F)
            {
                m_labelRect.Y = rectOut.Top + m_labelRect.Height / 2F;
            }
            preRects.Add(this);
            m_beginPoint.X = m_labelRect.X;
            m_beginPoint.Y = m_labelRect.Y;
            if (IsLeftSlice)
            {
                m_endPoint.X = m_beginPoint.X + size.Width;
                m_endPoint.Y = m_beginPoint.Y;
            }
            else
            {
                m_endPoint.X = m_beginPoint.X - size.Width;
                m_endPoint.Y = m_beginPoint.Y;
            }
            if (m_midAngle > -90 && m_midAngle <= 0)
            {
                if (m_endPoint.Y < m_intersectionPoint.Y)
                {
                    m_pivotPoint.Y = m_endPoint.Y;
                    m_pivotPoint.X = rectCenter.X - (rectCenter.Y - m_endPoint.Y) / (float)Math.Tan(m_midAngle * Math.PI / 180);
                    if (m_pivotPoint.X > m_endPoint.X)
                    {
                        m_pivotPoint.X = m_endPoint.X;
                    }
                }
                else
                {
                    m_pivotPoint = m_intersectionPoint;
                }
            }
            else if (m_midAngle > 0 && m_midAngle <= 90)
            {
                if (m_endPoint.Y > m_intersectionPoint.Y)
                {
                    m_pivotPoint.Y = m_endPoint.Y;
                    m_pivotPoint.X = rectCenter.X + (m_endPoint.Y - rectCenter.Y) / (float)Math.Tan(m_midAngle * Math.PI / 180);
                    if (m_pivotPoint.X > m_endPoint.X)
                    {
                        m_pivotPoint.X = m_endPoint.X;
                    }
                }
                else
                {
                    m_pivotPoint = m_intersectionPoint;
                }
            }
            else if (m_midAngle > 90 && m_midAngle <= 180)
            {
                if (m_endPoint.Y > m_intersectionPoint.Y)
                {
                    m_pivotPoint.Y = m_endPoint.Y;
                    m_pivotPoint.X = rectCenter.X + (m_endPoint.Y - rectCenter.Y) / (float)Math.Tan(m_midAngle * Math.PI / 180);
                    if (m_pivotPoint.X < m_endPoint.X)
                    {
                        m_pivotPoint.X = m_endPoint.X;
                    }
                }
                else
                {
                    m_pivotPoint = m_intersectionPoint;
                }
            }
            else if (m_midAngle > 180 && m_midAngle <= 270)
            {
                if (m_endPoint.Y < m_intersectionPoint.Y)
                {
                    m_pivotPoint.Y = m_endPoint.Y;
                    m_pivotPoint.X = rectCenter.X - (rectCenter.Y - m_endPoint.Y) / (float)Math.Tan(m_midAngle * Math.PI / 180);
                    if (m_pivotPoint.X < m_endPoint.X)
                    {
                        m_pivotPoint.X = m_endPoint.X;
                    }
                }
                else
                {
                    m_pivotPoint = m_intersectionPoint;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pie">饼图</param>
        /// <param name="maxDisplacement">最大位移</param>
        private static void CalculatePieChartParams(Pie pie, ref double maxDisplacement)
        {
            String lblStr = " ";
            double pieTotalValue = 0;
            foreach (PieItem curve in pie.Slices)
            {
                pieTotalValue += curve.m_pieValue;
                if (curve.Displacement > maxDisplacement)
                    maxDisplacement = curve.Displacement;
            }
            double nextStartAngle = -90;
            foreach (PieItem curve in pie.Slices)
            {
                lblStr = curve.m_labelStr;
                curve.StartAngle = (float)nextStartAngle;
                curve.SweepAngle = (float)(360 * curve.Value / pieTotalValue);
                curve.MidAngle = curve.StartAngle + curve.SweepAngle / 2;
                nextStartAngle = curve.m_startAngle + curve.m_sweepAngle;
                PieItem.BuildLabelString(curve, -1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect">区域</param>
        /// <param name="midAngle">中间角度</param>
        private void CalculateLinePoints(RectangleF rect, double midAngle)
        {
            PointF rectCenter = new PointF((rect.X + rect.Width / 2), (rect.Y + rect.Height / 2));
            m_intersectionPoint = new PointF((float)(rectCenter.X + (rect.Width / 2 * Math.Cos((midAngle) * Math.PI / 180))),
                (float)(rectCenter.Y + (rect.Height / 2 * Math.Sin((midAngle) * Math.PI / 180))));
            m_pivotPoint = new PointF((float)(m_intersectionPoint.X + (rect.Width * .05 * Math.Cos(midAngle * Math.PI / 180))),
                (float)(m_intersectionPoint.Y + (.05 * rect.Width * Math.Sin((midAngle) * Math.PI / 180))));
            if (m_pivotPoint.X >= rectCenter.X)
            {
                m_endPoint = new PointF((float)(m_pivotPoint.X + .2 * rect.Width), m_pivotPoint.Y);
                m_labelDetail.Location.AlignH = AlignH.Left;
            }
            else
            {
                m_endPoint = new PointF((float)(m_pivotPoint.X - .2 * rect.Width), m_pivotPoint.Y);
                m_labelDetail.Location.AlignH = AlignH.Right;
            }
            m_midAngle = (float)midAngle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxDisplacement">最大位移</param>
        /// <param name="baseRect">基础区域</param>
        private static void CalcNewBaseRect(double maxDisplacement, ref RectangleF baseRect)
        {
            float xDispl = (float)((maxDisplacement * baseRect.Width));
            float yDispl = (float)((maxDisplacement * baseRect.Height));
            baseRect.Inflate(-(float)((xDispl / 10)), -(float)((xDispl / 10)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="rect">区域</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="pie">饼图</param>
        internal void Draw(Graphics g, GraphPane pane, RectangleF rect, float scaleFactor, Pie pie)
        {
            SmoothingMode sMode = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.HighQuality;
            RectangleF tempRect;
            double maxDisplacement = 0;
            PieItem.CalculatePieChartParams(pie, ref maxDisplacement);
            m_slicePath = new GraphicsPath();
            using (Brush brush = m_fill.MakeBrush(rect))
            {
                g.FillPie(brush, rect.X, rect.Y, rect.Width, rect.Height, this.StartAngle, this.SweepAngle);
                m_slicePath.AddPie(rect.X, rect.Y, rect.Width, rect.Height,
                    this.StartAngle, this.SweepAngle);
                if (m_labelType != PieLabelType.None && m_midAngle - m_startAngle > 400 / rect.Height)
                {
                    m_boundingRectangle = rect;
                    if (Displacement != 0)
                    {
                        tempRect = rect;
                        CalcExplodedRect(ref tempRect);
                        m_boundingRectangle = tempRect;
                    }
                    DesignLabel(g, pane, m_boundingRectangle, scaleFactor);
                    DrawLabel(g, pane, rect, scaleFactor);
                }
            }
            g.SmoothingMode = sMode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="rect">区域</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="pie">饼图</param>
        internal void Draw2(Graphics g, GraphPane pane, RectangleF rect, float scaleFactor, Pie pie)
        {
            try
            {
                SmoothingMode sMode = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.HighQuality;
                RectangleF tempRect;
                m_slicePath = new GraphicsPath();
                using (Brush brush = m_fill.MakeBrush(rect))
                {
                    g.FillPie(brush, rect.X, rect.Y, rect.Width, rect.Height, this.StartAngle, this.SweepAngle);
                    m_slicePath.AddPie(rect.X, rect.Y, rect.Width, rect.Height,
                        this.StartAngle, this.SweepAngle);
                    if (m_labelType != PieLabelType.None)
                    {
                        m_boundingRectangle = rect;
                        if (Displacement != 0)
                        {
                            tempRect = rect;
                            CalcExplodedRect(ref tempRect);
                            m_boundingRectangle = tempRect;
                        }
                        if (m_midAngle > 90 && m_midAngle < 270)
                        {
                            this.Index = pie.leftSliceCount;
                            this.IsLeftSlice = true;
                            pie.leftSliceCount++;
                        }
                        else
                        {
                            this.Index = pie.rightSliceCount;
                            this.IsLeftSlice = false;
                            pie.rightSliceCount++;
                        }
                    }
                }
                g.SmoothingMode = sMode;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="pos">位置</param>
        /// <param name="scaleFactor">刻度因子</param>
        public override void Draw(Graphics g, GraphPane pane, int pos, float scaleFactor)
        {
            if (pane.Chart.m_rect.Width <= 0 && pane.Chart.m_rect.Height <= 0)
            {
                m_slicePath = null;
            }
            else
            {
                CalcPieSliceRect(g, pane, scaleFactor, pane.Chart.m_rect);
                m_slicePath = new GraphicsPath();
                if (!m_isVisible)
                    return;
                RectangleF tRect = m_boundingRectangle;
                if (tRect.Width >= 1 && tRect.Height >= 1)
                {
                    SmoothingMode sMode = g.SmoothingMode;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    Fill tFill = m_fill;
                    Border tBorder = m_border;
                    if (this.IsSelected)
                    {
                        tFill = Selection.Fill;
                        tBorder = Selection.Border;
                    }
                    using (Brush brush = tFill.MakeBrush(m_boundingRectangle))
                    {
                        g.FillPie(brush, tRect.X, tRect.Y, tRect.Width, tRect.Height, this.StartAngle, this.SweepAngle);
                        m_slicePath.AddPie(tRect.X, tRect.Y, tRect.Width, tRect.Height,
                            this.StartAngle, this.SweepAngle);
                        if (this.Border.IsVisible)
                        {
                            Pen borderPen = tBorder.GetPen(pane, scaleFactor);
                            g.DrawPie(borderPen, tRect.X, tRect.Y, tRect.Width, tRect.Height,
    this.StartAngle, this.SweepAngle);
                        }
                        if (m_labelType != PieLabelType.None)
                            DrawLabel(g, pane, tRect, scaleFactor);
                    }
                    g.SmoothingMode = sMode;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="rect">区域</param>
        /// <param name="scaleFactor">刻度因子</param>
        public void DrawLabel(Graphics g, GraphPane pane, RectangleF rect, float scaleFactor)
        {
            if (!m_labelDetail.IsVisible)
                return;
            Pen labelPen = this.Border.GetPen(pane, scaleFactor);
            g.DrawLine(labelPen, m_intersectionPoint, m_pivotPoint);
            g.DrawLine(labelPen, m_pivotPoint, m_endPoint);
            m_labelDetail.Draw(g, pane, scaleFactor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        public void DrawLabel2(Graphics g, GraphPane pane, float scaleFactor)
        {
            m_labelDetail.LayoutArea = new SizeF();
            m_labelDetail.Location.AlignV = AlignV.Center;
            m_labelDetail.Location.CoordinateFrame = CoordType.PaneFraction;
            m_labelDetail.Location.X = (m_beginPoint.X - pane.Rect.X) / pane.Rect.Width;
            m_labelDetail.Location.Y = (m_beginPoint.Y - pane.Rect.Y) / pane.Rect.Height;
            m_labelDetail.Text = m_labelStr;
            m_labelDetail.FontSpec.Border.IsVisible = false;
            m_labelDetail.FontSpec.Border.Color = Color.Red;
            m_labelDetail.FontSpec.Fill.IsVisible = true;
            m_labelDetail.FontSpec.Fill.IsScaled = true;
            m_labelDetail.FontSpec.IsBold = true;
            Pen labelPen = this.Border.GetPen(pane, scaleFactor);
            labelPen.Color = Color;
            {
                SmoothingMode sm = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.DrawLine(labelPen, (int)m_intersectionPoint.X, (int)m_intersectionPoint.Y, (int)m_pivotPoint.X, (int)m_pivotPoint.Y);
                g.DrawLine(labelPen, (int)m_pivotPoint.X, (int)m_pivotPoint.Y, (int)m_endPoint.X, (int)m_endPoint.Y);
                g.SmoothingMode = sm;
                m_labelDetail.fontSpec.Border.Color = Color;
                m_labelDetail.fontSpec.FontColor = Color;
                m_labelDetail.Draw(g, pane, scaleFactor);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="rect">区域</param>
        /// <param name="scaleFactor">刻度因子</param>
        public void DesignLabel(Graphics g, GraphPane pane, RectangleF rect, float scaleFactor)
        {
            if (!m_labelDetail.IsVisible)
                return;
            m_labelDetail.LayoutArea = new SizeF();
            CalculateLinePoints(rect, m_midAngle);
            SizeF size = m_labelDetail.FontSpec.BoundingBox(g, m_labelStr, scaleFactor);
            RectangleF chartRect = pane.Chart.m_rect;
            float fill = 0;
            if (m_midAngle > 315 || m_midAngle <= 45)
            {
                fill = chartRect.X + chartRect.Width - m_endPoint.X - 5;
                if (size.Width > fill)
                {
                    m_labelDetail.LayoutArea = new SizeF(fill, size.Height * 3.0F);
                }
            }
            if (m_midAngle > 45 && m_midAngle <= 135)
            {
                fill = chartRect.Y + chartRect.Height - m_endPoint.Y - 5;
                if (size.Height / 2 > fill)
                {
                    if (m_midAngle > 90)	
                        CalculateLinePoints(rect, m_midAngle + (m_sweepAngle + m_startAngle - m_midAngle) / 3);
                    else						
                        CalculateLinePoints(rect, m_midAngle - (m_midAngle - (m_midAngle - m_startAngle) / 3));
                }
            }
            if (m_midAngle > 135 && m_midAngle <= 225)
            {
                fill = m_endPoint.X - chartRect.X - 5;
                if (size.Width > fill)
                {
                    m_labelDetail.LayoutArea = new SizeF(fill, size.Height * 3.0F);
                }
            }
            if (m_midAngle > 225 && m_midAngle <= 315)
            {
                fill = m_endPoint.Y - 5 - chartRect.Y;
                if (size.Height / 2 > fill)
                {
                    if (m_midAngle < 270)	
                        CalculateLinePoints(rect, m_midAngle - (m_sweepAngle + m_startAngle - m_midAngle) / 3);
                    else						
                        CalculateLinePoints(rect, m_midAngle + (m_midAngle - m_startAngle) / 3);
                }
            }
            m_labelDetail.Location.AlignV = AlignV.Center;
            m_labelDetail.Location.CoordinateFrame = CoordType.PaneFraction;
            m_labelDetail.Location.X = (m_endPoint.X - pane.Rect.X) / pane.Rect.Width;
            m_labelDetail.Location.Y = (m_endPoint.Y - pane.Rect.Y) / pane.Rect.Height;
            m_labelDetail.Text = m_labelStr;
            m_labelDetail.FontSpec.Border.IsVisible = false;
            m_labelDetail.FontSpec.Fill.IsVisible = false;
            m_labelDetail.FontSpec.Size = 7;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="rect">区域</param>
        /// <param name="scaleFactor">刻度因子</param>
        public override void DrawLegendKey(Graphics g, GraphPane pane, RectangleF rect, float scaleFactor)
        {
            SmoothingMode smoothingMode = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.HighQuality;
            m_fill.Type = FillType.GradientByColorValue;
            g.FillPie(m_fill.MakeBrush(rect), rect.X - 2, rect.Y - 2, rect.Width * 3 / 4, rect.Width * 3 / 4, 270, 90);
            g.SmoothingMode = smoothingMode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="i">索引</param>
        /// <param name="coords">坐标</param>
        /// <returns></returns>
        public override bool GetCoords(GraphPane pane, int i, out String coords)
        {
            coords = String.Empty;
            PointF pt = m_boundingRectangle.Location;
            pt.X += m_boundingRectangle.Width / 2.0f;
            pt.Y += m_boundingRectangle.Height / 2.0f;
            float radius = m_boundingRectangle.Width / 2.0f;
            Matrix matrix = new Matrix();
            matrix.Translate(pt.X, pt.Y);
            matrix.Rotate(this.StartAngle);
            int count = (int)Math.Floor(SweepAngle / 5) + 1;
            PointF[] pts = new PointF[2 + count];
            pts[0] = new PointF(0, 0);
            pts[1] = new PointF(radius, 0);
            double angle = 0.0;
            for (int j = 2; j < count + 2; j++)
            {
                angle += SweepAngle / count;
                pts[j] = new PointF(radius * (float)Math.Cos(angle * Math.PI / 180.0),
                                            radius * (float)Math.Sin(angle * Math.PI / 180.0));
            }
            matrix.TransformPoints(pts);
            coords = String.Format("{0:f0},{1:f0},{2:f0},{3:f0},",
                        pts[0].X, pts[0].Y, pts[1].X, pts[1].Y);
            for (int j = 2; j < count + 2; j++)
                coords += String.Format(j > count ? "{0:f0},{1:f0}" : "{0:f0},{1:f0},", pts[j].X, pts[j].Y);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns></returns>
        internal override bool IsXIndependent(GraphPane pane)
        {
            return true;
        }
        #endregion
    }
}
