/*****************************************************************************\
*                                                                             *
* MinorTic.cs -   MinorTic functions, types, and definitions                  *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
******************************************************************************/

using System;
using System.Drawing;
using System.Text;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OwLib
{
    /// <summary>
    /// 次变动值
    /// </summary>
    [Serializable]
    public class MinorTic
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建次变动值
        /// </summary>
        public MinorTic()
        {
            m_size = Default.Size;
            m_color = Default.Color;
            m_penWidth = Default.PenWidth;
            this.IsOutside = Default.IsOutside;
            this.IsInside = Default.IsInside;
            this.IsOpposite = Default.IsOpposite;
            m_isCrossOutside = Default.IsCrossOutside;
            m_isCrossInside = Default.IsCrossInside;
        }

        /// <summary>
        /// 创建次变动值
        /// </summary>
        /// <param name="rhs">其他对象</param>
        public MinorTic(MinorTic rhs)
        {
            m_size = rhs.m_size;
            m_color = rhs.m_color;
            m_penWidth = rhs.m_penWidth;
            this.IsOutside = rhs.IsOutside;
            this.IsInside = rhs.IsInside;
            this.IsOpposite = rhs.IsOpposite;
            m_isCrossOutside = rhs.m_isCrossOutside;
            m_isCrossInside = rhs.m_isCrossInside;
        }

        /// <summary>
        /// 默认属性
        /// </summary>
        public struct Default
        {
            public static float Size = 2.5F;
            public static float PenWidth = 1.0F;
            public static bool IsOutside = true;
            public static bool IsInside = true;
            public static bool IsOpposite = true;
            public static bool IsCrossOutside = false;
            public static bool IsCrossInside = false;
            public static Color Color = Color.FromArgb(30, 30, 30);
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

        /// <summary>
        /// 
        /// </summary>
        public bool IsAllTics
        {
            set
            {
                this.IsOutside = value;
                this.IsInside = value;
                this.IsOpposite = value;
                m_isCrossOutside = value;
                m_isCrossInside = value;
            }
        }

        internal bool m_isCrossInside;

        /// <summary>
        /// 
        /// </summary>
        public bool IsCrossInside
        {
            get { return m_isCrossInside; }
            set { m_isCrossInside = value; }
        }

        internal bool m_isCrossOutside;

        /// <summary>
        /// 
        /// </summary>
        public bool IsCrossOutside
        {
            get { return m_isCrossOutside; }
            set { m_isCrossOutside = value; }
        }

        internal bool m_isInside;

        /// <summary>
        /// 
        /// </summary>
        public bool IsInside
        {
            get { return m_isInside; }
            set { m_isInside = value; }
        }

        internal bool m_isOpposite;

        /// <summary>
        /// 
        /// </summary>
        public bool IsOpposite
        {
            get { return m_isOpposite; }
            set { m_isOpposite = value; }
        }

        internal bool m_isOutside;

        /// <summary>
        /// 
        /// </summary>
        public bool IsOutside
        {
            get { return m_isOutside; }
            set { m_isOutside = value; }
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

        internal float m_size;

        /// <summary>
        /// 
        /// </summary>
        public float Size
        {
            get { return m_size; }
            set { m_size = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="pen">画笔</param>
        /// <param name="pixVal">象素</param>
        /// <param name="topPix">顶部象素</param>
        /// <param name="shift">位移</param>
        /// <param name="scaledTic">最小变动值</param>
        internal void Draw(Graphics g, GraphPane pane, Pen pen, float pixVal, float topPix,
                    float shift, float scaledTic)
        {
            if (this.IsOutside)
                g.DrawLine(pen, pixVal, shift, pixVal, shift + scaledTic);
            if (m_isCrossOutside)
                if (this.IsInside)
                    g.DrawLine(pen, pixVal, shift, pixVal, shift - scaledTic);
            if (m_isCrossInside)
                g.DrawLine(pen, pixVal, 0.0f, pixVal, -scaledTic);
            if (this.IsOpposite) return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <returns></returns>
        internal Pen GetPen(GraphPane pane, float scaleFactor)
        {
            return new Pen(m_color, pane.ScaledPenWidth(m_penWidth, scaleFactor));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scaleFactor">刻度因子</param>
        /// <returns></returns>
        public float ScaledTic(float scaleFactor)
        {
            return (float)(m_size * scaleFactor);
        }
        #endregion
    }
}
