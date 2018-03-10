namespace EmReportWatch.Entity
{
    using System;

    [Serializable]
    public class ModSpecialtopicCategory : IComparable
    {
        private string _cateGoryCode;
        private string _categoryEnName;
        private string _categoryName;
        private string _descRiption;
        private string _id;
        private string _pCateGoryCode;
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

        public string CategoryEnName
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

        public string CategoryName
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

        public string ID
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

        public string PCateGoryCode
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

