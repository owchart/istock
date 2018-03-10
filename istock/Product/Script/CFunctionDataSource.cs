/*******************************************************************************************\
*                                                                                           *
* CFunctionDataSource.cs -  Datasource functions, types, and definitions.                   *
*                                                                                           *
*               Version 1.00  ★★★                                                        *
*                                                                                           *
*               Copyright (c) 2016-2016, iTeam. All rights reserved.                    *
*               Created by Todd 2016/10/17.                                                  *
*                                                                                           *
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace OwLib
{
    /// <summary>
    /// 界面相关的库
    /// </summary>
    public class CFunctionDataSource : CFunction
    {
        /// <summary>
        /// 创建方法
        /// </summary>
        /// <param name="indicator">指标</param>
        /// <param name="id">ID</param>
        /// <param name="name">名称</param>
        /// <param name="withParameters">是否有参数</param>
        public CFunctionDataSource(CIndicator indicator, int id, String name)
        {
            m_indicator = indicator;
            m_ID = id;
            m_name = name;
        }

        /// <summary>
        /// 指标
        /// </summary>
        public CIndicator m_indicator;

        /// <summary>
        /// 方法
        /// </summary>
        private static String FUNCTIONS = "GETINDEX,GETVALUE,GETVALUE2,GETVALUE3,GETXVALUE,SETVALUE,SETVALUE2,SETVALUE3,COLUMNSCOUNT,ROWSCOUNT,ADDCOLUMN,CLEAR,GETCOLUMNINDEX,GETROWINDEX,REMOVE,REMOVEAT,REMOVECOLUMN";

        /// <summary>
        /// 前缀
        /// </summary>
        private static String PREFIX = "DATASOURCE.";

        /// <summary>
        /// 开始索引
        /// </summary>
        private const int STARTINDEX = 6000;

        /// <summary>
        /// 添加方法
        /// </summary>
        /// <param name="indicator">方法库</param>
        /// <param name="native">脚本</param>
        /// <returns>指标</returns>
        public static void AddFunctions(CIndicator indicator)
        {
            String[] functions = FUNCTIONS.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            int functionsSize = functions.Length;
            for (int i = 0; i < functionsSize; i++)
            {
                indicator.AddFunction(new CFunctionDataSource(indicator, STARTINDEX + i, PREFIX + functions[i]));
            }
        }

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>结果</returns>
        public override double OnCalculate(CVariable var)
        {
            switch (var.m_functionID)
            {
                case STARTINDEX + 0:
                    return DATASOURCE_GETINDEX(var);
                case STARTINDEX + 1:
                    return DATASOURCE_GETVALUE(var);
                case STARTINDEX + 2:
                    return DATASOURCE_GETVALUE2(var);
                case STARTINDEX + 3:
                    return DATASOURCE_GETVALUE3(var);
                case STARTINDEX + 4:
                    return DATASOURCE_GETXVALUE(var);
                case STARTINDEX + 5:
                    return DATASOURCE_SETVALUE(var);
                case STARTINDEX + 6:
                    return DATASOURCE_SETVALUE2(var);
                case STARTINDEX + 7:
                    return DATASOURCE_SETVALUE3(var);
                case STARTINDEX + 8:
                    return DATASOURCE_COLUMNSCOUNT(var);
                case STARTINDEX + 9:
                    return DATASOURCE_ROWSCOUNT(var);
                case STARTINDEX + 10:
                    return DATASOURCE_ADDCOLUMN(var);
                case STARTINDEX + 11:
                    return DATASOURCE_CLEAR(var);
                case STARTINDEX + 12:
                    return DATASOURCE_GETCOLUMNINDEX(var);
                case STARTINDEX + 13:
                    return DATASOURCE_GETROWINDEX(var);
                case STARTINDEX + 14:
                    return DATASOURCE_REMOVE(var);
                case STARTINDEX + 15:
                    return DATASOURCE_REMOVEAT(var);
                case STARTINDEX + 16:
                    return DATASOURCE_REMOVECOLUMN(var);
                default: return 0;
            }
        }

        /// <summary>
        /// 添加列
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double DATASOURCE_ADDCOLUMN(CVariable var)
        {
            int colName = (int)m_indicator.GetValue(var.m_parameters[0]);
            m_indicator.DataSource.AddColumn(colName);
            return 0;
        }

        /// <summary>
        /// 清除数据源
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double DATASOURCE_CLEAR(CVariable var)
        {
            m_indicator.DataSource.Clear();
            return 0;
        }

        /// <summary>
        /// 获取列数
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double DATASOURCE_COLUMNSCOUNT(CVariable var)
        {
            return m_indicator.DataSource.ColumnsCount;
        }

        /// <summary>
        /// 获取列的索引
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double DATASOURCE_GETCOLUMNINDEX(CVariable var)
        {
            int colName = (int)m_indicator.GetValue(var.m_parameters[0]);
            return m_indicator.DataSource.GetColumnIndex(colName);
        }

        /// <summary>
        /// 获取当前索引
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double DATASOURCE_GETINDEX(CVariable var)
        {
            return m_indicator.Index;
        }

        /// <summary>
        /// 获取行的索引
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double DATASOURCE_GETROWINDEX(CVariable var)
        {
            double pk = m_indicator.GetValue(var.m_parameters[0]);
            return m_indicator.DataSource.GetRowIndex(pk);
        }

        /// <summary>
        /// 获取当前数值
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double DATASOURCE_GETVALUE(CVariable var)
        {
            double pk = m_indicator.GetValue(var.m_parameters[0]);
            int colName = (int)m_indicator.GetValue(var.m_parameters[1]);
            double value = m_indicator.DataSource.Get(pk, colName);
            return value;
        }

        /// <summary>
        /// 获取当前数值2
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double DATASOURCE_GETVALUE2(CVariable var)
        {
            int rowIndex = (int)m_indicator.GetValue(var.m_parameters[0]);
            int colName = (int)m_indicator.GetValue(var.m_parameters[1]);
            double value = m_indicator.DataSource.Get2(rowIndex, colName);
            return value;
        }

        /// <summary>
        /// 获取当前数值3
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double DATASOURCE_GETVALUE3(CVariable var)
        {
            int rowIndex = (int)m_indicator.GetValue(var.m_parameters[0]);
            int colIndex = (int)m_indicator.GetValue(var.m_parameters[1]);
            double value = m_indicator.DataSource.Get3(rowIndex, colIndex);
            return value;
        }

        /// <summary>
        /// 获取X的值
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double DATASOURCE_GETXVALUE(CVariable var)
        {
            int rowIndex = (int)m_indicator.GetValue(var.m_parameters[0]);
            double value = m_indicator.DataSource.GetXValue(rowIndex);
            return value;
        }

        /// <summary>
        /// 获取行数
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double DATASOURCE_ROWSCOUNT(CVariable var)
        {
            return m_indicator.DataSource.RowsCount;
        }

        /// <summary>
        /// 移除行
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double DATASOURCE_REMOVE(CVariable var)
        {
            double pk = m_indicator.GetValue(var.m_parameters[0]);
            m_indicator.DataSource.Remove(pk);
            return 0;
        }

        /// <summary>
        /// 移除行
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double DATASOURCE_REMOVEAT(CVariable var)
        {
            int rowIndex = (int)m_indicator.GetValue(var.m_parameters[0]);
            m_indicator.DataSource.RemoveAt(rowIndex);
            return 0;
        }

        /// <summary>
        /// 移除列
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double DATASOURCE_REMOVECOLUMN(CVariable var)
        {
            int colName = (int)m_indicator.GetValue(var.m_parameters[0]);
            m_indicator.DataSource.RemoveColumn(colName);
            return 0;
        }

        /// <summary>
        /// 设置当前数值
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double DATASOURCE_SETVALUE(CVariable var)
        {
            double pk = m_indicator.GetValue(var.m_parameters[0]);
            int colName = (int)m_indicator.GetValue(var.m_parameters[1]);
            double value = m_indicator.GetValue(var.m_parameters[2]);
            m_indicator.DataSource.Set(pk, colName, value);
            return 0;
        }

        /// <summary>
        /// 设置当前数值2
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double DATASOURCE_SETVALUE2(CVariable var)
        {
            int rowIndex = (int)m_indicator.GetValue(var.m_parameters[0]);
            int colName = (int)m_indicator.GetValue(var.m_parameters[1]);
            double value = m_indicator.GetValue(var.m_parameters[2]);
            m_indicator.DataSource.Set2(rowIndex, colName, value);
            return 0;
        }

        /// <summary>
        /// 设置当前数值3
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double DATASOURCE_SETVALUE3(CVariable var)
        {
            int rowIndex = (int)m_indicator.GetValue(var.m_parameters[0]);
            int colIndex = (int)m_indicator.GetValue(var.m_parameters[1]);
            double value = m_indicator.GetValue(var.m_parameters[2]);
            m_indicator.DataSource.Set3(rowIndex, colIndex, value);
            return 0;
        }
    }
}
