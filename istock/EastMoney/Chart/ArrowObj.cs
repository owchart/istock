/*****************************************************************************\
*                                                                             *
* ArrowObj.cs -   Arrow functions, types, and definitions                     *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OwLib
{
    /// <summary>
    /// 箭头对象
    /// </summary>
    [Serializable]
    public class ArrowObj : LineObj
    {
        #region 陶德 2016/5/31
        /// <summary>
        /// 创建箭头
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="size">尺寸</param>
        /// <param name="x1">第一个点的横坐标</param>
        /// <param name="y1">第一个点的纵坐标</param>
        /// <param name="x2">第二个点的横坐标</param>
        /// <param name="y2">第二个点的纵坐标</param>
        public ArrowObj(Color color, float size, double x1, double y1,
                    double x2, double y2)
            : base(color, x1, y1, x2, y2)
        {
            m_isArrowHead = Default.IsArrowHead;
            m_size = size;
        }

        /// <summary>
        /// 创建箭头
        /// </summary>
        /// <param name="rhs">其他对象</param>
        public ArrowObj(ArrowObj rhs)
            : base(rhs)
        {
            m_size = rhs.Size;
            m_isArrowHead = rhs.IsArrowHead;
        }

        /// <summary>
        /// 默认属性
        /// </summary>
        public new struct Default
        {
            public static float Size = 12.0F;
            public static bool IsArrowHead = true;
        }

        private bool m_isArrowHead;

        /// <summary>
        /// 获取或设置是否有箭头的头部
        /// </summary>
        public bool IsArrowHead
        {
            get { return m_isArrowHead; }
            set { m_isArrowHead = value; }
        }

        private float m_size;

        /// <summary>
        /// 获取或设置尺寸
        /// </summary>
        public float Size
        {
            get { return m_size; }
            set { m_size = value; }
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">框架</param>
        /// <param name="scaleFactor">刻度因子</param>
        public override void Draw(Graphics g, PaneBase pane, float scaleFactor)
        {
            PointF pix1 = this.Location.TransformTopLeft(pane);
            PointF pix2 = this.Location.TransformBottomRight(pane);
            if (pix1.X > -10000 && pix1.X < 100000 && pix1.Y > -100000 && pix1.Y < 100000 &&
                pix2.X > -10000 && pix2.X < 100000 && pix2.Y > -100000 && pix2.Y < 100000)
            {
                Pen pen = m_line.GetPen(pane, scaleFactor);
                float scaledSize = (float)(m_size * scaleFactor);
                SmoothingMode sm = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.HighQuality;
                if (gp == null)
                {
                    gp = new GraphicsPath();
                }
                gp.Reset();
                gp.AddLine(pix1, pix2);
                g.DrawPath(pen, gp);
                if (m_isArrowHead)
                {
                    double dy = pix2.Y - pix1.Y;
                    double dx = pix2.X - pix1.X;
                    float angle = this.angle = (float)Math.Atan2(dy, dx) * 180.0F / (float)Math.PI;
                    float length = (float)Math.Sqrt(dx * dx + dy * dy);
                    Matrix transform = g.Transform;
                    g.TranslateTransform(pix1.X, pix1.Y);
                    g.RotateTransform(angle);
                    PointF[] polyPt = new PointF[4];
                    float hsize = scaledSize / 3.0F;
                    polyPt[0].X = length;
                    polyPt[0].Y = 0;
                    polyPt[1].X = length - scaledSize;
                    polyPt[1].Y = hsize;
                    polyPt[2].X = length - scaledSize;
                    polyPt[2].Y = -hsize;
                    polyPt[3] = polyPt[0];
                    using (SolidBrush brush = new SolidBrush(m_line.m_color))
                        g.FillPolygon(brush, polyPt);
                    g.Transform = transform;
                }
                DrawControler(g, pix1.X, pix1.Y, pix2.X, pix2.Y);
                g.SmoothingMode = sm;
                this.graphPane = (GraphPane)pane;
                this.pix1 = pix1;
                this.pix2 = pix2;
            }
        }
        #endregion
    }
}
