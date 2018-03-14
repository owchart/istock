/***************************************************************************\
*                                                                           *
* CurveItem.cs -  CurveItem functions, types, and definitions               *
*                                                                           *
*               Version 1.00                                                *
*                                                                           *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved. *
*                                                                           *
****************************************************************************/

using System;
using System.Drawing;
using System.Collections;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Collections.Generic;
namespace OwLib
{
    /// <summary>
    /// 曲线
    /// </summary>
    [Serializable]
    public abstract class CurveItem : IDisposable
    {
        #region 陶德 2016/5/31
        /// <summary>
        /// 创建曲线
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="x">X轴数值</param>
        /// <param name="y">Y轴数值</param>
        public CurveItem(String label, double[] x, double[] y)
            :
            this(label, new PointPairList(x, y))
        {
        }

        /// <summary>
        /// 创建曲线
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="points">点集</param>
        public CurveItem(String label, IPointList points)
        {
            Init(label);
            if (points == null)
                m_points = new PointPairList();
            else
                m_points = points;
            this.legendCheckBox.OwnerCurve = this;
        }

        /// <summary>
        /// 创建曲线
        /// </summary>
        /// <param name="label">标签</param>
        public CurveItem(String label)
            : this(label, null)
        {
        }

        /// <summary>
        /// 创建曲线
        /// </summary>
        public CurveItem()
        {
            Init(null);
        }

