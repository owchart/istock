/*****************************************************************************\
*                                                                             *
* Label.cs -    Label functions, types, and definitions                       *
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
    /// 标签
    /// </summary>
    [Serializable]
    public class Label : IDisposable
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
        public Label(String text, String fontFamily, float fontSize, Color color, bool isBold,
    bool isItalic, bool isUnderline)
        {
            m_text = (text == null) ? String.Empty : text;
            m_fontSpec = new FontSpec(fontFamily, fontSize, color, isBold, isItalic, isUnderline);
            m_isVisible = true;
        }

        /// <summary>
        /// 创建标签
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="fontSpec">字体</param>
        public Label(String text, FontSpec fontSpec)
        {
            m_text = (text == null) ? String.Empty : text;
            m_fontSpec = fontSpec;
            m_isVisible = true;
        }

        /// <summary>
        /// 创建标签
        /// </summary>
        /// <param name="rhs">其他对象</param>
        public Label(Label rhs)
        {
            if (rhs.m_text != null)
                m_text = (String)rhs.m_text.Clone();
            else
                m_text = String.Empty;
            m_isVisible = rhs.m_isVisible;
            if (rhs.m_fontSpec != null)
                m_fontSpec = new FontSpec(rhs.m_fontSpec);
            else
                m_fontSpec = null;
        }

        internal FontSpec m_fontSpec;

        /// <summary>
        /// 获取或设置字体
        /// </summary>
        public FontSpec FontSpec
        {
            get { return m_fontSpec; }
            set { m_fontSpec = value; }
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

        internal String m_text;

        /// <summary>
        /// 获取或设置文字
        /// </summary>
        public String Text
        {
            get { return m_text; }
            set { m_text = value; }
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public virtual void Dispose()
        {
            m_fontSpec.Dispose();
        }
        #endregion
    }
}
