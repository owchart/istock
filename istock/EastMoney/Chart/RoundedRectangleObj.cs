/********************************************************************************\
*                                                                                *
* RoundedRectangleObj.cs - RoundedRectangleObj functions, types, and definitions *
*                                                                                *
*               Version 1.00                                                     *
*                                                                                *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.      *
*                                                                                *
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;
using System.Security.Permissions;
using System.Drawing.Drawing2D;

namespace OwLib
{
    /// <summary>
    /// 圆角矩形
    /// </summary>
    class RoundedRectangleObj : GraphObjRect
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建圆角矩形
        /// </summary>
        public RoundedRectangleObj()
            : this(0, 0, 1, 1)
        {
        }

        /// <summary>
        /// 创建圆角矩形
        /// </summary>
        /// <param name="rhs">其他对象</param>
        public RoundedRectangleObj(RoundedRectangleObj rhs)
            : base(rhs)
        {
            this.Border = rhs.Border;
            this.Fill = rhs.Fill;
        }

        /// <summary>
        /// 创建圆角矩形
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public RoundedRectangleObj(double x, double y, double width, double height)
            : base(x, y, width, height)
        {
            this.Border = new Border(Default.BorderColor, Default.PenWith);
            this.Fill = new Fill(Default.FillColor);
        }

        /// <summary>
        /// 创建圆角矩形
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="borderColor">边线颜色</param>
        /// <param name="fillColor">填充色</param>
        public RoundedRectangleObj(double x, double y, double width, double height, Color borderColor, Color fillColor)
            : base(x, y, width, height)
        {
            this.Border = new Border(borderColor, Default.PenWith);
            this.Fill = new Fill(fillColor);
        }

        /// <summary>
        /// 创建圆角矩形
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="borderColor">边线颜色</param>
        /// <param name="fillColor1">填充色1</param>
        /// <param name="fillColor2">填充色2</param>
        public RoundedRectangleObj(double x, double y, double width, double height, Color borderColor, Color fillColor1, Color fillColor2)
            : base(x, y, width, height)
        {
            this.Border = new Border(borderColor, Default.PenWith);
            this.Fill = new Fill(fillColor1, fillColor2);
        }

        /// <summary>
        /// 默认属性
        /// </summary>
        public new struct Default
        {
            public static float PenWith = 1.0F;
            public static Color BorderColor = Color.FromArgb(30, 30, 30);
            public static Color FillColor = Color.White;
        }

        private Border m_Border;

        /// <summary>
        /// 获取或设置边线
        /// </summary>
        public new Border Border
        {
            get { return m_Border; }
            set { m_Border = value; }
        }

        private Fill m_Fill;

        /// <summary>
        /// 获取或设置填充
        /// </summary>
        public new Fill Fill
        {
            get { return m_Fill; }
            set { m_Fill = value; }
        }

        /// <summary>
        /// 绘图
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        public override void Draw(System.Drawing.Graphics g, PaneBase pane, float scaleFactor)
        {
            RectangleF pixRect = this.Location.TransformRect(pane);
            RectangleF tmpRect = pane.Rect;
            tmpRect.Inflate(20, 20);
            pixRect.Intersect(tmpRect);
            if (Math.Abs(pixRect.Left) < 100000 &&
                    Math.Abs(pixRect.Top) < 100000 &&
                    Math.Abs(pixRect.Right) < 100000 &&
                    Math.Abs(pixRect.Bottom) < 100000)
            {
                SmoothingMode TempSm = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.HighQuality;
                m_Fill.DrawRoundRect(g, pixRect);
                m_Border.DrawRoundRec(g, pane, scaleFactor, pixRect);
                g.SmoothingMode = TempSm;
                DrawControler(g, pixRect);
                DrawText(g, scaleFactor, pixRect, m_Fill);
                this.rect = pixRect;
                this.graphPane = (GraphPane)pane;
            }
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
            throw new Exception("The method or operation is not implemented.");
        }
        #endregion
    }
}
