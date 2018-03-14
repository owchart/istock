using System;
using System.Collections.Generic;
using System.Text;
using OwLib;

namespace OwLib
{
    /// <summary>
    /// ��������ͼ
    /// </summary>
    public class FundCurveDiv:DivA
    {
        /// <summary>
        /// ��������
        /// </summary>
        public FundCurveDiv()
        {
            ForeColor = COLOR.ARGB(0, 0, 0);
        }

        // Ĭ�ϻ��Ƶ�İ뾶
        private const float DEFAULT_POINT_RADIUS = 6.0F;
        // Ĭ����ʾ�Ŀ̶���
        private const int DEFAULY_SCALE_COUNT = 4;
        // Ĭ�ϻ��ƵĿ̶ȵĳ��Ȼ��߸߶�
        private const float DEFAULT_SCALE_LENGTH = 3;
        // Ĭ�ϻ���Ŀ��
        private const float DEFAULT_SLIDER_WIDTH = 38;
        // Ĭ�ϻ���ĸ߶�
        private const float DEFAULT_SLIDER_HEIGHT = 18;
        // �ߵĿ��1
        private const int LINE_WIDTH_1 = 1;
        // �ߵ���ʽ--ʵ��
        private const int LINE_STYLE_SOLID = 0;
        // �ߵ���ʽ--����
        private const int LINE_STYLE_DASH = 1;
        // �ߵ���ʽ--����
        private const int LINT_STYLE_DOT = 2;
        // �̶ȵ����������
        private const int MAX_SCALE_SPAN = 50;
        // ͼ�������ٵĵ�ĸ���
        private const int MIN_POINT_COUNT_IN_CURVE = 10;
        // �̶ȵ���С�������
        private const int MIN_SCALE_SPAN = 30;

        private FONT m_axisTitleFont = new FONT("����", 12, false, false, false);
        /// <summary>
        /// ��ȡ����������������������
        /// </summary>
        public FONT AxisTitleFont
        {
            get { return m_axisTitleFont; }
            set { m_axisTitleFont = value; }
        }

        private int m_curveMargin = 18;
        /// <summary>
        /// ��ȡ�����������ߵ�Margin
        /// </summary>
        public int CurveMargin
        {
            get { return m_curveMargin; }
            set { m_curveMargin = value; }
        }

        // ��������Ϣ��ʾ��ǰ׺
        private String m_curvePointDescPrefix = "ֵ";
        /// <summary>
        /// ���û��߻�ȡ��������Ϣ��ʾ��ǰ׺
        /// </summary>
        public String CurvePointDescPrefix
        {
            set { m_curvePointDescPrefix = value; }
            get { return m_curvePointDescPrefix; }
        }

        private FONT m_curveTitleFont = new FONT("����", 12, false, false, false);
        /// <summary>
        /// ��ȡ�����������߱��������
        /// </summary>
        public FONT CurveTitleFont
        {
            get { return m_curveTitleFont; }
            set { m_curveTitleFont = value; }
        }

        private int m_curveTitleHeight = 30;
        /// <summary>
        /// ��ȡ�����������߱���ĸ߶�
        /// </summary>
        public int CurveTitleHeight
        {
            get { return m_curveTitleHeight; }
            set { m_curveTitleHeight = value; }
        }

        /// ����ͼ��Ϣ
        private FundCurveData m_fundCurveData = new FundCurveData();
        /// <summary>
        /// ��ȡ������������ͼ��Ϣ
        /// </summary>
        public FundCurveData FundCurveData
        {
            get { return m_fundCurveData; }
            set
            {
                m_fundCurveData = value;
                DrawFundCurve();
            }
        }

        private bool m_isShowPointDescription = true;
        /// <summary>
        /// ��ȡ���������Ƿ���ʾ���˵��
        /// </summary>
        public bool IsShowPointDescription
        {
            get { return m_isShowPointDescription; }
            set { m_isShowPointDescription = value; }
        }

        private long m_lineColor = COLOR.ARGB(76, 76, 76);
        /// <summary>
        /// ��ȡ���������ߵ���ɫ
        /// </summary>
        public long LineColor
        {
            get { return m_lineColor; }
            set { m_lineColor = value; }
        }

        private long m_pointDescColor = COLOR.ARGB(192, 192, 192);
        /// <summary>
        /// ��ȡ�������õ�˵������ɫ
        /// </summary>
        public long PointDescColor
        {
            get { return m_pointDescColor; }
            set { m_pointDescColor = value; }
        }

        private FONT m_pointDescFont = new FONT("Arial", 12, false, false, false);
        /// <summary>
        /// ��ȡ�������õ�˵��������
        /// </summary>
        public FONT PointDescFont
        {
            get { return m_pointDescFont; }
            set { m_pointDescFont = value; }
        }

        private long m_scaleMonthColor = COLOR.ARGB(80, 255, 80);
        /// <summary>
        /// ��ȡ�������ÿ̶��µ���ɫ
        /// </summary>
        public long ScaleMonthColor
        {
            get { return m_scaleMonthColor; }
            set { m_scaleMonthColor = value; }
        }

        private long m_scaleYearColor = COLOR.ARGB(255, 82, 82);
        /// <summary>
        /// ��ȡ�������ÿ̶������ɫ
        /// </summary>
        public long ScaleYearColor
        {
            get { return m_scaleYearColor; }
            set { m_scaleYearColor = value; }
        }

        private FONT m_scaleTitleFont = new FONT("����", 12, false, false, false);
        /// <summary>
        /// ��ȡ�������ÿ̶ȱ��������
        /// </summary>
        public FONT ScaleTitleFont
        {
            get { return m_scaleTitleFont; }
            set { m_scaleTitleFont = value; }
        }

        private long m_titleColor = COLOR.ARGB(160, 160, 160);
        /// <summary>
        /// ��ȡ�������ñ������ɫ
        /// </summary>
        public long TitleColor
        {
            get { return m_titleColor; }
            set { m_titleColor = value; }
        }

        private int m_xAsixTitleHeight = 35;
        /// <summary>
        /// ��ȡ��������X������ĸ߶�
        /// </summary>
        public int XAsixTitleHeight
        {
            get { return m_xAsixTitleHeight; }
            set { m_xAsixTitleHeight = value; }
        }
        /// <summary>
        /// Y�����Ŀ��
        /// </summary>
        private int m_yAsixTitleWidth = 80;
        /// <summary>
        /// ���û��߻�ȡY�����Ŀ��
        /// </summary>
        public int YAsixTitleWidth
        {
            set { m_yAsixTitleWidth = value; }
            get { return m_yAsixTitleWidth; }
        }

        private int m_zoomPointCount = 10;
        /// <summary>
        /// ��ȡ��������������С�ĵ�ĸ���
        /// </summary>
        public int ZoomPointCount
        {
            get { return m_zoomPointCount; }
            set { m_zoomPointCount = value; }
        }

        // ��ǰ����±�
        private int m_currentPointIndex = 0;
        // ���Y�����б�
        private IDictionary<int, List<double>> m_dicYLocations = new Dictionary<int, List<double>>();
        // ��������±�
        private int m_endPointIndex = int.MaxValue;
        // �Ƿ����ʮ����
        private bool m_isDrawCrossLine = false;
        // �Ƿ��Ѿ�������ݷ���
        private bool m_isFinishedRedistributionData = false;
        // ���X�����б�
        private List<double> m_lstXLocations = new List<double>();
        // X��̶ȵı���
        private List<String> m_lstXScalesText = new List<String>();
        // X��̶Ȼ�����Ϣ
        private List<XAxisScaleDrawInfo> m_lstXAxisScaleDrawInfos = new List<XAxisScaleDrawInfo>();
        // ����X����ֵ�ı���
        private List<String> m_lstXValueText = new List<String>();
        // Y������ϵ����Ϣ
        private List<YAxisStyle> m_lstYAxisStyles = new List<YAxisStyle>();
        // �Ƿ�������
        private bool m_isMouseOperate = false;
        // ����ƶ�������
        private POINTF m_mouseMovePoint;
        // ��ǰѡ�е��ߵ�����
        private int m_selectedLineIndex = -1;
        // ��ʼ����±�
        private int m_startPointIndex = 0;
        // ����ͼ�������ܵĵ�ĸ���
        private int m_totalPointCount = 0;
        // ����ͼ�е�ǰ��ĸ���
        private int m_currentPointCountInCurve = 30;
        // Y������Ŀ��(����)
        private int m_yAsixLeftTitleWidth = 0;
        // Y������Ŀ��(����)
        private int m_yAsixRightTitleWidth = 0;

        /// <summary>
        /// ����ѡ�е��ߵ�����
        /// </summary>
        /// <param name="mouseLocation">����λ��</param>
        /// <returns></returns>
        private int CalculateSelectLineIndex(POINT mouseLocation)
        {
            int xIndex = 0, selectedLineIndex = -1;
            double kSlope, bYIntercept, lineYLocation;
            double xStartLocation = 0, xEndLocation = 0, yStartLocation = 0, yEndLocation = 0;
            for (int i = 1; i < m_lstXLocations.Count; i++)
            {
                if (m_selectedLineIndex >= 0)
                {
                    break;
                }
                if (m_lstXLocations[i] >= mouseLocation.x
                    && m_lstXLocations[i - 1] <= mouseLocation.x)
                {
                    xIndex = i - 1;
                    xStartLocation = m_lstXLocations[xIndex];
                    xEndLocation = m_lstXLocations[i];
                    foreach (KeyValuePair<int, List<double>> yLocations in m_dicYLocations)
                    {
                        List<double> lstYLocations = yLocations.Value;
                        yStartLocation = lstYLocations[xIndex];
                        yEndLocation = lstYLocations[i];
                        if (yStartLocation > yEndLocation)
                        {
                            kSlope = (yEndLocation - yStartLocation) / (xEndLocation - xStartLocation);
                            bYIntercept = yEndLocation - kSlope * xEndLocation;
                        }
                        else
                        {
                            kSlope = (yStartLocation - yEndLocation) / (xStartLocation - xEndLocation);
                            bYIntercept = yEndLocation - kSlope * xEndLocation;
                        }

                        lineYLocation = kSlope * m_mouseMovePoint.x + bYIntercept;
                        if (Math.Abs(lineYLocation - m_mouseMovePoint.y) < 10)
                        {
                            selectedLineIndex = yLocations.Key;
                            break;
                        }
                    }
                }
            }

            return selectedLineIndex;
        }

