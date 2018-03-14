using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OwLib;

namespace OwLib
{
    /// <summary>
    /// X���������Ķ��뷽ʽ
    /// </summary>
    public enum XAxisTitleAlign
    {
        /// <summary>
        /// ���
        /// </summary>
        Left = 0,
        /// <summary>
        /// �м�
        /// </summary>
        Center = 1,
        /// <summary>
        /// �Ҳ�
        /// </summary>
        Right = 2
    }

    /// <summary>
    /// X������������
    /// </summary>
    public enum XAxisType
    {
        /// <summary>
        /// ������
        /// </summary>
        Date = 1,
        /// <summary>
        /// ������
        /// </summary>
        Number = 2,
        /// <summary>
        /// �ı���
        /// </summary>
        Text = 3,
    }

    /// <summary>
    /// Y��ķ���
    /// </summary>
    public enum YAxisDirection
    {
        /// <summary>
        /// ���
        /// </summary>
        Left = 0,
        /// <summary>
        /// �Ҳ�
        /// </summary>
        Right = 1
    }

    /// <summary>
    /// Y���������Ķ��뷽ʽ
    /// </summary>
    public enum YAxisTitleAlign
    {
        /// <summary>
        /// ����
        /// </summary>
        Top = 0,
        /// <summary>
        /// �м�
        /// </summary>
        Middle = 1,
        /// <summary>
        /// �ײ�
        /// </summary>
        Bottom = 2
    }

    /// <summary>
    ///  Y������������
    /// </summary>
    public enum YAxisType
    {
        /// <summary>
        /// �ٷֱ���
        /// </summary>
        Percent = 1,
        /// <summary>
        /// ������
        /// </summary>
        Number = 2,
        /// <summary>
        /// �ı���
        /// </summary>
        Text = 3,
    }

    /// <summary>
    /// X��ĵ���Ϣ
    /// </summary>
    public class XAxisPointData
    {
        private String m_text;
        /// <summary>
        /// ��ȡ��������X����ʾ���ı���Ϣ
        /// </summary>
        public String Text
        {
            get { return m_text; }
            set { m_text = value; }
        }

        private double m_value;
        /// <summary>
        /// ��ȡ��������X�����ֵ��Ϣ
        /// </summary>
        public double Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        /// <summary>
        /// ��������
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
    /// X��̶Ȼ��Ƶ���Ϣ
    /// </summary>
    public class XAxisScaleDrawInfo
    {
        private int m_capacity = 0;
        /// <summary>
        /// ��ȡ�������û��Ƶ�ÿ���̶�֮�������
        /// </summary>
        public int Capacity
        {
            get { return m_capacity; }
            set { m_capacity = value; }
        }
        
        private bool m_isMonth = false;
        /// <summary>
        /// ��ȡ�������û��ƵĿ̶��Ƿ����·�
        /// </summary>
        public bool IsMonth
        {
            get { return m_isMonth; }
            set { m_isMonth = value; }
        }

        private bool m_isYear = false;
        /// <summary>
        /// ��ȡ�������û��ƵĿ̶��Ƿ������
        /// </summary>
        public bool IsYear
        {
            get { return m_isYear; }
            set { m_isYear = value; }
        }

        private String m_text;
        /// <summary>
        /// ��ȡ�������û��ƿ̶ȵı���
        /// </summary>
        public String Text
        {
            get { return m_text; }
            set { m_text = value; }
        }

        private double m_value;
        /// <summary>
        /// ��ȡ�������û��ƿ̶ȵ�ֵ
        /// </summary>
        public double Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        /// <summary>
        /// ��������
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
    /// X�����ʽ��Ϣ
    /// </summary>
    public class XAxisStyle
    {
        private bool m_isEqualProportion = true;
        /// <summary>
        /// ��ȡ��������X���Ƿ�ȱ�������
        /// </summary>
        public bool IsEqualProportion
        {
            get { return m_isEqualProportion; }
            set { m_isEqualProportion = value; }
        }

        private String m_xAxisTitle = "";
        /// <summary>
        /// ��ȡ��������X��ı���
        /// </summary>
        public String XAxisTitle
        {
            get { return m_xAxisTitle; }
            set { m_xAxisTitle = value; }
        }

        public XAxisTitleAlign m_xAxisTitleAlign = XAxisTitleAlign.Center;
        /// <summary>
        /// ��ȡ��������X�����Ķ��䷽ʽ
        /// </summary>
        public XAxisTitleAlign XAxisTitleAlign
        {
            get { return m_xAxisTitleAlign; }
            set { m_xAxisTitleAlign = value; }
        }

        public XAxisType m_xAxisType = XAxisType.Number;
        /// <summary>
        /// ��ȡ��������X����������
        /// </summary>
        public XAxisType XAxisType
        {
            get { return m_xAxisType; }
            set { m_xAxisType = value; }
        }

        /// <summary>
        /// ��������
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
    /// �������ݶ�ӦY�����Ϣ
    /// </summary>
    public class YAxisCurveData
    {
        private YAxisDirection m_attachYAxisDirection = YAxisDirection.Left;
        /// <summary>
        /// ��ȡ���������������Y�ỹ���Ҳ�Y��
        /// 0���������Y��
        /// 1�������Ҳ�Y��
        /// </summary>
        public YAxisDirection AttachYAxisDirection
        {
            get { return m_attachYAxisDirection; }
            set { m_attachYAxisDirection = value; }
        }

        private long m_lineColor = COLOR.ARGB(255, 0, 0);
        /// <summary>
        /// ��ȡ�������û������ߵ���ɫ
        /// </summary>
        public long LineColor
        {
            get { return m_lineColor; }
            set { m_lineColor = value; }
        }

