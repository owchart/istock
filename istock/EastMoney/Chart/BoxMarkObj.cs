/*****************************************************************************\
*                                                                             *
* BoxMarkObj.cs -  Box mark functions, types, and definitions                 *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace OwLib
{
    /// <summary>
    /// 箱型标记
    /// </summary>
    public class BoxMarkObj : BoxObj
    {
        #region 陶德 2016/5/31
        /// <summary>
        /// 创建箱型标记
        /// </summary>
        /// <param name="x">X值</param>
        /// <param name="y">Y值</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="borderColor">边线颜色</param>
        /// <param name="fillColor">填充色</param>
        /// <param name="mousePointFixed">鼠标位置</param>
        public BoxMarkObj(double x, double y, double width, double height, Color borderColor, Color fillColor, PointF mousePointFixed)
            : base(x, y, width, height, borderColor, fillColor)
        {
            this.mousePointFixed = mousePointFixed;
        }
        
        /// <summary>
        /// 点集
        /// </summary>
        private PointF[] points = new PointF[7];

        /// <summary>
        /// 鼠标位置
        /// </summary>
        private PointF mousePointFixed;

        /// <summary>
        /// 屏幕位置
        /// </summary>
        public PointF screenPointFixed = new PointF(10000, 10000);

        /// <summary>
        /// 计算点
        /// </summary>
        /// <param name="pixRect">区域</param>
        /// <param name="screenPointFixed">屏幕坐标</param>
        /// <param name="points">点集</param>
        public void CalcPoints(RectangleF pixRect, PointF screenPointFixed, ref PointF[] points)
        {
            PointF p1, p2, p3, p4, p5, p6, p7;
            PointF center = new PointF(pixRect.X + pixRect.Width * .5F, pixRect.Y + pixRect.Height * .5F);
            bool smallAngle = Math.Abs(screenPointFixed.Y - center.Y) / Math.Abs(screenPointFixed.X - center.X) < Math.Tan(45 * Math.PI / 180) ? true : false;
            p1 = new PointF(pixRect.X, pixRect.Y);
            if (screenPointFixed.X < center.X)
            {
                if (screenPointFixed.Y < center.Y)
                {
                    if (smallAngle)
                    {
                        p2 = new PointF(pixRect.X, pixRect.Y + pixRect.Height * .2F);
                        p3 = screenPointFixed;
                        p4 = new PointF(pixRect.X, pixRect.Y + pixRect.Height * .45F);
                        p5 = new PointF(pixRect.X, pixRect.Bottom);
                        p6 = new PointF(pixRect.Right, pixRect.Bottom);
                        p7 = new PointF(pixRect.Right, pixRect.Y);
                    }
                    else
                    {
                        p2 = new PointF(pixRect.X, pixRect.Bottom);
                        p3 = new PointF(pixRect.Right, pixRect.Bottom);
                        p4 = new PointF(pixRect.Right, pixRect.Y);
                        p5 = new PointF(pixRect.X + pixRect.Width * .45F, pixRect.Y);
                        p6 = screenPointFixed;
                        p7 = new PointF(pixRect.X + pixRect.Width * .2F, pixRect.Y);
                    }
                }
                else
                {
                    if (smallAngle)
                    {
                        p2 = new PointF(pixRect.X, pixRect.Y + pixRect.Height * .55F);
                        p3 = screenPointFixed;
                        p4 = new PointF(pixRect.X, pixRect.Y + pixRect.Height * .8F);
                        p5 = new PointF(pixRect.X, pixRect.Bottom);
                        p6 = new PointF(pixRect.Right, pixRect.Bottom);
                        p7 = new PointF(pixRect.Right, pixRect.Y);
                    }
                    else
                    {
                        p2 = new PointF(pixRect.X, pixRect.Bottom);
                        p3 = new PointF(pixRect.X + pixRect.Width * .2F, pixRect.Bottom);
                        p4 = screenPointFixed;
                        p5 = new PointF(pixRect.X + pixRect.Width * .45F, pixRect.Bottom);
                        p6 = new PointF(pixRect.Right, pixRect.Bottom);
                        p7 = new PointF(pixRect.Right, pixRect.Y);
                    }
                }
            }
            else
            {
                if (screenPointFixed.Y > center.Y)
                {
                    if (smallAngle)
                    {
                        p2 = new PointF(pixRect.X, pixRect.Bottom);
                        p3 = new PointF(pixRect.Right, pixRect.Bottom);
                        p4 = new PointF(pixRect.Right, pixRect.Y + pixRect.Height * .8F);
                        p5 = screenPointFixed;
                        p6 = new PointF(pixRect.Right, pixRect.Y + pixRect.Height * .55F);
                        p7 = new PointF(pixRect.Right, pixRect.Y);
                    }
                    else
                    {
                        p2 = new PointF(pixRect.X, pixRect.Bottom);
                        p3 = new PointF(pixRect.X + pixRect.Width * .55F, pixRect.Bottom);
                        p4 = screenPointFixed;
                        p5 = new PointF(pixRect.X + pixRect.Width * .8F, pixRect.Bottom);
                        p6 = new PointF(pixRect.Right, pixRect.Bottom);
                        p7 = new PointF(pixRect.Right, pixRect.Y);
                    }
                }
                else
                {
                    if (smallAngle)
                    {
                        p2 = new PointF(pixRect.X, pixRect.Bottom);
                        p3 = new PointF(pixRect.Right, pixRect.Bottom);
                        p4 = new PointF(pixRect.Right, pixRect.Y + pixRect.Height * .45F);
                        p5 = screenPointFixed;
                        p6 = new PointF(pixRect.Right, pixRect.Y + pixRect.Height * .2F);
                        p7 = new PointF(pixRect.Right, pixRect.Y);
                    }
                    else
                    {
                        p2 = new PointF(pixRect.X, pixRect.Bottom);
                        p3 = new PointF(pixRect.Right, pixRect.Bottom);
                        p4 = new PointF(pixRect.Right, pixRect.Y);
                        p5 = new PointF(pixRect.X + pixRect.Width * .8F, pixRect.Y);
                        p6 = screenPointFixed;
                        p7 = new PointF(pixRect.X + pixRect.Width * .55F, pixRect.Y);
                    }
                }
            }
            points[0] = p1;
            points[1] = p2;
            points[2] = p3;
            points[3] = p4;
            points[4] = p5;
            points[5] = p6;
            points[6] = p7;
        }

        /// <summary>
        /// 是否包含坐标
        /// </summary>
        /// <param name="mousePt">坐标</param>
        /// <returns>是否包含</returns>
        internal override bool Contains(Point mousePt)
        {
            if (this.rect.Contains(screenPointFixed))
            {
                return base.Contains(mousePt);
            }
            else
            {
                if (this.rect.Contains(mousePt))
                {
                    return true;
                }
                else if (this.Focused)
                {
                    return (this.buttonList.Contains(mousePt));
                }
                else return false;
            }
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        public override void Draw(Graphics g, PaneBase pane, float scaleFactor)
        {
            RectangleF pixRect = this.Location.TransformRect(pane);
            RectangleF tmpRect = pane.Rect;
            tmpRect.Inflate(20, 20);
            pixRect.Intersect(tmpRect);
            if (screenPointFixed.X == 10000 || screenPointFixed.Y == 10000) 
            {
                screenPointFixed = Location.Transform(pane, mousePointFixed.X, mousePointFixed.Y, this.Location.CoordinateFrame);
            }
            if (!pixRect.Contains(screenPointFixed)) 
            {
                CalcPoints(pixRect, screenPointFixed, ref points);
            }
            if (Math.Abs(pixRect.Left) < 100000 &&
                    Math.Abs(pixRect.Top) < 100000 &&
                    Math.Abs(pixRect.Right) < 100000 &&
                    Math.Abs(pixRect.Bottom) < 100000)
            {
                if (pixRect.Contains(screenPointFixed))
                {
                    m_fill.Draw(g, pixRect);
                    m_border.Draw(g, pane, scaleFactor, pixRect);
                }
                else
                {
                    using (Pen pen = new Pen(this.Border.Color))
                    {
                        g.DrawPolygon(pen, points);
                    }
                    using (Brush brush = new SolidBrush(this.Fill.Color))
                    {
                        g.FillPolygon(brush, points);
                    }
                }
            }
            DrawControler(g, pixRect);
            DrawText(g, scaleFactor, pixRect, null);
            this.rect = pixRect;
            this.graphPane = (GraphPane)pane;
            DrawFixedControler(g);
        }

        /// <summary>
        /// 绘制控制器
        /// </summary>
        /// <param name="g">绘图对象</param>
        private void DrawFixedControler(Graphics g)
        {
            if (Focused)
            {
                float with = CircleButton.buttonDiameter;
                RectangleF rect = new RectangleF(screenPointFixed.X - with / 2, screenPointFixed.Y - with / 2, with, with);
                using (Brush brush = new SolidBrush(Color.FromArgb(252, 240, 26)))
                {
                    g.FillEllipse(brush, rect);
                }
                using (Pen pen = new Pen(ColorObj.DefaultColors[0], 0.3F))
                {
                    g.DrawEllipse(pen, rect);
                }
                CircleButton cb = new CircleButton(rect, ButtonType.Fixed, this);
                this.buttonList.Add(cb);
            }
        }
        #endregion
    }
}
