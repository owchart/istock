using System;
using System.Collections.Generic;
using System.Text;

namespace dataquery.indicator
{
    [Serializable]
    public class FilterEntity
    {
        private String _boolValue;
        private object _leftPart;
        private CommonEnumerators.FilterOperate _operate;
        private object _rightPart;
        private String _topValue;
        private String compareIndicatorInfo;

        public FilterEntity(object leftPart, object rightPart, CommonEnumerators.FilterOperate operate)
        {
            this.LeftPart = leftPart;
            this.RightPart = rightPart;
            this.Operate = operate;
        }

        public static FilterEntity And(params FilterEntity[] filterArray)
        {
            FilterEntity left = null;
            foreach (FilterEntity entity2 in filterArray)
            {
                left = And(left, entity2);
            }
            return left;
        }

        private static FilterEntity And(FilterEntity left, FilterEntity right)
        {
            if (left == null)
            {
                return right;
            }
            if (right == null)
            {
                return left;
            }
            return new FilterEntity(left, right, CommonEnumerators.FilterOperate.And);
        }

        public static String BuildSql(FilterEntity entity)
        {
            if ((entity.LeftPart == null) || (entity.RightPart == null))
            {
                return String.Empty;
            }
            StringBuilder builder = new StringBuilder();
            if ((entity.Operate == CommonEnumerators.FilterOperate.And) || (entity.Operate == CommonEnumerators.FilterOperate.Or))
            {
                builder.Append("(");
                FilterEntity leftPart = entity.LeftPart as FilterEntity;
                if (leftPart != null)
                {
                    builder.Append(BuildSql(leftPart));
                }
                FilterEntity rightPart = entity.RightPart as FilterEntity;
                if (rightPart != null)
                {
                    builder.Append(CommonEnumerators.GetOperate(entity.Operate));
                    builder.Append(BuildSql(rightPart));
                }
                builder.Append(")");
            }
            else
            {
                builder.Append(BuildSql(entity.LeftPart.ToString(), entity.RightPart, entity.Operate));
            }
            return builder.ToString();
        }

        private static String BuildSql(String leftPart, object rightPart, CommonEnumerators.FilterOperate operate)
        {
            String format = " [{0}] {1} {2} ";
            String str2 = CommonEnumerators.GetOperate(operate);
            switch (rightPart.GetType().FullName)
            {
                case "System.String":
                {
                    String str3 = rightPart.ToString().Replace("'", "").Replace("%", "").Trim();
                    if (operate == CommonEnumerators.FilterOperate.StartWith)
                    {
                        return String.Format(format, leftPart, str2, String.Format("'{0}%'", str3));
                    }
                    if (operate == CommonEnumerators.FilterOperate.EndWith)
                    {
                        return String.Format(format, leftPart, str2, String.Format("'%{0}'", str3));
                    }
                    if ((operate == CommonEnumerators.FilterOperate.Contains) || (operate == CommonEnumerators.FilterOperate.NotContains))
                    {
                        return String.Format(format, leftPart, str2, String.Format("'%{0}%'", str3));
                    }
                    if ((operate == CommonEnumerators.FilterOperate.GtIndicator) || (operate == CommonEnumerators.FilterOperate.LtIndicator))
                    {
                        return String.Format(format, leftPart, str2, String.Format("[{0}]", rightPart));
                    }
                    return String.Format(format, leftPart, str2, String.Format("'{0}'", str3));
                }
                case "System.DateTime":
                {
                    DateTime time = Convert.ToDateTime(rightPart);
                    return String.Format(format, leftPart, str2, String.Format("'{0}'", time.ToString("yyyy-MM-dd")));
                }
            }
            return String.Format(format, leftPart, str2, rightPart);
        }

        public static List<FilterEntity> ExtractFilterList(FilterEntity entity)
        {
            List<FilterEntity> list = new List<FilterEntity>();
            if ((entity.LeftPart == null) || (entity.RightPart == null))
            {
                list.Add(entity);
                return list;
            }
            if ((entity.Operate == CommonEnumerators.FilterOperate.And) || (entity.Operate == CommonEnumerators.FilterOperate.Or))
            {
                FilterEntity leftPart = entity.LeftPart as FilterEntity;
                if (leftPart != null)
                {
                    list.AddRange(ExtractFilterList(leftPart));
                }
                FilterEntity rightPart = entity.RightPart as FilterEntity;
                if (rightPart != null)
                {
                    list.AddRange(ExtractFilterList(rightPart));
                }
                return list;
            }
            list.Add(entity);
            return list;
        }

        public static FilterEntity Or(params FilterEntity[] filterArray)
        {
            FilterEntity left = null;
            foreach (FilterEntity entity2 in filterArray)
            {
                left = Or(left, entity2);
            }
            return left;
        }

        private static FilterEntity Or(FilterEntity left, FilterEntity right)
        {
            if (left == null)
            {
                return right;
            }
            if (right == null)
            {
                return left;
            }
            return new FilterEntity(left, right, CommonEnumerators.FilterOperate.Or);
        }

        public String BoolValue
        {
            get
            {
                return this._boolValue;
            }
            set
            {
                this._boolValue = value;
            }
        }

        public String CompareIndicatorInfo
        {
            get
            {
                return compareIndicatorInfo;
            }
            set
            {
                compareIndicatorInfo = value;
            }
        }

        public object LeftPart
        {
            get
            {
                return this._leftPart;
            }
            set
            {
                this._leftPart = value;
            }
        }

        public CommonEnumerators.FilterOperate Operate
        {
            get
            {
                return this._operate;
            }
            set
            {
                this._operate = value;
            }
        }

        public object RightPart
        {
            get
            {
                return this._rightPart;
            }
            set
            {
                this._rightPart = value;
            }
        }

        public String TopValue
        {
            get
            {
                return this._topValue;
            }
            set
            {
                this._topValue = value;
            }
        }
    }
}
