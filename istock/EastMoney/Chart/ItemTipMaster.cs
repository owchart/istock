/*****************************************************************************\
*                                                                             *
* ItemTipMaster.cs -   ItemTipMaster functions, types, and definitions        *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace OwLib
{
    /// <summary>
    /// 提示标签
    /// </summary>
    public class ItemTipMaster : IDisposable
    {
        #region 陶德 2016/6/4
        private FontSpec m_FontSpec = new FontSpec(TextObj.Default.FontFamily, TextObj.Default.FontSize - 2,
            TextObj.Default.FontColor, TextObj.Default.FontBold, TextObj.Default.FontItalic,
            TextObj.Default.FontUnderline);

        /// <summary>
        /// 行长度
        /// </summary>
        public int lineLength = 10;

        /// <summary>
        /// 项列表
        /// </summary>
        public List<ItemTip> ItemTips = new List<ItemTip>();

        private bool m_IsShowLine = true;

        /// <summary>
        /// 是否显示横线
        /// </summary>
        public bool IsShowLine
        {
            get { return m_IsShowLine; }
            set { m_IsShowLine = value; }
        }

        /// <summary>
        /// 计算提示
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="gp">路径</param>
        /// <param name="item">线</param>
        /// <param name="point">点</param>
        /// <param name="scaleFactor">刻度因子</param>
        public void CalcItemTip(Graphics g, GraphPane gp, CurveItem item, PointPair point, float scaleFactor)
        {
            CalcItemTip(g, gp, item, point, scaleFactor, false);
        }

        /// <summary>
        /// 计算提示
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="gp">路径</param>
        /// <param name="item">线</param>
        /// <param name="point">点</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="isHorizontalBar">是否横向</param>
        public void CalcItemTip(Graphics g, GraphPane gp, CurveItem item, PointPair point, float scaleFactor, bool isHorizontalBar)
        {
            if (point.Y == PointPair.Missing)
            {
                return;
            }
            Axis xAxis = item.GetXAxis(gp);
            Axis yAxis = item.GetYAxis(gp);
            float x, y;
            if (item.IsBar && !isHorizontalBar)
            {
                x = point.TopCenterPoint.X;
                y = point.TopCenterPoint.Y;
            }
            else
            {
                x = xAxis.Scale.Transform(point.X);
                y = yAxis.Scale.Transform(point.Y);
            }
            String value;
            if (isHorizontalBar)
                if (point.X == double.MaxValue)
                    return;
            else 
                value = point.X.ToString(item.ValueFormat);
            else
                value = point.Y.ToString(item.ValueFormat);
            SizeF size = m_FontSpec.BoundingBox(g, value, scaleFactor);
            Rectangle newRect;
            if (m_IsShowLine)
            {
                newRect = new Rectangle((int)x + lineLength, (int)y - lineLength, (int)size.Width, (int)size.Height);
            }
            else
            {
                newRect = new Rectangle((int)x, (int)y, (int)size.Width, (int)size.Height);
            }
            lineLength = newRect.Height;
            Rectangle GrpahRect = new Rectangle((int)gp.Rect.X, (int)gp.Rect.Y, (int)gp.Rect.Width, (int)gp.Rect.Height);
            Rectangle ChartRect = new Rectangle((int)gp.Chart.Rect.X, (int)gp.Chart.Rect.Y, (int)gp.Chart.Rect.Width, (int)gp.Chart.Rect.Height);
            GraphBorderCollision(ChartRect, ref newRect);
            if (!NeighborCollision(ItemTips, ChartRect, ref newRect, (int)x, (int)y))
                return;
            ItemTip it = new ItemTip();
            it.Rect = newRect;
            it.IntersectionPoint = new Point((int)x, (int)y);
            it.EndPoint = new Point(newRect.X, newRect.Y);
            it.Color = item.Color;
            it.Value = value;
            ItemTips.Add(it);
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public void Dispose()
        {
            if (ItemTips != null)
            {
                ItemTips.Clear();
            }
        }

        /// <summary>
        /// 绘制提示
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="gp">路径</param>
        /// <param name="scaleFactor">刻度因子</param>
        public void DrawItemTip(Graphics g, GraphPane gp, float scaleFactor)
        {
            if (m_IsShowLine)
            {
                foreach (ItemTip it in ItemTips)
                {

                    if (it.IntersectionPoint.X < 0)
                        continue;
                    using (Pen pen = new Pen(it.Color))
                    {
                        g.DrawLine(pen, it.IntersectionPoint, it.EndPoint);
                    }

                }
            }
            foreach (ItemTip it in ItemTips)
            {

                m_FontSpec.FontColor = it.Color;
                m_FontSpec.Border.Color = it.Color;
                if (it.Rect.X < 0)
                    continue;
                m_FontSpec.Draw(g, gp, it.Value, it.Rect.X, it.Rect.Y, AlignH.Left, AlignV.Center, scaleFactor);

            }
        }

        /// <summary>
        /// 生成碰撞
        /// </summary>
        /// <param name="referRect">区域</param>
        /// <param name="newRect">新区域</param>
        private void GraphBorderCollision(Rectangle referRect, ref Rectangle newRect)
        {
            int top = newRect.Top - (newRect.Height / 2 + 3);
            int bottom = newRect.Bottom - (newRect.Height / 2 + 3);
            int left = newRect.Left - 2;
            int right = newRect.Right + 2;
            if (left < referRect.Left)
            {
                newRect.X += referRect.Left - left;
            }
            if (right > referRect.Right)
            {
                newRect.X -= right - referRect.Right;
            }
            if (top < referRect.Top)
            {
                newRect.Y += referRect.Top - top;
            }
            if (bottom > referRect.Bottom)
            {
                newRect.Y -= bottom - referRect.Bottom;
            }
        }

        /// <summary>
        /// 判断是否在矩形外
        /// </summary>
        /// <param name="rect1">区域1</param>
        /// <param name="rect2">区域2</param>
        /// <returns>是否在矩形外</returns>
        private bool IsOutOfRect(RectangleF rect1, Rectangle rect2)
        {
            if (!rect1.Contains(rect2.Left, rect2.Top)) return true;
            if (!rect1.Contains(rect2.Left, rect2.Bottom)) return true;
            if (!rect1.Contains(rect2.Right, rect2.Top)) return true;
            if (!rect1.Contains(rect2.Right, rect2.Bottom)) return true;
            return false;
        }

        /// <summary>
        /// 临近碰撞
        /// </summary>
        /// <param name="ItemTips">项列表</param>
        /// <param name="borderRect">边界区域</param>
        /// <param name="newRect">新区域</param>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <returns></returns>
        private bool NeighborCollision(List<ItemTip> ItemTips, Rectangle borderRect, ref Rectangle newRect, int x, int y)
        {
            bool collision = false;
            foreach (ItemTip iTip in ItemTips)
            {
                Rectangle referRect = iTip.Rect;
                if (referRect.IntersectsWith(newRect))
                {
                    for (int i = 1; i < 50; i++)
                    {
                        int tempLen = i * lineLength;
                        newRect.X = x; newRect.Y = y - tempLen;
                        if (referRect.IntersectsWith(newRect))
                        {
                            newRect.X = x - tempLen; newRect.Y = y - tempLen;
                            if (referRect.IntersectsWith(newRect))
                            {
                                newRect.X = x - tempLen; newRect.Y = y;

                                if (referRect.IntersectsWith(newRect))
                                {
                                    newRect.X = x - tempLen; newRect.Y = y + tempLen;
                                    if (referRect.IntersectsWith(newRect))
                                    {
                                        newRect.X = x; newRect.Y = y + tempLen;
                                        if (referRect.IntersectsWith(newRect))
                                        {
                                            newRect.X = x + tempLen; newRect.Y = y + tempLen;
                                            if (referRect.IntersectsWith(newRect))
                                            {
                                                newRect.X = x + tempLen; newRect.Y = y;
                                                if (referRect.IntersectsWith(newRect))
                                                {
                                                    continue;
                                                }
                                                else
                                                {
                                                    foreach (ItemTip iTip2 in ItemTips)
                                                    {
                                                        if (iTip2.Rect.IntersectsWith(newRect) || NotInRect(borderRect, newRect))
                                                        {
                                                            collision = true;
                                                            break;
                                                        }
                                                    }
                                                    if (collision)
                                                    {
                                                        collision = false;
                                                        continue;
                                                    }
                                                    return true;
                                                }
                                            }
                                            else
                                            {
                                                foreach (ItemTip iTip2 in ItemTips)
                                                {
                                                    if (iTip2.Rect.IntersectsWith(newRect) || NotInRect(borderRect, newRect))
                                                    {
                                                        collision = true;
                                                        break;
                                                    }
                                                }
                                                if (collision)
                                                {
                                                    collision = false;
                                                    continue;
                                                }
                                                return true;
                                            }
                                        }
                                        else
                                        {
                                            foreach (ItemTip iTip2 in ItemTips)
                                            {
                                                if (iTip2.Rect.IntersectsWith(newRect) || NotInRect(borderRect, newRect))
                                                {
                                                    collision = true;
                                                    break;
                                                }
                                            }
                                            if (collision)
                                            {
                                                collision = false;
                                                continue;
                                            }
                                            return true;
                                        }
                                    }
                                    else
                                    {
                                        foreach (ItemTip iTip2 in ItemTips)
                                        {
                                            if (iTip2.Rect.IntersectsWith(newRect) || NotInRect(borderRect, newRect))
                                            {
                                                collision = true;
                                                break;
                                            }
                                        }
                                        if (collision)
                                        {
                                            collision = false;
                                            continue;
                                        }
                                        return true;
                                    }
                                }
                                else
                                {
                                    foreach (ItemTip iTip2 in ItemTips)
                                    {
                                        if (iTip2.Rect.IntersectsWith(newRect) || NotInRect(borderRect, newRect))
                                        {
                                            collision = true;
                                            break;
                                        }
                                    }
                                    if (collision)
                                    {
                                        collision = false;
                                        continue;
                                    }
                                    return true;
                                }
                            }
                            else
                            {
                                foreach (ItemTip iTip2 in ItemTips)
                                {
                                    if (iTip2.Rect.IntersectsWith(newRect) || NotInRect(borderRect, newRect))
                                    {
                                        collision = true;
                                        break;
                                    }
                                }
                                if (collision)
                                {
                                    collision = false;
                                    continue;
                                }
                                return true;
                            }
                        }
                        else
                        {
                            foreach (ItemTip iTip2 in ItemTips)
                            {
                                if (iTip2.Rect.IntersectsWith(newRect) || NotInRect(borderRect, newRect))
                                {
                                    collision = true;
                                    break;
                                }
                            }
                            if (collision)
                            {
                                collision = false;
                                continue;
                            }
                            return true;
                        }
                    }
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 判断是否不在区域内
        /// </summary>
        /// <param name="outRect">区域</param>
        /// <param name="newRect">新区域</param>
        /// <returns>是否不在区域内</returns>
        private bool NotInRect(Rectangle outRect, Rectangle newRect)
        {
            int top = newRect.Top - (newRect.Height / 2 + 3);
            int bottom = newRect.Bottom - (newRect.Height / 2 + 3);
            int left = newRect.Left - 2;
            int right = newRect.Right + 2;
            if (left < outRect.Left || right > outRect.Right || top < outRect.Top || bottom > outRect.Bottom)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 提示项
        /// </summary>
        public class ItemTip
        {
            /// <summary>
            /// 颜色
            /// </summary>
            public Color Color;
            /// <summary>
            /// 结束点
            /// </summary>
            public Point EndPoint;
            /// <summary>
            /// 相交点
            /// </summary>
            public Point IntersectionPoint;
            /// <summary>
            /// 区域
            /// </summary>
            public Rectangle Rect;
            /// <summary>
            /// 数值
            /// </summary>
            public String Value;
        }
        #endregion
    }
}