/*****************************************************************************\
*                                                                             *
* SearchDiv.cs - Search div functions, types, and definitions.                *
*                                                                             *
*               Version 1.00  ★★★                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/4/12.                                    *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    /// <summary>
    /// 键盘精灵
    /// </summary>
    public class SearchDiv : MenuA
    {
        #region Lord 2016/4/12
        /// <summary>
        /// 创建键盘精灵
        /// </summary>
        public SearchDiv()
        {
            m_gridCellClickEvent = new GridCellMouseEvent(GridCellClick);
            m_gridKeyDownEvent = new ControlKeyEvent(GridKeyDown);
            m_textBoxInputChangedEvent = new ControlEvent(TextBoxInputChanged);
            m_textBoxKeyDownEvent = new ControlKeyEvent(TextBoxKeyDown);
            BackColor = COLOR.EMPTY;
            IsWindow = true;
        }

        /// <summary>
        /// 表格控件
        /// </summary>
        private GridA m_grid;

        /// <summary>
        /// 表格单元格点击事件
        /// </summary>
        private GridCellMouseEvent m_gridCellClickEvent;

        /// <summary>
        /// 表格键盘事件
        /// </summary>
        private ControlKeyEvent m_gridKeyDownEvent;

        /// <summary>
        /// 文本框输入改变事件
        /// </summary>
        private ControlEvent m_textBoxInputChangedEvent;

        /// <summary>
        /// 文本框键盘事件
        /// </summary>
        private ControlKeyEvent m_textBoxKeyDownEvent;

        private MainFrame m_mainFrame;

        /// <summary>
        /// 获取或设置股票控件
        /// </summary>
        public MainFrame MainFrame
        {
            get { return m_mainFrame; }
            set { m_mainFrame = value; }
        }

        private TextBoxA m_searchTextBox;

        /// <summary>
        /// 获取或设置查找文本框
        /// </summary>
        public TextBoxA SearchTextBox
        {
            get { return m_searchTextBox; }
            set { m_searchTextBox = value; }
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                if (m_grid != null)
                {
                    m_grid.UnRegisterEvent(m_gridCellClickEvent, EVENTID.GRIDCELLCLICK);
                    m_grid.UnRegisterEvent(m_gridKeyDownEvent, EVENTID.KEYDOWN);
                }
                if (m_searchTextBox != null)
                {
                    if (m_textBoxInputChangedEvent != null)
                    {
                        m_searchTextBox.UnRegisterEvent(m_textBoxInputChangedEvent, EVENTID.TEXTCHANGED);
                        m_textBoxInputChangedEvent = null;
                    }
                    if (m_textBoxKeyDownEvent != null)
                    {
                        m_searchTextBox.UnRegisterEvent(m_textBoxKeyDownEvent, EVENTID.KEYDOWN);
                        m_textBoxKeyDownEvent = null;
                    }
                }
            }
            base.Dispose();
        }

        /// <summary>
        /// 过滤查找
        /// </summary>
        public void FilterSearch()
        {
            String sText = m_searchTextBox.Text.ToUpper();
            m_grid.BeginUpdate();
            m_grid.ClearRows();
            int row = 0;
            CList<GSecurity> securities = new CList<GSecurity>();
            if (sText.Length > 0)
            {
                foreach (GSecurity gSecurity in SecurityService.m_codedMap.Values)
                {
                    if (gSecurity.m_name != null)
                    {
                        if (gSecurity.m_code.ToUpper().IndexOf(sText) == 0 || gSecurity.m_name.ToUpper().IndexOf(sText) == 0
                            || gSecurity.m_pingyin.ToUpper().IndexOf(sText) == 0)
                        {
                            securities.push_back(gSecurity);
                        }
                    }
                }
            }
            if (securities != null)
            {
                int rowCount = securities.size();
                for (int i = 0; i < rowCount; i++)
                {
                    GSecurity security = securities.get(i);
                    GridRow gridRow = new GridRow();
                    m_grid.AddRow(gridRow);
                    gridRow.AddCell(0, new GridStringCell(security.m_code));
                    gridRow.AddCell(1, new GridStringCell(security.m_name));
                    row++;
                }
            }
            securities.Dispose();
            m_grid.EndUpdate();
        }

        /// <summary>
        /// 表格单元格点击事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="cell">单元格</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮值</param>
        private void GridCellClick(object sender, GridCell cell, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            if (button == MouseButtonsA.Left && clicks == 2)
            {
                OnSelectRow();
            }
        }

        /// <summary>
        /// 表格键盘事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="key">按键</param>
        private void GridKeyDown(object sender, char key)
        {
            if (key == 13)
            {
                OnSelectRow();
            }
        }

        /// <summary>
        /// 添加控件方法
        /// </summary>
        public override void OnLoad()
        {
            base.OnLoad();
            if (m_grid == null)
            {
                m_grid = new GridA();
                m_grid.AutoEllipsis = true;
                m_grid.GridLineColor = COLOR.EMPTY;
                m_grid.Size = new SIZE(240, 200);
                m_grid.RegisterEvent(m_gridCellClickEvent, EVENTID.GRIDCELLCLICK);
                m_grid.RegisterEvent(m_gridKeyDownEvent, EVENTID.KEYDOWN);
                AddControl(m_grid);
                m_grid.BeginUpdate();
                //添加列
                GridColumn securityCodeColumn = new GridColumn("股票代码");
                securityCodeColumn.BackColor = CDraw.PCOLORS_BACKCOLOR;
                securityCodeColumn.BorderColor = COLOR.EMPTY;
                securityCodeColumn.Font = new FONT("Simsun", 14, true, false, false);
                securityCodeColumn.ForeColor = CDraw.PCOLORS_FORECOLOR;
                securityCodeColumn.TextAlign = ContentAlignmentA.MiddleLeft;
                securityCodeColumn.Width = 120;
                m_grid.AddColumn(securityCodeColumn);
                GridColumn securityNameColumn = new GridColumn("股票名称");
                securityNameColumn.BackColor = CDraw.PCOLORS_BACKCOLOR;
                securityNameColumn.BorderColor = COLOR.EMPTY;
                securityNameColumn.Font = new FONT("Simsun", 14, true, false, false);
                securityNameColumn.ForeColor = CDraw.PCOLORS_FORECOLOR;
                securityNameColumn.TextAlign = ContentAlignmentA.MiddleLeft;
                securityNameColumn.Width = 110;
                m_grid.AddColumn(securityNameColumn);
                m_grid.EndUpdate();
            }
            if (m_searchTextBox == null)
            {
                m_searchTextBox = new TextBoxA();
                m_searchTextBox.Location = new POINT(0, 200);
                m_searchTextBox.Size = new SIZE(240, 20);
                m_searchTextBox.Font = new FONT("SimSun", 16, true, false, false);
                m_searchTextBox.RegisterEvent(m_textBoxInputChangedEvent, EVENTID.TEXTCHANGED);
                m_searchTextBox.RegisterEvent(m_textBoxKeyDownEvent, EVENTID.KEYDOWN);
                AddControl(m_searchTextBox);
            }
        }

        /// <summary>
        /// 选中行方法
        /// </summary>
        private void OnSelectRow()
        {
            List<GridRow> rows = m_grid.SelectedRows;
            if (rows != null && rows.Count > 0)
            {
                GridRow selectedRow = rows[0];
                GSecurity security = new GSecurity();
                SecurityService.GetSecurityByCode(selectedRow.GetCell(0).Text, ref security);
                m_mainFrame.FindControl("txtCode").Text=security.m_code;
                Visible = false;
                Invalidate();
            }
        }

        /// <summary>
        /// 键盘按下方法
        /// </summary>
        /// <param name="sender">控件</param>
        /// <param name="key">按键</param>
        /// <returns>是否处理</returns>
        private void TextBoxKeyDown(object sender, char key)
        {
            if (key == 13)
            {
                OnSelectRow();
            }
            else if (key == 38 || key == 40)
            {
                OnKeyDown(key);
            }
        }

        /// <summary>
        /// 文本框输入
        /// </summary>
        /// <param name="sender">控件</param>
        private void TextBoxInputChanged(object sender)
        {
            TextBoxA control = sender as TextBoxA;
            SearchTextBox = control;
            FilterSearch();
            String text = control.Text;
            if (text != null && text.Length == 0)
            {
                Visible = false;
            }
            Invalidate();
        }

        /// <summary>
        /// 键盘方法
        /// </summary>
        /// <param name="key">按键</param>
        public override void OnKeyDown(char key)
        {
            base.OnKeyDown(key);
            if (key == 13)
            {
                OnSelectRow();
            }
            else if (key == 38 || key == 40)
            {
                m_grid.OnKeyDown(key);
            }
        }
        #endregion
    }
}
