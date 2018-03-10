using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using EmCore.Utils;
using System.Runtime.InteropServices;
using EmCore;

namespace OwLib
{
    public class CustomerIndicatorDataHelper
    {
        public static CustomerIndicatorDataHelper Intance = new CustomerIndicatorDataHelper();

        private void AssignIndiatorParameterToSubIndicator(IndicatorEntity indicator, IndicatorEntity subIndicator)
        {
            List<ParamterObject> list = JSONHelper.DeserializeObject<List<ParamterObject>>(indicator.Parameters);
            List<ParamterObject> list2 = JSONHelper.DeserializeObject<List<ParamterObject>>(subIndicator.Parameters);
            if ((list != null) && (list2 != null))
            {
                Dictionary<String, IndicatorEntity> dictionary = new Dictionary<String, IndicatorEntity>();
                foreach (ParamterObject obj2 in list2)
                {
                    foreach (ParamterObject obj3 in list)
                    {
                        if (!dictionary.ContainsKey(subIndicator.NO))
                        {
                            dictionary.Add(subIndicator.NO, subIndicator);
                        }
                        if (((!(obj3.Type == "419") && !(obj3.Type == "461")) && (obj3.IndicatorNo.Contains(subIndicator.NO) && !(obj2.Type != obj3.Type))) && !(obj2.DefaultValue.ToString() == obj3.DefaultValue.ToString()))
                        {
                            obj2.DefaultValue = obj3.DefaultValue;
                            subIndicator.Parameters = JSONHelper.SerializeObject(list2);
                        }
                    }
                }
                foreach (IndicatorEntity entity in dictionary.Values)
                {
                    entity.CustomerId = DataCore.CreateInstance().GetIndicatorCusotmerId(entity);
                }
            }
        }

        private void Calculate(IndicatorEntity indicator, DataTable accept)
        {
           
        }

        private DataTable GetCalculateTable(IndicatorEntity indicator, List<String> blockCodeList)
        {
            DataTable accept = new DataTable();
            accept.Columns.Add();
            Dictionary<String, IndicatorEntity> dictionary = new Dictionary<String, IndicatorEntity>();
            int num = 0;
            foreach (IndicatorEntity entity in indicator.CustomIndicator.IndicatorList)
            {
                num++;
                String columnName = accept.Columns.Contains(entity.NO) ? (entity.NO + "_" + num) : entity.NO;
                accept.Columns.Add(columnName).DataType = this.IsDouble(entity.IndDataType) ? typeof(double) : typeof(String);
                if (!dictionary.ContainsKey(columnName))
                {
                    dictionary.Add(columnName, entity);
                }
            }
            Dictionary<int, double> dictionary2 = new Dictionary<int, double>();
            foreach (String str2 in blockCodeList)
            {
                DataRow row = accept.NewRow();
                row[0] = str2;
                for (int i = 1; i < accept.Columns.Count; i++)
                {
                    String str3 = accept.Columns[i].ColumnName;
                    if (dictionary[str3].CustomIndicator != null)
                    {
                        Dictionary<String, double> customerIndicatorData = this.GetCustomerIndicatorData(dictionary[str3], blockCodeList);
                        double num3 = customerIndicatorData.ContainsKey(str2) ? customerIndicatorData[str2] : 0.0;
                        if (num3 == double.MinValue)
                        {
                            row[i] = DBNull.Value;
                        }
                        else
                        {
                            row[i] = num3;
                        }
                    }
                    else
                    {
                        try
                        {
                            double num5;
                            double num6;
                            double num7;
                            if (!dictionary2.ContainsKey(dictionary[str3].CustomerId))
                            {
                                dictionary2.Add(dictionary[str3].CustomerId, DataCore.CreateInstance().GetIndicatorUnit(dictionary[str3].CustomerId));
                            }
                            double unitValue = dictionary2[dictionary[str3].CustomerId];
                            switch (dictionary[str3].IndDataType)
                            {
                                case DataType.Bool:
                                case DataType.Byte:
                                case DataType.Short:
                                case DataType.UShort:
                                    num5 = DataCore.CreateInstance().GetIndicatorInt32Value(str2, dictionary[str3].CustomerId.ToString(), unitValue);
                                    if (num5 != double.MinValue)
                                    {
                                        break;
                                    }
                                    row[i] = DBNull.Value;
                                    goto Label_03AA;

                                case DataType.Decimal:
                                case DataType.Double:
                                case DataType.Float:
                                    num7 = DataCore.CreateInstance().GetIndicatorDoubleValue(str2, dictionary[str3].CustomerId.ToString(), unitValue);
                                    if (num7 != double.MinValue)
                                    {
                                        goto Label_0345;
                                    }
                                    row[i] = DBNull.Value;
                                    goto Label_03AA;

                                case DataType.Int:
                                case DataType.Long:
                                case DataType.UInt:
                                case DataType.ULong:
                                    num6 = DataCore.CreateInstance().GetIndicatorInt64Value(str2, dictionary[str3].CustomerId.ToString(), unitValue);
                                    if (num6 != double.MinValue)
                                    {
                                        goto Label_02ED;
                                    }
                                    row[i] = DBNull.Value;
                                    goto Label_03AA;

                                default:
                                    goto Label_0357;
                            }
                            row[i] = num5;
                            goto Label_03AA;
                        Label_02ED:
                            row[i] = num6;
                            goto Label_03AA;
                        Label_0345:
                            row[i] = num7;
                            goto Label_03AA;
                        Label_0357:
                            row[i] = DataCore.CreateInstance().GetIndicatorStringValue(str2, dictionary[str3].CustomerId.ToString());
                        }
                        catch (Exception exception)
                        {
                            //LogUtility.LogTableMessage("获取自定义指标值：" + exception.Message + Environment.NewLine + exception.StackTrace);
                        }
                    Label_03AA: ;
                    }
                }
                accept.Rows.Add(row);
            }
            this.Calculate(indicator, accept);
            return accept;
        }

