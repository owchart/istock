using System;
using System.Collections.Generic;
using System.IO;
 
using System.Text;
using System.Xml;
using EmQComm;

namespace EmQDataIO
{
    /// <summary>
    /// SysCfgFileIO
    /// </summary>
    public static class SysCfgFileIO
    {
        private static readonly string FileName = PathUtilities.CfgPath + "SysCfg.xml";
        /// <summary>
        /// GetUserInfo
        /// </summary>
        /// <returns></returns>
        public static UserInfo GetUserInfo()
        {
            UserInfo userInfo = new UserInfo();
            if (File.Exists(FileName))
            {
                XmlDocument doc = new XmlDocument();
                
                try
                {
                    doc.Load(FileName);
                    XmlNode root = doc.SelectSingleNode(@"SysConfig/UserInfo");
                    if (root != null)
                    {
                        XmlAttribute atr = root.Attributes["HKDelay"];
                        if (atr != null)
                            userInfo.HaveHKDelayRight = (atr.Value == "YES");


                        atr = root.Attributes["HKReal"];
                        if (atr != null)
                        {
                            userInfo.HaveHKRealTimeRight = (atr.Value == "YES");
                            if (userInfo.HaveHKRealTimeRight)
                                userInfo.HaveHKDelayRight = true;
                        }

                        atr = root.Attributes["SHLevel2"];
                        if (atr != null)
                            userInfo.HaveSHLevel2Right = (atr.Value == "YES");

                        atr = root.Attributes["SZLevel2"];
                        if (atr != null)
                            userInfo.HaveSZLevel2Right = (atr.Value == "YES");

                        atr = root.Attributes["InterbankBond"];
                        if (atr != null)
                            userInfo.HaveInterbankBondRight = (atr.Value == "YES");

                        atr = root.Attributes["ThirdBoardMarket"];
                        if (atr != null)
                            userInfo.HaveThirdBoardMarketRight = (atr.Value == "YES");

                        atr = root.Attributes["IndexChinaBond"];
                        if (atr != null)
                            userInfo.HaveIndexChinaBondRight = (atr.Value == "YES");

                        atr = root.Attributes["IndexFuture"];
                        if (atr != null)
                            userInfo.HaveIndexFutureRight = (atr.Value == "YES");

                        atr = root.Attributes["WebF10Address"];
                        if (atr != null)
                            userInfo.WebF10Address = atr.Value;


                        atr = root.Attributes["IsVIPTerminal"];
                        if (atr != null)
                            userInfo.IsVIPTerminal = (atr.Value == "YES");

                        atr = root.Attributes["IsVerifyFromSrv"];
                        if (atr != null)
                            userInfo.IsVerifyFromSrv = (atr.Value == "YES");
                        
                    }
                }
                catch (Exception ex)
                {
                    LogUtilities.LogMessage("Load SysCfg Error : " + ex.Message);
                }
            }
            return userInfo;
        }
    }
}
