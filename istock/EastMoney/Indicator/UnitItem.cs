using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{

    public class UnitItem
    {
        private String code;
        private String text;
        private List<UnitNameValue> units;

        public UnitItem(String code, String text, List<UnitNameValue> units)
        {
            this.code = code;
            this.text = text;
            this.units = units;
        }

        public override String ToString()
        {
            if (this.text != null)
            {
                return this.text.ToString();
            }
            return base.ToString();
        }

        public String Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value;
            }
        }

        public String Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }

        public List<UnitNameValue> Units
        {
            get
            {
                return this.units;
            }
            set
            {
                this.units = value;
            }
        }
    }
}
