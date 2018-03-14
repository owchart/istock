/*****************************************************************************\
*                                                                             *
* PointPairList.cs -   PointPairList functions, types, and definitions        *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using System.Drawing;
using System.Collections.Generic;

namespace OwLib
{
    /// <summary>
    /// 点值列表
    /// </summary>
    [Serializable]
    public class PointPairList : List<PointPair>, IPointList, IPointListEdit, IDisposable
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 
        /// </summary>
        public PointPairList()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">横坐标值列表</param>
        /// <param name="y">纵坐标值列表</param>
        public PointPairList(double[] x, double[] y)
        {
            Add(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list">点集</param>
        public PointPairList(IPointList list)
        {
            int count = list.Count;
            for (int i = 0; i < count; i++)
                Add(list[i]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">横坐标值列表</param>
        /// <param name="y">纵坐标值列表</param>
        /// <param name="baseVal">基础值</param>
        public PointPairList(double[] x, double[] y, double[] baseVal)
        {
            Add(x, y, baseVal);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rhs">其他对象</param>
        public PointPairList(PointPairList rhs)
        {
            Add(rhs);
        }

        /// <summary>
        /// 
        /// </summary>
        public List<String> XValues = new List<String>();

        /// <summary>
        /// 
        /// </summary>
        public List<double> YValues = new List<double>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        public void Add(double x, double y)
        {
            PointPair point = new PointPair(x, y);
            base.Add(point);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point">点</param>
        public new void Add(PointPair point)
        {
            base.Add(new PointPair(point));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointList">点集</param>
        public void Add(PointPairList pointList)
        {
            foreach (PointPair point in pointList)
                Add(point);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">横坐标值列表</param>
        /// <param name="y">纵坐标值列表</param>
        public void Add(double[] x, double[] y)
        {
            int len = 0;
            if (x != null)
                len = x.Length;
            if (y != null && y.Length > len)
                len = y.Length;
            for (int i = 0; i < len; i++)
            {
                PointPair point = new PointPair(0, 0, 0);
                if (x == null)
                    point.X = (double)i + 1.0;
                else if (i < x.Length)
                    point.X = x[i];
                else
                    point.X = PointPair.Missing;
                if (y == null)
                    point.Y = (double)i + 1.0;
                else if (i < y.Length)
                    point.Y = y[i];
                else
                    point.Y = PointPair.Missing;
                base.Add(point);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x">横坐标值列表</param>
        /// <param name="y">纵坐标值列表</param>
        /// <param name="z">纵深轴坐标值列表</param>
        public void Add(double[] x, double[] y, double[] z)
        {
            int len = 0;
            if (x != null)
                len = x.Length;
            if (y != null && y.Length > len)
                len = y.Length;
            if (z != null && z.Length > len)
                len = z.Length;
            for (int i = 0; i < len; i++)
            {
                PointPair point = new PointPair();
                if (x == null)
                    point.X = (double)i + 1.0;
                else if (i < x.Length)
                    point.X = x[i];
                else
                    point.X = PointPair.Missing;
                if (y == null)
                    point.Y = (double)i + 1.0;
                else if (i < y.Length)
                    point.Y = y[i];
                else
                    point.Y = PointPair.Missing;
                if (z == null)
                    point.Z = (double)i + 1.0;
                else if (i < z.Length)
                    point.Z = z[i];
                else
                    point.Z = PointPair.Missing;
                base.Add(point);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            XValues.Clear();
            YValues.Clear();
        }
        #endregion
    }
}
