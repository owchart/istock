/*****************************************************************************\
*                                                                             *
* PointPairBase.cs -   PointPairBase functions, types, and definitions        *
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
using IComparer = System.Collections.IComparer;

namespace OwLib
{
    /// <summary>
    /// ��ԵĻ���
    /// </summary>
    [Serializable]
    public class PointPairBase
    {
        #region �յ� 2016/6/7
        /// <summary>
        /// ������ԵĻ���
        /// </summary>
        public PointPairBase()
            : this(0, 0)
        {
        }

        /// <summary>
        /// ������ԵĻ���
        /// </summary>
        /// <param name="x">������ֵ</param>
        /// <param name="y">������ֵ</param>
        public PointPairBase(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// ������ԵĻ���
        /// </summary>
        /// <param name="pt">����</param>
        public PointPairBase(PointF pt)
            : this(pt.X, pt.Y)
        {
        }

        /// <summary>
        /// ������ԵĻ���
        /// </summary>
        /// <param name="rhs">��������</param>
        public PointPairBase(PointPairBase rhs)
        {
            this.X = rhs.X;
            this.Y = rhs.Y;
        }

        public const double Missing = Double.MaxValue;
        public const String DefaultFormat = "G";
        public double X;
        public double Y;

        /// <summary>
        /// �Ƿ���Ч
        /// </summary>
        public bool IsInvalid
        {
            get
            {
                return this.X == PointPairBase.Missing ||
                        this.Y == PointPairBase.Missing ||
                        Double.IsInfinity(this.X) ||
                        Double.IsInfinity(this.Y) ||
                        Double.IsNaN(this.X) ||
                        Double.IsNaN(this.Y);
            }
        }

        /// <summary>
        /// �Ƚϵ�ķ���
        /// </summary>
        /// <param name="pair">��</param>
        /// <returns>�Ƿ����</returns>
        public static implicit operator PointF(PointPairBase pair)
        {
            return new PointF((float)pair.X, (float)pair.Y);
        }
        #endregion
    }
}
