using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace OwLib
{
    /// <summary>
    /// �����ļ�������
    /// </summary>
    public class FieldInfoCfgHelper
    {
        /// <summary>
        /// ���ݵ�ǰ��������͡�(ExportType)���ԺͶ�Ӧ��FieldIndex��ö�Ӧ���������
        /// �߼��������������͡�(ExportType)����û��/Ϊ�գ���ȡ���Ӧ�ĵ�FieldIndex������
        /// </summary>
        /// <param name="ExportTypeStr">��������͡�(ExportType)�����ַ���</param>
        /// <param name="fieldIndex">���Ӧ�ĵ�FieldIndex</param>
        /// <returns></returns>
        public static Type GetExportType(string ExportTypeStr, FieldIndex fieldIndex)
        {
            if (string.IsNullOrEmpty(ExportTypeStr))
            {
                short fieldIndexSeq = (short)fieldIndex;

                if (fieldIndexSeq < 300)
                    return typeof(Int32);

                if (fieldIndexSeq < 800)
                    return typeof(Single);

                if (fieldIndexSeq < 1000)
                    return typeof(Double);

                if (fieldIndexSeq < 1200)
                    return typeof(Int64);

                if (fieldIndexSeq < 9000)
                    return typeof(String);

                return typeof(Object);
            }
            else
            {
                Type type = typeof(Object);
                switch (ExportTypeStr)
                {
                    case "Int32":
                        type = typeof(Int32);
                        break;

                    case "Single":
                        type = typeof(Single);
                        break;

                    case "Double":
                        type = typeof(Double);
                        break;

                    case "Int64":
                        type = typeof(Int64);
                        break;

                    case "String":
                        type = typeof(String);
                        break;

                    default:
                        type = typeof(Object);
                        break;
                }

                return type;
            }

        }
    }

    /// <summary>
    /// �����ʽ
    /// </summary>
    public class Format
    {
        /// <summary>
        /// �������
        /// </summary>
        public ZeroRule ZeroRule;

        /// <summary>
        /// �������
        /// </summary>
        public List<CalculateRule> CalculateRules;

        /// <summary>
        /// ���վ��ȱ���С������
        /// </summary>
        public NRule NRule;
        /// <summary>
        /// ǰ׺����
        /// </summary>
        public PRule PRule;
        /// <summary>
        /// ��׺����
        /// </summary>
        public QRule QRule;
        /// <summary>
        /// �������
        /// </summary>
        public SRule SRule;
    }

    /// <summary>
    /// �����ֶ�������Ϣ
    /// </summary>
    public class FieldInfo
    {
        /// <summary>
        /// �����ֶζ�Ӧ�Ĵ洢���ֶ�
        /// </summary>
        public FieldIndex FieldIndex;
        /// <summary>
        /// �����ֶζ�Ӧ����ɫ������Ϣ
        /// </summary>
        public List<string> ColorSetting;

        /// <summary>
        /// �����ʽ
        /// </summary>
        public Format Format;
        /// <summary>
        /// ��ѡ�ֶΣ���ʾ�����������ʽ�����û������ֶΣ�ȡFormat�ֶεĸ�ʽ
        /// </summary>
        public Format ExportFormat;
        /// <summary>
        /// ��ѡ�ֶΣ���ʾ���������ݵ����ͣ����û������ֶΣ���ȡName��Ӧ��FieldIndex�����ͣ�Ĭ����String��
        /// </summary>
        public Type ExportType;
    }
    /// <summary>
    /// �������ıȽ�����
    /// </summary>
    public enum CompareTarget
    {
        /// <summary>
        /// �õ�ǰ֤ȯ�����ĳ�������ֶ�������
        /// </summary>
        SpecialFieldIndex,
        /// <summary>
        /// �õ�ǰ֤ȯ�����ָ��ֵ����
        /// </summary>
        This
    }

    /// <summary>
    /// ���������
    /// </summary>
    public enum ZeroTypeFlag
    {
        /// <summary>
        /// �ж�Ϊ��Ļ�ָ�귵�غ��ߣ�"��"
        /// </summary>
        H,
        /// <summary>
        /// �ж�Ϊ��Ļ�ָ�귵��string.Empty
        /// </summary>
        E
    }

    /// <summary>
    /// ��ʽ����1���ж�Ϊ�㣬����ִ��
    /// </summary>
    public class ZeroRule
    {
        /// <summary>
        /// �����ַ���
        /// </summary>
        public static string HorStr = "��";
        /// <summary>
        /// �������Ĺ��캯��
        /// </summary>
        /// <param name="compareTarget">�Ƚ�����</param>
        /// <param name="comparedIndex"></param>
        /// <param name="zeroTypeFlag"></param>
        public ZeroRule(CompareTarget compareTarget, FieldIndex comparedIndex, ZeroTypeFlag zeroTypeFlag)
        {
            this.CompareTarget = compareTarget;
            this.ZeroTypeFlag = zeroTypeFlag;
            this.ComparedIndex = comparedIndex;
        }
        /// <summary>
        /// �Ƚ�����
        /// </summary>
        public CompareTarget CompareTarget;
        /// <summary>
        /// ��Ϊ��ı�ʶ������������string.Empty
        /// </summary>
        public ZeroTypeFlag ZeroTypeFlag;

        /// <summary>
        /// ����ıȽ��ֶ�
        /// </summary>
        public FieldIndex ComparedIndex;

        /// <summary>
        /// ����String.Empty���Ǻ���
        /// </summary>
        public string ZeroStr { get { return ZeroTypeFlag == ZeroTypeFlag.E ? string.Empty : HorStr; } }


        /// <summary>
        /// �ж��Ƿ�Ϊ��
        /// </summary>
        /// <param name="originalData">ԭʼֵ</param>
        /// <param name="comparedIndexValue">����ıȽ��ֶε�ֵ</param>
        /// <returns></returns>
        public bool IsZero(string originalData, float comparedIndexValue)
        {
            if (CompareTarget == CompareTarget.SpecialFieldIndex)
                return comparedIndexValue >= -float.Epsilon
                    && comparedIndexValue <= float.Epsilon;

            double value;
            if (double.TryParse(originalData, out value))
                return value >= -double.Epsilon
                   && value <= double.Epsilon;

            long lValue;
            if (long.TryParse(originalData, out lValue))
            {
                return lValue < 0;
            }

            return false;
        }
    }

    /// <summary>
    /// �������
    /// </summary>
    public enum CalMethod
    {
        /// <summary>
        /// �ӷ�
        /// </summary>
        A,
        /// <summary>
        /// ����
        /// </summary>
        C,
        /// <summary>
        /// �˷�
        /// </summary>
        M,
        /// <summary>
        /// ����
        /// </summary>
        D
    }
    /// <summary>
    /// ��ʽ����2������
    /// </summary>
    public class CalculateRule
    {
        /// <summary>
        /// �������Ĺ��캯��
        /// </summary>
        /// <param name="calMethod"></param>
        /// <param name="zoomNum"></param>
        /// <param name="numFormat"></param>
        public CalculateRule(CalMethod calMethod, float zoomNum, string numFormat)
        {
            this.CalMethod = calMethod;
            this.ZoomNum = zoomNum;
            this.NumFormat = numFormat;
        }

        /// <summary>
        /// ���㷽ʽ����/��/��/����
        /// </summary>
        public CalMethod CalMethod;
        /// <summary>
        /// ����/��/��/����������
        /// </summary>
        public float ZoomNum;
        /// <summary>
        /// ����С��λ(eg. "F2")
        /// </summary>
        public string NumFormat;
        /// <summary>
        /// ��ü����Ľ��
        /// </summary>
        /// <param name="originalData"></param>
        /// <returns></returns>
        public string GetCalculatedString(string originalData)
        {
            string result = string.Empty;
            double tempValue;

            if (double.TryParse(originalData, out tempValue))
            {
                switch (CalMethod)
                {
                    case CalMethod.A:
                        tempValue += ZoomNum;
                        break;

                    case CalMethod.C:
                        tempValue -= ZoomNum;
                        break;

                    case CalMethod.D:
                        if (ZoomNum != 0)
                        { tempValue /= ZoomNum; }
                        break;

                    case CalMethod.M:
                        tempValue *= ZoomNum;
                        break;

                }

                result = tempValue.ToString(NumFormat);
            }



            return result;
        }
    }
    /// <summary>
    ///  N��ͷ��ʾ��ԭʼ���ݣ����������
    /// </summary>
    public class NRule
    {
        /// <summary>
        /// NRule�Ĺ��캯��
        /// </summary>
        /// <param name="format"></param>
        public NRule(string format)
        { this.FormatStr = format; }

        /// <summary>
        /// ���ȱ�����ʽ
        /// </summary>
        public string FormatStr;

        /// <summary>
        /// ��þ��ȱ�������ַ���
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public string GetNFormatString(string strValue)
        {
            double tempValue;
            if (double.TryParse(strValue, out tempValue))
            {
                return tempValue.ToString(FormatStr);
            }

            long tempLValue;
            if (long.TryParse(strValue, out tempLValue))
            {
                return tempLValue.ToString(FormatStr);
            }

            return strValue;
        }
    }
    /// <summary>
    ///  P��ͷ��ʾǰ׺����0�Ƚϣ�����0�ļ�ǰ׺"+",������P:A�����ʾ���ַ���ǰ׺"A"
    /// </summary>
    public class PRule
    {
        /// <summary>
        /// PRule
        /// </summary>
        /// <param name="profix"></param>
        public PRule(string profix)
        { this.Profix = profix; }
        /// <summary>
        /// ǰ׺����
        /// </summary>
        public string Profix;

        /// <summary>
        /// �������ǰ׺�����ĸ�ʽ�ַ���
        /// </summary>
        /// <param name="originalData"></param>
        /// <returns></returns>
        public string GetProfixedString(string originalData)
        {
            string result = originalData;

            double value;
            if (Profix.Equals("$0"))
            {
                if (double.TryParse(originalData, out value))
                {
                    if (value > 0)
                        result = string.Format("+{0}", originalData);
                }
            }
            else
            {
                result = string.Format("{0}{1}", Profix, originalData);
            }

            return result;
        }
    }
    /// <summary>
    /// Q��ͷ��ʾ��׺�����ַ�����BP����Ϊ�����׺
    /// </summary>
    public class QRule
    {
        /// <summary>
        /// ��׺����
        /// </summary>
        /// <param name="suffix"></param>
        public QRule(string suffix)
        { this.Suffix = suffix; }
        /// <summary>
        /// ��׺
        /// </summary>
        public string Suffix;

        /// <summary>
        /// ������к�׺�����ĸ�ʽ�ַ���
        /// </summary>
        /// <param name="originalData"></param>
        /// <returns></returns>
        public string GetSuffixedString(string originalData)
        {

            return string.Format("{0}{1}", originalData, Suffix);
        }
    }
    /// <summary>
    ///  S��ͷ��ʾ�����ʽ����S:Date��ʾDate��ʽ
    /// </summary>
    public class SRule
    {
        /// <summary>
        /// ��������캯��
        /// </summary>
        /// <param name="specialContent"></param>
        public SRule(string specialContent)
        { this.SpecialContent = specialContent; }
        /// <summary>
        /// �����������20
        /// </summary>
        public string SpecialContent;

        /// <summary>
        /// ������к�׺�����ĸ�ʽ�ַ���
        /// </summary>
        /// <param name="originalData"></param>
        /// <returns></returns>
        public string GetSpecialedString(string originalData)
        {
            string result = string.Empty;
            int data;
            double dData;
            long lData;

            switch (SpecialContent)
            {
                case "Date":
                    if (int.TryParse(originalData, out data))
                    {
                        int day = (int)(data % 100);
                        int month = (int)((data - day) % 10000) / 100;
                        int year = (data - day - month * 100) / 10000;
                        result = year.ToString("D4") + "-" + month.ToString("D2") + "-" + day.ToString("D2");
                    }
                    break;

                case "Time":
                    if (int.TryParse(originalData, out data))
                    {
                        int sec = (int)(data % 100);
                        int min = (int)((data - sec) % 10000) / 100;
                        int hour = (data - sec - min * 100) / 10000;
                        result = hour.ToString("D2") + ":" + min.ToString("D2") + ":" + sec.ToString("D2");
                    }
                    break;

                case "Volume":
                    if (double.TryParse(originalData, out dData))
                    {
                        if ((dData < 10000 && dData > 0) || (dData > -10000 && dData < 0))
                            return string.Format("{0:F0}", dData);

                        dData /= 10000;
                        if (dData >= 1000000 || dData <= -1000000) //100��
                        {
                            dData /= 10000;
                            result = string.Format("{0:F0}��", dData);
                        }
                        else if (dData >= 10000 || dData <= -10000) //99.99��
                        {
                            dData /= 10000;
                            result = string.Format("{0:F2}��", dData);

                        }
                        else if (dData >= 100 || dData <= -100) //100~9999��
                        {
                            result = string.Format("{0:F0}��", dData);
                        }
                        else if (dData >= 1 || dData <= -1) //99.99��
                        {
                            result = string.Format("{0:F2}��", dData);
                        }
                        else
                            result = string.Format("{0:F0}", dData);
                    }
                    else if (long.TryParse(originalData, out lData))
                    {
                        if ((lData < 10000 && lData > 0) || (lData > -10000 && lData < 0))
                            return string.Format("{0:F0}", lData);
                        lData /= 10000;
                        if (lData >= 1000000 || lData <= -1000000) //100��
                        {
                            lData /= 10000;
                            result = string.Format("{0:F0}��", lData);
                        }
                        else if (lData >= 10000 || lData <= -10000) //99.99��
                        {
                            lData /= 10000;
                            result = string.Format("{0:F2}��", lData);

                        }
                        else if (lData >= 100 || lData <= -100) //100~9999��
                        {
                            result = string.Format("{0:F0}��", lData);
                        }
                        else if (lData >= 1 || lData <= -1) //99.99��
                        {
                            result = string.Format("{0:F2}��", lData);
                        }
                        else
                            result = string.Format("{0:F0}", lData);
                    }
                    break;

                case "Normal":
                    result = originalData;
                    break;

                default:
                    result = "Undefined";
                    break;

            }

            return result;
        }
    }
    /// <summary>
    /// ƴ������
    /// </summary>
    public enum CombinType
    {
        /// <summary>
        /// ǰ׺
        /// </summary>
        P,
        /// <summary>
        /// ԭ�����
        /// </summary>
        N,
        /// <summary>
        /// ��׺
        /// </summary>
        Q,
        /// <summary>
        /// �������
        /// </summary>
        S
    }

    /// <summary>
    /// �ֶ�������Ϣ��ȡ���洢
    /// </summary>
    public static class FieldInfoCfgFileIO
    {
        #region �����ļ���������
        private const string FieldIndexAttriName = "Field";
        private const string NameAttriName = "Name";
        private const string FormatAttriName = "Format";
        private const string ColorAttriName = "Color";
        private const string ExportTypeAttriName = "ExportType";
        private const string ExportFormatAttriName = "ExportFormat";
        #endregion

        private const string SecondDefaultNode = "/AllMarket/Default";
        private const string SecondMarketNode = "/AllMarket/Market";
        private const string ColorStartChart = "$";

        private static readonly string QuoteFieldFilePath
            = Path.Combine(PathUtilities.CfgPath, "QuoteField.xml");

        /// <summary>
        /// Ĭ���ֶ�������Ϣ
        /// Key->�����ļ��е�Name����; Value->��Name�ֶ�������Ϣ
        /// </summary>
        public static Dictionary<string, FieldInfo> DicDefaultFieldInfo;

        /// <summary>
        /// �����г��ֶ�������Ϣ
        /// Key->�����ļ��е��г�����; Value->���г�MarketType�µ�����Name:������Ϣ��
        /// </summary>
        public static Dictionary<MarketType, Dictionary<string, FieldInfo>> DicMarketFieldInfo;



        static FieldInfoCfgFileIO()
        {
            LoadConfig();
        }

        public static void LoadConfig()
        {
            if (!File.Exists(QuoteFieldFilePath))
                return;

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(QuoteFieldFilePath);
            }
            catch (IOException ioe)
            {
                LogUtilities.LogMessage("Load FieldInfo Error : " + ioe.Message);
                throw;
            }
            catch (XmlException xe)
            {
                LogUtilities.LogMessage("Load FieldInfo Error : " + xe.Message);
                throw;
            }
            catch (Exception ex)
            {
                LogUtilities.LogMessage("Load FieldInfo Error : " + ex.Message);
                throw;
            }

            try
            {
                DicDefaultFieldInfo = GetDefaultFieldInfo(doc);
                DicMarketFieldInfo = GetMarketFieldInfo(doc);
            }
            catch (Exception ex)
            {
                LogUtilities.LogMessage("Load FieldInfo Error : " + ex.Message);
                throw;
            }
        }


        private static Dictionary<string, FieldInfo> GetDefaultFieldInfo(XmlDocument doc)
        {
            Dictionary<string, FieldInfo> dic = new Dictionary<string, FieldInfo>();
            // Set default dictionary:
            XmlNode root = doc.SelectSingleNode(SecondDefaultNode);

            if (root == null)
                return dic;
            if (root.ChildNodes == null)
                return dic;

            int length = root.ChildNodes.Count;

            if (length == 0)
                return dic;

            if (TryGetAllColFieldInfo(root, out dic))
                return dic;


            return dic;
        }

        /// <summary>
        /// ��ȡColumn Ҷ�ӽڵ㷵���ֵ�
        /// </summary>
        /// <param name="pareentNode"></param>
        ///  <param name="dic">�����ֵ�</param>
        /// <returns></returns>
        private static bool TryGetAllColFieldInfo(XmlNode pareentNode, out Dictionary<string, FieldInfo> dic)
        {
            dic = new Dictionary<string, FieldInfo>();

            if (pareentNode == null)
                return false;
            if (pareentNode.ChildNodes == null)
                return false;

            int length = pareentNode.ChildNodes.Count;

            if (length == 0)
                return false;

            FieldInfo info;
            foreach (XmlNode node in pareentNode.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    string name = node.Attributes[NameAttriName].Value;

                    // Assign the info
                    info = GetFieldInfoByNode(node);

                    dic[name] = info;
                }
            }

            return true;
        }

        private static Dictionary<MarketType, Dictionary<string, FieldInfo>> GetMarketFieldInfo
            (XmlDocument doc)
        {
            Dictionary<MarketType, Dictionary<string, FieldInfo>> marketDic =
                new Dictionary<MarketType, Dictionary<string, FieldInfo>>();
            // Set default dictionary:
            XmlNodeList marketNodes = doc.SelectNodes(SecondMarketNode);

            Dictionary<string, FieldInfo> dic;
            foreach (XmlNode node in marketNodes)
            {
                if (TryGetAllColFieldInfo(node, out dic))
                {
                    string marketsStr = node.Attributes[NameAttriName].Value;
                    string[] markets = marketsStr.Split(',');

                    foreach (string marketStr in markets)
                    {
                        MarketType market = (MarketType)Enum.Parse(typeof(MarketType), marketStr);

                        marketDic[market] = dic;
                    }
                }

            }

            return marketDic;
        }
        private static FieldInfo GetFieldInfoByNode(XmlNode node)
        {
            FieldInfo result = new FieldInfo();

            try
            {
                // Assign the info
                string nameStr = node.Attributes[NameAttriName].Value;
                XmlAttribute fieldIndexObj = node.Attributes[FieldIndexAttriName];
                string fieldIndexStr = fieldIndexObj == null ? nameStr : fieldIndexObj.Value;
                string colorStr = node.Attributes[ColorAttriName].Value;
                string formatStr = node.Attributes[FormatAttriName].Value;

                FieldIndex fieldIndex = (FieldIndex)Enum.Parse(typeof(FieldIndex), fieldIndexStr);
                result.FieldIndex = fieldIndex;
                result.ColorSetting = GetColorSetting(colorStr);
                result.Format = GetFormat(formatStr);

                //��ѡ���ԵĻ��
                //
                // ������������
                XmlAttribute exportTypeAttri = node.Attributes[ExportTypeAttriName];
                string exportTypeAttriStr = exportTypeAttri == null ? string.Empty : exportTypeAttri.Value;
                result.ExportType = FieldInfoCfgHelper.GetExportType(exportTypeAttriStr, fieldIndex);
                //
                // �������ݸ�ʽ
                XmlAttribute exportFormatAttri = node.Attributes[ExportFormatAttriName];
                result.ExportFormat = exportFormatAttri == null ? result.Format : GetFormat(exportFormatAttri.Value);
            }
            catch
            {
                throw;
            }

            return result;
        }

        /// <summary>
        /// �����ɫ��������
        /// </summary>
        /// <param name="colorAttri"></param>
        /// <returns></returns>
        private static List<string> GetColorSetting(string colorAttri)
        {
            List<string> result = new List<string>(2);

            // Start with normal letter.
            if( (64 < colorAttri[0] && colorAttri[0] < 91)
                ||(96 < colorAttri[0] && colorAttri[0] < 123))
            {
                result.Add(colorAttri);
                return result;
            }

            // Start with $(# ... etc.) 
            result.Add(colorAttri[0].ToString());
            string leftStr = colorAttri.Substring(1);
            result.Add(leftStr);           

            return result;
        }

        /// <summary>
        /// ��������ʽ��������
        /// </summary>
        /// <param name="formatAttri"></param>
        /// <returns></returns>
        private static List<List<string>> GetFormatSetting(string formatAttri)
        {
            List<List<string>> result = new List<List<string>>(3);
            if (string.IsNullOrEmpty(formatAttri))
                return result;
            string[] formats = formatAttri.Split(',');

            if (formats.Length == 0)
                return result;

            foreach (string eachFormat in formats)
            {
                string[] subFormats = eachFormat.Split(':');
                List<string> subFormatList = new List<string>(subFormats);
                result.Add(subFormatList);
            }


            return result;
        }

        /// <summary>
        /// ���������ļ���formate�������ַ�����ö�Ӧ��Format����
        /// </summary>
        /// <param name="formatAttri">�����ļ���formate�������ַ���</param>
        /// <returns>Format����</returns>
        private static Format GetFormat(string formatAttri)
        {
            Format result = new Format();

            if (string.IsNullOrEmpty(formatAttri))
                return result;

            string[] formats = formatAttri.Split(',');
            if (formats.Length == 0)
                return result;

            foreach (string eachFormat in formats)
            {
                string[] subFormats = eachFormat.Split(':');

                if (subFormats.Length < 2)
                {
                    LogUtilities.LogMessage(eachFormat + " is incorrect format string!");
                    continue;
                }

                string formatFlag = subFormats[0];
                string formatConetent = subFormats[1];

                switch (formatFlag)
                {
                    case "Z":
                        {
                            // Sample: Z:Now:E �� Z:this:H
                            //  Z��ͷ-���ĳ���ֶ���0��ʾ���/�գ�
                            // ��Z:Now:E��ʾ���Nowֵ��0����ʾString.Empty; 
                            // ��Z:this:H��ʾ������ֶε�ֵ��0����ʾ���
                            CompareTarget CompareTarget; FieldIndex comparedIndex = FieldIndex.Market;
                            if (subFormats[1].Equals("this", StringComparison.CurrentCultureIgnoreCase))
                                CompareTarget = CompareTarget.This;
                            else
                            {
                                CompareTarget = CompareTarget.SpecialFieldIndex;
                                comparedIndex = (FieldIndex)Enum.Parse(typeof(FieldIndex), formatConetent, true);
                            }

                            ZeroTypeFlag ZeroTypeFlag = (ZeroTypeFlag)Enum.Parse(typeof(ZeroTypeFlag), subFormats[2]);
                            result.ZeroRule = new ZeroRule(CompareTarget, comparedIndex, ZeroTypeFlag);
                        }
                        break;

                    case "A":
                    case "C":
                    case "M":
                    case "D":
                        {
                            // Sample:  A:100:F2 
                            // A:100:F2   A��ͷ��ʾ�ӣ���100,������λС��
                            // C:100:F2   C��ͷ��ʾ������100,������λС��
                            // M:100:F2   M��ͷ��ʾ�ˣ���100,������λС��
                            // D:100:F2   D��ͷ��ʾ������100,������λС��

                            if (result.CalculateRules == null)
                                result.CalculateRules = new List<CalculateRule>(1);

                            CalMethod calMethod = (CalMethod)Enum.Parse(typeof(CalMethod), subFormats[0]);
                            float zoomNum;
                            if (float.TryParse(subFormats[1], out zoomNum))
                            {
                                result.CalculateRules.Add(new CalculateRule(calMethod, zoomNum, subFormats[2]));
                            }
                        }
                        break;



                    case "N":
                        result.NRule = new NRule(formatConetent);
                        break;

                    case "P":
                        result.PRule = new PRule(formatConetent);
                        break;

                    case "Q":
                        result.QRule = new QRule(formatConetent);
                        break;

                    case "S":
                        result.SRule = new SRule(formatConetent);
                        break;

                    default:
                        break;

                }
            }

            return result;
        }

    }
}
