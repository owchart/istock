/*****************************************************************************\
*                                                                             *
* ControlerButton.cs -  ControlerButton functions, types, and definitions     *
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

namespace OwLib
{
    /// <summary>
    /// 创建控制按钮
    /// </summary>
    public abstract class ControlerButton
    {
        #region 陶德 2016/5/31
        /// <summary>
        /// 按钮类型
        /// </summary>
        public ButtonType buttonType;

        /// <summary>
        /// 区域
        /// </summary>
        public RectangleF rect;

        /// <summary>
        /// 容器
        /// </summary>
        public GraphObj owner;

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="g">绘图对象</param>
        public abstract void Draw(Graphics g);

        /// <summary>
        /// 拖动方法
        /// </summary>
        /// <param name="mousePtNow">鼠标位置</param>
        /// <param name="cs">图形</param>
        internal void OnDrag(Point mousePtNow, CustomerShape cs)
        {
            cs.DraggingForButton(mousePtNow, this);
        }

        /// <summary>
        /// 鼠标按下方法
        /// </summary>
        /// <param name="mousePt">鼠标位置</param>
        /// <param name="cs">图形</param>
        internal void OnMouseDown(Point mousePt, CustomerShape cs)
        {
            cs.DragStartForButton(mousePt, this);
        }

        /// <summary>
        /// 鼠标抬起方法
        /// </summary>
        /// <param name="mousePt">鼠标位置</param>
        /// <param name="cs">图形</param>
        internal void OnMouseUp(Point mousePt, CustomerShape cs)
        {
            cs.DragCompleteForButton(mousePt, this);
        }
        #endregion
    }

    /// <summary>
    /// 按钮类型
    /// </summary>
    public enum ButtonType
    {
        /// <summary>
        /// 左上
        /// </summary>
        TopLeft,
        /// <summary>
        /// 上
        /// </summary>
        Top,
        /// <summary>
        /// 右上
        /// </summary>
        TopRight,
        /// <summary>
        /// 右
        /// </summary>
        Right,
        /// <summary>
        /// 右下
        /// </summary>
        RightBottom,
        /// <summary>
        /// 下
        /// </summary>
        Bottom,
        /// <summary>
        /// 左下
        /// </summary>
        LeftBottom,
        /// <summary>
        /// 左
        /// </summary>
        Left,
        /// <summary>
        /// 旋转
        /// </summary>
        Rotate,
        /// <summary>
        /// 第一个
        /// </summary>
        First,
        /// <summary>
        /// 第二个
        /// </summary>
        Second,
        /// <summary>
        /// 混合
        /// </summary>
        Fixed
    }
}
