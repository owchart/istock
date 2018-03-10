using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    public class BlockElementsEventArgs : EventArgs
    {
        private String _blockCode;
        private uint _chartId;
        private String _id;
        private List<StockEntity> _stockList;

        public BlockElementsEventArgs(String id, uint chartId)
            : this(id, chartId, null)
        {
        }

        public BlockElementsEventArgs(String id, uint chartId, String blockCode)
            : this(id, chartId, blockCode, null)
        {
        }

        public BlockElementsEventArgs(String id, uint chartId, String blockCode, List<StockEntity> stockList)
        {
            this._id = id;
            this._chartId = chartId;
            this._stockList = stockList;
            this._blockCode = blockCode;
        }

        public String BlockCode
        {
            get
            {
                return this._blockCode;
            }
            set
            {
                this._blockCode = value;
            }
        }

        public uint ChartId
        {
            get
            {
                return this._chartId;
            }
            set
            {
                this._chartId = value;
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

        public List<StockEntity> StockList
        {
            get
            {
                return this._stockList;
            }
            set
            {
                this._stockList = value;
            }
        }
    }
}
