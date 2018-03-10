/*****************************************************************************\
*                                                                             *
* WindowButton.cs -    Window button functions, types, and definitions             *
*                                                                             *
*               Version 4.00 ★★★                                           *
*                                                                             *
*               Copyright (c) 2016-2016, Lord's UI. All rights reserved.      *
*                                                                             *
*******************************************************************************/



using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
namespace OwLib
{
    /// <summary>
    /// 窗体按钮样式
    /// </summary>
    public enum WindowButtonStyle
    {
        Close, //关闭
        Max, //最大化
        Min, //最小化
        Restore //恢复
    }

    /// <summary>
    /// 窗体按钮
    /// </summary>
    public class WindowButton : ButtonA
    {
        /// <summary>
        /// 创建按钮
        /// </summary>
        public WindowButton()
        {
            Size = new SIZE(150, 150);
        }

        private bool m_isEllipse = true;

        /// <summary>
        /// 获取或设置是否是圆形按钮
        /// </summary>
        public bool IsEllipse
        {
            get { return m_isEllipse; }
            set { m_isEllipse = value; }
        }

        private WindowButtonStyle m_style = WindowButtonStyle.Close;

        /// <summary>
        /// 获取或设置样式
        /// </summary>
        public WindowButtonStyle Style
        {
            get { return m_style; }
            set { m_style = value; }
        }

        /// <summary>
        /// 获取正在绘制的背景色
        /// </summary>
        /// <returns></returns>
        protected override long GetPaintingBackColor()
        {
            INativeBase native = Native;
            if (m_style == WindowButtonStyle.Close)
            {
                if (native.PushedControl == this)
                {
                    return COLOR.ARGB(255, 0, 0);
                }
                else if (native.HoveredControl == this)
                {
                    return COLOR.ARGB(255, 150, 150);
                }
                else
                {
                    return COLOR.ARGB(255, 80, 80);
                }
            }
            else if (m_style == WindowButtonStyle.Min)
            {
                if (native.PushedControl == this)
                {
                    return COLOR.ARGB(0, 255, 0);
                }
                else if (native.HoveredControl == this)
                {
                    return COLOR.ARGB(150, 255, 150);
                }
                else
                {
                    return COLOR.ARGB(80, 255, 80);
                }
            }
            else
            {
                if (native.PushedControl == this)
                {
                    return COLOR.ARGB(255, 255, 0);
                }
                else if (native.HoveredControl == this)
                {
                    return COLOR.ARGB(255, 255, 150);
                }
                else
                {
                    return COLOR.ARGB(255, 255, 80);
                }
            }
        }

