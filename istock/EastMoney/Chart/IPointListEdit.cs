/*****************************************************************************\
*                                                                             *
* IPointListEdit.cs -   IPointListEdit functions, types, and definitions      *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using System.Text;
namespace OwLib
{
    /// <summary>
    /// 可编辑点集
    /// </summary>
    public interface IPointListEdit : IPointList
    {
        #region 陶德 2016/6/4
        /// <summary>
        /// 获取点
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>点</returns>
        new PointPair this[int index] { get; set; }

        /// <summary>
        /// 添加点
        /// </summary>
        /// <param name="point">点</param>
        void Add(PointPair point);
        
        /// <summary>
        /// 添加点
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        void Add(double x, double y);

        /// <summary>
        /// 清除
        /// </summary>
        void Clear();

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="index">索引</param>
        void RemoveAt(int index);
        #endregion
    }
}
