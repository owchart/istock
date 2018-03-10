using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
 
using System.Text;
using dataquery;


namespace EmQComm
{
    /// <summary>
    /// 路径
    /// </summary>
    public static class PathUtilities
    {
        /// <summary>
        /// 设置路径
        /// </summary>
        public static void SetDataPath(string configPath, string dataPath, string userPath, string skinsPath)
        {
            DataPath = dataPath;
            CfgPath = configPath;
            UserPath = userPath;
            SkinsPath = skinsPath;

            if (Directory.Exists(OldNewsPath))
                Directory.Delete(OldNewsPath, true);
            if (Directory.Exists(OldReportPath))
                Directory.Delete(OldReportPath, true);
            if (Directory.Exists(DataWgtPath))
                Directory.Delete(DataWgtPath, true);
            try
            {
                if (Directory.Exists(F10Path))
                    Directory.Delete(F10Path, true);
            }
            catch (Exception ex)
            {
                LogUtilities.LogMessage(ex.Message);
            }
            try
            {
                if (Directory.Exists(DataDealPath))
                    Directory.Delete(DataDealPath, true);
                if (Directory.Exists(DataNowPath))
                    Directory.Delete(DataNowPath, true);
            }
            catch (Exception ex)
            {
                LogUtilities.LogMessage(ex.Message);
            }


            if (!Directory.Exists(DataPath))
                Directory.CreateDirectory(DataPath);
            if (!Directory.Exists(DataDayPath))
                Directory.CreateDirectory(DataDayPath);
            if (!Directory.Exists(DataMinPath))
                Directory.CreateDirectory(DataMinPath);

            if (!Directory.Exists(DataTickPath))
                Directory.CreateDirectory(DataTickPath);
            if (!Directory.Exists(DataMntPath))
                Directory.CreateDirectory(DataMntPath);

            if (!Directory.Exists(F10Path))
                Directory.CreateDirectory(F10Path);
            if (!Directory.Exists(NewsPath))
                Directory.CreateDirectory(NewsPath);
            if (!Directory.Exists(ReportPath))
                Directory.CreateDirectory(ReportPath);
        }


        private static string _rootPath = "";
 //      private static string _rootPath = Directory.GetParent(Process.GetCurrentProcess().MainModule.FileName).FullName.Replace("bin", "");
        /// <summary>
        /// 根路径
        /// </summary>
        public static string RootPath
        {
            get { return _rootPath; }
            set { _rootPath = value; }
        }

        private static string _dataPath;
        /// <summary>
        /// 数据路径
        /// </summary>
        public static string DataPath
        {
            get
            {
                    return _dataPath;
            }
            set
            {
                _dataPath = value;
            }
        }

        /// <summary>
        /// sData
        /// </summary>
        public static string SDataPath
        {
            get
            {
                if (_dataPath == null)
                {
                    _dataPath = DataCenter.GetAppPath() + "\\NecessaryData\\";
                }
                return _dataPath;
            }
            
        }

        public static string MDataPath
        {
            get
            {
                return _dataPath;
            }
        }

