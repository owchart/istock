using System;
using System.Collections.Generic;
using System.Text;

namespace dataquery.indicator
{
    public class SearchIndicatorResultEntity
    {
        private String _categoryCodes;
        private List<NodeData> _nodeList;
        private String _requestId;

        public String CategoryCodes
        {
            get
            {
                return this._categoryCodes;
            }
            set
            {
                this._categoryCodes = value;
            }
        }

        public List<NodeData> NodeList
        {
            get
            {
                return this._nodeList;
            }
            set
            {
                this._nodeList = value;
            }
        }

        public String RequestId
        {
            get
            {
                return this._requestId;
            }
            set
            {
                this._requestId = value;
            }
        }
    }
}
