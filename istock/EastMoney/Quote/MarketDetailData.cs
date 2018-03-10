using System;
using System.Collections.Generic;
using EmQComm;

namespace EmQDS.Data
{
    /// <summary>
    /// MarketDetailData
    /// </summary>
    public class MarketDetailData : QuoteDataBase
    {
        private ReqSecurityType _type;
        private List<byte> _fieldList;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fieldList"></param>
        public MarketDetailData(ReqSecurityType type, List<byte> fieldList)
        {
            _type = type;
            _fieldList = fieldList;
            IsPush = false;
        }
        /// <summary>
        /// SubscribeData
        /// </summary>
        protected override void SubscribeData()
        {
            ReqSectorQuoteReportDataPacket packet = new ReqSectorQuoteReportDataPacket((Convert.ToInt32(_type)).ToString());
            packet.FieldIndexList = _fieldList;
            Cm.Request(packet);
        }
        /// <summary>
        /// CancelSubscribe
        /// </summary>
        protected override void CancelSubscribe()
        {
            ReqSectorQuoteReportDataPacket packet = new ReqSectorQuoteReportDataPacket((Convert.ToInt32(_type)).ToString());
            packet.FieldIndexList = _fieldList;
            packet.IsPush = false;
            Cm.Request(packet);
        }
        /// <summary>
        /// _cm_DoCMReceiveData
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void _cm_DoCMReceiveData(object sender, EmQTCP.CMRecvDataEventArgs e)
        {
            if(e.DataPacket is ResSectorQuoteReportDataPacket)
            {
                if(((ResSectorQuoteReportDataPacket)e.DataPacket).PacketStatus == 1)
                {
                    SendDataReceived(0);
                    if(IsPush == false)
                    {
                        Quit();
                    }
                }
            }
        }

        ///// <summary>
        ///// 获取代码字段的键值对
        ///// </summary>
        ///// <returns></returns>
        //public Dictionary<int, Dictionary<FieldIndex, object>> GetFieldValue()
        //{
        //    return Dc.GetDetailData();
        //}
    }
}