        private static string _skinsPath;
        /// <summary>
        /// 皮肤路径
        /// </summary>
        public static string SkinsPath
        {
            get
            {
                if (string.IsNullOrEmpty(_skinsPath))
                    return Path.Combine(_rootPath, @"skins\default\quote\");
                else
                    return Path.Combine(_skinsPath, @"quote\");
            }
            set
            {
                _skinsPath = value;
            }
        }

        private static string _cfgPath;
        /// <summary>
        /// 配置路径
        /// </summary>
        public static string CfgPath
        {
            get
            {
                if (string.IsNullOrEmpty(_cfgPath))
                    return Path.Combine(_rootPath, @"Config\QuoteTest\");
                else
                    return Path.Combine(_cfgPath, @"Quote\");
            }
            set
            {
                _cfgPath = value;
            }
        }

        private static string _userPath;
        /// <summary>
        /// 用户路径
        /// </summary>
        public static string UserPath
        {
            get
            {
                string userPath = "" + @"Quote\";
                if (!Directory.Exists(userPath))
                    Directory.CreateDirectory(userPath);
                return userPath;
                //测试用户
                string path = @"D:\work\FinalTerminal\src\run\" + @"config\users\test001\";
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                return path;

                if (string.IsNullOrEmpty(_userPath))
                {
                    string tmp = Path.Combine(_rootPath, @"users\");
                    tmp = Path.Combine(tmp, SystemConfig.UserInfo.UserName + @"\");
                    return tmp;
                }
                else
                    return _userPath;
            }
            set
            {
                _userPath = value;
            }
        }
        /// <summary>
        /// 日志路径
        /// </summary>
        public static string LogPath
        {
            get
            {
                
                return Path.Combine(_rootPath, @"Log");
            }
        }
        /// <summary>
        /// 板块树路径
        /// </summary>
        public static string BlockTreePath
        {
            get { return Path.Combine(_dataPath, @"NecessaryData\BLK_BLOCKTREE"); }
        }
        /// <summary>
        /// 旧新闻路径
        /// </summary>
        public static string OldNewsPath
        {
            get
            {
                return Path.Combine(_dataPath, @"News");
            }
        }
        /// <summary>
        /// 旧报告路径
        /// </summary>
        public static string OldReportPath
        {
            get
            {
                return Path.Combine(_dataPath, @"Report");
            }
        }

        /// <summary>
        /// 资源文件
        /// </summary>
        public  static string ImagePath
        {
            get
            {
                return Path.Combine(_dataPath, @"mdata\\Plugin\\Images\\Quote\\");
            }
        }
        /// <summary>
        /// 新闻路径
        /// </summary>
        public static string NewsPath
        {
            get
            {
                return Path.Combine(_dataPath, @"QuoteNews");
            }
        }

        /// <summary>
        /// F10路径
        /// </summary>
        public static string F10Path
        {
            get
            {
                return Path.Combine(_dataPath, @"F10");
            }
        }
        /// <summary>
        /// 报告路径
        /// </summary>
        public static string ReportPath
        {
            get
            {
                return Path.Combine(_dataPath, @"QuoteReports");
            }
        }
        /// <summary>
        /// Bmp文件路径
        /// </summary>
        public static string BmpPath
        {
            get { return Path.Combine(_rootPath, @"Bmp"); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string DataWgtPath
        {
            get { return Path.Combine(DataPath, @"Weight\"); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string DataMntPath
        {
            get { return Path.Combine(DataPath, @"Mnt\"); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string DataDayPath
        {
            get { return Path.Combine(DataPath, @"Day\"); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string DataDBFPath
        {
            get { return DataPath + @"DBF\"; }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string DataMinPath
        {
            get { return Path.Combine(DataPath, @"Min\"); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string DataTickPath
        {
            get { return Path.Combine(DataPath, @"Tick\"); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string DataDealPath
        {
            get { return Path.Combine(DataPath, @"Deal\"); }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string DataNowPath
        {
            get { return Path.Combine(DataPath, @"Now\"); }
        }


        /*
        /// <summary>
        /// 从完整的路径中获得文件名称
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>文件名</returns>
        public static string GetFileNameWithoutDot(string filePath)
        {
            int startIndex = filePath.LastIndexOf(@"\");
            int endIndex = filePath.LastIndexOf(@".");
            if (endIndex == -1)
                endIndex = filePath.Length;
            int length = endIndex - startIndex - 1;
            return filePath.Substring(startIndex + 1, length);
        }

        /// <summary>
        /// 从完整的路径中获得文件名称
        /// </summary>
        /// <returns>文件名</returns>
        public static string GetFileName(string filePath)
        {
            int startIndex = filePath.LastIndexOf(@"\");
            return startIndex == -1 ? filePath : filePath.Substring(startIndex + 1, filePath.Length - (startIndex + 1));
        }

        public static string GetFilePath(string filePath)
        {
            int length = filePath.LastIndexOf(@"\");
            if( length == -1)
                length = filePath.Length;
            return filePath.Substring(0, length);
        }*/
    }
}
