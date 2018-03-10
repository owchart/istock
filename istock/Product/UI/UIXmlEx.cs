/*****************************************************************************\
*                                                                             *
* ChartUIXml.cs - Chart xml functions, types, and definitions.                *
*                                                                             *
*               Version 1.00  ★★★                                          *
*                                                                             *
*               Copyright (c) 2016-2016, iTeam. All rights reserved.      *
*               Created by Lord 2016/12/24.                                   *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;
using System.Data;

namespace OwLib
{
    /// <summary>
    /// 股票图形控件Xml解析
    /// </summary>
    public class UIXmlEx:UIXml
    {
        #region Lord 2016/12/24
        private double m_scaleFactor = 1;

        /// <summary>
        /// 获取或设置缩放因子
        /// </summary>
        public double ScaleFactor
        {
            get { return m_scaleFactor; }
            set { m_scaleFactor = value; }
        }

        /// <summary>
        /// 创建控件
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="type">类型</param>
        /// <returns>控件</returns>
        public override ControlA CreateControl(XmlNode node, String type)
        {
            INativeBase native = Native;
            if (type == "barragediv")
            {
                return new BarrageDiv();
            }
            else if (type == "floatdiv")
            {
                return new FloatDiv();
            }
            else if (type == "klinediv")
            {
                return new ChartA();
            }
            else if (type == "latestdiv")
            {
                return new LatestDiv();
            }
            else if (type == "column" || type == "th")
            {
                return new GridColumnEx();
            }
            else if (type == "ribbonbutton")
            {
                return new RibbonButton();
            }
            else if (type == "windowex")
            {
                return new WindowEx();
            }
            ControlA control = base.CreateControl(node, type);
            if (control != null)
            {
                control.Font.m_fontFamily = "微软雅黑";
                if (control is CheckBoxA)
                {
                    (control as CheckBoxA).ButtonBackColor = CDraw.PCOLORS_BACKCOLOR8;
                }
            }
            return control;
        }

        /// <summary>
        /// 创建菜单项
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="menu">菜单</param>
        /// <param name="parentItem">父节点</param>
        protected override void CreateMenuItem(XmlNode node, MenuA menu, MenuItemA parentItem)
        {
            MenuItemA item = new MenuItemA();
            item.Native = Native;
            item.Font = new FONT("微软雅黑", 12, false, false, false);
            SetAttributesBefore(node, item);
            if (parentItem != null)
            {
                parentItem.AddItem(item);
            }
            else
            {
                menu.AddItem(item);
            }
            if (node.ChildNodes != null && node.ChildNodes.Count > 0)
            {
                foreach (XmlNode subNode in node.ChildNodes)
                {
                    CreateMenuItem(subNode, menu, item);
                }
            }
            SetAttributesAfter(node, item);
            OnAddControl(item, node);
        }

        /// <summary>
        /// 退出方法
        /// </summary>
        public virtual void Exit()
        {
        }

        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <param name="path">路径</param>
        public static void ExportToExcel(String fileName, GridA grid)
        {
            DataTable dataTable = new DataTable();
            List<GridColumn> columns = grid.m_columns;
            int columnsSize = columns.Count;
            for (int i = 0; i < columnsSize; i++)
            {
                dataTable.Columns.Add(new DataColumn(columns[i].Text));
            }
            List<GridRow> rows = grid.m_rows;
            int rowsSize = rows.Count;
            for (int i = 0; i < rowsSize; i++)
            {
                if (rows[i].Visible)
                {
                    DataRow dr = dataTable.NewRow();
                    for (int j = 0; j < columnsSize; j++)
                    {
                        GridCell cell = grid.m_rows[i].GetCell(j);
                        if (cell is GridStringCell)
                        {
                            dr[j] = cell.GetString();
                        }
                        else
                        {
                            dr[j] = cell.GetDouble();
                        }
                    }
                    dataTable.Rows.Add(dr);
                }
            }
            DataCenter.ExportService.ExportDataTableToExcel(dataTable, fileName);
            dataTable.Dispose();
        }

        /// <summary>
        /// 导出到Word
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="html">内容</param>
        public static void ExportToWord(String fileName, String html)
        {
            DataCenter.ExportService.ExportHtmlToWord(fileName, html);
        }

        /// <summary>
        /// 导出到Txt
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="grid">表格控件</param>
        public static void ExportToTxt(String fileName, GridA grid)
        {
            StringBuilder sb = new StringBuilder();
            List<GridColumn> columns = grid.m_columns;
            int columnsSize = columns.Count;
            for (int i = 0; i < columnsSize; i++)
            {
                sb.Append(columns[i].Text);
                if (i != columnsSize - 1)
                {
                    sb.Append(",");
                }
            }
            List<GridRow> rows = grid.m_rows;
            int rowsSize = rows.Count;
            List<GridRow> visibleRows = new List<GridRow>();
            for (int i = 0; i < rowsSize; i++)
            {
                if (rows[i].Visible)
                {
                    visibleRows.Add(rows[i]);
                }
            }
            int visibleRowsSize = visibleRows.Count;
            if (visibleRowsSize > 0)
            {
                sb.Append("\r\n");
                for (int i = 0; i < visibleRowsSize; i++)
                {
                    for (int j = 0; j < columnsSize; j++)
                    {
                        GridCell cell = visibleRows[i].GetCell(j);
                        String cellValue = cell.GetString();
                        sb.Append(cellValue);
                        if (j != columnsSize - 1)
                        {
                            sb.Append(",");
                        }
                    }
                    if (i != visibleRowsSize - 1)
                    {
                        sb.Append("\r\n");
                    }
                }
            }
            DataCenter.ExportService.ExportHtmlToTxt(fileName, sb.ToString());
        }

        /// <summary>
        /// 加载XML
        /// </summary>
        /// <param name="xmlPath">XML地址</param>
        public virtual void Load(String xmlPath)
        {
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public virtual void LoadData()
        {
        }

        /// <summary>
        /// 重置缩放尺寸
        /// </summary>
        /// <param name="clientSize">客户端大小</param>
        public void ResetScaleSize(SIZE clientSize)
        {
            INativeBase native = Native;
            if (native != null)
            {
                ControlHost host = native.Host;
                SIZE nativeSize = native.DisplaySize;
                List<ControlA> controls = native.GetControls();
                int controlsSize = controls.Count;
                for (int i = 0; i < controlsSize; i++)
                {
                    WindowFrameA frame = controls[i] as WindowFrameA;
                    if (frame != null)
                    {
                        WindowEx window = frame.GetControls()[0] as WindowEx;
                        if (window != null && !window.AnimateMoving)
                        {
                            POINT location = window.Location;
                            if (location.x < 10 || location.x > nativeSize.cx - 10)
                            {
                                location.x = 0;
                            }
                            if (location.y < 30 || location.y > nativeSize.cy - 30)
                            {
                                location.y = 0;
                            }
                            window.Location = location;
                        }
                    }
                }
                native.ScaleSize = new SIZE((int)(clientSize.cx * m_scaleFactor), (int)(clientSize.cy * m_scaleFactor));
                native.Update();
            }
        }

        /// <summary>
        /// 是否确认关闭
        /// </summary>
        /// <returns>不处理</returns>
        public virtual int Submit()
        {
            return 0;
        }
        #endregion
    }

    /// <summary>
    /// 窗体XML扩展
    /// </summary>
    public class WindowXmlEx : UIXmlEx
    {
        /// <summary>
        /// 调用控件方法事件
        /// </summary>
        private ControlInvokeEvent m_invokeEvent;

        protected UIXmlEx m_parent;

        /// <summary>
        /// 获取或设置父容器
        /// </summary>
        public UIXmlEx Parent
        {
            get { return m_parent; }
            set { m_parent = value; }
        }

        protected WindowEx m_window;

        /// <summary>
        /// 获取或设置窗体
        /// </summary>
        public WindowEx Window
        {
            get { return m_window; }
        }

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="click">点击次数</param>
        /// <param name="delta">滚轮滚动值</param>
        private void ClickButton(object sender, POINT mp, MouseButtonsA button, int click, int delta)
        {
            if (button == MouseButtonsA.Left && click == 1)
            {
                ControlA control = sender as ControlA;
                if (m_window != null && control == m_window.CloseButton)
                {
                    Close();
                }
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public virtual void Close()
        {
            m_window.Invoke("close");
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                if (m_window != null)
                {
                    m_invokeEvent = null;
                    m_window.Close();
                    m_window.Dispose();
                    m_window = null;
                }
                base.Dispose();
            }
        }

        /// <summary>
        /// 加载界面
        /// </summary>
        public virtual void Load(INativeBase native, String xmlName, String windowName)
        {
            Native = native;
            String xmlPath = DataCenter.GetAppPath() + "\\config\\" + xmlName + ".html";
            Script = new GaiaScript(this);
            LoadFile(xmlPath, null);
            m_window = FindControl(windowName) as WindowEx;
            m_invokeEvent = new ControlInvokeEvent(Invoke);
            m_window.RegisterEvent(m_invokeEvent, EVENTID.INVOKE);
            //注册点击事件
            RegisterEvents(m_window);
        }

        /// <summary>
        /// 调用控件线程方法
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="args">参数</param>
        private void Invoke(object sender, object args)
        {
            OnInvoke(args);
        }

        /// <summary>
        /// 调用控件线程方法
        /// </summary>
        /// <param name="args">参数</param>
        public void OnInvoke(object args)
        {
            if (args != null && args.ToString() == "close")
            {
                Dispose();
                Native.Invalidate();
            }
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="control">控件</param>
        private void RegisterEvents(ControlA control)
        {
            ControlMouseEvent clickButtonEvent = new ControlMouseEvent(ClickButton);
            List<ControlA> controls = control.GetControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++)
            {
                ButtonA button = controls[i] as ButtonA;
                if (button != null)
                {
                    button.RegisterEvent(clickButtonEvent, EVENTID.CLICK);
                }
                RegisterEvents(controls[i]);
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        public virtual void Show()
        {
            m_window.Location = new POINT(-m_window.Width, -m_window.Height);
            m_window.AnimateShow(false);
            m_window.Invalidate();
        }

        /// <summary>
        /// 显示
        /// </summary>
        public virtual void ShowDialog()
        {
            m_window.Location = new POINT(-m_window.Width, -m_window.Height);
            m_window.AnimateShow(true);
            m_window.Invalidate();
        }
    }

    /// <summary>
    /// 表格列扩展
    /// </summary>
    public class GridColumnEx : GridColumn
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GridColumnEx()
        {
            BorderColor = COLOR.EMPTY;
            ForeColor = COLOR.ARGB(255, 255, 255);
            Font = new FONT("微软雅黑", 14, true, false, false);
        }

        /// <summary>
        /// 重绘背景方法
        /// </summary>
        /// <param name="paint">绘图区域</param>
        /// <param name="clipRect">裁剪对象</param>
        public override void OnPaintBackground(CPaint paint, RECT clipRect)
        {
            int width = Width, height = Height;
            RECT drawRect = new RECT(0, 0, width, height);
            paint.FillRect(COLOR.ARGB(0, 0, 0), drawRect);
        }
    }
}
