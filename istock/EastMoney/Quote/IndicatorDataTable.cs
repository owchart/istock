using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    /// <summary>
    /// 行业指标面板需求
    /// </summary>
    public class IndicatorDataTable : DataTableBase
    {
        /// <summary>
        /// Init the cache container.
        /// </summary>
        public IndicatorDataTable()
        {
            AllLeftItems = new Dictionary<String, StockIndicatorLeftItem>();
            AllRightItems = new Dictionary<String, StockIndicatorRightItem>();
            DicLeftIndicatorOfStock = new Dictionary<int, HashSet<String>>();
            DicRightIndicatorOfStock = new Dictionary<int, HashSet<String>>();
        }

        /// <summary>
        ///  All left style macro-indicator items.
        /// (Key is macroid, value is left macro-indicator entity.)
        /// </summary>
        public Dictionary<String, StockIndicatorLeftItem> AllLeftItems;
        /// <summary>
        ///  All right style macro-indicator items.
        /// (Key is macroid, value is right macro-indicator entity.)
        /// </summary>
        public Dictionary<String, StockIndicatorRightItem> AllRightItems;

        /// <summary>
        /// Cache of the left-side macro-indicator Id list belong to the stock.
        /// ( Key is the stock id, value is macro-indicator Id list.)
        /// </summary>
        public Dictionary<int, HashSet<String>> DicLeftIndicatorOfStock;
        /// <summary>
        /// Cache of the right-side macro-indicator Id list belong to the stock.
        /// ( Key is the stock id, value is macro-indicator Id list.)
        /// </summary> 
        public Dictionary<int, HashSet<String>> DicRightIndicatorOfStock;

        /// <summary>
        /// Cache of the macro-indicator value list.
        /// ( Key is the macro-indicator id, value is time-value pair.)
        /// </summary>
        public Dictionary<String, SortedList<int, double>> _dicIndicatorValueReport;

        /// <summary>
        /// Once client receive the response-packet,
        /// process the data for UI demond.
        /// </summary>
        /// <param name="dataPacket"></param>
        public override void SetData(DataPacket dataPacket)
        {
            IndicatorDataPacket iPacket = dataPacket as IndicatorDataPacket;
            switch (iPacket.RequestId)
            {
                case IndicateRequestType.LeftIndicatorsReport:
                    ResIndicatorsReportDataPacket indicatorsData = dataPacket as ResIndicatorsReportDataPacket;
                    FillCacheWithIndicatorsData(indicatorsData);
                    break;
                case IndicateRequestType.RightIndicatorsReport:
                    ResIndicatorValuesDataPacket indicatorValuesData = dataPacket as ResIndicatorValuesDataPacket;
                    FillCacheWithIndicatorValuesData(indicatorValuesData);
                    break;

                case IndicateRequestType.IndicatorValuesReport:
                    break;

            }
        }

        private void FillCacheWithIndicatorValuesData(ResIndicatorValuesDataPacket packet)
        {


        }

        private void FillCacheWithIndicatorsData(ResIndicatorsReportDataPacket packet)
        {
           
        }
    }
}
