using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    public class BrokerEntity
    {
        private String _cat;
        private String _code;
        private String _name;

        public String Cat
        {
            get
            {
                return this._cat;
            }
            set
            {
                this._cat = value;
            }
        }

        public String Code
        {
            get
            {
                return this._code;
            }
            set
            {
                this._code = value;
            }
        }

        public String Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }
    }
}
