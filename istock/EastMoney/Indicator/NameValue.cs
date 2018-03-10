using System;
using System.Collections.Generic;
using System.Text;
using EmCore;

namespace dataquery.indicator
{
    [Serializable]
    public class NameValue
    {
        private String name;
        private List<NameValue> nameValues;
        private bool selected;
        private String value;

        public NameValue()
        {
        }

        public NameValue(String name, String value)
        {
            this.Name = name;
            this.Value = value;
        }

        public override String ToString()
        {
            return JSONHelper.SerializeObject(this);
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

        public List<NameValue> NameValues
        {
            get
            {
                return this.nameValues;
            }
            set
            {
                this.nameValues = value;
            }
        }

        public bool Selected
        {
            get
            {
                return this.selected;
            }
            set
            {
                this.selected = value;
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
