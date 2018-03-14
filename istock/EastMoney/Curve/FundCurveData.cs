using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OwLib;

namespace OwLib
{
    /// <summary>
    /// X轴坐标标题的对齐方式
    /// </summary>
    public enum XAxisTitleAlign
    {
        /// <summary>
        /// 左侧
        /// </summary>
        Left = 0,
        /// <summary>
        /// 中间
        /// </summary>
        Center = 1,
        /// <summary>
        /// 右侧
        /// </summary>
        Right = 2
    }

    /// <summary>
    /// X轴坐标轴类型
    /// </summary>
    public enum XAxisType
    {
        /// <summary>
        /// 日期型
        /// </summary>
        Date = 1,
        /// <summary>
        /// 数字型
        /// </summary>
        Number = 2,
        /// <summary>
        /// 文本型
        /// </summary>
        Text = 3,
    }

    /// <summary>
    /// Y轴的方向
    /// </summary>
    public enum YAxisDirection
    {
        /// <summary>
        /// 左侧
        /// </summary>
        Left = 0,
        /// <summary>
        /// 右侧
        /// </summary>
        Right = 1
    }

    /// <summary>
    /// Y轴坐标标题的对齐方式
    /// </summary>
    public enum YAxisTitleAlign
    {
        /// <summary>
        /// 顶部
        /// </summary>
        Top = 0,
        /// <summary>
        /// 中间
        /// </summary>
        Middle = 1,
        /// <summary>
        /// 底部
        /// </summary>
        Bottom = 2
    }

    /// <summary>
    ///  Y轴坐标轴类型
    /// </summary>
    public enum YAxisType
    {
        /// <summary>
        /// 百分比型
        /// </summary>
        Percent = 1,
        /// <summary>
        /// 数字型
        /// </summary>
        Number = 2,
        /// <summary>
        /// 文本型
        /// </summary>
        Text = 3,
    }

    /// <summary>
    /// X轴的点信息
    /// </summary>
    public class XAxisPointData
    {
        private String m_text;
        /// <summary>
        /// 获取或者设置X轴显示的文本信息
        /// </summary>
        public String Text
        {
            get { return m_text; }
            set { m_text = value; }
        }

        private double m_value;
        /// <summary>
        /// 获取或者设置X轴的数值信息
        /// </summary>
        public double Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        /// <summary>
        /// 复制数据
        /// </summary>
        public XAxisPointData Copy()
        {
            XAxisPointData data = new XAxisPointData();
            data.Text = m_text;
            data.Value = m_value;
            return data;
        }
    }

    /// <summary>
    /// X轴刻度绘制的信息
    /// </summary>
    public class XAxisScaleDrawInfo
    {
        private int m_capacity = 0;
        /// <summary>
        /// 获取或者设置绘制的每个刻度之间的容量
        /// </summary>
        public int Capacity
        {
            get { return m_capacity; }
            set { m_capacity = value; }
        }
        
        private bool m_isMonth = false;
        /// <summary>
        /// 获取或者设置绘制的刻度是否是月份
        /// </summary>
        public bool IsMonth
        {
            get { return m_isMonth; }
            set { m_isMonth = value; }
        }

        private bool m_isYear = false;
        /// <summary>
        /// 获取或者设置绘制的刻度是否是年份
        /// </summary>
        public bool IsYear
        {
            get { return m_isYear; }
            set { m_isYear = value; }
        }

        private String m_text;
        /// <summary>
        /// 获取或者设置绘制刻度的标题
        /// </summary>
        public String Text
        {
            get { return m_text; }
            set { m_text = value; }
        }

        private double m_value;
        /// <summary>
        /// 获取或者设置绘制刻度的值
        /// </summary>
        public double Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        /// <summary>
        /// 复制数据
        /// </summary>
        public XAxisScaleDrawInfo Copy()
        {
            XAxisScaleDrawInfo data = new XAxisScaleDrawInfo();
            data.Capacity = m_capacity;
            data.IsMonth = m_isMonth;
            data.IsYear = m_isYear;
            data.Text = m_text;
            data.Value = m_value;
            return data;
        }
    }

    /// <summary>
    /// X轴的样式信息
    /// </summary>
    public class XAxisStyle
    {
        private bool m_isEqualProportion = true;
        /// <summary>
        /// 获取或者设置X轴是否等比例划分
        /// </summary>
        public bool IsEqualProportion
        {
            get { return m_isEqualProportion; }
            set { m_isEqualProportion = value; }
        }

        private String m_xAxisTitle = "";
        /// <summary>
        /// 获取或者设置X轴的标题
        /// </summary>
        public String XAxisTitle
        {
            get { return m_xAxisTitle; }
            set { m_xAxisTitle = value; }
        }

