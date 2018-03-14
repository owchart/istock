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
    /// �㼯
    /// </summary>
    public interface IPointList : IDisposable
    {
        #region �յ� 2016/6/4
        /// <summary>
        /// ��ȡ��
        /// </summary>
        /// <param name="index">����</param>
        /// <returns>����</returns>
        PointPair this[int index] { get; }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        int Count { get; }
        #endregion
    }
}
