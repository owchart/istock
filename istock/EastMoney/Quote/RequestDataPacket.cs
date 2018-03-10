using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace OwLib
{
    #region 财富通实时接口
    /// <summary>
    /// 心跳包
    /// </summary>
    public class ReqHeartDataPacket : RealTimeDataPacket
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReqHeartDataPacket()
        {
            RequestType = FuncTypeRealTime.Heart;
        }
    }

    /// <summary>
    /// 码表
    /// </summary>
    public class ReqStockDictDataPacket : RealTimeDataPacket
    {
        private int _date;
        /// <summary>
        /// 日期
        /// </summary>
        public int Date
        {
            get { return _date; }
            set { this._date = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReqStockDictDataPacket()
        {
            Debug.Print("send dict");
            RequestType = FuncTypeRealTime.StockDict;
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(Date);
        }
    }

    /// <summary>
    /// 个股Detail行情
    /// </summary>
    public class ReqStockDetailDataPacket : RealTimeDataPacket
    {
        private int _code;
        private byte _market;
        private string _shortCode;

        public ReqStockDetailDataPacket()
        {
            base.RequestType = FuncTypeRealTime.StockDetail;
            this.IsPush = true;
        }

        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(this._market);
            bw.Write(Encoding.ASCII.GetBytes(this._shortCode));
            bw.Write(this.IsPush);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != base.GetType())
            {
                return false;
            }
            ReqStockDetailDataPacket packet = (ReqStockDetailDataPacket)obj;
            return (packet.Code == this.Code);
        }

        public override int GetHashCode()
        {
            return this.Code.GetHashCode();
        }

        public int Code
        {
            get
            {
                return this._code;
            }
            set
            {
                this._code = value;
                ReqMarketType market = ReqMarketType.MT_NA;
                DataPacket.ParseCode(DllImportHelper.GetFieldDataString(this._code, FieldIndex.EMCode), out market, out this._shortCode);
                this._market = (byte)market;
            }
        }
    }

    /// <summary>
    /// 个股level2行情
    /// </summary>  
    public class ReqStockDetailLev2DataPacket : RealTimeDataPacket
    {

        private int _code;
        private byte _market;
        private string _shortCode;
        private int _unicode;
        public List<int> Codes;

        public ReqStockDetailLev2DataPacket()
        {
            base.RequestType = FuncTypeRealTime.StockDetailLev2;
            this.IsPush = true;
            this.Codes = new List<int>();
            IsGetOnceNoPush = false;
        }

        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            if (IsGetOnceNoPush)
            {
                bw.Write((byte)5);
            }
            else if (this.IsPush)
            {
                bw.Write((byte)2);
            }
            else
            {
                bw.Write((byte)0);
            }
            byte count = 0;
            if (this.Codes != null)
            {
                count = (byte)this.Codes.Count;
            }
            bw.Write(count);
            for (int i = 0; i < this.Codes.Count; i++)
            {
                int num3 = ConvertCodeOrgToCft(this.Codes[i]);
                bw.Write((uint)num3);
            }
        }

        public static int ConvertCodeOrgToCft(int code)
        {
            ReqMarketType type;
            string str;
            DataPacket.ParseCode(DetailData.FieldIndexDataString[code][ FieldIndex.EMCode], out type, out str);
            byte market = (byte)type;
            return ConvertCode.ConvertCodeToInt(Encoding.ASCII.GetBytes(str), market);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != base.GetType())
            {
                return false;
            }
            ReqStockDetailLev2DataPacket packet = (ReqStockDetailLev2DataPacket)obj;
            int num = -1;
            if ((packet.Codes != null) && (packet.Codes.Count > 0))
            {
                num = packet.Codes[0];
            }
            int num2 = 0;
            if ((this.Codes != null) && (this.Codes.Count > 0))
            {
                num2 = this.Codes[0];
            }
            return (num2 == num);
        }

        public override int GetHashCode()
        {
            if ((this.Codes != null) && (this.Codes.Count > 0))
            {
                int num = this.Codes[0];
                return num.GetHashCode();
            }
            return base.GetHashCode();
        }

        public int Code
        {
            get
            {
                return this._code;
            }
            set
            {
                this._code = value;
                ReqMarketType market = ReqMarketType.MT_NA;
                DataPacket.ParseCode(DetailData.FieldIndexDataString[_code][FieldIndex.EMCode], out market, out this._shortCode);
                this._market = (byte)market;
                this._unicode = ConvertCode.ConvertCodeToInt(Encoding.ASCII.GetBytes(this._shortCode), this._market);
                Codes.Add(_code);
            }
        }

    }



    /// <summary>
    /// 百档行情
    /// </summary>
    public class ReqAllOrderStockDetailLev2DataPacket : RealTimeDataPacket
    {

        private List<int> _codeList;
        private int _testCode;
        private List<int> _unicodeList;
        private int ContainerId;
        private int CtrlId;
        private static object objLock = new object();

        public ReqAllOrderStockDetailLev2DataPacket()
        {
            base.RequestType = FuncTypeRealTime.AllOrderStockDetailLevel2;
            this.IsPush = true;
            this.CtrlId = DataPacket.TrendCtrlId;
            base.IsGetOnceNoPush = false;
        }

        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            if (base.IsGetOnceNoPush)
            {
                bw.Write((byte)5);
            }
            else
            {
                bw.Write(this.IsPush);
            }
            bw.Write(this.CtrlId);
            bw.Write((byte)1);
            lock (objLock)
            {
                if (!base.IsGetOnceNoPush)
                {
                    if (!this.IsPush)
                    {
                        CftContainerIdMgr.ReleaseContainerId(base.RequestType, this.ContainerId);
                    }
                    else
                    {
                        this.ContainerId = CftContainerIdMgr.CreateContainerId(base.RequestType);
                    }
                }
                else
                {
                    this.ContainerId = 0x4c4b40;
                }
            }
            if (this.ContainerId != 0)
            {
                bw.Write(this.ContainerId);
                byte count = (byte)this._unicodeList.Count;
                bw.Write(count);
                foreach (int num2 in this._unicodeList)
                {
                    bw.Write(num2);
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != base.GetType())
            {
                return false;
            }
            ReqAllOrderStockDetailLev2DataPacket packet = (ReqAllOrderStockDetailLev2DataPacket)obj;
            int num = 0;
            if ((this._unicodeList != null) && (this._unicodeList.Count > 0))
            {
                num = this._unicodeList[0];
            }
            int num2 = -1;
            if ((packet._unicodeList != null) && (packet._unicodeList.Count > 0))
            {
                num2 = packet._unicodeList[0];
            }
            return (num == num2);
        }

        public override int GetHashCode()
        {
            if ((this._unicodeList != null) && (this._unicodeList.Count > 0))
            {
                int num = this._unicodeList[0];
                return num.GetHashCode();
            }
            return base.GetHashCode();
        }

        private int SetCode(int code)
        {
            string shortCode = string.Empty;
            ReqMarketType market = ReqMarketType.MT_NA;
            DataPacket.ParseCode(DllImportHelper.GetFieldDataString(code, FieldIndex.EMCode), out market, out shortCode);
            byte num = (byte)market;
            return ConvertCode.ConvertCodeToInt(Encoding.ASCII.GetBytes(shortCode), num);
        }

        public int Code
        {
            get
            {
                if ((this._unicodeList != null) && (this._unicodeList.Count > 0))
                {
                    return this._unicodeList[0];
                }
                return 0;
            }
            set
            {
                this._testCode = value;
                this._unicodeList = new List<int>(1);
                this._unicodeList.Add(this.SetCode(value));
            }
        }

        public List<int> CodeList
        {
            set
            {
                this._codeList = value;
                if ((this._codeList != null) && (this._codeList.Count > 0))
                {
                    this._unicodeList = new List<int>(this._codeList.Count);
                    foreach (int num in this._codeList)
                    {
                        this._unicodeList.Add(this.SetCode(num));
                    }
                }
            }
        }

        public int TestCode
        {
            get
            {
                return this._testCode;
            }
        }

        public int TestContaierId
        {
            get
            {
                return this.ContainerId;
            }
        }

        public int TestCtrlId
        {
            get
            {
                return this.CtrlId;
            }
        }

        protected List<int> UniCodeList
        {
            get
            {
                return this._unicodeList;
            }
        }
    }

    /// <summary>
    /// N档行情
    /// </summary>
    public class ReqNOrderStockDetailLevel2DataPacket : ReqAllOrderStockDetailLev2DataPacket
    {
        public ReqNOrderStockDetailLevel2DataPacket()
        {
            base.RequestType = FuncTypeRealTime.NOrderStockDetailLevel2;
            this.IsPush = true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != base.GetType())
            {
                return false;
            }
            ReqNOrderStockDetailLevel2DataPacket packet = (ReqNOrderStockDetailLevel2DataPacket)obj;
            int num = 0;
            if ((base.UniCodeList != null) && (base.UniCodeList.Count > 0))
            {
                num = base.UniCodeList[0];
            }
            int num2 = -1;
            if ((packet.UniCodeList != null) && (packet.UniCodeList.Count > 0))
            {
                num2 = packet.UniCodeList[0];
            }
            return (num == num2);
        }

        public override int GetHashCode()
        {
            if ((base.UniCodeList != null) && (base.UniCodeList.Count > 0))
            {
                int num = base.UniCodeList[0];
                return num.GetHashCode();
            }
            return base.GetHashCode();
        }
    }


    /// <summary>
    /// 百档挂单行情请求结构
    /// </summary>
    public class StructReqDetailOrderQueue
    {
        /// <summary>
        /// 内码
        /// </summary>
        public int Code;

        /// <summary>
        /// 挂单价格
        /// </summary>
        public float Price;

        /// <summary>
        /// 买卖方向,0是buy，1是sell
        /// </summary>
        public byte BSFlag;

    }

    /// <summary>
    /// 百档挂单队列行情
    /// </summary>
    public class ReqStockDetailLev2OrderQueueDataPacket : RealTimeDataPacket
    {

        /// <summary>
        /// 请求参数
        /// </summary>
        public StructReqDetailOrderQueue ReqData
        {
            set
            {
                _reqDataList = new List<StructReqDetailOrderQueue>(1);
                value.Code = SetCode(value.Code);
                _reqDataList.Add(value);
            }
        }

        private List<StructReqDetailOrderQueue> _reqDataList;


        /// <summary>
        /// 请求参数
        /// </summary>
        public List<StructReqDetailOrderQueue> ReqDataList
        {
            set
            {
                _reqDataList = value;
                if (_reqDataList != null && _reqDataList.Count > 0)
                {
                    foreach (StructReqDetailOrderQueue data in _reqDataList)
                    {
                        data.Code = SetCode(data.Code);
                    }
                }

            }
        }


        private int SetCode(int code)
        {
            string shortCode = string.Empty;
            ReqMarketType tmp = ReqMarketType.MT_NA;
            byte market;
            string emcode = string.Empty;
            if (DetailData.FieldIndexDataString.ContainsKey(code))
                DetailData.FieldIndexDataString[code].TryGetValue(FieldIndex.EMCode, out emcode);

            ParseCode(emcode, out tmp, out shortCode);
            market = (byte)tmp;

            return ConvertCode.ConvertCodeToInt(Encoding.ASCII.GetBytes(shortCode), market);
        }

        public ReqStockDetailLev2OrderQueueDataPacket()
        {
            RequestType = FuncTypeRealTime.StockDetailOrderQueue;
            IsPush = true;
        }

        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(IsPush);
            if (_reqDataList != null)
            {
                byte count = (byte)_reqDataList.Count;
                bw.Write(count);
                foreach (StructReqDetailOrderQueue data in _reqDataList)
                {
                    bw.Write(data.Code);
                    bw.Write(data.Price);
                    bw.Write(data.BSFlag);
                }
                //if (IsPush)
                //    DetailData.MsgIdIntPtrs[MsgId] = CreateInstance();
                //else
                //{
                //    if (DetailData.MsgIdIntPtrs.ContainsKey(MsgId))
                //    {
                //        FreeInstance(DetailData.MsgIdIntPtrs[MsgId]);
                //        DetailData.MsgIdIntPtrs.Remove(MsgId);
                //    }

                //}

            }
        }

    }


    /// <summary>
    /// 股指期货行情
    /// </summary>
    public class ReqIndexFuturesDetailDataPacket : RealTimeDataPacket
    {
        /// <summary>
        /// 
        /// </summary>
        public ReqIndexFuturesDetailDataPacket()
        {
            RequestType = FuncTypeRealTime.IndexFuturesDetail;
            IsPush = true;
        }

        /// <summary>
        /// 股票代码
        /// </summary>
        public String Code
        {
            get { return _code; }
            set
            {
                _code = value;

                ReqMarketType tmp = ReqMarketType.MT_NA;
                string emcode = _code;

                if (emcode.StartsWith("I"))
                {
                    if (emcode == "IF00C1.CFE")
                        _shortCode = "040120";
                    else if (emcode == "IF00C2.CFE")
                        _shortCode = "040121";
                    else if (emcode == "IF00C3.CFE")
                        _shortCode = "040122";
                    else if (emcode == "IF00C4.CFE")
                        _shortCode = "040123";
                    else
                        _shortCode = "0411" + emcode.Substring(emcode.Length - 6, 2) + '\0';
                }
                else if (emcode.StartsWith("T"))
                {
                    if (emcode == "TF00C1.CFE")
                        _shortCode = "050120";
                    else if (emcode == "TF00C2.CFE")
                        _shortCode = "050121";
                    else if (emcode == "TF00C3.CFE")
                        _shortCode = "050122";
                    else if (emcode == "TF00C4.CFE")
                        _shortCode = "050123";
                    else
                        _shortCode = "0511" + emcode.Substring(emcode.Length - 6, 2) + '\0';
                }

                _market = (byte)ReqMarketType.MT_IndexFutures;
                _unicode = ConvertCode.ConvertCodeToInt(Encoding.ASCII.GetBytes(_shortCode), _market);
            }
        }

        private byte _market;
        private string _shortCode;
        private String _code;
        private int _unicode;


        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(_unicode);
            bw.Write(IsPush);
        }

    }

    /// <summary>
    /// 个股走势
    /// </summary>
    public class ReqStockTrendDataPacket : RealTimeDataPacket
    {
        private int _code;
        private int _lastRequestPoint;
        private byte _market;
        private string _shortCode;
        private int _time;
        private int ContainerId;
        private int CtrlId;
        public int Date;
        private static object objLock = new object();
        private int stockId;
        public byte TrendFlag;

        public ReqStockTrendDataPacket()
        {
            base.RequestType = FuncTypeRealTime.StockTrend;
            this.TrendFlag = 1;
            this.CtrlId = DataPacket.TrendCtrlId;
            this.IsPush = true;
        }

        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(this.IsPush);
            bw.Write(this.CtrlId);
            bw.Write((byte)1);
            lock (objLock)
            {
                if (!this.IsPush)
                {
                    CftContainerIdMgr.ReleaseContainerId(base.RequestType, this.ContainerId);
                }
                else
                {
                    this.ContainerId = CftContainerIdMgr.CreateContainerId(base.RequestType);
                }
            }
            if (this.ContainerId == 0)
            {
                //LogUtilities.LogMessagePublishInfo(string.Concat(new object[] { "trend containerId = 0!, ispush=", this.IsPush, ", code=", this._shortCode.TrimEnd(new char[1]) }));
            }
            else
            {
                bw.Write(this.ContainerId);
                bw.Write((byte)1);
                bw.Write(this.stockId);
                bw.Write(0);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj.GetType() != base.GetType())
            {
                return false;
            }
            ReqStockTrendDataPacket packet = (ReqStockTrendDataPacket)obj;
            return (packet.stockId == this.stockId);
        }

        public override int GetHashCode()
        {
            return this.stockId.GetHashCode();
        }

        public int Code
        {
            get
            {
                return this._code;
            }
            set
            {
                this._code = value;
                ReqMarketType market = ReqMarketType.MT_NA;
                DataPacket.ParseCode(DetailData.FieldIndexDataString[_code][FieldIndex.EMCode], out market, out this._shortCode);
                this._market = (byte)market;
                this.stockId = ConvertCode.ConvertCodeToInt(Encoding.ASCII.GetBytes(this._shortCode), this._market);
                switch (DllImportHelper.GetMarketType(this._code))
                {
                    case MarketType.IF:
                    case MarketType.GoverFutures:
                        this.stockId = ConvertCode.CovertFuturesOrgCodeToCftStockId(this._code);
                        break;
                }
            }
        }

        public int LastRequestPoint
        {
            get
            {
                return this._lastRequestPoint;
            }
            set
            {
                this._lastRequestPoint = value;
                this._time = TimeUtilities.GetTimeFromPoint(this.Code, this._lastRequestPoint);
            }
        }
    }

    /// <summary>
    /// 股指期货走势
    /// </summary>
    public class ReqIndexFuturesTrendDataPacket : RealTimeDataPacket
    {
        /// <summary>
        /// 日期
        /// </summary>
        public int Date;

        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set
            {
                _code = value;

                string shortcode = string.Empty;
                if (DetailData.FieldIndexDataString.ContainsKey(_code))
                    DetailData.FieldIndexDataString[_code].TryGetValue(FieldIndex.Code, out shortcode);

                if (shortcode.StartsWith("I"))
                {
                    if (shortcode == "IF00C1")
                        _shortCode = "040120";
                    else if (shortcode == "IF00C2")
                        _shortCode = "040121";
                    else if (shortcode == "IF00C3")
                        _shortCode = "040122";
                    else if (shortcode == "IF00C4")
                        _shortCode = "040123";
                    else if (!string.IsNullOrEmpty(shortcode))
                        _shortCode = "0411" + shortcode.Substring(shortcode.Length - 2, 2) + '\0';
                }
                else if (shortcode.StartsWith("T"))
                {
                    if (shortcode == "TF00C1")
                        _shortCode = "050120";
                    else if (shortcode == "TF00C2")
                        _shortCode = "050121";
                    else if (shortcode == "TF00C3")
                        _shortCode = "050122";
                    else if (shortcode == "TF00C4")
                        _shortCode = "050123";
                    else
                        _shortCode = "0511" + shortcode.Substring(shortcode.Length - 2, 2) + '\0';
                }
                _market = (byte)ReqMarketType.MT_IndexFutures;
                _unicode = (uint)ConvertCode.ConvertCodeToInt(Encoding.ASCII.GetBytes(_shortCode), _market);
            }
        }

        private uint _unicode;

        /// <summary>
        /// 从第几个点开始请求，下标从0开始
        /// </summary>
        private short _lastRequestPoint;

        public short LastRequestPoint
        {
            get { return _lastRequestPoint; }
            set { _lastRequestPoint = value; }
        }


        private byte _market;
        private string _shortCode;
        private int _code;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReqIndexFuturesTrendDataPacket()
        {
            RequestType = FuncTypeRealTime.IndexFuturesTrend;
        }

        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(_unicode);
            bw.Write(Date);
            bw.Write(LastRequestPoint);
        }

    }

    /// <summary>
    /// 个股走势推送
    /// </summary>
    public class ReqStockTrendPushDataPacket : RealTimeDataPacket
    {

        private byte _market;
        private string _shortCode;
        private string _code;

        /// <summary>
        /// 上一次请求的下标
        /// </summary>
        public int LastRequestPoint;

        /// <summary>
        /// 股票代码
        /// </summary>
        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
                ReqMarketType tmp;
                ParseCode(_code, out tmp, out _shortCode);
                _market = (byte)tmp;
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReqStockTrendPushDataPacket()
        {
            RequestType = FuncTypeRealTime.StockTrendPush;
            IsPush = true;
            LastRequestPoint = 0;
        }

        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(_market);
            bw.Write(Encoding.ASCII.GetBytes(_shortCode));
            bw.Write(IsPush);
            bw.Write(LastRequestPoint);
        }

    }

    /// <summary>
    /// 分时委买委卖量
    /// </summary>
    public class ReqTrendAskBidDataPacket : RealTimeDataPacket
    {
        /// <summary>
        /// 日期
        /// </summary>
        public int Date;

        /// <summary>
        /// 时间
        /// </summary>
        private int _time;

        /// <summary>
        /// 请求走势最后一个点的下标
        /// </summary>
        public int LastRequestPoint
        {
            get { return _lastRequestPoint; }
            set
            {
                _lastRequestPoint = value;
                _time = TimeUtilities.GetTimeFromPoint(Code, _lastRequestPoint);
            }
        }

        private byte _market;
        private string _shortCode;
        private int _code;
        private int _lastRequestPoint;

        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set
            {
                _code = value;
                ReqMarketType tmp = ReqMarketType.MT_NA;
                string emcode = string.Empty;
                if (DetailData.FieldIndexDataString.ContainsKey(_code))
                    DetailData.FieldIndexDataString[_code].TryGetValue(FieldIndex.EMCode, out emcode);

                ParseCode(emcode, out tmp, out _shortCode);
                _market = (byte)tmp;
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReqTrendAskBidDataPacket()
        {
            RequestType = FuncTypeRealTime.StockTrendAskBid;
        }

        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(Date);
            bw.Write(_time);
            bw.Write(_market);
            bw.Write(Encoding.ASCII.GetBytes(_shortCode));
        }
    }

    /// <summary>
    /// 分时内外盘差
    /// </summary>
    public class ReqTrendInOutDiffDataPacket : ReqTrendAskBidDataPacket
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReqTrendInOutDiffDataPacket()
        {
            RequestType = FuncTypeRealTime.StockTrendInOutDiff;
        }
    }

    /// <summary>
    /// 走势红绿柱
    /// </summary>
    public class ReqRedGreenDataPacket : RealTimeDataPacket
    {
        /// <summary>
        /// 日期
        /// </summary>
        public int Date;

        /// <summary>
        /// 时间
        /// </summary>
        private int _time;

        /// <summary>
        /// 请求走势最后一个点的下标
        /// </summary>
        public int LastRequestPoint
        {
            get { return _lastRequestPoint; }
            set
            {
                _lastRequestPoint = value;
                _time = TimeUtilities.GetTimeFromPoint(Code, _lastRequestPoint);
            }
        }

        private byte _market;
        private string _shortCode;
        private int _code;
        private int _lastRequestPoint;

        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set
            {
                _code = value;
                ReqMarketType tmp = ReqMarketType.MT_NA;
                string emcode = string.Empty;
                if (DetailData.FieldIndexDataString.ContainsKey(_code))
                    DetailData.FieldIndexDataString[_code].TryGetValue(FieldIndex.EMCode, out emcode);

                ParseCode(emcode, out tmp, out _shortCode);

                _market = (byte)tmp;
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReqRedGreenDataPacket()
        {
            RequestType = FuncTypeRealTime.RedGreen;
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(Date);
            bw.Write(_time);
            bw.Write(_market);
            bw.Write(Encoding.ASCII.GetBytes(_shortCode));
        }
    }

    /// <summary>
    /// 历史走势
    /// </summary>
    public class ReqHisTrendDataPacket : RealTimeDataPacket
    {
        /// <summary>
        /// 日期
        /// </summary>
        public int Date;

        /// <summary>
        /// 时间
        /// </summary>
        public int Time;

        /// <summary>
        /// 0当天，n往后推n天，-n往前推n天
        /// </summary>
        public int PreDay;

        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set
            {
                _code = value;
                ReqMarketType tmp = ReqMarketType.MT_NA;
                string emcode = string.Empty;
                if (DetailData.FieldIndexDataString.ContainsKey(_code))
                    DetailData.FieldIndexDataString[_code].TryGetValue(FieldIndex.EMCode, out emcode);
                ParseCode(emcode, out tmp, out _shortCode);
                _market = (byte)tmp;
            }
        }

        private byte _market;
        private string _shortCode;
        private int _code;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReqHisTrendDataPacket()
        {
            RequestType = FuncTypeRealTime.HisTrend;
        }

        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(Date);
            bw.Write(Time);
            bw.Write(PreDay);
            bw.Write(_market);
            bw.Write(Encoding.ASCII.GetBytes(_shortCode));
        }
    }

    /// <summary>
    /// 指数Detail行情，支持多个指数
    /// </summary>
    public class ReqIndexDetailDataPacket : RealTimeDataPacket
    {

        /// <summary>
        /// 指数SecuCode代码集
        /// </summary>
        private List<int> UniCodeList;

        /// <summary>
        /// 指数代码集合
        /// </summary>
        public List<int> CodeList
        {
            get { return _codeList; }
            set
            {
                _codeList = value;
                UniCodeList.Clear();
                CodeToUnicode(_codeList);
            }
        }

        private List<int> _codeList;

        private void CodeToUnicode(List<int> codes)
        {
            foreach (int code in codes)
            {
                ReqMarketType tmp = ReqMarketType.MT_NA;
                string shortCode = string.Empty;
                string emcode = string.Empty;
                if (DetailData.FieldIndexDataString.ContainsKey(code))
                    DetailData.FieldIndexDataString[code].TryGetValue(FieldIndex.EMCode, out emcode);

                ParseCode(emcode, out tmp, out shortCode);
                int uniCode = ConvertCode.ConvertCodeToInt(Encoding.ASCII.GetBytes(shortCode), (byte)tmp);
                UniCodeList.Add(uniCode);
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReqIndexDetailDataPacket()
        {
            RequestType = FuncTypeRealTime.IndexDetail;
            IsPush = true;
            UniCodeList = new List<int>();
        }

        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(IsPush);

            byte num = 0;
            if (UniCodeList != null)
                num = (byte)UniCodeList.Count;
            bw.Write(num);

            if (UniCodeList != null)
            {
                foreach (int uniCode in UniCodeList)
                    bw.Write(uniCode);
            }
        }
    }

    /// <summary>
    /// 历史K线
    /// </summary>
    public class ReqHisKLineDataPacket : RealTimeDataPacket
    {
        private byte _market;
        private string _shortCode;
        private int _code;

        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set
            {
                _code = value;
                string emcode = string.Empty;
                if (DetailData.FieldIndexDataString.ContainsKey(_code))
                    DetailData.FieldIndexDataString[_code].TryGetValue(FieldIndex.EMCode, out emcode);

                MarketType mt = MarketType.NA;
                int mtInt = 0;
                if (DetailData.FieldIndexDataInt32.ContainsKey(_code))
                    DetailData.FieldIndexDataInt32[_code].TryGetValue(FieldIndex.Market, out mtInt);
                mt = (MarketType)mtInt;

                if (mt == MarketType.IF)
                {

                    if (emcode == "IF00C1.CFE")
                        _shortCode = "040120\0";
                    else if (emcode == "IF00C2.CFE")
                        _shortCode = "040121\0";
                    else if (emcode == "IF00C3.CFE")
                        _shortCode = "040122\0";
                    else if (emcode == "IF00C4.CFE")
                        _shortCode = "040123\0";
                    else
                        _shortCode = "0411" + emcode.Substring(emcode.Length - 6, 2) + '\0';
                    _market = (byte)ReqMarketType.MT_IndexFutures;
                }
                else if (mt == MarketType.GoverFutures)
                {

                    if (emcode == "TF00C1.CFE")
                        _shortCode = "050120\0";
                    else if (emcode == "TF00C2.CFE")
                        _shortCode = "050121\0";
                    else if (emcode == "TF00C3.CFE")
                        _shortCode = "050122\0";
                    else if (emcode == "TF00C4.CFE")
                        _shortCode = "050123\0";
                    else
                        _shortCode = "0511" + emcode.Substring(emcode.Length - 6, 2) + '\0';
                    _market = (byte)ReqMarketType.MT_IndexFutures;
                }
                else
                {
                    ReqMarketType tmp = ReqMarketType.MT_NA;
                    ParseCode(emcode, out tmp, out _shortCode);
                    _market = (byte)tmp;
                }
            }

        }

        /// <summary>
        /// K线周期
        /// </summary>
        public KLineCycle Cycle;

        /// <summary>
        /// 请求K线的数据范围
        /// </summary>
        public ReqKLineDataRange DataRange;

        /// <summary>
        /// 开始日期 格式[YYYYMMDD]
        /// </summary>
        public int StartDate;

        /// <summary>
        /// 结束日期 格式[YYYYMMDD]
        /// </summary>
        public int EndDate;

        /// <summary>
        /// 请求K线的个数
        /// </summary>
        public int ApplySize;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReqHisKLineDataPacket()
        {
            RequestType = FuncTypeRealTime.HisKLine;
            Cycle = KLineCycle.CycleDay;
            DataRange = ReqKLineDataRange.SizeToNow;
        }

        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(_market);
            bw.Write(Encoding.ASCII.GetBytes(_shortCode));
            bw.Write((byte)Cycle);
            bw.Write((byte)DataRange);
            bw.Write(StartDate);
            bw.Write(EndDate);
            bw.Write(ApplySize);
        }
    }

    /// <summary>
    /// 当日K线
    /// </summary>
    public class ReqMinKLineDataPacket : ReqHisKLineDataPacket
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReqMinKLineDataPacket()
        {
            RequestType = FuncTypeRealTime.MinKLine;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ReqBlocksOverViewDataPacket : RealTimeDataPacket
    {
        /// <summary>
        /// 板块类型
        /// </summary>
        public byte BlockType;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReqBlocksOverViewDataPacket()
        {
            RequestType = FuncTypeRealTime.BlockOverViewList;
            IsPush = true;
            BlockType = 2;//行业
        }

        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(BlockType);
            bw.Write(IsPush);
        }
    }

    /// <summary>
    /// 板块简易行情
    /// </summary>
    public class ReqBlockSimpleQuoteDataPacket : RealTimeDataPacket
    {
        private bool _isPush;
        /// <summary>
        /// 推送
        /// </summary>
        public bool IsPush
        {
            get { return _isPush; }
            set { this._isPush = value; }
        }

        private List<int> _unicodeList;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ReqBlockSimpleQuoteDataPacket(string blockId)
        {
            RequestType = FuncTypeRealTime.BlockSimpleQuote;
            IsPush = true;
            int unicode = ConvertCode.ConvertCodeToInt(Encoding.ASCII.GetBytes(blockId),
                                                       Convert.ToByte(ReqMarketType.MT_Plate));
            _unicodeList = new List<int>(1);
            _unicodeList.Add(unicode);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReqBlockSimpleQuoteDataPacket(List<string> blockIdList)
        {
            RequestType = FuncTypeRealTime.BlockSimpleQuote;
            IsPush = true;
            _unicodeList = new List<int>(blockIdList.Count);
            foreach (string blockid in blockIdList)
            {
                int unicode = ConvertCode.ConvertCodeToInt(Encoding.ASCII.GetBytes(blockid),
                                                       Convert.ToByte(ReqMarketType.MT_Plate));
                _unicodeList.Add(unicode);
            }
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(IsPush);
            bw.Write((uint)_unicodeList.Count);
            foreach (int unicode in _unicodeList)
                bw.Write((uint)unicode);
        }


    }

    /// <summary>
    /// 市场报价请求包
    /// </summary>
    public class ReqSectorQuoteReportDataPacket : RealTimeDataPacket
    {
        /// <summary>
        /// 0-仅返回请求信息；1-返回请求信息和数据
        /// </summary>
        public bool IsResponseData;

        /// <summary>
        /// 0-股票；1-板块
        /// </summary>
        public bool IsBlock
        {
            get { return _isBlock; }
            set
            {
                _isBlock = value;
                if (value)
                    RequestType = FuncTypeRealTime.BlockQuoteReport;
                else
                    RequestType = FuncTypeRealTime.SectorQuoteReport;

                if (_isBlock)
                {
                    foreach (string id in _idInput)
                    {
                        int unicodeId = ConvertCode.ConvertCodeToInt(Encoding.ASCII.GetBytes(id), (byte)ReqMarketType.MT_Plate);
                        _unicodeList.Add(unicodeId);
                    }
                }
                else
                {
                    foreach (string id in _idInput)
                        _unicodeList.Add(Convert.ToInt32(id));
                }
            }
        }

        private bool _isBlock;

        /// <summary>
        /// 请求字段List
        /// </summary>
        public List<byte> FieldIndexList
        {
            get { return _fieldIndexList; }
            set
            {
                _fieldIndexList = value;
                _numFieldIndex = Convert.ToByte(_fieldIndexList.Count);
            }
        }

        /// <summary>
        /// 0-1字节；其它-2个字节
        /// </summary>
        private bool _bNumFieldIndex;

        /// <summary>
        /// 栏位个数
        /// </summary>
        private short _numFieldIndex;

        private List<byte> _fieldIndexList;

        private List<int> _unicodeList;
        private List<string> _idInput;
        /// <summary>
        /// 
        /// </summary>
        public List<string> IdInput
        {
            get { return _idInput; }
            set { _idInput = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public ReqSectorQuoteReportDataPacket(List<string> id)
        {
            RequestType = FuncTypeRealTime.SectorQuoteReport;
            IsResponseData = true;
            IsPush = true;
            _bNumFieldIndex = false;
            _unicodeList = new List<int>(id.Count);
            _idInput = id;
            _isBlock = false;
        }
        /// <summary>
        /// 
        /// </summary>
        public ReqSectorQuoteReportDataPacket(string id)
        {
            RequestType = FuncTypeRealTime.SectorQuoteReport;
            IsResponseData = true;
            IsPush = true;
            _bNumFieldIndex = false;
            _unicodeList = new List<int>(1);
            _idInput = new List<string>(1);
            _idInput.Add(id);
            _isBlock = false;
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(IsResponseData);
            bw.Write(IsPush);
            bw.Write(IsBlock);
            bw.Write((byte)_unicodeList.Count);

            for (int i = 0; i < _unicodeList.Count; i++)
            {
                if (!_isBlock)
                    bw.Write(Convert.ToByte(_unicodeList[i]));
                else
                    bw.Write(Convert.ToUInt32(_unicodeList[i]));
            }


            bw.Write(_bNumFieldIndex);
            bw.Write(_numFieldIndex);
            for (int i = 0; i < _numFieldIndex; i++)
                bw.Write(FieldIndexList[i]);
        }
    }

    /// <summary>
    /// 自选股类型的板块栏位报价
    /// </summary>
    public class ReqBlockIndexReportDataPacket : RealTimeDataPacket
    {
        /// <summary>
        /// 0-仅返回请求信息；1-返回请求信息和数据
        /// </summary>
        public bool IsResponseData;

        private List<byte> _fieldIndexList;
        /// <summary>
        /// 请求字段List
        /// </summary>
        public List<byte> FieldIndexList
        {
            get { return _fieldIndexList; }
            set
            {
                _fieldIndexList = value;
                _numFieldIndex = Convert.ToByte(_fieldIndexList.Count);
            }
        }

        /// <summary>
        /// 0-1字节；其它-2个字节
        /// </summary>
        private bool _bNumFieldIndex;

        /// <summary>
        /// 栏位个数
        /// </summary>
        private short _numFieldIndex;

        private List<int> _unicodeList;
        /// <summary>
        /// 
        /// </summary>
        public ReqBlockIndexReportDataPacket(string blockId)
        {
            RequestType = FuncTypeRealTime.BlockIndexReport;
            IsResponseData = true;
            IsPush = true;
            _bNumFieldIndex = false;

            _unicodeList = new List<int>(1);

            int unicodeId = ConvertCode.ConvertCodeToInt(Encoding.ASCII.GetBytes(blockId), (byte)ReqMarketType.MT_Plate);
            _unicodeList.Add(unicodeId);
        }
        /// <summary>
        /// 
        /// </summary>
        public ReqBlockIndexReportDataPacket(List<string> blockId)
        {
            RequestType = FuncTypeRealTime.BlockIndexReport;
            IsResponseData = true;
            IsPush = true;
            _bNumFieldIndex = false;

            _unicodeList = new List<int>(blockId.Count);
            for (int i = 0; i < blockId.Count; i++)
            {
                int unicodeId = ConvertCode.ConvertCodeToInt(Encoding.ASCII.GetBytes(blockId[i]), (byte)ReqMarketType.MT_Plate);
                _unicodeList.Add(unicodeId);
            }
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(IsResponseData);
            bw.Write(IsPush);
            bw.Write((byte)0);
            bw.Write((byte)_unicodeList.Count);

            for (int i = 0; i < _unicodeList.Count; i++)
                bw.Write(Convert.ToUInt32(_unicodeList[i]));

            bw.Write(_bNumFieldIndex);
            bw.Write(_numFieldIndex);
            for (int i = 0; i < _numFieldIndex; i++)
                bw.Write(FieldIndexList[i]);
        }
    }

    /// <summary>
    /// 成交明细请求
    /// </summary>
    public class ReqDealRequestDataPacket : RealTimeDataPacket
    {
        private byte _market;
        private string _shortCode;
        private int _code;

        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set
            {
                _code = value;

                string emcode = string.Empty;
                if (DetailData.FieldIndexDataString.ContainsKey(_code))
                    DetailData.FieldIndexDataString[_code].TryGetValue(FieldIndex.EMCode, out emcode);

                string shortcode = string.Empty;
                if (DetailData.FieldIndexDataString.ContainsKey(_code))
                    DetailData.FieldIndexDataString[_code].TryGetValue(FieldIndex.Code, out shortcode);

                MarketType mt = MarketType.NA;
                int mtInt = 0;
                if (DetailData.FieldIndexDataInt32.ContainsKey(_code))
                    DetailData.FieldIndexDataInt32[_code].TryGetValue(FieldIndex.Code, out mtInt);
                mt = (MarketType)mtInt;


                if (mt == MarketType.IF)
                {
                    if (shortcode == "IF00C1")
                        _shortCode = "040120\0";
                    else if (shortcode == "IF00C2")
                        _shortCode = "040121\0";
                    else if (shortcode == "IF00C3")
                        _shortCode = "040122\0";
                    else if (shortcode == "IF00C4")
                        _shortCode = "040123\0";
                    else if (!string.IsNullOrEmpty(shortcode))
                        _shortCode = "0411" + shortcode.Substring(shortcode.Length - 2, 2) + '\0';
                    _market = (byte)ReqMarketType.MT_IndexFutures;
                }
                else if (mt == MarketType.GoverFutures)
                {

                    if (shortcode == "TF00C1")
                        _shortCode = "050120\0";
                    else if (shortcode == "TF00C2")
                        _shortCode = "050121\0";
                    else if (shortcode == "TF00C3")
                        _shortCode = "050122\0";
                    else if (shortcode == "TF00C4")
                        _shortCode = "050123\0";
                    else if (!string.IsNullOrEmpty(shortcode))
                        _shortCode = "0511" + shortcode.Substring(shortcode.Length - 2, 2) + '\0';
                    _market = (byte)ReqMarketType.MT_IndexFutures;
                }
                else
                {
                    ReqMarketType tmp = ReqMarketType.MT_NA;
                    ParseCode(emcode, out tmp, out _shortCode);
                    _market = (byte)tmp;
                }
            }
        }

        /// <summary>
        /// 日期,格式如20120516
        /// </summary>
        public int Date;

        /// <summary>
        /// RecentNum = 0, 取所有记录, > 0 取最近多少条记录
        /// </summary>
        public int RecentNum;

        private int _time;
        /// <summary>
        /// 
        /// </summary>
        public ReqDealRequestDataPacket()
        {
            RequestType = FuncTypeRealTime.DealRequest;

        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(Date);
            bw.Write(_time);
            bw.Write(RecentNum);
            bw.Write(_market);
            bw.Write(Encoding.ASCII.GetBytes(_shortCode));

        }
    }

    /// <summary>
    /// 成交明细订阅
    /// </summary>
    public class ReqDealSubscribeDataPacket : RealTimeDataPacket
    {
        private byte _market;
        private string _shortCode;
        private int _code;

        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set
            {
                _code = value;
                string shortcode = string.Empty;
                if (DetailData.FieldIndexDataString.ContainsKey(_code))
                    DetailData.FieldIndexDataString[_code].TryGetValue(FieldIndex.Code, out shortcode);

                string emcode = string.Empty;
                if (DetailData.FieldIndexDataString.ContainsKey(_code))
                    DetailData.FieldIndexDataString[_code].TryGetValue(FieldIndex.EMCode, out emcode);

                MarketType mt = MarketType.NA;
                int mtInt = 0;
                if (DetailData.FieldIndexDataInt32.ContainsKey(_code))
                    DetailData.FieldIndexDataInt32[_code].TryGetValue(FieldIndex.Market, out mtInt);
                mt = (MarketType)mtInt;


                if (mt == MarketType.IF)
                {

                    if (shortcode == "IF00C1")
                        _shortCode = "040120\0";
                    else if (shortcode == "IF00C2")
                        _shortCode = "040121\0";
                    else if (shortcode == "IF00C3")
                        _shortCode = "040122\0";
                    else if (shortcode == "IF00C4")
                        _shortCode = "040123\0";
                    else if (!string.IsNullOrEmpty(shortcode))
                        _shortCode = "0411" + shortcode.Substring(shortcode.Length - 2, 2) + '\0';
                    _market = (byte)ReqMarketType.MT_IndexFutures;
                }
                else if (mt == MarketType.GoverFutures)
                {

                    if (shortcode == "TF00C1")
                        _shortCode = "050120\0";
                    else if (shortcode == "TF00C2")
                        _shortCode = "050121\0";
                    else if (shortcode == "TF00C3")
                        _shortCode = "050122\0";
                    else if (shortcode == "TF00C4")
                        _shortCode = "050123\0";
                    else if (!string.IsNullOrEmpty(shortcode))
                        _shortCode = "0511" + shortcode.Substring(shortcode.Length - 2, 2) + '\0';
                    _market = (byte)ReqMarketType.MT_IndexFutures;
                }
                else
                {
                    ReqMarketType tmp = ReqMarketType.MT_NA;
                    ParseCode(emcode, out tmp, out _shortCode);
                    _market = (byte)tmp;
                }
            }
        }

        /// <summary>
        /// 是否推送
        /// </summary>
        public override bool IsPush
        {
            set
            {
                if (value == false)
                {
                    PushType = 0;
                }
            }
        }

        /// <summary>
        /// 推送方式，0取消，1基于时间，2基于数目
        /// </summary>
        public byte PushType;

        /// <summary>
        /// PushType==1，Optional时间之后所有数据，PushType==2,最新Optional条数据
        /// </summary>
        public int Optional;
        /// <summary>
        /// 
        /// </summary>
        public ReqDealSubscribeDataPacket()
        {
            RequestType = FuncTypeRealTime.DealSubscribe;
            PushType = 2;
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(_market);
            bw.Write(Encoding.ASCII.GetBytes(_shortCode));
            bw.Write(PushType);
            bw.Write(Optional);
            if (Optional < 0)
            {
                LogUtilities.LogMessage("!!!!!!!!=========!!!!!!!!!!包请求数据小于0");
            }
        }
    }

    /// <summary>
    /// 个股资金流
    /// </summary>
    public class ReqCapitalFlow : RealTimeDataPacket
    {


        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set
            {
                _code = value;
                ReqMarketType tmp = ReqMarketType.MT_NA;
                string emcode = string.Empty;
                if (DetailData.FieldIndexDataString.ContainsKey(_code))
                    DetailData.FieldIndexDataString[_code].TryGetValue(FieldIndex.EMCode, out emcode);

                ParseCode(emcode, out tmp, out _shortCode);
                _market = (byte)tmp;

                _unicode = ConvertCode.ConvertCodeToInt(Encoding.ASCII.GetBytes(_shortCode), _market);
            }
        }

        private byte _market;
        private string _shortCode;
        private int _code;
        private int _unicode;
        /// <summary>
        /// 
        /// </summary>
        public ReqCapitalFlow()
        {
            RequestType = FuncTypeRealTime.CapitalFlow;
            IsPush = true;
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(_unicode);
            bw.Write(IsPush);
        }
    }

    /// <summary>
    /// 分价表
    /// </summary>
    public class ReqPriceStatusDataPacket : RealTimeDataPacket
    {

        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set
            {
                _code = value;
                ReqMarketType tmp = ReqMarketType.MT_NA;
                string emcode = string.Empty;
                if (DetailData.FieldIndexDataString.ContainsKey(_code))
                    DetailData.FieldIndexDataString[_code].TryGetValue(FieldIndex.EMCode, out emcode);
                ParseCode(emcode, out tmp, out _shortCode);
                _market = (byte)tmp;

                _unicode = ConvertCode.ConvertCodeToInt(Encoding.ASCII.GetBytes(_shortCode), _market);
            }
        }

        private byte _market;
        private string _shortCode;
        private int _code;
        private int _unicode;
        /// <summary>
        /// 
        /// </summary>
        public ReqPriceStatusDataPacket()
        {
            RequestType = FuncTypeRealTime.PriceStatus;
            IsPush = true;
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(_unicode);
            bw.Write(IsPush);
        }
    }

    /// <summary>
    /// 股指期货分价表
    /// </summary>
    public class ReqIndexFuturesPriceStatusDataPacket : ReqPriceStatusDataPacket
    {
        /// <summary>
        /// construct
        /// </summary>
        public ReqIndexFuturesPriceStatusDataPacket()
        {
            RequestType = FuncTypeRealTime.IndexFutruesPriceStatus;
        }

    }

    /// <summary>
    /// 综合排名
    /// </summary>
    public class ReqRankDataRacket : RealTimeDataPacket
    {
        /// <summary>
        /// 证券类别码
        /// </summary>
        public ReqSecurityType SType;
        /// <summary>
        /// 
        /// </summary>
        public ReqRankDataRacket()
        {
            RequestType = FuncTypeRealTime.Rank;
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write((byte)SType);
        }
    }

    /// <summary>
    /// F10
    /// </summary>
    public class ReqF10DataPacket : RealTimeDataPacket
    {
        private byte _market;
        private string _shortCode;
        private int _code;

        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set
            {
                _code = value;
                ReqMarketType tmp = ReqMarketType.MT_NA;
                string emcode = string.Empty;
                if (DetailData.FieldIndexDataString.ContainsKey(_code))
                    DetailData.FieldIndexDataString[_code].TryGetValue(FieldIndex.EMCode, out emcode);

                ParseCode(emcode, out tmp, out _shortCode);
                _market = (byte)tmp;
            }
        }

        /// <summary>
        /// F10栏位标记
        /// </summary>
        public byte F10Field;

        /// <summary>
        /// 上次更新日期,格式YYYYMMDD
        /// </summary>
        public int Date;

        /// <summary>
        /// 上次更新时间，格式HHMMSS
        /// </summary>
        public int Time;
        /// <summary>
        /// 
        /// </summary>
        public ReqF10DataPacket()
        {
            RequestType = FuncTypeRealTime.ReqF10;

        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(_market);
            bw.Write(Encoding.ASCII.GetBytes(_shortCode));
            bw.Write(F10Field);
            bw.Write(Date);
            bw.Write(Time);
        }
    }
    /// <summary>
    /// 历史分时资金流 请求
    /// </summary>
    public class ReqHisTrendlinecfsDataPacket : RealTimeDataPacket
    {
        private int _date;
        private int _preDay;
        private int _stockID;
        private int _code;
        private byte _market;
        private string _shortCode;
        /// <summary>
        /// 股票内码
        /// </summary>
        public int Code { get { return _code; } set { _code = value; } }
        /// <summary>
        /// 股票StockID
        /// </summary>
        public int StockID { get { return _stockID; } set { _stockID = SetCode(_code); } }


        private int SetCode(int code)
        {
            ReqMarketType tmp = ReqMarketType.MT_NA;
            string emcode = string.Empty;
            if (DetailData.FieldIndexDataString.ContainsKey(_code))
                DetailData.FieldIndexDataString[_code].TryGetValue(FieldIndex.EMCode, out emcode);

            ParseCode(emcode, out tmp, out _shortCode);
            _market = (byte)tmp;

            return ConvertCode.ConvertCodeToInt(Encoding.ASCII.GetBytes(_shortCode), _market);
        }
        /// <summary>
        /// 更新日期
        /// </summary>
        public int Date
        {
            get { return _date; }
            set { _date = value; }
        }
        /// <summary>
        /// 0 当天， 1 之前一天, -1 之后
        /// </summary>
        public int PreDay
        {
            get { return _preDay; }
            set { _preDay = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ReqHisTrendlinecfsDataPacket()
        {
            RequestType = FuncTypeRealTime.HisTrendlinecfs;
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(_date);
            bw.Write(_preDay);
            bw.Write(_stockID);
        }
    }
    /// <summary>
    /// 资金流日K线 请求包
    /// </summary>
    public class ReqCapitalFlowDayDataPacket : RealTimeDataPacket
    {
        private int _code;
        private byte _market;
        private string _shortCode;

        /// <summary>
        /// 市场代码
        /// </summary>
        public byte Market { get { return _market; } set { _market = value; } }
        /// <summary>
        /// 股票代码(7个Byte)
        /// </summary>
        public string ShortCode { get { return _shortCode; } set { _shortCode = value; } }
        /// <summary>
        /// 请求数据类型数据模式
        /// </summary>
        public KLineCycle DataType;
        /// <summary>
        /// 请求类型
        /// </summary>
        public ReqKLineDataRange DataRange;

        /// <summary>
        /// 开始日期[YYYYMMDD]
        /// </summary>
        public Int32 StartDate;
        /// <summary>
        /// 结束日期[YYYYMMDD]
        /// </summary>
        public Int32 EndDate;
        /// <summary>
        /// 请求日线（5分钟线）最大个数
        /// </summary>
        public Int32 ApplySize;

        /// <summary>
        /// 股票代码(内码=> 股票代码和市场)
        /// </summary>
        public int Code
        {
            get { return _code; }
            set
            {
                _code = value;

                string shortcode = string.Empty;
                if (DetailData.FieldIndexDataString.ContainsKey(_code))
                    DetailData.FieldIndexDataString[_code].TryGetValue(FieldIndex.Code, out shortcode);

                string emcode = string.Empty;
                if (DetailData.FieldIndexDataString.ContainsKey(_code))
                    DetailData.FieldIndexDataString[_code].TryGetValue(FieldIndex.EMCode, out emcode);

                MarketType mt = MarketType.NA;
                int mtInt = 0;
                if (DetailData.FieldIndexDataInt32.ContainsKey(_code))
                    DetailData.FieldIndexDataInt32[_code].TryGetValue(FieldIndex.Market, out mtInt);
                mt = (MarketType)mtInt;


                if (mt == MarketType.IF)
                {

                    if (emcode == "IF00C1.CFE")
                        _shortCode = "040120\0";
                    else if (emcode == "IF00C2.CFE")
                        _shortCode = "040121\0";
                    else if (emcode == "IF00C3.CFE")
                        _shortCode = "040122\0";
                    else if (emcode == "IF00C4.CFE")
                        _shortCode = "040123\0";
                    else
                        _shortCode = "0411" + emcode.Substring(emcode.Length - 6, 2) + '\0';
                    _market = (byte)ReqMarketType.MT_IndexFutures;
                }
                else if (mt == MarketType.GoverFutures)
                {

                    if (emcode == "TF00C1.CFE")
                        _shortCode = "050120\0";
                    else if (emcode == "TF00C2.CFE")
                        _shortCode = "050121\0";
                    else if (emcode == "TF00C3.CFE")
                        _shortCode = "050122\0";
                    else if (emcode == "TF00C4.CFE")
                        _shortCode = "050123\0";
                    else
                        _shortCode = "0511" + emcode.Substring(emcode.Length - 6, 2) + '\0';
                    _market = (byte)ReqMarketType.MT_IndexFutures;
                }
                else
                {
                    ReqMarketType tmp = ReqMarketType.MT_NA;
                    ParseCode(emcode, out tmp, out _shortCode);
                    _market = (byte)tmp;
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public ReqCapitalFlowDayDataPacket()
        {
            RequestType = FuncTypeRealTime.CapitalFlowDay;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code">股票代码(内码)</param>
        /// <param name="kLineCycle">K线周期</param>
        /// <param name="dataRange">请求K线数据范围</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        /// <param name="applySize">请求K线最大个数</param>
        public ReqCapitalFlowDayDataPacket(int code, KLineCycle kLineCycle, ReqKLineDataRange dataRange, int startDate, int endDate, int applySize)
            : this()
        {
            this.Code = code;
            this.DataType = kLineCycle;
            this.DataRange = dataRange;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.ApplySize = applySize;
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(Market);
            bw.Write(Encoding.ASCII.GetBytes(_shortCode));
            bw.Write((byte)DataType);
            bw.Write((byte)DataRange);
            bw.Write(StartDate);
            bw.Write(EndDate);
            bw.Write(ApplySize);
        }
    }


    /// <summary>
    /// 逐笔成交
    /// </summary>
    public class ReqTickDataPacket : RealTimeDataPacket
    {

        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set
            {
                _code = value;
                ReqMarketType tmp = ReqMarketType.MT_NA;
                string emcode = string.Empty;
                if (DetailData.FieldIndexDataString.ContainsKey(_code))
                    DetailData.FieldIndexDataString[_code].TryGetValue(FieldIndex.EMCode, out emcode);

                ParseCode(emcode, out tmp, out _shortCode);
                _market = (byte)tmp;

                _unicode = ConvertCode.ConvertCodeToInt(Encoding.ASCII.GetBytes(_shortCode), _market);
            }
        }

        /// <summary>
        /// 数量，0表示取所有的
        /// </summary>
        public int Num;

        /// <summary>
        /// 时间，0表示取最近的Num条数据
        /// </summary>
        public int Time;

        /// <summary>
        /// Time和TradeNo确定的唯一一条数据
        /// </summary>
        public int TradeNo;

        /// <summary>
        /// 数据下标，-1表示从第一条向后取
        /// </summary>
        public int Index;

        /// <summary>
        /// 获取数据方式
        /// </summary>
        public ReqTickFlag Flag;

        private byte _market;
        private string _shortCode;
        private int _code;
        private int _unicode;
        /// <summary>
        /// 
        /// </summary>
        public ReqTickDataPacket()
        {
            RequestType = FuncTypeRealTime.TickTrade;
            IsPush = true;
            Flag = 0;
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(_unicode);
            bw.Write(IsPush);
            bw.Write((byte)Flag);
            bw.Write(Time);
            bw.Write(TradeNo);
            bw.Write(Num);
            bw.Write(Index);
        }
    }

    /// <summary>
    /// 委托明细(深证)
    /// </summary>
    public class ReqOrderDetailDataPacket : ReqTickDataPacket
    {
        /// <summary>
        /// 
        /// </summary>
        public ReqOrderDetailDataPacket()
        {
            RequestType = FuncTypeRealTime.OrderDetail;
        }
    }

    /// <summary>
    /// 委托队列
    /// </summary>
    public class ReqOrderQueueDataPacket : RealTimeDataPacket
    {
        private byte _market;
        private string _shortCode;
        private int _code;
        private int _unicode;

        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set
            {
                _code = value;
                ReqMarketType tmp = ReqMarketType.MT_NA;
                string emcode = string.Empty;
                if (DetailData.FieldIndexDataString.ContainsKey(_code))
                    DetailData.FieldIndexDataString[_code].TryGetValue(FieldIndex.EMCode, out emcode);

                ParseCode(emcode, out tmp, out _shortCode);
                _market = (byte)tmp;

                _unicode = ConvertCode.ConvertCodeToInt(Encoding.ASCII.GetBytes(_shortCode), _market);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ReqOrderQueueDataPacket()
        {
            RequestType = FuncTypeRealTime.OrderQueue;
            IsPush = true;
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(_unicode);
            bw.Write(IsPush);
        }

    }

    /// <summary>
    /// 短线精灵
    /// </summary>
    public class ReqShortLineStrategyDataPacket : RealTimeDataPacket
    {
        /// <summary>
        /// 是否推送，0取消，1推送，5只取一次
        /// </summary>
        public byte IsPush;

        /// <summary>
        /// 已有数据的最后一个id
        /// </summary>
        public int LastId;

        /// <summary>
        /// 已有数据的最后一次时间
        /// </summary>
        public int LastTime;

        /// <summary>
        /// 返回最大条数，0为无限制
        /// </summary>
        public int Count;
        /// <summary>
        /// 
        /// </summary>
        public ReqShortLineStrategyDataPacket()
        {
            RequestType = FuncTypeRealTime.ShortLineStrategy;
            IsPush = 1;
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write((byte)0);//消息分类
            bw.Write(IsPush);//是否推送，0取消，1推送，5只取一次
            bw.Write(LastId);//lastId
            bw.Write(1);//msgType
            bw.Write(LastTime);//lasttime
            bw.Write(Count);//返回最大条数
        }
    }

    /// <summary>
    /// 个股贡献指数点数
    /// </summary>
    public class ReqContributionStockDataPacket : RealTimeDataPacket
    {
        /// <summary>
        /// 市场标记，0深圳,1上海
        /// </summary>
        public byte MarketFlag;
        /// <summary>
        /// 
        /// </summary>
        public ReqContributionStockDataPacket()
        {
            RequestType = FuncTypeRealTime.ContributionStock;
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(MarketFlag);
        }
    }

    /// <summary>
    /// 板块贡献指数点数
    /// </summary>
    public class ReqContributionBlockDataPacket : RealTimeDataPacket
    {
        /// <summary>
        /// 市场标记，0深圳,1上海
        /// </summary>
        public byte MarketFlag;
        /// <summary>
        /// 
        /// </summary>
        public ReqContributionBlockDataPacket()
        {
            RequestType = FuncTypeRealTime.ContributionBlock;
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(MarketFlag);
        }
    }


    /// <summary>
    /// 统计明细
    /// </summary>
    public class ReqStatisticsAnalysisDataPacket : RealTimeDataPacket
    {
        private int _code;
        public string Url;
        public int Code
        {
            set
            {
                _code = value;
                try
                {
                    string emcode = string.Empty;
                    if (DetailData.FieldIndexDataString.ContainsKey(_code))
                        DetailData.FieldIndexDataString[_code].TryGetValue(FieldIndex.EMCode, out emcode);


                    string[] codeArray = emcode.Split('.');
                    Url = string.Format(@"{0}/{1}.dat", codeArray[1].ToLower(), codeArray[0]);

                }
                catch (Exception e)
                {
                    LogUtilities.LogMessage("统计分析code转url发生错误" + e.Message);
                }

            }
        }
        public ReqStatisticsAnalysisDataPacket()
        {
            RequestType = FuncTypeRealTime.StatisticsAnalysis;
        }
    }


    /// <summary>
    /// 分时资金流
    /// </summary>
    public class ReqTrendCaptialFlowDataPacket : RealTimeDataPacket
    {
        private byte _market;
        private string _shortCode;
        private int _code;

        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set
            {
                _code = value;
                ReqMarketType tmp = ReqMarketType.MT_NA;
                string emcode = string.Empty;
                if (DetailData.FieldIndexDataString.ContainsKey(_code))
                    DetailData.FieldIndexDataString[_code].TryGetValue(FieldIndex.EMCode, out emcode);

                ParseCode(emcode, out tmp, out _shortCode);
                _market = (byte)tmp;
            }
        }

        /// <summary>
        /// 上次请求的下标
        /// </summary>
        public int LastRequestPoint;

        public new bool IsPush
        {
            get
            {
                if (_isPush == 0)
                {
                    return false;
                }
                else
                    return true;
            }

            set
            {
                if (value)
                    _isPush = 2;
                else
                    _isPush = 0;
            }
        }
        private byte _isPush = 2;
        public ReqTrendCaptialFlowDataPacket()
        {
            RequestType = FuncTypeRealTime.TrendCapitalFlow;
            IsPush = true;
        }

        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(_market);
            bw.Write(Encoding.ASCII.GetBytes(_shortCode));
            bw.Write(_isPush);
            bw.Write(LastRequestPoint);
        }
    }
    #endregion

    #region 财富通资讯接口

    /// <summary>
    /// 心跳
    /// </summary>
    public class ReqInfoHeart : InfoDataPacket
    {
        /// <summary>
        /// 
        /// </summary>
        public ReqInfoHeart()
        {
            IsInfoHeartPacket = true;
            RequestType = FuncTypeInfo.InfoHeart;
        }
    }

    /// <summary>
    /// 24小时滚动新闻
    /// </summary>
    public class ReqNews24DataPacket : InfoDataPacket
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public int DateStart;
        /// <summary>
        /// 开始时间
        /// </summary>
        public int TimeStart;
        /// <summary>
        /// 结束日期
        /// </summary>
        public int DateEnd;
        /// <summary>
        /// 结束时间
        /// </summary>
        public int TimeEnd;
        /// <summary>
        /// 最大数量
        /// </summary>
        public short MaxNum;

        /// <summary>
        /// 
        /// </summary>
        public ReqNews24DataPacket()
        {
            RequestType = FuncTypeInfo.News24;
        }

        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(DateStart);
            bw.Write(TimeStart);
            bw.Write(DateEnd);
            bw.Write(TimeEnd);
            bw.Write(9999);
            bw.Write(MaxNum);
        }
    }

    /// <summary>
    /// 板块成分结构等信息
    /// </summary>
    public class ReqBlockInfoDataPacket : InfoDataPacket
    {
        /// <summary>
        /// 日期
        /// </summary>
        public int Date;
        /// <summary>
        /// 时间
        /// </summary>
        public int Time;

        /// <summary>
        /// 
        /// </summary>
        public ReqBlockInfoDataPacket()
        {
            RequestType = FuncTypeInfo.Block;
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(Date);
            bw.Write(Time);
        }
    }

    /// <summary>
    /// 新闻研报
    /// </summary>
    public class ReqNewsReportDataPacket : InfoDataPacket
    {
        private byte _market;
        private string _shortCode;
        private int _code;

        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set
            {
                _code = value;
                ReqMarketType tmp = ReqMarketType.MT_NA;
                string emcode = string.Empty;
                if (DetailData.FieldIndexDataString.ContainsKey(_code))
                    DetailData.FieldIndexDataString[_code].TryGetValue(FieldIndex.EMCode, out emcode);

                ParseCode(emcode, out tmp, out _shortCode);
                _market = (byte)tmp;
            }
        }
        /// <summary>
        /// 开始日期
        /// </summary>
        public int StartDate;
        /// <summary>
        /// 开始时间
        /// </summary>
        public int StartTime;
        /// <summary>
        /// 结束日期
        /// </summary>
        public int EndDate;
        /// <summary>
        /// 结束时间
        /// </summary>
        public int EndTime;



        /// <summary>
        /// 返回的最大记录数（0为不限制返回个数）
        /// </summary>
        public int MaxCount;

        /// <summary>
        /// 信息地雷的类型
        /// </summary>
        public InfoMine InfoType;

        /// <summary>
        /// 
        /// </summary>
        public ReqNewsReportDataPacket()
        {
            RequestType = FuncTypeInfo.NewsReport;
            InfoType = InfoMine.News;
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(StartDate);
            bw.Write(StartTime);
            bw.Write(EndDate);
            bw.Write(EndTime);
            bw.Write(Encoding.ASCII.GetBytes(_shortCode));
            bw.Write(_market);
            bw.Write(Convert.ToUInt16(InfoType));
            bw.Write((ushort)0xFFFF);
            bw.Write((ushort)MaxCount);
        }
    }


    /// <summary>
    /// 自选股新闻
    /// </summary>
    public class ReqCustomStockNewsDataPacket : InfoDataPacket
    {
        /// <summary>
        /// 返回的最大记录数（0为不限制返回个数）
        /// </summary>
        public ushort MaxCount;

        /// <summary>
        /// 信息地雷的类型
        /// </summary>
        public InfoMine InfoType;

        /// <summary>
        /// 每个自选股参数
        /// </summary>
        public List<CustomStockNewsParam> StockParams;

        private int stockCount;

        /// <summary>
        /// construct
        /// </summary>
        public ReqCustomStockNewsDataPacket()
        {
            RequestType = FuncTypeInfo.CustomStockNewsReport;
            stockCount = 0;
        }

        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(1);
            bw.Write(Convert.ToUInt16(InfoType));
            bw.Write((ushort)0xFFFF);
            bw.Write(MaxCount);
            if (StockParams != null)
            {
                foreach (CustomStockNewsParam customStockNewsParam in StockParams)
                {
                    if (customStockNewsParam.IsValide)
                        stockCount++;
                }
                bw.Write(stockCount);

                string _shortCode;
                byte _market;

                foreach (CustomStockNewsParam customStockNewsParam in StockParams)
                {
                    if (customStockNewsParam.IsValide)
                    {
                        customStockNewsParam.GetCode(out _shortCode, out _market);
                        bw.Write(Encoding.ASCII.GetBytes(_shortCode));
                        bw.Write(_market);
                        bw.Write(customStockNewsParam.DateStart);
                        bw.Write(customStockNewsParam.TimeStart);
                        bw.Write(customStockNewsParam.DateEnd);
                        bw.Write(customStockNewsParam.TimeEnd);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 财务数据
    /// </summary>
    public class ReqFinanceDataPacket : InfoDataPacket
    {
        /// <summary>
        /// 日期
        /// </summary>
        public int Date;
        /// <summary>
        /// 时间
        /// </summary>
        public int Time;
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReqFinanceDataPacket()
        {
            RequestType = FuncTypeInfo.Finance;
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(Date);
            bw.Write(Time);
        }
    }

    /// <summary>
    /// 机构评级
    /// </summary>
    public class ReqOrgRateDataPacket : InfoDataPacket
    {
        private byte _market;
        private string _shortCode;
        private int _code;

        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set
            {
                _code = value;
                ReqMarketType tmp = ReqMarketType.MT_NA;
                string emcode = string.Empty;
                if (DetailData.FieldIndexDataString.ContainsKey(_code))
                    DetailData.FieldIndexDataString[_code].TryGetValue(FieldIndex.EMCode, out emcode);

                ParseCode(emcode, out tmp, out _shortCode);
                _market = (byte)tmp;
            }
        }
        /// <summary>
        /// 日期
        /// </summary>
        public int Date;
        /// <summary>
        /// 时间
        /// </summary>
        public int Time;

        /// <summary>
        /// 最大返回个数(0为不限制)
        /// </summary>
        public ushort MaxCount;

        /// <summary>
        /// 
        /// </summary>
        public ReqOrgRateDataPacket()
        {
            RequestType = FuncTypeInfo.OrgRate;
        }

        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(_market);
            bw.Write(Encoding.ASCII.GetBytes(_shortCode));
            bw.Write(Date);
            bw.Write(Time);
            bw.Write(MaxCount);
        }
    }

    /// <summary>
    /// 盈利预测
    /// </summary>
    public class ReqProfitForecastDataPacket : InfoDataPacket
    {
        /// <summary>
        /// 
        /// </summary>
        public ReqProfitForecastDataPacket()
        {
            RequestType = FuncTypeInfo.ProfitForecast;
        }
    }

    /// <summary>
    /// 除权除息
    /// </summary>
    public class ReqDivideRightDataPacket : InfoDataPacket
    {
        /// <summary>
        /// 上一个日期
        /// </summary>
        public int LastDate;
        /// <summary>
        /// 上一个时间
        /// </summary>
        public int LastTime;

        /// <summary>
        /// 
        /// </summary>
        public ReqDivideRightDataPacket()
        {
            RequestType = FuncTypeInfo.DivideRight;
        }

        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(LastDate);
            bw.Write(LastTime);
        }
    }
    #endregion

    #region 机构版接口

    /// <summary>
    /// 机构服务器心跳
    /// </summary>
    public class ReqHeartOrgDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 
        /// </summary>
        public ReqHeartOrgDataPacket()
        {
            RequestType = FuncTypeOrg.HeartOrg;
        }
    }

    /// <summary>
    /// 历史交易日
    /// </summary>
    public class ReqTradeDateDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 
        /// </summary>
        public ReqTradeDateDataPacket()
        {
            RequestType = FuncTypeOrg.TradeDate;
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write((byte)0);
        }
    }

    /// <summary>
    /// 走势机构版
    /// </summary>
    public class ReqTrendOrgDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 日期
        /// </summary>
        public int Date;

        /// <summary>
        /// 时间
        /// </summary>
        private int _time;

        /// <summary>
        /// 请求走势最后一个点的下标
        /// </summary>
        public int LastRequestPoint
        {
            get { return _lastRequestPoint; }
            set
            {
                _lastRequestPoint = value;
                _time = TimeUtilities.GetTimeFromPoint(Code, _lastRequestPoint);
            }
        }

        /// <summary>
        /// 0盘前，1盘中，2都要
        /// </summary>
        public byte TrendFlag;

        private int _code;
        private int _lastRequestPoint;

        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set
            {
                _code = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ReqTrendOrgDataPacket()
        {
            RequestType = FuncTypeOrg.TrendOrg;
            TrendFlag = 2;
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write((long)_code);
            bw.Write(TrendFlag);
            bw.Write(Date);
            bw.Write(_time);
        }
    }

    /// <summary>
    /// 走势机构版()
    /// </summary>
    public class ReqTrendOrgLowDataPacket : ReqTrendOrgDataPacket
    {
        public ReqTrendOrgLowDataPacket()
        {
            RequestType = FuncTypeOrg.TrendOrgDP;
        }
    }

    /// <summary>
    /// 机构版历史K线
    /// </summary>
    public class ReqHisKLineOrgDataPacket : OrgDataPacket
    {
        private int _code;

        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set
            {
                _code = value;
            }
        }

        /// <summary>
        /// K线周期
        /// </summary>
        public KLineCycle Cycle;

        /// <summary>
        /// 请求K线的数据范围
        /// </summary>
        public ReqKLineDataRange DataRange;

        /// <summary>
        /// 开始日期 格式[YYYYMMDD]
        /// </summary>
        public int StartDate;

        /// <summary>
        /// 结束日期 格式[YYYYMMDD]
        /// </summary>
        public int EndDate;

        /// <summary>
        /// 请求K线的个数
        /// </summary>
        public int ApplySize;

        /// <summary>
        /// 
        /// </summary>
        public ReqHisKLineOrgDataPacket()
        {
            RequestType = FuncTypeOrg.HisKLineOrg;
            Cycle = KLineCycle.CycleDay;
            DataRange = ReqKLineDataRange.SizeToNow;
        }

        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write((long)_code);
            //if (DetailData.AllStockDetailData.ContainsKey(_code))
            //    bw.Write(ConvertCodeOrg.ConvertCodeToLong((string)DetailData.AllStockDetailData[_code][FieldIndex.EMCode]));
            KLineCycleOrg cycleOrg = (KLineCycleOrg)Enum.Parse(typeof(KLineCycleOrg), Cycle.ToString());
            bw.Write((short)cycleOrg);
            //DataRange = ReqKLineDataRange.All;
            bw.Write((byte)DataRange);
            bw.Write(StartDate);
            bw.Write(EndDate);
            bw.Write(ApplySize);
        }
    }

    /// <summary>
    /// 机构版当日K线
    /// </summary>
    public class ReqMintKLineOrgDataPacket : ReqHisKLineOrgDataPacket
    {
        /// <summary>
        /// 
        /// </summary>
        public ReqMintKLineOrgDataPacket()
        {
            RequestType = FuncTypeOrg.MinKLineOrg;
        }
    }

    /// <summary>
    /// 机构版当日K线
    /// </summary>
    public class ReqMintKLineOrgLowDataPacket : ReqMintKLineOrgDataPacket
    {
        public ReqMintKLineOrgLowDataPacket()
        {
            RequestType = FuncTypeOrg.MinKLineOrgDP;
        }
    }

    /// <summary>
    /// 指定代码的报价列表行情
    /// </summary>
    public class ReqBlockReportDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 0-仅返回请求信息；1-返回请求信息和数据
        /// </summary>
        public bool IsResponseData;


        /// <summary>
        /// 字段标记，1实时，2静态，3混合
        /// </summary>
        public byte FieldFlag;

        //private List<long> _unicodeList;
        private List<string> _idInput;
        /// <summary>
        /// 栏位个数
        /// </summary>
        private byte _numFieldIndex;

        private List<short> _fieldIndexList;

        /// <summary>
        /// 请求字段List
        /// </summary>
        public List<short> FieldIndexList
        {
            get { return _fieldIndexList; }
            set
            {
                _fieldIndexList = value;
                _numFieldIndex = Convert.ToByte(_fieldIndexList.Count);
            }
        }

        // public ReqBlockReportDataPacket(List<string> id)
        //{
        //    RequestType = FuncTypeOrg.BlockReport;
        //    IsResponseData = true;
        //    IsPush = true;
        //    _idInput = id;
        //}
        /// <summary>
        /// 
        /// </summary>
        public ReqBlockReportDataPacket(string id)
        {
            if (id == "001004")
            {
                DateTime dt = DateTime.Now;
                Debug.Print("发请求  m=" + dt.Minute + "  s=" + dt.Second + "  ms=" + dt.Millisecond);
            }
            RequestType = FuncTypeOrg.BlockReport;
            IsResponseData = true;
            IsPush = true;
            _idInput = new List<string>(1);
            _idInput.Add("91" + id);
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            Debug.Print("发送报价请求：" + DateTime.Now.ToLongTimeString() + "ms" + DateTime.Now.Millisecond);
            base.Coding(bw);
            bw.Write(IsResponseData);
            bw.Write(IsPush);
            bw.Write((short)_idInput.Count);

            for (int i = 0; i < _idInput.Count; i++)
            {
                bw.Write(Convert.ToInt64(_idInput[i]));
            }
            bw.Write(FieldFlag);
            bw.Write(_numFieldIndex);
            for (int i = 0; i < _numFieldIndex; i++)
                bw.Write(FieldIndexList[i]);
        }


    }
    /// <summary>
    /// 国债综合屏-“公开市场操作”模块的数据请求
    /// </summary>
    public class ReqBondDashboardPublicMarketOpeartion : OrgDataPacket
    {
        /// <summary>
        /// 0-仅返回请求信息；1-返回请求信息和数据
        /// </summary>
        public const bool IsResponseData = true;

        /// <summary>
        /// 字段标记，1实时，2静态，3混合
        /// </summary>
        public const byte FieldFlag = 2;

        /// <summary>
        /// 栏位个数
        /// </summary>
        public const byte NumFieldIndex = 5;

        /// <summary>
        /// 请求字段List
        /// </summary>
        public short[] FieldIndexList ={ 7001, 17002, 17003, 17004, 17005 };

        public string[] _idInput ={ "911" };

        /// <summary>
        /// 构造该请求包体
        /// </summary>
        public ReqBondDashboardPublicMarketOpeartion()
        {
            RequestType = FuncTypeOrg.BondPublicOpeartion;
            //IsResponseData = true;
            IsPush = true;
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            Debug.Print("发送报价请求：" + DateTime.Now.ToLongTimeString() + "ms" + DateTime.Now.Millisecond);
            base.Coding(bw);
            bw.Write(IsResponseData);
            bw.Write(IsPush);
            bw.Write((short)_idInput.Length);

            for (int i = 0; i < _idInput.Length; i++)
            {
                bw.Write(Convert.ToInt64(_idInput[i]));
            }

            bw.Write(FieldFlag);
            bw.Write(NumFieldIndex);
            for (int i = 0; i < NumFieldIndex; i++)
                bw.Write(FieldIndexList[i]);
        }
    }
    /// <summary>
    /// 板块指数报价列表行情
    /// </summary>
    public class ReqBlockIndexReportOrgDataPacket : ReqBlockReportDataPacket
    {
        /// <summary>
        /// 
        /// </summary>
        public ReqBlockIndexReportOrgDataPacket(string id)
            : base(id)
        {
            RequestType = FuncTypeOrg.BlockIndexReport;
        }
    }

    /// <summary>
    /// 全球指数
    /// </summary>
    public class ReqGlobalIndexReportOrgDataPacket : ReqBlockReportDataPacket
    {
        public ReqGlobalIndexReportOrgDataPacket(string id)
            : base(id)
        {
            RequestType = FuncTypeOrg.GlobalIndexReport;
        }
    }

    /// <summary>
    /// 东财指数
    /// </summary>
    public class ReqEmIndexReportOrgDataPacket : ReqBlockReportDataPacket
    {
        public ReqEmIndexReportOrgDataPacket(string id)
            : base(id)
        {
            RequestType = FuncTypeOrg.EmIndexReport;
        }
    }

    /// <summary>
    /// 指数静态数据
    /// </summary>
    public class ReqIndexStaticOrgDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        public int Code;
        public ReqIndexStaticOrgDataPacket()
        {
            RequestType = FuncTypeOrg.IndexStatic;
            IsPush = true;
        }

        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            int objMarket = -1;

            if (DetailData.FieldIndexDataInt32.ContainsKey(Code))
                DetailData.FieldIndexDataInt32[Code].TryGetValue(FieldIndex.Market, out objMarket);

            string emcode = string.Empty;
            if (DetailData.FieldIndexDataString.ContainsKey(Code))
                DetailData.FieldIndexDataString[Code].TryGetValue(FieldIndex.EMCode, out emcode);

            if (objMarket >= 0)
            {
                MarketType mt = (MarketType)objMarket;
                switch (mt)
                {
                    case MarketType.SHINDEX:
                    case MarketType.SZINDEX:
                        long sid = ConvertCodeOrg.ConvertCodeToLong(emcode);
                        bw.Write(sid);
                        bw.Write(IsPush);
                        return;
                }
            }
            bw.Write((long)Code);
            bw.Write(IsPush);
        }
    }

    /// <summary>
    /// 自选股方式报价列表行情
    /// </summary>
    public class ReqCustomReportOrgDataPacket : OrgDataPacket
    {
        private List<short> _fieldIndexList;
        public List<int> CustomCodeList;
        public byte FieldFlag;
        public bool IsResponseData;

        public ReqCustomReportOrgDataPacket()
        {
            base.RequestType = FuncTypeOrg.CustomReport;
            this.IsResponseData = true;
            this.IsPush = true;
            this.CustomCodeList = new List<int>();
        }

        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(this.IsResponseData);
            bw.Write(this.IsPush);
            bw.Write((short)this.CustomCodeList.Count);
            for (int i = 0; i < this.CustomCodeList.Count; i++)
            {
                long num2 = ConvertCodeOrg.CommonConvertUnicodeToLong(this.CustomCodeList[i]);
                bw.Write(num2);
            }
            bw.Write(this.FieldFlag);
            byte num3 = Convert.ToByte(this._fieldIndexList.Count);
            bw.Write(num3);
            for (int j = 0; j < num3; j++)
            {
                bw.Write(this.FieldIndexList[j]);
            }
        }

        public List<short> FieldIndexList
        {
            get
            {
                return this._fieldIndexList;
            }
            set
            {
                this._fieldIndexList = value;
            }
        }

        // <summary>
        // 0-仅返回请求信息；1-返回请求信息和数据
        // </summary>
        //public bool IsResponseData;


        // <summary>
        // 字段标记，1实时，2静态，3混合
        // </summary>
        //public byte FieldFlag;

        // <summary>
        // 栏位个数
        // </summary>
        //private byte _numFieldIndex;

        //private List<short> _fieldIndexList;

        // <summary>
        // 请求字段List
        // </summary>
        //public List<short> FieldIndexList
        //{
        //    get { return _fieldIndexList; }
        //    set
        //    {
        //        _fieldIndexList = value;
        //        _numFieldIndex = Convert.ToByte(_fieldIndexList.Count);
        //    }
        //}
        // <summary>
        // 用户code集
        // </summary>
        //public List<int> CustomCodeList;

        // <summary>
         
        // </summary>
        //public ReqCustomReportOrgDataPacket()
        //{
        //    RequestType = FuncTypeOrg.CustomReport;
        //    IsResponseData = true;
        //    IsPush = true;
        //    CustomCodeList = new List<int>();
        //}

        // <summary>
        // 包转流
        // </summary>
        //public override void Coding(BinaryWriter bw)
        //{
        //    base.Coding(bw);
        //    bw.Write(IsResponseData);
        //    bw.Write(IsPush);
        //    bw.Write((short)CustomCodeList.Count);

        //    int mtObj = -1;
        //    string emcode = string.Empty;
        //    for (int i = 0; i < CustomCodeList.Count; i++)
        //    {
        //        if (DetailData.FieldIndexDataInt32.ContainsKey(CustomCodeList[i]))
        //            DetailData.FieldIndexDataInt32[CustomCodeList[i]].TryGetValue(FieldIndex.Market, out mtObj);

        //        if (DetailData.FieldIndexDataString.ContainsKey(CustomCodeList[i]))
        //            DetailData.FieldIndexDataString[CustomCodeList[i]].TryGetValue(FieldIndex.EMCode, out emcode);

        //        if (mtObj >= 0)
        //        {
        //            switch ((MarketType)mtObj)
        //            {
        //                case MarketType.SHALev1:
        //                case MarketType.SHALev2:
        //                case MarketType.SHBLev1:
        //                case MarketType.SHBLev2:
        //                case MarketType.SZALev1:
        //                case MarketType.SZALev2:
        //                case MarketType.SZBLev1:
        //                case MarketType.SZBLev2:
        //                case MarketType.SHConvertBondLev1:
        //                case MarketType.SHConvertBondLev2:
        //                case MarketType.SZConvertBondLev1:
        //                case MarketType.SZConvertBondLev2:
        //                case MarketType.SHNonConvertBondLev1:
        //                case MarketType.SHNonConvertBondLev2:
        //                case MarketType.SZNonConvertBondLev1:
        //                case MarketType.SZNonConvertBondLev2:
        //                case MarketType.SHFundLev1:
        //                case MarketType.SHFundLev2:
        //                case MarketType.SZFundLev1:
        //                case MarketType.SZFundLev2:
        //                case MarketType.SHRepurchaseLevel1:
        //                case MarketType.SHRepurchaseLevel2:
        //                case MarketType.SZRepurchaseLevel1:
        //                case MarketType.SZRepurchaseLevel2:
        //                case MarketType.SHINDEX:
        //                case MarketType.SZINDEX:
        //                case MarketType.IF:
        //                case MarketType.GoverFutures:
        //                case MarketType.TB_NEW:
        //                case MarketType.TB_OLD:
        //                    bw.Write(ConvertCodeOrg.ConvertCodeToLong(emcode));
        //                    break;
        //                default:
        //                    bw.Write((long)CustomCodeList[i]);
        //                    break;
        //            }
        //        }
        //    }

        //    bw.Write(FieldFlag);
        //    bw.Write(_numFieldIndex);
        //    for (int i = 0; i < _numFieldIndex; i++)
        //        bw.Write(FieldIndexList[i]);
        //}
    }

    /// <summary>
    /// 沪深报价列表行情
    /// </summary>
    public class ReqBlockStockReportOrgDataPacket : ReqBlockReportDataPacket
    {
        /// <summary>
        /// 
        /// </summary>
        public ReqBlockStockReportOrgDataPacket(string id)
            : base(id)
        {
            RequestType = FuncTypeOrg.BlockStockReport;
        }
    }

    /// <summary>
    /// 港股报价
    /// </summary>
    public class ReqHKStockReportOrgDataPacket : ReqBlockReportDataPacket
    {
        /// <summary>
        /// 
        /// </summary>
        public ReqHKStockReportOrgDataPacket(string id)
            : base(id)
        {
            RequestType = FuncTypeOrg.HKStockReport;
        }
    }

    /// <summary>
    /// 基金报价
    /// </summary>
    public class ReqFundStockReportOrgDataPacket : ReqBlockReportDataPacket
    {
        /// <summary>
        /// 
        /// </summary>
        public ReqFundStockReportOrgDataPacket(string id)
            : base(id)
        {
            RequestType = FuncTypeOrg.FundStockReport;
        }
    }

    /// <summary>
    /// 债券报价
    /// </summary>
    public class ReqBondStockReportOrgDataPacket : ReqBlockReportDataPacket
    {
        /// <summary>
        /// 
        /// </summary>
        public ReqBondStockReportOrgDataPacket(string id)
            : base(id)
        {
            RequestType = FuncTypeOrg.BondStockReport;
        }
    }

    /// <summary>
    /// 期货报价
    /// </summary>
    public class ReqFuturesStockReportOrgDataPacket : ReqBlockReportDataPacket
    {
        /// <summary>
        /// 
        /// </summary>
        public ReqFuturesStockReportOrgDataPacket(string id)
            : base(id)
        {
            RequestType = FuncTypeOrg.FuturesStockReport;
        }
    }

    /// <summary>
    /// 股指期货
    /// </summary>
    public class ReqIndexFuturesReportOrgDataPacket : ReqBlockReportDataPacket
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="id"></param>
        public ReqIndexFuturesReportOrgDataPacket(string id)
            : base(id)
        {
            RequestType = FuncTypeOrg.IndexFuturesReport;
        }
    }

    /// <summary>
    /// 利率
    /// </summary>
    public class ReqRateReportOrgDataPacket : ReqBlockReportDataPacket
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="id"></param>
        public ReqRateReportOrgDataPacket(string id)
            : base(id)
        {
            RequestType = FuncTypeOrg.RateReport;
        }
    }

    /// <summary>
    /// 理财
    /// </summary>
    public class ReqFinanceReportOrgDataPacket : ReqBlockReportDataPacket
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="id"></param>
        public ReqFinanceReportOrgDataPacket(string id)
            : base(id)
        {
            RequestType = FuncTypeOrg.FinanceReport;
        }
    }

    /// <summary>
    /// 资金流向报价
    /// </summary>
    public class ReqCapitalFlowReportOrgDataPacket : ReqBlockReportDataPacket
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="id"></param>
        public ReqCapitalFlowReportOrgDataPacket(string id)
            : base(id)
        {
            RequestType = FuncTypeOrg.CapitalFlowReport;
        }
    }

    /// <summary>
    /// DDE报价
    /// </summary>
    public class ReqDDEReportOrgDataPacket : ReqBlockReportDataPacket
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="id"></param>
        public ReqDDEReportOrgDataPacket(string id)
            : base(id)
        {
            RequestType = FuncTypeOrg.DDEReport;
        }
    }

    /// <summary>
    /// 增仓排名
    /// </summary>
    public class ReqNetInFlowReportOrgDataPacket : ReqBlockReportDataPacket
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="id"></param>
        public ReqNetInFlowReportOrgDataPacket(string id)
            : base(id)
        {
            RequestType = FuncTypeOrg.NetInFlowReport;
        }
    }

    /// <summary>
    /// 财务数据报价
    /// </summary>
    public class ReqFinanceStockReportOrgDataPacket : ReqBlockReportDataPacket
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="id"></param>
        public ReqFinanceStockReportOrgDataPacket(string id)
            : base(id)
        {
            RequestType = FuncTypeOrg.FinanceStockReport;
        }
    }

    /// <summary>
    /// 盈利预测
    /// </summary>
    public class ReqProfitForecastReportOrgDataPacket : ReqBlockReportDataPacket
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="id"></param>
        public ReqProfitForecastReportOrgDataPacket(string id)
            : base(id)
        {
            RequestType = FuncTypeOrg.ProfitForecastReport;
        }
    }

    /// <summary>
    /// 外汇报价
    /// </summary>
    public class ReqForexReportDataPacket : ReqBlockReportDataPacket
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="id"></param>
        public ReqForexReportDataPacket(string id)
            : base(id)
        {
            RequestType = FuncTypeOrg.ForexReport;
        }
    }

    /// <summary>
    /// 美股报价
    /// </summary>
    public class ReqUSStockReportDataPacket : ReqBlockReportDataPacket
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="id"></param>
        public ReqUSStockReportDataPacket(string id)
            : base(id)
        {
            RequestType = FuncTypeOrg.USStockReport;
        }
    }



    /// <summary>
    /// 国外期货报价
    /// </summary>
    public class ReqOSFuturesReportDataPacket : ReqBlockReportDataPacket
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="id"></param>
        public ReqOSFuturesReportDataPacket(string id)
            : base(id)
        {
            RequestType = FuncTypeOrg.OSFuturesReport;
        }
    }

    /// <summary>
    /// 国外期货报价(新)
    /// </summary>
    public class ReqOSFuturesReportNewDataPacket : ReqBlockReportDataPacket
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="id"></param>
        public ReqOSFuturesReportNewDataPacket(string id)
            : base(id)
        {
            RequestType = FuncTypeOrg.OSFuturesReportNew;
        }
    }

    /// <summary>
    /// LME
    /// </summary>
    public class ReqOSFuturesLMEReportDataPacket : ReqBlockReportDataPacket
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="id"></param>
        public ReqOSFuturesLMEReportDataPacket(string id)
            : base(id)
        {
            RequestType = FuncTypeOrg.OsFuturesLMEReport;
        }
    }

    /// <summary>
    /// 自选股资金流向
    /// </summary>
    public class ReqCustomCapitalFlowReportOrgDataPacket : ReqCustomReportOrgDataPacket
    {
        public ReqCustomCapitalFlowReportOrgDataPacket()
        {
            RequestType = FuncTypeOrg.CustomCapitalFlowReport;
        }
    }

    /// <summary>
    /// 自选股DDE
    /// </summary>
    public class ReqCustomDDEReportOrgDataPacket : ReqCustomReportOrgDataPacket
    {
        public ReqCustomDDEReportOrgDataPacket()
        {
            RequestType = FuncTypeOrg.CustomDDEReport;
        }
    }

    /// <summary>
    /// 自选股增仓排名
    /// </summary>
    public class ReqCustomNetInFlowReportOrgDataPacket : ReqCustomReportOrgDataPacket
    {
        public ReqCustomNetInFlowReportOrgDataPacket()
        {
            RequestType = FuncTypeOrg.CustomNetInFlowReport;
        }
    }

    /// <summary>
    /// 自选股财务数据
    /// </summary>
    public class ReqCustomFinanceStockReportOrgDataPacket : ReqCustomReportOrgDataPacket
    {
        public ReqCustomFinanceStockReportOrgDataPacket()
        {
            RequestType = FuncTypeOrg.CustomFinanceStockReport;
        }
    }

    /// <summary>
    /// 自选股盈利预测
    /// </summary>
    public class ReqCustomProfitForecastReportOrgDataPacket : ReqCustomReportOrgDataPacket
    {
        public ReqCustomProfitForecastReportOrgDataPacket()
        {
            RequestType = FuncTypeOrg.CustomProfitForecastReport;
        }
    }

    /// <summary>
    /// 报价初始化静态数据请求包
    /// </summary>
    public class ReqReportInitOrgDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 构造
        /// </summary>
        public ReqReportInitOrgDataPacket()
        {
            RequestType = FuncTypeOrg.InitReportData;
        }
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write((long)0);
        }
    }

    /// <summary>
    /// 财务数据
    /// </summary>
    public class ReqFinanceOrgDataPacket : OrgDataPacket
    {
        /// <summary>
        /// construct
        /// </summary>
        public ReqFinanceOrgDataPacket()
        {
            RequestType = FuncTypeOrg.FinanceOrg;
        }

        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write((byte)1);
        }

    }

    /// <summary>
    /// 除复权
    /// </summary>
    public class ReqDivideRightOrgDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 起始日期
        /// </summary>
        public int Date;

        public ReqDivideRightOrgDataPacket()
        {
            RequestType = FuncTypeOrg.DivideRightOrg;
        }

        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(Date);
        }
    }

    /// <summary>
    /// 货币式基金detail
    /// </summary>
    public class ReqMonetaryFundDetailDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        public int Code;

        public ReqMonetaryFundDetailDataPacket()
        {
            RequestType = FuncTypeOrg.MonetaryFundDetail;
        }

        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write((long)Code);
        }
    }

    /// <summary>
    /// 非货币式基金Detail
    /// </summary>
    public class ReqNonMonetaryFundDetailDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        public int Code;

        public ReqNonMonetaryFundDetailDataPacket()
        {
            RequestType = FuncTypeOrg.NonMonetaryFundDetail;
        }

        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write((long)Code);
        }
    }

    /// <summary>
    /// 巨潮指数
    /// </summary>
    public class ReqCNIndexDetailDataPacket : ReqNonMonetaryFundDetailDataPacket
    {
        public ReqCNIndexDetailDataPacket()
        {
            RequestType = FuncTypeOrg.CNIndexDetail;
        }
    }

    /// <summary>
    /// 中证指数
    /// </summary>
    public class ReqCSIIndexDetailDataPacket : ReqNonMonetaryFundDetailDataPacket
    {
        public ReqCSIIndexDetailDataPacket()
        {
            RequestType = FuncTypeOrg.CSIIndexDetail;
        }
    }

    /// <summary>
    /// 中债指数
    /// </summary>
    public class ReqCSIndexDetailDataPacket : ReqNonMonetaryFundDetailDataPacket
    {
        public ReqCSIndexDetailDataPacket()
        {
            RequestType = FuncTypeOrg.CSIndexDetail;
        }
    }

    /// <summary>
    /// 全球指数
    /// </summary>
    public class ReqGlobalIndexDetailDataPacket : ReqNonMonetaryFundDetailDataPacket
    {
        public ReqGlobalIndexDetailDataPacket()
        {
            RequestType = FuncTypeOrg.GlobalIndexDetail;
        }
    }


    /// <summary>
    /// 银行间债券detail
    /// </summary>
    public class ReqInterBankDetailDataPacket : OrgDataPacket
    {
        public int Code;
        public ReqInterBankDetailDataPacket()
        {
            RequestType = FuncTypeOrg.InterBankDetail;
            IsPush = true;
        }

        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write((long)Code);
            bw.Write(IsPush);
        }
    }

    /// <summary>
    /// 可转债detail
    /// </summary>
    public class ReqConvertBondDetailDataPacket : OrgDataPacket
    {
        public int Code;
        public ReqConvertBondDetailDataPacket()
        {
            IsPush = true;
            RequestType = FuncTypeOrg.ConvertBondDetail;
        }

        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            Dictionary<FieldIndex, object> memData = null;
            long sid = 0;
            string emcode = string.Empty;
            if (DetailData.FieldIndexDataString.ContainsKey(Code))
                DetailData.FieldIndexDataString[Code].TryGetValue(FieldIndex.EMCode, out emcode);

            if (emcode != null)
                sid = ConvertCodeOrg.ConvertCodeToLong(Convert.ToString(emcode));

            bw.Write(sid);
            bw.Write(IsPush);
        }

    }

    /// <summary>
    /// 非可转债detail
    /// </summary>
    public class ReqNonConvertBondDetailDataPacket : OrgDataPacket
    {
        public int Code;
        public ReqNonConvertBondDetailDataPacket()
        {
            IsPush = true;
            RequestType = FuncTypeOrg.NonConvertBondDetail;
        }

        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            Dictionary<FieldIndex, object> memData = null;
            long sid = 0;

            string emcode = string.Empty;
            if (DetailData.FieldIndexDataString.ContainsKey(Code))
                DetailData.FieldIndexDataString[Code].TryGetValue(FieldIndex.EMCode, out emcode);
            if (emcode != null)
                sid = ConvertCodeOrg.ConvertCodeToLong(Convert.ToString(emcode));

            bw.Write(sid);
            bw.Write(IsPush);
        }

    }

    /// <summary>
    /// 利率互换
    /// </summary>
    public class ReqRateSwapDetailDataPacket : ReqInterBankDetailDataPacket
    {
        public ReqRateSwapDetailDataPacket()
        {
            RequestType = FuncTypeOrg.RateSwapDetail;
        }
    }

    /// <summary>
    /// 银行间回购与拆借
    /// </summary>
    public class ReqInterBankRepurchaseDetailDataPacket : ReqInterBankDetailDataPacket
    {
        public ReqInterBankRepurchaseDetailDataPacket()
        {
            RequestType = FuncTypeOrg.InterBankRepurchaseDetail;
        }
    }

    /// <summary>
    /// shibor的detail
    /// </summary>
    public class ReqShiborDetailDataPacket : ReqInterBankDetailDataPacket
    {
        public ReqShiborDetailDataPacket()
        {
            RequestType = FuncTypeOrg.ShiborDetail;
        }
    }


    /// <summary>
    /// 美股Detail
    /// </summary>
    public class ReqUSStockDetailDataPacket : ReqInterBankDetailDataPacket
    {
        public ReqUSStockDetailDataPacket()
        {
            RequestType = FuncTypeOrg.USStockDetail;
        }
    }

    /// <summary>
    /// 外汇Detail
    /// </summary>
    public class ReqForexDetailDataPacket : ReqInterBankDetailDataPacket
    {
        public ReqForexDetailDataPacket()
        {
            RequestType = FuncTypeOrg.ForexDetail;
        }
    }

    /// <summary>
    /// 海外期货
    /// </summary>
    public class ReqOSFuturesDetailDataPacket : ReqInterBankDetailDataPacket
    {
        public ReqOSFuturesDetailDataPacket()
        {
            RequestType = FuncTypeOrg.OSFuturesDetail;
        }
    }

    public class ReqOSFuturesLMEDetailDataPacket : ReqInterBankDetailDataPacket
    {
        public ReqOSFuturesLMEDetailDataPacket()
        {
            RequestType = FuncTypeOrg.OSFuturesLMEDetail;
        }
    }

    /// <summary>
    /// 基金净值后复权
    /// </summary>
    public class ReqFundKLineAfterDivideDataPacket : OrgDataPacket
    {
        private int _code;

        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set
            {
                _code = value;
            }
        }

        /// <summary>
        /// K线周期
        /// </summary>
        public KLineCycle Cycle;

        /// <summary>
        /// 请求K线的数据范围
        /// </summary>
        public ReqKLineDataRange DataRange;

        /// <summary>
        /// 开始日期 格式[YYYYMMDD]
        /// </summary>
        public int StartDate;

        /// <summary>
        /// 结束日期 格式[YYYYMMDD]
        /// </summary>
        public int EndDate;

        /// <summary>
        /// 请求K线的个数
        /// </summary>
        public int ApplySize;

        /// <summary>
        /// 
        /// </summary>
        public ReqFundKLineAfterDivideDataPacket()
        {
            RequestType = FuncTypeOrg.FundKlineAfterDivide;
            Cycle = KLineCycle.CycleDay;
            DataRange = ReqKLineDataRange.SizeToNow;
        }

        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write((long)Code);
            KLineCycleOrg cycleOrg = (KLineCycleOrg)Enum.Parse(typeof(KLineCycleOrg), Cycle.ToString());
            bw.Write((short)cycleOrg);
            //DataRange = ReqKLineDataRange.All;
            bw.Write((byte)DataRange);
            bw.Write(StartDate);
            bw.Write(EndDate);
            bw.Write(ApplySize);
        }
    }


    /// <summary>
    /// 信托理财和阳光私募
    /// </summary>
    public class ReqTrpAndSunDetailDataPacket : ReqMonetaryFundDetailDataPacket
    {
        public ReqTrpAndSunDetailDataPacket()
        {
            RequestType = FuncTypeOrg.FundTrpAndSunDetail;
        }
    }

    /// <summary>
    /// 券商集合（货币式）Detail
    /// </summary>
    public class ReqCIPMonetaryDetailDataPacket : ReqMonetaryFundDetailDataPacket
    {
        public ReqCIPMonetaryDetailDataPacket()
        {
            RequestType = FuncTypeOrg.FundCIPMonetaryDetail;
        }
    }

    /// <summary>
    /// 券商集合（非货币式）Detail
    /// </summary>
    public class ReqCIPNonMonetaryDetailDataPacket : ReqMonetaryFundDetailDataPacket
    {
        public ReqCIPNonMonetaryDetailDataPacket()
        {
            RequestType = FuncTypeOrg.FundCIPNonMonetaryDetail;
        }
    }

    /// <summary>
    /// 银行理财
    /// </summary>
    public class ReqBFPDetailDataPacket : ReqMonetaryFundDetailDataPacket
    {
        public ReqBFPDetailDataPacket()
        {
            RequestType = FuncTypeOrg.FundBFPDetail;
        }
    }




    /// <summary>
    /// 重仓持股
    /// </summary>
    public class ReqFundHeaveStockReport : ReqMonetaryFundDetailDataPacket
    {
        public ReqFundHeaveStockReport()
        {
            RequestType = FuncTypeOrg.FundHeaveStockReport;
        }
    }

    /// <summary>
    /// 重仓行业
    /// </summary>
    public class ReqFundHYReport : ReqMonetaryFundDetailDataPacket
    {
        public ReqFundHYReport()
        {
            RequestType = FuncTypeOrg.FundHYReport;
        }
    }

    /// <summary>
    /// 重仓债券
    /// </summary>
    public class ReqKeyBondReport : ReqMonetaryFundDetailDataPacket
    {
        public ReqKeyBondReport()
        {
            RequestType = FuncTypeOrg.FundKeyBondReport;
        }
    }

    /// <summary>
    /// 重仓基金
    /// </summary>
    public class ReqFinanceHeaveFundReprotDataPacket : ReqMonetaryFundDetailDataPacket
    {
        public ReqFinanceHeaveFundReprotDataPacket()
        {
            RequestType = FuncTypeOrg.FinanceHeaveFundReport;
        }
    }

    /// <summary>
    /// 基金经理
    /// </summary>
    public class ReqFundManager : ReqMonetaryFundDetailDataPacket
    {
        public ReqFundManager()
        {
            RequestType = FuncTypeOrg.FundManager;
        }
    }

    /// <summary>
    /// 理财重仓持股
    /// </summary>
    public class ReqFinanceHeaveStockReport : ReqMonetaryFundDetailDataPacket
    {
        public ReqFinanceHeaveStockReport()
        {
            RequestType = FuncTypeOrg.FinanceHeaveStockReport;
        }
    }

    /// <summary>
    /// 理财重仓行业
    /// </summary>
    public class ReqFinanceHYReport : ReqMonetaryFundDetailDataPacket
    {
        public ReqFinanceHYReport()
        {
            RequestType = FuncTypeOrg.FinanceHeaveHYReport;
        }
    }

    /// <summary>
    /// 理财重仓债券
    /// </summary>
    public class ReqFinanceKeyBondReport : ReqMonetaryFundDetailDataPacket
    {
        public ReqFinanceKeyBondReport()
        {
            RequestType = FuncTypeOrg.FinanceHeaveBondReport;
        }
    }

    /// <summary>
    /// 理财基金经理
    /// </summary>
    public class ReqFinanceManager : ReqMonetaryFundDetailDataPacket
    {
        public ReqFinanceManager()
        {
            RequestType = FuncTypeOrg.FinanceHeaveManagerReport;
        }
    }

    /// <summary>
    /// 深度分析
    /// </summary>
    public class ReqDepthAnalyseDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        public int Code;

        public ReqDepthAnalyseDataPacket()
        {
            RequestType = FuncTypeOrg.DepthAnalyse;
        }

        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);

            string emcode = string.Empty;
            if (DetailData.FieldIndexDataString.ContainsKey(Code))
                DetailData.FieldIndexDataString[Code].TryGetValue(FieldIndex.EMCode, out emcode);
            bw.Write(ConvertCodeOrg.ConvertCodeToLong(emcode));
        }
    }

    /// <summary>
    /// 东财指数Detail
    /// </summary>
    public class ReqEMIndexDetailDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        public int Code;

        public ReqEMIndexDetailDataPacket()
        {
            RequestType = FuncTypeOrg.EMIndexDetail;
            IsPush = true;
        }

        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write((long)Code);
            bw.Write(IsPush);
        }
    }

    /// <summary>
    /// 综合排名（机构版）
    /// </summary>
    public class ReqRankOrgDataPacket : OrgDataPacket
    {
        private string _blockId;

        /// <summary>
        /// 排名种类(默认0,表示全部)，涨跌，量比，金额等，支持或操作
        /// </summary>
        public RankType RankEnum;

        /// <summary>
        /// 字段标记，1实时，2静态，3混合
        /// </summary>
        public byte FieldFlag;

        private short[] _fields = new short[]
                                      {
                                          9501, 8002, 8004, 9502, 9503, 9504, 9505, 9506, 8047, 9507, 8048, 9508, 8024,
                                          9509
                                      };

        public ReqRankOrgDataPacket(string id)
        {
            RequestType = FuncTypeOrg.Rank;
            IsPush = true;
            _blockId = "91" + id;
            RankEnum = 0;
            FieldFlag = 1;
        }

        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write((byte)1);
            bw.Write(IsPush);
            bw.Write((int)RankEnum);
            bw.Write((short)1);
            bw.Write(Convert.ToInt64(_blockId));
            bw.Write(FieldFlag);
            bw.Write((byte)14);
            for (int i = 0; i < _fields.Length; i++)
                bw.Write(_fields[i]);
        }
    }

    /// <summary>
    /// 净流入排名
    /// </summary>
    public class ReqNetInflowRankDataPacket : OrgDataPacket
    {
        private string _blockId;

        /// <summary>
        /// 字段标记，1实时，2静态，3混合
        /// </summary>
        public byte FieldFlag;

        private short[] _fields = new short[]
                                      {
                                         9006,9007,9008,9009,9201,9204,9207
                                      };

        private bool IsResponseData = false;

        public ReqNetInflowRankDataPacket(string id)
        {
            RequestType = FuncTypeOrg.NetInFlowRank;
            IsPush = true;
            _blockId = "91" + id;
            FieldFlag = 1;
        }

        public ReqNetInflowRankDataPacket()
        {
            RequestType = FuncTypeOrg.NetInFlowRank;
            IsPush = true;
            _blockId = "91001001";
            FieldFlag = 1;
        }

        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(IsResponseData);
            bw.Write(IsPush);
            bw.Write((short)1);
            bw.Write(Convert.ToInt64(_blockId));
            bw.Write(FieldFlag);
            bw.Write((byte)_fields.Length);
            for (int i = 0; i < _fields.Length; i++)
                bw.Write(_fields[i]);
        }
    }

    /// <summary>
    /// LME成交明细
    /// </summary>
    public class ReqOSFuturesLMEDealDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 日期
        /// </summary>
        public int Date;

        /// <summary>
        /// 个数限制，=0时表示无限制
        /// </summary>
        public int Count;

        /// <summary>
        /// 内码
        /// </summary>
        public int Code;

        public ReqOSFuturesLMEDealDataPacket()
        {
            RequestType = FuncTypeOrg.OSFuturesLMEDeal;
            IsPush = true;
        }

        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(Convert.ToInt64(Code));
            bw.Write(Date);
            bw.Write(IsPush);
            bw.Write(Count);
        }


    }

    /// <summary>
    /// 低频分笔交易请求包
    /// </summary>
    public class ReqLowFrequencyTBYDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        public int Code;
        /// <summary>
        /// 请求数据日期
        /// </summary>
        public int Date;
        /// <summary>
        /// 请求数据个数
        /// </summary>
        public int DataNum;
        /// <summary>
        /// 
        /// </summary>
        public ReqLowFrequencyTBYDataPacket()
        {
            RequestType = FuncTypeOrg.LowFrequencyTBY;
            IsPush = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="date"></param>
        /// <param name="dataNum"></param>
        public ReqLowFrequencyTBYDataPacket(int code, int date, int dataNum)
        {
            RequestType = FuncTypeOrg.LowFrequencyTBY;
            IsPush = true;
            Code = code;
            Date = date;
            DataNum = dataNum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bw"></param>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(Convert.ToInt64(Code));
            bw.Write(Date);
            bw.Write(IsPush);
            bw.Write(DataNum);
        }
    }

    /// <summary>
    /// 银行间债券报价明细请求包
    /// </summary>
    public class ReqBankBondReportDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        public int Code;
        /// <summary>
        /// 请求数据日期
        /// </summary>
        public int Date;
        /// <summary>
        /// 请求数据个数
        /// </summary>
        public int DataNum;
        /// <summary>
        /// 
        /// </summary>
        public ReqBankBondReportDataPacket()
        {
            RequestType = FuncTypeOrg.BankBondReport;
            IsPush = true;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        /// <param name="date"></param>
        /// <param name="dataNum"></param>
        public ReqBankBondReportDataPacket(int code, int date, int dataNum)
            : this()
        {

            Code = code;
            Date = date;
            DataNum = dataNum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bw"></param>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(Convert.ToInt64(Code));
            bw.Write(Date);
            bw.Write(IsPush);
            bw.Write(DataNum);
        }
    }

    /// <summary>
    /// SHIBOR报价行明细请求包
    /// </summary>
    public class ReqShiborReportDataPacket : OrgDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        public int Code;
        /// <summary>
        /// 请求数据日期
        /// </summary>
        public int Date;
        /// <summary>
        /// 请求数据个数
        /// </summary>
        public int DataNum;
        /// <summary>
        /// 默认构造
        /// </summary>
        public ReqShiborReportDataPacket()
        {
            RequestType = FuncTypeOrg.ShiborReport;
            IsPush = true;
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="code"></param>
        /// <param name="date"></param>
        /// <param name="dataNum"></param>
        public ReqShiborReportDataPacket(int code, int date, int dataNum)
            :this()
        {           
            Code = code;
            Date = date;
            DataNum = dataNum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bw"></param>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(Convert.ToInt64(Code));
            bw.Write(Date);
            bw.Write(IsPush);
            bw.Write(DataNum);
        }
    }

    /// <summary>
    /// 个股盈利预测(高频)-发送包
    /// </summary>
    public class ReqNewProfitForecastDataPacket : OrgDataPacket
    { 
        /// <summary>
        /// 内码
        /// </summary>
        public int Code;
      
        /// <summary>
        /// 默认构造
        /// </summary>
        public ReqNewProfitForecastDataPacket()
        {
            RequestType = FuncTypeOrg.NewProfitForcast;
        }         

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bw"></param>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            string emcode = string.Empty;
            Dictionary<FieldIndex, string> fieldString;
            if (DetailData.FieldIndexDataString.TryGetValue(Code, out fieldString))
            {
                if (!fieldString.TryGetValue(FieldIndex.EMCode, out emcode))
                    emcode = string.Empty;
            }
            bw.Write(ConvertCodeOrg.ConvertCodeToLong(emcode));
        }
    }

    /// <summary>
    /// 股票中文名称更改请求包
    /// </summary>
    public class ReqChangeNameDataPacket : OrgDataPacket
    {
        public ReqChangeNameDataPacket()
        {
            RequestType = FuncTypeOrg.ChangeName;
        }
    }
    #endregion

    #region 财富通外盘接口
    /// <summary>
    /// 外盘登陆请求包
    /// </summary>
    public class ReqLogonDataPacket : RealTimeDataPacket
    {
        private byte[] _userName;
        private byte[] _password;
        private byte[] szClientID;

        /// <summary>
        /// 
        /// </summary>
        public ReqLogonDataPacket()
        {
            RequestType = FuncTypeRealTime.InitLogon;
            _userName = new byte[32];
            _password = new byte[32];
            byte[] guestByte = Encoding.Default.GetBytes("guest");

            for (int i = 0; i < guestByte.Length; i++)
            {
                _userName[i] = guestByte[i];
                _password[i] = guestByte[i];
            }
            for (int i = 5; i < 32; i++)
            {
                _password[i] = (byte)0;
                _userName[i] = (byte)0;
            }

            szClientID = new byte[64];
            for (int i = 0; i < 64; i++)
            {
                szClientID[i] = (byte)0;
            }
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write((byte)1);//主版本
            bw.Write((byte)3);//次版本
            bw.Write((byte)2);//修订版本
            bw.Write((byte)0);//代理服务器类型
            bw.Write((byte)0);//产品定制号
            bw.Write(0);//dummy
            bw.Write(0);//用户登录类型定义
            bw.Write(_userName);// 用户名
            bw.Write(0);// 硬盘ID
            bw.Write(_password);// 密码
            bw.Write(0);//loginID
            bw.Write((byte)0);// 登录类型, 0, 正常登录, 1, 重连登录
            bw.Write(szClientID);
        }
    }

    /// <summary>
    /// 外盘心跳
    /// </summary>
    public class ReqOceanHeartDataPacket : RealTimeDataPacket
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReqOceanHeartDataPacket()
        {
            RequestType = FuncTypeRealTime.OceanHeart;
        }
    }

    /// <summary>
    /// 外盘行情记录请求包
    /// </summary>
    public class ReqOceanRecordDataPacket : RealTimeDataPacket
    {
        private byte _market;
        private string _shortCode = string.Empty;
        private int _code;

        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set
            {
                _code = value;
                ReqMarketType tmp = ReqMarketType.MT_NA;
                string emcode = string.Empty;
                if (DetailData.FieldIndexDataString.ContainsKey(Code))
                    DetailData.FieldIndexDataString[Code].TryGetValue(FieldIndex.EMCode, out emcode);
                ParseCode(emcode, out tmp, out _shortCode);
                _market = (byte)tmp;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ReqOceanRecordDataPacket()
        {
            RequestType = FuncTypeRealTime.OceanRecord;
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(_market);
            bw.Write(Encoding.ASCII.GetBytes(_shortCode));
        }
    }

    /// <summary>
    /// 外盘走势请求包
    /// </summary>
    public class ReqOceanTrendDataPacket : RealTimeDataPacket
    {

        /// <summary>
        /// 日期
        /// </summary>
        public int Date;

        /// <summary>
        /// 时间
        /// </summary>
        private int _time;

        /// <summary>
        /// 请求走势最后一个点的下标
        /// </summary>
        public int LastRequestPoint
        {
            get { return _lastRequestPoint; }
            set
            {
                _lastRequestPoint = value;
                _time = TimeUtilities.GetTimeFromPoint(Code, _lastRequestPoint);
            }
        }

        private byte _market;
        private string _shortCode;
        private int _code;
        private int _lastRequestPoint;

        /// <summary>
        /// 股票代码
        /// </summary>
        public int Code
        {
            get { return _code; }
            set
            {
                _code = value;
                ReqMarketType tmp = ReqMarketType.MT_NA;
                string emcode = string.Empty;
                if (DetailData.FieldIndexDataString.ContainsKey(_code))
                    DetailData.FieldIndexDataString[_code].TryGetValue(FieldIndex.EMCode, out emcode);

                ParseCode(emcode, out tmp, out _shortCode);
                _market = (byte)tmp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ReqOceanTrendDataPacket()
        {
            RequestType = FuncTypeRealTime.OceanTrend;
        }
        /// <summary>
        /// 包转流
        /// </summary>
        public override void Coding(BinaryWriter bw)
        {
            base.Coding(bw);
            bw.Write(Date);
            bw.Write(_time);
            bw.Write(_market);
            bw.Write(Encoding.ASCII.GetBytes(_shortCode));
        }

    }

    #endregion

    #region 机构版资讯接口

    /// <summary>
    /// 机构版新闻资讯
    /// </summary>
    public class ReqInfoOrgDataPacket : InfoOrgBaseDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        public List<int> Code;

        /// <summary>
        /// 种类（新闻，研报等）
        /// </summary>
        public InfoMineOrg InfoOrg;

        /// <summary>
        /// 数目
        /// </summary>
        public ushort MaxCount;

        /// <summary>
        /// 开始日期
        /// </summary>
        public int LastDate;

        /// <summary>
        /// 开始时间
        /// </summary>
        public int LastTime;

        /// <summary>
        /// 结束日期
        /// </summary>
        public int EndDate;

        /// <summary>
        /// 结束时间
        /// </summary>
        public int EndTime;

        /// <summary>
        /// 构造
        /// </summary>
        public ReqInfoOrgDataPacket()
        {
            RequestId = FuncTypeInfoOrg.InfoMineOrg;
            MaxCount = 200;
            InfoOrg = InfoMineOrg.News;
        }

        /// <summary>
        /// 组包
        /// </summary>
        /// <returns></returns>
        public override byte[] CodeInfoPacket()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(memoryStream))
                {
                    try
                    {
                        bw.Write((byte)RequestId);
                        bw.Write((ushort)Code.Count);
                        for (int i = 0; i < Code.Count; i++)
                            bw.Write(Code[i]);
                        bw.Write(MaxCount);
                        bw.Write((byte)InfoOrg);
                        bw.Write(LastDate);
                        bw.Write(LastTime);
                        bw.Write(EndDate);
                        bw.Write(EndTime);
                    }
                    catch (Exception e)
                    {
                        LogUtilities.LogMessage("机构版资讯组包错误" + e.Message);
                    }
                }
                return memoryStream.ToArray();
            }
        }
    }

    /// <summary>
    /// 24小时新闻
    /// </summary>
    public class ReqNews24HOrgDataPacket : InfoOrgBaseDataPacket
    {
        /// <summary>
        /// 数目
        /// </summary>
        public ushort MaxCount;

        /// <summary>
        /// 日期
        /// </summary>
        public int LastDate;

        /// <summary>
        /// 时间
        /// </summary>
        public int LastTime;

        /// <summary>
        /// 构造
        /// </summary>
        public ReqNews24HOrgDataPacket()
        {
            RequestId = FuncTypeInfoOrg.News24H;
        }

        public override byte[] CodeInfoPacket()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(memoryStream))
                {
                    try
                    {
                        bw.Write((byte)RequestId);
                        bw.Write(MaxCount);
                        bw.Write(LastDate);
                        bw.Write(LastTime);
                    }
                    catch (Exception e)
                    {
                        LogUtilities.LogMessage("机构版资讯组包错误" + e.Message);
                    }
                }
                return memoryStream.ToArray();
            }
        }
    }

    /// <summary>
    /// 要闻精华
    /// </summary>
    public class ReqImportantNewsDataPacket : ReqNews24HOrgDataPacket
    {
        public ReqImportantNewsDataPacket()
        {
            RequestId = FuncTypeInfoOrg.ImportantNews;
        }
    }

    /// <summary>
    /// 公司快讯
    /// </summary>
    public class ReqNewsFlashDataPacket : ReqNews24HOrgDataPacket
    {
        public ReqNewsFlashDataPacket()
        {
            RequestId = FuncTypeInfoOrg.NewsFlash;
        }
    }

    /// <summary>
    /// 盈利预测(机构版)
    /// </summary>
    public class ReqProfitForecastOrgDataPacket : InfoOrgBaseDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        public int Code;

        /// <summary>
        /// 构造
        /// </summary>
        public ReqProfitForecastOrgDataPacket()
        {
            RequestId = FuncTypeInfoOrg.ProfitForecast;
        }

        public override byte[] CodeInfoPacket()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(memoryStream))
                {
                    try
                    {
                        bw.Write((byte)RequestId);
                        bw.Write(Code);
                    }
                    catch (Exception e)
                    {
                        LogUtilities.LogMessage("机构版盈利预测组包错误" + e.Message);
                    }
                }
                return memoryStream.ToArray();
            }
        }
    }

    /// <summary>
    /// 机构评级
    /// </summary>
    public class ReqInfoRateOrgDataPacket : InfoOrgBaseDataPacket
    {
        /// <summary>
        /// 代码
        /// </summary>
        public int Code;

        /// <summary>
        /// 最大返回数，=0时，无限制
        /// </summary>
        public ushort MaxCount;

        public ReqInfoRateOrgDataPacket()
        {
            RequestId = FuncTypeInfoOrg.OrgRate;
        }

        public override byte[] CodeInfoPacket()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(memoryStream))
                {
                    try
                    {
                        bw.Write((byte)RequestId);
                        bw.Write(Code);
                        bw.Write(MaxCount);
                    }
                    catch (Exception e)
                    {
                        LogUtilities.LogMessage("机构版盈利预测组包错误" + e.Message);
                    }
                }
                return memoryStream.ToArray();
            }
        }
    }
    /// <summary>
    /// 研究报告(机构版)
    /// </summary>
    public class ReqResearchReportOrgDataPacket : InfoOrgBaseDataPacket
    {
        /// <summary>
        /// 记录数
        /// </summary>
        public ushort Count;
        /// <summary>
        /// 开始日期
        /// </summary>
        public Int32 StartDate;
        /// <summary>
        /// 开始时间
        /// </summary>
        public Int32 StartTime;

        /// <summary>
        /// 构造
        /// </summary>
        public ReqResearchReportOrgDataPacket()
        {
            RequestId = FuncTypeInfoOrg.ResearchReport;
        }
        /// <summary>
        /// 构造
        /// </summary>
        public ReqResearchReportOrgDataPacket(ushort count, Int32 startDate, Int32 startTime)
            : this()
        {
            Count = count;
            StartDate = startDate;
            StartTime = startTime;
        }


        public override byte[] CodeInfoPacket()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(memoryStream))
                {
                    try
                    {
                        bw.Write((byte)RequestId);
                        bw.Write(Count);
                        bw.Write(StartDate);
                        bw.Write(StartTime);
                    }
                    catch (Exception e)
                    {
                        LogUtilities.LogMessage("机构版'研究报告'组包错误" + e.Message);
                    }
                }
                return memoryStream.ToArray();
            }
        }
    }

    /// <summary>
    /// 8. 新资讯列表请求
    /// </summary>
    public class ReqNewInfoOrgDataPacket : InfoOrgBaseDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        public List<int> Code;

        /// <summary>
        /// 种类（新闻，研报等）
        /// </summary>
        public InfoMineOrg InfoOrg;

        /// <summary>
        /// 数目
        /// </summary>
        public ushort MaxCount;

        /// <summary>
        /// 开始日期
        /// </summary>
        public int LastDate;

        /// <summary>
        /// 开始时间
        /// </summary>
        public int LastTime;

        /// <summary>
        /// 结束日期
        /// </summary>
        public int EndDate;

        /// <summary>
        /// 结束时间
        /// </summary>
        public int EndTime;

        /// <summary>
        /// classType为1时等同于request=1(FuncTypeInfoOrg)的请求。
        ///本次新需求新增的请求都为2
        /// </summary>
        public ClassType ClassType = ClassType.OldInfo;

        /// <summary>
        /// 默认构造函数: 用于请求旧有格式的咨询内容
        /// </summary>
        public ReqNewInfoOrgDataPacket()
        {
            RequestId = FuncTypeInfoOrg.NewInfoMineOrg;
            MaxCount = 200;
            InfoOrg = InfoMineOrg.News;
        }
        /// <summary>
        /// 构造函数: 用于请求新/旧格式的咨询内容
        /// </summary>
        public ReqNewInfoOrgDataPacket(ClassType classType)
            : this()
        {
            ClassType = classType;
        }


        /// <summary>
        /// 组包
        /// </summary>
        /// <returns></returns>
        public override byte[] CodeInfoPacket()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(memoryStream))
                {
                    try
                    {
                        bw.Write((byte)RequestId);
                        bw.Write((ushort)Code.Count);
                        for (int i = 0; i < Code.Count; i++)
                            bw.Write(Code[i]);
                        bw.Write(MaxCount);
                        bw.Write((byte)InfoOrg);
                        bw.Write(LastDate);
                        bw.Write(LastTime);
                        bw.Write(EndDate);
                        bw.Write(EndTime);
                        bw.Write((byte)ClassType);
                    }
                    catch (Exception e)
                    {
                        LogUtilities.LogMessage("机构版资讯组包错误" + e.Message);
                    }
                }
                return memoryStream.ToArray();
            }
        }
    }


    /// <summary>
    /// 9. 根据列表id取资讯请求
    /// </summary>
    public class ReqInfoOrgByIdsDataPacket : InfoOrgBaseDataPacket
    {
        /// <summary>
        /// 内码
        /// </summary>
        public List<int> Code;

        /// <summary>
        /// 新闻/研报
        /// </summary>
        public InfoMineOrg InfoOrg = InfoMineOrg.News;

        /// <summary>
        /// 数目
        /// </summary>
        public ushort MaxCount = 0;

        /// <summary>
        /// 开始日期
        /// </summary>
        public int LastDate = 0;

        /// <summary>
        /// 开始时间
        /// </summary>
        public int LastTime = 0;
        
        /// <summary>
        /// 小类，比如F001，S001001(typeLevel2)
        /// </summary>
        public string InnerType;

        /// <summary>
        /// 研究报告类型（新闻/评级）
        /// </summary>
        public ReportType ReportType;


        /// <summary>
        /// 默认构造函数: 用于根据列表id取资讯请求 
        /// 注意：资讯内容只能是新闻：1；研报：3
        /// </summary>
        /// <param name="orgType">资讯内容: 新闻：1；研报：3</param>
        /// <param name="innerType">小类，比如F001，S001001(typeLevel2)</param>
        /// <param name="reportType">返回格式 （暂时只对研报有效） 1：个股的返回类型 2：全景图的返回类型 </param>
        public ReqInfoOrgByIdsDataPacket(InfoMineOrg orgType, string innerType, ReportType reportType)
        {
            RequestId = FuncTypeInfoOrg.InfoMineOrgByIds;
            MaxCount = 200;
            InfoOrg = orgType;
            InnerType = innerType;
            ReportType = reportType;
        }

        /// <summary>
        /// 组包
        /// </summary>
        /// <returns></returns>
        public override byte[] CodeInfoPacket()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(memoryStream))
                {
                    try
                    {
                        bw.Write((byte)RequestId);
                        bw.Write((ushort)MaxCount);
                        bw.Write((byte)InfoOrg);
                        byte[] strBytes = new UTF8Encoding().GetBytes(InnerType);
                        short strLength = (short)strBytes.Length;
                        bw.Write(strLength);
                        for (int i = 0; i < strLength; i++)
                            bw.Write(strBytes[i]);

                        bw.Write(LastDate);
                        bw.Write(LastTime);
                        bw.Write((byte)ReportType);
                    }
                    catch (Exception e)
                    {
                        LogUtilities.LogMessage("机构版资讯组包错误" + e.Message);
                    }

                    LogUtilities.LogMessage(DateTime.Now.ToString() + ": 发包ReqInfoOrgByIdsDataPacket");
                }
                return memoryStream.ToArray();
            }
        }
    }
    #endregion

    #region 宏观指标
    /// <summary>
    /// 宏观指标的在侧报表——请求包
    /// </summary>
    public class ReqIndicatorsReportDataPacket : IndicatorDataPacket
    {
        /// <summary>
        /// 请求宏观指标ID列表(当有自定义指标存在时需要发送单侧的所有ID)
        /// </summary>
        public List<string> MacroIds;

        /// <summary>
        /// 
        /// </summary>
        public ReqIndicatorsReportDataPacket()
        {
            MacroIds = new List<string>(1);
        }
       
        /// <summary>
        /// 请求股票自定义的宏观指标
        /// </summary>
        /// <param name="code">股票内码</param>   
        /// <param name="requestType">宏观指标报表类型</param>  
        public ReqIndicatorsReportDataPacket(int code, IndicateRequestType requestType)
            :this()
        {
            // Get RequestId.
            this.RequestId = requestType;

            Dictionary<FieldIndex, string> innerDict;
            if (DetailData.FieldIndexDataString.TryGetValue(code, out innerDict)
                && innerDict.TryGetValue(FieldIndex.EMCode, out   this.TableKeyCode))
            {
                // Get the customs macroIds from local file.
                HashSet<string> macroIds;
                if (StockCustIndicatorFileIO.TryGetCustIndicatorIds(code, requestType, out macroIds))
                {
                    //MacroIds = macroIds.ToList();
                }
            }
        }

        /// <summary>
        /// 生产请求内容
        /// </summary>
        public override string CreateCommand()
        {
            if (this.MacroIds.Count == 0)
            {
                Cmd = string.Format(IndicatorDataPacket.DefaultIndicatorsReportCmd,
                    this.TableKeyCode, (int)this.RequestId);
            }
            else
            {
                Cmd = string.Format(IndicatorDataPacket.CustomIndicatorsReportCmd,
                    this.TableKeyCode, string.Join(",", MacroIds.ToArray()), (int)this.RequestId);
            }

            return Cmd;
        }

    }

    /// <summary>
    /// 宏观指标的指标值——请求包
    /// </summary>
    public class ReqIndicatorValuesDataPacket : IndicatorDataPacket
    {
        /// <summary>
        /// DateForamt string: "yyyy-MM-dd" 
        /// </summary>
        private const string DateFormateStr = "yyyy-MM-dd";
        /// <summary>
        /// 
        /// </summary>
        public string MacroId;
        /// <summary>
        /// 
        /// </summary>
        public DateTime StartDate;
        /// <summary>
        /// 
        /// </summary>
        public DateTime EndDate;
        /// <summary>
        /// 
        /// </summary>
        public ReqIndicatorValuesDataPacket()
        {
            this.RequestId = IndicateRequestType.IndicatorValuesReport;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="macroId">宏观指标Id</param>
        public ReqIndicatorValuesDataPacket(string macroId)
            : this()
        {
            this.MacroId = macroId;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="macroId">宏观指标Id</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="endDate">结束日期</param>
        public ReqIndicatorValuesDataPacket(string macroId, DateTime startDate, DateTime endDate)
            : this(macroId)
        {
            this.StartDate = startDate;
            this.EndDate = endDate;
        }
        /// <summary>
        /// 生产请求内容
        /// </summary>
        public override string CreateCommand()
        {
            this.Cmd = string.Format(IndicatorDataPacket.IndicatorValuesReportCmd,
                this.MacroId, StartDate.ToString(DateFormateStr), this.EndDate.ToString(DateFormateStr));

            return Cmd;
        }
    }
    #endregion
}
