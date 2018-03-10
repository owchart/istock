/*****************************************************************************\
*                                                                             *
* BarrageDiv.cs - Barrage div functions, types, and definitions.              *
*                                                                             *
*               Version 1.00  ★★★                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by QiChunyou 2016/6/1.                                *
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
    /// 弹幕对象
    /// </summary>
    public class Barrage
    {
        #region 齐春友 2016/5/28
        /// <summary>
        /// 构造函数
        /// </summary>
        public Barrage()
        {

        }

        private long m_color;

        /// <summary>
        /// 获取或设置颜色
        /// </summary>
        public long Color
        {
            get { return m_color; }
            set { m_color = value; }
        }

        private FONT m_font = new FONT("SimSun", 40, true, false, false);

        /// <summary>
        /// 获取或设置字体
        /// </summary>
        public FONT Font
        {
            get { return m_font; }
            set { m_font = value; }
        }

        private int m_mode;

        /// <summary>
        /// 获取或设置模式
        /// </summary>
        public int Mode
        {
            get { return m_mode; }
            set { m_mode = value; }
        }

        private RECT m_rect;

        /// <summary>
        /// 获取或设置范围
        /// </summary>
        public RECT Rect
        {
            get { return m_rect; }
            set { m_rect = value; }
        }

        private int m_speed = 10;

        /// <summary>
        /// 获取或设置速度
        /// </summary>
        public int Speed 
        {
            get { return m_speed; }
            set { m_speed = value; }
        }

        private int m_tick = 200;

        /// <summary>
        /// 获取或设置倒计时读数
        /// </summary>
        public int Tick
        {
            get { return m_tick; }
            set { m_tick = value; }
        }

        private String m_text;

        /// <summary>
        /// 获取或设置文字
        /// </summary>
        public String Text
        {
            get { return m_text; }
            set { m_text = value; }
        }

        /// <summary>
        /// 计算位置
        /// </summary>
        public void Calculate()
        {
            m_rect.left -= m_speed;
            m_rect.right -= m_speed;         
        }
        #endregion
    }

    /// <summary>
    /// 弹幕系统
    /// </summary>
    public class BarrageDiv : ControlA
    {
        #region 齐春友 2016/06/01
        /// <summary>
        /// 创建弹幕系统
        /// </summary>
        public BarrageDiv()
        {
            BackColor = COLOR.EMPTY;
        }

        /// <summary>
        /// 弹幕列表
        /// </summary>
        private List<Barrage> m_barrages = new List<Barrage>();

        /// <summary>
        /// 系统颜色
        /// </summary>
        private long[] m_sysColors = new long[] { COLOR.ARGB(255, 255, 255), COLOR.ARGB(255,255,0), COLOR.ARGB(255, 0, 255),
            COLOR.ARGB(0, 255, 0), COLOR.ARGB(82, 255, 255), COLOR.ARGB(255, 82, 82) };

        /// <summary>
        /// 随机种子
        /// </summary>
        private Random m_rd = new Random();

        /// <summary>
        /// 计数
        /// </summary>
        private int m_tick;

        /// <summary>
        /// 秒表ID
        /// </summary>
        private int m_timerID = ControlA.GetNewTimerID();

        /// <summary>
        /// 启动弹幕
        /// </summary>
        /// <param name="barrage">弹幕参数</param>
        public void AddBarrage(Barrage barrage)
        {
            barrage.Color = m_sysColors[m_tick % 6];
            int width = Width, height = Height;
            if (width < 100)
            {
                width = 100;
            }
            if (height < 100)
            {
                height = 100;
            }
            int mode = barrage.Mode;
            if (mode == 0)
            {
                barrage.Rect = new RECT(width, m_rd.Next(0, height), width, 0);
            }
            else
            {
                int left = 0, top = 0;
                if (width > 200)
                {
                    left = m_rd.Next(0, width - 200);
                }
                if (height > 200)
                {
                    top = m_rd.Next(0, height - 200);
                }
                barrage.Rect = new RECT(left, top, left, 0);
            }
            lock (m_barrages)
            {
                m_barrages.Add(barrage);
            }
            m_tick++;
        }

        /// <summary>
        /// 是否包含坐标
        /// </summary>
        /// <param name="point">坐标</param>
        /// <returns>是否包含</returns>
        public override bool ContainsPoint(POINT point)
        {
            return false;
        }

        /// <summary>
        /// 销毁资源方法
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                StopTimer(m_timerID);
                lock (m_barrages)
                {
                    m_barrages.Clear();
                }
            }
            base.Dispose();
        }

        /// <summary>
        /// 控件加载方法
        /// </summary>
        public override void OnLoad()
        {
            base.OnLoad();
            StartTimer(m_timerID, 10);
        }

        /// <summary>
        /// 重绘前景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void OnPaintBackground(CPaint paint, RECT clipRect)
        {
            base.OnPaintBackground(paint, clipRect);
            lock (m_barrages)
            {
                int barragesSize = m_barrages.Count;
                for (int i = 0; i < barragesSize; i++)
                {
                    Barrage brg = m_barrages[i];
                    FONT font = brg.Font;
                    RECT rect = brg.Rect;
                    String str = brg.Text;
                    SIZE size = paint.TextSize(str, font);
                    rect.right = rect.left + size.cx;
                    rect.bottom = rect.top + size.cy;
                    brg.Rect = rect;
                    long color = brg.Color;
                    int mode = brg.Mode;
                    if (mode == 1)
                    {
                        int a = 0, r = 0, g = 0, b = 0;
                        COLOR.ToARGB(null, color, ref a, ref r, ref g, ref b);
                        a = a * brg.Tick / 400;
                        color = COLOR.ARGB(a, r, g, b);
                    }
                    paint.DrawText(str, color, font, rect);
                }
            }
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
                bool paint = false;
                lock (m_barrages)
                {
                    int barragesSize = m_barrages.Count;
                    if (barragesSize > 0)
                    {
                        int width = Width, height = Height;
                        for (int i = 0; i < barragesSize; i++)
                        {
                            Barrage brg = m_barrages[i];
                            int mode = brg.Mode;
                            if (mode == 0)
                            {
                                if (brg.Rect.right < 0)
                                {
                                    m_barrages.Remove(brg);
                                    i--;
                                    barragesSize--;
                                }
                                else
                                {
                                    brg.Calculate();
                                }
                                paint = true;
                            }
                            else if (mode == 1)
                            {
                                int tick = brg.Tick;
                                tick--;
                                if (tick <= 0)
                                {
                                    m_barrages.Remove(brg);
                                    i--;
                                    barragesSize--;
                                }
                                else
                                {
                                    brg.Tick = tick;
                                }
                                if (tick % 20 == 0)
                                {
                                    paint = true;
                                }
                            }
                        }
                    }
                }
                if (paint)
                {
                    Invalidate();
                }
            }
        }
        #endregion
    }
}
