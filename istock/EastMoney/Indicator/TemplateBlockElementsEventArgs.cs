using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace dataquery.indicator
{
    public class TemplateBlockElementsEventArgs : EventArgs
    {
        private uint _chartId;
        private Hashtable _hashBlockAndElements;

        public TemplateBlockElementsEventArgs(uint chartId, Hashtable hashBlockAndElements)
        {
            this._chartId = chartId;
            this._hashBlockAndElements = hashBlockAndElements;
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

        public Hashtable HashBlockAndElements
        {
            get
            {
                return this._hashBlockAndElements;
            }
            set
            {
                this._hashBlockAndElements = value;
            }
        }
    }
}
