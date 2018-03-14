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
    /// �������ǩ
    /// </summary>
    [Serializable]
    public class AxisLabel : GapLabel
    {
        #region �յ� 2016/5/31
        /// <summary>
        /// ������ǩ
        /// </summary>
        /// <param name="text">����</param>
        /// <param name="fontFamily">����</param>
        /// <param name="fontSize">���ִ�С</param>
        /// <param name="color">��ɫ</param>
        /// <param name="isBold">�Ƿ����</param>
        /// <param name="isItalic">�Ƿ�б��</param>
        /// <param name="isUnderline">�Ƿ��»���</param>
        public AxisLabel(String text, String fontFamily, float fontSize, Color color, bool isBold,
                        bool isItalic, bool isUnderline)
            :
    base(text, fontFamily, fontSize, color, isBold, isItalic, isUnderline)
        {
            m_isOmitMag = false;
            m_isTitleAtCross = true;
        }

        /// <summary>
        /// ������ǩ
        /// </summary>
        /// <param name="rhs">������ǩ</param>
        public AxisLabel(AxisLabel rhs)
            : base(rhs)
        {
            m_isOmitMag = rhs.m_isOmitMag;
            m_isTitleAtCross = rhs.m_isTitleAtCross;
        }

        internal bool m_isOmitMag;

        /// <summary>
        /// ��ȡ�������Ƿ�ʡ�Ը���
        /// </summary>
        public bool IsOmitMag
        {
            get { return m_isOmitMag; }
            set { m_isOmitMag = value; }
        }

        internal bool m_isTitleAtCross;

        /// <summary>
        /// ��ȡ�������Ƿ��ཻ��ı���
        /// </summary>
        public bool IsTitleAtCross
        {
            get { return m_isTitleAtCross; }
            set { m_isTitleAtCross = value; }
        }
        #endregion
    }
}
