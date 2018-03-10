using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    
    //$-edbrpt\r\n$rpt(name=report01|columns=A,B|sdate=2000-1-1|edate=2010-1-1|top=3)\r\n
    //Columns:表示你需要请求的列
    //Sdate:表示获取开始时间。
    //Edate:表示获取结束时间
    //Top:表示取前多少条数据。
    /// <summary>
    /// 宏观行业指标类
    /// </summary>
    public class MacroIndicateParam
    {
        
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string columns;

        public string Columns
        {
            get { return columns; }
            set { columns = value; }
        }
        private DateTime sdate;

        public DateTime Sdate
        {
            get { return sdate; }
            set { sdate = value; }
        }
        private DateTime edate;

        public DateTime Edate
        {
            get { return edate; }
            set { edate = value; }
        }
        private int top;

        public int Top
        {
            get { return top; }
            set { top = value; }
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("name=" + this.name);
            if (!string.IsNullOrEmpty(this.columns))
            {
                sb.Append("|columns=" + this.columns);
            }
            if (this.sdate!=new DateTime())
            {
                sb.Append("|sdate=" + this.sdate.ToShortDateString());
            }
            if (this.edate != new DateTime())
            {
                sb.Append("|edate=" + this.edate.ToShortDateString());
            }
            if (this.top>0)
            {
                sb.Append("|top=" + this.top);
            }
            return sb.ToString();
        }
    }
}
