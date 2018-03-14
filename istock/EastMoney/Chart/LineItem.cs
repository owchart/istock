/*****************************************************************************\
*                                                                             *
* LineItem.cs -  LineItem functions, types, and definitions                   *
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
using System.Collections.Generic;

namespace OwLib
{
    /// <summary>
    /// 线项
    /// </summary>
    [Serializable]
    public class LineItem : CurveItem
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建线项
        /// </summary>
        /// <param name="label">标签</param>
        public LineItem(String label)
            : base(label)
        {
            m_symbol = new Symbol();
            m_line = new Line();
        }

        /// <summary>
        /// 创建线项
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="x">横坐标值列表</param>
        /// <param name="y">纵坐标值列表</param>
        /// <param name="color">颜色</param>
        /// <param name="symbolType">标记类型</param>
        /// <param name="lineWidth">线宽</param>
        public LineItem(String label, double[] x, double[] y, Color color, SymbolType symbolType, float lineWidth)
            : this(label, new PointPairList(x, y), color, symbolType, lineWidth)
        {
        }

        /// <summary>
        /// 创建线项
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="x">横坐标值列表</param>
        /// <param name="y">纵坐标值列表</param>
        /// <param name="color">颜色</param>
        /// <param name="symbolType">标记类型</param>
        public LineItem(String label, double[] x, double[] y, Color color, SymbolType symbolType)
            : this(label, new PointPairList(x, y), color, symbolType)
        {
        }

        /// <summary>
        /// 创建线项
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="points">点集</param>
        /// <param name="color">颜色</param>
        /// <param name="symbolType">标记类型</param>
        /// <param name="lineWidth">线宽</param>
        public LineItem(String label, IPointList points, Color color, SymbolType symbolType, float lineWidth)
            : base(label, points)
        {
            m_line = new Line(color);
            if (lineWidth == 0)
                m_line.IsVisible = false;
            else
                m_line.Width = lineWidth;
            m_symbol = new Symbol(symbolType, color);
        }

        /// <summary>
        /// 创建线项
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="points">点集</param>
        /// <param name="color">颜色</param>
        /// <param name="symbolType">标记类型</param>
        public LineItem(String label, IPointList points, Color color, SymbolType symbolType)
            : this(label, points, color, symbolType, LineBase.Default.Width)
        {
        }

        /// <summary>
        /// 创建线项
        /// </summary>
        /// <param name="rhs">其他对象</param>
        public LineItem(LineItem rhs)
            : base(rhs)
        {
            m_symbol = new Symbol(rhs.Symbol);
            m_line = new Line(rhs.Line);
        }

        private bool isAreaChart;

        /// <summary>
        /// 获取或设置是否区域图
        /// </summary>
        public bool IsAreaChart
        {
            get { return isAreaChart; }
            set { isAreaChart = value; }
        }

        private bool isScatterPlot;

        /// <summary>
        /// 获取或设置是否散点图
        /// </summary>
        public bool IsScatterPlot
        {
            get { return isScatterPlot; }
            set { isScatterPlot = value; }
        }

        protected Line m_line;

        /// <summary>
        /// 获取或设置线
        /// </summary>
        public Line Line
        {
            get { return m_line; }
            set { m_line = value; }
        }

        protected Symbol m_symbol;

        /// <summary>
        /// 获取或设置标记
        /// </summary>
        public Symbol Symbol
        {
            get { return m_symbol; }
            set { m_symbol = value; }
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="pos">位置</param>
        /// <param name="scaleFactor">刻度因子</param>
        public override void Draw(Graphics g, GraphPane pane, int pos, float scaleFactor)
        {
            if (m_isVisible)
            {
                SmoothingMode smoothMode = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.HighQuality;
                if (IsScatterPlot)
                {
                    Symbol.Draw(g, pane, this, scaleFactor, IsSelected);
                }
                else
                {
                    Line.Draw(g, pane, this, scaleFactor);
                    if (m_isSelected)
                    {
                        Symbol.Draw(g, pane, this, scaleFactor, IsSelected);
                    }
                }
                g.SmoothingMode = smoothMode;
            }
        }

        /// <summary>
        /// 绘制图例键
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="rect">区域</param>
        /// <param name="scaleFactor">刻度因子</param>
        public override void DrawLegendKey(Graphics g, GraphPane pane, RectangleF rect, float scaleFactor)
        {
            SmoothingMode smoothMode = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.HighQuality;
            int xMid = (int)(rect.Left + rect.Width / 2.0F);
            int yMid = (int)(rect.Top + rect.Height / 2.0F);
            if (this.isScatterPlot)
            {
                m_symbol.Fill.Type = FillType.GradientByColorValue;
                g.FillEllipse(m_symbol.Fill.MakeBrush(rect), rect.X + 4, rect.Y - 3, rect.Width / 2, rect.Width / 2);
            }
            else if (this.isAreaChart)
            {
                PointF p1 = new PointF(rect.X, rect.Bottom);
                PointF p2 = new PointF(rect.X + rect.Width / 4, rect.Y - 2);
                PointF p3 = new PointF(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
                PointF p4 = new PointF(rect.X + rect.Width * 3 / 4, rect.Y + rect.Height / 2 - 2);
                PointF p5 = new PointF(rect.X + rect.Width, rect.Y + rect.Height);
                GraphicsPath gp = new GraphicsPath();
                gp.AddCurve(new PointF[] { p1, p2, p3, p4, p5 });
                gp.CloseFigure();
                g.FillPath(m_symbol.Fill.MakeBrush(rect), gp);
                gp.Dispose();
            }
            else
            {
                using (Pen pen = new Pen(this.Color))
                {
                    List<PointF> sineList = new List<PointF>();
                    double f = 2.0 * Math.PI / ((rect.Width));
                    for (float x = 0; x <= rect.Width; x++)
                    {
                        float y = rect.Height / 2 * (1 - (float)Math.Sin(x * 2 * Math.PI / (rect.Width - 1)));
                        sineList.Add(new PointF(x + rect.X, y + rect.Y));
                    }
                    PointF[] pf = sineList.ToArray();
                    g.DrawCurve(pen, pf);
                }
            }
            g.SmoothingMode = smoothMode;
        }

        /// <summary>
        /// 获取坐标
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="i">索引</param>
        /// <param name="coords">坐标</param>
        /// <returns></returns>
        public override bool GetCoords(GraphPane pane, int i, out String coords)
        {
            coords = String.Empty;
            if (i < 0 || i >= m_points.Count)
                return false;
            PointPair pt = m_points[i];
            if (pt.IsInvalid)
                return false;
            double x, y, z;
            ValueHandler valueHandler = new ValueHandler(pane, false);
            valueHandler.GetValues(this, i, out x, out z, out y);
            Axis yAxis = GetYAxis(pane);
            Axis xAxis = GetXAxis(pane);
            PointF pixPt = new PointF(xAxis.Scale.Transform(m_isOverrideOrdinal, i, x),
                            yAxis.Scale.Transform(m_isOverrideOrdinal, i, y));
            if (!pane.Chart.Rect.Contains(pixPt))
                return false;
            float halfSize = m_symbol.Size * pane.CalcScaleFactor();
            coords = String.Format("{0:f0},{1:f0},{2:f0},{3:f0}",
                    pixPt.X - halfSize, pixPt.Y - halfSize,
                    pixPt.X + halfSize, pixPt.Y + halfSize);
            return true;
        }

        /// <summary>
        /// X轴是否独立
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns>是否独立</returns>
        internal override bool IsXIndependent(GraphPane pane)
        {
            return true;
        }

        /// <summary>
        /// 创建唯一属性
        /// </summary>
        /// <param name="rotator">旋转色</param>
        public override void MakeUnique(ColorSymbolRotator rotator)
        {
            this.Color = rotator.NextColor;
            this.Symbol.Type = rotator.NextSymbol;
        }
        #endregion
    }
}
