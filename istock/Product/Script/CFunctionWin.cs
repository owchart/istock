/*******************************************************************************************\
*                                                                                           *
* CFunctionWin.cs -  Win functions, types, and definitions.                            *
*                                                                                           *
*               Version 1.00  ����                                                        *
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
    /// ������صĿ�
    /// </summary>
    public class CFunctionWin : CFunction
    {
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="indicator">ָ��</param>
        /// <param name="id">ID</param>
        /// <param name="name">����</param>
        /// <param name="withParameters">�Ƿ��в���</param>
        public CFunctionWin(CIndicator indicator, int id, String name)
        {
            m_indicator = indicator;
            m_ID = id;
            m_name = name;
        }

        /// <summary>
        /// ָ��
        /// </summary>
        public CIndicator m_indicator;

        /// <summary>
        /// ����
        /// </summary>
        private static String FUNCTIONS = "BEEP,EXECUTE,MOUSEEVENT,MOUSECLICK,SETTEXT,SENDKEY,GETVALUE,";

        /// <summary>
        /// ǰ׺
        /// </summary>
        private static String PREFIX = "WIN.";

        /// <summary>
        /// ��ʼ����
        /// </summary>
        private const int STARTINDEX = 20000;

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>���</returns>
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
        /// ��ӷ���
        /// </summary>
        /// <param name="indicator">������</param>
        /// <param name="native">�ű�</param>
        /// <param name="xml">XML</param>
        /// <returns>ָ��</returns>
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
        /// Windows��������
        /// </summary_
        /// <param name="var">����</param>
        /// <returns>״̬</returns>
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
        /// Windows��ִ�г���
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>״̬</returns>
        private double WIN_EXECUTE(CVariable var)
        {
            WinHostEx.Execute(m_indicator.GetText(var.m_parameters[0]));
            return 1;
        }

        /// <summary>
        /// Windows�»�ȡ�ؼ���ֵ
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>��ֵ</returns>
        private double WIN_GETVALUE(CVariable var)
        {
            double value = 0;
            String text = WinHostEx.GetText();
            value = CStr.ConvertStrToDouble(text);
            return value;
        }

        /// <summary>
        /// Windows�µļ����¼�
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>״̬</returns>
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
        /// Windows�µ�����¼�
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>״̬</returns>
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
        /// Windows�µ�������¼�
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>״̬</returns>

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
        /// Windows����������
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>״̬</returns>
        private double WIN_SETTEXT(CVariable var)
        {
            WinHostEx.SetText(m_indicator.GetText(var.m_parameters[0]));
            return 1;
        }
    }
}