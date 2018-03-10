using System;
using System.Collections.Generic;
using System.Text;

namespace dataquery.indicator
{
    public class IndicatorTableDataCore
    {
        private static Dictionary<String, double> _doubleIndicatorDict = new Dictionary<String, double>();
        private static Dictionary<String, float> _floatIndicatorDict = new Dictionary<String, float>();
        private static Dictionary<String, int> _intIndicatorDict = new Dictionary<String, int>();
        private static Dictionary<String, long> _longIndicatorDict = new Dictionary<String, long>();
        private static Dictionary<String, String> _strIndicatorDict = new Dictionary<String, String>();

        public static Dictionary<String, double> DoubleIndicatorDict
        {
            get
            {
                return _doubleIndicatorDict;
            }
            set
            {
                _doubleIndicatorDict = value;
            }
        }

        public static Dictionary<String, float> FloatIndicatorDict
        {
            get
            {
                return _floatIndicatorDict;
            }
            set
            {
                _floatIndicatorDict = value;
            }
        }

        public static Dictionary<String, int> Int32IndicatorDict
        {
            get
            {
                return _intIndicatorDict;
            }
            set
            {
                _intIndicatorDict = value;
            }
        }

        public static Dictionary<String, long> LongIndicatorDict
        {
            get
            {
                return _longIndicatorDict;
            }
            set
            {
                _longIndicatorDict = value;
            }
        }

        public static Dictionary<String, String> StrIndicatorDict
        {
            get
            {
                return _strIndicatorDict;
            }
            set
            {
                _strIndicatorDict = value;
            }
        }
    }
}
