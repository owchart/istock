using System;
using System.Collections.Generic;
 
using System.Text;
using EmQComm;

namespace EmQDataCore
{
    /// <summary>
    /// TickDataTable
    /// </summary>
    public class TickDataTable : DataTableBase
    {
        private Dictionary<int, List<OneTickDataRec>> _allTickData;
        /// <summary>
        /// AllDealData
        /// </summary>
        public Dictionary<int, List<OneTickDataRec>> AllDealData { get { return _allTickData; } }
        /// <summary>
        /// 构造函数
        /// </summary>
        public TickDataTable()
        {
            _allTickData = new Dictionary<int, List<OneTickDataRec>>();
        }
        /// <summary>
        /// SetData
        /// </summary>
        /// <param name="dataPacket"></param>
        public override void SetData(DataPacket dataPacket)
        {
            if(dataPacket is ResTickDataPacket)
                SetTickData((ResTickDataPacket)dataPacket);
        }

        private void SetTickData(ResTickDataPacket dataPacket)
        {
            List<OneTickDataRec> memData;
            if(_allTickData.TryGetValue(dataPacket.TickData.Code,out memData))
            {
                int lastIndex = 0;
                for (int i = 0; i < dataPacket.TickData.TickDatasList.Count; i++)
                {
                    lastIndex = FindIndex(memData, dataPacket.TickData.TickDatasList[i].Index, lastIndex);
                    if (lastIndex >= 0)
                    {
                        if (lastIndex == memData.Count - 1)
                        {
                            memData.AddRange(dataPacket.TickData.TickDatasList);
                            return;
                        }
                        else
                        {
                            memData.Insert(lastIndex, dataPacket.TickData.TickDatasList[i]);
                        }
                    }
                }
            }
            else
            {
                _allTickData.Add(dataPacket.TickData.Code,dataPacket.TickData.TickDatasList);
            }
        }

        /// <summary>
        /// 找到indexPacket的插入位子
        /// </summary>
        /// <param name="memDatas"></param>
        /// <param name="indexPacket"></param>
        /// <param name="beginIndex"></param>
        /// <returns>若memDatas里有timePacket相同的项，则返回-1</returns>
        private int FindIndex(List<OneTickDataRec> memDatas, int indexPacket, int beginIndex)
        {
            if (memDatas == null)
                return -1;
            if (memDatas.Count < 1)
                return 0;
            int lowerBound = beginIndex;
            int upperBound = memDatas.Count - 1;
            int curIndex = 0;
            while (true)
            {
                curIndex = (upperBound + lowerBound) / 2;
               
                if (memDatas[curIndex].Index > indexPacket)
                {
                    upperBound = curIndex - 1;
                    if (lowerBound > upperBound)
                        return curIndex;
                }
                else if (memDatas[curIndex].Index < indexPacket)
                {
                    lowerBound = curIndex + 1;
                    if (lowerBound > upperBound)
                        return lowerBound;
                }
                else
                    return -1;
            }
        }

        public override void ClearData(InitOrgStatus status)
        {
            if (status == InitOrgStatus.SHSZ)
            {
                if (_allTickData != null)
                    _allTickData.Clear();
            }

        }
    }
}
