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
    /// 点对的基类
    /// </summary>
    [Serializable]
    public class PointPairBase
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建点对的基类
        /// </summary>
        public PointPairBase()
            : this(0, 0)
        {
        }

        /// <summary>
        /// 创建点对的基类
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        public PointPairBase(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// 创建点对的基类
        /// </summary>
        /// <param name="pt">坐标</param>
        public PointPairBase(PointF pt)
            : this(pt.X, pt.Y)
        {
        }

        /// <summary>
        /// 创建点对的基类
        /// </summary>
        /// <param name="rhs">其他对象</param>
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
        /// 是否有效
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
        /// 比较点的方法
        /// </summary>
        /// <param name="pair">点</param>
        /// <returns>是否相等</returns>
        public static implicit operator PointF(PointPairBase pair)
        {
            return new PointF((float)pair.X, (float)pair.Y);
        }
        #endregion
    }
}
