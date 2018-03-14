/*****************************************************************************\
*                                                                             *
* RotateButton.cs -   RotateButton functions, types, and definitions          *
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
    /// 旋转按钮
    /// </summary>
    public class RotateButton : ControlerButton
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建旋转按钮
        /// </summary>
        /// <param name="rect">区域</param>
        /// <param name="p1">坐标1</param>
        /// <param name="p2">坐标2</param>
        /// <param name="buttonType">按钮类型</param>
        /// <param name="owner">所属对象</param>
        public RotateButton(RectangleF rect, PointF p1, PointF p2, ButtonType buttonType, GraphObj owner)
        {
            this.rect = rect;
            this.LineStart = p1;
            this.LineEnd = p2;
            this.buttonType = buttonType;
            this.owner = owner;
        }

        public static float Diameter = 8;
        public PointF LineEnd;
        public static float LineLength = 15;
        public PointF LineStart;

        /// <summary>
        /// 绘图
        /// </summary>
        /// <param name="g">绘图对象</param>
        public override void Draw(System.Drawing.Graphics g)
        {
            using (Pen pen = new Pen(ColorObj.DefaultColors[0], 0.3F))
            {
                g.DrawEllipse(pen, rect);
                using (Brush brush = new LinearGradientBrush(this.rect, Color.FromArgb(196, 255, 146), Color.FromArgb(133, 225, 55), LinearGradientMode.Vertical))
                {
                    g.FillEllipse(brush, rect);
                }
                g.DrawLine(pen, LineStart, LineEnd);
            }
        }
        #endregion
    }
}
