/********************************************************************************\
*                                                                                *
* TextScale.cs -    TextScale functions, types, and definitions                  *
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
    /// 文字刻度
    /// </summary>
    [Serializable]
    class TextScale : Scale
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建文字刻度
        /// </summary>
        /// <param name="owner">所属对象</param>
        public TextScale(Axis owner)
            : base(owner)
        {
        }

        /// <summary>
        /// 创建文字刻度
        /// </summary>
        /// <param name="rhs">其他对象</param>
        /// <param name="owner">所属对象</param>
        public TextScale(Scale rhs, Axis owner)
            : base(rhs, owner)
        {
        }

        /// <summary>
        /// 获取坐标轴类型
        /// </summary>
        public override AxisType Type
        {
            get { return AxisType.Text; }
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
                return 1.0;
        }

        /// <summary>
        /// 计算最小开始
        /// </summary>
        /// <param name="baseVal">基础值</param>
        /// <returns>索引</returns>
        internal override int CalcMinorStart(double baseVal)
        {
            return 0;
        }

        /// <summary>
        /// 计算变动值数量
        /// </summary>
        /// <returns>数量</returns>
        internal override int CalcNumTics()
        {
            int nTics = 1;
            if (m_textLabels == null)
                nTics = 10;
            else
                nTics = m_textLabels.Length;
            if (nTics < 1)
                nTics = 1;
            else if (nTics > 1000)
                nTics = 1000;
            return nTics;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner">所属对象</param>
        /// <returns></returns>
        public override Scale Clone(Axis owner)
        {
            return new TextScale(this, owner);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="index">索引</param>
        /// <param name="dVal">数值</param>
        /// <returns></returns>
        internal override String MakeLabel(GraphPane pane, int index, double dVal)
        {
            if (m_format == null)
                m_format = Scale.Default.Format;
            index *= (int)m_majorStep;
            if (m_textLabels == null || index < 0 || index >= m_textLabels.Length)
                return String.Empty;
            else
                return m_textLabels[index];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="g">绘图对象</param>
        /// <param name="scaleFactor">刻度因子</param>
        public override void PickScale(GraphPane pane, Graphics g, float scaleFactor)
        {
            base.PickScale(pane, g, scaleFactor);
            if (m_textLabels != null)
            {
                if (m_minAuto)
                    m_min = 0.5;
                if (m_maxAuto)
                    m_max = m_textLabels.Length + 0.5;
            }
            else
            {
                if (m_minAuto)
                    m_min -= 0.5;
                if (m_maxAuto)
                    m_max += 0.5;
            }
            if (m_max - m_min < .1)
            {
                if (m_maxAuto)
                    m_max = m_min + 10.0;
                else
                    m_min = m_max - 10.0;
            }
            if (m_majorStepAuto)
            {
                if (!m_isPreventLabelOverlap)
                {
                    m_majorStep = 1;
                }
                else if (m_textLabels != null)
                {
                    double maxLabels = (double)this.CalcMaxLabels(g, pane, scaleFactor);
                    double tmpStep = Math.Ceiling((m_max - m_min) / maxLabels);
                    m_majorStep = tmpStep;
                }
                else
                    m_majorStep = (int)((m_max - m_min - 1.0) / Default.MaxTextLabels) + 1.0;
            }
            else
            {
                m_majorStep = (int)m_majorStep;
                if (m_majorStep <= 0)
                    m_majorStep = 1.0;
            }
            if (m_minorStepAuto)
            {
                m_minorStep = m_majorStep / 10;
                if (m_minorStep < 1)
                    m_minorStep = 1;
            }
            m_mag = 0;
        }
        #endregion
    }
}
