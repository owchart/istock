/*****************************************************************************\
*                                                                             *
* GraphPane.cs -   GraphPane functions, types, and definitions                *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.ComponentModel;
using System.Data;
using System.Collections.Generic;

namespace OwLib
{
    /// <summary>
    /// ͼ��
    /// </summary>
    [Serializable]
    public class GraphPane : PaneBase
    {
        #region �յ� 2016/6/4
        /// <summary>
        /// ����ͼ��
        /// </summary>
        public GraphPane()
            : this(new RectangleF(0, 0, 500, 375), "", "", "")
        {
        }

        /// <summary>
        /// ����ͼ��
        /// </summary>
        /// <param name="rect">����</param>
        /// <param name="title">����</param>
        /// <param name="xTitle">X�����</param>
        /// <param name="yTitle">Y�����</param>
        public GraphPane(RectangleF rect, String title,
            String xTitle, String yTitle)
            : base(title, rect)
        {
            m_xAxis = new XAxis(xTitle);
            m_yAxisList = new List<YAxis>();
            m_y2AxisList = new List<Y2Axis>();
            m_yAxisList.Add(new YAxis(yTitle));
            m_y2AxisList.Add(new Y2Axis(String.Empty));
            m_curveList = new CurveList();
            m_isIgnoreInitial = Default.IsIgnoreInitial;
            m_isBoundedRanges = Default.IsBoundedRanges;
            m_isAlignGrids = false;
            m_chart = new Chart();
            m_barSettings = new BarSettings(this);
            m_lineType = Default.LineType;
        }

        /// <summary>
        /// Ĭ������
        /// </summary>
        public new struct Default
        {
            public static bool IsIgnoreInitial = false;
            public static bool IsBoundedRanges = false;
            public static LineType LineType = LineType.Normal;
            public static double ClusterScaleWidth = 1.0;
            public static double NearestTol = 7.0;
        }

        /// <summary>
        /// �ؼ�
        /// </summary>
        public GraphA owner;
        /// <summary>
        /// Y���б�
        /// </summary>
        private List<YAxis> m_yAxisList;
        /// <summary>
        /// ��Y���б�
        /// </summary>
        private List<Y2Axis> m_y2AxisList;

        private double m_alpha;

        /// <summary>
        /// ��ȡ������͸����
        /// </summary>
        public double Alpha
        {
            get { return m_alpha; }
            set
            {
                m_alpha = value;
                if (m_Trend != null)
                {
                    m_Trend.Alpha = value;
                }
            }
        }

        internal BarSettings m_barSettings;

        /// <summary>
        /// ��ȡ��״ͼ������
        /// </summary>
        public BarSettings BarSettings
        {
            get { return m_barSettings; }
        }

        private double m_beta;

        /// <summary>
        /// ��ȡ�����ñ���ֵ
        /// </summary>
        public double Beta
        {
            get { return m_beta; }
            set
            {
                m_beta = value;
                if (m_Trend != null)
                {
                    m_Trend.Beta = value;
                }
            }
        }

        internal Chart m_chart;

        /// <summary>
        /// ��ȡͼ��
        /// </summary>
        public Chart Chart
        {
            get { return m_chart; }
        }

        private CurveList m_curveList;

        /// <summary>
        /// ��ȡ�������ߵ��б�
        /// </summary>
        public CurveList CurveList
        {
            get { return m_curveList; }
            set { m_curveList = value; }
        }

        /// <summary>
        /// ��ȡ�Ƿ���ɢ��ͼ
        /// </summary>
        public bool HasScatterPlot
        {
            get
            {
                foreach (CurveItem ci in CurveList)
                {
                    if (ci.IsLine)
                    {
                        return ((LineItem)ci).IsScatterPlot;
                    }
                }
                return false;
            }
        }

        public bool m_HasZeroLine = false;

        /// <summary>
        /// ��ȡ������������
        /// </summary>
        public bool HasZeroLine
        {
            get { return m_HasZeroLine; }
            set { m_HasZeroLine = value; }
        }

        private bool m_isAlignGrids;

        /// <summary>
        /// ��ȡ�������Ƿ��������
        /// </summary>
        public bool IsAlignGrids
        {
            get { return m_isAlignGrids; }
            set { m_isAlignGrids = value; }
        }

        private bool m_isBoundedRanges;

        /// <summary>
        /// ��ȡ�������Ƿ��о��α߽�
        /// </summary>
        public bool IsBoundedRanges
        {
            get { return m_isBoundedRanges; }
            set { m_isBoundedRanges = value; }
        }

        private bool m_IsDrawTrend = false;

        /// <summary>
        /// ��ȡ�������Ƿ���������
        /// </summary>
        public bool IsDrawTrend
        {
            get { return m_IsDrawTrend; }
            set
            {
                m_IsDrawTrend = value;
                if (value || m_Trend == null)
                {
                    m_Trend = new Trend();
                    m_Trend.Alpha = Alpha;
                    m_Trend.Beta = Beta;
                }
                m_Trend.IsDrawTrend = value;
            }
        }

        private bool m_isIgnoreInitial;

        /// <summary>
        /// ��ȡ�������Ƿ���ӳ�ʼ��
        /// </summary>
        public bool IsIgnoreInitial
        {
            get { return m_isIgnoreInitial; }
            set { m_isIgnoreInitial = value; }
        }

        private bool m_isIgnoreMissing;

        /// <summary>
        /// ��ȡ�������Ƿ���Ӵ���
        /// </summary>
        public bool IsIgnoreMissing
        {
            get { return m_isIgnoreMissing; }
            set { m_isIgnoreMissing = value; }
        }

        private LineType m_lineType;

        /// <summary>
        /// ��ȡ�������ߵ�����
        /// </summary>
        public LineType LineType
        {
            get { return m_lineType; }
            set { m_lineType = value; }
        }

        private Trend m_Trend;

        /// <summary>
        /// ��ȡ������������
        /// </summary>
        public Trend Trend
        {
            get { return m_Trend; }
            set
            {
                m_Trend = value;
            }
        }

        private XAxis m_xAxis;

        /// <summary>
        /// ��ȡX��
        /// </summary>
        public XAxis XAxis
        {
            get { return m_xAxis; }
        }

        /// <summary>
        /// ��ȡY��
        /// </summary>
        public YAxis YAxis
        {
            get { return m_yAxisList[0] as YAxis; }
        }

        /// <summary>
        /// ��ȡ��Y��
        /// </summary>
        public Y2Axis Y2Axis
        {
            get { return m_y2AxisList[0] as Y2Axis; }
        }

        /// <summary>
        /// ��ȡY���б�
        /// </summary>
        public List<YAxis> YAxisList
        {
            get { return m_yAxisList; }
        }

        /// <summary>
        /// ��ȡ��Y���б�
        /// </summary>
        public List<Y2Axis> Y2AxisList
        {
            get { return m_y2AxisList; }
        }

        /// <summary>
        /// �����״ͼ
        /// </summary>
        /// <param name="label">��ǩ</param>
        /// <param name="points">�㼯</param>
        /// <param name="color">��ɫ</param>
        /// <returns>��״ͼ</returns>
        public BarItem AddBar(String label, IPointList points, Color color)
        {
            BarItem curve = new BarItem(label, points, color);
            m_curveList.Add(curve);
            return curve;
        }

        /// <summary>
        /// �����״ͼ
        /// </summary>
        /// <param name="label">��ǩ</param>
        /// <param name="x">X����б�</param>
        /// <param name="y">Y����б�</param>
        /// <param name="color">��ɫ</param>
        /// <returns>��״ͼ</returns>
        public BarItem AddBar(String label, double[] x, double[] y, Color color)
        {
            BarItem curve = new BarItem(label, x, y, color);
            m_curveList.Add(curve);
            return curve;
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="label">��ǩ</param>
        /// <param name="x">X����б�</param>
        /// <param name="y">Y����б�</param>
        /// <param name="color">��ɫ</param>
        /// <returns>����</returns>
        public LineItem AddCurve(String label, double[] x, double[] y, Color color)
        {
            LineItem curve = new LineItem(label, x, y, color, SymbolType.Default);
            m_curveList.Add(curve);
            return curve;
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="label">��ǩ</param>
        /// <param name="points">�㼯</param>
        /// <param name="color">��ɫ</param>
        /// <returns>����</returns>
        public LineItem AddCurve(String label, IPointList points, Color color)
        {
            LineItem curve = new LineItem(label, points, color, SymbolType.Default);
            m_curveList.Add(curve);
            return curve;
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="label">��ǩ</param>
        /// <param name="x">X����б�</param>
        /// <param name="y">Y����б�</param>
        /// <param name="color">��ɫ</param>
        /// <param name="symbolType">�Ǻ�����</param>
        /// <returns>����</returns>
        public LineItem AddCurve(String label, double[] x, double[] y,
            Color color, SymbolType symbolType)
        {
            LineItem curve = new LineItem(label, x, y, color, symbolType);
            m_curveList.Add(curve);
            return curve;
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="label">��ǩ</param>
        /// <param name="points">�㼯</param>
        /// <param name="color">��ɫ</param>
        /// <param name="symbolType">�Ǻ�����</param>
        /// <returns>����</returns>
        public LineItem AddCurve(String label, IPointList points,
            Color color, SymbolType symbolType)
        {
            LineItem curve = new LineItem(label, points, color, symbolType);
            m_curveList.Add(curve);
            return curve;
        }

        /// <summary>
        /// ��ӱ�ͼ��Ƭ
        /// </summary>
        /// <param name="value">��ֵ</param>
        /// <param name="color">��ɫ</param>
        /// <param name="displacement">λ��</param>
        /// <param name="label">��ǩ</param>
        /// <returns>��ͼ��Ƭ</returns>
        public PieItem AddPieSlice(double value, Color color, double displacement, String label)
        {
            PieItem slice = new PieItem(value, color, displacement, label);
            this.CurveList.Add(slice);
            return slice;
        }

        /// <summary>
        /// ��ӱ�ͼ��Ƭ
        /// </summary>
        /// <param name="value">��ֵ</param>
        /// <param name="color1">��ɫ1</param>
        /// <param name="color2">��ɫ2</param>
        /// <param name="fillAngle">���Ƕ�</param>
        /// <param name="displacement">λ��</param>
        /// <param name="label">��ǩ</param>
        /// <returns>��ͼ��Ƭ</returns>
        public PieItem AddPieSlice(double value, Color color1, Color color2, float fillAngle,
                        double displacement, String label)
        {
            PieItem slice = new PieItem(value, color1, color2, fillAngle, displacement, label);
            this.CurveList.Add(slice);
            return slice;
        }

        /// <summary>
        /// ��ӱ�ͼ
        /// </summary>
        /// <param name="pie">��ͼ</param>
        public void AddPie(Pie pie)
        {
            pie.Index = this.CurveList.Count;
            this.CurveList.Add(pie);
        }

        /// <summary>
        /// ��ӱ�ͼ��Ƭ��
        /// </summary>
        /// <param name="values">��ֵ�б�</param>
        /// <param name="labels">��ǩ�б�</param>
        /// <returns>��Ƭ��</returns>
        public PieItem[] AddPieSlices(double[] values, String[] labels)
        {
            PieItem[] slices = new PieItem[values.Length];
            for (int x = 0; x < values.Length; x++)
            {
                slices[x] = new PieItem(values[x], labels[x]);
                this.CurveList.Add(slices[x]);
            }
            return slices;
        }

        /// <summary>
        /// ��Ӱ�ͼ
        /// </summary>
        /// <param name="label">��ǩ</param>
        /// <param name="x">X����б�</param>
        /// <param name="y">Y����б�</param>
        /// <param name="color">��ɫ</param>
        /// <returns>��ͼ</returns>
        public StickItem AddStick(String label, double[] x, double[] y, Color color)
        {
            StickItem curve = new StickItem(label, x, y, color);
            m_curveList.Add(curve);
            return curve;
        }

        /// <summary>
        /// ��Ӱ�ͼ
        /// </summary>
        /// <param name="label">��ǩ</param>
        /// <param name="points">�㼯</param>
        /// <param name="color">��ɫ</param>
        /// <returns>��ͼ</returns>
        public StickItem AddStick(String label, IPointList points, Color color)
        {
            StickItem curve = new StickItem(label, points, color);
            m_curveList.Add(curve);
            return curve;
        }

        /// <summary>
        /// ������ı�
        /// </summary>
        /// <param name="g">��ͼ����</param>
        public void AxisChange(Graphics g)
        {
            m_curveList.GetRange( 
                m_isIgnoreInitial, m_isBoundedRanges, this);
            float scaleFactor = this.CalcScaleFactor();
            if (this.CurveList.IsPieOnly)
            {
                this.XAxis.IsVisible = false;
                this.YAxis.IsVisible = false;
                this.Y2Axis.IsVisible = false;
                m_chart.Border.IsVisible = false;
                this.Legend.Position = LegendPos.TopCenter;
            }
            else
            {
                m_chart.Border.IsVisible = true;
            }
            if (m_barSettings.m_clusterScaleWidthAuto)
                m_barSettings.m_clusterScaleWidth = 1.0;
            if (m_chart.m_isRectAuto)
            {
                PickScale(g, scaleFactor);
                m_chart.m_rect = CalcChartRect(g);
            }
            PickScale(g, scaleFactor);
            m_barSettings.CalcClusterScaleWidth();
        }

        /// <summary>
        /// ������������֤
        /// </summary>
        /// <returns></returns>
        private bool AxisRangesValid()
        {
            bool showGraf = m_xAxis.m_scale.m_min < m_xAxis.m_scale.m_max;
            foreach (Axis axis in m_yAxisList)
                if (axis.m_scale.m_min >= axis.m_scale.m_max)
                    showGraf = false;
            foreach (Axis axis in m_y2AxisList)
                if (axis.m_scale.m_min >= axis.m_scale.m_max)
                    showGraf = false;
            return showGraf;
        }

        /// <summary>
        /// ���Y��
        /// </summary>
        /// <param name="title">����</param>
        /// <returns>����</returns>
        public int AddYAxis(String title)
        {
            YAxis axis = new YAxis(title);
            axis.MajorTic.IsOpposite = false;
            axis.MinorTic.IsOpposite = false;
            axis.MajorTic.IsInside = false;
            axis.MinorTic.IsInside = false;
            m_yAxisList.Add(axis);
            return m_yAxisList.Count - 1;
        }

        /// <summary>
        /// �����Y��
        /// </summary>
        /// <param name="title">����</param>
        /// <returns>����</returns>
        public int AddY2Axis(String title)
        {
            Y2Axis axis = new Y2Axis(title);
            axis.MajorTic.IsOpposite = false;
            axis.MinorTic.IsOpposite = false;
            axis.MajorTic.IsInside = false;
            axis.MinorTic.IsInside = false;
            m_y2AxisList.Add(axis);
            return m_y2AxisList.Count - 1;
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="g">��ͼ����</param>
        /// <returns>����</returns>
        public RectangleF CalcChartRect(Graphics g)
        {
            return CalcChartRect(g, CalcScaleFactor());
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="g">��ͼ����</param>
        /// <param name="scaleFactor">�̶�����</param>
        /// <returns>����</returns>
        public RectangleF CalcChartRect(Graphics g, float scaleFactor)
        {
            RectangleF clientRect = this.CalcClientRect(g, scaleFactor);
            float totSpaceY = 0;
            float minSpaceL = 0;
            float minSpaceR = 0;
            float minSpaceB = 0;
            float minSpaceT = 5;
            m_xAxis.CalcSpace(g, this, scaleFactor, out minSpaceB);
            foreach (Axis axis in m_yAxisList)
            {
                float fixedSpace;
                float tmp = axis.CalcSpace(g, this, scaleFactor, out fixedSpace);
                if (axis.IsCrossShifted(this))
                    totSpaceY += tmp;
                minSpaceL += fixedSpace;
            }
            foreach (Axis axis in m_y2AxisList)
            {
                float fixedSpace;
                float tmp = axis.CalcSpace(g, this, scaleFactor, out fixedSpace);
                if (axis.IsCrossShifted(this))
                    totSpaceY += tmp;
                minSpaceR += fixedSpace;
            }
            float spaceB = 0, spaceT = 0, spaceL = 0, spaceR = 0;
            SetSpace(m_xAxis, clientRect.Height - m_xAxis.m_tmpSpace, ref spaceB, ref spaceT);
            m_xAxis.m_tmpSpace = spaceB;
            float totSpaceL = 0;
            float totSpaceR = 0;
            foreach (Axis axis in m_yAxisList)
            {
                SetSpace(axis, clientRect.Width - totSpaceY, ref spaceL, ref spaceR);
                minSpaceR = Math.Max(minSpaceR, spaceR);
                totSpaceL += spaceL;
                axis.m_tmpSpace = spaceL;
            }
            foreach (Axis axis in m_y2AxisList)
            {
                SetSpace(axis, clientRect.Width - totSpaceY, ref spaceR, ref spaceL);
                minSpaceL = Math.Max(minSpaceL, spaceL);
                totSpaceR += spaceR;
                axis.m_tmpSpace = spaceR;
            }
            RectangleF tmpRect = clientRect;
            totSpaceL = Math.Max(totSpaceL, minSpaceL);
            totSpaceR = Math.Max(totSpaceR, minSpaceR);
            spaceB = Math.Max(spaceB, minSpaceB);
            spaceT = Math.Max(spaceT, minSpaceT);
            tmpRect.X += totSpaceL;
            tmpRect.Width -= totSpaceL + totSpaceR;
            tmpRect.Height -= spaceT + spaceB;
            tmpRect.Y += spaceT;
            m_legend.CalcRect(g, this, scaleFactor, ref tmpRect);
            return tmpRect;
        }

        /// <summary>
        /// �ȽϷ���
        /// </summary>
        /// <param name="gpo1">ͼ��1</param>
        /// <param name="gpo2">ͼ��2</param>
        /// <returns>�ȽϽ��</returns>
        public int CompFunc(GraphObj gpo1, GraphObj gpo2)
        {
            if (gpo1.ZIndex > gpo2.ZIndex)
            {
                return 1;
            }
            else if (gpo1.ZIndex == gpo2.ZIndex)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// ���ٶ���
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            if (XAxis != null)
                XAxis.Dispose();
            if (m_yAxisList != null)
            {
                foreach (Axis x in m_yAxisList)
                    x.Dispose();
                m_yAxisList.Clear();
            }
            if (m_y2AxisList != null)
            {
                foreach (Axis x in m_y2AxisList)
                    x.Dispose();
                m_y2AxisList.Clear();
            }
            if (CurveList != null)
            {
                foreach (CurveItem item in CurveList)
                {
                    item.Dispose();
                }
                CurveList.Clear();
            }
            if (Chart != null)
                Chart.Dispose();
        }

        /// <summary>
        /// ��ͼ����
        /// </summary>
        /// <param name="g">��ͼ����</param>
        public override void Draw(Graphics g)
        {
            base.Draw(g);
            if (m_rect.Width <= 1 || m_rect.Height <= 1)
                return;
            g.SetClip(m_rect);
            float scaleFactor = this.CalcScaleFactor();
            if (m_chart.m_isRectAuto)
            {
                m_chart.m_rect = CalcChartRect(g, scaleFactor);
            }
            else
                CalcChartRect(g, scaleFactor);
            if (m_chart.m_rect.Width < 1 || m_chart.m_rect.Height < 1)
                return;
            bool showGraf = true;
            if (showGraf)
            {
                m_graphObjList.Sort(new Comparison<GraphObj>(CompFunc));
            }
            m_xAxis.Scale.SetupScaleData(this, m_xAxis);
            foreach (Axis axis in m_yAxisList)
                axis.Scale.SetupScaleData(this, axis);
            foreach (Axis axis in m_y2AxisList)
                axis.Scale.SetupScaleData(this, axis);
            if (showGraf)
                m_graphObjList.Draw(g, this, scaleFactor, ZOrder.G_BehindChartFill);
            m_chart.Fill.Draw(g, m_chart.m_rect);
            if (showGraf)
            {
                m_graphObjList.Draw(g, this, scaleFactor, ZOrder.F_BehindGrid);
                DrawGrid(g, scaleFactor);
                m_graphObjList.Draw(g, this, scaleFactor, ZOrder.E_BehindCurves);
                RectangleF tempChartRec = new RectangleF(m_chart.m_rect.X, m_chart.m_rect.Y - 10, m_chart.m_rect.Width, m_chart.Rect.Height + 10);
                g.SetClip(tempChartRec);
                m_curveList.Draw(g, this, scaleFactor);
                g.SetClip(m_rect);
            }
            if (showGraf)
            {
                m_graphObjList.Draw(g, this, scaleFactor, ZOrder.D_BehindAxis);
                m_xAxis.Draw(g, this, scaleFactor, 0.0f);
                float yPos = 0;
                foreach (Axis axis in m_yAxisList)
                {
                    axis.Draw(g, this, scaleFactor, yPos);
                    yPos += axis.m_tmpSpace;
                }
                yPos = 0;
                foreach (Axis axis in m_y2AxisList)
                {
                    axis.Draw(g, this, scaleFactor, yPos);
                    yPos += axis.m_tmpSpace;
                }
                m_graphObjList.Draw(g, this, scaleFactor, ZOrder.C_BehindChartBorder);
            }
            if (showGraf)
            {
                m_graphObjList.Draw(g, this, scaleFactor, ZOrder.B_BehindLegend);
                m_legend.Draw(g, this, scaleFactor);
                m_graphObjList.Draw(g, this, scaleFactor, ZOrder.A_InFront);
            }
            if (IsDrawTrend)
            {
                m_Trend.Draw(g, this);
            }
            g.ResetClip();
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="g">��ͼ����</param>
        /// <param name="scaleFactor">�̶�����</param>
        internal void DrawGrid(Graphics g, float scaleFactor)
        {
            m_xAxis.DrawGrid(g, this, scaleFactor, 0.0f);
            float shiftPos = 0.0f;
            foreach (YAxis yAxis in m_yAxisList)
            {
                yAxis.DrawGrid(g, this, scaleFactor, shiftPos);
                shiftPos += yAxis.m_tmpSpace;
            }
            shiftPos = 0.0f;
            foreach (Y2Axis y2Axis in m_y2AxisList)
            {
                y2Axis.DrawGrid(g, this, scaleFactor, shiftPos);
                shiftPos += y2Axis.m_tmpSpace;
            }
        }

        /// <summary>
        /// ���Ұ����Ķ���
        /// </summary>
        /// <param name="rectF">����</param>
        /// <param name="g">��ͼ����</param>
        /// <param name="containedObjs">������</param>
        /// <returns>�Ƿ��ҵ�</returns>
        public bool FindContainedObjects(RectangleF rectF, Graphics g,
out CurveList containedObjs)
        {
            containedObjs = new CurveList();
            foreach (CurveItem ci in this.CurveList)
            {
                for (int i = 0; i < ci.Points.Count; i++)
                {
                    if (ci.Points[i].X > rectF.Left &&
                         ci.Points[i].X < rectF.Right &&
                         ci.Points[i].Y > rectF.Bottom &&
                         ci.Points[i].Y < rectF.Top)
                    {
                        containedObjs.Add(ci);
                    }
                }
            }
            return (containedObjs.Count > 0);
        }

        /// <summary>
        /// ��������Ķ���
        /// </summary>
        /// <param name="mousePt">����</param>
        /// <param name="g">��ͼ����</param>
        /// <param name="nearestObj">����Ķ���</param>
        /// <param name="index">����</param>
        /// <returns>����Ķ���</returns>
        public bool FindNearestObject(PointF mousePt, Graphics g,
            out object nearestObj, out int index)
        {
            nearestObj = null;
            index = -1;
            if (AxisRangesValid())
            {
                float scaleFactor = CalcScaleFactor();
                RectangleF tmpRect;
                GraphObj saveGraphItem = null;
                int saveIndex = -1;
                ZOrder saveZOrder = ZOrder.H_BehindAll;
                RectangleF tmpChartRect = CalcChartRect(g, scaleFactor);
                if (this.GraphObjList.FindPoint(mousePt, this, g, scaleFactor, out index))
                {
                    saveGraphItem = this.GraphObjList[index];
                    saveIndex = index;
                    saveZOrder = saveGraphItem.ZOrder;
                }
                if (saveZOrder <= ZOrder.B_BehindLegend &&
                    this.Legend.FindPoint(mousePt, this, scaleFactor, out index))
                {
                    nearestObj = this.Legend;
                    return true;
                }
                SizeF paneTitleBox = m_title.m_fontSpec.BoundingBox(g, m_title.m_text, scaleFactor);
                if (saveZOrder <= ZOrder.H_BehindAll && m_title.m_isVisible)
                {
                    tmpRect = new RectangleF((m_rect.Left + m_rect.Right - paneTitleBox.Width) / 2,
                        m_rect.Top + m_margin.Top * scaleFactor,
                        paneTitleBox.Width, paneTitleBox.Height);
                    if (tmpRect.Contains(mousePt))
                    {
                        nearestObj = this;
                        return true;
                    }
                }
                float left = tmpChartRect.Left;
                for (int yIndex = 0; yIndex < m_yAxisList.Count; yIndex++)
                {
                    Axis yAxis = m_yAxisList[yIndex];
                    float width = yAxis.m_tmpSpace;
                    if (width > 0)
                    {
                        tmpRect = new RectangleF(left - width, tmpChartRect.Top,
                            width, tmpChartRect.Height);
                        if (saveZOrder <= ZOrder.D_BehindAxis && tmpRect.Contains(mousePt))
                        {
                            nearestObj = yAxis;
                            index = yIndex;
                            return true;
                        }
                        left -= width;
                    }
                }
                left = tmpChartRect.Right;
                for (int yIndex = 0; yIndex < m_y2AxisList.Count; yIndex++)
                {
                    Axis y2Axis = m_y2AxisList[yIndex];
                    float width = y2Axis.m_tmpSpace;
                    if (width > 0)
                    {
                        tmpRect = new RectangleF(left, tmpChartRect.Top,
                            width, tmpChartRect.Height);
                        if (saveZOrder <= ZOrder.D_BehindAxis && tmpRect.Contains(mousePt))
                        {
                            nearestObj = y2Axis;
                            index = yIndex;
                            return true;
                        }
                        left += width;
                    }
                }
                float height = m_xAxis.m_tmpSpace;
                tmpRect = new RectangleF(tmpChartRect.Left, tmpChartRect.Bottom,
                    tmpChartRect.Width, height);
                if (saveZOrder <= ZOrder.D_BehindAxis && tmpRect.Contains(mousePt))
                {
                    nearestObj = this.XAxis;
                    return true;
                }
                tmpRect = new RectangleF(tmpChartRect.Left,
                        tmpChartRect.Top - height,
                        tmpChartRect.Width,
                        height);
                CurveItem curve;
                if (saveZOrder <= ZOrder.E_BehindCurves && FindNearestPoint(mousePt, out curve, out index))
                {
                    nearestObj = curve;
                    return true;
                }
                if (saveGraphItem != null)
                {
                    index = saveIndex;
                    nearestObj = saveGraphItem;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ��������ĵ�
        /// </summary>
        /// <param name="mousePt">����</param>
        /// <param name="targetCurve">Ŀ����</param>
        /// <param name="nearestCurve">�������</param>
        /// <param name="iNearest">���������</param>
        /// <returns>�Ƿ��ҵ�</returns>
        public bool FindNearestPoint(PointF mousePt, CurveItem targetCurve,
                out CurveItem nearestCurve, out int iNearest)
        {
            CurveList targetCurveList = new CurveList();
            targetCurveList.Add(targetCurve);
            return FindNearestPoint(mousePt, targetCurveList,
                out nearestCurve, out iNearest);
        }

        /// <summary>
        /// ��������ĵ�
        /// </summary>
        /// <param name="mousePt">����</param>
        /// <param name="nearestCurve">�������</param>
        /// <param name="iNearest">���������</param>
        /// <returns>�Ƿ��ҵ�</returns>
        public bool FindNearestPoint(PointF mousePt,
            out CurveItem nearestCurve, out int iNearest)
        {
            return FindNearestPoint(mousePt, m_curveList,
                out nearestCurve, out iNearest);
        }

        /// <summary>
        /// ��������ĵ�
        /// </summary>
        /// <param name="mousePt">����</param>
        /// <param name="targetCurveList">Ŀ�����б�</param>
        /// <param name="nearestCurve">�������</param>
        /// <param name="iNearest">���������</param>
        /// <returns>�Ƿ��ҵ�</returns>
        public bool FindNearestPoint(PointF mousePt, CurveList targetCurveList,
            out CurveItem nearestCurve, out int iNearest)
        {
            CurveItem nearestBar = null;
            int iNearestBar = -1;
            nearestCurve = null;
            iNearest = -1;
            if (!m_chart.m_rect.Contains(mousePt))
                return false;
            double x, x2;
            double[] y;
            double[] y2;
            ReverseTransform(mousePt, out x, out x2, out y, out y2);
            if (!AxisRangesValid())
                return false;
            ValueHandler valueHandler = new ValueHandler(this, false);
            double yPixPerUnitAct, yAct, yMinAct, yMaxAct, xAct;
            double minDist = 1e20;
            double xVal, yVal, dist = 99999, distX, distY;
            double tolSquared = Default.NearestTol * Default.NearestTol;
            int iBar = 0;
            foreach (CurveItem curve in targetCurveList)
            {
                if (curve is Pie && curve.IsVisible)
                {
                    Pie pie = curve as Pie;
                    foreach (PieItem item in pie.Slices)
                    {
                        if (item.SlicePath != null &&
        item.SlicePath.IsVisible(mousePt))
                        {
                            nearestBar = curve;
                            iNearestBar = 0;
                        }
                    }
                    continue;
                }
                else if (curve.IsVisible)
                {
                    int yIndex = curve.GetYAxisIndex(this);
                    Axis yAxis = curve.GetYAxis(this);
                    Axis xAxis = curve.GetXAxis(this);
                    if (curve.IsY2Axis)
                    {
                        yAct = y2[yIndex];
                        yMinAct = m_y2AxisList[yIndex].m_scale.m_min;
                        yMaxAct = m_y2AxisList[yIndex].m_scale.m_max;
                    }
                    else
                    {
                        yAct = y[yIndex];
                        yMinAct = m_yAxisList[yIndex].m_scale.m_min;
                        yMaxAct = m_yAxisList[yIndex].m_scale.m_max;
                    }
                    yPixPerUnitAct = m_chart.m_rect.Height / (yMaxAct - yMinAct);
                    double xPixPerUnit = m_chart.m_rect.Width / (xAxis.m_scale.m_max - xAxis.m_scale.m_min);
                    xAct = xAxis is XAxis ? x : x2;
                    IPointList points = curve.Points;
                    float barWidth = curve.GetBarWidth(this);
                    double barWidthUserHalf;
                    Axis baseAxis = curve.BaseAxis(this);
                    bool isXBaseAxis = (baseAxis is XAxis);
                    if (isXBaseAxis)
                        barWidthUserHalf = barWidth / xPixPerUnit / 2.0;
                    else
                        barWidthUserHalf = barWidth / yPixPerUnitAct / 2.0;
                    if (points != null)
                    {
                        for (int iPt = 0; iPt < curve.NPts; iPt++)
                        {
                            if (xAxis.m_scale.IsAnyOrdinal && !curve.IsOverrideOrdinal)
                                xVal = (double)iPt + 1.0;
                            else
                                xVal = points[iPt].X;
                            if (yAxis.m_scale.IsAnyOrdinal && !curve.IsOverrideOrdinal)
                                yVal = (double)iPt + 1.0;
                            else
                                yVal = points[iPt].Y;
                            if (xVal != PointPair.Missing &&
                                    yVal != PointPair.Missing)
                            {
                                if (curve.IsBar)
                                {
                                    double baseVal, lowVal, hiVal;
                                    valueHandler.GetValues(curve, iPt, out baseVal,
                                            out lowVal, out hiVal);
                                    if (lowVal > hiVal)
                                    {
                                        double tmpVal = lowVal;
                                        lowVal = hiVal;
                                        hiVal = tmpVal;
                                    }
                                    if (isXBaseAxis)
                                    {
                                        double centerVal = valueHandler.BarCenterValue(curve, barWidth, iPt, xVal, iBar);
                                        if (xAct < centerVal - barWidthUserHalf ||
                                                xAct > centerVal + barWidthUserHalf ||
                                                yAct < lowVal || yAct > hiVal)
                                            continue;
                                    }
                                    else
                                    {
                                        double centerVal = valueHandler.BarCenterValue(curve, barWidth, iPt, yVal, iBar);
                                        if (yAct < centerVal - barWidthUserHalf ||
                                                yAct > centerVal + barWidthUserHalf ||
                                                xAct < lowVal || xAct > hiVal)
                                            continue;
                                    }
                                    if (nearestBar == null)
                                    {
                                        iNearestBar = iPt;
                                        nearestBar = curve;
                                    }
                                }
                                else if (xVal >= xAxis.m_scale.m_min && xVal <= xAxis.m_scale.m_max &&
                                            yVal >= yMinAct && yVal <= yMaxAct)
                                {
                                    if (curve is LineItem && m_lineType == LineType.Stack)
                                    {
                                        double zVal;
                                        valueHandler.GetValues(curve, iPt, out xVal, out zVal, out yVal);
                                    }
                                    distX = (xVal - xAct) * xPixPerUnit;
                                    distY = (yVal - yAct) * yPixPerUnitAct;
                                    dist = distX * distX + distY * distY;
                                    if (dist >= minDist)
                                        continue;
                                    minDist = dist;
                                    iNearest = iPt;
                                    nearestCurve = curve;
                                }
                            }
                        }
                        if (curve.IsBar)
                            iBar++;
                    }
                }
            }
            if (nearestCurve is LineItem)
            {
                float halfSymbol = (float)(((LineItem)nearestCurve).Symbol.Size *
                    CalcScaleFactor() / 2);
                minDist -= halfSymbol * halfSymbol;
                if (minDist < 0)
                    minDist = 0;
            }
            if (minDist >= tolSquared && nearestBar != null)
            {
                nearestCurve = nearestBar;
                iNearest = iNearestBar;
                return true;
            }
            else if (minDist < tolSquared)
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// ���Ҷ���
        /// </summary>
        /// <param name="mousePt">����</param>
        /// <param name="gpoOut">ͼ��</param>
        /// <returns>�Ƿ��ҵ�</returns>
        internal bool FindObj(Point mousePt, out GraphObj gpoOut)
        {
            gpoOut = null;
            foreach (GraphObj gpo in this.GraphObjList)
            {
                if (gpo.InBox(mousePt, out gpoOut))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// �ҵ��������
        /// </summary>
        /// <param name="gpoOut">ͼ��</param>
        /// <returns>�Ƿ��ҵ�</returns>
        internal bool FindObjFocused(out GraphObj gpoOut)
        {
            gpoOut = null;
            foreach (GraphObj gpo in this.GraphObjList)
            {
                if (gpo.Focused)
                {
                    gpoOut = gpo;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ǿ�����Ǻ�
        /// </summary>
        /// <param name="axis">������</param>
        /// <param name="numTics">�Ǻ�</param>
        private void ForceNumTics(Axis axis, int numTics)
        {
            if (axis.m_scale.MaxAuto)
            {
                int nTics = axis.m_scale.CalcNumTics();
                if (nTics < numTics)
                    axis.m_scale.m_maxLinearized += axis.m_scale.m_majorStep * (numTics - nTics);
            }
        }

        /// <summary>
        /// ��ȡ������ĵ�
        /// </summary>
        /// <param name="ptF">����</param>
        /// <param name="x">������ֵ</param>
        /// <param name="y">������ֵ</param>
        public void GetFixedPostion(PointF ptF, out double x, out double y)
        {
            x = 0; y = 0;
            x = (ptF.X - this.Rect.X) / this.Rect.Width;
            y = (ptF.Y - this.Rect.Y) / this.Rect.Height;
        }

        /// <summary>
        /// ��ȡ������ĵ�
        /// </summary>
        /// <param name="points">�㼯</param>
        /// <param name="outPoints">�����</param>
        public void GetFixedPostion(PointF[] points, out PointF[] outPoints)
        {
            outPoints = new PointF[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                double x, y;
                GetFixedPostion(points[i], out x, out y);
                outPoints[i] = new PointF((float)x, (float)y);
            }
        }

        /// <summary>
        /// ����ת����
        /// </summary>
        /// <param name="ptF">����</param>
        /// <param name="coord">����������</param>
        /// <returns>ת����</returns>
        public PointF GeneralTransform(PointF ptF, CoordType coord)
        {
            m_xAxis.Scale.SetupScaleData(this, m_xAxis);
            foreach (Axis axis in m_yAxisList)
                axis.Scale.SetupScaleData(this, axis);
            foreach (Axis axis in m_y2AxisList)
                axis.Scale.SetupScaleData(this, axis);
            return this.TransformCoord(ptF.X, ptF.Y, coord);
        }

        /// <summary>
        /// ����ת����
        /// </summary>
        /// <param name="x">������ֵ</param>
        /// <param name="y">������ֵ</param>
        /// <param name="coord">����������</param>
        /// <returns>ת����</returns>
        public PointF GeneralTransform(double x, double y, CoordType coord)
        {
            m_xAxis.Scale.SetupScaleData(this, m_xAxis);
            foreach (Axis axis in m_yAxisList)
                axis.Scale.SetupScaleData(this, axis);
            foreach (Axis axis in m_y2AxisList)
                axis.Scale.SetupScaleData(this, axis);
            return this.TransformCoord(x, y, coord);
        }

        /// <summary>
        /// ѡ��̶�
        /// </summary>
        /// <param name="g">��ͼ����</param>
        /// <param name="scaleFactor">�̶�����</param>
        private void PickScale(Graphics g, float scaleFactor)
        {
            int maxTics = 0;
            m_xAxis.m_scale.PickScale(this, g, scaleFactor);
            foreach (Axis axis in m_yAxisList)
            {
                axis.m_scale.PickScale(this, g, scaleFactor);
                if (axis.m_scale.MaxAuto)
                {
                    int nTics = axis.m_scale.CalcNumTics();
                    maxTics = nTics > maxTics ? nTics : maxTics;
                }
            }
            foreach (Axis axis in m_y2AxisList)
            {
                axis.m_scale.PickScale(this, g, scaleFactor);
                if (axis.m_scale.MaxAuto)
                {
                    int nTics = axis.m_scale.CalcNumTics();
                    maxTics = nTics > maxTics ? nTics : maxTics;
                }
            }
            if (m_isAlignGrids)
            {
                foreach (Axis axis in m_yAxisList)
                    ForceNumTics(axis, maxTics);
                foreach (Axis axis in m_y2AxisList)
                    ForceNumTics(axis, maxTics);
            }
        }

        /// <summary>
        /// ��ת
        /// </summary>
        /// <param name="ptF">����</param>
        /// <param name="x">������ֵ</param>
        /// <param name="y">������ֵ</param>
        public void ReverseTransform(PointF ptF, out double x, out double y)
        {
            m_xAxis.Scale.SetupScaleData(this, m_xAxis);
            this.YAxis.Scale.SetupScaleData(this, this.YAxis);
            x = this.XAxis.Scale.ReverseTransform(ptF.X);
            y = this.YAxis.Scale.ReverseTransform(ptF.Y);
        }

        /// <summary>
        /// ��ת
        /// </summary>
        /// <param name="ptF">����</param>
        /// <param name="x">������ֵ</param>
        /// <param name="x2">������ֵ2</param>
        /// <param name="y">������ֵ</param>
        /// <param name="y2">������ֵ2</param>
        public void ReverseTransform(PointF ptF, out double x, out double x2, out double y,
            out double y2)
        {
            m_xAxis.Scale.SetupScaleData(this, m_xAxis);
            this.YAxis.Scale.SetupScaleData(this, this.YAxis);
            this.Y2Axis.Scale.SetupScaleData(this, this.Y2Axis);
            x = this.XAxis.Scale.ReverseTransform(ptF.X);
            y = this.YAxis.Scale.ReverseTransform(ptF.Y);
            x2 = x;
            y2 = this.Y2Axis.Scale.ReverseTransform(ptF.Y);
        }

        /// <summary>
        /// ��ת
        /// </summary>
        /// <param name="ptF">����</param>
        /// <param name="isX2Axis">�Ƿ�X��</param>
        /// <param name="isY2Axis">�Ƿ�Y��</param>
        /// <param name="yAxisIndex">Y������</param>
        /// <param name="x">������ֵ</param>
        /// <param name="y">������ֵ</param>
        public void ReverseTransform(PointF ptF, bool isX2Axis, bool isY2Axis, int yAxisIndex,
                    out double x, out double y)
        {
            Axis xAxis = m_xAxis;
            xAxis.Scale.SetupScaleData(this, xAxis);
            x = xAxis.Scale.ReverseTransform(ptF.X);
            Axis yAxis = null;
            if (isY2Axis && Y2AxisList.Count > yAxisIndex)
                yAxis = Y2AxisList[yAxisIndex];
            else if (!isY2Axis && YAxisList.Count > yAxisIndex)
                yAxis = YAxisList[yAxisIndex];
            if (yAxis != null)
            {
                yAxis.Scale.SetupScaleData(this, yAxis);
                y = yAxis.Scale.ReverseTransform(ptF.Y);
            }
            else
                y = PointPair.Missing;
        }

        /// <summary>
        /// ��ת
        /// </summary>
        /// <param name="ptF">����</param>
        /// <param name="x">������ֵ</param>
        /// <param name="x2">������ֵ2</param>
        /// <param name="y">������ֵ�б�</param>
        /// <param name="y2">������ֵ2</param>
        public void ReverseTransform(PointF ptF, out double x, out double x2, out double[] y,
            out double[] y2)
        {
            m_xAxis.Scale.SetupScaleData(this, m_xAxis);
            x = this.XAxis.Scale.ReverseTransform(ptF.X);
            x2 = x;
            y = new double[m_yAxisList.Count];
            y2 = new double[m_y2AxisList.Count];
            for (int i = 0; i < m_yAxisList.Count; i++)
            {
                Axis axis = m_yAxisList[i];
                axis.Scale.SetupScaleData(this, axis);
                y[i] = axis.Scale.ReverseTransform(ptF.Y);
            }
            for (int i = 0; i < m_y2AxisList.Count; i++)
            {
                Axis axis = m_y2AxisList[i];
                axis.Scale.SetupScaleData(this, axis);
                y2[i] = axis.Scale.ReverseTransform(ptF.Y);
            }
        }

        /// <summary>
        /// ������С�ռ仺��
        /// </summary>
        /// <param name="g">��ͼ����</param>
        /// <param name="bufferFraction">��������</param>
        /// <param name="isGrowOnly">�Ƿ�����</param>
        public void SetMinSpaceBuffer(Graphics g, float bufferFraction, bool isGrowOnly)
        {
            m_xAxis.SetMinSpaceBuffer(g, this, bufferFraction, isGrowOnly);
            foreach (Axis axis in m_yAxisList)
                axis.SetMinSpaceBuffer(g, this, bufferFraction, isGrowOnly);
            foreach (Axis axis in m_y2AxisList)
                axis.SetMinSpaceBuffer(g, this, bufferFraction, isGrowOnly);
        }

        /// <summary>
        /// ���ÿռ�
        /// </summary>
        /// <param name="axis">������</param>
        /// <param name="clientSize">�ͻ��˳ߴ�</param>
        /// <param name="spaceNorm">�ռ��׼</param>
        /// <param name="spaceAlt">�ռ����</param>
        private void SetSpace(Axis axis, float clientSize, ref float spaceNorm, ref float spaceAlt)
        {
            float crossFrac = axis.CalcCrossFraction(this);
            float crossPix = crossFrac * (1 + crossFrac) * (1 + crossFrac * crossFrac) * clientSize;
            if (!axis.IsPrimary(this) && axis.IsCrossShifted(this))
                axis.m_tmpSpace = 0;
            if (axis.m_tmpSpace < crossPix)
                axis.m_tmpSpace = 0;
            else if (crossPix > 0)
                axis.m_tmpSpace -= crossPix;
            if (axis.m_scale.m_isLabelsInside && (axis.IsPrimary(this) || (crossFrac != 0.0 && crossFrac != 1.0)))
                spaceAlt = axis.m_tmpSpace;
            else
                spaceNorm = axis.m_tmpSpace;
        }
        #endregion
    }
}
