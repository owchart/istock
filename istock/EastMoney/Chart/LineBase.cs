/*****************************************************************************\
*                                                                             *
* LineBase.cs -  LineBase functions, types, and definitions                   *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OwLib
{
    /// <summary>
    /// �ߵĻ���
    /// </summary>
    [Serializable]
    public class LineBase
    {
        #region �յ� 2016/6/7
        /// <summary>
        /// �����ߵĻ���
        /// </summary>
        public LineBase()
            : this(Color.Empty)
        {
        }

        /// <summary>
        /// �����ߵĻ���
        /// </summary>
        /// <param name="color">��ɫ</param>
        public LineBase(Color color)
        {
            m_width = Default.Width;
            m_style = Default.Style;
            m_dashOn = Default.DashOn;
            m_dashOff = Default.DashOff;
            m_isVisible = Default.IsVisible;
            m_color = color.IsEmpty ? Default.Color : color;
            m_isAntiAlias = Default.IsAntiAlias;
            m_gradientFill = new Fill(Color.Red, Color.White);
            m_gradientFill.Type = FillType.None;
        }

        /// <summary>
        /// �����ߵĻ���
        /// </summary>
        /// <param name="rhs">��������</param>
        public LineBase(LineBase rhs)
        {
            m_width = rhs.m_width;
            m_style = rhs.m_style;
            m_dashOn = rhs.m_dashOn;
            m_dashOff = rhs.m_dashOff;
            m_isVisible = rhs.m_isVisible;
            m_color = rhs.m_color;
            m_isAntiAlias = rhs.m_isAntiAlias;
            m_gradientFill = new Fill(rhs.m_gradientFill);
        }

        /// <summary>
        /// 
        /// </summary>
        ~LineBase()
        {
            if (pen != null) pen.Dispose();
        }

        /// <summary>
        /// Ĭ������
        /// </summary>
        public struct Default
        {
            public static bool IsVisible = true;
            public static float Width = 1;
            public static bool IsAntiAlias = false;
            public static DashStyle Style = DashStyle.Solid;
            public static float DashOn = 1.0F;
            public static float DashOff = 1.0F;
            public static Color Color = Color.FromArgb(30, 30, 30);
        }

        internal Color m_color;

        /// <summary>
        /// ��ȡ��������ɫ
        /// </summary>
        public Color Color
        {
            get { return m_color; }
            set { m_color = value; }
        }

        internal float m_dashOff;

        /// <summary>
        /// ��ȡ�����ö̻��߲���ʾ����
        /// </summary>
        public float DashOff
        {
            get { return m_dashOff; }
            set { m_dashOff = value; }
        }

        internal float m_dashOn;

        /// <summary>
        /// ��ȡ�����ö̻�����ʾ����
        /// </summary>
        public float DashOn
        {
            get { return m_dashOn; }
            set { m_dashOn = value; }
        }

        internal Fill m_gradientFill;

        /// <summary>
        /// ��ȡ�����ý������
        /// </summary>
        public Fill GradientFill
        {
            get { return m_gradientFill; }
            set { m_gradientFill = value; }
        }

        internal bool m_isAntiAlias;

        /// <summary>
        /// ��ȡ�������Ƿ����
        /// </summary>
        public bool IsAntiAlias
        {
            get { return m_isAntiAlias; }
            set { m_isAntiAlias = value; }
        }

        internal bool m_isVisible;

        /// <summary>
        /// ��ȡ�������Ƿ�ɼ�
        /// </summary>
        public bool IsVisible
        {
            get { return m_isVisible; }
            set { m_isVisible = value; }
        }

        internal DashStyle m_style;

        /// <summary>
        /// ��ȡ������������ʽ
        /// </summary>
        public DashStyle Style
        {
            get { return m_style; }
            set { m_style = value; }
        }

        internal float m_width;

        /// <summary>
        /// ��ȡ�����ÿ��
        /// </summary>
        public float Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        private Pen pen;

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="pane">ͼ��</param>
        /// <param name="scaleFactor">�̶�����</param>
        /// <returns>����</returns>
        public Pen GetPen(PaneBase pane, float scaleFactor)
        {
            return GetPen(pane, scaleFactor, null);
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="pane">ͼ��</param>
        /// <param name="scaleFactor">�̶�����</param>
        /// <param name="dataValue">��</param>
        /// <returns>����</returns>
        public Pen GetPen(PaneBase pane, float scaleFactor, PointPair dataValue)
        {
            Color color = m_color;
            if (m_gradientFill.IsGradientValueType)
                color = m_gradientFill.GetGradientColor(dataValue);
            if (pen == null)
            {
                pen = new Pen(color,
                        pane.ScaledPenWidth(m_width, scaleFactor));
            }
            else
            {
                pen.Color = color;
                pen.Width = pane.ScaledPenWidth(m_width, scaleFactor);
                pen.DashStyle = DashStyle.Solid;
            }
            pen.DashStyle = m_style;
            if (m_style == DashStyle.Custom)
            {
                if (m_dashOff > 1e-10 && m_dashOn > 1e-10)
                {
                    pen.DashStyle = DashStyle.Custom;
                    float[] pattern = new float[2];
                    pattern[0] = m_dashOn;
                    pattern[1] = m_dashOff;
                    pen.DashPattern = pattern;
                }
                else
                    pen.DashStyle = DashStyle.Solid;
            }
            return pen;
        }
        #endregion
    }
}
