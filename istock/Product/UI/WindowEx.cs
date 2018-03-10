/*****************************************************************************\
*                                                                             *
* WindowEx.cs - Window functions, types, and definitions.                     *
*                                                                             *
*               Version 1.00  ★★★                                          *
*                                                                             *
*               Copyright (c) 2016-2016, iTeam. All rights reserved.      *
*               Created by Todd 2016/5/8.                                     *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Drawing;

namespace OwLib
{
    /// <summary>
    /// 窗体状态
    /// </summary>
    public enum WindowStateA
    {
        /// <summary>
        /// 普通
        /// </summary>
        Normal,
        /// <summary>
        /// 最大化
        /// </summary>
        Max,
        /// <summary>
        /// 最小化
        /// </summary>
        Min
    }


    /// <summary>
    /// 重绘窗体
    /// </summary>
    public class WindowEx : WindowA
    {
        #region Lord 2016/5/8
        /// <summary>
        /// 创建窗体
        /// </summary>
        public WindowEx()
        {
            BackColor = COLOR.EMPTY;
            BorderColor = CDraw.PCOLORS_LINECOLOR3;
            CaptionHeight = 26;
            ForeColor = COLOR.EMPTY;
            Opacity = 0;
            ShadowColor = CDraw.PCOLORS_BACKCOLOR5;
            ShadowSize = 0;
        }

        /// <summary>
        /// 移动方向
        /// </summary>
        private int m_animateDirection = -1;

        /// <summary>
        /// 动画类型
        /// </summary>
        private int m_animateType = 0;

        /// <summary>
        /// 普通尺寸
        /// </summary>
        private POINT m_normalLocation;

        /// <summary>
        /// 普通尺寸
        /// </summary>
        private SIZE m_normalSize;

        /// <summary>
        /// 随机种子
        /// </summary>
        private Random m_rd = new Random();

        /// <summary>
        /// 秒表ID
        /// </summary>
        private int m_timerID = ControlA.GetNewTimerID();

        private bool m_animateMoving;

        /// <summary>
        /// 获取是否正在动画移动
        /// </summary>
        public bool AnimateMoving
        {
            get { return m_animateMoving; }
        }

        private WindowButton m_closeButton;

        /// <summary>
        /// 获取或设置关闭按钮
        /// </summary>
        public WindowButton CloseButton
        {
            get { return m_closeButton; }
            set { m_closeButton = value; }
        }

        private bool m_isChildWindow;

        /// <summary>
        /// 获取或设置是否子窗体
        /// </summary>
        public bool IsChildWindow
        {
            get { return m_isChildWindow; }
            set { m_isChildWindow = value; }
        }

        private WindowButton m_maxOrRestoreButton;

        /// <summary>
        /// 获取或设置最大化按钮
        /// </summary>
        public WindowButton MaxOrRestoreButton
        {
            get { return m_maxOrRestoreButton; }
            set { m_maxOrRestoreButton = value; }
        }

        private WindowButton m_minButton;

        /// <summary>
        /// 获取或设置最小化按钮
        /// </summary>
        public WindowButton MinButton
        {
            get { return m_minButton; }
            set { m_minButton = value; }
        }

        private WindowStateA m_windowState = WindowStateA.Normal;

        /// <summary>
        /// 获取或设置当前的状态
        /// </summary>
        public WindowStateA WindowState
        {
            get { return m_windowState; }
            set { m_windowState = value; }
        }

        /// <summary>
        /// 以动画形式隐藏
        /// </summary>
        public void AnimateHide()
        {
            m_animateType = 1;
            INativeBase native = Native;
            ControlHost host = native.Host;
            m_animateDirection = m_rd.Next(0, 4);
            StartTimer(m_timerID, 10);
            m_animateMoving = true;
            host.AllowOperate = false;
        }

        /// <summary>
        /// 以动画形式显示
        /// </summary>
        /// <param name="showDialog">是否对话框打开</param>
        public void AnimateShow(bool showDialog)
        {
            m_animateType = 0;
            INativeBase native = Native;
            ControlHost host = native.Host;
            SIZE nativeSize = native.DisplaySize;
            int width = Width, height = Height, mx = (nativeSize.cx - width) / 2, my = (nativeSize.cy - height) / 2, x = mx, y = my;
            m_animateDirection = m_rd.Next(0, 4);
            switch (m_animateDirection)
            {
                case 0:
                    x = -width;
                    break;
                case 1:
                    x = nativeSize.cx;
                    break;
                case 2:
                    y = -height;
                    break;
                case 3:
                    y = nativeSize.cy;
                    break;
            }
            POINT location = new POINT(x, y);
            Location = location;
            if (showDialog)
            {
                ShowDialog();
            }
            else
            {
                Show();
            }
            Update();
            StartTimer(m_timerID, 10);
            m_animateMoving = true;
            host.AllowOperate = false;
        }

        /// <summary>
        /// 点击按钮方法
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
                String name = control.Name;
                if (name == "btnMaxOrRestore")
                {
                    MaxOrRestore();
                }
                else if (name == "btnMin")
                {
                    Min();
                }
            }
        }

        /// <summary>
        /// 销毁控件方法
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                m_animateMoving = false;
                StopTimer(m_timerID);
            }
            base.Dispose();
        }

        /// <summary>
        /// 控件添加方法
        /// </summary>
        public override void OnAdd()
        {
            base.OnAdd();
            if (m_closeButton == null)
            {
                m_closeButton = new WindowButton();
                m_closeButton.Name = "btnClose";
                SIZE buttonSize = new SIZE(20, 20);
                m_closeButton.Size = buttonSize;
                AddControl(m_closeButton);
            }
            if (m_maxOrRestoreButton == null)
            {
                m_maxOrRestoreButton = new WindowButton();
                m_maxOrRestoreButton.Name = "btnMaxOrRestore";
                m_maxOrRestoreButton.Style = WindowButtonStyle.Max;
                SIZE buttonSize = new SIZE(20, 20);
                m_maxOrRestoreButton.Size = buttonSize;
                AddControl(m_maxOrRestoreButton);
                m_maxOrRestoreButton.RegisterEvent(new ControlMouseEvent(ClickButton), EVENTID.CLICK);
            }
            if (m_minButton == null)
            {
                m_minButton = new WindowButton();
                m_minButton.Name = "btnMin";
                m_minButton.Style = WindowButtonStyle.Min;
                SIZE buttonSize = new SIZE(20, 20);
                m_minButton.Size = buttonSize;
                AddControl(m_minButton);
                m_minButton.RegisterEvent(new ControlMouseEvent(ClickButton), EVENTID.CLICK);
            }
            Font = new FONT("微软雅黑", 14, true, false, false);
        }

        /// <summary>
        /// 拖动开始方法
        /// </summary>
        /// <param name="startOffset">偏移启动量</param>
        public override void OnDragReady(ref POINT startOffset)
        {
            startOffset.x = 0;
            startOffset.y = 0;
        }

        /// <summary>
        /// 重绘前景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void OnPaintBackground(CPaint paint, RECT clipRect)
        {
            int width = Width;
            int height = Height;
            RECT rect = new RECT(0, 0, width, height - 1);
            long foreColor = CDraw.PCOLORS_WINDOWFORECOLOR;
            int captionHeight = CaptionHeight;
            RECT contentRect = rect;
            contentRect.top += captionHeight;
            contentRect.bottom -= 6;
            contentRect.left += 6;
            contentRect.right -= 6;
            paint.BeginPath();
            paint.AddRect(contentRect);
            paint.ExcludeClipPath();
            paint.ClosePath();
            paint.FillGradientRect(CDraw.PCOLORS_BACKCOLOR2, CDraw.PCOLORS_BACKCOLOR2, rect, 6, 0);
            paint.SetClip(clipRect);
            paint.FillGradientRect(CDraw.PCOLORS_BACKCOLOR, CDraw.PCOLORS_BACKCOLOR2, new RECT(0, 0, width, captionHeight), 6, 90);
            paint.DrawRoundRect(COLOR.CONTROLBORDER, 1, 0, rect, 6);
            if (contentRect.right - contentRect.left > 0 && contentRect.bottom - contentRect.top > 0)
            {
                contentRect.top -= 1;
                contentRect.left -= 1;
                contentRect.right += 1;
                contentRect.bottom += 1;
                paint.FillGradientRect(CDraw.PCOLORS_BACKCOLOR2, CDraw.PCOLORS_BACKCOLOR2, contentRect, 6, 0);
                //paint.FillRect(CDraw.PCOLORS_WINDOWCONTENTBACKCOLOR, contentRect);
            }
            paint.DrawLine(COLOR.CONTROLBORDER, 1, 0, 0, captionHeight - 1, width, captionHeight - 1);
            CDraw.DrawText(paint, Text, foreColor, Font, 10, 4);
        }

        /// <summary>
        /// 秒表方法
        /// </summary>
        /// <param name="timerID">秒表ID</param>
        public override void OnTimer(int timerID)
        {
            base.OnTimer(timerID);
            if (m_timerID == timerID)
            {
                INativeBase native = Native;
                ControlHost host = native.Host;
                SIZE nativeSize = native.DisplaySize;
                int x = Left, y = Top, width = Width, height = Height;
                if (m_animateType == 0)
                {
                    int xSub = nativeSize.cx / 4;
                    int ySub = nativeSize.cy / 4;
                    int mx = (nativeSize.cx - width) / 2;
                    int my = (nativeSize.cy - height) / 2;
                    float opacity = Opacity;
                    opacity += 0.1F;
                    if (opacity > 1)
                    {
                        opacity = 1;
                    }
                    Opacity = opacity;
                    bool stop = false;
                    switch (m_animateDirection)
                    {
                        //从左向右
                        case 0:
                            if (x + xSub >= mx)
                            {
                                x = mx;
                                stop = true;
                            }
                            else
                            {
                                x += xSub;
                            }
                            break;
                        //从右向左
                        case 1:
                            if (x - xSub <= mx)
                            {
                                x = mx;
                                stop = true;
                            }
                            else
                            {
                                x -= xSub;
                            }
                            break;
                        //从上往下
                        case 2:
                            if (y + ySub >= my)
                            {
                                y = my;
                                stop = true;
                            }
                            else
                            {
                                y += ySub;
                            }
                            break;
                        //从下往上
                        case 3:
                            if (y - ySub <= my)
                            {
                                y = my;
                                stop = true;
                            }
                            else
                            {
                                y -= ySub;
                            }
                            break;
                    }
                    if (stop)
                    {
                        Opacity = 1;
                        m_animateMoving = false;
                        StopTimer(m_timerID);
                        host.AllowOperate = true;
                    }
                }
                else
                {
                    int xSub = nativeSize.cx / 4;
                    int ySub = nativeSize.cy / 4;
                    float opacity = Opacity;
                    opacity -= 0.1F;
                    if (opacity < 0)
                    {
                        opacity = 0;
                    }
                    Opacity = opacity;
                    bool stop = false;
                    switch (m_animateDirection)
                    {
                        //从右向左
                        case 0:
                            if (x - xSub <= -width)
                            {
                                x = 0;
                                stop = true;
                            }
                            else
                            {
                                x -= xSub;
                            }
                            break;
                        //从左向右
                        case 1:
                            if (x + xSub >= nativeSize.cx)
                            {
                                x = 0;
                                stop = true;
                            }
                            else
                            {
                                x += xSub;
                            }
                            break;
                        //从下往上
                        case 2:
                            if (y - ySub <= -height)
                            {
                                y = 0;
                                stop = true;
                            }
                            else
                            {
                                y -= ySub;
                            }
                            break;
                        //从上往下
                        case 3:
                            if (y + ySub >= nativeSize.cy)
                            {
                                y = 0;
                                stop = true;
                            }
                            else
                            {
                                y += ySub;
                            }
                            break;
                    }
                    if (stop)
                    {
                        Opacity = 0;
                        m_animateMoving = false;
                        StopTimer(m_timerID);
                        host.AllowOperate = true;
                        Hide();
                    }
                }
                POINT location = new POINT(x, y);
                Location = location;
                native.Invalidate();
            }
        }

        /// <summary>
        /// 最大化或恢复
        /// </summary>
        public void MaxOrRestore()
        {
            if (m_windowState == WindowStateA.Normal)
            {
                m_normalLocation = Location;
                m_normalSize = Size;
                Dock = DockStyleA.Fill;
                m_windowState = WindowStateA.Max;
                POINT maxLocation = new POINT(0, 0);
                Location = maxLocation;
                SIZE maxSize = Native.DisplaySize;
                Size = maxSize;
                m_maxOrRestoreButton.Style = WindowButtonStyle.Restore;
                Native.Update();
                Native.Invalidate();
            }
            else
            {
                Dock = DockStyleA.None;
                m_windowState = WindowStateA.Normal;
                Location = m_normalLocation;
                Size = m_normalSize;
                m_maxOrRestoreButton.Style = WindowButtonStyle.Max;
                Native.Update();
                Native.Invalidate();
            }
        }

        /// <summary>
        /// 最小化
        /// </summary>
        public void Min()
        {
            m_normalLocation = Location;
            m_normalSize = Size;
            Dock = DockStyleA.None;
            m_windowState = WindowStateA.Min;
            m_maxOrRestoreButton.Style = WindowButtonStyle.Restore;
            SIZE minSize = new SIZE(150, CaptionHeight);
            Size = minSize;
            Update();
            Native.Invalidate();
        }

        /// <summary>
        /// 布局改变方法
        /// </summary>
        public override void Update()
        {
            base.Update();
            int width = Width;
            if (m_closeButton != null)
            {
                POINT location = new POINT(width - 26, 3);
                m_closeButton.Location = location;
            }
            if (m_maxOrRestoreButton != null)
            {
                POINT location = new POINT(width - 48, 3);
                m_maxOrRestoreButton.Location = location;
            }
            if (m_minButton != null)
            {
                POINT location = new POINT(width - 70, 3);
                m_minButton.Location = location;
            }
        }
        #endregion
    }
}
