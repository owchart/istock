/*****************************************************************************\
*                                                                             *
* MajorTic.cs -  MajorTic functions, types, and definitions                   *
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
    /// 主变动值
    /// </summary>
    [Serializable]
    public class MajorTic : MinorTic
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建主变动值
        /// </summary>
        public MajorTic()
        {
            m_size = Default.Size;
            m_color = Default.Color;
            m_penWidth = Default.PenWidth;
            this.IsOutside = Default.IsOutside;
            this.IsInside = Default.IsInside;
            this.IsOpposite = Default.IsOpposite;
            m_isCrossOutside = Default.IsCrossOutside;
            m_isCrossInside = Default.IsCrossInside;
            m_isBetweenLabels = false;
        }

        /// <summary>
        /// 创建主变动值
        /// </summary>
        /// <param name="rhs">其他对象</param>
        public MajorTic(MajorTic rhs)
            : base(rhs)
        {
            m_isBetweenLabels = rhs.m_isBetweenLabels;
        }

        /// <summary>
        /// 默认属性
        /// </summary>
        public new struct Default
        {
            public static float Size = 5;
            public static float PenWidth = 1.0F;
            public static bool IsOutside = true;
            public static bool IsInside = true;
            public static bool IsOpposite = true;
            public static bool IsCrossOutside = false;
            public static bool IsCrossInside = false;
            public static Color Color = Color.FromArgb(30, 30, 30);
        }

        internal bool m_isBetweenLabels;

        /// <summary>
        /// 获取或设置是否在标签之间
        /// </summary>
        public bool IsBetweenLabels
        {
            get { return m_isBetweenLabels; }
            set { m_isBetweenLabels = value; }
        }
        #endregion
    }
}
