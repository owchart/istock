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
    /// X��
    /// </summary>
    [Serializable]
    public class XAxis : Axis
    {
        #region �յ� 2016/6/7
        /// <summary>
        /// ����X��
        /// </summary>
        public XAxis()
            : this("X Axis")
        {
        }

        /// <summary>
        /// ����X��
        /// </summary>
        /// <param name="title">����</param>
        public XAxis(String title)
            : base(title)
        {
            m_isVisible = Default.IsVisible;
            m_majorGrid.m_isZeroLine = Default.IsZeroLine;
            m_scale.m_fontSpec.Angle = 0F;
        }

        /// <summary>
        /// ����X��
        /// </summary>
        /// <param name="rhs">��������</param>
        public XAxis(XAxis rhs)
            : base(rhs)
        {
        }

        /// <summary>
        /// Ĭ������
        /// </summary>
        public new struct Default
        {
            public static bool IsVisible = true;
            public static bool IsZeroLine = false;
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
                return pane.YAxis.Scale.Transform(effCross) - pane.YAxis.Scale.m_maxPix;
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
            return pane.YAxis;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">ͼ��</param>
        /// <returns></returns>
        internal override bool IsPrimary(GraphPane pane)
        {
            return this == pane.XAxis;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">��ͼ����</param>
        /// <param name="pane">ͼ��</param>
        /// <param name="scaleFactor">�̶�����</param>
        public override void SetTransformMatrix(Graphics g, GraphPane pane, float scaleFactor)
        {
            g.TranslateTransform(pane.Chart.m_rect.Left, pane.Chart.m_rect.Bottom);
        }
        #endregion
    }
}
