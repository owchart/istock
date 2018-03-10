using System;
using System.Collections.Generic;
 
using System.Text;
using EmQComm;

namespace EmQDataCore
{
    /// <summary>
    /// OrderDetailDataTable
    /// </summary>
    public class OrderDetailDataTable : DataTableBase
    {
        private Dictionary<int, List<OneOrderDetailDataRec>> _allOrderDetailData;
        /// <summary>
        /// AllOrderDetailData
        /// </summary>
        public Dictionary<int, List<OneOrderDetailDataRec>> AllOrderDetailData { get { return _allOrderDetailData; } }
        /// <summary>
        /// 构造函数
        /// </summary>
        public OrderDetailDataTable()
        {
            _allOrderDetailData = new Dictionary<int, List<OneOrderDetailDataRec>>();
        }
        /// <summary>
        /// SetData
        /// </summary>
        /// <param name="dataPacket"></param>
        public override void SetData(DataPacket dataPacket)
        {
            if(dataPacket is ResOrderDetailDataPacket)
                SetTickData((ResOrderDetailDataPacket)dataPacket);
        }

        private void SetTickData(ResOrderDetailDataPacket dataPacket)
        {
            List<OneOrderDetailDataRec> memData;
            if (_allOrderDetailData.TryGetValue(dataPacket.OrderDetailData.Code, out memData))
            {
                int lastIndex = 0;
                for (int i = 0; i < dataPacket.OrderDetailData.OrderDetailList.Count; i++)
                {
                    lastIndex = FindIndex(memData, dataPacket.OrderDetailData.OrderDetailList[i].Index, lastIndex);
                    if (lastIndex >= 0)
                    {
                        if (lastIndex == memData.Count - 1)
                        {
                            memData.AddRange(dataPacket.OrderDetailData.OrderDetailList);
                            return;
                        }
                        else
                        {
                            memData.Insert(lastIndex, dataPacket.OrderDetailData.OrderDetailList[i]);
                        }
                    }
                }
            }
            else
            {
                _allOrderDetailData.Add(dataPacket.OrderDetailData.Code, dataPacket.OrderDetailData.OrderDetailList);
            }
        }

        /// <summary>
        /// 找到indexPacket的插入位子
        /// </summary>
        /// <param name="memDatas"></param>
        /// <param name="indexPacket"></param>
        /// <param name="beginIndex"></param>
        /// <returns>若memDatas里有timePacket相同的项，则返回-1</returns>
        private int FindIndex(List<OneOrderDetailDataRec> memDatas, int indexPacket, int beginIndex)
        {
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
            MarketType mt = MarketType.NA;
            switch (status)
            {
                case InitOrgStatus.SHSZ:
                    if (_allOrderDetailData != null)
                        _allOrderDetailData.Clear();
                    break;
            }
                
        }
    }
}
