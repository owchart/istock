/*****************************************************************************\
*                                                                             *
* PointPair.cs -   PointPair functions, types, and definitions                *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Collections;
using IComparer = System.Collections.IComparer;
using System.Collections.Generic;

namespace OwLib
{
    /// <summary>
    /// ���
    /// </summary>
    [Serializable]
    public class PointPair : PointPairBase
    {
        #region �յ� 2016/6/7
        /// <summary>
        /// �������
        /// </summary>
        public PointPair()
            : this(0, 0, 0, null)
        {
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="x">������ֵ</param>
        /// <param name="y">������ֵ</param>
        public PointPair(double x, double y)
            : this(x, y, 0, null)
        {
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="x">������ֵ</param>
        /// <param name="y">������ֵ</param>
        /// <param name="label">��ǩ</param>
        public PointPair(double x, double y, String label)
            : this(x, y, 0, label as object)
        {
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="x">������ֵ</param>
        /// <param name="y">������ֵ</param>
        /// <param name="z">����������ֵ�б�</param>
        public PointPair(double x, double y, double z)
            : this(x, y, z, null)
        {
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="x">������ֵ</param>
        /// <param name="y">������ֵ</param>
        /// <param name="z">����������ֵ�б�</param>
        /// <param name="label">��ǩ</param>
        public PointPair(double x, double y, double z, String label)
            : this(x, y, z, label as object)
        {
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="x">������ֵ</param>
        /// <param name="y">������ֵ</param>
        /// <param name="z">����������ֵ�б�</param>
        /// <param name="tag">��ǩֵ</param>
        public PointPair(double x, double y, double z, object tag)
            : base(x, y)
        {
            this.Z = z;
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="pt">����</param>
        public PointPair(PointF pt)
            : this(pt.X, pt.Y, 0, null)
        {
        }

        public double Z;
        public PointF TopCenterPoint;

        /// <summary>
        /// ��ȡ��������ɫ
        /// </summary>
        public virtual double ColorValue
        {
            get { return Z; }
            set { Z = value; }
        }

        /// <summary>
        /// �Ƿ���Ч3D
        /// </summary>
        public bool IsInvalid3D
        {
            get
            {
                return this.X == PointPair.Missing ||
                          this.Y == PointPair.Missing ||
                          this.Z == PointPair.Missing ||
                          Double.IsInfinity(this.X) ||
                          Double.IsInfinity(this.Y) ||
                          Double.IsInfinity(this.Z) ||
                          Double.IsNaN(this.X) ||
                          Double.IsNaN(this.Y) ||
                          Double.IsNaN(this.Z);
            }
        }
        #endregion
    }
}
