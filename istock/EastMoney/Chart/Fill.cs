/*****************************************************************************\
*                                                                             *
* Fill.cs -     Fill functions, types, and definitions                        *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OwLib
{
    /// <summary>
    /// 填充
    /// </summary>
    [Serializable]
    public class Fill : IDisposable
    {
        #region 陶德 2016/6/3
        /// <summary>
        /// 创建填充
        /// </summary>
        public Fill()
        {
            Init();
        }

        /// <summary>
        /// 创建填充
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="brush">画刷</param>
        /// <param name="type">类型</param>
        public Fill(Color color, Brush brush, FillType type)
        {
            Init();
            m_color = color;
            m_brush = brush;
            m_type = type;
        }

        /// <summary>
        /// 创建填充
        /// </summary>
        /// <param name="color">颜色</param>
        public Fill(Color color)
        {
            Init();
            m_color = color;
            if (color != Color.Empty)
                m_type = FillType.Solid;
        }

        /// <summary>
        /// 创建填充
        /// </summary>
        /// <param name="color1">颜色1</param>
        /// <param name="color2">颜色2</param>
        /// <param name="angle">角度</param>
        public Fill(Color color1, Color color2, float angle)
        {
            Init();
            m_color = color2;
            ColorBlend blend = new ColorBlend(2);
            blend.Colors[0] = color1;
            blend.Colors[1] = color2;
            blend.Positions[0] = 0.0f;
            blend.Positions[1] = 1.0f;
            m_type = FillType.Brush;
            this.CreateBrushFromBlend(blend, angle);
        }

        /// <summary>
        /// 创建填充
        /// </summary>
        /// <param name="color1">颜色1</param>
        /// <param name="color2">颜色2</param>
        public Fill(Color color1, Color color2)
            : this(color1, color2, 0.0F)
        {
        }

        /// <summary>
        /// 创建填充
        /// </summary>
        /// <param name="color1">颜色1</param>
        /// <param name="color2">颜色2</param>
        /// <param name="color3">颜色3</param>
        public Fill(Color color1, Color color2, Color color3)
            :
            this(color1, color2, color3, 0.0f)
        {
        }

        /// <summary>
        /// 创建填充
        /// </summary>
        /// <param name="color1">颜色1</param>
        /// <param name="color2">颜色2</param>
        /// <param name="color3">颜色3</param>
        /// <param name="angle">角度</param>
        public Fill(Color color1, Color color2, Color color3, float angle)
        {
            Init();
            m_color = color3;
            ColorBlend blend = new ColorBlend(3);
            blend.Colors[0] = color1;
            blend.Colors[1] = color2;
            blend.Colors[2] = color3;
            blend.Positions[0] = 0.0f;
            blend.Positions[1] = 0.5f;
            blend.Positions[2] = 1.0f;
            m_type = FillType.Brush;
            this.CreateBrushFromBlend(blend, angle);
        }

        /// <summary>
        /// 创建填充
        /// </summary>
        /// <param name="blend">混合色</param>
        public Fill(ColorBlend blend)
            :
            this(blend, 0.0F)
        {
        }

        /// <summary>
        /// 创建填充
        /// </summary>
        /// <param name="blend">混合色</param>
        /// <param name="angle">角度</param>
        public Fill(ColorBlend blend, float angle)
        {
            Init();
            m_type = FillType.Brush;
            this.CreateBrushFromBlend(blend, angle);
        }

        /// <summary>
        /// 创建填充
        /// </summary>
        /// <param name="colors">颜色数组</param>
        public Fill(Color[] colors)
            :
            this(colors, 0.0F)
        {
        }

        /// <summary>
        /// 创建填充
        /// </summary>
        /// <param name="colors">颜色数组</param>
        /// <param name="angle">角度</param>
        public Fill(Color[] colors, float angle)
        {
            Init();
            m_color = colors[colors.Length - 1];
            ColorBlend blend = new ColorBlend();
            blend.Colors = colors;
            blend.Positions = new float[colors.Length];
            blend.Positions[0] = 0.0F;
            for (int i = 1; i < colors.Length; i++)
                blend.Positions[i] = (float)i / (float)(colors.Length - 1);
            m_type = FillType.Brush;
            this.CreateBrushFromBlend(blend, angle);
        }

        /// <summary>
        /// 创建填充
        /// </summary>
        /// <param name="colors">颜色数组</param>
        /// <param name="positions">位置数组</param>
        public Fill(Color[] colors, float[] positions)
            :
            this(colors, positions, 0.0F)
        {
        }

        /// <summary>
        /// 创建填充
        /// </summary>
        /// <param name="colors">颜色数组</param>
        /// <param name="positions">位置数组</param>
        /// <param name="angle">角度</param>
        public Fill(Color[] colors, float[] positions, float angle)
        {
            Init();
            m_color = colors[colors.Length - 1];
            ColorBlend blend = new ColorBlend();
            blend.Colors = colors;
            blend.Positions = positions;
            m_type = FillType.Brush;
            this.CreateBrushFromBlend(blend, angle);
        }

        /// <summary>
        /// 创建填充
        /// </summary>
        /// <param name="image">图片</param>
        /// <param name="wrapMode">渐变方式</param>
        public Fill(Image image, WrapMode wrapMode)
        {
            Init();
            m_color = Color.White;
            m_brush = new TextureBrush(image, wrapMode);
            m_type = FillType.Brush;
            m_image = image;
            m_wrapMode = wrapMode;
        }

        /// <summary>
        /// 创建填充
        /// </summary>
        /// <param name="brush">画刷</param>
        public Fill(Brush brush)
            : this(brush, Default.IsScaled)
        {
        }

        /// <summary>
        /// 创建填充
        /// </summary>
        /// <param name="brush">画刷</param>
        /// <param name="isScaled">是否根据比例</param>
        public Fill(Brush brush, bool isScaled)
        {
            Init();
            m_isScaled = isScaled;
            m_color = Color.White;
            m_brush = (Brush)brush.Clone();
            m_type = FillType.Brush;
        }

        /// <summary>
        /// 创建填充
        /// </summary>
        /// <param name="brush">画刷</param>
        /// <param name="alignH">横向对齐</param>
        /// <param name="alignV">纵向对齐</param>
        public Fill(Brush brush, AlignH alignH, AlignV alignV)
        {
            Init();
            m_alignH = alignH;
            m_alignV = alignV;
            m_isScaled = false;
            m_color = Color.White;
            m_brush = (Brush)brush.Clone();
            m_type = FillType.Brush;
        }

        /// <summary>
        /// 创建填充
        /// </summary>
        /// <param name="rhs">其他填充</param>
        public Fill(Fill rhs)
        {
            m_color = rhs.m_color;
            m_secondaryValueGradientColor = rhs.m_color;
            if (rhs.m_brush != null)
                m_brush = (Brush)rhs.m_brush.Clone();
            else
                m_brush = null;
            m_type = rhs.m_type;
            m_alignH = rhs.AlignH;
            m_alignV = rhs.AlignV;
            m_isScaled = rhs.IsScaled;
            m_rangeMin = rhs.m_rangeMin;
            m_rangeMax = rhs.m_rangeMax;
            m_rangeDefault = rhs.m_rangeDefault;
            m_gradientBM = null;
            if (rhs.m_colorList != null)
                m_colorList = (Color[])rhs.m_colorList.Clone();
            else
                m_colorList = null;
            if (rhs.m_positionList != null)
            {
                m_positionList = (float[])rhs.m_positionList.Clone();
            }
            else
                m_positionList = null;
            if (rhs.m_image != null)
                m_image = (Image)rhs.m_image.Clone();
            else
                m_image = null;
            m_angle = rhs.m_angle;
            m_wrapMode = rhs.m_wrapMode;
        }

        /// <summary>
        /// 默认属性
        /// </summary>
        public struct Default
        {
            public static bool IsScaled = true;
            public static AlignH AlignH = AlignH.Center;
            public static AlignV AlignV = AlignV.Center;
        }

        /// <summary>
        /// 角度
        /// </summary>
        private float m_angle;

        /// <summary>
        /// 颜色列表
        /// </summary>
        private Color[] m_colorList;

        /// <summary>
        /// 渐变位图
        /// </summary>
        private Bitmap m_gradientBM;

        /// <summary>
        /// 图片缓存
        /// </summary>
        private Image m_image;

        /// <summary>
        /// 位置列表
        /// </summary>
        private float[] m_positionList;

        /// <summary>
        /// 渐变方式
        /// </summary>
        private WrapMode m_wrapMode;

        private AlignH m_alignH;

        /// <summary>
        /// 获取或设置横向对齐
        /// </summary>
        public AlignH AlignH
        {
            get { return m_alignH; }
            set { m_alignH = value; }
        }

        private AlignV m_alignV;

        /// <summary>
        /// 获取或设置纵向对齐
        /// </summary>
        public AlignV AlignV
        {
            get { return m_alignV; }
            set { m_alignV = value; }
        }

        protected Brush m_brush;

        /// <summary>
        /// 获取或设置画刷
        /// </summary>
        public Brush Brush
        {
            get { return m_brush; }
            set { m_brush = value; }
        }

        private Color m_color;

        /// <summary>
        /// 获取或设置颜色
        /// </summary>
        public Color Color
        {
            get { return m_color; }
            set
            {
                m_color = value;
                if (m_brush is LinearGradientBrush)
                {
                    m_brush = new LinearGradientBrush(new Rectangle(0, 0, 100, 100),
value, Color.White, m_angle);
                }
            }
        }

        /// <summary>
        /// 获取是否渐变类型
        /// </summary>
        public bool IsGradientValueType
        {
            get
            {
                return m_type == FillType.GradientByX || m_type == FillType.GradientByY ||
                  m_type == FillType.GradientByZ || m_type == FillType.GradientByColorValue;
            }
        }

        private bool m_isScaled;

        /// <summary>
        /// 获取或设置是否根据比例
        /// </summary>
        public bool IsScaled
        {
            get { return m_isScaled; }
            set { m_isScaled = value; }
        }

        /// <summary>
        /// 获取或设置是否可见
        /// </summary>
        public bool IsVisible
        {
            get { return m_type != FillType.None; }
            set { m_type = value ? (m_type == FillType.None ? FillType.Brush : m_type) : FillType.None; }
        }

        private double m_rangeDefault;

        /// <summary>
        /// 获取或设置默认幅度
        /// </summary>
        public double RangeDefault
        {
            get { return m_rangeDefault; }
            set { m_rangeDefault = value; }
        }

        private double m_rangeMax;

        /// <summary>
        /// 获取或设置最大值
        /// </summary>
        public double RangeMax
        {
            get { return m_rangeMax; }
            set { m_rangeMax = value; }
        }

        private double m_rangeMin;

        /// <summary>
        /// 获取或设置最小值
        /// </summary>
        public double RangeMin
        {
            get { return m_rangeMin; }
            set { m_rangeMin = value; }
        }

        private Color m_secondaryValueGradientColor;

        /// <summary>
        /// 获取或设置第二种颜色
        /// </summary>
        public Color SecondaryValueGradientColor
        {
            get { return m_secondaryValueGradientColor; }
            set { m_secondaryValueGradientColor = value; }
        }

        private FillType m_type;

        /// <summary>
        /// 获取或设置填充类型
        /// </summary>
        public FillType Type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        /// <summary>
        /// 创建混合色画刷
        /// </summary>
        /// <param name="blend">混合色</param>
        /// <param name="angle">角度</param>
        private void CreateBrushFromBlend(ColorBlend blend, float angle)
        {
            m_angle = angle;
            m_colorList = (Color[])blend.Colors.Clone();
            m_positionList = (float[])blend.Positions.Clone();
            m_brush = new LinearGradientBrush(new Rectangle(0, 0, 100, 100),
                Color.Red, Color.White, angle);
            ((LinearGradientBrush)m_brush).InterpolationColors = blend;
        }

        /// <summary>
        /// 销毁对象
        /// </summary>
        public void Dispose()
        {
            if (m_brush != null)
                m_brush.Dispose();
            if (m_gradientBM != null)
                m_gradientBM.Dispose();
            if (m_image != null)
                m_image.Dispose();
            m_colorList = null;
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="rect">区域</param>
        public void Draw(Graphics g, RectangleF rect)
        {
            Draw(g, rect, null);
        }

        /// <summary>
        /// 绘制圆角矩形
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="rect">区域</param>
        public void DrawRoundRect(Graphics g, RectangleF rect)
        {
            if (this.IsVisible)
            {
                using (Brush brush = this.MakeBrush(rect, null))
                {
                    GraphicsPath gp = CustomerShape.CreateMyRoundRectPath(rect);
                    g.FillPath(brush, gp);
                    gp.Dispose();
                }
            }
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="rect">区域</param>
        /// <param name="pt">点值</param>
        public void Draw(Graphics g, RectangleF rect, PointPair pt)
        {
            if (this.IsVisible)
            {
                using (Brush brush = this.MakeBrush(rect, pt))
                {
                    g.FillRectangle(brush, rect);
                    if (pt != null)
                    {
                        pt.TopCenterPoint = new PointF(rect.X + rect.Width * 0.5F, rect.Y);
                    }
                }
            }
        }

        /// <summary>
        /// 获取渐变色
        /// </summary>
        /// <param name="dataValue">点值</param>
        /// <returns>颜色</returns>
        internal Color GetGradientColor(PointPair dataValue)
        {
            double val;
            if (dataValue == null)
                val = m_rangeDefault;
            else if (m_type == FillType.GradientByColorValue)
                val = dataValue.ColorValue;
            else if (m_type == FillType.GradientByZ)
                val = dataValue.Z;
            else if (m_type == FillType.GradientByY)
                val = dataValue.Y;
            else
                val = dataValue.X;
            return GetGradientColor(val);
        }

        /// <summary>
        /// 获取渐变色
        /// </summary>
        /// <param name="val">数值</param>
        /// <returns>颜色</returns>
        internal Color GetGradientColor(double val)
        {
            double valueFraction;
            if (Double.IsInfinity(val) || double.IsNaN(val) || val == PointPair.Missing)
                val = m_rangeDefault;
            if (m_rangeMax - m_rangeMin < 1e-20 || val == double.MaxValue)
                valueFraction = 0.5;
            else
                valueFraction = (val - m_rangeMin) / (m_rangeMax - m_rangeMin);
            if (valueFraction < 0.0)
                valueFraction = 0.0;
            else if (valueFraction > 1.0)
                valueFraction = 1.0;
            if (m_gradientBM == null)
            {
                RectangleF rect = new RectangleF(0, 0, 100, 1);
                m_gradientBM = new Bitmap(100, 1);
                Graphics gBM = Graphics.FromImage(m_gradientBM);
                Brush tmpBrush = ScaleBrush(rect, m_brush, true);
                gBM.FillRectangle(tmpBrush, rect);
            }
            return m_gradientBM.GetPixel((int)(99.9 * valueFraction), 0);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            m_color = Color.White;
            m_secondaryValueGradientColor = Color.White;
            m_brush = null;
            m_type = FillType.None;
            m_isScaled = Default.IsScaled;
            m_alignH = Default.AlignH;
            m_alignV = Default.AlignV;
            m_rangeMin = 0.0;
            m_rangeMax = 1.0;
            m_rangeDefault = double.MaxValue;
            m_gradientBM = null;
            m_colorList = null;
            m_positionList = null;
            m_angle = 0;
            m_image = null;
            m_wrapMode = WrapMode.Tile;
        }

        /// <summary>
        /// 生成画刷
        /// </summary>
        /// <param name="rect">区域</param>
        /// <returns>画刷</returns>
        public Brush MakeBrush(RectangleF rect)
        {
            return MakeBrush(rect, null);
        }

        /// <summary>
        /// 生成画刷
        /// </summary>
        /// <param name="rect">区域</param>
        /// <param name="dataValue">点值</param>
        /// <returns>画刷</returns>
        public Brush MakeBrush(RectangleF rect, PointPair dataValue)
        {
            if (this.IsVisible && (!m_color.IsEmpty || m_brush != null))
            {
                if (rect.Height < 1.0F)
                    rect.Height = 1.0F;
                if (rect.Width < 1.0F)
                    rect.Width = 1.0F;
                if (m_type == FillType.Brush)
                {
                    return ScaleBrush(rect, m_brush, m_isScaled);
                }
                else if (IsGradientValueType)
                {
                    if (dataValue != null)
                    {
                        if (!m_secondaryValueGradientColor.IsEmpty)
                        {
                            Fill tmpFill = new Fill(m_secondaryValueGradientColor,
                                    GetGradientColor(dataValue), m_angle);
                            return tmpFill.MakeBrush(rect);
                        }
                        else
                            return new SolidBrush(GetGradientColor(dataValue));
                    }
                    else if (m_rangeDefault != double.MaxValue)
                    {
                        if (!m_secondaryValueGradientColor.IsEmpty)
                        {
                            Fill tmpFill = new Fill(m_secondaryValueGradientColor,
                                    GetGradientColor(m_rangeDefault), m_angle);
                            return tmpFill.MakeBrush(rect);
                        }
                        else
                            return new SolidBrush(GetGradientColor(m_rangeDefault));
                    }
                    else
                        return ScaleBrush(rect, m_brush, true);
                }
                else
                    return new SolidBrush(m_color);
            }
            return new SolidBrush(Color.White);
        }

        /// <summary>
        /// 获取比例画刷
        /// </summary>
        /// <param name="rect">区域</param>
        /// <param name="brush">画刷</param>
        /// <param name="isScaled">是否比例</param>
        /// <returns>画刷</returns>
        public Brush ScaleBrush(RectangleF rect, Brush brush, bool isScaled)
        {
            if (brush != null)
            {
                if (brush is SolidBrush)
                {
                    return (Brush)brush.Clone();
                }
                else if (brush is LinearGradientBrush)
                {
                    LinearGradientBrush linBrush = (LinearGradientBrush)brush.Clone();
                    if (isScaled)
                    {
                        linBrush.ScaleTransform(rect.Width / linBrush.Rectangle.Width,
                            rect.Height / linBrush.Rectangle.Height, MatrixOrder.Append);
                        linBrush.TranslateTransform(rect.Left - linBrush.Rectangle.Left,
                            rect.Top - linBrush.Rectangle.Top, MatrixOrder.Append);
                    }
                    else
                    {
                        float dx = 0,
                                dy = 0;
                        switch (m_alignH)
                        {
                            case AlignH.Left:
                                dx = rect.Left - linBrush.Rectangle.Left;
                                break;
                            case AlignH.Center:
                                dx = (rect.Left + rect.Width / 2.0F) - linBrush.Rectangle.Left;
                                break;
                            case AlignH.Right:
                                dx = (rect.Left + rect.Width) - linBrush.Rectangle.Left;
                                break;
                        }
                        switch (m_alignV)
                        {
                            case AlignV.Top:
                                dy = rect.Top - linBrush.Rectangle.Top;
                                break;
                            case AlignV.Center:
                                dy = (rect.Top + rect.Height / 2.0F) - linBrush.Rectangle.Top;
                                break;
                            case AlignV.Bottom:
                                dy = (rect.Top + rect.Height) - linBrush.Rectangle.Top;
                                break;
                        }
                        linBrush.TranslateTransform(dx, dy, MatrixOrder.Append);
                    }
                    return linBrush;
                } 
                else if (brush is TextureBrush)
                {
                    TextureBrush texBrush = (TextureBrush)brush.Clone();
                    if (isScaled)
                    {
                        texBrush.ScaleTransform(rect.Width / texBrush.Image.Width,
                            rect.Height / texBrush.Image.Height, MatrixOrder.Append);
                        texBrush.TranslateTransform(rect.Left, rect.Top, MatrixOrder.Append);
                    }
                    else
                    {
                        float dx = 0,
                                dy = 0;
                        switch (m_alignH)
                        {
                            case AlignH.Left:
                                dx = rect.Left;
                                break;
                            case AlignH.Center:
                                dx = (rect.Left + rect.Width / 2.0F);
                                break;
                            case AlignH.Right:
                                dx = (rect.Left + rect.Width);
                                break;
                        }
                        switch (m_alignV)
                        {
                            case AlignV.Top:
                                dy = rect.Top;
                                break;
                            case AlignV.Center:
                                dy = (rect.Top + rect.Height / 2.0F);
                                break;
                            case AlignV.Bottom:
                                dy = (rect.Top + rect.Height);
                                break;
                        }
                        texBrush.TranslateTransform(dx, dy, MatrixOrder.Append);
                    }
                    return texBrush;
                }
                else 
                {
                    return (Brush)brush.Clone();
                }
            }
            else
            {
                if (m_color == Color.White)
                {
                    return new LinearGradientBrush(rect, m_color, Color.White, 0F);
                }
                Color tempColor = Color.FromArgb((int)(m_color.R * 0.5), (int)(m_color.G * 0.5), (int)(m_color.B * 0.5));
                return new LinearGradientBrush(rect, m_color, tempColor, 0F);
            }
        }
        #endregion
    }
}
