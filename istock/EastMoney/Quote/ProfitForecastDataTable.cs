using System;
using System.Collections.Generic;

using System.Text;
using EmQComm;

namespace EmQDataCore
{
    /// <summary>
    /// ProfitForecastDataTable
    /// </summary>
    class ProfitForecastDataTable : DataTableBase
    {
        private Dictionary<int, OneProfitForecastDataRec> _allProfitForecastData;
        private Dictionary<int, OneProfitForecastOrgDataRec> _allProfitForecastOrgData;

        /// <summary>
        /// AllProfitForecastData
        /// </summary>
        public Dictionary<int, OneProfitForecastDataRec> AllProfitForecastData
        {
            get { return _allProfitForecastData; }
            private set { _allProfitForecastData = value; }
        }

        /// <summary>
        /// AllProfitForecastData
        /// </summary>
        public Dictionary<int, OneProfitForecastOrgDataRec> AllProfitForecastOrgData
        {
            get { return _allProfitForecastOrgData; }
            private set { _allProfitForecastOrgData = value; }
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        public ProfitForecastDataTable()
        {
            AllProfitForecastData = new Dictionary<int, OneProfitForecastDataRec>();
            AllProfitForecastOrgData = new Dictionary<int, OneProfitForecastOrgDataRec>();
        }

        /// <summary>
        /// 获得最新价(如果没有用昨收价代替)
        /// </summary>
        /// <param name="code">证券代码</param>
        /// <param name="lastestPrice">最新价或则昨收价</param>
        /// <returns>true，能获得最新价或者昨收价；fasle，不能获取</returns>
        private bool TryGetLastestPrice(int code, out float lastestPrice)
        {
            lastestPrice = Dc.GetFieldDataSingle(code, FieldIndex.Now);
            if (lastestPrice.Equals(0))
                lastestPrice = Dc.GetFieldDataSingle(code, FieldIndex.PreClose);
            if (lastestPrice.Equals(0))
                return false;
            return true;
        }

        /// <summary>
        /// SetData
        /// </summary>
        /// <param name="dataPacket"></param>
        public override void SetData(DataPacket dataPacket)
        {
            if (dataPacket is ResProfitForecastDataPacket)
            {
                AllProfitForecastData = ((ResProfitForecastDataPacket)dataPacket).AllProfitForecast;
            }
            else if (dataPacket is ResProfitForecastOrgDataPacket)
            {
                int code = ((ResProfitForecastOrgDataPacket)dataPacket).ProfitForecastData.Code;
                AllProfitForecastOrgData[code] =
                    ((ResProfitForecastOrgDataPacket)dataPacket).ProfitForecastData;

                float now;
                if (TryGetLastestPrice(code, out now))
                {
                    foreach (ProfitForecast oneProfitForecastOrgDataRec in AllProfitForecastOrgData[code].ProfitForecastData)
                    {
                        if (oneProfitForecastOrgDataRec.BasicEPS != 0)
                            oneProfitForecastOrgDataRec.PredictPe = now / oneProfitForecastOrgDataRec.BasicEPS;
                    }
                }
            }
            else if (dataPacket is ResStockDetailLev2DataPacket)
            {
                int code = ((ResStockDetailLev2DataPacket) dataPacket).Code;

                if (!AllProfitForecastOrgData.ContainsKey(code))
                    return;

                //float now = Dc.GetFieldDataSingle(code, FieldIndex.Now);
                //foreach (ProfitForecast oneProfitForecastOrgDataRec in AllProfitForecastOrgData[code].ProfitForecastData
                //    )
                //{
                //    if (oneProfitForecastOrgDataRec.BasicEPS != 0)
                //        oneProfitForecastOrgDataRec.PredictPe = now/oneProfitForecastOrgDataRec.BasicEPS;
                //}
                
                float now;
                if (TryGetLastestPrice(code, out now))
                {
                    foreach (ProfitForecast oneProfitForecastOrgDataRec in AllProfitForecastOrgData[code].ProfitForecastData
                        )
                    {
                        if (oneProfitForecastOrgDataRec.BasicEPS != 0)
                            oneProfitForecastOrgDataRec.PredictPe = now / oneProfitForecastOrgDataRec.BasicEPS;
                    }
                }
            }
            else if (dataPacket is ResNewProfitForecastDataPacket)
            {
                ResNewProfitForecastDataPacket packet = dataPacket as ResNewProfitForecastDataPacket;
                int code = packet.Code;
                AllProfitForecastOrgData[code] = packet.ProfitForecastDataByOneCode;

                float now;
                if (TryGetLastestPrice(code, out now))
                {
                    foreach (ProfitForecast oneProfitForecastOrgDataRec in AllProfitForecastOrgData[code].ProfitForecastData)
                    {
                        if (oneProfitForecastOrgDataRec.BasicEPS != 0)
                            oneProfitForecastOrgDataRec.PredictPe = now / oneProfitForecastOrgDataRec.BasicEPS;
                    }
                }
            }
        }

        public override void ClearData(InitOrgStatus status)
        {
            if (status == InitOrgStatus.SHSZ)
            {
                if (_allProfitForecastData != null)
                    _allProfitForecastData.Clear();
            }

        }
    }
}
