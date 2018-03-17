using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace OwLib
{
    public class ChartAEx : ChartA
    {
        private CTable m_dataSourceHistory;

        public CTable DataSourceHistory
        {
            get { return m_dataSourceHistory; }
            set { m_dataSourceHistory = value; }
        }
    }
}
