using System;
using System.Collections.Generic;
using System.Text;

namespace dataquery.indicator
{
    public abstract class Mark
    {
        protected int _priority = 100;

        protected Mark()
        {
        }

        public abstract int Priority();
        public abstract MarkType Type();
        public abstract String Value();
    }
}
