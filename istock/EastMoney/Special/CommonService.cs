using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Data;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using EmCore;
using EmSerDataService;

namespace OwLib
{
    /// <summary>
    /// 公用方法集合类
    /// </summary>
    public class CommonService
    {     

        /// <summary>
        /// 专题分类枚举
        /// </summary>
        public enum SpeicalType
        {
            /// <summary>
            /// 默认
            /// </summary>
            DEFAULT,

            /// <summary>
            /// 股票
            /// </summary>
            SPE,

            /// <summary>
            /// 宏观专题
            /// </summary>
            MAC,

            /// <summary>
            /// 债券专题
            /// </summary>
            BSPE,

            /// <summary>
            /// 基金专题
            /// </summary>
            FSPE,

            /// <summary>
            /// 证券市场专题
            /// </summary>
            MSPE,

            /// <summary>
            /// 指数专题
            /// </summary>
            ISPE,
            /// <summary>
            /// 期货专题
            /// </summary>
            FTSPE,
           

            /// <summary>
            /// 行业专题
            /// </summary>
            IND,

            /// <summary>
            /// 期货专题
            /// </summary>
            FUTURE

        }

        /// <summary>
        /// 获取过滤框的对应时间
        /// </summary>
        /// <param name="date"></param>
        /// <param name="nowTime">当前时间</param>
        /// <returns>获取的时间</returns>
        public static DateTime GetCustomDate(CustomDate date, DateTime nowTime)
        {
            DateTime resultTime = nowTime;
            switch (date.CalType)
            {
                case "年":
                    //resultTime = CommonContant.ServerDateTime.AddYears(0 - date.Value);
                    
                    resultTime =Convert.ToDateTime(CommonContant.ServerDateTime.Year.ToString()+"-01-01").AddYears(0 - date.Value);
                    break;
                case "季":
                    resultTime = CommonContant.ServerDateTime.AddMonths(0 - date.Value*3);
                    break;
                case "月":
                    resultTime = CommonContant.ServerDateTime.AddMonths(0 - date.Value);
                    break;
                case "天":
                    resultTime = CommonContant.ServerDateTime.AddDays(0 - date.Value);
                    break;
                case "CuuertMonth":
                    resultTime = CommonContant.ServerDateTime.AddMonths(0 - date.Value);
                    break;
                case "日":
                    resultTime = date.DateTime;
                    break;
                case "StartMonth":
                    DateTime startMonth = nowTime.AddDays(1 - nowTime.Day); //本月月初
                    resultTime = startMonth.AddMonths(0 - date.Value);
                    break;
                case "CuuertDay":
                    resultTime = nowTime.AddDays(0 - date.Value);
                    break;
                case "EndMonth":
                    DateTime startMonth1 = nowTime.AddDays(1 - nowTime.Day); //本月月初
                    resultTime =
                        startMonth1.AddDays((nowTime.AddMonths(1) - nowTime).Days - 1).AddMonths(0 - date.Value);

                    break;
                case "StartWeek":
                    DateTime startWeek = nowTime.AddDays(1 - Convert.ToInt32(nowTime.DayOfWeek.ToString("d"))); //周一
                    resultTime = startWeek.AddDays(0 - date.Value*7);
                    break;
                case "EndWeek":
                    DateTime startWeek1 = nowTime.AddDays(1 - Convert.ToInt32(nowTime.DayOfWeek.ToString("d"))); //周一
                    resultTime = startWeek1.AddDays(6).AddDays(0 - date.Value*7);
                    ; //周日
                    break;
                case "StartYear":
                    DateTime startYear = new DateTime(nowTime.Year, 1, 1); //本年年初
                    resultTime = startYear.AddYears(0 - date.Value);
                    break;
                case "EndYear":
                    DateTime endYear = new DateTime(nowTime.Year, 12, 31); //本年年末
                    resultTime = endYear.AddYears(0 - date.Value);
                    break;
                case "CuuertYear":
                    DateTime cuuertYear = nowTime.Date;
                    resultTime = cuuertYear.AddYears(0 - date.Value);
                    break;
            }
            return resultTime;
        }
        //private static ClientType ClientType = "0";
        /// <summary>
        /// 获取或设置客户端类型
        /// </summary>
        public static ClientType ISCLIENT { get; set; }

        //public static SCGridTabControl SCGridTabControl = new SCGridTabControl();