        public Dictionary<String, double> GetCustomerIndicatorData(IndicatorEntity indicator, List<String> blockCodeList)
        {
            DataTable calculateTable = this.GetCalculateTable(indicator, blockCodeList);
            Dictionary<String, double> dictionary = new Dictionary<String, double>();
            int num = calculateTable.Columns.Count - 1;
            foreach (DataRow row in calculateTable.Rows)
            {
                double result = 0.0;
                String str = (row[num] == DBNull.Value) ? String.Empty : row[num].ToString();
                if (!String.IsNullOrEmpty(str))
                {
                    if (indicator.IndDataType == DataType.Bool)
                    {
                        result = (str.ToLower() == "true") ? ((double)1) : ((double)0);
                    }
                    else
                    {
                        double.TryParse(str, out result);
                    }
                }
                else
                {
                    result = double.MinValue;
                }
                dictionary[row[0].ToString()] = result;
            }
            return dictionary;
        }

        public Dictionary<String, double> GetCustomerIndicatorData(int customerIndicatorId, List<String> blockCodeList)
        {
            IndicatorEntity indicatorEntityByCustomerId = DataCore.CreateInstance().GetIndicatorEntityByCustomerId(customerIndicatorId);
            return this.GetCustomerIndicatorData(indicatorEntityByCustomerId, blockCodeList);
        }

        public Dictionary<String, String> GetCustomerIndicatorDataString(IndicatorEntity indicator, List<String> blockCodeList)
        {
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            DataTable calculateTable = this.GetCalculateTable(indicator, blockCodeList);
            int num = calculateTable.Columns.Count - 1;
            bool flag = indicator.IndDataType == DataType.DateTime;
            foreach (DataRow row in calculateTable.Rows)
            {
                String str = (row[num] == DBNull.Value) ? String.Empty : row[num].ToString();
                String str2 = str;
                if (!String.IsNullOrEmpty(str) && flag)
                {
                    DateTime result = new DateTime();
                    DateTime.TryParse(str, out result);
                    str2 = result.ToString("yyyy-MM-dd");
                    if (str2 == "0001-01-01")
                    {
                        str2 = "--";
                    }
                }
                dictionary[row[0].ToString()] = str2;
            }
            return dictionary;
        }

        public Dictionary<String, String> GetCustomerIndicatorDataString(int customerIndicatorId, List<String> blockCodeList)
        {
            IndicatorEntity indicatorEntityByCustomerId = DataCore.CreateInstance().GetIndicatorEntityByCustomerId(customerIndicatorId);
            return this.GetCustomerIndicatorDataString(indicatorEntityByCustomerId, blockCodeList);
        }

        public List<DateTime> GetCustomerIndicatorDateTime(int customerIndicatorId, List<String> blockCodeList)
        {
            List<DateTime> list = new List<DateTime>();
            IndicatorEntity indicatorEntityByCustomerId = DataCore.CreateInstance().GetIndicatorEntityByCustomerId(customerIndicatorId);
            DataTable calculateTable = this.GetCalculateTable(indicatorEntityByCustomerId, blockCodeList);
            int num = calculateTable.Columns.Count - 1;
            if (indicatorEntityByCustomerId.IndDataType == DataType.DateTime)
            {
                foreach (DataRow row in calculateTable.Rows)
                {
                    if (row[num] != DBNull.Value)
                    {
                        try
                        {
                            String str = row[num].ToString();
                            if (!String.IsNullOrEmpty(str) && (str != "0001-01-01"))
                            {
                                list.Add(Convert.ToDateTime(str));
                            }
                            continue;
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }
            return list;
        }

        public String GetIndicatorFormatValue(DataType dType, double indicatorValue, [Optional, DefaultParameterValue(true)] bool isBaseUnit, [Optional, DefaultParameterValue(false)] bool isCalculateInd)
        {
            String format = "";
            switch (dType)
            {
                case DataType.Byte:
                case DataType.Int:
                case DataType.Long:
                case DataType.Short:
                case DataType.UInt:
                case DataType.ULong:
                case DataType.UShort:
                    format = "#,##0";
                    break;

                case DataType.Decimal:
                case DataType.Double:
                case DataType.Float:
                    format = "#,##0.0000";
                    break;
            }
            if (!isBaseUnit || isCalculateInd)
            {
                format = "#,##0.0000";
            }
            return indicatorValue.ToString(format);
        }

        public bool IsDouble(DataType dataType)
        {
            switch (dataType)
            {
                case DataType.Bool:
                case DataType.Byte:
                case DataType.Decimal:
                case DataType.Double:
                case DataType.Float:
                case DataType.Int:
                case DataType.Long:
                case DataType.Short:
                case DataType.UInt:
                case DataType.ULong:
                case DataType.UShort:
                    return true;
            }
            return false;
        }

        public void SetIndicatorParam(IndicatorEntity indicator)
        {
            if (!String.IsNullOrEmpty(indicator.Parameters))
            {
                foreach (IndicatorEntity entity in indicator.CustomIndicator.IndicatorList)
                {
                    this.AssignIndiatorParameterToSubIndicator(indicator, entity);
                }
            }
        }
    }
}
