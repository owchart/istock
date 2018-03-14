/*****************************************************************************\
*                                                                             *
* Legend.cs -    Legend functions, types, and definitions                     *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Collections.Generic;
namespace OwLib
{
    /// <summary>
    /// 图例
    /// </summary>
    [Serializable]
    public class Legend : IDisposable
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建图例
        /// </summary>
        public Legend()
        {
            m_position = Default.Position;
            m_isHStack = Default.IsHStack;
            m_isVisible = Default.IsVisible;
            this.Location = new Location(0, 0, CoordType.PaneFraction);
            m_fontSpec = new FontSpec(Default.FontFamily, Default.FontSize,
                Default.FontColor, Default.FontBold,
                Default.FontItalic, Default.FontUnderline,
                Default.FontFillColor, Default.FontFillBrush,
                Default.FontFillType);
            m_fontSpec.Border.IsVisible = false;
            m_border = new Border(Default.IsBorderVisible, Default.BorderColor, Default.BorderWidth);
            m_fill = new Fill(Default.FillColor, Default.FillBrush, Default.FillType);
            m_gap = Default.Gap;
            m_isReverse = Default.IsReverse;
            m_isShowLegendSymbols = Default.IsShowLegendSymbols;
        }

        /// <summary>
        /// 创建图例
        /// </summary>
        /// <param name="rhs">其他对象</param>
        public Legend(Legend rhs)
        {
            m_rect = rhs.Rect;
            m_position = rhs.Position;
            m_isHStack = rhs.IsHStack;
            m_isVisible = rhs.IsVisible;
            m_location = rhs.Location;
            m_border = new Border(rhs.Border);
            m_fill = new Fill(rhs.Fill);
            m_fontSpec = new FontSpec(rhs.FontSpec);
            m_gap = rhs.m_gap;
            m_isReverse = rhs.m_isReverse;
            m_isShowLegendSymbols = rhs.m_isShowLegendSymbols;
        }

        /// <summary>
        /// 默认属性
        /// </summary>
        public struct Default
        {
            public static float BorderWidth = 1;
            public static Color BorderColor = Color.FromArgb(30, 30, 30);
            public static Color FillColor = Color.White;
            public static Brush FillBrush = null;
            public static FillType FillType = FillType.Brush;
            public static LegendPos Position = LegendPos.Top;
            public static bool IsBorderVisible = true;
            public static bool IsVisible = true;
            public static bool IsFilled = true;
            public static bool IsHStack = true;
            public static String FontFamily = "Arial";
            public static float FontSize = 12;
            public static Color FontColor = Color.FromArgb(30, 30, 30);
            public static bool FontBold = false;
            public static bool FontItalic = false;
            public static bool FontUnderline = false;
            public static Color FontFillColor = Color.White;
            public static Brush FontFillBrush = null;
            public static FillType FontFillType = FillType.None;
            public static float Gap = 0.5f;
            public static bool IsReverse = false;
            public static bool IsShowLegendSymbols = true;
        }

        private int m_hStack;
        private List<LegendCheckBox> LegendCheckBoxes = new List<LegendCheckBox>();
        private float m_legendItemWidth;
        private float m_legendItemHeight;
        private float m_tmpSize;

        private Border m_border;

        /// <summary>
        /// 获取或设置边线
        /// </summary>
        public Border Border
        {
            get { return m_border; }
            set { m_border = value; }
        }

        private Fill m_fill;

        /// <summary>
        /// 获取或设置填充
        /// </summary>
        public Fill Fill
        {
            get { return m_fill; }
            set { m_fill = value; }
        }

        private FontSpec m_fontSpec;

        /// <summary>
        /// 获取或设置字体
        /// </summary>
        public FontSpec FontSpec
        {
            get { return m_fontSpec; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Uninitialized FontSpec in Legend");
                m_fontSpec = value;
            }
        }

        private float m_gap;

        /// <summary>
        /// 获取或设置间隔
        /// </summary>
        public float Gap
        {
            get { return m_gap; }
            set { m_gap = value; }
        }

        private bool m_isHStack;

        /// <summary>
        /// 获取或设置横轴栈
        /// </summary>
        public bool IsHStack
        {
            get { return m_isHStack; }
            set { m_isHStack = value; }
        }

        private Location m_location;

        /// <summary>
        /// 获取或设置位置
        /// </summary>
        public Location Location
        {
            get { return m_location; }
            set { m_location = value; }
        }

        private bool m_isReverse;

        /// <summary>
        /// 获取或设置是否反转
        /// </summary>
        public bool IsReverse
        {
            get { return m_isReverse; }
            set { m_isReverse = value; }
        }

        bool m_IsShowLegendCheckBox = true;

        /// <summary>
        /// 获取或设置是否显示图例复选框
        /// </summary>
        public bool IsShowLegendCheckBox
        {
            get { return m_IsShowLegendCheckBox; }
            set { m_IsShowLegendCheckBox = value; }
        }

        private bool m_isShowLegendSymbols;

        /// <summary>
        /// 获取或设置是否显示标记
        /// </summary>
        public bool IsShowLegendSymbols
        {
            get { return m_isShowLegendSymbols; }
            set { m_isShowLegendSymbols = value; }
        }

        private bool m_isVisible;

        /// <summary>
        /// 获取或设置是否可见
        /// </summary>
        public bool IsVisible
        {
            get { return m_isVisible; }
            set { m_isVisible = value; }
        }

        private LegendPos m_position;

        /// <summary>
        /// 获取或设置图例位置
        /// </summary>
        public LegendPos Position
        {
            get { return m_position; }
            set { m_position = value; }
        }

        private RectangleF m_rect;

        /// <summary>
        /// 获取或设置区域
        /// </summary>
        public RectangleF Rect
        {
            get { return m_rect; }
        }


        /// <summary>
        /// 计算区域
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="tChartRect">区域</param>
        public void CalcRect(Graphics g, PaneBase pane, float scaleFactor,
            ref RectangleF tChartRect)
        {
            m_rect = Rectangle.Empty;
            m_hStack = 1;
            m_legendItemWidth = 1;
            m_legendItemHeight = 0;
            RectangleF clientRect = pane.CalcClientRect(g, scaleFactor);
            if (!m_isVisible)
                return;
            GraphPane grpahPane = pane as GraphPane;
            m_tmpSize = GetMaxHeight(grpahPane, g, scaleFactor);
            int nCurve = 0;
            float halfGap = m_tmpSize / 2.0F,
                    maxWidth = 0,
                    tmpWidth,
                    gapPix = m_gap * m_tmpSize;
            if (grpahPane != null && grpahPane.CurveList.Count == 1 && grpahPane.CurveList[0] is Pie) // When there is only one Pie in a pane.We make PieItem as Ledgend.
            {
                CurveItem curve = grpahPane.CurveList[0];
                FontSpec tmpFont = (curve.m_label.m_fontSpec != null) ?
                                curve.m_label.m_fontSpec : this.FontSpec;
                Pie pie = grpahPane.CurveList[0] as Pie;
                foreach (PieItem pieItem in pie.Slices)
                {
                    tmpWidth = tmpFont.GetWidth(g, pieItem.m_label.m_text, scaleFactor);
                    if (tmpWidth > maxWidth)
                        maxWidth = tmpWidth;
                    nCurve++;
                }
            }
            else
            {
                if (grpahPane != null)
                {
                    int count = grpahPane.CurveList.Count;
                    for (int i = 0; i < count; i++)
                    {
                        CurveItem curve = grpahPane.CurveList[m_isReverse ? count - i - 1 : i];
                        if (curve.m_label.m_text != String.Empty && curve.m_label.m_isVisible)
                        {
                            FontSpec tmpFont = (curve.m_label.m_fontSpec != null) ?
                                            curve.m_label.m_fontSpec : this.FontSpec;
                            tmpWidth = tmpFont.GetWidth(g, curve.m_label.m_text, scaleFactor);
                            if (tmpWidth > maxWidth)
                                maxWidth = tmpWidth;
                            if (curve is LineItem && ((LineItem)curve).Symbol.Size > m_legendItemHeight)
                                m_legendItemHeight = ((LineItem)curve).Symbol.Size;
                            nCurve++;
                        }
                    }
                }
            }
            float widthAvail;
            if (m_isHStack)
            {
                switch (m_position)
                {
                    case LegendPos.Right:
                    case LegendPos.Left:
                        widthAvail = 0;
                        break;
                    case LegendPos.Top:
                    case LegendPos.TopCenter:
                    case LegendPos.Bottom:
                    case LegendPos.BottomCenter:
                    case LegendPos.TopRight:
                        widthAvail = tChartRect.Width;
                        break;
                    case LegendPos.TopFlushLeft:
                    case LegendPos.BottomFlushLeft:
                        widthAvail = clientRect.Width;
                        break;
                    case LegendPos.InsideTopRight:
                    case LegendPos.InsideTopLeft:
                    case LegendPos.InsideBotRight:
                    case LegendPos.InsideBotLeft:
                    case LegendPos.Float:
                        widthAvail = tChartRect.Width / 2;
                        break;
                    default:
                        widthAvail = 0;
                        break;
                }
                if (m_isShowLegendSymbols)
                    m_legendItemWidth = 3.0f * m_tmpSize + maxWidth;
                else
                    m_legendItemWidth = 0.5f * m_tmpSize + maxWidth;
                if (maxWidth > 0)
                    m_hStack = (int)((widthAvail - halfGap) / m_legendItemWidth);
                if (m_hStack > nCurve)
                    m_hStack = nCurve;
                if (m_hStack == 0)
                    m_hStack = 1;
            }
            else
            {
                if (m_isShowLegendSymbols)
                    m_legendItemWidth = 3.0F * m_tmpSize + maxWidth;
                else
                    m_legendItemWidth = 0.5F * m_tmpSize + maxWidth;
            }
            float totLegWidth = m_hStack * m_legendItemWidth;
            m_legendItemHeight = m_legendItemHeight * (float)scaleFactor + halfGap;
            if (m_tmpSize > m_legendItemHeight)
                m_legendItemHeight = m_tmpSize;
            float totLegHeight = (float)Math.Ceiling((double)nCurve / (double)m_hStack)
                * m_legendItemHeight + 1;
            RectangleF newRect = new RectangleF();
            if (nCurve > 0)
            {
                newRect = new RectangleF(0, 0, totLegWidth, totLegHeight);
                switch (m_position)
                {
                    case LegendPos.Right:
                        newRect.X = clientRect.Right - totLegWidth;
                        newRect.Y = tChartRect.Top;
                        tChartRect.Width -= totLegWidth + gapPix;
                        break;
                    case LegendPos.Top:
                        newRect.X = tChartRect.Left;
                        newRect.Y = clientRect.Top;
                        tChartRect.Y += totLegHeight + gapPix;
                        tChartRect.Height -= totLegHeight + gapPix;
                        break;
                    case LegendPos.TopRight:
                        newRect.X = tChartRect.Left + (tChartRect.Width - totLegWidth);
                        newRect.Y = tChartRect.Top;
                        tChartRect.Y += totLegHeight + gapPix;
                        tChartRect.Height -= totLegHeight + gapPix;
                        break;
                    case LegendPos.TopFlushLeft:
                        newRect.X = clientRect.Left;
                        newRect.Y = clientRect.Top;
                        tChartRect.Y += totLegHeight + gapPix * 1.5f;
                        tChartRect.Height -= totLegHeight + gapPix * 1.5f;
                        break;
                    case LegendPos.TopCenter:
                        newRect.X = tChartRect.Left + (tChartRect.Width - totLegWidth) / 2;
                        newRect.Y = tChartRect.Top;
                        tChartRect.Y += totLegHeight + gapPix;
                        tChartRect.Height -= totLegHeight + gapPix;
                        break;
                    case LegendPos.Bottom:
                        newRect.X = tChartRect.Left;
                        newRect.Y = clientRect.Bottom - totLegHeight;
                        tChartRect.Height -= totLegHeight + gapPix;
                        break;
                    case LegendPos.BottomFlushLeft:
                        newRect.X = clientRect.Left;
                        newRect.Y = clientRect.Bottom - totLegHeight;
                        tChartRect.Height -= totLegHeight + gapPix;
                        break;
                    case LegendPos.BottomCenter:
                        newRect.X = tChartRect.Left + (tChartRect.Width - totLegWidth) / 2;
                        newRect.Y = clientRect.Bottom - totLegHeight;
                        tChartRect.Height -= totLegHeight + gapPix;
                        break;
                    case LegendPos.Left:
                        newRect.X = clientRect.Left;
                        newRect.Y = tChartRect.Top;
                        tChartRect.X += totLegWidth + halfGap;
                        tChartRect.Width -= totLegWidth + gapPix;
                        break;
                    case LegendPos.InsideTopRight:
                        newRect.X = tChartRect.Right - totLegWidth;
                        newRect.Y = tChartRect.Top;
                        break;
                    case LegendPos.InsideTopLeft:
                        newRect.X = tChartRect.Left;
                        newRect.Y = tChartRect.Top;
                        break;
                    case LegendPos.InsideBotRight:
                        newRect.X = tChartRect.Right - totLegWidth;
                        newRect.Y = tChartRect.Bottom - totLegHeight;
                        break;
                    case LegendPos.InsideBotLeft:
                        newRect.X = tChartRect.Left;
                        newRect.Y = tChartRect.Bottom - totLegHeight;
                        break;
                    case LegendPos.Float:
                        newRect.Location = this.Location.TransformTopLeft(pane, totLegWidth, totLegHeight);
                        break;
                }
            }
            m_rect = newRect;
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public void Dispose()
        {
            if (Fill != null)
                Fill.Dispose();
            if (LegendCheckBoxes != null)
            {
                foreach (LegendCheckBox checkbox in LegendCheckBoxes)
                    checkbox.Dispose();
                LegendCheckBoxes.Clear();
            }
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        public void Draw(Graphics g, PaneBase pane, float scaleFactor)
        {
            if (!m_isVisible)
                return;
            m_fill.Draw(g, m_rect);
            List<PaneBase> paneList = GetPaneList(pane);
            float halfGap = m_tmpSize / 2.0F;
            if (m_hStack <= 0)
                m_hStack = 1;
            if (m_legendItemWidth <= 0)
                m_legendItemWidth = 100;
            if (m_legendItemHeight <= 0)
                m_legendItemHeight = m_tmpSize;
            int iEntry = 0;
            float x, y;
            using (SolidBrush brushB = new SolidBrush(Color.FromArgb(30, 30, 30)))
            {
                foreach (GraphPane tmpPane in paneList)
                {
                    int count = tmpPane.CurveList.Count;
                    Pie pie = null;
                    if (tmpPane.CurveList.Count > 0)
                    {
                        pie = tmpPane.CurveList[0] as Pie;
                    }
                    if (pie != null && count == 1) 
                    {
                        foreach (PieItem pieItem in pie.Slices)
                        {
                            x = m_rect.Left + halfGap / 2.0F +
                                (iEntry % m_hStack) * m_legendItemWidth;
                            y = m_rect.Top + (int)(iEntry / m_hStack) * m_legendItemHeight;
                            FontSpec tmpFont = (pie.m_label.m_fontSpec != null) ?
                                        pie.m_label.m_fontSpec : this.FontSpec;
                            tmpFont.StringAlignment = StringAlignment.Near;
                            RectangleF rect = new RectangleF(x, y + m_legendItemHeight / 4.0F,
                                2 * m_tmpSize, m_legendItemHeight / 2.0F);
                            pieItem.DrawLegendKey(g, tmpPane, rect, scaleFactor);
                            tmpFont.Draw(g, pane, pieItem.m_label.m_text,
                                    x + 2.5F * m_tmpSize, y + m_legendItemHeight / 2.0F,
                                    AlignH.Left, AlignV.Center, scaleFactor);
                            iEntry++;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < count; i++)
                        {
                            CurveItem curve = tmpPane.CurveList[m_isReverse ? count - i - 1 : i];
                            if (curve.m_label.m_text != "" && curve.m_label.m_isVisible)
                            {
                                x = m_rect.Left + halfGap / 2.0F +
                                    (iEntry % m_hStack) * m_legendItemWidth;
                                y = m_rect.Top + (int)(iEntry / m_hStack) * m_legendItemHeight;
                                FontSpec tmpFont = (curve.m_label.m_fontSpec != null) ?
                                            curve.m_label.m_fontSpec : this.FontSpec;
                                tmpFont.StringAlignment = StringAlignment.Near;
                                if (m_isShowLegendSymbols)
                                {
                                    if (m_IsShowLegendCheckBox)
                                    {
                                        RectangleF rect = new RectangleF(x - 2, y + m_legendItemHeight / 4.0F - 2, m_legendItemHeight / 2.0F + 4, m_legendItemHeight / 2.0F + 4);
                                        curve.legendCheckBox.Draw(g, rect);
                                        rect = new RectangleF(x + m_legendItemHeight / 2.0F + m_legendItemHeight / 4.0F, y + m_legendItemHeight / 4.0F,
                                            2 * m_tmpSize - m_legendItemHeight / 2.0F, m_legendItemHeight / 2.0F);
                                        curve.DrawLegendKey(g, tmpPane, new RectangleF(rect.X + 2, rect.Y, rect.Width, rect.Height), scaleFactor);
                                        tmpFont.Draw(g, pane, curve.m_label.m_text,
                                                x + 2.5F * m_tmpSize, y + m_legendItemHeight / 2.0F,
                                                AlignH.Left, AlignV.Center, scaleFactor);
                                    }
                                    else
                                    {
                                        RectangleF rect = new RectangleF(x, y + m_legendItemHeight / 4.0F,
                                            2 * m_tmpSize, m_legendItemHeight / 2.0F);
                                        curve.DrawLegendKey(g, tmpPane, rect, scaleFactor);
                                        tmpFont.Draw(g, pane, curve.m_label.m_text,
                                                x + 2.5F * m_tmpSize, y + m_legendItemHeight / 2.0F,
                                                AlignH.Left, AlignV.Center, scaleFactor);
                                    }
                                }
                                else
                                {
                                    if (curve.m_label.m_fontSpec == null)
                                        tmpFont.FontColor = curve.Color;
                                    tmpFont.Draw(g, pane, curve.m_label.m_text,
                                        x + 0.0F * m_tmpSize, y + m_legendItemHeight / 2.0F,
                                        AlignH.Left, AlignV.Center, scaleFactor);
                                }
                                iEntry++;
                            }
                        }
                    }
                    if (pane is MasterPane && ((MasterPane)pane).IsUniformLegendEntries)
                        break;
                }
            }
        }

        /// <summary>
        /// 查找点
        /// </summary>
        /// <param name="mousePt">坐标</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public bool FindPoint(PointF mousePt, PaneBase pane, float scaleFactor, out int index)
        {
            index = -1;
            if (m_rect.Contains(mousePt))
            {
                int j = (int)((mousePt.Y - m_rect.Top) / m_legendItemHeight);
                int i = (int)((mousePt.X - m_rect.Left - m_tmpSize / 2.0f) / m_legendItemWidth);
                if (i < 0)
                    i = 0;
                if (i >= m_hStack)
                    i = m_hStack - 1;
                int pos = i + j * m_hStack;
                index = 0;
                List<PaneBase> paneList = GetPaneList(pane);
                foreach (GraphPane tmpPane in paneList)
                {
                    foreach (CurveItem curve in tmpPane.CurveList)
                    {
                        if (curve.m_label.m_isVisible && curve.m_label.m_text != String.Empty)
                        {
                            if (pos == 0)
                                return true;
                            pos--;
                        }
                        index++;
                    }
                }
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 获取最大高度
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="g">绘图对象</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <returns></returns>
        private float GetMaxHeight(GraphPane pane, Graphics g, float scaleFactor)
        {
            float defaultCharHeight = this.FontSpec.GetHeight(scaleFactor);
            float maxCharHeight = defaultCharHeight;
            if (pane != null)
            {
                foreach (CurveItem curve in pane.CurveList)
                {
                    if (curve.m_label.m_text != String.Empty && curve.m_label.m_isVisible)
                    {
                        float tmpHeight = defaultCharHeight;
                        if (curve.m_label.m_fontSpec != null)
                            tmpHeight = curve.m_label.m_fontSpec.GetHeight(scaleFactor);
                        tmpHeight *= curve.m_label.m_text.Split('\n').Length;
                        if (tmpHeight > maxCharHeight)
                            maxCharHeight = tmpHeight;
                    }
                }
            }
            return maxCharHeight;
        }

        /// <summary>
        /// 获取图层列表
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns></returns>
        private List<PaneBase> GetPaneList(PaneBase pane)
        {
            List<PaneBase> paneList;
            if (pane is GraphPane)
            {
                paneList = new List<PaneBase>();
                paneList.Add((GraphPane)pane);
            }
            else
                paneList = ((MasterPane)pane).PaneList;
            return paneList;
        }
        #endregion
    }
}
