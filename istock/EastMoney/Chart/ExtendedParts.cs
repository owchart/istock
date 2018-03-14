/*****************************************************************************\
*                                                                             *
* ExtendedParts.cs -  ExtendedParts functions, types, and definitions         *
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
using System.Windows.Forms;
using System.Data;
using OwLib;

namespace OwLib
{
    /// <summary>
    /// 图例的复选框
    /// </summary>
    public class LegendCheckBox : IDisposable
    {
        #region 陶德 2016/6/3
        /// <summary>
        /// 创建复选框
        /// </summary>
        public LegendCheckBox()
        {
        }

        /***************绘图参数***************/
        private Color colorOutBorder1 = Color.FromArgb(157, 160, 170);
        private Color colorOutPadding1 = Color.FromArgb(240, 240, 240);
        private Color colorInBorder1 = Color.FromArgb(200, 200, 200);
        private Color colorBeginFill1 = Color.FromArgb(180, 188, 193);
        private Color colorEndFill1 = Color.FromArgb(232, 232, 235);
        private Color colorHook1 = Color.FromArgb(74, 74, 92);
        private Color colorOutBorder2 = Color.FromArgb(123, 166, 188);
        private Color colorOutPadding2 = Color.FromArgb(240, 240, 240);
        private Color colorInBorder2 = Color.FromArgb(176, 222, 248);
        private Color colorBeginFill2 = Color.FromArgb(167, 214, 248);
        private Color colorEndFill2 = Color.FromArgb(215, 235, 248);
        private Color colorHook2 = Color.FromArgb(74, 74, 92);
        private Pen p = new Pen(Color.White);
        private GraphicsPath gp = new GraphicsPath();
        private SolidBrush sb = new SolidBrush(Color.White);

        private LegendCheckState m_CheckState = LegendCheckState.Checked;

        /// <summary>
        /// 获取或设置选中状态
        /// </summary>
        public LegendCheckState CheckState
        {
            get { return m_CheckState; }
            set
            {
                m_CheckState = value;
                m_OwnerCurve.IsVisible = value == LegendCheckState.Checked ? true : false;
            }
        }

        private LegendMouseState m_MouseState = LegendMouseState.Out;

        /// <summary>
        /// 获取或设置鼠标状态
        /// </summary>
        public LegendMouseState MouseState
        {
            get { return m_MouseState; }
            set { m_MouseState = value; }
        }

        private CurveItem m_OwnerCurve;

        /// <summary>
        /// 获取或设置对应的线条
        /// </summary>
        public CurveItem OwnerCurve
        {
            get { return m_OwnerCurve; }
            set { m_OwnerCurve = value; }
        }

        private RectangleF m_rect;

        /// <summary>
        /// 获取或设置区域
        /// </summary>
        public RectangleF Rect
        {
            get { return m_rect; }
            set { m_rect = value; }
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public void Dispose()
        {
            p.Dispose();
            gp.Dispose();
            sb.Dispose();
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="rect">区域</param>
        internal void Draw(Graphics g, RectangleF rect)
        {
            this.m_rect = rect;
            if (m_CheckState == LegendCheckState.Checked)
            {
                DrawCheckedBoxMouseOut(g, rect.X, rect.Y, rect.Width, rect.Height);
            }
            else if (m_CheckState == LegendCheckState.UnCheck)
            {
                DrawUnCheckedBoxMouseOut(g, rect.X, rect.Y, rect.Width, rect.Height);
            }
        }

        /// <summary>
        /// 绘制鼠标移出时的复选框
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public void DrawCheckedBoxMouseOut(Graphics g, float x, float y, float with, float height)
        {
            RectangleF recF = new RectangleF((int)x, (int)y, (int)with, (int)height);
            p.Color = colorOutBorder1; p.Width = 1F;
            g.DrawRectangle(p, recF.X, recF.Y, recF.Width, recF.Height);
            recF.Inflate(-.08F * recF.Width, -.08F * recF.Height);
            sb.Color = colorOutPadding1;
            g.FillRectangle(sb, recF.X, recF.Y, recF.Width, recF.Height);
            recF.Inflate(-.08F * recF.Width, -.08F * recF.Height);
            p.Color = colorInBorder1; p.Width = 1F;
            g.DrawRectangle(p, recF.X, recF.Y, recF.Width, recF.Height);
            recF.Inflate(-.08F * recF.Width, -.08F * recF.Height);
            gp.Reset();
            g.SmoothingMode = SmoothingMode.AntiAlias;
            PointF p1 = new PointF(x, y + height / 2);
            PointF p2 = new PointF(x + with / 2 - 2, y + height - 1);
            PointF p3 = new PointF(x + with, y);
            gp.AddLines(new PointF[] { p1, p2, p3 });
            p.Color = colorHook1;
            p.Width = 0.25F * recF.Width;
            g.DrawPath(p, gp);
        }

        /// <summary>
        /// 绘制鼠标未移出时的复选框
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        public void DrawUnCheckedBoxMouseOut(Graphics g, float x, float y, float with, float height)
        {
            RectangleF recF = new RectangleF((int)x, (int)y, (int)with, (int)height);
            p.Color = colorOutBorder1; p.Width = 1F;
            g.DrawRectangle(p, recF.X, recF.Y, recF.Width, recF.Height);
            recF.Inflate(-.08F * recF.Width, -.08F * recF.Height);
            sb.Color = colorOutPadding1;
            g.FillRectangle(sb, recF.X, recF.Y, recF.Width, recF.Height);
            recF.Inflate(-.08F * recF.Width, -.08F * recF.Height);
            p.Color = colorInBorder1; p.Width = 1F;
            g.DrawRectangle(p, recF.X, recF.Y, recF.Width, recF.Height);
        }

        /// <summary>
        /// 复选状态
        /// </summary>
        public enum LegendCheckState
        {
            /// <summary>
            /// 未选中
            /// </summary>
            UnCheck,
            /// <summary>
            /// 选中
            /// </summary>
            Checked
        }

        /// <summary>
        /// 鼠标状态
        /// </summary>
        public enum LegendMouseState
        {
            /// <summary>
            /// 移出
            /// </summary>
            Out,
            /// <summary>
            /// 移入
            /// </summary>
            In
        }
        #endregion
    }

    /// <summary>
    /// 自定义图形
    /// </summary>
    public class CustomerShape : IDisposable
    {
        #region 陶德 2016/6/3
        /// <summary>
        /// 创建自定义图形
        /// </summary>
        /// <param name="graph">控件</param>
        public CustomerShape(GraphA graph)
        {
            pen = new Pen(colorBorderDrawing);
            penComplete = new Pen(m_ColorBorderComplete);
            penErase = new Pen(ColorTranslator.FromWin32((int)graph.BackColor));
            brushErase = new SolidBrush(ColorTranslator.FromWin32((int)graph.BackColor));
            brushDrawing = new SolidBrush(colorFillDrawing);
            brushComplete = new SolidBrush(m_ColorFillComplete);
            m_shape = CustomShapeType.Rectangle;
            centerPoint = new PointF();
            scale = 1F;
            previousRect = new RectangleF();
            this.Graph = graph;
            drawComplete = false;
        }

        /***************绘图参数**************/
        private Bitmap bm;
        private Brush brushErase;
        private Brush brushDrawing;
        private Brush brushComplete;
        private ControlerButton button;
        private PointF centerPoint;
        private Color colorFillDrawing = Color.FromArgb(140, 79, 129, 189);
        private Color colorBorderDrawing = Color.FromArgb(140, 56, 93, 138);
        private bool controlPress;
        private bool drawComplete;
        private bool mousePress;
        private Pen pen;
        private Pen penErase;
        private Pen penComplete;
        private PointF[] points = new PointF[7];
        private RectangleF previousRect;
        private float scale;
        private bool shiftPress;

        private bool m_addWord = false;

        /// <summary>
        /// 获取或设置是否正在添加文字
        /// </summary>
        public bool AddWord
        {
            get { return m_addWord; }
            set { m_addWord = value; }
        } 

        private Color m_ColorBorderComplete = Color.FromArgb(255, 56, 93, 138);

        /// <summary>
        /// 获取边线色
        /// </summary>
        public Color ColorBorder
        {
            get { return m_ColorBorderComplete; }
        }

        private Color m_ColorFillComplete = Color.FromArgb(255, 79, 129, 189);

        /// <summary>
        /// 获取填充色
        /// </summary>
        public Color ColorFill
        {
            get { return m_ColorFillComplete; }
        }

        private Color m_ColorNone = Color.FromArgb(0, Color.White);

        /// <summary>
        /// 获取无颜色
        /// </summary>
        public Color ColorNone
        {
            get { return m_ColorNone; }
        }

        private Point m_endPosition;

        /// <summary>
        /// 获取或设置结束点坐标
        /// </summary>
        public Point EndPosition
        {
            get { return m_endPosition; }
            set { m_endPosition = value; }
        }

        private PointF m_FixedPoint = new PointF(10000, 10000);

        /// <summary>
        /// 获取或设置修正点坐标
        /// </summary>
        public PointF FixedPoint
        {
            get { return m_FixedPoint; }
            set { m_FixedPoint = value; }
        }

        private GraphA m_graph;

        /// <summary>
        /// 获取或设置图形控件
        /// </summary>
        public GraphA Graph
        {
            get { return m_graph; }
            set { m_graph = value; }
        }

        private GraphObj m_graphObj;

        /// <summary>
        /// 获取或设置图形
        /// </summary>
        public GraphObj GraphObj
        {
            get { return m_graphObj; }
            set { m_graphObj = value; }
        }

        private Point m_nowPosition;

        /// <summary>
        /// 获取或设置当前坐标
        /// </summary>
        public Point nowPosition
        {
            get { return m_nowPosition; }
            set { m_nowPosition = value; }
        }

        private PointF m_P1 = new PointF();

        /// <summary>
        /// 获取或设置坐标1
        /// </summary>
        public PointF P1
        {
            get { return m_P1; }
            set { m_P1 = value; }
        }

        private PointF m_P2 = new PointF();

        /// <summary>
        /// 获取或设置坐标2
        /// </summary>
        public PointF P2
        {
            get { return m_P2; }
            set { m_P2 = value; }
        }

        private RectangleF m_rect;

        /// <summary>
        /// 获取或设置区域
        /// </summary>
        public RectangleF Rect
        {
            get { return m_rect; }
            set { m_rect = value; }
        }

        private CustomShapeType m_shape;

        /// <summary>
        /// 获取或设置图形
        /// </summary>
        public CustomShapeType Shape
        {
            get { return m_shape; }
            set { m_shape = value; }
        }

        private Point m_startPosition;

        /// <summary>
        /// 获取或设置开始位置
        /// </summary>
        public Point StartPosition
        {
            get { return m_startPosition; }
            set
            {
                m_startPosition = value;
                centerPoint.X = value.X;
                centerPoint.Y = value.Y;
            }
        }

        /// <summary>
        /// 拖动完成委托
        /// </summary>
        /// <param name="cs">自定义图形</param>
        public delegate void DelegateDrawComplete(CustomerShape cs);

        /// <summary>
        /// 绘图完成事件
        /// </summary>
        public event DelegateDrawComplete EventDrawComplete;

        /// <summary>
        /// 绘制图行完成事件
        /// </summary>
        public event DelegateDrawComplete EventDragGraphObjComplete;

        /// <summary>
        /// 绘制按钮完成事件
        /// </summary>
        public event DelegateDrawComplete EventDragButtonComplete;

        /// <summary>
        /// 从位图中拷贝
        /// </summary>
        /// <param name="bmp">位图</param>
        public void CopyControlToBMP(ref Bitmap bmp)
        {
            bmp = new Bitmap(Graph.Width, Graph.Height);
            Graphics gTemp = Graphics.FromImage(bmp);
            RECT displayRect = Graph.DisplayRect;
            Rectangle gdiplusRect = new Rectangle(displayRect.left, displayRect.top, displayRect.right - displayRect.left, displayRect.bottom - displayRect.top);
            Rectangle rectScreen = Graph.Container.RectangleToScreen(gdiplusRect);
            gTemp.CopyFromScreen(rectScreen.X, rectScreen.Y, 0, 0, gdiplusRect.Size);
        }

        /// <summary>
        /// 创建我的圆角矩形路径
        /// </summary>
        /// <param name="rect">区域</param>
        /// <returns>圆角矩形路径</returns>
        public static GraphicsPath CreateMyRoundRectPath(RectangleF rect)
        {
            float cornerDiameter = Math.Min(rect.Width, rect.Height) / 2;
            if (cornerDiameter > 80)
            {
                cornerDiameter = 80;
            }
            return CustomerShape.CreateRoundRectPath(rect, cornerDiameter, 90);
        }

        /// <summary>
        /// 创建圆角矩形路径
        /// </summary>
        /// <param name="rect">区域</param>
        /// <param name="cornerDiameter">角直径</param>
        /// <param name="cornerAngle">角度</param>
        /// <returns>路径</returns>
        private static GraphicsPath CreateRoundRectPath(RectangleF rect, float cornerDiameter, float cornerAngle)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.AddArc(rect.Left, rect.Top, cornerDiameter, cornerDiameter, 225 - cornerAngle / 2, cornerAngle);
            gp.AddArc(rect.Right - cornerDiameter, rect.Top, cornerDiameter, cornerDiameter, 315 - cornerAngle / 2, cornerAngle);
            gp.AddArc(rect.Right - cornerDiameter, rect.Bottom - cornerDiameter, cornerDiameter, cornerDiameter, 45 - cornerAngle / 2, cornerAngle);
            gp.AddArc(rect.Left, rect.Bottom - cornerDiameter, cornerDiameter, cornerDiameter, 135 - cornerAngle / 2, cornerAngle);
            gp.CloseAllFigures();
            return gp;
        }

        /// <summary>
        /// 计算矩形
        /// </summary>
        /// <param name="p1">点1</param>
        /// <param name="p2">点2</param>
        /// <param name="rectTemp">区域</param>
        /// <returns>是否在区域内</returns>
        private bool CalcuRectF(Point p1, Point p2, ref RectangleF rectTemp)
        {
            float with = p2.X - p1.X;
            float height = p2.Y - p1.Y;
            float x = 0F, y = 0F;
            if (height == 0)
            {
                return false;
            }
            if (shiftPress && controlPress)
            {
                with = p2.X - centerPoint.X;
                height = p2.Y - centerPoint.Y;
                float withTemp = Math.Abs(with), heightTemp = Math.Abs(height);
                if (withTemp / heightTemp < scale)
                {
                    withTemp = scale * heightTemp;
                }
                else
                {
                    heightTemp = withTemp / scale;
                }
                x = centerPoint.X - withTemp;
                y = centerPoint.Y - heightTemp;
                with = 2F * withTemp; height = 2F * heightTemp;
            }
            else if (shiftPress)
            {
                float withTemp = Math.Abs(with), heightTemp = Math.Abs(height);
                if (withTemp / heightTemp < scale)
                {
                    withTemp = scale * heightTemp;
                }
                else
                {
                    heightTemp = withTemp / scale;
                }
                x = with < 0 ? p1.X - withTemp : p1.X;
                y = height < 0 ? p1.Y - heightTemp : p1.Y;
                with = withTemp; height = heightTemp;
            }
            else if (controlPress)
            {
                with = p2.X - centerPoint.X;
                height = p2.Y - centerPoint.Y;
                float withTemp = Math.Abs(with), heightTemp = Math.Abs(height);
                x = with < 0 ? p2.X : centerPoint.X - withTemp;
                y = height < 0 ? p2.Y : centerPoint.Y - heightTemp;
                with = 2F * withTemp; height = 2F * heightTemp;
            }
            else
            {
                x = with < 0 ? p2.X : p1.X;
                y = height < 0 ? p2.Y : p1.Y;
                with = Math.Abs(with); height = Math.Abs(height);
            }
            this.centerPoint.X = (x + .5F * with); this.centerPoint.Y = (y + .5F * height);
            this.scale = with / height;
            rectTemp.X = x; rectTemp.Y = y; rectTemp.Width = with; rectTemp.Height = height;
            return true;
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public void Dispose()
        {
            if (pen != null)
                pen.Dispose();
            if (penErase != null)
                penErase.Dispose();
            if (penComplete != null)
                penComplete.Dispose();
            if (brushErase != null)
                brushErase.Dispose();
            if (brushDrawing != null)
                brushDrawing.Dispose();
            if (brushComplete != null)
                brushComplete.Dispose();
        }

        /// <summary>
        /// 按钮拖动完成
        /// </summary>
        /// <param name="mousePt">坐标</param>
        /// <param name="controlerButton">按钮</param>
        internal void DragCompleteForButton(Point mousePt, ControlerButton controlerButton)
        {
            this.mousePress = false;
            this.drawComplete = true;
            CopyControlToBMP(ref this.bm);
            if (this.EventDragButtonComplete != null)
            {
                this.EventDragButtonComplete(this);
            }
        }

        /// <summary>
        /// 图形拖动中
        /// </summary>
        /// <param name="pointNow">当前点</param>
        /// <param name="rect">区域</param>
        public void DraggingForGraphObj(Point pointNow, RectangleF rect)
        {
            WinHost host = Graph.Native.Host as WinHost;
            using (Graphics g = Graphics.FromHwnd(host.HWnd))
            {
                int clientX = Graph.Native.ClientX(Graph);
                int clientY = Graph.Native.ClientY(Graph);
                GraphicsContainer container = g.BeginContainer(new Rectangle(clientX, clientY, Graph.Width, Graph.Height), new Rectangle(0, 0, Graph.Width, Graph.Height), GraphicsUnit.Pixel);
                if (this.bm != null)
                {
                    g.DrawImage(this.bm, 0, 0);
                    this.DrawForDragGraphObj(this.m_startPosition, pointNow, rect, this.pen, g, this.brushDrawing);
                }
                else
                {
                    this.Erase(this.previousRect, g);
                    this.DrawForDragGraphObj(this.m_startPosition, pointNow, rect, this.pen, g, this.brushDrawing);
                }
                this.previousRect = this.Rect;
                this.drawComplete = false;
                g.EndContainer(container);
            }
        }

        /// <summary>
        /// 拖动图形
        /// </summary>
        /// <param name="point">点1</param>
        /// <param name="point_2">点2</param>
        /// <param name="rect">区域</param>
        /// <param name="pen">画笔</param>
        /// <param name="g">绘图对象</param>
        /// <param name="brush">画刷</param>
        private void DrawForDragGraphObj(Point point_1, Point point_2, RectangleF rect, Pen pen, Graphics g, Brush brush)
        {
            if (this.m_shape == CustomShapeType.ArrowLine || this.m_shape == CustomShapeType.Line)
            {
                PointF pTem1 = new PointF(m_P1.X + point_2.X - point_1.X, P1.Y + point_2.Y - point_1.Y);
                PointF pTem2 = new PointF(m_P2.X + point_2.X - point_1.X, P2.Y + point_2.Y - point_1.Y);
                this.DrawLine(g, pen, pTem1, pTem2);
            }
            else
            {
                rect.X += (point_2.X - point_1.X);
                rect.Y += (point_2.Y - point_1.Y);
                this.Rect = rect;
                this.Draw(this.pen, g, this.brushDrawing);
            }
        }

        /// <summary>
        /// 拖动按钮开始
        /// </summary>
        /// <param name="mousePt">坐标</param>
        /// <param name="controlerButton">按钮</param>
        internal void DragStartForButton(Point mousePt, ControlerButton controlerButton)
        {
            this.button = controlerButton;
            this.GraphObj = controlerButton.owner;
            this.m_shape = GraphObj.shape;
            SetStartAndEndPoint(controlerButton, this.GraphObj.rect);
            this.mousePress = true;
            this.CopyControlToBMP(ref bm);
        }

        /// <summary>
        /// 拖动开始
        /// </summary>
        /// <param name="point">坐标</param>
        /// <param name="gpo">图形</param>
        public void DragStartForGraphObj(Point point, GraphObj gpo)
        {
            this.GraphObj = gpo;
            this.m_shape = gpo.shape;
            this.StartPosition = point;
            this.mousePress = true;
            this.CopyControlToBMP(ref bm);
            if (this.m_shape == CustomShapeType.Line || this.m_shape == CustomShapeType.ArrowLine)
            {
                this.P1 = this.GraphObj.pix1;
                this.P2 = this.GraphObj.pix2;
            }
            if (this.m_shape == CustomShapeType.Mark)
            {
                this.m_FixedPoint = (GraphObj as BoxMarkObj).screenPointFixed;
            }
        }

        /// <summary>
        /// 拖动结束
        /// </summary>
        /// <param name="point">坐标</param>
        public void DragCompleteForGraphObj(Point point)
        {
            this.mousePress = false;
            if (!this.drawComplete)
            {
                if (this.m_shape == CustomShapeType.ArrowLine || this.m_shape == CustomShapeType.Line)
                {
                    this.m_P1 = new PointF(P1.X + point.X - this.StartPosition.X, P1.Y + point.Y - this.StartPosition.Y);
                    this.m_P2 = new PointF(P2.X + point.X - this.StartPosition.X, P2.Y + point.Y - this.StartPosition.Y);
                }
                this.EndPosition = point;
                this.drawComplete = true;
                CopyControlToBMP(ref this.bm);
                if (this.EventDragGraphObjComplete != null)
                {
                    this.EventDragGraphObjComplete(this);
                }
            }
        }

        /// <summary>
        /// 按钮拖动
        /// </summary>
        /// <param name="mousePt">坐标</param>
        /// <param name="controlerButton">按钮</param>
        internal void DraggingForButton(Point mousePt, ControlerButton controlerButton)
        {
            WinHost host = Graph.Native.Host as WinHost;
            using (Graphics g = Graphics.FromHwnd(host.HWnd))
            {
                int clientX = Graph.Native.ClientX(Graph);
                int clientY = Graph.Native.ClientY(Graph);
                GraphicsContainer container = g.BeginContainer(new Rectangle(clientX, clientY, Graph.Width, Graph.Height), new Rectangle(0, 0, Graph.Width, Graph.Height), GraphicsUnit.Pixel);
                switch (controlerButton.buttonType)
                {
                    case ButtonType.TopLeft:
                        Drawing(this.StartPosition, mousePt, g);
                        break;
                    case ButtonType.Top:
                        this.StartPosition = new Point((int)this.GraphObj.rect.X, mousePt.Y);
                        this.EndPosition = new Point((int)this.GraphObj.rect.Right, (int)this.GraphObj.rect.Bottom);
                        Drawing(StartPosition, EndPosition, g);
                        break;
                    case ButtonType.TopRight:
                        Drawing(this.StartPosition, mousePt, g);
                        break;
                    case ButtonType.Right:
                        this.StartPosition = new Point((int)this.GraphObj.rect.X, (int)this.GraphObj.rect.Y);
                        this.EndPosition = new Point(mousePt.X, (int)this.GraphObj.rect.Bottom);
                        Drawing(this.StartPosition, EndPosition, g);
                        break;
                    case ButtonType.RightBottom:
                        Drawing(this.StartPosition, mousePt, g);
                        break;
                    case ButtonType.Bottom:
                        this.StartPosition = new Point((int)this.GraphObj.rect.X, (int)this.GraphObj.rect.Y);
                        this.EndPosition = new Point((int)this.GraphObj.rect.Right, mousePt.Y);
                        Drawing(this.StartPosition, EndPosition, g);
                        break;
                    case ButtonType.LeftBottom:
                        Drawing(this.StartPosition, mousePt, g);
                        break;
                    case ButtonType.Left:
                        this.StartPosition = new Point(mousePt.X, (int)this.GraphObj.rect.Y);
                        this.EndPosition = new Point((int)this.GraphObj.rect.Right, (int)this.GraphObj.rect.Bottom);
                        Drawing(this.StartPosition, EndPosition, g);
                        break;
                    case ButtonType.Rotate:
                        break;
                    case ButtonType.First:
                        this.StartPosition = mousePt;
                        Drawing(this.StartPosition, this.EndPosition, g);
                        break;
                    case ButtonType.Second:
                        this.EndPosition = mousePt;
                        Drawing(this.StartPosition, this.EndPosition, g);
                        break;
                    case ButtonType.Fixed:
                        this.m_FixedPoint = new PointF((float)mousePt.X, (float)mousePt.Y);
                        this.StartPosition = new Point((int)this.GraphObj.rect.X, (int)this.GraphObj.rect.Y);
                        this.EndPosition = new Point((int)this.GraphObj.rect.Right, (int)this.GraphObj.rect.Bottom);
                        Drawing(this.StartPosition, this.EndPosition, g);
                        break;
                    default:
                        break;
                }
                g.EndContainer(container);
            }
        }

        /// <summary>
        /// 移动中绘图
        /// </summary>
        /// <param name="point1">点1</param>
        /// <param name="point2">点2</param>
        /// <param name="g">绘图对象</param>
        internal void Drawing(Point point1, Point point2, Graphics g)
        {
            if (this.mousePress)
            {
                if (this.bm != null)
                {
                    int clientX = Graph.Native.ClientX(Graph);
                    int clientY = Graph.Native.ClientY(Graph);
                    g.DrawImage(this.bm, 0, 0);
                    this.Draw(point1, point2, this.pen, g, this.brushDrawing);
                }
                else
                {
                    this.Erase(this.previousRect, g); // 擦除上次绘制的图形
                    this.Draw(point1, point2, this.pen, g, this.brushDrawing);
                }
                this.previousRect = this.Rect;
                this.drawComplete = false;
            }
        }

        /// <summary>
        /// 在位图上绘制
        /// </summary>
        /// <param name="bitmap">位图</param>
        /// <param name="p1">点1</param>
        /// <param name="p2">点2</param>
        /// <param name="pen">画笔</param>
        /// <param name="g">绘图对象</param>
        /// <param name="brush">画刷</param>
        private void DrawOnBMP(ref Bitmap bitmap, Point p1, Point p2, Pen pen, Graphics g, Brush brush)
        {
            if (CalcuRectF(p1, p2, ref this.m_rect))
            {
                Graphics gTemp = Graphics.FromImage(bitmap);
                this.Erase(this.previousRect, gTemp); // 擦除上次绘制的图形
                this.Draw(pen, gTemp, brush);
            }
        }

        /// <summary>
        /// 绘图结束
        /// </summary>
        /// <param name="point">坐标</param>
        internal void DrawComplete(Point point)
        {
            this.drawComplete = true;
            this.mousePress = false;
            CopyControlToBMP(ref this.bm);
            if (this.EventDrawComplete != null)
            {
                this.EventDrawComplete(this);
            }
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="p1">点1</param>
        /// <param name="p2">点2</param>
        /// <param name="pen">画笔</param>
        /// <param name="g">绘图对象</param>
        /// <param name="brush">画刷</param>
        private void Draw(Point p1, Point p2, Pen pen, Graphics g, Brush brush)
        {
            this.nowPosition = p2;
            if (CalcuRectF(p1, p2, ref this.m_rect))
            {
                Draw(pen, g, brush);
            }
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="pen">画笔</param>
        /// <param name="g">绘图对象</param>
        /// <param name="brush">画刷</param>
        private void Draw(Pen pen, Graphics g, Brush brush)
        {
            GraphicsPath gp;
            switch (this.m_shape)
            {
                case CustomShapeType.ArrowLine:
                    g.DrawLine(pen, this.StartPosition, this.nowPosition);
                    break;
                case CustomShapeType.Circle:
                    g.DrawEllipse(pen, Rect);
                    g.FillEllipse(brush, Rect);
                    break;
                case CustomShapeType.CircularBorder:
                    g.DrawEllipse(pen, Rect);
                    break;
                case CustomShapeType.Line:
                    g.DrawLine(pen, this.StartPosition, this.nowPosition);
                    break;
                case CustomShapeType.Mark:
                    g.DrawRectangle(pen, (int)Rect.X, (int)Rect.Y, (int)Rect.Width, (int)Rect.Height);
                    g.FillRectangle(brush, Rect);
                    if (this.GraphObj == null)
                    {
                        PointF p1 = new PointF(Rect.X, Rect.Y);
                        PointF p2 = new PointF(Rect.X, Rect.Bottom);
                        PointF p3 = new PointF(Rect.X + Rect.Width * 0.2F, Rect.Bottom);
                        PointF p4 = new PointF(Rect.X + Rect.Width * 0.325F, Rect.Bottom + Rect.Height * 0.25F);
                        PointF p5 = new PointF(Rect.X + Rect.Width * 0.45F, Rect.Bottom);
                        PointF p6 = new PointF(Rect.Right, Rect.Bottom);
                        PointF p7 = new PointF(Rect.Right, Rect.Y);
                        points = new PointF[] { p1, p2, p3, p4, p5, p6, p7 };
                        this.m_FixedPoint = p4;
                        g.DrawPolygon(pen, points);
                        g.FillPolygon(brush, points);
                    }
                    else
                    {
                        if (this.Rect.Contains(this.m_FixedPoint))
                        {
                            g.DrawRectangle(pen, Rect.X, Rect.Y, Rect.Width, Rect.Height);
                            g.FillRectangle(brush, Rect);
                        }
                        else
                        {
                            (this.GraphObj as BoxMarkObj).CalcPoints(Rect, m_FixedPoint, ref points);
                            g.DrawPolygon(pen, points);
                            g.FillPolygon(brush, points);
                        }
                    }
                    break;
                case CustomShapeType.Rectangle:
                    g.DrawRectangle(pen, (int)Rect.X, (int)Rect.Y, (int)Rect.Width, (int)Rect.Height);
                    g.FillRectangle(brush, (int)Rect.X, (int)Rect.Y, (int)Rect.Width, (int)Rect.Height);
                    break;
                case CustomShapeType.RectBorder:
                    g.DrawRectangle(pen, (int)Rect.X, (int)Rect.Y, (int)Rect.Width, (int)Rect.Height);
                    break;
                case CustomShapeType.RoundRectBorder:
                    if (Rect.Width == 0 || Rect.Height == 0)
                    {
                        return;
                    }
                    gp = CreateMyRoundRectPath(Rect);
                    g.DrawPath(pen, gp);
                    break;
                case CustomShapeType.RoundRectangle:
                    if (Rect.Width == 0 || Rect.Height == 0)
                    {
                        return;
                    }
                    gp = CreateMyRoundRectPath(Rect);
                    g.DrawPath(pen, gp);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 画线
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pen">画笔</param>
        /// <param name="p1">点1</param>
        /// <param name="p2">点2</param>
        private void DrawLine(Graphics g, Pen pen, PointF p1, PointF p2)
        {
            g.DrawLine(pen, p1, p2);
        }

        /// <summary>
        /// 绘图开始
        /// </summary>
        /// <param name="point">开始点</param>
        private void DrawStart(Point point)
        {
            this.GraphObj = null;
            this.StartPosition = point;
            this.mousePress = true;
            this.CopyControlToBMP(ref bm);
        }

        /// <summary>
        /// 擦除
        /// </summary>
        /// <param name="rect">区域</param>
        /// <param name="g">绘图对象</param>
        private void Erase(RectangleF rect, Graphics g)
        {
            if (!drawComplete)
            {
                switch (m_shape)
                {
                    case CustomShapeType.Rectangle:
                        g.DrawRectangle(penErase, (int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
                        g.FillRectangle(brushErase, (int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
                        break;
                    case CustomShapeType.Circle:
                        g.DrawEllipse(penErase, rect);
                        g.FillEllipse(brushErase, rect);
                        break;
                }
            }
        }

        /// <summary>
        /// 键盘按下
        /// </summary>
        /// <param name="e">按键</param>
        internal void KeyDown(KeyEventArgs e)
        {
            this.SetKeyStates(CustomerShape.KeyStatus.Down, e);
        }

        /// <summary>
        /// 键盘弹起
        /// </summary>
        /// <param name="e">按键</param>
        internal void KeyUp(KeyEventArgs e)
        {
            this.SetKeyStates(CustomerShape.KeyStatus.Up, e);
        }

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="point">坐标</param>
        internal void MouseDown(Point point)
        {
            this.DrawStart(point);
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="point">坐标</param>
        internal void MouseMove(Point point)
        {
            WinHost host = Graph.Native.Host as WinHost;
            using (Graphics g = Graphics.FromHwnd(host.HWnd))
            {
                int clientX = Graph.Native.ClientX(Graph);
                int clientY = Graph.Native.ClientY(Graph);
                GraphicsContainer container = g.BeginContainer(new Rectangle(clientX, clientY, Graph.Width, Graph.Height), new Rectangle(0, 0, Graph.Width, Graph.Height), GraphicsUnit.Pixel);
                this.Drawing(this.StartPosition, point, g);
                g.EndContainer(container);
            }
        }

        /// <summary>
        /// 鼠标弹起
        /// </summary>
        /// <param name="point">坐标</param>
        internal void MouseUp(Point point)
        {
            this.EndPosition = point;
            this.DrawComplete(point);
        }

        /// <summary>
        /// 设置按键状态
        /// </summary>
        /// <param name="keyStatus">按键状态</param>
        /// <param name="e">按键</param>
        internal void SetKeyStates(KeyStatus keyStatus, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey)
            {
                this.shiftPress = keyStatus == KeyStatus.Down ? true : false;
            }
            else if (e.KeyCode == Keys.ControlKey)
            {
                this.controlPress = keyStatus == KeyStatus.Down ? true : false;
            }
        }

        /// <summary>
        /// 设置开始和结束点
        /// </summary>
        /// <param name="button">按钮</param>
        /// <param name="rectangleF">区域</param>
        private void SetStartAndEndPoint(ControlerButton button, RectangleF rectangleF)
        {
            switch (button.buttonType)
            {
                case ButtonType.TopLeft:
                    this.StartPosition = new Point((int)rectangleF.Right, (int)rectangleF.Bottom);
                    return;
                case ButtonType.Top:
                    break;
                case ButtonType.TopRight:
                    this.StartPosition = new Point((int)rectangleF.Left, (int)rectangleF.Bottom);
                    return;
                case ButtonType.Right:
                    break;
                case ButtonType.RightBottom:
                    this.StartPosition = new Point((int)rectangleF.Left, (int)rectangleF.Top);
                    return;
                case ButtonType.Bottom:
                    break;
                case ButtonType.LeftBottom:
                    this.StartPosition = new Point((int)rectangleF.Right, (int)rectangleF.Top);
                    break;
                case ButtonType.Left:
                    break;
                case ButtonType.Rotate:
                    break;
                case ButtonType.First:
                    if (button.rect.Contains(this.GraphObj.pix1))
                    {
                        this.EndPosition = new Point((int)this.GraphObj.pix2.X, (int)this.GraphObj.pix2.Y);
                        return;
                    }
                    else
                    {
                        this.EndPosition = new Point((int)this.GraphObj.pix1.X, (int)this.GraphObj.pix1.Y);
                        return;
                    }
                case ButtonType.Second:
                    if (button.rect.Contains(this.GraphObj.pix1))
                    {
                        this.StartPosition = new Point((int)this.GraphObj.pix2.X, (int)this.GraphObj.pix2.Y);
                        return;
                    }
                    else
                    {
                        this.StartPosition = new Point((int)this.GraphObj.pix1.X, (int)this.GraphObj.pix1.Y);
                        return;
                    }
                default:
                    break;
            }
            this.StartPosition = new Point(-1, -1);
            return;
        }

        /// <summary>
        /// 自定义图形类型
        /// </summary>
        public enum CustomShapeType
        {
            /// <summary>
            /// 肩头线
            /// </summary>
            ArrowLine,
            /// <summary>
            /// 圆
            /// </summary>
            Circle,
            /// <summary>
            /// 空心圆
            /// </summary>
            CircularBorder,
            /// <summary>
            /// 线
            /// </summary>
            Line,
            /// <summary>
            /// 标记
            /// </summary>
            Mark,
            /// <summary>
            /// 矩形
            /// </summary>
            Rectangle,
            /// <summary>
            /// 空心矩形
            /// </summary>
            RectBorder,
            /// <summary>
            /// 圆角空心矩形
            /// </summary>
            RoundRectBorder,
            /// <summary>
            /// 圆角矩形
            /// </summary>
            RoundRectangle
        }

        /// <summary>
        /// 键盘状态
        /// </summary>
        public enum KeyStatus
        {
            /// <summary>
            /// 按下
            /// </summary>
            Down,
            /// <summary>
            /// 弹起
            /// </summary>
            Up
        }
        #endregion
    }
}