        public XAxisTitleAlign m_xAxisTitleAlign = XAxisTitleAlign.Center;
        /// <summary>
        /// 获取或者设置X轴标题的对其方式
        /// </summary>
        public XAxisTitleAlign XAxisTitleAlign
        {
            get { return m_xAxisTitleAlign; }
            set { m_xAxisTitleAlign = value; }
        }

        public XAxisType m_xAxisType = XAxisType.Number;
        /// <summary>
        /// 获取或者设置X轴坐标类型
        /// </summary>
        public XAxisType XAxisType
        {
            get { return m_xAxisType; }
            set { m_xAxisType = value; }
        }

        /// <summary>
        /// 复制数据
        /// </summary>
        public XAxisStyle Copy()
        {
            XAxisStyle data = new XAxisStyle();
            data.m_isEqualProportion = m_isEqualProportion;
            data.m_xAxisTitle = m_xAxisTitle;
            data.XAxisTitleAlign = m_xAxisTitleAlign;
            data.XAxisType = m_xAxisType;
            return data;
        }
    }

    /// <summary>
    /// 曲线数据对应Y轴的信息
    /// </summary>
    public class YAxisCurveData
    {
        private YAxisDirection m_attachYAxisDirection = YAxisDirection.Left;
        /// <summary>
        /// 获取或者设置依附左侧Y轴还是右侧Y轴
        /// 0：依附左侧Y轴
        /// 1：依附右侧Y轴
        /// </summary>
        public YAxisDirection AttachYAxisDirection
        {
            get { return m_attachYAxisDirection; }
            set { m_attachYAxisDirection = value; }
        }

        private long m_lineColor = COLOR.ARGB(255, 0, 0);
        /// <summary>
        /// 获取或者设置绘制曲线的颜色
        /// </summary>
        public long LineColor
        {
            get { return m_lineColor; }
            set { m_lineColor = value; }
        }

        private int m_lineId = 0;
        /// <summary>
        /// 当前曲线在整个坐标里面的ID（唯一）
        /// </summary>
        public int LineId
        {
            get { return m_lineId; }
            set { m_lineId = value; }
        }

        private float m_lineWidth = 1.0F;
        /// <summary>
        /// 获取或者设置绘制曲线的宽度
        /// </summary>
        public float LineWidth
        {
            get { return m_lineWidth; }
            set { m_lineWidth = value; }
        }

        private bool m_showCycle;

        /// <summary>
        /// 获取或设置是否显示点
        /// </summary>
        public bool ShowCycle
        {
            get { return m_showCycle; }
            set { m_showCycle = value; }
        }

        private String m_text;

        /// <summary>
        /// 获取或设置文字
        /// </summary>
        public String Text
        {
            get { return m_text; }
            set { m_text = value; }
        }

        private List<double> m_yPointValues = new List<double>();
        /// <summary>
        /// 获取或者设置对应Y轴的数值信息
        /// </summary>
        public List<double> YPointValues
        {
            get { return m_yPointValues; }
            set { m_yPointValues = value; }
        }

        /// <summary>
        /// 复制数据
        /// </summary>
        public YAxisCurveData Copy()
        {
            YAxisCurveData data = new YAxisCurveData();
            data.AttachYAxisDirection = m_attachYAxisDirection;
            data.LineColor = m_lineColor;
            data.LineId = m_lineId;
            data.LineWidth = m_lineWidth;
            data.YPointValues.AddRange(m_yPointValues.ToArray());
            return data;
        }
    }

    /// <summary>
    /// Y轴的样式信息
    /// </summary>
    public class YAxisStyle
    {
        private YAxisDirection _yAxisDirection = YAxisDirection.Left;
        /// <summary>
        /// 获取或者设置绘制左侧Y轴还是右侧Y轴
        /// 0：左侧Y轴
        /// 1：右侧Y轴
        /// </summary>
        public YAxisDirection YAxisDirection
        {
            get { return _yAxisDirection; }
            set { _yAxisDirection = value; }
        }

        private String _yAxisScaleUnit = "";
        /// <summary>
        /// 获取或者设置Y坐标轴的刻度的单位
        /// </summary>
        public String YAxisScaleUnit
        {
            get { return _yAxisScaleUnit; }
            set { _yAxisScaleUnit = value; }
        }

        private String _yAxisTitle = "";
        /// <summary>
        /// 获取或设置Y轴的标题
        /// </summary>
        public String YAxisTitle
        {
            get { return _yAxisTitle; }
            set { _yAxisTitle = value; }
        }

