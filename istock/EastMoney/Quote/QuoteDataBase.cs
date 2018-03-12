using System;
using System.Collections.Generic;

namespace OwLib
{
    /// <summary>
    /// QuoteDataBase
    /// </summary>
    public class QuoteDataBase:IDisposable
    {
        /// <summary>
        /// 是否推送数据
        /// </summary>
        public bool IsPush
        {
            get { return _isPush; }
            set { _isPush = value; }
        }

        /// <summary>
        /// 股票代码，带市场类型的，如"600000.SH"
        /// </summary>
        public int Code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        /// 网络层实例
        /// </summary>
        protected readonly ConnectManager2 Cm;

        /// <summary>
        /// 数据层实例
        /// </summary>
        protected readonly DataCenterCore Dc;

        private int _code;
        private bool _isPush;
        private bool _isInvokedStart = false;
        /// <summary>
        /// 收到数据后的消息
        /// </summary>
        public event DataReceivedHandle DataReceived;
        /// <summary>
        /// 收到多个数据后的消息
        /// </summary>
        public event DataReceivedsHandle DataReceiveds;
        /// <summary>
        /// DataReceivedHandle
        /// </summary>
        /// <param name="code"></param>
        public delegate void DataReceivedHandle(int code);
        /// <summary>
        /// 多个code一起回调
        /// </summary>
        /// <param name="codes"></param>
        public delegate void DataReceivedsHandle(IList<int> codes);
       /// <summary>
       /// 构造函数
       /// </summary>
        public QuoteDataBase()
        {
            Dc = DataCenterCore.CreateInstance();
            Cm = ConnectManager2.CreateInstance();
           
            IsPush = false;
        }
        /// <summary>
        /// RequestData
        /// </summary>
        protected virtual void RequestData(){}
        /// <summary>
        /// SubscribeData
        /// </summary>
        protected virtual void SubscribeData(){}
        /// <summary>
        /// CancelSubscribe
        /// </summary>
        protected virtual void CancelSubscribe()
        {
            
        }
        /// <summary>
        /// _cm_DoCMReceiveData
        /// </summary>
        protected virtual void _cm_DoCMReceiveData(object sender, CMRecvDataEventArgs e){}

        /// <summary>
        /// 开始请求数据
        /// </summary>
        public void Start() {
            Cm.DoCMReceiveData += new EventHandler<CMRecvDataEventArgs>(_cm_DoCMReceiveData);
            Cm.DoAddOneClient += Cm_DoAddOneClient;
            RequestData();
            SubscribeData();
            _isInvokedStart = true;
        }

        void Cm_DoAddOneClient(object sender, ConnectEventArgs e) {
            if (_isInvokedStart) {
                RequestData();
                SubscribeData();
            }
        }

        /// <summary>
        /// 退出本次请求
        /// </summary>
        public void Quit() {
            Cm.DoCMReceiveData -= new EventHandler<CMRecvDataEventArgs>(_cm_DoCMReceiveData);
            Cm.DoAddOneClient -= Cm_DoAddOneClient;
            CancelSubscribe();
            _isInvokedStart = false;
        }
        /// <summary>
        /// SendDataReceived
        /// </summary>
        /// <param name="code"></param>
        protected void SendDataReceived(int code)
        {
            if(DataReceived != null)
                DataReceived(code);
        }

        protected void SendDataReceived(IList<int> codes ){
          if (null != DataReceiveds){
              DataReceiveds(codes);
          }  
        }

	    public void Dispose(){
		    GC.SuppressFinalize(this);
	    }

    }
}
