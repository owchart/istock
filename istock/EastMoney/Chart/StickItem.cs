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
    /// 棒图项
    /// </summary>
    [Serializable]
    public class StickItem : LineItem
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建棒图项
        /// </summary>
        /// <param name="label">标签</param>
        public StickItem(String label)
            : base(label)
        {
            m_symbol.IsVisible = false;
        }

        /// <summary>
        /// 创建棒图项
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值列表</param>
        /// <param name="color">颜色</param>
        /// <param name="lineWidth">线宽</param>
        public StickItem(String label, double[] x, double[] y, Color color, float lineWidth)
            : this(label, new PointPairList(x, y), color, lineWidth)
        {
        }

        /// <summary>
        /// 创建棒图项
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="x">横坐标值列表</param>
        /// <param name="y">纵坐标值列表</param>
        /// <param name="color">颜色</param>
        public StickItem(String label, double[] x, double[] y, Color color)
            : this(label, new PointPairList(x, y), color)
        {
        }

        /// <summary>
        /// 创建棒图项
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="points">点集</param>
        /// <param name="color">颜色</param>
        public StickItem(String label, IPointList points, Color color)
            : this(label, points, color, LineBase.Default.Width)
        {
        }

        /// <summary>
        /// 创建棒图项
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="points">点集</param>
        /// <param name="color">颜色</param>
        /// <param name="lineWidth">线宽</param>
        public StickItem(String label, IPointList points, Color color, float lineWidth)
            : base(label, points, color, Symbol.Default.Type, lineWidth)
        {
            m_symbol.IsVisible = false;
        }

        /// <summary>
        /// 创建棒图项
        /// </summary>
        /// <param name="rhs">其他对象</param>
        public StickItem(StickItem rhs)
            : base(rhs)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns></returns>
        internal override bool IsXIndependent(GraphPane pane)
        {
            return true;
        }
        #endregion
    }
}
