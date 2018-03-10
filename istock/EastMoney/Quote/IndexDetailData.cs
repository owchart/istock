using System.Collections.Generic;
using EmQComm;
using EmQDataCore;

namespace EmQDS.Data {
    /// <summary>
    /// IndexDetailData
    /// </summary>
    public class IndexDetailData : QuoteDataBase {
        private List<int> _indexCodes;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="codes"></param>
        public IndexDetailData(List<int> codes) {
            _indexCodes = codes;
            IsPush = true;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="codes"></param>
        public IndexDetailData(List<string> codes) {
            _indexCodes = new List<int>();
            foreach (string code in codes) {
                int unicode = 0;
                if (DetailData.EmCodeToUnicode.TryGetValue(code, out unicode))
                    _indexCodes.Add(unicode);

            }

            IsPush = true;
        }

        /// <summary>
        /// SubscribeData
        /// </summary>
        protected override void SubscribeData() {
            ReqIndexDetailDataPacket packet = new ReqIndexDetailDataPacket();
            packet.CodeList = _indexCodes;
            Cm.Request(packet);
        }
        /// <summary>
        /// _cm_DoCMReceiveData
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void _cm_DoCMReceiveData(object sender, EmQTCP.CMRecvDataEventArgs e) {
            if (e.DataPacket is ResIndexDetailDataPacket) {
                SendDataReceived((e.DataPacket as ResIndexDetailDataPacket).Codes);
            }
        }

        /// <summary>
        /// 获取一只股票，一个字段的值
        /// </summary>
        /// <param name="code">股票代码</param>
        /// <param name="field">字段名称</param>
        /// <returns>object字段值</returns>
        public object GetIndexQuoteData(int code, FieldIndex field)
        {
            return FieldInfoHelper.GetObjectValue(code, field);
        }
    }
}
