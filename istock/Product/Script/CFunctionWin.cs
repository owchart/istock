/*******************************************************************************************\
*                                                                                           *
* CFunctionWin.cs -  Win functions, types, and definitions.                            *
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
using System.Threading;
using System.Windows.Forms;

namespace OwLib
{
    /// <summary>
    /// 界面相关的库
    /// </summary>
    public class CFunctionWin : CFunction
    {
        /// <summary>
        /// 创建方法
        /// </summary>
        /// <param name="indicator">指标</param>
        /// <param name="id">ID</param>
        /// <param name="name">名称</param>
        /// <param name="withParameters">是否有参数</param>
        public CFunctionWin(CIndicator indicator, int id, String name)
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
        private static String FUNCTIONS = "BEEP,EXECUTE,MOUSEEVENT,MOUSECLICK,SETTEXT,SENDKEY,GETVALUE,";

        /// <summary>
        /// 前缀
        /// </summary>
        private static String PREFIX = "WIN.";

        /// <summary>
        /// 开始索引
        /// </summary>
        private const int STARTINDEX = 20000;

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
                    return WIN_BEEP(var);
                case STARTINDEX + 1:
                    return WIN_EXECUTE(var);
                case STARTINDEX + 2:
                    return WIN_MOUSEEVENT(var);
                case STARTINDEX + 3:
                    return WIN_MOUSECLICK(var);
                case STARTINDEX + 4:
                    return WIN_SETTEXT(var);
                case STARTINDEX + 5:
                    return WIN_SENDKEY(var);
                case STARTINDEX + 6:
                    return WIN_GETVALUE(var);
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 添加方法
        /// </summary>
        /// <param name="indicator">方法库</param>
        /// <param name="native">脚本</param>
        /// <param name="xml">XML</param>
        /// <returns>指标</returns>
        public static void AddFunctions(CIndicator indicator)
        {
            String[] functions = FUNCTIONS.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            int functionsSize = functions.Length;
            for (int i = 0; i < functionsSize; i++)
            {
                indicator.AddFunction(new CFunctionWin(indicator, STARTINDEX + i, PREFIX + functions[i]));
            }
        }


        /// <summary>
        /// Windows下主板响
        /// </summary_
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double WIN_BEEP(CVariable var)
        {
            int frequency = 0, duration = 0;
            int vlen = var.m_parameters.Length;
            if (vlen >= 1)
            {
                frequency = (int)m_indicator.GetValue(var.m_parameters[0]);
            }
            if (vlen >= 2)
            {
                duration = (int)m_indicator.GetValue(var.m_parameters[1]);
            }
            Console.Beep(frequency, duration);
            return 0;
        }

        /// <summary>
        /// Windows下执行程序
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double WIN_EXECUTE(CVariable var)
        {
            WinHostEx.Execute(m_indicator.GetText(var.m_parameters[0]));
            return 1;
        }

        /// <summary>
        /// Windows下获取控件数值
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>数值</returns>
        private double WIN_GETVALUE(CVariable var)
        {
            double value = 0;
            String text = WinHostEx.GetText();
            value = CStr.ConvertStrToDouble(text);
            return value;
        }

        /// <summary>
        /// Windows下的键盘事件
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double WIN_SENDKEY(CVariable var)
        {
            int vlen = var.m_parameters.Length;
            String key = "";
            if (vlen >= 1)
            {
                key = m_indicator.GetText(var.m_parameters[0]);
            }
            WinHostEx.SendKey(key);
            return 1;
        }

        /// <summary>
        /// Windows下的鼠标事件
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double WIN_MOUSEEVENT(CVariable var)
        {
            int dx = 0, dy = 0, data = 0;
            int vlen = var.m_parameters.Length;
            String eventID = "";
            if (vlen >= 1)
            {
                eventID = m_indicator.GetText(var.m_parameters[0]);
            }
            if (vlen >= 2)
            {
                dx = (int)m_indicator.GetValue(var.m_parameters[1]);
            }
            if (vlen >= 3)
            {
                dy = (int)m_indicator.GetValue(var.m_parameters[2]);
            }
            if (vlen >= 4)
            {
                data = (int)m_indicator.GetValue(var.m_parameters[3]);
            }
            WinHostEx.MouseEvent(eventID, dx, dy, data);
            return 1;
        }

        /// <summary>
        /// Windows下的鼠标点击事件
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>

        private double WIN_MOUSECLICK(CVariable var)
        {
            int dx = 0, dy = 0;
            int vlen = var.m_parameters.Length;
            if (vlen >= 1)
            {
                dx = (int)m_indicator.GetValue(var.m_parameters[0]);
            }
            if (vlen >= 2)
            {
                dy = (int)m_indicator.GetValue(var.m_parameters[1]);
            }
            WinHostEx.MouseEvent("SETCURSOR", dx, dy, 0);
            WinHostEx.MouseEvent("LEFTDOWN", 0, 0, 0);
            WinHostEx.MouseEvent("LEFTUP", 0, 0, 0);
            return 1;
        }

        /// <summary>
        /// Windows下设置文字
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double WIN_SETTEXT(CVariable var)
        {
            WinHostEx.SetText(m_indicator.GetText(var.m_parameters[0]));
            return 1;
        }
    }
}