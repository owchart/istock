/*****************************************************************************\
*                                                                             *
* GraphPane.cs -   GraphPane functions, types, and definitions                *
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
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.ComponentModel;
using System.Data;
using System.Collections.Generic;

namespace OwLib
{
    /// <summary>
    /// 图层
    /// </summary>
    [Serializable]
    public class GraphPane : PaneBase
    {
        #region 陶德 2016/6/4
        /// <summary>
        /// 创建图层
        /// </summary>
        public GraphPane()
            : this(new RectangleF(0, 0, 500, 375), "", "", "")
        {
        }

        /// <summary>
        /// 创建图层
        /// </summary>
        /// <param name="rect">区域</param>
        /// <param name="title">标题</param>
        /// <param name="xTitle">X轴标题</param>
        /// <param name="yTitle">Y轴标题</param>
        public GraphPane(RectangleF rect, String title,
            String xTitle, String yTitle)
            : base(title, rect)
        {
            m_xAxis = new XAxis(xTitle);
            m_yAxisList = new List<YAxis>();
            m_y2AxisList = new List<Y2Axis>();
            m_yAxisList.Add(new YAxis(yTitle));
            m_y2AxisList.Add(new Y2Axis(String.Empty));
            m_curveList = new CurveList();
            m_isIgnoreInitial = Default.IsIgnoreInitial;
            m_isBoundedRanges = Default.IsBoundedRanges;
            m_isAlignGrids = false;
            m_chart = new Chart();
            m_barSettings = new BarSettings(this);
            m_lineType = Default.LineType;
        }

        /// <summary>
        /// 默认属性
        /// </summary>
        public new struct Default
        {
            public static bool IsIgnoreInitial = false;
            public static bool IsBoundedRanges = false;
            public static LineType LineType = LineType.Normal;
            public static double ClusterScaleWidth = 1.0;
            public static double NearestTol = 7.0;
        }

        /// <summary>
        /// 控件
        /// </summary>
        public GraphA owner;
        /// <summary>
        /// Y轴列表
        /// </summary>
        private List<YAxis> m_yAxisList;
        /// <summary>
        /// 右Y轴列表
        /// </summary>
        private List<Y2Axis> m_y2AxisList;

        private double m_alpha;

        /// <summary>
        /// 获取或设置透明度
        /// </summary>
        public double Alpha
        {
            get { return m_alpha; }
            set
            {
                m_alpha = value;
                if (m_Trend != null)
                {
                    m_Trend.Alpha = value;
                }
            }
        }

        internal BarSettings m_barSettings;

        /// <summary>
        /// 获取柱状图的设置
        /// </summary>
        public BarSettings BarSettings
        {
            get { return m_barSettings; }
        }

        private double m_beta;

        /// <summary>
        /// 获取或设置贝塔值
        /// </summary>
        public double Beta
        {
            get { return m_beta; }
            set
            {
                m_beta = value;
                if (m_Trend != null)
                {
                    m_Trend.Beta = value;
                }
            }
        }

        internal Chart m_chart;

        /// <summary>
        /// 获取图形
        /// </summary>
        public Chart Chart
        {
            get { return m_chart; }
        }

        private CurveList m_curveList;

        /// <summary>
        /// 获取或设置线的列表
        /// </summary>
        public CurveList CurveList
        {
            get { return m_curveList; }
            set { m_curveList = value; }
        }

        /// <summary>
        /// 获取是否是散点图
        /// </summary>
        public bool HasScatterPlot
        {
            get
            {
                foreach (CurveItem ci in CurveList)
                {
                    if (ci.IsLine)
                    {
                        return ((LineItem)ci).IsScatterPlot;
                    }
                }
                return false;
            }
        }

        public bool m_HasZeroLine = false;

        /// <summary>
        /// 获取或设置是零线
        /// </summary>
        public bool HasZeroLine
        {
            get { return m_HasZeroLine; }
            set { m_HasZeroLine = value; }
        }

        private bool m_isAlignGrids;

        /// <summary>
        /// 获取或设置是否网格对齐
        /// </summary>
        public bool IsAlignGrids
        {
            get { return m_isAlignGrids; }
            set { m_isAlignGrids = value; }
        }

        private bool m_isBoundedRanges;

        /// <summary>
        /// 获取或设置是否有矩形边界
        /// </summary>
        public bool IsBoundedRanges
        {
            get { return m_isBoundedRanges; }
            set { m_isBoundedRanges = value; }
        }

        private bool m_IsDrawTrend = false;

        /// <summary>
        /// 获取或设置是否是趋势线
        /// </summary>
        public bool IsDrawTrend
        {
            get { return m_IsDrawTrend; }
            set
            {
                m_IsDrawTrend = value;
                if (value || m_Trend == null)
                {
                    m_Trend = new Trend();
                    m_Trend.Alpha = Alpha;
                    m_Trend.Beta = Beta;
                }
                m_Trend.IsDrawTrend = value;
            }
        }

        private bool m_isIgnoreInitial;

        /// <summary>
        /// 获取或设置是否忽视初始化
        /// </summary>
        public bool IsIgnoreInitial
        {
            get { return m_isIgnoreInitial; }
            set { m_isIgnoreInitial = value; }
        }

        private bool m_isIgnoreMissing;

        /// <summary>
        /// 获取或设置是否忽视错误
        /// </summary>
        public bool IsIgnoreMissing
        {
            get { return m_isIgnoreMissing; }
            set { m_isIgnoreMissing = value; }
        }

        private LineType m_lineType;

        /// <summary>
        /// 获取或设置线的类型
        /// </summary>
        public LineType LineType
        {
            get { return m_lineType; }
            set { m_lineType = value; }
        }

        private Trend m_Trend;

        /// <summary>
        /// 获取或设置趋势线
        /// </summary>
        public Trend Trend
        {
            get { return m_Trend; }
            set
            {
                m_Trend = value;
            }
        }

        private XAxis m_xAxis;

        /// <summary>
        /// 获取X轴
        /// </summary>
        public XAxis XAxis
        {
            get { return m_xAxis; }
        }

        /// <summary>
        /// 获取Y轴
        /// </summary>
        public YAxis YAxis
        {
            get { return m_yAxisList[0] as YAxis; }
        }

        /// <summary>
        /// 获取右Y轴
        /// </summary>
        public Y2Axis Y2Axis
        {
            get { return m_y2AxisList[0] as Y2Axis; }
        }

        /// <summary>
        /// 获取Y轴列表
        /// </summary>
        public List<YAxis> YAxisList
        {
            get { return m_yAxisList; }
        }

        /// <summary>
        /// 获取右Y轴列表
        /// </summary>
        public List<Y2Axis> Y2AxisList
        {
            get { return m_y2AxisList; }
        }

        /// <summary>
        /// 添加柱状图
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="points">点集</param>
        /// <param name="color">颜色</param>
        /// <returns>柱状图</returns>
        public BarItem AddBar(String label, IPointList points, Color color)
        {
            BarItem curve = new BarItem(label, points, color);
            m_curveList.Add(curve);
            return curve;
        }

        /// <summary>
        /// 添加柱状图
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="x">X轴点列表</param>
        /// <param name="y">Y轴点列表</param>
        /// <param name="color">颜色</param>
        /// <returns>柱状图</returns>
        public BarItem AddBar(String label, double[] x, double[] y, Color color)
        {
            BarItem curve = new BarItem(label, x, y, color);
            m_curveList.Add(curve);
            return curve;
        }

        /// <summary>
        /// 添加曲线
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="x">X轴点列表</param>
        /// <param name="y">Y轴点列表</param>
        /// <param name="color">颜色</param>
        /// <returns>曲线</returns>
        public LineItem AddCurve(String label, double[] x, double[] y, Color color)
        {
            LineItem curve = new LineItem(label, x, y, color, SymbolType.Default);
            m_curveList.Add(curve);
            return curve;
        }

        /// <summary>
        /// 添加曲线
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="points">点集</param>
        /// <param name="color">颜色</param>
        /// <returns>曲线</returns>
        public LineItem AddCurve(String label, IPointList points, Color color)
        {
            LineItem curve = new LineItem(label, points, color, SymbolType.Default);
            m_curveList.Add(curve);
            return curve;
        }

        /// <summary>
        /// 添加曲线
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="x">X轴点列表</param>
        /// <param name="y">Y轴点列表</param>
        /// <param name="color">颜色</param>
        /// <param name="symbolType">记号类型</param>
        /// <returns>曲线</returns>
        public LineItem AddCurve(String label, double[] x, double[] y,
            Color color, SymbolType symbolType)
        {
            LineItem curve = new LineItem(label, x, y, color, symbolType);
            m_curveList.Add(curve);
            return curve;
        }

        /// <summary>
        /// 添加曲线
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="points">点集</param>
        /// <param name="color">颜色</param>
        /// <param name="symbolType">记号类型</param>
        /// <returns>曲线</returns>
        public LineItem AddCurve(String label, IPointList points,
            Color color, SymbolType symbolType)
        {
            LineItem curve = new LineItem(label, points, color, symbolType);
            m_curveList.Add(curve);
            return curve;
        }

        /// <summary>
        /// 添加饼图切片
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="color">颜色</param>
        /// <param name="displacement">位移</param>
        /// <param name="label">标签</param>
        /// <returns>饼图切片</returns>
        public PieItem AddPieSlice(double value, Color color, double displacement, String label)
        {
            PieItem slice = new PieItem(value, color, displacement, label);
            this.CurveList.Add(slice);
            return slice;
        }

        /// <summary>
        /// 添加饼图切片
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="color1">颜色1</param>
        /// <param name="color2">颜色2</param>
        /// <param name="fillAngle">填充角度</param>
        /// <param name="displacement">位移</param>
        /// <param name="label">标签</param>
        /// <returns>饼图切片</returns>
        public PieItem AddPieSlice(double value, Color color1, Color color2, float fillAngle,
                        double displacement, String label)
        {
            PieItem slice = new PieItem(value, color1, color2, fillAngle, displacement, label);
            this.CurveList.Add(slice);
            return slice;
        }

        /// <summary>
        /// 添加饼图
        /// </summary>
        /// <param name="pie">饼图</param>
        public void AddPie(Pie pie)
        {
            pie.Index = this.CurveList.Count;
            this.CurveList.Add(pie);
        }

        /// <summary>
        /// 添加饼图切片组
        /// </summary>
        /// <param name="values">数值列表</param>
        /// <param name="labels">标签列表</param>
        /// <returns>切片组</returns>
        public PieItem[] AddPieSlices(double[] values, String[] labels)
        {
            PieItem[] slices = new PieItem[values.Length];
            for (int x = 0; x < values.Length; x++)
            {
                slices[x] = new PieItem(values[x], labels[x]);
                this.CurveList.Add(slices[x]);
            }
            return slices;
        }

        /// <summary>
        /// 添加棒图
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="x">X轴点列表</param>
        /// <param name="y">Y轴点列表</param>
        /// <param name="color">颜色</param>
        /// <returns>棒图</returns>
        public StickItem AddStick(String label, double[] x, double[] y, Color color)
        {
            StickItem curve = new StickItem(label, x, y, color);
            m_curveList.Add(curve);
            return curve;
        }

        /// <summary>
        /// 添加棒图
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="points">点集</param>
        /// <param name="color">颜色</param>
        /// <returns>棒图</returns>
        public StickItem AddStick(String label, IPointList points, Color color)
        {
            StickItem curve = new StickItem(label, points, color);
            m_curveList.Add(curve);
            return curve;
        }

        /// <summary>
        /// 坐标轴改变
        /// </summary>
        /// <param name="g">绘图对象</param>
        public void AxisChange(Graphics g)
        {
            m_curveList.GetRange( 
                m_isIgnoreInitial, m_isBoundedRanges, this);
            float scaleFactor = this.CalcScaleFactor();
            if (this.CurveList.IsPieOnly)
            {
                this.XAxis.IsVisible = false;
                this.YAxis.IsVisible = false;
                this.Y2Axis.IsVisible = false;
                m_chart.Border.IsVisible = false;
                this.Legend.Position = LegendPos.TopCenter;
            }
            else
            {
                m_chart.Border.IsVisible = true;
            }
            if (m_barSettings.m_clusterScaleWidthAuto)
                m_barSettings.m_clusterScaleWidth = 1.0;
            if (m_chart.m_isRectAuto)
            {
                PickScale(g, scaleFactor);
                m_chart.m_rect = CalcChartRect(g);
            }
            PickScale(g, scaleFactor);
            m_barSettings.CalcClusterScaleWidth();
        }

        /// <summary>
        /// 坐标轴区域验证
        /// </summary>
        /// <returns></returns>
        private bool AxisRangesValid()
        {
            bool showGraf = m_xAxis.m_scale.m_min < m_xAxis.m_scale.m_max;
            foreach (Axis axis in m_yAxisList)
                if (axis.m_scale.m_min >= axis.m_scale.m_max)
                    showGraf = false;
            foreach (Axis axis in m_y2AxisList)
                if (axis.m_scale.m_min >= axis.m_scale.m_max)
                    showGraf = false;
            return showGraf;
        }

        /// <summary>
        /// 添加Y轴
        /// </summary>
        /// <param name="title">标题</param>
        /// <returns>索引</returns>
        public int AddYAxis(String title)
        {
            YAxis axis = new YAxis(title);
            axis.MajorTic.IsOpposite = false;
            axis.MinorTic.IsOpposite = false;
            axis.MajorTic.IsInside = false;
            axis.MinorTic.IsInside = false;
            m_yAxisList.Add(axis);
            return m_yAxisList.Count - 1;
        }

        /// <summary>
        /// 添加右Y轴
        /// </summary>
        /// <param name="title">标题</param>
        /// <returns>索引</returns>
        public int AddY2Axis(String title)
        {
            Y2Axis axis = new Y2Axis(title);
            axis.MajorTic.IsOpposite = false;
            axis.MinorTic.IsOpposite = false;
            axis.MajorTic.IsInside = false;
            axis.MinorTic.IsInside = false;
            m_y2AxisList.Add(axis);
            return m_y2AxisList.Count - 1;
        }

        /// <summary>
        /// 计算区域
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <returns>区域</returns>
        public RectangleF CalcChartRect(Graphics g)
        {
            return CalcChartRect(g, CalcScaleFactor());
        }

        /// <summary>
        /// 计算区域
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <returns>区域</returns>
        public RectangleF CalcChartRect(Graphics g, float scaleFactor)
        {
            RectangleF clientRect = this.CalcClientRect(g, scaleFactor);
            float totSpaceY = 0;
            float minSpaceL = 0;
            float minSpaceR = 0;
            float minSpaceB = 0;
            float minSpaceT = 5;
            m_xAxis.CalcSpace(g, this, scaleFactor, out minSpaceB);
            foreach (Axis axis in m_yAxisList)
            {
                float fixedSpace;
                float tmp = axis.CalcSpace(g, this, scaleFactor, out fixedSpace);
                if (axis.IsCrossShifted(this))
                    totSpaceY += tmp;
                minSpaceL += fixedSpace;
            }
            foreach (Axis axis in m_y2AxisList)
            {
                float fixedSpace;
                float tmp = axis.CalcSpace(g, this, scaleFactor, out fixedSpace);
                if (axis.IsCrossShifted(this))
                    totSpaceY += tmp;
                minSpaceR += fixedSpace;
            }
            float spaceB = 0, spaceT = 0, spaceL = 0, spaceR = 0;
            SetSpace(m_xAxis, clientRect.Height - m_xAxis.m_tmpSpace, ref spaceB, ref spaceT);
            m_xAxis.m_tmpSpace = spaceB;
            float totSpaceL = 0;
            float totSpaceR = 0;
            foreach (Axis axis in m_yAxisList)
            {
                SetSpace(axis, clientRect.Width - totSpaceY, ref spaceL, ref spaceR);
                minSpaceR = Math.Max(minSpaceR, spaceR);
                totSpaceL += spaceL;
                axis.m_tmpSpace = spaceL;
            }
            foreach (Axis axis in m_y2AxisList)
            {
                SetSpace(axis, clientRect.Width - totSpaceY, ref spaceR, ref spaceL);
                minSpaceL = Math.Max(minSpaceL, spaceL);
                totSpaceR += spaceR;
                axis.m_tmpSpace = spaceR;
            }
            RectangleF tmpRect = clientRect;
            totSpaceL = Math.Max(totSpaceL, minSpaceL);
            totSpaceR = Math.Max(totSpaceR, minSpaceR);
            spaceB = Math.Max(spaceB, minSpaceB);
            spaceT = Math.Max(spaceT, minSpaceT);
            tmpRect.X += totSpaceL;
            tmpRect.Width -= totSpaceL + totSpaceR;
            tmpRect.Height -= spaceT + spaceB;
            tmpRect.Y += spaceT;
            m_legend.CalcRect(g, this, scaleFactor, ref tmpRect);
            return tmpRect;
        }

        /// <summary>
        /// 比较方法
        /// </summary>
        /// <param name="gpo1">图形1</param>
        /// <param name="gpo2">图形2</param>
        /// <returns>比较结果</returns>
        public int CompFunc(GraphObj gpo1, GraphObj gpo2)
        {
            if (gpo1.ZIndex > gpo2.ZIndex)
            {
                return 1;
            }
            else if (gpo1.ZIndex == gpo2.ZIndex)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (XAxis != null)
                XAxis.Dispose();
            if (m_yAxisList != null)
            {
                foreach (Axis x in m_yAxisList)
                    x.Dispose();
                m_yAxisList.Clear();
            }
            if (m_y2AxisList != null)
            {
                foreach (Axis x in m_y2AxisList)
                    x.Dispose();
                m_y2AxisList.Clear();
            }
            if (CurveList != null)
            {
                foreach (CurveItem item in CurveList)
                {
                    item.Dispose();
                }
                CurveList.Clear();
            }
            if (Chart != null)
                Chart.Dispose();
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="g">绘图对象</param>
        public override void Draw(Graphics g)
        {
            base.Draw(g);
            if (m_rect.Width <= 1 || m_rect.Height <= 1)
                return;
            g.SetClip(m_rect);
            float scaleFactor = this.CalcScaleFactor();
            if (m_chart.m_isRectAuto)
            {
                m_chart.m_rect = CalcChartRect(g, scaleFactor);
            }
            else
                CalcChartRect(g, scaleFactor);
            if (m_chart.m_rect.Width < 1 || m_chart.m_rect.Height < 1)
                return;
            bool showGraf = true;
            if (showGraf)
            {
                m_graphObjList.Sort(new Comparison<GraphObj>(CompFunc));
            }
            m_xAxis.Scale.SetupScaleData(this, m_xAxis);
            foreach (Axis axis in m_yAxisList)
                axis.Scale.SetupScaleData(this, axis);
            foreach (Axis axis in m_y2AxisList)
                axis.Scale.SetupScaleData(this, axis);
            if (showGraf)
                m_graphObjList.Draw(g, this, scaleFactor, ZOrder.G_BehindChartFill);
            m_chart.Fill.Draw(g, m_chart.m_rect);
            if (showGraf)
            {
                m_graphObjList.Draw(g, this, scaleFactor, ZOrder.F_BehindGrid);
                DrawGrid(g, scaleFactor);
                m_graphObjList.Draw(g, this, scaleFactor, ZOrder.E_BehindCurves);
                RectangleF tempChartRec = new RectangleF(m_chart.m_rect.X, m_chart.m_rect.Y - 10, m_chart.m_rect.Width, m_chart.Rect.Height + 10);
                g.SetClip(tempChartRec);
                m_curveList.Draw(g, this, scaleFactor);
                g.SetClip(m_rect);
            }
            if (showGraf)
            {
                m_graphObjList.Draw(g, this, scaleFactor, ZOrder.D_BehindAxis);
                m_xAxis.Draw(g, this, scaleFactor, 0.0f);
                float yPos = 0;
                foreach (Axis axis in m_yAxisList)
                {
                    axis.Draw(g, this, scaleFactor, yPos);
                    yPos += axis.m_tmpSpace;
                }
                yPos = 0;
                foreach (Axis axis in m_y2AxisList)
                {
                    axis.Draw(g, this, scaleFactor, yPos);
                    yPos += axis.m_tmpSpace;
                }
                m_graphObjList.Draw(g, this, scaleFactor, ZOrder.C_BehindChartBorder);
            }
            if (showGraf)
            {
                m_graphObjList.Draw(g, this, scaleFactor, ZOrder.B_BehindLegend);
                m_legend.Draw(g, this, scaleFactor);
                m_graphObjList.Draw(g, this, scaleFactor, ZOrder.A_InFront);
            }
            if (IsDrawTrend)
            {
                m_Trend.Draw(g, this);
            }
            g.ResetClip();
        }

        /// <summary>
        /// 绘制网格
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="scaleFactor">刻度因子</param>
        internal void DrawGrid(Graphics g, float scaleFactor)
        {
            m_xAxis.DrawGrid(g, this, scaleFactor, 0.0f);
            float shiftPos = 0.0f;
            foreach (YAxis yAxis in m_yAxisList)
            {
                yAxis.DrawGrid(g, this, scaleFactor, shiftPos);
                shiftPos += yAxis.m_tmpSpace;
            }
            shiftPos = 0.0f;
            foreach (Y2Axis y2Axis in m_y2AxisList)
            {
                y2Axis.DrawGrid(g, this, scaleFactor, shiftPos);
                shiftPos += y2Axis.m_tmpSpace;
            }
        }

        /// <summary>
        /// 查找包含的对象
        /// </summary>
        /// <param name="rectF">区域</param>
        /// <param name="g">绘图对象</param>
        /// <param name="containedObjs">包含线</param>
        /// <returns>是否找到</returns>
        public bool FindContainedObjects(RectangleF rectF, Graphics g,
out CurveList containedObjs)
        {
            containedObjs = new CurveList();
            foreach (CurveItem ci in this.CurveList)
            {
                for (int i = 0; i < ci.Points.Count; i++)
                {
                    if (ci.Points[i].X > rectF.Left &&
                         ci.Points[i].X < rectF.Right &&
                         ci.Points[i].Y > rectF.Bottom &&
                         ci.Points[i].Y < rectF.Top)
                    {
                        containedObjs.Add(ci);
                    }
                }
            }
            return (containedObjs.Count > 0);
        }

        /// <summary>
        /// 查找最近的对象
        /// </summary>
        /// <param name="mousePt">坐标</param>
        /// <param name="g">绘图对象</param>
        /// <param name="nearestObj">最近的对象</param>
        /// <param name="index">索引</param>
        /// <returns>最近的对象</returns>
        public bool FindNearestObject(PointF mousePt, Graphics g,
            out object nearestObj, out int index)
        {
            nearestObj = null;
            index = -1;
            if (AxisRangesValid())
            {
                float scaleFactor = CalcScaleFactor();
                RectangleF tmpRect;
                GraphObj saveGraphItem = null;
                int saveIndex = -1;
                ZOrder saveZOrder = ZOrder.H_BehindAll;
                RectangleF tmpChartRect = CalcChartRect(g, scaleFactor);
                if (this.GraphObjList.FindPoint(mousePt, this, g, scaleFactor, out index))
                {
                    saveGraphItem = this.GraphObjList[index];
                    saveIndex = index;
                    saveZOrder = saveGraphItem.ZOrder;
                }
                if (saveZOrder <= ZOrder.B_BehindLegend &&
                    this.Legend.FindPoint(mousePt, this, scaleFactor, out index))
                {
                    nearestObj = this.Legend;
                    return true;
                }
                SizeF paneTitleBox = m_title.m_fontSpec.BoundingBox(g, m_title.m_text, scaleFactor);
                if (saveZOrder <= ZOrder.H_BehindAll && m_title.m_isVisible)
                {
                    tmpRect = new RectangleF((m_rect.Left + m_rect.Right - paneTitleBox.Width) / 2,
                        m_rect.Top + m_margin.Top * scaleFactor,
                        paneTitleBox.Width, paneTitleBox.Height);
                    if (tmpRect.Contains(mousePt))
                    {
                        nearestObj = this;
                        return true;
                    }
                }
                float left = tmpChartRect.Left;
                for (int yIndex = 0; yIndex < m_yAxisList.Count; yIndex++)
                {
                    Axis yAxis = m_yAxisList[yIndex];
                    float width = yAxis.m_tmpSpace;
                    if (width > 0)
                    {
                        tmpRect = new RectangleF(left - width, tmpChartRect.Top,
                            width, tmpChartRect.Height);
                        if (saveZOrder <= ZOrder.D_BehindAxis && tmpRect.Contains(mousePt))
                        {
                            nearestObj = yAxis;
                            index = yIndex;
                            return true;
                        }
                        left -= width;
                    }
                }
                left = tmpChartRect.Right;
                for (int yIndex = 0; yIndex < m_y2AxisList.Count; yIndex++)
                {
                    Axis y2Axis = m_y2AxisList[yIndex];
                    float width = y2Axis.m_tmpSpace;
                    if (width > 0)
                    {
                        tmpRect = new RectangleF(left, tmpChartRect.Top,
                            width, tmpChartRect.Height);
                        if (saveZOrder <= ZOrder.D_BehindAxis && tmpRect.Contains(mousePt))
                        {
                            nearestObj = y2Axis;
                            index = yIndex;
                            return true;
                        }
                        left += width;
                    }
                }
                float height = m_xAxis.m_tmpSpace;
                tmpRect = new RectangleF(tmpChartRect.Left, tmpChartRect.Bottom,
                    tmpChartRect.Width, height);
                if (saveZOrder <= ZOrder.D_BehindAxis && tmpRect.Contains(mousePt))
                {
                    nearestObj = this.XAxis;
                    return true;
                }
                tmpRect = new RectangleF(tmpChartRect.Left,
                        tmpChartRect.Top - height,
                        tmpChartRect.Width,
                        height);
                CurveItem curve;
                if (saveZOrder <= ZOrder.E_BehindCurves && FindNearestPoint(mousePt, out curve, out index))
                {
                    nearestObj = curve;
                    return true;
                }
                if (saveGraphItem != null)
                {
                    index = saveIndex;
                    nearestObj = saveGraphItem;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 查找最近的点
        /// </summary>
        /// <param name="mousePt">坐标</param>
        /// <param name="targetCurve">目标线</param>
        /// <param name="nearestCurve">最近的线</param>
        /// <param name="iNearest">最近的索引</param>
        /// <returns>是否找到</returns>
        public bool FindNearestPoint(PointF mousePt, CurveItem targetCurve,
                out CurveItem nearestCurve, out int iNearest)
        {
            CurveList targetCurveList = new CurveList();
            targetCurveList.Add(targetCurve);
            return FindNearestPoint(mousePt, targetCurveList,
                out nearestCurve, out iNearest);
        }

        /// <summary>
        /// 查找最近的点
        /// </summary>
        /// <param name="mousePt">坐标</param>
        /// <param name="nearestCurve">最近的线</param>
        /// <param name="iNearest">最近的索引</param>
        /// <returns>是否找到</returns>
        public bool FindNearestPoint(PointF mousePt,
            out CurveItem nearestCurve, out int iNearest)
        {
            return FindNearestPoint(mousePt, m_curveList,
                out nearestCurve, out iNearest);
        }

        /// <summary>
        /// 查找最近的点
        /// </summary>
        /// <param name="mousePt">坐标</param>
        /// <param name="targetCurveList">目标线列表</param>
        /// <param name="nearestCurve">最近的线</param>
        /// <param name="iNearest">最近的索引</param>
        /// <returns>是否找到</returns>
        public bool FindNearestPoint(PointF mousePt, CurveList targetCurveList,
            out CurveItem nearestCurve, out int iNearest)
        {
            CurveItem nearestBar = null;
            int iNearestBar = -1;
            nearestCurve = null;
            iNearest = -1;
            if (!m_chart.m_rect.Contains(mousePt))
                return false;
            double x, x2;
            double[] y;
            double[] y2;
            ReverseTransform(mousePt, out x, out x2, out y, out y2);
            if (!AxisRangesValid())
                return false;
            ValueHandler valueHandler = new ValueHandler(this, false);
            double yPixPerUnitAct, yAct, yMinAct, yMaxAct, xAct;
            double minDist = 1e20;
            double xVal, yVal, dist = 99999, distX, distY;
            double tolSquared = Default.NearestTol * Default.NearestTol;
            int iBar = 0;
            foreach (CurveItem curve in targetCurveList)
            {
                if (curve is Pie && curve.IsVisible)
                {
                    Pie pie = curve as Pie;
                    foreach (PieItem item in pie.Slices)
                    {
                        if (item.SlicePath != null &&
        item.SlicePath.IsVisible(mousePt))
                        {
                            nearestBar = curve;
                            iNearestBar = 0;
                        }
                    }
                    continue;
                }
                else if (curve.IsVisible)
                {
                    int yIndex = curve.GetYAxisIndex(this);
                    Axis yAxis = curve.GetYAxis(this);
                    Axis xAxis = curve.GetXAxis(this);
                    if (curve.IsY2Axis)
                    {
                        yAct = y2[yIndex];
                        yMinAct = m_y2AxisList[yIndex].m_scale.m_min;
                        yMaxAct = m_y2AxisList[yIndex].m_scale.m_max;
                    }
                    else
                    {
                        yAct = y[yIndex];
                        yMinAct = m_yAxisList[yIndex].m_scale.m_min;
                        yMaxAct = m_yAxisList[yIndex].m_scale.m_max;
                    }
                    yPixPerUnitAct = m_chart.m_rect.Height / (yMaxAct - yMinAct);
                    double xPixPerUnit = m_chart.m_rect.Width / (xAxis.m_scale.m_max - xAxis.m_scale.m_min);
                    xAct = xAxis is XAxis ? x : x2;
                    IPointList points = curve.Points;
                    float barWidth = curve.GetBarWidth(this);
                    double barWidthUserHalf;
                    Axis baseAxis = curve.BaseAxis(this);
                    bool isXBaseAxis = (baseAxis is XAxis);
                    if (isXBaseAxis)
                        barWidthUserHalf = barWidth / xPixPerUnit / 2.0;
                    else
                        barWidthUserHalf = barWidth / yPixPerUnitAct / 2.0;
                    if (points != null)
                    {
                        for (int iPt = 0; iPt < curve.NPts; iPt++)
                        {
                            if (xAxis.m_scale.IsAnyOrdinal && !curve.IsOverrideOrdinal)
                                xVal = (double)iPt + 1.0;
                            else
                                xVal = points[iPt].X;
                            if (yAxis.m_scale.IsAnyOrdinal && !curve.IsOverrideOrdinal)
                                yVal = (double)iPt + 1.0;
                            else
                                yVal = points[iPt].Y;
                            if (xVal != PointPair.Missing &&
                                    yVal != PointPair.Missing)
                            {
                                if (curve.IsBar)
                                {
                                    double baseVal, lowVal, hiVal;
                                    valueHandler.GetValues(curve, iPt, out baseVal,
                                            out lowVal, out hiVal);
                                    if (lowVal > hiVal)
                                    {
                                        double tmpVal = lowVal;
                                        lowVal = hiVal;
                                        hiVal = tmpVal;
                                    }
                                    if (isXBaseAxis)
                                    {
                                        double centerVal = valueHandler.BarCenterValue(curve, barWidth, iPt, xVal, iBar);
                                        if (xAct < centerVal - barWidthUserHalf ||
                                                xAct > centerVal + barWidthUserHalf ||
                                                yAct < lowVal || yAct > hiVal)
                                            continue;
                                    }
                                    else
                                    {
                                        double centerVal = valueHandler.BarCenterValue(curve, barWidth, iPt, yVal, iBar);
                                        if (yAct < centerVal - barWidthUserHalf ||
                                                yAct > centerVal + barWidthUserHalf ||
                                                xAct < lowVal || xAct > hiVal)
                                            continue;
                                    }
                                    if (nearestBar == null)
                                    {
                                        iNearestBar = iPt;
                                        nearestBar = curve;
                                    }
                                }
                                else if (xVal >= xAxis.m_scale.m_min && xVal <= xAxis.m_scale.m_max &&
                                            yVal >= yMinAct && yVal <= yMaxAct)
                                {
                                    if (curve is LineItem && m_lineType == LineType.Stack)
                                    {
                                        double zVal;
                                        valueHandler.GetValues(curve, iPt, out xVal, out zVal, out yVal);
                                    }
                                    distX = (xVal - xAct) * xPixPerUnit;
                                    distY = (yVal - yAct) * yPixPerUnitAct;
                                    dist = distX * distX + distY * distY;
                                    if (dist >= minDist)
                                        continue;
                                    minDist = dist;
                                    iNearest = iPt;
                                    nearestCurve = curve;
                                }
                            }
                        }
                        if (curve.IsBar)
                            iBar++;
                    }
                }
            }
            if (nearestCurve is LineItem)
            {
                float halfSymbol = (float)(((LineItem)nearestCurve).Symbol.Size *
                    CalcScaleFactor() / 2);
                minDist -= halfSymbol * halfSymbol;
                if (minDist < 0)
                    minDist = 0;
            }
            if (minDist >= tolSquared && nearestBar != null)
            {
                nearestCurve = nearestBar;
                iNearest = iNearestBar;
                return true;
            }
            else if (minDist < tolSquared)
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 查找对象
        /// </summary>
        /// <param name="mousePt">坐标</param>
        /// <param name="gpoOut">图形</param>
        /// <returns>是否找到</returns>
        internal bool FindObj(Point mousePt, out GraphObj gpoOut)
        {
            gpoOut = null;
            foreach (GraphObj gpo in this.GraphObjList)
            {
                if (gpo.InBox(mousePt, out gpoOut))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 找到焦点对象
        /// </summary>
        /// <param name="gpoOut">图形</param>
        /// <returns>是否找到</returns>
        internal bool FindObjFocused(out GraphObj gpoOut)
        {
            gpoOut = null;
            foreach (GraphObj gpo in this.GraphObjList)
            {
                if (gpo.Focused)
                {
                    gpoOut = gpo;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 强迫数记号
        /// </summary>
        /// <param name="axis">坐标轴</param>
        /// <param name="numTics">记号</param>
        private void ForceNumTics(Axis axis, int numTics)
        {
            if (axis.m_scale.MaxAuto)
            {
                int nTics = axis.m_scale.CalcNumTics();
                if (nTics < numTics)
                    axis.m_scale.m_maxLinearized += axis.m_scale.m_majorStep * (numTics - nTics);
            }
        }

        /// <summary>
        /// 获取修正后的点
        /// </summary>
        /// <param name="ptF">坐标</param>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        public void GetFixedPostion(PointF ptF, out double x, out double y)
        {
            x = 0; y = 0;
            x = (ptF.X - this.Rect.X) / this.Rect.Width;
            y = (ptF.Y - this.Rect.Y) / this.Rect.Height;
        }

        /// <summary>
        /// 获取修正后的点
        /// </summary>
        /// <param name="points">点集</param>
        /// <param name="outPoints">输出点</param>
        public void GetFixedPostion(PointF[] points, out PointF[] outPoints)
        {
            outPoints = new PointF[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                double x, y;
                GetFixedPostion(points[i], out x, out y);
                outPoints[i] = new PointF((float)x, (float)y);
            }
        }

        /// <summary>
        /// 生成转换点
        /// </summary>
        /// <param name="ptF">坐标</param>
        /// <param name="coord">坐标轴类型</param>
        /// <returns>转换点</returns>
        public PointF GeneralTransform(PointF ptF, CoordType coord)
        {
            m_xAxis.Scale.SetupScaleData(this, m_xAxis);
            foreach (Axis axis in m_yAxisList)
                axis.Scale.SetupScaleData(this, axis);
            foreach (Axis axis in m_y2AxisList)
                axis.Scale.SetupScaleData(this, axis);
            return this.TransformCoord(ptF.X, ptF.Y, coord);
        }

        /// <summary>
        /// 生成转换点
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="coord">坐标轴类型</param>
        /// <returns>转换点</returns>
        public PointF GeneralTransform(double x, double y, CoordType coord)
        {
            m_xAxis.Scale.SetupScaleData(this, m_xAxis);
            foreach (Axis axis in m_yAxisList)
                axis.Scale.SetupScaleData(this, axis);
            foreach (Axis axis in m_y2AxisList)
                axis.Scale.SetupScaleData(this, axis);
            return this.TransformCoord(x, y, coord);
        }

        /// <summary>
        /// 选择刻度
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="scaleFactor">刻度因子</param>
        private void PickScale(Graphics g, float scaleFactor)
        {
            int maxTics = 0;
            m_xAxis.m_scale.PickScale(this, g, scaleFactor);
            foreach (Axis axis in m_yAxisList)
            {
                axis.m_scale.PickScale(this, g, scaleFactor);
                if (axis.m_scale.MaxAuto)
                {
                    int nTics = axis.m_scale.CalcNumTics();
                    maxTics = nTics > maxTics ? nTics : maxTics;
                }
            }
            foreach (Axis axis in m_y2AxisList)
            {
                axis.m_scale.PickScale(this, g, scaleFactor);
                if (axis.m_scale.MaxAuto)
                {
                    int nTics = axis.m_scale.CalcNumTics();
                    maxTics = nTics > maxTics ? nTics : maxTics;
                }
            }
            if (m_isAlignGrids)
            {
                foreach (Axis axis in m_yAxisList)
                    ForceNumTics(axis, maxTics);
                foreach (Axis axis in m_y2AxisList)
                    ForceNumTics(axis, maxTics);
            }
        }

        /// <summary>
        /// 反转
        /// </summary>
        /// <param name="ptF">坐标</param>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        public void ReverseTransform(PointF ptF, out double x, out double y)
        {
            m_xAxis.Scale.SetupScaleData(this, m_xAxis);
            this.YAxis.Scale.SetupScaleData(this, this.YAxis);
            x = this.XAxis.Scale.ReverseTransform(ptF.X);
            y = this.YAxis.Scale.ReverseTransform(ptF.Y);
        }

        /// <summary>
        /// 反转
        /// </summary>
        /// <param name="ptF">坐标</param>
        /// <param name="x">横坐标值</param>
        /// <param name="x2">横坐标值2</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="y2">纵坐标值2</param>
        public void ReverseTransform(PointF ptF, out double x, out double x2, out double y,
            out double y2)
        {
            m_xAxis.Scale.SetupScaleData(this, m_xAxis);
            this.YAxis.Scale.SetupScaleData(this, this.YAxis);
            this.Y2Axis.Scale.SetupScaleData(this, this.Y2Axis);
            x = this.XAxis.Scale.ReverseTransform(ptF.X);
            y = this.YAxis.Scale.ReverseTransform(ptF.Y);
            x2 = x;
            y2 = this.Y2Axis.Scale.ReverseTransform(ptF.Y);
        }

        /// <summary>
        /// 反转
        /// </summary>
        /// <param name="ptF">坐标</param>
        /// <param name="isX2Axis">是否X轴</param>
        /// <param name="isY2Axis">是否Y轴</param>
        /// <param name="yAxisIndex">Y轴索引</param>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        public void ReverseTransform(PointF ptF, bool isX2Axis, bool isY2Axis, int yAxisIndex,
                    out double x, out double y)
        {
            Axis xAxis = m_xAxis;
            xAxis.Scale.SetupScaleData(this, xAxis);
            x = xAxis.Scale.ReverseTransform(ptF.X);
            Axis yAxis = null;
            if (isY2Axis && Y2AxisList.Count > yAxisIndex)
                yAxis = Y2AxisList[yAxisIndex];
            else if (!isY2Axis && YAxisList.Count > yAxisIndex)
                yAxis = YAxisList[yAxisIndex];
            if (yAxis != null)
            {
                yAxis.Scale.SetupScaleData(this, yAxis);
                y = yAxis.Scale.ReverseTransform(ptF.Y);
            }
            else
                y = PointPair.Missing;
        }

        /// <summary>
        /// 反转
        /// </summary>
        /// <param name="ptF">坐标</param>
        /// <param name="x">横坐标值</param>
        /// <param name="x2">横坐标值2</param>
        /// <param name="y">纵坐标值列表</param>
        /// <param name="y2">纵坐标值2</param>
        public void ReverseTransform(PointF ptF, out double x, out double x2, out double[] y,
            out double[] y2)
        {
            m_xAxis.Scale.SetupScaleData(this, m_xAxis);
            x = this.XAxis.Scale.ReverseTransform(ptF.X);
            x2 = x;
            y = new double[m_yAxisList.Count];
            y2 = new double[m_y2AxisList.Count];
            for (int i = 0; i < m_yAxisList.Count; i++)
            {
                Axis axis = m_yAxisList[i];
                axis.Scale.SetupScaleData(this, axis);
                y[i] = axis.Scale.ReverseTransform(ptF.Y);
            }
            for (int i = 0; i < m_y2AxisList.Count; i++)
            {
                Axis axis = m_y2AxisList[i];
                axis.Scale.SetupScaleData(this, axis);
                y2[i] = axis.Scale.ReverseTransform(ptF.Y);
            }
        }

        /// <summary>
        /// 设置最小空间缓存
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="bufferFraction">分数缓存</param>
        /// <param name="isGrowOnly">是否增长</param>
        public void SetMinSpaceBuffer(Graphics g, float bufferFraction, bool isGrowOnly)
        {
            m_xAxis.SetMinSpaceBuffer(g, this, bufferFraction, isGrowOnly);
            foreach (Axis axis in m_yAxisList)
                axis.SetMinSpaceBuffer(g, this, bufferFraction, isGrowOnly);
            foreach (Axis axis in m_y2AxisList)
                axis.SetMinSpaceBuffer(g, this, bufferFraction, isGrowOnly);
        }

        /// <summary>
        /// 设置空间
        /// </summary>
        /// <param name="axis">坐标轴</param>
        /// <param name="clientSize">客户端尺寸</param>
        /// <param name="spaceNorm">空间标准</param>
        /// <param name="spaceAlt">空间加速</param>
        private void SetSpace(Axis axis, float clientSize, ref float spaceNorm, ref float spaceAlt)
        {
            float crossFrac = axis.CalcCrossFraction(this);
            float crossPix = crossFrac * (1 + crossFrac) * (1 + crossFrac * crossFrac) * clientSize;
            if (!axis.IsPrimary(this) && axis.IsCrossShifted(this))
                axis.m_tmpSpace = 0;
            if (axis.m_tmpSpace < crossPix)
                axis.m_tmpSpace = 0;
            else if (crossPix > 0)
                axis.m_tmpSpace -= crossPix;
            if (axis.m_scale.m_isLabelsInside && (axis.IsPrimary(this) || (crossFrac != 0.0 && crossFrac != 1.0)))
                spaceAlt = axis.m_tmpSpace;
            else
                spaceNorm = axis.m_tmpSpace;
        }
        #endregion
    }
}
