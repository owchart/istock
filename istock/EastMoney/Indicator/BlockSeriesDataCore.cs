using System;
using System.Collections.Generic;
using System.Text;

namespace dataquery.indicator
{
    public class BlockSeriesDataCore
    {
        private static Dictionary<String, SortedList<int, SortedList<DateTime, double>>> _blockSeriesDict = new Dictionary<String, SortedList<int, SortedList<DateTime, double>>>();
        private static Dictionary<String, SortedList<int, SortedList<DateTime, double>>> _blockSeriesMonthDict = new Dictionary<String, SortedList<int, SortedList<DateTime, double>>>();
        private static Dictionary<String, SortedList<int, SortedList<DateTime, double>>> _blockSeriesWeekDict = new Dictionary<String, SortedList<int, SortedList<DateTime, double>>>();
        private static Dictionary<String, SortedList<int, SortedList<DateTime, double>>> _blockSeriesYearDict = new Dictionary<String, SortedList<int, SortedList<DateTime, double>>>();

        public static double GetValue(String blockCode, int indicatorCustomerId, DateTime dateTime, CommonEnumerators.DateCycle cycle)
        {
            Dictionary<String, SortedList<int, SortedList<DateTime, double>>> dictionary = _blockSeriesDict;
            switch (cycle)
            {
                case CommonEnumerators.DateCycle.Week:
                    dictionary = _blockSeriesWeekDict;
                    break;

                case CommonEnumerators.DateCycle.Month:
                    dictionary = _blockSeriesMonthDict;
                    break;

                case CommonEnumerators.DateCycle.Year:
                    dictionary = _blockSeriesYearDict;
                    break;
            }
            if (!dictionary.ContainsKey(blockCode))
            {
                return 0.0;
            }
            SortedList<int, SortedList<DateTime, double>> list = dictionary[blockCode];
            if (!list.ContainsKey(indicatorCustomerId))
            {
                return 0.0;
            }
            SortedList<DateTime, double> list2 = list[indicatorCustomerId];
            if (!list2.ContainsKey(dateTime))
            {
                return 0.0;
            }
            return list2[dateTime];
        }

        public static void SetValue(String blockCode, int indicatorCustomerId, DateTime dateTime, CommonEnumerators.DateCycle cycle, double v)
        {
            Dictionary<String, SortedList<int, SortedList<DateTime, double>>> dictionary = _blockSeriesDict;
            switch (cycle)
            {
                case CommonEnumerators.DateCycle.Week:
                    dictionary = _blockSeriesWeekDict;
                    break;

                case CommonEnumerators.DateCycle.Month:
                    dictionary = _blockSeriesMonthDict;
                    break;

                case CommonEnumerators.DateCycle.Year:
                    dictionary = _blockSeriesYearDict;
                    break;
            }
            if (!dictionary.ContainsKey(blockCode))
            {
                dictionary[blockCode] = new SortedList<int, SortedList<DateTime, double>>();
            }
            SortedList<int, SortedList<DateTime, double>> list = dictionary[blockCode];
            if (!list.ContainsKey(indicatorCustomerId))
            {
                list[indicatorCustomerId] = new SortedList<DateTime, double>();
            }
            SortedList<DateTime, double> list2 = list[indicatorCustomerId];
            list2[dateTime] = v;
        }
    }
}
