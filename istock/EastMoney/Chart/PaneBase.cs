/*****************************************************************************\
*                                                                             *
* PaneBase.cs -   PaneBase functions, types, and definitions                  *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
******************************************************************************/

using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.IO;

namespace OwLib
{
    /// <summary>
    /// 图层
    /// </summary>
    public abstract class PaneBase : IDisposable
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建图层
        /// </summary>
        public PaneBase()
            : this("", new RectangleF(0, 0, 0, 0))
        {
        }

        /// <summary>
        /// 创建图层
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="paneRect">区域</param>
        public PaneBase(String title, RectangleF paneRect)
        {
            m_rect = paneRect;
            m_legend = new Legend();
            m_baseDimension = Default.BaseDimension;
            m_margin = new Margin();
            m_titleGap = Default.TitleGap;
            m_isFontsScaled = Default.IsFontsScaled;
            m_isPenWidthScaled = Default.IsPenWidthScaled;
            m_fill = new Fill(Default.FillColor);
            m_border = new Border(Default.IsBorderVisible, Default.BorderColor,
                Default.BorderPenWidth);
            m_title = new GapLabel(title, Default.FontFamily,
                Default.FontSize, Default.FontColor, Default.FontBold,
                Default.FontItalic, Default.FontUnderline);
            m_title.m_fontSpec.Fill.IsVisible = false;
            m_title.m_fontSpec.Border.IsVisible = false;
            m_mark = new GapLabel(String.Empty, Default.FontFamily,
                Default.FontSize, Default.FontColor, Default.FontBold,
                Default.FontItalic, Default.FontUnderline);
            m_mark.m_fontSpec.Fill.IsVisible = false;
            m_mark.m_fontSpec.Border.IsVisible = false;
            m_graphObjList = new GraphObjList();
        }

        /// <summary>
        /// 默认属性
        /// </summary>
        public struct Default
        {
            public static bool IsShowTitle = true;
            public static String FontFamily = "Arial";
            public static float FontSize = 16;
            public static Color FontColor = Color.FromArgb(30, 30, 30);
            public static bool FontBold = true;
            public static bool FontItalic = false;
            public static bool FontUnderline = false;
            public static bool IsBorderVisible = true;
            public static Color BorderColor = Color.FromArgb(30, 30, 30);
            public static Color FillColor = Color.White;
            public static float BorderPenWidth = 1;
            public static float BaseDimension = 8.0F;
            public static bool IsPenWidthScaled = false;
            public static bool IsFontsScaled = true;
            public static float TitleGap = 0.5f;
        }

        protected RectangleF m_rect;
        protected GapLabel m_title;
        protected GapLabel m_mark;
        protected Legend m_legend;
        internal Margin m_margin;
        protected bool m_isFontsScaled;
        protected bool m_isPenWidthScaled;
        protected Fill m_fill;
        protected Border m_border;
        protected GraphObjList m_graphObjList;
        protected float m_baseDimension;
        protected float m_titleGap;
        protected AlignH m_titleAlign = AlignH.Center;
        protected AlignH m_markAlign = AlignH.Right;

        public RectangleF Rect
        {
            get { return m_rect; }
            set { m_rect = value; }
        }

        public Legend Legend
        {
            get { return m_legend; }
        }

        public Label Title
        {
            get { return m_title; }
        }

        public Label Mark
        {
            get { return m_mark; }
        }

        public AlignH TitleAlign
        {
            get { return m_titleAlign; }
            set { m_titleAlign = value; }
        }

        public AlignH MarkAlign
        {
            get { return m_titleAlign; }
            set { m_titleAlign = value; }
        }

        public Border Border
        {
            get { return m_border; }
            set { m_border = value; }
        }

        public Fill Fill
        {
            get { return m_fill; }
            set { m_fill = value; }
        }

        public GraphObjList GraphObjList
        {
            get { return m_graphObjList; }
            set { m_graphObjList = value; }
        }

        public Margin Margin
        {
            get { return m_margin; }
            set { m_margin = value; }
        }

        public float BaseDimension
        {
            get { return m_baseDimension; }
            set { m_baseDimension = value; }
        }

        public float TitleGap
        {
            get { return m_titleGap; }
            set { m_titleGap = value; }
        }

