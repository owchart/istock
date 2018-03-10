using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    public class UnitNameValue
    {
        private String name;
        private String value;

        public UnitNameValue(String name, String value)
        {
            this.name = name;
            this.value = value;
        }

        public decimal GetValue()
        {
            if ((this.value == null) || (this.value.Length <= 0))
            {
                return 0M;
            }
            String str = "0123456789.";
            int length = -1;
            int num2 = 0;
            foreach (char ch in this.value)
            {
                if (str.IndexOf(ch) == -1)
                {
                    length = num2;
                    break;
                }
                num2++;
            }
            String s = this.value;
            if (length != -1)
            {
                s = this.value.Substring(0, length);
            }
            String str3 = String.Empty;
            if ((length != -1) && (length < this.value.Length))
            {
                str3 = this.value.Substring(length);
            }
            decimal result = 1M;
            decimal.TryParse(s, out result);
            if ((str3 != null) && (str3.Length > 0))
            {
                String str5 = str3;
                for (int i = 0; i < str5.Length; i++)
                {
                    switch (str5[i])
                    {
                        case 'W':
                            result *= 10000M;
                            break;

                        case 'Y':
                            result *= 100000000M;
                            break;
                    }
                }
            }
            return result;
        }

        public String Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public String Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }
    }
}
