using System;
using System.Collections.Generic;
using System.Text;

namespace dataquery.indicator
{
    public class NodeData
    {
        private int sortIndex;
        public bool HasLeaf;
        public String Id;
        public String Info;
        public String Info2;
        public bool IsButton;
        public bool IsCatalog;
        public bool IsExpanding;
        public int Mark;
        public String Name;
        public String ParentId;

        public int SortIndex
        {
            get
            {
                return sortIndex;
            }
            set
            {
                sortIndex = value;
            }
        }
    }
}