        // public static string StaticticsName { get; set; }
        /// <summary>
        /// url地址
        /// </summary>
        public const string URL = "HTTP://app.jg.eastmoney.com";

        /// <summary>
        /// 是否显示板块树
        /// </summary>
        public static bool IsShowBlock { get; set; }

        /// <summary>
        /// 专题类型
        /// </summary>
        public static SpeicalType TypeName;

        /// <summary>
        ///获取或设置服务标准时间
        /// </summary>
        public static DateTime ServerSysTime
        {
            get { return DataQuery.ServerSysTime; }
            private set { }
        }

        /// <summary>
        /// 获取当前月
        /// </summary>
        /// <returns></returns>
        public static int GetCurMonth()
        {
            DateTime date=CommonContant.ServerDateTime;
            if (CommonService.TypeName == SpeicalType.BSPE)
                if (date.Day > 7)
                    return CommonContant.ServerDateTime.Month - 1;
                else
                {
                    return CommonContant.ServerDateTime.Month - 2;
                }
            else
            {
              return  CommonContant.ServerDateTime.Month-1;
            }
        }

        /// <summary>
        /// 获取当前季度
        /// </summary>
        /// <returns></returns>
        public static int GetCurQnarter()
        {
            switch (CommonContant.ServerDateTime.Month)
            {
                case 1:
                case 2:
                case 3:
                    return 0;
                case 4:
                case 5:
                case 6:
                    return 1;
                case 7:
                case 8:
                case 9:
                    return 2;
                case 10:
                case 11:
                case 12:
                    return 3;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="t">对象</param>
        /// <returns>返回序列化结果xml</returns>
        public static string Serialize<T>(T t)
        {
            string res = null;
            try
            {
                res = XmlConvertor.Serializable(t);
            }
            catch
            {
                Type tt = t.GetType();
                if (tt != null)
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(tt);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        XmlWriter writer = XmlWriter.Create(new MemoryStream());

                        // MemoryStream ms = new MemoryStream();
                        serializer.Serialize(writer, t);
                        StreamReader sr = new StreamReader(ms);
                        res = sr.ReadToEnd();
                    }


                }
            }
            return res;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">反序列化类型</typeparam>
        /// <param name="s">数据的存放路径</param>
        /// <returns>反序列化得到的对象</returns>
        public static object Deserialize<T>(string s)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            using (StreamReader sr = new StreamReader(s))
            {
                return serializer.Deserialize(sr);
            }

            //  Type t = Type.GetType(T);
            // return XmlConvertor.Deserialize(typeof(T), s, false);

        }
        /// <summary>
        /// 反序列化为对象
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="s">对象序列化后的Xml字符串</param>
        /// <returns></returns>
        public static object Deserialize(string s, string type)
        {
            Type t = Type.GetType(type);
            return Deserialize(s, t);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="s">字符串</param>
        /// <param name="type">对象类型</param>
        /// <returns>反序列化后的对象</returns>
        public static object Deserialize(string s, Type type)
        {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(type);
            using (StringReader reader=new StringReader(s))
            {
                
                return serializer.Deserialize(reader);
            }
            //return XmlConvertor.Deserialize(type, s, false);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] rawSerialize(object obj)
        {
            int rawsize = Marshal.SizeOf(obj);
            IntPtr buffer = Marshal.AllocHGlobal(rawsize);
            Marshal.StructureToPtr(obj, buffer, false);
            byte[] rawdatas = new byte[rawsize];
            Marshal.Copy(buffer, rawdatas, 0, rawsize);
            Marshal.FreeHGlobal(buffer);
            return rawdatas;
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="rawdatas"></param>
        /// <returns></returns>
        public static Object rawDeserialize(byte[] rawdatas,Type type)
        {
            Type anytype = type;
            int rawsize = Marshal.SizeOf(anytype);
            if (rawsize > rawdatas.Length) return null;
            IntPtr buffer = Marshal.AllocHGlobal(rawsize);
            Marshal.Copy(rawdatas, 0, buffer, rawsize);
            object retobj = Marshal.PtrToStructure(buffer, anytype);
            Marshal.FreeHGlobal(buffer);
            return  retobj;
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">要序列化的对象</typeparam>
        /// <param name="str">字符串</param>
        /// <returns>返回的反序列化对象结果</returns>
        public static T JsonDeserializeObject<T>(string str)
        {
            return JSONHelper.DeserializeObject<T>(str);
        }

        /// <summary>
        /// 获取时间名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetDateTimeName(string fileName)
        {
            return fileName + CommonContant.ServerDateTime.ToString("yyyy/MM/DD");
        }

        /// <summary>
        /// 对象序列化
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>序列化后的字符串</returns>
        public static string ObjectToStringSerialize(object o)
        {
            return Convert.ToBase64String(BinaryFormatHelper.ToByteSerialize(o));
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="str">要反序列化的字符串</param>
        /// <returns>反序列化后的对象</returns>
        public static T StringToObjDeserialize<T>(string str)
        {
            return BinaryFormatHelper.ToDeserialize<T>(Convert.FromBase64String(str));
        }

        /// <summary>
        /// 读取xml配置信息给对象赋值
        /// </summary>
        /// <param name="xml">xml配置文本</param>
        public static void Deserialize(object deserialize, XmlNode xml)
        {
            Type type = deserialize.GetType();
            foreach (XmlNode node in xml.ChildNodes)
            {
                PropertyInfo propertyInfo = type.GetProperty(node.Name);
                if (propertyInfo != null)
                {
                    Type t = propertyInfo.PropertyType;
                    GetValue(deserialize, t, propertyInfo, node,null);
                }
            }
        }

        private static  void GetValue(object deserialize,Type t, PropertyInfo propertyInfo, XmlNode node,List<Object> list )
        {
            if (t == typeof (string))
            {
                SetProperty(deserialize, propertyInfo.Name, node.Value);
            }
            else if (t.IsEnum)
            {
                SetProperty(deserialize, propertyInfo.Name, node.Value);
            }
            else if (t.IsArray)
            {
                Type type = t.GetElementType();
                GetValue(deserialize,type, propertyInfo, node,list);
            }
            else if (t == typeof (int))
            {
                int value = int.Parse(node.Value);
                SetProperty(deserialize, propertyInfo.Name, value);
            }
            else
            {
               //Deserialize(, node)
                ;
            }
        }
        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="obj">设置属性的控件</param>
        /// <param name="property">属性名</param>
        /// <param name="value">属性值</param>
        public static void SetProperty(object obj, string property, object value)
        {
            System.Reflection.PropertyInfo lObjProperties = obj.GetType().GetProperty(property);
            if (value == null)
            {
                return;
            }
            if (value.GetType().IsArray)
            {
                Array arr = (Array) value;
                object o = Activator.CreateInstance(lObjProperties.PropertyType, arr.Length);
                Array.Copy(arr, (Array)o, arr.Length);
                lObjProperties.SetValue(obj, o, null);


            }
            else
            {
                if (lObjProperties == null)
                    return;
                lObjProperties.SetValue(obj, Convert.ChangeType(value, lObjProperties.PropertyType), null);
            }

        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="property">属性信息</param>
        /// <param name="value"></param>
        public static void SetProperty(object obj, PropertyInfo property, object value)
        {

            if (value == null)
            {
                return;
            }
            if (value.GetType().BaseType == typeof (Array))
            {
                Array arr = (Array) value;
                object o = null;
                try
                {
                    o = Activator.CreateInstance(property.PropertyType, arr.Length);
                }
                catch
                {
                    o = new object[arr.Length];
                }
                Array.Copy(arr, (Array) o, arr.Length);
                property.SetValue(obj, o, null);


            }
            else
            {
                if (property.PropertyType.IsEnum)
                {
                    value = Enum.Parse(property.PropertyType, value.ToString());
                }

                property.SetValue(obj, value, null);
            }

        }

        /// <summary>
        /// 从对象取属性的值
        /// </summary>
        /// <param name="p"></param>
        /// <param name="o"></param>
        /// <returns></returns>
        public static object GetValue(PropertyInfo p, object o)
        {
            return p.GetValue(o, null);
        }

        /// <summary>
        /// 列集合克隆
        /// </summary>
        /// <param name="filters">数据列</param>
        /// <returns></returns>
        public static List<EMGridColumn> Clone(EMGridColumn[] filters)
        {
            if (filters == null)
                return null;
            List<EMGridColumn> result = new List<EMGridColumn>();
            foreach (EMGridColumn filter in filters)
            {
                if (filter != null)
                    result.Add(Clone(filter));
            }
            return result;
        }

        /// <summary>
        /// 列集合克隆
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static List<EMGridColumn> Clone(List<EMGridColumn> filters)
        {
            List<EMGridColumn> result = new List<EMGridColumn>();
            foreach (EMGridColumn filter in filters)
            {
                if (filter != null)
                    result.Add(Clone(filter));
            }
            return result;
        }

        /// <summary>
        /// 过滤条件集合克隆
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static List<FilterMode> Clone(List<FilterMode> filters)
        {
            List<FilterMode> result = new List<FilterMode>();
            foreach (FilterMode filter in filters)
            {
                if (filter != null)
                    result.Add(Clone(filter));
            }
            return result;
        }

        /// <summary>
        /// 属性集合克隆
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static List<PropertiyName> Clone(List<PropertiyName> filters)
        {
            List<PropertiyName> result = new List<PropertiyName>();
            foreach (PropertiyName filter in filters)
            {
                if (filter != null)
                    result.Add(Clone(filter));
            }
            return result;
        }

        /// <summary>
        /// 过滤条件克隆
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static FilterMode Clone(FilterMode filter)
        {

            return (FilterMode) filter.Clone();
        }

        /// <summary>
        /// 数据列克隆
        /// </summary>
        /// <param name="gridColumn"></param>
        /// <returns></returns>
        public static EMGridColumn Clone(EMGridColumn gridColumn)
        {

            return (EMGridColumn)gridColumn.Clone();
        }

        /// <summary>
        /// 属性类克隆
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static PropertiyName Clone(PropertiyName propertyName)
        {

            return (PropertiyName) propertyName.Clone();
        }

        /// <summary>
        /// 数据源类克隆
        /// </summary>
        /// <param name="adaptor"></param>
        /// <returns></returns>
        public static DataSourceAdaptor Clone(DataSourceAdaptor adaptor)
        {

            return (DataSourceAdaptor) adaptor.Clone();
        }


        /// <summary>
        /// 写终端日志
        /// </summary>
        /// <param name="log"></param>
        public static void Log(string log)
        {

            EmCore.EmLog.Write("专题:",
                                string.Format("***专题logStart***\r\n{0}\r\n***专题logEnd***", log), LogScope.Debug);

        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="e"></param>
        public static void Log(Exception e)
        {
            string info = string.Format("***专题logStart***\r\n{0}\r\n{1}\r\n***专题logEnd***", e.Message, e.StackTrace);           

            EmCore.EmLog.Write("专题:",
                                string.Format("***专题logStart***\r\n{0}\r\n{1}\r\n***专题logEnd***", e.Message, e.StackTrace), LogScope.Debug);
            //MessageBox.Show(info);

        }
        public static void EmLog(string log,LogScope logType)
        {

            EmCore.EmLog.Write("专题:", log, logType);

        }

        /// <summary>
        /// 时间转换键值对
        /// </summary>
        public static Dictionary<DateTime, DateTime> DateExchange = new Dictionary<DateTime, DateTime>();

        /// <summary>
        /// 初始化时间数据
        /// </summary>
        public static void InitTimeData()
        {
            try
            {
                DataTable exchanggedate;
                exchanggedate = DataCache.QueryModDatere();
                for (int i = 0; i < exchanggedate.Rows.Count; i++)
                {
                   
                    // if (exchanggedate.Rows[i]["DAT_DAYS"] == null || exchanggedate.Rows[i]["DAT_DAYS"] == DBNull.Value) return;
                    //DateExchange[((DateTime)exchanggedate.Rows[i]["DAT_DAYS"]).Date] =
                    DateExchange[Convert.ToDateTime(exchanggedate.Rows[i]["DAT_DAYS"]).Date] =
                        Convert.ToDateTime(exchanggedate.Rows[i]["TDAY"]);
                }
                exchanggedate.Clear();
                exchanggedate.Dispose();
            }
            catch (Exception)
            {
                
                //throw;
            }
            
        }

        /// <summary>
        /// 获取转换后的日期
        /// </summary>
        /// <param name="datevalue"></param>
        /// <returns></returns>
        public static DateTime GetTradingDay(DateTime datevalue)
        {
            if (CommonService.ISCLIENT == ClientType.Config)
            {
                InitTimeData();
            }
            DateTime dt = datevalue.Date;
            if (DateExchange.Count==0)
                InitTimeData();
            if (DateExchange.ContainsKey(dt))
            {
                return DateExchange[dt];
            }
            else
                return datevalue;

        }

        /// <summary>
        /// 设置背景颜色
        /// </summary>
        /// <returns></returns>
        public static Color SetBackColor()
        {
            Version ver = System.Environment.OSVersion.Version;

            if (ver.Major == 5 && ver.Minor == 1)
            {
                return Color.FromArgb(227, 229, 232);
                //  strClient = "Win XP";
            }
            else if (ver.Major == 6 && ver.Minor == 0)
            {
                return System.Drawing.SystemColors.Control;
                // strClient = "Win Vista";
            }
            else if (ver.Major == 6 && ver.Minor == 1)
            {
                return System.Drawing.SystemColors.Control;
                //  strClient = "Win 7";
            }
            else if (ver.Major == 5 && ver.Minor == 0)
            {
                return Color.FromArgb(227, 229, 232);
                // strClient = "Win 2000";
            }
            else if (ver.Major == 5 && ver.Minor == 2)
            {
                return Color.FromArgb(227, 229, 232);
                // strClient = "SERVER 2003";
            }
            else
            {
                return System.Drawing.SystemColors.Control;
                //strClient = "未知";
            }
        }

        /// <summary>
        /// 控件销毁
        /// </summary>
        /// <param name="control">需要销毁的控件</param>
        public static void DisposeControl(Control control)
        {
            if (control == null)
                return;
            foreach (Control c in control.Controls)
            {
                if (c != null)
                    DisposeControl(c);
            }
            try
            {
                Type type = control.GetType();
                if (type.Name != "ToolStripContentPanel" && type.Name != "ToolStripPanel")
                {
                    
                    control.Dispose();
                    control.Controls.Clear();
                }
                else
                {

                }

            }
            catch (Exception exception)
            {
                CommonService.Log(string.Format("控件垃圾回收出错: {0}{1}", exception.Message, exception.StackTrace));
            }
         
        }


        /// <summary>
        /// 销毁数据表
        /// </summary>
        /// <param name="dt"></param>
        public static void DisposeDataTable(DataTable dt)
        {
            if (dt != null)
            {
                //foreach (DataColumn col in dt.Columns)
                //    col.Dispose();
              //   dt.Columns.Clear();
                dt.Rows.Clear();
                dt.Clear();
                dt.Dispose();
            }
        }

        #region 获得数字过滤框items的最大值

        /// <summary>
        /// 获得数字过滤框items的最大值
        /// </summary>
        /// <param name="customDate">配置文件配置过来的CustomDate实体</param>
        /// <param name="dateTime">时间参数</param>
        /// <returns>返回的数字过滤框item的最大值</returns>
        internal static int GetMaxValue(CustomDate customDate, DateTime dateTime)
        {
            int resultStr = dateTime.Year;
            switch (customDate.MaxValueType)
            {
                case "年":
                    resultStr = CommonContant.ServerDateTime.AddYears(0 - customDate.PianyiNum).Year;
                    break;
                case "月":
                    resultStr = CommonContant.ServerDateTime.AddMonths(0 - customDate.PianyiNum).Month;
                    break;
                case "季":
                    resultStr = ConvertMonthToQuarter(CommonContant.ServerDateTime.AddMonths(0 - customDate.PianyiNum*3).Month);
                    break;
                case "周":
                    resultStr = ConvertDayToWeek(CommonContant.ServerDateTime.AddDays(0 - customDate.PianyiNum*7).Day);
                    break;
                case "日":
                    resultStr = CommonContant.ServerDateTime.AddDays(0 - customDate.PianyiNum).Day;
                    break;

            }
            return resultStr;
        }

        /// <summary>
        /// 月转换为季度
        /// </summary>
        /// <param name="month"></param>
        /// <returns></returns>
        public static int ConvertMonthToQuarter(int month)
        {
            double f = Convert.ToDouble(month)/3f;
            if (f > Convert.ToInt32(f))
            {
                return Convert.ToInt32(f) + 1;
            }
            return Convert.ToInt32(f);
        }

        /// <summary>
        /// 天转换为周
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static int ConvertDayToWeek(int day)
        {
            double f = Convert.ToDouble(day)/7f;
            if (f > Convert.ToInt32(f))
            {
                return Convert.ToInt32(f) + 1;
            }
            return Convert.ToInt32(f);
        }

        #endregion
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize); ////// 释放内存 

        public static void ClearMemory()
        {
            return;
         //   return;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
            }
        }  
    }
}