using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    /// <summary>
    /// ShortLineStrategyDataTable
    /// </summary>
    public class ShortLineStrategyDataTable : DataTableBase
    {
        private Dictionary<ShortLineType, List<int>> _typeListData;
        private Dictionary<int, OneShortLineDataRec> _allData;
        /// <summary>
        /// 构造函数
        /// </summary>
        public ShortLineStrategyDataTable()
        {
            _typeListData = new Dictionary<ShortLineType, List<int>>();
            _allData = new Dictionary<int, OneShortLineDataRec>();
        }
        /// <summary>
        /// SetData
        /// </summary>
        /// <param name="dataPacket"></param>
        public override void SetData(DataPacket dataPacket)
        {
            if (dataPacket is ResShortLineStragedytDataPacket)
                SetShortLineData((ResShortLineStragedytDataPacket) dataPacket);
        }

        private static int Compare(int a,int b)
        {
            if (a > b)
                return 1;
            if(a < b)
                return -1;
            return 0;
        }

        public static int CompareTime(OneShortLineDataRec a, OneShortLineDataRec b)
        {
            if (a.Time > b.Time)
                return 1;
            if (a.Time < b.Time)
                return -1;
            return 0;
        }
        /// <summary>
        /// GetShortLineData
        /// </summary>
        /// <returns></returns>
        public List<OneShortLineDataRec> GetShortLineData()
        {
            lock (_typeListData)
            {
                try
                {
                    List<OneShortLineDataRec> result = new List<OneShortLineDataRec>();
                    List<int> idList = new List<int>();
                    foreach (KeyValuePair<ShortLineType, List<int>> oneTypeData in _typeListData)
                        idList.AddRange(oneTypeData.Value);
                    //idList.Sort(Compare);
                    foreach (int id in idList)
                        result.Add(_allData[id]);
                    result.Sort(CompareTime);
                    return result;
                }
                catch (Exception e)
                {
                    LogUtilities.LogMessage("GetShortLineData error" + e.Message);
                    return null;
                }
              
            }
            
        }
        /// <summary>
        /// GetShortLineData
        /// </summary>
        /// <param name="shortLine"></param>
        /// <returns></returns>
        public List<OneShortLineDataRec> GetShortLineData(ShortLineType shortLine)
        {
            List<ShortLineType> types = new List<ShortLineType>();
            ShortLineType[] slTypes = (ShortLineType[])Enum.GetValues(typeof(ShortLineType));
            for (int i = 0; i < slTypes.Length; i++)
            {
                ShortLineType tmp = slTypes[i];
                if ((tmp & shortLine) > 0)
                    types.Add(tmp);
            }

            List<OneShortLineDataRec> result = new List<OneShortLineDataRec>();
            List<int> idList = new List<int>();
	        foreach (ShortLineType oneType in types)
	        {
		        if (_typeListData.ContainsKey(oneType))
			        idList.AddRange(_typeListData[oneType]);
	        }
	        //idList.Sort(Compare);
            foreach (int id in idList)
                result.Add(_allData[id]);
            result.Sort(CompareTime);
            return result;
        }

		/// <summary>
		/// 获取键盘精灵数据
		/// </summary>
		/// <param name="shortLine"></param>
		/// <returns></returns>
		public List<OneShortLineDataRec> GetShortLineData(IList<ShortLineType> shortLines)
		{
			List<OneShortLineDataRec> result = new List<OneShortLineDataRec>();
            if (shortLines == null)
                return result;
			List<int> idList = new List<int>();
			foreach (ShortLineType oneType in shortLines)
			{
				if (_typeListData.ContainsKey(oneType))
					idList.AddRange(_typeListData[oneType]);
			}
			//idList.Sort(Compare);
			foreach(int id in idList)
				result.Add(_allData[id]);
            result.Sort(CompareTime);
			return result;
		}

        void SetShortLineData(ResShortLineStragedytDataPacket dataPacket)
        {
            foreach (OneShortLineDataRec oneData in dataPacket.ShortLineData)
            {
                if(!_allData.ContainsKey(oneData.SeriesId))
                {
                    _allData.Add(oneData.SeriesId, oneData);
                    if (!_typeListData.ContainsKey(oneData.SlType))
                    {
                        List<int> idList = new List<int>();
                        idList.Add(oneData.SeriesId);
                        _typeListData.Add(oneData.SlType,idList);
                    }
                    else
                        _typeListData[oneData.SlType].Add(oneData.SeriesId);
                }
            }
        }

        public override void ClearData(InitOrgStatus status)
        {

            switch (status)
            {
                case InitOrgStatus.All:
                case InitOrgStatus.SHSZ:
                    if(_allData != null)
                        _allData.Clear();
                    if(_typeListData != null)
                        _typeListData.Clear();
                    break;
            }
        }
    }
}
