/*****************************************************************************\
*                                                                             *
* Border.cs -     Border functions, types, and definitions                    *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OwLib
{
    /// <summary>
    /// 边框
    /// </summary>
    [Serializable]
    public class Border : LineBase
    {
        #region 陶德 2016/5/31
        /// <summary>
        /// 创建边框
        /// </summary>
        public Border()
            : base()
        {
            m_inflateFactor = Default.InflateFactor;
        }

        /// <summary>
        /// 创建边框
        /// </summary>
        /// <param name="isVisible">是否可见</param>
        /// <param name="color">颜色</param>
        /// <param name="width">宽度</param>
        public Border(bool isVisible, Color color, float width)
            :
              base(color)
        {
            m_width = width;
            m_isVisible = isVisible;
        }

        /// <summary>
        /// 创建边框
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="width">宽度</param>
        public Border(Color color, float width)
            :
                this(!color.IsEmpty, color, width)
        {
        }

        /// <summary>
        /// 创建边框
        /// </summary>
        /// <param name="rhs">其他边框</param>
        public Border(Border rhs)
            : base(rhs)
        {
            m_inflateFactor = rhs.m_inflateFactor;
        }

        /// <summary>
        /// 默认属性
        /// </summary>
        public new struct Default
        {
            public static float InflateFactor = 0.0F;
        }

        private float m_inflateFactor;

        /// <summary>
        /// 获取或设置膨胀系数
        /// </summary>
        public float InflateFactor
        {
            get { return m_inflateFactor; }
            set { m_inflateFactor = value; }
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="rect">矩形</param>
        public void Draw(Graphics g, PaneBase pane, float scaleFactor, RectangleF rect)
        {
            if (m_isVisible)
            {
                RectangleF tRect = rect;
                float scaledInflate = (float)(m_inflateFactor * scaleFactor);
                tRect.Inflate(scaledInflate, scaledInflate);
                Pen pen = GetPen(pane, scaleFactor);
                g.DrawRectangle(pen, (int)tRect.X, (int)tRect.Y, (int)tRect.Width, (int)tRect.Height);
            }
        }

        /// <summary>
        /// 绘制圆边框
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="rect">矩形</param>
        public void DrawRoundRec(Graphics g, PaneBase pane, float scaleFactor, RectangleF rect)
        {
            if (m_isVisible)
            {
                RectangleF tRect = rect;
                float scaledInflate = (float)(m_inflateFactor * scaleFactor);
                tRect.Inflate(scaledInflate, scaledInflate);
                Pen pen = GetPen(pane, scaleFactor);
                GraphicsPath gp = CustomerShape.CreateMyRoundRectPath(rect);
                g.DrawPath(pen, gp);
                gp.Dispose();
            }
        }
        #endregion
    }
}
