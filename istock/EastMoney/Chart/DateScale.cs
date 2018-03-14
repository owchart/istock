/***************************************************************************\
*                                                                           *
* DateScale.cs -  DateScale functions, types, and definitions               *
*                                                                           *
*               Version 1.00                                                *
*                                                                           *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved. *
*                                                                           *
****************************************************************************/

using System;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OwLib
{
    /// <summary>
    /// 日期刻度
    /// </summary>
    [Serializable]
    public class DateScale : Scale
    {
        #region 陶德 2016/5/31
        /// <summary>
        /// 创建刻度
        /// </summary>
        /// <param name="owner">所属坐标</param>
        public DateScale(Axis owner)
            : base(owner)
        {
        }

        /// <summary>
        /// 创建刻度
        /// </summary>
        /// <param name="rhs">其他刻度</param>
        /// <param name="owner">所属坐标</param>
        public DateScale(Scale rhs, Axis owner)
            : base(rhs, owner)
        {
        }

        /// <summary>
        /// 获取主单位乘积
        /// </summary>
        internal override double MajorUnitMultiplier
        {
            get { return GetUnitMultiple(m_majorUnit); }
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
        /// 获取次单位乘积
        /// </summary>
        internal override double MinorUnitMultiplier
        {
            get { return GetUnitMultiple(m_minorUnit); }
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        public override AxisType Type
        {
            get { return AxisType.Date; }
        }

        /// <summary>
        /// 创建拷贝
        /// </summary>
        /// <param name="owner">所属坐标轴</param>
        /// <returns>拷贝</returns>
        public override Scale Clone(Axis owner)
        {
            return new DateScale(this, owner);
        }

        /// <summary>
        /// 计算基础变动值
        /// </summary>
        /// <returns>变动值</returns>
        internal override double CalcBaseTic()
        {
            if (m_baseTic != PointPair.Missing)
                return m_baseTic;
            else
            {
                int year, month, day, hour, minute, second, millisecond;
                XDate.XLDateToCalendarDate(m_min, out year, out month, out day, out hour, out minute,
                                            out second, out millisecond);
                switch (m_majorUnit)
                {
                    case DateUnit.Year:
                    default:
                        month = 1; day = 1; hour = 0; minute = 0; second = 0; millisecond = 0;
                        break;
                    case DateUnit.Month:
                        day = 1; hour = 0; minute = 0; second = 0; millisecond = 0;
                        break;
                    case DateUnit.Day:
                        hour = 0; minute = 0; second = 0; millisecond = 0;
                        break;
                    case DateUnit.Hour:
                        minute = 0; second = 0; millisecond = 0;
                        break;
                    case DateUnit.Minute:
                        second = 0; millisecond = 0;
                        break;
                    case DateUnit.Second:
                        millisecond = 0;
                        break;
                    case DateUnit.Millisecond:
                        break;
                }
                double xlDate = XDate.CalendarDateToXLDate(year, month, day, hour, minute, second, millisecond);
                if (xlDate < m_min)
                {
                    switch (m_majorUnit)
                    {
                        case DateUnit.Year:
                        default:
                            year++;
                            break;
                        case DateUnit.Month:
                            month++;
                            break;
                        case DateUnit.Day:
                            day++;
                            break;
                        case DateUnit.Hour:
                            hour++;
                            break;
                        case DateUnit.Minute:
                            minute++;
                            break;
                        case DateUnit.Second:
                            second++;
                            break;
                        case DateUnit.Millisecond:
                            millisecond++;
                            break;
                    }
                    xlDate = XDate.CalendarDateToXLDate(year, month, day, hour, minute, second, millisecond);
                }
                return xlDate;
            }
        }

        /// <summary>
        /// 计算日期增长大小
        /// </summary>
        /// <param name="range">幅度</param>
        /// <param name="targetSteps">目标步长</param>
        /// <returns>大小</returns>
        protected double CalcDateStepSize(double range, double targetSteps)
        {
            return CalcDateStepSize(range, targetSteps, this);
        }

        /// <summary>
        /// 计算日期增长大小
        /// </summary>
        /// <param name="range">幅度</param>
        /// <param name="targetSteps">目标步长</param>
        /// <param name="scale">刻度</param>
        /// <returns>大小</returns>
        internal static double CalcDateStepSize(double range, double targetSteps, Scale scale)
        {
            double tempStep = range / targetSteps;
            if (range > Default.RangeYearYear)
            {
                scale.m_majorUnit = DateUnit.Year;
                if (scale.m_formatAuto)
                    scale.m_format = Default.FormatYearYear;
                tempStep = Math.Ceiling(tempStep / 365.0);
                if (scale.m_minorStepAuto)
                {
                    scale.m_minorUnit = DateUnit.Year;
                    if (tempStep == 1.0)
                        scale.m_minorStep = 0.25;
                    else
                        scale.m_minorStep = Scale.CalcStepSize(tempStep, targetSteps);
                }
            }
            else if (range > Default.RangeYearMonth)
            {
                scale.m_majorUnit = DateUnit.Year;
                if (scale.m_formatAuto)
                    scale.m_format = Default.FormatYearMonth;
                tempStep = Math.Ceiling(tempStep / 365.0);
                if (scale.m_minorStepAuto)
                {
                    scale.m_minorUnit = DateUnit.Month;
                    scale.m_minorStep = Math.Ceiling(range / (targetSteps * 3) / 30.0);
                    if (scale.m_minorStep > 6)
                        scale.m_minorStep = 12;
                    else if (scale.m_minorStep > 3)
                        scale.m_minorStep = 6;
                }
            }
            else if (range > Default.RangeMonthMonth)
            {
                scale.m_majorUnit = DateUnit.Month;
                if (scale.m_formatAuto)
                    scale.m_format = Default.FormatMonthMonth;
                tempStep = Math.Ceiling(tempStep / 30.0);
                if (scale.m_minorStepAuto)
                {
                    scale.m_minorUnit = DateUnit.Month;
                    scale.m_minorStep = tempStep * 0.25;
                }
            }
            else if (range > Default.RangeDayDay)
            {
                scale.m_majorUnit = DateUnit.Day;
                if (scale.m_formatAuto)
                    scale.m_format = Default.FormatDayDay;
                tempStep = Math.Ceiling(tempStep);
                if (scale.m_minorStepAuto)
                {
                    scale.m_minorUnit = DateUnit.Day;
                    scale.m_minorStep = tempStep * 0.25;
                }
            }
            else if (range > Default.RangeDayHour)
            {
                scale.m_majorUnit = DateUnit.Day;
                if (scale.m_formatAuto)
                    scale.m_format = Default.FormatDayHour;
                tempStep = Math.Ceiling(tempStep);
                if (scale.m_minorStepAuto)
                {
                    scale.m_minorUnit = DateUnit.Hour;
                    scale.m_minorStep = Math.Ceiling(range / (targetSteps * 3) * XDate.HoursPerDay);
                    if (scale.m_minorStep > 6)
                        scale.m_minorStep = 12;
                    else if (scale.m_minorStep > 3)
                        scale.m_minorStep = 6;
                    else
                        scale.m_minorStep = 1;
                }
            }
            else if (range > Default.RangeHourHour)
            {
                scale.m_majorUnit = DateUnit.Hour;
                tempStep = Math.Ceiling(tempStep * XDate.HoursPerDay);
                if (scale.m_formatAuto)
                    scale.m_format = Default.FormatHourHour;
                if (tempStep > 12.0)
                    tempStep = 24.0;
                else if (tempStep > 6.0)
                    tempStep = 12.0;
                else if (tempStep > 2.0)
                    tempStep = 6.0;
                else if (tempStep > 1.0)
                    tempStep = 2.0;
                else
                    tempStep = 1.0;
                if (scale.m_minorStepAuto)
                {
                    scale.m_minorUnit = DateUnit.Hour;
                    if (tempStep <= 1.0)
                        scale.m_minorStep = 0.25;
                    else if (tempStep <= 6.0)
                        scale.m_minorStep = 1.0;
                    else if (tempStep <= 12.0)
                        scale.m_minorStep = 2.0;
                    else
                        scale.m_minorStep = 4.0;
                }
            }
            else if (range > Default.RangeHourMinute)
            {
                scale.m_majorUnit = DateUnit.Hour;
                tempStep = Math.Ceiling(tempStep * XDate.HoursPerDay);
                if (scale.m_formatAuto)
                    scale.m_format = Default.FormatHourMinute;
                if (scale.m_minorStepAuto)
                {
                    scale.m_minorUnit = DateUnit.Minute;
                    scale.m_minorStep = Math.Ceiling(range / (targetSteps * 3) * XDate.MinutesPerDay);
                    if (scale.m_minorStep > 15.0)
                        scale.m_minorStep = 30.0;
                    else if (scale.m_minorStep > 5.0)
                        scale.m_minorStep = 15.0;
                    else if (scale.m_minorStep > 1.0)
                        scale.m_minorStep = 5.0;
                    else
                        scale.m_minorStep = 1.0;
                }
            }
            else if (range > Default.RangeMinuteMinute)
            {
                scale.m_majorUnit = DateUnit.Minute;
                if (scale.m_formatAuto)
                    scale.m_format = Default.FormatMinuteMinute;
                tempStep = Math.Ceiling(tempStep * XDate.MinutesPerDay);
                if (tempStep > 15.0)
                    tempStep = 30.0;
                else if (tempStep > 5.0)
                    tempStep = 15.0;
                else if (tempStep > 1.0)
                    tempStep = 5.0;
                else
                    tempStep = 1.0;
                if (scale.m_minorStepAuto)
                {
                    scale.m_minorUnit = DateUnit.Minute;
                    if (tempStep <= 1.0)
                        scale.m_minorStep = 0.25;
                    else if (tempStep <= 5.0)
                        scale.m_minorStep = 1.0;
                    else
                        scale.m_minorStep = 5.0;
                }
            }
            else if (range > Default.RangeMinuteSecond)
            {
                scale.m_majorUnit = DateUnit.Minute;
                tempStep = Math.Ceiling(tempStep * XDate.MinutesPerDay);
                if (scale.m_formatAuto)
                    scale.m_format = Default.FormatMinuteSecond;
                if (scale.m_minorStepAuto)
                {
                    scale.m_minorUnit = DateUnit.Second;
                    scale.m_minorStep = Math.Ceiling(range / (targetSteps * 3) * XDate.SecondsPerDay);
                    if (scale.m_minorStep > 15.0)
                        scale.m_minorStep = 30.0;
                    else if (scale.m_minorStep > 5.0)
                        scale.m_minorStep = 15.0;
                    else if (scale.m_minorStep > 1.0)
                        scale.m_minorStep = 5.0;
                    else
                        scale.m_minorStep = 1.0;
                }
            }
            else if (range > Default.RangeSecondSecond)
            {
                scale.m_majorUnit = DateUnit.Second;
                if (scale.m_formatAuto)
                    scale.m_format = Default.FormatSecondSecond;
                tempStep = Math.Ceiling(tempStep * XDate.SecondsPerDay);
                if (tempStep > 15.0)
                    tempStep = 30.0;
                else if (tempStep > 5.0)
                    tempStep = 15.0;
                else if (tempStep > 1.0)
                    tempStep = 5.0;
                else
                    tempStep = 1.0;
                if (scale.m_minorStepAuto)
                {
                    scale.m_minorUnit = DateUnit.Second;
                    if (tempStep <= 1.0)
                        scale.m_minorStep = 0.25;
                    else if (tempStep <= 5.0)
                        scale.m_minorStep = 1.0;
                    else
                        scale.m_minorStep = 5.0;
                }
            }
            else
            {
                scale.m_majorUnit = DateUnit.Millisecond;
                if (scale.m_formatAuto)
                    scale.m_format = Default.FormatMillisecond;
                tempStep = CalcStepSize(range * XDate.MillisecondsPerDay, Default.TargetXSteps);
                if (scale.m_minorStepAuto)
                {
                    scale.m_minorStep = CalcStepSize(tempStep,
                            (scale.m_ownerAxis is XAxis) ?
                            Default.TargetMinorXSteps : Default.TargetMinorYSteps);
                    scale.m_minorUnit = DateUnit.Millisecond;
                }
            }
            return tempStep;
        }

        /// <summary>
        /// 计算事件增长日期
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="direction">方向</param>
        /// <returns>日期</returns>
        protected double CalcEvenStepDate(double date, int direction)
        {
            int year, month, day, hour, minute, second, millisecond;
            XDate.XLDateToCalendarDate(date, out year, out month, out day,
                                        out hour, out minute, out second, out millisecond);
            if (direction < 0)
                direction = 0;
            switch (m_majorUnit)
            {
                case DateUnit.Year:
                default:
                    if (direction == 1 && month == 1 && day == 1 && hour == 0
                        && minute == 0 && second == 0)
                        return date;
                    else
                        return XDate.CalendarDateToXLDate(year + direction, 1, 1,
                                                        0, 0, 0);
                case DateUnit.Month:
                    if (direction == 1 && day == 1 && hour == 0
                        && minute == 0 && second == 0)
                        return date;
                    else
                        return XDate.CalendarDateToXLDate(year, month + direction, 1,
                                                0, 0, 0);
                case DateUnit.Day:
                    if (direction == 1 && hour == 0 && minute == 0 && second == 0)
                        return date;
                    else
                        return XDate.CalendarDateToXLDate(year, month,
                                            day + direction, 0, 0, 0);
                case DateUnit.Hour:
                    if (direction == 1 && minute == 0 && second == 0)
                        return date;
                    else
                        return XDate.CalendarDateToXLDate(year, month, day,
                                                    hour + direction, 0, 0);
                case DateUnit.Minute:
                    if (direction == 1 && second == 0)
                        return date;
                    else
                        return XDate.CalendarDateToXLDate(year, month, day, hour,
                                                    minute + direction, 0);
                case DateUnit.Second:
                    return XDate.CalendarDateToXLDate(year, month, day, hour,
                                                    minute, second + direction);
                case DateUnit.Millisecond:
                    return XDate.CalendarDateToXLDate(year, month, day, hour,
                                                    minute, second, millisecond + direction);
            }
        }

        /// <summary>
        /// 计算主记号值
        /// </summary>
        /// <param name="baseVal">基值</param>
        /// <param name="tic">记号</param>
        /// <returns>记号值</returns>
        internal override double CalcMajorTicValue(double baseVal, double tic)
        {
            XDate xDate = new XDate(baseVal);
            switch (m_majorUnit)
            {
                case DateUnit.Year:
                default:
                    xDate.AddYears(tic * m_majorStep);
                    break;
                case DateUnit.Month:
                    xDate.AddMonths(tic * m_majorStep);
                    break;
                case DateUnit.Day:
                    xDate.AddDays(tic * m_majorStep);
                    break;
                case DateUnit.Hour:
                    xDate.AddHours(tic * m_majorStep);
                    break;
                case DateUnit.Minute:
                    xDate.AddMinutes(tic * m_majorStep);
                    break;
                case DateUnit.Second:
                    xDate.AddSeconds(tic * m_majorStep);
                    break;
                case DateUnit.Millisecond:
                    xDate.AddMilliseconds(tic * m_majorStep);
                    break;
            }
            return xDate.XLDate;
        }

        /// <summary>
        /// 计算次记号值
        /// </summary>
        /// <param name="baseVal">基值</param>
        /// <param name="tic">记号</param>
        /// <returns>记号值</returns>
        internal override double CalcMinorTicValue(double baseVal, int iTic)
        {
            XDate xDate = new XDate(baseVal);
            switch (m_minorUnit)
            {
                case DateUnit.Year:
                default:
                    xDate.AddYears((double)iTic * m_minorStep);
                    break;
                case DateUnit.Month:
                    xDate.AddMonths((double)iTic * m_minorStep);
                    break;
                case DateUnit.Day:
                    xDate.AddDays((double)iTic * m_minorStep);
                    break;
                case DateUnit.Hour:
                    xDate.AddHours((double)iTic * m_minorStep);
                    break;
                case DateUnit.Minute:
                    xDate.AddMinutes((double)iTic * m_minorStep);
                    break;
                case DateUnit.Second:
                    xDate.AddSeconds((double)iTic * m_minorStep);
                    break;
            }
            return xDate.XLDate;
        }

        /// <summary>
        /// 计算最小开始
        /// </summary>
        /// <param name="baseVal">基础值</param>
        /// <returns>索引</returns>
        internal override int CalcMinorStart(double baseVal)
        {
            switch (m_minorUnit)
            {
                case DateUnit.Year:
                default:
                    return (int)((m_min - baseVal) / (365.0 * m_minorStep));
                case DateUnit.Month:
                    return (int)((m_min - baseVal) / (28.0 * m_minorStep));
                case DateUnit.Day:
                    return (int)((m_min - baseVal) / m_minorStep);
                case DateUnit.Hour:
                    return (int)((m_min - baseVal) * XDate.HoursPerDay / m_minorStep);
                case DateUnit.Minute:
                    return (int)((m_min - baseVal) * XDate.MinutesPerDay / m_minorStep);
                case DateUnit.Second:
                    return (int)((m_min - baseVal) * XDate.SecondsPerDay / m_minorStep);
            }
        }

        /// <summary>
        /// 计算变动值数量
        /// </summary>
        /// <returns>数量</returns>
        internal override int CalcNumTics()
        {
            int nTics = 1;
            int year1, year2, month1, month2, day1, day2, hour1, hour2, minute1, minute2;
            int second1, second2, millisecond1, millisecond2;
            XDate.XLDateToCalendarDate(m_min, out year1, out month1, out day1,
                                        out hour1, out minute1, out second1, out millisecond1);
            XDate.XLDateToCalendarDate(m_max, out year2, out month2, out day2,
                                        out hour2, out minute2, out second2, out millisecond2);
            switch (m_majorUnit)
            {
                case DateUnit.Year:
                default:
                    nTics = (int)((year2 - year1) / m_majorStep + 1.001);
                    break;
                case DateUnit.Month:
                    nTics = (int)((month2 - month1 + 12.0 * (year2 - year1)) / m_majorStep + 1.001);
                    break;
                case DateUnit.Day:
                    nTics = (int)((m_max - m_min) / m_majorStep + 1.001);
                    break;
                case DateUnit.Hour:
                    nTics = (int)((m_max - m_min) / (m_majorStep / XDate.HoursPerDay) + 1.001);
                    break;
                case DateUnit.Minute:
                    nTics = (int)((m_max - m_min) / (m_majorStep / XDate.MinutesPerDay) + 1.001);
                    break;
                case DateUnit.Second:
                    nTics = (int)((m_max - m_min) / (m_majorStep / XDate.SecondsPerDay) + 1.001);
                    break;
                case DateUnit.Millisecond:
                    nTics = (int)((m_max - m_min) / (m_majorStep / XDate.MillisecondsPerDay) + 1.001);
                    break;
            }
            if (nTics < 1)
                nTics = 1;
            else if (nTics > 1000)
                nTics = 1000;
            return nTics;
        }

        /// <summary>
        /// 获取单位乘积
        /// </summary>
        /// <param name="unit">单位</param>
        /// <returns>乘积</returns>
        private double GetUnitMultiple(DateUnit unit)
        {
            switch (unit)
            {
                case DateUnit.Year:
                default:
                    return 365.0;
                case DateUnit.Month:
                    return 30.0;
                case DateUnit.Day:
                    return 1.0;
                case DateUnit.Hour:
                    return 1.0 / XDate.HoursPerDay;
                case DateUnit.Minute:
                    return 1.0 / XDate.MinutesPerDay;
                case DateUnit.Second:
                    return 1.0 / XDate.SecondsPerDay;
                case DateUnit.Millisecond:
                    return 1.0 / XDate.MillisecondsPerDay;
            }
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
            return XDate.ToString(dVal, m_format);
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
            if (m_max - m_min < 1.0e-20)
            {
                if (m_maxAuto)
                    m_max = m_max + 0.2 * (m_max == 0 ? 1.0 : Math.Abs(m_max));
                if (m_minAuto)
                    m_min = m_min - 0.2 * (m_min == 0 ? 1.0 : Math.Abs(m_min));
            }
            double targetSteps = (m_ownerAxis is XAxis) ?
                        Default.TargetXSteps : Default.TargetYSteps;
            double tempStep = CalcDateStepSize(m_max - m_min, targetSteps);
            if (m_majorStepAuto)
            {
                m_majorStep = tempStep;
                if (m_isPreventLabelOverlap)
                {
                    double maxLabels = (double)this.CalcMaxLabels(g, pane, scaleFactor);
                    if (maxLabels < this.CalcNumTics())
                        m_majorStep = CalcDateStepSize(m_max - m_min, maxLabels);
                }
            }
            if (m_minAuto)
                m_min = CalcEvenStepDate(m_min, -1);
            if (m_maxAuto)
                m_max = CalcEvenStepDate(m_max, 1);
            m_mag = 0;
        }
        #endregion
    }
}
