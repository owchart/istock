/*******************************************************************************\
*                                                                               *
* RibbonButton.cs - Ribbon button functions, types, and definitions.            *
*                                                                               *
*               Version 6.00 ★                                                 *
*                                                                               *
*               Copyright (c) 2016-2016, Order. All rights reserved.            *
*               Created by Todd.                                                *
*                                                                               *
********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    /// <summary>
    /// 透明按钮
    /// </summary>
    public class RibbonButton : ButtonA
    {
        #region Lord 2016/12/2
        /// <summary>
        /// 创建透明按钮
        /// </summary>
        public RibbonButton()
        {
            BackColor = COLOR.EMPTY;
            BorderColor = COLOR.EMPTY;
            Font = new FONT("微软雅黑", 12, false, false, false);
        }

        private int m_angle = 90;

        /// <summary>
        /// 获取或设置渐变角度
        /// </summary>
        public int Angle
        {
            get { return m_angle; }
            set { m_angle = value; }
        }

        private int m_arrowType;

        /// <summary>
        /// 获取或设置箭头类型
        /// </summary>
        public int ArrowType
        {
            get { return m_arrowType; }
            set { m_arrowType = value; }
        }

        private bool m_isClose;

        /// <summary>
        /// 获取或设置是否是关闭按钮
        /// </summary>
        public bool IsClose
        {
            get { return m_isClose; }
            set { m_isClose = value; }
        }

        /// <summary>
        /// 获取或设置是否选中
        /// </summary>
        public bool Selected
        {
            get
            {
                ControlA parent = Parent;
                if (parent != null)
                {
                    TabControlA tabControl = parent as TabControlA;
                    if (tabControl != null)
                    {
                        TabPageA selectedTabPage = tabControl.SelectedTabPage;
                        if (selectedTabPage != null)
                        {
                            if (this == selectedTabPage.HeaderButton)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 获取要绘制的前景色
        /// </summary>
        /// <returns>前景色</returns>
        protected override long GetPaintingForeColor()
        {
            if (Enabled)
            {
                if (Selected)
                {
                    return CDraw.PCOLORS_FORECOLOR4;
                }
                else
                {
                    return CDraw.PCOLORS_FORECOLOR;
                }
            }
            else
            {
                return CDraw.PCOLORS_FORECOLOR2;
            }
        }

        /// <summary>
        /// 重绘背景
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void OnPaintBackground(CPaint paint, RECT clipRect)
        {
            int width = Width;
            int height = Height;
            int mw = width / 2;
            int mh = height / 2;
            RECT drawRect = new RECT(0, 0, width, height);
            if (m_isClose)
            {
                long lineColor = CDraw.PCOLORS_LINECOLOR;
                RECT ellipseRect = new RECT(1, 1, width - 2, height - 2);
                paint.FillEllipse(CDraw.PCOLORS_UPCOLOR, ellipseRect);
                paint.DrawLine(lineColor, 2, 0, 4, 4, width - 7, height - 7);
                paint.DrawLine(lineColor, 2, 0, 4, height - 7, width - 7, 3);
            }
            else
            {
                int cornerRadius = 3;
                if (m_arrowType > 0)
                {
                    cornerRadius = 0;
                }
                ControlA parent = Parent;
                if (parent != null)
                {
                    TabControlA tabControl = parent as TabControlA;
                    if (tabControl != null)
                    {
                        cornerRadius = 0;
                    }
                }
                paint.FillGradientRect(CDraw.PCOLORS_BACKCOLOR, CDraw.PCOLORS_BACKCOLOR2, drawRect, cornerRadius, m_angle);
                paint.DrawRoundRect(COLOR.CONTROLBORDER, 1, 0, drawRect, cornerRadius);
            }
            if (m_arrowType > 0)
            {
                POINT[] points = new POINT[3];
                int dSize = Math.Min(width, height) / 4;
                switch (m_arrowType)
                {
                    //向左
                    case 1:
                        points[0] = new POINT(mw - dSize, mh);
                        points[1] = new POINT(mw + dSize, mh - dSize);
                        points[2] = new POINT(mw + dSize, mh + dSize);
                        break;
                    //向右
                    case 2:
                        points[0] = new POINT(mw + dSize, mh);
                        points[1] = new POINT(mw - dSize, mh - dSize);
                        points[2] = new POINT(mw - dSize, mh + dSize);
                        break;
                    //向上
                    case 3:
                        points[0] = new POINT(mw, mh - dSize);
                        points[1] = new POINT(mw - dSize, mh + dSize);
                        points[2] = new POINT(mw + dSize, mh + dSize);
                        break;
                    //向下
                    case 4:
                        points[0] = new POINT(mw, mh + dSize);
                        points[1] = new POINT(mw - dSize, mh - dSize);
                        points[2] = new POINT(mw + dSize, mh - dSize);
                        break;
                }
                paint.FillPolygon(CDraw.PCOLORS_FORECOLOR, points);
            }
            //绘制选中效果
            if (paint.SupportTransparent())
            {
                if (Selected)
                {
                    paint.FillRect(CDraw.PCOLORS_BACKCOLOR8, drawRect);
                }
                else
                {
                    INativeBase native = Native;
                    if (this == native.PushedControl)
                    {
                        paint.FillRect(CDraw.PCOLORS_BACKCOLOR4, drawRect);
                    }
                    else if (this == native.HoveredControl)
                    {
                        paint.FillRect(CDraw.PCOLORS_BACKCOLOR3, drawRect);
                    }
                }
            }
        }
        #endregion
    }
}
