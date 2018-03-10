namespace OwLib
{
    using System;

    [Serializable]
    public class ModSpecialtopic : IComparable, ICloneable
    {
        private string _cateGoryCode;
        private string _cateGoryName;
        private int _cateGorySortCode;
        private string _configFileName;
        private string _descRiption;
        private string _engName;
        private string _HelpUrl;
        private string _ID;
        private int _isImportant;
        private int _isShowBlock;
        private int _sortCode;
        private string _specialTopicCode;
        private string _specialTopicName;
        private double _updateID;
        private double _updatePerson;
        public int Version;

        public object Clone()
        {
            ModSpecialtopic specialtopic1 = new ModSpecialtopic();
            specialtopic1.CateGoryCode = this.CateGoryCode;
            specialtopic1.CateGoryName = this.CateGoryName;
            specialtopic1.CateGorySortCode = this.CateGorySortCode;
            specialtopic1.ConfigFileName = this.ConfigFileName;
            specialtopic1.DescRiption = this.DescRiption;
            specialtopic1.EngName = this.EngName;
            specialtopic1.ID = this.ID;
            specialtopic1.IsImportant = this.IsImportant;
            specialtopic1.IsShowBlock = this.IsShowBlock;
            specialtopic1.SortCode = this.SortCode;
            specialtopic1.SpecialTopicCode = this.SpecialTopicCode;
            specialtopic1.SpecialTopicName = this.SpecialTopicName;
            specialtopic1.UpdateID = this.UpdateID;
            specialtopic1.UpdatePerson = this.UpdatePerson;
            specialtopic1.Version = this.Version;
            specialtopic1.HlepUrl = this.HlepUrl;
            return specialtopic1;
        }

        public int CompareTo(object obj)
        {
            ModSpecialtopic specialtopic = obj as ModSpecialtopic;
            if (this.SortCode < specialtopic.SortCode)
            {
                return -1;
            }
            if (this.SortCode == specialtopic.SortCode)
            {
                return 0;
            }
            return 1;
        }

        public string CateGoryCode
        {
            get
            {
                return this._cateGoryCode;
            }
            set
            {
                this._cateGoryCode = value;
            }
        }

        public string CateGoryName
        {
            get
            {
                return this._cateGoryName;
            }
            set
            {
                this._cateGoryName = value;
            }
        }

        public int CateGorySortCode
        {
            get
            {
                return this._cateGorySortCode;
            }
            set
            {
                this._cateGorySortCode = value;
            }
        }

        public string ConfigFileName
        {
            get
            {
                return this._configFileName;
            }
            set
            {
                this._configFileName = value;
            }
        }

        public string DescRiption
        {
            get
            {
                return this._descRiption;
            }
            set
            {
                this._descRiption = value;
            }
        }

        public string EngName
        {
            get
            {
                return this._engName;
            }
            set
            {
                this._engName = value;
            }
        }

        public string HlepUrl
        {
            get
            {
                return this._HelpUrl;
            }
            set
            {
                this._HelpUrl = value;
            }
        }

        public string ID
        {
            get
            {
                return this._ID;
            }
            set
            {
                this._ID = value;
            }
        }

        public int IsImportant
        {
            get
            {
                return this._isImportant;
            }
            set
            {
                this._isImportant = value;
            }
        }

        public int IsShowBlock
        {
            get
            {
                return this._isShowBlock;
            }
            set
            {
                this._isShowBlock = value;
            }
        }

        public int SortCode
        {
            get
            {
                return this._sortCode;
            }
            set
            {
                this._sortCode = value;
            }
        }

        public string SpecialTopicCode
        {
            get
            {
                return this._specialTopicCode;
            }
            set
            {
                this._specialTopicCode = value;
            }
        }

        public string SpecialTopicName
        {
            get
            {
                return this._specialTopicName;
            }
            set
            {
                this._specialTopicName = value;
            }
        }

        public double UpdateID
        {
            get
            {
                return this._updateID;
            }
            set
            {
                this._updateID = value;
            }
        }

        public double UpdatePerson
        {
            get
            {
                return this._updatePerson;
            }
            set
            {
                this._updatePerson = value;
            }
        }
    }
}

