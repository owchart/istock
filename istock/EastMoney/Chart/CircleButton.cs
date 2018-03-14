/*****************************************************************************\
*                                                                             *
* CircleButton.cs -  CircleButton functions, types, and definitions           *
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
    /// 圆型按钮
    /// </summary>
    public class CircleButton : ControlerButton
    {
        #region 陶德 2016/5/31
        /// <summary>
        /// 创建圆型按钮
        /// </summary>
        /// <param name="rect">区域</param>
        /// <param name="buttonType">按钮类型</param>
        /// <param name="owner">包含者</param>
        public CircleButton(RectangleF rect, ButtonType buttonType, GraphObj owner)
        {
            this.rect = rect;
            this.buttonType = buttonType;
            this.owner = owner;
        }

        /// <summary>
        /// 按钮直径
        /// </summary>
        public static float buttonDiameter = 8;

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="g">绘图对象</param>
        public override void Draw(System.Drawing.Graphics g)
        {
            using (Pen pen = new Pen(ColorObj.DefaultColors[0], 0.3F))
            {
                g.DrawEllipse(pen, this.rect);
                using (Brush brush = new LinearGradientBrush(this.rect, Color.FromArgb(246, 255, 255), Color.FromArgb(202, 234, 237), LinearGradientMode.Vertical))
                    g.FillEllipse(brush, this.rect);
            }
        }
        #endregion
    }
}
