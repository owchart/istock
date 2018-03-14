using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OwLib
{
    public class ChartAEx : ChartA
    {
        private CTable m_dataSourceHistory;

        public CTable DataSourceHistory
        {
            get { return m_dataSourceHistory; }
            set { m_dataSourceHistory = value; }
        }

        private double m_preClosePrice;

        public double PreClosePrice
        {
            get { return m_preClosePrice; }
            set { m_preClosePrice = value; }
        }

        private bool m_showMinuteLine;

        public bool ShowMinuteLine
        {
            get { return m_showMinuteLine; }
            set { m_showMinuteLine = value; }
        }

        private bool m_showRightVScale;

        public bool ShowRightVScale
        {
            get { return m_showRightVScale; }
            set { m_showRightVScale = value; }
        }

        private double m_minuteLineMaxValue;
        private double m_minuteLineMinValue;

        public void DefaultMinuteLine()
        {
            m_minuteLineMaxValue = m_preClosePrice * 1.01f;
            m_minuteLineMinValue = m_preClosePrice * 0.99f;
        }

        public float GetMinuteX(int index, int scaleStepsSize)
        {
            float x = 0;
            x = (float)(m_leftVScaleWidth + ((float)index / (float)scaleStepsSize) * m_workingAreaWidth);
            if (m_reverseHScale)
            {
                return m_workingAreaWidth - (x - m_leftVScaleWidth) + m_leftVScaleWidth + m_blankSpace;
            }
            else
            {
                return x;
            }
        }

        public override void Adjust()
        {
            if (m_workingAreaWidth > 0)
            {
                m_lastUnEmptyIndex = -1;
                if (m_firstVisibleIndex < 0 || m_lastVisibleIndex > m_dataSource.RowsCount - 1)
                {
                    return;
                }
                List<CDiv> divsCopy = GetDivs();
                foreach (CDiv cDiv in divsCopy)
                {
                    VScale leftVScale = cDiv.LeftVScale;
                    VScale rightVScale = cDiv.RightVScale;
                    cDiv.WorkingAreaHeight = cDiv.Height - cDiv.HScale.Height - cDiv.TitleBar.Height - 1;
                    List<BaseShape> shapesCopy = cDiv.GetShapes(SortType.NONE);
                    double leftMax = 0, leftMin = 0;
                    double rightMax = 0, rightMin = 0;
                    double leftMaxShowLine = leftVScale.VisibleMax, leftMinShowLine = leftVScale.VisibleMin;
                    double rightMaxShowLine = rightVScale.VisibleMax, rightMinShowLine = rightVScale.VisibleMin;
                    bool leftMaxInit = false, leftMinInit = false, rightMaxInit = false, rightMinInit = false;
                    if (m_dataSource.RowsCount > 0)
                    {
                        foreach (BaseShape bs in shapesCopy)
                        {
                            if (!bs.Visible)
                            {
                                continue;
                            }
                            BarShape bar = bs as BarShape;
                            int fieldsLength = 0;
                            int[] fields = bs.GetFields();
                            fieldsLength = fields.Length;
                            for (int f = 0; f < fieldsLength; f++)
                            {
                                int field = m_dataSource.GetColumnIndex(fields[f]);
                                for (int m = m_firstVisibleIndex; m <= m_lastVisibleIndex; m++)
                                {
                                    double fieldValue = m_dataSource.Get3(m, field);
                                    if (!double.IsNaN(fieldValue))
                                    {
                                        m_lastUnEmptyIndex = m;
                                        if (bs.AttachVScale == AttachVScale.Left)
                                        {
                                            if (fieldValue > leftMax || !leftMaxInit)
                                            {
                                                leftMaxInit = true;
                                                leftMax = fieldValue;
                                            }
                                            if (fieldValue < leftMin || !leftMinInit)
                                            {
                                                leftMinInit = true;
                                                leftMin = fieldValue;
                                            }
                                        }
                                        else
                                        {
                                            if (fieldValue > rightMax || !rightMaxInit)
                                            {
                                                rightMaxInit = true;
                                                rightMax = fieldValue;
                                            }
                                            if (fieldValue < rightMin || !rightMinInit)
                                            {
                                                rightMinInit = true;
                                                rightMin = fieldValue;
                                            }
                                        }
                                        if (m_showMinuteLine)
                                        {
                                            if (leftVScale.Type == VScaleType.EqualRatio)
                                            {
                                                if (fieldValue > leftMaxShowLine)
                                                {
                                                    leftMaxShowLine = fieldValue * 1.001;
                                                }
                                                if (fieldValue < leftMinShowLine)
                                                {
                                                    leftMinShowLine = fieldValue * 0.999;
                                                }
                                            }
                                            if (rightVScale.Type == VScaleType.EqualRatio)
                                            {
                                                if (fieldValue > rightMaxShowLine)
                                                {
                                                    rightMaxShowLine = fieldValue * 1.001;
                                                }
                                                if (fieldValue < rightMinShowLine)
                                                {
                                                    rightMinShowLine = fieldValue * 0.999;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (bar != null)
                            {
                                double midValue = 0;
                                if (bar.FieldName2 == CTable.NULLFIELD)
                                {
                                    if (bs.AttachVScale == AttachVScale.Left)
                                    {
                                        if (midValue > leftMax || !leftMaxInit)
                                        {
                                            leftMaxInit = true;
                                            leftMax = midValue;
                                        }
                                        if (midValue < leftMin || !leftMinInit)
                                        {
                                            leftMinInit = true;
                                            leftMin = midValue;
                                        }
                                    }
                                    else
                                    {
                                        if (midValue > rightMax || !rightMaxInit)
                                        {
                                            rightMaxInit = true;
                                            rightMax = midValue;
                                        }
                                        if (midValue < rightMin || !rightMinInit)
                                        {
                                            rightMinInit = true;
                                            rightMin = midValue;
                                        }
                                    }
                                }
                            }
                        }
                        if (leftMax == leftMin)
                        {
                            leftMax = leftMax * 1.01;
                            leftMin = leftMin * 0.99;
                        }
                        if (rightMax == rightMin)
                        {
                            rightMax = rightMax * 1.01;
                            rightMin = rightMin * 0.99;
                        }
                    }
                    //修正
                    if (leftVScale.AutoMaxMin)
                    {
                        //设置主轴的最大最小值
                        leftVScale.VisibleMax = leftMax;
                        leftVScale.VisibleMin = leftMin;
                    }
                    if (rightVScale.AutoMaxMin)
                    {
                        //设置副轴的最大最小值
                        rightVScale.VisibleMax = rightMax;
                        rightVScale.VisibleMin = rightMin;
                    }
                    //修正
                    if (leftVScale.AutoMaxMin && leftVScale.VisibleMax == 0 && leftVScale.VisibleMin == 0)
                    {
                        leftVScale.VisibleMax = rightVScale.VisibleMax;
                        leftVScale.VisibleMin = rightVScale.VisibleMin;
                    }
                    if (rightVScale.AutoMaxMin && rightVScale.VisibleMax == 0 && rightVScale.VisibleMin == 0)
                    {
                        rightVScale.VisibleMax = leftVScale.VisibleMax;
                        rightVScale.VisibleMin = leftVScale.VisibleMin;
                    }
                    if (m_showMinuteLine)
                    {
                        if (leftVScale.Type == VScaleType.EqualRatio)
                        {
                            double sep1 = leftMaxShowLine - m_preClosePrice;
                            double sep2 = m_preClosePrice - leftMinShowLine;
                            if (sep1 >= sep2)
                            {
                                leftMinShowLine = m_preClosePrice - sep1;
                            }
                            else
                            {
                                leftMaxShowLine = m_preClosePrice + sep2;
                            }
                            leftVScale.VisibleMax = leftMaxShowLine;
                            leftVScale.VisibleMin = leftMinShowLine;
                            m_minuteLineMaxValue = leftMaxShowLine;
                            m_minuteLineMinValue = leftMinShowLine;
                        }
                        if (rightVScale.Type == VScaleType.EqualRatio)
                        {
                            double sep1 = rightMaxShowLine - m_preClosePrice;
                            double sep2 = m_preClosePrice - rightMinShowLine;
                            if (sep1 >= sep2)
                            {
                                rightMinShowLine = m_preClosePrice - sep1;
                            }
                            else
                            {
                                rightMaxShowLine = m_preClosePrice + sep2;
                            }
                            rightVScale.VisibleMax = rightMaxShowLine;
                            rightVScale.VisibleMin = rightMinShowLine;
                            m_minuteLineMaxValue = rightMaxShowLine;
                            m_minuteLineMinValue = rightMinShowLine;
                        }
                    }
                }
            }
        }

        public float GetYMinuteLine(CDiv div, double value, AttachVScale attach)
        {
            if (div != null)
            {
                int separater = 12;
                VScale scale = div.GetVScale(attach);
                int wHeight = div.WorkingAreaHeight - scale.PaddingTop - scale.PaddingBottom - separater;
                TitleBar titleBar = div.TitleBar;
                if (wHeight > 0)
                {
                    float y = (float)((m_minuteLineMaxValue - value) / (m_minuteLineMaxValue - m_minuteLineMinValue) * wHeight);
                    return titleBar.Height + scale.PaddingTop + y + separater / 2;
                }
            }
            return 0;
        }

        public double GetVScaleValue(int y, double max, double min, float vHeight)
        {
            double every = (max - min) / vHeight;
            return max - y * every;
        }

        public override double GetNumberValue(CDiv div, POINT mp, AttachVScale attachVScale)
        {
            VScale vScale = div.GetVScale(attachVScale);
            VScale leftVScale = div.LeftVScale;
            VScale rightVScale = div.RightVScale;
            int vHeight = div.WorkingAreaHeight - vScale.PaddingTop - vScale.PaddingBottom;
            int heigth = div.TitleBar.Height;
            int paddingTop = vScale.PaddingTop;
            //int cY = mp.y - div->GetTitleBar()->GetHeight() - vScale->GetPaddingTop();
            int cY = mp.y - heigth - paddingTop;
            if (vScale.Reverse)
            {
                cY = vHeight - cY;
            }
            if (vHeight > 0)
            {
                double max = 0;
                double min = 0;
                bool isLog = false;
                if (attachVScale == AttachVScale.Left)
                {
                    max = leftVScale.VisibleMax;
                    min = leftVScale.VisibleMin;
                    if (max == 0 && min == 0)
                    {
                        max = rightVScale.VisibleMax;
                        min = rightVScale.VisibleMin;
                    }
                    isLog = leftVScale.System == VScaleSystem.Logarithmic;
                }
                else if (attachVScale == AttachVScale.Right)
                {
                    max = rightVScale.VisibleMax;
                    min = rightVScale.VisibleMin;
                    if (max == 0 && min == 0)
                    {
                        max = leftVScale.VisibleMax;
                        min = leftVScale.VisibleMin;
                    }
                    isLog = rightVScale.System == VScaleSystem.Logarithmic;
                }
                if (isLog)
                {
                    if (max >= 0)
                    {
                        max = Math.Log10(max);
                    }
                    else
                    {
                        max = -Math.Log10(Math.Abs(max));
                    }
                    if (min >= 0)
                    {
                        min = Math.Log10(min);
                    }
                    else
                    {
                        min = -Math.Log10(Math.Abs(min));
                    }
                    double value = GetVScaleValue(cY, max, min, (float)vHeight);
                    return Math.Pow(10, value);
                }
                else
                {
                    return GetVScaleValue(cY, max, min, (float)vHeight);
                }
            }
            return 0;

        }

        double GetNumberValueMinuteLine(CDiv div, POINT mp, AttachVScale attachVScale)
        {
            int separater = 12;
            VScale vScale = div.GetVScale(attachVScale);
            VScale leftVScale = div.LeftVScale;
            VScale rightVScale = div.RightVScale;
            int vHeight = div.WorkingAreaHeight - vScale.PaddingTop - vScale.PaddingBottom - separater;
            int cY = mp.y - div.Top - div.TitleBar.Height - vScale.PaddingTop - separater / 2;
            if (vScale.Reverse)
            {
                cY = vHeight - cY;
            }
            if (vHeight > 0)
            {
                double max = m_minuteLineMaxValue;
                double min = m_minuteLineMinValue;
                bool isLog = false;
                if (isLog)
                {
                    if (max >= 0)
                    {
                        max = Math.Log10(max);
                    }
                    else
                    {
                        max = -Math.Log10(Math.Abs(max));
                    }
                    if (min >= 0)
                    {
                        min = Math.Log10(min);
                    }
                    else
                    {
                        min = -Math.Log10(Math.Abs(min));
                    }
                    double value = GetVScaleValue(cY, max, min, (float)vHeight);
                    return Math.Pow(10, value);
                }
                else
                {
                    return GetVScaleValue(cY, max, min, (float)vHeight);
                }
            }
            return 0;
        }

        public void DrawTipLine(CPaint paint, POINT mousePoint, int rediusArc, int rediusArc2, int angle, long lineColor)
        {
            int leftPointX1 = mousePoint.x + (int)(Math.Cos(angle * 3.14 / 180) * rediusArc);
            int leftPointY1 = mousePoint.y - (int)(Math.Sin(angle * 3.14 / 180) * rediusArc);
            int leftPointX2 = mousePoint.x + (int)(Math.Cos(angle * 3.14 / 180) * rediusArc2);
            int leftPointY2 = mousePoint.y - (int)(Math.Sin(angle * 3.14 / 180) * rediusArc2);
            POINT point1 = new POINT(leftPointX1, leftPointY1);
            POINT point2 = new POINT(leftPointX2, leftPointY2);
            paint.DrawLine(lineColor, 2, 0, point1, point2);
        }

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("owmath.dll", SetLastError = false, CallingConvention = CallingConvention.Cdecl)]
        public static extern int M012(double min, double max, int yLen, int maxSpan, int minSpan, int defCount, ref double step, ref int digit);

        public List<double> GetVScaleMinuteLineStep(double max, double min, CDiv div, VScale vScale)
        {
            List<double> scaleStepList = new List<double>();
            if (vScale.Type == VScaleType.EqualDiff || vScale.Type == VScaleType.Percent)
            {
                double step = 0;
                int distance = div.VGrid.Distance;
                int digit = 0, gN = div.WorkingAreaHeight / distance;
                if (gN == 0)
                {
                    gN = 1;
                }
                int wHeight = div.WorkingAreaHeight - vScale.PaddingTop - vScale.PaddingBottom;
                M012(min, max, wHeight / 2, distance, distance / 2, gN, ref step, ref digit);
                if (step > 0)
                {
                    double start = 0;
                    if (min >= 0)
                    {
                        while (start + step < min)
                        {
                            start += step;
                        }
                    }
                    else
                    {
                        while (start - step > min)
                        {
                            start -= step;
                        }
                    }
                    while (start <= max)
                    {
                        scaleStepList.Add(start);
                        start += step;
                    }
                }
            }
            else if (vScale.Type == VScaleType.EqualRatio)
            {
                scaleStepList.Add(max);
                scaleStepList.Add(min + (max - min) * 0.66);
                scaleStepList.Add(min + (max - min) * 0.33);
                scaleStepList.Add(min);
                scaleStepList.Add(min - (max - min) * 0.33);
                scaleStepList.Add(min - (max - min) * 0.66);
                scaleStepList.Add(min - (max - min) * 1);
            }
            else if (vScale.Type == VScaleType.Divide)
            {
                scaleStepList.Add(min + (max - min) * 0.25);
                scaleStepList.Add(min + (max - min) * 0.5);
                scaleStepList.Add(min + (max - min) * 0.75);
            }
            else if (vScale.Type == VScaleType.GoldenRatio)
            {
                scaleStepList.Add(min);
                scaleStepList.Add(min + (max - min) * 0.191);
                scaleStepList.Add(min + (max - min) * 0.382);
                scaleStepList.Add(min + (max - min) * 0.5);
                scaleStepList.Add(min + (max - min) * 0.618);
                scaleStepList.Add(min + (max - min) * 0.809);
            }
            if ((max != min) && scaleStepList.Count == 0)
            {
                if (!double.IsNaN(min))
                {
                    scaleStepList.Add(min);
                }
            }
            return scaleStepList;
        }
    }
}
