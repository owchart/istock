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
    /// ͼ���б�
    /// </summary>
    [Serializable]
    public class GraphObjList : List<GraphObj>, IDisposable
    {
        #region �յ� 2016/6/4
        /// <summary>
        /// ����ͼ���б�
        /// </summary>
        public GraphObjList()
        {
        }

        /// <summary>
        /// ���ٷ���
        /// </summary>
        public void Dispose()
        {
            foreach (GraphObj obj in this)
                obj.Dispose();
            this.Clear();
        }

        /// <summary>
        /// ��ͼ����
        /// </summary>
        /// <param name="g">��ͼ����</param>
        /// <param name="pane">ͼ��</param>
        /// <param name="scaleFactor">�̶�����</param>
        /// <param name="zOrder">���</param>
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
        /// ���ҵ�
        /// </summary>
        /// <param name="mousePt">����</param>
        /// <param name="pane">ͼ��</param>
        /// <param name="g">��ͼ����</param>
        /// <param name="scaleFactor">�̶�����</param>
        /// <param name="index">����</param>
        /// <returns>�Ƿ����</returns>
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
        /// �ƶ�
        /// </summary>
        /// <param name="index">����</param>
        /// <param name="relativePos">���λ��</param>
        /// <returns>�ƶ�������</returns>
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
        /// �ƶ�
        /// </summary>
        /// <param name="graphObj">ͼ��</param>
        /// <param name="relativePos">���λ��</param>
        /// <returns>�ƶ�������</returns>
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
