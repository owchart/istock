/***************************************************************************\
*                                                                           *
* EllipseObj.cs -  EllipseObj functions, types, and definitions             *
*                                                                           *
*               Version 1.00                                                *
*                                                                           *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved. *
*                                                                           *
****************************************************************************/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Runtime.Serialization;
using System.Security.Permissions;
namespace OwLib
{
    /// <summary>
    /// 椭圆
    /// </summary>
    [Serializable]
    public class EllipseObj : BoxObj
    {
        #region 陶德 2016/5/31
        /// <summary>
        /// 创建椭圆
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public EllipseObj(double x, double y, double width, double height)
            : base(x, y, width, height)
        {
        }

        /// <summary>
        /// 创建椭圆
        /// </summary>
        public EllipseObj()
            : base()
        {
        }

        /// <summary>
        /// 创建椭圆
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="borderColor">边线色</param>
        /// <param name="fillColor">填充色</param>
        public EllipseObj(double x, double y, double width, double height, Color borderColor, Color fillColor)
            : base(x, y, width, height, borderColor, fillColor)
        {
        }

        /// <summary>
        /// 创建椭圆
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="borderColor">边线</param>
        /// <param name="fillColor1">填充色1</param>
        /// <param name="fillColor2">填充色2</param>
        public EllipseObj(double x, double y, double width, double height, Color borderColor,
                            Color fillColor1, Color fillColor2)
            :
                base(x, y, width, height, borderColor, fillColor1, fillColor2)
        {
        }

        /// <summary>
        /// 创建椭圆
        /// </summary>
        /// <param name="rhs">其他对象</param>
        public EllipseObj(BoxObj rhs)
            : base(rhs)
        {
        }

        /// <summary>
        /// 绘制方法
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        public override void Draw(Graphics g, PaneBase pane, float scaleFactor)
        {
            RectangleF pixRect = this.Location.TransformRect(pane);
            if (Math.Abs(pixRect.Left) < 100000 &&
                    Math.Abs(pixRect.Top) < 100000 &&
                    Math.Abs(pixRect.Right) < 100000 &&
                    Math.Abs(pixRect.Bottom) < 100000)
            {
                SmoothingMode TempSm = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.HighQuality;
                if (m_fill.IsVisible)
                    using (Brush brush = m_fill.MakeBrush(pixRect))
                        g.FillEllipse(brush, pixRect);
                if (m_border.IsVisible)
                {
                    Pen pen = m_border.GetPen(pane, scaleFactor);
                    g.DrawEllipse(pen, pixRect);
                }
                g.SmoothingMode = TempSm;
                DrawControler(g, pixRect);
                DrawText(g, scaleFactor, pixRect, null);
                this.rect = pixRect;
                this.graphPane = (GraphPane)pane;
            }
        }

        /// <summary>
        /// 判断点是否在椭圆上
        /// </summary>
        /// <param name="pt">坐标</param>
        /// <param name="pane">图层</param>
        /// <param name="g">绘图对象</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <returns>点是否在椭圆上</returns>
        public override bool PointInBox(PointF pt, PaneBase pane, Graphics g, float scaleFactor)
        {
            if (!base.PointInBox(pt, pane, g, scaleFactor))
                return false;
            RectangleF pixRect = m_location.TransformRect(pane);
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(pixRect);
                return path.IsVisible(pt);
            }
        }
        #endregion
    }
}
