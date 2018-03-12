using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace OwLib
{
    /// <summary>
    /// 同花顺交易结构定义
    /// </summary>
    public class THSDealInfo
    {
        // 交易类型
        // 0:默认值
        // 1:买入
        // 2:卖出
        // 3:撤销买入
        // 4:撤销卖出
        // 5:查询持仓
        // 6:查询成交
        // 7:查询资金
        // 8:查询委托
        public int m_operateType = 0;
        /// <summary>
        /// 请求编号
        /// </summary>
        public int m_reqID;
        // 股票代码
        public String m_securityCode = "";
    }

    /// <summary>
    /// 同花顺交易返回
    /// </summary>
    /// <param name="operateType">操作类型</param>
    /// <param name="requstID">请求ID</param>
    /// <param name="result">返回的结果</param>
    public delegate void THSDealCallBack(int operateType, int requstID, String result);

    /// <summary>
    /// 同花顺交易消息监听
    /// </summary>
    public class THSDealListener
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public THSDealListener()
        {
        }

        /// <summary>
        /// 析构方法
        /// </summary>
        ~THSDealListener()
        {
            Clear();
        }

        /// <summary>
        /// 监听回调列表
        /// </summary>
        private List<THSDealCallBack> m_callBacks = new List<THSDealCallBack>();

        /// <summary>
        /// 添加回调
        /// </summary>
        /// <param name="callBack">回调</param>
        public void Add(THSDealCallBack callBack)
        {
            m_callBacks.Add(callBack);
        }

        /// <summary>
        /// 回调方法
        /// </summary>
        /// <param name="operateType">操作类型</param>
        /// <param name="requstID">请求ID</param>
        /// <param name="result">返回的结果</param>
        public void CallBack(int operateType, int requstID, String result)
        {
            int callBackSize = m_callBacks.Count;
            for (int i = 0; i < callBackSize; i++)
            {
                m_callBacks[i](operateType, requstID, result);
            }
        }

        /// <summary>
        /// 清除监听
        /// </summary>
        public void Clear()
        {
            m_callBacks.Clear();
        }

        /// <summary>
        /// 移除回调
        /// </summary>
        /// <param name="callBack">回调</param>
        public void Remove(THSDealCallBack callBack)
        {
        }
    }

    /// <summary>
    /// 同花顺交易服务
    /// </summary>
    public class THSDealService:IDisposable
    {
        public THSDealService()
        {
        }
        
        /// <summary>
        /// 所有交易请求请求
        /// </summary>
        private List<THSDealInfo> m_thsDealRequests = new List<THSDealInfo>();

        /// <summary>
        /// 监听回调列表
        /// </summary>
        private List<THSDealCallBack> m_callBacks = new List<THSDealCallBack>();

        /// <summary>
        /// 监听者集合
        /// </summary>
        private Dictionary<int, THSDealListener> m_listeners = new Dictionary<int, THSDealListener>();

        /// <summary>
        /// 推送数据的ID
        /// </summary>
        private List<int> m_pushRegisterIDs = new List<int>();

        /// <summary>
        /// 请求ID
        /// </summary>
        private int m_requestID = 80000;

        /// <summary>
        /// 添加回调
        /// </summary>
        /// <param name="callBack">回调</param>
        public void Add(THSDealCallBack callBack)
        {
            m_callBacks.Add(callBack);
        }

        /// <summary>
        /// 回调方法
        /// </summary>
        /// <param name="operateType">操作类型</param>
        /// <param name="requstID">请求ID</param>
        /// <param name="result">返回的结果</param>
        public void CallBack(int operateType, int requstID, String str)
        {
            int callBackSize = m_callBacks.Count;
            for (int i = 0; i < callBackSize; i++)
            {
                m_callBacks[i](operateType, requstID, str);
            }
        }

        /// <summary>
        /// 添加交易请求
        /// </summary>
        /// <param name="req">请求结构</param>
        public void AddTHSDealReq(THSDealInfo req)
        {
            lock (m_thsDealRequests)
            {
                m_thsDealRequests.Add(req);
            }
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public void Dispose()
        {
            m_thsDealRequests.Clear();
            m_thsDealRequests = null;
            m_callBacks.Clear();
            m_callBacks = null;
        }

        /// <summary>
        /// 当前队列中CTP请求数
        /// </summary>
        /// <returns>CTP请求数</returns>
        public int GetTHSDealCount()
        {
            int count = 0;
            lock (m_thsDealRequests)
            {
                count = m_thsDealRequests.Count;
            }
            return count;
        }

        /// <summary>
        /// 返回第一个交易请求
        /// </summary>
        /// <returns>第一个CTP请求</returns>
        public THSDealInfo GetTHSDealRequest()
        {
            THSDealInfo req = null;
            lock (m_thsDealRequests)
            {
                if (m_thsDealRequests.Count > 0)
                {
                    req = m_thsDealRequests[0];
                }
            }
            return req;
        }

        /// <summary>
        /// 获取请求ID
        /// </summary>
        /// <returns>请求ID</returns>
        public int GetRequestID()
        {
            return m_requestID++;
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="operateType">操作类型</param>
        /// <param name="requstID">请求ID</param>
        /// <param name="result">返回的结果</param>
        public void OnReceive(int operateType, int requstID, String result)
        {
            SendToListener(operateType, requstID, result);
        }

        /// <summary>
        /// 注册数据监听
        /// </summary>
        /// <param name="operateType">请求ID</param>
        /// <param name="callBack">回调函数</param>
        public void RegisterListener(int operateType, THSDealCallBack callBack)
        {
            lock (m_listeners)
            {
                THSDealListener listener = null;
                if (!m_listeners.ContainsKey(operateType))
                {
                    listener = new THSDealListener();
                    m_listeners[operateType] = listener;
                    m_pushRegisterIDs.Add(operateType);
                }
                else
                {
                    listener = m_listeners[operateType];
                }
                listener.Add(callBack);
            }
        }

        /// <summary>
        /// 移除无效的交易请求
        /// </summary>
        /// <param name="req">请求结构</param>
        public void RemoveThsDealRequest(THSDealInfo req)
        {
            lock (m_thsDealRequests)
            {
                m_thsDealRequests.Remove(req);
            }
        }

        /// <summary>
        /// 发送到监听者
        /// </summary>
        /// <param name="operateType">操作类型</param>
        /// <param name="requstID">请求ID</param>
        /// <param name="result">返回的结果</param>
        public void SendToListener(int operateType, int requstID, String result)
        {
            THSDealListener listener = null;
            lock (m_listeners)
            {
                if (m_listeners.ContainsKey(operateType))
                {
                    listener = m_listeners[operateType];
                }
            }
            if (listener != null)
            {
                listener.CallBack(operateType, requstID, result);
            }
        }

        /// <summary>
        /// 启动同花顺交易服务
        /// </summary>
        public void StartTHSDealService()
        {
            Thread sThread = new Thread(new ThreadStart(ThreadFunc));
            sThread.IsBackground = true;
            sThread.Start();
        }

        /// <summary>
        /// 同花顺交易线程处理函数
        /// </summary>
        public static void ThreadFunc()
        {
            StringBuilder str = new StringBuilder(1024 * 10);
            while (true)
            {
                THSDealInfo req = DataCenter.ThsDealService.GetTHSDealRequest();
                if (req != null)
                {
                    String result = "";
                    // 交易类型
                    // 0:默认值
                    // 1:买入
                    // 2:卖出
                    // 3:撤销买入
                    // 4:撤销卖出
                    // 5:查询持仓
                    // 6:查询成交
                    // 7:查询资金
                    // 8:查询委托
                    switch (req.m_operateType)
                    {
                        case 0:
                            break;
                        case 1:
                            break;
                        case 2:
                            break;
                        case 3:
                            AutoTradeService.CancelBuy();
                            break;
                        case 4:
                            AutoTradeService.CancelSell();
                            break;
                        case 5:
                            result = AutoTradeService.GetSecurityPosition();
                            break;
                        case 6:
                            result = AutoTradeService.GetSecurityTrade();
                            break;
                        case 7:
                            result = AutoTradeService.GetSecurityCaptial();
                            break;
                        case 8:
                            result = AutoTradeService.GetSecurityCommission();
                            break;
                    }
                    DataCenter.ThsDealService.OnReceive(req.m_operateType, req.m_reqID, result);
                    DataCenter.ThsDealService.RemoveThsDealRequest(req);
                }
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// 取消注册数据监听
        /// </summary>
        /// <param name="requestID">请求ID</param>
        public void UnRegisterListener(int requestID)
        {
            lock (m_listeners)
            {
                m_listeners.Remove(requestID);
            }
        }

        /// <summary>
        /// 取消注册监听
        /// </summary>
        /// <param name="requestID">请求ID</param>
        /// <param name="callBack">回调函数</param>
        public void UnRegisterListener(int requestID, THSDealCallBack callBack)
        {
            lock (m_listeners)
            {
                if (m_listeners.ContainsKey(requestID))
                {
                    m_listeners[requestID].Remove(callBack);
                }
            }
        }
    }
}
