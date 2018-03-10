using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    public class BlockIndicatorParams
    {
        private CommonEnumerators.DateCycle _cycle;
        private DateTime _endDate;
        private CommonEnumerators.SortMode _sort;
        private DateTime _startDate;

        public BlockIndicatorParams(DateTime startDate, DateTime endDate)
            : this(startDate, endDate, CommonEnumerators.DateCycle.Day, CommonEnumerators.SortMode.Desc)
        {
        }

        public BlockIndicatorParams(DateTime startDate, DateTime endDate, CommonEnumerators.DateCycle cycle, CommonEnumerators.SortMode sort)
        {
            this._startDate = startDate;
            this._endDate = endDate;
            this._cycle = cycle;
            this._sort = sort;
        }

        public CommonEnumerators.DateCycle Cycle
        {
            get
            {
                return this._cycle;
            }
            set
            {
                this._cycle = value;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return this._endDate;
            }
            set
            {
                this._endDate = value;
            }
        }

        public CommonEnumerators.SortMode Sort
        {
            get
            {
                return this._sort;
            }
            set
            {
                this._sort = value;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return this._startDate;
            }
            set
            {
                this._startDate = value;
            }
        }
    }
}
