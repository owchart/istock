/*****************************************************************************\
*                                                                             *
* GraphObjList.cs -   GraphObjList functions, types, and definitions          *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using System.Drawing;
using System.Collections.Generic;

namespace OwLib
{
    /// <summary>
    /// 图形列表
    /// </summary>
    [Serializable]
    public class GraphObjList : List<GraphObj>, IDisposable
    {
        #region 陶德 2016/6/4
        /// <summary>
        /// 创建图形列表
        /// </summary>
        public GraphObjList()
        {
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public void Dispose()
        {
            foreach (GraphObj obj in this)
                obj.Dispose();
            this.Clear();
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="zOrder">层次</param>
        public void Draw(Graphics g, PaneBase pane, float scaleFactor,
                    ZOrder zOrder)
        {
            for (int i = 0; i < this.Count; i++)
            {
                GraphObj item = this[i];
                if (item.ZOrder == zOrder && item.IsVisible)
                {
                    Region region = null;
                    if (item.IsClippedToChartRect && pane is GraphPane)
                    {
                        region = g.Clip.Clone();
                        g.SetClip(((GraphPane)pane).Chart.m_rect);
                    }
                    item.Draw(g, pane, scaleFactor);
                    if (item.IsClippedToChartRect && pane is GraphPane)
                        g.Clip = region;
                }
            }
        }

        /// <summary>
        /// 查找点
        /// </summary>
        /// <param name="mousePt">坐标</param>
        /// <param name="pane">图层</param>
        /// <param name="g">绘图对象</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <param name="index">索引</param>
        /// <returns>是否存在</returns>
        public bool FindPoint(PointF mousePt, PaneBase pane, Graphics g, float scaleFactor, out int index)
        {
            index = -1;
            for (int i = Count - 1; i >= 0; i--)
            {
                if (this[i].PointInBox(mousePt, pane, g, scaleFactor))
                {
                    if ((index >= 0 && this[i].ZOrder > this[index].ZOrder) || index < 0)
                        index = i;
                }
            }
            if (index >= 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="relativePos">相对位置</param>
        /// <returns>移动后索引</returns>
        public int Move(int index, int relativePos)
        {
            if (index < 0 || index >= Count)
                return -1;
            GraphObj graphObj = this[index];
            this.RemoveAt(index);
            index += relativePos;
            if (index < 0)
                index = 0;
            if (index > Count)
                index = Count;
            Insert(index, graphObj);
            return index;
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="graphObj">图形</param>
        /// <param name="relativePos">相对位置</param>
        /// <returns>移动后索引</returns>
        public int Move(GraphObj graphObj, int relativePos)
        {
            this.Remove(graphObj);
            if (relativePos < 0)
                relativePos = 0;
            if (relativePos > Count)
                relativePos = Count;
            Insert(relativePos, graphObj);
            return relativePos;
        }
        #endregion
    }
}
