/*****************************************************************************\
*                                                                             *
* Line.cs -      Line functions, types, and definitions                       *
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
    /// 线
    /// </summary>
    [Serializable]
    public class Line : LineBase
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建线
        /// </summary>
        public Line()
            : this(Color.Empty)
        {
        }

        /// <summary>
        /// 创建线
        /// </summary>
        /// <param name="color">颜色</param>
        public Line(Color color)
        {
            m_color = color.IsEmpty ? Default.Color : color;
            m_stepType = Default.StepType;
            m_isSmooth = Default.IsSmooth;
            m_smoothTension = Default.SmoothTension;
            m_fill = new Fill(Default.FillColor, Default.FillBrush, Default.FillType);
            m_isOptimizedDraw = Default.IsOptimizedDraw;
        }

        /// <summary>
        /// 创建线
        /// </summary>
        /// <param name="rhs">其他对象</param>
        public Line(Line rhs)
            : base(rhs)
        {
            m_color = rhs.m_color;
            m_stepType = rhs.m_stepType;
            m_isSmooth = rhs.m_isSmooth;
            m_smoothTension = rhs.m_smoothTension;
            m_fill = new Fill(rhs.m_fill);
            m_isOptimizedDraw = rhs.m_isOptimizedDraw;
        }

        /// <summary>
        /// 默认属性
        /// </summary>
        public new struct Default
        {
            public static Color Color = Color.Red;
            public static Color FillColor = Color.Red;
            public static Brush FillBrush = null;
            public static FillType FillType = FillType.None;
            public static bool IsSmooth = false;
            public static float SmoothTension = 0.5F;
            public static bool IsOptimizedDraw = false;
            public static StepType StepType = StepType.NonStep;
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


        private bool m_isOptimizedDraw;

        /// <summary>
        /// 获取或设置是否效率绘图
        /// </summary>
        public bool IsOptimizedDraw
        {
            get { return m_isOptimizedDraw; }
            set { m_isOptimizedDraw = value; }
        }

        private bool m_isSmooth;

        /// <summary>
        /// 获取或设置是否平滑
        /// </summary>
        public bool IsSmooth
        {
            get { return m_isSmooth; }
            set { m_isSmooth = value; }
        }

        private float m_smoothTension;

        /// <summary>
        /// 获取或设置是否平滑组合
        /// </summary>
        public float SmoothTension
        {
            get { return m_smoothTension; }
            set { m_smoothTension = value; }
        }

        private StepType m_stepType;

        /// <summary>
        /// 获取或设置步长类型
        /// </summary>
        public StepType StepType
        {
            get { return m_stepType; }
            set { m_stepType = value; }
        }

        /// <summary>
        /// 建立点阵
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="curve">图形</param>
        /// <param name="arrPoints">点集</param>
        /// <param name="count">数量</param>
        /// <returns>是否成功</returns>
        public bool BuildPointsArray(GraphPane pane, CurveItem curve,
    out PointF[] arrPoints, out int count)
        {
            arrPoints = null;
            count = 0;
            IPointList points = curve.Points;
            if (this.IsVisible && !this.Color.IsEmpty && points != null)
            {
                int index = 0;
                float curX, curY,
                            lastX = 0,
                            lastY = 0;
                double x, y, lowVal;
                ValueHandler valueHandler = new ValueHandler(pane, false);
                arrPoints = new PointF[(m_stepType == StepType.NonStep ? 1 : 2) *
                                            points.Count + 1];
                for (int i = 0; i < points.Count; i++)
                {
                    if (!points[i].IsInvalid)
                    {
                        if (pane.LineType == LineType.Stack)
                        {
                            valueHandler.GetValues(curve, i, out x, out lowVal, out y);
                        }
                        else
                        {
                            x = points[i].X;
                            y = points[i].Y;
                        }
                        if (x == PointPair.Missing || y == PointPair.Missing)
                            continue;
                        Axis xAxis = curve.GetXAxis(pane);
                        curX = xAxis.Scale.Transform(curve.IsOverrideOrdinal, i, x);
                        Axis yAxis = curve.GetYAxis(pane);
                        curY = yAxis.Scale.Transform(curve.IsOverrideOrdinal, i, y);
                        if (curX < -1000000 || curY < -1000000 || curX > 1000000 || curY > 1000000)
                            continue;
                        if (m_isSmooth || index == 0 || this.StepType == StepType.NonStep)
                        {
                            arrPoints[index].X = curX;
                            arrPoints[index].Y = curY;
                        }
                        else if (this.StepType == StepType.ForwardStep ||
                                        this.StepType == StepType.ForwardSegment)
                        {
                            arrPoints[index].X = curX;
                            arrPoints[index].Y = lastY;
                            index++;
                            arrPoints[index].X = curX;
                            arrPoints[index].Y = curY;
                        }
                        else if (this.StepType == StepType.RearwardStep ||
                                        this.StepType == StepType.RearwardSegment)
                        {
                            arrPoints[index].X = lastX;
                            arrPoints[index].Y = curY;
                            index++;
                            arrPoints[index].X = curX;
                            arrPoints[index].Y = curY;
                        }
                        lastX = curX;
                        lastY = curY;
                        index++;
                    }
                }
                if (index == 0)
                    return false;
                arrPoints[index] = arrPoints[index - 1];
                index++;
                count = index;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 建立低端点位阵
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="curve">图形</param>
        /// <param name="arrPoints">点集</param>
        /// <param name="count">数量</param>
        /// <returns>状态</returns>
        public bool BuildLowPointsArray(GraphPane pane, CurveItem curve,
                        out PointF[] arrPoints, out int count)
        {
            arrPoints = null;
            count = 0;
            IPointList points = curve.Points;
            if (this.IsVisible && !this.Color.IsEmpty && points != null)
            {
                int index = 0;
                float curX, curY,
                        lastX = 0,
                        lastY = 0;
                double x, y, hiVal;
                ValueHandler valueHandler = new ValueHandler(pane, false);
                arrPoints = new PointF[(m_stepType == StepType.NonStep ? 1 : 2) *
                    (pane.LineType == LineType.Stack ? 2 : 1) *
                    points.Count + 1];
                for (int i = points.Count - 1; i >= 0; i--)
                {
                    if (!points[i].IsInvalid)
                    {
                        valueHandler.GetValues(curve, i, out x, out y, out hiVal);
                        if (x == PointPair.Missing || y == PointPair.Missing)
                            continue;
                        Axis xAxis = curve.GetXAxis(pane);
                        curX = xAxis.Scale.Transform(curve.IsOverrideOrdinal, i, x);
                        Axis yAxis = curve.GetYAxis(pane);
                        curY = yAxis.Scale.Transform(curve.IsOverrideOrdinal, i, y);
                        if (m_isSmooth || index == 0 || this.StepType == StepType.NonStep)
                        {
                            arrPoints[index].X = curX;
                            arrPoints[index].Y = curY;
                        }
                        else if (this.StepType == StepType.ForwardStep)
                        {
                            arrPoints[index].X = curX;
                            arrPoints[index].Y = lastY;
                            index++;
                            arrPoints[index].X = curX;
                            arrPoints[index].Y = curY;
                        }
                        else if (this.StepType == StepType.RearwardStep)
                        {
                            arrPoints[index].X = lastX;
                            arrPoints[index].Y = curY;
                            index++;
                            arrPoints[index].X = curX;
                            arrPoints[index].Y = curY;
                        }
                        lastX = curX;
                        lastY = curY;
                        index++;
                    }
                }
                if (index == 0)
                    return false;
                arrPoints[index] = arrPoints[index - 1];
                index++;
                count = index;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 关闭曲线
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="curve">图形</param>
        /// <param name="arrPoints">点集</param>
        /// <param name="count">数量</param>
        /// <param name="yMin">y轴最小值</param>
        /// <param name="path">路径</param>
        public void CloseCurve(GraphPane pane, CurveItem curve, PointF[] arrPoints,
                            int count, double yMin, GraphicsPath path)
        {
            if (pane.LineType != LineType.Stack)
            {
                float yBase;
                Axis yAxis = curve.GetYAxis(pane);
                yBase = yAxis.Scale.Transform(yMin);
                path.AddLine(arrPoints[count - 1].X, arrPoints[count - 1].Y, arrPoints[count - 1].X, yBase);
                path.AddLine(arrPoints[count - 1].X, yBase, arrPoints[0].X, yBase);
                path.AddLine(arrPoints[0].X, yBase, arrPoints[0].X, arrPoints[0].Y);
            }
            else
            {
                PointF[] arrPoints2;
                int count2;
                float tension = m_isSmooth ? m_smoothTension : 0f;
                int index = pane.CurveList.IndexOf(curve);
                if (index > 0)
                {
                    CurveItem tmpCurve;
                    for (int i = index - 1; i >= 0; i--)
                    {
                        tmpCurve = pane.CurveList[i];
                        if (tmpCurve is LineItem)
                        {
                            tension = ((LineItem)tmpCurve).Line.IsSmooth ? ((LineItem)tmpCurve).Line.SmoothTension : 0f;
                            break;
                        }
                    }
                }
                BuildLowPointsArray(pane, curve, out arrPoints2, out count2);
                path.AddCurve(arrPoints2, 0, count2 - 2, tension);
            }
        }

        /// <summary>
        /// 比较坐标
        /// </summary>
        /// <param name="p1">坐标1</param>
        /// <param name="p2">坐标2</param>
        /// <returns>是否相等</returns>
        private int CompareASC(PointF p1, PointF p2)
        {
            if (p1.X < p2.X)
            {
                return -1;
            }
            else if (p1.X > p2.X)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 比较DES
        /// </summary>
        /// <param name="p1">坐标1</param>
        /// <param name="p2">坐标2</param>
        /// <returns>是否相等</returns>
        private int CompareDES(PointF p1, PointF p2)
        {
            if (p1.X > p2.X)
            {
                return -1;
            }
            else if (p1.X < p2.X)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="curve">图形</param>
        /// <param name="scaleFactor">刻度因子</param>
        public void Draw(Graphics g, GraphPane pane, CurveItem curve, float scaleFactor)
        {
            if (this.IsVisible)
            {
                SmoothingMode sModeSave = g.SmoothingMode;
                if (m_isAntiAlias)
                    g.SmoothingMode = SmoothingMode.HighQuality;
                if (curve is StickItem)
                    DrawSticks(g, pane, curve, scaleFactor);
                else if (this.IsSmooth || this.Fill.IsVisible)
                    DrawSmoothFilledCurve(g, pane, curve, scaleFactor);
                else
                {
                    m_isSmooth = true;
                    m_width = 2;
                    DrawSmoothFilledCurve(g, pane, curve, scaleFactor);
                }
                g.SmoothingMode = sModeSave;
            }
        }

        /// <summary>
        /// 绘制曲线
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="curve">图形</param>
        /// <param name="scaleFactor">刻度因子</param>
        public void DrawCurve(Graphics g, GraphPane pane,
                        CurveItem curve, float scaleFactor)
        {
            Line source = this;
            if (curve.IsSelected)
                source = Selection.Line;
            int tmpX, tmpY,
                    lastX = int.MaxValue,
                    lastY = int.MaxValue;
            double curX, curY, lowVal;
            PointPair curPt, lastPt = new PointPair();
            bool lastBad = true;
            IPointList points = curve.Points;
            ValueHandler valueHandler = new ValueHandler(pane, false);
            Axis yAxis = curve.GetYAxis(pane);
            Axis xAxis = curve.GetXAxis(pane);
            bool xIsLog = xAxis.m_scale.IsLog;
            bool yIsLog = yAxis.m_scale.IsLog;
            int minX = (int)pane.Chart.Rect.Left;
            int maxX = (int)pane.Chart.Rect.Right;
            int minY = (int)pane.Chart.Rect.Top;
            int maxY = (int)pane.Chart.Rect.Bottom;
            Pen pen = source.GetPen(pane, scaleFactor);
            pen.Width = 2.0F;
            if (points != null && !m_color.IsEmpty && this.IsVisible)
            {
                bool isOut;
                bool isOptDraw = m_isOptimizedDraw && points.Count > 1000;
                bool[,] isPixelDrawn = null;
                if (isOptDraw)
                    isPixelDrawn = new bool[maxX + 1, maxY + 1];
                for (int i = 0; i < points.Count; i++)
                {
                    curPt = points[i];
                    if (pane.LineType == LineType.Stack)
                    {
                        if (!valueHandler.GetValues(curve, i, out curX, out lowVal, out curY))
                        {
                            curX = PointPair.Missing;
                            curY = PointPair.Missing;
                        }
                    }
                    else
                    {
                        curX = curPt.X;
                        curY = curPt.Y;
                    }
                    if (curX == PointPair.Missing ||
                            curY == PointPair.Missing ||
                            System.Double.IsNaN(curX) ||
                            System.Double.IsNaN(curY) ||
                            System.Double.IsInfinity(curX) ||
                            System.Double.IsInfinity(curY) ||
                            (xIsLog && curX <= 0.0) ||
                            (yIsLog && curY <= 0.0))
                    {
                        lastBad = lastBad || !pane.IsIgnoreMissing;
                        isOut = true;
                    }
                    else
                    {
                        tmpX = (int)xAxis.Scale.Transform(curve.IsOverrideOrdinal, i, curX);
                        tmpY = (int)yAxis.Scale.Transform(curve.IsOverrideOrdinal, i, curY);
                        if (isOptDraw && tmpX >= minX && tmpX <= maxX &&
                                    tmpY >= minY && tmpY <= maxY)
                        {
                            if (isPixelDrawn[tmpX, tmpY])
                                continue;
                            isPixelDrawn[tmpX, tmpY] = true;
                        }
                        isOut = (tmpX < minX && lastX < minX) || (tmpX > maxX && lastX > maxX) ||
                            (tmpY < minY && lastY < minY) || (tmpY > maxY && lastY > maxY);
                        if (!lastBad)
                        {
                            try
                            {
                                if (lastX > 5000000 || lastX < -5000000 ||
                                        lastY > 5000000 || lastY < -5000000 ||
                                        tmpX > 5000000 || tmpX < -5000000 ||
                                        tmpY > 5000000 || tmpY < -5000000)
                                    InterpolatePoint(g, pane, curve, lastPt, scaleFactor, pen,
                                                    lastX, lastY, tmpX, tmpY);
                                else if (!isOut)
                                {
                                    if (!curve.IsSelected && this.m_gradientFill.IsGradientValueType)
                                    {
                                        Pen tPen = GetPen(pane, scaleFactor, lastPt);
                                        if (this.StepType == StepType.NonStep)
                                        {
                                            g.DrawLine(tPen, lastX, lastY, tmpX, tmpY);
                                        }
                                        else if (this.StepType == StepType.ForwardStep)
                                        {
                                            g.DrawLine(tPen, lastX, lastY, tmpX, lastY);
                                            g.DrawLine(tPen, tmpX, lastY, tmpX, tmpY);
                                        }
                                        else if (this.StepType == StepType.RearwardStep)
                                        {
                                            g.DrawLine(tPen, lastX, lastY, lastX, tmpY);
                                            g.DrawLine(tPen, lastX, tmpY, tmpX, tmpY);
                                        }
                                        else if (this.StepType == StepType.ForwardSegment)
                                        {
                                            g.DrawLine(tPen, lastX, lastY, tmpX, lastY);
                                        }
                                        else
                                        {
                                            g.DrawLine(tPen, lastX, tmpY, tmpX, tmpY);
                                        }
                                    }
                                    else
                                    {
                                        if (this.StepType == StepType.NonStep)
                                        {
                                            g.DrawLine(pen, lastX, lastY, tmpX, tmpY);
                                        }
                                        else if (this.StepType == StepType.ForwardStep)
                                        {
                                            g.DrawLine(pen, lastX, lastY, tmpX, lastY);
                                            g.DrawLine(pen, tmpX, lastY, tmpX, tmpY);
                                        }
                                        else if (this.StepType == StepType.RearwardStep)
                                        {
                                            g.DrawLine(pen, lastX, lastY, lastX, tmpY);
                                            g.DrawLine(pen, lastX, tmpY, tmpX, tmpY);
                                        }
                                        else if (this.StepType == StepType.ForwardSegment)
                                        {
                                            g.DrawLine(pen, lastX, lastY, tmpX, lastY);
                                        }
                                        else if (this.StepType == StepType.RearwardSegment)
                                        {
                                            g.DrawLine(pen, lastX, tmpY, tmpX, tmpY);
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                InterpolatePoint(g, pane, curve, lastPt, scaleFactor, pen,
                                            lastX, lastY, tmpX, tmpY);
                            }
                        }
                        lastPt = curPt;
                        lastX = tmpX;
                        lastY = tmpY;
                        lastBad = false;
                    }
                }
            }
        }

        /// <summary>
        /// 绘图原始曲线
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="curve">图形</param>
        /// <param name="scaleFactor">刻度因子</param>
        public void DrawCurveOriginal(Graphics g, GraphPane pane,
                                          CurveItem curve, float scaleFactor)
        {
            Line source = this;
            if (curve.IsSelected)
                source = Selection.Line;
            float tmpX, tmpY,
                    lastX = float.MaxValue,
                    lastY = float.MaxValue;
            double curX, curY, lowVal;
            PointPair curPt, lastPt = new PointPair();
            bool lastBad = true;
            IPointList points = curve.Points;
            ValueHandler valueHandler = new ValueHandler(pane, false);
            Axis yAxis = curve.GetYAxis(pane);
            Axis xAxis = curve.GetXAxis(pane);
            bool xIsLog = xAxis.m_scale.IsLog;
            bool yIsLog = yAxis.m_scale.IsLog;
            float minX = pane.Chart.Rect.Left;
            float maxX = pane.Chart.Rect.Right;
            float minY = pane.Chart.Rect.Top;
            float maxY = pane.Chart.Rect.Bottom;
            Pen pen = source.GetPen(pane, scaleFactor);
            if (points != null && !m_color.IsEmpty && this.IsVisible)
            {
                bool isOut;
                for (int i = 0; i < points.Count; i++)
                {
                    curPt = points[i];
                    if (pane.LineType == LineType.Stack)
                    {
                        if (!valueHandler.GetValues(curve, i, out curX, out lowVal, out curY))
                        {
                            curX = PointPair.Missing;
                            curY = PointPair.Missing;
                        }
                    }
                    else
                    {
                        curX = curPt.X;
                        curY = curPt.Y;
                    }
                    if (curX == PointPair.Missing ||
                            curY == PointPair.Missing ||
                            System.Double.IsNaN(curX) ||
                            System.Double.IsNaN(curY) ||
                            System.Double.IsInfinity(curX) ||
                            System.Double.IsInfinity(curY) ||
                            (xIsLog && curX <= 0.0) ||
                            (yIsLog && curY <= 0.0))
                    {
                        lastBad = lastBad || !pane.IsIgnoreMissing;
                        isOut = true;
                    }
                    else
                    {
                        tmpX = xAxis.Scale.Transform(curve.IsOverrideOrdinal, i, curX);
                        tmpY = yAxis.Scale.Transform(curve.IsOverrideOrdinal, i, curY);
                        isOut = (tmpX < minX && lastX < minX) || (tmpX > maxX && lastX > maxX) ||
                            (tmpY < minY && lastY < minY) || (tmpY > maxY && lastY > maxY);
                        if (!lastBad)
                        {
                            try
                            {
                                if (lastX > 5000000 || lastX < -5000000 ||
                                        lastY > 5000000 || lastY < -5000000 ||
                                        tmpX > 5000000 || tmpX < -5000000 ||
                                        tmpY > 5000000 || tmpY < -5000000)
                                    InterpolatePoint(g, pane, curve, lastPt, scaleFactor, pen,
                                                    lastX, lastY, tmpX, tmpY);
                                else if (!isOut)
                                {
                                    if (!curve.IsSelected && this.m_gradientFill.IsGradientValueType)
                                    {
                                        Pen tPen = GetPen(pane, scaleFactor, lastPt);
                                        if (this.StepType == StepType.NonStep)
                                        {
                                            g.DrawLine(tPen, lastX, lastY, tmpX, tmpY);
                                        }
                                        else if (this.StepType == StepType.ForwardStep)
                                        {
                                            g.DrawLine(tPen, lastX, lastY, tmpX, lastY);
                                            g.DrawLine(tPen, tmpX, lastY, tmpX, tmpY);
                                        }
                                        else if (this.StepType == StepType.RearwardStep)
                                        {
                                            g.DrawLine(tPen, lastX, lastY, lastX, tmpY);
                                            g.DrawLine(tPen, lastX, tmpY, tmpX, tmpY);
                                        }
                                        else if (this.StepType == StepType.ForwardSegment)
                                        {
                                            g.DrawLine(tPen, lastX, lastY, tmpX, lastY);
                                        }
                                        else
                                        {
                                            g.DrawLine(tPen, lastX, tmpY, tmpX, tmpY);
                                        }
                                    }
                                    else
                                    {
                                        if (this.StepType == StepType.NonStep)
                                        {
                                            g.DrawLine(pen, lastX, lastY, tmpX, tmpY);
                                        }
                                        else if (this.StepType == StepType.ForwardStep)
                                        {
                                            g.DrawLine(pen, lastX, lastY, tmpX, lastY);
                                            g.DrawLine(pen, tmpX, lastY, tmpX, tmpY);
                                        }
                                        else if (this.StepType == StepType.RearwardStep)
                                        {
                                            g.DrawLine(pen, lastX, lastY, lastX, tmpY);
                                            g.DrawLine(pen, lastX, tmpY, tmpX, tmpY);
                                        }
                                        else if (this.StepType == StepType.ForwardSegment)
                                        {
                                            g.DrawLine(pen, lastX, lastY, tmpX, lastY);
                                        }
                                        else if (this.StepType == StepType.RearwardSegment)
                                        {
                                            g.DrawLine(pen, lastX, tmpY, tmpX, tmpY);
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                InterpolatePoint(g, pane, curve, lastPt, scaleFactor, pen,
                                            lastX, lastY, tmpX, tmpY);
                            }
                        }
                        lastPt = curPt;
                        lastX = tmpX;
                        lastY = tmpY;
                        lastBad = false;
                    }
                }
            }
        }

        /// <summary>
        /// 绘制线段
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="x1">横坐标值1</param>
        /// <param name="y1">纵坐标值1</param>
        /// <param name="x2">横坐标值2</param>
        /// <param name="y2">纵坐标值2</param>
        /// <param name="scaleFactor">刻度因子</param>
        public void DrawSegment(Graphics g, GraphPane pane, float x1, float y1,
                                  float x2, float y2, float scaleFactor)
        {
            if (m_isVisible && !this.Color.IsEmpty)
            {
                Pen pen = GetPen(pane, scaleFactor);
                g.DrawLine(pen, x1, y1, x2, y2);
            }
        }

        /// <summary>
        /// 绘制平滑曲线
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="curve">图形</param>
        /// <param name="scaleFactor">刻度因子</param>
        public void DrawSmoothFilledCurve(Graphics g, GraphPane pane,
                        CurveItem curve, float scaleFactor)
        {
            Line source = this;
            if (curve.IsSelected)
                source = Selection.Line;
            PointF[] arrPoints;
            int count;
            IPointList points = curve.Points;
            if (this.IsVisible && !this.Color.IsEmpty && points != null &&
                BuildPointsArray(pane, curve, out arrPoints, out count) &&
                count > 2)
            {
                float tension = m_isSmooth ? m_smoothTension : 0f;
                if (this.Fill.IsVisible)
                {
                    Axis yAxis = curve.GetYAxis(pane);
                    using (GraphicsPath path = new GraphicsPath(FillMode.Winding))
                    {
                        path.AddCurve(arrPoints, 0, count - 2, tension);
                        double yMin = yAxis.m_scale.m_min < 0 ? 0.0 : yAxis.m_scale.m_min;
                        CloseCurve(pane, curve, arrPoints, count, yMin, path);
                        RectangleF rect = path.GetBounds();
                        using (Brush brush = source.m_fill.MakeBrush(rect))
                        {
                            if (pane.LineType == LineType.Stack && yAxis.Scale.m_min < 0 &&
                                    this.IsFirstLine(pane, curve))
                            {
                                float zeroPix = yAxis.Scale.Transform(0);
                                RectangleF tRect = pane.Chart.m_rect;
                                tRect.Height = zeroPix - tRect.Top;
                                if (tRect.Height > 0)
                                {
                                    Region reg = g.Clip;
                                    g.SetClip(tRect);
                                    g.FillPath(brush, path);
                                    g.SetClip(pane.Chart.m_rect);
                                }
                            }
                            else
                                g.FillPath(brush, path);
                        }
                    }
                }
                if (m_isSmooth)
                {
                    Pen pen = GetPen(pane, scaleFactor);
                    DrawCurve(g, pane, curve, scaleFactor);
                }
                else
                    DrawCurve(g, pane, curve, scaleFactor);
            }
        }

        /// <summary>
        /// 回去柱状图
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="curve">图形</param>
        /// <param name="scaleFactor">刻度因子</param>
        public void DrawSticks(Graphics g, GraphPane pane, CurveItem curve, float scaleFactor)
        {
            Line source = this;
            if (curve.IsSelected)
                source = Selection.Line;
            Axis yAxis = curve.GetYAxis(pane);
            Axis xAxis = curve.GetXAxis(pane);
            float basePix = yAxis.Scale.Transform(0.0);
            Pen pen = source.GetPen(pane, scaleFactor);
            for (int i = 0; i < curve.Points.Count; i++)
            {
                PointPair pt = curve.Points[i];
                if (pt.X != PointPair.Missing &&
                        pt.Y != PointPair.Missing &&
                        !System.Double.IsNaN(pt.X) &&
                        !System.Double.IsNaN(pt.Y) &&
                        !System.Double.IsInfinity(pt.X) &&
                        !System.Double.IsInfinity(pt.Y) &&
                        (!xAxis.m_scale.IsLog || pt.X > 0.0) &&
                        (!yAxis.m_scale.IsLog || pt.Y > 0.0))
                {
                    float pixY = yAxis.Scale.Transform(curve.IsOverrideOrdinal, i, pt.Y);
                    float pixX = xAxis.Scale.Transform(curve.IsOverrideOrdinal, i, pt.X);
                    if (pixX >= pane.Chart.m_rect.Left && pixX <= pane.Chart.m_rect.Right)
                    {
                        if (pixY > pane.Chart.m_rect.Bottom)
                            pixY = pane.Chart.m_rect.Bottom;
                        if (pixY < pane.Chart.m_rect.Top)
                            pixY = pane.Chart.m_rect.Top;
                        if (!curve.IsSelected && this.m_gradientFill.IsGradientValueType)
                        {
                            Pen tPen = GetPen(pane, scaleFactor, pt);
                            g.DrawLine(tPen, pixX, pixY, pixX, basePix);
                        }
                        else
                            g.DrawLine(pen, pixX, pixY, pixX, basePix);
                    }
                }
            }
        }

        /// <summary>
        /// 是否第一根线
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="curve">图形</param>
        /// <returns></returns>
        private bool IsFirstLine(GraphPane pane, CurveItem curve)
        {
            CurveList curveList = pane.CurveList;
            for (int j = 0; j < curveList.Count; j++)
            {
                CurveItem tCurve = curveList[j];
                if (tCurve is LineItem && tCurve.IsY2Axis == curve.IsY2Axis &&
                        tCurve.YAxisIndex == curve.YAxisIndex)
                {
                    return tCurve == curve;
                }
            }
            return false;
        }

        /// <summary>
        /// 插入点
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="curve">图形</param>
        /// <param name="lastPt">上次坐标</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="pen">画笔</param>
        /// <param name="lastX">上一次横坐标</param>
        /// <param name="lastY">上一次纵坐标</param>
        /// <param name="tmpX">临时横坐标</param>
        /// <param name="tmpY">临时纵坐标</param>
        private void InterpolatePoint(Graphics g, GraphPane pane, CurveItem curve, PointPair lastPt,
                        float scaleFactor, Pen pen, float lastX, float lastY, float tmpX, float tmpY)
        {
            try
            {
                RectangleF chartRect = pane.Chart.m_rect;
                bool lastIn = chartRect.Contains(lastX, lastY);
                bool curIn = chartRect.Contains(tmpX, tmpY);
                if (!lastIn)
                {
                    float newX, newY;
                    if (Math.Abs(lastX) > Math.Abs(lastY))
                    {
                        newX = lastX < 0 ? chartRect.Left : chartRect.Right;
                        newY = lastY + (tmpY - lastY) * (newX - lastX) / (tmpX - lastX);
                    }
                    else
                    {
                        newY = lastY < 0 ? chartRect.Top : chartRect.Bottom;
                        newX = lastX + (tmpX - lastX) * (newY - lastY) / (tmpY - lastY);
                    }
                    lastX = newX;
                    lastY = newY;
                }
                if (!curIn)
                {
                    float newX, newY;
                    if (Math.Abs(tmpX) > Math.Abs(tmpY))
                    {
                        newX = tmpX < 0 ? chartRect.Left : chartRect.Right;
                        newY = tmpY + (lastY - tmpY) * (newX - tmpX) / (lastX - tmpX);
                    }
                    else
                    {
                        newY = tmpY < 0 ? chartRect.Top : chartRect.Bottom;
                        newX = tmpX + (lastX - tmpX) * (newY - tmpY) / (lastY - tmpY);
                    }
                    tmpX = newX;
                    tmpY = newY;
                }
                if (!curve.IsSelected && this.m_gradientFill.IsGradientValueType)
                {
                    Pen tPen = GetPen(pane, scaleFactor, lastPt);
                    if (this.StepType == StepType.NonStep)
                    {
                        g.DrawLine(tPen, lastX, lastY, tmpX, tmpY);
                    }
                    else if (this.StepType == StepType.ForwardStep)
                    {
                        g.DrawLine(tPen, lastX, lastY, tmpX, lastY);
                        g.DrawLine(tPen, tmpX, lastY, tmpX, tmpY);
                    }
                    else if (this.StepType == StepType.RearwardStep)
                    {
                        g.DrawLine(tPen, lastX, lastY, lastX, tmpY);
                        g.DrawLine(tPen, lastX, tmpY, tmpX, tmpY);
                    }
                    else if (this.StepType == StepType.ForwardSegment)
                    {
                        g.DrawLine(tPen, lastX, lastY, tmpX, lastY);
                    }
                    else
                    {
                        g.DrawLine(tPen, lastX, tmpY, tmpX, tmpY);
                    }
                }
                else
                {
                    if (this.StepType == StepType.NonStep)
                    {
                        g.DrawLine(pen, lastX, lastY, tmpX, tmpY);
                    }
                    else if (this.StepType == StepType.ForwardStep)
                    {
                        g.DrawLine(pen, lastX, lastY, tmpX, lastY);
                        g.DrawLine(pen, tmpX, lastY, tmpX, tmpY);
                    }
                    else if (this.StepType == StepType.RearwardStep)
                    {
                        g.DrawLine(pen, lastX, lastY, lastX, tmpY);
                        g.DrawLine(pen, lastX, tmpY, tmpX, tmpY);
                    }
                    else if (this.StepType == StepType.ForwardSegment)
                    {
                        g.DrawLine(pen, lastX, lastY, tmpX, lastY);
                    }
                    else if (this.StepType == StepType.RearwardSegment)
                    {
                        g.DrawLine(pen, lastX, tmpY, tmpX, tmpY);
                    }
                }
            }
            catch { }
        }
        #endregion
    }
}
