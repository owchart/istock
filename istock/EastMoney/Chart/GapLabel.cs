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
    /// �����ǩ
    /// </summary>
    [Serializable]
    public class GapLabel : Label
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
        public GapLabel(String text, String fontFamily, float fontSize, Color color, bool isBold,
                                bool isItalic, bool isUnderline)
            : base(text, fontFamily, fontSize, color, isBold, isItalic, isUnderline)
        {
            m_gap = Default.Gap;
        }

        /// <summary>
        /// ������ǩ
        /// </summary>
        /// <param name="rhs">������ǩ</param>
        public GapLabel(GapLabel rhs)
            : base(rhs)
        {
            m_gap = rhs.m_gap;
        }

        /// <summary>
        /// Ĭ������
        /// </summary>
        public struct Default
        {
            public static float Gap = 0.3f;
        }

        internal float m_gap;

        /// <summary>
        /// ��ȡ�����ü��
        /// </summary>
        public float Gap
        {
            get { return m_gap; }
            set { m_gap = value; }
        }

        /// <summary>
        /// ���ٷ���
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
        }

        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <param name="scaleFactor">�̶�����</param>
        /// <returns>����</returns>
        public float GetScaledGap(float scaleFactor)
        {
            return m_fontSpec.GetHeight(scaleFactor) * m_gap;
        }
        #endregion
    }
}
