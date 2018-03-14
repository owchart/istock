/*****************************************************************************\
*                                                                             *
* Location.cs -  Location functions, types, and definitions                   *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
******************************************************************************/

using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OwLib
{
    /// <summary>
    /// λ��
    /// </summary>
    [Serializable]
    public class Location
    {
        #region �յ� 2016/6/7
        /// <summary>
        /// ����λ��
        /// </summary>
        public Location()
            : this(0, 0, CoordType.ChartFraction)
        {
        }

        /// <summary>
        /// ����λ��
        /// </summary>
        /// <param name="x">������ֵ</param>
        /// <param name="y">������ֵ</param>
        /// <param name="coordType">����������</param>
        public Location(double x, double y, CoordType coordType)
            :
                this(x, y, coordType, AlignH.Left, AlignV.Top)
        {
        }

        /// <summary>
        /// ����λ��
        /// </summary>
        /// <param name="x">������ֵ</param>
        /// <param name="y">������ֵ</param>
        /// <param name="coordType">����������</param>
        /// <param name="alignH">�Ƿ�������</param>
        /// <param name="alignV">�Ƿ��������</param>
        public Location(double x, double y, CoordType coordType, AlignH alignH, AlignV alignV)
        {
            m_x = x;
            m_y = y;
            m_width = 0;
            m_height = 0;
            m_coordinateFrame = coordType;
            m_alignH = alignH;
            m_alignV = alignV;
        }

        /// <summary>
        /// ����λ��
        /// </summary>
        /// <param name="x">������ֵ</param>
        /// <param name="y">������ֵ</param>
        /// <param name="width">���</param>
        /// <param name="height">�߶�</param>
        /// <param name="coordType">����������</param>
        /// <param name="alignH">�Ƿ�������</param>
        /// <param name="alignV">�Ƿ��������</param>
        public Location(double x, double y, double width, double height,
            CoordType coordType, AlignH alignH, AlignV alignV)
            :
                this(x, y, coordType, alignH, alignV)
        {
            m_width = width;
            m_height = height;
        }

        /// <summary>
        /// ����λ��
        /// </summary>
        /// <param name="rhs">��������</param>
        public Location(Location rhs)
        {
            m_x = rhs.m_x;
            m_y = rhs.m_y;
            m_width = rhs.m_width;
            m_height = rhs.m_height;
            m_coordinateFrame = rhs.CoordinateFrame;
            m_alignH = rhs.AlignH;
            m_alignV = rhs.AlignV;
        }

        private AlignH m_alignH;

        /// <summary>
        /// ��ȡ�����ú������
        /// </summary>
        public AlignH AlignH
        {
            get { return m_alignH; }
            set { m_alignH = value; }
        }

        private AlignV m_alignV;

        /// <summary>
        /// ��ȡ�������������
        /// </summary>
        public AlignV AlignV
        {
            get { return m_alignV; }
            set { m_alignV = value; }
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        public PointF BottomRight
        {
            get { return new PointF((float)this.X2, (float)this.Y2); }
        }

        private CoordType m_coordinateFrame;

        /// <summary>
        /// ��ȡ��������������
        /// </summary>
        public CoordType CoordinateFrame
        {
            get { return m_coordinateFrame; }
            set { m_coordinateFrame = value; }
        }

        private double m_height;

        /// <summary>
        /// ��ȡ�����ø߶�
        /// </summary>
        public double Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        /// <summary>
        /// ��ȡ����������
        /// </summary>
        public RectangleF Rect
        {
            get { return new RectangleF((float)m_x, (float)m_y, (float)m_width, (float)m_height); }
            set
            {
                m_x = value.X;
                m_y = value.Y;
                m_width = value.Width;
                m_height = value.Height;
            }
        }

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        public PointF TopLeft
        {
            get { return new PointF((float)m_x, (float)m_y); }
            set { m_x = value.X; m_y = value.Y; }
        }

        private double m_width;

        /// <summary>
        /// ��ȡ�����ÿ��
        /// </summary>
        public double Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        private double m_x;

        /// <summary>
        /// ��ȡ�����ú�����
        /// </summary>
        public double X
        {
            get { return m_x; }
            set { m_x = value; }
        }

        /// <summary>
        /// ��ȡ�����ú�����1
        /// </summary>
        public double X1
        {
            get { return m_x; }
            set { m_x = value; }
        }

        public double X2
        {
            get { return m_x + m_width; }
        }

        private double m_y;

        /// <summary>
        /// ��ȡ������������
        /// </summary>
        public double Y
        {
            get { return m_y; }
            set { m_y = value; }
        }

        /// <summary>
        /// ��ȡ�����ú�����2
        /// </summary>
        public double Y1
        {
            get { return m_y; }
            set { m_y = value; }
        }

        /// <summary>
        /// ��ȡ�����ú�����3
        /// </summary>
        public double Y2
        {
            get { return m_y + m_height; }
        }

        /// <summary>
        /// ת����
        /// </summary>
        /// <param name="pane">ͼ��</param>
        /// <returns>ת����ĵ�</returns>
        public PointF Transform(PaneBase pane)
        {
            return Transform(pane, m_x, m_y,
                        m_coordinateFrame);
        }

        /// <summary>
        /// ת����
        /// </summary>
        /// <param name="pane">ͼ��</param>
        /// <param name="x">������ֵ</param>
        /// <param name="y">������ֵ</param>
        /// <param name="coord">����������</param>
        /// <returns>ת����ĵ�</returns>
        public static PointF Transform(PaneBase pane, double x, double y, CoordType coord)
        {
            return pane.TransformCoord(x, y, coord);
        }

        /// <summary>
        /// ת�����ϵ�
        /// </summary>
        /// <param name="pane">ͼ��</param>
        /// <param name="width">���</param>
        /// <param name="height">�߶�</param>
        /// <returns>ת����ĵ�</returns>
        public PointF TransformTopLeft(PaneBase pane, float width, float height)
        {
            PointF pt = Transform(pane);
            if (m_alignH == AlignH.Right)
                pt.X -= width;
            else if (m_alignH == AlignH.Center)
                pt.X -= width / 2.0F;
            if (m_alignV == AlignV.Bottom)
                pt.Y -= height;
            else if (m_alignV == AlignV.Center)
                pt.Y -= height / 2.0F;
            return pt;
        }

        /// <summary>
        /// ת�����ϵ�
        /// </summary>
        /// <param name="pane">ͼ��</param>
        /// <returns>ת����ĵ�</returns>
        public PointF TransformTopLeft(PaneBase pane)
        {
            return Transform(pane);
        }

        /// <summary>
        /// ת�����µ�
        /// </summary>
        /// <param name="pane">ͼ��</param>
        /// <returns>���µ�</returns>
        public PointF TransformBottomRight(PaneBase pane)
        {
            return Transform(pane, this.X2, this.Y2, m_coordinateFrame);
        }

        /// <summary>
        /// ת������
        /// </summary>
        /// <param name="pane">ͼ��</param>
        /// <returns>����</returns>
        public RectangleF TransformRect(PaneBase pane)
        {
            PointF pix1 = TransformTopLeft(pane);
            PointF pix2 = TransformBottomRight(pane);
            return new RectangleF(pix1.X, pix1.Y, Math.Abs(pix2.X - pix1.X), Math.Abs(pix2.Y - pix1.Y));
        }
        #endregion
    }
}
