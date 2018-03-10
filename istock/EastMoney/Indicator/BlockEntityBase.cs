using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace OwLib
{
     public class BlockEntityBase : IComparable<BlockEntityBase>
    {
        private String _blockCode;
        private List<BlockEntityBase> _childBlockList;
        private object _data;
        private String _kid;
        private String _kPid;
        private int _level;
        private String _pBlockCode;
        private CommonEnumerators.SortMode _sortMode;
        private String _text;
        private bool isCatalog;

        public int CompareTo(BlockEntityBase other)
        {
            try
            {
                object data = this.Data;
                object b = other.Data;
                if (this.Mode == CommonEnumerators.SortMode.Desc)
                {
                    data = other.Data;
                    b = this.Data;
                }
                if (other.Level == this.Level)
                {
                    int num = Comparer.Default.Compare(data, b);
                    if (num != 0)
                    {
                        return num;
                    }
                    String blockCode = this.BlockCode;
                    String strB = other.BlockCode;
                    if (this.Mode == CommonEnumerators.SortMode.Desc)
                    {
                        blockCode = other.BlockCode;
                        strB = this.BlockCode;
                    }
                    return String.Compare(blockCode, strB);
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public override String ToString()
        {
            return String.Format("Level={0}, BlockCode={1}, PBlockCode={2}, Data={3}", new object[] { this.Level, this.BlockCode, this.PBlockCode, this.Data });
        }

        public String BlockCode
        {
            get
            {
                return this._blockCode;
            }
            set
            {
                this._blockCode = value;
            }
        }

        public List<BlockEntityBase> ChildBlockList
        {
            get
            {
                return this._childBlockList;
            }
            set
            {
                this._childBlockList = value;
            }
        }

        public object Data
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

        public bool HasChild
        {
            get
            {
                return ((this._childBlockList != null) && (this._childBlockList.Count > 0));
            }
        }

        public bool IsCatalog
        {
            get
            {
                return isCatalog;
            }
            set
            {
                isCatalog = value;
            }
        }

        public String Kid
        {
            get
            {
                return this._kid;
            }
            set
            {
                this._kid = value;
            }
        }

        public String KPid
        {
            get
            {
                return this._kPid;
            }
            set
            {
                this._kPid = value;
            }
        }

        public int Level
        {
            get
            {
                return this._level;
            }
            set
            {
                this._level = value;
            }
        }

        public CommonEnumerators.SortMode Mode
        {
            get
            {
                return this._sortMode;
            }
            set
            {
                this._sortMode = value;
            }
        }

        public String PBlockCode
        {
            get
            {
                return this._pBlockCode;
            }
            set
            {
                this._pBlockCode = value;
            }
        }

        public String Text
        {
            get
            {
                return this._text;
            }
            set
            {
                this._text = value;
            }
        }
    }
}
