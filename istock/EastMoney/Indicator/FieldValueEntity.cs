using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace OwLib
{
    public class FieldValueEntity<T> : IComparable<FieldValueEntity<T>>
    {
        private T _data;
        private String _fieldCode;

        public FieldValueEntity(T data, String fCode)
        {
            this._data = data;
            this._fieldCode = fCode;
        }

        public int CompareTo(FieldValueEntity<T> other)
        {
            try
            {
                int num = Comparer.Default.Compare(this.Data, other.Data);
                if (num == 0)
                {
                    return String.Compare(this.FieldCode, other.FieldCode);
                }
                return num;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public T Data
        {
            get
            {
                return this._data;
            }
            set
            {
                this._data = value;
            }
        }

        public String FieldCode
        {
            get
            {
                return this._fieldCode;
            }
            set
            {
                this._fieldCode = value;
            }
        }
    }
}
