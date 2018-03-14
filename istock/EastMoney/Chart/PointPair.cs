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
    /// 点对
    /// </summary>
    [Serializable]
    public class PointPair : PointPairBase
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建点对
        /// </summary>
        public PointPair()
            : this(0, 0, 0, null)
        {
        }

        /// <summary>
        /// 创建点对
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        public PointPair(double x, double y)
            : this(x, y, 0, null)
        {
        }

        /// <summary>
        /// 创建点对
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="label">标签</param>
        public PointPair(double x, double y, String label)
            : this(x, y, 0, label as object)
        {
        }

        /// <summary>
        /// 创建点对
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="z">纵深轴坐标值列表</param>
        public PointPair(double x, double y, double z)
            : this(x, y, z, null)
        {
        }

        /// <summary>
        /// 创建点对
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="z">纵深轴坐标值列表</param>
        /// <param name="label">标签</param>
        public PointPair(double x, double y, double z, String label)
            : this(x, y, z, label as object)
        {
        }

        /// <summary>
        /// 创建点对
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="z">纵深轴坐标值列表</param>
        /// <param name="tag">标签值</param>
        public PointPair(double x, double y, double z, object tag)
            : base(x, y)
        {
            this.Z = z;
        }

        /// <summary>
        /// 创建点对
        /// </summary>
        /// <param name="pt">坐标</param>
        public PointPair(PointF pt)
            : this(pt.X, pt.Y, 0, null)
        {
        }

        public double Z;
        public PointF TopCenterPoint;

        /// <summary>
        /// 获取或设置颜色
        /// </summary>
        public virtual double ColorValue
        {
            get { return Z; }
            set { Z = value; }
        }

        /// <summary>
        /// 是否有效3D
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
