/*****************************************************************************\
*                                                                             *
* FontSpec.cs -   FontSpec functions, types, and definitions                  *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OwLib
{
    /// <summary>
    /// 字体
    /// </summary>
    [Serializable]
    public class FontSpec : IDisposable
    {
        #region 陶德 2016/6/3
        /// <summary>
        /// 创建字体
        /// </summary>
        public FontSpec()
            : this("Arial", 12, Color.FromArgb(30, 30, 30), false, false, false)
        {
        }

        /// <summary>
        /// 创建字体
        /// </summary>
        /// <param name="family">字体</param>
        /// <param name="size">尺寸</param>
        /// <param name="color">颜色</param>
        /// <param name="isBold">是否粗体</param>
        /// <param name="isItalic">是否斜体</param>
        /// <param name="isUnderline">是否下划线</param>
        public FontSpec(String family, float size, Color color, bool isBold,
            bool isItalic, bool isUnderline)
        {
            Init(family, size, color, isBold, isItalic, isUnderline,
                    Default.FillColor, Default.FillBrush, Default.FillType);
        }

        /// <summary>
        /// 创建字体
        /// </summary>
        /// <param name="family">字体</param>
        /// <param name="size">尺寸</param>
        /// <param name="color">颜色</param>
        /// <param name="isBold">是否粗体</param>
        /// <param name="isItalic">是否斜体</param>
        /// <param name="isUnderline">是否下划线</param>
        /// <param name="fillColor">填充色</param>
        /// <param name="fillBrush">填充刷</param>
        /// <param name="fillType">填充类型</param>
        public FontSpec(String family, float size, Color color, bool isBold,
                            bool isItalic, bool isUnderline, Color fillColor, Brush fillBrush,
                            FillType fillType)
        {
            Init(family, size, color, isBold, isItalic, isUnderline,
                    fillColor, fillBrush, fillType);
        }

        /// <summary>
        /// 创建字体
        /// </summary>
        /// <param name="rhs">其他字体</param>
        public FontSpec(FontSpec rhs)
        {
            m_fontColor = rhs.FontColor;
            m_family = rhs.Family;
            m_isBold = rhs.IsBold;
            m_isItalic = rhs.IsItalic;
            m_isUnderline = rhs.IsUnderline;
            m_fill = new Fill(rhs.Fill);
            m_border = new Border(rhs.Border);
            m_isAntiAlias = rhs.m_isAntiAlias;
            m_stringAlignment = rhs.StringAlignment;
            m_angle = rhs.Angle;
            m_size = rhs.Size;
            m_isDropShadow = rhs.m_isDropShadow;
            m_dropShadowColor = rhs.m_dropShadowColor;
            m_dropShadowAngle = rhs.m_dropShadowAngle;
            m_dropShadowOffset = rhs.m_dropShadowOffset;
            m_minSize = rhs.m_minSize;
            m_maxSize = rhs.m_maxSize;
            m_minSize2 = rhs.m_minSize2;
            m_maxSize2 = rhs.m_maxSize2;
            m_scaledSize = rhs.m_scaledSize;
            Remake(1.0F, m_size, ref m_scaledSize, ref m_font);
        }

        /// <summary>
        /// 默认属性
        /// </summary>
        public struct Default
        {
            public static float SuperSize = 0.85F;
            public static float SuperShift = 0.4F;
            public static Color FillColor = Color.White;
            public static Brush FillBrush = null;
            public static FillType FillType = FillType.Solid;
            public static StringAlignment StringAlignment = StringAlignment.Center;
            public static bool IsDropShadow = false;
            public static bool IsAntiAlias = false;
            public static Color DropShadowColor = Color.FromArgb(30, 30, 30);
            public static float DropShadowAngle = 45f;
            public static float DropShadowOffset = 0.05f;
            public static float MinSizeOfFont = 11F;
            public static float MaxSizeOfFont = 15F;
            public static float MinSizeOfFont2 = 11F;
            public static float MaxSizeOfFont2 = 15F;
            public static bool IsUseMaxMinLimitScenario1 = true;
        }

        private String m_family;

        /// <summary>
        /// 获取或设置字体
        /// </summary>
        public String Family
        {
            get { return m_family; }
            set
            {
                if (value != m_family)
                {
                    m_family = value;
                    Remake(m_scaledSize / m_size, this.Size, ref m_scaledSize, ref m_font);
                }
            }
        }

        private Color m_fontColor;

        /// <summary>
        /// 获取或设置字体颜色
        /// </summary>
        public Color FontColor
        {
            get { return m_fontColor; }
            set { m_fontColor = value; }
        }

        private bool m_isBold;

        /// <summary>
        /// 获取或设置是否粗体
        /// </summary>
        public bool IsBold
        {
            get { return m_isBold; }
            set
            {
                if (value != m_isBold)
                {
                    m_isBold = value;
                    Remake(m_scaledSize / m_size, this.Size, ref m_scaledSize, ref m_font);
                }
            }
        }
        private bool m_isItalic;
        private bool m_isUnderline;
        private Fill m_fill;
        private Border m_border;
        private float m_angle;
        private StringAlignment m_stringAlignment;
        private float m_size;
        private Font m_font;
        private bool m_isAntiAlias;
        private bool m_isDropShadow;
        private Color m_dropShadowColor;
        private float m_dropShadowAngle;
        private float m_dropShadowOffset;
        private Font m_superScriptFont;
        private float m_scaledSize;
        private float m_minSize;
        private float m_maxSize;
        private float m_minSize2;
        private float m_maxSize2;
        private bool m_ignoreScale;

        public bool IsItalic
        {
            get { return m_isItalic; }
            set
            {
                if (value != m_isItalic)
                {
                    m_isItalic = value;
                    Remake(m_scaledSize / m_size, this.Size, ref m_scaledSize, ref m_font);
                }
            }
        }

        public bool IsUnderline
        {
            get { return m_isUnderline; }
            set
            {
                if (value != m_isUnderline)
                {
                    m_isUnderline = value;
                    Remake(m_scaledSize / m_size, this.Size, ref m_scaledSize, ref m_font);
                }
            }
        }

        public float Angle
        {
            get { return m_angle; }
            set { m_angle = value; }
        }

        public StringAlignment StringAlignment
        {
            get { return m_stringAlignment; }
            set { m_stringAlignment = value; }
        }

        public float Size
        {
            get { return m_size; }
            set
            {
                if (value != m_size)
                {
                    Remake(m_scaledSize / m_size * value, m_size, ref m_scaledSize,
                                ref m_font);
                    m_size = value;
                }
            }
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

        public bool IsAntiAlias
        {
            get { return m_isAntiAlias; }
            set { m_isAntiAlias = value; }
        }

        public bool IsDropShadow
        {
            get { return m_isDropShadow; }
            set { m_isDropShadow = value; }
        }

        public Color DropShadowColor
        {
            get { return m_dropShadowColor; }
            set { m_dropShadowColor = value; }
        }

        public float DropShadowAngle
        {
            get { return m_dropShadowAngle; }
            set { m_dropShadowAngle = value; }
        }

        public float DropShadowOffset
        {
            get { return m_dropShadowOffset; }
            set { m_dropShadowOffset = value; }
        }

        public bool IgnoreScale
        {
            get { return m_ignoreScale; }
            set { m_ignoreScale = value; }
        }

        

        private void Init(String family, float size, Color color, bool isBold,
            bool isItalic, bool isUnderline, Color fillColor, Brush fillBrush,
            FillType fillType)
        {
            m_fontColor = color;
            m_family = family;
            m_isBold = isBold;
            m_isItalic = isItalic;
            m_isUnderline = isUnderline;
            m_size = size;
            m_angle = 0F;
            m_isAntiAlias = Default.IsAntiAlias;
            m_stringAlignment = Default.StringAlignment;
            m_isDropShadow = Default.IsDropShadow;
            m_dropShadowColor = Default.DropShadowColor;
            m_dropShadowAngle = Default.DropShadowAngle;
            m_dropShadowOffset = Default.DropShadowOffset;
            m_minSize = Default.MinSizeOfFont;
            m_maxSize = Default.MaxSizeOfFont;
            m_minSize2 = Default.MinSizeOfFont2;
            m_maxSize2 = Default.MaxSizeOfFont2;
            m_fill = new Fill(fillColor, fillBrush, fillType);
            m_border = new Border(true, Color.FromArgb(30, 30, 30), 1.0F);
            m_scaledSize = -1;
            Remake(1.0F, m_size, ref m_scaledSize, ref m_font);
        }

        private void Remake(float scaleFactor, float size, ref float scaledSize, ref Font font)
        {
            if (m_ignoreScale)
            {
                FontStyle style = FontStyle.Regular;
                style = (m_isBold ? FontStyle.Bold : style) |
                            (m_isItalic ? FontStyle.Italic : style) |
                             (m_isUnderline ? FontStyle.Underline : style);
                font = new Font(m_family, size, style, GraphicsUnit.World);
            }
            else
            {
                float newSize = size * scaleFactor;
                float oldSize = (font == null) ? 0.0f : font.Size;
                if (font == null ||
                        Math.Abs(newSize - oldSize) > 0.1 ||
                        font.Name != this.Family ||
                        font.Bold != m_isBold ||
                        font.Italic != m_isItalic ||
                        font.Underline != m_isUnderline)
                {
                    FontStyle style = FontStyle.Regular;
                    style = (m_isBold ? FontStyle.Bold : style) |
                                (m_isItalic ? FontStyle.Italic : style) |
                                 (m_isUnderline ? FontStyle.Underline : style);
                    scaledSize = size * (float)scaleFactor;
                    if (Default.IsUseMaxMinLimitScenario1)
                    {
                        scaledSize = scaledSize < m_minSize ? m_minSize : scaledSize;
                        scaledSize = scaledSize > m_maxSize ? m_maxSize : scaledSize;
                    }
                    else
                    {
                        scaledSize = scaledSize < m_minSize2 ? m_minSize2 : scaledSize;
                        scaledSize = scaledSize > m_maxSize2 ? m_maxSize2 : scaledSize;
                    }
                    font = new Font(m_family, scaledSize, style, GraphicsUnit.World);
                }
            }
        }

        private void Remake(float scaleFactor, float size, ref float scaledSize, ref Font font, bool IsScale)
        {
            float newSize = size * scaleFactor;
            font = new Font(m_family, scaledSize, font.Style, GraphicsUnit.World);
        }

        public Font GetFont(float scaleFactor)
        {
            Remake(scaleFactor, this.Size, ref m_scaledSize, ref m_font);
            return m_font;
        }

        public void Draw(Graphics g, PaneBase pane, String text, float x,
            float y, AlignH alignH, AlignV alignV,
            float scaleFactor)
        {
            this.Draw(g, pane, text, x, y, alignH, alignV,
                        scaleFactor, new SizeF());
        }

        public void Draw(Graphics g, PaneBase pane, String text, float x,
            float y, AlignH alignH, AlignV alignV,
            float scaleFactor, bool isScale)
        {
            this.Draw(g, pane, text, x, y, alignH, alignV,
                        scaleFactor, new SizeF(), isScale);
        }

        public void Draw(Graphics g, PaneBase pane, String text, float x,
            float y, AlignH alignH, AlignV alignV,
            float scaleFactor, SizeF layoutArea)
        {
            SmoothingMode sModeSave = g.SmoothingMode;
            TextRenderingHint sHintSave = g.TextRenderingHint;
            if (m_isAntiAlias)
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            SizeF sizeF;
            if (layoutArea.IsEmpty)
                sizeF = MeasureString(g, text, scaleFactor);
            else
                sizeF = MeasureString(g, text, scaleFactor, layoutArea);
            Matrix saveMatrix = g.Transform;
            g.Transform = SetupMatrix(g.Transform, x, y, sizeF, alignH, alignV, m_angle);
            RectangleF rectF = new RectangleF(-sizeF.Width / 2.0F, 0.0F,
                                sizeF.Width, sizeF.Height);
            m_fill.Draw(g, rectF);
            m_border.Draw(g, pane, scaleFactor, rectF);
            StringFormat strFormat = new StringFormat();
            strFormat.Alignment = m_stringAlignment == StringAlignment.Center ? StringAlignment.Near : m_stringAlignment;
            if (m_isDropShadow)
            {
                float xShift = (float)(Math.Cos(m_dropShadowAngle) *
                            m_dropShadowOffset * m_font.Height);
                float yShift = (float)(Math.Sin(m_dropShadowAngle) *
                            m_dropShadowOffset * m_font.Height);
                RectangleF rectD = rectF;
                rectD.Offset(xShift, yShift);
                using (SolidBrush brushD = new SolidBrush(m_dropShadowColor))
                    g.DrawString(text, m_font, brushD, rectD, strFormat);
            }
            using (SolidBrush brush = new SolidBrush(m_fontColor))
            {
                RectangleF rectForText = new RectangleF(rectF.X, rectF.Y + 1, sizeF.Width, sizeF.Height + 1);
                g.DrawString(text, m_font, brush, rectForText.X, rectForText.Y, strFormat);
            }
            g.Transform = saveMatrix;
            g.SmoothingMode = sModeSave;
            g.TextRenderingHint = sHintSave;
        }

        public void Draw(Graphics g, PaneBase pane, String text, float x,
           float y, AlignH alignH, AlignV alignV,
           float scaleFactor, SizeF layoutArea, bool isScale)
        {
            SmoothingMode sModeSave = g.SmoothingMode;
            TextRenderingHint sHintSave = g.TextRenderingHint;
            if (m_isAntiAlias)
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            SizeF sizeF;
            if (layoutArea.IsEmpty)
                sizeF = MeasureString(g, text, scaleFactor, isScale);
            else
                sizeF = MeasureString(g, text, scaleFactor, layoutArea, isScale);
            Matrix saveMatrix = g.Transform;
            g.Transform = SetupMatrix(g.Transform, x, y, sizeF, alignH, alignV, m_angle);
            RectangleF rectF = new RectangleF(-sizeF.Width / 2.0F, 0.0F,
                                sizeF.Width, sizeF.Height);
            m_fill.Draw(g, rectF);
            m_border.Draw(g, pane, scaleFactor, rectF);
            StringFormat strFormat = new StringFormat();
            strFormat.Alignment = StringAlignment.Near;
            if (m_isDropShadow)
            {
                float xShift = (float)(Math.Cos(m_dropShadowAngle) *
                            m_dropShadowOffset * m_font.Height);
                float yShift = (float)(Math.Sin(m_dropShadowAngle) *
                            m_dropShadowOffset * m_font.Height);
                RectangleF rectD = rectF;
                rectD.Offset(xShift, yShift);
                using (SolidBrush brushD = new SolidBrush(m_dropShadowColor))
                    g.DrawString(text, m_font, brushD, rectD, strFormat);
            }
            using (SolidBrush brush = new SolidBrush(m_fontColor))
            {
                g.DrawString(text, m_font, brush, rectF.X, rectF.Y + 1, strFormat);
            }
            g.Transform = saveMatrix;
            g.SmoothingMode = sModeSave;
            g.TextRenderingHint = sHintSave;
        }

        public void DrawTenPower(Graphics g, GraphPane pane, String text, float x,
            float y, AlignH alignH, AlignV alignV,
            float scaleFactor)
        {
            SmoothingMode sModeSave = g.SmoothingMode;
            TextRenderingHint sHintSave = g.TextRenderingHint;
            if (m_isAntiAlias)
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
            }
            Remake(scaleFactor, this.Size, ref m_scaledSize, ref m_font);
            float scaledSuperSize = m_scaledSize * Default.SuperSize;
            Remake(scaleFactor, this.Size * Default.SuperSize, ref scaledSuperSize,
                ref m_superScriptFont);
            SizeF size10 = g.MeasureString("10", m_font);
            SizeF sizeText = g.MeasureString(text, m_superScriptFont);
            SizeF totSize = new SizeF(size10.Width + sizeText.Width,
                                    size10.Height + sizeText.Height * Default.SuperShift);
            float charWidth = g.MeasureString("x", m_superScriptFont).Width;
            Matrix saveMatrix = g.Transform;
            g.Transform = SetupMatrix(g.Transform, x, y, totSize, alignH, alignV, m_angle);
            StringFormat strFormat = new StringFormat();
            strFormat.Alignment = m_stringAlignment;
            RectangleF rectF = new RectangleF(-totSize.Width / 2.0F, 0.0F,
                totSize.Width, totSize.Height);
            m_fill.Draw(g, rectF);
            m_border.Draw(g, pane, scaleFactor, rectF);
            using (SolidBrush brush = new SolidBrush(m_fontColor))
            {
                g.DrawString("10", m_font, brush,
                                (-totSize.Width + size10.Width) / 2.0F,
                                sizeText.Height * Default.SuperShift, strFormat);
                g.DrawString(text, m_superScriptFont, brush,
                                (totSize.Width - sizeText.Width - charWidth) / 2.0F,
                                0.0F,
                                strFormat);
            }
            g.Transform = saveMatrix;
            g.SmoothingMode = sModeSave;
            g.TextRenderingHint = sHintSave;
        }

        public float GetHeight(float scaleFactor)
        {
            Remake(scaleFactor, this.Size, ref m_scaledSize, ref m_font);
            float height = m_font.Height;
            if (m_isDropShadow)
                height += (float)(Math.Sin(m_dropShadowAngle) * m_dropShadowOffset * m_font.Height);
            return height;
        }

        public float GetWidth(Graphics g, float scaleFactor)
        {
            Remake(scaleFactor, this.Size, ref m_scaledSize, ref m_font);
            return g.MeasureString("x", m_font).Width;
        }

        public float GetWidth(Graphics g, String text, float scaleFactor)
        {
            Remake(scaleFactor, this.Size, ref m_scaledSize, ref m_font);
            float width = g.MeasureString(text, m_font).Width;
            if (m_isDropShadow)
                width += (float)(Math.Cos(m_dropShadowAngle) * m_dropShadowOffset * m_font.Height);
            return width;
        }

        public SizeF MeasureString(Graphics g, String text, float scaleFactor)
        {
            Remake(scaleFactor, this.Size, ref m_scaledSize, ref m_font);
            SizeF size = g.MeasureString(text, m_font);
            if (m_isDropShadow)
            {
                size.Width += (float)(Math.Cos(m_dropShadowAngle) *
                                m_dropShadowOffset * m_font.Height);
                size.Height += (float)(Math.Sin(m_dropShadowAngle) *
                                m_dropShadowOffset * m_font.Height);
            }
            return size;
        }

        public SizeF MeasureString(Graphics g, String text, float scaleFactor, bool isScale)
        {
            Remake(scaleFactor, this.Size, ref m_scaledSize, ref m_font, isScale);
            SizeF size = g.MeasureString(text, m_font);
            if (m_isDropShadow)
            {
                size.Width += (float)(Math.Cos(m_dropShadowAngle) *
                                m_dropShadowOffset * m_font.Height);
                size.Height += (float)(Math.Sin(m_dropShadowAngle) *
                                m_dropShadowOffset * m_font.Height);
            }
            return size;
        }

        public SizeF MeasureString(Graphics g, String text, float scaleFactor, SizeF layoutArea)
        {
            Remake(scaleFactor, this.Size, ref m_scaledSize, ref m_font);
            SizeF size = g.MeasureString(text, m_font, layoutArea);
            if (m_isDropShadow)
            {
                size.Width += (float)(Math.Cos(m_dropShadowAngle) *
                                m_dropShadowOffset * m_font.Height);
                size.Height += (float)(Math.Sin(m_dropShadowAngle) *
                                m_dropShadowOffset * m_font.Height);
            }
            return size;
        }

        public SizeF MeasureString(Graphics g, String text, float scaleFactor, SizeF layoutArea, bool IsScale)
        {
            Remake(scaleFactor, this.Size, ref m_scaledSize, ref m_font, IsScale);
            SizeF size = g.MeasureString(text, m_font, layoutArea);
            if (m_isDropShadow)
            {
                size.Width += (float)(Math.Cos(m_dropShadowAngle) *
                                m_dropShadowOffset * m_font.Height);
                size.Height += (float)(Math.Sin(m_dropShadowAngle) *
                                m_dropShadowOffset * m_font.Height);
            }
            return size;
        }

        public SizeF BoundingBox(Graphics g, String text, float scaleFactor)
        {
            return BoundingBox(g, text, scaleFactor, new SizeF());
        }

        public SizeF BoundingBox(Graphics g, String text, float scaleFactor, SizeF layoutArea)
        {
            SizeF s;
            if (layoutArea.IsEmpty)
                s = MeasureString(g, text, scaleFactor);
            else
                s = MeasureString(g, text, scaleFactor, layoutArea);
            float cs = (float)Math.Abs(Math.Cos(m_angle * Math.PI / 180.0));
            float sn = (float)Math.Abs(Math.Sin(m_angle * Math.PI / 180.0));
            SizeF s2 = new SizeF(s.Width * cs + s.Height * sn,
                s.Width * sn + s.Height * cs);
            return s2;
        }

        public SizeF BoundingBoxTenPower(Graphics g, String text, float scaleFactor)
        {
            float scaledSuperSize = m_scaledSize * Default.SuperSize;
            Remake(scaleFactor, this.Size * Default.SuperSize, ref scaledSuperSize,
                ref m_superScriptFont);
            SizeF size10 = MeasureString(g, "10", scaleFactor);
            SizeF sizeText = g.MeasureString(text, m_superScriptFont);
            if (m_isDropShadow)
            {
                sizeText.Width += (float)(Math.Cos(m_dropShadowAngle) *
                            m_dropShadowOffset * m_superScriptFont.Height);
                sizeText.Height += (float)(Math.Sin(m_dropShadowAngle) *
                            m_dropShadowOffset * m_superScriptFont.Height);
            }
            SizeF totSize = new SizeF(size10.Width + sizeText.Width,
                size10.Height + sizeText.Height * Default.SuperShift);
            float cs = (float)Math.Abs(Math.Cos(m_angle * Math.PI / 180.0));
            float sn = (float)Math.Abs(Math.Sin(m_angle * Math.PI / 180.0));
            SizeF s2 = new SizeF(totSize.Width * cs + totSize.Height * sn,
                totSize.Width * sn + totSize.Height * cs);
            return s2;
        }

        public bool PointInBox(PointF pt, Graphics g, String text, float x,
            float y, AlignH alignH, AlignV alignV,
            float scaleFactor)
        {
            return PointInBox(pt, g, text, x, y, alignH, alignV, scaleFactor, new SizeF());
        }

        public bool PointInBox(PointF pt, Graphics g, String text, float x,
            float y, AlignH alignH, AlignV alignV,
            float scaleFactor, SizeF layoutArea)
        {
            Remake(scaleFactor, this.Size, ref m_scaledSize, ref m_font);
            SizeF sizeF;
            if (layoutArea.IsEmpty)
                sizeF = g.MeasureString(text, m_font);
            else
                sizeF = g.MeasureString(text, m_font, layoutArea);
            RectangleF rect = new RectangleF(new PointF(-sizeF.Width / 2.0F, 0.0F), sizeF);
            Matrix matrix = GetMatrix(x, y, sizeF, alignH, alignV, m_angle);
            PointF[] pts = new PointF[1];
            pts[0] = pt;
            matrix.TransformPoints(pts);
            return rect.Contains(pts[0]);
        }

        private Matrix SetupMatrix(Matrix matrix, float x, float y, SizeF sizeF, AlignH alignH,
                AlignV alignV, float angle)
        {
            matrix.Translate(x, y, MatrixOrder.Prepend);
            if (m_angle != 0.0F)
                matrix.Rotate(-angle, MatrixOrder.Prepend);
            float xa, ya;
            if (alignH == AlignH.Left)
                xa = sizeF.Width / 2.0F;
            else if (alignH == AlignH.Right)
                xa = -sizeF.Width / 2.0F;
            else
                xa = 0.0F;
            if (alignV == AlignV.Center)
                ya = -sizeF.Height / 2.0F;
            else if (alignV == AlignV.Bottom)
                ya = -sizeF.Height;
            else
                ya = 0.0F;
            matrix.Translate(xa, ya, MatrixOrder.Prepend);
            return matrix;
        }

        private Matrix GetMatrix(float x, float y, SizeF sizeF, AlignH alignH, AlignV alignV,
                            float angle)
        {
            Matrix matrix = new Matrix();
            float xa, ya;
            if (alignH == AlignH.Left)
                xa = sizeF.Width / 2.0F;
            else if (alignH == AlignH.Right)
                xa = -sizeF.Width / 2.0F;
            else
                xa = 0.0F;
            if (alignV == AlignV.Center)
                ya = -sizeF.Height / 2.0F;
            else if (alignV == AlignV.Bottom)
                ya = -sizeF.Height;
            else
                ya = 0.0F;
            matrix.Translate(-xa, -ya, MatrixOrder.Prepend);
            if (angle != 0.0F)
                matrix.Rotate(angle, MatrixOrder.Prepend);
            matrix.Translate(-x, -y, MatrixOrder.Prepend);
            return matrix;
        }

        public PointF[] GetBox(Graphics g, String text, float x,
                float y, AlignH alignH, AlignV alignV,
                float scaleFactor, SizeF layoutArea)
        {
            Remake(scaleFactor, this.Size, ref m_scaledSize, ref m_font);
            SizeF sizeF;
            if (layoutArea.IsEmpty)
                sizeF = g.MeasureString(text, m_font);
            else
                sizeF = g.MeasureString(text, m_font, layoutArea);
            RectangleF rect = new RectangleF(new PointF(-sizeF.Width / 2.0F, 0.0F), sizeF);
            Matrix matrix = new Matrix();
            SetupMatrix(matrix, x, y, sizeF, alignH, alignV, m_angle);
            PointF[] pts = new PointF[4];
            pts[0] = new PointF(rect.Left, rect.Top);
            pts[1] = new PointF(rect.Right, rect.Top);
            pts[2] = new PointF(rect.Right, rect.Bottom);
            pts[3] = new PointF(rect.Left, rect.Bottom);
            matrix.TransformPoints(pts);
            return pts;
        }

        public void Dispose()
        {
            if (m_fill != null)
                m_fill.Dispose();
            if (m_superScriptFont != null)
                m_superScriptFont.Dispose();
        }
        #endregion
    }
}
