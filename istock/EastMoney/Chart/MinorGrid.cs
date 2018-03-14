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
    /// 次网格
    /// </summary>
    [Serializable]
    public class MinorGrid
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建次网格
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
        /// 创建次网格
        /// </summary>
        /// <param name="rhs">其他对象</param>
        public MinorGrid(MinorGrid rhs)
        {
            m_dashOn = rhs.m_dashOn;
            m_dashOff = rhs.m_dashOff;
            m_penWidth = rhs.m_penWidth;
            m_isVisible = rhs.m_isVisible;
            m_color = rhs.m_color;
        }

        /// <summary>
        /// 默认属性
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
        /// <param name="g">绘图对象</param>
        /// <param name="pen">画笔</param>
        /// <param name="pixVal">象素</param>
        /// <param name="topPix">顶部象素</param>
        internal void Draw(Graphics g, Pen pen, float pixVal, float topPix)
        {
            if (m_isVisible)
                g.DrawLine(pen, pixVal, 0.0F, pixVal, topPix);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
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
