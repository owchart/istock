using System;

namespace OwLib
{
    [Serializable]
    public class ModSpecialtopicCategory : IComparable
    {
        private String _cateGoryCode;
        private String _categoryEnName;
        private String _categoryName;
        private String _descRiption;
        private String _id;
        private String _pCateGoryCode;
        private int _sortCode;
        private double _updateID;

        public int CompareTo(object obj)
        {
            ModSpecialtopicCategory category = obj as ModSpecialtopicCategory;
            if (this.SortCode < category.SortCode)
            {
                return -1;
            }
            if (this.SortCode == category.SortCode)
            {
                return 0;
            }
            return 1;
        }

        public String CateGoryCode
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

        public String CategoryEnName
        {
            get
            {
                return this._categoryEnName;
            }
            set
            {
                this._categoryEnName = value;
            }
        }

        public String CategoryName
        {
            get
            {
                return this._categoryName;
            }
            set
            {
                this._categoryName = value;
            }
        }

        public String DescRiption
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

        public String ID
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        public String PCateGoryCode
        {
            get
            {
                return this._pCateGoryCode;
            }
            set
            {
                this._pCateGoryCode = value;
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
    }
}