        public bool IsFontsScaled
        {
            get { return m_isFontsScaled; }
            set { m_isFontsScaled = value; }
        }

        public bool IsPenWidthScaled
        {
            get { return m_isPenWidthScaled; }
            set { m_isPenWidthScaled = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PaneBase ShallowClone()
        {
            return this.MemberwiseClone() as PaneBase;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        public virtual void Draw(Graphics g)
        {
            if (m_rect.Width <= 1 || m_rect.Height <= 1)
                return;
            float scaleFactor = this.CalcScaleFactor();
            DrawPaneFrame(g, scaleFactor);
            g.SetClip(m_rect);
            m_graphObjList.Draw(g, this, scaleFactor, ZOrder.H_BehindAll);
            DrawTitle(g, scaleFactor);
            DrawMark(g, scaleFactor);
            g.ResetClip();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <returns></returns>
        public RectangleF CalcClientRect(Graphics g, float scaleFactor)
        {
            float charHeight = m_title.m_fontSpec.GetHeight(scaleFactor);
            RectangleF innerRect = new RectangleF(
                            m_rect.Left + m_margin.Left * scaleFactor,
                            m_rect.Top + m_margin.Top * scaleFactor,
                            m_rect.Width - scaleFactor * (m_margin.Left + m_margin.Right),
                            m_rect.Height - scaleFactor * (m_margin.Top + m_margin.Bottom));
            if (m_title.m_isVisible && m_title.m_text != String.Empty)
            {
                SizeF titleSize = m_title.m_fontSpec.BoundingBox(g, m_title.m_text, scaleFactor);
                innerRect.Y += titleSize.Height + charHeight * m_titleGap;
                innerRect.Height -= titleSize.Height + charHeight * m_titleGap;
            }
            if (m_mark.m_isVisible && m_mark.m_text != String.Empty)
            {
                SizeF markSize = m_mark.m_fontSpec.BoundingBox(g, m_mark.m_text, scaleFactor);
                innerRect.Height -= markSize.Height - m_margin.Bottom * (float)scaleFactor;
            }
            return innerRect;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="scaleFactor">刻度因子</param>
        public void DrawPaneFrame(Graphics g, float scaleFactor)
        {
            m_fill.Draw(g, m_rect);
            RectangleF rect = new RectangleF(m_rect.X, m_rect.Y, m_rect.Width - 3, m_rect.Height - 1);
            m_border.Draw(g, this, scaleFactor, rect);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="scaleFactor">刻度因子</param>
        public void DrawTitle(Graphics g, float scaleFactor)
        {
            if (m_title.m_isVisible)
            {
                SizeF size = m_title.m_fontSpec.BoundingBox(g, m_title.m_text, scaleFactor);
                float x = 0F;
                if (m_titleAlign == AlignH.Center)
                {
                    x = (m_rect.Left + m_rect.Right) / 2;
                }
                else if (m_titleAlign == AlignH.Left)
                {
                    x = m_rect.Left + size.Width / 2 + 5;
                }
                else
                {
                    x = m_rect.Right - size.Width / 2 - 5;
                }
                m_title.m_fontSpec.Draw(g, this, m_title.m_text,
                    x,
                    m_rect.Top + m_margin.Top * (float)scaleFactor + size.Height / 2.0F,
                    AlignH.Center, AlignV.Center, scaleFactor);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="scaleFactor">刻度因子</param>
        protected void DrawMark(Graphics g, float scaleFactor)
        {
            if (m_mark.m_isVisible)
            {
                SizeF size = m_mark.m_fontSpec.BoundingBox(g, m_mark.m_text, scaleFactor);
                float x = 0F;
                if (m_markAlign == AlignH.Right)
                {
                    x = m_rect.Right - size.Width / 2;
                }
                else if (m_markAlign == AlignH.Center)
                {
                    x = (m_rect.Left + m_rect.Right) / 2;
                }
                else
                {
                    x = m_rect.Left + size.Width / 2;
                }
                m_mark.m_fontSpec.Draw(g, this, m_mark.m_text,
                    x,
                    m_rect.Bottom - size.Height / 2.0F - m_margin.Bottom * (float)scaleFactor,
                    AlignH.Center, AlignV.Center, scaleFactor);
            }
        }

        public virtual void ReSize(Graphics g, RectangleF rect)
        {
            m_rect = rect;
        }

        public float CalcScaleFactor()
        {
            float scaleFactor; 
            const float ASPECTLIMIT = 1.5F;
            if (!m_isFontsScaled)
                return 1.0f;
            if (m_rect.Height <= 0)
                return 1.0F;
            float length = m_rect.Width;
            float aspect = m_rect.Width / m_rect.Height;
            if (aspect > ASPECTLIMIT)
                length = m_rect.Height * ASPECTLIMIT;
            if (aspect < 1.0F / ASPECTLIMIT)
                length = m_rect.Width * ASPECTLIMIT;
            scaleFactor = length / (m_baseDimension * 72F);
            if (scaleFactor < 0.1F)
                scaleFactor = 0.1F;
            return scaleFactor;
        }

        public virtual void Dispose()
        {
            if (m_title != null)
                m_title.Dispose();
            if (m_mark != null)
                m_mark.Dispose();
            if (m_legend != null)
                m_legend.Dispose();
            if (m_fill != null)
                m_fill.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Bitmap GetImage()
        {
            return GetImage(false);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isAntiAlias">是否清晰</param>
        /// <returns></returns>
        public Bitmap GetImage(bool isAntiAlias)
        {
            Bitmap bitmap = new Bitmap((int)m_rect.Width, (int)m_rect.Height);
            using (Graphics bitmapGraphics = Graphics.FromImage(bitmap))
            {
                bitmapGraphics.TranslateTransform(-m_rect.Left, -m_rect.Top);
                this.Draw(bitmapGraphics);
            }
            return bitmap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="dpi">字体DPI</param>
        /// <param name="isAntiAlias">是否清晰</param>
        /// <returns></returns>
        public Bitmap GetImage(int width, int height, float dpi, bool isAntiAlias)
        {
            Bitmap bitmap = new Bitmap(width, height);
            bitmap.SetResolution(dpi, dpi);
            using (Graphics bitmapGraphics = Graphics.FromImage(bitmap))
            {
                MakeImage(bitmapGraphics, width, height, isAntiAlias);
            }
            return bitmap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="dpi">字体DPI</param>
        /// <returns></returns>
        public Bitmap GetImage(int width, int height, float dpi)
        {
            return GetImage(width, height, dpi, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="isAntiAlias">是否清晰</param>
        /// <returns></returns>
        public Metafile GetMetafile(int width, int height, bool isAntiAlias)
        {
            Bitmap bm = new Bitmap(1, 1);
            using (Graphics g = Graphics.FromImage(bm))
            {
                IntPtr hdc = g.GetHdc();
                Stream stream = new MemoryStream();
                Metafile metafile = new Metafile(stream, hdc, m_rect,
                            MetafileFrameUnit.Pixel, EmfType.EmfPlusDual);
                g.ReleaseHdc(hdc);
                using (Graphics metafileGraphics = Graphics.FromImage(metafile))
                {
                    metafileGraphics.PageUnit = System.Drawing.GraphicsUnit.Pixel;
                    PointF P = new PointF(width, height);
                    PointF[] PA = new PointF[] { P };
                    metafileGraphics.TransformPoints(CoordinateSpace.Page, CoordinateSpace.Device, PA);
                    MakeImage(metafileGraphics, width, height, isAntiAlias);
                    return metafile;
                }
            }
        }

        public Metafile GetMetafile(int width, int height)
        {
            return GetMetafile(width, height, false);
        }

        public Metafile GetMetafile()
        {
            Bitmap bm = new Bitmap(1, 1);
            using (Graphics g = Graphics.FromImage(bm))
            {
                IntPtr hdc = g.GetHdc();
                Stream stream = new MemoryStream();
                Metafile metafile = new Metafile(stream, hdc, m_rect,
                            MetafileFrameUnit.Pixel, EmfType.EmfOnly);
                using (Graphics metafileGraphics = Graphics.FromImage(metafile))
                {
                    metafileGraphics.TranslateTransform(-m_rect.Left, -m_rect.Top);
                    metafileGraphics.PageUnit = System.Drawing.GraphicsUnit.Pixel;
                    PointF P = new PointF(m_rect.Width, m_rect.Height);
                    PointF[] PA = new PointF[] { P };
                    metafileGraphics.TransformPoints(CoordinateSpace.Page, CoordinateSpace.Device, PA);
                    this.Draw(metafileGraphics);
                    g.ReleaseHdc(hdc);
                    return metafile;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="width">宽度</param>
        /// <param name="height">高度</param>
        /// <param name="antiAlias">是否清晰</param>
        private void MakeImage(Graphics g, int width, int height, bool antiAlias)
        {
            SetAntiAliasMode(g, antiAlias);
            PaneBase tempPane = this.ShallowClone();
            tempPane.ReSize(g, new RectangleF(0, 0, width, height));
            tempPane.Draw(g);
            Bitmap bm = new Bitmap(1, 1);
            using (Graphics bmg = Graphics.FromImage(bm))
            {
                this.ReSize(bmg, this.Rect);
                SetAntiAliasMode(bmg, antiAlias);
                this.Draw(bmg);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="penWidth">画笔宽度</param>
        /// <param name="scaleFactor">刻度因子</param>
        /// <returns></returns>
        public float ScaledPenWidth(float penWidth, float scaleFactor)
        {
            if (m_isPenWidthScaled)
                return (float)(penWidth * scaleFactor);
            else
                return penWidth;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="isAntiAlias">是否清晰</param>
        internal void SetAntiAliasMode(Graphics g, bool isAntiAlias)
        {
            if (isAntiAlias)
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            }
        }

        internal PointF TransformCoord(double x, double y, CoordType coord)
        {
            if (!(this is GraphPane) && !(coord == CoordType.PaneFraction))
            {
                coord = CoordType.PaneFraction;
                x = 0.5;
                y = 0.5;
            }
            GraphPane gPane = null;
            RectangleF chartRect = new RectangleF(0, 0, 1, 1);
            if (this is GraphPane)
            {
                gPane = this as GraphPane;
                chartRect = gPane.Chart.m_rect;
            }
            PointF ptPix = new PointF();
            if (coord == CoordType.ChartFraction)
            {
                ptPix.X = (float)(chartRect.Left + x * chartRect.Width);
                ptPix.Y = (float)(chartRect.Top + y * chartRect.Height);
            }
            else if (coord == CoordType.AxisXYScale)
            {
                ptPix.X = gPane.XAxis.Scale.Transform(x);
                ptPix.Y = gPane.YAxis.Scale.Transform(y);
            }
            else if (coord == CoordType.AxisXY2Scale)
            {
                ptPix.X = gPane.XAxis.Scale.Transform(x);
                ptPix.Y = gPane.Y2Axis.Scale.Transform(y);
            }
            else if (coord == CoordType.XScaleYChartFraction)
            {
                ptPix.X = gPane.XAxis.Scale.Transform(x);
                ptPix.Y = (float)(chartRect.Top + y * chartRect.Height);
            }
            else if (coord == CoordType.XChartFractionYScale)
            {
                ptPix.X = (float)(chartRect.Left + x * chartRect.Width);
                ptPix.Y = gPane.YAxis.Scale.Transform(y);
            }
            else if (coord == CoordType.XChartFractionY2Scale)
            {
                ptPix.X = (float)(chartRect.Left + x * chartRect.Width);
                ptPix.Y = gPane.Y2Axis.Scale.Transform(y);
            }
            else if (coord == CoordType.XChartFractionYPaneFraction)
            {
                ptPix.X = (float)(chartRect.Left + x * chartRect.Width);
                ptPix.Y = (float)(this.Rect.Top + y * m_rect.Height);
            }
            else if (coord == CoordType.XPaneFractionYChartFraction)
            {
                ptPix.X = (float)(this.Rect.Left + x * m_rect.Width);
                ptPix.Y = (float)(chartRect.Top + y * chartRect.Height);
            }
            else	
            {
                ptPix.X = (float)(m_rect.Left + x * m_rect.Width);
                ptPix.Y = (float)(m_rect.Top + y * m_rect.Height);
            }
            return ptPix;
        }
        #endregion
    }
}