        private YAxisTitleAlign _yAxisTitleAlign = YAxisTitleAlign.Top;
        /// <summary>
        /// 获取或者设置Y轴标题的对其方式
        /// </summary>
        public YAxisTitleAlign YAxisTitleAlign
        {
            get { return _yAxisTitleAlign; }
            set { _yAxisTitleAlign = value; }
        }

        private YAxisType _yAxisType = YAxisType.Number;
        /// <summary>
        /// 获取或者设置Y轴坐标类型
        /// </summary>
        public YAxisType YAxisType
        {
            get { return _yAxisType; }
            set { _yAxisType = value; }
        }

        /// <summary>
        /// 复制数据
        /// </summary>
        public YAxisStyle Copy()
        {
            YAxisStyle data = new YAxisStyle();
            data.YAxisDirection = _yAxisDirection;
            data.YAxisScaleUnit = _yAxisScaleUnit;
            data.YAxisTitle = _yAxisTitle;
            data.YAxisTitleAlign = _yAxisTitleAlign;
            data.YAxisType = _yAxisType;
            return data;
        }
    }

    /// <summary>
    /// 图表的样式风格
    /// </summary>
    public class FundCurveStyle
    {
        private String _chartTitle = "";
        /// <summary>
        /// 图表的标题
        /// </summary>
        public String ChartTitle
        {
            get { return _chartTitle; }
            set { _chartTitle = value; }
        }

        /// <summary>
        /// 复制数据
        /// </summary>
        public FundCurveStyle Copy()
        {
            FundCurveStyle data = new FundCurveStyle();
            data.ChartTitle = _chartTitle;
            return data;
        }
    }

    /// <summary>
    /// 绘制图形用的数据结构
    /// </summary>
    public class FundCurveData
    {
        private Color _backgroundColor = Color.Black;
        /// <summary>
        /// 获取或者设置背景色
        /// </summary>
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }

        private String _chartTitle = "";
        /// <summary>
        /// 获取或者设置整个图表的标题
        /// </summary>
        public String ChartTitle
        {
            get { return _chartTitle; }
            set { _chartTitle = value; }
        }

        private List<XAxisPointData> _lstXAxisPointDatas = new List<XAxisPointData>();
        /// <summary>
        /// 获取或者设置X轴上数据的列表
        /// </summary>
        public List<XAxisPointData> LstXAxisPointDatas
        {
            get { return _lstXAxisPointDatas; }
            set { _lstXAxisPointDatas = value; }
        }

        private List<YAxisCurveData> _lstYAxisPointDatas = new List<YAxisCurveData>();
        /// <summary>
        /// 获取或者设置Y轴上数据的列表
        /// </summary>
        public List<YAxisCurveData> LstYAxisPointDatas
        {
            get { return _lstYAxisPointDatas; }
            set { _lstYAxisPointDatas = value; }
        }

        private List<YAxisStyle> _lstYAxisStyle = new List<YAxisStyle>();
        /// <summary>
        /// 获取或者设置Y轴的样式风格
        /// </summary>
        public List<YAxisStyle> LstYAxisStyle
        {
            get { return _lstYAxisStyle; }
            set { _lstYAxisStyle = value; }
        }

        private FundCurveStyle _style = new FundCurveStyle();
        /// <summary>
        /// 设置或者获取图表的样式风格
        /// </summary>
        public FundCurveStyle Style
        {
            get { return _style; }
            set { _style = value; }
        }

        private XAxisStyle _xAxisStyle = new XAxisStyle();
        /// <summary>
        /// 获取或者设置X轴的样式风格
        /// </summary>
        public XAxisStyle XAxisStyle
        {
            get { return _xAxisStyle; }
            set { _xAxisStyle = value; }
        }

        /// <summary>
        /// 复制数据
        /// </summary>
        public FundCurveData Copy()
        {
            FundCurveData data = new FundCurveData();
            data.BackgroundColor = _backgroundColor;
            data.ChartTitle = _chartTitle;
            data.Style = _style.Copy();
            data.LstXAxisPointDatas = new List<XAxisPointData>();
            foreach (XAxisPointData xPoint in _lstXAxisPointDatas)
            {
                data.LstXAxisPointDatas.Add(xPoint.Copy());
            }
            data.LstYAxisPointDatas = new List<YAxisCurveData>();
            foreach (YAxisCurveData yAxisData in _lstYAxisPointDatas)
            {
                data.LstYAxisPointDatas.Add(yAxisData.Copy());
            }
            data.LstYAxisStyle = new List<YAxisStyle>();
            foreach (YAxisStyle yStyle in _lstYAxisStyle)
            {
                data.LstYAxisStyle.Add(yStyle.Copy());
            }
            data.XAxisStyle = _xAxisStyle.Copy();
            return data;
        }
    }
}
