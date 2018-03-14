/********************************************************************************\
*                                                                                *
* XAxis.cs -    XAxis functions, types, and definitions                          *
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
    /// X轴
    /// </summary>
    [Serializable]
    public class XAxis : Axis
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建X轴
        /// </summary>
        public XAxis()
            : this("X Axis")
        {
        }

        /// <summary>
        /// 创建X轴
        /// </summary>
        /// <param name="title">标题</param>
        public XAxis(String title)
            : base(title)
        {
            m_isVisible = Default.IsVisible;
            m_majorGrid.m_isZeroLine = Default.IsZeroLine;
            m_scale.m_fontSpec.Angle = 0F;
        }

        /// <summary>
        /// 创建X轴
        /// </summary>
        /// <param name="rhs">其他对象</param>
        public XAxis(XAxis rhs)
            : base(rhs)
        {
        }

        /// <summary>
        /// 默认属性
        /// </summary>
        public new struct Default
        {
            public static bool IsVisible = true;
            public static bool IsZeroLine = false;
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
                return pane.YAxis.Scale.Transform(effCross) - pane.YAxis.Scale.m_maxPix;
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
            return pane.YAxis;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns></returns>
        internal override bool IsPrimary(GraphPane pane)
        {
            return this == pane.XAxis;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        public override void SetTransformMatrix(Graphics g, GraphPane pane, float scaleFactor)
        {
            g.TranslateTransform(pane.Chart.m_rect.Left, pane.Chart.m_rect.Bottom);
        }
        #endregion
    }
}
