/*****************************************************************************\
*                                                                             *
* Chart.cs -     Chart functions, types, and definitions                      *
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
    /// 图表
    /// </summary>
    [Serializable]
    public class Chart : IDisposable
    {
        #region 陶德 2016/5/31
        /// <summary>
        /// 创建图表
        /// </summary>
        public Chart()
        {
            m_isRectAuto = true;
            m_border = new Border(Default.IsBorderVisible, Default.BorderColor, Default.BorderPenWidth);
            m_fill = new Fill(Default.FillColor, Default.FillBrush, Default.FillType);
        }

        /// <summary>
        /// 创建图表
        /// </summary>
        /// <param name="rhs">其他对象</param>
        public Chart(Chart rhs)
        {
            m_border = new Border(rhs.m_border);
            m_fill = new Fill(rhs.m_fill);
            m_rect = rhs.m_rect;
            m_isRectAuto = rhs.m_isRectAuto;
        }

        /// <summary>
        /// 默认属性
        /// </summary>
        public struct Default
        {
            public static Color BorderColor = Color.FromArgb(30, 30, 30);
            public static Color FillColor = Color.White;
            public static Brush FillBrush = null;
            public static FillType FillType = FillType.Brush;
            public static float BorderPenWidth = 1F;
            public static bool IsBorderVisible = true;
            public static int SymbolSize = 7;
            public static int MaxPieSliceCount = 50;
        }

        internal Border m_border;

        /// <summary>
        /// 获取或设置边框
        /// </summary>
        public Border Border
        {
            get { return m_border; }
            set { m_border = value; }
        }

        internal Fill m_fill;

        /// <summary>
        /// 获取或设置填充
        /// </summary>
        public Fill Fill
        {
            get { return m_fill; }
            set { m_fill = value; }
        }

        internal bool m_isRectAuto;

        /// <summary>
        /// 获取或设置是否自动矩形
        /// </summary>
        public bool IsRectAuto
        {
            get { return m_isRectAuto; }
            set { m_isRectAuto = value; }
        }

        internal RectangleF m_rect;

        /// <summary>
        /// 获取或设置区域
        /// </summary>
        public RectangleF Rect
        {
            get { return m_rect; }
            set { m_rect = value; m_isRectAuto = false; }
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public void Dispose()
        {
            if (m_fill != null)
                m_fill.Dispose();
        }
        #endregion
    }
}
