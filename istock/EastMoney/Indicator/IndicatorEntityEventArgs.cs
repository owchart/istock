using System;
using System.Collections.Generic;
using System.Text;

namespace dataquery.indicator
{
    public class IndicatorEntityEventArgs : EventArgs
    {
        private uint _chartId;
        private List<IndicatorEntityEx> _indicatorExList;

        public IndicatorEntityEventArgs(uint chartId, List<IndicatorEntityEx> indicatorList)
        {
            this._chartId = chartId;
            this._indicatorExList = indicatorList;
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

        public List<IndicatorEntityEx> IndicatorExList
        {
            get
            {
                return this._indicatorExList;
            }
            set
            {
                this._indicatorExList = value;
            }
        }
    }
}
