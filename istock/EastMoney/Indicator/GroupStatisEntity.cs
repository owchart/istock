using System;
using System.Collections.Generic;
using System.Text;

namespace dataquery.indicator
{

    public class GroupStatisEntity
    {
        private CommonEnumerators.CalculateType _calcType;
        private List<RangeEntity> _groupEntityList;
        private int _groupIndicatorId;
        private List<String> _groupTextList;
        private int _rightIndicatorId;
        private CommonEnumerators.StatisticDataType _statisticsType;
        private GroupCategoryInfo categoryInfo;

        public CommonEnumerators.CalculateType CalcType
        {
            get
            {
                return this._calcType;
            }
            set
            {
                this._calcType = value;
            }
        }

        public GroupCategoryInfo CategoryInfo
        {
            get
            {
                return categoryInfo;
            }
            set
            {
                categoryInfo = value;
            }
        }

        public List<RangeEntity> GroupEntityList
        {
            get
            {
                return this._groupEntityList;
            }
            set
            {
                this._groupEntityList = value;
            }
        }

        public int GroupIndicatorId
        {
            get
            {
                return this._groupIndicatorId;
            }
            set
            {
                this._groupIndicatorId = value;
            }
        }

        public List<String> GroupTextList
        {
            get
            {
                return this._groupTextList;
            }
            set
            {
                this._groupTextList = value;
            }
        }

        public int RightIndicatorId
        {
            get
            {
                return this._rightIndicatorId;
            }
            set
            {
                this._rightIndicatorId = value;
            }
        }

        public CommonEnumerators.StatisticDataType StatisticType
        {
            get
            {
                return this._statisticsType;
            }
            set
            {
                this._statisticsType = value;
            }
        }
    }
}