        /// <summary>
        /// 创建曲线
        /// </summary>
        /// <param name="rhs">其他曲线</param>
        public CurveItem(CurveItem rhs)
        {
            m_label = new Label(rhs.m_label);
            m_isY2Axis = rhs.IsY2Axis;
            m_isVisible = rhs.IsVisible;
            m_isOverrideOrdinal = rhs.m_isOverrideOrdinal;
            m_yAxisIndex = rhs.m_yAxisIndex;
            m_points = new PointPairList(rhs.Points);
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~CurveItem()
        {
            Dispose();
        }

        /// <summary>
        /// 图例的复选框
        /// </summary>
        public LegendCheckBox legendCheckBox = new LegendCheckBox();

        /// <summary>
        /// 数值格式
        /// </summary>
        public String ValueFormat = "N4";

        /// <summary>
        /// 获取或设置颜色
        /// </summary>
        public Color Color
        {
            get
            {
                if (this is BarItem)
                    return ((BarItem)this).Bar.Fill.Color;
                else if (this is LineItem && ((LineItem)this).Line.IsVisible)
                    return ((LineItem)this).Line.Color;
                else if (this is LineItem)
                    return ((LineItem)this).Symbol.Border.Color;
                else if (this is Pie)
                    return ((Pie)this).Color;
                else
                    return Color.Empty;
            }
            set
            {
                if (this is BarItem)
                {
                    ((BarItem)this).Bar.Fill.Color = value;
                }
                else if (this is LineItem)
                {
                    ((LineItem)this).Line.Color = value;
                    ((LineItem)this).Symbol.Border.Color = value;
                    ((LineItem)this).Symbol.Fill.Color = value;
                    if (((LineItem)this).Line.Fill.IsVisible == true)
                    {
                        ((LineItem)this).Line.Fill = new Fill(Color.White, value, 45F);
                    }
                }
            }
        }

        /// <summary>
        /// 获取是否是柱状图
        /// </summary>
        public bool IsBar
        {
            get { return this is BarItem; }
        }

        /// <summary>
        /// 获取是否是线
        /// </summary>
        public bool IsLine
        {
            get { return this is LineItem; }
        }

       /// <summary>
       /// 获取点的数量
       /// </summary>
        public int NPts
        {
            get
            {
                if (m_points == null)
                    return 0;
                else
                    return m_points.Count;
            }
        }

        protected bool m_isOverrideOrdinal;

        /// <summary>
        /// 获取或设置是否重写序号
        /// </summary>
        public bool IsOverrideOrdinal
        {
            get { return m_isOverrideOrdinal; }
            set { m_isOverrideOrdinal = value; }
        }

        /// <summary>
        /// 获取是否是饼图
        /// </summary>
        public bool IsPie
        {
            get { return (this is PieItem || this is Pie); }
        }

        protected bool m_isSelectable;

        /// <summary>
        /// 获取或设置是否可选
        /// </summary>
        public bool IsSelectable
        {
            get { return m_isSelectable; }
            set { m_isSelectable = value; }
        }

        protected bool m_isSelected;

        /// <summary>
        /// 获取或设置是否选中
        /// </summary>
        public bool IsSelected
        {
            get { return m_isSelected; }
            set { m_isSelected = value; }
        }

        protected bool m_IsNotShowCurveButLegend = false;

        /// <summary>
        /// 获取或设置是否只显示图例
        /// </summary>
        public bool IsNotShowCurveButLegend
        {
            get { return m_IsNotShowCurveButLegend; }
            set { m_IsNotShowCurveButLegend = value; }
        }

        protected bool m_isY2Axis;

        /// <summary>
        /// 获取或设置是否在右Y轴
        /// </summary>
        public bool IsY2Axis
        {
            get { return m_isY2Axis; }
            set { m_isY2Axis = value; }
        }

        protected bool m_isVisible;

        /// <summary>
        /// 获取或设置是否可见
        /// </summary>
        public bool IsVisible
        {
            get { return m_isVisible; }
            set { m_isVisible = value; }
        }

        internal Label m_label;

        /// <summary>
        /// 获取或设置标签
        /// </summary>
        public Label Label
        {
            get { return m_label; }
            set { m_label = value; }
        }

        protected IPointList m_points;

        /// <summary>
        /// 获取或设置所有的点
        /// </summary>
        public IPointList Points
        {
            get { return m_points; }
            set { m_points = value; }
        }

        /// <summary>
        /// 获取点
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>点</returns>
        public PointPair this[int index]
        {
            get
            {
                if (m_points == null)
                    return new PointPair(PointPair.Missing, PointPair.Missing);
                else
                    return (m_points)[index];
            }
        }

        protected int m_yAxisIndex;

        /// <summary>
        /// 获取或设置Y轴的索引
        /// </summary>
        public int YAxisIndex
        {
            get { return m_yAxisIndex; }
            set { m_yAxisIndex = value; }
        }

        /// <summary>
        /// 添加点
        /// </summary>
        /// <param name="x">X值</param>
        /// <param name="y">Y值</param>
        public void AddPoint(double x, double y)
        {
            this.AddPoint(new PointPair(x, y));
        }

        /// <summary>
        /// 添加点
        /// </summary>
        /// <param name="point">点</param>
        public void AddPoint(PointPair point)
        {
            if (m_points == null)
                this.Points = new PointPairList();
            if (m_points is IPointListEdit)
                (m_points as IPointListEdit).Add(point);
            else
                throw new NotImplementedException();
        }

        /// <summary>
        /// 获取基础坐标轴
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns>坐标轴</returns>
        public virtual Axis BaseAxis(GraphPane pane)
        {
            BarBase barBase;
            if (this is BarItem)
                barBase = pane.m_barSettings.Base;
            else
                barBase = BarBase.X;
            if (barBase == BarBase.X)
                return pane.XAxis;
            else if (barBase == BarBase.Y)
                return pane.YAxis;
            else
                return pane.Y2Axis;
        }

        /// <summary>
        /// 清除点
        /// </summary>
        public void Clear()
        {
            if (m_points is IPointListEdit)
                (m_points as IPointListEdit).Clear();
            else
                throw new NotImplementedException();
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            if (m_points != null)
                m_points.Dispose();
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="pos">位置</param>
        /// <param name="scaleFactor">刻度因子</param>
        public abstract void Draw(Graphics g, GraphPane pane, int pos, float scaleFactor);

        /// <summary>
        /// 绘制图例
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="rect">区域</param>
        /// <param name="scaleFactor">刻度因子</param>
        public abstract void DrawLegendKey(Graphics g, GraphPane pane, RectangleF rect, float scaleFactor);

        /// <summary>
        /// 获取宽度
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns>宽度</returns>
        public float GetBarWidth(GraphPane pane)
        {
            float barWidth;
            float numBars = 1.0F;
            if (pane.m_barSettings.Type == BarType.Cluster)
                numBars = pane.CurveList.NumClusterableBars;
            float denom = numBars * (1.0F + pane.m_barSettings.MinBarGap) -
                        pane.m_barSettings.MinBarGap + pane.m_barSettings.MinClusterGap;
            if (denom <= 0)
                denom = 1;
            barWidth = pane.BarSettings.GetClusterWidth() / denom;
            if (barWidth <= 0)
                return 1;
            return barWidth;
        }

        /// <summary>
        /// 获取坐标
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="i">索引</param>
        /// <param name="coords">坐标</param>
        /// <returns>是否获取成功</returns>
        public abstract bool GetCoords(GraphPane pane, int i, out String coords);

        /// <summary>
        /// 获取区域
        /// </summary>
        /// <param name="xMin">X最小值</param>
        /// <param name="xMax">X最大值</param>
        /// <param name="yMin">Y最小值</param>
        /// <param name="yMax">Y最大值</param>
        /// <param name="ignoreInitial">是否忽视初始化</param>
        /// <param name="isBoundedRanges">是否矩形区域</param>
        /// <param name="pane">图层</param>
        public virtual void GetRange(out double xMin, out double xMax,
                                        out double yMin, out double yMax,
                                        bool ignoreInitial,
                                        bool isBoundedRanges,
                                        GraphPane pane)
        {
            double xLBound = double.MinValue;
            double xUBound = double.MaxValue;
            double yLBound = double.MinValue;
            double yUBound = double.MaxValue;
            xMin = yMin = Double.MaxValue;
            xMax = yMax = Double.MinValue;
            Axis yAxis = this.GetYAxis(pane);
            Axis xAxis = this.GetXAxis(pane);
            if (yAxis == null || xAxis == null)
                return;
            if (isBoundedRanges)
            {
                xLBound = xAxis.m_scale.m_lBound;
                xUBound = xAxis.m_scale.m_uBound;
                yLBound = yAxis.m_scale.m_lBound;
                yUBound = yAxis.m_scale.m_uBound;
            }
            bool isXIndependent = this.IsXIndependent(pane);
            bool isXLog = xAxis.Scale.IsLog;
            bool isYLog = yAxis.Scale.IsLog;
            bool isXOrdinal = xAxis.Scale.IsAnyOrdinal;
            bool isYOrdinal = yAxis.Scale.IsAnyOrdinal;
            bool isZOrdinal = (isXIndependent ? yAxis : xAxis).Scale.IsAnyOrdinal;
            if (this.Points == null)
            {
                return;
            }
            for (int i = 0; i < this.Points.Count; i++)
            {
                PointPair point = this.Points[i];
                double curX = isXOrdinal ? i + 1 : point.X;
                double curY = isYOrdinal ? i + 1 : point.Y;
                double curZ = isZOrdinal ? i + 1 : point.Z;
                bool outOfBounds = curX < xLBound || curX > xUBound ||
                    curY < yLBound || curY > yUBound ||
                    (isXIndependent && (curZ < yLBound || curZ > yUBound)) ||
                    (!isXIndependent && (curZ < xLBound || curZ > xUBound)) ||
                    (curX <= 0 && isXLog) || (curY <= 0 && isYLog);
                if (ignoreInitial && curY != 0 &&
                        curY != PointPair.Missing)
                    ignoreInitial = false;
                if (!ignoreInitial &&
                        !outOfBounds &&
                        curX != PointPair.Missing &&
                        curY != PointPair.Missing)
                {
                    if (curX < xMin)
                        xMin = curX;
                    if (curX > xMax)
                        xMax = curX;
                    if (curY < yMin)
                        yMin = curY;
                    if (curY > yMax)
                        yMax = curY;
                    if (isXIndependent && curZ != PointPair.Missing)
                    {
                        if (curZ < yMin)
                            yMin = curZ;
                        if (curZ > yMax)
                            yMax = curZ;
                    }
                    else if (curZ != PointPair.Missing)
                    {
                        if (curZ < xMin)
                            xMin = curZ;
                        if (curZ > xMax)
                            xMax = curZ;
                    }
                }
            }
        }

        /// <summary>
        /// 获取X轴
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns>X轴</returns>
        public Axis GetXAxis(GraphPane pane)
        {
            return pane.XAxis;
        }

        /// <summary>
        /// 获取Y轴
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns>Y轴</returns>
        public Axis GetYAxis(GraphPane pane)
        {
            if (m_isY2Axis)
            {
                if (m_yAxisIndex < pane.Y2AxisList.Count)
                    return pane.Y2AxisList[m_yAxisIndex];
                else
                    return pane.Y2AxisList[0];
            }
            else
            {
                if (m_yAxisIndex < pane.YAxisList.Count)
                    return pane.YAxisList[m_yAxisIndex];
                else
                    return pane.YAxisList[0];
            }
        }

        /// <summary>
        /// 获取Y轴的索引
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns>索引</returns>
        public int GetYAxisIndex(GraphPane pane)
        {
            if (m_yAxisIndex >= 0 &&
                    m_yAxisIndex < (m_isY2Axis ? pane.Y2AxisList.Count : pane.YAxisList.Count))
                return m_yAxisIndex;
            else
                return 0;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="label">标签</param>
        private void Init(String label)
        {
            m_label = new Label(label, null);
            m_isY2Axis = false;
            m_isVisible = true;
            m_isOverrideOrdinal = false;
            m_yAxisIndex = 0;
        }

        /// <summary>
        /// 生成唯一
        /// </summary>
        public void MakeUnique()
        {
            this.MakeUnique(ColorSymbolRotator.StaticInstance);
        }

        /// <summary>
        /// 生成唯一
        /// </summary>
        /// <param name="rotator">旋转</param>
        public virtual void MakeUnique(ColorSymbolRotator rotator)
        {
            this.Color = rotator.NextColor;
        }

        /// <summary>
        /// 是否X轴独立
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns>是否独立</returns>
        internal abstract bool IsXIndependent(GraphPane pane);

        /// <summary>
        /// 移除点
        /// </summary>
        /// <param name="index">索引</param>
        public void RemovePoint(int index)
        {
            if (m_points is IPointListEdit)
                (m_points as IPointListEdit).RemoveAt(index);
            else
                throw new NotImplementedException();
        }

        /// <summary>
        /// 获取值的轴
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns>轴</returns>
        public virtual Axis ValueAxis(GraphPane pane)
        {
            BarBase barBase;
            if (this is BarItem)
                barBase = pane.m_barSettings.Base;
            else
                barBase = BarBase.X;
            if (barBase == BarBase.X)
            {
                return GetYAxis(pane);
            }
            else
                return GetXAxis(pane);
        }

        /// <summary>
        /// 曲线比较类
        /// </summary>
        public class Comparer : IComparer<CurveItem>
        {
            /// <summary>
            /// 索引
            /// </summary>
            private int index;

            /// <summary>
            /// 排序类型
            /// </summary>
            private SortType2 sortType;

            /// <summary>
            /// 比较方法
            /// </summary>
            /// <param name="type">排序类型</param>
            /// <param name="index">索引</param>
            public Comparer(SortType2 type, int index)
            {
                this.sortType = type;
                this.index = index;
            }

            /// <summary>
            /// 比较方法
            /// </summary>
            /// <param name="l">线1</param>
            /// <param name="r">线2</param>
            /// <returns>大小比较</returns>
            public int Compare(CurveItem l, CurveItem r)
            {
                if (l == null && r == null)
                    return 0;
                else if (l == null && r != null)
                    return -1;
                else if (l != null && r == null)
                    return 1;
                if (r != null && r.NPts <= index)
                    r = null;
                if (l != null && l.NPts <= index)
                    l = null;
                double lVal, rVal;
                if (sortType == SortType2.XValues)
                {
                    lVal = (l != null) ? System.Math.Abs(l[index].X) : PointPair.Missing;
                    rVal = (r != null) ? System.Math.Abs(r[index].X) : PointPair.Missing;
                }
                else
                {
                    lVal = (l != null) ? System.Math.Abs(l[index].Y) : PointPair.Missing;
                    rVal = (r != null) ? System.Math.Abs(r[index].Y) : PointPair.Missing;
                }
                if (lVal == PointPair.Missing || Double.IsInfinity(lVal) || Double.IsNaN(lVal))
                    l = null;
                if (rVal == PointPair.Missing || Double.IsInfinity(rVal) || Double.IsNaN(rVal))
                    r = null;
                if ((l == null && r == null) || (System.Math.Abs(lVal - rVal) < 1e-10))
                    return 0;
                else if (l == null && r != null)
                    return -1;
                else if (l != null && r == null)
                    return 1;
                else
                    return rVal < lVal ? -1 : 1;
            }
        }
        #endregion
    }
}
