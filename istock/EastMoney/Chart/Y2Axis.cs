/********************************************************************************\
*                                                                                *
* Y2Axis.cs -    Y2Axis functions, types, and definitions                        *
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
    /// Y2轴
    /// </summary>
    [Serializable]
    public class Y2Axis : Axis
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建Y2轴
        /// </summary>
        public Y2Axis()
            : this("Y2 Axis")
        {
        }

        /// <summary>
        /// 创建Y2轴
        /// </summary>
        /// <param name="title">标题</param>
        public Y2Axis(String title)
            : base(title)
        {
            m_isVisible = Default.IsVisible;
            m_majorGrid.m_isZeroLine = Default.IsZeroLine;
            m_scale.m_fontSpec.Angle = -90.0F;
        }

        /// <summary>
        /// 创建Y2轴
        /// </summary>
        /// <param name="rhs">其他对象</param>
        public Y2Axis(Y2Axis rhs)
            : base(rhs)
        {
        }

        /// <summary>
        /// 默认属性
        /// </summary>
        public new struct Default
        {
            public static bool IsVisible = false;
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
                return pane.XAxis.Scale.Transform(effCross) - pane.XAxis.Scale.m_maxPix;
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
            return this == pane.Y2Axis;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        public override void SetTransformMatrix(Graphics g, GraphPane pane, float scaleFactor)
        {
            g.TranslateTransform(pane.Chart.m_rect.Right, pane.Chart.m_rect.Bottom);
            g.RotateTransform(-90);
        }
        #endregion
    }
}
