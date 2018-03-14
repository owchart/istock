/*****************************************************************************\
*                                                                             *
* LinearScale.cs - LinearScale functions, types, and definitions         *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OwLib
{
    /// <summary>
    /// ���Կ̶�
    /// </summary>
    [Serializable]
    class LinearScale : Scale
    {
        #region �յ� 2016/6/7
        /// <summary>
        /// �������Կ̶�
        /// </summary>
        /// <param name="owner">��������</param>
        public LinearScale(Axis owner)
            : base(owner)
        {
        }

        /// <summary>
        /// �������Կ̶�
        /// </summary>
        /// <param name="rhs">��������</param>
        /// <param name="owner">��������</param>
        public LinearScale(Scale rhs, Axis owner)
            : base(rhs, owner)
        {
        }

        /// <summary>
        /// ��ȡ����������
        /// </summary>
        public override AxisType Type
        {
            get { return AxisType.Linear; }
        }

        /// <summary>
        /// ���ƶ���
        /// </summary>
        /// <param name="owner">��������</param>
        /// <returns>�̶�</returns>
        public override Scale Clone(Axis owner)
        {
            return new LinearScale(this, owner);
        }

        /// <summary>
        /// ��ѡ�̶�
        /// </summary>
        /// <param name="pane">ͼ��</param>
        /// <param name="g">��ͼ����</param>
        /// <param name="scaleFactor">�̶�����</param>
        public override void PickScale(GraphPane pane, Graphics g, float scaleFactor)
        {
            base.PickScale(pane, g, scaleFactor);
            if (m_max - m_min < 1.0e-30)
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
            SetScaleMag(m_min, m_max, m_majorStep);
        }
        #endregion
    }
}
