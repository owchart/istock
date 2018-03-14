/*****************************************************************************\
*                                                                             *
* GraphObj.cs -   GraphObj functions, types, and definitions                  *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Text;
using OwLib;

namespace OwLib
{
    /// <summary>
    /// 创建图形
    /// </summary>
    [Serializable]
    public abstract class GraphObj : IDisposable
    {
        #region 陶德 2016/6/4
        /// <summary>
        /// 创建图形
        /// </summary>
        public GraphObj()
            :
            this(0, 0, Default.CoordFrame, Default.AlignH, Default.AlignV)
        {
        }

        /// <summary>
        /// 创建图形
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        public GraphObj(double x, double y)
            :
            this(x, y, Default.CoordFrame, Default.AlignH, Default.AlignV)
        {
        }

        /// <summary>
        /// 创建图形
        /// </summary>
        /// <param name="x">横坐标值1</param>
        /// <param name="y">纵坐标值1</param>
        /// <param name="x2">横坐标值2</param>
        /// <param name="y2">纵坐标值2</param>
        public GraphObj(double x, double y, double x2, double y2)
            :
            this(x, y, x2, y2, Default.CoordFrame, Default.AlignH, Default.AlignV)
        {
        }

        /// <summary>
        /// 创建图形
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="coordType">坐标轴类型</param>
        public GraphObj(double x, double y, CoordType coordType)
            :
            this(x, y, coordType, Default.AlignH, Default.AlignV)
        {
        }

        /// <summary>
        /// 创建图形
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="coordType">坐标轴类型</param>
        /// <param name="alignH">横向对齐</param>
        /// <param name="alignV">纵向对齐</param>
        public GraphObj(double x, double y, CoordType coordType, AlignH alignH, AlignV alignV)
        {
            m_isVisible = true;
            m_isClippedToChartRect = Default.IsClippedToChartRect;
            m_zOrder = ZOrder.A_InFront;
            m_location = new Location(x, y, coordType, alignH, alignV);
            m_zIndex = zindexCounter++;
        }

        /// <summary>
        /// 创建图形
        /// </summary>
        /// <param name="x">横坐标值1</param>
        /// <param name="y">纵坐标值1</param>
        /// <param name="x2">横坐标值2</param>
        /// <param name="y2">纵坐标值2</param>
        /// <param name="coordType">坐标轴类型</param>
        /// <param name="alignH">横向对齐</param>
        /// <param name="alignV">纵向对齐</param>
        public GraphObj(double x, double y, double x2, double y2, CoordType coordType,
                    AlignH alignH, AlignV alignV)
        {
            m_isVisible = true;
            m_isClippedToChartRect = Default.IsClippedToChartRect;
            m_zOrder = ZOrder.A_InFront;
            m_location = new Location(x, y, x2, y2, coordType, alignH, alignV);
            m_zIndex = zindexCounter++;
        }

        /// <summary>
        /// 创建图形
        /// </summary>
        /// <param name="rhs">其他图形</param>
        public GraphObj(GraphObj rhs)
        {
            m_isVisible = rhs.IsVisible;
            m_isClippedToChartRect = rhs.m_isClippedToChartRect;
            m_zOrder = rhs.ZOrder;
            m_zIndex = rhs.m_zIndex;
            m_location = new Location(rhs.Location);
        }

        /// <summary>
        /// 默认属性
        /// </summary>
        public struct Default
        {
            public static AlignV AlignV = AlignV.Center;
            public static AlignH AlignH = AlignH.Center;
            public static CoordType CoordFrame = CoordType.AxisXYScale;
            public static bool IsClippedToChartRect = false;
            public static float PenWidth = 1.0F;
            public static Color BorderColor = Color.FromArgb(30, 30, 30);
            public static Color FillColor = Color.FromArgb(0, Color.White);
        }

        /********************绘图参数*******************/
        public bool AllowRotate;
        public float angle;
        public Color BorderColor;
        public ControlerButtonList buttonList = new ControlerButtonList();
        public Color FillColor;
        public bool Focused;
        public ControlerButton focusedButton;
        public FontSpec fontSpec = new FontSpec();
        public GraphicsPath gp;
        public GraphPane graphPane;
        public Point mouseDownPt;
        public Pen pen;
        public PointF pix1;
        public PointF pix2;
        public RectangleF rect;
        public CustomerShape.CustomShapeType shape;
        public StringBuilder textBuider = new StringBuilder();
        public static int zindexCounter = 0;

        protected bool m_isClippedToChartRect;

        /// <summary>
        /// 获取或设置是否复制矩形
        /// </summary>
        public bool IsClippedToChartRect
        {
            get { return m_isClippedToChartRect; }
            set { m_isClippedToChartRect = value; }
        }

        /// <summary>
        /// 获取或设置是否在数据前
        /// </summary>
        public bool IsInFrontOfData
        {
            get
            {
                return m_zOrder == ZOrder.A_InFront ||
                            m_zOrder == ZOrder.B_BehindLegend ||
                            m_zOrder == ZOrder.C_BehindChartBorder;
            }
        }

        protected bool m_isVisible;

        /// <summary>
        /// 获取或设置是否可见
        /// </summary>
        public bool IsVisible
        {
            get { return m_isVisible; }
            set { m_isVisible = value; }
        }

        protected Location m_location;

        /// <summary>
        /// 获取或设置位置
        /// </summary>
        public Location Location
        {
            get { return m_location; }
            set { m_location = value; }
        }


        internal int m_zIndex;

        /// <summary>
        /// 获取或设置层次索引
        /// </summary>
        public int ZIndex
        {
            get { return m_zIndex; }
            set { m_zIndex = value; }
        }

        internal ZOrder m_zOrder;

        /// <summary>
        /// 获取或设置层次
        /// </summary>
        public ZOrder ZOrder
        {
            get { return m_zOrder; }
            set { m_zOrder = value; }
        }

        /// <summary>
        /// 是否包含点
        /// </summary>
        /// <param name="mousePt">坐标</param>
        /// <returns>是否包含</returns>
        internal abstract bool Contains(Point mousePt);

        /// <summary>
        /// 销毁方法
        /// </summary>
        public void Dispose()
        {
            if (graphPane != null)
            {
                graphPane.Dispose();
                graphPane = null;
            }
            fontSpec.Dispose();
            fontSpec = null;
            if (pen != null)
            {
                pen.Dispose();
            }
        }

        /// <summary>
        /// 拖动结束
        /// </summary>
        /// <param name="mousePt">坐标</param>
        /// <param name="cs">自定义图形</param>
        private void DragComplete(Point mousePt, CustomerShape cs)
        {
            cs.DragCompleteForGraphObj(mousePt);
        }

        /// <summary>
        /// 拖动开始
        /// </summary>
        /// <param name="mousePt">坐标</param>
        /// <param name="cs">自定义图形</param>
        private void DragStart(Point mousePt, CustomerShape cs)
        {
            cs.DragStartForGraphObj(mouseDownPt, this);
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        public abstract void Draw(Graphics g, PaneBase pane, float scaleFactor);

        /// <summary>
        /// 查找按钮
        /// </summary>
        /// <param name="mousePt">坐标</param>
        /// <param name="button">按钮</param>
        /// <returns>是否找到</returns>
        public bool FindButton(Point mousePt, out ControlerButton button)
        {
            button = null;
            foreach (ControlerButton cb in this.buttonList)
            {
                if (cb.rect.Contains(mousePt))
                {
                    button = cb;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取坐标
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="g">绘图对象</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="shape">图形</param>
        /// <param name="coords">坐标</param>
        public abstract void GetCoords(PaneBase pane, Graphics g, float scaleFactor,
        out String shape, out String coords);

        /// <summary>
        /// 是否在箱体内
        /// </summary>
        /// <param name="mousePt">坐标</param>
        /// <param name="gpo">图形</param>
        /// <returns>是否获取成功</returns>
        public bool InBox(Point mousePt, out GraphObj gpo)
        {
            gpo = null;
            if (this.Contains(mousePt))
            {
                gpo = this;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 拖动中
        /// </summary>
        /// <param name="mousePtNow">坐标</param>
        /// <param name="graph">控件</param>
        /// <param name="cs">自定义图形</param>
        public void OnDrag(Point mousePtNow, GraphA graph, CustomerShape cs)
        {
            if (focusedButton != null)
            {
                focusedButton.OnDrag(mousePtNow, cs);
            }
            else
            {
                graph.Native.Cursor = CursorsA.SizeAll;
                cs.DraggingForGraphObj(mousePtNow, this.rect);
            }
        }

        /// <summary>
        /// 获取焦点
        /// </summary>
        /// <param name="graph">控件</param>
        public void OnGotFocus(GraphA graph)
        {
            this.m_zIndex = zindexCounter++;
            this.Focused = true;
        }

        /// <summary>
        /// 键盘按下
        /// </summary>
        /// <param name="graph">控件</param>
        /// <param name="e">键盘参数</param>
        /// <param name="cs">自定义图形</param>
        public void OnKeyDown(GraphA graph, KeyEventArgs e, CustomerShape cs)
        {
            if (Focused)
            {
                switch (e.KeyCode)
                {
                    case Keys.Escape:
                        Focused = false;
                        cs.AddWord = false;
                        break;
                    case Keys.Delete:
                        Focused = false;
                        graphPane.GraphObjList.Remove(this);
                        cs.AddWord = false;
                        break;
                    case Keys.Back:
                        if (this.textBuider.Length > 0)
                        {
                            this.textBuider.Remove(textBuider.Length - 1, 1);
                            cs.AddWord = false;
                        }
                        break;
                    default:
                        cs.AddWord = true;
                        break;
                }
            }
            else
            {
                cs.AddWord = false;
            }
        }

        /// <summary>
        /// 丢失焦点
        /// </summary>
        /// <param name="graph">控件</param>
        public void OnLostFocus(GraphA graph)
        {
            this.Focused = false;
        }

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="mousePt">坐标</param>
        /// <param name="graph">控件</param>
        /// <param name="cs">自定义图形</param>
        public void OnMouseDown(Point mousePt, GraphA graph, CustomerShape cs)
        {
            this.mouseDownPt = mousePt;
            ControlerButton cb = null;
            if (FindButton(mousePt, out cb))
            {
                focusedButton = cb;
                cb.OnMouseDown(mousePt, cs);
            }
            else
            {
                focusedButton = null;
                this.DragStart(mousePt, cs);
            }
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="mousePt">坐标</param>
        /// <param name="graph">控件</param>
        public void OnMouseMove(PointF mousePt, GraphA graph)
        {
            bool OnButton = false;
            if (this.Focused)
            {
                foreach (ControlerButton cb in this.buttonList)
                {
                    if (cb.rect.Contains(mousePt))
                    {
                        switch (cb.buttonType)
                        {
                            case ButtonType.TopLeft:
                                graph.Native.Cursor = CursorsA.SizeNWSE;
                                break;
                            case ButtonType.Top:
                                graph.Native.Cursor = CursorsA.SizeNS;
                                break;
                            case ButtonType.TopRight:
                                graph.Native.Cursor = CursorsA.SizeNESW;
                                break;
                            case ButtonType.Right:
                                graph.Native.Cursor = CursorsA.SizeWE;
                                break;
                            case ButtonType.RightBottom:
                                graph.Native.Cursor = CursorsA.SizeNWSE;
                                break;
                            case ButtonType.Bottom:
                                graph.Native.Cursor = CursorsA.SizeNS;
                                break;
                            case ButtonType.LeftBottom:
                                graph.Native.Cursor = CursorsA.SizeNESW;
                                break;
                            case ButtonType.Left:
                                graph.Native.Cursor = CursorsA.SizeWE;
                                break;
                            case ButtonType.Rotate:
                                graph.Native.Cursor = CursorsA.Hand;
                                break;
                            case ButtonType.First:
                                if ((angle >= 0 && angle < 90) || (angle >= 180 && angle < 270))
                                    graph.Native.Cursor = CursorsA.SizeNWSE;
                                else
                                    graph.Native.Cursor = CursorsA.SizeNESW;
                                break;
                            case ButtonType.Second:
                                if ((angle >= 0 && angle < 90) || (angle >= 180 && angle < 270))
                                    graph.Native.Cursor = CursorsA.SizeNWSE;
                                else
                                    graph.Native.Cursor = CursorsA.SizeNESW;
                                break;
                            case ButtonType.Fixed:
                                graph.Native.Cursor = CursorsA.Hand;
                                break;
                            default:
                                break;
                        }
                        OnButton = true;
                        break;
                    }
                }
            }
            if (!OnButton)
            {
                graph.Native.Cursor = CursorsA.SizeAll;
            }
        }

        /// <summary>
        /// 鼠标抬起
        /// </summary>
        /// <param name="mousePt">坐标</param>
        /// <param name="cs">自定义图形</param>
        public void OnMouseUp(Point mousePt, CustomerShape cs)
        {
            if (focusedButton != null)
            {
                focusedButton.OnMouseUp(mousePt, cs);
            }
            else
            {
                this.DragComplete(mousePt, cs);
            }
        }

        /// <summary>
        /// 点是否在箱体内
        /// </summary>
        /// <param name="pt">坐标</param>
        /// <param name="pane">图层</param>
        /// <param name="g">绘图对象</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <returns>是否在箱体内</returns>
        public virtual bool PointInBox(PointF pt, PaneBase pane, Graphics g, float scaleFactor)
        {
            GraphPane gPane = pane as GraphPane;
            if (gPane != null && m_isClippedToChartRect && !gPane.Chart.Rect.Contains(pt))
                return false;
            return true;
        }

        /// <summary>
        /// 点是否在箱体内
        /// </summary>
        /// <param name="pt">坐标</param>
        /// <returns>是否在箱体内</returns>
        public virtual bool PointInBox(PointF pt)
        {
            if (this.rect.Contains(pt))
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
