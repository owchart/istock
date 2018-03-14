/*****************************************************************************\
*                                                                             *
* GraphObjLine.cs -   GraphObjLine functions, types, and definitions          *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Drawing;

namespace OwLib
{
    /// <summary>
    /// 线图
    /// </summary>
    public class GraphObjLine : GraphObj
    {
        #region 陶德 2016/6/4
        /// <summary>
        /// 创建线图
        /// </summary>
        /// <param name="x1">横坐标值</param>
        /// <param name="y1">纵坐标值</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public GraphObjLine(double x1, double y1, double width, double height)
            : base(x1, y1, width, height) { }

        /// <summary>
        /// 创建线图
        /// </summary>
        /// <param name="rhs">其他对象</param>
        public GraphObjLine(GraphObjLine rhs)
            : base(rhs)
        {
        }

        /// <summary>
        /// 是否包含点
        /// </summary>
        /// <param name="mousePt">坐标</param>
        /// <returns>是否包含</returns>
        internal override bool Contains(Point mousePt)
        {
            float with = 0;
            bool result = false;
            if (pen == null)
            {
                pen = new Pen(Color.Empty, 20);
            }
            else
            {
                with = pen.Width;
                pen.Width = 20;
            }
            if (gp != null && gp.IsOutlineVisible(mousePt, pen))
            {
                result = true;
            }
            else if (this.Focused && this.buttonList.Contains(mousePt))
            {
                result = true;
            }
            else
            {
                result = false;
            }
            pen.Width = with;
            return result;
        }

        /// <summary>
        /// 绘制控制
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="x1">横坐标值1</param>
        /// <param name="y1">纵坐标值1</param>
        /// <param name="x2">横坐标值2</param>
        /// <param name="y2">纵坐标值2</param>
        public void DrawControler(Graphics g, float x1, float y1, float x2, float y2)
        {
            if (Focused)
            {
                this.buttonList.Clear();
                float diameter = CircleButton.buttonDiameter;
                float radius = .5F * diameter;
                RectangleF rect1 = new RectangleF(x1 - radius, y1 - radius, diameter, diameter);
                RectangleF rect2 = new RectangleF(x2 - radius, y2 - radius, diameter, diameter);
                this.buttonList.Add(new CircleButton(rect1, ButtonType.First, this));
                this.buttonList.Add(new CircleButton(rect2, ButtonType.Second, this));
                this.buttonList.Draw(g);
            }
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        public override void Draw(System.Drawing.Graphics g, PaneBase pane, float scaleFactor)
        {
        }

        /// <summary>
        /// 获取坐标
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="g">绘图对象</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="shape">图形</param>
        /// <param name="coords">坐标</param>
        public override void GetCoords(PaneBase pane, System.Drawing.Graphics g, float scaleFactor, out String shape, out String coords)
        {
            shape = String.Empty;
            coords = String.Empty;
        }
        #endregion
    }
}
