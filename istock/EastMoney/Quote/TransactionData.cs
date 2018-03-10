using System;
using System.Collections.Generic;
using System.Text;
using EmQComm;

namespace EmQDS.Data {
	/// <summary>
	/// 盘口异动
	/// </summary>
	public class TransactionData:QuoteDataBase {
        /// <summary>
        /// TransactionDataArgs
        /// </summary>
		public class TransactionDataArgs:EventArgs {
            private int _code1;
            private string _name;
            private string _message;

            /// <summary>
            /// TransactionDataArgs
            /// </summary>
            /// <param name="code"></param>
            /// <param name="name"></param>
            /// <param name="message"></param>
            public TransactionDataArgs(int code, string name, string message)
            {
				this.Code = code;
				this.Name = name;
				this.Message = message;
			}
            /// <summary>
            /// Code
            /// </summary>
            public int Code
            {
                get { return _code1; }
                set { _code1 = value; }
            }

            /// <summary>
            /// Name
            /// </summary>
			public string Name
            {
                get { return _name; }
                set { _name = value; }
            }

            /// <summary>
            /// Message
            /// </summary>
			public string Message
            {
                get { return _message; }
                set { _message = value; }
            }
		}
        /// <summary>
        /// SubscribeData
        /// </summary>
		protected override void SubscribeData() {
			ReqShortLineStrategyDataPacket dataPacket = new ReqShortLineStrategyDataPacket();
            dataPacket.Count = 20;
            dataPacket.IsPush = 1;
            Cm.Request(dataPacket);
		}
        /// <summary>
        /// _cm_DoCMReceiveData
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		protected override void _cm_DoCMReceiveData(object sender, EmQTCP.CMRecvDataEventArgs e) {
			if (e.DataPacket is ResShortLineStragedytDataPacket){
				if(null != OnTransactionReceived){
                    List<OneShortLineDataRec> temp = Dc.GetShortLineData(Dc.UserShortLineTypes);
                    if (temp.Count == 0 || null==Dc.GetFieldDataString(temp[temp.Count - 1].Code, FieldIndex.Name))
						return;
					TransactionDataArgs args = new TransactionDataArgs(temp[temp.Count - 1].Code,
                                                                       Dc.GetFieldDataString(temp[temp.Count - 1].Code, FieldIndex.Name),
					                                                   EnumUtility.GetShortLineTypeString(temp[temp.Count - 1].SlType));
					OnTransactionReceived(args);
				}
			}
		}
        /// <summary>
        /// TransactionReceivedHandle
        /// </summary>
        /// <param name="args"></param>
		public delegate void TransactionReceivedHandle(TransactionDataArgs args);
        /// <summary>
        /// OnTransactionReceived
        /// </summary>
		public event TransactionReceivedHandle OnTransactionReceived;
	}
}
