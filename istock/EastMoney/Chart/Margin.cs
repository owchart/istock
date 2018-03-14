/*****************************************************************************\
*                                                                             *
* Margin.cs -   Margin functions, types, and definitions                      *
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
    /// 边界
    /// </summary>
    [Serializable]
    public class Margin
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建边界
        /// </summary>
        public Margin()
        {
            m_left = Default.Left;
            m_right = Default.Right;
            m_top = Default.Top;
            m_bottom = Default.Bottom;
        }

        /// <summary>
        /// 创建边界
        /// </summary>
        /// <param name="rhs">其他对象</param>
        public Margin(Margin rhs)
        {
            m_left = rhs.m_left;
            m_right = rhs.m_right;
            m_top = rhs.m_top;
            m_bottom = rhs.m_bottom;
        }

        /// <summary>
        /// 默认属性
        /// </summary>
        public class Default
        {
            public static float Left = 10.0F;
            public static float Right = 10.0F;
            public static float Top = 10.0F;
            public static float Bottom = 10.0F;
        }

        /// <summary>
        /// 设置所有边界
        /// </summary>
        public float All
        {
            set
            {
                m_bottom = value;
                m_top = value;
                m_left = value;
                m_right = value;
            }
        }

        protected float m_bottom;

        /// <summary>
        /// 获取或设置底部边界
        /// </summary>
        public float Bottom
        {
            get { return m_bottom; }
            set { m_bottom = value; }
        }

        protected float m_left;

        /// <summary>
        /// 获取或设置左侧边界
        /// </summary>
        public float Left
        {
            get { return m_left; }
            set { m_left = value; }
        }

        protected float m_right;

        /// <summary>
        /// 获取或设置右侧边界
        /// </summary>
        public float Right
        {
            get { return m_right; }
            set { m_right = value; }
        }

        protected float m_top;
        
        /// <summary>
        /// 获取或设置顶部边界
        /// </summary>
        public float Top
        {
            get { return m_top; }
            set { m_top = value; }
        }
        #endregion
    }
}
