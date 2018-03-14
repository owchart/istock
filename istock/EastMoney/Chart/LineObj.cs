/*****************************************************************************\
*                                                                             *
* LineObj.cs -  LineObj functions, types, and definitions                     *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
******************************************************************************/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OwLib
{
    /// <summary>
    /// ��
    /// </summary>
    [Serializable]
    public class LineObj : GraphObjLine
    {
        #region �յ� 2016/6/7
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="color">��ɫ</param>
        /// <param name="x1">������ֵ1</param>
        /// <param name="y1">������ֵ1</param>
        /// <param name="x2">������ֵ2</param>
        /// <param name="y2">������ֵ2</param>
        public LineObj(Color color, double x1, double y1, double x2, double y2)
            : base(x1, y1, x2 - x1, y2 - y1)
        {
            m_line = new LineBase(color);
            this.Location.AlignH = AlignH.Left;
            this.Location.AlignV = AlignV.Top;
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="x1">������ֵ1</param>
        /// <param name="y1">������ֵ1</param>
        /// <param name="x2">������ֵ2</param>
        /// <param name="y2">������ֵ2</param>
        public LineObj(double x1, double y1, double x2, double y2)
            : this(LineBase.Default.Color, x1, y1, x2, y2)
        {
        }

        /// <summary>
        /// ������
        /// </summary>
        public LineObj()
            : this(LineBase.Default.Color, 0, 0, 1, 1)
        {
        }

        /// <summary>
        /// ������
        /// </summary>
        /// <param name="rhs">��������</param>
        public LineObj(LineObj rhs)
            : base(rhs)
        {
            m_line = new LineBase(rhs.m_line);
        }

        protected LineBase m_line;

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        public LineBase Line
        {
            get { return m_line; }
            set { m_line = value; }
        }

        /// <summary>
        /// ��ͼ����
        /// </summary>
        /// <param name="g">��ͼ����</param>
        /// <param name="pane">ͼ��</param>
        /// <param name="scaleFactor">�̶�����</param>
        public override void Draw(Graphics g, PaneBase pane, float scaleFactor)
        {
            PointF pix1 = this.Location.TransformTopLeft(pane);
            PointF pix2 = this.Location.TransformBottomRight(pane);
            if (pix1.X > -10000 && pix1.X < 100000 && pix1.Y > -100000 && pix1.Y < 100000 &&
                pix2.X > -10000 && pix2.X < 100000 && pix2.Y > -100000 && pix2.Y < 100000)
            {
                double dy = pix2.Y - pix1.Y;
                double dx = pix2.X - pix1.X;
                float angle = this.angle = (float)Math.Atan2(dy, dx) * 180.0F / (float)Math.PI;
                if (pen == null)
                {
                    pen = m_line.GetPen(pane, scaleFactor);
                }
                SmoothingMode sm = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.HighQuality;
                {
                    if (gp == null)
                    {
                        gp = new GraphicsPath();
                    }
                    gp.Reset();
                    gp.AddLine(pix1.X, pix1.Y, pix2.X, pix2.Y);
                    g.DrawPath(pen, gp);
                }
                DrawControler(g, pix1.X, pix1.Y, pix2.X, pix2.Y);
                g.SmoothingMode = sm;
                this.graphPane = (GraphPane)pane;
                this.pix1 = pix1;
                this.pix2 = pix2;
            }
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="pane">ͼ��</param>
        /// <param name="g">��ͼ����</param>
        /// <param name="scaleFactor">�̶�����</param>
        /// <param name="shape">ͼ��</param>
        /// <param name="coords">����</param>
        public override void GetCoords(PaneBase pane, Graphics g, float scaleFactor,
        out String shape, out String coords)
        {
            RectangleF pixRect = m_location.TransformRect(pane);
            Matrix matrix = new Matrix();
            if (pixRect.Right == 0)
                pixRect.Width = 1;
            float angle = (float)Math.Atan((pixRect.Top - pixRect.Bottom) /
                    (pixRect.Left - pixRect.Right));
            matrix.Rotate(angle, MatrixOrder.Prepend);
            matrix.Translate(-pixRect.Left, -pixRect.Top, MatrixOrder.Prepend);
            PointF[] pts = new PointF[4];
            pts[0] = new PointF(0, 3);
            pts[1] = new PointF(pixRect.Width, 3);
            pts[2] = new PointF(pixRect.Width, -3);
            pts[3] = new PointF(0, -3);
            matrix.TransformPoints(pts);
            shape = "poly";
            coords = String.Format("{0:f0},{1:f0},{2:f0},{3:f0},{4:f0},{5:f0},{6:f0},{7:f0},",
                        pts[0].X, pts[0].Y, pts[1].X, pts[1].Y,
                        pts[2].X, pts[2].Y, pts[3].X, pts[3].Y);
        }

        /// <summary>
        /// �ж��Ƿ������ڵĵ�
        /// </summary>
        /// <param name="pt">����</param>
        /// <param name="pane">ͼ��</param>
        /// <param name="g">��ͼ����</param>
        /// <param name="scaleFactor">�̶�����</param>
        /// <returns>�Ƿ������ڵĵ�</returns>
        public override bool PointInBox(PointF pt, PaneBase pane, Graphics g, float scaleFactor)
        {
            if (!base.PointInBox(pt, pane, g, scaleFactor))
                return false;
            PointF pix = m_location.TransformTopLeft(pane);
            PointF pix2 = m_location.TransformBottomRight(pane);
            using (Pen pen = new Pen(Color.FromArgb(30, 30, 30), (float)GraphPane.Default.NearestTol * 2.0F))
            {
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddLine(pix, pix2);
                    return path.IsOutlineVisible(pt, pen);
                }
            }
        }
        #endregion
    }
}
