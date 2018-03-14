/*****************************************************************************\
*                                                                             *
* OrdinalScale.cs -   OrdinalScale functions, types, and definitions          *
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
    /// 序数刻度
    /// </summary>
    [Serializable]
    class OrdinalScale : Scale
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建序数刻度
        /// </summary>
        /// <param name="owner">所属对象</param>
        public OrdinalScale(Axis owner)
            : base(owner)
        {
        }

        /// <summary>
        /// 创建序数刻度
        /// </summary>
        /// <param name="rhs">其他对象</param>
        /// <param name="owner">所属对象</param>
        public OrdinalScale(Scale rhs, Axis owner)
            : base(rhs, owner)
        {
        }

        /// <summary>
        /// 获取坐标轴类型
        /// </summary>
        public override AxisType Type
        {
            get { return AxisType.Ordinal; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner">所属对象</param>
        /// <returns></returns>
        public override Scale Clone(Axis owner)
        {
            return new OrdinalScale(this, owner);
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
            PickScale(pane, g, scaleFactor, this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="g">绘图对象</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="scale">坐标轴</param>
        internal static void PickScale(GraphPane pane, Graphics g, float scaleFactor, Scale scale)
        {
            if (scale.m_max - scale.m_min < 1.0)
            {
                if (scale.m_maxAuto)
                    scale.m_max = scale.m_min + 0.5;
                else
                    scale.m_min = scale.m_max - 0.5;
            }
            else
            {
                if (scale.m_majorStepAuto)
                {
                    scale.m_majorStep = Scale.CalcStepSize(scale.m_max - scale.m_min,
                        (scale.m_ownerAxis is XAxis) ?
                                Default.TargetXSteps : Default.TargetYSteps);
                    if (scale.IsPreventLabelOverlap)
                    {
                        double maxLabels = (double)scale.CalcMaxLabels(g, pane, scaleFactor);
                        double tmpStep = Math.Ceiling((scale.m_max - scale.m_min) / maxLabels);
                        if (tmpStep > scale.m_majorStep)
                            scale.m_majorStep = tmpStep;
                    }
                }
                scale.m_majorStep = (int)scale.m_majorStep;
                if (scale.m_majorStep < 1.0)
                    scale.m_majorStep = 1.0;
                if (scale.m_minorStepAuto)
                    scale.m_minorStep = Scale.CalcStepSize(scale.m_majorStep,
                        (scale.m_ownerAxis is XAxis) ?
                                Default.TargetMinorXSteps : Default.TargetMinorYSteps);
                if (scale.m_minAuto)
                    scale.m_min -= 0.5;
                if (scale.m_maxAuto)
                    scale.m_max += 0.5;
            }
        }
        #endregion
    }
}