        /// <summary>
        /// �����������������
        /// </summary>
        /// <param name="aLen">������������</param>
        /// <param name="aLogLen">�������߼�����</param>
        /// <param name="maxSpan">�̶ȵ����������</param>
        /// <param name="minSpan">�̶ȵ���С�������</param>
        /// <param name="defaultCount">Ĭ����ʾ�Ŀ̶���</param>
        /// <param name="step">������̶ȼ���߼�����</param>
        /// <param name="nDecidig">�������߼������ľ���(С����λ��)</param>
        private void CalculateStepLen(double aLen, double aLogLen, int maxSpan, int minSpan, int defaultCount, ref double step, ref int nDecidig)
        {
            step = 0;
            nDecidig = 0;
            if (maxSpan < MAX_SCALE_SPAN)
            {
                maxSpan = MAX_SCALE_SPAN;
            }
            if (minSpan < MIN_SCALE_SPAN)
            {
                minSpan = MIN_SCALE_SPAN;
            }
            //������ʾ�Ŀ̶���
            int nMinCount = Convert.ToInt32(Math.Ceiling((double)aLen / maxSpan));  // ������ʾ�̶���
            int nMaxCount = Convert.ToInt32(Math.Floor((double)aLen / minSpan));    // �����ʾ�̶���
            //ʵ����ʾ�̶���Ϊ��ӽ���Ĭ�Ͽ̶���������������ٿ̶�֮��
            int nCount = defaultCount;
            nCount = Math.Max(nMinCount, nCount);
            nCount = Math.Min(nMaxCount, nCount);
            //����Ϊ1���̶�
            nCount = Math.Max(nCount, 1);
            //����̶�֮����߼����
            double dLogStep = aLogLen / nCount; //�ܵ��߼����ȳ��Կ̶�������Ϊÿ���̶ȵ��߼�����
            //������㷨�����ҵ��������dLogStep���������������㷨�����Լ�д�ģ�����Բο��£�Ҳ�����Լ���
            bool bStart = false;
            //��10��15�η�ѭ����10��-6�η��������������ƥ���������
            for (int i = 15; i >= -6; i--)
            {
                double dDivisor = Math.Pow(10, i); //����
                if (dDivisor < 1) //����С��1��˵�����뵽С��λ��ƥ�䣬��Ҫ�����ȼ�1
                {
                    nDecidig++;
                }
                int nTemp = Convert.ToInt32(Math.Floor(dLogStep / dDivisor));
                if (bStart)
                {
                    if (nTemp < 4)  //�����0��1��2��3��ʡ����һλ
                    {
                        if (nDecidig > 0)
                            nDecidig--;
                    }
                    else if (nTemp >= 4 && nTemp <= 6)  //�����4��5��6���Ϊ5
                    {
                        nTemp = 5;
                        step += nTemp * dDivisor;
                    }
                    else  //�����7��8��9���һλ
                    {
                        step += 10 * dDivisor;
                        if (nDecidig > 0)
                            nDecidig--;
                    }
                    break;
                }
                else if (nTemp > 0)
                {
                    step = nTemp * dDivisor + step;
                    dLogStep -= step;
                    bStart = true;
                }
            }
        }

        /// <summary>
        /// ���������ռ��������ڼ�
        /// </summary>
        /// <param name="year">��</param>
        /// <param name="month">��</param>
        /// <param name="day">��</param>
        /// <returns>����</returns>
        private String CalculateWeek(int year, int month, int day)
        {
            //��ķ����ɭ���㹫ʽ
            DateTime a = new DateTime(year, month, day);
            System.DayOfWeek weekDay = a.DayOfWeek;
            String weekstr = "";
            switch (Convert.ToInt32(weekDay))
            {
                case 0: weekstr = "��"; break;
                case 1: weekstr = "һ"; break;
                case 2: weekstr = "��"; break;
                case 3: weekstr = "��"; break;
                case 4: weekstr = "��"; break;
                case 5: weekstr = "��"; break;
                case 6: weekstr = "��"; break;
                case 7: weekstr = "��"; break;
            }
            return weekstr;
        }

