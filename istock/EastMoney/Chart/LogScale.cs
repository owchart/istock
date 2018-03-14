/*****************************************************************************\
*                                                                             *
* LogScale.cs -  LogScale functions, types, and definitions                   *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
******************************************************************************/

using System;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OwLib
{
    /// <summary>
    /// 对数刻度
    /// </summary>
    [Serializable]
    class LogScale : Scale
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建对数刻度
        /// </summary>
        /// <param name="owner">所属对象</param>
        public LogScale(Axis owner)
            : base(owner)
        {
        }

        /// <summary>
        /// 创建对数刻度
        /// </summary>
        /// <param name="rhs">其他对象</param>
        /// <param name="owner">所属对象</param>
        public LogScale(Scale rhs, Axis owner)
            : base(rhs, owner)
        {
        }

        /// <summary>
        /// 创建对数刻度
        /// </summary>
        public override double Max
        {
            get { return m_max; }
            set { if (value > 0) m_max = value; }
        }

        /// <summary>
        /// 创建对数刻度
        /// </summary>
        public override double Min
        {
            get { return m_min; }
            set { if (value > 0) m_min = value; }
        }

        /// <summary>
        /// 获取坐标轴类型
        /// </summary>
        public override AxisType Type
        {
            get { return AxisType.Log; }
        }

        /// <summary>
        /// 计算主要刻度值
        /// </summary>
        /// <param name="baseVal">基础值</param>
        /// <param name="tic">最小变动值</param>
        /// <returns>刻度值</returns>
        internal override double CalcMajorTicValue(double baseVal, double tic)
        {
            return baseVal + (double)tic * CyclesPerStep;
        }

        /// <summary>
        /// 计算次要刻度值
        /// </summary>
        /// <param name="baseVal">基础值</param>
        /// <param name="iTic">最小变动范围</param>
        /// <returns>刻度值</returns>
        internal override double CalcMinorTicValue(double baseVal, int iTic)
        {
            double[] dLogVal = { 0, 0.301029995663981, 0.477121254719662, 0.602059991327962,
									0.698970004336019, 0.778151250383644, 0.845098040014257,
									0.903089986991944, 0.954242509439325, 1 };
            return baseVal + Math.Floor((double)iTic / 9.0) + dLogVal[(iTic + 9) % 9];
        }

        /// <summary>
        /// 计算最小开始
        /// </summary>
        /// <param name="baseVal">基础值</param>
        /// <returns>索引</returns>
        internal override int CalcMinorStart(double baseVal)
        {
            return -9;
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
                return Math.Ceiling(Scale.SafeLog(m_min) - 0.00000001);
            }
        }

        /// <summary>
        /// 计算变动值数量
        /// </summary>
        /// <returns>数量</returns>
        internal override int CalcNumTics()
        {
            int nTics = 1;
            nTics = (int)((Scale.SafeLog(m_max) - Scale.SafeLog(m_min)) / CyclesPerStep) + 1;
            if (nTics < 1)
                nTics = 1;
            else if (nTics > 1000)
                nTics = 1000;
            return nTics;
        }
        
        /// <summary>
        /// 获取每步的周期数
        /// </summary>
        private double CyclesPerStep
        {
            get { return m_majorStep; }
        }

        /// <summary>
        /// 克隆对象
        /// </summary>
        /// <param name="owner">所属对象</param>
        /// <returns>刻度</returns>
        public override Scale Clone(Axis owner)
        {
            return new LogScale(this, owner);
        }

        /// <summary>
        /// 反线性
        /// </summary>
        /// <param name="val">数值</param>
        /// <returns>结果</returns>
        public override double DeLinearize(double val)
        {
            return Math.Pow(10.0, val);
        }

        /// <summary>
        /// 线性
        /// </summary>
        /// <param name="val">数值</param>
        /// <returns>结果</returns>
        public override double Linearize(double val)
        {
            return SafeLog(val);
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
            if (m_isUseTenPower)
                return String.Format("{0:F0}", dVal);
            else
                return Math.Pow(10.0, dVal).ToString(m_format);
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
            if (m_majorStepAuto)
                m_majorStep = 1.0;
            m_mag = 0;
            if (m_min <= 0.0 && m_max <= 0.0)
            {
                m_min = 1.0;
                m_max = 10.0;
            }
            else if (m_min <= 0.0)
            {
                m_min = m_max / 10.0;
            }
            else if (m_max <= 0.0)
            {
                m_max = m_min * 10.0;
            }
            if (m_max - m_min < 1.0e-20)
            {
                if (m_maxAuto)
                    m_max = m_max * 2.0;
                if (m_minAuto)
                    m_min = m_min / 2.0;
            }
            if (m_minAuto)
                m_min = Math.Pow((double)10.0,
                    Math.Floor(Math.Log10(m_min)));
            if (m_maxAuto)
                m_max = Math.Pow((double)10.0,
                    Math.Ceiling(Math.Log10(m_max)));
        }

        /// <summary>
        /// 建立坐标数值
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="axis">坐标轴</param>
        public override void SetupScaleData(GraphPane pane, Axis axis)
        {
            base.SetupScaleData(pane, axis);
            m_minLinTemp = Linearize(m_min);
            m_maxLinTemp = Linearize(m_max);
        }
        #endregion
    }
}
