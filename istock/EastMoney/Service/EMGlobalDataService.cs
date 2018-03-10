using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Data;

namespace OwLib
{
    /// <summary>
    /// HTTP简单服务
    /// </summary>
    public class EMGlobalDataService
    {
        public const int SDATA_TYPE_KEYSPRITE_IMPORTANT = 101;//证券代码表(重要)
        public const int SDATA_TYPE_KEYSPRITE_DELETE_IMPT = 102;//证券代码删除表(重要)
        public const int SDATA_TYPE_KEYSPRITE_NOTIMPORTANT = 103;//证券代码表(非重要)
        public const int SDATA_TYPE_KEYSPRITE_DELETE_NOTIMPT = 104;//证券代码删除表(非重要)
        public const int SDATA_TYPE_BLOCKDATATREE = 105;//板块表
        public const int SDATA_TYPE_BLOCKTREE_DELETE = 106;//板块删除表
        public const int SDATA_TYPE_BLOCKDETAIL_IMPT = 107;//板块明细表（重要）
        public const int SDATA_TYPE_BLOCKDETAIL_DELETE_IMPT = 108;//板块明细删除表（重要）
        public const int SDATA_TYPE_BLOCKDETAIL_NOTIMPT = 109;//板块明细表（非重要）
        public const int SDATA_TYPE_BLOCKDETAIL_DELETE_NOTIMPT = 110;//板块明细删除表（非重要）
        public const int SDATA_TYPE_LOGIN = 10;//用户登录时请求加载数据
        public const int SDATA_TYPE_RUNING = 20;//登录成功后请求加载数据;

        public const int SDATA_TYPE_BKZSDYGX = 201;
        public const int SDATA_TYPE_MAININDEX = 203;
        public const int SDATA_TYPE_AREAINDGNLISH = 205;
        public const int SDATA_TYPE_AREAINDGNLISH_GN = 207;
        public const int SDATA_TYPE_URL = 301;
        public const int SDATA_TYPE_URL_DELETE = 302;
        public const int SDATA_TYPE_TYPE = 303;
        public const int SDATA_TYPE_FUNDCOMPCODE = 305;
        public const int SDATA_TYPE_STATE = 998;

