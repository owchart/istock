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
    /// ͼ��
    /// </summary>
    [Serializable]
    public class Chart : IDisposable
    {
        #region �յ� 2016/5/31
        /// <summary>
        /// ����ͼ��
        /// </summary>
        public Chart()
        {
            m_isRectAuto = true;
            m_border = new Border(Default.IsBorderVisible, Default.BorderColor, Default.BorderPenWidth);
            m_fill = new Fill(Default.FillColor, Default.FillBrush, Default.FillType);
        }

        /// <summary>
        /// ����ͼ��
        /// </summary>
        /// <param name="rhs">��������</param>
        public Chart(Chart rhs)
        {
            m_border = new Border(rhs.m_border);
            m_fill = new Fill(rhs.m_fill);
            m_rect = rhs.m_rect;
            m_isRectAuto = rhs.m_isRectAuto;
        }

        /// <summary>
        /// Ĭ������
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
        /// ��ȡ�����ñ߿�
        /// </summary>
        public Border Border
        {
            get { return m_border; }
            set { m_border = value; }
        }

        internal Fill m_fill;

        /// <summary>
        /// ��ȡ���������
        /// </summary>
        public Fill Fill
        {
            get { return m_fill; }
            set { m_fill = value; }
        }

        internal bool m_isRectAuto;

        /// <summary>
        /// ��ȡ�������Ƿ��Զ�����
        /// </summary>
        public bool IsRectAuto
        {
            get { return m_isRectAuto; }
            set { m_isRectAuto = value; }
        }

        internal RectangleF m_rect;

        /// <summary>
        /// ��ȡ����������
        /// </summary>
        public RectangleF Rect
        {
            get { return m_rect; }
            set { m_rect = value; m_isRectAuto = false; }
        }

        /// <summary>
        /// ���ٷ���
        /// </summary>
        public void Dispose()
        {
            if (m_fill != null)
                m_fill.Dispose();
        }
        #endregion
    }
}
