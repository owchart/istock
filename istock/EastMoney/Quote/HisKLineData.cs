using System.Collections.Generic;

namespace OwLib
{
    /// <summary>
    /// OneStockHisKLineData
    /// </summary>
    public class OneStockHisKLineData : QuoteDataBase
    {
        private KLineCycle _cycle;
        private int _date1, _date2;
        private int _code;
        public ReqKLineDataRange _reqKLineDataRange;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cycle"></param>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        public OneStockHisKLineData(int code, KLineCycle cycle, int date1, int date2)
        {
            _reqKLineDataRange = ReqKLineDataRange.StartDateToEndDate;
            _cycle = cycle;
            _code = code;
            _date1 = date1;
            _date2 = date2;
        }
        /// <summary>
        /// RequestData
        /// </summary>
        protected override void RequestData()
        {
            ReqHisKLineDataPacket packet = new ReqHisKLineDataPacket();
            packet.Code = _code;
            packet.Cycle = _cycle;
            packet.DataRange = _reqKLineDataRange;
            packet.StartDate = _date1;
            packet.EndDate = _date2;
            Cm.Request(packet);
        }
        /// <summary>
        /// _cm_DoCMReceiveData
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void _cm_DoCMReceiveData(object sender, CMRecvDataEventArgs e)
        {
            lock (this)
            {
                if (e.DataPacket is ResHisKLineDataPacket)
                {
                    if (((ResHisKLineDataPacket)e.DataPacket).KLineDataRec.Code == _code &&
                        ((ResHisKLineDataPacket)e.DataPacket).KLineDataRec.Cycle == _cycle)
                    {
                        SendDataReceived(_code);
                    }
                }
            }
        }
        
        /// <summary>
        /// 获取一只股票K线的数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cycle"></param>
        /// <returns></returns>
        public OneStockKLineDataRec GetKLineData(int code, KLineCycle cycle)
        {
            return Dc.GetHisKLineData(code, cycle);
        }
    }
    /// <summary>
    /// GroupStockHisKLineData
    /// </summary>
    public class GroupStockHisKLineData : QuoteDataBase
    {
        private List<int> _codes;
        private KLineCycle _cycle;
        private int _date1, _date2;
        private ReqKLineDataRange _reqKLineDataRange;

        private List<int> _receivedCodes = new List<int>(); 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="codes"></param>
        /// <param name="cycle"></param>
        /// <param name="date1"></param>
        /// <param name="date2"></param>
        public GroupStockHisKLineData(List<int> codes, KLineCycle cycle, int date1, int date2)
        {
            _codes = codes;
            _cycle = cycle;
            _date1 = date1;
            _date2 = date2;
            _reqKLineDataRange = ReqKLineDataRange.StartDateToEndDate;
        }
        /// <summary>
        /// RequestData
        /// </summary>
        protected override void RequestData()
        {
            List<DataPacket> packets = new List<DataPacket>(_codes.Count);
            foreach (int code in _codes)
            {
                ReqHisKLineDataPacket packet = new ReqHisKLineDataPacket();
                packet.Code = code;
                packet.Cycle = _cycle;
                packet.DataRange = _reqKLineDataRange;
                packet.StartDate = _date1;
                packet.EndDate = _date2;
                packets.Add(packet);
            }
            Cm.Request(packets);
        }
        /// <summary>
        /// _cm_DoCMReceiveData
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void _cm_DoCMReceiveData(object sender, CMRecvDataEventArgs e)
        {
            if(e.DataPacket is ResHisKLineDataPacket)
            {
                if(((ResHisKLineDataPacket)e.DataPacket).KLineDataRec.Cycle == _cycle && 
                    !_receivedCodes.Contains(((ResHisKLineDataPacket)e.DataPacket).KLineDataRec.Code))
                {
                    _receivedCodes.Add(((ResHisKLineDataPacket)e.DataPacket).KLineDataRec.Code);
                    if(_receivedCodes.Count == _codes.Count)
                        SendDataReceived(_codes[0]);
                }
            }
        }

        /// <summary>
        /// 获取一只股票K线的数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cycle"></param>
        /// <returns></returns>
        public OneStockKLineDataRec GetKLineData(int code, KLineCycle cycle)
        {
            return Dc.GetHisKLineData(code, cycle);
        }

    }
}
