using System;
using System.Collections.Generic;
 
using System.Text;

namespace OwLib
{
    /// <summary>
    /// 
    /// </summary>
    public class ConnectInfo 
    {
        /// <summary>
        /// 连接模式
        /// </summary>
        public string ConnectMode ;
        /// <summary>
        /// TCP地址
        /// </summary>
        public string TcpAddress ;
        /// <summary>
        /// TCP端口
        /// </summary>
        public string TcpPort ;
        /// <summary>
        /// http连接字符串
        /// </summary>
        public string HttpConnectStr ;
    }

    ///<summary>
    /// 系统配置类
    ///</summary>
    static public class SystemConfig
    {
        static SystemConfig()
        {
            ClientProgramVersion = 91028001;
            FeeCommision = 0.0015;
            LastSortMode = SortMode.Mode_Code;
            LastSortFieldName = "DifferRange";
            LastSelectedBlockId = -1;
            IndexNames = new List<string>(30);
            IndexNames.Add("indexkline");
            IndexNames.Add("indexvol");
            IndexNames.Add("indexmacd");
            IndexNames.Add("indexkdj");
            IndexNames.Add("indexrsi");
            IndexNames.Add("indexarbr");
            KlinePadNum = 2;
        }

        ///<summary>
        /// 判断是否是5.0操作系统的手机
        ///</summary>
        public static bool IsMobile50
        {
            get
            {
                Version vs = Environment.Version;
                if (vs.Minor <= 1 && vs.Major <= 5)
                    return true;
                else
                    return false;
            }
        }

        ///<summary>
        /// 客户端代码版本
        ///</summary>
        private static int _clientProgramVersion;

        public static int ClientProgramVersion
        {
            get { return SystemConfig._clientProgramVersion; }
            private set { SystemConfig._clientProgramVersion = value; }
        }

        ///<summary>
        /// 服务器端代码版本
        ///</summary>
        public static int ServerProgramVersion ;

        ///<summary>
        /// 用户基本信息
        ///</summary>
        public static UserInfo UserInfo ;

        ///<summary>
        /// 佣金手续费
        ///</summary>
        public static double FeeCommision ;

        ///<summary>
        /// 手机左功能键键值
        ///</summary>
        public static int LeftSoftKey = 112;

        ///<summary>
        /// 手机右功能键键值
        ///</summary>
        public static int RightSoftKey = 113;

        #region 手机上不同分辨率下的放大处理
        /// <summary>
        /// 
        /// </summary>
        public static float DeviceDpiY;
        const int PointsPerInch = 96;
        /// <summary>
        /// 
        /// </summary>
        public static int ScaleX(int argx, float dpiX)
        {
            return Convert.ToInt32(argx * dpiX / PointsPerInch);
        }
        /// <summary>
        /// 
        /// </summary>
        public static int ScaleY(int argy, float dpiY)
        {
            return Convert.ToInt32(argy * dpiY / PointsPerInch);
        }
        /// <summary>
        /// 
        /// </summary>
        public static int ScaleY(int argy)
        {
            return Convert.ToInt32(argy * DeviceDpiY / PointsPerInch);
        }
        /// <summary>
        /// 
        /// </summary>
        public static int FScaleY(int argy, float dpiY)
        {
            return Convert.ToInt32(argy * PointsPerInch / dpiY);
        }
        /// <summary>
        /// 
        /// </summary>
        public static int FScaleY(int argy)
        {
            if (DeviceDpiY > 0)
                return Convert.ToInt32(argy * PointsPerInch / DeviceDpiY);
            else
                return argy;
        }
        #endregion

        #region 最后一次板块排序的设置
        /// <summary>
        /// 最后一次选择的板块的ID号
        /// </summary>
        public static long LastSelectedBlockId ;

        /// <summary>
        /// 最后一次排序的指标
        /// </summary>
        public static string LastSortFieldName ;

        /// <summary>
        /// 最后一次排序的顺序
        /// </summary>
        public static SortMode LastSortMode ;
        /// <summary>
        /// 最后一次报价显示的TabName
        /// </summary>
        public static string LastQuoteTabName ;

        /// <summary>
        /// 已过时，未使用，在手机终端使用的最后一次选择的非自选股的板块名
        /// </summary>
        private static string _lastSelectBlockNameNonZixuan;

        public static string LastSelectBlockNameNonZixuan
        {
            get { return SystemConfig._lastSelectBlockNameNonZixuan; }
            set { SystemConfig._lastSelectBlockNameNonZixuan = value; }
        }
        #endregion

        #region 日线界面的设置
        /// <summary>
        /// 指示目前日线界面有几个窗口
        /// </summary>
        public static int KlinePadNum ;

        /// <summary>
        /// 缺省情况下5个窗口的缺省指标名
        /// </summary>
        public static List<string> IndexNames ;
        #endregion


        /*
        #region url解析对象类
        private static IUrlParser _urlParser;
        ///<summary>
        /// 解析参数类
        ///</summary>
        public static IUrlParser UrlParser
        {
            get
            {
                if (_urlParser == null)
                    _urlParser = ServiceHelper.GetService<IUrlParser>();
                return _urlParser;
            }
        }
        #endregion*/
    }
}
