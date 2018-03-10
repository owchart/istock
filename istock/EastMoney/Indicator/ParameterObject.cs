using System;
using System.Collections.Generic;
using System.Text;

namespace dataquery.indicator
{
    [Serializable]
    public class ReferenceParamInfo
    {
        private IndicatorEntity indicator;
        private int outID;
        private String text;

        public IndicatorEntity Indicator
        {
            get
            {
                return indicator;
            }
            set
            {
                indicator = value;
            }
        }

        public int OutId
        {
            get
            {
                return outID;
            }
            set
            {
                outID = value;
            }
        }

        public String Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }
    }

    [Serializable]
    public class ParamterObject
    {
        private object _defaultValue;
        private String _description;
        private String _indicatorNo;
        private bool _isHide;
        private String _name;
        private String _paraType;
        private SignType _sign;
        private String _type;
        private bool isMerge;
        private bool isSetConst;
        private ReferenceParamInfo refreParamInfo;
        private String showMergeParamDescription;
        private String showNewDescription;

        public object DefaultValue
        {
            get
            {
                return this._defaultValue;
            }
            set
            {
                this._defaultValue = value;
            }
        }

        public String Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        public String IndicatorNo
        {
            get
            {
                return this._indicatorNo;
            }
            set
            {
                this._indicatorNo = value;
            }
        }

        public bool IsHide
        {
            get
            {
                return this._isHide;
            }
            set
            {
                this._isHide = value;
            }
        }

        public bool IsMerge
        {
            get
            {
                return this.isMerge;
            }
            set
            {
                this.isMerge = value;
            }
        }

        public bool IsSetConst
        {
            get
            {
                return this.isSetConst;
            }
            set
            {
                this.isSetConst = value;
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

        public String ParaType
        {
            get
            {
                return this._paraType;
            }
            set
            {
                this._paraType = value;
            }
        }

        public ReferenceParamInfo ReferenceParamInfo
        {
            get
            {
                return this.refreParamInfo;
            }
            set
            {
                this.refreParamInfo = value;
                if (value != null)
                {
                    this._defaultValue = value.Text;
                }
            }
        }

        public String ShowMergeParmDescription
        {
            get
            {
                return this.showMergeParamDescription;
            }
            set
            {
                this.showMergeParamDescription = value;
            }
        }

        public String ShowNewDescription
        {
            get
            {
                return this.showNewDescription;
            }
            set
            {
                this.showNewDescription = value;
            }
        }

        public SignType Sign
        {
            get
            {
                return this._sign;
            }
            set
            {
                this._sign = value;
            }
        }

        public String Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        public enum SignType
        {
            GE,
            GT,
            EQ,
            LT,
            LE,
            NE
        }
    }
}
