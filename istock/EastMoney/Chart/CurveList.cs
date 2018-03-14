/***************************************************************************\
*                                                                           *
* CurveList.cs -  CurveList functions, types, and definitions               *
*                                                                           *
*               Version 1.00                                                *
*                                                                           *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved. *
*                                                                           *
****************************************************************************/

using System;
using System.Drawing;
using System.Collections.Generic;

namespace OwLib
{
    /// <summary>
    /// 曲线集合
    /// </summary>
    [Serializable]
    public class CurveList : List<CurveItem>
    {
        #region 陶德 2016/5/31
        /// <summary>
        /// 创建集合
        /// </summary>
        public CurveList()
        {
            maxPts = 1;
        }

        /// <summary>
        /// 获取后一对象
        /// </summary>
        public IEnumerable<CurveItem> Backward
        {
            get
            {
                for (int i = this.Count - 1; i >= 0; i--)
                    yield return this[i];
            }
        }

        /// <summary>
        /// 获取前一对象
        /// </summary>
        public IEnumerable<CurveItem> Forward
        {
            get
            {
                for (int i = 0; i < this.Count; i++)
                    yield return this[i];
            }
        }

        /// <summary>
        /// 获取是否只有饼图
        /// </summary>
        public bool IsPieOnly
        {
            get
            {
                bool hasPie = false;
                foreach (CurveItem curve in this)
                {
                    if (!curve.IsPie)
                        return false;
                    else
                        hasPie = true;
                }
                return hasPie;
            }
        }

        private int maxPts;

        /// <summary>
        /// 获取最大点
        /// </summary>
        public int MaxPts
        {
            get { return maxPts; }
        }

        /// <summary>
        /// 获取柱的数量
        /// </summary>
        public int NumBars
        {
            get
            {
                int count = 0;
                foreach (CurveItem curve in this)
                {
                    if (curve.IsBar)
                        count++;
                }
                return count;
            }
        }

        /// <summary>
        /// 获取集群柱的数量
        /// </summary>
        public int NumClusterableBars
        {
            get
            {
                int count = 0;
                foreach (CurveItem curve in this)
                {
                    if (curve.IsBar)
                        count++;
                }
                return count;
            }
        }

        /// <summary>
        /// 获取饼图的数量
        /// </summary>
        public int NumPies
        {
            get
            {
                int count = 0;
                foreach (CurveItem curve in this)
                {
                    if (curve.IsPie)
                        count++;
                }
                return count;
            }
        }

