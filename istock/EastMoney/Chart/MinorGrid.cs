/*****************************************************************************\
*                                                                             *
* MinorGrid.cs -   MinorGrid functions, types, and definitions                *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
******************************************************************************/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OwLib
{
    /// <summary>
    /// ������
    /// </summary>
    [Serializable]
    public class MinorGrid
    {
        #region �յ� 2016/6/7
        /// <summary>
        /// ����������
        /// </summary>
        public MinorGrid()
        {
            m_dashOn = Default.DashOn;
            m_dashOff = Default.DashOff;
            m_penWidth = Default.PenWidth;
            m_isVisible = Default.IsVisible;
            m_color = Default.Color;
        }

        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="rhs">��������</param>
        public MinorGrid(MinorGrid rhs)
        {
            m_dashOn = rhs.m_dashOn;
            m_dashOff = rhs.m_dashOff;
            m_penWidth = rhs.m_penWidth;
            m_isVisible = rhs.m_isVisible;
            m_color = rhs.m_color;
        }

        /// <summary>
        /// Ĭ������
        /// </summary>
        public struct Default
        {
            public static float DashOn = 1.0F;
            public static float DashOff = 10.0F;
            public static float PenWidth = 1.0F;
            public static Color Color = Color.Gray;
            public static bool IsVisible = false;
        }

        internal Color m_color;

        /// <summary>
        /// 
        /// </summary>
        public Color Color
        {
            get { return m_color; }
            set { m_color = value; }
        }

        internal float m_dashOff;

        /// <summary>
        /// 
        /// </summary>
        public float DashOff
        {
            get { return m_dashOff; }
            set { m_dashOff = value; }
        }

        internal float m_dashOn;

        /// <summary>
        /// 
        /// </summary>
        public float DashOn
        {
            get { return m_dashOn; }
            set { m_dashOn = value; }
        }

        internal bool m_isVisible;

        /// <summary>
        /// 
        /// </summary>
        public bool IsVisible
        {
            get { return m_isVisible; }
            set { m_isVisible = value; }
        }

        internal float m_penWidth;

        /// <summary>
        /// 
        /// </summary>
        public float PenWidth
        {
            get { return m_penWidth; }
            set { m_penWidth = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">��ͼ����</param>
        /// <param name="pen">����</param>
        /// <param name="pixVal">����</param>
        /// <param name="topPix">��������</param>
        internal void Draw(Graphics g, Pen pen, float pixVal, float topPix)
        {
            if (m_isVisible)
                g.DrawLine(pen, pixVal, 0.0F, pixVal, topPix);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">ͼ��</param>
        /// <param name="scaleFactor">�̶�����</param>
        /// <returns></returns>
        internal Pen GetPen(GraphPane pane, float scaleFactor)
        {
            Pen pen = new Pen(m_color,
                        pane.ScaledPenWidth(m_penWidth, scaleFactor));
            if (m_dashOff > 1e-10 && m_dashOn > 1e-10)
            {
                pen.DashStyle = DashStyle.Custom;
                float[] pattern = new float[2];
                pattern[0] = m_dashOn;
                pattern[1] = m_dashOff;
                pen.DashPattern = pattern;
            }
            return pen;
        }
        #endregion
    }
}