        /// <summary>
        /// 绘制背景
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void OnPaintBackground(CPaint paint, RECT clipRect)
        {
            int width = Width, height = Height;
            float xRate = (float)width / 200;
            float yRate = (float)height / 200;
            RECT drawRect = new RECT(0, 0, width - 1, height - 1);
            if (m_isEllipse)
            {
                paint.FillEllipse(GetPaintingBackColor(), drawRect);
            }
            else
            {
                paint.FillRect(GetPaintingBackColor(), drawRect);
            }
            long foreColor = GetPaintingForeColor();
            float lineWidth = 10 * xRate;
            if (m_style == WindowButtonStyle.Close)
            {
                paint.SetLineCap(2, 2);
                paint.DrawLine(foreColor, lineWidth, 0, (int)(135 * xRate), (int)(70 * yRate), (int)(70 * xRate), (int)(135 * yRate));
                paint.DrawLine(foreColor, lineWidth, 0, (int)(70 * xRate), (int)(70 * yRate), (int)(135 * xRate), (int)(135 * yRate));
            }
            else if (m_style == WindowButtonStyle.Max)
            {
                paint.SetLineCap(2, 2);
                paint.DrawLine(foreColor, lineWidth, 0, (int)(80 * xRate), (int)(80 * yRate), (int)(60 * xRate), (int)(60 * yRate));
                paint.DrawLine(foreColor, lineWidth, 0, (int)(125 * xRate), (int)(145 * yRate), (int)(145 * xRate), (int)(145 * yRate));
                paint.DrawLine(foreColor, lineWidth, 0, (int)(145 * xRate), (int)(125 * yRate), (int)(145 * xRate), (int)(145 * yRate));
                paint.DrawLine(foreColor, lineWidth, 0, (int)(125 * xRate), (int)(125 * yRate), (int)(145 * xRate), (int)(145 * yRate));
                paint.DrawLine(foreColor, lineWidth, 0, (int)(60 * xRate), (int)(80 * yRate), (int)(60 * xRate), (int)(60 * yRate));
                paint.DrawLine(foreColor, lineWidth, 0, (int)(80 * xRate), (int)(60 * yRate), (int)(60 * xRate), (int)(60 * yRate));
                paint.DrawLine(foreColor, lineWidth, 0, (int)(125 * xRate), (int)(80 * yRate), (int)(145 * xRate), (int)(60 * yRate));
                paint.DrawLine(foreColor, lineWidth, 0, (int)(145 * xRate), (int)(80 * yRate), (int)(145 * xRate), (int)(60 * yRate));
                paint.DrawLine(foreColor, lineWidth, 0, (int)(125 * xRate), (int)(60 * yRate), (int)(145 * xRate), (int)(60 * yRate));
                paint.DrawLine(foreColor, lineWidth, 0, (int)(80 * xRate), (int)(125 * yRate), (int)(60 * xRate), (int)(145 * yRate));
                paint.DrawLine(foreColor, lineWidth, 0, (int)(60 * xRate), (int)(125 * yRate), (int)(60 * xRate), (int)(145 * yRate));
                paint.DrawLine(foreColor, lineWidth, 0, (int)(80 * xRate), (int)(145 * yRate), (int)(60 * xRate), (int)(145 * yRate));
            }
            else if (m_style == WindowButtonStyle.Min)
            {
                paint.SetLineCap(2, 2);
                paint.DrawLine(foreColor, lineWidth, (int)(0 * xRate), (int)(60 * yRate), (int)(105 * xRate), (int)(135 * xRate), (int)(105 * yRate));
            }
            else if (m_style == WindowButtonStyle.Restore)
            {
                paint.SetLineCap(2, 2);
                paint.DrawLine(foreColor, lineWidth, (int)(0 * xRate), (int)(90 * yRate), (int)(90 * xRate), (int)(70 * xRate), (int)(70 * yRate));
                paint.DrawLine(foreColor, lineWidth, (int)(0 * xRate), (int)(90 * yRate), (int)(90 * xRate), (int)(70 * xRate), (int)(90 * yRate));
                paint.DrawLine(foreColor, lineWidth, (int)(0 * xRate), (int)(90 * yRate), (int)(90 * xRate), (int)(90 * xRate), (int)(70 * yRate));
                paint.DrawLine(foreColor, lineWidth, (int)(0 * xRate), (int)(115 * yRate), (int)(115 * xRate), (int)(135 * xRate), (int)(135 * yRate));
                paint.DrawLine(foreColor, lineWidth, (int)(0 * xRate), (int)(115 * yRate), (int)(115 * xRate), (int)(135 * xRate), (int)(115 * yRate));
                paint.DrawLine(foreColor, lineWidth, (int)(0 * xRate), (int)(115 * yRate), (int)(115 * xRate), (int)(115 * xRate), (int)(135 * yRate));
            }
            paint.SetLineCap(0, 0);
        }

        /// <summary>
        /// 绘制边线方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void OnPaintBorder(CPaint paint, RECT clipRect)
        {
            int width = Width, height = Height;
            RECT drawRect = new RECT(0, 0, width, height);
            if (m_isEllipse)
            {
                paint.DrawEllipse(GetPaintingBorderColor(), 1, 0, drawRect);
            }
            else
            {
                paint.DrawRect(GetPaintingBorderColor(), 1, 0, drawRect);
            }
        }
    }
}
