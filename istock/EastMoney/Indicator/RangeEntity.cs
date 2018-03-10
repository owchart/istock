using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    public class RangeEntity
    {
        private object _end;
        private bool _isOther;
        private object _start;
        public const String OtherText = "所有其他";

        public override String ToString()
        {
            if (this.IsOther)
            {
                return "所有其他";
            }
            return String.Format("{0}~{1}", this.Start, this.End);
        }

        public object End
        {
            get
            {
                return this._end;
            }
            set
            {
                this._end = value;
            }
        }

        public bool IsOther
        {
            get
            {
                return this._isOther;
            }
            set
            {
                this._isOther = value;
            }
        }

        public object Start
        {
            get
            {
                return this._start;
            }
            set
            {
                this._start = value;
            }
        }
    }
}
