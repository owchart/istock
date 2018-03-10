using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using EmQComm;

namespace EmQDS.Data
{
    /// <summary>
    /// 新闻数据接口
    /// </summary>
    public sealed class NewsData : QuoteDataBase
    {

        /// <summary>
        /// 新闻数据变动事件参数
        /// </summary>
        public class NewsDataChangedArgs : EventArgs
        {
            private int _newCount;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="count"></param>
            public NewsDataChangedArgs(int count)
            {
                NewCount = count;
            }

            /// <summary>
            /// 新数据条数
            /// </summary>
            public int NewCount
            {
                get { return _newCount; }
                set { _newCount = value; }
            }
        }

        IList<OneNews24HOrgDataRec> _data;
        //OneNews24HOrgDataRec _lastData;
        int _lastDataDate = 0;
        int _lastDataTime = 0;

        /// <summary>
        /// 新闻资讯服务
        /// </summary>
        public NewsData()
        {
            _data = new List<OneNews24HOrgDataRec>();
            HasSetTimer = false;
        }

        /// <summary>
        /// 新闻列表
        /// </summary>
        public IList<OneNews24HOrgDataRec> Data { get { return _data; } }

        private Timer _timer;
        private int _currentMsgId;
        private bool _hasSetTimer;

        /// <summary>
        /// 当前有效的msgid
        /// </summary>
        private int CurrentMsgId
        {
            get { return _currentMsgId; }
            set { _currentMsgId = value; }
        }

        /// <summary>
        /// 是否已经设置定时器
        /// </summary>
        private bool HasSetTimer
        {
            get { return _hasSetTimer; }
            set { _hasSetTimer = value; }
        }

        /// <summary>
        /// 请求数据
        /// </summary>
        protected override void RequestData()
        {
            if (!HasSetTimer)
            {
                HasSetTimer = true;
                _timer = new Timer(15000);
                _timer.Elapsed += TimerElapsed;
                _timer.AutoReset = true;
                _timer.Start();
            }
            ReqNews24HOrgDataPacket data = new ReqNews24HOrgDataPacket();
            data.MaxCount = 50;
            //if (_lastData != null)
            //{
            data.LastDate = _lastDataDate;
            data.LastTime = _lastDataTime;
            //}
            //if (_data.Count != 0)
            //{
            //    data.LastDate = _data[0].PublishDate;
            //    data.LastTime = _data[0].PublishTime;
            //}
            Cm.Request(data);
            this.CurrentMsgId = data.MsgId;
        }

        void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            RequestData();
        }

        protected override void _cm_DoCMReceiveData(object sender, EmQTCP.CMRecvDataEventArgs e)
        {
            if (e.DataPacket.MsgId != CurrentMsgId)
                return;
            if (e.DataPacket is ResNews24HOrgDataPacket && e.DataPacket.MsgId == this.CurrentMsgId)
            {
                int newsCount = ((ResNews24HOrgDataPacket)e.DataPacket).News24HData.Count;
                NewsDataChangedArgs args = new NewsDataChangedArgs(newsCount);
                _data = Dc.GetNews24HOrgData();
                //_lastData = _data[0];
                if (_data.Count > 0) {
                    _lastDataDate = _data[0].PublishDate;
                    _lastDataTime = _data[0].PublishTime;
                }
                if (OnNewsReceived != null)
                    OnNewsReceived(args);
            }
        }

        /// <summary>
        /// 收到新新闻委托
        /// </summary>
        /// <param name="args"></param>
        public delegate void NewsReceivedHandle(NewsDataChangedArgs args);
        /// <summary>
        /// 收到新新闻事件
        /// </summary>
        public event NewsReceivedHandle OnNewsReceived;
    }
}
