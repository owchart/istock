using System;
using System.Collections.Generic;
using System.Text;

namespace dataquery.indicator
{
    public class EventArgsMgr
    {
        public class CommonEventArgs : EventArgs
        {
            private object _data;
            private Exception _ex;
            private String _id;
            private bool _success;

            public CommonEventArgs()
            {
            }

            public CommonEventArgs(String id)
            {
                this.Id = id;
            }

            public object Data
            {
                get
                {
                    return this._data;
                }
                set
                {
                    this._data = value;
                }
            }

            public Exception Ex
            {
                get
                {
                    return this._ex;
                }
                set
                {
                    this._ex = value;
                }
            }

            public String Id
            {
                get
                {
                    return this._id;
                }
                set
                {
                    this._id = value;
                }
            }

            public bool Success
            {
                get
                {
                    return this._success;
                }
                set
                {
                    this._success = value;
                }
            }
        }
    }
}
