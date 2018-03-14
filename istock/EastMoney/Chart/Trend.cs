/********************************************************************************\
*                                                                                *
* Trend.cs -    Trend functions, types, and definitions                          *
*                                                                                *
*               Version 1.00                                                     *
*                                                                                *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.      *
*                                                                                *
*********************************************************************************/

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;

namespace OwLib
{
    /// <summary>
    /// 趋势线
    /// </summary>
    public class Trend
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建趋势线
        /// </summary>
        public Trend()
        {
        }

        public Pen Pen;
        public Point StartP;
        public Point EndP;

        private double m_alpha;

        /// <summary>
        /// 
        /// </summary>
        public double Alpha 
        { 
            get { return m_alpha; } 
            set { m_alpha = value; }
        }

        private double m_beta;

        /// <summary>
        /// 
        /// </summary>
        public double Beta
        { 
            get { return m_beta; } 
            set { m_beta = value; }
        }

        private bool m_IsDrawTrend = false;

        /// <summary>
        /// 
        /// </summary>
        public bool IsDrawTrend 
        { 
            get { return m_IsDrawTrend; } 
            set { m_IsDrawTrend = value; } 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        public void Draw(Graphics g, GraphPane pane)
        {
            if (this.m_IsDrawTrend)
            {
                if (Pen == null)
                {
                    Brush brush = new SolidBrush(Color.Black);
                    Pen = new Pen(brush);
                }
                Pen.Width = 2;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.DrawLine(Pen, StartP, EndP);
            }
        }
        #endregion
    }
}
