using System;
using System.Collections.Generic;
using System.Text;

namespace dataquery.indicator
{
    public class GroupCategoryInfo
    {
        private String end;
        private bool isCustomer;
        private List<Item> itemList;
        private String start;
        private String step;

        public String End
        {
            get
            {
                return end;
            }
            set
            {
                end = value;
            }
        }

        public bool IsCustomer
        {
            get
            {
                return isCustomer;
            }
            set
            {
                isCustomer = value;
            }
        }

        public List<Item> ItemList
        {
            get
            {
                return itemList;
            }
            set
            {
                itemList = value;
            }
        }

        public String Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value;
            }
        }

        public String Step
        {
            get
            {
                return step;
            }
            set
            {
                step = value;
            }
        }
    }
}