        /// <summary>
        /// ����X��̶���Ϣ
        /// </summary>
        /// <param name="lstXAxisPointDatas">X������������Ϣ</param>
        /// <param name="xAxisStyle">X������������Ϣ</param>
        /// <param name="drawPoint">��ͼ��������</param>1
        /// <param name="drawSize">��ͼ�����С</param>
        /// <param name="xAxisMinValue">��Сֵ</param>
        /// <param name="xAxisMaxValue">���ֵ</param>
        /// <param name="xAxisStep">X��̶ȵļ��</param>
        /// <param name="xAxisDecidig">X��̶ȵļ����ȷ��</param>
        /// <returns></returns>
        private int CalculateXAxisScaleInfo(List<XAxisPointData> lstXAxisPointDatas, XAxisStyle xAxisStyle, POINT drawPoint, SIZE drawSize,
            ref double xAxisMinValue, ref double xAxisMaxValue, ref double xAxisStep, ref int xAxisDecidig)
        {
            xAxisMinValue = lstXAxisPointDatas[m_startPointIndex].Value;
            xAxisMaxValue = lstXAxisPointDatas[m_endPointIndex].Value;
            xAxisStep = 0;
            xAxisDecidig = 0;
            switch (xAxisStyle.XAxisType)
            {
                // ������
                case XAxisType.Date:
                case XAxisType.Text:
                    break;
                // ��ֵ��
                case XAxisType.Number:
                    // X��ȷִ���
                    if (xAxisStyle.IsEqualProportion)
                    {
                        double xAxisLen = xAxisMaxValue - xAxisMinValue;
                        int xAxisSpan = drawSize.cx / m_currentPointCountInCurve;
                        // ����Y��̶�֮��Ĳ���
                        CalculateStepLen(drawSize.cx, xAxisLen, (int)(xAxisSpan * 1.5), xAxisSpan, DEFAULY_SCALE_COUNT, ref xAxisStep, ref xAxisDecidig);
                        // ������͵�
                        if (xAxisMinValue % xAxisStep != 0)
                        {
                            int count = (int)(xAxisMinValue / xAxisStep);
                            if (count < 0)
                            {
                                xAxisMinValue = xAxisStep * (count - 1);
                            }
                            else
                            {
                                xAxisMinValue = xAxisStep * count;
                            }
                        }
                        // ������ߵ�
                        if (xAxisMaxValue % xAxisStep != 0)
                        {
                            int count = (int)(xAxisMaxValue / xAxisStep);
                            if (count < 0)
                            {
                                xAxisMaxValue = xAxisStep * count;
                            }
                            else
                            {
                                xAxisMaxValue = xAxisStep * (count + 1);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            return 1;
        }

        /// <summary>
        /// ����X��Ŀ̶���Ϣ
        /// </summary>
        /// <param name="curveData">��������</param>
        /// <param name="drawPoint">��ͼ��������</param>
        /// <param name="drawSize">��ͼ�����С</param>
        private void CalculateXAxisScale(FundCurveData curveData, POINT drawPoint, SIZE drawSize)
        {
            XAxisStyle xAxisStyle = curveData.XAxisStyle;
            List<XAxisPointData> lstXAxisPointDatas = curveData.LstXAxisPointDatas;
            int xAxisDecidig = 0;// X���߼������ľ���(С����λ��)
            int minXAxisLocation = drawPoint.x;
            int maxXAxisLocation = minXAxisLocation + drawSize.cx;
            double xAxisLocation = 0, xAxisPointPerSeparater = 0;
            // X�������Сֵ��X��̶ȼ���߼�����
            double xAxisMinValue = 0, xAxisMaxValue = 0, xAxisStep = 0;
            // ����X��Ŀ̶���Ϣ
            CalculateXAxisScaleInfo(lstXAxisPointDatas, xAxisStyle, drawPoint, drawSize, ref xAxisMinValue, ref xAxisMaxValue, ref xAxisStep, ref xAxisDecidig);
            switch (xAxisStyle.XAxisType)
            {
                // ������
                case XAxisType.Date:
                    int dateValue = 0;
                    String strDay = "", strMonth = "", strYear = "", strDate = "";
                    // ����X��Ŀ̶ȵ��б�
                    xAxisPointPerSeparater = (double)drawSize.cx / (m_currentPointCountInCurve - 1);
                    // ����X������ÿ���㻬�����ʾ��Ϣ
                    for (int i = m_startPointIndex; i <= m_endPointIndex; i++)
                    {
                        xAxisLocation = (i - m_startPointIndex) * xAxisPointPerSeparater + minXAxisLocation;
                        m_lstXLocations.Add(xAxisLocation);
                        dateValue = (int)lstXAxisPointDatas[i].Value;
                        strYear = YearConvert(dateValue);
                        strMonth = MonthConvert(dateValue);
                        strDay = DayConvert(dateValue);
                        strDate = strYear + "/" + strMonth + "/" + strDay + "/" + CalculateWeek(Convert.ToInt16(strYear), Convert.ToInt16(strMonth), Convert.ToInt16(strDay));
                        m_lstXValueText.Add(strDate);
                    }
                    XAxisScaleDrawInfo scaleDrawInfo;
                    // ��
                    if (xAxisPointPerSeparater >= 20)
                    {
                        for (int i = m_startPointIndex; i <= m_endPointIndex; i++)
                        {
                            scaleDrawInfo = new XAxisScaleDrawInfo();
                            dateValue = (int)lstXAxisPointDatas[i].Value;
                            scaleDrawInfo.Value = dateValue;
                            scaleDrawInfo.Capacity = 1;
                            strDay = DayConvert(dateValue);
                            if (strDay == "01")
                            {
                                strMonth = MonthConvert(dateValue);
                                if (strMonth == "01")
                                {
                                    strYear = YearConvert(dateValue);
                                    scaleDrawInfo.Text = strYear;
                                    scaleDrawInfo.IsYear = true;
                                    m_lstXAxisScaleDrawInfos.Add(scaleDrawInfo);
                                    m_lstXScalesText.Add(strYear);
                                    continue;
                                }
                                scaleDrawInfo.Text = strMonth;
                                scaleDrawInfo.IsMonth = true;
                                m_lstXAxisScaleDrawInfos.Add(scaleDrawInfo);
                                m_lstXScalesText.Add(strMonth);
                                continue;
                            }
                            scaleDrawInfo.Text = strDay;
                            m_lstXAxisScaleDrawInfos.Add(scaleDrawInfo);
                            m_lstXScalesText.Add(strDay);
                        }
                    }
                    // ��
                    else if (xAxisPointPerSeparater < 20 && xAxisPointPerSeparater >= 5)
                    {
                        String preMonth = "";
                        int capityMonth = 0;
                        int preMonthDateValue = 0;
                        for (int i = m_startPointIndex; i <= m_endPointIndex; i++)
                        {
                            capityMonth++;
                            dateValue = (int)lstXAxisPointDatas[i].Value;
                            strMonth = MonthConvert(dateValue);
                            if (preMonth == "")
                                preMonth = strMonth;
                            if (preMonthDateValue == 0)
                                preMonthDateValue = dateValue;
                            if (strMonth == preMonth)
                                continue;
                            scaleDrawInfo = new XAxisScaleDrawInfo();
                            if (preMonth == "01")
                            {
                                strYear = YearConvert(preMonthDateValue);
                                scaleDrawInfo.Value = preMonthDateValue;
                                scaleDrawInfo.Text = strYear;
                                scaleDrawInfo.IsYear = true;
                                scaleDrawInfo.Capacity = capityMonth - 1;
                                m_lstXAxisScaleDrawInfos.Add(scaleDrawInfo);
                                capityMonth = 1;
                                m_lstXScalesText.Add(strYear);
                                preMonth = strMonth;
                                preMonthDateValue = dateValue;
                                continue;
                            }
                            scaleDrawInfo.Value = dateValue;
                            scaleDrawInfo.Text = preMonth;
                            scaleDrawInfo.IsMonth = false;
                            scaleDrawInfo.Capacity = capityMonth - 1;
                            m_lstXAxisScaleDrawInfos.Add(scaleDrawInfo);
                            capityMonth = 1;
                            m_lstXScalesText.Add(preMonth);
                            preMonth = strMonth;
                            preMonthDateValue = dateValue;
                        }
                        if (capityMonth != 0)
                        {
                            scaleDrawInfo = new XAxisScaleDrawInfo();
                            scaleDrawInfo.Text = preMonth;
                            scaleDrawInfo.Value = preMonthDateValue;
                            scaleDrawInfo.Capacity = capityMonth;
                            m_lstXAxisScaleDrawInfos.Add(scaleDrawInfo);
                            m_lstXScalesText.Add(preMonth);
                        }
                        m_lstXScalesText.Add("");
                    }
                    // ��
                    else if (xAxisPointPerSeparater < 5)
                    {
                        String preYear = "";
                        int preYearDateValue = 0;
                        int capityYear = 0;
                        for (int i = m_startPointIndex; i <= m_endPointIndex; i++)
                        {
                            dateValue = (int)lstXAxisPointDatas[i].Value;
                            strYear = YearConvert(dateValue);
                            capityYear++;
                            if (preYear == "")
                                preYear = strYear;
                            if (preYearDateValue == 0)
                                preYearDateValue = dateValue;
                            if (strYear == preYear)
                                continue;
                            scaleDrawInfo = new XAxisScaleDrawInfo();
                            scaleDrawInfo.Text = preYear;
                            scaleDrawInfo.IsYear = false;
                            scaleDrawInfo.Value = preYearDateValue;
                            scaleDrawInfo.Capacity = capityYear - 1;
                            m_lstXAxisScaleDrawInfos.Add(scaleDrawInfo);
                            capityYear = 1;
                            m_lstXScalesText.Add(preYear);
                            preYear = strYear;
                            preYearDateValue = dateValue;
                        }
                        if (capityYear != 0)
                        {
                            scaleDrawInfo = new XAxisScaleDrawInfo();
                            scaleDrawInfo.Text = preYear;
                            scaleDrawInfo.Value = preYearDateValue;
                            scaleDrawInfo.Capacity = capityYear - 1;
                            m_lstXAxisScaleDrawInfos.Add(scaleDrawInfo);
                            capityYear = 1;
                            m_lstXScalesText.Add(preYear);
                        }
                        m_lstXScalesText.Add("");
                    }
                    break;
                // ��ֵ��
                case XAxisType.Number:
                    // X��ȷִ���
                    if (xAxisStyle.IsEqualProportion)
                    {
                        xAxisPointPerSeparater = (double)drawSize.cx / (xAxisMaxValue - xAxisMinValue);
                        for (int i = m_startPointIndex; i <= m_endPointIndex; i++)
                        {
                            xAxisLocation = (lstXAxisPointDatas[i].Value - xAxisMinValue) * xAxisPointPerSeparater + minXAxisLocation;
                            m_lstXLocations.Add(xAxisLocation);
                        }
                        for (double xAxisValue = xAxisMinValue; xAxisValue <= xAxisMaxValue; xAxisValue += xAxisStep)
                        {
                            m_lstXScalesText.Add(xAxisValue.ToString());
                        }
                    }
                    else
                    {
                        for (int i = m_startPointIndex; i <= m_endPointIndex; i++)
                        {
                            xAxisLocation = (lstXAxisPointDatas[i].Value - xAxisMinValue) / (xAxisMaxValue - xAxisMinValue) * drawSize.cx;
                            xAxisLocation += minXAxisLocation;
                            m_lstXLocations.Add(xAxisLocation);
                            m_lstXScalesText.Add(lstXAxisPointDatas[i].Text);
                        }
                    }
                    break;
                // �ı���
                case XAxisType.Text:
                    xAxisPointPerSeparater = (double)drawSize.cx / (m_currentPointCountInCurve - 1);
                    for (int i = m_startPointIndex; i <= m_endPointIndex; i++)
                    {
                        xAxisLocation = (i - m_startPointIndex) * xAxisPointPerSeparater;
                        xAxisLocation += minXAxisLocation;
                        m_lstXLocations.Add(xAxisLocation);
                        m_lstXScalesText.Add(lstXAxisPointDatas[i].Text);
                        m_lstXValueText.Add(lstXAxisPointDatas[i].Text);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// ��ȡX�Ử���ֵ
        /// </summary>
        /// <param name="curveData">��������</param>
        /// <param name="xLocation">����ӦX�������</param>
        /// <param name="drawPoint">��ͼ��������</param>
        /// <param name="drawSize">��ͼ�����С</param>
        /// <returns></returns>
        private String CalculateXAsixSlidersText(FundCurveData curveData, double xLocation, POINT drawPoint, SIZE drawSize)
        {
            XAxisStyle xAxisStyle = curveData.XAxisStyle;
            int minValueXLocation = drawPoint.x;
            int maxValueXLocation = minValueXLocation + drawSize.cx;
            List<XAxisPointData> lstXAxisPointDatas = curveData.LstXAxisPointDatas;
            int nXDecidig = 0;// X���߼������ľ���(С����λ��)
            // X�������Сֵ��X��̶ȼ���߼�����
            double xAxisMinValue = 0, xAxisMaxValue = 0, dXStep = 0;
            // ����X��Ŀ̶���Ϣ
            CalculateXAxisScaleInfo(lstXAxisPointDatas, xAxisStyle, drawPoint, drawSize, ref xAxisMinValue, ref xAxisMaxValue, ref dXStep, ref nXDecidig);
            switch (xAxisStyle.XAxisType)
            {
                // ��ֵ��
                case XAxisType.Number:
                    double xValue = (xLocation - minValueXLocation) / (maxValueXLocation - minValueXLocation) * (xAxisMaxValue - xAxisMinValue) + xAxisMinValue;
                    return Math.Round(xValue, 2).ToString();
                // �ı���
                case XAxisType.Text:
                // ������
                case XAxisType.Date:
                    int index = 0;
                    int count = m_lstXLocations.Count;
                    double middle = 0;
                    for (int i = 0; i < count; i++)
                    {
                        if (xLocation == m_lstXLocations[i])
                        {
                            index = i; 
                            break;
                        }
                        else if (xLocation < m_lstXLocations[i])
                        {
                            middle = (m_lstXLocations[i] + m_lstXLocations[i - 1]) / 2;
                            if (xLocation <= middle)
                            {
                                index = i - 1;
                                break;
                            }
                            else
                            {
                                index = i;
                                break;
                            }
                        }
                    }
                    if (index > 0 && index < count)
                    {
                        return m_lstXValueText[index];
                    }
                    return m_lstXValueText[0];
                default:
                    return "";
            }
        }

        /// <summary>
        /// ����Y��Ŀ̶���Ϣ
        /// </summary>
        /// <param name="lstYAxisData">Y���������� </param>
        /// <param name="yAxisStyle">Y���������Ϣ</param>
        /// <param name="yAxisHeight">Y��ĸ߶�</param>
        /// <param name="scaleCount">Y��̶ȵĸ���</param>
        /// <param name="yAxisMaxValue">Y�����ֵ</param>
        /// <param name="yAxisMinValue">Y����Сֵ</param>
        /// <param name="dYStep">Y��̶ȵļ��</param>
        /// <param name="nYDecidig">Y��̶ȵļ����ȷ��</param>
        private void CalculateYAxisScaleInfo(List<YAxisCurveData> lstYAxisData, YAxisStyle yAxisStyle, int yAxisHeight, int scaleCount
            , ref double yAxisMaxValue, ref double yAxisMinValue, ref double dYStep, ref int nYDecidig)
        {
            yAxisMaxValue = double.MinValue;
            yAxisMinValue = double.MaxValue;
            // ����Y���ϵ����ֵ��Сֵ
            CalculateYAxisMaxAndMin(lstYAxisData, yAxisStyle, ref yAxisMaxValue, ref yAxisMinValue);
            double step = scaleCount;
            if (scaleCount == 0)
            {
                // ����Y��̶�֮��Ĳ���
                CalculateStepLen(yAxisHeight, yAxisMaxValue - yAxisMinValue, MAX_SCALE_SPAN, MIN_SCALE_SPAN, DEFAULY_SCALE_COUNT, ref dYStep, ref nYDecidig);
                step = dYStep;
            }
            // ������͵�
            if (yAxisMinValue % step == 0)
            {
                yAxisMinValue = yAxisMinValue - step;
            }
            else
            {
                int count = (int)(yAxisMinValue / step);
                if (count <= 0)
                {
                    yAxisMinValue = step * (count - 1);
                }
                else
                {
                    yAxisMinValue = step * count;
                }
            }
            // ������ߵ�
            if (yAxisMaxValue % step == 0)
            {
                yAxisMaxValue = yAxisMaxValue + step;
            }
            else
            {
                int count = (int)(yAxisMaxValue / step);
                if (count < 0)
                {
                    yAxisMaxValue = step * count;
                }
                else
                {
                    yAxisMaxValue = step * (count + 1);
                }
            }
            if (scaleCount != 0)
            {
                dYStep = (yAxisMaxValue - yAxisMinValue) / scaleCount;
            }
        }

        /// <summary>
        /// ����Y���ϵ����ֵ��Сֵ
        /// </summary>
        /// <param name="lstYAxisData">Y��������Ϣ</param>
        /// <param name="yAxisStyle">Y����ʽ��Ϣ</param>
        /// <param name="yAxisMaxValue">Y�����ֵ</param>
        /// <param name="yAxisMinValue">Y����Сֵ</param>
        private void CalculateYAxisMaxAndMin(List<YAxisCurveData> lstYAxisData, YAxisStyle yAxisStyle, ref double yAxisMaxValue, ref double yAxisMinValue)
        {
            yAxisMaxValue = double.MinValue;
            yAxisMinValue = double.MaxValue;
            int totalYLineCount = lstYAxisData.Count;
            for (int j = 0; j < totalYLineCount; j++)
            {
                // ����һ��Y��������޳���
                if (lstYAxisData[j].AttachYAxisDirection != yAxisStyle.YAxisDirection)
                {
                    continue;
                }
                for (int i = m_startPointIndex; i <= m_endPointIndex; i++)
                {
                    double yValue = lstYAxisData[j].YPointValues[i];
                    // �Ƿ�����
                    if (yValue == double.NaN)
                    {
                        continue;
                    }
                    if (yAxisMinValue > yValue)
                    {
                        yAxisMinValue = yValue;
                    }
                    if (yAxisMaxValue < yValue)
                    {
                        yAxisMaxValue = yValue;
                    }
                }
            }
            if (yAxisMaxValue.Equals(yAxisMinValue))
            {
                yAxisMaxValue = yAxisMaxValue * (1.0F + 1.0F / 100);
                yAxisMinValue = yAxisMinValue * (1.0F - 1.0F / 100);
            }
        }

        /// <summary>
        /// ���
        /// </summary>
        public void Clear()
        {
            m_currentPointCountInCurve = 30;
            m_isFinishedRedistributionData = false;
            m_dicYLocations.Clear();
            m_lstXLocations.Clear();
            m_lstXScalesText.Clear();
            m_lstXAxisScaleDrawInfos.Clear();
            m_lstXValueText.Clear();
            m_lstYAxisStyles.Clear();
        }

        /// <summary>
        /// ת������
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private String DayConvert(int date)
        {
            return ((date % 10000) % 100).ToString("D2");
        }

        /// <summary>
        /// ���ٷ���
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                Clear();
                base.Dispose();
            }
        }

        /// <summary>
        /// ��ʮ����
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="drawPoint">��ͼ��������</param>
        /// <param name="drawSize">��ͼ�����С</param>
        private void DrawCrossingLine(CPaint paint, POINT drawPoint, SIZE drawSize)
        {
            if (!m_isMouseOperate)
            {
                m_mouseMovePoint.x = (float)m_lstXLocations[m_currentPointIndex - m_startPointIndex];
                if (m_selectedLineIndex < 0)
                {
                    m_mouseMovePoint.y = (float)m_dicYLocations[0][m_currentPointIndex - m_startPointIndex];
                }
                else
                {
                    m_mouseMovePoint.y = (float)m_dicYLocations[m_selectedLineIndex][m_currentPointIndex - m_startPointIndex];
                }
            }
            else
            {
                for (int i = 0; i < m_lstXLocations.Count; i++)
                {
                    double xLocation = m_lstXLocations[i];
                    if (xLocation > m_mouseMovePoint.x)
                    {
                        m_currentPointIndex = i + m_startPointIndex;
                        break;
                    }
                }
            }
            if (!m_isDrawCrossLine)
            {
                return;
            }
            if (!IsMatchCurveLocation(m_mouseMovePoint.y, drawPoint, drawSize))
            {
                return;
            }
            if (m_mouseMovePoint.x < drawPoint.x
                || m_mouseMovePoint.x > drawPoint.x + drawSize.cx)
            {
                return;
            }
            //������
            paint.DrawLine(m_lineColor, LINE_WIDTH_1, LINE_STYLE_SOLID, drawPoint.x, (int)m_mouseMovePoint.y, drawPoint.x + drawSize.cx, (int)m_mouseMovePoint.y);
            //������
            paint.DrawLine(m_lineColor, LINE_WIDTH_1, LINE_STYLE_SOLID, (int)m_mouseMovePoint.x, drawPoint.y, (int)m_mouseMovePoint.x, drawPoint.y + drawSize.cy);
        }

        /// <summary>
        /// ���ƻ�������ͼ
        /// </summary>
        private void DrawFundCurve()
        {
            if (m_fundCurveData == null)
            {
                return;
            }
            // ���������е����������ʼ����
            List<XAxisPointData> lstXAxisPointDatas = m_fundCurveData.LstXAxisPointDatas;
            if (lstXAxisPointDatas == null || lstXAxisPointDatas.Count <= 1)
            {
                m_currentPointCountInCurve = 0;
                return;
            }
            m_currentPointCountInCurve = 30;
            if (lstXAxisPointDatas.Count < m_currentPointCountInCurve)
            {
                m_currentPointCountInCurve = lstXAxisPointDatas.Count;
            }
            m_totalPointCount = lstXAxisPointDatas.Count;
            m_endPointIndex = lstXAxisPointDatas.Count - 1;
            m_startPointIndex = m_endPointIndex - m_currentPointCountInCurve + 1;
            m_currentPointIndex = m_endPointIndex;
            // ���·������ߺ�Y������������
            RedistributionYAxisAndCurveData(m_fundCurveData);
            m_isFinishedRedistributionData = true;
            Invalidate();
        }

        /// <summary>
        /// ���ƻ�������ͼ���
        /// <param name="paint">��ͼ����</param>
        /// <param name="curveData">��������</param>
        /// <param name="drawPoint">��ͼ��������</param>
        /// <param name="drawSize">��ͼ�����С</param>
        /// </summary>
        private void DrawFundCurveFrame(CPaint paint, FundCurveData curveData, POINT drawPoint, SIZE drawSize)
        {
            // ��������ͼ��ı���
            if (!String.IsNullOrEmpty(curveData.ChartTitle))
            {
                SIZE titleSizeF = paint.TextSize(curveData.ChartTitle, m_curveTitleFont);
                float titleXLocation = (float)(drawPoint.x + drawSize.cx / 2) - titleSizeF.cx / 2;
                float titleYLocation = drawPoint.y - m_curveMargin / 2;
                RECT rect = new RECT(titleXLocation, titleYLocation - titleSizeF.cy, titleXLocation + titleSizeF.cx, titleYLocation);
                paint.DrawText(curveData.ChartTitle, m_titleColor, m_curveTitleFont, rect);
            }
            //��������
            paint.DrawLine(m_lineColor, LINE_WIDTH_1, LINE_STYLE_SOLID, drawPoint.x, drawPoint.y + drawSize.cy, drawPoint.x, drawPoint.y);
            //��������
            paint.DrawLine(m_lineColor, LINE_WIDTH_1, LINE_STYLE_SOLID, drawPoint.x + drawSize.cx, drawPoint.y + drawSize.cy, drawPoint.x + drawSize.cx, drawPoint.y);
            //���Ϻ���
            paint.DrawLine(m_lineColor, LINE_WIDTH_1, LINE_STYLE_SOLID, drawPoint.x, drawPoint.y, drawPoint.x + drawSize.cx, drawPoint.y);
            //���º���
            paint.DrawLine(m_lineColor, LINE_WIDTH_1, LINE_STYLE_SOLID, drawPoint.x, drawPoint.y + drawSize.cy, drawPoint.x + drawSize.cx, drawPoint.y + drawSize.cy);
        }

        /// <summary>
        /// ���Ƶ������
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="curveData">��������</param>
        /// <param name="drawPoint">��ͼ��������</param>
        /// <param name="drawSize">��ͼ�����С</param>
        private void DrawPointAndCurve(CPaint paint, FundCurveData data, POINT drawPoint, SIZE drawSize)
        {
            POINT point = new POINT();
            List<POINT> lstPoints = new List<POINT>();
            foreach (KeyValuePair<int, List<double>> yLocations in m_dicYLocations)
            {
                YAxisCurveData yData = GetYAxisDataByLineId(yLocations.Key);
                if (yData == null)
                {
                    continue;
                }
                int lineWidth = (int)yData.LineWidth;
                if (yLocations.Key == m_selectedLineIndex)
                {
                    lineWidth += lineWidth;
                }
                lstPoints.Clear();
                int pointCount = m_lstXLocations.Count;
                for (int i = 0; i < pointCount; i++)
                {
                    double xLocation = m_lstXLocations[i];
                    double yLocation = yLocations.Value[i];
                    if (yLocation == double.NaN)
                    {
                        continue;
                    }
                    point = new POINT((float)xLocation, (float)yLocation);
                    lstPoints.Add(point);
                }
                if (lstPoints.Count > 0)
                {
                    paint.DrawPolyline(yData.LineColor, lineWidth, LINE_STYLE_SOLID, lstPoints.ToArray());
                    if (yData.ShowCycle)
                    {
                        int lstPointsSize = lstPoints.Count;
                        for (int i = 0; i < lstPointsSize; i++)
                        {
                            POINT cPoint = lstPoints[i];
                            int cSize = 5;
                            paint.FillEllipse(yData.LineColor, new RECT(cPoint.x - cSize, cPoint.y - cSize, cPoint.x + cSize, cPoint.y + cSize));
                        }
                    } 
                    if (yData.Text != null && yData.Text.Length > 0)
                    {
                        paint.DrawText(yData.Text, ForeColor, Font,
                            new RECT(lstPoints[0].x, lstPoints[0].y, lstPoints[0].x + 100, lstPoints[0].y + 30));
                    }
                }
            }
        }

        /// <summary>
        /// ����X���Y���ֵ
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="curveData">��������</param>
        /// <param name="drawPoint">��ͼ��������</param>
        /// <param name="drawSize">��ͼ�����С</param>
        private void DrawXAndYStats(CPaint paint, FundCurveData data, POINT drawPoint, SIZE drawSize)
        {
            if (!IsMatchCurveLocation(m_mouseMovePoint.y, drawPoint, drawSize))
            {
                return;
            }
            if (m_mouseMovePoint.x < drawPoint.x
                || m_mouseMovePoint.x > drawPoint.x + drawSize.cx)
            {
                return;
            }
            // ��������
            float xLocation = m_mouseMovePoint.x;
            float yLocation = m_mouseMovePoint.y;
            if (m_isShowPointDescription && m_isMouseOperate)
            {
                String yText = "";
                String xText = "";
                // ���˵��
                if (IsMatchCurvePoint(m_mouseMovePoint, ref yText, ref xText))
                {
                    String showText = xText + "\r\n" + yText;
                    SIZE ySizeFShow = paint.TextSize(showText, m_pointDescFont);
                    float xShowLocation = xLocation + 10;
                    float yShowLocation = yLocation + 5;
                    if (xLocation + ySizeFShow.cx > drawPoint.x + drawSize.cx)
                    {
                        xShowLocation = xLocation - ySizeFShow.cx;
                    }
                    if (yLocation + ySizeFShow.cy > drawPoint.y + drawSize.cy)
                    {
                        yShowLocation = yLocation - ySizeFShow.cy;
                    }
                    RECT showRect = new RECT(xShowLocation, yShowLocation, ySizeFShow.cx, ySizeFShow.cy);
                    paint.DrawText(showText, m_pointDescColor, m_pointDescFont, showRect);
                }
            }
            // ���Ƶ�����
            // ��ȡ��ʾ��Xֵ
            String drawXStats = CalculateXAsixSlidersText(data, xLocation, drawPoint, drawSize);
            // ��ȡ��������Ĵ�С
            SIZE xSizeF = paint.TextSize(drawXStats, m_pointDescFont);
            float sliderWidth = DEFAULT_SLIDER_WIDTH;
            float sliderHeight = DEFAULT_SLIDER_HEIGHT;
            // X��
            if (xSizeF.cx > sliderWidth)
            {
                sliderWidth = xSizeF.cx;
            }
            if (xSizeF.cy > sliderHeight)
            {
                sliderHeight = xSizeF.cy;
            }
            paint.FillRect(m_lineColor, (int)xLocation, (int)drawPoint.y + drawSize.cy, (int)(xLocation + sliderWidth), (int)(drawPoint.y + drawSize.cy + sliderHeight));
            paint.DrawText(drawXStats, m_pointDescColor, m_pointDescFont, new RECT(xLocation, drawPoint.y + drawSize.cy + 1, xSizeF.cx, xSizeF.cy));
            // ��ȡ��ʾ��Yֵ
            List<YAxisStyle> lstYAxisStyles = GetYAxisStyles(yLocation, drawPoint,drawSize);
            if (lstYAxisStyles == null || lstYAxisStyles.Count == 0)
            {
                return;
            }
            // Y�����ֵ����Сֵ��Y��̶ȼ���߼�����
            double yAxisMaxValue = 0, yAxisMinValue = 0, dYStep = 0;
            // //Y���߼������ľ���(С����λ��),�̶ȵĸ���
            int nYDecidig = 0, scaleCount = 0, digits = 2;
            foreach (YAxisStyle yAxisStyle in lstYAxisStyles)
            {
                digits = 2;
                // ����Y��Ŀ̶���Ϣ
                CalculateYAxisScaleInfo(data.LstYAxisPointDatas, yAxisStyle, drawSize.cy, scaleCount, ref yAxisMaxValue, ref yAxisMinValue, ref dYStep, ref nYDecidig);
                if (nYDecidig > digits)
                {
                    digits = nYDecidig;
                }
                // ��ȡY���ϻ��Ƶ�Ŀ̶�
                scaleCount = (int)((yAxisMaxValue - yAxisMinValue) / dYStep);
                double yValue = (drawPoint.y + drawSize.cy - yLocation) / (drawSize.cy) * (yAxisMaxValue - yAxisMinValue) + yAxisMinValue;
                String drawYStats = Math.Round(yValue, digits).ToString() + yAxisStyle.YAxisScaleUnit;
                SIZE ySizeF = paint.TextSize(drawYStats, m_pointDescFont);
                // Y��
                sliderWidth = DEFAULT_SLIDER_WIDTH;
                sliderHeight = DEFAULT_SLIDER_HEIGHT;
                if (ySizeF.cx > sliderWidth)
                {
                    sliderWidth = ySizeF.cx;
                }
                if (ySizeF.cy > sliderHeight)
                {
                    sliderHeight = ySizeF.cy;
                }
                // ����
                if (yAxisStyle.YAxisDirection == 0)
                {
                    paint.FillRect(m_lineColor, (int)(drawPoint.x - sliderWidth - 2), (int)yLocation, (int)(drawPoint.x), (int)(yLocation + sliderHeight));
                    paint.DrawText(drawYStats, m_pointDescColor, m_pointDescFont, new RECT(drawPoint.x - sliderWidth - 2, yLocation, ySizeF.cx, ySizeF.cy));
                }
                // ����
                else if (yAxisStyle.YAxisDirection == YAxisDirection.Right)
                {
                    paint.FillRect(m_lineColor, (int)(drawPoint.x + drawSize.cx + 2), (int)yLocation, (int)(drawPoint.x + drawSize.cx + sliderWidth), (int)(yLocation + sliderHeight));
                    paint.DrawText(drawYStats, m_pointDescColor, m_pointDescFont, new RECT(drawPoint.x + drawSize.cx + 2, yLocation, ySizeF.cx, ySizeF.cy));
                }
            }
        }

        /// <summary>
        /// ����X��
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="curveData">��������</param>
        /// <param name="drawPoint">��ͼ��������</param>
        /// <param name="drawSize">��ͼ�����С</param>
        private void DrawXAsix(CPaint paint, FundCurveData curveData, POINT drawPoint, SIZE drawSize)
        {
            XAxisStyle xAxisStyle = curveData.XAxisStyle;
            // ����X��ı���
            if (!String.IsNullOrEmpty(xAxisStyle.XAxisTitle))
            {
                SIZE xSizeF = paint.TextSize(xAxisStyle.XAxisTitle, m_axisTitleFont);
                // ��ȡX�����ʼX����
                float titleXLocation = 0;
                if (xAxisStyle.XAxisTitleAlign == XAxisTitleAlign.Left)
                {
                    titleXLocation = (float)drawPoint.x;
                }
                else if (xAxisStyle.XAxisTitleAlign == XAxisTitleAlign.Center)
                {
                    titleXLocation = (float)(drawPoint.x + drawSize.cx / 2) - xSizeF.cx / 2;
                }
                else if (xAxisStyle.XAxisTitleAlign == XAxisTitleAlign.Right)
                {
                    titleXLocation = (float)(drawPoint.x + drawSize.cx) - xSizeF.cx;
                }
                float titleYLocation = drawPoint.y + drawSize.cy + DEFAULT_SCALE_LENGTH + m_curveMargin;
                RECT rect = new RECT(titleXLocation, titleYLocation, titleXLocation + xSizeF.cx, titleYLocation + xSizeF.cy);
                paint.DrawText(xAxisStyle.XAxisTitle, m_titleColor, m_axisTitleFont, rect);
            }
            m_lstXAxisScaleDrawInfos.Clear();
            m_lstXLocations.Clear();
            m_lstXScalesText.Clear();
            m_lstXValueText.Clear();
            // ����X��̶���Ϣ
            CalculateXAxisScale(curveData, drawPoint, drawSize);
            // ����X��Ŀ̶�
            DrawXAxisScale(paint, curveData, drawPoint, drawSize);
        }

        /// <summary>
        /// ����X��Ŀ̶���Ϣ
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="curveData">��������</param>
        /// <param name="drawPoint">��ͼ��������</param>
        /// <param name="drawSize">��ͼ�����С</param>
        private void DrawXAxisScale(CPaint paint, FundCurveData curveData, POINT drawPoint, SIZE drawSize)
        {
            float yLocation = drawPoint.y + drawSize.cy;
            int minValueXLocation = drawPoint.x;
            XAxisStyle xAxisStyle = curveData.XAxisStyle;
            List<XAxisPointData> lstXAxisPointDatas = curveData.LstXAxisPointDatas;
            if (xAxisStyle.XAxisType == XAxisType.Number
                || xAxisStyle.XAxisType == XAxisType.Text)
            {
                for (int i = 0; i < m_lstXScalesText.Count; i++)
                {
                    double xLocation = m_lstXLocations[i];
                    SIZE xSizeF = paint.TextSize(m_lstXScalesText[i], m_scaleTitleFont);
                    RECT rect = new RECT((float)xLocation - xSizeF.cx / 2, yLocation + DEFAULT_SCALE_LENGTH, xSizeF.cx, xSizeF.cy);
                    // ��С����
                    paint.DrawLine(m_lineColor, LINE_WIDTH_1, LINE_STYLE_SOLID, (int)xLocation, (int)yLocation, (int)xLocation, (int)(yLocation + DEFAULT_SCALE_LENGTH));
                    // ����X�����Ŀ̶�ֵ
                    paint.DrawText(m_lstXScalesText[i], m_titleColor, m_scaleTitleFont, rect);
                }
            }
            else if (xAxisStyle.XAxisType == XAxisType.Date)
            {
                int drawCount = 0;
                double perSeparater = (double)drawSize.cx / (m_currentPointCountInCurve - 1);
                for (int i = 0; i < m_lstXAxisScaleDrawInfos.Count; i++)
                {
                    XAxisScaleDrawInfo drawInfo = m_lstXAxisScaleDrawInfos[i];
                    double xLocation = drawCount * perSeparater + minValueXLocation;
                    SIZE xSizeF = paint.TextSize(m_lstXScalesText[i], m_scaleTitleFont);
                    RECT rect = new RECT((float)xLocation - xSizeF.cx / 2, yLocation + DEFAULT_SCALE_LENGTH, xSizeF.cx, xSizeF.cy);
                    // ��С����
                    paint.DrawLine(m_lineColor, LINE_WIDTH_1, LINE_STYLE_SOLID, (int)xLocation, (int)yLocation, (int)xLocation, (int)(yLocation + DEFAULT_SCALE_LENGTH));
                    if (drawInfo.IsYear)
                    {
                        // ����X�����Ŀ̶�ֵ
                        paint.DrawText(m_lstXScalesText[i], m_scaleYearColor, m_scaleTitleFont, rect);
                    }
                    else if (drawInfo.IsMonth)
                    {
                        // ����X�����Ŀ̶�ֵ
                        paint.DrawText(m_lstXScalesText[i], m_scaleMonthColor, m_scaleTitleFont, rect);
                    }
                    else
                    {
                        // ����X�����Ŀ̶�ֵ
                        paint.DrawText(m_lstXScalesText[i], m_titleColor, m_scaleTitleFont, rect);
                    }
                    drawCount += drawInfo.Capacity;
                }
            }
        }

        /// <summary>
        /// ����Y��
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="curveData">��������</param>
        /// <param name="drawPoint">��ͼ��������</param>
        /// <param name="drawSize">��ͼ�����С</param>
        private void DrawYAsix(CPaint paint, FundCurveData data, POINT drawPoint, SIZE drawSize)
        {
            m_dicYLocations.Clear();
            List<YAxisCurveData> lstYAxisData = new List<YAxisCurveData>();
            // Y�����ֵ����Сֵ��Y��̶ȼ���߼�����
            double yAxisMaxValue = 0, yAxisMinValue = 0, dYStep = 0;
            // //Y���߼������ľ���(С����λ��),�̶ȵĸ���
            int nYDecidig = 0, scaleCount = 0;
            // ����Y��̶ȼ�����ÿ��������ڸ���Y��������
            for (int i = 0; i < m_lstYAxisStyles.Count; i++)
            {
                YAxisStyle yAxisStyle = m_lstYAxisStyles[i];
                // ����Y��Ŀ̶���Ϣ
                CalculateYAxisScaleInfo(data.LstYAxisPointDatas, yAxisStyle, drawSize.cy, scaleCount, ref yAxisMaxValue, ref yAxisMinValue, ref dYStep, ref nYDecidig);
                // ��ȡY���ϻ��Ƶ�Ŀ̶�
                scaleCount = (int)((yAxisMaxValue - yAxisMinValue) / dYStep);
                double perSeparater = drawSize.cy / (yAxisMaxValue - yAxisMinValue);
                double yStartLocation = 0, yStartValue = 0, xStartLocation = 0,xEndLocation = 0;
                if (yAxisStyle.YAxisDirection == YAxisDirection.Left)
                {
                    xStartLocation = drawPoint.x;
                    xEndLocation = xStartLocation - DEFAULT_SCALE_LENGTH;
                }
                else
                {
                    xStartLocation = drawPoint.x + drawSize.cx;
                    xEndLocation = xStartLocation + DEFAULT_SCALE_LENGTH;
                }
                float maxYSValueLength = 0;
                String strYValue = "";
                // ��ʽ���ַ�������֤Y��̶���ֵС����λ����ͳһ
                String formatString = "0";
                if (nYDecidig > 0)
                {
                    formatString += ".";
                    for (int n = 0; n < nYDecidig; n++)
                    {
                        formatString += "0";
                    }
                }
                for (int j = 0; j <= scaleCount; j++)
                {
                    yStartValue = yAxisMinValue + j * dYStep;
                    yStartLocation = drawSize.cy + drawPoint.y - (yStartValue - yAxisMinValue) * perSeparater;
                    strYValue = yStartValue.ToString(formatString);
                    strYValue += yAxisStyle.YAxisScaleUnit;
                    SIZE ySizeF = paint.TextSize(strYValue, m_scaleTitleFont);
                    //��Y��Ŀ̶�
                    paint.DrawLine(m_lineColor, LINE_WIDTH_1, LINE_STYLE_SOLID, (int)xStartLocation, (int)yStartLocation, (int)xEndLocation, (int)yStartLocation);
                    if (j > 0 && j < scaleCount)
                    {
                        // ���ƺ�������
                        paint.DrawLine(m_lineColor, LINE_WIDTH_1, LINE_STYLE_DASH, (int)drawPoint.x, (int)yStartLocation, (int)drawPoint.x + drawSize.cx, (int)yStartLocation);
                    }
                    RECT rect = new RECT((float)drawPoint.x - DEFAULT_SCALE_LENGTH - ySizeF.cx, (float)yStartLocation - ySizeF.cy / 2
                        , (float)drawPoint.x - DEFAULT_SCALE_LENGTH, (float)yStartLocation + ySizeF.cy / 2);
                    if (yAxisStyle.YAxisDirection == YAxisDirection.Right)
                    {
                        rect = new RECT((float)xEndLocation, (float)yStartLocation - ySizeF.cy / 2, (float)xEndLocation + ySizeF.cx, (float)yStartLocation + ySizeF.cy / 2);
                    }
                    if (ySizeF.cx > maxYSValueLength)
                    {
                        maxYSValueLength = ySizeF.cx;
                    }
                    // ��¼�̶�
                    paint.DrawText(strYValue, m_titleColor, m_scaleTitleFont, rect);
                }
                // ����Y�����
                if (!String.IsNullOrEmpty(yAxisStyle.YAxisTitle))
                {
                    char[] charArray = yAxisStyle.YAxisTitle.ToCharArray();
                    SIZE titleSizeF;
                    RECT rect;
                    float titleSXLocation = 0;
                    float titleSYLocation = 0;
                    titleSizeF = paint.TextSize(yAxisStyle.YAxisTitle, m_axisTitleFont);
                    // ��ȡY��������ʼX����
                    if (yAxisStyle.YAxisDirection == YAxisDirection.Left)
                    {
                        titleSXLocation = (float)(drawPoint.x) - DEFAULT_SCALE_LENGTH - maxYSValueLength;
                    }
                    else if (yAxisStyle.YAxisDirection == YAxisDirection.Right)
                    {
                        titleSXLocation = (float)(drawPoint.x + drawSize.cx) + DEFAULT_SCALE_LENGTH + maxYSValueLength;
                    }
                    // ��ȡY��������ʼY����
                    if (yAxisStyle.YAxisTitleAlign == YAxisTitleAlign.Top)
                    {
                        titleSYLocation = (float)drawPoint.y;
                    }
                    else if (yAxisStyle.YAxisTitleAlign == YAxisTitleAlign.Middle)
                    {
                        titleSYLocation = (float)(drawPoint.y + drawPoint.y + drawSize.cy) / 2 - (titleSizeF.cy * charArray.Length) / 2;
                    }
                    else if (yAxisStyle.YAxisTitleAlign == YAxisTitleAlign.Bottom)
                    {
                        titleSYLocation = (float)drawPoint.y + drawSize.cy - (titleSizeF.cy * (charArray.Length));
                    }
                    float maxYTitleWidth = 0;
                    for (int charIndex = 0; charIndex < charArray.Length; charIndex++)
                    {
                        titleSizeF = paint.TextSize(charArray[charIndex].ToString(), m_axisTitleFont);
                        if (maxYTitleWidth < titleSizeF.cx)
                            maxYTitleWidth = titleSizeF.cx;
                    }
                    for (int charIndex = 0; charIndex < charArray.Length; charIndex++)
                    {
                        titleSizeF = paint.TextSize(charArray[charIndex].ToString(), m_axisTitleFont);
                        if (yAxisStyle.YAxisDirection == YAxisDirection.Left)
                        {
                            rect = new RECT(titleSXLocation - maxYTitleWidth, titleSYLocation, titleSXLocation, titleSYLocation + titleSizeF.cy);
                        }
                        else
                        {
                            rect = new RECT(titleSXLocation, titleSYLocation, titleSXLocation + titleSizeF.cx, titleSYLocation + titleSizeF.cy);
                        }
                        paint.DrawText(charArray[charIndex].ToString(), m_titleColor, m_axisTitleFont, rect);
                        titleSYLocation += titleSizeF.cy + 2;
                    }
                }
                lstYAxisData.Clear();
                foreach (YAxisCurveData ydata in data.LstYAxisPointDatas)
                {
                    if (ydata.AttachYAxisDirection == yAxisStyle.YAxisDirection)
                    {
                        lstYAxisData.Add(ydata);
                    }
                }
                for (int n = 0; n < lstYAxisData.Count; n++)
                {
                    List<double> lstYLocations = new List<double>();
                    List<double> lstYValues = new List<double>();
                    double yLocation;
                    for (int j = m_startPointIndex; j <= m_endPointIndex; j++)
                    {
                        double yValue = lstYAxisData[n].YPointValues[j];
                        if (yValue == double.NaN)
                        {
                            lstYLocations.Add(double.NaN);
                            continue;
                        }
                        yLocation = (yValue - yAxisMinValue) * perSeparater;
                        yLocation = (double)drawSize.cy + drawPoint.y - yLocation;
                        lstYLocations.Add(yLocation);
                        lstYValues.Add(yValue);
                    }
                    int index = i * 100 + n;
                    lstYAxisData[n].LineId = index;
                    m_dicYLocations.Add(index, lstYLocations);
                }
            }
        }

        /// <summary>
        /// ��ȡ��������Ĵ�С
        /// </summary>
        /// <param name="drawRect">��ͼ����</param>
        /// <param name="drawPoint">��ͼ���������</param>
        /// <param name="drawSize">��ͼ����Ĵ�С</param>
        /// <returns></returns>
        private int GetDrawRect(RECT drawRect, ref  POINT drawPoint, ref SIZE drawSize)
        {
            POINT location = new POINT(drawRect.left, drawRect.top);
            SIZE size;
            if (drawRect.bottom > drawRect.top)
            {
                size = new SIZE(drawRect.right - drawRect.left, drawRect.bottom - drawRect.top);
            }
            else
            {
                size = new SIZE(drawRect.right - drawRect.left, drawRect.top - drawRect.bottom);
            }
            drawPoint = new POINT(location.x + m_curveMargin + m_yAsixLeftTitleWidth, location.y + m_curveMargin + m_curveTitleHeight);
            drawSize = new SIZE(size.cx - m_curveMargin * 2 - m_yAsixLeftTitleWidth - m_yAsixRightTitleWidth, size.cy - m_curveTitleHeight - m_xAsixTitleHeight - m_curveMargin * 2);
            return 1;
        }

        /// <summary>
        /// ��������ID��ȡ���ߵ�������Ϣ
        /// </summary>
        /// <param name="lineId">�������������������ID��Ψһ��</param>
        /// <returns></returns>
        private YAxisCurveData GetYAxisDataByLineId(int lineId)
        {
            foreach (YAxisCurveData yData in m_fundCurveData.LstYAxisPointDatas)
            {
                if (yData.LineId == lineId)
                {
                    return yData;
                }
            }
            return null;
        }

        /// <summary>
        /// ��������ID��ȡ���ߵ���������Ϣ
        /// </summary>
        /// <param name="lineId">�������������������ID��Ψһ��</param>
        /// <param name="yData">��������</param>
        /// <returns></returns>
        private YAxisStyle GetYAxisStyleByLineId(int lineId, YAxisCurveData yData)
        {
            foreach (YAxisStyle yAxisStyle in m_lstYAxisStyles)
            {
                if (yAxisStyle.YAxisDirection == yData.AttachYAxisDirection)
                {
                    return yAxisStyle;
                }
            }
            return null;
        }

        /// <summary>
        /// ����������ڵ�Y�����ȡ�������ߵ�Y����ϵ
        /// </summary>
        /// <param name="mouseYLocation">���Y������</param>
        /// <param name="drawPoint">��ͼ��������</param>
        /// <param name="drawSize">��ͼ�����С</param>
        /// <returns></returns>
        private List<YAxisStyle> GetYAxisStyles(float mouseYLocation, POINT drawPoint, SIZE drawSize)
        {
            if (mouseYLocation <= drawPoint.y + drawSize.cy
                && mouseYLocation >= drawPoint.y)
            {
                return m_lstYAxisStyles;
            }
            return null;
        }

        /// <summary>
        /// ����ƶ��ĵ��Y�����Ƿ��ڻ�ͼ������
        /// </summary>
        /// <param name="mouseYLocation">���Y������</param>
        /// <param name="drawPoint">��ͼ��������</param>
        /// <param name="drawSize">��ͼ�����С</param>
        /// <returns></returns>
        private bool IsMatchCurveLocation(float mouseYLocation, POINT drawPoint, SIZE drawSize)
        {
            if (mouseYLocation <= drawPoint.y + drawSize.cy
                && mouseYLocation >= drawPoint.y)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// �Ƿ�ƥ�������ϵĵ�
        /// </summary>
        /// <param name="mouseLoacation"></param>
        /// <param name="yText"></param>
        /// <param name="xText"></param>
        /// <returns></returns>
        private bool IsMatchCurvePoint(POINTF mouseLoacation, ref String yText, ref String xText)
        {
            YAxisCurveData yData = null;
            YAxisStyle yStyle = null;
            double yValue = 0;
            float diameter = DEFAULT_POINT_RADIUS;
            // ��������
            float xMouseLocation = m_mouseMovePoint.x;
            float yMouseLocation = m_mouseMovePoint.y;
            foreach (KeyValuePair<int, List<double>> yLocations in m_dicYLocations)
            {
                yData = GetYAxisDataByLineId(yLocations.Key);
                if (yData == null)
                {
                    continue;
                }
                yStyle = GetYAxisStyleByLineId(yLocations.Key, yData);
                if (yStyle == null)
                {
                    continue;
                }
                for (int i = 0; i < m_lstXLocations.Count; i++)
                {
                    double xLocation = m_lstXLocations[i];
                    double yLocation = yLocations.Value[i];
                    if (xMouseLocation <= diameter + xLocation
                        && xMouseLocation >= xLocation - diameter
                        && yMouseLocation <= diameter + yLocation
                        && yMouseLocation >= yLocation - diameter)
                    {
                        yValue = Math.Round(yData.YPointValues[m_startPointIndex + i], 2);
                        yText = m_curvePointDescPrefix + ":" + yValue.ToString() + yStyle.YAxisScaleUnit;
                        xText = m_lstXValueText[i];
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// ת���·�
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private String MonthConvert(int date)
        {
            return ((date % 10000) / 100).ToString("D2");
        }

        /// <summary>
        /// ���̰����¼�
        /// </summary>
        /// <param name="key">��ֵ</param>
        public override void OnKeyDown(char key)
        {
            if (!m_isFinishedRedistributionData)
            {
                return;
            }
            switch ((int)key)
            {
                // Up
                case 38:
                    ZoomOut();
                    break;
                // DOWN
                case 40:
                    ZoomIn();
                    break;
                // Left
                case 37:
                    ScrollLeft();
                    break;
                //Right
                case 39:
                    ScrollRight();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// ������¼�
        /// </summary>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">����ֵ</param>
        public override void OnMouseDown(POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            if (button == MouseButtonsA.Left)
            {
                if (clicks == 1)
                {
                    m_mouseMovePoint = new POINTF(mp.x, mp.y);
                    m_selectedLineIndex = -1;
                    m_selectedLineIndex = CalculateSelectLineIndex(mp);
                    m_isMouseOperate = true;
                }
                else if (clicks == 2)
                {
                    m_isDrawCrossLine = !m_isDrawCrossLine;
                }
                Invalidate();
            }
        }

        /// <summary>
        /// ����ƶ��¼�
        /// </summary>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">����ֵ</param>
        public override void OnMouseMove(POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            m_mouseMovePoint = new POINTF(mp.x, mp.y);
            m_isMouseOperate = true;
            Invalidate();
        }

        /// <summary>
        /// �������¼�
        /// </summary>
        /// <param name="mp">����</param>
        /// <param name="button">��ť</param>
        /// <param name="clicks">�������</param>
        /// <param name="delta">����ֵ</param>
        public override void OnMouseWheel(POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            base.OnMouseWheel(mp, button, clicks, delta);
            if (delta > 0)
            {
                ZoomOut();
            }
            else if (delta < 0)
            {
                ZoomIn();
            }
        }

        /// <summary>
        /// ����ͼ��
        /// </summary>
        /// <param name="paint"></param>
        /// <param name="clipRect"></param>
        public override void OnPaint(CPaint paint, RECT clipRect)
        {
            OnPaintBackground(paint, clipRect);
            OnPaintForeground(paint, clipRect);
        }

        /// <summary>
        /// ���Ʊ���ɫ
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="clipRect">��ͼ����</param>
        public override void OnPaintBackground(CPaint paint, RECT clipRect)
        {
            paint.FillRect(COLOR.ARGB(m_fundCurveData.BackgroundColor.R, m_fundCurveData.BackgroundColor.G, m_fundCurveData.BackgroundColor.B), clipRect);
            paint.DrawRect(m_lineColor, 1, 1, clipRect);
        }

        /// <summary>
        /// ����ǰ��ɫ
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="clipRect">��ͼ����</param>
        public override void OnPaintForeground(CPaint paint, RECT clipRect)
        {
            POINT drawPoint = new POINT(0, 0);
            SIZE drawSize = new SIZE(0, 0);
            // ��ȡ��������
            GetDrawRect(clipRect, ref drawPoint, ref drawSize);
            drawSize.cy = Height - 100;
            drawPoint.y = 50;
            if (m_isFinishedRedistributionData && m_fundCurveData != null)
            {
                // ���ƻ�������ͼ���
                DrawFundCurveFrame(paint, m_fundCurveData, drawPoint, drawSize);
                // ����X��
                DrawXAsix(paint, m_fundCurveData, drawPoint, drawSize);
                // ����Y��
                DrawYAsix(paint, m_fundCurveData, drawPoint, drawSize);
                // ���Ƶ����
                DrawPointAndCurve(paint, m_fundCurveData, drawPoint, drawSize);
                // ����ʮ�ֹ��
                DrawCrossingLine(paint, drawPoint, drawSize);
                // ����X���Y����ʾ��ֵ
                DrawXAndYStats(paint, m_fundCurveData, drawPoint, drawSize);
            }
        }

        /// <summary>
        /// ���·�����������
        /// </summary>
        /// <param name="curveData"></param>
        private void RedistributionCurveData(FundCurveData curveData)
        {
            List<YAxisCurveData> lstYAxisPointDatas = curveData.LstYAxisPointDatas;
            List<YAxisCurveData> lstTmpYData = null;
            for (int i = 0; i < lstYAxisPointDatas.Count; i++)
            {
                YAxisCurveData yData = lstYAxisPointDatas[i];
                if (m_lstYAxisStyles == null || m_lstYAxisStyles.Count == 0)
                {
                    Console.WriteLine("������������Y�����겻���ڣ����Բ��ܻ��Ƹ����ݵ����ߡ�");
                    continue;
                }
                else
                {
                    bool isFind = false;
                    foreach (YAxisStyle yTmpStyle in m_lstYAxisStyles)
                    {
                        if (yTmpStyle.YAxisDirection == yData.AttachYAxisDirection)
                        {
                            isFind = true;
                            break;
                        }
                    }
                    if (!isFind)
                    {
                        Console.WriteLine("������������Y�����겻���ڣ����Բ��ܻ��Ƹ����ݵ����ߡ�");
                    }
                }
                if (lstTmpYData == null)
                {
                    lstTmpYData = new List<YAxisCurveData>();
                }
                lstTmpYData.Add(yData);
            }
        }

        /// <summary>
        /// ���·���Y��������
        /// </summary>
        /// <param name="curveData">��������</param>
        private void RedistributionYAxis(FundCurveData curveData)
        {
            m_lstYAxisStyles.Clear();
            bool isHasLeftYAxisTitle = false;
            bool isHasRightYAxisTitle = false;
            bool isHasLeftYAxis = false;
            bool isHasRightYAxis = false;
            List<YAxisStyle> lstYAxisStyle = curveData.LstYAxisStyle;
            List<YAxisCurveData> lstYAxisPointDatas = curveData.LstYAxisPointDatas;
            for (int i = 0; i < lstYAxisStyle.Count; i++)
            {
                YAxisStyle yStyle = lstYAxisStyle[i];
                bool isAdd = true;
                if (m_lstYAxisStyles.Count == 2)
                {
                    Console.WriteLine("���ֻ������2��Y�����꣬���Ҳ��һ����");
                    isAdd = false;
                    continue;
                }
                foreach (YAxisStyle yTmpStyle in m_lstYAxisStyles)
                {
                    if (yTmpStyle.YAxisDirection == yStyle.YAxisDirection)
                    {
                        Console.WriteLine("����ͬʱ������������һ����Y�����ֻ꣬�������Ҳ��һ����");
                        isAdd = false;
                        continue;
                    }
                }
                if (isAdd)
                {
                    m_lstYAxisStyles.Add(yStyle);
                    if (yStyle.YAxisDirection == YAxisDirection.Left)
                    {
                        // ���Y��
                        isHasLeftYAxis = true;
                        if (!String.IsNullOrEmpty(yStyle.YAxisTitle))
                        {
                            isHasLeftYAxisTitle = true;
                        }
                    }
                    else
                    {
                        // �Ҳ�Y��
                        isHasRightYAxis = true;
                        if (!String.IsNullOrEmpty(yStyle.YAxisTitle))
                        {
                            isHasRightYAxisTitle = true;
                        }
                    }
                }
            }
            // �������������Ŀ��
            if (isHasLeftYAxis && isHasLeftYAxisTitle)
            {
                m_yAsixLeftTitleWidth = m_yAsixTitleWidth;
            }
            else if (isHasLeftYAxis)
            {
                m_yAsixLeftTitleWidth = m_yAsixTitleWidth / 2;
            }
            // �����Ҳ�������Ŀ��
            if (isHasRightYAxis && isHasRightYAxisTitle)
            {
                m_yAsixRightTitleWidth = m_yAsixTitleWidth;
            }
            else if (isHasRightYAxis)
            {
                m_yAsixRightTitleWidth = m_yAsixTitleWidth / 2;
            }
        }

        /// <summary>
        /// ���·������ߺ�Y������������
        /// </summary>
        /// <param name="curveData">��������</param>
        private void RedistributionYAxisAndCurveData(FundCurveData curveData)
        {
            // ���·���Y��������
            RedistributionYAxis(curveData);
            // ���·�����������
            RedistributionCurveData(curveData);
        }

        /// <summary>
        /// �������
        /// </summary>
        public void ScrollLeft()
        {
            if (m_currentPointIndex <= 0)
            {
                return;
            }
            if (!m_isDrawCrossLine)
                m_currentPointIndex = m_startPointIndex;
            if (m_currentPointIndex <= 0)
                return;
            if (m_currentPointIndex == m_startPointIndex)
            {
                m_startPointIndex--;
                m_endPointIndex--;
            }
            m_currentPointIndex--;
            m_isMouseOperate = false;
            Invalidate();
        }

        /// <summary>
        /// ���ҹ���
        /// </summary>
        public void ScrollRight()
        {
            if (m_currentPointIndex == m_totalPointCount - 1)
            {
                return;
            }
            if (!m_isDrawCrossLine)
            {
                m_currentPointIndex = m_endPointIndex;
            }
            if (m_currentPointIndex == m_totalPointCount - 1)
            {
                return;
            }
            if (m_currentPointIndex == m_endPointIndex)
            {
                m_startPointIndex++;
                m_endPointIndex++;
            }
            m_currentPointIndex++;
            m_isMouseOperate = false;
            Invalidate();
        }

        /// <summary>
        /// ת�����
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private String YearConvert(int date)
        {
            return (date / 10000).ToString();
        }

        /// <summary>
        /// �Ŵ�
        /// </summary>
        public void ZoomIn()
        {
            if (m_startPointIndex == 0 && m_endPointIndex == m_totalPointCount - 1)
            {
                return;
            }
            int startIndex = m_startPointIndex - m_zoomPointCount;
            if (startIndex < 0)
            {
                int startTotalCount = m_zoomPointCount + startIndex;
                int endTotalCount = m_zoomPointCount - startTotalCount;
                int endIndex = m_endPointIndex + endTotalCount;
                if (endIndex > m_totalPointCount - 1)
                {
                    endTotalCount = endTotalCount - (endIndex - m_totalPointCount + 1);
                    m_endPointIndex += endTotalCount;
                }
                else
                {
                    m_endPointIndex += endTotalCount;
                }
                m_currentPointCountInCurve += startTotalCount + endTotalCount;
                m_startPointIndex = 0;
            }
            else
            {
                m_startPointIndex -= m_zoomPointCount;
                m_currentPointCountInCurve += m_zoomPointCount;
            }
            System.Console.WriteLine(m_currentPointCountInCurve == m_endPointIndex - m_startPointIndex + 1);
            m_isMouseOperate = false;
            Invalidate();
        }

        /// <summary>
        /// ��С
        /// </summary>
        public void ZoomOut()
        {
            if (m_currentPointCountInCurve == MIN_POINT_COUNT_IN_CURVE
    || m_currentPointCountInCurve == m_totalPointCount)
            {
                return;
            }
            int totalPointCount = m_currentPointCountInCurve - m_zoomPointCount;
            if (totalPointCount < MIN_POINT_COUNT_IN_CURVE)
            {
                m_startPointIndex += m_currentPointCountInCurve - MIN_POINT_COUNT_IN_CURVE;
                m_currentPointCountInCurve = MIN_POINT_COUNT_IN_CURVE;
            }
            else
            {
                m_startPointIndex += m_zoomPointCount;
                m_currentPointCountInCurve = totalPointCount;
            }
            m_isMouseOperate = false;
            if (m_currentPointIndex < m_startPointIndex)
                m_currentPointIndex = m_startPointIndex;
            Invalidate();
        }
    }
}
