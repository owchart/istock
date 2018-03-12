using System.Collections.Generic;

namespace OwLib
{
    /// <summary>
    /// 单支股票行情
    /// </summary>
    public class OneStockDetailData : QuoteDataBase
    {
        private bool _hasLevel2;

        /// <summary>
        /// 是否有Level2权限
        /// </summary>
        public bool HasLevel2
        {
            get { return _hasLevel2; }
            set { _hasLevel2 = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public OneStockDetailData()
        {
            HasLevel2 = true;//临时
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
                if (e.DataPacket is ResStockDetailLev2DataPacket && HasLevel2)
                {
                    if (((ResStockDetailLev2DataPacket)e.DataPacket).Code == Code)
                    {
                        SendDataReceived(Code);
                        if (IsPush == false)
                            Quit();
                    }

                }
                else if (e.DataPacket is ResStockDetailDataPacket && !HasLevel2)
                {
                    if (((ResStockDetailDataPacket)e.DataPacket).Code == Code)
                    {
                        SendDataReceived(Code);
                        if (IsPush == false)
                            Quit();
                    }
                }
            }
           
        }

        private DataPacket m_packet;
        
        /// <summary>
        /// SubscribeData
        /// </summary>
        protected override void SubscribeData()
        {
            DataPacket packet;
            if (HasLevel2)
            {
                packet = new ReqStockDetailLev2DataPacket();
                (packet as ReqStockDetailLev2DataPacket).Code = Code;
            }
            else
            {
                packet = new ReqStockDetailDataPacket();
                (packet as ReqStockDetailDataPacket).Code = Code;
            }
            Cm.Request(packet);
            m_packet = packet;
        }

        /// <summary>
        /// CancelSubscribe
        /// </summary>
        protected override void CancelSubscribe()
        {
            if (m_packet != null)
            {
                m_packet.IsPush = false;
                Cm.Request(m_packet);
            }
        }
    }


    /// <summary>
    /// 一组股票行情
    /// </summary>
    public class GroupStockDetailData : QuoteDataBase
    {
        /// <summary>
        /// 股票代码集合
        /// </summary>
        public List<int> Codes
        {
            get { return _codes; }
            set { _codes = value; }
        }

        private List<int> ReceivedCodes = new List<int>();
        private List<int> _codes;

        /// <summary>
        /// SubscribeData
        /// </summary>
        protected override void SubscribeData()
        {
            List<DataPacket> packets = new List<DataPacket>();
            foreach (int code in Codes)
            {
                OneStockDetailData oneStockDetailData = new OneStockDetailData();
                oneStockDetailData.Code = code;
                oneStockDetailData.IsPush = false;
                oneStockDetailData.DataReceived += new DataReceivedHandle(oneStockDetailData_DataReceived);
                
                ReqStockDetailLev2DataPacket packet = new ReqStockDetailLev2DataPacket();
                packet.Code = code;
                packets.Add(packet);
            }
            Cm.Request(packets);
        }

        void oneStockDetailData_DataReceived(int code)
        {
            lock (this)
            {
                if (!ReceivedCodes.Contains(code))
                {
                    ReceivedCodes.Add(code);
                    if (ReceivedCodes.Count == Codes.Count)
                        SendDataReceived(code);
                }
            }

        }

    }
}
