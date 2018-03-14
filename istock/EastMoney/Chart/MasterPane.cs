/*****************************************************************************\
*                                                                             *
* MasterPane.cs -   MasterPane functions, types, and definitions              *
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
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Collections;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Collections.Generic;
namespace OwLib
{
    /// <summary>
    /// 主图层
    /// </summary>
    [Serializable]
    public class MasterPane : PaneBase
    {
        #region 陶德 2016/6/7
        /// <summary>
        /// 创建主图层
        /// </summary>
        public MasterPane()
            : this("", new RectangleF(0, 0, 500, 375))
        {
        }

        /// <summary>
        /// 创建主图层
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="paneRect">区域</param>
        public MasterPane(String title, RectangleF paneRect)
            : base(title, paneRect)
        {
            m_innerPaneGap = Default.InnerPaneGap;
            m_isUniformLegendEntries = Default.IsUniformLegendEntries;
            m_isCommonScaleFactor = Default.IsCommonScaleFactor;
            m_paneList = new List<PaneBase>();
            m_legend.IsVisible = Default.IsShowLegend;
            m_isAntiAlias = false;
            InitLayout();
        }

        /// <summary>
        /// 默认属性
        /// </summary>
        public new struct Default
        {
            public static PaneLayout PaneLayout = PaneLayout.SquareColPreferred;
            public static float InnerPaneGap = 10;
            public static bool IsShowLegend = false;
            public static bool IsUniformLegendEntries = false;
            public static bool IsCommonScaleFactor = false;
        }

        public int ColsCount = 2;
        internal int[] m_countList;
        internal bool m_isColumnSpecified;
        internal float[] m_prop;
        public int RowsCount = 2;

        private bool m_isAntiAlias = false;

        /// <summary>
        /// 获取或设置清晰度
        /// </summary>
        public bool IsAntiAlias
        {
            get { return m_isAntiAlias; }
            set { m_isAntiAlias = value; }
        }

        private bool m_isCommonScaleFactor;

        /// <summary>
        /// 获取或设置通用网格因子
        /// </summary>
        public bool IsCommonScaleFactor
        {
            get { return m_isCommonScaleFactor; }
            set { m_isCommonScaleFactor = value; }
        }

        internal float m_innerPaneGap;

        /// <summary>
        /// 获取或设置内层边距
        /// </summary>
        public float InnerPaneGap
        {
            get { return m_innerPaneGap; }
            set { m_innerPaneGap = value; }
        }

        private bool m_isUniformLegendEntries;

        /// <summary>
        /// 获取或设置一直图例
        /// </summary>
        public bool IsUniformLegendEntries
        {
            get { return (m_isUniformLegendEntries); }
            set { m_isUniformLegendEntries = value; }
        }

        internal PaneLayout m_paneLayout;

        /// <summary>
        /// 获取或设置布局
        /// </summary>
        public PaneLayout PaneLayout
        {
            get { return m_paneLayout; }
            set { m_paneLayout = value; }
        }

        internal List<PaneBase> m_paneList;

        /// <summary>
        /// 获取或设置布局列表
        /// </summary>
        public List<PaneBase> PaneList
        {
            get { return m_paneList; }
            set { m_paneList = value; }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="pane">图层</param>
        public void Add(GraphPane pane)
        {
            m_paneList.Add(pane);
        }

        /// <summary>
        /// 坐标改变
        /// </summary>
        public void AxisChange()
        {
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
                AxisChange(g);
        }

        /// <summary>
        /// 坐标改变
        /// </summary>
        /// <param name="g">绘图对象</param>
        public void AxisChange(Graphics g)
        {
            foreach (GraphPane pane in m_paneList)
                pane.AxisChange(g);
        }

        /// <summary>
        /// 通用坐标因子
        /// </summary>
        public void CommonScaleFactor()
        {
            if (m_isCommonScaleFactor)
            {
                float maxFactor = 0;
                foreach (GraphPane pane in PaneList)
                {
                    pane.BaseDimension = PaneBase.Default.BaseDimension;
                    float scaleFactor = pane.CalcScaleFactor();
                    maxFactor = scaleFactor > maxFactor ? scaleFactor : maxFactor;
                }
                foreach (GraphPane pane in PaneList)
                {
                    float scaleFactor = pane.CalcScaleFactor();
                    pane.BaseDimension *= scaleFactor / maxFactor;
                }
            }
        }

        /// <summary>
        /// 销毁资源
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (PaneList != null)
            {
                foreach (GraphPane pane in PaneList)
                    pane.Dispose();
                PaneList.Clear();
            }
        }

        /// <summary>
        /// 布局
        /// </summary>
        /// <param name="g">绘图对象</param>
        public void DoLayout(Graphics g)
        {
            if (m_countList != null)
                DoLayout(g, m_isColumnSpecified, m_countList, m_prop);
            else
            {
                int count = m_paneList.Count;
                if (count == 0)
                    return;
                int rows,
                        cols,
                        root = (int)(Math.Sqrt((double)count) + 0.9999999);
                switch (m_paneLayout)
                {
                    case PaneLayout.ForceSquare:
                        rows = root;
                        cols = root;
                        DoLayout(g, rows, cols);
                        break;
                    case PaneLayout.SingleColumn:
                        rows = count;
                        cols = 1;
                        DoLayout(g, rows, cols);
                        break;
                    case PaneLayout.SingleRow:
                        rows = 1;
                        cols = count;
                        DoLayout(g, rows, cols);
                        break;
                    default:
                    case PaneLayout.SquareColPreferred:
                        rows = root;
                        cols = root;
                        if (count <= root * (root - 1))
                            rows--;
                        DoLayout(g, rows, cols);
                        break;
                    case PaneLayout.SquareRowPreferred:
                        rows = root;
                        cols = root;
                        if (count <= root * (root - 1))
                            cols--;
                        DoLayout(g, rows, cols);
                        break;
                    case PaneLayout.ExplicitCol12:
                        DoLayout(g, true, new int[2] { 1, 2 }, null);
                        break;
                    case PaneLayout.ExplicitCol21:
                        DoLayout(g, true, new int[2] { 2, 1 }, null);
                        break;
                    case PaneLayout.ExplicitCol23:
                        DoLayout(g, true, new int[2] { 2, 3 }, null);
                        break;
                    case PaneLayout.ExplicitCol32:
                        DoLayout(g, true, new int[2] { 3, 2 }, null);
                        break;
                    case PaneLayout.ExplicitRow12:
                        DoLayout(g, false, new int[2] { 1, 2 }, null);
                        break;
                    case PaneLayout.ExplicitRow21:
                        DoLayout(g, false, new int[2] { 2, 1 }, null);
                        break;
                    case PaneLayout.ExplicitRow23:
                        DoLayout(g, false, new int[2] { 2, 3 }, null);
                        break;
                    case PaneLayout.ExplicitRow32:
                        DoLayout(g, false, new int[2] { 3, 2 }, null);
                        break;
                    case PaneLayout.FixedCol:
                        cols = ColsCount;
                        if (cols > 0)
                        {
                            rows = (int)(((double)count / cols) + 0.9999999);
                            DoLayout(g, rows, cols);
                        }
                        break;
                    case PaneLayout.FixedRow:
                        rows = RowsCount;
                        if (rows > 0)
                        {
                            cols = (int)(((double)count / rows) + 0.9999999);
                            DoLayout(g, rows, cols);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 布局
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="rows">行</param>
        /// <param name="columns">列</param>
        internal void DoLayout(Graphics g, int rows, int columns)
        {
            if (rows < 1)
                rows = 1;
            if (columns < 1)
                columns = 1;
            int[] countList = new int[rows];
            for (int i = 0; i < rows; i++)
                countList[i] = columns;
            DoLayout(g, true, countList, null);
        }

        /// <summary>
        /// 布局
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="isColumnSpecified">是否特殊列</param>
        /// <param name="countList">统计数据</param>
        /// <param name="proportion">比率</param>
        internal void DoLayout(Graphics g, bool isColumnSpecified, int[] countList,
                    float[] proportion)
        {
            float scaleFactor = CalcScaleFactor();
            RectangleF innerRect = CalcClientRect(g, scaleFactor);
            m_legend.CalcRect(g, this, scaleFactor, ref innerRect);
            float scaledInnerGap = (float)(m_innerPaneGap * scaleFactor);
            int iPane = 0;
            if (isColumnSpecified)
            {
                int rows = countList.Length;
                float y = 0.0f;
                for (int rowNum = 0; rowNum < rows; rowNum++)
                {
                    float propFactor = m_prop == null ? 1.0f / rows : m_prop[rowNum];
                    float height = (innerRect.Height - (float)(rows - 1) * scaledInnerGap) *
                                    propFactor;
                    int columns = countList[rowNum];
                    if (columns <= 0)
                        columns = 1;
                    float width = (innerRect.Width - (float)(columns - 1) * scaledInnerGap) /
                                    (float)columns;
                    for (int colNum = 0; colNum < columns; colNum++)
                    {
                        if (iPane >= m_paneList.Count)
                            return;
                        this.PaneList[iPane].Rect = new RectangleF(
                                            innerRect.X + colNum * (width + scaledInnerGap),
                                            innerRect.Y + y,
                                            width,
                                            height);
                        iPane++;
                    }
                    y += height + scaledInnerGap;
                }
            }
            else
            {
                int columns = countList.Length;
                float x = 0.0f;
                for (int colNum = 0; colNum < columns; colNum++)
                {
                    float propFactor = m_prop == null ? 1.0f / columns : m_prop[colNum];
                    float width = (innerRect.Width - (float)(columns - 1) * scaledInnerGap) *
                                    propFactor;
                    int rows = countList[colNum];
                    if (rows <= 0)
                        rows = 1;
                    float height = (innerRect.Height - (float)(rows - 1) * scaledInnerGap) / (float)rows;
                    for (int rowNum = 0; rowNum < rows; rowNum++)
                    {
                        if (iPane >= m_paneList.Count)
                            return;
                        this.PaneList[iPane].Rect = new RectangleF(
                                            innerRect.X + x,
                                            innerRect.Y + rowNum * (height + scaledInnerGap),
                                            width,
                                            height);
                        iPane++;
                    }
                    x += width + scaledInnerGap;
                }
            }
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="g">绘图对象</param>
        public override void Draw(Graphics g)
        {
            SmoothingMode sModeSave = g.SmoothingMode;
            TextRenderingHint sHintSave = g.TextRenderingHint;
            CompositingQuality sCompQual = g.CompositingQuality;
            InterpolationMode sInterpMode = g.InterpolationMode;
            SetAntiAliasMode(g, m_isAntiAlias);
            base.Draw(g);
            if (m_rect.Width <= 1 || m_rect.Height <= 1)
                return;
            float scaleFactor = CalcScaleFactor();
            g.SetClip(m_rect);
            m_graphObjList.Draw(g, this, scaleFactor, ZOrder.G_BehindChartFill);
            m_graphObjList.Draw(g, this, scaleFactor, ZOrder.E_BehindCurves);
            m_graphObjList.Draw(g, this, scaleFactor, ZOrder.D_BehindAxis);
            m_graphObjList.Draw(g, this, scaleFactor, ZOrder.C_BehindChartBorder);
            g.ResetClip();
            foreach (GraphPane pane in m_paneList)
                pane.Draw(g);
            g.SetClip(m_rect);
            m_graphObjList.Draw(g, this, scaleFactor, ZOrder.B_BehindLegend);
            m_graphObjList.Draw(g, this, scaleFactor, ZOrder.A_InFront);
            g.ResetClip();
            g.SmoothingMode = sModeSave;
            g.TextRenderingHint = sHintSave;
            g.CompositingQuality = sCompQual;
            g.InterpolationMode = sInterpMode;
        }

        /// <summary>
        /// 查找区域
        /// </summary>
        /// <param name="mousePt">坐标</param>
        /// <returns></returns>
        public GraphPane FindChartRect(PointF mousePt)
        {
            foreach (GraphPane pane in m_paneList)
            {
                if (pane.Chart.m_rect.Contains(mousePt))
                    return pane;
            }
            return null;
        }

        /// <summary>
        /// 查找最近的对象
        /// </summary>
        /// <param name="mousePt">坐标</param>
        /// <param name="g">绘图对象</param>
        /// <param name="pane">图层</param>
        /// <param name="nearestObj">最近对象</param>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public bool FindNearestPaneObject(PointF mousePt, Graphics g, out GraphPane pane,
            out object nearestObj, out int index)
        {
            pane = null;
            nearestObj = null;
            index = -1;
            GraphObj saveGraphItem = null;
            int saveIndex = -1;
            float scaleFactor = CalcScaleFactor();
            if (this.GraphObjList.FindPoint(mousePt, this, g, scaleFactor, out index))
            {
                saveGraphItem = this.GraphObjList[index];
                saveIndex = index;
                if (saveGraphItem.ZOrder == ZOrder.A_InFront)
                {
                    nearestObj = saveGraphItem;
                    index = saveIndex;
                    return true;
                }
            }
            foreach (GraphPane tPane in m_paneList)
            {
                if (tPane.Rect.Contains(mousePt))
                {
                    pane = tPane;
                    return tPane.FindNearestObject(mousePt, g, out nearestObj, out index);
                }
            }
            if (saveGraphItem != null)
            {
                nearestObj = saveGraphItem;
                index = saveIndex;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 查找区域
        /// </summary>
        /// <param name="mousePt">坐标</param>
        /// <returns></returns>
        public GraphPane FindPane(PointF mousePt)
        {
            foreach (GraphPane pane in m_paneList)
            {
                if (pane.Rect.Contains(mousePt))
                    return pane;
            }
            return null;
        }

        /// <summary>
        /// 初始化图层
        /// </summary>
        private void InitLayout()
        {
            m_paneLayout = Default.PaneLayout;
            m_countList = null;
            m_isColumnSpecified = false;
            m_prop = null;
        }

        /// <summary>
        /// 调整大小
        /// </summary>
        /// <param name="g">绘图对象</param>
        public void ReSize(Graphics g)
        {
            ReSize(g, m_rect);
        }

        /// <summary>
        /// 调整大小
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="rect">区域</param>
        public override void ReSize(Graphics g, RectangleF rect)
        {
            m_rect = rect;
            DoLayout(g);
            CommonScaleFactor();
        }

        /// <summary>
        /// 设置布局
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="paneLayout">布局</param>
        public void SetLayout(Graphics g, PaneLayout paneLayout)
        {
            InitLayout();
            m_paneLayout = paneLayout;
            DoLayout(g);
        }

        /// <summary>
        /// 设置布局
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="rows">行</param>
        /// <param name="columns">列</param>
        public void SetLayout(Graphics g, int rows, int columns)
        {
            InitLayout();
            if (rows < 1)
                rows = 1;
            if (columns < 1)
                columns = 1;
            int[] countList = new int[rows];
            for (int i = 0; i < rows; i++)
                countList[i] = columns;
            SetLayout(g, true, countList, null);
        }

        /// <summary>
        /// 设置布局
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="isColumnSpecified">是否特殊列</param>
        /// <param name="countList">统计数据</param>
        public void SetLayout(Graphics g, bool isColumnSpecified, int[] countList)
        {
            SetLayout(g, isColumnSpecified, countList, null);
        }

        /// <summary>
        /// 设置布局
        /// </summary>
        /// <param name="g">绘图对象</param>
        /// <param name="isColumnSpecified">是否特殊列</param>
        /// <param name="countList">统计数据</param>
        /// <param name="proportion">比率</param>
        public void SetLayout(Graphics g, bool isColumnSpecified, int[] countList, float[] proportion)
        {
            InitLayout();
            if (countList != null && countList.Length > 0)
            {
                m_prop = new float[countList.Length];
                float sumProp = 0.0f;
                for (int i = 0; i < countList.Length; i++)
                {
                    m_prop[i] = (proportion == null || proportion.Length <= i || proportion[i] < 1e-10) ?
                                                1.0f : proportion[i];
                    sumProp += m_prop[i];
                }
                for (int i = 0; i < countList.Length; i++)
                    m_prop[i] /= sumProp;
                m_isColumnSpecified = isColumnSpecified;
                m_countList = countList;
                DoLayout(g);
            }
        }
        #endregion
    }
}
