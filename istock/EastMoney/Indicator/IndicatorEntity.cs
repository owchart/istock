using System;
using System.Collections.Generic;
using System.Text;
using EmCore.Utils;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using EmCore;

namespace OwLib
{
    [Serializable]
    public class IndicatorEntity
    {
        private String _categoryCode;
        private String _categoryDesc;
        private String _categoryLevel;
        private String _categoryName;
        private CustomIndicatorEntity _customIndicator;
        private String _dataSourceEngName;
        private String _decimalMarker;
        private String _emFunction;
        private String _engName;
        private String _fieldName;
        private long _id;
        private DataType _indDataType;
        private String _indicatorCode;
        private String _indicatorDesc;
        private String _indicatorType;
        private int _isPCode;
        private int _isTemporalSeq;
        private String _isUsable;
        private String _locationInfo;
        private String _no;
        private String _objCode;
        private int _orderNum;
        private String _originalUnitCode;
        private String _paraInfo;
        private String _parameters;
        private String _pCategoryCode;
        private String _productFunc;
        private String _pyName;
        private String _refIndicatorCode;
        private String _secuCategory;
        private String _secuCategoryCode;
        private String _showFormat;
        private String _showUnitCode;
        private String _staticName;
        private String _subject;
        private String _unitCOEfficent;
        private String _unitName;
        private String _unitRelationCode;
        private String _unitRelationDesc;
        private String _unitRelationName;
        private String _unitType;
        private String _unitTypeName;
        private int _version;
        private String _wdFunction;
        private int customerId;
        private String customerIndicatorDesc;
        private String expressName;
        private int filterKey;
        private String indicatorName;
        private bool isInsertIndicator;

