/*******************************************************************************************\
*                                                                                           *
* CFunctionEx.cs -  Indicator functions, types, and definitions.                            *
*                                                                                           *
*               Version 1.00  ★★★                                                        *
*                                                                                           *
*               Copyright (c) 2016-2016, iTeam. All rights reserved.                    *
*               Created by Todd 2016/10/17.                                                  *
*                                                                                           *
********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;

namespace OwLib
{
    /// <summary>
    /// 提示方法
    /// </summary>
    public class CFunctionEx : CFunction
    {
        /// <summary>
        /// 创建方法
        /// </summary>
        /// <param name="indicator">指标</param>
        /// <param name="id">ID</param>
        /// <param name="name">名称</param>
        /// <param name="withParameters">是否有参数</param>
        public CFunctionEx(CIndicator indicator, int id, String name, UIXml xml)
        {
            m_indicator = indicator;
            m_ID = id;
            m_name = name;
            m_xml = xml;
        }

        /// <summary>
        /// 指标
        /// </summary>
        public CIndicator m_indicator;

        /// <summary>
        /// XML对象
        /// </summary>
        public UIXml m_xml;

        /// <summary>
        /// 方法字段
        /// </summary>
        private const String FUNCTIONS = "";

        /// <summary>
        /// 开始索引
        /// </summary>
        private const int STARTINDEX = 1000000;

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>结果</returns>
        public override double OnCalculate(CVariable var)
        {
            switch (var.m_functionID)
            {
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 创建指标
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="script">脚本</param>
        /// <param name="xml">XML</param>
        /// <returns>指标</returns>
        public static CIndicator CreateIndicator(String id, String script, UIXml xml)
        {
            CIndicator indicator = xml.Native.CreateIndicator();
            indicator.Name = id;
            CTable table = xml.Native.CreateTable();
            indicator.DataSource = table;
            CFunctionBase.AddFunctions(indicator);
            CFunctionUI.AddFunctions(indicator, xml);
            CFunctionWin.AddFunctions(indicator);
            int index = STARTINDEX;
            String[] functions = FUNCTIONS.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            int functionsSize = functions.Length;
            for (int i = 0; i < functionsSize; i++)
            {
                indicator.AddFunction(new CFunctionEx(indicator, index + i, functions[i], xml));
            }
            indicator.Script = script;
            table.AddColumn(0);
            table.Set(0, 0, 0);
            indicator.OnCalculate(0);
            return indicator;
        }
    }
}
