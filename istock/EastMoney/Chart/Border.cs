/*****************************************************************************\
*                                                                             *
* Border.cs -     Border functions, types, and definitions                    *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OwLib
{
    /// <summary>
    /// �߿�
    /// </summary>
    [Serializable]
    public class Border : LineBase
    {
        #region �յ� 2016/5/31
        /// <summary>
        /// �����߿�
        /// </summary>
        public Border()
            : base()
        {
            m_inflateFactor = Default.InflateFactor;
        }

        /// <summary>
        /// �����߿�
        /// </summary>
        /// <param name="isVisible">�Ƿ�ɼ�</param>
        /// <param name="color">��ɫ</param>
        /// <param name="width">���</param>
        public Border(bool isVisible, Color color, float width)
            :
              base(color)
        {
            m_width = width;
            m_isVisible = isVisible;
        }

        /// <summary>
        /// �����߿�
        /// </summary>
        /// <param name="color">��ɫ</param>
        /// <param name="width">���</param>
        public Border(Color color, float width)
            :
                this(!color.IsEmpty, color, width)
        {
        }

        /// <summary>
        /// �����߿�
        /// </summary>
        /// <param name="rhs">�����߿�</param>
        public Border(Border rhs)
            : base(rhs)
        {
            m_inflateFactor = rhs.m_inflateFactor;
        }

        /// <summary>
        /// Ĭ������
        /// </summary>
        public new struct Default
        {
            public static float InflateFactor = 0.0F;
        }

        private float m_inflateFactor;

        /// <summary>
        /// ��ȡ����������ϵ��
        /// </summary>
        public float InflateFactor
        {
            get { return m_inflateFactor; }
            set { m_inflateFactor = value; }
        }

        /// <summary>
        /// ��ͼ����
        /// </summary>
        /// <param name="g">��ͼ����</param>
        /// <param name="pane">ͼ��</param>
        /// <param name="scaleFactor">�̶�����</param>
        /// <param name="rect">����</param>
        public void Draw(Graphics g, PaneBase pane, float scaleFactor, RectangleF rect)
        {
            if (m_isVisible)
            {
                RectangleF tRect = rect;
                float scaledInflate = (float)(m_inflateFactor * scaleFactor);
                tRect.Inflate(scaledInflate, scaledInflate);
                Pen pen = GetPen(pane, scaleFactor);
                g.DrawRectangle(pen, (int)tRect.X, (int)tRect.Y, (int)tRect.Width, (int)tRect.Height);
            }
        }

        /// <summary>
        /// ����Բ�߿�
        /// </summary>
        /// <param name="g">��ͼ����</param>
        /// <param name="pane">ͼ��</param>
        /// <param name="scaleFactor">�̶�����</param>
        /// <param name="rect">����</param>
        public void DrawRoundRec(Graphics g, PaneBase pane, float scaleFactor, RectangleF rect)
        {
            if (m_isVisible)
            {
                RectangleF tRect = rect;
                float scaledInflate = (float)(m_inflateFactor * scaleFactor);
                tRect.Inflate(scaledInflate, scaledInflate);
                Pen pen = GetPen(pane, scaleFactor);
                GraphicsPath gp = CustomerShape.CreateMyRoundRectPath(rect);
                g.DrawPath(pen, gp);
                gp.Dispose();
            }
        }
        #endregion
    }
}
