/*****************************************************************************\
*                                                                             *
* Axis.cs -     Axis functions, types, and definitions                        *
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
    /// 坐标轴的基类
    /// </summary>
    [Serializable]
    public abstract class Axis : IDisposable
    {
        #region 陶德 2016/5/31
        /// <summary>
        /// 创建坐标轴
        /// </summary>
        public Axis()
        {
            m_scale = new LinearScale(this);
            m_cross = 0.0;
            m_crossAuto = true;
            m_majorTic = new MajorTic();
            m_minorTic = new MinorTic();
            m_majorGrid = new MajorGrid();
            m_minorGrid = new MinorGrid();
            m_axisGap = Default.AxisGap;
            m_minSpace = Default.MinSpace;
            m_isVisible = true;
            m_isAxisSegmentVisible = Default.IsAxisSegmentVisible;
            m_title = new AxisLabel("", Default.TitleFontFamily, Default.TitleFontSize,
                    Default.TitleFontColor, Default.TitleFontBold,
                    Default.TitleFontUnderline, Default.TitleFontItalic);
            m_title.FontSpec.Fill = new Fill(Default.TitleFillColor, Default.TitleFillBrush,
                    Default.TitleFillType);
            m_title.FontSpec.Border.IsVisible = false;
            m_color = Default.Color;
        }

        /// <summary>
        /// 创建坐标轴
        /// </summary>
        /// <param name="title">标题</param>
        public Axis(String title)
            : this()
        {
            m_title.m_text = title;
        }

        /// <summary>
        /// 创建坐标轴
        /// </summary>
        /// <param name="rhs">坐标轴</param>
        public Axis(Axis rhs)
        {
            m_scale = rhs.m_scale.Clone(this);
            m_cross = rhs.m_cross;
            m_crossAuto = rhs.m_crossAuto;
            m_majorTic = new MajorTic(rhs.MajorTic);
            m_minorTic = new MinorTic(rhs.MinorTic);
            m_majorGrid = new MajorGrid(rhs.m_majorGrid);
            m_minorGrid = new MinorGrid(rhs.m_minorGrid);
            m_isVisible = rhs.IsVisible;
            m_isAxisSegmentVisible = rhs.m_isAxisSegmentVisible;
            m_title = (AxisLabel)new GapLabel(rhs.Title);
            m_axisGap = rhs.m_axisGap;
            m_minSpace = rhs.MinSpace;
            m_color = rhs.Color;
        }

        /// <summary>
        /// 默认属性
        /// </summary>
        public struct Default
        {
            public static float AxisGap = 5;
            public static float TitleGap = 0.0f;
            public static String TitleFontFamily = "Arial";
            public static float TitleFontSize = 14;
            public static Color TitleFontColor = Color.FromArgb(30, 30, 30);
            public static bool TitleFontBold = true;
            public static bool TitleFontItalic = false;
            public static bool TitleFontUnderline = false;
            public static Color TitleFillColor = Color.White;
            public static Brush TitleFillBrush = null;
            public static FillType TitleFillType = FillType.None;
            public static Color BorderColor = Color.FromArgb(30, 30, 30);
            public static bool IsAxisSegmentVisible = true;
            public static AxisType Type = AxisType.Linear;
            public static Color Color = Color.FromArgb(30, 30, 30);
            public static float MinSpace = 0f;
        }

        /// <summary>
        /// 间隔的临时变量
        /// </summary>
        internal float m_tmpSpace;

        private float m_axisGap;

        /// <summary>
        /// 获取或设置坐标轴间隔
        /// </summary>
        public float AxisGap
        {
            get { return m_axisGap; }
            set { m_axisGap = value; }
        }

        private Color m_color;

        /// <summary>
        /// 获取或设置颜色
        /// </summary>
        public Color Color
        {
            get { return m_color; }
            set { m_color = value; }
        }

        internal double m_cross;

        /// <summary>
        /// 获取或设置相交值
        /// </summary>
        public double Cross
        {
            get { return m_cross; }
            set { m_cross = value; m_crossAuto = false; }
        }

        internal bool m_crossAuto;

        /// <summary>
        /// 获取或设置是否自动相交
        /// </summary>
        public bool CrossAuto
        {
            get { return m_crossAuto; }
            set { m_crossAuto = value; }
        }

        protected bool m_isAxisSegmentVisible;

        /// <summary>
        /// 获取或设置坐标轴线段是否可见
        /// </summary>
        public bool IsAxisSegmentVisible
        {
            get { return m_isAxisSegmentVisible; }
            set { m_isAxisSegmentVisible = value; }
        }

        private bool m_IsScientificNotation;

        /// <summary>
        /// 获取或设置是否科学计数
        /// </summary>
        public bool IsScientificNotation
        {
            get { return m_IsScientificNotation; }
            set
            {
                m_IsScientificNotation = value;
                this.Scale.IsScientificNotation = value;
            }
        }

        protected bool m_isVisible;

        /// <summary>
        /// 获取或设置是否显示
        /// </summary>
        public bool IsVisible
        {
            get { return m_isVisible; }
            set { m_isVisible = value; }
        }

        /// <summary>
        /// 获取或设置是否有零线
        /// </summary>
        public bool IsZeroLine
        {
            get { return m_majorGrid.m_isZeroLine; }
            set { m_majorGrid.m_isZeroLine = value; }
        }


        internal MajorGrid m_majorGrid;

        /// <summary>
        /// 获取主网格
        /// </summary>
        public MajorGrid MajorGrid
        {
            get { return m_majorGrid; }
        }


        internal MinorGrid m_minorGrid;

        /// <summary>
        /// 获取次网格
        /// </summary>
        public MinorGrid MinorGrid
        {
            get { return m_minorGrid; }
        }

        internal MajorTic m_majorTic;

        /// <summary>
        /// 获取主记号
        /// </summary>
        public MajorTic MajorTic
        {
            get { return m_majorTic; }
        }

        internal MinorTic m_minorTic;

        /// <summary>
        /// 获取次记号
        /// </summary>
        public MinorTic MinorTic
        {
            get { return m_minorTic; }
        }

        private float m_minSpace;

        /// <summary>
        /// 获取或设置最小值下方空间
        /// </summary>
        public float MinSpace
        {
            get { return m_minSpace; }
            set { m_minSpace = value; }
        }

        internal Scale m_scale;

        /// <summary>
        /// 获取或设置刻度
        /// </summary>
        public Scale Scale
        {
            get { return m_scale; }
        }

        protected AxisLabel m_title;

        /// <summary>
        /// 获取或设置标题
        /// </summary>
        public AxisLabel Title
        {
            get { return m_title; }
            set { m_title = value; }
        }

        /// <summary>
        /// 获取或设置坐标轴类型
        /// </summary>
        public AxisType Type
        {
            get { return m_scale.Type; }
            set { m_scale = Scale.MakeNewScale(m_scale, value); }
        }

        /// <summary>
        /// 计算相交位移
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns>位移</returns>
        internal abstract float CalcCrossShift(GraphPane pane);

        /// <summary>
        /// 计算十字线的分数部分
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns>分数部分</returns>
        internal float CalcCrossFraction(GraphPane pane)
        {
            if (!this.IsCrossShifted(pane))
            {
                if (IsPrimary(pane) && m_scale.m_isLabelsInside)
                    return 1.0f;
                else
                    return 0.0f;
            }
            double effCross = EffectiveCrossValue(pane);
            Axis crossAxis = GetCrossAxis(pane);
            double max = crossAxis.m_scale.Linearize(crossAxis.m_scale.m_min);
            double min = crossAxis.m_scale.Linearize(crossAxis.m_scale.m_max);
            float frac;
            if (((this is XAxis || this is YAxis) && m_scale.m_isLabelsInside == crossAxis.m_scale.IsReverse) ||
                 ((this is Y2Axis) && m_scale.m_isLabelsInside != crossAxis.m_scale.IsReverse))
                frac = (float)((effCross - min) / (max - min));
            else
                frac = (float)((max - effCross) / (max - min));
            if (frac < 0.0f)
                frac = 0.0f;
            if (frac > 1.0f)
                frac = 1.0f;
            return frac;
        }

        /// <summary>
        /// 计算总位移
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="shiftPos">偏移</param>
        /// <returns>总位移</returns>
        private float CalcTotalShift(GraphPane pane, float scaleFactor, float shiftPos)
        {
            if (!IsPrimary(pane))
            {
                if (IsCrossShifted(pane))
                {
                    shiftPos = 0;
                }
                else
                {
                    float ticSize = m_majorTic.ScaledTic(scaleFactor);
                    if (m_scale.m_isLabelsInside)
                    {
                        shiftPos += m_tmpSpace;
                        if (m_majorTic.IsOutside || m_majorTic.m_isCrossOutside ||
                                        m_minorTic.IsOutside || m_minorTic.m_isCrossOutside)
                            shiftPos -= ticSize;
                    }
                    else
                    {
                        shiftPos += m_axisGap * scaleFactor;
                        if (m_majorTic.IsInside || m_majorTic.m_isCrossInside ||
                                m_minorTic.IsInside || m_minorTic.m_isCrossInside)
                            shiftPos += ticSize;
                    }
                }
            }
            float crossShift = CalcCrossShift(pane);
            shiftPos += crossShift;
            return shiftPos;
        }

        /// <summary>
        ///计算间隙
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="fixedSpace">修正空间</param>
        /// <returns>间隙</returns>
        public float CalcSpace(Graphics g, GraphPane pane, float scaleFactor, out float fixedSpace)
        {
            float charHeight = m_scale.m_fontSpec.GetHeight(scaleFactor);
            float ticSize = m_majorTic.ScaledTic(scaleFactor);
            float axisGap = m_axisGap * scaleFactor;
            float scaledLabelGap = m_scale.m_labelGap * charHeight;
            float scaledTitleGap = m_title.GetScaledGap(scaleFactor);
            fixedSpace = 0;
            m_tmpSpace = 0;
            if (m_isVisible)
            {
                bool hasTic = this.MajorTic.IsOutside || this.MajorTic.m_isCrossOutside ||
                                    this.MinorTic.IsOutside || this.MinorTic.m_isCrossOutside;
                if (hasTic)
                    m_tmpSpace += ticSize;
                if (!IsPrimary(pane))
                {
                    m_tmpSpace += axisGap;
                    if (this.MajorTic.m_isInside || this.MajorTic.m_isCrossInside ||
                            this.MinorTic.m_isInside || this.MinorTic.m_isCrossInside)
                        m_tmpSpace += ticSize;
                }
                m_tmpSpace += m_scale.GetScaleMaxSpace(g, pane, scaleFactor, true).Height +
                        scaledLabelGap;
                String str = MakeTitle();
                if (!String.IsNullOrEmpty(str) && m_title.m_isVisible)
                {
                    fixedSpace = this.Title.FontSpec.BoundingBox(g, str, scaleFactor).Height +
                            scaledTitleGap;
                    m_tmpSpace += fixedSpace;
                    fixedSpace += scaledTitleGap;
                }
                if (hasTic)
                    fixedSpace += ticSize;
            }
            if (this.IsPrimary(pane) && ((
                    (this is YAxis && (
                        (!pane.XAxis.m_scale.m_isSkipFirstLabel && !pane.XAxis.m_scale.m_isReverse) ||
                        (!pane.XAxis.m_scale.m_isSkipLastLabel && pane.XAxis.m_scale.m_isReverse))) ||
                    (this is Y2Axis && (
                        (!pane.XAxis.m_scale.m_isSkipFirstLabel && pane.XAxis.m_scale.m_isReverse) ||
                        (!pane.XAxis.m_scale.m_isSkipLastLabel && !pane.XAxis.m_scale.m_isReverse)))) &&
                    pane.XAxis.IsVisible && pane.XAxis.m_scale.m_isVisible))
            {
                float tmp = pane.XAxis.m_scale.GetScaleMaxSpace(g, pane, scaleFactor, true).Width / 2.0F;
                fixedSpace = Math.Max(tmp, fixedSpace);
            }
            m_tmpSpace = Math.Max(m_tmpSpace, m_minSpace * (float)scaleFactor);
            fixedSpace = Math.Max(fixedSpace, m_minSpace * (float)scaleFactor);
            return m_tmpSpace;
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public void Dispose()
        {
            if (Scale != null)
                Scale.Dispose();
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="shiftPos">位移距离</param>
        public void Draw(Graphics g, GraphPane pane, float scaleFactor, float shiftPos)
        {
            Matrix saveMatrix = g.Transform;
            m_scale.SetupScaleData(pane, this);
            if (m_isVisible)
            {
                SetTransformMatrix(g, pane, scaleFactor);
                shiftPos = CalcTotalShift(pane, scaleFactor, shiftPos);
                m_scale.Draw(g, pane, scaleFactor, shiftPos);
                g.Transform = saveMatrix;
            }
        }

        /// <summary>
        /// 绘制网格
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="shiftPos">位移距离</param>
        internal void DrawGrid(Graphics g, GraphPane pane, float scaleFactor, float shiftPos)
        {
            if (m_isVisible)
            {
                Matrix saveMatrix = g.Transform;
                SetTransformMatrix(g, pane, scaleFactor);
                double baseVal = m_scale.CalcBaseTic();
                float topPix, rightPix;
                m_scale.GetTopRightPix(pane, out topPix, out rightPix);
                shiftPos = CalcTotalShift(pane, scaleFactor, shiftPos);
                m_scale.DrawGrid(g, pane, baseVal, topPix, scaleFactor);
                DrawMinorTics(g, pane, baseVal, shiftPos, scaleFactor, topPix);
                g.Transform = saveMatrix;
            }
        }

        /// <summary>
        /// 绘制次记号
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="baseVal">基础值</param>
        /// <param name="shift">偏移</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="topPix">顶部象素</param>
        public void DrawMinorTics(Graphics g, GraphPane pane, double baseVal, float shift,
                                float scaleFactor, float topPix)
        {
            if ((this.MinorTic.IsOutside || this.MinorTic.IsOpposite || this.MinorTic.IsInside ||
                    this.MinorTic.m_isCrossOutside || this.MinorTic.m_isCrossInside || m_minorGrid.m_isVisible)
                    && m_isVisible)
            {
                double tMajor = m_scale.m_majorStep * m_scale.MajorUnitMultiplier,
                            tMinor = m_scale.m_minorStep * m_scale.MinorUnitMultiplier;
                if (m_scale.IsLog || tMinor < tMajor)
                {
                    float minorScaledTic = this.MinorTic.ScaledTic(scaleFactor);
                    double first = m_scale.m_minLinTemp,
                                last = m_scale.m_maxLinTemp;
                    double dVal = first;
                    float pixVal;
                    int iTic = m_scale.CalcMinorStart(baseVal);
                    int MajorTic = 0;
                    double majorVal = m_scale.CalcMajorTicValue(baseVal, MajorTic);
                    using (Pen pen = new Pen(m_minorTic.m_color,
                                        pane.ScaledPenWidth(MinorTic.m_penWidth, scaleFactor)))
                    using (Pen minorGridPen = m_minorGrid.GetPen(pane, scaleFactor))
                    {
                        while (dVal < last && iTic < 5000)
                        {
                            dVal = m_scale.CalcMinorTicValue(baseVal, iTic);
                            if (dVal > majorVal)
                                majorVal = m_scale.CalcMajorTicValue(baseVal, ++MajorTic);
                            if (((Math.Abs(dVal) < 1e-20 && Math.Abs(dVal - majorVal) > 1e-20) ||
                                (Math.Abs(dVal) > 1e-20 && Math.Abs((dVal - majorVal) / dVal) > 1e-10)) &&
                                (dVal >= first && dVal <= last))
                            {
                                pixVal = m_scale.LocalTransform(dVal);
                                m_minorGrid.Draw(g, minorGridPen, pixVal, topPix);
                                m_minorTic.Draw(g, pane, pen, pixVal, topPix, shift, minorScaledTic);
                            }
                            iTic++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 绘制标题
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="shiftPos">偏移距离</param>
        /// <param name="scaleFactor">刻度因子</param>
        public void DrawTitle(Graphics g, GraphPane pane, float shiftPos, float scaleFactor)
        {
            String str = MakeTitle();
            if (m_isVisible && m_title.m_isVisible && !String.IsNullOrEmpty(str))
            {
                bool hasTic = (m_scale.m_isLabelsInside ?
                        (this.MajorTic.IsInside || this.MajorTic.m_isCrossInside ||
                                this.MinorTic.IsInside || this.MinorTic.m_isCrossInside) :
                        (this.MajorTic.IsOutside || this.MajorTic.m_isCrossOutside || this.MinorTic.IsOutside || this.MinorTic.m_isCrossOutside));
                float x = (m_scale.m_maxPix - m_scale.m_minPix) / 2;
                float scaledTic = MajorTic.ScaledTic(scaleFactor);
                float scaledLabelGap = m_scale.m_fontSpec.GetHeight(scaleFactor) * m_scale.m_labelGap;
                float scaledTitleGap = m_title.GetScaledGap(scaleFactor);
                float gap = scaledTic * (hasTic ? 1.0f : 0.0f) +
                            this.Title.FontSpec.BoundingBox(g, str, scaleFactor).Height / 2.0F;
                float y = (m_scale.m_isVisible ? m_scale.GetScaleMaxSpace(g, pane, scaleFactor, true).Height
                            + scaledLabelGap : 0);
                if (m_scale.m_isLabelsInside)
                    y = shiftPos - y - gap;
                else
                    y = shiftPos + y + gap;
                if (!m_crossAuto && !m_title.m_isTitleAtCross)
                    y = Math.Max(y, gap);
                AlignV alignV = AlignV.Center;
                y += scaledTitleGap;
                this.Title.FontSpec.Draw(g, pane, str, x, y,
                            AlignH.Center, alignV, scaleFactor);
            }
        }

        /// <summary>
        /// 获取相交坐标轴
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns>坐标轴</returns>
        public abstract Axis GetCrossAxis(GraphPane pane);

        /// <summary>
        /// 影响相交值
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns>相交值</returns>
        internal double EffectiveCrossValue(GraphPane pane)
        {
            Axis crossAxis = GetCrossAxis(pane);
            double min = crossAxis.m_scale.Linearize(crossAxis.m_scale.m_min);
            double max = crossAxis.m_scale.Linearize(crossAxis.m_scale.m_max);
            if (m_crossAuto)
            {
                if (crossAxis.m_scale.IsReverse == (this is Y2Axis))
                    return max;
                else
                    return min;
            }
            else if (m_cross < min)
                return min;
            else if (m_cross > max)
                return max;
            else
                return m_scale.Linearize(m_cross);
        }

        /// <summary>
        /// 修正零轴
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="left">左侧坐标</param>
        /// <param name="right">右侧坐标</param>
        internal void FixZeroLine(Graphics g, GraphPane pane, float scaleFactor,
                float left, float right)
        {
            if (m_isVisible && m_majorGrid.m_isZeroLine &&
                    m_scale.m_min < 0.0 && m_scale.m_max > 0.0)
            {
                float zeroPix = m_scale.Transform(0.0);
                using (Pen zeroPen = new Pen(m_color,
                        pane.ScaledPenWidth(m_majorGrid.m_penWidth, scaleFactor)))
                {
                    g.DrawLine(zeroPen, (int)left, (int)zeroPix, (int)right, (int)zeroPix);
                }
            }
        }

        /// <summary>
        /// 十字线是否位移
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns>是否位移</returns>
        internal bool IsCrossShifted(GraphPane pane)
        {
            if (m_crossAuto)
                return false;
            else
            {
                Axis crossAxis = GetCrossAxis(pane);
                if (((this is XAxis || this is YAxis) && !crossAxis.m_scale.IsReverse) ||
                    ((this is Y2Axis) && crossAxis.m_scale.IsReverse))
                {
                    if (m_cross <= crossAxis.m_scale.m_min)
                        return false;
                }
                else
                {
                    if (m_cross >= crossAxis.m_scale.m_max)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 判断是否主图层
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns>是否主图层</returns>
        internal abstract bool IsPrimary(GraphPane pane);

        /// <summary>
        /// 生成标签
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="index">索引</param>
        /// <param name="dVal">数值</param>
        /// <returns>标签</returns>
        internal String MakeLabelEventWorks(GraphPane pane, int index, double dVal)
        {
            if (this.Scale != null)
                return m_scale.MakeLabel(pane, index, dVal);
            else
                return "?";
        }

        /// <summary>
        /// 生成标题
        /// </summary>
        /// <returns>标题</returns>
        private String MakeTitle()
        {
            if (m_title.m_text == null)
                m_title.m_text = "";
            if (m_IsScientificNotation && m_scale.m_mag != 0 && !m_title.m_isOmitMag && !m_scale.IsLog)
                return m_title.m_text + String.Format(" (10^{0})", m_scale.m_mag);
            else
                return m_title.m_text;
        }

        /// <summary>
        /// 重置数据
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="g">绘图对象</param>
        public void ResetAutoScale(GraphPane pane, Graphics g)
        {
            m_scale.m_minAuto = true;
            m_scale.m_maxAuto = true;
            m_scale.m_majorStepAuto = true;
            m_scale.m_minorStepAuto = true;
            m_crossAuto = true;
            m_scale.m_magAuto = true;
            pane.AxisChange(g);
        }

        /// <summary>
        /// 设置最小值空间缓存
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="bufferFraction">分数</param>
        /// <param name="isGrowOnly">是否增长</param>
        public void SetMinSpaceBuffer(Graphics g, GraphPane pane, float bufferFraction,
                                        bool isGrowOnly)
        {
            float oldSpace = this.MinSpace;
            this.MinSpace = 0;
            float fixedSpace;
            float space = this.CalcSpace(g, pane, 1.0F, out fixedSpace) * bufferFraction;
            if (isGrowOnly)
                space = Math.Max(oldSpace, space);
            this.MinSpace = space;
        }

        /// <summary>
        /// 设置转换矩阵
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        public abstract void SetTransformMatrix(Graphics g, GraphPane pane, float scaleFactor);
        #endregion
    }
}
