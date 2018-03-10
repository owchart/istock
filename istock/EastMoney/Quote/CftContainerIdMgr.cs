namespace EmQComm
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public static class CftContainerIdMgr
    {
        private static Dictionary<FuncTypeRealTime, CftContainerId> _idMgr = new Dictionary<FuncTypeRealTime, CftContainerId>(5);
        private static object _objLock = new object();

        static CftContainerIdMgr()
        {
            CftContainerId id = new CftContainerId();
            _idMgr[FuncTypeRealTime.StockTrend] = id;
            CftContainerId id2 = new CftContainerId();
            _idMgr[FuncTypeRealTime.NOrderStockDetailLevel2] = id2;
            CftContainerId id3 = new CftContainerId();
            _idMgr[FuncTypeRealTime.AllOrderStockDetailLevel2] = id3;
            CftContainerId id4 = new CftContainerId();
            _idMgr[FuncTypeRealTime.StockDetailOrderQueue] = id4;
            CftContainerId id5 = new CftContainerId();
            _idMgr[FuncTypeRealTime.TrendCapitalFlow] = id5;
        }

        public static void ClearContainerId()
        {
            object obj2;
            Monitor.Enter(obj2 = _objLock);
            try
            {
                if (_idMgr != null)
                {
                    foreach (KeyValuePair<FuncTypeRealTime, CftContainerId> pair in _idMgr)
                    {
                        if (pair.Value != null)
                        {
                            pair.Value.Clear();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                //LogUtilities.LogMessagePublishException(exception.StackTrace);
            }
            finally
            {
                Monitor.Exit(obj2);
            }
        }

        public static int CreateContainerId(FuncTypeRealTime funcType)
        {
            int num = 1;
            if ((_idMgr != null) && _idMgr.ContainsKey(funcType))
            {
                CftContainerId id = _idMgr[funcType];
                if (id != null)
                {
                    num = id.CreateContainerId();
                }
            }
            return num;
        }

        public static void ReleaseContainerId(FuncTypeRealTime funcType, int containerId)
        {
            if ((_idMgr != null) && _idMgr.ContainsKey(funcType))
            {
                CftContainerId id = _idMgr[funcType];
                if (id != null)
                {
                    id.ReleaseContainerId(containerId);
                }
            }
        }
    }
}

