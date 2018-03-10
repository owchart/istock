using System;
using System.Collections.Generic;
using System.Text;
using EmSerDataService;
using System.Threading;
using System.Data;
using System.Drawing;
using EmCore.Utils;
using System.Net;
using EmCore;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace OwLib
{
    public class DataCore
    {
        private IDictionary<String, BlockElementsEventArgs> _blockElementsChartIdDict = new Dictionary<String, BlockElementsEventArgs>();
        private IDictionary<String, Action<List<NodeData>>> _blockTreeCallBackDict = new Dictionary<String, Action<List<NodeData>>>();
        private IDictionary<String, BlockCatRequestEntity> _blockTreeCategoryDict = new Dictionary<String, BlockCatRequestEntity>();
        private static int _counter = 0;
        private static DataCore _dataCore;
        private DataQuery _dataQuery;
        private static bool _isCreating = false;
        private static readonly object _treeLockObj = new object();
        private Dictionary<String, object> _waitHandlerDataDict = new Dictionary<String, object>();
        private EventHandler<BlockElementsEventArgs> blockElementsDataReceived;
        private DelegateMgr2.BlockTreeDataChangedHandler blockTreeDataChanged;
        private EventHandler<IndicatorEntityEventArgs> customerIndicatorEntityGenerated;
        private EventHandler<BlockElementsEventArgs> filterBlockElementsCompleted;
        private Action<FileUploadEventArgs> pictureUploadCompleted;
        private Action<EventArgsMgr.CommonEventArgs> queryBrokerReceived;
        private Action<SearchIndicatorResultEntity> searchIndicatorDataReceived;
        private Action<TemplateBlockElementsEventArgs> temlpateBlockElementsReceived;

        public event EventHandler<BlockElementsEventArgs> BlockElementsDataReceived
        {
            add
            {
                EventHandler<BlockElementsEventArgs> handler2;
                EventHandler<BlockElementsEventArgs> blockElementsDataReceived = this.blockElementsDataReceived;
                do
                {
                    handler2 = blockElementsDataReceived;
                    EventHandler<BlockElementsEventArgs> handler3 = (EventHandler<BlockElementsEventArgs>) Delegate.Combine(handler2, value);
                    blockElementsDataReceived = Interlocked.CompareExchange<EventHandler<BlockElementsEventArgs>>(ref this.blockElementsDataReceived, handler3, handler2);
                }
                while (blockElementsDataReceived != handler2);
            }
            remove
            {
                EventHandler<BlockElementsEventArgs> handler2;
                EventHandler<BlockElementsEventArgs> blockElementsDataReceived = this.blockElementsDataReceived;
                do
                {
                    handler2 = blockElementsDataReceived;
                    EventHandler<BlockElementsEventArgs> handler3 = (EventHandler<BlockElementsEventArgs>) Delegate.Remove(handler2, value);
                    blockElementsDataReceived = Interlocked.CompareExchange<EventHandler<BlockElementsEventArgs>>(ref this.blockElementsDataReceived, handler3, handler2);
                }
                while (blockElementsDataReceived != handler2);
            }
        }

        public event DelegateMgr2.BlockTreeDataChangedHandler BlockTreeDataChanged
        {
            add
            {
                DelegateMgr2.BlockTreeDataChangedHandler handler2;
                DelegateMgr2.BlockTreeDataChangedHandler blockTreeDataChanged = this.blockTreeDataChanged;
                do
                {
                    handler2 = blockTreeDataChanged;
                    DelegateMgr2.BlockTreeDataChangedHandler handler3 = (DelegateMgr2.BlockTreeDataChangedHandler) Delegate.Combine(handler2, value);
                    blockTreeDataChanged = Interlocked.CompareExchange<DelegateMgr2.BlockTreeDataChangedHandler>(ref this.blockTreeDataChanged, handler3, handler2);
                }
                while (blockTreeDataChanged != handler2);
            }
            remove
            {
                DelegateMgr2.BlockTreeDataChangedHandler handler2;
                DelegateMgr2.BlockTreeDataChangedHandler blockTreeDataChanged = this.blockTreeDataChanged;
                do
                {
                    handler2 = blockTreeDataChanged;
                    DelegateMgr2.BlockTreeDataChangedHandler handler3 = (DelegateMgr2.BlockTreeDataChangedHandler) Delegate.Remove(handler2, value);
                    blockTreeDataChanged = Interlocked.CompareExchange<DelegateMgr2.BlockTreeDataChangedHandler>(ref this.blockTreeDataChanged, handler3, handler2);
                }
                while (blockTreeDataChanged != handler2);
            }
        }

        public event EventHandler<IndicatorEntityEventArgs> CustomerIndicatorEntityGenerated
        {
            add
            {
                EventHandler<IndicatorEntityEventArgs> handler2;
                EventHandler<IndicatorEntityEventArgs> customerIndicatorEntityGenerated = this.customerIndicatorEntityGenerated;
                do
                {
                    handler2 = customerIndicatorEntityGenerated;
                    EventHandler<IndicatorEntityEventArgs> handler3 = (EventHandler<IndicatorEntityEventArgs>) Delegate.Combine(handler2, value);
                    customerIndicatorEntityGenerated = Interlocked.CompareExchange<EventHandler<IndicatorEntityEventArgs>>(ref this.customerIndicatorEntityGenerated, handler3, handler2);
                }
                while (customerIndicatorEntityGenerated != handler2);
            }
            remove
            {
                EventHandler<IndicatorEntityEventArgs> handler2;
                EventHandler<IndicatorEntityEventArgs> customerIndicatorEntityGenerated = this.customerIndicatorEntityGenerated;
                do
                {
                    handler2 = customerIndicatorEntityGenerated;
                    EventHandler<IndicatorEntityEventArgs> handler3 = (EventHandler<IndicatorEntityEventArgs>) Delegate.Remove(handler2, value);
                    customerIndicatorEntityGenerated = Interlocked.CompareExchange<EventHandler<IndicatorEntityEventArgs>>(ref this.customerIndicatorEntityGenerated, handler3, handler2);
                }
                while (customerIndicatorEntityGenerated != handler2);
            }
        }

        public event EventHandler<BlockElementsEventArgs> FilterBlockElementsCompleted
        {
            add
            {
                EventHandler<BlockElementsEventArgs> handler2;
                EventHandler<BlockElementsEventArgs> filterBlockElementsCompleted = this.filterBlockElementsCompleted;
                do
                {
                    handler2 = filterBlockElementsCompleted;
                    EventHandler<BlockElementsEventArgs> handler3 = (EventHandler<BlockElementsEventArgs>) Delegate.Combine(handler2, value);
                    filterBlockElementsCompleted = Interlocked.CompareExchange<EventHandler<BlockElementsEventArgs>>(ref this.filterBlockElementsCompleted, handler3, handler2);
                }
                while (filterBlockElementsCompleted != handler2);
            }
            remove
            {
                EventHandler<BlockElementsEventArgs> handler2;
                EventHandler<BlockElementsEventArgs> filterBlockElementsCompleted = this.filterBlockElementsCompleted;
                do
                {
                    handler2 = filterBlockElementsCompleted;
                    EventHandler<BlockElementsEventArgs> handler3 = (EventHandler<BlockElementsEventArgs>) Delegate.Remove(handler2, value);
                    filterBlockElementsCompleted = Interlocked.CompareExchange<EventHandler<BlockElementsEventArgs>>(ref this.filterBlockElementsCompleted, handler3, handler2);
                }
                while (filterBlockElementsCompleted != handler2);
            }
        }

        public event Action<FileUploadEventArgs> PictureUploadCompleted
        {
            add
            {
                Action<FileUploadEventArgs> action2;
                Action<FileUploadEventArgs> pictureUploadCompleted = this.pictureUploadCompleted;
                do
                {
                    action2 = pictureUploadCompleted;
                    Action<FileUploadEventArgs> action3 = (Action<FileUploadEventArgs>) Delegate.Combine(action2, value);
                    pictureUploadCompleted = Interlocked.CompareExchange<Action<FileUploadEventArgs>>(ref this.pictureUploadCompleted, action3, action2);
                }
                while (pictureUploadCompleted != action2);
            }
            remove
            {
                Action<FileUploadEventArgs> action2;
                Action<FileUploadEventArgs> pictureUploadCompleted = this.pictureUploadCompleted;
                do
                {
                    action2 = pictureUploadCompleted;
                    Action<FileUploadEventArgs> action3 = (Action<FileUploadEventArgs>) Delegate.Remove(action2, value);
                    pictureUploadCompleted = Interlocked.CompareExchange<Action<FileUploadEventArgs>>(ref this.pictureUploadCompleted, action3, action2);
                }
                while (pictureUploadCompleted != action2);
            }
        }

        public event Action<EventArgsMgr.CommonEventArgs> QueryBrokerReceived
        {
            add
            {
                Action<EventArgsMgr.CommonEventArgs> action2;
                Action<EventArgsMgr.CommonEventArgs> queryBrokerReceived = this.queryBrokerReceived;
                do
                {
                    action2 = queryBrokerReceived;
                    Action<EventArgsMgr.CommonEventArgs> action3 = (Action<EventArgsMgr.CommonEventArgs>) Delegate.Combine(action2, value);
                    queryBrokerReceived = Interlocked.CompareExchange<Action<EventArgsMgr.CommonEventArgs>>(ref this.queryBrokerReceived, action3, action2);
                }
                while (queryBrokerReceived != action2);
            }
            remove
            {
                Action<EventArgsMgr.CommonEventArgs> action2;
                Action<EventArgsMgr.CommonEventArgs> queryBrokerReceived = this.queryBrokerReceived;
                do
                {
                    action2 = queryBrokerReceived;
                    Action<EventArgsMgr.CommonEventArgs> action3 = (Action<EventArgsMgr.CommonEventArgs>) Delegate.Remove(action2, value);
                    queryBrokerReceived = Interlocked.CompareExchange<Action<EventArgsMgr.CommonEventArgs>>(ref this.queryBrokerReceived, action3, action2);
                }
                while (queryBrokerReceived != action2);
            }
        }

        public event Action<SearchIndicatorResultEntity> SearchIndicatorDataReceived
        {
            add
            {
                Action<SearchIndicatorResultEntity> action2;
                Action<SearchIndicatorResultEntity> searchIndicatorDataReceived = this.searchIndicatorDataReceived;
                do
                {
                    action2 = searchIndicatorDataReceived;
                    Action<SearchIndicatorResultEntity> action3 = (Action<SearchIndicatorResultEntity>) Delegate.Combine(action2, value);
                    searchIndicatorDataReceived = Interlocked.CompareExchange<Action<SearchIndicatorResultEntity>>(ref this.searchIndicatorDataReceived, action3, action2);
                }
                while (searchIndicatorDataReceived != action2);
            }
            remove
            {
                Action<SearchIndicatorResultEntity> action2;
                Action<SearchIndicatorResultEntity> searchIndicatorDataReceived = this.searchIndicatorDataReceived;
                do
                {
                    action2 = searchIndicatorDataReceived;
                    Action<SearchIndicatorResultEntity> action3 = (Action<SearchIndicatorResultEntity>) Delegate.Remove(action2, value);
                    searchIndicatorDataReceived = Interlocked.CompareExchange<Action<SearchIndicatorResultEntity>>(ref this.searchIndicatorDataReceived, action3, action2);
                }
                while (searchIndicatorDataReceived != action2);
            }
        }

        public event Action<TemplateBlockElementsEventArgs> TemlpateBlockElementsReceived
        {
            add
            {
                Action<TemplateBlockElementsEventArgs> action2;
                Action<TemplateBlockElementsEventArgs> temlpateBlockElementsReceived = this.temlpateBlockElementsReceived;
                do
                {
                    action2 = temlpateBlockElementsReceived;
                    Action<TemplateBlockElementsEventArgs> action3 = (Action<TemplateBlockElementsEventArgs>) Delegate.Combine(action2, value);
                    temlpateBlockElementsReceived = Interlocked.CompareExchange<Action<TemplateBlockElementsEventArgs>>(ref this.temlpateBlockElementsReceived, action3, action2);
                }
                while (temlpateBlockElementsReceived != action2);
            }
            remove
            {
                Action<TemplateBlockElementsEventArgs> action2;
                Action<TemplateBlockElementsEventArgs> temlpateBlockElementsReceived = this.temlpateBlockElementsReceived;
                do
                {
                    action2 = temlpateBlockElementsReceived;
                    Action<TemplateBlockElementsEventArgs> action3 = (Action<TemplateBlockElementsEventArgs>) Delegate.Remove(action2, value);
                    temlpateBlockElementsReceived = Interlocked.CompareExchange<Action<TemplateBlockElementsEventArgs>>(ref this.temlpateBlockElementsReceived, action3, action2);
                }
                while (temlpateBlockElementsReceived != action2);
            }
        }

        private DataCore()
        {
            try
            {
                this._dataQuery = DataCenter.DataQuery;
            }
            catch (Exception)
            {
            }
        }

        private void BlockService_loadBlockTreeReceive(BlockTreeReceiveEventArgs args)
        {
            if (this._blockTreeCategoryDict.ContainsKey(args.Guid))
            {
                BlockCatRequestEntity requestEntity = this._blockTreeCategoryDict[args.Guid];
                this._blockTreeCategoryDict.Remove(args.Guid);
                //LogUtility.LogTableMessage("数据中心|开始接收返回的板块树数据，time=" + DateTime.Now.ToLongTimeString());
                List<NodeData> nodeSource = new List<NodeData>();
                try
                {
                    nodeSource = this.SplitBlockTree(args.SystemBlockStream, requestEntity);
                    nodeSource.AddRange(this.SplitDynamicBlockTree(args.SystemBlockStream));
                    if (requestEntity.CategoryList.Contains("-100.U"))
                    {
                        nodeSource.AddRange(this.SplitUserBlockTree(args.UserBlockStream));
                    }
                    lock (_treeLockObj)
                    {
                        if (this._blockTreeCallBackDict.ContainsKey(args.Guid))
                        {
                            Action<List<NodeData>> action = this._blockTreeCallBackDict[args.Guid];
                            this._blockTreeCallBackDict.Remove(args.Guid);
                            if (action != null)
                            {
                                List<NodeData> resultList = new List<NodeData>();
                                if (!String.IsNullOrEmpty(requestEntity.ChildCategoryCode))
                                {
                                    if (requestEntity.ChildCategoryCode.Contains(","))
                                    {
                                        if (requestEntity.ChildCategoryCode == "014002,014014,014015")
                                        {
                                            NodeData item = new NodeData();
                                            item.Id = "014002";
                                            item.ParentId = requestEntity.ChildCategoryCode;
                                            item.Name = "已挂牌";
                                            item.SortIndex = 0;
                                            resultList.Add(item);
                                            item = new NodeData();
                                            item.Id = "014014";
                                            item.ParentId = requestEntity.ChildCategoryCode;
                                            item.Name = "待挂牌";
                                            item.SortIndex = 0;
                                            resultList.Add(item);
                                            item = new NodeData();
                                            item.Id = "014015";
                                            item.ParentId = requestEntity.ChildCategoryCode;
                                            item.Name = "申报中";
                                            item.SortIndex = 0;
                                            resultList.Add(item);
                                        }
                                        if (requestEntity.ChildCategoryCode == "014011001,014011002")
                                        {
                                            NodeData data2 = new NodeData();
                                            data2.Id = "014011001";
                                            data2.ParentId = requestEntity.ChildCategoryCode;
                                            data2.Name = "协议转让";
                                            data2.SortIndex = 0;
                                            resultList.Add(data2);
                                            data2 = new NodeData();
                                            data2.Id = "014011002";
                                            data2.ParentId = requestEntity.ChildCategoryCode;
                                            data2.Name = "做市转让";
                                            data2.SortIndex = 0;
                                            resultList.Add(data2);
                                        }
                                    }
                                    else
                                    {
                                        this.GetChildBlocks(resultList, nodeSource, requestEntity.ChildCategoryCode);
                                    }
                                    action(resultList);
                                }
                                else
                                {
                                    action(nodeSource);
                                }
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    //LogUtility.LogTableMessage("数据中心|处理板块树数据【BlockService_loadBlockTreeReceive】异常," + exception.ToString());
                }
                if (this.blockTreeDataChanged != null)
                {
                    //<>c__DisplayClass11 class3;
                    //<>c__DisplayClass11 CS$<>8__locals12 = class3;
                    //AutoResetEvent autoReset = new AutoResetEvent(false);
                    //Thread thread = new Thread(delegate {
                    //    String sysBlockTreeStr = CS$<>8__locals12.args.SystemBlockStream;
                    //    String userBlockTreeStr = CS$<>8__locals12.args.UserBlockStream;
                    //    String categorys = String.Join(",", CS$<>8__locals12.requestEntity.CategoryList.ToArray());
                    //    if (CS$<>8__locals12.<>4__this.BlockTreeDataChanged != null)
                    //    {
                    //        CS$<>8__locals12.<>4__this.BlockTreeDataChanged(systemBlockStream, userBlockStream, categorys);
                    //    }
                    //    autoReset.Set();
                    //});
                    //thread.IsBackground = true;
                    //thread.Start();
                }
                //LogUtility.LogTableMessage("数据中心|完成接收返回的板块树数据，time=" + DateTime.Now.ToLongTimeString());
            }
        }

        public static DataCore CreateInstance()
        {
            if (_isCreating)
            {
                while (_dataCore == null)
                {
                    Thread.Sleep(10);
                }
            }
            _isCreating = true;
            if (_dataCore == null)
            {
            }
            return (_dataCore = new DataCore());
        }


        ~DataCore()
        {
        }

        private int GenerateId()
        {
            if (_counter == 0x7fffffff)
            {
                _counter = 0;
            }
            return ++_counter;
        }

        public double GetAverge(List<String> stockCodeList, int indicatorId)
        {
            IndicatorEntity indicatorEntityByCustomerId = this.GetIndicatorEntityByCustomerId(indicatorId);
            double indicatorUnit = CreateInstance().GetIndicatorUnit(indicatorId);
            double num2 = 0.0;
            if (indicatorEntityByCustomerId.CustomIndicator != null)
            {
                Dictionary<String, double> customerIndicatorData = CustomerIndicatorDataHelper.Intance.GetCustomerIndicatorData(indicatorEntityByCustomerId, stockCodeList);
                double num3 = 0.0;
                foreach (double num4 in customerIndicatorData.Values)
                {
                    if (num4 != double.MinValue)
                    {
                        num3 += num4;
                    }
                }
                return (num3 / ((double) stockCodeList.Count));
            }
            int num6 = 0;
            switch (indicatorEntityByCustomerId.IndDataType)
            {
                case DataType.Byte:
                case DataType.Int:
                case DataType.Long:
                case DataType.Short:
                case DataType.UInt:
                case DataType.ULong:
                case DataType.UShort:
                    foreach (String str in stockCodeList)
                    {
                        double num7 = this.GetIndicatorInt64Value(str, indicatorId.ToString(), indicatorUnit);
                        if (num7 != double.MinValue)
                        {
                            num6++;
                            num2 += num7;
                        }
                    }
                    break;

                case DataType.Decimal:
                case DataType.Double:
                case DataType.Float:
                    foreach (String str2 in stockCodeList)
                    {
                        double num8 = this.GetIndicatorDoubleValue(str2, indicatorId.ToString(), indicatorUnit);
                        if (num8 != double.MinValue)
                        {
                            num6++;
                            num2 += num8;
                        }
                    }
                    break;
            }
            if (num6 > 0)
            {
                return (num2 / ((double) num6));
            }
            return 0.0;
        }

        public List<NodeData> GetCategoryList(BrowserType browserType)
        {
            return IndicatorDataCore.GetCategoryValue(browserType);
        }

        public void GetChildBlocks(List<NodeData> resultList, List<NodeData> nodeSource, String pNodeId)
        {
            foreach (NodeData data in nodeSource.FindAll(delegate (NodeData node) {
                return node.ParentId.Equals(pNodeId);
            }))
            {
                resultList.Add(data);
                this.GetChildBlocks(resultList, nodeSource, data.Id);
            }
        }

        public int GetIndicatorCusotmerId(IndicatorEntity entity)
        {
            IndicatorEntity entity2 = entity.Copy();
            int item = -2147483648;
            List<int> indicatorCustomerIdList = IndicatorDataCore.GetIndicatorCustomerIdList(entity2.IndicatorCode);
            if ((indicatorCustomerIdList == null) || (indicatorCustomerIdList.Count == 0))
            {
                item = this.GenerateId();
                entity2.CustomerId = item;
                List<int> customerIdList = new List<int>(1);
                customerIdList.Add(item);
                IndicatorDataCore.SetIndicatorCustomerIdList(entity2.IndicatorCode, customerIdList);
                IndicatorDataCore.SetCustomerIndicatorEntity(item, entity2);
                IndicatorDataCore.SetIndicatorEntity(entity2.IndicatorCode, entity2);
                return item;
            }
            foreach (int num2 in indicatorCustomerIdList)
            {
                IndicatorEntity indicatorEntityByCustomerId = IndicatorDataCore.GetIndicatorEntityByCustomerId(num2);
                if (this.IsSameIndicatorParamter(entity2.Parameters, indicatorEntityByCustomerId.Parameters))
                {
                    item = num2;
                    break;
                }
            }
            if (item == -2147483648)
            {
                item = this.GenerateId();
                entity2.CustomerId = item;
                indicatorCustomerIdList.Add(item);
                IndicatorDataCore.SetIndicatorCustomerIdList(entity2.IndicatorCode, indicatorCustomerIdList);
                IndicatorDataCore.SetCustomerIndicatorEntity(item, entity2);
                IndicatorDataCore.SetIndicatorEntity(entity2.IndicatorCode, entity2);
            }
            return item;
        }

        public Type GetIndicatorDataType(IndicatorEntity indicatorEntity)
        {
            return CommonEnumerators.GetMappingDataType(indicatorEntity.IndDataType);
        }

        public Type GetIndicatorDataType(int customerIndicatorId)
        {
            return CommonEnumerators.GetMappingDataType(IndicatorDataCore.GetIndicatorEntityByCustomerId(customerIndicatorId).IndDataType);
        }

        public DateTime GetIndicatorDateTimeValue(String securityCode, IndicatorEntity entity)
        {
            String key = String.Format("{0}{1}", securityCode, entity.CustomerId);
            if (!IndicatorTableDataCore.StrIndicatorDict.ContainsKey(key))
            {
                return new DateTime();
            }
            String str2 = IndicatorTableDataCore.StrIndicatorDict[key];
            if (String.IsNullOrEmpty(str2))
            {
                return new DateTime();
            }
            return DateTime.Parse(str2);
        }

        public double GetIndicatorDecimalValue(String securityCode, IndicatorEntity entity, double unitValue)
        {
            String key = String.Format("{0}{1}", securityCode, entity.CustomerId);
            switch (entity.IndDataType)
            {
                case DataType.Bool:
                case DataType.Byte:
                case DataType.Short:
                case DataType.UShort:
                    if (!IndicatorTableDataCore.Int32IndicatorDict.ContainsKey(key))
                    {
                        return double.NaN;
                    }
                    if ((unitValue != 1.0) && (unitValue != 0.0))
                    {
                        return (((double) IndicatorTableDataCore.Int32IndicatorDict[key]) / unitValue);
                    }
                    return (double) IndicatorTableDataCore.Int32IndicatorDict[key];

                case DataType.Decimal:
                case DataType.Double:
                case DataType.Float:
                    if (!IndicatorTableDataCore.DoubleIndicatorDict.ContainsKey(key))
                    {
                        return double.NaN;
                    }
                    if ((unitValue != 1.0) && (unitValue != 0.0))
                    {
                        return (IndicatorTableDataCore.DoubleIndicatorDict[key] / unitValue);
                    }
                    return IndicatorTableDataCore.DoubleIndicatorDict[key];

                case DataType.Int:
                case DataType.Long:
                case DataType.UInt:
                case DataType.ULong:
                    if (!IndicatorTableDataCore.LongIndicatorDict.ContainsKey(key))
                    {
                        return double.NaN;
                    }
                    if ((unitValue != 1.0) && (unitValue != 0.0))
                    {
                        return (((double) IndicatorTableDataCore.LongIndicatorDict[key]) / unitValue);
                    }
                    return (double) IndicatorTableDataCore.LongIndicatorDict[key];
            }
            return double.NaN;
        }

        public double GetIndicatorDoubleValue(String securityCode, String customIndicatorId)
        {
            double indicatorUnit = this.GetIndicatorUnit(this.GetIndicatorEntityByCustomerId(int.Parse(customIndicatorId)));
            return this.GetIndicatorDoubleValue(securityCode, customIndicatorId, indicatorUnit);
        }

        public double GetIndicatorDoubleValue(String securityCode, String customIndicatorId, double unitValue)
        {
            String key = String.Format("{0}{1}", securityCode, customIndicatorId);
            if (!IndicatorTableDataCore.DoubleIndicatorDict.ContainsKey(key))
            {
                return double.MinValue;
            }
            if ((unitValue != 1.0) && (unitValue != 0.0))
            {
                return (IndicatorTableDataCore.DoubleIndicatorDict[key] / unitValue);
            }
            return IndicatorTableDataCore.DoubleIndicatorDict[key];
        }

        public IndicatorEntity GetIndicatorEntityByCategoryCode(String indicatorCode)
        {
            return IndicatorDataCore.GetIndicatorEntityByCategoryCode(indicatorCode);
        }

        public IndicatorEntity GetIndicatorEntityByCustomerId(int customerIndicatorId)
        {
            return IndicatorDataCore.GetIndicatorEntityByCustomerId(customerIndicatorId);
        }

        public Dictionary<String, IndicatorEntity> GetIndicatorEntityListByCategoryCodes(List<String> categoryCodeList)
        {
            Dictionary<String, IndicatorEntity> dictionary = new Dictionary<String, IndicatorEntity>();
            foreach (String str in categoryCodeList)
            {
                IndicatorEntity indicatorEntityByCategoryCode = this.GetIndicatorEntityByCategoryCode(str);
                if (indicatorEntityByCategoryCode == null)
                {
                    return null;
                }
                dictionary[str] = indicatorEntityByCategoryCode;
            }
            return dictionary;
        }

        public float GetIndicatorFloatValue(String securityCode, String customIndicatorId)
        {
            String key = String.Format("{0}{1}", securityCode, customIndicatorId);
            if (!IndicatorTableDataCore.FloatIndicatorDict.ContainsKey(key))
            {
                return float.MinValue;
            }
            return IndicatorTableDataCore.FloatIndicatorDict[key];
        }

        public double GetIndicatorFloatValue(String securityCode, String customIndicatorId, double unitValue)
        {
            String key = String.Format("{0}{1}", securityCode, customIndicatorId);
            if (!IndicatorTableDataCore.FloatIndicatorDict.ContainsKey(key))
            {
                return double.MinValue;
            }
            if ((unitValue != 1.0) && (unitValue != 0.0))
            {
                return (((double) IndicatorTableDataCore.FloatIndicatorDict[key]) / unitValue);
            }
            return (double) IndicatorTableDataCore.FloatIndicatorDict[key];
        }

        private String GetIndicatorFormat(DataType dataType)
        {
            String str = "";
            switch (dataType)
            {
                case DataType.Byte:
                case DataType.Int:
                case DataType.Long:
                case DataType.Short:
                case DataType.UInt:
                case DataType.ULong:
                case DataType.UShort:
                    return "#,##0";

                case DataType.ByteArray:
                case DataType.Char:
                case DataType.UshortDate:
                case DataType.DateTime:
                case DataType.String:
                    return str;

                case DataType.Decimal:
                case DataType.Double:
                case DataType.Float:
                    return "#,##0.0000";
            }
            return str;
        }

        public String GetIndicatorFormatValue(String securityCode, IndicatorEntity entity, double unitValue)
        {
            String str = String.Empty;
            String key = String.Format("{0}{1}", securityCode, entity.CustomerId);
            String str3 = entity.ShowFormat ?? String.Empty;
            if (!String.IsNullOrEmpty(str3) && !str3.Equals("null"))
            {
                FormatType type = JSONHelper.DeserializeObject<FormatType>(str3);
                String format = String.IsNullOrEmpty(type.Value) ? "#,##0.0000" : type.Value;
                if (type == null)
                {
                    return str;
                }
                switch (CommonEnumerators.GetMappingDataType(entity.IndDataType).FullName)
                {
                    case "System.Int32":
                    case "System.Int64":
                        if (IndicatorTableDataCore.LongIndicatorDict.ContainsKey(key))
                        {
                            if ((unitValue == 1.0) || (unitValue == 0.0))
                            {
                                long num7 = IndicatorTableDataCore.LongIndicatorDict[key];
                                return num7.ToString(format);
                            }
                            double num8 = ((double) IndicatorTableDataCore.LongIndicatorDict[key]) / unitValue;
                            return num8.ToString(format);
                        }
                        return String.Empty;

                    case "System.Single":
                    case "System.Double":
                    case "System.Decimal":
                        if (IndicatorTableDataCore.DoubleIndicatorDict.ContainsKey(key))
                        {
                            if ((unitValue == 1.0) || (unitValue == 0.0))
                            {
                                double num9 = IndicatorTableDataCore.DoubleIndicatorDict[key];
                                return num9.ToString(format);
                            }
                            double num10 = IndicatorTableDataCore.DoubleIndicatorDict[key] / unitValue;
                            return num10.ToString(format);
                        }
                        return String.Empty;
                }
                return (IndicatorTableDataCore.StrIndicatorDict.ContainsKey(key) ? IndicatorTableDataCore.StrIndicatorDict[key] : String.Empty);
            }
            switch (entity.IndDataType)
            {
                case DataType.Bool:
                case DataType.Byte:
                case DataType.Short:
                case DataType.UShort:
                {
                    if (!IndicatorTableDataCore.Int32IndicatorDict.ContainsKey(key))
                    {
                        return String.Empty;
                    }
                    if ((unitValue != 1.0) && (unitValue != 0.0))
                    {
                        double num2 = ((double) IndicatorTableDataCore.Int32IndicatorDict[key]) / unitValue;
                        return num2.ToString("#,##0.0000");
                    }
                    int num = IndicatorTableDataCore.Int32IndicatorDict[key];
                    return num.ToString("#,##0");
                }
                case DataType.UshortDate:
                case DataType.DateTime:
                    if (IndicatorTableDataCore.StrIndicatorDict.ContainsKey(key))
                    {
                        String str4 = IndicatorTableDataCore.StrIndicatorDict[key];
                        if (!String.IsNullOrEmpty(str4))
                        {
                            return DateTime.Parse(str4).ToString("yyyy-MM-dd");
                        }
                    }
                    return String.Empty;

                case DataType.Decimal:
                case DataType.Double:
                case DataType.Float:
                {
                    if (!IndicatorTableDataCore.DoubleIndicatorDict.ContainsKey(key))
                    {
                        return String.Empty;
                    }
                    if ((unitValue != 1.0) && (unitValue != 0.0))
                    {
                        double num6 = IndicatorTableDataCore.DoubleIndicatorDict[key] / unitValue;
                        return num6.ToString("#,##0.0000");
                    }
                    double num5 = IndicatorTableDataCore.DoubleIndicatorDict[key];
                    return num5.ToString("#,##0.0000");
                }
                case DataType.Int:
                case DataType.Long:
                case DataType.UInt:
                case DataType.ULong:
                {
                    if (!IndicatorTableDataCore.LongIndicatorDict.ContainsKey(key))
                    {
                        return String.Empty;
                    }
                    if ((unitValue != 1.0) && (unitValue != 0.0))
                    {
                        double num4 = ((double) IndicatorTableDataCore.LongIndicatorDict[key]) / unitValue;
                        return num4.ToString("#,##0.0000");
                    }
                    long num3 = IndicatorTableDataCore.LongIndicatorDict[key];
                    return num3.ToString("#,##0");
                }
            }
            return (IndicatorTableDataCore.StrIndicatorDict.ContainsKey(key) ? IndicatorTableDataCore.StrIndicatorDict[key] : String.Empty);
        }

        public String GetIndicatorFormatValue(String securityCode, String customIndicatorId, double unitValue)
        {
            String str = String.Empty;
            String key = String.Format("{0}{1}", securityCode, customIndicatorId);
            IndicatorEntity indicatorEntityByCustomerId = IndicatorDataCore.GetIndicatorEntityByCustomerId(int.Parse(customIndicatorId));
            String str3 = indicatorEntityByCustomerId.ShowFormat ?? String.Empty;
            if (!String.IsNullOrEmpty(str3) && !str3.Equals("null"))
            {
                FormatType type = JSONHelper.DeserializeObject<FormatType>(str3);
                if (type == null)
                {
                    return str;
                }
                String format = String.IsNullOrEmpty(type.Value) ? "#,##0.0000" : type.Value;
                switch (CommonEnumerators.GetMappingDataType(indicatorEntityByCustomerId.IndDataType).Name)
                {
                    case "System.Int32":
                    case "System.Int64":
                        if (IndicatorTableDataCore.LongIndicatorDict.ContainsKey(key))
                        {
                            if ((unitValue == 1.0) || (unitValue == 0.0))
                            {
                                long num7 = IndicatorTableDataCore.LongIndicatorDict[key];
                                return num7.ToString(format);
                            }
                            double num8 = ((double) IndicatorTableDataCore.LongIndicatorDict[key]) / unitValue;
                            return num8.ToString(format);
                        }
                        return String.Empty;

                    case "System.Double":
                    case "System.Decimal":
                    case "System.Single":
                        if (IndicatorTableDataCore.DoubleIndicatorDict.ContainsKey(key))
                        {
                            if ((unitValue == 1.0) || (unitValue == 0.0))
                            {
                                double num9 = IndicatorTableDataCore.DoubleIndicatorDict[key];
                                return num9.ToString(format);
                            }
                            double num10 = IndicatorTableDataCore.DoubleIndicatorDict[key] / unitValue;
                            return num10.ToString(format);
                        }
                        return String.Empty;
                }
                return (IndicatorTableDataCore.StrIndicatorDict.ContainsKey(key) ? IndicatorTableDataCore.StrIndicatorDict[key] : String.Empty);
            }
            switch (indicatorEntityByCustomerId.IndDataType)
            {
                case DataType.Bool:
                case DataType.Byte:
                case DataType.Short:
                case DataType.UShort:
                {
                    if (!IndicatorTableDataCore.Int32IndicatorDict.ContainsKey(key))
                    {
                        return String.Empty;
                    }
                    if ((unitValue != 1.0) && (unitValue != 0.0))
                    {
                        double num2 = ((double) IndicatorTableDataCore.Int32IndicatorDict[key]) / unitValue;
                        return num2.ToString("#,##0.0000");
                    }
                    int num = IndicatorTableDataCore.Int32IndicatorDict[key];
                    return num.ToString("#,##0");
                }
                case DataType.UshortDate:
                case DataType.DateTime:
                    if (!IndicatorTableDataCore.StrIndicatorDict.ContainsKey(key))
                    {
                        return String.Empty;
                    }
                    return DateTime.Parse(IndicatorTableDataCore.StrIndicatorDict[key]).ToString("yyyy-MM-dd");

                case DataType.Decimal:
                case DataType.Double:
                case DataType.Float:
                {
                    if (!IndicatorTableDataCore.DoubleIndicatorDict.ContainsKey(key))
                    {
                        return String.Empty;
                    }
                    if ((unitValue != 1.0) && (unitValue != 0.0))
                    {
                        double num6 = IndicatorTableDataCore.DoubleIndicatorDict[key] / unitValue;
                        return num6.ToString("#,##0.0000");
                    }
                    double num5 = IndicatorTableDataCore.DoubleIndicatorDict[key];
                    return num5.ToString("#,##0.00");
                }
                case DataType.Int:
                case DataType.Long:
                case DataType.UInt:
                case DataType.ULong:
                {
                    if (!IndicatorTableDataCore.LongIndicatorDict.ContainsKey(key))
                    {
                        return String.Empty;
                    }
                    if ((unitValue != 1.0) && (unitValue != 0.0))
                    {
                        double num4 = ((double) IndicatorTableDataCore.LongIndicatorDict[key]) / unitValue;
                        return num4.ToString("#,##0.0000");
                    }
                    long num3 = IndicatorTableDataCore.LongIndicatorDict[key];
                    return num3.ToString("#,##0");
                }
            }
            return (IndicatorTableDataCore.StrIndicatorDict.ContainsKey(key) ? IndicatorTableDataCore.StrIndicatorDict[key] : String.Empty);
        }

        public int GetIndicatorInt32Value(String securityCode, String customIndicatorId)
        {
            String key = String.Format("{0}{1}", securityCode, customIndicatorId);
            if (!IndicatorTableDataCore.Int32IndicatorDict.ContainsKey(key))
            {
                return -2147483648;
            }
            return IndicatorTableDataCore.Int32IndicatorDict[key];
        }

        public double GetIndicatorInt32Value(String securityCode, String customIndicatorId, double unitValue)
        {
            String key = String.Format("{0}{1}", securityCode, customIndicatorId);
            if (!IndicatorTableDataCore.Int32IndicatorDict.ContainsKey(key))
            {
                return double.MinValue;
            }
            if ((unitValue != 1.0) && (unitValue != 0.0))
            {
                return (((double) IndicatorTableDataCore.Int32IndicatorDict[key]) / unitValue);
            }
            return (double) IndicatorTableDataCore.Int32IndicatorDict[key];
        }

        public long GetIndicatorInt64Value(String securityCode, String customIndicatorId)
        {
            String key = String.Format("{0}{1}", securityCode, customIndicatorId);
            if (!IndicatorTableDataCore.LongIndicatorDict.ContainsKey(key))
            {
                return -9223372036854775808L;
            }
            return IndicatorTableDataCore.LongIndicatorDict[key];
        }

        public double GetIndicatorInt64Value(String securityCode, String customIndicatorId, double unitValue)
        {
            String key = String.Format("{0}{1}", securityCode, customIndicatorId);
            if (!IndicatorTableDataCore.LongIndicatorDict.ContainsKey(key))
            {
                return double.MinValue;
            }
            if ((unitValue != 1.0) && (unitValue != 0.0))
            {
                return (((double) IndicatorTableDataCore.LongIndicatorDict[key]) / unitValue);
            }
            return (double) IndicatorTableDataCore.LongIndicatorDict[key];
        }

        public String GetIndicatorStringValue(String securityCode, String customIndicatorId)
        {
            String key = String.Format("{0}{1}", securityCode, customIndicatorId);
            if (IndicatorTableDataCore.StrIndicatorDict.ContainsKey(key))
            {
                return IndicatorTableDataCore.StrIndicatorDict[key];
            }
            return String.Empty;
        }

        public double GetIndicatorUnit(IndicatorEntity indicatorEntity)
        {
            decimal unitProportion = 1M;
            if (indicatorEntity == null)
            {
                return 1.0;
            }
            if (String.IsNullOrEmpty(indicatorEntity.Parameters) || indicatorEntity.Parameters.Equals("null"))
            {
                unitProportion = 1M;
            }
            else
            {
                foreach (ParamterObject obj2 in JSONHelper.DeserializeObject<List<ParamterObject>>(indicatorEntity.Parameters))
                {
                    if (obj2.Type == "261")
                    {
                        unitProportion = UnitDataHandle.GetUnitProportion(obj2.DefaultValue.ToString());
                    }
                }
            }
            return double.Parse(unitProportion.ToString());
        }

        public double GetIndicatorUnit(int customerIndicatorId)
        {
            IndicatorEntity indicatorEntityByCustomerId = IndicatorDataCore.GetIndicatorEntityByCustomerId(customerIndicatorId, false);
            return this.GetIndicatorUnit(indicatorEntityByCustomerId);
        }

        public object GetIndicatorValue(String securityCode, int customIndicatorId)
        {
            double indicatorUnit = this.GetIndicatorUnit(customIndicatorId);
            object obj2 = 0;
            String key = String.Format("{0}{1}", securityCode, customIndicatorId);
            IndicatorEntity indicatorEntityByCustomerId = IndicatorDataCore.GetIndicatorEntityByCustomerId(customIndicatorId);
            String str2 = indicatorEntityByCustomerId.ShowFormat ?? String.Empty;
            if (!String.IsNullOrEmpty(str2) && !str2.Equals("null"))
            {
                if (JSONHelper.DeserializeObject<FormatType>(str2) == null)
                {
                    return obj2;
                }
                switch (CommonEnumerators.GetMappingDataType(indicatorEntityByCustomerId.IndDataType).FullName)
                {
                    case "System.Int32":
                    case "System.Int64":
                        if (IndicatorTableDataCore.LongIndicatorDict.ContainsKey(key))
                        {
                            if (indicatorUnit == 0.0 || indicatorUnit == 1.0)
                            {
                                return IndicatorTableDataCore.LongIndicatorDict[key];
                            }
                            return (((double) IndicatorTableDataCore.LongIndicatorDict[key]) / indicatorUnit);
                        }
                        return String.Empty;

                    case "System.Single":
                    case "System.Double":
                    case "System.Decimal":
                        if (IndicatorTableDataCore.DoubleIndicatorDict.ContainsKey(key))
                        {
                            if (indicatorUnit == 0.0 || indicatorUnit == 1.0)
                            {
                                return IndicatorTableDataCore.DoubleIndicatorDict[key];
                            }
                            return (IndicatorTableDataCore.DoubleIndicatorDict[key] / indicatorUnit);
                        }
                        return String.Empty;
                }
                return (IndicatorTableDataCore.StrIndicatorDict.ContainsKey(key) ? IndicatorTableDataCore.StrIndicatorDict[key] : String.Empty);
            }
            switch (indicatorEntityByCustomerId.IndDataType)
            {
                case DataType.Bool:
                case DataType.Byte:
                case DataType.Short:
                case DataType.UShort:
                    if (!IndicatorTableDataCore.Int32IndicatorDict.ContainsKey(key))
                    {
                        return String.Empty;
                    }
                    if ((indicatorUnit != 1.0) && (indicatorUnit != 0.0))
                    {
                        return (((double) IndicatorTableDataCore.Int32IndicatorDict[key]) / indicatorUnit);
                    }
                    return IndicatorTableDataCore.Int32IndicatorDict[key];

                case DataType.UshortDate:
                case DataType.DateTime:
                    if (!IndicatorTableDataCore.StrIndicatorDict.ContainsKey(key))
                    {
                        return String.Empty;
                    }
                    return DateTime.Parse(IndicatorTableDataCore.StrIndicatorDict[key]).ToString("yyyy-MM-dd");

                case DataType.Decimal:
                case DataType.Double:
                case DataType.Float:
                    if (!IndicatorTableDataCore.DoubleIndicatorDict.ContainsKey(key))
                    {
                        return String.Empty;
                    }
                    if ((indicatorUnit != 1.0) && (indicatorUnit != 0.0))
                    {
                        return (IndicatorTableDataCore.DoubleIndicatorDict[key] / indicatorUnit);
                    }
                    return IndicatorTableDataCore.DoubleIndicatorDict[key];

                case DataType.Int:
                case DataType.Long:
                case DataType.UInt:
                case DataType.ULong:
                    if (!IndicatorTableDataCore.LongIndicatorDict.ContainsKey(key))
                    {
                        return String.Empty;
                    }
                    if ((indicatorUnit != 1.0) && (indicatorUnit != 0.0))
                    {
                        return (((double) IndicatorTableDataCore.LongIndicatorDict[key]) / indicatorUnit);
                    }
                    return IndicatorTableDataCore.LongIndicatorDict[key];
            }
            return (IndicatorTableDataCore.StrIndicatorDict.ContainsKey(key) ? IndicatorTableDataCore.StrIndicatorDict[key] : String.Empty);
        }

        public object GetMainBrokers(int type)
        {
            Dictionary<String, List<BrokerEntity>> dictionary = new Dictionary<String, List<BrokerEntity>>();
            try
            {
                String cmd = "$-rpt\r\n$name=RPT_NTSHOST_LISTED\r\n";
                if (type != 0)
                {
                    cmd = @"$-rpt\r\n$name=RPT_NTS_HOSTMAKER\r\n$type=" + type.ToString();
                }
                DataSet set = this._dataQuery.QueryIndicate(cmd) as DataSet;
                if ((set == null) || (set.Tables.Count == 0))
                {
                    return null;
                }
                DataTable table = set.Tables[0];
                foreach (DataRow row in table.Rows)
                {
                    String str2 = row["CODE"].ToString();
                    String str3 = row["MAKER"].ToString();
                    String key = row["PINYIN"].ToString();
                    BrokerEntity item = new BrokerEntity();
                    item.Cat = key;
                    item.Code = str2;
                    item.Name = str3;
                    if (!dictionary.ContainsKey(key))
                    {
                        dictionary[key] = new List<BrokerEntity>();
                    }
                    dictionary[key].Add(item);
                }
                return dictionary;
            }
            catch (Exception exception)
            {
                //LogUtility.LogTableMessage("数据中心|取主办券商异常," + exception);
            }
            return dictionary;
        }

        public void GetMainBrokersAsync(String id, int type)
        {
            DelegateMgr2.QueryAllBrokersHandler handle = new DelegateMgr2.QueryAllBrokersHandler(this.GetMainBrokers);
            handle.BeginInvoke(type, delegate (IAsyncResult ar) {
                try
                {
                    object obj2 = handle.EndInvoke(ar);
                    EventArgsMgr.CommonEventArgs args = new EventArgsMgr.CommonEventArgs();
                    args.Id = id;
                    args.Data = obj2;
                    if (this.queryBrokerReceived != null)
                    {
                        this.queryBrokerReceived(args);
                    }
                }
                catch (Exception exception)
                {
                    //LogUtility.LogTableMessage("取主办券商异常," + exception);
                }
            }, null);
        }

        public object GetTopValue(int topIndex, List<String> stockCodeList, int customerId)
        {
            IndicatorEntity indicatorEntityByCustomerId = CreateInstance().GetIndicatorEntityByCustomerId(customerId);
            if (((indicatorEntityByCustomerId.CustomIndicator != null) && (indicatorEntityByCustomerId.CustomIndicator.IndicatorList != null)) && (indicatorEntityByCustomerId.CustomIndicator.IndicatorList.Count > 0))
            {
                Dictionary<String, double> customerIndicatorData = CustomerIndicatorDataHelper.Intance.GetCustomerIndicatorData(customerId, stockCodeList);
                List<FieldValueEntity<double>> list = new List<FieldValueEntity<double>>();
                foreach (KeyValuePair<String, double> pair in customerIndicatorData)
                {
                    FieldValueEntity<double> item = new FieldValueEntity<double>(pair.Value, pair.Key);
                    list.Add(item);
                }
                list.Sort();
                int num = list.Count - topIndex;
                return list[num].Data;
            }
            List<String> list2 = CreateInstance().SortIndicator(stockCodeList, customerId, CommonEnumerators.SortMode.Desc);
            if (list2.Count < topIndex)
            {
                topIndex = list2.Count;
            }
            return this.GetIndicatorValue(list2[topIndex - 1], customerId);
        }

        private double GetWeightedIndicatorValue(IndicatorEntity weightedIndicator, String stockCode, int weightedIndicatorId, double weightedUnitValue)
        {
            if (weightedIndicator == null)
            {
                return 0.0;
            }
            double num = 0.0;
            switch (weightedIndicator.IndDataType)
            {
                case DataType.Byte:
                case DataType.Int:
                case DataType.Long:
                case DataType.Short:
                case DataType.UInt:
                case DataType.ULong:
                case DataType.UShort:
                    return this.GetIndicatorInt64Value(stockCode, weightedIndicatorId.ToString(), weightedUnitValue);

                case DataType.ByteArray:
                case DataType.Char:
                case DataType.UshortDate:
                case DataType.DateTime:
                case DataType.String:
                    return num;

                case DataType.Decimal:
                case DataType.Double:
                case DataType.Float:
                    return this.GetIndicatorDoubleValue(stockCode, weightedIndicatorId.ToString(), weightedUnitValue);
            }
            return num;
        }

        public object IndicatorValueParse(IndicatorEntity indicator, String value)
        {
            double indicatorUnit = this.GetIndicatorUnit(indicator);
            bool flag = indicator.CustomIndicator != null;
            switch (indicator.IndDataType)
            {
                case DataType.Byte:
                case DataType.Double:
                case DataType.Float:
                case DataType.Int:
                case DataType.Long:
                case DataType.Short:
                case DataType.UInt:
                case DataType.ULong:
                case DataType.UShort:
                    double num2;
                    double.TryParse(value, out num2);
                    if (!flag)
                    {
                        return (num2 * indicatorUnit);
                    }
                    return num2;

                case DataType.ByteArray:
                case DataType.Char:
                case DataType.UshortDate:
                case DataType.String:
                    return value;

                case DataType.DateTime:
                    DateTime time;
                    DateTime.TryParse(value, out time);
                    return time;

                case DataType.Decimal:
                {
                    decimal num3;
                    decimal num4 = Convert.ToDecimal(indicatorUnit);
                    decimal.TryParse(value, out num3);
                    if (!flag)
                    {
                        return (num3 * num4);
                    }
                    return num3;
                }
            }
            return value;
        }

        public bool IsCustomIndicator(IndicatorEntity indicatorEntity)
        {
            return (((indicatorEntity.CustomIndicator != null) && (indicatorEntity.CustomIndicator.IndicatorList != null)) && (indicatorEntity.CustomIndicator.IndicatorList.Count != 0));
        }

        private bool IsSameIndicatorParamter(String paramter, String comParamter)
        {
            if (String.IsNullOrEmpty(paramter))
            {
                return String.IsNullOrEmpty(comParamter);
            }
            if (String.IsNullOrEmpty(comParamter))
            {
                return false;
            }
            try
            {
                List<ParamterObject> list = JSONHelper.DeserializeObject<List<ParamterObject>>(paramter);
                List<ParamterObject> list2 = JSONHelper.DeserializeObject<List<ParamterObject>>(comParamter);
                for (int i = 0; i < list.Count; i++)
                {
                    if (!list[i].IsHide && !object.Equals(list[i].DefaultValue, list2[i].DefaultValue))
                    {
                        return false;
                    }
                }
            }
            catch (Exception exception)
            {
                //LogUtility.LogTableMessage("数据中心:-比较指标：paramter=" + paramter + " comParamter=" + comParamter + "--" + exception.Message + exception.StackTrace);
            }
            return true;
        }

        public List<String> Matching(List<String> stockCodeList, FilterEntity filterEntity)
        {
            List<FilterEntity> list = FilterEntity.ExtractFilterList(filterEntity);
            DataTable table = new DataTable();
            table.Columns.Add("stockcode", typeof(String));
            Dictionary<String, IndicatorEntity> dictionary = new Dictionary<String, IndicatorEntity>();
            List<IndicatorEntity> list2 = new List<IndicatorEntity>();
            List<IndicatorEntity> list3 = new List<IndicatorEntity>();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<String> list4 = new List<String>();
            foreach (FilterEntity entity in list)
            {
                String item = entity.LeftPart.ToString();
                if (!list4.Contains(item))
                {
                    list4.Add(item);
                }
                if ((entity.Operate == CommonEnumerators.FilterOperate.GtIndicator) || (entity.Operate == CommonEnumerators.FilterOperate.LtIndicator))
                {
                    item = entity.RightPart.ToString();
                    if (!list4.Contains(item))
                    {
                        list4.Add(item);
                    }
                }
            }
            foreach (String str2 in list4)
            {
                if (table.Columns.Contains(str2))
                {
                    continue;
                }
                IndicatorEntity indicatorEntityByCustomerId = IndicatorDataCore.GetIndicatorEntityByCustomerId(int.Parse(str2));
                Type type = null;
                if (this.IsCustomIndicator(indicatorEntityByCustomerId))
                {
                    switch (indicatorEntityByCustomerId.IndDataType)
                    {
                        case DataType.DateTime:
                        case DataType.String:
                            type = typeof(String);
                            list3.Add(indicatorEntityByCustomerId);
                            goto Label_019D;
                    }
                    type = typeof(double);
                    list2.Add(indicatorEntityByCustomerId);
                }
                else
                {
                    switch (indicatorEntityByCustomerId.IndDataType)
                    {
                        case DataType.DateTime:
                        case DataType.String:
                            type = typeof(String);
                            break;

                        default:
                            type = typeof(double);
                            break;
                    }
                    if (dictionary.ContainsKey(str2))
                    {
                        continue;
                    }
                    dictionary[str2] = indicatorEntityByCustomerId;
                }
            Label_019D:
                table.Columns.Add(str2, type);
            }
            stopwatch.Stop();
            //LogUtility.LogTableMessage("Matching 初始化表结构耗时," + stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();
            stopwatch.Start();
            String filterExpression = FilterEntity.BuildSql(filterEntity);
            stopwatch.Stop();
            //LogUtility.LogTableMessage("Matching 构造sql耗时," + stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();
            stopwatch.Start();
            Dictionary<String, double> dictionary2 = new Dictionary<String, double>();
            foreach (String str4 in dictionary.Keys)
            {
                dictionary2[str4] = this.GetIndicatorUnit(this.GetIndicatorEntityByCustomerId(int.Parse(str4)));
            }
            if (dictionary.Count > 0)
            {
                foreach (String str5 in stockCodeList)
                {
                    DataRow row = table.NewRow();
                    row["stockcode"] = str5;
                    foreach (KeyValuePair<String, IndicatorEntity> pair in dictionary)
                    {
                        String key = pair.Key;
                        double num = 0.0;
                        switch (this.GetIndicatorDataType(pair.Value).FullName)
                        {
                            case "System.Int32":
                            case "System.Int64":
                            {
                                num = this.GetIndicatorInt64Value(str5, key, dictionary2[key]);
                                if (num != double.MinValue)
                                {
                                    row[key] = num;
                                }
                                continue;
                            }
                            case "System.Single":
                            case "System.Double":
                            case "System.Decimal":
                            {
                                num = this.GetIndicatorDoubleValue(str5, key, dictionary2[key]);
                                if (num != double.MinValue)
                                {
                                    row[key] = num;
                                }
                                continue;
                            }
                            case "System.DateTime":
                            {
                                String str7 = this.GetIndicatorStringValue(str5, key);
                                if (!String.IsNullOrEmpty(str7))
                                {
                                    row[key] = DateTime.Parse(str7);
                                }
                                continue;
                            }
                        }
                        String indicatorStringValue = this.GetIndicatorStringValue(str5, key);
                        row[key] = String.IsNullOrEmpty(indicatorStringValue) ? "--" : indicatorStringValue;
                    }
                    table.Rows.Add(row);
                }
            }
            stopwatch.Stop();
            //LogUtility.LogTableMessage("Matching 填充系统指标耗时," + stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();
            stopwatch.Start();
            if (list2.Count > 0)
            {
                foreach (IndicatorEntity entity3 in list2)
                {
                    String str9 = entity3.CustomerId.ToString();
                    Dictionary<String, double> customerIndicatorData = CustomerIndicatorDataHelper.Intance.GetCustomerIndicatorData(entity3, stockCodeList);
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row2 in table.Rows)
                        {
                            row2[str9] = customerIndicatorData[row2["stockcode"].ToString()];
                        }
                    }
                    else
                    {
                        foreach (String str10 in stockCodeList)
                        {
                            DataRow row3 = table.NewRow();
                            row3["stockcode"] = str10;
                            row3[str9] = customerIndicatorData[str10];
                            table.Rows.Add(row3);
                        }
                    }
                    customerIndicatorData.Clear();
                }
            }
            if (list3.Count > 0)
            {
                foreach (IndicatorEntity entity4 in list3)
                {
                    String str11 = entity4.CustomerId.ToString();
                    Dictionary<String, String> customerIndicatorDataString = CustomerIndicatorDataHelper.Intance.GetCustomerIndicatorDataString(entity4, stockCodeList);
                    if (table.Rows.Count > 0)
                    {
                        foreach (DataRow row4 in table.Rows)
                        {
                            row4[str11] = customerIndicatorDataString[row4["stockcode"].ToString()];
                        }
                    }
                    else
                    {
                        foreach (String str12 in stockCodeList)
                        {
                            DataRow row5 = table.NewRow();
                            row5["stockcode"] = str12;
                            row5[str11] = customerIndicatorDataString[str12];
                            table.Rows.Add(row5);
                        }
                    }
                    customerIndicatorDataString.Clear();
                }
            }
            stopwatch.Stop();
            //LogUtility.LogTableMessage("Matching 填充自定义指标耗时," + stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();
            stopwatch.Start();
            DataRow[] rowArray = table.Select(filterExpression);
            stopwatch.Stop();
            //LogUtility.LogTableMessage("Matching 筛选耗时," + stopwatch.ElapsedMilliseconds);
            if (rowArray.Length == 0)
            {
                return new List<String>();
            }
            List<String> list5 = new List<String>();
            foreach (DataRow row6 in rowArray)
            {
                list5.Add(row6["stockcode"].ToString());
            }
            table.Rows.Clear();
            table.Dispose();
            return list5;
        }

        public void QueryTradeDateAsync(BlockIndicatorParams blockParams, DelegateMgr2.CommonDataReceiveHandler handler)
        {
            EmSocketClient.DelegateMgr.SendBackHandle handle = null;
            try
            {
                String str;
                StringBuilder builder = new StringBuilder();
                builder.AppendLine("$-rpt");
                builder.AppendLine("$name=Const_TradeDate");
                builder.AppendLine(String.Format("$StartDate={0},EndDate={1},Period={2},TEXCH={3},DataType=1,Desc={4}", new object[] { blockParams.StartDate.ToString("yyyy-MM-dd"), blockParams.EndDate.ToString("yyyy-MM-dd"), ((int) blockParams.Cycle) + 1, "CNSESH", blockParams.Sort.ToString().ToUpper() }));
                if (handle == null)
                {
                    handle = delegate (MessageEntity response) {
                        DataSet msgBody = response.MsgBody as DataSet;
                        List<String> data = new List<String>();
                        if ((msgBody != null) && (msgBody.Tables.Count > 0))
                        {
                            foreach (DataRow row in msgBody.Tables[0].Rows)
                            {
                                data.Add(Convert.ToDateTime(row[0]).ToString("yyyy-MM-dd"));
                            }
                        }
                        if (handler != null)
                        {
                            handler(data);
                        }
                    };
                }
                this._dataQuery.QueryIndicate(builder.ToString(), out str, handle);
            }
            catch (Exception exception)
            {
                //LogUtility.LogTableMessage("数据中心|取市场交易日异常," + exception);
            }
        }

        public void SaveIndicatorEntityCustomer(IndicatorEntity entity)
        {
            IndicatorDataCore.SetCustomerIndicatorEntity(entity.CustomerId, entity);
        }

        private void SearchIndicatorCallback(IAsyncResult callback)
        {
            SearchIndcatorDelegate asyncState = (SearchIndcatorDelegate) callback.AsyncState;
            if (asyncState != null)
            {
                SearchIndicatorResultEntity entity = asyncState.EndInvoke(callback);
                if (this.searchIndicatorDataReceived != null)
                {
                    this.searchIndicatorDataReceived(entity);
                }
            }
        }

        public SearchIndicatorResultEntity SearchIndicatorInfos(String requestId, String categorycodes)
        {
            SearchIndicatorResultEntity entity = new SearchIndicatorResultEntity();
            entity.RequestId = requestId;
            entity.CategoryCodes = categorycodes;
            List<NodeData> list = new List<NodeData>();
            try
            {
                foreach (KeyValuePair<String, String> pair in SearchIndicatorTextBox.GetIndicatorListInfos(categorycodes))
                {
                    if (!pair.Value.ToString().Equals("0"))
                    {
                        String[] strArray = pair.Value.Split(new char[] { '}' }, StringSplitOptions.RemoveEmptyEntries);
                        int num = categorycodes.LastIndexOf('/');
                        String str = categorycodes.Substring(0, num + 1);
                        String str2 = categorycodes.Substring(num + 1);
                        foreach (String str3 in strArray)
                        {
                            String[] strArray2 = str3.Split(new char[] { '◎' }, StringSplitOptions.RemoveEmptyEntries);
                            NodeData data2 = new NodeData();
                            data2.Id = strArray2[6];
                            data2.ParentId = strArray2[1];
                            data2.Name = strArray2[2];
                            data2.SortIndex = int.Parse(strArray2[3]);
                            data2.IsCatalog = !strArray2[4].Equals("0");
                            NodeData item = data2;
                            if (!item.IsCatalog && str2.Equals(strArray2[0]))
                            {
                                entity.CategoryCodes = str + item.Id;
                            }
                            list.Add(item);
                        }
                    }
                }
                entity.NodeList = list;
            }
            catch (Exception exception)
            {
                //LogUtility.LogMessage("[数据中心]指标树搜索失败," + exception.ToString());
            }
            return entity;
        }

        public void SearchIndicatorInfosAsync(String requestId, String categoryCodes)
        {
            SearchIndcatorDelegate delegate2 = new SearchIndcatorDelegate(this.SearchIndicatorInfos);
            delegate2.BeginInvoke(requestId, categoryCodes, new AsyncCallback(this.SearchIndicatorCallback), delegate2);
        }

        private void SetBlockIndicatorData(int indicatorCustomerId, CommonEnumerators.SortMode sortMode, List<BlockEntityBase> sortBlockList, IndicatorEntity indicatorEntity)
        {
            switch (indicatorEntity.IndDataType)
            {
                case DataType.Bool:
                case DataType.Byte:
                case DataType.Short:
                case DataType.UShort:
                {
                    int num = 0;
                    foreach (BlockEntityBase base2 in sortBlockList)
                    {
                        num = this.GetIndicatorInt32Value(base2.BlockCode, indicatorCustomerId.ToString());
                        base2.Data = (num == -2147483648) ? 0 : num;
                        base2.Mode = sortMode;
                        if ((base2.ChildBlockList != null) && (base2.ChildBlockList.Count != 0))
                        {
                            this.SetBlockIndicatorData(indicatorCustomerId, sortMode, base2.ChildBlockList, indicatorEntity);
                        }
                    }
                    break;
                }
                case DataType.ByteArray:
                    break;

                case DataType.Char:
                case DataType.UshortDate:
                case DataType.DateTime:
                case DataType.String:
                    foreach (BlockEntityBase base3 in sortBlockList)
                    {
                        base3.Data = this.GetIndicatorStringValue(base3.BlockCode, indicatorCustomerId.ToString());
                        base3.Mode = sortMode;
                        if ((base3.ChildBlockList != null) && (base3.ChildBlockList.Count != 0))
                        {
                            this.SetBlockIndicatorData(indicatorCustomerId, sortMode, base3.ChildBlockList, indicatorEntity);
                        }
                    }
                    break;

                case DataType.Decimal:
                case DataType.Double:
                case DataType.Float:
                {
                    double indicatorDoubleValue = 0.0;
                    foreach (BlockEntityBase base4 in sortBlockList)
                    {
                        indicatorDoubleValue = this.GetIndicatorDoubleValue(base4.BlockCode, indicatorCustomerId.ToString());
                        base4.Data = (indicatorDoubleValue == double.MinValue) ? 0.0 : indicatorDoubleValue;
                        base4.Mode = sortMode;
                        if ((base4.ChildBlockList != null) && (base4.ChildBlockList.Count != 0))
                        {
                            this.SetBlockIndicatorData(indicatorCustomerId, sortMode, base4.ChildBlockList, indicatorEntity);
                        }
                    }
                    break;
                }
                case DataType.Int:
                case DataType.Long:
                case DataType.UInt:
                case DataType.ULong:
                {
                    long num3 = 0L;
                    foreach (BlockEntityBase base5 in sortBlockList)
                    {
                        num3 = this.GetIndicatorInt64Value(base5.BlockCode, indicatorCustomerId.ToString());
                        base5.Data = (num3 == -9223372036854775808L) ? 0L : num3;
                        base5.Mode = sortMode;
                        if ((base5.ChildBlockList != null) && (base5.ChildBlockList.Count != 0))
                        {
                            this.SetBlockIndicatorData(indicatorCustomerId, sortMode, base5.ChildBlockList, indicatorEntity);
                        }
                    }
                    break;
                }
                default:
                    return;
            }
        }

        public void SetCustomerIndicatorEntity(IndicatorEntity entity, int indicatorIndex, uint chartId)
        {
            IndicatorEntityEventArgs e = null;
            List<IndicatorEntityEx> indicatorList = new List<IndicatorEntityEx>();
            int indicatorCusotmerId = -2147483648;
            indicatorCusotmerId = this.GetIndicatorCusotmerId(entity);
            IndicatorEntityEx item = new IndicatorEntityEx();
            item.CustomerIndicatorEntity = entity;
            item.CustomerIndicatorId = indicatorCusotmerId;
            item.IndicatorIndex = indicatorIndex;
            indicatorList.Add(item);
            e = new IndicatorEntityEventArgs(chartId, indicatorList);
            if (this.customerIndicatorEntityGenerated != null)
            {
                this.customerIndicatorEntityGenerated(this, e);
            }
        }

        public void SetCustomerIndicatorEntityEx(IndicatorEntity entity, int indicatorIndex, uint chartId)
        {
            IndicatorEntityEventArgs e = null;
            List<IndicatorEntityEx> indicatorList = new List<IndicatorEntityEx>();
            int customerId = entity.CustomerId;
            if (indicatorIndex == -1)
            {
                customerId = this.GenerateId();
            }
            IndicatorDataCore.SetCustomerIndicatorEntity(customerId, entity);
            IndicatorEntityEx item = new IndicatorEntityEx();
            item.CustomerIndicatorEntity = entity;
            item.CustomerIndicatorId = customerId;
            item.IndicatorIndex = indicatorIndex;
            indicatorList.Add(item);
            e = new IndicatorEntityEventArgs(chartId, indicatorList);
            if (this.customerIndicatorEntityGenerated != null)
            {
                this.customerIndicatorEntityGenerated(this, e);
            }
        }

        public void SetManyCustomerIndicatorEntity(List<IndicatorEntityEx> indicatorExList, uint chartId)
        {
            IndicatorEntityEventArgs e = null;
            foreach (IndicatorEntityEx ex in indicatorExList)
            {
                int indicatorCusotmerId = -2147483648;
                indicatorCusotmerId = this.GetIndicatorCusotmerId(ex.CustomerIndicatorEntity);
                ex.CustomerIndicatorId = indicatorCusotmerId;
            }
            e = new IndicatorEntityEventArgs(chartId, indicatorExList);
            if (this.customerIndicatorEntityGenerated != null)
            {
                this.customerIndicatorEntityGenerated(this, e);
            }
        }

        public List<String> SortCustomIndicator(List<String> stockCodeList, int indicatorCustomerId, CommonEnumerators.SortMode sortMode)
        {
            List<String> list = new List<String>();
            IndicatorEntity indicatorEntityByCustomerId = CreateInstance().GetIndicatorEntityByCustomerId(indicatorCustomerId);
            if (((indicatorEntityByCustomerId.CustomIndicator != null) && (indicatorEntityByCustomerId.CustomIndicator.IndicatorList != null)) && (indicatorEntityByCustomerId.CustomIndicator.IndicatorList.Count > 0))
            {
                if (CustomerIndicatorDataHelper.Intance.IsDouble(indicatorEntityByCustomerId.IndDataType))
                {
                    Dictionary<String, double> customerIndicatorData = CustomerIndicatorDataHelper.Intance.GetCustomerIndicatorData(indicatorCustomerId, stockCodeList);
                    List<FieldValueEntity<double>> list2 = new List<FieldValueEntity<double>>();
                    foreach (KeyValuePair<String, double> pair in customerIndicatorData)
                    {
                        FieldValueEntity<double> item = new FieldValueEntity<double>(pair.Value, pair.Key);
                        list2.Add(item);
                    }
                    list2.Sort();
                    if (sortMode == CommonEnumerators.SortMode.Desc)
                    {
                        for (int k = list2.Count - 1; k >= 0; k--)
                        {
                            list.Add(list2[k].FieldCode);
                        }
                        return list;
                    }
                    for (int j = 0; j < list2.Count; j++)
                    {
                        list.Add(list2[j].FieldCode);
                    }
                    return list;
                }
                Dictionary<String, String> customerIndicatorDataString = CustomerIndicatorDataHelper.Intance.GetCustomerIndicatorDataString(indicatorCustomerId, stockCodeList);
                List<FieldValueEntity<String>> list3 = new List<FieldValueEntity<String>>();
                foreach (KeyValuePair<String, String> pair2 in customerIndicatorDataString)
                {
                    FieldValueEntity<String> entity3 = new FieldValueEntity<String>(pair2.Value, pair2.Key);
                    list3.Add(entity3);
                }
                list3.Sort();
                if (sortMode == CommonEnumerators.SortMode.Desc)
                {
                    for (int m = list3.Count - 1; m >= 0; m--)
                    {
                        list.Add(list3[m].FieldCode);
                    }
                    return list;
                }
                for (int i = 0; i < list3.Count; i++)
                {
                    list.Add(list3[i].FieldCode);
                }
            }
            return list;
        }

        public List<String> SortIndicator(List<String> stockCodeList, int indicatorId, CommonEnumerators.SortMode sortMode)
        {
            List<String> list = new List<String>();
            List<String> list2 = new List<String>();
            List<FieldValueEntity<int>> list3 = null;
            List<FieldValueEntity<float>> list4 = null;
            List<FieldValueEntity<double>> list5 = null;
            List<FieldValueEntity<long>> list6 = null;
            List<FieldValueEntity<String>> list7 = null;
            try
            {
                IndicatorEntity indicatorEntityByCustomerId = this.GetIndicatorEntityByCustomerId(indicatorId);
                if (indicatorEntityByCustomerId == null)
                {
                    return stockCodeList;
                }
                switch (indicatorEntityByCustomerId.IndDataType)
                {
                    case DataType.Bool:
                    case DataType.Byte:
                    case DataType.Short:
                    case DataType.UShort:
                        list3 = new List<FieldValueEntity<int>>(stockCodeList.Count);
                        foreach (String str in stockCodeList)
                        {
                            int data = this.GetIndicatorInt32Value(str, indicatorId.ToString());
                            if (data == -2147483648)
                            {
                                list2.Add(str);
                            }
                            else
                            {
                                list3.Add(new FieldValueEntity<int>(data, str));
                            }
                        }
                        break;

                    case DataType.Char:
                    case DataType.String:
                        list7 = new List<FieldValueEntity<String>>(stockCodeList.Count);
                        foreach (String str2 in stockCodeList)
                        {
                            String indicatorStringValue = this.GetIndicatorStringValue(str2, indicatorId.ToString());
                            if (String.IsNullOrEmpty(indicatorStringValue))
                            {
                                list2.Add(str2);
                            }
                            else
                            {
                                list7.Add(new FieldValueEntity<String>(indicatorStringValue, str2));
                            }
                        }
                        break;

                    case DataType.UshortDate:
                    case DataType.DateTime:
                        list7 = new List<FieldValueEntity<String>>(stockCodeList.Count);
                        foreach (String str4 in stockCodeList)
                        {
                            String str5 = this.GetIndicatorStringValue(str4, indicatorId.ToString());
                            if (!String.IsNullOrEmpty(str5))
                            {
                                DateTime time;
                                DateTime.TryParse(str5, out time);
                                str5 = time.ToString("yyyy-MM-dd");
                                list7.Add(new FieldValueEntity<String>(str5, str4));
                            }
                            else
                            {
                                list2.Add(str4);
                            }
                        }
                        break;

                    case DataType.Decimal:
                    case DataType.Double:
                    case DataType.Float:
                    {
                        list5 = new List<FieldValueEntity<double>>(stockCodeList.Count);
                        double indicatorUnit = this.GetIndicatorUnit(indicatorEntityByCustomerId);
                        foreach (String str6 in stockCodeList)
                        {
                            double num3 = this.GetIndicatorDoubleValue(str6, indicatorId.ToString(), indicatorUnit);
                            if (num3 == double.MinValue)
                            {
                                list2.Add(str6);
                            }
                            else
                            {
                                list5.Add(new FieldValueEntity<double>(num3, str6));
                            }
                        }
                        break;
                    }
                    case DataType.Int:
                    case DataType.Long:
                    case DataType.UInt:
                    case DataType.ULong:
                        list6 = new List<FieldValueEntity<long>>(stockCodeList.Count);
                        foreach (String str7 in stockCodeList)
                        {
                            long num4 = this.GetIndicatorInt64Value(str7, indicatorId.ToString());
                            if (num4 == -9223372036854775808L)
                            {
                                list2.Add(str7);
                            }
                            else
                            {
                                list6.Add(new FieldValueEntity<long>(num4, str7));
                            }
                        }
                        break;
                }
                if (list3 != null)
                {
                    list3.Sort();
                }
                else if (list4 != null)
                {
                    list4.Sort();
                }
                else if (list5 != null)
                {
                    list5.Sort();
                }
                else if (list6 != null)
                {
                    list6.Sort();
                }
                else if (list7 != null)
                {
                    list7.Sort();
                }
                if (sortMode == CommonEnumerators.SortMode.Desc)
                {
                    if (list3 != null)
                    {
                        for (int i = list3.Count - 1; i >= 0; i--)
                        {
                            list.Add(list3[i].FieldCode);
                        }
                    }
                    else if (list4 != null)
                    {
                        for (int j = list4.Count - 1; j >= 0; j--)
                        {
                            list.Add(list4[j].FieldCode);
                        }
                    }
                    else if (list5 != null)
                    {
                        for (int k = list5.Count - 1; k >= 0; k--)
                        {
                            list.Add(list5[k].FieldCode);
                        }
                    }
                    else if (list6 != null)
                    {
                        for (int m = list6.Count - 1; m >= 0; m--)
                        {
                            list.Add(list6[m].FieldCode);
                        }
                    }
                    else if (list7 != null)
                    {
                        for (int n = list7.Count - 1; n >= 0; n--)
                        {
                            list.Add(list7[n].FieldCode);
                        }
                    }
                }
                else if (list3 != null)
                {
                    for (int num10 = 0; num10 < list3.Count; num10++)
                    {
                        list.Add(list3[num10].FieldCode);
                    }
                }
                else if (list4 != null)
                {
                    for (int num11 = 0; num11 < list4.Count; num11++)
                    {
                        list.Add(list4[num11].FieldCode);
                    }
                }
                else if (list5 != null)
                {
                    for (int num12 = 0; num12 < list5.Count; num12++)
                    {
                        list.Add(list5[num12].FieldCode);
                    }
                }
                else if (list6 != null)
                {
                    for (int num13 = 0; num13 < list6.Count; num13++)
                    {
                        list.Add(list6[num13].FieldCode);
                    }
                }
                else if (list7 != null)
                {
                    for (int num14 = 0; num14 < list7.Count; num14++)
                    {
                        list.Add(list7[num14].FieldCode);
                    }
                }
                if (list3 != null)
                {
                    list3.Clear();
                }
                else if (list4 != null)
                {
                    list4.Clear();
                }
                else if (list5 != null)
                {
                    list5.Clear();
                }
                else if (list6 != null)
                {
                    list6.Clear();
                }
                else if (list7 != null)
                {
                    list7.Clear();
                }
            }
            catch (Exception exception)
            {
                //LogUtility.LogTableMessage("数据排序异常||" + exception.ToString());
                list = stockCodeList;
            }
            list.AddRange(list2.ToArray());
            return list;
        }

        public void SortSeriesData(SeriesDataPacket seriesDataPacket, String stockCode, int customerId, CommonEnumerators.SeriesDataSortType seriesDataType, CommonEnumerators.SortMode sortMode)
        {
            if (seriesDataPacket != null)
            {
                List<KeyValuePair<DateTime, double>> list = new List<KeyValuePair<DateTime, double>>();
                try
                {
                    int num;
                    FinanceSeriesDataPacket packet2;
                    switch (seriesDataType)
                    {
                        case CommonEnumerators.SeriesDataSortType.Date:
                            num = 0;
                            goto Label_0052;

                        case CommonEnumerators.SeriesDataSortType.Value:
                        {
                            List<double> list2 = seriesDataPacket.GetValueList(stockCode, customerId, seriesDataPacket.DateList);
                            for (int i = 0; i < list2.Count; i++)
                            {
                                list.Add(new KeyValuePair<DateTime, double>(seriesDataPacket.DateList[i], list2[i]));
                            }
                            if (sortMode == CommonEnumerators.SortMode.Asc)
                            {
                                list.Sort(delegate (KeyValuePair<DateTime, double> x, KeyValuePair<DateTime, double> y) {
                                    int num2 = x.Value.CompareTo(y.Value);
                                    if (num2 != 0)
                                    {
                                        return num2;
                                    }
                                    return x.Key.CompareTo(y.Key);
                                });
                            }
                            else
                            {
                                list.Sort(delegate (KeyValuePair<DateTime, double> x, KeyValuePair<DateTime, double> y) {
                                    int num2 = x.Value.CompareTo(y.Value) * -1;
                                    if (num2 != 0)
                                    {
                                        return num2;
                                    }
                                    return x.Key.CompareTo(y.Key) * -1;
                                });
                            }
                            seriesDataPacket.DateList.Clear();
                            foreach (KeyValuePair<DateTime, double> pair2 in list)
                            {
                                seriesDataPacket.DateList.Add(pair2.Key);
                            }
                            return;
                        }
                        case CommonEnumerators.SeriesDataSortType.Same:
                            goto Label_02BE;

                        case CommonEnumerators.SeriesDataSortType.Cycle:
                        {
                            FinanceSeriesDataPacket packet = seriesDataPacket as FinanceSeriesDataPacket;
                            if (packet != null)
                            {
                                List<double> huanCompareList = packet.GetHuanCompareList(stockCode, customerId);
                                for (int j = 0; j < huanCompareList.Count; j++)
                                {
                                    list.Add(new KeyValuePair<DateTime, double>(seriesDataPacket.DateList[j], huanCompareList[j]));
                                }
                                if (sortMode == CommonEnumerators.SortMode.Asc)
                                {
                                    list.Sort(delegate (KeyValuePair<DateTime, double> x, KeyValuePair<DateTime, double> y) {
                                        int num2 = x.Value.CompareTo(y.Value);
                                        if (num2 != 0)
                                        {
                                            return num2;
                                        }
                                        return x.Key.CompareTo(y.Key);
                                    });
                                }
                                else
                                {
                                    list.Sort(delegate (KeyValuePair<DateTime, double> x, KeyValuePair<DateTime, double> y) {
                                        int num2 = x.Value.CompareTo(y.Value) * -1;
                                        if (num2 != 0)
                                        {
                                            return num2;
                                        }
                                        return x.Key.CompareTo(y.Key) * -1;
                                    });
                                }
                                seriesDataPacket.DateList.Clear();
                                foreach (KeyValuePair<DateTime, double> pair3 in list)
                                {
                                    seriesDataPacket.DateList.Add(pair3.Key);
                                }
                            }
                            return;
                        }
                        default:
                            return;
                    }
                Label_002E:
                    list.Add(new KeyValuePair<DateTime, double>(seriesDataPacket.DateList[num], 1.0));
                    num++;
                Label_0052:
                    if (num < seriesDataPacket.DateList.Count)
                    {
                        goto Label_002E;
                    }
                    if (sortMode == CommonEnumerators.SortMode.Asc)
                    {
                        list.Sort(delegate (KeyValuePair<DateTime, double> x, KeyValuePair<DateTime, double> y) {
                            return x.Key.CompareTo(y.Key);
                        });
                    }
                    else
                    {
                        list.Sort(delegate (KeyValuePair<DateTime, double> x, KeyValuePair<DateTime, double> y) {
                            return x.Key.CompareTo(y.Key) * -1;
                        });
                    }
                    seriesDataPacket.DateList.Clear();
                    foreach (KeyValuePair<DateTime, double> pair in list)
                    {
                        seriesDataPacket.DateList.Add(pair.Key);
                    }
                    return;
                Label_02BE:
                    packet2 = seriesDataPacket as FinanceSeriesDataPacket;
                    if (packet2 != null)
                    {
                        List<double> sameCompareList = packet2.GetSameCompareList(stockCode, customerId);
                        for (int k = 0; k < sameCompareList.Count; k++)
                        {
                            list.Add(new KeyValuePair<DateTime, double>(seriesDataPacket.DateList[k], sameCompareList[k]));
                        }
                        if (sortMode == CommonEnumerators.SortMode.Asc)
                        {
                            list.Sort(delegate (KeyValuePair<DateTime, double> x, KeyValuePair<DateTime, double> y) {
                                int num2 = x.Value.CompareTo(y.Value);
                                if (num2 != 0)
                                {
                                    return num2;
                                }
                                return x.Key.CompareTo(y.Key);
                            });
                        }
                        else
                        {
                            list.Sort(delegate (KeyValuePair<DateTime, double> x, KeyValuePair<DateTime, double> y) {
                                int num2 = x.Value.CompareTo(y.Value) * -1;
                                if (num2 != 0)
                                {
                                    return num2;
                                }
                                return x.Key.CompareTo(y.Key) * -1;
                            });
                        }
                        seriesDataPacket.DateList.Clear();
                        foreach (KeyValuePair<DateTime, double> pair4 in list)
                        {
                            seriesDataPacket.DateList.Add(pair4.Key);
                        }
                    }
                }
                catch (Exception exception)
                {
                    //LogUtility.LogTableMessage("序列数据排序异常," + exception);
                }
            }
        }

        public DataTable SortStatisticWithGroup(DataTable sourceTable, int sortCustomerId, CommonEnumerators.SortMode mode)
        {
            try
            {
                DataTable table = new DataTable();
                foreach (DataColumn column in sourceTable.Columns)
                {
                    table.Columns.Add(column.ColumnName, column.ColumnName.Equals("group") ? typeof(String) : typeof(double));
                }
                foreach (DataRow row in sourceTable.Rows)
                {
                    DataRow row2 = table.NewRow();
                    foreach (DataColumn column2 in sourceTable.Columns)
                    {
                        if (column2.ColumnName.Equals("group"))
                        {
                            row2["group"] = row[column2];
                            continue;
                        }
                        if ((row[column2] == DBNull.Value) || String.IsNullOrEmpty(row[column2].ToString()))
                        {
                            row2[column2.ColumnName] = DBNull.Value;
                            continue;
                        }
                        row2[column2.ColumnName] = double.Parse(row[column2].ToString());
                    }
                    table.Rows.Add(row2);
                }
                DataView defaultView = table.DefaultView;
                defaultView.Sort = String.Format("{0} {1}", sortCustomerId, (mode == CommonEnumerators.SortMode.Asc) ? "ASC" : "DESC");
                return defaultView.ToTable();
            }
            catch (Exception exception)
            {
                //LogUtility.LogTableMessage("数据中心||分组统计排序异常 || " + exception);
                return sourceTable;
            }
        }

        private List<NodeData> SplitBlockTree(String blockContent, BlockCatRequestEntity requestEntity)
        {
            List<NodeData> list = new List<NodeData>();
            try
            {
                bool flag = true;
                if ((requestEntity.CategoryList == null) || (requestEntity.CategoryList.Count == 0))
                {
                    flag = false;
                }
                if (requestEntity.Browser == BrowserType.New3Board)
                {
                    NodeData item = new NodeData();
                    item.Id = "1";
                    item.ParentId = "0";
                    item.Name = "三板股票";
                    list.Add(item);
                }
                String[] strArray = blockContent.Split(new String[] { "$", "}" }, StringSplitOptions.None);
                int index = 0;
                while (index < strArray.Length)
                {
                    NodeData data2 = new NodeData();
                    data2.Id = strArray[index];
                    index++;
                    if (index >= strArray.Length)
                    {
                        return list;
                    }
                    data2.Name = strArray[index];
                    index++;
                    if (index >= strArray.Length)
                    {
                        return list;
                    }
                    data2.ParentId = strArray[index];
                    index++;
                    if (index >= strArray.Length)
                    {
                        return list;
                    }
                    String str = strArray[index];
                    index++;
                    if (index >= strArray.Length)
                    {
                        return list;
                    }
                    index++;
                    if (index >= strArray.Length)
                    {
                        return list;
                    }
                    index++;
                    if (index >= strArray.Length)
                    {
                        return list;
                    }
                    int result = 0;
                    int.TryParse(strArray[index], out result);
                    data2.SortIndex = result;
                    index++;
                    if (index >= strArray.Length)
                    {
                        return list;
                    }
                    data2.IsCatalog = false;
                    index++;
                    if ((((requestEntity.Browser != BrowserType.New3Board) || data2.Id.StartsWith("014")) || ((data2.Id.StartsWith("017") || data2.Id.StartsWith("018")) || data2.Id.StartsWith("019"))) && ((requestEntity.Browser != BrowserType.Option) || !data2.Id.StartsWith("124")))
                    {
                        if (!flag)
                        {
                            list.Add(data2);
                        }
                        else if (requestEntity.CategoryList.Contains(str))
                        {
                            list.Add(data2);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                //LogUtility.LogTableMessage("数据中心|板块树转换【SplitBlockTree】异常," + exception.ToString());
            }
            return list;
        }

        private List<NodeData> SplitDynamicBlockTree(String blockContent)
        {
            List<NodeData> list = new List<NodeData>();
            try
            {
                bool flag = false;
                String[] strArray = blockContent.Split(new String[] { "$", "}" }, StringSplitOptions.None);
                int index = 0;
                while (index < strArray.Length)
                {
                    NodeData data = new NodeData();
                    data.Id = strArray[index];
                    if (data.Id.Equals("-1000.D"))
                    {
                        flag = true;
                    }
                    index++;
                    if (index >= strArray.Length)
                    {
                        break;
                    }
                    data.Name = strArray[index];
                    index++;
                    if (index >= strArray.Length)
                    {
                        break;
                    }
                    data.ParentId = strArray[index];
                    index++;
                    if (index >= strArray.Length)
                    {
                        break;
                    }
                    String text1 = strArray[index];
                    index++;
                    if (index >= strArray.Length)
                    {
                        break;
                    }
                    index++;
                    if (index >= strArray.Length)
                    {
                        break;
                    }
                    index++;
                    if (index >= strArray.Length)
                    {
                        break;
                    }
                    int result = 0;
                    int.TryParse(strArray[index], out result);
                    data.SortIndex = result;
                    index++;
                    if (index >= strArray.Length)
                    {
                        break;
                    }
                    data.IsCatalog = false;
                    index++;
                    list.Add(data);
                }
                if (flag)
                {
                    return list;
                }
                NodeData item = new NodeData();
                item.Id = "-1000.D";
                item.ParentId = "0";
                item.Name = "动态自选股";
                item.IsCatalog = false;
                item.SortIndex = 0x63;
                if (list.Count > 0)
                {
                    item.HasLeaf = true;
                }
                list.Add(item);
            }
            catch (Exception exception)
            {
                //LogUtility.LogTableMessage("数据中心|板块树转换【SplitDynamicBlockTree】异常," + exception.ToString());
            }
            return list;
        }

        private List<NodeData> SplitUserBlockTree(String blockContent)
        {
            List<NodeData> list = new List<NodeData>();
            try
            {
                bool flag = false;
                String[] strArray = blockContent.Split(new String[] { "$", "}" }, StringSplitOptions.None);
                int index = 0;
                while (index < strArray.Length)
                {
                    NodeData data = new NodeData();
                    data.Id = strArray[index];
                    if (data.Id.Equals("-100.U"))
                    {
                        flag = true;
                    }
                    index++;
                    if (index >= strArray.Length)
                    {
                        break;
                    }
                    data.Name = strArray[index];
                    index++;
                    if (index >= strArray.Length)
                    {
                        break;
                    }
                    data.ParentId = strArray[index];
                    index++;
                    if (index >= strArray.Length)
                    {
                        break;
                    }
                    String text1 = strArray[index];
                    index++;
                    if (index >= strArray.Length)
                    {
                        break;
                    }
                    index++;
                    if (index >= strArray.Length)
                    {
                        break;
                    }
                    index++;
                    if (index >= strArray.Length)
                    {
                        break;
                    }
                    int result = 0;
                    int.TryParse(strArray[index], out result);
                    data.SortIndex = result;
                    index++;
                    if (index >= strArray.Length)
                    {
                        break;
                    }
                    data.IsCatalog = false;
                    index++;
                    list.Add(data);
                }
                if (flag)
                {
                    return list;
                }
                NodeData item = new NodeData();
                item.Id = "-100.U";
                item.ParentId = "0";
                item.Name = "自选股";
                item.IsCatalog = false;
                item.SortIndex = 100;
                if (list.Count > 0)
                {
                    item.HasLeaf = true;
                }
                list.Add(item);
            }
            catch (Exception exception)
            {
                //LogUtility.LogTableMessage("数据中心|板块树转换【SplitBlockTree】异常," + exception.ToString());
            }
            return list;
        }

        public DataTable StatisticWithGroup(List<StockEntity> stockList, List<int> indicatorCustomerIdList, GroupStatisEntity groupEntity)
        {
            DataTable table = new DataTable();
            try
            {
                Dictionary<String, List<StockEntity>> dictionary = new Dictionary<String, List<StockEntity>>();
                if (groupEntity.StatisticType == CommonEnumerators.StatisticDataType.Text)
                {
                    foreach (String str in groupEntity.GroupTextList)
                    {
                        dictionary[str] = new List<StockEntity>();
                    }
                }
                else
                {
                    foreach (RangeEntity entity in groupEntity.GroupEntityList)
                    {
                        dictionary[entity.ToString()] = new List<StockEntity>();
                    }
                }
                table.Columns.Add("group", typeof(String));
                foreach (int num in indicatorCustomerIdList)
                {
                    table.Columns.Add(num.ToString(), typeof(String));
                }
                IndicatorEntity indicatorEntityByCustomerId = this.GetIndicatorEntityByCustomerId(groupEntity.GroupIndicatorId);
                String customIndicatorId = groupEntity.GroupIndicatorId.ToString();
                double indicatorUnit = this.GetIndicatorUnit(indicatorEntityByCustomerId);
                switch (indicatorEntityByCustomerId.IndDataType)
                {
                    case DataType.Bool:
                    case DataType.Byte:
                    case DataType.Short:
                    case DataType.UShort:
                        foreach (RangeEntity entity3 in groupEntity.GroupEntityList)
                        {
                            if (!entity3.IsOther)
                            {
                                double num3 = double.Parse(entity3.Start.ToString());
                                double num4 = double.Parse(entity3.End.ToString());
                                foreach (StockEntity entity4 in stockList)
                                {
                                    double num5 = this.GetIndicatorInt32Value(entity4.StockCode, customIndicatorId, indicatorUnit);
                                    if ((num5 >= num3) && (num5 < num4))
                                    {
                                        dictionary[entity3.ToString()].Add(entity4);
                                    }
                                }
                                continue;
                            }
                        }
                        break;

                    case DataType.Char:
                    case DataType.String:
                        foreach (String str3 in groupEntity.GroupTextList)
                        {
                            foreach (StockEntity entity9 in stockList)
                            {
                                String indicatorStringValue = this.GetIndicatorStringValue(entity9.StockCode, customIndicatorId);
                                if (str3.Equals(indicatorStringValue))
                                {
                                    dictionary[str3].Add(entity9);
                                }
                            }
                        }
                        break;

                    case DataType.UshortDate:
                    case DataType.DateTime:
                        foreach (RangeEntity entity10 in groupEntity.GroupEntityList)
                        {
                            if (!entity10.IsOther)
                            {
                                String strB = entity10.Start.ToString();
                                String str6 = entity10.End.ToString();
                                foreach (StockEntity entity11 in stockList)
                                {
                                    String str7 = this.GetIndicatorStringValue(entity11.StockCode, customIndicatorId);
                                    if ((str7.CompareTo(strB) >= 0) && (str7.CompareTo(str6) < 0))
                                    {
                                        dictionary[entity10.ToString()].Add(entity11);
                                    }
                                }
                                continue;
                            }
                        }
                        break;

                    case DataType.Decimal:
                    case DataType.Double:
                    case DataType.Float:
                        foreach (RangeEntity entity7 in groupEntity.GroupEntityList)
                        {
                            if (!entity7.IsOther)
                            {
                                double num9 = double.Parse(entity7.Start.ToString());
                                double num10 = double.Parse(entity7.End.ToString());
                                foreach (StockEntity entity8 in stockList)
                                {
                                    double num11 = this.GetIndicatorDoubleValue(entity8.StockCode, customIndicatorId, indicatorUnit);
                                    if ((num11 >= num9) && (num11 < num10))
                                    {
                                        dictionary[entity7.ToString()].Add(entity8);
                                    }
                                }
                                continue;
                            }
                        }
                        break;

                    case DataType.Int:
                    case DataType.Long:
                    case DataType.UInt:
                    case DataType.ULong:
                        foreach (RangeEntity entity5 in groupEntity.GroupEntityList)
                        {
                            if (!entity5.IsOther)
                            {
                                double num6 = double.Parse(entity5.Start.ToString());
                                double num7 = double.Parse(entity5.End.ToString());
                                foreach (StockEntity entity6 in stockList)
                                {
                                    double num8 = this.GetIndicatorInt64Value(entity6.StockCode, customIndicatorId, indicatorUnit);
                                    if ((num8 >= num6) && (num8 < num7))
                                    {
                                        dictionary[entity5.ToString()].Add(entity6);
                                    }
                                }
                                continue;
                            }
                        }
                        break;
                }
                foreach (KeyValuePair<String, List<StockEntity>> pair in dictionary)
                {
                    DataRow row = table.NewRow();
                    row["group"] = pair.Key;
                    if (groupEntity.CalcType == CommonEnumerators.CalculateType.Count)
                    {
                        foreach (int num12 in indicatorCustomerIdList)
                        {
                            row[num12.ToString()] = dictionary[pair.Key].Count.ToString();
                        }
                        table.Rows.Add(row);
                        continue;
                    }
                    List<String> stockCodeList = new List<String>();
                    foreach (StockEntity entity12 in pair.Value)
                    {
                        stockCodeList.Add(entity12.StockCode);
                    }
                    foreach (int num13 in indicatorCustomerIdList)
                    {
                        if (stockCodeList.Count == 0)
                        {
                            row[num13.ToString()] = "0";
                        }
                        List<String> list2 = null;
                        switch (groupEntity.CalcType)
                        {
                            case CommonEnumerators.CalculateType.Sum:
                            {
                                list2 = this.Statitcal(stockCodeList, num13, 0, true);
                                row[num13.ToString()] = list2[0];
                                continue;
                            }
                            case CommonEnumerators.CalculateType.Count:
                            {
                                continue;
                            }
                            case CommonEnumerators.CalculateType.Average:
                            {
                                list2 = this.Statitcal(stockCodeList, num13, 0, true);
                                row[num13.ToString()] = list2[2];
                                continue;
                            }
                            case CommonEnumerators.CalculateType.RightAverage:
                            {
                                list2 = this.Statitcal(stockCodeList, num13, groupEntity.RightIndicatorId, true);
                                row[num13.ToString()] = list2[3];
                                continue;
                            }
                        }
                    }
                    table.Rows.Add(row);
                }
                return table;
            }
            catch (Exception exception)
            {
                //LogUtility.LogTableMessage("数据中心||分组统计异常 || " + exception.ToString());
            }
            return table;
        }

        public Dictionary<int, List<String>> Statitcal(List<String> stockCodeList, List<int> indicatorIdList, int weightedIndicatorId)
        {
            Dictionary<int, List<String>> dictionary = new Dictionary<int, List<String>>();
            foreach (int num in indicatorIdList)
            {
                dictionary[num] = this.Statitcal(stockCodeList, num, weightedIndicatorId, false);
            }
            return dictionary;
        }

        public List<String> Statitcal(List<String> stockCodeList, int indicatorId, int weightedIndicatorId, [Optional, DefaultParameterValue(false)] bool isGroupStatical)
        {
            List<String> list = new List<String>();
            try
            {
                IndicatorEntity indicatorEntityByCustomerId = this.GetIndicatorEntityByCustomerId(indicatorId);
                double indicatorUnit = CreateInstance().GetIndicatorUnit(indicatorId);
                IndicatorEntity indicator = this.GetIndicatorEntityByCustomerId(weightedIndicatorId);
                double weightedUnitValue = CreateInstance().GetIndicatorUnit(weightedIndicatorId);
                bool flag = (indicatorEntityByCustomerId != null) && (indicatorEntityByCustomerId.CustomIndicator != null);
                bool flag2 = (indicator != null) && (indicator.CustomIndicator != null);
                Dictionary<String, double> dictionary = new Dictionary<String, double>();
                Dictionary<String, double> customerIndicatorData = new Dictionary<String, double>();
                if (flag2)
                {
                    customerIndicatorData = CustomerIndicatorDataHelper.Intance.GetCustomerIndicatorData(indicator, stockCodeList);
                }
                String str = indicatorEntityByCustomerId.ShowFormat ?? String.Empty;
                String indicatorFormat = str;
                if (String.IsNullOrEmpty(indicatorFormat) || (indicatorFormat == "null"))
                {
                    indicatorFormat = this.GetIndicatorFormat(indicatorEntityByCustomerId.IndDataType);
                }
                else
                {
                    FormatType type = JSONHelper.DeserializeObject<FormatType>(str);
                    if (type != null)
                    {
                        indicatorFormat = type.Value;
                    }
                }
                double num3 = 0.0;
                double num4 = 0.0;
                double num5 = 0.0;
                double item = 0.0;
                double num7 = 0.0;
                double num8 = 0.0;
                double num9 = 0.0;
                double num10 = 0.0;
                double num11 = 0.0;
                List<double> list2 = new List<double>();
                int num12 = 0;
                switch (indicatorEntityByCustomerId.IndDataType)
                {
                    case DataType.Byte:
                    case DataType.Int:
                    case DataType.Long:
                    case DataType.Short:
                    case DataType.UInt:
                    case DataType.ULong:
                    case DataType.UShort:
                        if (flag)
                        {
                            dictionary = CustomerIndicatorDataHelper.Intance.GetCustomerIndicatorData(indicatorEntityByCustomerId, stockCodeList);
                        }
                        foreach (String str3 in stockCodeList)
                        {
                            if (flag)
                            {
                                if (dictionary[str3] == double.MinValue)
                                {
                                    continue;
                                }
                                item = dictionary[str3];
                            }
                            else
                            {
                                item = this.GetIndicatorInt64Value(str3, indicatorId.ToString(), indicatorUnit);
                            }
                            if (item != double.MinValue)
                            {
                                num3 += item;
                                list2.Add(item);
                                if (weightedIndicatorId > 0)
                                {
                                    if (flag2)
                                    {
                                        num9 = customerIndicatorData[str3];
                                    }
                                    else
                                    {
                                        num9 = this.GetWeightedIndicatorValue(indicator, str3, weightedIndicatorId, weightedUnitValue);
                                    }
                                    if (num9 != double.MinValue)
                                    {
                                        num11 += item * num9;
                                        num10 += num9;
                                    }
                                }
                            }
                        }
                        goto Label_03E9;

                    case DataType.Decimal:
                    case DataType.Double:
                    case DataType.Float:
                        break;

                    default:
                    {
                        Dictionary<String, String> customerIndicatorDataString = new Dictionary<String, String>(0);
                        if (flag)
                        {
                            customerIndicatorDataString = CustomerIndicatorDataHelper.Intance.GetCustomerIndicatorDataString(indicatorEntityByCustomerId, stockCodeList);
                        }
                        foreach (String str5 in stockCodeList)
                        {
                            if (flag)
                            {
                                if (customerIndicatorDataString.ContainsKey(str5) && !String.IsNullOrEmpty(customerIndicatorDataString[str5]))
                                {
                                    num12++;
                                }
                            }
                            else
                            {
                                item = this.GetIndicatorFloatValue(str5, indicatorId.ToString(), indicatorUnit);
                                if (!String.IsNullOrEmpty(this.GetIndicatorStringValue(str5, indicatorId.ToString())))
                                {
                                    num12++;
                                }
                            }
                        }
                        list = new List<String>();
                        goto Label_03E9;
                    }
                }
                if (flag)
                {
                    dictionary = CustomerIndicatorDataHelper.Intance.GetCustomerIndicatorData(indicatorEntityByCustomerId, stockCodeList);
                }
                foreach (String str4 in stockCodeList)
                {
                    if (flag)
                    {
                        if (dictionary[str4] == double.MinValue)
                        {
                            continue;
                        }
                        item = dictionary[str4];
                    }
                    else
                    {
                        item = this.GetIndicatorDoubleValue(str4, indicatorId.ToString(), indicatorUnit);
                    }
                    if (item != double.MinValue)
                    {
                        num3 += item;
                        list2.Add(item);
                        if (weightedIndicatorId > 0)
                        {
                            if (flag2)
                            {
                                num9 = customerIndicatorData[str4];
                            }
                            else
                            {
                                num9 = this.GetWeightedIndicatorValue(indicator, str4, weightedIndicatorId, weightedUnitValue);
                            }
                            if (num9 != double.MinValue)
                            {
                                num11 += item * num9;
                                num10 += num9;
                            }
                        }
                    }
                }
            Label_03E9:
                if (list2.Count > 0)
                {
                    num7 = num3 / ((double) list2.Count);
                    list2.Sort();
                    foreach (double num13 in list2)
                    {
                        if (num4 == 0.0)
                        {
                            num4 = num13;
                        }
                        if (num5 == 0.0)
                        {
                            num5 = num13;
                        }
                        if (num4 < num13)
                        {
                            num4 = num13;
                        }
                        if (num5 > num13)
                        {
                            num5 = num13;
                        }
                        num8 += Math.Pow(num13 - num7, 2.0);
                    }
                    int num14 = list2.Count / 2;
                    double num15 = 0.0;
                    if (list2.Count > 0)
                    {
                        if (list2.Count == 1)
                        {
                            num15 = list2[0];
                        }
                        else if ((list2.Count % 2) == 0)
                        {
                            num15 = list2[num14 - 1];
                        }
                        else
                        {
                            num15 = (list2[num14] + list2[num14 - 1]) / 2.0;
                        }
                    }
                    double d = (list2.Count == 1) ? 0.0 : (num8 / ((double) (list2.Count - 1)));
                    double num17 = num8 / ((double) list2.Count);
                    double num18 = 0.0;
                    if (num10 > 0.0)
                    {
                        num18 = num11 / num10;
                    }
                    if (((indicatorUnit != 0.0) && (indicatorUnit != 1.0)) && (!flag && String.IsNullOrEmpty(indicatorFormat)))
                    {
                        indicatorFormat = "#,##0.0000";
                    }
                    if (isGroupStatical)
                    {
                        indicatorFormat = "##0.0000";
                        list.Add(num3.ToString(indicatorFormat));
                        list.Add(list2.Count.ToString());
                        list.Add(num7.ToString(indicatorFormat));
                        list.Add(num18.ToString(indicatorFormat));
                        return list;
                    }
                    list.Add(num3.ToString(indicatorFormat));
                    list.Add(num4.ToString(indicatorFormat));
                    list.Add(num5.ToString(indicatorFormat));
                    list.Add(list2.Count.ToString());
                    list.Add(num7.ToString(indicatorFormat));
                    list.Add(num15.ToString(indicatorFormat));
                    list.Add(d.ToString(indicatorFormat));
                    list.Add(Math.Sqrt(d).ToString(indicatorFormat));
                    list.Add(num17.ToString(indicatorFormat));
                    list.Add(Math.Sqrt(num17).ToString(indicatorFormat));
                    list.Add(num18.ToString(indicatorFormat));
                    list.Add(num7.ToString());
                    return list;
                }
                if ((indicatorEntityByCustomerId.IndDataType == DataType.DateTime) || (indicatorEntityByCustomerId.IndDataType == DataType.String))
                {
                    if (isGroupStatical)
                    {
                        list.Add("");
                        list.Add(num12.ToString());
                        list.Add("");
                        list.Add("");
                        return list;
                    }
                    list.Add("");
                    list.Add("");
                    list.Add("");
                    list.Add(num12.ToString());
                    list.Add("");
                    list.Add("");
                    list.Add("");
                    list.Add("");
                    list.Add("");
                    list.Add("");
                    list.Add("");
                    list.Add("");
                    return list;
                }
                if (isGroupStatical)
                {
                    indicatorFormat = "##0.0000";
                    list.Add(num3.ToString(indicatorFormat));
                    list.Add(num12.ToString());
                    list.Add(num7.ToString(indicatorFormat));
                    list.Add(0.ToString(indicatorFormat));
                    return list;
                }
                list.Add(num3.ToString(indicatorFormat));
                list.Add(num4.ToString(indicatorFormat));
                list.Add(num5.ToString(indicatorFormat));
                list.Add(num12.ToString());
                list.Add(num7.ToString(indicatorFormat));
                list.Add(0.ToString(indicatorFormat));
                list.Add(0.ToString(indicatorFormat));
                list.Add(0.ToString(indicatorFormat));
                list.Add(0.ToString(indicatorFormat));
                list.Add(0.ToString(indicatorFormat));
                list.Add(0.ToString(indicatorFormat));
                list.Add(0.ToString());
            }
            catch (Exception exception)
            {
                //LogUtility.LogTableMessage(String.Concat(new object[] { "数据中心|customerIndicatorId=", indicatorId, " 统计异常,", exception.ToString() }));
            }
            return list;
        }

        public List<String> StatitcalSingle(List<String> stockCodeList, int indicatorId, int weightedIndicatorId)
        {
            List<String> list = new List<String>();
            try
            {
                IndicatorEntity indicatorEntityByCustomerId = this.GetIndicatorEntityByCustomerId(indicatorId);
                double indicatorUnit = CreateInstance().GetIndicatorUnit(indicatorId);
                IndicatorEntity indicator = this.GetIndicatorEntityByCustomerId(weightedIndicatorId);
                double weightedUnitValue = CreateInstance().GetIndicatorUnit(weightedIndicatorId);
                bool flag = (indicatorEntityByCustomerId != null) && (indicatorEntityByCustomerId.CustomIndicator != null);
                bool flag2 = (indicator != null) && (indicator.CustomIndicator != null);
                Dictionary<String, double> dictionary = new Dictionary<String, double>();
                Dictionary<String, double> customerIndicatorData = new Dictionary<String, double>();
                if (flag2)
                {
                    customerIndicatorData = CustomerIndicatorDataHelper.Intance.GetCustomerIndicatorData(indicator, stockCodeList);
                }
                String str = indicatorEntityByCustomerId.ShowFormat ?? String.Empty;
                String indicatorFormat = str;
                if (String.IsNullOrEmpty(indicatorFormat) || (indicatorFormat == "null"))
                {
                    indicatorFormat = this.GetIndicatorFormat(indicatorEntityByCustomerId.IndDataType);
                }
                else
                {
                    FormatType type = JSONHelper.DeserializeObject<FormatType>(str);
                    if (type != null)
                    {
                        indicatorFormat = type.Value;
                    }
                }
                double num3 = 0.0;
                double num4 = 0.0;
                double num5 = 0.0;
                double item = 0.0;
                double num7 = 0.0;
                double num8 = 0.0;
                double num9 = 0.0;
                double num10 = 0.0;
                double num11 = 0.0;
                List<double> list2 = new List<double>();
                int num12 = 0;
                switch (indicatorEntityByCustomerId.IndDataType)
                {
                    case DataType.Byte:
                    case DataType.Int:
                    case DataType.Long:
                    case DataType.Short:
                    case DataType.UInt:
                    case DataType.ULong:
                    case DataType.UShort:
                        if (flag)
                        {
                            dictionary = CustomerIndicatorDataHelper.Intance.GetCustomerIndicatorData(indicatorEntityByCustomerId, stockCodeList);
                        }
                        foreach (String str3 in stockCodeList)
                        {
                            if (flag)
                            {
                                if (dictionary[str3] == double.MinValue)
                                {
                                    continue;
                                }
                                item = dictionary[str3];
                            }
                            else
                            {
                                item = this.GetIndicatorInt64Value(str3, indicatorId.ToString(), indicatorUnit);
                            }
                            if (item != double.MinValue)
                            {
                                num3 += item;
                                list2.Add(item);
                                if (weightedIndicatorId > 0)
                                {
                                    if (flag2)
                                    {
                                        num9 = customerIndicatorData[str3];
                                    }
                                    else
                                    {
                                        num9 = this.GetWeightedIndicatorValue(indicator, str3, weightedIndicatorId, weightedUnitValue);
                                    }
                                    if (num9 != double.MinValue)
                                    {
                                        num11 += item * num9;
                                        num10 += num9;
                                    }
                                }
                            }
                        }
                        goto Label_03E9;

                    case DataType.Decimal:
                    case DataType.Double:
                    case DataType.Float:
                        break;

                    default:
                    {
                        Dictionary<String, String> customerIndicatorDataString = new Dictionary<String, String>(0);
                        if (flag)
                        {
                            customerIndicatorDataString = CustomerIndicatorDataHelper.Intance.GetCustomerIndicatorDataString(indicatorEntityByCustomerId, stockCodeList);
                        }
                        foreach (String str5 in stockCodeList)
                        {
                            if (flag)
                            {
                                if (customerIndicatorDataString.ContainsKey(str5) && !String.IsNullOrEmpty(customerIndicatorDataString[str5]))
                                {
                                    num12++;
                                }
                            }
                            else
                            {
                                item = this.GetIndicatorFloatValue(str5, indicatorId.ToString(), indicatorUnit);
                                if (!String.IsNullOrEmpty(this.GetIndicatorStringValue(str5, indicatorId.ToString())))
                                {
                                    num12++;
                                }
                            }
                        }
                        list = new List<String>();
                        goto Label_03E9;
                    }
                }
                if (flag)
                {
                    dictionary = CustomerIndicatorDataHelper.Intance.GetCustomerIndicatorData(indicatorEntityByCustomerId, stockCodeList);
                }
                foreach (String str4 in stockCodeList)
                {
                    if (flag)
                    {
                        if (dictionary[str4] == double.MinValue)
                        {
                            continue;
                        }
                        item = dictionary[str4];
                    }
                    else
                    {
                        item = this.GetIndicatorDoubleValue(str4, indicatorId.ToString(), indicatorUnit);
                    }
                    if (item != double.MinValue)
                    {
                        num3 += item;
                        list2.Add(item);
                        if (weightedIndicatorId > 0)
                        {
                            if (flag2)
                            {
                                num9 = customerIndicatorData[str4];
                            }
                            else
                            {
                                num9 = this.GetWeightedIndicatorValue(indicator, str4, weightedIndicatorId, weightedUnitValue);
                            }
                            if (num9 != double.MinValue)
                            {
                                num11 += item * num9;
                                num10 += num9;
                            }
                        }
                    }
                }
            Label_03E9:
                if (list2.Count > 0)
                {
                    num7 = num3 / ((double) list2.Count);
                    list2.Sort();
                    foreach (double num13 in list2)
                    {
                        if (num4 == 0.0)
                        {
                            num4 = num13;
                        }
                        if (num5 == 0.0)
                        {
                            num5 = num13;
                        }
                        if (num4 < num13)
                        {
                            num4 = num13;
                        }
                        if (num5 > num13)
                        {
                            num5 = num13;
                        }
                        num8 += Math.Pow(num13 - num7, 2.0);
                    }
                    int num14 = list2.Count / 2;
                    double num15 = 0.0;
                    if (list2.Count > 0)
                    {
                        if (list2.Count == 1)
                        {
                            num15 = list2[0];
                        }
                        else if ((list2.Count % 2) == 0)
                        {
                            num15 = list2[num14 - 1];
                        }
                        else
                        {
                            num15 = (list2[num14] + list2[num14 - 1]) / 2.0;
                        }
                    }
                    double d = (list2.Count == 1) ? 0.0 : (num8 / ((double) (list2.Count - 1)));
                    double num17 = num8 / ((double) list2.Count);
                    double num18 = 0.0;
                    if (num10 > 0.0)
                    {
                        num18 = num11 / num10;
                    }
                    if (((indicatorUnit != 0.0) && (indicatorUnit != 1.0)) && (!flag && String.IsNullOrEmpty(indicatorFormat)))
                    {
                        indicatorFormat = "#,##0.0000";
                    }
                    list.Add(num3.ToString(indicatorFormat));
                    list.Add(num4.ToString(indicatorFormat));
                    list.Add(num5.ToString(indicatorFormat));
                    list.Add(list2.Count.ToString());
                    list.Add(num7.ToString(indicatorFormat));
                    list.Add(num15.ToString(indicatorFormat));
                    list.Add(d.ToString(indicatorFormat));
                    list.Add(Math.Sqrt(d).ToString(indicatorFormat));
                    list.Add(num17.ToString(indicatorFormat));
                    list.Add(Math.Sqrt(num17).ToString(indicatorFormat));
                    list.Add(num18.ToString(indicatorFormat));
                    list.Add(num7.ToString());
                    return list;
                }
                if ((indicatorEntityByCustomerId.IndDataType == DataType.DateTime) || (indicatorEntityByCustomerId.IndDataType == DataType.String))
                {
                    list.Add("");
                    list.Add("");
                    list.Add("");
                    list.Add(num12.ToString());
                    list.Add("");
                    list.Add("");
                    list.Add("");
                    list.Add("");
                    list.Add("");
                    list.Add("");
                    list.Add("");
                    list.Add("");
                    return list;
                }
                list.Add(num3.ToString(indicatorFormat));
                list.Add(num4.ToString(indicatorFormat));
                list.Add(num5.ToString(indicatorFormat));
                list.Add(num12.ToString());
                list.Add(num7.ToString(indicatorFormat));
                list.Add(0.ToString(indicatorFormat));
                list.Add(0.ToString(indicatorFormat));
                list.Add(0.ToString(indicatorFormat));
                list.Add(0.ToString(indicatorFormat));
                list.Add(0.ToString(indicatorFormat));
                list.Add(0.ToString(indicatorFormat));
                list.Add(0.ToString());
            }
            catch (Exception exception)
            {
                //LogUtility.LogTableMessage(String.Concat(new object[] { "数据中心|customerIndicatorId=", indicatorId, " 统计异常,", exception.ToString() }));
            }
            return list;
        }

        public delegate List<StockEntity> FilterBlockElemsDel(String id, List<List<String>> blockCodeList);

        public delegate SearchIndicatorResultEntity SearchIndcatorDelegate(String requestId, String categoryCodes);
    }
}
