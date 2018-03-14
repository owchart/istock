/*****************************************************************************\
*                                                                             *
* RectButton.cs -   RectButton functions, types, and definitions              *
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
    /// 矩形按钮
    /// </summary>
    public class RectButton : ControlerButton
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 
        /// </summary>
        public static float sideLength = 6;

        /// <summary>
        /// 创建矩形按钮
        /// </summary>
        /// <param name="rect">区域</param>
        /// <param name="buttonType">按钮类型</param>
        /// <param name="owner">所属对象</param>
        public RectButton(RectangleF rect, ButtonType buttonType, GraphObj owner)
        {
            this.rect = rect;
            this.buttonType = buttonType;
            this.owner = owner;
        }

        /// <summary>
        /// 绘图
        /// </summary>
        /// <param name="g">绘图对象</param>
        public override void Draw(System.Drawing.Graphics g)
        {
            using (Pen pen = new Pen(ColorObj.DefaultColors[0], 0.3F))
            {
                g.DrawRectangle(pen, (int)this.rect.X, (int)this.rect.Y, (int)this.rect.Width, (int)this.rect.Height);
                using (Brush brush = new LinearGradientBrush(this.rect, Color.FromArgb(246, 255, 255), Color.FromArgb(202, 234, 237), LinearGradientMode.Vertical))
                {
                    g.FillRectangle(brush, (int)this.rect.X, (int)this.rect.Y, (int)this.rect.Width, (int)this.rect.Height);
                }
            }
        }
        #endregion
    }
}
