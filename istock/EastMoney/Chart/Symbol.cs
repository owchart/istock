/********************************************************************************\
*                                                                                *
* Symbol.cs -    Symbol functions, types, and definitions                        *
*                                                                                *
*               Version 1.00                                                     *
*                                                                                *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.      *
*                                                                                *
*********************************************************************************/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OwLib
{
    /// <summary>
    /// 标记
    /// </summary>
    [Serializable]
    public class Symbol
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建标记
        /// </summary>
        public Symbol()
            : this(SymbolType.Default, Color.Empty)
        {
        }

        /// <summary>
        /// 创建标记
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="color">颜色</param>
        public Symbol(SymbolType type, Color color)
        {
            m_size = Default.Size;
            m_type = type;
            m_isAntiAlias = Default.IsAntiAlias;
            m_isVisible = Default.IsVisible;
            m_border = new Border(Default.IsBorderVisible, color, Default.PenWidth);
            m_fill = new Fill(color, Default.FillBrush, Default.FillType);
            m_userSymbol = null;
        }

        /// <summary>
        /// 创建标记
        /// </summary>
        /// <param name="rhs">其他对象</param>
        public Symbol(Symbol rhs)
        {
            m_size = rhs.Size;
            m_type = rhs.Type;
            m_isAntiAlias = rhs.m_isAntiAlias;
            m_isVisible = rhs.IsVisible;
            m_fill = new Fill(rhs.Fill);
            m_border = new Border(rhs.Border);
            if (rhs.UserSymbol != null)
                m_userSymbol = rhs.UserSymbol.Clone() as GraphicsPath;
            else
                m_userSymbol = null;
        }

        /// <summary>
        /// 默认属性
        /// </summary>
        public struct Default
        {
            public static float Size = 7;
            public static float PenWidth = 1.0F;
            public static Color FillColor = Color.Red;
            public static Brush FillBrush = null;
            public static FillType FillType = FillType.None;
            public static SymbolType Type = SymbolType.Square;
            public static bool IsAntiAlias = false;
            public static bool IsVisible = true;
            public static bool IsBorderVisible = true;
            public static Color BorderColor = Color.Red;
        }

        private Border m_border;

        /// <summary>
        /// 
        /// </summary>
        public Border Border
        {
            get { return m_border; }
            set { m_border = value; }
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

        private bool m_isAntiAlias;

        /// <summary>
        /// 
        /// </summary>
        public bool IsAntiAlias
        {
            get { return m_isAntiAlias; }
            set { m_isAntiAlias = value; }
        }

        private bool m_isVisible;

        /// <summary>
        /// 
        /// </summary>
        public bool IsVisible
        {
            get { return m_isVisible; }
            set { m_isVisible = value; }
        }

        private float m_size;

        /// <summary>
        /// 
        /// </summary>
        public float Size
        {
            get { return m_size; }
            set { m_size = value; }
        }

        private SymbolType m_type;

        /// <summary>
        /// 
        /// </summary>
        public SymbolType Type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        private GraphicsPath m_userSymbol;

        /// <summary>
        /// 
        /// </summary>
        public GraphicsPath UserSymbol
        {
            get { return m_userSymbol; }
            set { m_userSymbol = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points">点集</param>
        /// <param name="pane">图层</param>
        /// <param name="curve">图形</param>
        /// <returns></returns>
        private Point CalTrendStartPoint(IPointList points, GraphPane pane, LineItem curve)
        {
            int startPos = Convert.ToInt32(points.Count * 0.05);
            double curX = 0;
            int i;
            for (i = startPos; i < points.Count; i++)
            {
                if (String.IsNullOrEmpty(pane.XAxis.Scale.TextLabels[i]))
                    continue;
                curX = Convert.ToDouble(pane.XAxis.Scale.TextLabels[i]);
                break;
            }
            double curY = pane.Alpha + pane.Beta * curX;
            Scale xScale = curve.GetXAxis(pane).Scale;
            Scale yScale = curve.GetYAxis(pane).Scale;
            int tmpStartX = (int)xScale.Transform(curve.IsOverrideOrdinal, i, curX);
            int tmpStartY = (int)yScale.Transform(curve.IsOverrideOrdinal, i, curY);
            return new Point(tmpStartX, tmpStartY);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points">点集</param>
        /// <param name="pane">图层</param>
        /// <param name="curve">图形</param>
        /// <returns></returns>
        private Point CalTrendEndPoint(IPointList points, GraphPane pane, LineItem curve)
        {
            int startPos = Convert.ToInt32(points.Count * 0.95);
            if (startPos >= points.Count)
                startPos = points.Count - 1;
            double curX = 0;
            int i;
            for (i = startPos; i > 0; i--)
            {
                if (String.IsNullOrEmpty(pane.XAxis.Scale.TextLabels[i]))
                    continue;
                curX = Convert.ToDouble(pane.XAxis.Scale.TextLabels[i]);
                break;
            }
            double curY = pane.Alpha + pane.Beta * curX;
            Scale xScale = curve.GetXAxis(pane).Scale;
            Scale yScale = curve.GetYAxis(pane).Scale;
            int tmpStartX = (int)xScale.Transform(curve.IsOverrideOrdinal, i, curX);
            int tmpStartY = (int)yScale.Transform(curve.IsOverrideOrdinal, i, curY);
            return new Point(tmpStartX, tmpStartY);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="curve">图形</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="isSelected">是否选中</param>
        public void Draw(Graphics g, GraphPane pane, LineItem curve, float scaleFactor,
            bool isSelected)
        {
            Symbol source = this;
            if (isSelected)
                source = Selection.Symbol;
            int tmpX, tmpY;
            int minX = (int)pane.Chart.Rect.Left;
            int maxX = (int)pane.Chart.Rect.Right;
            int minY = (int)pane.Chart.Rect.Top;
            int maxY = (int)pane.Chart.Rect.Bottom;
            bool[,] isPixelDrawn = new bool[maxX + 1, maxY + 1];
            double curX, curY, lowVal;
            IPointList points = curve.Points;
            if (points != null && (m_border.IsVisible || m_fill.IsVisible))
            {
                SmoothingMode sModeSave = g.SmoothingMode;
                if (m_isAntiAlias)
                    g.SmoothingMode = SmoothingMode.HighQuality;
                Pen pen = source.m_border.GetPen(pane, scaleFactor);
                using (GraphicsPath path = MakePath(g, scaleFactor))
                {
                    RectangleF rect = path.GetBounds();
                    using (Brush brush = source.Fill.MakeBrush(rect))
                    {
                        ValueHandler valueHandler = new ValueHandler(pane, false);
                        Scale xScale = curve.GetXAxis(pane).Scale;
                        Scale yScale = curve.GetYAxis(pane).Scale;
                        bool xIsLog = xScale.IsLog;
                        bool yIsLog = yScale.IsLog;
                        bool xIsOrdinal = xScale.IsAnyOrdinal;
                        double xMin = xScale.Min;
                        double xMax = xScale.Max;
                        for (int i = 0; i < points.Count; i++)
                        {
                            if (pane.LineType == LineType.Stack)
                            {
                                valueHandler.GetValues(curve, i, out curX, out lowVal, out curY);
                            }
                            else
                            {
                                curX = points[i].X;
                                if (curve is StickItem)
                                    curY = points[i].Z;
                                else
                                    curY = points[i].Y;
                            }
                            if (curX != PointPair.Missing &&
                                    curY != PointPair.Missing &&
                                    !System.Double.IsNaN(curX) &&
                                    !System.Double.IsNaN(curY) &&
                                    !System.Double.IsInfinity(curX) &&
                                    !System.Double.IsInfinity(curY) &&
                                    (curX > 0 || !xIsLog) &&
                                    (!yIsLog || curY > 0.0) &&
                                    (xIsOrdinal || (curX >= xMin && curX <= xMax)))
                            {
                                tmpX = (int)xScale.Transform(curve.IsOverrideOrdinal, i, curX);
                                tmpY = (int)yScale.Transform(curve.IsOverrideOrdinal, i, curY);
                                if (tmpX >= minX && tmpX <= maxX && tmpY >= minY && tmpY <= maxY)
                                {
                                    if (isPixelDrawn[tmpX, tmpY])
                                        continue;
                                    isPixelDrawn[tmpX, tmpY] = true;
                                }
                                if (m_fill.IsGradientValueType || m_border.m_gradientFill.IsGradientValueType)
                                {
                                    Pen tPen = m_border.GetPen(pane, scaleFactor, points[i]);
                                    using (Brush tBrush = m_fill.MakeBrush(rect, points[i]))
                                        this.DrawSymbol(g, tmpX, tmpY, path, tPen, tBrush);
                                }
                                else
                                {
                                    this.DrawSymbol(g, tmpX, tmpY, path, pen, brush);
                                }
                            }
                        }
                        if (pane.IsDrawTrend)
                        {
                            pane.Trend.Pen = pen;
                            SetTrend(points, pane, curve);
                        }
                    }
                }
                g.SmoothingMode = sModeSave;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="path">路径</param>
        /// <param name="pen">画笔</param>
        /// <param name="brush">画刷</param>
        private void DrawSymbol(Graphics g, int x, int y, GraphicsPath path,
                            Pen pen, Brush brush)
        {
            if (m_isVisible &&
                    this.Type != SymbolType.None &&
                    x < 100000 && x > -100000 &&
                    y < 100000 && y > -100000)
            {
                Matrix saveMatrix = g.Transform;
                g.TranslateTransform(x, y);
                if (m_fill.IsVisible)
                    g.FillPath(brush, path);
                if (m_border.IsVisible)
                    g.DrawPath(pen, path);
                g.Transform = saveMatrix;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="isSelected">是否选中</param>
        /// <param name="dataValue">点</param>
        public void DrawSymbol(Graphics g, GraphPane pane, int x, int y,
                            float scaleFactor, bool isSelected, PointPair dataValue)
        {
            Symbol source = this;
            if (isSelected)
                source = Selection.Symbol;
            if (m_isVisible &&
                    this.Type != SymbolType.None &&
                    x < 100000 && x > -100000 &&
                    y < 100000 && y > -100000)
            {
                SmoothingMode sModeSave = g.SmoothingMode;
                if (m_isAntiAlias)
                    g.SmoothingMode = SmoothingMode.HighQuality;
                Pen pen = m_border.GetPen(pane, scaleFactor, dataValue);
                using (GraphicsPath path = this.MakePath(g, scaleFactor))
                using (Brush brush = this.Fill.MakeBrush(path.GetBounds(), dataValue))
                {
                    DrawSymbol(g, x, y, path, pen, brush);
                }
                g.SmoothingMode = sModeSave;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <returns></returns>
        public GraphicsPath MakePath(Graphics g, float scaleFactor)
        {
            float scaledSize = (float)(m_size * scaleFactor);
            float hsize = scaledSize / 2,
                    hsize1 = hsize + 1;
            GraphicsPath path = new GraphicsPath();
            switch (m_type == SymbolType.Default || (m_type == SymbolType.UserDefined && m_userSymbol == null) ? Default.Type : m_type)
            {
                case SymbolType.Square:
                    path.AddLine(-hsize, -hsize, hsize, -hsize);
                    path.AddLine(hsize, -hsize, hsize, hsize);
                    path.AddLine(hsize, hsize, -hsize, hsize);
                    path.AddLine(-hsize, hsize, -hsize, -hsize);
                    break;
                case SymbolType.Diamond:
                    path.AddLine(0, -hsize, hsize, 0);
                    path.AddLine(hsize, 0, 0, hsize);
                    path.AddLine(0, hsize, -hsize, 0);
                    path.AddLine(-hsize, 0, 0, -hsize);
                    break;
                case SymbolType.Triangle:
                    path.AddLine(0, -hsize, hsize, hsize);
                    path.AddLine(hsize, hsize, -hsize, hsize);
                    path.AddLine(-hsize, hsize, 0, -hsize);
                    break;
                case SymbolType.Circle:
                    path.AddEllipse(-hsize, -hsize, scaledSize, scaledSize);
                    break;
                case SymbolType.XCross:
                    path.AddLine(-hsize, -hsize, hsize1, hsize1);
                    path.StartFigure();
                    path.AddLine(hsize, -hsize, -hsize1, hsize1);
                    break;
                case SymbolType.Plus:
                    path.AddLine(0, -hsize, 0, hsize1);
                    path.StartFigure();
                    path.AddLine(-hsize, 0, hsize1, 0);
                    break;
                case SymbolType.Star:
                    path.AddLine(0, -hsize, 0, hsize1);
                    path.StartFigure();
                    path.AddLine(-hsize, 0, hsize1, 0);
                    path.StartFigure();
                    path.AddLine(-hsize, -hsize, hsize1, hsize1);
                    path.StartFigure();
                    path.AddLine(hsize, -hsize, -hsize1, hsize1);
                    break;
                case SymbolType.TriangleDown:
                    path.AddLine(0, hsize, hsize, -hsize);
                    path.AddLine(hsize, -hsize, -hsize, -hsize);
                    path.AddLine(-hsize, -hsize, 0, hsize);
                    break;
                case SymbolType.HDash:
                    path.AddLine(-hsize, 0, hsize1, 0);
                    break;
                case SymbolType.VDash:
                    path.AddLine(0, -hsize, 0, hsize1);
                    break;
                case SymbolType.UserDefined:
                    path = m_userSymbol.Clone() as GraphicsPath;
                    Matrix scaleTransform = new Matrix(scaledSize, 0.0f, 0.0f, scaledSize, 0.0f, 0.0f);
                    path.Transform(scaleTransform);
                    break;
            }
            return path;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="points">点集</param>
        /// <param name="pane">图层</param>
        /// <param name="curve">图形</param>
        private void SetTrend(IPointList points, GraphPane pane, LineItem curve)
        {
            Point startP = CalTrendStartPoint(points, pane, curve);
            Point endP = CalTrendEndPoint(points, pane, curve);
            pane.Trend.StartP = startP;
            pane.Trend.EndP = endP;
        }
        #endregion
    }
}
