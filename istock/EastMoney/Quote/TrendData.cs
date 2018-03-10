using EmQComm;

namespace EmQDS.Data
{
    /// <summary>
    /// TrendData
    /// </summary>
    public class TrendData : QuoteDataBase
    {
        /// <summary>
        /// 日期，格式如20120516
        /// </summary>
        public int Date
        {
            get { return _date; }
            set { _date = value; }
        }

        private DataPacket _subscribeDataPacket;
        private int _date;

        /// <summary>
        /// 构造函数
        /// </summary>
        public TrendData()
        {
            Date = Dc.GetTradeDate(Code);
        }

        /// <summary>
        /// 获取走势数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public OneDayTrendDataRec GetTrendData(int code, int date)
        {
            return Dc.GetTrendData(code,date);
        }

        /// <summary>
        /// 获取走势数据
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public OneDayTrendDataRec GetTrendData(int code)
        {
            return Dc.GetTrendData(code);
        }
        /// <summary>
        /// _cm_DoCMReceiveData
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void _cm_DoCMReceiveData(object sender, EmQTCP.CMRecvDataEventArgs e)
        {
            if(e.DataPacket is ResStockTrendDataPacket)
            {
                
            }
        }
        /// <summary>
        /// RequestData
        /// </summary>
        protected override void RequestData()
        {
            int index = 0;
            OneDayTrendDataRec data = Dc.GetTrendData(Code, Date);
            if(data != null)
                index = data.RequestLastPoint;

            ReqStockTrendDataPacket packet = new ReqStockTrendDataPacket();
            packet.Code = this.Code;
            packet.Date = this.Date;
            packet.LastRequestPoint = index;
            Cm.Request(packet);
        }
        /// <summary>
        /// SubscribeData
        /// </summary>
        protected override void SubscribeData()
        {
            MarketType mt = Dc.GetMarketType(Code);
            switch (mt)
            {
                case MarketType.SHALev2:
                case MarketType.SZALev2:
                case MarketType.SHBLev2:
                case MarketType.SZBLev2:
                    _subscribeDataPacket = new ReqStockDetailLev2DataPacket();
                    ((ReqStockDetailLev2DataPacket)_subscribeDataPacket).Code = this.Code;
                    break;
                default:
                    _subscribeDataPacket = new ReqStockDetailDataPacket();
                    ((ReqStockDetailDataPacket)_subscribeDataPacket).Code = this.Code;
                    break;
            }
            Cm.Request(_subscribeDataPacket);    
        }
        /// <summary>
        /// CancelSubscribe
        /// </summary>
        protected override void CancelSubscribe()
        {
            if(_subscribeDataPacket is ReqStockDetailLev2DataPacket)
                ((ReqStockDetailLev2DataPacket) _subscribeDataPacket).IsPush = false;
            else if(_subscribeDataPacket is ReqStockDetailDataPacket)
                ((ReqStockDetailDataPacket)_subscribeDataPacket).IsPush = false;
            Cm.Request(_subscribeDataPacket);
        }
    }
}
