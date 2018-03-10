/*******************************************************************************************\
*                                                                                           *
* CFunctionUI.cs -  UI functions, types, and definitions.                            *
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
    public class CFunctionUI : CFunction
    {
        /// <summary>
        /// 创建方法
        /// </summary>
        /// <param name="indicator">指标</param>
        /// <param name="id">ID</param>
        /// <param name="name">名称</param>
        /// <param name="withParameters">是否有参数</param>
        public CFunctionUI(CIndicator indicator, int id, String name, UIXml xml)
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
        /// 方法
        /// </summary>
        private static String FUNCTIONS = "GETPROPERTY,SETPROPERTY,GETSENDER,ALERT,INVALIDATE,SHOWWINDOW,CLOSEWINDOW,STARTTIMER,STOPTIMER,GETMOUSEBUTTON,GETMOUSEPOINT,GETCLICKS,GETKEY,GETCOOKIE,SETCOOKIE,SHOWRIGHTMENU,ADDBARRAGE,UPDATE";

        /// <summary>
        /// 前缀
        /// </summary>
        private static String PREFIX = "";

        /// <summary>
        /// 开始索引
        /// </summary>
        private const int STARTINDEX = 2000;

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
                    return GETPROPERTY(var);
                case STARTINDEX + 1:
                    return SETPROPERTY(var);
                case STARTINDEX + 2:
                    return GETSENDER(var);
                case STARTINDEX + 3:
                    return ALERT(var);
                case STARTINDEX + 4:
                    return INVALIDATE(var);
                case STARTINDEX + 5:
                    return SHOWWINDOW(var);
                case STARTINDEX + 6:
                    return CLOSEWINDOW(var);
                case STARTINDEX + 7:
                    return STARTTIMER(var);
                case STARTINDEX + 8:
                    return STOPTIMER(var);
                case STARTINDEX + 9:
                    return GETMOUSEBUTTON(var);
                case STARTINDEX + 10:
                    return GETMOUSEPOINT(var);
                case STARTINDEX + 11:
                    return GETCLICKS(var);
                case STARTINDEX + 12:
                    return GETKEY(var);
                case STARTINDEX + 13:
                    return GETCOOKIE(var);
                case STARTINDEX + 14:
                    return SETCOOKIE(var);
                case STARTINDEX + 15:
                    return SHOWRIGHTMENU(var);
                case STARTINDEX + 16:
                    return ADDBARRAGE(var);
                case STARTINDEX + 17:
                    return UPDATE(var);
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
        public static void AddFunctions(CIndicator indicator, UIXml xml)
        {
            String[] functions = FUNCTIONS.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            int functionsSize = functions.Length;
            for (int i = 0; i < functionsSize; i++)
            {
                indicator.AddFunction(new CFunctionUI(indicator, STARTINDEX + i, PREFIX + functions[i], xml));
            }
        }

        /// <summary>
        /// 提示
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>结果</returns>
        public double ADDBARRAGE(CVariable var)
        {
            String text = "";
            int len = var.m_parameters.Length;
            for (int i = 0; i < len; i++)
            {
                text += m_indicator.GetText(var.m_parameters[i]);
            }
            BarrageDiv barrageDiv = (m_xml as MainFrame).FindControl("divBarrage") as BarrageDiv;
            Barrage barrage = new Barrage();
            barrage.Text = text;
            barrage.Mode = 0;
            barrageDiv.AddBarrage(barrage);
            return 1;
        }

        /// <summary>
        /// 弹出提示
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double ALERT(CVariable var)
        {
            double result = 0;
            int len = var.m_parameters.Length;
            if (len == 1)
            {
                if (DialogResult.OK == MessageBox.Show(m_indicator.GetText(var.m_parameters[0])))
                {
                    result = 1;
                }
            }
            else
            {
                if (DialogResult.OK == MessageBox.Show(m_indicator.GetText(var.m_parameters[0]),
                    m_indicator.GetText(var.m_parameters[1])))
                {
                    result = 1;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取点击次数
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        public int GETCLICKS(CVariable var)
        {
            return m_xml.Event.Clicks;
        }

        /// <summary>
        /// 获取按键
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        public int GETKEY(CVariable var)
        {
            return (int)m_xml.Event.Key;
        }

        /// <summary>
        /// 获取鼠标按键
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        public int GETMOUSEBUTTON(CVariable var)
        {
            if (m_xml.Event.MouseButton == MouseButtonsA.Left)
            {
                return 1;
            }
            else if (m_xml.Event.MouseButton == MouseButtonsA.Right)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取鼠标按键
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        public int GETMOUSEPOINT(CVariable var)
        {
            POINT mousePoint = m_xml.Event.MousePoint;
            CVariable newVar = new CVariable(m_indicator);
            newVar.m_expression = mousePoint.x.ToString();
            m_indicator.SetVariable(var.m_parameters[0], newVar);
            CVariable newVar2 = new CVariable(m_indicator);
            newVar2.m_expression = mousePoint.y.ToString();
            m_indicator.SetVariable(var.m_parameters[1], newVar2);
            return 1;
        }


        /// <summary>
        /// 获取COOKIE
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double GETCOOKIE(CVariable var)
        {
            String cookieName = m_indicator.GetText(var.m_parameters[1]);
            UserCookieService cookieService = DataCenter.UserCookieService;
            UserCookie cookie = new UserCookie();
            if (cookieService.GetCookie(cookieName, ref cookie) > 0)
            {
                CVariable newVar = new CVariable(m_indicator);
                newVar.m_expression = "'" + cookie.m_value + "'";
                m_indicator.SetVariable(var.m_parameters[0], newVar);
                return 1;
            }
            return 0;
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        public double GETPROPERTY(CVariable var)
        {
            GaiaScript gaiaScript = m_xml.Script as GaiaScript;
            String name = m_indicator.GetText(var.m_parameters[1]);
            String propertyName = m_indicator.GetText(var.m_parameters[2]);
            String text = gaiaScript.GetProperty(name, propertyName);
            CVariable newVar = new CVariable(m_indicator);
            newVar.m_expression = "'" + text + "'";
            m_indicator.SetVariable(var.m_parameters[0], newVar);
            return 0;
        }

        /// <summary>
        /// 获取调用者
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        public double GETSENDER(CVariable var)
        {
            GaiaScript gaiaScript = m_xml.Script as GaiaScript;
            String text = gaiaScript.GetSender();
            CVariable newVar = new CVariable(m_indicator);
            newVar.m_expression = "'" + text + "'";
            m_indicator.SetVariable(var.m_parameters[0], newVar);
            return 0;
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double INVALIDATE(CVariable var)
        {
            if (m_xml != null)
            {
                int pLen = var.m_parameters != null ? var.m_parameters.Length : 0;
                if (pLen == 0)
                {
                    m_xml.Native.Invalidate();
                }
                else
                {
                    ControlA control = m_xml.FindControl(m_indicator.GetText(var.m_parameters[0]));
                    if (control != null)
                    {
                        control.Invalidate();
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// 设置COOKIE
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>结果</returns>
        private double SETCOOKIE(CVariable var)
        {
            String cookieName = m_indicator.GetText(var.m_parameters[0]);
            UserCookieService cookieService = DataCenter.UserCookieService;
            UserCookie cookie = new UserCookie();
            cookie.m_key = cookieName;
            cookie.m_value = m_indicator.GetText(var.m_parameters[1]);
            return cookieService.AddCookie(cookie);
        }


        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double SETPROPERTY(CVariable var)
        {
            GaiaScript gaiaScript = m_xml.Script as GaiaScript;
            String name = m_indicator.GetText(var.m_parameters[0]);
            String propertyName = m_indicator.GetText(var.m_parameters[1]);
            String propertyValue = m_indicator.GetText(var.m_parameters[2]);
            gaiaScript.SetProperty(name, propertyName, propertyValue);
            return 0;
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double CLOSEWINDOW(CVariable var)
        {
           WindowXmlEx windowXmlEx = m_xml as WindowXmlEx;
            if (windowXmlEx != null)
            {
                windowXmlEx.Close();
            }
            return 0;
        }

        /// <summary>
        /// 显示右键菜单
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double SHOWRIGHTMENU(CVariable var)
        {
            GaiaScript gaiaScript = m_xml.Script as GaiaScript;
            INativeBase native = m_xml.Native;
            ControlA control = m_xml.FindControl(gaiaScript.GetSender());
            int clientX = native.ClientX(control);
            int clientY = native.ClientY(control);
            MenuA menu = m_xml.GetMenu(m_indicator.GetText(var.m_parameters[0]));
            menu.Location = new POINT(clientX, clientY + control.Height);
            menu.Visible = true;
            menu.Focused = true;
            menu.BringToFront();
            native.Invalidate();
            return 0;
        }

        /// <summary>
        /// 显示窗体
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double SHOWWINDOW(CVariable var)
        {
            String xmlName = m_indicator.GetText(var.m_parameters[0]);
            String windowName = m_indicator.GetText(var.m_parameters[1]);
            WindowXmlEx window = new WindowXmlEx();
            window.Load(m_xml.Native, xmlName, windowName);
            window.Show();
            if (xmlName == "HotKeyWindow")
            {
                TextBoxA txtKey = window.GetTextBox("txtHotKey");
                txtKey.Text = "本软件有如下快捷键：\r1. F1-F3  --> 快速出价100-300元\r2. F4-F10 --> 快速伏击400-1000元\r3. F11    --> 出价   \rHome   --> 伏击   \rEnd    --> 取消出价   \rEnter  --> 提交验证码   \rEsc    --> 手动断网重连4. 方向键【↑】及【↓】快速调整价格";
            }
            return 0;
        }

        /// <summary>
        /// 开始秒表
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double STARTTIMER(CVariable var)
        {
            ControlA control = m_xml.FindControl(m_indicator.GetText(var.m_parameters[0]));
            control.StartTimer((int)m_indicator.GetValue(var.m_parameters[1]), (int)m_indicator.GetValue(var.m_parameters[2]));
            return 0;
        }

        /// <summary>
        /// 结束秒表
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double STOPTIMER(CVariable var)
        {
            ControlA control = m_xml.FindControl(m_indicator.GetText(var.m_parameters[0]));
            control.StopTimer((int)m_indicator.GetValue(var.m_parameters[1]));
            return 0;
        }

        /// <summary>
        /// 更新布局
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>状态</returns>
        private double UPDATE(CVariable var)
        {
            if (m_xml != null)
            {
                int pLen = var.m_parameters != null ? var.m_parameters.Length : 0;
                if (pLen == 0)
                {
                    m_xml.Native.Update();
                }
                else
                {
                    ControlA control = m_xml.FindControl(m_indicator.GetText(var.m_parameters[0]));
                    if (control != null)
                    {
                        control.Update();
                    }
                }
            }
            return 0;
        }
    }
}