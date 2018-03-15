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
                }
            }
        }
    }
}
