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
    /// �����̶�
    /// </summary>
    [Serializable]
    class LogScale : Scale
    {
        #region �յ� 2016/6/7
        /// <summary>
        /// ���������̶�
        /// </summary>
        /// <param name="owner">��������</param>
        public LogScale(Axis owner)
            : base(owner)
        {
        }

        /// <summary>
        /// ���������̶�
        /// </summary>
        /// <param name="rhs">��������</param>
        /// <param name="owner">��������</param>
        public LogScale(Scale rhs, Axis owner)
            : base(rhs, owner)
        {
        }

        /// <summary>
        /// ���������̶�
        /// </summary>
        public override double Max
        {
            get { return m_max; }
            set { if (value > 0) m_max = value; }
        }

        /// <summary>
        /// ���������̶�
        /// </summary>
        public override double Min
        {
            get { return m_min; }
            set { if (value > 0) m_min = value; }
        }

        /// <summary>
        /// ��ȡ����������
        /// </summary>
        public override AxisType Type
        {
            get { return AxisType.Log; }
        }

        /// <summary>
        /// ������Ҫ�̶�ֵ
        /// </summary>
        /// <param name="baseVal">����ֵ</param>
        /// <param name="tic">��С�䶯ֵ</param>
        /// <returns>�̶�ֵ</returns>
        internal override double CalcMajorTicValue(double baseVal, double tic)
        {
            return baseVal + (double)tic * CyclesPerStep;
        }

        /// <summary>
        /// �����Ҫ�̶�ֵ
        /// </summary>
        /// <param name="baseVal">����ֵ</param>
        /// <param name="iTic">��С�䶯��Χ</param>
        /// <returns>�̶�ֵ</returns>
        internal override double CalcMinorTicValue(double baseVal, int iTic)
        {
            double[] dLogVal = { 0, 0.301029995663981, 0.477121254719662, 0.602059991327962,
									0.698970004336019, 0.778151250383644, 0.845098040014257,
									0.903089986991944, 0.954242509439325, 1 };
            return baseVal + Math.Floor((double)iTic / 9.0) + dLogVal[(iTic + 9) % 9];
        }

        /// <summary>
        /// ������С��ʼ
        /// </summary>
        /// <param name="baseVal">����ֵ</param>
        /// <returns>����</returns>
        internal override int CalcMinorStart(double baseVal)
        {
            return -9;
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
            {
                return Math.Ceiling(Scale.SafeLog(m_min) - 0.00000001);
            }
        }

        /// <summary>
        /// ����䶯ֵ����
        /// </summary>
        /// <returns>����</returns>
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
        /// ��ȡÿ����������
        /// </summary>
        private double CyclesPerStep
        {
            get { return m_majorStep; }
        }

        /// <summary>
        /// ��¡����
        /// </summary>
        /// <param name="owner">��������</param>
        /// <returns>�̶�</returns>
        public override Scale Clone(Axis owner)
        {
            return new LogScale(this, owner);
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="val">��ֵ</param>
        /// <returns>���</returns>
        public override double DeLinearize(double val)
        {
            return Math.Pow(10.0, val);
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="val">��ֵ</param>
        /// <returns>���</returns>
        public override double Linearize(double val)
        {
            return SafeLog(val);
        }

        /// <summary>
        /// ������ǩ
        /// </summary>
        /// <param name="pane">ͼ��</param>
        /// <param name="index">����</param>
        /// <param name="dVal">��ֵ</param>
        /// <returns>��ǩ</returns>
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
        /// ��ѡ�̶�
        /// </summary>
        /// <param name="pane">ͼ��</param>
        /// <param name="g">��ͼ����</param>
        /// <param name="scaleFactor">�̶�����</param>
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
        /// ����������ֵ
        /// </summary>
        /// <param name="pane">ͼ��</param>
        /// <param name="axis">������</param>
        public override void SetupScaleData(GraphPane pane, Axis axis)
        {
            base.SetupScaleData(pane, axis);
            m_minLinTemp = Linearize(m_min);
            m_maxLinTemp = Linearize(m_max);
        }
        #endregion
    }
}
