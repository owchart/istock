/*******************************************************************************************\
*                                                                                           *
* CFunctionBase.cs -  Base functions, types, and definitions.                            *
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

namespace OwLib
{
    /// <summary>
    /// ������صĿ�
    /// </summary>
    public class CFunctionBase : CFunction
    {
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="indicator">ָ��</param>
        /// <param name="id">ID</param>
        /// <param name="name">����</param>
        /// <param name="withParameters">�Ƿ��в���</param>
        public CFunctionBase(CIndicator indicator, int id, String name)
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
        private static String FUNCTIONS = "IN,OUT,SLEEP";

        /// <summary>
        /// ǰ׺
        /// </summary>
        private static String PREFIX = "";

        /// <summary>
        /// ��ʼ����
        /// </summary>
        private const int STARTINDEX = 1000;

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
                    return IN(var);
                case STARTINDEX + 1:
                    return OUT(var);
                case STARTINDEX + 2:
                    return SLEEP(var);
                default: return 0;
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
                indicator.AddFunction(new CFunctionBase(indicator, STARTINDEX + i, PREFIX + functions[i]));
            }
        }

        /// <summary>
        /// ���뺯��
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>״̬</returns>
        private double IN(CVariable var)
        {
            CVariable newVar = new CVariable(m_indicator);
            newVar.m_expression = "'" + Console.ReadLine() + "'";
            m_indicator.SetVariable(var.m_parameters[0], newVar);
            return 0;
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>״̬</returns>
        private double OUT(CVariable var)
        {
            int len = var.m_parameters.Length;
            for (int i = 0; i < len; i++)
            {
                String text = m_indicator.GetText(var.m_parameters[i]);
                Console.Write(text);
            }
            Console.WriteLine("");
            return 0;
        }

        /// <summary>
        /// ˯��
        /// </summary>
        /// <param name="var">����</param>
        /// <returns>״̬</returns>
        private double SLEEP(CVariable var)
        {
            Thread.Sleep((int)m_indicator.GetValue(var.m_parameters[0]));
            return 1;
        }
    }
}
