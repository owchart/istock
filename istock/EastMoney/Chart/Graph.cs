/*****************************************************************************\
*                                                                             *
* Graph.cs -   Graph functions, types, and definitions                        *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Data;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OwLib;

namespace OwLib
{
    /// <summary>
    /// 图形控件
    /// </summary>
    public partial class GraphA : ControlA
    {
        #region 陶德 2016/5/31
        /// <summary>
        /// 创建图形控件
        /// </summary>
        public GraphA()
        {
        }

        /// <summary>
        /// 上次点击的坐标
        /// </summary>
        private Point m_oldPoint = new Point(0, 0);

        /// <summary>
        /// 提示
        /// </summary>
        private String m_toolTip;

        /// <summary>
        /// 提示的位置
        /// </summary>
        private Point m_toolTipPoint;

        bool m_AlloCustomerShapeAction = true;

        /// <summary>
        /// 获取或设置是否允许自定义图形动作
        /// </summary>
        public bool AlloCustomerShapeAction
        {
            get { return m_AlloCustomerShapeAction; }
            set { m_AlloCustomerShapeAction = value; }
        }

        private Control m_container;

        /// <summary>
        /// 获取或设置容器
        /// </summary>
        public Control Container
        {
            get { return m_container; }
            set { m_container = value; }
        }

        private CustomerShape cs;

        /// <summary>
        /// 获取或设置自定义图形
        /// </summary>
        public CustomerShape Cs
        {
            get { return cs; }
            set { cs = value; }
        }

        private bool m_drawCustomerShape = false;

        /// <summary>
        /// 获取或设置是否开始绘制自定义图形
        /// </summary>
        public bool DrawCustomerShape
        {
            get { return m_drawCustomerShape; }
            set { m_drawCustomerShape = value; }
        }

        public GraphPane m_GraphPane;

        /// <summary>
        /// 获取或设置图形
        /// </summary>
        public GraphPane GraphPane
        {
            get
            {
                if (m_masterPane != null && m_masterPane.PaneList.Count > 0)
                    return m_masterPane.PaneList[0] as GraphPane;
                else
                    return new GraphPane();
            }
            set
            {
                if (m_masterPane != null)
                {
                    m_masterPane.PaneList.Clear();
                    m_masterPane.Add(value);
                }
            }
        }

        private bool m_isShowCursorValues = false;

        /// <summary>
        /// 获取或设置是否显示光标的值
        /// </summary>
        public bool IsShowCursorValues
        {
            get { return m_isShowCursorValues; }
            set { m_isShowCursorValues = value; }
        }

        private bool m_IsShowItemTip;

        /// <summary>
        /// 获取或设置是否显示项的提示
        /// </summary>
        public bool IsShowItemTip
        {
            get { return m_IsShowItemTip; }
            set { m_IsShowItemTip = value; }
        }

        private bool m_isShowPointValues = true;

        /// <summary>
        /// 获取或设置是否显示点值
        /// </summary>
        public bool IsShowPointValues
        {
            get { return m_isShowPointValues; }
            set { m_isShowPointValues = value; }
        }

        private MasterPane m_masterPane;

        /// <summary>
        /// 获取或设置主框
        /// </summary>
        public MasterPane MasterPane
        {
            get { return m_masterPane; }
            set { m_masterPane = value; }
        }

        /// <summary>
        /// 获取绘图类库
        /// </summary>
        public CPaint Paint
        {
            get { return Native.Paint; }
        }

        private String m_pointDateFormat = XDate.DefaultFormatStr;

        /// <summary>
        /// 获取或设置点值日期的格式
        /// </summary>
        public String PointDateFormat
        {
            get { return m_pointDateFormat; }
            set { m_pointDateFormat = value; }
        }

        private String m_pointValueFormat = PointPair.DefaultFormat;

        /// <summary>
        /// 获取或设置点值的格式
        /// </summary>
        public String PointValueFormat
        {
            get { return m_pointValueFormat; }
            set { m_pointValueFormat = value; }
        }

        /// <summary>
        /// 自定义按钮拖动完成
        /// </summary>
        /// <param name="cs">自定义图形</param>
        private void cs_EventDragButtonComplete(CustomerShape cs)
        {
            double X1 = 0, Y1 = 0, X2 = 0, Y2 = 0;
            GraphPane graphPane = this.MasterPane.FindPane(cs.EndPosition);
            if (graphPane == null)
                return;
            if (cs.Shape == CustomerShape.CustomShapeType.Line || cs.Shape == CustomerShape.CustomShapeType.ArrowLine)
            {
                graphPane.GetFixedPostion(cs.StartPosition, out X1, out Y1);
                graphPane.GetFixedPostion(cs.EndPosition, out X2, out Y2);
            }
            else
            {
                graphPane.GetFixedPostion(cs.Rect.Location, out X1, out Y1);
                graphPane.GetFixedPostion(new PointF(cs.Rect.Right, cs.Rect.Bottom), out X2, out Y2);
            }
            cs.GraphObj.Location.X = X1;
            cs.GraphObj.Location.Y = Y1;
            cs.GraphObj.Location.Width = X2 - X1;
            cs.GraphObj.Location.Height = Y2 - Y1;
            if (cs.Shape == CustomerShape.CustomShapeType.Mark)
            {
                (cs.GraphObj as BoxMarkObj).screenPointFixed = cs.FixedPoint;
            }
            this.Invalidate();
        }

        /// <summary>
        /// 自定义图形拖动完成
        /// </summary>
        /// <param name="cs">自定义图形</param>
        private void cs_EventDragComplete(CustomerShape cs)
        {
            double Xout = 0, Yout = 0;
            GraphPane graphPane = this.MasterPane.FindPane(cs.EndPosition);
            if (graphPane == null)
                return;
            if (cs.Shape == CustomerShape.CustomShapeType.ArrowLine || cs.Shape == CustomerShape.CustomShapeType.Line)
            {
                graphPane.GetFixedPostion(cs.P1, out Xout, out Yout);
            }
            else
            {
                graphPane.GetFixedPostion(cs.Rect.Location, out Xout, out Yout);
            }
            cs.GraphObj.Location.X = Xout;
            cs.GraphObj.Location.Y = Yout;
            this.Invalidate();
        }

        /// <summary>
        /// 自定义图形绘图完成
        /// </summary>
        /// <param name="cs">自定义图形</param>
        private void cs_EventDrawComplete(CustomerShape cs)
        {
            GraphObj gObj = null;
            double Xstart, Ystart, Xend, Yend;
            GraphPane graphPane = this.MasterPane.FindPane(cs.EndPosition);
            if (graphPane == null)
                return;
            graphPane.GetFixedPostion(cs.Rect.Location, out Xstart, out Ystart);
            graphPane.GetFixedPostion(new PointF(cs.Rect.X + cs.Rect.Width, cs.Rect.Y + cs.Rect.Height), out Xend, out Yend);
            double with = Xstart - Xend, height = Ystart - Yend;
            switch (cs.Shape)
            {
                case CustomerShape.CustomShapeType.ArrowLine:
                    this.GraphPane.GetFixedPostion(cs.StartPosition, out Xstart, out Ystart);
                    this.GraphPane.GetFixedPostion(cs.EndPosition, out Xend, out Yend);
                    gObj = new ArrowObj(cs.ColorBorder, 10, Xstart, Ystart, Xend, Yend);
                    break;
                case CustomerShape.CustomShapeType.Circle:
                    gObj = new EllipseObj(Xstart, Ystart, with, height, cs.ColorBorder, cs.ColorFill);
                    break;
                case CustomerShape.CustomShapeType.CircularBorder:
                    gObj = new EllipseObj(Xstart, Ystart, with, height, cs.ColorBorder, cs.ColorNone);
                    break;
                case CustomerShape.CustomShapeType.Line:
                    graphPane.GetFixedPostion(cs.StartPosition, out Xstart, out Ystart);
                    graphPane.GetFixedPostion(cs.EndPosition, out Xend, out Yend);
                    gObj = new LineObj(cs.ColorBorder, Xstart, Ystart, Xend, Yend);
                    break;
                case CustomerShape.CustomShapeType.Mark:
                    graphPane.GetFixedPostion(cs.StartPosition, out Xstart, out Ystart);
                    double x, y;
                    graphPane.GetFixedPostion(cs.FixedPoint, out x, out y);
                    PointF pointFixed = new PointF((float)x, (float)y);
                    gObj = new BoxMarkObj(Xstart, Ystart, with, height, cs.ColorBorder, cs.ColorFill, pointFixed);
                    break;
                case CustomerShape.CustomShapeType.Rectangle:
                    gObj = new BoxObj(Xstart, Ystart, with, height, cs.ColorBorder, cs.ColorFill);
                    break;
                case CustomerShape.CustomShapeType.RectBorder:
                    gObj = new BoxObj(Xstart, Ystart, with, height, cs.ColorBorder, cs.ColorNone);
                    break;
                case CustomerShape.CustomShapeType.RoundRectBorder:
                    gObj = new RoundedRectangleObj(Xstart, Ystart, with, height, cs.ColorBorder, cs.ColorNone);
                    break;
                case CustomerShape.CustomShapeType.RoundRectangle:
                    gObj = new RoundedRectangleObj(Xstart, Ystart, with, height, cs.ColorBorder, cs.ColorFill);
                    break;
                default:
                    break;
            }
            gObj.Location.CoordinateFrame = CoordType.PaneFraction;
            gObj.shape = cs.Shape;
            gObj.Focused = true;
            graphPane.GraphObjList.Add(gObj);
            cs.GraphObj = gObj;
        }


        /// <summary>
        /// 销毁方法
        /// </summary>
        public override void Dispose()
        {
            if (m_masterPane != null)
            {
                m_masterPane.Dispose();
            }
            if (cs != null)
                cs.Dispose();
        }

        /// <summary>
        /// 获取光标位置
        /// </summary>
        /// <param name="mousePt">鼠标坐标</param>
        /// <returns>光标坐标</returns>
        private Point HandleCursorValues(Point mousePt)
        {
            GraphPane pane = m_masterPane.FindPane(mousePt);
            if (pane != null && pane.Chart.m_rect.Contains(mousePt))
            {
                double x, x2, y, y2;
                pane.ReverseTransform(mousePt, out x, out x2, out y, out y2);
                String xStr = MakeValueLabel(pane.XAxis, x, -1, true);
                String yStr = MakeValueLabel(pane.YAxis, y, -1, true);
                String y2Str = MakeValueLabel(pane.Y2Axis, y2, -1, true);
                m_toolTip = "( " + xStr + ", " + yStr + ", " + y2Str + " )";
                m_toolTipPoint = mousePt;
            }
            else
            {
                m_toolTip = null;
            }
            return mousePt;
        }

        /// <summary>
        /// 检查图例
        /// </summary>
        /// <param name="mousePt">鼠标位置</param>
        private void HandleLegendCheck(Point mousePt)
        {
            foreach (GraphPane gp in this.MasterPane.PaneList)
            {
                foreach (CurveItem ci in gp.CurveList)
                {
                    if (ci.legendCheckBox.Rect.Contains(mousePt))
                    {
                        ci.legendCheckBox.MouseState = LegendCheckBox.LegendMouseState.In;
                    }
                    else
                    {
                        ci.legendCheckBox.MouseState = LegendCheckBox.LegendMouseState.Out;
                    }
                }
            }
        }

        /// <summary>
        /// 检查自定义图形鼠标移动
        /// </summary>
        /// <param name="mousePt">鼠标位置</param>
        private void HandleMouseMoveOnCustomerShape(Point mousePt)
        {
            GraphObj gpo = null;
            GraphPane gp = this.MasterPane.FindPane(mousePt);
            if (gp == null)
                return;
            if (Control.MouseButtons == MouseButtons.Left)
            {
                if (gp.FindObjFocused(out gpo))
                {
                    gpo.OnDrag(mousePt, this, cs);
                }
            }
            else
            {
                if (gp.FindObj(mousePt, out gpo))
                {
                    gpo.OnMouseMove(mousePt, this);
                }
            }
        }

        /// <summary>
        /// 获取点值的坐标
        /// </summary>
        /// <param name="mousePt">鼠标位置</param>
        /// <returns>坐标</returns>
        private Point HandlePointValues(Point mousePt)
        {
            int iPt;
            GraphPane pane;
            object nearestObj;
            double res = Math.Pow(Math.Pow((mousePt.X - m_oldPoint.X), 2) + Math.Pow((mousePt.Y - m_oldPoint.Y), 2), 0.5);
            if (res < 1)
            {
                m_oldPoint = mousePt;
                return mousePt;
            }
            m_oldPoint = mousePt;
            WinHost host = Native.Host as WinHost;
            using (Graphics g = Graphics.FromHwnd(host.HWnd))
            {
                if (m_masterPane.FindNearestPaneObject(mousePt,
                    g, out pane, out nearestObj, out iPt))
                {
                    if (nearestObj is CurveItem && iPt >= 0)
                    {
                        CurveItem curve = (CurveItem)nearestObj;
                        if (curve is Pie)
                        {
                            Pie pie = curve as Pie;
                            foreach (PieItem item in pie.Slices)
                            {
                                if (item.SlicePath != null &&
                                 item.SlicePath.IsVisible(mousePt))
                                {
                                    m_toolTip = item.Label.Text + ":" + item.Value.ToString(item.ValueFormat);
                                    m_toolTipPoint = mousePt;
                                }
                            }
                        }
                        else
                        {
                            PointPair pt = curve.Points[iPt];
                            double xVal, yVal, lowVal;
                            ValueHandler valueHandler = new ValueHandler(pane, false);
                            if ((curve is BarItem)
                                    && pane.BarSettings.Base != BarBase.X)
                                valueHandler.GetValues(curve, iPt, out yVal, out lowVal, out xVal);
                            else
                                valueHandler.GetValues(curve, iPt, out xVal, out lowVal, out yVal);
                            String xStr = MakeValueLabel(curve.GetXAxis(pane), xVal, iPt,
                                curve.IsOverrideOrdinal);
                            String yStr = MakeValueLabel(curve.GetYAxis(pane), yVal, iPt,
                                curve.IsOverrideOrdinal, curve.ValueFormat);
                            m_toolTip = xStr + ":" + yStr;
                            m_toolTipPoint = mousePt;
                        }
                    }
                    else
                        m_toolTip = null;
                }
                else
                    m_toolTip = null;
            }
            return mousePt;
        }

        /// <summary>
        /// 设置值标签
        /// </summary>
        /// <param name="axis">坐标轴</param>
        /// <param name="val">值</param>
        /// <param name="iPt">坐标</param>
        /// <param name="isOverrideOrdinal">是否重载</param>
        /// <returns>标签</returns>
        public String MakeValueLabel(Axis axis, double val, int iPt, bool isOverrideOrdinal)
        {
            if (axis != null)
            {
                if (axis.Scale.IsDate || axis.Scale.Type == AxisType.DateAsOrdinal)
                {
                    return XDate.ToString(val, axis.Scale.Format);
                }
                else if (axis.m_scale.IsText && axis.m_scale.m_textLabels != null)
                {
                    int i = iPt;
                    if (isOverrideOrdinal)
                        i = (int)(val - 0.5);
                    if (i >= 0 && i < axis.m_scale.m_textLabels.Length)
                        return axis.m_scale.m_textLabels[i];
                    else
                        return (i + 1).ToString();
                }
                else if (axis.Scale.IsAnyOrdinal && axis.Scale.Type != AxisType.LinearAsOrdinal
                                && !isOverrideOrdinal)
                {
                    return iPt.ToString(m_pointValueFormat);
                }
                else
                    return val.ToString(m_pointValueFormat);
            }
            else
                return "";
        }

        /// <summary>
        /// 设置值标签
        /// </summary>
        /// <param name="axis">坐标轴</param>
        /// <param name="val">值</param>
        /// <param name="iPt">坐标</param>
        /// <param name="isOverrideOrdinal">是否重载</param>
        /// <param name="valueFormat">格式</param>
        /// <returns>标签</returns>
        public String MakeValueLabel(Axis axis, double val, int iPt, bool isOverrideOrdinal, String valueFormat)
        {
            if (axis != null)
            {
                if (axis.Scale.IsDate || axis.Scale.Type == AxisType.DateAsOrdinal)
                {
                    return XDate.ToString(val, axis.Scale.Format);
                }
                else if (axis.m_scale.IsText && axis.m_scale.m_textLabels != null)
                {
                    int i = iPt;
                    if (isOverrideOrdinal)
                        i = (int)(val - 0.5);
                    if (i >= 0 && i < axis.m_scale.m_textLabels.Length)
                        return axis.m_scale.m_textLabels[i];
                    else
                        return (i + 1).ToString();
                }
                else if (axis.Scale.IsAnyOrdinal && axis.Scale.Type != AxisType.LinearAsOrdinal
                                && !isOverrideOrdinal)
                {
                    return iPt.ToString(valueFormat);
                }
                else
                    return val.ToString(valueFormat);
            }
            else
                return "";
        }

        /// <summary>
        /// 控件添加方法
        /// </summary>
        public override void OnAdd()
        {
            WinHost host = Native.Host as WinHost;
            m_container = Control.FromHandle(host.HWnd);
            cs = new CustomerShape(this);
            this.cs.EventDrawComplete += new CustomerShape.DelegateDrawComplete(cs_EventDrawComplete);
            this.cs.EventDragGraphObjComplete += new CustomerShape.DelegateDrawComplete(cs_EventDragComplete);
            this.cs.EventDragButtonComplete += new CustomerShape.DelegateDrawComplete(cs_EventDragButtonComplete);
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            m_masterPane = new MasterPane("", rect);
            m_masterPane.Margin.All = 0;
            m_masterPane.Title.IsVisible = true;
            m_masterPane.Title.Text = String.Empty;
            String titleStr = String.Empty;
            String xStr = String.Empty;
            String yStr = String.Empty;
            GraphPane graphPane = new GraphPane(rect, titleStr, xStr, yStr);
            using (Graphics g = Graphics.FromHwnd(host.HWnd))
            {
                graphPane.AxisChange(g);
            }
            m_masterPane.Add(graphPane);
        }

        /// <summary>
        /// 字符输入方法
        /// </summary>
        /// <param name="ch">字符</param>
        public override void OnChar(char ch)
        {
            if (cs.AddWord)
            {
                cs.GraphObj.textBuider.Append(ch);
                Invalidate();
            }
        }

        /// <summary>
        /// 鼠标点击方法
        /// </summary>
        /// <param name="mp">鼠标位置</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚动值</param>
        public override void OnClick(POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            if (m_masterPane != null)
            {
                Point mousePt = new Point(mp.x, mp.y);
                SetCursor(mousePt);
                foreach (GraphPane gp in this.MasterPane.PaneList)
                {
                    foreach (CurveItem ci in gp.CurveList)
                    {
                        if (ci.legendCheckBox.Rect.Contains(mousePt))
                        {
                            ci.legendCheckBox.CheckState = ci.legendCheckBox.CheckState == LegendCheckBox.LegendCheckState.Checked ? LegendCheckBox.LegendCheckState.UnCheck : LegendCheckBox.LegendCheckState.Checked;
                            break;
                        }
                    }
                }
                this.Invalidate();
            }
        }

        /// <summary>
        /// 键盘按下方法
        /// </summary>
        /// <param name="key">按键</param>
        public override void OnKeyDown(char key)
        {
            SetCursor(m_container.PointToClient(Control.MousePosition));
            if (m_AlloCustomerShapeAction)
            {
                if (cs.GraphObj != null)
                {
                    cs.GraphObj.OnKeyDown(this, new KeyEventArgs((Keys)key), cs);
                }
            }
            if (m_drawCustomerShape)
            {
                cs.KeyDown(new KeyEventArgs((Keys)key));
            }
            Invalidate();
        }

        /// <summary>
        /// 键盘抬起方法
        /// </summary>
        /// <param name="e">按键</param>
        public override void OnKeyUp(char key)
        {
            SetCursor(m_container.PointToClient(Control.MousePosition));
            if (m_drawCustomerShape)
            {
                cs.KeyUp(new KeyEventArgs((Keys)key));
            }
        }

        /// <summary>
        /// 鼠标按下方法
        /// </summary>
        /// <param name="mp">鼠标位置</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚动值</param>
        public override void OnMouseDown(POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            m_toolTip = null;
            Point mousePt = new Point(mp.x, mp.y);
            if (clicks > 1 || m_masterPane == null)
                return;
            GraphPane pane = this.MasterPane.FindPane(mousePt);
            if (m_drawCustomerShape)
            {
                cs.MouseDown(mousePt);
            }
            if (pane != null)
            {
                bool getOne = false;
                for (int i = pane.GraphObjList.Count - 1; i > -1; i--)
                {
                    GraphObj gpo = pane.GraphObjList[i];
                    if (gpo.Contains(mousePt))
                    {
                        if (!getOne)
                        {
                            getOne = true;
                            gpo.OnGotFocus(this);
                            gpo.OnMouseDown(mousePt, this, cs);
                        }
                        else
                        {
                            gpo.OnLostFocus(this);
                        }
                    }
                    else
                    {
                        gpo.OnLostFocus(this);
                    }
                }
            }
            this.Invalidate();
        }

        /// <summary>
        /// 鼠标移动方法
        /// </summary>
        /// <param name="mp">鼠标位置</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚动值</param>
        public override void OnMouseMove(POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            m_toolTip = null;
            if (m_masterPane != null && GraphPane != null)
            {
                Point mousePt = new Point(mp.x, mp.y);
                SetCursor(mousePt);
                if (GraphPane.Legend.IsShowLegendCheckBox)
                {
                    HandleLegendCheck(mousePt);
                }
                if (m_AlloCustomerShapeAction)
                {
                    HandleMouseMoveOnCustomerShape(mousePt);
                }
                if (m_drawCustomerShape)
                {
                    cs.MouseMove(mousePt);
                }
                else if (m_isShowCursorValues)
                    HandleCursorValues(mousePt);
                else if (m_isShowPointValues)
                    HandlePointValues(mousePt);
                this.Invalidate();
            }
        }

        /// <summary>
        /// 鼠标抬起方法
        /// </summary>
        /// <param name="mp">鼠标位置</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚动值</param>
        public override void OnMouseUp(POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            m_toolTip = null;
            if (m_masterPane == null || GraphPane == null)
            {
                return;
            }
            Cursor = CursorsA.Arrow;
            Point mousePt = new Point(mp.x, mp.y);
            if (m_drawCustomerShape)
            {
                cs.MouseUp(mousePt);
                m_drawCustomerShape = false;
            }
            if (m_AlloCustomerShapeAction)
            {
                GraphObj gpo = null;
                GraphPane graphPane = this.MasterPane.FindPane(cs.EndPosition);
                if (graphPane == null)
                    return;
                if (graphPane.FindObjFocused(out gpo))
                {
                    gpo.OnMouseUp(mousePt, cs);
                }
            }
        }

        /// <summary>
        /// 重绘背景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void OnPaintBackground(CPaint paint, RECT clipRect)
        {
            try
            {
                if (m_masterPane == null || this.GraphPane == null)
                {
                    return;
                }
                GdiPlusPaintEx gdiPlusPaint = paint as GdiPlusPaintEx;
                Rectangle containerRect = gdiPlusPaint.GetContainer(DisplayRect);
                Graphics g = gdiPlusPaint.Graphics;
                GraphicsContainer container = g.BeginContainer(containerRect, new Rectangle(0, 0, Width, Height), GraphicsUnit.Pixel);
                m_masterPane.AxisChange(g);
                m_masterPane.Draw(g);
                if (m_toolTip != null)
                {
                    using (Brush foreBrush = new SolidBrush(ColorTranslator.FromWin32((int)this.ForeColor)))
                    {
                        g.DrawString(m_toolTip, new Font("宋体", 12, FontStyle.Bold), foreBrush, new Point(m_toolTipPoint.X, m_toolTipPoint.Y - 10));
                    }
                }
                if (IsShowItemTip)
                {
                    foreach (GraphPane gp in m_masterPane.PaneList)
                    {
                        if (gp.CurveList != null && gp.CurveList.Count > 0)
                        {
                            using (ItemTipMaster itemTipMaster = new ItemTipMaster())
                            {
                                float scaleFactor = gp.CalcScaleFactor();
                                foreach (CurveItem item in gp.CurveList)
                                {
                                    if (item.IsVisible && IsShowItemTip)
                                    {
                                        for (int i = 0; i < item.Points.Count; i++)
                                        {
                                            itemTipMaster.CalcItemTip(g, gp, item, item.Points[i], scaleFactor, true);
                                        }
                                    }
                                }
                                itemTipMaster.DrawItemTip(g, gp, scaleFactor);
                            }
                        }
                    }
                }
                g.EndContainer(container);
            }
            catch { }
        }

        /// <summary>
        /// 尺寸改变方法
        /// </summary>
        public override void OnSizeChanged()
        {
            if (m_masterPane == null)
            {
                return;
            }
            Size newSize = new Size(this.Width, this.Height);
            WinHost host = Native.Host as WinHost;
            m_masterPane.ReSize(Graphics.FromHwnd(host.HWnd), new RectangleF(0, 0, newSize.Width, newSize.Height));
            Invalidate();
        }

        /// <summary>
        /// 设置光标
        /// </summary>
        /// <param name="mousePt">坐标</param>
        protected void SetCursor(Point mousePt)
        {
            if (m_drawCustomerShape)
            {
                Cursor = CursorsA.Hand;
            }
            else
            {
                Cursor = CursorsA.Arrow;
            }
        }
        #endregion
    }
}