        /// <summary>
        /// 获取项
        /// </summary>
        /// <param name="label">标签</param>
        /// <returns>项</returns>
        public CurveItem this[String label]
        {
            get
            {
                int index = IndexOf(label);
                if (index >= 0)
                    return (this[index]);
                else
                    return null;
            }
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        public void Draw(Graphics g, GraphPane pane, float scaleFactor)
        {
            int pos = this.NumBars;
            if (pane.m_barSettings.Type == BarType.SortedOverlay)
            {
                CurveList tempList = new CurveList();
                foreach (CurveItem curve in this)
                    if (curve.IsBar)
                        tempList.Add((CurveItem)curve);
                for (int i = 0; i < this.maxPts; i++)
                {
                    tempList.Sort(pane.m_barSettings.Base == BarBase.X ? SortType2.YValues : SortType2.XValues, i);
                    foreach (BarItem barItem in tempList)
                        barItem.Bar.DrawSingleBar(g, pane, barItem,
                            ((BarItem)barItem).BaseAxis(pane),
                            ((BarItem)barItem).ValueAxis(pane),
                            0, i, ((BarItem)barItem).GetBarWidth(pane), scaleFactor);
                }
            }
            for (int i = this.Count - 1; i >= 0; i--)
            {
                CurveItem curve = this[i];
                if (curve.IsBar)
                    pos--;
                if (!(curve.IsBar && pane.m_barSettings.Type == BarType.SortedOverlay))
                {
                    curve.Draw(g, pane, pos, scaleFactor);
                }
            }
        }

        /// <summary>
        /// 获取柱状图的位置
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="barItem">柱状图</param>
        /// <returns>位置</returns>
        public int GetBarItemPos(GraphPane pane, BarItem barItem)
        {
            if (pane.m_barSettings.Type == BarType.Overlay ||
                    pane.m_barSettings.Type == BarType.Stack ||
                    pane.m_barSettings.Type == BarType.PercentStack)
                return 0;
            int i = 0;
            foreach (CurveItem curve in this)
            {
                if (curve == barItem)
                    return i;
                else if (curve is BarItem)
                    i++;
            }
            return -1;
        }

        /// <summary>
        /// 获取区域
        /// </summary>
        /// <param name="bIgnoreInitial">是否忽视初始化</param>
        /// <param name="isBoundedRanges">是否矩形区域</param>
        /// <param name="pane">图层</param>
        public void GetRange(bool bIgnoreInitial, bool isBoundedRanges, GraphPane pane)
        {
            double tXMinVal, tXMaxVal, tYMinVal, tYMaxVal;
            InitScale(pane.XAxis.Scale, isBoundedRanges);
            foreach (YAxis axis in pane.YAxisList)
                InitScale(axis.Scale, isBoundedRanges);
            foreach (Y2Axis axis in pane.Y2AxisList)
                InitScale(axis.Scale, isBoundedRanges);
            maxPts = 1;
            bool OnlyLine = true;
            foreach (CurveItem curve in this)
            {
                if (curve.IsVisible)
                {
                    if (!(curve.IsLine && !((LineItem)curve).IsScatterPlot))
                    {
                        OnlyLine = false;
                        break;
                    }
                }
            }
            bool firstSet = true;
            foreach (CurveItem curve in this)
            {
                if (curve.IsVisible)
                {
                    if (((curve is BarItem) && (pane.m_barSettings.Type == BarType.Stack ||
                            pane.m_barSettings.Type == BarType.PercentStack)) ||
                        ((curve is LineItem) && pane.LineType == LineType.Stack))
                    {
                        GetStackRange(pane, curve, out tXMinVal, out tYMinVal,
                                        out tXMaxVal, out tYMaxVal);
                    }
                    else
                    {
                        curve.GetRange(out tXMinVal, out tXMaxVal,
                                        out tYMinVal, out tYMaxVal, bIgnoreInitial, true, pane);
                    }
                    if (OnlyLine)
                    {
                        if (firstSet)
                        {
                            pane.XAxis.Scale.Min = tXMinVal;
                            pane.XAxis.Scale.Max = tXMaxVal;
                            firstSet = false;
                        }
                        else
                        {
                            if (pane.XAxis.Scale.Min > tXMinVal)
                            {
                                pane.XAxis.Scale.Min = tXMinVal;
                            }
                            if (pane.XAxis.Scale.Max < tXMaxVal)
                            {
                                pane.XAxis.Scale.Max = tXMaxVal;
                            }
                        }
                    }
                    else
                    {
                        pane.XAxis.Scale.MinAuto = true;
                        pane.XAxis.Scale.MaxAuto = true;
                        pane.YAxis.Scale.MinAuto = true;
                        pane.YAxis.Scale.MaxAuto = true;
                    }
                    Scale yScale = curve.GetYAxis(pane).Scale;
                    Scale xScale = curve.GetXAxis(pane).Scale;
                    bool isYOrd = yScale.IsAnyOrdinal;
                    bool isXOrd = xScale.IsAnyOrdinal;
                    if (isYOrd && !curve.IsOverrideOrdinal)
                    {
                        tYMinVal = 1.0;
                        tYMaxVal = curve.NPts;
                    }
                    if (isXOrd && !curve.IsOverrideOrdinal)
                    {
                        tXMinVal = 1.0;
                        tXMaxVal = curve.NPts;
                    }
                    if (curve.IsBar)
                    {
                        if (pane.m_barSettings.Base == BarBase.X)
                        {
                            if (tYMinVal > 0)
                                tYMinVal = 0;
                            else if (tYMaxVal < 0)
                                tYMaxVal = 0;
                            if (!isXOrd)
                            {
                                tXMinVal -= pane.m_barSettings.m_clusterScaleWidth / 2.0;
                                tXMaxVal += pane.m_barSettings.m_clusterScaleWidth / 2.0;
                            }
                        }
                        else
                        {
                            if (tXMinVal > 0)
                                tXMinVal = 0;
                            else if (tXMaxVal < 0)
                                tXMaxVal = 0;
                            if (!isYOrd)
                            {
                                tYMinVal -= pane.m_barSettings.m_clusterScaleWidth / 2.0;
                                tYMaxVal += pane.m_barSettings.m_clusterScaleWidth / 2.0;
                            }
                        }
                    }
                    if (curve.NPts > maxPts)
                        maxPts = curve.NPts;
                    if (tYMinVal < yScale.m_rangeMin)
                        yScale.m_rangeMin = tYMinVal;
                    if (tYMaxVal > yScale.m_rangeMax)
                        yScale.m_rangeMax = tYMaxVal;
                    if (tXMinVal < xScale.m_rangeMin)
                        xScale.m_rangeMin = tXMinVal;
                    if (tXMaxVal > xScale.m_rangeMax)
                        xScale.m_rangeMax = tXMaxVal;
                }
            }
            pane.XAxis.Scale.SetRange(pane, pane.XAxis);
            foreach (YAxis axis in pane.YAxisList)
                axis.Scale.SetRange(pane, axis);
            foreach (Y2Axis axis in pane.Y2AxisList)
                axis.Scale.SetRange(pane, axis);
        }

        /// <summary>
        /// 获取栈区域
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="curve">图层</param>
        /// <param name="tXMinVal">X最小值</param>
        /// <param name="tYMinVal">Y最小值</param>
        /// <param name="tXMaxVal">X最大值</param>
        /// <param name="tYMaxVal">Y最大值</param>
        private void GetStackRange(GraphPane pane, CurveItem curve, out double tXMinVal,
                                    out double tYMinVal, out double tXMaxVal, out double tYMaxVal)
        {
            tXMinVal = tYMinVal = Double.MaxValue;
            tXMaxVal = tYMaxVal = Double.MinValue;
            ValueHandler valueHandler = new ValueHandler(pane, false);
            Axis baseAxis = curve.BaseAxis(pane);
            bool isXBase = baseAxis is XAxis;
            double lowVal, baseVal, hiVal;
            for (int i = 0; i < curve.Points.Count; i++)
            {
                valueHandler.GetValues(curve, i, out baseVal, out lowVal, out hiVal);
                double x = isXBase ? baseVal : hiVal;
                double y = isXBase ? hiVal : baseVal;
                if (x != PointPair.Missing && y != PointPair.Missing && lowVal != PointPair.Missing)
                {
                    if (x < tXMinVal)
                        tXMinVal = x;
                    if (x > tXMaxVal)
                        tXMaxVal = x;
                    if (y < tYMinVal)
                        tYMinVal = y;
                    if (y > tYMaxVal)
                        tYMaxVal = y;
                    if (!isXBase)
                    {
                        if (lowVal < tXMinVal)
                            tXMinVal = lowVal;
                        if (lowVal > tXMaxVal)
                            tXMaxVal = lowVal;
                    }
                    else
                    {
                        if (lowVal < tYMinVal)
                            tYMinVal = lowVal;
                        if (lowVal > tYMaxVal)
                            tYMaxVal = lowVal;
                    }
                }
            }
        }

        /// <summary>
        /// 判断是否有数据
        /// </summary>
        /// <returns>是否有数据</returns>
        public bool HasData()
        {
            foreach (CurveItem curve in this)
            {
                if (curve.Points.Count > 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 获取标签的索引
        /// </summary>
        /// <param name="label">标签</param>
        /// <returns>索引</returns>
        public int IndexOf(String label)
        {
            int index = 0;
            foreach (CurveItem p in this)
            {
                if (String.Compare(p.m_label.m_text, label, true) == 0)
                    return index;
                index++;
            }
            return -1;
        }

        /// <summary>
        /// 初始化刻度
        /// </summary>
        /// <param name="scale">刻度</param>
        /// <param name="isBoundedRanges">是否矩形区域</param>
        private void InitScale(Scale scale, bool isBoundedRanges)
        {
            scale.m_rangeMin = double.MaxValue;
            scale.m_rangeMax = double.MinValue;
            scale.m_lBound = (isBoundedRanges && !scale.m_minAuto) ?
                scale.m_min : double.MinValue;
            scale.m_uBound = (isBoundedRanges && !scale.m_maxAuto) ?
                scale.m_max : double.MaxValue;
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="relativePos">相关位置</param>
        /// <returns>新索引</returns>
        public int Move(int index, int relativePos)
        {
            if (index < 0 || index >= Count)
                return -1;
            CurveItem curve = this[index];
            this.RemoveAt(index);
            index += relativePos;
            if (index < 0)
                index = 0;
            if (index > Count)
                index = Count;
            Insert(index, curve);
            return index;
        }

        /// <summary>
        /// 排序方法
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="index">索引</param>
        public void Sort(SortType2 type, int index)
        {
            this.Sort(new CurveItem.Comparer(type, index));
        }
        #endregion
    }
}
