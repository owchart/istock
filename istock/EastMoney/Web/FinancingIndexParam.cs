using System;
using System.Collections.Generic;
using System.Text;

namespace EastMoney.FM.Web.Data.Model
{
    public class FinancingIndexParam
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private Dictionary<string, string> _dic;

        public Dictionary<string, string> Dic
        {
            get { return _dic; }
            set { _dic = value; }
        }

        public override string ToString()
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
