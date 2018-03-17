using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    /// <summary>
    /// 1.美股分红派息 testCmd = @"$-rpt\r\n$name=USDivdendReport\r\n$secucode=COG.N\r\n$\r\n";
    /// 2.美股内部人交易 testCmd = @"$-rpt\r\n$name=USInsiderTradeReport\r\n$secucode=VSR.A\r\n$StartDate=2013-01-01,EndDate=2013-09-10\r\n";
    /// 3.港股股票回购     testCmd = @"$-rpt\r\n$name=HKPurchaseReport\r\n$secucode=00001.HK\r\n$StartDate=2013-01-01,EndDate=2013-09-10\r\n";
    /// 4.港股股票拆细合并    testCmd = @"$-rpt\r\n$name=HKShareSpiltReport\r\n$secucode=00001.HK\r\n$StartDate=2013-01-01,EndDate=2013-09-10\r\n";
    /// </summary>
    public class StockIndicateParam
    {
        private String _name;

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private String _secucode;

        public String Secucode
        {
            get { return _secucode; }
            set { _secucode = value; }
        }

        private Dictionary<String, String> _dic;
        /// <summary>
        /// 指标函数 参数
        /// $StartDate=2013-01-01,EndDate=2013-09-10\r\n
        /// 其中StartDate=2013-01-01,EndDate=2013-09-10为参数
        /// </summary>
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
            sb.Append("\r\n");
            if (!String.IsNullOrEmpty(this._secucode))
            {
                sb.Append("$secucode=" + this._secucode);
            }
            else
            {
                sb.Append("$");
            }
            sb.Append("\r\n$");
            if (Dic != null)
            {
                int i = 0;
                foreach (var item in Dic)
                {
                    i++;
                    if (i > 1)
                    {
                        sb.Append(",");
                    }
                    sb.Append(item.Key + "=" + item.Value);
                }
            }
            sb.Append("\r\n");
            return sb.ToString();
        }

        public String ToString(String type)
        {
            StringBuilder sb = new StringBuilder();
            switch (type)
            {
                case "$-fun"://国债期货F9
                    sb.Append(type);
                    sb.Append("\r\n");
                    sb.Append("$name=" + this._name);
                    sb.Append("\r\n");
                    if (!String.IsNullOrEmpty(this._secucode))
                    {
                        sb.Append("$secuCode=" + this._secucode);
                    }
                    else
                    {
                        sb.Append("$");
                    }
                    sb.Append("\r\n$");
                    if (Dic != null)
                    {
                        int i = 0;
                        foreach (var item in Dic)
                        {
                            i++;
                            if (i > 1)
                            {
                                sb.Append(",");
                            }
                            sb.Append(item.Key + "=" + item.Value);
                        }
                    }
                    sb.Append("\r\n");
                    break;
                case "$-srpt"://美股报表
                    sb.Append(type);
                    sb.Append("\r\n");
                    sb.Append("$name=" + this._name);
                    sb.Append("\r\n");
                    if (!String.IsNullOrEmpty(this._secucode))
                    {
                        sb.Append("$secuCode=" + this._secucode);
                    }
                    else
                    {
                        sb.Append("$");
                    }
                    sb.Append("\r\n");
                    if (Dic != null)
                    {
                        int i = 0;
                        foreach (var item in Dic)
                        {
                            i++;
                            if (i > 1)
                            {
                                sb.Append("\r\n");
                            }
                            sb.Append("$"+item.Key + "=" + item.Value);
                        }
                    }
                    sb.Append("\r\n");
                    break;
            }
            return sb.ToString();
        }
    }
}
