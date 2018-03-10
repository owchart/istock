using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    public class IndicatorEntityEx
    {
        private int _customerIndicatorId;
        private IndicatorEntity _indicatorEntity;
        private int _indicatorIndex;

        public IndicatorEntity CustomerIndicatorEntity
        {
            get
            {
                return this._indicatorEntity;
            }
            set
            {
                this._indicatorEntity = value;
            }
        }

        public int CustomerIndicatorId
        {
            get
            {
                return this._customerIndicatorId;
            }
            set
            {
                this._customerIndicatorId = value;
            }
        }

        public int IndicatorIndex
        {
            get
            {
                return this._indicatorIndex;
            }
            set
            {
                this._indicatorIndex = value;
            }
        }
    }
}
