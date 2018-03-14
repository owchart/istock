/*****************************************************************************\
*                                                                             *
* Location.cs -  Location functions, types, and definitions                   *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
******************************************************************************/

using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OwLib
{
    /// <summary>
    /// 位置
    /// </summary>
    [Serializable]
    public class Location
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建位置
        /// </summary>
        public Location()
            : this(0, 0, CoordType.ChartFraction)
        {
        }

        /// <summary>
        /// 创建位置
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="coordType">坐标轴类型</param>
        public Location(double x, double y, CoordType coordType)
            :
                this(x, y, coordType, AlignH.Left, AlignV.Top)
        {
        }

        /// <summary>
        /// 创建位置
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="coordType">坐标轴类型</param>
        /// <param name="alignH">是否横向对齐</param>
        /// <param name="alignV">是否纵向对齐</param>
        public Location(double x, double y, CoordType coordType, AlignH alignH, AlignV alignV)
        {
            m_x = x;
            m_y = y;
            m_width = 0;
            m_height = 0;
            m_coordinateFrame = coordType;
            m_alignH = alignH;
            m_alignV = alignV;
        }

        /// <summary>
        /// 创建位置
        /// </summary>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="coordType">坐标轴类型</param>
        /// <param name="alignH">是否横向对齐</param>
        /// <param name="alignV">是否纵向对齐</param>
        public Location(double x, double y, double width, double height,
            CoordType coordType, AlignH alignH, AlignV alignV)
            :
                this(x, y, coordType, alignH, alignV)
        {
            m_width = width;
            m_height = height;
        }

        /// <summary>
        /// 创建位置
        /// </summary>
        /// <param name="rhs">其他对象</param>
        public Location(Location rhs)
        {
            m_x = rhs.m_x;
            m_y = rhs.m_y;
            m_width = rhs.m_width;
            m_height = rhs.m_height;
            m_coordinateFrame = rhs.CoordinateFrame;
            m_alignH = rhs.AlignH;
            m_alignV = rhs.AlignV;
        }

        private AlignH m_alignH;

        /// <summary>
        /// 获取或设置横轴对齐
        /// </summary>
        public AlignH AlignH
        {
            get { return m_alignH; }
            set { m_alignH = value; }
        }

        private AlignV m_alignV;

        /// <summary>
        /// 获取或设置纵轴对齐
        /// </summary>
        public AlignV AlignV
        {
            get { return m_alignV; }
            set { m_alignV = value; }
        }

        /// <summary>
        /// 获取右下
        /// </summary>
        public PointF BottomRight
        {
            get { return new PointF((float)this.X2, (float)this.Y2); }
        }

        private CoordType m_coordinateFrame;

        /// <summary>
        /// 获取或设置坐标类型
        /// </summary>
        public CoordType CoordinateFrame
        {
            get { return m_coordinateFrame; }
            set { m_coordinateFrame = value; }
        }

        private double m_height;

        /// <summary>
        /// 获取或设置高度
        /// </summary>
        public double Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        /// <summary>
        /// 获取或设置区域
        /// </summary>
        public RectangleF Rect
        {
            get { return new RectangleF((float)m_x, (float)m_y, (float)m_width, (float)m_height); }
            set
            {
                m_x = value.X;
                m_y = value.Y;
                m_width = value.Width;
                m_height = value.Height;
            }
        }

        /// <summary>
        /// 获取设置左上
        /// </summary>
        public PointF TopLeft
        {
            get { return new PointF((float)m_x, (float)m_y); }
            set { m_x = value.X; m_y = value.Y; }
        }

        private double m_width;

        /// <summary>
        /// 获取或设置宽度
        /// </summary>
        public double Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        private double m_x;

        /// <summary>
        /// 获取或设置横坐标
        /// </summary>
        public double X
        {
            get { return m_x; }
            set { m_x = value; }
        }

        /// <summary>
        /// 获取或设置横坐标1
        /// </summary>
        public double X1
        {
            get { return m_x; }
            set { m_x = value; }
        }

        public double X2
        {
            get { return m_x + m_width; }
        }

        private double m_y;

        /// <summary>
        /// 获取或设置纵坐标
        /// </summary>
        public double Y
        {
            get { return m_y; }
            set { m_y = value; }
        }

        /// <summary>
        /// 获取或设置横坐标2
        /// </summary>
        public double Y1
        {
            get { return m_y; }
            set { m_y = value; }
        }

        /// <summary>
        /// 获取或设置横坐标3
        /// </summary>
        public double Y2
        {
            get { return m_y + m_height; }
        }

        /// <summary>
        /// 转换点
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns>转换后的点</returns>
        public PointF Transform(PaneBase pane)
        {
            return Transform(pane, m_x, m_y,
                        m_coordinateFrame);
        }

        /// <summary>
        /// 转换点
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="x">横坐标值</param>
        /// <param name="y">纵坐标值</param>
        /// <param name="coord">坐标轴类型</param>
        /// <returns>转换后的点</returns>
        public static PointF Transform(PaneBase pane, double x, double y, CoordType coord)
        {
            return pane.TransformCoord(x, y, coord);
        }

        /// <summary>
        /// 转换左上点
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <returns>转换后的点</returns>
        public PointF TransformTopLeft(PaneBase pane, float width, float height)
        {
            PointF pt = Transform(pane);
            if (m_alignH == AlignH.Right)
                pt.X -= width;
            else if (m_alignH == AlignH.Center)
                pt.X -= width / 2.0F;
            if (m_alignV == AlignV.Bottom)
                pt.Y -= height;
            else if (m_alignV == AlignV.Center)
                pt.Y -= height / 2.0F;
            return pt;
        }

        /// <summary>
        /// 转换左上点
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns>转换后的点</returns>
        public PointF TransformTopLeft(PaneBase pane)
        {
            return Transform(pane);
        }

        /// <summary>
        /// 转换右下点
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns>右下点</returns>
        public PointF TransformBottomRight(PaneBase pane)
        {
            return Transform(pane, this.X2, this.Y2, m_coordinateFrame);
        }

        /// <summary>
        /// 转换区域
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns>区域</returns>
        public RectangleF TransformRect(PaneBase pane)
        {
            PointF pix1 = TransformTopLeft(pane);
            PointF pix2 = TransformBottomRight(pane);
            return new RectangleF(pix1.X, pix1.Y, Math.Abs(pix2.X - pix1.X), Math.Abs(pix2.Y - pix1.Y));
        }
        #endregion
    }
}
