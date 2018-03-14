/**********************************************************************************\
*                                                                                  *
* LinearAsOrdinalScale.cs - LinearAsOrdinalScale functions, types, and definitions *
*                                                                                  *
*               Version 1.00                                                       *
*                                                                                  *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.        *
*                                                                                  *
***********************************************************************************/

using System;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security.Permissions;
namespace OwLib
{
    /// <summary>
    /// 线性序数刻度
    /// </summary>
    [Serializable]
    class LinearAsOrdinalScale : Scale
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建线性序数刻度
        /// </summary>
        /// <param name="owner">所属对象</param>
        public LinearAsOrdinalScale(Axis owner)
            : base(owner)
        {
        }

        /// <summary>
        /// 创建线性序数刻度
        /// </summary>
        /// <param name="rhs">其他对象</param>
        /// <param name="owner">所属对象</param>
        public LinearAsOrdinalScale(Scale rhs, Axis owner)
            : base(rhs, owner)
        {
        }

        /// <summary>
        /// 获取坐标轴类型
        /// </summary>
        public override AxisType Type
        {
            get { return AxisType.LinearAsOrdinal; }
        }

        /// <summary>
        /// 克隆对象
        /// </summary>
        /// <param name="owner">所属对象</param>
        /// <returns>刻度</returns>
        public override Scale Clone(Axis owner)
        {
            return new LinearAsOrdinalScale(this, owner);
        }

        /// <summary>
        /// 创建标签
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
                val = pane.CurveList[0].Points[tmpIndex].X;
                double scaleMult = Math.Pow((double)10.0, m_mag);
                return (val / scaleMult).ToString(m_format);
            }
            else
                return String.Empty;
        }

        /// <summary>
        /// 挑选刻度
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="g">绘图对象</param>
        /// <param name="scaleFactor">刻度因子</param>
        public override void PickScale(GraphPane pane, Graphics g, float scaleFactor)
        {
            base.PickScale(pane, g, scaleFactor);
            double xMin;
            double xMax;
            double yMin;
            double yMax; 
            double tMin = 0;
            double tMax = 1;
            foreach (CurveItem curve in pane.CurveList)
            {
                if ((m_ownerAxis is Y2Axis && curve.IsY2Axis) ||
                        (m_ownerAxis is YAxis && !curve.IsY2Axis) ||
                        (m_ownerAxis is XAxis))
                {
                    curve.GetRange(out xMin, out xMax, out yMin, out yMax, false, false, pane);
                    if (m_ownerAxis is XAxis)
                    {
                        tMin = xMin;
                        tMax = xMax;
                    }
                    else
                    {
                        tMin = yMin;
                        tMax = yMax;
                    }
                }
            }
            double range = Math.Abs(tMax - tMin);
            base.PickScale(pane, g, scaleFactor);
            OrdinalScale.PickScale(pane, g, scaleFactor, this);
            SetScaleMag(tMin, tMax, range / Default.TargetXSteps);
        }
        #endregion
    }
}