        private int m_lineId = 0;
        /// <summary>
        /// ��ǰ�������������������ID��Ψһ��
        /// </summary>
        public int LineId
        {
            get { return m_lineId; }
            set { m_lineId = value; }
        }

        private float m_lineWidth = 1.0F;
        /// <summary>
        /// ��ȡ�������û������ߵĿ��
        /// </summary>
        public float LineWidth
        {
            get { return m_lineWidth; }
            set { m_lineWidth = value; }
        }

        private bool m_showCycle;

        /// <summary>
        /// ��ȡ�������Ƿ���ʾ��
        /// </summary>
        public bool ShowCycle
        {
            get { return m_showCycle; }
            set { m_showCycle = value; }
        }

        private String m_text;

        /// <summary>
        /// ��ȡ����������
        /// </summary>
        public String Text
        {
            get { return m_text; }
            set { m_text = value; }
        }

        private List<double> m_yPointValues = new List<double>();
        /// <summary>
        /// ��ȡ�������ö�ӦY�����ֵ��Ϣ
        /// </summary>
        public List<double> YPointValues
        {
            get { return m_yPointValues; }
            set { m_yPointValues = value; }
        }

        /// <summary>
        /// ��������
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
    /// Y�����ʽ��Ϣ
    /// </summary>
    public class YAxisStyle
    {
        private YAxisDirection _yAxisDirection = YAxisDirection.Left;
        /// <summary>
        /// ��ȡ�������û������Y�ỹ���Ҳ�Y��
        /// 0�����Y��
        /// 1���Ҳ�Y��
        /// </summary>
        public YAxisDirection YAxisDirection
        {
            get { return _yAxisDirection; }
            set { _yAxisDirection = value; }
        }

        private String _yAxisScaleUnit = "";
        /// <summary>
        /// ��ȡ��������Y������Ŀ̶ȵĵ�λ
        /// </summary>
        public String YAxisScaleUnit
        {
            get { return _yAxisScaleUnit; }
            set { _yAxisScaleUnit = value; }
        }

        private String _yAxisTitle = "";
        /// <summary>
        /// ��ȡ������Y��ı���
        /// </summary>
        public String YAxisTitle
        {
            get { return _yAxisTitle; }
            set { _yAxisTitle = value; }
        }

        private YAxisTitleAlign _yAxisTitleAlign = YAxisTitleAlign.Top;
        /// <summary>
        /// ��ȡ��������Y�����Ķ��䷽ʽ
        /// </summary>
        public YAxisTitleAlign YAxisTitleAlign
        {
            get { return _yAxisTitleAlign; }
            set { _yAxisTitleAlign = value; }
        }

        private YAxisType _yAxisType = YAxisType.Number;
        /// <summary>
        /// ��ȡ��������Y����������
        /// </summary>
        public YAxisType YAxisType
        {
            get { return _yAxisType; }
            set { _yAxisType = value; }
        }

        /// <summary>
        /// ��������
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
    /// ͼ�����ʽ���
    /// </summary>
    public class FundCurveStyle
    {
        private String _chartTitle = "";
        /// <summary>
        /// ͼ��ı���
        /// </summary>
        public String ChartTitle
        {
            get { return _chartTitle; }
            set { _chartTitle = value; }
        }

        /// <summary>
        /// ��������
        /// </summary>
        public FundCurveStyle Copy()
        {
            FundCurveStyle data = new FundCurveStyle();
            data.ChartTitle = _chartTitle;
            return data;
        }
    }

    /// <summary>
    /// ����ͼ���õ����ݽṹ
    /// </summary>
    public class FundCurveData
    {
        private Color _backgroundColor = Color.Black;
        /// <summary>
        /// ��ȡ�������ñ���ɫ
        /// </summary>
        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set { _backgroundColor = value; }
        }

        private String _chartTitle = "";
        /// <summary>
        /// ��ȡ������������ͼ��ı���
        /// </summary>
        public String ChartTitle
        {
            get { return _chartTitle; }
            set { _chartTitle = value; }
        }

        private List<XAxisPointData> _lstXAxisPointDatas = new List<XAxisPointData>();
        /// <summary>
        /// ��ȡ��������X�������ݵ��б�
        /// </summary>
        public List<XAxisPointData> LstXAxisPointDatas
        {
            get { return _lstXAxisPointDatas; }
            set { _lstXAxisPointDatas = value; }
        }

        private List<YAxisCurveData> _lstYAxisPointDatas = new List<YAxisCurveData>();
        /// <summary>
        /// ��ȡ��������Y�������ݵ��б�
        /// </summary>
        public List<YAxisCurveData> LstYAxisPointDatas
        {
            get { return _lstYAxisPointDatas; }
            set { _lstYAxisPointDatas = value; }
        }

        private List<YAxisStyle> _lstYAxisStyle = new List<YAxisStyle>();
        /// <summary>
        /// ��ȡ��������Y�����ʽ���
        /// </summary>
        public List<YAxisStyle> LstYAxisStyle
        {
            get { return _lstYAxisStyle; }
            set { _lstYAxisStyle = value; }
        }

        private FundCurveStyle _style = new FundCurveStyle();
        /// <summary>
        /// ���û��߻�ȡͼ�����ʽ���
        /// </summary>
        public FundCurveStyle Style
        {
            get { return _style; }
            set { _style = value; }
        }

        private XAxisStyle _xAxisStyle = new XAxisStyle();
        /// <summary>
        /// ��ȡ��������X�����ʽ���
        /// </summary>
        public XAxisStyle XAxisStyle
        {
            get { return _xAxisStyle; }
            set { _xAxisStyle = value; }
        }

        /// <summary>
        /// ��������
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
