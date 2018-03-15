/************************************************************************************\
*                                                                                    *
* SecurityService.cs -  Security service functions, types, and definitions.          *
*                                                                                    *
*               Version 1.00 ��                                                      *
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
    /// ֤ȯ����
    /// </summary>
    public class EMSecurityService : EMGlobalDataService
    {
        /// <summary>
        /// ����֤ȯ����
        /// </summary>
        public EMSecurityService()
        {
        }

        private static Dictionary<String, KwItem> kwItems = new Dictionary<String, KwItem>();
        /// <summary>
        /// �����
        /// </summary>
        public static Dictionary<String, KwItem> KwItems
        {
            get { return EMSecurityService.kwItems; }
            set { EMSecurityService.kwItems = value; }
        }

        private static Dictionary<int, KwItem> kwItems2 = new Dictionary<int, KwItem>();

        /// <summary>
        /// �����
        /// </summary>
        public static Dictionary<int, KwItem> KwItems2
        {
            get { return EMSecurityService.kwItems2; }
            set { EMSecurityService.kwItems2 = value; }
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="loadAll">�Ƿ�ȫ�����ر��</param>
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
        /// ���������ȡ��Ʊ
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
        /// ��ȡ���õĹ�Ʊ
        /// </summary>
        /// <param name="items">��Ʊ�б�</param>
        /// <returns>���õĹ�Ʊ</returns>
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
        /// ��ȡ���
        /// </summary>
        /// <returns>���</returns>
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
                        marketName = "��֤A��";
                    }
                    else if (marketCode == "2")
                    {
                        marketName = "��֤B��";
                    }
                    else if (marketCode == "3")
                    {
                        marketName = "��֤A��";
                    }
                    else if (marketCode == "4")
                    {
                        marketName = "��֤B��";
                    }
                    else if (marketCode == "13")
                    {
                        marketName = "��֤����";
                    }
                    else if (marketCode == "14")
                    {
                        marketName = "��֤����";
                    }
                    else if (marketCode == "15")
                    {
                        marketName = "�����г��ͻ���";
                    }
                    else if (marketCode == "16")
                    {
                        marketName = "����ͻ���";
                    }
                    else if (marketCode == "22")
                    {
                        marketName = "ȯ���ʹ�";
                    }
                    else if (marketCode == "23")
                    {
                        marketName = "����";
                    }
                    else if (marketCode == "24")
                    {
                        marketName = "�������";
                    }
                    else if (marketCode == "25")
                    {
                        marketName = "����";
                    }
                    else if (marketCode == "26")
                    {
                        marketName = "����˽ļ";
                    }
                    else if (marketCode == "33")
                    {
                        marketName = "�����ͻ���";
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
                        marketName = "����"; //B1W.IR
                    }
                    else if (marketCode == "21")
                    {
                        marketName = ""; //DIBO1.IR
                    }
                    else if (marketCode == "27")
                    {
                        marketName = "�����ڻ�";
                    }
                    else if (marketCode == "34")
                    {
                        marketName = "";  //��ҹ14��
                    }
                    else if (marketCode == "101")
                    {
                        marketName = "����";
                    }
                    else if (marketCode == "11")
                    {
                        marketName = "��Ʒ�ڻ�";  //�߲�
                    }
                    else if (marketCode == "12")
                    {
                        marketName = "��ָ�ڻ�";
                    }
                    else if (marketCode == "50")
                    {
                        marketName = "�����";  //����
                    }
                    else if (marketCode == "51")
                    {
                        marketName = "��ծ�ڻ�";
                    }
                    else if (marketCode == "52")
                    {
                        marketName = "�Ϻ��ڻ�������";  //��ͭ
                    }
                    else if (marketCode == "68")
                    {
                        marketName = "�Ϻ��ڻ�������";  //��
                    }
                    else if (marketCode == "80")
                    {
                        marketName = "֣����Ʒ������";  //֣��
                    }
                    else if (marketCode == "81")
                    {
                        marketName = "������Ʒ������";  //֣��
                    }
                    else if (marketCode == "83")
                    {
                        marketName = "��֤������";  //DZDCE001.DCE
                    }
                    else if (marketCode == "55")
                    {
                        marketName = "�����Ϳ���ʽ����";  //50ETF
                    }
                    else if (marketCode == "56")
                    {
                        marketName = "��֤��Ȩ";  //10000010.SH
                    }
                    else if (marketCode == "59")
                    {
                        marketName = "�۹�";
                    }
                    else if (marketCode == "60")
                    {
                        marketName = "�۹���Ȩ";  //12108.HK
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
                    sb.AppendLine("��������:" + typename);
                    sb.AppendLine("���ʹ���:" + marketCode);
                    sb.AppendLine("��������:" + marketName);
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
        /// ���̾�������ת�����ַ���
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
        * Note: ����������� ����ͨ��
        * args: ctLast,����ʱ���ݵ�ʱ���������˴Ӹ�ʱ�����ʼ����
        *		szCode,����ʱ���������룬
        *		nRequestId : 0=������1=ȫ����2=ָ�����ȫ��
        *		IsImportant���Ƿ�Ϊ��Ҫ���
        * void��
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
        * Note: �������� sdata/china/BKZSDYGX������ͨ��
        * args: ctLast,����ʱ���ݵ�ʱ���������˴Ӹ�ʱ�����ʼ����
        * void��
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
        * Note: ��������IND_MAININDEX�ļ����ݡ�����ͨ��
        * args: ctLast,����ʱ���ݵ�ʱ���������˴Ӹ�ʱ�����ʼ����
        * void��
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
        * Note: ��������AREAINDGNLISH�ļ����ݡ�����ͨ��
        * args: ctLast,����ʱ���ݵ�ʱ���������˴Ӹ�ʱ�����ʼ����
        * void��
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
        * Note: ��������AREAINDGNLISH_GN�ļ����ݡ�����ͨ��
        * args: ctLast,����ʱ���ݵ�ʱ���������˴Ӹ�ʱ�����ʼ����
        * void��
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
        * Note: ���Ŀ¼��URL�ļ����ݻ�ȡ������ͨ��
        * args: ctLast,����ʱ���ݵ�ʱ���������˴Ӹ�ʱ�����ʼ����
        * void��
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
        * Note: ���Ŀ¼��URL�ļ�����ɾ����ȡ ����ͨ��
        * args: ctLast,����ʱ���ݵ�ʱ���������˴Ӹ�ʱ�����ʼ����
        * void��
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
        * Note: ���Ŀ¼��TYPE�ļ����ݸ��� ����ͨ��
        * args: ctLast,����ʱ���ݵ�ʱ���������˴Ӹ�ʱ�����ʼ����
        * void��
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
        * Note: ����FuncompCode ����ͨ��
        * args: ctLast,����ʱ���ݵ�ʱ���������˴Ӹ�ʱ�����ʼ����
        * void��
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
        * Note: ����״̬ ����ͨ��
        * void��
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
