/***************************************************************************\
*                                                                           *
* ExponentScale.cs -  ExponentScale functions, types, and definitions       *
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
    /// 扩展刻度
    /// </summary>
    [Serializable]
    class ExponentScale : Scale
    {
        #region 陶德 2016/6/3
        /// <summary>
        /// 创建扩展刻度
        /// </summary>
        /// <param name="owner">坐标轴</param>
        public ExponentScale(Axis owner)
            : base(owner)
        {
        }

        /// <summary>
        /// 创建扩展刻度
        /// </summary>
        /// <param name="rhs">其他刻度</param>
        /// <param name="owner">坐标轴</param>
        public ExponentScale(Scale rhs, Axis owner)
            : base(rhs, owner)
        {
        }

        /// <summary>
        /// 获取坐标轴类型
        /// </summary>
        public override AxisType Type
        {
            get { return AxisType.Exponent; }
        }

        /// <summary>
        /// 获取拷贝
        /// </summary>
        /// <param name="owner">坐标轴</param>
        /// <returns>拷贝</returns>
        public override Scale Clone(Axis owner)
        {
            return new ExponentScale(this, owner);
        }

        /// <summary>
        /// 计算主记号值
        /// </summary>
        /// <param name="baseVal">基值</param>
        /// <param name="tic">记号</param>
        /// <returns>主记号值</returns>
        internal override double CalcMajorTicValue(double baseVal, double tic)
        {
            if (m_exponent > 0.0)
            {
                return Math.Pow(Math.Pow(baseVal, 1 / m_exponent) + m_majorStep * tic, m_exponent);
            }
            else if (m_exponent < 0.0)
            {
                return Math.Pow(Math.Pow(baseVal, 1 / m_exponent) + m_majorStep * tic, m_exponent);
            }
            return 1.0;
        }

        /// <summary>
        /// 计算次记号值
        /// </summary>
        /// <param name="baseVal">基值</param>
        /// <param name="iTic">记号</param>
        /// <returns>次记号值</returns>
        internal override double CalcMinorTicValue(double baseVal, int iTic)
        {
            return baseVal + Math.Pow((double)m_majorStep * (double)iTic, m_exponent);
        }

        /// <summary>
        /// 计算最小开始
        /// </summary>
        /// <param name="baseVal">基础值</param>
        /// <returns>索引</returns>
        internal override int CalcMinorStart(double baseVal)
        {
            return (int)((Math.Pow(m_min, m_exponent) - baseVal) / Math.Pow(m_minorStep, m_exponent));
        }

        /// <summary>
        /// 反线性化
        /// </summary>
        /// <param name="val">值</param>
        /// <returns>反线性化值</returns>
        public override double DeLinearize(double val)
        {
            return Math.Pow(val, 1 / m_exponent);
        }

        /// <summary>
        /// 线性化
        /// </summary>
        /// <param name="val">值</param>
        /// <returns>线性化值</returns>
        public override double Linearize(double val)
        {
            return SafeExp(val, m_exponent);
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
            double scaleMult = Math.Pow((double)10.0, m_mag);
            double val = Math.Pow(dVal, 1 / m_exponent) / scaleMult;
            return val.ToString(m_format);
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
            if (m_minAuto && m_min > 0 &&
                m_min / (m_max - m_min) < Default.ZeroLever)
                m_min = 0;
            if (m_maxAuto && m_max < 0 &&
                Math.Abs(m_max / (m_max - m_min)) <
                Default.ZeroLever)
                m_max = 0;
            if (m_majorStepAuto)
            {
                double targetSteps = (m_ownerAxis is XAxis) ?
                        Default.TargetXSteps : Default.TargetYSteps;
                m_majorStep = CalcStepSize(m_max - m_min, targetSteps);
                if (m_isPreventLabelOverlap)
                {
                    double maxLabels = (double)this.CalcMaxLabels(g, pane, scaleFactor);
                    if (maxLabels < (m_max - m_min) / m_majorStep)
                        m_majorStep = CalcBoundedStepSize(m_max - m_min, maxLabels);
                }
            }
            if (m_minorStepAuto)
                m_minorStep = CalcStepSize(m_majorStep,
                    (m_ownerAxis is XAxis) ?
                            Default.TargetMinorXSteps : Default.TargetMinorYSteps);
            if (m_minAuto)
                m_min = m_min - MyMod(m_min, m_majorStep);
            if (m_maxAuto)
                m_max = MyMod(m_max, m_majorStep) == 0.0 ? m_max :
                    m_max + m_majorStep - MyMod(m_max, m_majorStep);
            if (m_magAuto)
            {
                double mag = 0;
                double mag2 = 0;
                if (Math.Abs(m_min) > 1.0e-10)
                    mag = Math.Floor(Math.Log10(Math.Abs(m_min)));
                if (Math.Abs(m_max) > 1.0e-10)
                    mag2 = Math.Floor(Math.Log10(Math.Abs(m_max)));
                if (Math.Abs(mag2) > Math.Abs(mag))
                    mag = mag2;
                if (Math.Abs(mag) <= 3)
                    mag = 0;
                m_mag = (int)(Math.Floor(mag / 3.0) * 3.0);
            }
            if (m_formatAuto)
            {
                int numDec = 0 - (int)(Math.Floor(Math.Log10(m_majorStep)) - m_mag);
                if (numDec < 0)
                    numDec = 0;
                m_format = "f" + numDec.ToString();
            }
        }

        /// <summary>
        /// 建立刻度数据
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="axis">坐标轴</param>
        public override void SetupScaleData(GraphPane pane, Axis axis)
        {
            base.SetupScaleData(pane, axis);
            if (m_exponent > 0)
            {
                m_minLinTemp = Linearize(m_min);
                m_maxLinTemp = Linearize(m_max);
            }
            else if (m_exponent < 0)
            {
                m_minLinTemp = Linearize(m_max);
                m_maxLinTemp = Linearize(m_min);
            }
        }
        #endregion
    }
}
