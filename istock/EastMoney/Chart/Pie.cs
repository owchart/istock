/*****************************************************************************\
*                                                                             *
* Pie.cs -      Pie functions, types, and definitions                         *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace OwLib
{
    /// <summary>
    /// 饼图
    /// </summary>
    [Serializable]
    public class Pie : CurveItem
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建饼图
        /// </summary>
        /// <param name="label">标签</param>
        public Pie(String label)
            : this(Color.Empty, label)
        {
        }

        /// <summary>
        /// 创建饼图
        /// </summary>
        /// <param name="color">颜色</param>
        /// <param name="label">标签</param>
        public Pie(Color color, String label)
            : base(label)
        {
            m_fill = new Fill(color.IsEmpty ? m_rotator.NextColor : color);
            m_border = new Border(Default.BorderColor, Default.BorderWidth);
        }

        /// <summary>
        /// 创建饼图
        /// </summary>
        /// <param name="rhs">其他对象</param>
        public Pie(Pie rhs)
            : base(rhs)
        {
            Slices = new List<PieItem>();
            for (int i = 0; i < rhs.Slices.Count; i++)
            {
                Slices.Add(new PieItem(rhs.Slices[i]));
            }
            m_fill = new Fill(rhs.m_fill);
            this.m_border = new Border(rhs.m_border);
        }

        /// <summary>
        /// 默认属性
        /// </summary>
        public struct Default
        {
            public static double Displacement = 0;
            public static float BorderWidth = 1.0F;
            public static FillType FillType = FillType.Brush;
            public static bool IsBorderVisible = true;
            public static Color BorderColor = Color.FromArgb(30, 30, 30);
            public static Color FillColor = Color.Red;
            public static Brush FillBrush = null;
            public static bool isVisible = true;
            public static PieLabelType LabelType = PieLabelType.Name;
            public static float FontSize = 10;
            public static int ValueDecimalDigits = 0;
            public static int PercentDecimalDigits = 2;
        }

        private Border m_border;
        private Fill m_fill;
        public int Index;
        public int leftSliceCount;
        public RectangleF PieRec;
        public int rightSliceCount;
        private static ColorSymbolRotator m_rotator = new ColorSymbolRotator();
        public List<PieItem> Slices = new List<PieItem>();

        /// <summary>
        /// 
        /// </summary>
        public new Color Color
        {
            get { return m_fill.Color; }
            set { m_fill.Color = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="color">颜色</param>
        /// <param name="displacement">位移</param>
        /// <param name="label">标签</param>
        /// <returns></returns>
        public PieItem AddPieSlice(double value, Color color, double displacement, String label)
        {
            PieItem slice = new PieItem(value, color, displacement, label);
            this.Slices.Add(slice);
            return slice;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">数值</param>
        /// <param name="color1">颜色1</param>
        /// <param name="color2">颜色2</param>
        /// <param name="fillAngle">填充角度</param>
        /// <param name="displacement">位移</param>
        /// <param name="label">标签</param>
        /// <returns></returns>
        public PieItem AddPieSlice(double value, Color color1, Color color2, float fillAngle,
                        double displacement, String label)
        {
            PieItem slice = new PieItem(value, color1, color2, fillAngle, displacement, label);
            this.Slices.Add(slice);
            return slice;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns></returns>
        private RectangleF CalculatePieRec(GraphPane pane)
        {
            RectangleF nonExplRect = pane.Chart.Rect;
            return nonExplRect;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="pos">位置</param>
        /// <param name="scaleFactor">刻度因子</param>
        public override void Draw(Graphics g, GraphPane pane, int pos, float scaleFactor)
        {
            if (!m_isVisible)
                return;
            RectangleF rectPie = CalculatePieRec(pane);
            double maxDisplacement = 0;
            float labelWith = PieItem.CalculateAngleAndLabelLength(g, scaleFactor, this, ref maxDisplacement);
            if (rectPie.Width > rectPie.Height)
            {
                if (labelWith * 2F + rectPie.Height < rectPie.Width)
                {
                    float reduce = rectPie.Width - (labelWith * 2F + rectPie.Height);
                    rectPie.Inflate(-0.5F * reduce, 0);
                }
            }
            RectangleF rectOut = rectPie;
            rectPie.Inflate(-1F * labelWith, 0);
            if (rectPie.Width > rectPie.Height)
            {
                rectPie.X = rectPie.X + (rectPie.Width - rectPie.Height) / 2;
                rectPie.Width = rectPie.Height;
            }
            else if (rectPie.Width < rectPie.Height)
            {
                rectPie.Y = rectPie.Y + (rectPie.Height - rectPie.Width) / 2;
                rectPie.Height = rectPie.Width;
            }
            float extensionLine = rectPie.Width * 0.191F;
            rectPie.Inflate(-1F * extensionLine, -1F * extensionLine);
            if (true)
            {
            }
            if (true)
            {
                this.leftSliceCount = 0;
                this.rightSliceCount = 0;
                foreach (PieItem item in Slices)
                {
                    item.Pie = this;
                    item.Draw2(g, pane, rectPie, scaleFactor, this);
                }
                List<PieItem> prePieItems = new List<PieItem>();
                foreach (PieItem item in Slices)
                {
                    item.CalcLabel2(g, pane, rectPie, rectOut, extensionLine, scaleFactor, prePieItems);
                }
                prePieItems.Clear();
                PointF rectCenter = new PointF((rectPie.X + rectPie.Width / 2), (rectPie.Y + rectPie.Height / 2));
                for (int i = Slices.Count - 1; i >= 0; i--)
                {
                    PieItem item = Slices[i];
                    item.CollisionCounterClockWise(g, ref rectOut, prePieItems, ref rectCenter, scaleFactor);
                }
                foreach (PieItem item in Slices)
                {
                    item.DrawLabel2(g, pane, scaleFactor);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="rect">区域</param>
        /// <param name="scaleFactor">刻度因子</param>
        public override void DrawLegendKey(Graphics g, GraphPane pane, RectangleF rect, float scaleFactor)
        {
            m_fill.Type = FillType.GradientByColorValue;
            g.FillPie(m_fill.MakeBrush(rect), rect.X - 2, rect.Y - 1, rect.Width * 3 / 4, rect.Width * 3 / 4, 270, 90);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">图层</param>
        /// <param name="i">索引</param>
        /// <param name="coords">坐标</param>
        /// <returns></returns>
        public override bool GetCoords(GraphPane pane, int i, out String coords)
        {
            coords = String.Empty;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">图层</param>
        /// <returns></returns>
        internal override bool IsXIndependent(GraphPane pane)
        {
            return false;
        }
        #endregion
    }
}
