/*****************************************************************************\
*                                                                             *
* GraphObjRect.cs -   GraphObjRect functions, types, and definitions          *
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
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
namespace OwLib
{
    /// <summary>
    /// 矩形图
    /// </summary>
    public class GraphObjRect : GraphObj
    {
        #region 陶德 2016/6/4
        /// <summary>
        /// 创建矩形
        /// </summary>
        public GraphObjRect()
            :
            this(0, 0, Default.CoordFrame, Default.AlignH, Default.AlignV)
        {
        }

        /// <summary>
        /// 创建矩形
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        public GraphObjRect(double x, double y)
            :
            this(x, y, Default.CoordFrame, Default.AlignH, Default.AlignV)
        {
        }

        /// <summary>
        /// 创建矩形
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="coordType">坐标类型</param>
        public GraphObjRect(double x, double y, CoordType coordType)
            :
            this(x, y, coordType, Default.AlignH, Default.AlignV)
        {
        }

        /// <summary>
        /// 创建矩形
        /// </summary>
        /// <param name="x">横坐标值1</param>
        /// <param name="y">纵坐标值1</param>
        /// <param name="x2">横坐标值2</param>
        /// <param name="y2">纵坐标值2</param>
        public GraphObjRect(double x, double y, double x2, double y2)
            :
            base(x, y, x2, y2, Default.CoordFrame, Default.AlignH, Default.AlignV)
        {
        }

        /// <summary>
        /// 创建矩形
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="coordType">坐标类型</param>
        /// <param name="alignH">横向对齐</param>
        /// <param name="alignV">纵向对齐</param>
        public GraphObjRect(double x, double y, CoordType coordType, AlignH alignH, AlignV alignV)
            : base(x, y, coordType, alignH, alignV)
        {
        }

        /// <summary>
        /// 创建矩形
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="coordType">坐标类型</param>
        /// <param name="alignH">横向对齐</param>
        /// <param name="alignV">纵向对齐</param>
        public GraphObjRect(double x, double y, double width, double height, CoordType coordType,
                    AlignH alignH, AlignV alignV)
            : base(x, y, width, height, coordType, alignH, alignV)
        { }

        /// <summary>
        /// 创建矩形
        /// </summary>
        /// <param name="rhs">其他对象</param>
        public GraphObjRect(GraphObjRect rhs)
            : base(rhs)
        {
        }

        protected Border m_border;

        /// <summary>
        /// 获取或设置边线
        /// </summary>
        public Border Border
        {
            get { return m_border; }
            set { m_border = value; }
        }

        protected Fill m_fill;

        /// <summary>
        /// 获取或设置填充
        /// </summary>
        public Fill Fill
        {
            get { return m_fill; }
            set { m_fill = value; }
        }

        /// <summary>
        /// 是否包含坐标
        /// </summary>
        /// <param name="mousePt">坐标</param>
        /// <returns>是否包含</returns>
        internal override bool Contains(Point mousePt)
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

        /// <summary>
        /// 创建旋转按钮
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="rect">区域</param>
        private void CreateRotateButton1(Graphics g, RectangleF rect)
        {
            float diameter = RotateButton.Diameter;
            float radius = .5F * RotateButton.Diameter;
            float x1 = rect.X + .5F * rect.Width;
            float y1 = rect.Y - RectButton.sideLength;
            float x2 = x1;
            float y2 = y1 - RotateButton.LineLength;
            RectangleF rect1 = new RectangleF(x1 - radius, y2 - diameter, diameter, diameter);
            PointF p1 = new PointF(x1, y1);
            PointF p2 = new PointF(x2, y2);
            this.buttonList.Add(new RotateButton(rect1, p1, p2, ButtonType.Rotate, this));
        }

        /// <summary>
        /// 创建矩形按钮
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="rect">区域</param>
        private void CreateRectButton4(Graphics g, RectangleF rect)
        {
            float width = CircleButton.buttonDiameter, height = CircleButton.buttonDiameter;
            float X1 = rect.X + .5F * rect.Width - CircleButton.buttonDiameter / 2;
            float Y1 = rect.Y - CircleButton.buttonDiameter / 2;
            float X2 = rect.Right - CircleButton.buttonDiameter / 2;
            float Y2 = rect.Y + .5F * rect.Height - CircleButton.buttonDiameter / 2;
            float X3 = X1;
            float Y3 = rect.Bottom - CircleButton.buttonDiameter / 2;
            float X4 = rect.X - CircleButton.buttonDiameter / 2;
            float Y4 = Y2;
            RectangleF rect1 = new RectangleF(X1, Y1, width, height);
            RectangleF rect2 = new RectangleF(X2, Y2, width, height);
            RectangleF rect3 = new RectangleF(X3, Y3, width, height);
            RectangleF rect4 = new RectangleF(X4, Y4, width, height);
            this.buttonList.Add(new RectButton(rect1, ButtonType.Top, this));
            this.buttonList.Add(new RectButton(rect2, ButtonType.Right, this));
            this.buttonList.Add(new RectButton(rect3, ButtonType.Bottom, this));
            this.buttonList.Add(new RectButton(rect4, ButtonType.Left, this));
        }

        /// <summary>
        /// 创建圆形按钮
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="rect">区域</param>
        private void CreateCircleButton4(Graphics g, RectangleF rect)
        {
            float width = CircleButton.buttonDiameter, height = CircleButton.buttonDiameter;
            float X1 = rect.X - width / 2;
            float Y1 = rect.Y - height / 2;
            float X2 = rect.Right - width / 2;
            float Y2 = Y1;
            float X3 = X2;
            float Y3 = rect.Bottom - height / 2;
            float X4 = X1;
            float Y4 = Y3;
            RectangleF rect1 = new RectangleF(X1, Y1, width, height);
            RectangleF rect2 = new RectangleF(X2, Y2, width, height);
            RectangleF rect3 = new RectangleF(X3, Y3, width, height);
            RectangleF rect4 = new RectangleF(X4, Y4, width, height);
            this.buttonList.Add(new CircleButton(rect1, ButtonType.TopLeft, this));
            this.buttonList.Add(new CircleButton(rect2, ButtonType.TopRight, this));
            this.buttonList.Add(new CircleButton(rect3, ButtonType.RightBottom, this));
            this.buttonList.Add(new CircleButton(rect4, ButtonType.LeftBottom, this));
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">绘图因子</param>
        public override void Draw(System.Drawing.Graphics g, PaneBase pane, float scaleFactor)
        {
        }

        /// <summary>
        /// 绘制控制
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="rect">区域</param>
        public void DrawControler(Graphics g, RectangleF rect)
        {
            if (Focused)
            {
                SmoothingMode TempSm = g.SmoothingMode;
                g.SmoothingMode = SmoothingMode.HighQuality;
                if (this.shape == CustomerShape.CustomShapeType.Circle || this.shape == CustomerShape.CustomShapeType.CircularBorder
                    || this.shape == CustomerShape.CustomShapeType.RoundRectangle || this.shape == CustomerShape.CustomShapeType.RoundRectBorder)
                {
                    using (Pen pen = new Pen(ColorObj.DefaultColors[0], 0.3F))
                        g.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
                }
                this.buttonList.Clear();
                if (AllowRotate)
                {
                    CreateRotateButton1(g, rect);
                }
                CreateCircleButton4(g, rect);
                CreateRectButton4(g, rect);
                this.buttonList.Draw(g);
                g.SmoothingMode = TempSm;
            }
        }

        /// <summary>
        /// 绘制文字
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="rect">区域</param>
        /// <param name="fill">填充</param>
        public void DrawText(System.Drawing.Graphics g, float scaleFactor, RectangleF rect, Fill fill)
        {
            if (textBuider.Length == 0)
            {
                return;
            }
            if (scaleFactor < 0.5F) 
            {
                scaleFactor = 0.5F;
            }
            Font font = fontSpec.GetFont(scaleFactor);
            String text = textBuider.ToString();
            Rectangle rectText = new Rectangle((int)rect.X + 10, (int)rect.Y + 10, Math.Abs((int)rect.Width - 20), Math.Abs((int)rect.Height - 20));
            if (fill == null)
                fill = this.Fill;
            Color color = GetSuitableColor(fill.Color);
            TextRenderer.DrawText(g, text, font, rectText, color, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak);
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

        /// <summary>
        /// 获取合适的颜色
        /// </summary>
        /// <param name="colorPra">原颜色</param>
        /// <returns>合适的颜色</returns>
        private Color GetSuitableColor(Color colorPra)
        {
            if (colorPra == Default.FillColor)
            {
                return Color.FromArgb(30, 30, 30);
            }
            else
            {
                return Color.White;
            }
        }
        #endregion
    }
}
