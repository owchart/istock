using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    public class FinancingIndexParam
    {
        private String _name;

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private Dictionary<String, String> _dic;

        public Dictionary<String, String> Dic
        {
            get { return _dic; }
            set { _dic = value; }
        }

        public override String ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("$-rpt");
            sb.Append("\r\n");
            sb.Append("$name=" + this._name);
            if (Dic != null)
            {
                foreach (var item in Dic)
                {
                    sb.Append("\r\n$" + item.Key + "=" + item.Value);
                }
            }
            sb.Append("\r\n");
            return sb.ToString();
        }
    }
}
