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
    /// ���̶ֿ�
    /// </summary>
    [Serializable]
    class TextScale : Scale
    {
        #region �յ� 2016/6/7
        /// <summary>
        /// �������̶ֿ�
        /// </summary>
        /// <param name="owner">��������</param>
        public TextScale(Axis owner)
            : base(owner)
        {
        }

        /// <summary>
        /// �������̶ֿ�
        /// </summary>
        /// <param name="rhs">��������</param>
        /// <param name="owner">��������</param>
        public TextScale(Scale rhs, Axis owner)
            : base(rhs, owner)
        {
        }

        /// <summary>
        /// ��ȡ����������
        /// </summary>
        public override AxisType Type
        {
            get { return AxisType.Text; }
        }

        /// <summary>
        /// ��������䶯ֵ
        /// </summary>
        /// <returns>�䶯ֵ</returns>
        internal override double CalcBaseTic()
        {
            if (m_baseTic != PointPair.Missing)
                return m_baseTic;
            else
                return 1.0;
        }

        /// <summary>
        /// ������С��ʼ
        /// </summary>
        /// <param name="baseVal">����ֵ</param>
        /// <returns>����</returns>
        internal override int CalcMinorStart(double baseVal)
        {
            return 0;
        }

        /// <summary>
        /// ����䶯ֵ����
        /// </summary>
        /// <returns>����</returns>
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
        /// <param name="owner">��������</param>
        /// <returns></returns>
        public override Scale Clone(Axis owner)
        {
            return new TextScale(this, owner);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">ͼ��</param>
        /// <param name="index">����</param>
        /// <param name="dVal">��ֵ</param>
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
        /// <param name="pane">ͼ��</param>
        /// <param name="g">��ͼ����</param>
        /// <param name="scaleFactor">�̶�����</param>
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
