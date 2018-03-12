/*****************************************************************************\
*                                                                             *
* SecurityDataHelper.cs - Security data functions, types, and definitions.    *
*                                                                             *
*               Version 1.00  ★★★                                          *
*                                                                             *
*               Copyright (c) 2016-2016, Piratecat. All rights reserved.      *
*               Created by Lord 2016/1/30.                                    *
*                                                                             *
******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    /// <summary>
    /// 股票数据处理
    /// </summary>
    public class SecurityDataHelper
    {
        #region Lord 2016/1/30
        /// <summary>
        /// 创建数据源
        /// </summary>
        /// <param name="chart">股票控件</param>
        /// <returns>数据源</returns>
        public static CTable CreateDataSource(ChartA chart)
        {
            CTable dataSource = chart.Native.CreateTable();
            dataSource.AddColumn(KeyFields.CLOSE_INDEX);
            dataSource.AddColumn(KeyFields.HIGH_INDEX);
            dataSource.AddColumn(KeyFields.LOW_INDEX);
            dataSource.AddColumn(KeyFields.OPEN_INDEX);
            dataSource.AddColumn(KeyFields.VOL_INDEX);
            dataSource.AddColumn(KeyFields.AMOUNT_INDEX);
            return dataSource;
        }

        /// <summary>
        /// 添加指标
        /// </summary>
        /// <param name="chart">股票控件</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="text">文本</param>
        /// <param name="parameters">参数</param>
        public static CIndicator CreateIndicator(ChartA chart, CTable dataSource, String text, String parameters)
        {
            CIndicator indicator = chart.Native.CreateIndicator();
            indicator.DataSource = dataSource;
            indicator.Name = "";
            //indicator.FullName = "";
            if (dataSource != null)
            {
                indicator.SetSourceField(KeyFields.CLOSE, KeyFields.CLOSE_INDEX);
                indicator.SetSourceField(KeyFields.HIGH, KeyFields.HIGH_INDEX);
                indicator.SetSourceField(KeyFields.LOW, KeyFields.LOW_INDEX);
                indicator.SetSourceField(KeyFields.OPEN, KeyFields.OPEN_INDEX);
                indicator.SetSourceField(KeyFields.VOL, KeyFields.VOL_INDEX);
                indicator.SetSourceField(KeyFields.AMOUNT, KeyFields.AMOUNT_INDEX);
                indicator.SetSourceField(KeyFields.CLOSE.Substring(0, 1), KeyFields.CLOSE_INDEX);
                indicator.SetSourceField(KeyFields.HIGH.Substring(0, 1), KeyFields.HIGH_INDEX);
                indicator.SetSourceField(KeyFields.LOW.Substring(0, 1), KeyFields.LOW_INDEX);
                indicator.SetSourceField(KeyFields.OPEN.Substring(0, 1), KeyFields.OPEN_INDEX);
                indicator.SetSourceField(KeyFields.VOL.Substring(0, 1), KeyFields.VOL_INDEX);
                indicator.SetSourceField(KeyFields.AMOUNT.Substring(0, 1), KeyFields.AMOUNT_INDEX);
            }
            IndicatorData indicatorData = new IndicatorData();
            indicatorData.m_parameters = parameters;
            indicatorData.m_script = text;
            indicator.Tag = indicatorData;
            String constValue = "";
            if (parameters != null && parameters.Length > 0)
            {
                String[] strs = parameters.Split(new String[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                int strsSize = strs.Length;
                for (int i = 0; i < strsSize; i++)
                {
                    String str = strs[i];
                    String[] strs2 = str.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    constValue += "const " + strs2[0] + ":" + strs2[3] + ";";
                }
            }
            if (text != null && text.Length > 0)
            {
                indicator.Script = constValue + text;
            }
            return indicator;
        }

        /// <summary>
        /// 绑定历史数据
        /// </summary>
        /// <param name="chart">股票控件</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="indicators">指标</param>
        /// <param name="fields">字段</param>
        /// <param name="historyDatas">历史数据</param>
        public static void BindHistoryDatas(ChartA chart, CTable dataSource, List<CIndicator> indicators, int[] fields, List<SecurityData> historyDatas)
        {
            dataSource.Clear();
            int size = historyDatas.Count;
            dataSource.SetRowsCapacity(size + 10);
            dataSource.SetRowsGrowStep(100);
            int columnsCount = dataSource.ColumnsCount;
            for (int i = 0; i < size; i++)
            {
                SecurityData securityData = historyDatas[i];
                if (dataSource == chart.DataSource)
                {
                    InsertData(chart, dataSource, fields, securityData);
                }
                else
                {
                    double[] ary = new double[columnsCount];
                    ary[0] = securityData.m_close;
                    ary[1] = securityData.m_high;
                    ary[2] = securityData.m_low;
                    ary[3] = securityData.m_open;
                    ary[4] = securityData.m_volume;
                    for (int j = 5; j < columnsCount; j++)
                    {
                        ary[j] = double.NaN;
                    }
                    dataSource.AddRow(securityData.m_date, ary, columnsCount);
                }
            }
            int indicatorsSize = indicators.Count;
            for (int i = 0; i < indicatorsSize; i++)
            {
                indicators[i].OnCalculate(0);
            }
        }

        public static void GetIndicatorByName(String name, Indicator indicator)
        {
            if(name == "MA")
            {
                indicator.m_name = "MA";
                indicator.m_description = "移动平均线";
                indicator.m_parameters = "N1,0,1000,5;N2,0,1000,10;N3,0,1000,20;N4,0,1000,60;N5,0,1000,130;N6,0,1000,250;";
                indicator.m_text = "MA5:MA(CLOSE,N1);MA10:MA(CLOSE,N2);MA20:MA(CLOSE,N3);MA60:MA(CLOSE,N4);MA120:MA(CLOSE,N5);MA250:MA(CLOSE,N6);";
            }
            else if(name == "MACD")
            {
                indicator.m_name = "MACD";
                indicator.m_description = "指数平滑异同平均线";
                indicator.m_parameters = "SHORT,0,1000,12;LONG,0,1000,26;MID,0,1000,9;";
                indicator.m_text = "DIF:EMA(CLOSE,SHORT)-EMA(CLOSE,LONG);DEA:EMA(DIF,MID);MACD:(DIF-DEA)*2,COLORSTICK;";
            }
            else if(name == "KDJ")
            {
                indicator.m_name = "KDJ";
                indicator.m_description = "随机指标";
                indicator.m_parameters = "N,0,1000,9;M1,0,1000,3;M2,0,1000,3;";
                indicator.m_text = "RSV:=(CLOSE-LLV(LOW,N))/(HHV(HIGH,N)-LLV(LOW,N))*100;K:SMA(RSV,M1,1);D:SMA(K,M2,1);J:3*K-2*D;";
            }
            else if(name == "RSI")
            {
                indicator.m_name = "RSI";
                indicator.m_description = "相对强弱指标";
                indicator.m_parameters = "N1,0,1000,6;N2,0,1000,12;N3,0,1000,24;";
                indicator.m_text = "LC:=REF(CLOSE,1);RSI1:SMA(MAX(CLOSE-LC,0),N1,1)/SMA(ABS(CLOSE-LC),N1,1)*100;RSI2:SMA(MAX(CLOSE-LC,0),N2,1)/SMA(ABS(CLOSE-LC),N2,1)*100;RSI3:SMA(MAX(CLOSE-LC,0),N3,1)/SMA(ABS(CLOSE-LC),N3,1)*100;";
            }
            else if(name == "ROC")
            {
                indicator.m_name = "ROC";
                indicator.m_description = "变动率指标";
                indicator.m_parameters = "N,0,1000,12;M,0,1000,6;";
                indicator.m_text = "ROC:100*(CLOSE-REF(CLOSE,N))/REF(CLOSE,N);MAROC:MA(ROC,M);";
            }
            else if(name == "BIAS")
            {
                indicator.m_name = "BIAS";
                indicator.m_description = "乖离率";
                indicator.m_parameters = "N1,0,100,6;N2,0,100,12;N3,0,100,24;M4,0,1000,60;";
                indicator.m_text = "BIAS1:(CLOSE-MA(CLOSE,N1))/MA(CLOSE,N1)*100; BIAS2:(CLOSE-MA(CLOSE,N2))/MA(CLOSE,N2)*100;BIAS3:(CLOSE-MA(CLOSE,N3))/MA(CLOSE,N3)*100;";
            }
            else if(name == "CCI")
            {
                indicator.m_name = "CCI";
                indicator.m_description = "商品路径指标";
                indicator.m_parameters = "N,0,100,14;M,0,1000,9;";
                indicator.m_text = "TYP:=(HIGH+LOW+CLOSE)/3;CCI:(TYP-MA(TYP,N))/(0.015*AVEDEV(TYP,N));";
            }
            else if(name == "WR")
            {
                indicator.m_name = "WR";
                indicator.m_description = "威廉指标";
                indicator.m_parameters = "N1,0,1000,5;N2,0,1000,10;";
                indicator.m_text = "WR1:100*(HHV(HIGH,N1)-CLOSE)/(HHV(HIGH,N1)-LLV(LOW,N1));WR2:100*(HHV(HIGH,N2)-CLOSE)/(HHV(HIGH,N2)-LLV(LOW,N2));";
            }
            else if(name == "BOLL")
            {
                indicator.m_name = "BOLL";
                indicator.m_description = "布林带";
                indicator.m_parameters = "N,0,100,20;";
                indicator.m_text = "BOLL:MA(CLOSE,N);UB:BOLL+2*STD(CLOSE,N);LB:BOLL-2*STD(CLOSE,N);";
            }
            else if(name == "BBI")
            {
                indicator.m_name = "BBI";
                indicator.m_description = "多空指标";
                indicator.m_parameters = "N1,0,100,3;N2,0,100,6;N3,0,100,12;N4,0,100,24;";
                indicator.m_text = "BBI:(MA(CLOSE,N1)+MA(CLOSE,N2)+MA(CLOSE,N3)+MA(CLOSE,N4))/4;";
            }
            else if(name == "TRIX")
            {
                indicator.m_name = "TRIX";
                indicator.m_description = "三重指数平均线";
                indicator.m_parameters = "N,0,1000,12;M,0,1000,9;";
                indicator.m_text = "MTR:=EMA(EMA(EMA(CLOSE,N),N),N);TRIX:(MTR-REF(MTR,1))/REF(MTR,1)*100;MATRIX:MA(TRIX,M);";
            }
            else if(name == "DMA")
            {
                indicator.m_name = "DMA";
                indicator.m_description = "平均差";
                indicator.m_parameters = "N1,0,1000,10;N2,0,10000,50;";
                indicator.m_text = "DIF:MA(CLOSE,N1)-MA(CLOSE,N2); DIFMA:MA(DIF,N1);";
            }
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="chart">证券控件</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="fields">字段</param>
        /// <param name="securityData">证券数据</param>
        /// <returns>索引</returns>
        public static int InsertData(ChartA chart, CTable dataSource, int[] fields, SecurityData securityData)
        {
            double close = securityData.m_close, high = securityData.m_high, low = securityData.m_low, open = securityData.m_open, avgPrice = securityData.m_avgPrice, volume = securityData.m_volume, amount = securityData.m_amount;
            if (volume > 0 || close > 0)
            {
                if (high == 0)
                {
                    high = close;
                }
                if (low == 0)
                {
                    low = close;
                }
                if (open == 0)
                {
                    open = close;
                }
                if (avgPrice == 0)
                {
                    avgPrice = double.NaN;
                }
            }
            else
            {
                close = double.NaN;
                high = double.NaN;
                low = double.NaN;
                open = double.NaN;
                volume = double.NaN;
                amount = double.NaN;
                avgPrice = double.NaN;
            }
            double date = securityData.m_date;
            dataSource.Set(date, fields[4], volume);
            int index = dataSource.GetRowIndex(date);
            dataSource.Set2(index, fields[0], close);
            dataSource.Set2(index, fields[1], high);
            dataSource.Set2(index, fields[2], low);
            dataSource.Set2(index, fields[3], open);
            dataSource.Set2(index, fields[5], amount);
            dataSource.Set2(index, fields[6], avgPrice);
            return index;
        }

        /// <summary>
        /// 插入最新数据
        /// </summary>
        /// <param name="chart">股票控件</param>
        /// <param name="dataSource">数据源</param>
        /// <param name="indicators">指标</param>
        /// <param name="fields">字段</param>
        /// <param name="historyDatas">最近的历史数据</param>
        /// <param name="latestData">实时数据</param>
        /// <returns>索引</returns>
        public static int InsertLatestData(ChartA chart, CTable dataSource, List<CIndicator> indicators, int[] fields, SecurityData latestData)
        {
            if (latestData.m_close > 0 && latestData.m_volume > 0)
            {
                int indicatorsSize = indicators.Count;
                int index = InsertData(chart, dataSource, fields, latestData);
                for (int i = 0; i < indicatorsSize; i++)
                {
                    indicators[i].OnCalculate(index);
                }
                return index;
            }
            else
            {
                return -1;
            }
        }
        #endregion
    }
}
