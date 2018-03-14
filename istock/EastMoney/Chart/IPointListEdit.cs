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
    /// �ɱ༭�㼯
    /// </summary>
    public interface IPointListEdit : IPointList
    {
        #region �յ� 2016/6/4
        /// <summary>
        /// ��ȡ��
        /// </summary>
        /// <param name="index">����</param>
        /// <returns>��</returns>
        new PointPair this[int index] { get; set; }

        /// <summary>
        /// ��ӵ�
        /// </summary>
        /// <param name="point">��</param>
        void Add(PointPair point);
        
        /// <summary>
        /// ��ӵ�
        /// </summary>
        /// <param name="x">������ֵ</param>
        /// <param name="y">������ֵ</param>
        void Add(double x, double y);

        /// <summary>
        /// ���
        /// </summary>
        void Clear();

        /// <summary>
        /// �Ƴ�
        /// </summary>
        /// <param name="index">����</param>
        void RemoveAt(int index);
        #endregion
    }
}
