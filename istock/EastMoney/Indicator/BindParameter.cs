using System;
using System.Collections.Generic;
using System.Text;

namespace dataquery.indicator
{
    [Serializable]
    public class BindParamter
    {
        private String base_indicator_name;
        private String base_indicator_param;
        private String constParamValue;
        private String constValue;
        private String dataType;
        private String defaultValue;
        private String description;
        private String indicator_name;
        private String indicatorNo;
        private BindParamter mergeBindParamter;
        private String no;
        private ParamterObject paramter_object;
        private String paramterName;
        private String showMergedParam;
        private String showNewParamterName;

        public String BaseIndicatorName
        {
            get
            {
                return this.base_indicator_name;
            }
            set
            {
                this.base_indicator_name = value;
            }
        }

        public String BaseIndicatorParam
        {
            get
            {
                return this.base_indicator_param;
            }
            set
            {
                this.base_indicator_param = value;
            }
        }

        public String ConstParmValue
        {
            get
            {
                return this.constParamValue;
            }
            set
            {
                this.constParamValue = value;
            }
        }

        public String ConstValue
        {
            get
            {
                return this.constValue;
            }
            set
            {
                this.constValue = value;
            }
        }

        public String DataType
        {
            get
            {
                return this.dataType;
            }
            set
            {
                this.dataType = value;
            }
        }

        public String DefaultValue
        {
            get
            {
                return this.defaultValue;
            }
            set
            {
                this.defaultValue = value;
            }
        }

        public String Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        public String IndicatorName
        {
            get
            {
                return this.indicator_name;
            }
            set
            {
                this.indicator_name = value;
            }
        }

        public String IndicatorNo
        {
            get
            {
                return this.indicatorNo;
            }
            set
            {
                this.indicatorNo = value;
            }
        }

        public BindParamter MergeBindParamter
        {
            get
            {
                return this.mergeBindParamter;
            }
            set
            {
                this.mergeBindParamter = value;
            }
        }

        public String No
        {
            get
            {
                return this.no;
            }
            set
            {
                this.no = value;
            }
        }

        public ParamterObject Paramter_object
        {
            get
            {
                return this.paramter_object;
            }
            set
            {
                this.paramter_object = value;
            }
        }

        public String ParamterName
        {
            get
            {
                return this.paramterName;
            }
            set
            {
                this.paramterName = value;
            }
        }

        public String ShowMergedParam
        {
            get
            {
                return this.showMergedParam;
            }
            set
            {
                this.showMergedParam = value;
            }
        }

        public String ShowNewParamterName
        {
            get
            {
                return this.showNewParamterName;
            }
            set
            {
                this.showNewParamterName = value;
            }
        }
    }
}
