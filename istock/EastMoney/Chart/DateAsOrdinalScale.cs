/********************************************************************************\
*                                                                                *
* DateAsOrdinalScale.cs -  DateAsOrdinalScale functions, types, and definitions  *
*                                                                                *
*               Version 1.00                                                     *
*                                                                                *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.      *
*                                                                                *
*********************************************************************************/

using System;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OwLib
{
    /// <summary>
    /// 日期序数坐标轴
    /// </summary>
    [Serializable]
    class DateAsOrdinalScale : Scale
    {
        #region 陶德 2016/5/31
        /// <summary>
        /// 创建坐标轴
        /// </summary>
        /// <param name="owner">坐标轴</param>
        public DateAsOrdinalScale(Axis owner)
            : base(owner)
        {
        }

        /// <summary>
        /// 创建坐标轴
        /// </summary>
        /// <param name="rhs">其他刻度</param>
        /// <param name="owner">所属坐标轴</param>
        public DateAsOrdinalScale(Scale rhs, Axis owner)
            : base(rhs, owner)
        {
        }

        /// <summary>
        /// 获取或设置最大值
        /// </summary>
        public override double Max
        {
            get { return m_max; }
            set { m_max = XDate.MakeValidDate(value); m_maxAuto = false; }
        }

        /// <summary>
        /// 获取或设置最小值
        /// </summary>
        public override double Min
        {
            get { return m_min; }
            set { m_min = XDate.MakeValidDate(value); m_minAuto = false; }
        }

        /// <summary>
        /// 获取坐标轴类型
        /// </summary>
        public override AxisType Type
        {
            get { return AxisType.DateAsOrdinal; }
        }

        /// <summary>
        /// 创建拷贝
        /// </summary>
        /// <param name="owner">所属坐标轴</param>
        /// <returns>拷贝</returns>
        public override Scale Clone(Axis owner)
        {
            return new DateAsOrdinalScale(this, owner);
        }

        /// <summary>
        /// 生成标签
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="index">索引</param>
        /// <param name="dVal">数值</param>
        /// <returns>标签</returns>
        internal override String MakeLabel(GraphPane pane, int index, double dVal)
        {
            if (m_format == null)
                m_format = Scale.Default.Format;
            double val;
            int tmpIndex = (int)dVal - 1;
            if (pane.CurveList.Count > 0 && pane.CurveList[0].Points.Count > tmpIndex)
            {
                if (m_ownerAxis is XAxis)
                    val = pane.CurveList[0].Points[tmpIndex].X;
                else
                    val = pane.CurveList[0].Points[tmpIndex].Y;
                return XDate.ToString(val, m_format);
            }
            else
                return String.Empty;
        }

        /// <summary>
        /// 选择刻度
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="g">绘图对象</param>
        /// <param name="scaleFactor">刻度因子</param>
        public override void PickScale(GraphPane pane, Graphics g, float scaleFactor)
        {
            base.PickScale(pane, g, scaleFactor);
            SetDateFormat(pane);
            base.PickScale(pane, g, scaleFactor);
            OrdinalScale.PickScale(pane, g, scaleFactor, this);
        }

        /// <summary>
        /// 设置日期格式
        /// </summary>
        /// <param name="pane">图层</param>
        internal void SetDateFormat(GraphPane pane)
        {
            if (m_formatAuto)
            {
                double range = 10;
                if (pane.CurveList.Count > 0 && pane.CurveList[0].Points.Count > 1)
                {
                    double val1, val2;
                    PointPair pt1 = pane.CurveList[0].Points[0];
                    PointPair pt2 = pane.CurveList[0].Points[pane.CurveList[0].Points.Count - 1];
                    int p1 = 1;
                    int p2 = pane.CurveList[0].Points.Count;
                    if (pane.IsBoundedRanges)
                    {
                        p1 = (int)Math.Floor(m_ownerAxis.Scale.Min);
                        p2 = (int)Math.Ceiling(m_ownerAxis.Scale.Max);
                        p1 = Math.Min(Math.Max(p1, 1), pane.CurveList[0].Points.Count);
                        p2 = Math.Min(Math.Max(p2, 1), pane.CurveList[0].Points.Count);
                        if (p2 > p1)
                        {
                            pt1 = pane.CurveList[0].Points[p1 - 1];
                            pt2 = pane.CurveList[0].Points[p2 - 1];
                        }
                    }
                    if (m_ownerAxis is XAxis)
                    {
                        val1 = pt1.X;
                        val2 = pt2.X;
                    }
                    else
                    {
                        val1 = pt1.Y;
                        val2 = pt2.Y;
                    }
                    if (val1 != PointPair.Missing &&
                            val2 != PointPair.Missing &&
                            !Double.IsNaN(val1) &&
                            !Double.IsNaN(val2) &&
                            !Double.IsInfinity(val1) &&
                            !Double.IsInfinity(val2) &&
                            Math.Abs(val2 - val1) > 1e-10)
                        range = Math.Abs(val2 - val1);
                }
                if (range > Default.RangeYearYear)
                    m_format = Default.FormatYearYear;
                else if (range > Default.RangeYearMonth)
                    m_format = Default.FormatYearMonth;
                else if (range > Default.RangeMonthMonth)
                    m_format = Default.FormatMonthMonth;
                else if (range > Default.RangeDayDay)
                    m_format = Default.FormatDayDay;
                else if (range > Default.RangeDayHour)
                    m_format = Default.FormatDayHour;
                else if (range > Default.RangeHourHour)
                    m_format = Default.FormatHourHour;
                else if (range > Default.RangeHourMinute)
                    m_format = Default.FormatHourMinute;
                else if (range > Default.RangeMinuteMinute)
                    m_format = Default.FormatMinuteMinute;
                else if (range > Default.RangeMinuteSecond)
                    m_format = Default.FormatMinuteSecond;
                else if (range > Default.RangeSecondSecond)
                    m_format = Default.FormatSecondSecond;
                else 
                    m_format = Default.FormatMillisecond;
            }
        }
        #endregion
    }
}
