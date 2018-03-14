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
    /// Y2��
    /// </summary>
    [Serializable]
    public class Y2Axis : Axis
    {
        #region �յ� 2016/6/7
        /// <summary>
        /// ����Y2��
        /// </summary>
        public Y2Axis()
            : this("Y2 Axis")
        {
        }

        /// <summary>
        /// ����Y2��
        /// </summary>
        /// <param name="title">����</param>
        public Y2Axis(String title)
            : base(title)
        {
            m_isVisible = Default.IsVisible;
            m_majorGrid.m_isZeroLine = Default.IsZeroLine;
            m_scale.m_fontSpec.Angle = -90.0F;
        }

        /// <summary>
        /// ����Y2��
        /// </summary>
        /// <param name="rhs">��������</param>
        public Y2Axis(Y2Axis rhs)
            : base(rhs)
        {
        }

        /// <summary>
        /// Ĭ������
        /// </summary>
        public new struct Default
        {
            public static bool IsVisible = false;
            public static bool IsZeroLine = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">ͼ��</param>
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
        /// <param name="pane">ͼ��</param>
        /// <returns></returns>
        public override Axis GetCrossAxis(GraphPane pane)
        {
            return pane.XAxis;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">ͼ��</param>
        /// <returns></returns>
        internal override bool IsPrimary(GraphPane pane)
        {
            return this == pane.Y2Axis;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">��ͼ����</param>
        /// <param name="pane">ͼ��</param>
        /// <param name="scaleFactor">�̶�����</param>
        public override void SetTransformMatrix(Graphics g, GraphPane pane, float scaleFactor)
        {
            g.TranslateTransform(pane.Chart.m_rect.Right, pane.Chart.m_rect.Bottom);
            g.RotateTransform(-90);
        }
        #endregion
    }
}
