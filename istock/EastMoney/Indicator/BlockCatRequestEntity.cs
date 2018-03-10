using System;
using System.Collections.Generic;
using System.Text;

namespace dataquery.indicator
{
    public class BlockCatRequestEntity
    {
        private BrowserType _browser;
        private List<String> _categoryList;
        private String _childCategoryCode;

        public BrowserType Browser
        {
            get
            {
                return this._browser;
            }
            set
            {
                this._browser = value;
            }
        }

        public List<String> CategoryList
        {
            get
            {
                return this._categoryList;
            }
            set
            {
                this._categoryList = value;
            }
        }

        public String ChildCategoryCode
        {
            get
            {
                return this._childCategoryCode;
            }
            set
            {
                this._childCategoryCode = value;
            }
        }
    }
}
