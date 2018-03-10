using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    public class IndicatorDataCore
    {
        private static Dictionary<String, List<int>> _CategoryCodeCustomerIndicatorIdDict = new Dictionary<String, List<int>>();
        private static Dictionary<BrowserType, List<NodeData>> _CategoryNodeDict = new Dictionary<BrowserType, List<NodeData>>();
        private static Dictionary<int, IndicatorEntity> _CustomerIndicatorDict = new Dictionary<int, IndicatorEntity>();
        public static Dictionary<String, IndicatorEntity> _IndicatorEntityDict = new Dictionary<String, IndicatorEntity>();

        public static List<NodeData> GetCategoryValue(BrowserType browserType)
        {
            if (_CategoryNodeDict.ContainsKey(browserType))
            {
                NodeData[] collection = _CategoryNodeDict[browserType].ToArray();
                List<NodeData> list = new List<NodeData>();
                list.AddRange(collection);
                return list;
            }
            return null;
        }

        public static List<int> GetIndicatorCustomerIdList(String categoryCode)
        {
            if (_CategoryCodeCustomerIndicatorIdDict.ContainsKey(categoryCode))
            {
                return _CategoryCodeCustomerIndicatorIdDict[categoryCode];
            }
            return new List<int>();
        }

        public static IndicatorEntity GetIndicatorEntityByCategoryCode(String categoryCode)
        {
            return GetIndicatorEntityByCategoryCode(categoryCode, true);
        }

        public static IndicatorEntity GetIndicatorEntityByCategoryCode(String categoryCode, bool isDeep)
        {
            if (!_IndicatorEntityDict.ContainsKey(categoryCode))
            {
                return null;
            }
            if (isDeep)
            {
                return _IndicatorEntityDict[categoryCode].Copy();
            }
            return _IndicatorEntityDict[categoryCode];
        }

        public static IndicatorEntity GetIndicatorEntityByCustomerId(int customerIndicatorId)
        {
            return GetIndicatorEntityByCustomerId(customerIndicatorId, true);
        }

        public static IndicatorEntity GetIndicatorEntityByCustomerId(int customerIndicatorId, bool isDeep)
        {
            if (!_CustomerIndicatorDict.ContainsKey(customerIndicatorId))
            {
                return null;
            }
            if (isDeep)
            {
                return _CustomerIndicatorDict[customerIndicatorId].Copy();
            }
            return _CustomerIndicatorDict[customerIndicatorId];
        }

        public static void SetCategoryValue(BrowserType browserType, List<NodeData> nodeList)
        {
            lock (_CategoryNodeDict)
            {
                _CategoryNodeDict[browserType] = nodeList;
            }
        }

        public static void SetCustomerIndicatorEntity(int customerIndicatorId, IndicatorEntity entity)
        {
            lock (_CustomerIndicatorDict)
            {
                _CustomerIndicatorDict[customerIndicatorId] = entity;
            }
        }

        public static void SetIndicatorCustomerIdList(String categoryCode, List<int> customerIdList)
        {
            _CategoryCodeCustomerIndicatorIdDict[categoryCode] = customerIdList;
        }

        public static void SetIndicatorEntity(String categoryCode, IndicatorEntity entity)
        {
            lock (_IndicatorEntityDict)
            {
                _IndicatorEntityDict[categoryCode] = entity;
            }
        }
    }
}
