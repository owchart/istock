/********************************************************************************\
*                                                                                *
* YAxis.cs -    YAxis functions, types, and definitions                          *
*                                                                                *
*               Version 1.00                                                     *
*                                                                                *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.      *
*                                                                                *
*********************************************************************************/

using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OwLib
{
    /// <summary>
    /// Y轴
    /// </summary>
    [Serializable]
    public class YAxis : Axis
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建Y轴
        /// </summary>
        public YAxis()
            : this("Y Axis")
        {
        }

        /// <summary>
        /// 创建Y轴
        /// </summary>
        /// <param name="title">标题</param>
        public YAxis(String title)
            : base(title)
        {
            m_isVisible = Default.IsVisible;
            m_majorGrid.m_isZeroLine = Default.IsZeroLine;
            m_scale.m_fontSpec.Angle = 90.0F;
            m_title.m_fontSpec.Angle = -180F;
        }

        /// <summary>
        /// 创建Y轴
        /// </summary>
        /// <param name="rhs">其他对象</param>
        public YAxis(YAxis rhs)
            : base(rhs)
        {
        }

        /// <summary>
        /// 默认属性
        /// </summary>
        public new struct Default
        {
            public static bool IsVisible = true;
            public static bool IsZeroLine = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns></returns>
        internal override float CalcCrossShift(GraphPane pane)
        {
            double effCross = EffectiveCrossValue(pane);
            if (!m_crossAuto)
                return pane.XAxis.Scale.m_minPix - pane.XAxis.Scale.Transform(effCross);
            else
                return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns></returns>
        public override Axis GetCrossAxis(GraphPane pane)
        {
            return pane.XAxis;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns></returns>
        internal override bool IsPrimary(GraphPane pane)
        {
            return this == pane.YAxis;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        public override void SetTransformMatrix(Graphics g, GraphPane pane, float scaleFactor)
        {
            g.TranslateTransform(pane.Chart.m_rect.Left, pane.Chart.m_rect.Top);
            g.RotateTransform(90);
        }
        #endregion
    }
}
