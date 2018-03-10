using System;
using System.Collections.Generic;
using System.Text;

namespace dataquery.indicator
{
    public class DataPacketEventArgs : EventArgs
    {
        private DataPacketBase _DataPacket;
        private bool _success;

        public DataPacketEventArgs(DataPacketBase dataPacket)
            : this(dataPacket, true)
        {
        }

        public DataPacketEventArgs(DataPacketBase dataPacket, bool success)
        {
            this.Success = success;
            this.DataPacket = dataPacket;
        }

        public DataPacketBase DataPacket
        {
            get
            {
                return this._DataPacket;
            }
            set
            {
                this._DataPacket = value;
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
