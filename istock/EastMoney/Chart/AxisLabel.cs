/*****************************************************************************\
*                                                                             *
* AxisLabel.cs -  Axis label  functions, types, and definitions               *
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
    /// 坐标轴标签
    /// </summary>
    [Serializable]
    public class AxisLabel : GapLabel
    {
        #region 陶德 2016/5/31
        /// <summary>
        /// 创建标签
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="fontFamily">字体</param>
        /// <param name="fontSize">文字大小</param>
        /// <param name="color">颜色</param>
        /// <param name="isBold">是否粗体</param>
        /// <param name="isItalic">是否斜体</param>
        /// <param name="isUnderline">是否下划线</param>
        public AxisLabel(String text, String fontFamily, float fontSize, Color color, bool isBold,
                        bool isItalic, bool isUnderline)
            :
    base(text, fontFamily, fontSize, color, isBold, isItalic, isUnderline)
        {
            m_isOmitMag = false;
            m_isTitleAtCross = true;
        }

        /// <summary>
        /// 创建标签
        /// </summary>
        /// <param name="rhs">其他标签</param>
        public AxisLabel(AxisLabel rhs)
            : base(rhs)
        {
            m_isOmitMag = rhs.m_isOmitMag;
            m_isTitleAtCross = rhs.m_isTitleAtCross;
        }

        internal bool m_isOmitMag;

        /// <summary>
        /// 获取或设置是否省略复数
        /// </summary>
        public bool IsOmitMag
        {
            get { return m_isOmitMag; }
            set { m_isOmitMag = value; }
        }

        internal bool m_isTitleAtCross;

        /// <summary>
        /// 获取或设置是否相交点的标题
        /// </summary>
        public bool IsTitleAtCross
        {
            get { return m_isTitleAtCross; }
            set { m_isTitleAtCross = value; }
        }
        #endregion
    }
}
