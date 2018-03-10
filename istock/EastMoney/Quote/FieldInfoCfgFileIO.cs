using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace OwLib
{
    /// <summary>
    /// 配置文件帮助类
    /// </summary>
    public class FieldInfoCfgHelper
    {
        /// <summary>
        /// 根据当前“输出类型”(ExportType)属性和对应的FieldIndex获得对应的输出类型
        /// 逻辑：如果“输出类型”(ExportType)属性没有/为空，则取其对应的的FieldIndex的类型
        /// </summary>
        /// <param name="ExportTypeStr">“输出类型”(ExportType)属性字符串</param>
        /// <param name="fieldIndex">其对应的的FieldIndex</param>
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
    /// 输出格式
    /// </summary>
    public class Format
    {
        /// <summary>
        /// 判零规则
        /// </summary>
        public ZeroRule ZeroRule;

        /// <summary>
        /// 计算规则
        /// </summary>
        public List<CalculateRule> CalculateRules;

        /// <summary>
        /// 按照精度保留小数规则
        /// </summary>
        public NRule NRule;
        /// <summary>
        /// 前缀规则
        /// </summary>
        public PRule PRule;
        /// <summary>
        /// 后缀规则
        /// </summary>
        public QRule QRule;
        /// <summary>
        /// 特殊规则
        /// </summary>
        public SRule SRule;
    }

    /// <summary>
    /// 界面字段配置信息
    /// </summary>
    public class FieldInfo
    {
        /// <summary>
        /// 界面字段对应的存储层字段
        /// </summary>
        public FieldIndex FieldIndex;
        /// <summary>
        /// 界面字段对应的颜色设置信息
        /// </summary>
        public List<string> ColorSetting;

        /// <summary>
        /// 输出格式
        /// </summary>
        public Format Format;
        /// <summary>
        /// 可选字段，表示导出的输出格式，如果没有这个字段，取Format字段的格式
        /// </summary>
        public Format ExportFormat;
        /// <summary>
        /// 可选字段，表示导出的数据的类型，如果没有这个字段，就取Name对应的FieldIndex的类型（默认是String）
        /// </summary>
        public Type ExportType;
    }
    /// <summary>
    /// 判零规则的比较类型
    /// </summary>
    public enum CompareTarget
    {
        /// <summary>
        /// 拿当前证券代码的某个特殊字段来判零
        /// </summary>
        SpecialFieldIndex,
        /// <summary>
        /// 拿当前证券代码的指标值判零
        /// </summary>
        This
    }

    /// <summary>
    /// 零出列类型
    /// </summary>
    public enum ZeroTypeFlag
    {
        /// <summary>
        /// 判断为零的话指标返回横线："─"
        /// </summary>
        H,
        /// <summary>
        /// 判断为零的话指标返回string.Empty
        /// </summary>
        E
    }

    /// <summary>
    /// 格式规则1：判断为零，优先执行
    /// </summary>
    public class ZeroRule
    {
        /// <summary>
        /// 横线字符串
        /// </summary>
        public static string HorStr = "─";
        /// <summary>
        /// 判零规则的构造函数
        /// </summary>
        /// <param name="compareTarget">比较类型</param>
        /// <param name="comparedIndex"></param>
        /// <param name="zeroTypeFlag"></param>
        public ZeroRule(CompareTarget compareTarget, FieldIndex comparedIndex, ZeroTypeFlag zeroTypeFlag)
        {
            this.CompareTarget = compareTarget;
            this.ZeroTypeFlag = zeroTypeFlag;
            this.ComparedIndex = comparedIndex;
        }
        /// <summary>
        /// 比较类型
        /// </summary>
        public CompareTarget CompareTarget;
        /// <summary>
        /// 若为零的标识符“─”或者string.Empty
        /// </summary>
        public ZeroTypeFlag ZeroTypeFlag;

        /// <summary>
        /// 判零的比较字段
        /// </summary>
        public FieldIndex ComparedIndex;

        /// <summary>
        /// 返回String.Empty还是横线
        /// </summary>
        public string ZeroStr { get { return ZeroTypeFlag == ZeroTypeFlag.E ? string.Empty : HorStr; } }


        /// <summary>
        /// 判断是否为零
        /// </summary>
        /// <param name="originalData">原始值</param>
        /// <param name="comparedIndexValue">判零的比较字段的值</param>
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
    /// 运算规则
    /// </summary>
    public enum CalMethod
    {
        /// <summary>
        /// 加法
        /// </summary>
        A,
        /// <summary>
        /// 减法
        /// </summary>
        C,
        /// <summary>
        /// 乘法
        /// </summary>
        M,
        /// <summary>
        /// 除法
        /// </summary>
        D
    }
    /// <summary>
    /// 格式规则2：运算
    /// </summary>
    public class CalculateRule
    {
        /// <summary>
        /// 运算规则的构造函数
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
        /// 运算方式（加/减/乘/除）
        /// </summary>
        public CalMethod CalMethod;
        /// <summary>
        /// （加/减/乘/除）的数字
        /// </summary>
        public float ZoomNum;
        /// <summary>
        /// 保留小数位(eg. "F2")
        /// </summary>
        public string NumFormat;
        /// <summary>
        /// 获得计算后的结果
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
    ///  N开头表示用原始数据，按精度输出
    /// </summary>
    public class NRule
    {
        /// <summary>
        /// NRule的构造函数
        /// </summary>
        /// <param name="format"></param>
        public NRule(string format)
        { this.FormatStr = format; }

        /// <summary>
        /// 精度保留格式
        /// </summary>
        public string FormatStr;

        /// <summary>
        /// 获得精度保留后的字符串
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
    ///  P开头表示前缀，与0比较，大于0的加前缀"+",另外如P:A，则表示加字符串前缀"A"
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
        /// 前缀内容
        /// </summary>
        public string Profix;

        /// <summary>
        /// 获得运行前缀规则后的格式字符串
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
    /// Q开头表示后缀，加字符串“BP”作为输出后缀
    /// </summary>
    public class QRule
    {
        /// <summary>
        /// 后缀规则
        /// </summary>
        /// <param name="suffix"></param>
        public QRule(string suffix)
        { this.Suffix = suffix; }
        /// <summary>
        /// 后缀
        /// </summary>
        public string Suffix;

        /// <summary>
        /// 获得运行后缀规则后的格式字符串
        /// </summary>
        /// <param name="originalData"></param>
        /// <returns></returns>
        public string GetSuffixedString(string originalData)
        {

            return string.Format("{0}{1}", originalData, Suffix);
        }
    }
    /// <summary>
    ///  S开头表示特殊格式，例S:Date表示Date格式
    /// </summary>
    public class SRule
    {
        /// <summary>
        /// 特殊规则构造函数
        /// </summary>
        /// <param name="specialContent"></param>
        public SRule(string specialContent)
        { this.SpecialContent = specialContent; }
        /// <summary>
        /// 特殊规则内容20
        /// </summary>
        public string SpecialContent;

        /// <summary>
        /// 获得运行后缀规则后的格式字符串
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
                        if (dData >= 1000000 || dData <= -1000000) //100亿
                        {
                            dData /= 10000;
                            result = string.Format("{0:F0}亿", dData);
                        }
                        else if (dData >= 10000 || dData <= -10000) //99.99亿
                        {
                            dData /= 10000;
                            result = string.Format("{0:F2}亿", dData);

                        }
                        else if (dData >= 100 || dData <= -100) //100~9999万
                        {
                            result = string.Format("{0:F0}万", dData);
                        }
                        else if (dData >= 1 || dData <= -1) //99.99万
                        {
                            result = string.Format("{0:F2}万", dData);
                        }
                        else
                            result = string.Format("{0:F0}", dData);
                    }
                    else if (long.TryParse(originalData, out lData))
                    {
                        if ((lData < 10000 && lData > 0) || (lData > -10000 && lData < 0))
                            return string.Format("{0:F0}", lData);
                        lData /= 10000;
                        if (lData >= 1000000 || lData <= -1000000) //100亿
                        {
                            lData /= 10000;
                            result = string.Format("{0:F0}亿", lData);
                        }
                        else if (lData >= 10000 || lData <= -10000) //99.99亿
                        {
                            lData /= 10000;
                            result = string.Format("{0:F2}亿", lData);

                        }
                        else if (lData >= 100 || lData <= -100) //100~9999万
                        {
                            result = string.Format("{0:F0}万", lData);
                        }
                        else if (lData >= 1 || lData <= -1) //99.99万
                        {
                            result = string.Format("{0:F2}万", lData);
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
    /// 拼接类型
    /// </summary>
    public enum CombinType
    {
        /// <summary>
        /// 前缀
        /// </summary>
        P,
        /// <summary>
        /// 原样输出
        /// </summary>
        N,
        /// <summary>
        /// 后缀
        /// </summary>
        Q,
        /// <summary>
        /// 特殊规则
        /// </summary>
        S
    }

    /// <summary>
    /// 字段配置信息读取并存储
    /// </summary>
    public static class FieldInfoCfgFileIO
    {
        #region 配置文件属性名称
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
        /// 默认字段配置信息
        /// Key->配置文件中的Name属性; Value->该Name字段配置信息
        /// </summary>
        public static Dictionary<string, FieldInfo> DicDefaultFieldInfo;

        /// <summary>
        /// 特殊市场字段配置信息
        /// Key->配置文件中的市场属性; Value->该市场MarketType下的所有Name:配置信息对
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
        /// 读取Column 叶子节点返回字典
        /// </summary>
        /// <param name="pareentNode"></param>
        ///  <param name="dic">返回字典</param>
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

                //可选属性的获得
                //
                // 导出数据类型
                XmlAttribute exportTypeAttri = node.Attributes[ExportTypeAttriName];
                string exportTypeAttriStr = exportTypeAttri == null ? string.Empty : exportTypeAttri.Value;
                result.ExportType = FieldInfoCfgHelper.GetExportType(exportTypeAttriStr, fieldIndex);
                //
                // 导出数据格式
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
        /// 获得颜色设置数组
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
        /// 获得输出格式设置数组
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
        /// 根据配置文件中formate的属性字符串获得对应的Format对象
        /// </summary>
        /// <param name="formatAttri">配置文件中formate的属性字符串</param>
        /// <returns>Format对象</returns>
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
                            // Sample: Z:Now:E 和 Z:this:H
                            //  Z开头-如果某个字段是0显示横杠/空，
                            // 例Z:Now:E表示如果Now值是0，显示String.Empty; 
                            // 例Z:this:H表示如果该字段的值是0，显示横杠
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
                            // A:100:F2   A开头表示加，加100,保留两位小数
                            // C:100:F2   C开头表示减，减100,保留两位小数
                            // M:100:F2   M开头表示乘，乘100,保留两位小数
                            // D:100:F2   D开头表示除，除100,保留两位小数

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
