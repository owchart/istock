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
    /// ��ǩ
    /// </summary>
    [Serializable]
    public class Label : IDisposable
    {
        #region �յ� 2016/6/4
        /// <summary>
        /// ������ǩ
        /// </summary>
        /// <param name="text">����</param>
        /// <param name="fontFamily">����</param>
        /// <param name="fontSize">���ִ�С</param>
        /// <param name="color">��ɫ</param>
        /// <param name="isBold">�Ƿ����</param>
        /// <param name="isItalic">�Ƿ�б��</param>
        /// <param name="isUnderline">�Ƿ����»���</param>
        public Label(String text, String fontFamily, float fontSize, Color color, bool isBold,
    bool isItalic, bool isUnderline)
        {
            m_text = (text == null) ? String.Empty : text;
            m_fontSpec = new FontSpec(fontFamily, fontSize, color, isBold, isItalic, isUnderline);
            m_isVisible = true;
        }

        /// <summary>
        /// ������ǩ
        /// </summary>
        /// <param name="text">����</param>
        /// <param name="fontSpec">����</param>
        public Label(String text, FontSpec fontSpec)
        {
            m_text = (text == null) ? String.Empty : text;
            m_fontSpec = fontSpec;
            m_isVisible = true;
        }

        /// <summary>
        /// ������ǩ
        /// </summary>
        /// <param name="rhs">��������</param>
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
        /// ��ȡ����������
        /// </summary>
        public FontSpec FontSpec
        {
            get { return m_fontSpec; }
            set { m_fontSpec = value; }
        }

        internal bool m_isVisible;

        /// <summary>
        /// ��ȡ�������Ƿ�ɼ�
        /// </summary>
        public bool IsVisible
        {
            get { return m_isVisible; }
            set { m_isVisible = value; }
        }

        internal String m_text;

        /// <summary>
        /// ��ȡ����������
        /// </summary>
        public String Text
        {
            get { return m_text; }
            set { m_text = value; }
        }

        /// <summary>
        /// ���ٶ���
        /// </summary>
        public virtual void Dispose()
        {
            m_fontSpec.Dispose();
        }
        #endregion
    }
}
