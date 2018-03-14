/*****************************************************************************\
*                                                                             *
* MajorGrid.cs -  MajorGrid functions, types, and definitions                 *
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
    /// 主网格
    /// </summary>
    [Serializable]
    public class MajorGrid : MinorGrid
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建主网格
        /// </summary>
        public MajorGrid()
        {
            m_dashOn = Default.DashOn;
            m_dashOff = Default.DashOff;
            m_penWidth = Default.PenWidth;
            m_isVisible = Default.IsVisible;
            m_color = Default.Color;
            m_isZeroLine = Default.IsZeroLine;
        }

        /// <summary>
        /// 创建主网格
        /// </summary>
        /// <param name="rhs">其他对象</param>
        public MajorGrid(MajorGrid rhs)
            : base(rhs)
        {
            m_isZeroLine = rhs.m_isZeroLine;
        }

        /// <summary>
        /// 默认属性
        /// </summary>
        public new struct Default
        {
            public static float DashOn = 1.0F;
            public static float DashOff = 5.0F;
            public static float PenWidth = 1.0F;
            public static Color Color = Color.FromArgb(30, 30, 30);
            public static bool IsVisible = false;
            public static bool IsZeroLine = false;
        }

        internal bool m_isZeroLine;

        /// <summary>
        /// 获取或设置是否0线
        /// </summary>
        public bool IsZeroLine
        {
            get { return m_isZeroLine; }
            set { m_isZeroLine = value; }
        }
        #endregion
    }
}