        /*获取数据*/
        /// <summary>
        /// 分析返回数据
        /// </summary>
        /// <param name="retOutput">返回数据</param>
        public DataTable AnalysisDMRetOutput(DMRetOutput retOutput)
        {
            DataTable result = new DataTable();
            if (retOutput.itemid == SDATA_TYPE_KEYSPRITE_IMPORTANT
            || retOutput.itemid == SDATA_TYPE_KEYSPRITE_NOTIMPORTANT)
            {
                // Generate return table.
                result.Columns.Add("code");
                result.Columns.Add("name");
                result.Columns.Add("pingyin");
                result.Columns.Add("marketcode");
                result.Columns.Add("state");
                result.Columns.Add("innercode");
                result.Columns.Add("type");
                result.Columns.Add("timestamp");
                if (retOutput.ptr != null)
                {
                    List<DMSecuItem> items = new List<DMSecuItem>();
                    using (MemoryStream ms = new MemoryStream(retOutput.ptr))
                    {
                        using (BinaryReader br = new BinaryReader(ms))
                        {
                            int size = retOutput.size;

                            for (int i = 0; i < size; i++)
                            {
                                DMSecuItem item = new DMSecuItem();
                                item.code = GetBytesString(br, 20);
                                item.name = GetBytesString(br, 100);
                                item.pingyin = GetBytesString(br, 100);
                                item.marketcode = GetBytesString(br, 40);
                                item.state = br.ReadInt32();
                                item.innercode = br.ReadInt32();
                                item.type = br.ReadInt32();
                                item.timestamp = br.ReadInt64();
                                items.Add(item);
                            }
                            for (int i = 0; i < size; i++)
                            {
                                DataRow r = result.NewRow();
                                r[0] = items[i].code;
                                r[1] = items[i].name;
                                r[2] = items[i].pingyin;
                                r[3] = items[i].marketcode;
                                r[4] = items[i].state;
                                r[5] = items[i].innercode;
                                r[6] = items[i].type;
                                r[7] = items[i].timestamp;
                                result.Rows.Add(r);
                            }

                        }
                    }
                }
            }
            else if (retOutput.itemid == SDATA_TYPE_KEYSPRITE_DELETE_IMPT
        || retOutput.itemid == SDATA_TYPE_KEYSPRITE_DELETE_NOTIMPT)
            {
                // Generate return table.
                result.Columns.Add("code");
                result.Columns.Add("timestamp");
                if (retOutput.ptr != null)
                {
                    List<DMSecuItemDelete> items = new List<DMSecuItemDelete>();
                    using (MemoryStream ms = new MemoryStream(retOutput.ptr))
                    {
                        using (BinaryReader br = new BinaryReader(ms))
                        {
                            int size = retOutput.size;
                            for (int i = 0; i < size; i++)
                            {
                                DMSecuItemDelete item = new DMSecuItemDelete();
                                item.code = GetBytesString(br, 20);
                                item.timestamp = br.ReadInt64();
                                items.Add(item);
                            }
                            for (int i = 0; i < size; i++)
                            {
                                DataRow r = result.NewRow();
                                r[0] = items[i].code;
                                r[1] = items[i].timestamp;
                                result.Rows.Add(r);
                            }
                        }
                    }
                }
            }
            else if (retOutput.itemid == SDATA_TYPE_BLOCKDATATREE)
            {
                // Generate return table.
                result.Columns.Add("code");
                result.Columns.Add("name");
                result.Columns.Add("parentcode");
                result.Columns.Add("typcode");
                result.Columns.Add("typname");
                result.Columns.Add("tmpl");
                result.Columns.Add("order");
                result.Columns.Add("timestamp");
                if (retOutput.ptr != null)
                {
                    List<DMBlockItem> items = new List<DMBlockItem>();
                    using (MemoryStream ms = new MemoryStream(retOutput.ptr))
                    {
                        using (BinaryReader br = new BinaryReader(ms))
                        {
                            int size = retOutput.size;
                            for (int i = 0; i < size; i++)
                            {
                                DMBlockItem item = new DMBlockItem();
                                item.innerCode = GetBytesString(br, 16);
                                item.code = GetBytesString(br, 50);
                                item.name = GetBytesString(br, 200);
                                item.parentcode = GetBytesString(br, 50);
                                item.typcode = GetBytesString(br, 10);
                                item.typname = GetBytesString(br, 200);
                                item.tmpl = GetBytesString(br, 10);
                                item.order = br.ReadInt32();
                                item.timestamp = br.ReadInt64();
                                items.Add(item);
                            }
                            for (int i = 0; i < size; i++)
                            {
                                DataRow r = result.NewRow();
                                r[0] = items[i].code;
                                r[1] = items[i].name;
                                r[2] = items[i].parentcode;
                                r[3] = items[i].typcode;
                                r[4] = items[i].typname;
                                r[5] = items[i].tmpl;
                                r[6] = items[i].order;
                                r[7] = items[i].timestamp;
                                result.Rows.Add(r);
                            }
                        }
                    }
                }
            }
            else if (retOutput.itemid == SDATA_TYPE_BLOCKDETAIL_IMPT
       || retOutput.itemid == SDATA_TYPE_BLOCKDETAIL_NOTIMPT)
            {
                if (retOutput.requestid == 0 || retOutput.requestid == 1)
                {
                    // Generate return table.
                    result.Columns.Add("bkcode");
                    result.Columns.Add("code");
                    result.Columns.Add("innercode");
                    result.Columns.Add("timestamp");
                    if (retOutput.ptr != null)
                    {
                        List<DMBlockDetailItem> items = new List<DMBlockDetailItem>();
                        using (MemoryStream ms = new MemoryStream(retOutput.ptr))
                        {
                            using (BinaryReader br = new BinaryReader(ms))
                            {
                                br.ReadBytes(50);
                                int size = retOutput.size;
                                for (int i = 0; i < size; i++)
                                {
                                    DMBlockDetailItem item = new DMBlockDetailItem();
                                    item.bkcode = GetBytesString(br, 50);
                                    item.code = GetBytesString(br, 22);
                                    item.innercode = br.ReadInt32();
                                    item.timestamp = br.ReadInt64();
                                    items.Add(item);
                                }
                                for (int i = 0; i < size; i++)
                                {
                                    DataRow r = result.NewRow();
                                    r[0] = items[i].bkcode;
                                    r[1] = items[i].code;
                                    r[2] = items[i].innercode;
                                    r[3] = items[i].timestamp;
                                    result.Rows.Add(r);
                                }
                            }
                        }
                    }
                }
                else if (retOutput.requestid == 2)
                {
                    // Generate return table.
                    result.Columns.Add("code");
                    if (retOutput.ptr != null)
                    {
                        List<DMBlockData> items = new List<DMBlockData>();
                        using (MemoryStream ms = new MemoryStream(retOutput.ptr))
                        {
                            using (BinaryReader br = new BinaryReader(ms))
                            {
                                int size = retOutput.size;
                                for (int i = 0; i < size; i++)
                                {
                                    DMBlockData item = new DMBlockData();
                                    item.code = GetBytesString(br, 20);
                                    items.Add(item);
                                }
                                for (int i = 0; i < size; i++)
                                {
                                    DataRow r = result.NewRow();
                                    r[0] = items[i].code;

                                    result.Rows.Add(r);
                                }
                            }
                        }
                    }
                }
            }
            else if (retOutput.itemid == SDATA_TYPE_BKZSDYGX)
            {
                // Generate return table.
                result.Columns.Add("cPUBLISHCODE");
                result.Columns.Add("cSTR_EMCODE");
                result.Columns.Add("cSTR_LX");
                result.Columns.Add("innercode");
                if (retOutput.ptr != null)
                {
                    //SDM_IND_BKZSDYGX_TEST
                    List<SDM_IND_BKZSDYGX_TEST> items = new List<SDM_IND_BKZSDYGX_TEST>();
                    using (MemoryStream ms = new MemoryStream(retOutput.ptr))
                    {
                        using (BinaryReader br = new BinaryReader(ms))
                        {
                            int size = retOutput.size;
                            for (int i = 0; i < size; i++)
                            {
                                SDM_IND_BKZSDYGX_TEST item = new SDM_IND_BKZSDYGX_TEST();
                                item.cPUBLISHCODE = GetBytesString(br, 50);
                                item.cSTR_EMCODE = GetBytesString(br, 60);
                                item.cSTR_LX = GetBytesString(br, 22);
                                item.innerCode = br.ReadInt32();
                                items.Add(item);
                            }
                            for (int i = 0; i < size; i++)
                            {
                                DataRow r = result.NewRow();
                                r[0] = items[i].cPUBLISHCODE;
                                r[1] = items[i].cSTR_EMCODE;
                                r[2] = items[i].cSTR_LX;
                                r[3] = items[i].innerCode;
                                result.Rows.Add(r);
                            }
                        }
                    }
                }
            }
            else if (retOutput.itemid == SDATA_TYPE_MAININDEX)
            {
                // Generate return table.
                result.Columns.Add("nINT_PAIXU");
                result.Columns.Add("cSTR_EMCODE");
                if (retOutput.ptr != null)
                {
                    List<sIND_MAININDEX> items = new List<sIND_MAININDEX>();
                    using (MemoryStream ms = new MemoryStream(retOutput.ptr))
                    {
                        using (BinaryReader br = new BinaryReader(ms))
                        {
                            int size = retOutput.size;
                            for (int i = 0; i < size; i++)
                            {
                                sIND_MAININDEX item = new sIND_MAININDEX();
                                item.nINT_PAIXU = br.ReadInt32();
                                item.cSTR_EMCODE = GetBytesString(br, 60);
                                items.Add(item);
                            }

                            for (int i = 0; i < size; i++)
                            {
                                DataRow r = result.NewRow();
                                r[0] = items[i].nINT_PAIXU;
                                r[1] = items[i].cSTR_EMCODE;

                                result.Rows.Add(r);
                            }
                        }
                    }
                }
            }
            else if (retOutput.itemid == SDATA_TYPE_AREAINDGNLISH)
            {
                // Generate return table.
                result.Columns.Add("cSECURITYVARIETYCODE");
                result.Columns.Add("cSECURITYVARIETYCODEDC");
                result.Columns.Add("cSECURITYVARIETYCODEDY");
                if (retOutput.ptr != null)
                {
                    List<sDM_SPE_AREAINDGNLISH> items = new List<sDM_SPE_AREAINDGNLISH>();
                    using (MemoryStream ms = new MemoryStream(retOutput.ptr))
                    {
                        using (BinaryReader br = new BinaryReader(ms))
                        {
                            int size = retOutput.size;
                            for (int i = 0; i < size; i++)
                            {
                                sDM_SPE_AREAINDGNLISH item = new sDM_SPE_AREAINDGNLISH();
                                item.cSECURITYVARIETYCODE = GetBytesString(br, 50);
                                item.cSECURITYVARIETYCODEDC = GetBytesString(br, 50);
                                item.cSECURITYVARIETYCODEDY = GetBytesString(br, 50);
                                items.Add(item);
                            }
                            for (int i = 0; i < size; i++)
                            {
                                DataRow r = result.NewRow();
                                r[0] = items[i].cSECURITYVARIETYCODE;
                                r[1] = items[i].cSECURITYVARIETYCODEDC;
                                r[2] = items[i].cSECURITYVARIETYCODEDY;

                                result.Rows.Add(r);
                            }
                        }
                    }
                }
            }
            else if (retOutput.itemid == SDATA_TYPE_AREAINDGNLISH_GN)
            {

                // Generate return table.
                result.Columns.Add("cSECURITYVARIETYCODE");
                result.Columns.Add("cSECURITYVARIETYCODEGN");
                if (retOutput.ptr != null)
                {
                    List<sDM_SPE_AREAINDGNLISH_GN> items = new List<sDM_SPE_AREAINDGNLISH_GN>();
                    using (MemoryStream ms = new MemoryStream(retOutput.ptr))
                    {
                        using (BinaryReader br = new BinaryReader(ms))
                        {
                            int size = retOutput.size;
                            for (int i = 0; i < size; i++)
                            {
                                sDM_SPE_AREAINDGNLISH_GN item = new sDM_SPE_AREAINDGNLISH_GN();
                                item.cSECURITYVARIETYCODE = GetBytesString(br, 50);
                                item.cSECURITYVARIETYCODEGN = GetBytesString(br, 50);
                                items.Add(item);
                            }
                            for (int i = 0; i < size; i++)
                            {
                                DataRow r = result.NewRow();
                                r[0] = items[i].cSECURITYVARIETYCODE;
                                r[1] = items[i].cSECURITYVARIETYCODEGN;

                                result.Rows.Add(r);
                            }
                        }
                    }
                }
            }
            else if (retOutput.itemid == SDATA_TYPE_URL)
            {
                // Generate return table.
                result.Columns.Add("code");
                result.Columns.Add("url");
                result.Columns.Add("timestamp");
                if (retOutput.ptr != null)
                {
                    List<EMUrlItem> items = new List<EMUrlItem>();
                    using (MemoryStream ms = new MemoryStream(retOutput.ptr))
                    {
                        using (BinaryReader br = new BinaryReader(ms))
                        {
                            int size = retOutput.size;
                            for (int i = 0; i < size; i++)
                            {
                                EMUrlItem item = new EMUrlItem();
                                item.code = GetBytesString(br, 20);
                                item.url = GetBytesString(br, 200);
                                item.timestamp = br.ReadInt64();
                                items.Add(item);
                            }
                            for (int i = 0; i < size; i++)
                            {
                                DataRow r = result.NewRow();
                                r[0] = items[i].code;
                                r[1] = items[i].url;
                                r[2] = items[i].timestamp;
                                result.Rows.Add(r);
                            }
                        }
                    }
                }
            }
            else if (retOutput.itemid == SDATA_TYPE_URL_DELETE)
            {
                // Generate return table.
                result.Columns.Add("code");

                result.Columns.Add("timestamp");
                if (retOutput.ptr != null)
                {
                    List<UrlItemDel> items = new List<UrlItemDel>();
                    using (MemoryStream ms = new MemoryStream(retOutput.ptr))
                    {
                        using (BinaryReader br = new BinaryReader(ms))
                        {
                            int size = retOutput.size;
                            for (int i = 0; i < size; i++)
                            {
                                UrlItemDel item = new UrlItemDel();
                                item.code = GetBytesString(br, 10);
                                item.timestamp = br.ReadInt64();
                                items.Add(item);
                            }
                            for (int i = 0; i < size; i++)
                            {
                                DataRow r = result.NewRow();
                                r[0] = items[i].code;
                                r[1] = items[i].timestamp;
                                result.Rows.Add(r);
                            }
                        }
                    }
                }
            }
            else if (retOutput.itemid == SDATA_TYPE_TYPE)
            {
                // Generate return table.
                result.Columns.Add("type");
                result.Columns.Add("typeCode");
                result.Columns.Add("typeCount");
                if (retOutput.ptr != null)
                {
                    List<sDMTyItem> items = new List<sDMTyItem>();
                    using (MemoryStream ms = new MemoryStream(retOutput.ptr))
                    {
                        using (BinaryReader br = new BinaryReader(ms))
                        {
                            int size = retOutput.size;
                            for (int i = 0; i < size; i++)
                            {
                                sDMTyItem item = new sDMTyItem();
                                item.type = GetBytesString(br, 40);
                                item.typeCode = GetBytesString(br, 40);
                                item.typeCount = br.ReadInt32();
                                items.Add(item);
                            }
                            for (int i = 0; i < size; i++)
                            {
                                DataRow r = result.NewRow();
                                r[0] = items[i].type;
                                r[1] = items[i].typeCode;
                                r[2] = items[i].typeCount;

                                result.Rows.Add(r);
                            }
                        }
                    }
                }
            }
            else if (retOutput.itemid == SDATA_TYPE_FUNDCOMPCODE)
            {
                // Generate return table.
                result.Columns.Add("pulishcode");
                result.Columns.Add("compcode");
                if (retOutput.ptr != null)
                {
                    List<sDMFundCompcode> items = new List<sDMFundCompcode>();
                    using (MemoryStream ms = new MemoryStream(retOutput.ptr))
                    {
                        using (BinaryReader br = new BinaryReader(ms))
                        {
                            int size = retOutput.size;
                            for (int i = 0; i < size; i++)
                            {
                                sDMFundCompcode item = new sDMFundCompcode();
                                item.pulishcode = GetBytesString(br, 50);
                                item.compcode = GetBytesString(br, 10);
                                items.Add(item);
                            }
                            for (int i = 0; i < size; i++)
                            {
                                DataRow r = result.NewRow();
                                r[0] = items[i].pulishcode;
                                r[1] = items[i].compcode;

                                result.Rows.Add(r);
                            }
                        }
                    }
                }
            }
            else if (retOutput.itemid == SDATA_TYPE_STATE)
            {
                result.Columns.Add("nversion");
                result.Columns.Add("cversion");
                result.Columns.Add("nper");
                if (retOutput.ptr != null)
                {
                    using (MemoryStream ms = new MemoryStream(retOutput.ptr))
                    {
                        using (BinaryReader br = new BinaryReader(ms))
                        {
                            DataRow r = result.NewRow();
                            r[0] = br.ReadInt32();
                            r[1] = br.ReadInt32();
                            r[2] = br.ReadInt32();
                            result.Rows.Add(r);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 请求数据
        /// </summary>
        /// <param name="dmReq">请求结构</param>
        /// <returns>返回结构</returns>
        protected DMRetOutput GetDMRet(DMReqInput dmReq)
        {
            DMRetOutput dmRet = new DMRetOutput();
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    byte[] result = dmReq.Write(ms, bw);
                    using (MemoryStream ms2 = new MemoryStream(result))
                    {
                        using (BinaryReader br = new BinaryReader(ms2))
                        {
                            dmRet.Read(result, br);
                        }
                    }
                    result = null;
                }
            }
            if (dmRet.itemid == 0)
            {
                dmRet.itemid = dmReq.itemid;
            }
            return dmRet;
        }

        /// <summary>
        /// 获取字符串流
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="count">数量</param>
        /// <returns>字符串流</returns>
        protected byte[] GetStringBytes(String str, int count)
        {
            byte[] bytes = new byte[count];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = 0;
            }
            if (!String.IsNullOrEmpty(str))
            {
                byte[] strBytes = Encoding.Default.GetBytes(str);
                for (int i = 0; i < strBytes.Length; i++)
                {
                    bytes[i] = strBytes[i];
                }
            }
            return bytes;
        }

        /// <summary>
        /// 请求全局数据
        /// </summary>
        /// <param name="cmd">命令</param>
        /// <returns>数据</returns>
        public static byte[] RequestGlobalData(byte[] cmd)
        {
            try
            {
                byte[] result = DataCenter.DataQuery.NewQueryGlobalData(cmd) as byte[];
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="br">流</param>
        /// <param name="count">数量</param>
        /// <returns>字符串</returns>
        public static String GetBytesString(BinaryReader br, int count)
        {
            byte[] strBytes = br.ReadBytes(count);
            List<byte> readBytes = new List<byte>();
            for (int i = 0; i < strBytes.Length; i++)
            {
                readBytes.Add(strBytes[i]);
                if (strBytes[i] == 0)
                {
                    break;
                }
            }
            String str = Encoding.Default.GetString(readBytes.ToArray());
            if (str.Length > 0 && str.IndexOf('\0') == str.Length - 1)
            {
                str = str.Substring(0, str.Length - 1);
            }
            return str;
        }
    }
}
