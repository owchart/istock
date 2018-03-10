using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    [Serializable]
    public class StockEntity
    {
        private int _categoryCode;
        private int _publishCode;
        private String _stockCode;
        private String _stockName;

        public int CategoryCode
        {
            get
            {
                return this._categoryCode;
            }
            set
            {
                this._categoryCode = value;
            }
        }

        public int PublishCode
        {
            get
            {
                return this._publishCode;
            }
            set
            {
                this._publishCode = value;
            }
        }

        public String StockCode
        {
            get
            {
                return this._stockCode;
            }
            set
            {
                this._stockCode = value;
            }
        }

        public String StockName
        {
            get
            {
                return this._stockName;
            }
            set
            {
                this._stockName = value;
            }
        }
    }
}
