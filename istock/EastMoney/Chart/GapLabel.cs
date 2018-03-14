/*****************************************************************************\
*                                                                             *
* GapLabel.cs -   GapLabel functions, types, and definitions                  *
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
    /// 间隔标签
    /// </summary>
    [Serializable]
    public class GapLabel : Label
    {
        #region 陶德 2016/6/4
        /// <summary>
        /// 创建标签
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="fontFamily">字体</param>
        /// <param name="fontSize">文字大小</param>
        /// <param name="color">颜色</param>
        /// <param name="isBold">是否粗体</param>
        /// <param name="isItalic">是否斜体</param>
        /// <param name="isUnderline">是否有下划线</param>
        public GapLabel(String text, String fontFamily, float fontSize, Color color, bool isBold,
                                bool isItalic, bool isUnderline)
            : base(text, fontFamily, fontSize, color, isBold, isItalic, isUnderline)
        {
            m_gap = Default.Gap;
        }

        /// <summary>
        /// 创建标签
        /// </summary>
        /// <param name="rhs">其他标签</param>
        public GapLabel(GapLabel rhs)
            : base(rhs)
        {
            m_gap = rhs.m_gap;
        }

        /// <summary>
        /// 默认属性
        /// </summary>
        public struct Default
        {
            public static float Gap = 0.3f;
        }

        internal float m_gap;

        /// <summary>
        /// 获取或设置间隔
        /// </summary>
        public float Gap
        {
            get { return m_gap; }
            set { m_gap = value; }
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
        }

        /// <summary>
        /// 获取间隔步长
        /// </summary>
        /// <param name="scaleFactor">刻度因子</param>
        /// <returns>步长</returns>
        public float GetScaledGap(float scaleFactor)
        {
            return m_fontSpec.GetHeight(scaleFactor) * m_gap;
        }
        #endregion
    }
}
