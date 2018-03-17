using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Threading;
using System.Windows.Forms;

namespace OwLibSV
{
    /// <summary>
    /// 选股的外部方法
    /// </summary>
    public class SecurityFilterExternFunc
    {
        #region Lord 2016/5/16
        /// <summary>
        /// 股票控件
        /// </summary>
        private static ChartAEx m_chart;

        /// <summary>
        /// 方法库
        /// </summary>
        private static INativeBase m_native;

        /// <summary>
        /// 流水号
        /// </summary>
        private static int m_serialNumber;

        /// <summary>
        /// 指标集合
        /// </summary>
        private static Dictionary<int, CIndicator> m_indicators = new Dictionary<int, CIndicator>();

        /// <summary>
        /// 创建指标
        /// </summary>
        /// <param name="text">脚本</param>
        /// <param name="parameters">参数</param>
        /// <returns>指标ID</returns>
        public static int CreateIndicatorExtern(String text, String parameters, StringBuilder fields)
        {
            try
            {
                if (m_native == null)
                {
                    m_native = NativeHandler.CreateNative();
                }
                if (m_chart == null)
                {
                    m_chart = new ChartAEx();
                    m_chart.Native = m_native;
                }
                m_serialNumber++;
                CTable dataSource = m_chart.Native.CreateTable();
                dataSource.AddColumn(KeyFields.CLOSE_INDEX);
                dataSource.AddColumn(KeyFields.HIGH_INDEX);
                dataSource.AddColumn(KeyFields.LOW_INDEX);
                dataSource.AddColumn(KeyFields.OPEN_INDEX);
                dataSource.AddColumn(KeyFields.VOL_INDEX);
                dataSource.AddColumn(KeyFields.AMOUNT_INDEX);
                CIndicator indicator = SecurityDataHelper.CreateIndicator(m_chart, dataSource, text, parameters);
                m_indicators[m_serialNumber] = indicator;
                indicator.OnCalculate(0);
                int pos = 0;
                int variablesSize = indicator.MainVariables.Count;
                foreach (String field in indicator.MainVariables.Keys)
                {
                    fields.Append(field);
                    if (pos != variablesSize - 1)
                    {
                        fields.Append(",");
                    }
                    pos++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
            }
            return m_serialNumber;
        }

        /// <summary>
        /// 计算指标
        /// </summary>
        /// <param name="id">指标ID</param>
        /// <param name="code">代码</param>
        /// <returns>返回数据</returns>
        public static double[] CalculateIndicatorExtern(int id, String code)
        {
            if (m_indicators.ContainsKey(id))
            {
                CIndicator indicator = m_indicators[id];
                List<CIndicator> indicators = new List<CIndicator>();
                indicators.Add(indicator);
                List<SecurityData> datas = new List<SecurityData>();
                List<SecurityData> oldSecurityDatas = SecurityService.m_historyDatas[code];
                int oldSecurityDatasSize = oldSecurityDatas.Count;
                for (int i = 0; i < oldSecurityDatasSize; i++)
                {
                    datas.Add(oldSecurityDatas[i]);
                }
                SecurityLatestData latestData = SecurityService.m_latestDatas[code];
                SecurityData newData = new SecurityData();
                StockService.GetSecurityData(latestData, latestData.m_lastClose, 1440, 1, ref newData);
                datas.Add(newData);
                CTable dataSource = indicator.DataSource;
                int[] fields = new int[] { KeyFields.CLOSE_INDEX, KeyFields.HIGH_INDEX, KeyFields.LOW_INDEX, KeyFields.OPEN_INDEX, KeyFields.VOL_INDEX, KeyFields.AMOUNT_INDEX };
                SecurityDataHelper.BindHistoryDatas(m_chart, dataSource, indicators, fields, datas);
                datas.Clear();
                int rowsCount = dataSource.RowsCount;
                int variablesSize = indicator.MainVariables.Count;
                double[] list = new double[variablesSize];
                if (rowsCount > 0)
                {
                    int pos = 0;
                    foreach (String name in indicator.MainVariables.Keys)
                    {
                        int field = indicator.MainVariables[name];
                        double value = dataSource.Get2(rowsCount - 1, field);
                        list[pos] = value;
                        pos++;
                    }
                }
                dataSource.Clear();
                return list;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 删除指标
        /// </summary>
        /// <param name="id">指标ID</param>
        public static void DeleteIndicatorExtern(int id)
        {
            if (m_indicators.ContainsKey(id))
            {
                CIndicator indicator = m_indicators[id];
                m_indicators.Remove(id);
                indicator.Clear();
                indicator.DataSource.Dispose();
                indicator.DataSource = null;
                indicator.Dispose();
            }
        }
        #endregion
    }
}
