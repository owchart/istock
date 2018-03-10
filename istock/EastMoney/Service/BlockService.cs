using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace OwLib
{
    /// <summary>
    /// 板块服务
    /// </summary>
    public class BlockService :HttpEasyService
    {
        /// <summary>
        /// 创建板块服务
        /// </summary>
        public BlockService()
        {
        }

        private static List<DMBlockItem> blocks = new List<DMBlockItem>();

        public static List<DMBlockItem> Blocks
        {
            get { return BlockService.blocks; }
            set { BlockService.blocks = value; }
        }

        private static Dictionary<String, List<DMBlockDetailItem>> blockDetails = new Dictionary<string, List<DMBlockDetailItem>>();

        /// <summary>
        /// 获取或设置板块明细
        /// </summary>
        public static Dictionary<String, List<DMBlockDetailItem>> BlockDetails
        {
            get { return BlockService.blockDetails; }
            set { BlockService.blockDetails = value; }
        }

        /// <summary>
        /// 获取板块明细
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, List<DMBlockDetailItem>> GetBlockDetailItems()
        {
            Dictionary<String, List<DMBlockDetailItem>> list = new Dictionary<String, List<DMBlockDetailItem>>();
            String code = "";
            long time = 0;
            while (true)
            {
                DMRetOutput dm1 = UpdateBlockDetail(time, code, 0, 1, true);
                DMBlockDetailItem lastItem = null;
                using (MemoryStream ms = new MemoryStream(dm1.ptr))
                {
                    using (BinaryReader br = new BinaryReader(ms))
                    {
                        int size = dm1.size;
                        br.ReadBytes(50);
                        for (int i = 0; i < size; i++)
                        {
                            DMBlockDetailItem item = new DMBlockDetailItem();
                            item.bkcode = GetBytesString(br, 50);
                            item.code = GetBytesString(br, 22);
                            item.innercode = br.ReadInt32();
                            item.timestamp = br.ReadInt64();
                            if (!list.ContainsKey(item.bkcode))
                            {
                                list[item.bkcode] = new List<DMBlockDetailItem>();
                            }
                            list[item.bkcode].Add(item);
                            lastItem = item;
                        }
                    }
                }
                if (!dm1.last)
                {
                    if (lastItem != null)
                    {
                        code = lastItem.code.Replace("\0", "");
                        time = lastItem.timestamp;
                    }
                }
                if (dm1.last)
                {
                    dm1.ptr = null;
                    break;
                }
                else
                {
                    dm1.ptr = null;
                }
            }
            code = "";
            time = 0;
            #region Delete NON_IMPORTANT
            //while (true)
            //{
            //    DMRetOutput dm1 = UpdateBlockDetail(time, code, 0, 1, false);
            //    DMBlockDetailItem lastItem = null;
            //    using (MemoryStream ms = new MemoryStream(dm1.ptr))
            //    {
            //        using (BinaryReader br = new BinaryReader(ms))
            //        {
            //            int size = dm1.size;
            //            br.ReadBytes(50);
            //            for (int i = 0; i < size; i++)
            //            {
            //                DMBlockDetailItem item = new DMBlockDetailItem();
            //                item.bkcode = GetBytesString(br, 50);
            //                item.code = GetBytesString(br, 22);
            //                item.innercode = br.ReadInt32();
            //                item.timestamp = br.ReadInt64();
            //                if (!list.ContainsKey(item.bkcode))
            //                {
            //                    list[item.bkcode] = new List<DMBlockDetailItem>();
            //                }
            //                list[item.bkcode].Add(item);
            //                lastItem = item;
            //            }
            //        }
            //    }
            //    if (!dm1.last)
            //    {
            //        if (lastItem != null)
            //        {
            //            code = lastItem.code.Replace("\0", "");
            //            time = lastItem.timestamp;
            //        }
            //    }
            //    if (dm1.last)
            //    {
            //        dm1.ptr = null;
            //        break;
            //    }
            //    else
            //    {
            //        dm1.ptr = null;
            //    }
            //}
            #endregion
            return list;
        }

        /// <summary>
        /// 获取板块项
        /// </summary>
        /// <returns>板块项</returns>
        public List<DMBlockItem> GetBlockItems()
        {
            String code = "";
            long time = 0;
            List<DMBlockItem> items = new List<DMBlockItem>();
            while (true)
            {
                DMRetOutput dm1 = UpdateBlockTree(time, 1, 0, code);
                DMBlockItem lastItem = null;
                using (MemoryStream ms = new MemoryStream(dm1.ptr))
                {
                    using (BinaryReader br = new BinaryReader(ms))
                    {
                        int size = dm1.size;
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
                            lastItem = item;
                        }
                    }
                }
                if (!dm1.last)
                {
                    if (lastItem != null)
                    {
                        code = lastItem.code;
                        time = lastItem.timestamp;
                    }
                }
                if (dm1.last)
                {
                    break;
                }
            }
            code = "";
            time = 0;
            return items;
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="loadAll">是否全部加载标记</param>
        public static void Load(bool loadAll)
        {
            if (loadAll)
            {
                blocks = DataCenter.BlockService.GetBlockItems();
                blockDetails = DataCenter.BlockService.GetBlockDetailItems();
                blocks.Sort(new DMBlockItemCompre());
            }
        }

        /// <summary>
        /// 板块树数据转换成字符串
        /// </summary>
        /// <returns></returns>
        public String BlockTreesToString()
        {
            StringBuilder sb = new StringBuilder(1024000);
            foreach (DMBlockItem item in BlockService.blocks)
            {
                sb.AppendLine(item.ToString());
            }
            return sb.ToString();
        }

        /// <summary>
        /// 板块明细数据转换成字符串
        /// </summary>
        /// <returns></returns>
        public String BlockDetailsToString()
        {
            StringBuilder sb = new StringBuilder(1024000);
            foreach (KeyValuePair<String, List<DMBlockDetailItem>> item in BlockService.BlockDetails)
            {
                List<DMBlockDetailItem> details = item.Value;
                foreach (DMBlockDetailItem blockItem in details)
                {
                    sb.AppendLine(blockItem.ToString());  
                }
            }
            return sb.ToString();
        }

        /**************************************************************************
        * Note: 请求板块明细表数据 测试通过
        * args: ctLast,请求时传递的时间戳，服务端从该时间戳开始数据
        *		szCode,请求时传递码表代码，
        *		nSize: 保存szCode的字节数
        *		nRequestId : 0=增量；1=全量；2=指定类别全量
        *		IsImportant：是否为重要板块
        * void：
        *
        **************************************************************************/
        public DMRetOutput UpdateBlockDetail(long ctLast, String szCode, int nSize, int nRequestId, bool bIsImportant)
        {
            DMReqInput reqInput = new DMReqInput();
            nSize = (nSize == 0 ? 70 : nSize);
            reqInput.size = nSize;
            if (bIsImportant)
            {
                reqInput.itemid = SDATA_TYPE_BLOCKDETAIL_IMPT;
            }
            else
            {
                reqInput.itemid = SDATA_TYPE_BLOCKDETAIL_NOTIMPT;
            }

            reqInput.timestamp = ctLast;
            reqInput.requestid = nRequestId;//测试
            if (szCode == null)
            {
                szCode = "";
            }
            reqInput.ptr = GetStringBytes(szCode, 70);
            DMRetOutput reqOutput = GetDMRet(reqInput);
            return reqOutput;
        }

        /**************************************************************************
        * Note: 请求板块树 数据 测试通过
        * args: ctLast,请求时传递的时间戳，服务端从该时间戳开始数据
        *		nRequestId : 0=增量；1=全量；2=指定类别全量
        * void：
        *
        **************************************************************************/
        public DMRetOutput UpdateBlockTree(long ctLast, int nRequestId, int nSize, String szCode)
        {
            DMReqInput reqInput = new DMReqInput();
            reqInput.itemid = SDATA_TYPE_BLOCKDATATREE;
            reqInput.timestamp = ctLast;
            reqInput.requestid = 1;
            reqInput.size = nSize;
            if (szCode != null && szCode.Length > 0)
            {
                reqInput.ptr = GetStringBytes(szCode, nSize);
            }
            DMRetOutput reqOutput = GetDMRet(reqInput);
            return reqOutput;
        }
    }
}
