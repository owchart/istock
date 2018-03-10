using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Timers;
using EmCore;
using EmQComm;
using Timer = System.Timers.Timer;
using EmQDataCore;

namespace EmQDS.Data
{
    public class CustomReportData : QuoteDataBase
    {
        public delegate void SendDataReceive(Dictionary<int, List<FieldIndex>> ChangedFields);

        public delegate void SendDataReceiveEmCode(Dictionary<string, List<FieldIndex>> ChangedFields);

        public delegate void SearchFinishHandle(int id);

        /// <summary>
        /// 内码查询结束
        /// </summary>
        public event SearchFinishHandle SearchFinished;

        /// <summary>
        /// 收到数据
        /// </summary>
        public event SendDataReceive ReceiveCustomReportData;

        /// <summary>
        /// 收到数据
        /// </summary>
        public event SendDataReceiveEmCode ReceiveCustomReportDataEmCode;



        /// <summary>
        /// 内码集合
        /// </summary>
        private List<int> _codes;

        private Dictionary<string, int> EmcodeToUnicode;

        private Stopwatch _stopwatch;


        /// <summary>
        /// 债券信息集合
        /// </summary>
        public List<Security> Securities
        {
            get { return _securities; }
            set
            {
                if (value != null && _securities != value)
                {
                    _securities = value;
                    if (EmcodeToUnicode != null)
                        EmcodeToUnicode.Clear();
                    foreach (Security security in _securities)
                    {
                        if (!DetailData.FieldIndexDataString.ContainsKey(Convert.ToInt32(security.UserDefine)))
                        DetailData.SetStockBasicField(Convert.ToInt32(security.UserDefine), security.EmCode,
                            security.StrTypeCode, security.Chinese);

                        EmcodeToUnicode[security.EmCode] = Convert.ToInt32(security.UserDefine);
                        Int32 innerCode = Convert.ToInt32(security.UserDefine);
                        if (!_codes.Contains(innerCode))
                            _codes.Add(innerCode);
                    }
                }
            }
        }

        private List<Security> _securities;

        /// <summary>
        /// 字段集合
        /// </summary>
        public List<short> Fields
        {
            get { return _fields; }
            set { _fields = value; }
        }

        public CustomReportData()
        {
            _codes = new List<int>(0);
            EmcodeToUnicode = new Dictionary<string, int>(1);
        }
        /// <summary>
        /// 设置股票代码
        /// </summary>
        /// <param name="id">标识</param>
        /// <param name="emcodes"></param>
        public void SetEmCode(int id, string emcodes)
        {
        }

        protected override void _cm_DoCMReceiveData(object sender, EmQTCP.CMRecvDataEventArgs e)
        {
            if (e.DataPacket is ResCustomReportOrgDataPacket)
            {
                if (ReceiveCustomReportData != null)
                    ReceiveCustomReportData(((ResCustomReportOrgDataPacket)e.DataPacket).ChangedFields);

                if (ReceiveCustomReportDataEmCode != null)
                {
                    Dictionary<string, List<FieldIndex>> ChangedFields =
                        new Dictionary<string, List<FieldIndex>>(
                            ((ResCustomReportOrgDataPacket)e.DataPacket).ChangedFields.Count);
                    foreach (
                        KeyValuePair<int, List<FieldIndex>> onePair in
                            ((ResCustomReportOrgDataPacket)e.DataPacket).ChangedFields)
                    {
                        Dictionary<FieldIndex, object> memData;
                        ChangedFields[Dc.GetFieldDataString(onePair.Key,FieldIndex.EMCode)] = onePair.Value;
                    }
                    ReceiveCustomReportDataEmCode(ChangedFields);
                }
            }
        }

        /// <summary>
        /// 获取一只股票，一个字段的数据
        /// </summary>
        /// <param name="code"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public object GetFieldData(int code, FieldIndex field)
        {
            return FieldInfoHelper.GetObjectValue(code, field);
        }

        public object GetFieldData(string emcode, FieldIndex field)
        {
            int unicode;
            if (EmcodeToUnicode.TryGetValue(emcode, out unicode))
            {
                return GetFieldData(unicode, field);
            }
            return null;
        }

        private ReqCustomReportOrgDataPacket _packet;
        private List<short> _fields;

        protected override void SubscribeData()
        {
            _packet = new ReqCustomReportOrgDataPacket();
            _packet.CustomCodeList = _codes;
            _packet.FieldFlag = 1;
            _packet.FieldIndexList = Fields;
            _packet.IsPush = true;
            Cm.Request(_packet);
        }

        protected override void CancelSubscribe()
        {
            if (_packet != null)
            {
                _packet.IsPush = false;
                Cm.Request(_packet);
            }

        }
    }
}
