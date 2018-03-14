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
    /// 线的基类
    /// </summary>
    [Serializable]
    public class LineBase
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建线的基类
        /// </summary>
        public LineBase()
            : this(Color.Empty)
        {
        }

        /// <summary>
        /// 创建线的基类
        /// </summary>
        /// <param name="color">颜色</param>
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
        /// 创建线的基类
        /// </summary>
        /// <param name="rhs">其他对象</param>
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
        /// 默认属性
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
        /// 获取或设置颜色
        /// </summary>
        public Color Color
        {
            get { return m_color; }
            set { m_color = value; }
        }

        internal float m_dashOff;

        /// <summary>
        /// 获取或设置短划线不显示部分
        /// </summary>
        public float DashOff
        {
            get { return m_dashOff; }
            set { m_dashOff = value; }
        }

        internal float m_dashOn;

        /// <summary>
        /// 获取或设置短划线显示部分
        /// </summary>
        public float DashOn
        {
            get { return m_dashOn; }
            set { m_dashOn = value; }
        }

        internal Fill m_gradientFill;

        /// <summary>
        /// 获取或设置渐变填充
        /// </summary>
        public Fill GradientFill
        {
            get { return m_gradientFill; }
            set { m_gradientFill = value; }
        }

        internal bool m_isAntiAlias;

        /// <summary>
        /// 获取或设置是否高清
        /// </summary>
        public bool IsAntiAlias
        {
            get { return m_isAntiAlias; }
            set { m_isAntiAlias = value; }
        }

        internal bool m_isVisible;

        /// <summary>
        /// 获取或设置是否可见
        /// </summary>
        public bool IsVisible
        {
            get { return m_isVisible; }
            set { m_isVisible = value; }
        }

        internal DashStyle m_style;

        /// <summary>
        /// 获取或设置虚线样式
        /// </summary>
        public DashStyle Style
        {
            get { return m_style; }
            set { m_style = value; }
        }

        internal float m_width;

        /// <summary>
        /// 获取或设置宽度
        /// </summary>
        public float Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        private Pen pen;

        /// <summary>
        /// 获取画笔
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <returns>画笔</returns>
        public Pen GetPen(PaneBase pane, float scaleFactor)
        {
            return GetPen(pane, scaleFactor, null);
        }

        /// <summary>
        /// 获取画笔
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="dataValue">点</param>
        /// <returns>画笔</returns>
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
