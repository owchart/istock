/********************************************************************************\
*                                                                                *
* StickItem.cs -    StickItem functions, types, and definitions                  *
*                                                                                *
*               Version 1.00                                                     *
*                                                                                *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.      *
*                                                                                *
*********************************************************************************/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OwLib
{
    /// <summary>
    /// ��ͼ��
    /// </summary>
    [Serializable]
    public class StickItem : LineItem
    {
        #region �յ� 2016/6/7
        /// <summary>
        /// ������ͼ��
        /// </summary>
        /// <param name="label">��ǩ</param>
        public StickItem(String label)
            : base(label)
        {
            m_symbol.IsVisible = false;
        }

        /// <summary>
        /// ������ͼ��
        /// </summary>
        /// <param name="label">��ǩ</param>
        /// <param name="x">������ֵ</param>
        /// <param name="y">������ֵ�б�</param>
        /// <param name="color">��ɫ</param>
        /// <param name="lineWidth">�߿�</param>
        public StickItem(String label, double[] x, double[] y, Color color, float lineWidth)
            : this(label, new PointPairList(x, y), color, lineWidth)
        {
        }

        /// <summary>
        /// ������ͼ��
        /// </summary>
        /// <param name="label">��ǩ</param>
        /// <param name="x">������ֵ�б�</param>
        /// <param name="y">������ֵ�б�</param>
        /// <param name="color">��ɫ</param>
        public StickItem(String label, double[] x, double[] y, Color color)
            : this(label, new PointPairList(x, y), color)
        {
        }

        /// <summary>
        /// ������ͼ��
        /// </summary>
        /// <param name="label">��ǩ</param>
        /// <param name="points">�㼯</param>
        /// <param name="color">��ɫ</param>
        public StickItem(String label, IPointList points, Color color)
            : this(label, points, color, LineBase.Default.Width)
        {
        }

        /// <summary>
        /// ������ͼ��
        /// </summary>
        /// <param name="label">��ǩ</param>
        /// <param name="points">�㼯</param>
        /// <param name="color">��ɫ</param>
        /// <param name="lineWidth">�߿�</param>
        public StickItem(String label, IPointList points, Color color, float lineWidth)
            : base(label, points, color, Symbol.Default.Type, lineWidth)
        {
            m_symbol.IsVisible = false;
        }

        /// <summary>
        /// ������ͼ��
        /// </summary>
        /// <param name="rhs">��������</param>
        public StickItem(StickItem rhs)
            : base(rhs)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">ͼ��</param>
        /// <returns></returns>
        internal override bool IsXIndependent(GraphPane pane)
        {
            return true;
        }
        #endregion
    }
}
