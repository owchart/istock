/********************************************************************************\
*                                                                                *
* Scale.cs -    Scale functions, types, and definitions                          *
*                                                                                *
*               Version 1.00                                                     *
*                                                                                *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.      *
*                                                                                *
*********************************************************************************/

using System;
using System.Collections;
using System.Text;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace OwLib
{
    /// <summary>
    /// 刻度
    /// </summary>
    [Serializable]
    public abstract class Scale : IDisposable
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建刻度
        /// </summary>
        /// <param name="ownerAxis">所属对象</param>
        public Scale(Axis ownerAxis)
        {
            m_ownerAxis = ownerAxis;
            m_min = 0.0;
            m_max = 1.0;
            m_majorStep = 0.1;
            m_minorStep = 0.1;
            m_exponent = 1.0;
            m_mag = 0;
            m_baseTic = PointPair.Missing;
            m_minGrace = Default.MinGrace;
            m_maxGrace = Default.MaxGrace;
            m_minAuto = true;
            m_maxAuto = true;
            m_majorStepAuto = true;
            m_minorStepAuto = true;
            m_magAuto = true;
            m_formatAuto = true;
            m_isReverse = Default.IsReverse;
            m_isUseTenPower = true;
            m_isPreventLabelOverlap = true;
            m_isVisible = true;
            m_isSkipFirstLabel = false;
            m_isSkipLastLabel = false;
            m_isSkipCrossLabel = false;
            m_majorUnit = DateUnit.Day;
            m_minorUnit = DateUnit.Day;
            m_format = null;
            m_textLabels = null;
            m_isLabelsInside = Default.IsLabelsInside;
            m_align = Default.Align;
            m_alignH = Default.AlignH;
            m_fontSpec = new FontSpec(
                Default.FontFamily, Default.FontSize,
                Default.FontColor, Default.FontBold,
                Default.FontUnderline, Default.FontItalic,
                Default.FillColor, Default.FillBrush,
                Default.FillType);
            m_fontSpec.Border.IsVisible = false;
            m_labelGap = Default.LabelGap;
        }

        /// <summary>
        /// 创建刻度
        /// </summary>
        /// <param name="rhs">其他对象</param>
        /// <param name="owner">所属对象</param>
        public Scale(Scale rhs, Axis owner)
        {
            m_ownerAxis = owner;
            m_min = rhs.m_min;
            m_max = rhs.m_max;
            m_majorStep = rhs.m_majorStep;
            m_minorStep = rhs.m_minorStep;
            m_exponent = rhs.m_exponent;
            m_baseTic = rhs.m_baseTic;
            m_minAuto = rhs.m_minAuto;
            m_maxAuto = rhs.m_maxAuto;
            m_majorStepAuto = rhs.m_majorStepAuto;
            m_minorStepAuto = rhs.m_minorStepAuto;
            m_magAuto = rhs.m_magAuto;
            m_formatAuto = rhs.m_formatAuto;
            m_minGrace = rhs.m_minGrace;
            m_maxGrace = rhs.m_maxGrace;
            m_mag = rhs.m_mag;
            m_isUseTenPower = rhs.m_isUseTenPower;
            m_isReverse = rhs.m_isReverse;
            m_isPreventLabelOverlap = rhs.m_isPreventLabelOverlap;
            m_isVisible = rhs.m_isVisible;
            m_isSkipFirstLabel = rhs.m_isSkipFirstLabel;
            m_isSkipLastLabel = rhs.m_isSkipLastLabel;
            m_isSkipCrossLabel = rhs.m_isSkipCrossLabel;
            m_majorUnit = rhs.m_majorUnit;
            m_minorUnit = rhs.m_minorUnit;
            m_format = rhs.m_format;
            m_isLabelsInside = rhs.m_isLabelsInside;
            m_align = rhs.m_align;
            m_alignH = rhs.m_alignH;
            m_fontSpec = new FontSpec(rhs.m_fontSpec);
            m_labelGap = rhs.m_labelGap;
            if (rhs.m_textLabels != null)
                m_textLabels = (String[])rhs.m_textLabels.Clone();
            else
                m_textLabels = null;
        }

        /// <summary>
        /// 默认属性
        /// </summary>
        public struct Default
        {
            public static double ZeroLever = 0.25;
            public static double MinGrace = 0.1;
            public static double MaxGrace = 0.1;
            public static double MaxTextLabels = 12.0;
            public static double TargetXSteps = 12.0;
            public static double TargetYSteps = 12.0;
            public static double TargetMinorXSteps = 5.0;
            public static double TargetMinorYSteps = 5.0;
            public static bool IsReverse = false;
            public static String Format = "g";
            public static double RangeYearYear = 1825;
            public static double RangeYearMonth = 730;
            public static double RangeMonthMonth = 300;
            public static double RangeDayDay = 10;
            public static double RangeDayHour = 3;
            public static double RangeHourHour = 0.4167;
            public static double RangeHourMinute = 0.125;
            public static double RangeMinuteMinute = 6.94e-3;
            public static double RangeMinuteSecond = 2.083e-3;
            public static double RangeSecondSecond = 3.472e-5;
            public static String FormatYearYear = "yyyy";
            public static String FormatYearMonth = "MMM-yyyy";
            public static String FormatMonthMonth = "MMM-yyyy";
            public static String FormatDayDay = "d-MMM";
            public static String FormatDayHour = "d-MMM HH:mm";
            public static String FormatHourHour = "HH:mm";
            public static String FormatHourMinute = "HH:mm";
            public static String FormatMinuteMinute = "HH:mm";
            public static String FormatMinuteSecond = "mm:ss";
            public static String FormatSecondSecond = "mm:ss";
            public static String FormatMillisecond = "ss.fff";

            public static AlignP Align = AlignP.Center;
            public static AlignH AlignH = AlignH.Center;
            public static String FontFamily = "Arial";
            public static float FontSize = 14;
            public static Color FontColor = Color.FromArgb(30, 30, 30);
            public static bool FontBold = false;
            public static bool FontItalic = false;
            public static bool FontUnderline = false;
            public static Color FillColor = Color.White;
            public static Brush FillBrush = null;
            public static FillType FillType = FillType.None;
            public static bool IsVisible = true;
            public static bool IsLabelsInside = false;
            public static float EdgeTolerance = 6;
            public static float LabelGap = 0.3f;
        }

        internal double m_min,
                                m_max,
                                m_majorStep,
                                m_minorStep,
                                m_exponent,
                                m_baseTic;
        internal bool m_minAuto,
                                m_maxAuto,
                                m_majorStepAuto,
                                m_minorStepAuto,
                                m_magAuto,
                                m_formatAuto;
        internal double m_minGrace,
                                m_maxGrace;
        internal int m_mag;
        internal bool m_isReverse,
                                m_isPreventLabelOverlap,
                                m_isUseTenPower,
                                m_isLabelsInside,
                                m_isSkipFirstLabel,
                                m_isSkipLastLabel,
                                m_isSkipCrossLabel,
                                m_isVisible;
        internal String[] m_textLabels = null;
        internal String m_format;
        internal DateUnit m_majorUnit,
                                    m_minorUnit;
        internal AlignP m_align;
        internal AlignH m_alignH;
        internal FontSpec m_fontSpec;
        internal float m_labelGap;
        internal double m_rangeMin,
                                m_rangeMax,
                                m_lBound,
                                m_uBound;
        internal float m_minPix,
                            m_maxPix;
        internal double m_minLinTemp,
                                m_maxLinTemp;

        internal double m_minLinearized
        {
            get { return Linearize(m_min); }
            set { m_min = DeLinearize(value); }
        }

        internal double m_maxLinearized
        {
            get { return Linearize(m_max); }
            set { m_max = DeLinearize(value); }
        }

        internal Axis m_ownerAxis;

        public bool IsScientificNotation
        {
            get { return m_IsScientificNotation; }
            set { m_IsScientificNotation = value; }
        }

        private bool m_IsScientificNotation;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner">所属对象</param>
        /// <returns></returns>
        public abstract Scale Clone(Axis owner);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldScale">旧坐标</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public Scale MakeNewScale(Scale oldScale, AxisType type)
        {
            switch (type)
            {
                case AxisType.Linear:
                    return new LinearScale(oldScale, m_ownerAxis);
                case AxisType.Date:
                    return new DateScale(oldScale, m_ownerAxis);
                case AxisType.Log:
                    return new LogScale(oldScale, m_ownerAxis);
                case AxisType.Exponent:
                    return new ExponentScale(oldScale, m_ownerAxis);
                case AxisType.Ordinal:
                    return new OrdinalScale(oldScale, m_ownerAxis);
                case AxisType.Text:
                    return new TextScale(oldScale, m_ownerAxis);
                case AxisType.DateAsOrdinal:
                    return new DateAsOrdinalScale(oldScale, m_ownerAxis);
                case AxisType.LinearAsOrdinal:
                    return new LinearAsOrdinalScale(oldScale, m_ownerAxis);
                default:
                    throw new Exception("Implementation Error: Invalid AxisType");
            }
        }

        public abstract AxisType Type { get; }
        public bool IsLog { get { return this is LogScale; } }
        public bool IsExponent { get { return this is ExponentScale; } }
        public bool IsDate { get { return this is DateScale; } }
        public bool IsText { get { return this is TextScale; } }
        public bool IsOrdinal { get { return this is OrdinalScale; } }

        public bool IsAnyOrdinal
        {
            get
            {
                AxisType type = this.Type;
                return type == AxisType.Ordinal ||
                            type == AxisType.Text ||
                            type == AxisType.LinearAsOrdinal ||
                            type == AxisType.DateAsOrdinal;
            }
        }

        public virtual double Min
        {
            get { return m_min; }
            set { m_min = value; m_minAuto = false; }
        }

        public virtual double Max
        {
            get { return m_max; }
            set { m_max = value; m_maxAuto = false; }
        }

        public double MajorStep
        {
            get { return m_majorStep; }
            set
            {
                if (value < 1e-300)
                {
                    m_majorStepAuto = true;
                }
                else
                {
                    m_majorStep = value;
                    m_majorStepAuto = false;
                }
            }
        }

        public double MinorStep
        {
            get { return m_minorStep; }
            set
            {
                if (value < 1e-300)
                {
                    m_minorStepAuto = true;
                }
                else
                {
                    m_minorStep = value;
                    m_minorStepAuto = false;
                }
            }
        }

        public double Exponent
        {
            get { return m_exponent; }
            set { m_exponent = value; }
        }

        public double BaseTic
        {
            get { return m_baseTic; }
            set { m_baseTic = value; }
        }

        public DateUnit MajorUnit
        {
            get { return m_majorUnit; }
            set { m_majorUnit = value; }
        }

        public DateUnit MinorUnit
        {
            get { return m_minorUnit; }
            set { m_minorUnit = value; }
        }

        internal virtual double MajorUnitMultiplier
        {
            get { return 1.0; }
        }
        internal virtual double MinorUnitMultiplier
        {
            get { return 1.0; }
        }
        public bool MinAuto
        {
            get { return m_minAuto; }
            set { m_minAuto = value; }
        }
        public bool MaxAuto
        {
            get { return m_maxAuto; }
            set { m_maxAuto = value; }
        }
        public bool MajorStepAuto
        {
            get { return m_majorStepAuto; }
            set { m_majorStepAuto = value; }
        }
        public bool MinorStepAuto
        {
            get { return m_minorStepAuto; }
            set { m_minorStepAuto = value; }
        }
        public bool FormatAuto
        {
            get { return m_formatAuto; }
            set { m_formatAuto = value; }
        }
        public String Format
        {
            get { return m_format; }
            set { m_format = value; m_formatAuto = false; }
        }
        public int Mag
        {
            get { return m_mag; }
            set { m_mag = value; m_magAuto = false; }
        }
        public bool MagAuto
        {
            get { return m_magAuto; }
            set { m_magAuto = value; }
        }
        public double MinGrace
        {
            get { return m_minGrace; }
            set { m_minGrace = value; }
        }
        public double MaxGrace
        {
            get { return m_maxGrace; }
            set { m_maxGrace = value; }
        }
        public AlignP Align
        {
            get { return m_align; }
            set { m_align = value; }
        }
        public AlignH AlignH
        {
            get { return m_alignH; }
            set { m_alignH = value; }
        }
        public FontSpec FontSpec
        {
            get { return m_fontSpec; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Uninitialized FontSpec in Scale");
                m_fontSpec = value;
            }
        }
        public float LabelGap
        {
            get { return m_labelGap; }
            set { m_labelGap = value; }
        }
        public bool IsLabelsInside
        {
            get { return m_isLabelsInside; }
            set { m_isLabelsInside = value; }
        }
        public bool IsSkipFirstLabel
        {
            get { return m_isSkipFirstLabel; }
            set { m_isSkipFirstLabel = value; }
        }
        public bool IsSkipLastLabel
        {
            get { return m_isSkipLastLabel; }
            set { m_isSkipLastLabel = value; }
        }
        public bool IsSkipCrossLabel
        {
            get { return m_isSkipCrossLabel; }
            set { m_isSkipCrossLabel = value; }
        }
        public bool IsReverse
        {
            get { return m_isReverse; }
            set { m_isReverse = value; }
        }
        public bool IsUseTenPower
        {
            get { return m_isUseTenPower; }
            set { m_isUseTenPower = value; }
        }
        public bool IsPreventLabelOverlap
        {
            get { return m_isPreventLabelOverlap; }
            set { m_isPreventLabelOverlap = value; }
        }
        public bool IsVisible
        {
            get { return m_isVisible; }
            set { m_isVisible = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public String[] TextLabels
        {
            get { return m_textLabels; }
            set { m_textLabels = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="axis">坐标轴</param>
        public virtual void SetupScaleData(GraphPane pane, Axis axis)
        {
            if (axis is XAxis)
            {
                if (pane.HasScatterPlot)
                {
                    m_minPix = pane.Chart.m_rect.Left + 5;
                    m_maxPix = pane.Chart.m_rect.Right - 5;
                }
                else
                {
                    m_minPix = pane.Chart.m_rect.Left;
                    m_maxPix = pane.Chart.m_rect.Right;
                }
            }
            else
            {
                if (pane.HasScatterPlot)
                {
                    m_minPix = pane.Chart.m_rect.Top + 5;
                    m_maxPix = pane.Chart.m_rect.Bottom - 5;
                }
                else
                {
                    m_minPix = pane.Chart.m_rect.Top;
                    m_maxPix = pane.Chart.m_rect.Bottom;
                }
            }
            m_minLinTemp = Linearize(m_min);
            m_maxLinTemp = Linearize(m_max);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val">数值</param>
        /// <returns></returns>
        public virtual double Linearize(double val)
        {
            return val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="val">数值</param>
        /// <returns></returns>
        public virtual double DeLinearize(double val)
        {
            return val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="index">索引</param>
        /// <param name="dVal">数值</param>
        /// <returns></returns>
        internal virtual String MakeLabel(GraphPane pane, int index, double dVal)
        {
            if (m_format == null)
                m_format = Scale.Default.Format;
            double scaleMult = Math.Pow((double)10.0, m_mag);
            if (m_IsScientificNotation)
            {
                return (dVal / scaleMult).ToString(m_format);
            }
            else
            {
                return dVal.ToString(m_format);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="applyAngle">角度</param>
        /// <returns></returns>
        internal SizeF GetScaleMaxSpace(Graphics g, GraphPane pane, float scaleFactor,
                            bool applyAngle)
        {
            if (m_isVisible)
            {
                double dVal,
                    scaleMult = Math.Pow((double)10.0, m_mag);
                int i;
                float saveAngle = m_fontSpec.Angle;
                if (!applyAngle)
                    m_fontSpec.Angle = 0;
                int nTics = CalcNumTics();
                double startVal = CalcBaseTic();
                SizeF maxSpace = new SizeF(0, 0);
                for (i = 0; i < nTics; i++)
                {
                    dVal = CalcMajorTicValue(startVal, i);
                    String tmpStr = m_ownerAxis.MakeLabelEventWorks(pane, i, dVal);
                    SizeF sizeF;
                    if (this.IsLog && m_isUseTenPower)
                        sizeF = m_fontSpec.BoundingBoxTenPower(g, tmpStr,
                            scaleFactor);
                    else
                        sizeF = m_fontSpec.BoundingBox(g, tmpStr,
                            scaleFactor);
                    if (sizeF.Height > maxSpace.Height)
                        maxSpace.Height = sizeF.Height;
                    if (sizeF.Width > maxSpace.Width)
                        maxSpace.Width = sizeF.Width;
                }
                m_fontSpec.Angle = saveAngle;
                return maxSpace;
            }
            else
                return new SizeF(0, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseVal">基础值</param>
        /// <param name="tic">最小变动值</param>
        /// <returns></returns>
        internal virtual double CalcMajorTicValue(double baseVal, double tic)
        {
            return baseVal + (double)m_majorStep * tic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseVal">基础值</param>
        /// <param name="iTic">最小变动范围</param>
        /// <returns></returns>
        internal virtual double CalcMinorTicValue(double baseVal, int iTic)
        {
            return baseVal + (double)m_minorStep * (double)iTic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseVal">基础值</param>
        /// <returns></returns>
        internal virtual int CalcMinorStart(double baseVal)
        {
            return (int)((m_min - baseVal) / m_minorStep);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal virtual double CalcBaseTic()
        {
            if (m_baseTic != PointPair.Missing)
                return m_baseTic;
            else if (IsAnyOrdinal)
            {
                return 1;
            }
            else
            {
                return Math.Ceiling((double)m_min / (double)m_majorStep - 0.00000001)
                                                        * (double)m_majorStep;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="baseVal">基础值</param>
        /// <param name="nTics">最小变动值</param>
        /// <param name="topPix">顶部象素</param>
        /// <param name="shift">位移</param>
        /// <param name="scaleFactor">刻度因子</param>
        internal void DrawLabels(Graphics g, GraphPane pane, double baseVal, int nTics,
                        float topPix, float shift, float scaleFactor)
        {
            MajorTic tic = m_ownerAxis.m_majorTic;
            double dVal, dVal2;
            float pixVal, pixVal2;
            float scaledTic = tic.ScaledTic(scaleFactor);
            double scaleMult = Math.Pow((double)10.0, m_mag);
            using (Pen ticPen = tic.GetPen(pane, scaleFactor))
            {
                SizeF maxLabelSize = GetScaleMaxSpace(g, pane, scaleFactor, true);
                float charHeight = m_fontSpec.GetHeight(scaleFactor);
                float maxSpace = maxLabelSize.Height;
                float edgeTolerance = Default.EdgeTolerance * scaleFactor;
                double rangeTol = (m_maxLinTemp - m_minLinTemp) * 0.001;
                int firstTic = (int)((m_minLinTemp - baseVal) / m_majorStep + 0.99);
                if (firstTic < 0)
                    firstTic = 0;
                float lastPixVal = -10000;
                for (int i = firstTic; i < nTics + firstTic; i++)
                {
                    dVal = CalcMajorTicValue(baseVal, i);
                    if (dVal < m_minLinTemp)
                        continue;
                    if (dVal > m_maxLinTemp + rangeTol)
                        break;
                    pixVal = LocalTransform(dVal);
                    if (tic.m_isBetweenLabels && IsText)
                    {
                        if (i == 0)
                        {
                            dVal2 = CalcMajorTicValue(baseVal, -0.5);
                            if (dVal2 >= m_minLinTemp)
                            {
                                pixVal2 = LocalTransform(dVal2);
                                tic.Draw(g, pane, ticPen, pixVal2, topPix, shift, scaledTic);
                            }
                        }
                        dVal2 = CalcMajorTicValue(baseVal, (double)i + 0.5);
                        if (dVal2 > m_maxLinTemp)
                            break;
                        pixVal2 = LocalTransform(dVal2);
                    }
                    else
                        pixVal2 = pixVal;
                    tic.Draw(g, pane, ticPen, pixVal2, topPix, shift, scaledTic);
                    bool isMaxValueAtMaxPix = ((m_ownerAxis is XAxis || m_ownerAxis is Y2Axis) &&
                                                            !IsReverse) ||
                                                (m_ownerAxis is Y2Axis && IsReverse);
                    bool isSkipZone = (((m_isSkipFirstLabel && isMaxValueAtMaxPix) ||
                                            (m_isSkipLastLabel && !isMaxValueAtMaxPix)) &&
                                                pixVal < edgeTolerance) ||
                                        (((m_isSkipLastLabel && isMaxValueAtMaxPix) ||
                                            (m_isSkipFirstLabel && !isMaxValueAtMaxPix)) &&
                                                pixVal > m_maxPix - m_minPix - edgeTolerance);
                    bool isSkipCross = m_isSkipCrossLabel && !m_ownerAxis.m_crossAuto &&
                                    Math.Abs(m_ownerAxis.m_cross - dVal) < rangeTol * 10.0;
                    isSkipZone = isSkipZone || isSkipCross;
                    if (m_isVisible && !isSkipZone)
                    {
                        if (IsPreventLabelOverlap &&
                                Math.Abs(pixVal - lastPixVal) < maxLabelSize.Width)
                            continue;
                        if (pane.CurveList.Count != 0)
                            DrawLabel(g, pane, i, dVal, pixVal, shift, maxSpace, scaledTic, charHeight, scaleFactor);
                        lastPixVal = pixVal;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="baseVal">基础值</param>
        /// <param name="topPix">顶部象素</param>
        /// <param name="scaleFactor">刻度因子</param>
        internal void DrawGrid(Graphics g, GraphPane pane, double baseVal, float topPix, float scaleFactor)
        {
            MajorTic tic = m_ownerAxis.m_majorTic;
            MajorGrid grid = m_ownerAxis.m_majorGrid;
            int nTics = CalcNumTics();
            double dVal, dVal2;
            float pixVal, pixVal2;
            using (Pen gridPen = grid.GetPen(pane, scaleFactor))
            {
                double rangeTol = (m_maxLinTemp - m_minLinTemp) * 0.001;
                int firstTic = (int)((m_minLinTemp - baseVal) / m_majorStep + 0.99);
                if (firstTic < 0)
                    firstTic = 0;
                for (int i = firstTic; i < nTics + firstTic; i++)
                {
                    dVal = CalcMajorTicValue(baseVal, i);
                    if (dVal < m_minLinTemp)
                        continue;
                    if (dVal > m_maxLinTemp + rangeTol)
                        break;
                    pixVal = LocalTransform(dVal);
                    if (tic.m_isBetweenLabels && IsText)
                    {
                        if (i == 0)
                        {
                            dVal2 = CalcMajorTicValue(baseVal, -0.5);
                            if (dVal2 >= m_minLinTemp)
                            {
                                pixVal2 = LocalTransform(dVal2);
                                grid.Draw(g, gridPen, pixVal2, topPix);
                            }
                        }
                        dVal2 = CalcMajorTicValue(baseVal, (double)i + 0.5);
                        if (dVal2 > m_maxLinTemp)
                            break;
                        pixVal2 = LocalTransform(dVal2);
                    }
                    else
                        pixVal2 = pixVal;
                    grid.Draw(g, gridPen, pixVal2, topPix);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="i">索引</param>
        /// <param name="dVal">数值</param>
        /// <param name="pixVal">象素</param>
        /// <param name="shift">位移</param>
        /// <param name="maxSpace">最大空间</param>
        /// <param name="scaledTic">最小变动值</param>
        /// <param name="charHeight">高度</param>
        /// <param name="scaleFactor">刻度因子</param>
        internal void DrawLabel(Graphics g, GraphPane pane, int i, double dVal, float pixVal,
                        float shift, float maxSpace, float scaledTic, float charHeight, float scaleFactor)
        {
            float textTop, textCenter;
            if (m_ownerAxis.MajorTic.IsOutside)
                textTop = scaledTic + charHeight * m_labelGap;
            else
                textTop = charHeight * m_labelGap;
            String tmpStr = m_ownerAxis.MakeLabelEventWorks(pane, i, dVal);
            float height;
            if (this.IsLog && m_isUseTenPower)
                height = m_fontSpec.BoundingBoxTenPower(g, tmpStr, scaleFactor).Height;
            else
                height = m_fontSpec.BoundingBox(g, tmpStr, scaleFactor).Height;
            if (m_align == AlignP.Center)
                textCenter = textTop + maxSpace / 2.0F;
            else if (m_align == AlignP.Outside)
                textCenter = textTop + maxSpace - height / 2.0F;
            else	
                textCenter = textTop + height / 2.0F;
            if (m_isLabelsInside)
                textCenter = shift - textCenter;
            else
                textCenter = shift + textCenter;
            AlignV av = AlignV.Center;
            AlignH ah = AlignH.Center;
            if (m_ownerAxis is XAxis)
                ah = m_alignH;
            else
                av = m_alignH == AlignH.Left ? AlignV.Top : (m_alignH == AlignH.Right ? AlignV.Bottom : AlignV.Center);
            if (this.IsLog && m_isUseTenPower)
                m_fontSpec.DrawTenPower(g, pane, tmpStr,
                    pixVal, textCenter,
                    ah, av,
                    scaleFactor);
            else
                m_fontSpec.Draw(g, pane, tmpStr,
                    pixVal, textCenter,
                    ah, av,
                    scaleFactor, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="shiftPos">位移</param>
        internal void Draw(Graphics g, GraphPane pane, float scaleFactor, float shiftPos)
        {
            MajorGrid majorGrid = m_ownerAxis.m_majorGrid;
            MajorTic majorTic = m_ownerAxis.m_majorTic;
            MinorTic minorTic = m_ownerAxis.m_minorTic;
            float rightPix,
                    topPix;
            GetTopRightPix(pane, out topPix, out rightPix);
            int nTics = CalcNumTics();
            double baseVal = CalcBaseTic();
            using (Pen pen = new Pen(m_ownerAxis.Color))
            {
                if (m_ownerAxis.IsAxisSegmentVisible)
                {
                    if (m_ownerAxis is XAxis)
                    {
                        g.DrawLine(pen, 0, (int)shiftPos, (int)rightPix + 6, (int)shiftPos);
                    }
                    else if (m_ownerAxis is YAxis)
                    {
                        g.DrawLine(pen, -6, (int)shiftPos, (int)rightPix, (int)shiftPos);
                    }
                    else if (m_ownerAxis is Y2Axis)
                    {
                        g.DrawLine(pen, 0, (int)shiftPos, (int)rightPix + 6, (int)shiftPos);
                    }
                }
                if (majorGrid.m_isZeroLine && m_min < 0.0 && m_max > 0.0 && pane.m_HasZeroLine)
                {
                    float zeroPix = LocalTransform(0.0);
                    pen.Color = Color.FromArgb(30, 30, 30);
                    g.DrawLine(pen, zeroPix, 0, zeroPix, topPix);
                }
            }
            DrawLabels(g, pane, baseVal, nTics, topPix, shiftPos, scaleFactor);
            m_ownerAxis.DrawTitle(g, pane, shiftPos, scaleFactor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="topPix">顶部象素</param>
        /// <param name="rightPix">右侧象素</param>
        internal void GetTopRightPix(GraphPane pane, out float topPix, out float rightPix)
        {
            if (m_ownerAxis is XAxis)
            {
                rightPix = pane.Chart.m_rect.Width;
                topPix = -pane.Chart.m_rect.Height;
            }
            else
            {
                rightPix = pane.Chart.m_rect.Height;
                topPix = -pane.Chart.m_rect.Width;
            }
            if (m_min >= m_max)
                return;
            if (!IsLog)
            {
                if (m_majorStep <= 0 || m_minorStep <= 0)
                    return;
                double tMajor = (m_max - m_min) / (m_majorStep * MajorUnitMultiplier);
                double tMinor = (m_max - m_min) / (m_minorStep * MinorUnitMultiplier);
                MinorTic minorTic = m_ownerAxis.m_minorTic;
                if (tMajor > 1000 ||
                    ((minorTic.IsOutside || minorTic.IsInside || minorTic.IsOpposite)
                    && tMinor > 5000))
                    return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns></returns>
        public float GetClusterWidth(GraphPane pane)
        {
            double basisVal = m_min;
            return Math.Abs(Transform(basisVal +
                    (IsAnyOrdinal ? 1.0 : pane.m_barSettings.m_clusterScaleWidth)) -
                    Transform(basisVal));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clusterScaleWidth">重叠坐标宽度</param>
        /// <returns></returns>
        public float GetClusterWidth(double clusterScaleWidth)
        {
            double basisVal = m_min;
            return Math.Abs(Transform(basisVal + clusterScaleWidth) -
                    Transform(basisVal));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="g">绘图对象</param>
        /// <param name="scaleFactor">刻度因子</param>
        public virtual void PickScale(GraphPane pane, Graphics g, float scaleFactor)
        {
            double minVal = m_rangeMin;
            double maxVal = m_rangeMax;
            if (Double.IsInfinity(minVal) || Double.IsNaN(minVal) || minVal == Double.MaxValue)
                minVal = 0.0;
            if (Double.IsInfinity(maxVal) || Double.IsNaN(maxVal) || maxVal == Double.MaxValue)
                maxVal = 0.0;
            double range = maxVal - minVal;
            bool numType = !this.IsAnyOrdinal;
            if (m_minAuto)
            {
                m_min = minVal;
            }
            if (m_maxAuto)
            {
                m_max = maxVal;
            }
            if (m_max == m_min && m_maxAuto && m_minAuto)
            {
                if (Math.Abs(m_max) > 1e-100)
                {
                    m_max *= (m_min < 0 ? 0.95 : 1.05);
                    m_min *= (m_min < 0 ? 1.05 : 0.95);
                }
                else
                {
                    m_max = 1.0;
                    m_min = -1.0;
                }
            }
            if (m_max <= m_min)
            {
                if (m_maxAuto)
                    m_max = m_min + 1.0;
                else if (m_minAuto)
                    m_min = m_max - 1.0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <returns></returns>
        public int CalcMaxLabels(Graphics g, GraphPane pane, float scaleFactor)
        {
            SizeF size = this.GetScaleMaxSpace(g, pane, scaleFactor, false);
            float maxWidth = 1000;
            float temp = 1000;
            float costh = (float)Math.Abs(Math.Cos(m_fontSpec.Angle * Math.PI / 180.0));
            float sinth = (float)Math.Abs(Math.Sin(m_fontSpec.Angle * Math.PI / 180.0));
            if (costh > 0.001)
                maxWidth = size.Width / costh;
            if (sinth > 0.001)
                temp = size.Height / sinth;
            if (temp < maxWidth)
                maxWidth = temp;
            if (maxWidth <= 0)
                maxWidth = 1;
            double width;
            RectangleF chartRect = pane.Chart.m_rect;
            if (m_ownerAxis is XAxis)
                width = (chartRect.Width == 0) ? pane.Rect.Width * 0.75 : chartRect.Width;
            else
                width = (chartRect.Height == 0) ? pane.Rect.Height * 0.75 : chartRect.Height;
            int maxLabels = (int)(width / maxWidth);
            if (maxLabels <= 0)
                maxLabels = 1;
            return maxLabels;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="step">步长</param>
        internal void SetScaleMag(double min, double max, double step)
        {
            if (m_magAuto)
            {
                double mag = -100;
                double mag2 = -100;
                if (Math.Abs(m_min) > 1.0e-30)
                    mag = Math.Floor(Math.Log10(Math.Abs(m_min)));
                if (Math.Abs(m_max) > 1.0e-30)
                    mag2 = Math.Floor(Math.Log10(Math.Abs(m_max)));
                mag = Math.Max(mag2, mag);
                if (mag == -100 || Math.Abs(mag) <= 3)
                    mag = 0;
                m_mag = (int)(Math.Floor(mag / 3.0) * 3.0);
            }
            if (m_formatAuto)
            {
                int numDec = 0 - (int)(Math.Floor(Math.Log10(m_majorStep)) - m_mag);
                if (numDec < 0)
                    numDec = 0;
                m_format = "f" + numDec.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range">幅度</param>
        /// <param name="targetSteps">目标步长</param>
        /// <returns></returns>
        protected static double CalcStepSize(double range, double targetSteps)
        {
            double tempStep = range / targetSteps;
            double mag = Math.Floor(Math.Log10(tempStep));
            double magPow = Math.Pow((double)10.0, mag);
            double magMsd = ((int)(tempStep / magPow + .5));
            if (magMsd > 5.0)
                magMsd = 10.0;
            else if (magMsd > 2.0)
                magMsd = 5.0;
            else if (magMsd > 1.0)
                magMsd = 2.0;
            return magMsd * magPow;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range">幅度</param>
        /// <param name="maxSteps">最大步长</param>
        /// <returns></returns>
        protected double CalcBoundedStepSize(double range, double maxSteps)
        {
            double tempStep = range / maxSteps;
            double mag = Math.Floor(Math.Log10(tempStep));
            double magPow = Math.Pow((double)10.0, mag);
            double magMsd = Math.Ceiling(tempStep / magPow);
            if (magMsd > 5.0)
                magMsd = 10.0;
            else if (magMsd > 2.0)
                magMsd = 5.0;
            else if (magMsd > 1.0)
                magMsd = 2.0;
            return magMsd * magPow;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal virtual int CalcNumTics()
        {
            int nTics = 1;
            nTics = (int)((m_max - m_min) / m_majorStep + 0.01) + 1;
            if (nTics < 1)
                nTics = 1;
            else if (nTics > 1000)
                nTics = 1000;
            return nTics;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <returns></returns>
        protected double MyMod(double x, double y)
        {
            double temp;
            if (y == 0)
                return 0;
            temp = x / y;
            return y * (temp - Math.Floor(temp));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="axis">坐标轴</param>
        internal void SetRange(GraphPane pane, Axis axis)
        {
            if (m_rangeMin >= Double.MaxValue || m_rangeMax <= Double.MinValue)
            {
                if (axis != pane.XAxis &&
                    pane.YAxis.Scale.m_rangeMin < double.MaxValue && pane.YAxis.Scale.m_rangeMax > double.MinValue)
                {
                    m_rangeMin = pane.YAxis.Scale.m_rangeMin;
                    m_rangeMax = pane.YAxis.Scale.m_rangeMax;
                }
                else if (axis != pane.XAxis &&
                    pane.Y2Axis.Scale.m_rangeMin < double.MaxValue && pane.Y2Axis.Scale.m_rangeMax > double.MinValue)
                {
                    m_rangeMin = pane.Y2Axis.Scale.m_rangeMin;
                    m_rangeMax = pane.Y2Axis.Scale.m_rangeMax;
                }
                else
                {
                    m_rangeMin = 0;
                    m_rangeMax = 1;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <returns></returns>
        public float Transform(double x)
        {
            double denom = (m_maxLinTemp - m_minLinTemp);
            double ratio;
            if (denom > 1e-100)
                ratio = (Linearize(x) - m_minLinTemp) / denom;
            else
                ratio = 0;
            if (m_isReverse == (m_ownerAxis is XAxis))
                return (float)(m_maxPix - (m_maxPix - m_minPix) * ratio);
            else
                return (float)(m_minPix + (m_maxPix - m_minPix) * ratio);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isOverrideOrdinal">是否覆盖序数</param>
        /// <param name="i">索引</param>
        /// <param name="x">横坐标值</param>
        /// <returns></returns>
        public float Transform(bool isOverrideOrdinal, int i, double x)
        {
            if (this.IsAnyOrdinal && i >= 0 && !isOverrideOrdinal)
                x = (double)i + 1.0;
            return Transform(x);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pixVal">象素</param>
        /// <returns></returns>
        public double ReverseTransform(float pixVal)
        {
            double val;
            if ((m_isReverse) == (m_ownerAxis is XAxis))
                val = (double)(pixVal - m_maxPix)
                        / (double)(m_minPix - m_maxPix)
                        * (m_maxLinTemp - m_minLinTemp) + m_minLinTemp;
            else
                val = (double)(pixVal - m_minPix)
                        / (double)(m_maxPix - m_minPix)
                        * (m_maxLinTemp - m_minLinTemp) + m_minLinTemp;
            return DeLinearize(val);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <returns></returns>
        public float LocalTransform(double x)
        {
            double ratio;
            float rv;
            ratio = (x - m_minLinTemp) /
                        (m_maxLinTemp - m_minLinTemp);
            if (m_isReverse == (m_ownerAxis is YAxis))
                rv = (float)((m_maxPix - m_minPix) * ratio);
            else
                rv = (float)((m_maxPix - m_minPix) * (1.0F - ratio));
            return rv;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <returns></returns>
        public static double SafeLog(double x)
        {
            if (x > 1.0e-20)
                return Math.Log10(x);
            else
                return 0.0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="exponent">指数</param>
        /// <returns></returns>
        public static double SafeExp(double x, double exponent)
        {
            if (x > 1.0e-20)
                return Math.Pow(x, exponent);
            else
                return 0.0;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (m_fontSpec != null)
                m_fontSpec.Dispose();
        }
        #endregion
    }
}
