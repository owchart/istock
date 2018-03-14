/*****************************************************************************\
*                                                                             *
* IPointList.cs -   IPointList functions, types, and definitions              *
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
    /// 点集
    /// </summary>
    public interface IPointList : IDisposable
    {
        #region 陶德 2016/6/4
        /// <summary>
        /// 获取点
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>索引</returns>
        PointPair this[int index] { get; }

        /// <summary>
        /// 获取数量
        /// </summary>
        int Count { get; }
        #endregion
    }
}
