using System;
using System.Collections.Generic;
using System.Text;

namespace dataquery.indicator
{
    [Serializable]
    public class CustomIndicatorEntity
    {
        private String _categoryCode;
        private EmCore.Utils.DataType _dataType;
        private String _defaultValue;
        private String _indexDescription;
        private int _indexType;
        private String _indicatorCode;
        private List<IndicatorEntity> _indicatorList;
        private String _indicatorName;
        private List<ParamterObject> _indicatorPara;
        private String _simpleExp;
        private String _simpleExpOutId;
        private String calculateIndicatorExp;
        private List<String> dependIndicatorNoList;
        private bool isCalculateIndicator;
        private bool isNew;
        private List<BindParamter> meger_ParamterList;
        private String paramExp;
        private String paramExpForCalculate;
        private String paramNameExp;
        private String paramNameExpForCalculate;

        public String CalculateIndicatorExp
        {
            get
            {
                return this.calculateIndicatorExp;
            }
            set
            {
                this.calculateIndicatorExp = value;
            }
        }

        public String CategoryCode
        {
            get
            {
                return this._categoryCode;
            }
            set
            {
                this._categoryCode = value;
            }
        }

        public EmCore.Utils.DataType DataType
        {
            get
            {
                return this._dataType;
            }
            set
            {
                this._dataType = value;
            }
        }

        public String DefaultValue
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

        public List<String> DependIndicatorNoList
        {
            get
            {
                return this.dependIndicatorNoList;
            }
            set
            {
                this.dependIndicatorNoList = value;
            }
        }

        public String IndexDescription
        {
            get
            {
                return this._indexDescription;
            }
            set
            {
                this._indexDescription = value;
            }
        }

        public int IndexType
        {
            get
            {
                return this._indexType;
            }
            set
            {
                this._indexType = value;
            }
        }

        public String IndicatorCode
        {
            get
            {
                return this._indicatorCode;
            }
            set
            {
                this._indicatorCode = value;
            }
        }

        public List<IndicatorEntity> IndicatorList
        {
            get
            {
                return this._indicatorList;
            }
            set
            {
                this._indicatorList = value;
            }
        }

        public String IndicatorName
        {
            get
            {
                return this._indicatorName;
            }
            set
            {
                this._indicatorName = value;
            }
        }

        public List<String> IndicatorNoList
        {
            get
            {
                List<String> list = new List<String>();
                if (this.IndicatorList != null)
                {
                    foreach (IndicatorEntity entity in this.IndicatorList)
                    {
                        list.Add(entity.NO);
                    }
                }
                return list;
            }
        }

        public List<ParamterObject> IndicatorPara
        {
            get
            {
                return this._indicatorPara;
            }
            set
            {
                this._indicatorPara = value;
            }
        }

        public bool IsCalculateIndicator
        {
            get
            {
                return this.isCalculateIndicator;
            }
            set
            {
                this.isCalculateIndicator = value;
            }
        }

        public bool IsNew
        {
            get
            {
                return this.isNew;
            }
            set
            {
                this.isNew = value;
            }
        }

        public List<BindParamter> Meger_ParamterList
        {
            get
            {
                return this.meger_ParamterList;
            }
            set
            {
                this.meger_ParamterList = value;
            }
        }

        public String ParamExp
        {
            get
            {
                return this.paramExp;
            }
            set
            {
                this.paramExp = value;
            }
        }

        public String ParamExpForCalculate
        {
            get
            {
                return this.paramExpForCalculate;
            }
            set
            {
                this.paramExpForCalculate = value;
            }
        }

        public String ParamNameExp
        {
            get
            {
                return this.paramNameExp;
            }
            set
            {
                this.paramNameExp = value;
            }
        }

        public String ParamNameExpForCalculate
        {
            get
            {
                return this.paramNameExpForCalculate;
            }
            set
            {
                this.paramNameExpForCalculate = value;
            }
        }

        public String SimpleExp
        {
            get
            {
                return this._simpleExp;
            }
            set
            {
                this._simpleExp = value;
            }
        }

        public String SimpleExpOutId
        {
            get
            {
                return this._simpleExpOutId;
            }
            set
            {
                this._simpleExpOutId = value;
            }
        }
    }
}
