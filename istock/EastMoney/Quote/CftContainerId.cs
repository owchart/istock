namespace OwLib
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class CftContainerId
    {
        private List<int> _available;
        private byte _count = 30;
        private static object _lockOjb = new object();
        private List<int> _used;

        public CftContainerId()
        {
            this._available = new List<int>(this._count);
            this._used = new List<int>(this._count);
            for (int i = 0; i < this._count; i++)
            {
                this._available.Add(i + 1);
            }
        }

        public void Clear()
        {
            object obj2;
            Monitor.Enter(obj2 = _lockOjb);
            try
            {
                if (this._available != null)
                {
                    this._available.Clear();
                    for (int i = 0; i < this._count; i++)
                    {
                        this._available.Add(i + 1);
                    }
                }
                if (this._used != null)
                {
                    this._used.Clear();
                }
            }
            catch (Exception exception)
            {
                //LogUtilities.LogMessagePublishInfo(exception.StackTrace);
            }
            finally
            {
                Monitor.Exit(obj2);
            }
        }

        public int CreateContainerId()
        {
            object obj2;
            int item = -1;
            Monitor.Enter(obj2 = _lockOjb);
            try
            {
                if (this._available == null)
                {
                    return item;
                }
                if (this._available.Count > 0)
                {
                    item = this._available[0];
                    this._available.RemoveAt(0);
                    if (this._used != null)
                    {
                        this._used.Add(item);
                    }
                    return item;
                }
                if ((this._used != null) && (this._used.Count > 0))
                {
                    item = this._used[0];
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
            return item;
        }

        public void ReleaseContainerId(int containerId)
        {
            if ((containerId > 0) && (containerId <= this._count))
            {
                object obj2;
                Monitor.Enter(obj2 = _lockOjb);
                try
                {
                    if ((((this._used != null) && this._used.Remove(containerId)) && ((this._available != null) && (this._available.Count < this._count))) && ((containerId > 0) && (containerId <= this._count)))
                    {
                        this._available.Add(containerId);
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
        }
    }
}

