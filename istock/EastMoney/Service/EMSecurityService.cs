/************************************************************************************\
*                                                                                    *
* SecurityService.cs -  Security service functions, types, and definitions.          *
*                                                                                    *
*               Version 1.00 ★                                                      *
*                                                                                    *
*               Copyright (c) 2010-2014, Server. All rights reserved.                *
*               Created by Todd.                                                     *
*                                                                                    *
*************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data;
using Newtonsoft.Json;

namespace OwLib
{
    /// <summary>
    /// 证券服务
    /// </summary>
    public class EMSecurityService : EMGlobalDataService
    {
        /// <summary>
        /// 创建证券服务
        /// </summary>
        public EMSecurityService()
        {
        }

        private static Dictionary<String, KwItem> kwItems = new Dictionary<String, KwItem>();
        /// <summary>
        /// 码表项
        /// </summary>
        public static Dictionary<String, KwItem> KwItems
        {
            get { return EMSecurityService.kwItems; }
            set { EMSecurityService.kwItems = value; }
        }

        private static Dictionary<int, KwItem> kwItems2 = new Dictionary<int, KwItem>();

        /// <summary>
        /// 码表项
        /// </summary>
        public static Dictionary<int, KwItem> KwItems2
        {
            get { return EMSecurityService.kwItems2; }
            set { EMSecurityService.kwItems2 = value; }
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="loadAll">是否全部加载标记</param>
        public static bool Load()
        {
            String sPath = Application.StartupPath + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            if (!File.Exists(sPath))
            {
                List<KwItem> items = DataCenter.EMSecurityService.GetKwItems();
                Dictionary<String, KwItem> availableItems = EMSecurityService.GetAvailableItems(items);
                StringBuilder sb = new StringBuilder();
                foreach (String key in availableItems.Keys)
                {
                    kwItems2[availableItems[key].Innercode] = availableItems[key];
                    sb.Append(key + "," + availableItems[key].Name + "\r\n");
                }
                CFileA.Write(Application.StartupPath + "\\codes.txt", sb.ToString());
                EMSecurityService.KwItems = availableItems;
                CFileA.Write(sPath, JsonConvert.SerializeObject(EMSecurityService.KwItems));
                //SecurityService.ImportSecurities(EMSecurityService.KwItems);
                return false;
            }
            else
            {
                EMSecurityService.KwItems = JsonConvert.DeserializeObject<Dictionary<String, KwItem>>(File.ReadAllText(sPath, Encoding.Default));
                foreach (String key in KwItems.Keys)
                {
                    kwItems2[KwItems[key].Innercode] = KwItems[key];
                }
                return true;
            }
        }

        /// <summary>
        /// 根据内码获取股票
        /// </summary>
        /// <param name="innerCode"></param>
        /// <returns></returns>
        public static KwItem GetKwItemByInnerCode(int innerCode)
        {
            if (kwItems2.ContainsKey(innerCode))
            {
                return kwItems2[innerCode];
            }
            return null;
        }

        /// <summary>
        /// 获取可用的股票
        /// </summary>
        /// <param name="items">股票列表</param>
        /// <returns>可用的股票</returns>
        public static Dictionary<String, KwItem> GetAvailableItems(List<KwItem> items)
        {
            Dictionary<String, KwItem> availableItems = new Dictionary<String, KwItem>();
            int itemsSize = items.Count;
            for (int i = 0; i < itemsSize; i++)
            {
                KwItem item = items[i];
                if (item.Type == 1 || item.Type == 2 || item.Code == "000001.SH"
                    || item.Code == "399001.SZ" || item.Code == "399005.SZ" 
                    || item.Code == "399006.SZ")
                {
                    availableItems[item.Code] = item;
                }
            }
            return availableItems;
        }

        /// <summary>
        /// 获取码表
        /// </summary>
        /// <returns>码表</returns>
        public List<KwItem> GetKwItems()
        {
            String code = "";
            long time = 0;
            List<KwItem> items = new List<KwItem>();
            Dictionary<String, List<KwItem>> items2 = new Dictionary<String, List<KwItem>>();
            while (true)
            {
                DMRetOutput dm1 = UpdateKeySpriteData(time, code, 0, true);
                KwItem lastItem = null;
                using (MemoryStream ms = new MemoryStream(dm1.ptr))
                {
                    using (BinaryReader br = new BinaryReader(ms))
                    {
                        int size = dm1.size;
                        for (int i = 0; i < size; i++)
                        {
                            KwItem item = new KwItem();
                            item.Code = GetBytesString(br, 20);
                            item.Name = GetBytesString(br, 100);
                            item.Pingyin = GetBytesString(br, 100);
                            item.Marketcode = GetBytesString(br, 40);
                            item.State = br.ReadInt32();
                            item.Innercode = br.ReadInt32();
                            item.Type = br.ReadInt32();
                            item.Timestamp = br.ReadInt64();
                            items.Add(item);
                            if (!items2.ContainsKey(item.Type.ToString()))
                            {
                                items2[item.Type.ToString()] = new List<KwItem>();
                            }
                            items2[item.Type.ToString()].Add(item);
                            lastItem = item;
                        }
                    }
                }
                if (!dm1.last)
                {
                    if (lastItem != null)
                    {
                        code = lastItem.Code;
                        time = lastItem.Timestamp;
                    }
                }
                if (dm1.last)
                {
                    break;
                }
            }
            //code = "";
            //time = 0;
            //while (true)
            //{
            //    DMRetOutput dm1 = UpdateKeySpriteData(time, code, 0, false);
            //    KwItem lastItem = null;
            //    using (MemoryStream ms = new MemoryStream(dm1.ptr))
            //    {
            //        using (BinaryReader br = new BinaryReader(ms))
            //        {
            //            int size = dm1.size;
            //            for (int i = 0; i < size; i++)
            //            {
            //                KwItem item = new KwItem();
            //                item.Code = GetBytesString(br, 20);
            //                item.Name = GetBytesString(br, 100);
            //                item.Pingyin = GetBytesString(br, 100);
            //                item.Marketcode = GetBytesString(br, 40);
            //                item.State = br.ReadInt32();
            //                item.Innercode = br.ReadInt32();
            //                item.Type = br.ReadInt32();
            //                item.Timestamp = br.ReadInt64();
            //                items.Add(item);
            //                if (!items2.ContainsKey(item.Type.ToString()))
            //                {
            //                    items2[item.Type.ToString()] = new List<KwItem>();
            //                }
            //                items2[item.Type.ToString()].Add(item);
            //                lastItem = item;
            //            }
            //        }
            //    }
            //    if (!dm1.last)
            //    {
            //        if (lastItem != null)
            //        {
            //            code = lastItem.Code;
            //            time = lastItem.Timestamp;
            //        }
            //    }
            //    if (dm1.last)
            //    {
            //        break;
            //    }
            //}
            DataTable dt = AnalysisDMRetOutput(UpdateTypeFile(0));
            foreach (DataRow row in dt.Rows)
            {
                String typename = row[0].ToString();
                String marketCode = row[1].ToString();
                String marketName = "";
                if (items2.ContainsKey(marketCode))
                {
                    List<KwItem> items3 = items2[marketCode];
                    if (marketCode == "1")
                    {
                        marketName = "上证A股";
                    }
                    else if (marketCode == "2")
                    {
                        marketName = "上证B股";
                    }
                    else if (marketCode == "3")
                    {
                        marketName = "深证A股";
                    }
                    else if (marketCode == "4")
                    {
                        marketName = "深证B股";
                    }
                    else if (marketCode == "13")
                    {
                        marketName = "上证基金";
                    }
                    else if (marketCode == "14")
                    {
                        marketName = "深证基金";
                    }
                    else if (marketCode == "15")
                    {
                        marketName = "货币市场型基金";
                    }
                    else if (marketCode == "16")
                    {
                        marketName = "混合型基金";
                    }
                    else if (marketCode == "22")
                    {
                        marketName = "券商资管";
                    }
                    else if (marketCode == "23")
                    {
                        marketName = "信托";
                    }
                    else if (marketCode == "24")
                    {
                        marketName = "银行理财";
                    }
                    else if (marketCode == "25")
                    {
                        marketName = "信托";
                    }
                    else if (marketCode == "26")
                    {
                        marketName = "阳光私募";
                    }
                    else if (marketCode == "33")
                    {
                        marketName = "货币型基金";
                    }
                    else if (marketCode == "18")
                    {
                        marketName = ""; //R09M.IB
                    }
                    else if (marketCode == "19")
                    {
                        marketName = ""; //131800.SH
                    }
                    else if (marketCode == "20")
                    {
                        marketName = "利率"; //B1W.IR
                    }
                    else if (marketCode == "21")
                    {
                        marketName = ""; //DIBO1.IR
                    }
                    else if (marketCode == "27")
                    {
                        marketName = "货币期货";
                    }
                    else if (marketCode == "34")
                    {
                        marketName = "";  //隔夜14日
                    }
                    else if (marketCode == "101")
                    {
                        marketName = "美股";
                    }
                    else if (marketCode == "11")
                    {
                        marketName = "商品期货";  //线材
                    }
                    else if (marketCode == "12")
                    {
                        marketName = "股指期货";
                    }
                    else if (marketCode == "50")
                    {
                        marketName = "贵金属";  //沪金
                    }
                    else if (marketCode == "51")
                    {
                        marketName = "国债期货";
                    }
                    else if (marketCode == "52")
                    {
                        marketName = "上海期货交易所";  //沪铜
                    }
                    else if (marketCode == "68")
                    {
                        marketName = "上海期货交易所";  //橡胶
                    }
                    else if (marketCode == "80")
                    {
                        marketName = "郑州商品交易所";  //郑棉
                    }
                    else if (marketCode == "81")
                    {
                        marketName = "大连商品交易所";  //郑棉
                    }
                    else if (marketCode == "83")
                    {
                        marketName = "东证大商所";  //DZDCE001.DCE
                    }
                    else if (marketCode == "55")
                    {
                        marketName = "交易型开放式基金";  //50ETF
                    }
                    else if (marketCode == "56")
                    {
                        marketName = "上证期权";  //10000010.SH
                    }
                    else if (marketCode == "59")
                    {
                        marketName = "港股";
                    }
                    else if (marketCode == "60")
                    {
                        marketName = "港股期权";  //12108.HK
                    }
                    else if (marketCode == "61")
                    {
                        marketName = "";  //69229.HK
                    }
                    else if (marketCode == "62")
                    {
                        marketName = "";  //66186.HK
                    }
                    else if (marketCode == "63")
                    {
                        marketName = "";  //M1801C2850.DCE
                    }
                    else if (marketCode == "64")
                    {
                        marketName = "";  //M1712P3100.DCE
                    }
                    else if (marketCode == "82")
                    {
                        marketName = "";  //890019.GZEE
                    }
                    else if (marketCode == "5")
                    {
                        marketName = "";  //832158.OC
                    }
                    else if (marketCode == "6")
                    {
                        marketName = "";  //400040.OC
                    }
                    else if (marketCode == "121")
                    {
                        marketName = "";  //GBPCNY3WF.IB
                    }
                    else if (marketCode == "122")
                    {
                        marketName = "";  //GBPUSD.IB
                    }
                    else if (marketCode == "124")
                    {
                        marketName = "";  //USDDKK.FX
                    }
                    else if (marketCode == "111")
                    {
                        marketName = "";  //QI14N.CMX
                    }
                    else if (marketCode == "112")
                    {
                        marketName = "";  //UL14H.CBT
                    }
                    else if (marketCode == "113")
                    {
                        marketName = "";  //RT13V.SG
                    }
                    else if (marketCode == "114")
                    {
                        marketName = "";  //LZNT.LME
                    }
                    else if (marketCode == "115")
                    {
                        marketName = "";  //CPRC.LME
                    }
                    else if (marketCode == "53")
                    {
                        marketName = "";  //BNI.BCE
                    }
                    else if (marketCode == "54")
                    {
                        marketName = "";  //CO.FME
                    }
                    else if (marketCode == "71")
                    {
                        marketName = "";  //01566.HK
                    }
                    else if (marketCode == "72")
                    {
                        marketName = "";  //08306.HK
                    }
                    else if (marketCode == "73")
                    {
                        marketName = "";  //02301.HK
                    }
                    else if (marketCode == "74")
                    {
                        marketName = "";  //05879.HK
                    }
                    else if (marketCode == "79")
                    {
                        marketName = "";  //00806.HK
                    }
                    else if (marketCode == "10")
                    {
                        marketName = "";  //110020.SH
                    }
                    else if (marketCode == "8")
                    {
                        marketName = "";  //031232001.IB
                    }
                    else if (marketCode == "9")
                    {
                        marketName = "";  //122721.SH
                    }
                    else if (marketCode == "147")
                    {
                        marketName = "";  //SCSHK.HI
                    }
                    else if (marketCode == "17")
                    {
                        marketName = "";  //861030.EI
                    }
                    else if (marketCode == "28")
                    {
                        marketName = "";  //CN6005.SZ
                    }
                    else if (marketCode == "29")
                    {
                        marketName = "";  //CBD00133.CS
                    }
                    else if (marketCode == "30")
                    {
                        marketName = "";  //H20615.CSI
                    }
                    else if (marketCode == "31")
                    {
                        marketName = "";  //H30373.CSI
                    }
                    else if (marketCode == "35")
                    {
                        marketName = "";  //HSI.HI
                    }
                    else if (marketCode == "36")
                    {
                        marketName = "";  //JKSE.GI
                    }
                    else if (marketCode == "37")
                    {
                        marketName = "";  //KLSE.GI
                    }
                    else if (marketCode == "38")
                    {
                        marketName = "";  //KS11.GI
                    }
                    else if (marketCode == "39")
                    {
                        marketName = "";  //N225.GI
                    }
                    else if (marketCode == "40")
                    {
                        marketName = "";  //PSI.GI
                    }
                    else if (marketCode == "41")
                    {
                        marketName = "";  //SENSEX.GI
                    }
                    else if (marketCode == "42")
                    {
                        marketName = "";  //STI.GI
                    }
                    else if (marketCode == "43")
                    {
                        marketName = "";  //TWII.TW
                    }
                    else if (marketCode == "44")
                    {
                        marketName = "";  //NZ50.GI
                    }
                    else if (marketCode == "45")
                    {
                        marketName = "";  //MXX.GI
                    }
                    else if (marketCode == "46")
                    {
                        marketName = "";  //AEX.GI
                    }
                    else if (marketCode == "47")
                    {
                        marketName = "";  //ATX.GI
                    }
                    else if (marketCode == "48")
                    {
                        marketName = "";  //OSEAX.GI
                    }
                    else if (marketCode == "49")
                    {
                        marketName = "";  //RTS.GI
                    }
                    else if (marketCode == "65")
                    {
                        marketName = "";  //HSCAHSIDV.HI
                    }
                    else if (marketCode == "66")
                    {
                        marketName = "";  //CSCSHQ.GI
                    }
                    else if (marketCode == "67")
                    {
                        marketName = "";  //AS51.GI
                    }
                    else if (marketCode == "69")
                    {
                        marketName = "";  //CES280.CES
                    }
                    else if (marketCode == "78")
                    {
                        marketName = "";  //CRB.GI
                    }
                    else if (marketCode == "78")
                    {
                        marketName = "";  //CRB.GI
                    }
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("大类名称:" + typename);
                    sb.AppendLine("类型代码:" + marketCode);
                    sb.AppendLine("类型名称:" + marketName);
                    for (int i = 0; i < items3.Count; i++)
                    {
                        sb.AppendLine(items3[i].ToString());
                    }
                    //CFileA.Write(Application.StartupPath+"\\details\\"+ marketCode + ".txt", sb.ToString());
                }
                else
                {
                    int a = 0;
                }
                Console.WriteLine("1");
            }
            Console.WriteLine("1");
            return items;
        }

        /// <summary>
        /// 键盘精灵数据转换成字符串
        /// </summary>
        /// <returns></returns>
        public String KwItemsToString()
        {
            StringBuilder sb = new StringBuilder(1024000);
            foreach (KeyValuePair<String, KwItem> item in EMSecurityService.KwItems)
            {
                sb.AppendLine(item.Value.ToString());
            }
            return sb.ToString();
        }

        /**************************************************************************
        * Note: 请求码表数据 测试通过
        * args: ctLast,请求时传递的时间戳，服务端从该时间戳开始数据
        *		szCode,请求时传递码表代码，
        *		nRequestId : 0=增量；1=全量；2=指定类别全量
        *		IsImportant：是否为重要板块
        * void：
        *
        **************************************************************************/
        public DMRetOutput UpdateKeySpriteData(long ctLast, String szCode, int nRequestId, bool bIsImportant)
        {
            DMReqInput reqInput = new DMReqInput();
            reqInput.size = 20;
            if (bIsImportant)
            {
                reqInput.itemid = SDATA_TYPE_KEYSPRITE_IMPORTANT;
            }
            else
            {
                reqInput.itemid = SDATA_TYPE_KEYSPRITE_NOTIMPORTANT;
            }

            reqInput.timestamp = ctLast;
            reqInput.requestid = nRequestId;
            if (szCode == null && szCode.Length > 0)
            {
                szCode = "";
            }
            reqInput.ptr = GetStringBytes(szCode, 20);
            DMRetOutput reqOutput = GetDMRet(reqInput);
            return reqOutput;
        }

        /**************************************************************************
        * Note: 请求数据 sdata/china/BKZSDYGX　测试通过
        * args: ctLast,请求时传递的时间戳，服务端从该时间戳开始数据
        * void：
        *
        **************************************************************************/
        public DMRetOutput UpdateBKZSDYGX(long nTime)
        {
            DMReqInput reqInput = new DMReqInput(); ;
            reqInput.itemid = SDATA_TYPE_BKZSDYGX;
            reqInput.timestamp = nTime;
            reqInput.requestid = 1;
            DMRetOutput reqOutput = GetDMRet(reqInput);
            return reqOutput;
        }

        /**************************************************************************
        * Note: 请求行情IND_MAININDEX文件数据　测试通过
        * args: ctLast,请求时传递的时间戳，服务端从该时间戳开始数据
        * void：
        *
        **************************************************************************/
        public DMRetOutput UpdateMAININDEX(long nTime)
        {
            DMReqInput reqInput = new DMReqInput();
            reqInput.itemid = SDATA_TYPE_MAININDEX;
            reqInput.timestamp = nTime;
            reqInput.requestid = 1;
            DMRetOutput reqOutput = GetDMRet(reqInput);
            return reqOutput;
        }

        /**************************************************************************
        * Note: 请求行情AREAINDGNLISH文件数据　测试通过
        * args: ctLast,请求时传递的时间戳，服务端从该时间戳开始数据
        * void：
        *
        **************************************************************************/
        public DMRetOutput UpdateAREAINDGNLISH(long nTime)
        {
            DMReqInput reqInput = new DMReqInput();
            reqInput.itemid = SDATA_TYPE_AREAINDGNLISH;
            reqInput.timestamp = nTime;
            reqInput.requestid = 1;
            DMRetOutput reqOutput = GetDMRet(reqInput);
            return reqOutput;
        }

        /**************************************************************************
        * Note: 请求行情AREAINDGNLISH_GN文件数据　测试通过
        * args: ctLast,请求时传递的时间戳，服务端从该时间戳开始数据
        * void：
        *
        **************************************************************************/
        public DMRetOutput UpdateAREAINDGNLISH_GN(long nTime)
        {
            DMReqInput reqInput = new DMReqInput();
            reqInput.itemid = SDATA_TYPE_AREAINDGNLISH_GN;
            reqInput.timestamp = nTime;
            reqInput.requestid = 1;
            DMRetOutput reqOutput = GetDMRet(reqInput);
            return reqOutput;
        }

        /**************************************************************************
        * Note: 码表目录下URL文件数据获取　测试通过
        * args: ctLast,请求时传递的时间戳，服务端从该时间戳开始数据
        * void：
        *
        **************************************************************************/
        public DMRetOutput UpdateUrlFile(long ctLast)
        {
            DMReqInput reqInput = new DMReqInput();
            reqInput.itemid = SDATA_TYPE_URL;
            reqInput.timestamp = ctLast;
            reqInput.requestid = 1;
            DMRetOutput reqOutput = GetDMRet(reqInput);
            return reqOutput;
        }

        /**************************************************************************
        * Note: 码表目录下URL文件数据删除获取 测试通过
        * args: ctLast,请求时传递的时间戳，服务端从该时间戳开始数据
        * void：
        *
        **************************************************************************/
        public DMRetOutput UpdateUrlFileDelete(long ctLast)
        {
            DMReqInput reqInput = new DMReqInput();
            reqInput.itemid = SDATA_TYPE_URL_DELETE;
            reqInput.timestamp = ctLast;
            reqInput.requestid = 1;
            DMRetOutput reqOutput = GetDMRet(reqInput);
            return reqOutput;
        }

        /**************************************************************************
        * Note: 码表目录下TYPE文件数据更新 测试通过
        * args: ctLast,请求时传递的时间戳，服务端从该时间戳开始数据
        * void：
        *
        **************************************************************************/
        public DMRetOutput UpdateTypeFile(long nTime)
        {
            DMReqInput reqInput = new DMReqInput();
            reqInput.itemid = SDATA_TYPE_TYPE;
            reqInput.timestamp = nTime;
            reqInput.requestid = 1;
            DMRetOutput reqOutput = GetDMRet(reqInput);
            return reqOutput;
        }


        /**************************************************************************
        * Note: 更新FuncompCode 测试通过
        * args: ctLast,请求时传递的时间戳，服务端从该时间戳开始数据
        * void：
        *
        **************************************************************************/
        public DMRetOutput UpdateFuncompCode(long nTime)
        {
            DMReqInput reqInput = new DMReqInput();
            reqInput.itemid = SDATA_TYPE_FUNDCOMPCODE;
            reqInput.timestamp = nTime;
            reqInput.requestid = 1;
            DMRetOutput reqOutput = GetDMRet(reqInput);
            return reqOutput;
        }

        /**************************************************************************
        * Note: 更新状态 测试通过
        * void：
        *
        **************************************************************************/
        public DMRetOutput UpdateState()
        {
            DMReqInput reqInput = new DMReqInput();
            reqInput.itemid = 997;
            reqInput.timestamp = 0;
            reqInput.requestid = 1;
            DMRetOutput reqOutput = GetDMRet(reqInput);
            return reqOutput;
        }
    }
}