        public IndicatorEntity Copy()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
                stream.Seek(0L, SeekOrigin.Begin);
                return (formatter.Deserialize(stream) as IndicatorEntity);
            }
        }

        public void CopyFromDefine(CustomIndicatorEntity defineIndicator)
        {
            this.CustomIndicator = defineIndicator;
            this.CategoryCode = defineIndicator.IndicatorCode;
            this.IndicatorCode = defineIndicator.IndicatorCode;
            this.CategoryName = defineIndicator.IndicatorName;
            this.CategoryDesc = defineIndicator.IndexDescription;
            this.IndDataType = defineIndicator.DataType;
            this.FieldName = Guid.NewGuid().ToString();
            this.Parameters = ((defineIndicator.IndicatorPara == null) || (defineIndicator.IndicatorPara.Count == 0)) ? "" : JSONHelper.SerializeObject(defineIndicator.IndicatorPara);
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

        public String CategoryDesc
        {
            get
            {
                return this._categoryDesc;
            }
            set
            {
                this._categoryDesc = value;
            }
        }

        public String CategoryLevel
        {
            get
            {
                return this._categoryLevel;
            }
            set
            {
                this._categoryLevel = value;
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

        public int CustomerId
        {
            get
            {
                return this.customerId;
            }
            set
            {
                this.customerId = value;
            }
        }

        public String CustomerIndicatorDesc
        {
            get
            {
                return this.customerIndicatorDesc;
            }
            set
            {
                this.customerIndicatorDesc = value;
            }
        }

        public CustomIndicatorEntity CustomIndicator
        {
            get
            {
                return this._customIndicator;
            }
            set
            {
                this._customIndicator = value;
            }
        }

        public String DataSourceEngName
        {
            get
            {
                return this._dataSourceEngName;
            }
            set
            {
                this._dataSourceEngName = value;
            }
        }

        public String DecimalMarker
        {
            get
            {
                return this._decimalMarker;
            }
            set
            {
                this._decimalMarker = value;
            }
        }

        public String EMFunction
        {
            get
            {
                return this._emFunction;
            }
            set
            {
                this._emFunction = value;
            }
        }

        public String EngName
        {
            get
            {
                return this._engName;
            }
            set
            {
                this._engName = value;
            }
        }

        public String ExpressName
        {
            get
            {
                return this.expressName;
            }
            set
            {
                this.expressName = value;
            }
        }

        public String FieldName
        {
            get
            {
                return this._fieldName;
            }
            set
            {
                this._fieldName = value;
            }
        }

        public int FilterKey
        {
            get
            {
                return this.filterKey;
            }
            set
            {
                this.filterKey = value;
            }
        }

        public long ID
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

        public DataType IndDataType
        {
            get
            {
                return this._indDataType;
            }
            set
            {
                this._indDataType = value;
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

        public String IndicatorDesc
        {
            get
            {
                return this._indicatorDesc;
            }
            set
            {
                this._indicatorDesc = value;
            }
        }

        public String IndicatorName
        {
            get
            {
                return this.indicatorName;
            }
            set
            {
                this.indicatorName = value;
            }
        }

        public String IndicatorType
        {
            get
            {
                return this._indicatorType;
            }
            set
            {
                this._indicatorType = value;
            }
        }

        public bool IsInsertIndicator
        {
            get
            {
                return this.isInsertIndicator;
            }
            set
            {
                this.isInsertIndicator = value;
            }
        }

        public int IsPCode
        {
            get
            {
                return this._isPCode;
            }
            set
            {
                this._isPCode = value;
            }
        }

        public int IsTemporalSeq
        {
            get
            {
                return this._isTemporalSeq;
            }
            set
            {
                this._isTemporalSeq = value;
            }
        }

        public String IsUsable
        {
            get
            {
                return this._isUsable;
            }
            set
            {
                this._isUsable = value;
            }
        }

        public String LocationInfo
        {
            get
            {
                return this._locationInfo;
            }
            set
            {
                this._locationInfo = value;
            }
        }

        public String NO
        {
            get
            {
                return this._no;
            }
            set
            {
                this._no = value;
            }
        }

        public String ObjCode
        {
            get
            {
                return this._objCode;
            }
            set
            {
                this._objCode = value;
            }
        }

        public int OrderNum
        {
            get
            {
                return this._orderNum;
            }
            set
            {
                this._orderNum = value;
            }
        }

        public String OriginalUnitCode
        {
            get
            {
                return this._originalUnitCode;
            }
            set
            {
                this._originalUnitCode = value;
            }
        }

        public String ParaInfo
        {
            get
            {
                return this._paraInfo;
            }
            set
            {
                this._paraInfo = value;
            }
        }

        public String Parameters
        {
            get
            {
                return this._parameters;
            }
            set
            {
                this._parameters = value;
            }
        }

        public String PCategoryCode
        {
            get
            {
                return this._pCategoryCode;
            }
            set
            {
                this._pCategoryCode = value;
            }
        }

        public String ProductFunc
        {
            get
            {
                return this._productFunc;
            }
            set
            {
                this._productFunc = value;
            }
        }

        public String PYName
        {
            get
            {
                return this._pyName;
            }
            set
            {
                this._pyName = value;
            }
        }

        public String RefIndicatorCode
        {
            get
            {
                return this._refIndicatorCode;
            }
            set
            {
                this._refIndicatorCode = value;
            }
        }

        public String SecuCategory
        {
            get
            {
                return this._secuCategory;
            }
            set
            {
                this._secuCategory = value;
            }
        }

        public String SecuCategoryCode
        {
            get
            {
                return this._secuCategoryCode;
            }
            set
            {
                this._secuCategoryCode = value;
            }
        }

        public String ShowFormat
        {
            get
            {
                return this._showFormat;
            }
            set
            {
                this._showFormat = value;
            }
        }

        public String ShowUnitCode
        {
            get
            {
                return this._showUnitCode;
            }
            set
            {
                this._showUnitCode = value;
            }
        }

        public String StaticName
        {
            get
            {
                return this._staticName;
            }
            set
            {
                this._staticName = value;
            }
        }

        public String Subject
        {
            get
            {
                return this._subject;
            }
            set
            {
                this._subject = value;
            }
        }

        public String UnitCOEfficient
        {
            get
            {
                return this._unitCOEfficent;
            }
            set
            {
                this._unitCOEfficent = value;
            }
        }

        public String UnitName
        {
            get
            {
                return this._unitName;
            }
            set
            {
                this._unitName = value;
            }
        }

        public String UnitRelationCode
        {
            get
            {
                return this._unitRelationCode;
            }
            set
            {
                this._unitRelationCode = value;
            }
        }

        public String UnitRelationDesc
        {
            get
            {
                return this._unitRelationDesc;
            }
            set
            {
                this._unitRelationDesc = value;
            }
        }

        public String UnitRelationName
        {
            get
            {
                return this._unitRelationName;
            }
            set
            {
                this._unitRelationName = value;
            }
        }

        public String UnitType
        {
            get
            {
                return this._unitType;
            }
            set
            {
                this._unitType = value;
            }
        }

        public String UnitTypeName
        {
            get
            {
                return this._unitTypeName;
            }
            set
            {
                this._unitTypeName = value;
            }
        }

        public int Version
        {
            get
            {
                return this._version;
            }
            set
            {
                this._version = value;
            }
        }

        public String WDFunction
        {
            get
            {
                return this._wdFunction;
            }
            set
            {
                this._wdFunction = value;
            }
        }
    }
}
