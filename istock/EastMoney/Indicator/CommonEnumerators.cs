using System;
using System.Collections.Generic;
using System.Text;
using EmCore.Utils;
using EmCore;

namespace dataquery.indicator
{
    public static class CommonEnumerators
    {
        public static String GetIndicatorRequestTypeCmd(IndicatorRequestType type)
        {
            switch (type)
            {
                case IndicatorRequestType.Fun:
                    return "n1fun";

                case IndicatorRequestType.Quote:
                    return "quote";

                case IndicatorRequestType.Rpt:
                    return "rpt";

                case IndicatorRequestType.Srpt:
                    return "srpt";

                case IndicatorRequestType.Blk:
                    return "blk";
            }
            return "n1fun";
        }

        public static Type GetMappingDataType(DataType datatType)
        {
            return GetMappingDataType(datatType, false);
        }

        public static Type GetMappingDataType(DataType datatType, bool saveDateTime)
        {
            switch (datatType.ToString().ToLower())
            {
                case "bool":
                case "byte":
                case "short":
                case "int":
                case "ushort":
                case "uint":
                    return typeof(int);

                case "char":
                case "String":
                    return typeof(String);

                case "ushortdate":
                case "datetime":
                    return (saveDateTime ? typeof(DateTime) : typeof(String));

                case "decimal":
                case "double":
                    return typeof(double);

                case "long":
                case "ulong":
                    return typeof(long);

                case "float":
                    return typeof(float);
            }
            return null;
        }

        public static String GetOperate(FilterOperate operate)
        {
            String str = String.Empty;
            switch (operate)
            {
                case FilterOperate.Eq:
                    return "=";

                case FilterOperate.NotEq:
                    return "<>";

                case FilterOperate.Gt:
                case FilterOperate.GtIndicator:
                    return ">";

                case FilterOperate.Gte:
                    return ">=";

                case FilterOperate.Lt:
                case FilterOperate.LtIndicator:
                    return "<";

                case FilterOperate.Lte:
                    return "<=";

                case FilterOperate.StartWith:
                case FilterOperate.EndWith:
                case FilterOperate.Contains:
                    return "like";

                case FilterOperate.NotContains:
                    return "not like";

                case FilterOperate.And:
                    return " and ";

                case FilterOperate.Or:
                    return " or ";
            }
            return str;
        }

        public static String GetSignTypeOperate(SignType type)
        {
            switch (type)
            {
                case SignType.GE:
                    return ">=";

                case SignType.GT:
                    return ">";

                case SignType.EQ:
                    return "=";

                case SignType.LT:
                    return "<";

                case SignType.LE:
                    return "<=";

                case SignType.NE:
                    return "!=";
            }
            return "=";
        }

        public enum CalculateType
        {
            Sum,
            Count,
            Average,
            RightAverage
        }

        [Serializable]
        public enum DateCycle
        {
            Day,
            Week,
            Month,
            Year
        }

        public enum FilterOperate
        {
            Eq,
            NotEq,
            Gt,
            Gte,
            Lt,
            Lte,
            StartWith,
            EndWith,
            Contains,
            NotContains,
            GtIndicator,
            LtIndicator,
            And,
            Or
        }

        public enum IndicatorRequestType
        {
            Fun,
            Quote,
            Rpt,
            Srpt,
            Blk
        }

        public enum MouseClickType
        {
            DoubleClick,
            RightClick,
            LeftClick
        }

        public enum SeriesDataSortType
        {
            Date,
            Value,
            Same,
            Cycle
        }

        public enum SortMode
        {
            Asc,
            Desc
        }

        public enum StatisticDataType
        {
            Text,
            Number,
            Date
        }
    }
}
