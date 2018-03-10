using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace OwLib
{
    /// <summary>
    /// 股票代码
    /// </summary>
    [Serializable]
    public class KwItem
    {
        private String code;              //代码 20
        public String Code
        {
            get { return code; }
            set { code = value; }
        }

        private String name;             //简称 100
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        private String pingyin;		    //拼音 100
        public String Pingyin
        {
            get { return pingyin; }
            set { pingyin = value; }
        }

        private int state;				    //上市状态
        public int State
        {
            get { return state; }
            set { state = value; }
        }

        private int innercode;              //内码 
        public int Innercode
        {
            get { return innercode; }
            set { innercode = value; }
        }

        private int type;                   //分类(文件:0,1,2...)
        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        private String marketcode;
        public String Marketcode
        {
            get { return marketcode; }
            set { marketcode = value; }
        }

        private long timestamp;            //时间戳
        public long Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        /// <summary>
        /// 对象转换成String
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}◎{1}◎{2}◎{3}◎{4}◎{5}◎{6}◎{7}", code, name, pingyin, state, innercode, type, marketcode, timestamp);
            return sb.ToString();
        }
    }

    /// <summary>
    /// 服务状态
    /// </summary>
    public struct DMState
    {
        public int cVersion; //当前版本
        public int nVersion; //正在更新的版本
        public int nPer; //当前更新进度
    }

    /// <summary>
    /// 证券代码表
    /// </summary>
    public struct DMSecuItem
    {
        public String code;              //代码 20
        public String name;             //简称 100
        public String pingyin;		    //拼音 100
        public int state;				    //上市状态
        public int innercode;              //内码 
        public int type;                   //分类(文件:0,1,2...)
        public String marketcode;
        public long timestamp;            //时间戳
    }

    /// <summary>
    /// 板块明细表
    /// </summary>
    public class DMBlockDetailItem
    {
        public String bkcode;            //板块编码 50
        public String code;              //证券代码 20
        public int innercode;            //内码
        public long timestamp;            //时间戳

        /// <summary>
        /// 对象转换成String
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}◎{1}◎{2}◎{3}", bkcode, code, innercode, timestamp);
            return sb.ToString();
        }
    }

    /// <summary>
    /// 板块类型
    /// </summary>
    public class DMBlockItem
    {
        public String innerCode; //内码 16
        public String code;              //编码 50
        public String name;             //名称 200
        public String parentcode;        //父编码 50
        public String typcode;           //分类编码 10
        public String typname;          //分类名称 200
        public String tmpl;             //10
        public int order;                  //排序
        public long timestamp;            //时间戳

        /// <summary>
        /// 对象转换成String
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}◎{1}◎{2}◎{3}◎{4}◎{5}◎{6}◎{7}◎{8}", innerCode, code, name, parentcode, typcode, typname, tmpl, order, timestamp);
            return sb.ToString();
        }
    }

    /// <summary>
    /// 板块数据
    /// </summary>
    public struct DMBlockData
    {
        public String code;              //证券代码 20
    }

    /// <summary>
    /// 码表删除数据
    /// </summary>
    public struct DMSecuItemDelete
    {
        public String code;              //代码 20
        public long timestamp;            //时间戳
    }

    /// <summary>
    /// 基金代码
    /// </summary>
    public struct sDMFundCompcode
    {
        public String pulishcode; //代码 50
        public String compcode; //组合 10
    }

    /// <summary>
    /// 码表类型
    /// </summary>
    public struct sDMTyItem
    {
        public String type; //类型 40
        public String typeCode; //类型代码 40
        public int typeCount; //数量
    }

    /// <summary>
    /// 删除地址
    /// </summary>
    public struct UrlItemDel
    {
        public String code; //代码 10
        public long timestamp;
    }

    /// <summary>
    /// 地址
    /// </summary>
    public struct EMUrlItem
    {
        public String code; //代码 20
        public String url; //地址 200
        public long timestamp;
    }

    public struct sDM_SPE_AREAINDGNLISH_GN
    {
        public String cSECURITYVARIETYCODE; //50
        public String cSECURITYVARIETYCODEGN; //50
    }

    public struct sDM_SPE_AREAINDGNLISH
    {
        public String cSECURITYVARIETYCODE; //50
        public String cSECURITYVARIETYCODEDC; //50
        public String cSECURITYVARIETYCODEDY; //50
    }

    public struct sIND_MAININDEX
    {
        public int nINT_PAIXU;
        public String cSTR_EMCODE; //60
    }

    public struct SDM_IND_BKZSDYGX_TEST
    {
        public String cPUBLISHCODE; //50
        public String cSTR_EMCODE; //60
        public String cSTR_LX; //20
        public int innerCode;
    }

    /// <summary>
    /// 板块排序
    /// </summary>
    public class DMBlockItemCompre : IComparer<DMBlockItem>
    {
        public int Compare(DMBlockItem x, DMBlockItem y)
        {
            if (y.name == "沪深股票")
            {
                return 1;
            }
            return (x.typcode + x.code).CompareTo(y.typcode + y.code);
        }
    }

    /// <summary>
    /// 请求结构
    /// </summary>
    public struct DMReqInput
    {
        public int itemid;
        public int requestid;
        public long timestamp;
        public int size;
        public byte[] ptr;

        public byte[] Write(MemoryStream ms, BinaryWriter sw)
        {
            sw.Write(itemid);
            sw.Write(requestid);
            sw.Write(timestamp);
            sw.Write(size);
            if (ptr != null)
            {
                sw.Write(ptr);
            }
            byte[] buffer = ms.GetBuffer();
            int length = sizeof(int) * 3 + sizeof(long) + (ptr == null ? 0 : ptr.Length);
            byte[] sendBytes = new byte[length];
            for (int i = 0; i < length; i++)
            {
                sendBytes[i] = buffer[i];
            }
            byte[] result = HttpEasyService.RequestGlobalData(sendBytes);
            return result;
        }

    };

    /// <summary>
    /// 返回结构
    /// </summary>
    public struct DMRetOutput
    {
        public int itemid;
        public int requestid;
        public long timestamp;
        public int size;
        public bool last;
        public int nTotalCount;
        public int nVersion;
        public byte[] ptr;
        public DMRetOutput Read(byte[] bytes, BinaryReader br)
        {
            this.itemid = br.ReadInt32();
            this.requestid = br.ReadInt32();
            this.size = br.ReadInt32();
            this.nTotalCount = br.ReadInt32();
            this.nVersion = br.ReadInt32();
            this.timestamp = br.ReadInt64();
            this.last = br.ReadBoolean();
            int length = bytes.Length - sizeof(int) * 5 - sizeof(long) - sizeof(bool);
            if (length > 0)
            {
                this.ptr = br.ReadBytes(length);
            }
            return this;
        }
    };
}
