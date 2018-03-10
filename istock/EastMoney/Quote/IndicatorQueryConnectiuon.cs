using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using EmCore;
using EmQComm;
using EmSerDataService;
using EmSocketClient;
using System.Data;

namespace EmQTCP
{
    /// <summary>
    /// 宏观指标请求通道
    /// </summary>
    public class IndicatorQueryConnectiuon
    {
        private DataQuery _dataQuery;
        private string _id;
        private Dictionary<string, int> _dicMsgId;
        private Queue<CMRecvDataEventArgs> _DataPacketQueue;
        private Thread _DataPacketPushSlave;

        private Queue<IndicatorDataPacket> _SendDataPacketQueue;
        private Thread _SendDataPacketPushSlave;

        ///<summary>
        /// 收到数据包的消息处理函数
        ///</summary>
        public event EventHandler<CMRecvDataEventArgs> OnReceiveData;

        public static DataTable SimpleTest(string emCode, List<string> MacroIds, IndicateRequestType type)
        {
          string Cmd = string.Format(IndicatorDataPacket.CustomIndicatorsReportCmd,
                    emCode, string.Join(",", MacroIds.ToArray()), (int)type);
          DataQuery _dataQuery = new DataQuery();
          DataSet ds = _dataQuery.QueryMacroIndicate(Cmd) as DataSet;
          return ds.Tables[0];
        }
        public static DataTable SimpleTest(string Cmd)
        {
            DataTable result = new DataTable();
            try
            {
                DataQuery _dataQuery = new DataQuery();
                DataSet ds = _dataQuery.QueryMacroIndicate(Cmd) as DataSet;
                result = ds.Tables[0];
            }
            catch
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// construct
        /// </summary>
        public IndicatorQueryConnectiuon(DataQuery dataQuery)
        {
            _dataQuery = dataQuery;
            _dicMsgId = new Dictionary<string, int>();
            _DataPacketQueue = new Queue<CMRecvDataEventArgs>();
            _DataPacketPushSlave = new Thread(PushDataPacket);
            _DataPacketPushSlave.IsBackground = false;
            _DataPacketPushSlave.Start();
            while (!_DataPacketPushSlave.IsAlive) ;

            _SendDataPacketQueue = new Queue<IndicatorDataPacket>();
            _SendDataPacketPushSlave = new Thread(PushSendDataPacket);
            _SendDataPacketPushSlave.IsBackground = false;
            _SendDataPacketPushSlave.Start();
            while (!_SendDataPacketPushSlave.IsAlive) ;
        }

        /// <summary>
        /// 发送请求包
        /// </summary>
        /// <param name="packet"></param>
        public void DoSendPacket(string cmd, int msgId)
        {
            IndicatorDataPacket sendPacket = new IndicatorDataPacket();
            sendPacket.Cmd = cmd;
            sendPacket.MsgId = msgId;
            lock (_SendDataPacketQueue)
                _SendDataPacketQueue.Enqueue(sendPacket);
        }

        /// <summary>
        /// 异步接收数据
        /// </summary>
        /// <param name="response"></param>
        public void SendDataCallBack(MessageEntity response)
        {
            if (response.MsgBody is DataSet)
            {
                IndicatorDataPacket dataPacket = null;
                using (DataSet ds = response.MsgBody as DataSet)
                {
                    if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                    {
                        lock (ds)
                        {
                            using (DataTable dt = ds.Tables[0])
                            {
                                IndicateRequestType requestId;
                                string tableKeyCode;

                                if (TryGetRequestType(dt.TableName, out requestId, out tableKeyCode))
                                {
                                    switch (requestId)
                                    {
                                        case IndicateRequestType.LeftIndicatorsReport:
                                            dataPacket = new ResIndicatorsReportDataPacket(tableKeyCode);
                                            break;

                                        case IndicateRequestType.RightIndicatorsReport:
                                            dataPacket = new ResIndicatorsReportDataPacket(tableKeyCode);
                                            break;

                                        case IndicateRequestType.IndicatorValuesReport:
                                            dataPacket = new ResIndicatorValuesDataPacket(tableKeyCode);
                                            break;

                                    }

                                    dataPacket.RequestId = requestId;
                                    if (_dicMsgId.ContainsKey((string)response.Tag))
                                    {
                                        dataPacket.MsgId = _dicMsgId[(string)response.Tag];
                                        LogUtilities.LogMessage("收到响应, id=" 
                                            + (string)response.Tag + ", msgId=" + dataPacket.MsgId);
                                        lock (_dicMsgId)
                                            _dicMsgId.Remove((string)response.Tag);
                                    }

                                    dataPacket.Decoding(dt);
                                }
                            }
                        }
                    }

                }
                if (dataPacket != null)
                {
                    lock (_DataPacketQueue)
                        _DataPacketQueue.Enqueue(new CMRecvDataEventArgs(TcpService.ZXCFT,
                            dataPacket, 100000));
                }
            }
        }

        /// <summary>
        /// 解析宏观指标的表名字节流，得到报表请求类型类型及请求code
        /// (当请求股票对应的左右宏观报表，code是股票名称 eg. "000960.SZ";
        /// 当请求宏观指标值，code是指标id eg. "EMI00064805")
        /// </summary>
        /// <param name="tableName">返回的待处理字节流</param>
        /// <param name="requestType">报表请求类型</param>
        /// <param name="tableKeyCode">请求code</param>
        /// <returns></returns>
        private bool TryGetRequestType(string tableName, out IndicateRequestType requestType,
            out string tableKeyCode)
        {
            requestType = IndicateRequestType.LeftIndicatorsReport;
            tableKeyCode = tableName;
            if (string.IsNullOrEmpty(tableName))
            {
                //Log error;
                return false;
            }
            string[] arr = tableName.Split('_');

            if (arr == null || arr.Length == 0)
                return false;

            if (arr.Length == 1)
            {
                requestType = IndicateRequestType.IndicatorValuesReport;
                tableKeyCode = arr[0];
                return true;
            }

            if (arr.Length == 2)
            {
                if (arr[0].StartsWith("1"))
                {
                    requestType = IndicateRequestType.LeftIndicatorsReport;
                }
                else if (arr[0].StartsWith("2"))
                {
                    requestType = IndicateRequestType.RightIndicatorsReport;
                }

                tableKeyCode = arr[1];
                return true;

            }

            return false;
        }

        void PushDataPacket()
        {
            while (true)
            {
                CMRecvDataEventArgs dataPacket = null;
                lock (_DataPacketQueue)
                {
                    if (_DataPacketQueue.Count > 0)
                    {
                        dataPacket = _DataPacketQueue.Dequeue();
                    }
                }
                if (dataPacket != null)
                {
                    if (OnReceiveData != null)
                        OnReceiveData(this, dataPacket);
                }
                Thread.Sleep(2);
            }
        }

        void PushSendDataPacket()
        {

            while (true)
            {
                try
                {
                    IndicatorDataPacket dataPacket = null;
                    lock (_SendDataPacketQueue)
                    {
                        if (_SendDataPacketQueue.Count > 0)
                        {
                            dataPacket = _SendDataPacketQueue.Dequeue();
                        }
                    }
                    if (dataPacket != null)
                    {
                        if (_dataQuery != null)
                        {
                            _dataQuery.QueryMacroIndicate(dataPacket.Cmd, out _id, SendDataCallBack);
                            lock (_dicMsgId)
                            {
                                _dicMsgId.Add(_id, dataPacket.MsgId);
                                LogUtilities.LogMessage("发送请求, id=" + _id + ", msgId=" + dataPacket.MsgId);
                            }
                        }
                    }
                    Thread.Sleep(2);
                }
                catch (Exception e)
                {
                    LogUtilities.LogMessage("Indicator 请求报错," + e.Message);
                    Thread.Sleep(2);
                }

            }
        }

    }
}
