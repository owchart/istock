/********************************************************************************\
*                                                                                *
* ValueHandler.cs -    ValueHandler functions, types, and definitions            *
*                                                                                *
*               Version 1.00                                                     *
*                                                                                *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.      *
*                                                                                *
*********************************************************************************/

using System;
using System.Text;
using System.Drawing;

namespace OwLib
{
    /// <summary>
    /// 数值处理
    /// </summary>
    public class ValueHandler
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建数值处理
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="initialize">是否初始化</param>
        public ValueHandler(GraphPane pane, bool initialize)
        {
            m_pane = pane;
            if (initialize)
            {
                using (Image image = pane.GetImage()) { }
            }
        }

        private GraphPane m_pane;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="curve">图形</param>
        /// <param name="barWidth">柱状图宽度</param>
        /// <param name="iCluster">串数</param>
        /// <param name="val">数值</param>
        /// <param name="iOrdinal">序数</param>
        /// <returns></returns>
        public double BarCenterValue(CurveItem curve, float barWidth, int iCluster,
                                  double val, int iOrdinal)
        {
            Axis baseAxis = curve.BaseAxis(m_pane);
            float clusterWidth = m_pane.m_barSettings.GetClusterWidth();
            float clusterGap = m_pane.m_barSettings.MinClusterGap * barWidth;
            float barGap = barWidth * m_pane.m_barSettings.MinBarGap;
            if (curve.IsBar && m_pane.m_barSettings.Type != BarType.Cluster)
                iOrdinal = 0;
            float centerPix = baseAxis.Scale.Transform(curve.IsOverrideOrdinal, iCluster, val)
                - clusterWidth / 2.0F + clusterGap / 2.0F +
                iOrdinal * (barWidth + barGap) + 0.5F * barWidth;
            return baseAxis.Scale.ReverseTransform(centerPix);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="curve">图形</param>
        /// <param name="iPt">索引</param>
        /// <param name="baseVal">基础值</param>
        /// <param name="lowVal">最小值</param>
        /// <param name="hiVal">最大值</param>
        /// <returns></returns>
        public bool GetValues(CurveItem curve, int iPt, out double baseVal,
                            out double lowVal, out double hiVal)
        {
            return GetValues(m_pane, curve, iPt, out baseVal,
                                    out lowVal, out hiVal);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="curve">图形</param>
        /// <param name="iPt">索引</param>
        /// <param name="baseVal">基础值</param>
        /// <param name="lowVal">最小值</param>
        /// <param name="hiVal">最大值</param>
        /// <returns></returns>
        public static bool GetValues(GraphPane pane, CurveItem curve, int iPt,
                            out double baseVal, out double lowVal, out double hiVal)
        {
            hiVal = PointPair.Missing;
            lowVal = PointPair.Missing;
            baseVal = PointPair.Missing;
            if (curve == null || curve.Points.Count <= iPt || !curve.IsVisible)
                return false;
            Axis baseAxis = curve.BaseAxis(pane);
            Axis valueAxis = curve.ValueAxis(pane);
            if (baseAxis is XAxis)
                baseVal = curve.Points[iPt].X;
            else
                baseVal = curve.Points[iPt].Y;
            if (curve is BarItem && (pane.m_barSettings.Type == BarType.Stack ||
                        pane.m_barSettings.Type == BarType.PercentStack))
            {
                double positiveStack = 0;
                double negativeStack = 0;
                double curVal;
                foreach (CurveItem tmpCurve in pane.CurveList)
                {
                    if (tmpCurve.IsBar && tmpCurve.IsVisible)
                    {
                        curVal = PointPair.Missing;
                        if (curve.IsOverrideOrdinal || !baseAxis.m_scale.IsAnyOrdinal)
                        {
                            IPointList points = tmpCurve.Points;
                            for (int i = 0; i < points.Count; i++)
                            {
                                if ((baseAxis is XAxis) && points[i].X == baseVal)
                                {
                                    curVal = points[i].Y;
                                    break;
                                }
                                else if (!(baseAxis is XAxis) && points[i].Y == baseVal)
                                {
                                    curVal = points[i].X;
                                    break;
                                }
                            }
                        }
                        else if (iPt < tmpCurve.Points.Count)
                        {
                            if (baseAxis is XAxis)
                                curVal = tmpCurve.Points[iPt].Y;
                            else
                                curVal = tmpCurve.Points[iPt].X;
                        }
                        if (curVal == PointPair.Missing)
                        {
                            curVal = 0.000000001;
                        }
                        if (tmpCurve == curve)
                        {
                            if (curVal >= 0)
                            {
                                lowVal = positiveStack;
                                hiVal = (curVal == PointPair.Missing || positiveStack == PointPair.Missing) ?
                                        PointPair.Missing : positiveStack + curVal;
                            }
                            else
                            {
                                hiVal = negativeStack;
                                lowVal = (curVal == PointPair.Missing || negativeStack == PointPair.Missing) ?
                                        PointPair.Missing : negativeStack + curVal;
                            }
                        }
                        if (curVal >= 0)
                            positiveStack = (curVal == PointPair.Missing || positiveStack == PointPair.Missing) ?
                                        PointPair.Missing : positiveStack + curVal;
                        else
                            negativeStack = (curVal == PointPair.Missing || negativeStack == PointPair.Missing) ?
                                        PointPair.Missing : negativeStack + curVal;
                    }
                }
                if (pane.m_barSettings.Type == BarType.PercentStack &&
                            hiVal != PointPair.Missing && lowVal != PointPair.Missing)
                {
                    positiveStack += Math.Abs(negativeStack);
                    if (positiveStack != 0)
                    {
                        lowVal = lowVal / positiveStack * 100.0;
                        hiVal = hiVal / positiveStack * 100.0;
                    }
                    else
                    {
                        lowVal = 0;
                        hiVal = 0;
                    }
                }
                if (baseVal == PointPair.Missing || lowVal == PointPair.Missing ||
                        hiVal == PointPair.Missing)
                    return false;
                else
                    return true;
            }
            else if (curve is LineItem && pane.LineType == LineType.Stack)
            {
                double stack = 0;
                double curVal;
                foreach (CurveItem tmpCurve in pane.CurveList)
                {
                    if (tmpCurve is LineItem && tmpCurve.IsVisible)
                    {
                        curVal = PointPair.Missing;
                        if (curve.IsOverrideOrdinal || !baseAxis.m_scale.IsAnyOrdinal)
                        {
                            IPointList points = tmpCurve.Points;
                            for (int i = 0; i < points.Count; i++)
                            {
                                if (points[i].X == baseVal)
                                {
                                    curVal = points[i].Y;
                                    break;
                                }
                            }
                        }
                        else if (iPt < tmpCurve.Points.Count)
                        {
                            curVal = tmpCurve.Points[iPt].Y;
                        }
                        if (curVal == PointPair.Missing)
                            stack = PointPair.Missing;
                        if (tmpCurve == curve)
                        {
                            lowVal = stack;
                            hiVal = (curVal == PointPair.Missing || stack == PointPair.Missing) ?
                                PointPair.Missing : stack + curVal;
                        }
                        stack = (curVal == PointPair.Missing || stack == PointPair.Missing) ?
                                PointPair.Missing : stack + curVal;
                    }
                }
                if (baseVal == PointPair.Missing || lowVal == PointPair.Missing ||
                    hiVal == PointPair.Missing)
                    return false;
                else
                    return true;
            }
            else
            {
                lowVal = 0;
                if (baseAxis is XAxis)
                    hiVal = curve.Points[iPt].Y;
                else
                    hiVal = curve.Points[iPt].X;
            }
            if (curve is BarItem && valueAxis.m_scale.IsLog && lowVal == 0)
                lowVal = valueAxis.m_scale.m_min;
            if (baseVal == PointPair.Missing || hiVal == PointPair.Missing ||
                    (lowVal == PointPair.Missing))
                return false;
            else
                return true;
        }
        #endregion
    }
}
