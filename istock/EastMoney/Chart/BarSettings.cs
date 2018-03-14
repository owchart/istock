/*****************************************************************************\
*                                                                             *
* BarSettings.cs -  Bar settings  functions, types, and definitions           *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using System.Drawing;
using System.Text;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OwLib
{
    /// <summary>
    /// ��״ͼ����
    /// </summary>
    [Serializable]
    public class BarSettings
    {
        #region �յ� 2016/5/31
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="parentPane">��ͼ��</param>
        public BarSettings(GraphPane parentPane)
        {
            m_minClusterGap = Default.MinClusterGap;
            m_minBarGap = Default.MinBarGap;
            m_clusterScaleWidth = Default.ClusterScaleWidth;
            m_clusterScaleWidthAuto = Default.ClusterScaleWidthAuto;
            m_base = Default.Base;
            m_type = Default.Type;
            m_ownerPane = parentPane;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="rhs">��������</param>
        /// <param name="parentPane">��ͼ��</param>
        public BarSettings(BarSettings rhs, GraphPane parentPane)
        {
            m_minClusterGap = rhs.m_minClusterGap;
            m_minBarGap = rhs.m_minBarGap;
            m_clusterScaleWidth = rhs.m_clusterScaleWidth;
            m_clusterScaleWidthAuto = rhs.m_clusterScaleWidthAuto;
            m_base = rhs.m_base;
            m_type = rhs.m_type;
            m_ownerPane = parentPane;
        }

        /// <summary>
        /// Ĭ������
        /// </summary>
        public struct Default
        {
            public static float MinClusterGap = 0.2F;
            public static float MinBarGap = 0.1F;
            public static BarBase Base = BarBase.X;
            public static BarType Type = BarType.Cluster;
            public static double ClusterScaleWidth = 1.0;
            public static bool ClusterScaleWidthAuto = true;
        }

        /// <summary>
        /// ����ͼ��
        /// </summary>
        internal GraphPane m_ownerPane;

        private BarBase m_base;

        /// <summary>
        /// ��ȡ����������������
        /// </summary>
        public BarBase Base
        {
            get { return m_base; }
            set { m_base = value; }
        }

        internal double m_clusterScaleWidth;

        /// <summary>
        /// ��ȡ�����ü�Ⱥ�̶ȿ��
        /// </summary>
        public double ClusterScaleWidth
        {
            get { return m_clusterScaleWidth; }
            set { m_clusterScaleWidth = value; m_clusterScaleWidthAuto = false; }
        }

        internal bool m_clusterScaleWidthAuto;

        /// <summary>
        /// ��ȡ�����ü�Ⱥ�̶��Զ����
        /// </summary>
        public bool ClusterScaleWidthAuto
        {
            get { return m_clusterScaleWidthAuto; }
            set { m_clusterScaleWidthAuto = value; }
        }

        private float m_minBarGap;

        /// <summary>
        /// ��ȡ��������С���
        /// </summary>
        public float MinBarGap
        {
            get { return m_minBarGap; }
            set { m_minBarGap = value; }
        }

        private float m_minClusterGap;

        /// <summary>
        /// ��ȡ��������С��Ⱥ���
        /// </summary>
        public float MinClusterGap
        {
            get { return m_minClusterGap; }
            set { m_minClusterGap = value; }
        }

        private BarType m_type;

        /// <summary>
        /// ��ȡ��������״ͼ����
        /// </summary>
        public BarType Type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        /// <summary>
        /// ��ȡ����������
        /// </summary>
        /// <returns>������</returns>
        public Axis BarBaseAxis()
        {
            Axis barAxis;
            if (m_base == BarBase.Y)
                barAxis = m_ownerPane.YAxis;
            else if (m_base == BarBase.Y2)
                barAxis = m_ownerPane.Y2Axis;
            else
                barAxis = m_ownerPane.XAxis;
            return barAxis;
        }

        /// <summary>
        /// ���㼯Ⱥ�̶ȿ��
        /// </summary>
        public void CalcClusterScaleWidth()
        {
            Axis baseAxis = BarBaseAxis();
            if (m_clusterScaleWidthAuto && !baseAxis.Scale.IsAnyOrdinal)
            {
                double minStep = Double.MaxValue;
                foreach (CurveItem curve in m_ownerPane.CurveList)
                {
                    IPointList list = curve.Points;
                    if (curve is BarItem)
                    {
                        double step = GetMinStepSize(curve.Points, baseAxis);
                        minStep = step < minStep ? step : minStep;
                    }
                }
                if (minStep == Double.MaxValue)
                    minStep = 1.0;
                m_clusterScaleWidth = minStep;
            }
        }

        /// <summary>
        /// ��ȡ��Ⱥ���
        /// </summary>
        /// <returns></returns>
        public float GetClusterWidth()
        {
            return BarBaseAxis().m_scale.GetClusterWidth(m_ownerPane);
        }

        /// <summary>
        /// ��ȡ��С������С
        /// </summary>
        /// <param name="list">��ֵ</param>
        /// <param name="baseAxis">��������</param>
        /// <returns>��С</returns>
        internal static double GetMinStepSize(IPointList list, Axis baseAxis)
        {
            double minStep = Double.MaxValue;
            if (list.Count <= 0 || baseAxis.m_scale.IsAnyOrdinal)
                return 1.0;
            PointPair lastPt = list[0];
            for (int i = 1; i < list.Count; i++)
            {
                PointPair pt = list[i];
                if (!pt.IsInvalid || !lastPt.IsInvalid)
                {
                    double step;
                    if (baseAxis is XAxis)
                        step = pt.X - lastPt.X;
                    else
                        step = pt.Y - lastPt.Y;
                    if (step > 0 && step < minStep)
                        minStep = step;
                }
                lastPt = pt;
            }
            double range = baseAxis.Scale.m_maxLinearized - baseAxis.Scale.m_minLinearized;
            if (range <= 0)
                minStep = 1.0;
            else if (minStep <= 0 || minStep > range)
                minStep = 0.1 * range;
            return minStep;
        }
        #endregion
    }
}
