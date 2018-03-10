using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Windows.Forms;

namespace OwLib
{
    public class GaiaScript :UIScript
    {
        /// <summary>
        /// 创建脚本
        /// </summary>
        /// <param name="xml">XML对象</param>
        public GaiaScript(UIXml xml)
        {
            m_xml = xml;
        }

        /// <summary>
        /// Gaia对象
        /// </summary>
        private CIndicator m_gaia;

        /// <summary>
        /// Gaia文本
        /// </summary>
        private String m_text;

        private bool m_isDisposed = false;

        /// <summary>
        /// 获取是否被销毁
        /// </summary>
        public bool IsDisposed
        {
            get { return m_isDisposed; }
        }

        private UIXml m_xml;

        /// <summary>
        /// 获取或设置XML对象
        /// </summary>
        public UIXml Xml
        {
            get { return m_xml; }
            set { m_xml = value; }
        }

        /// <summary>
        /// 调用方法
        /// </summary>
        /// <param name="function">方法文本</param>
        /// <returns>返回值</returns>
        public String CallFunction(String function)
        {
            return m_gaia.CallFunction(function).ToString();
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public virtual void Dispose()
        {
            if (!m_isDisposed)
            {
                if (m_gaia != null)
                {
                    m_gaia.Dispose();
                }
                m_isDisposed = true;
            }
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="name">控件名称</param>
        /// <param name="propertyName">属性名称</param>
        /// <returns>属性值</returns>
        public String GetProperty(String name, String propertyName)
        {
            if (m_xml != null)
            {
                ControlA control = m_xml.FindControl(name);
                if (control != null)
                {
                    String value = null, type = null;
                    control.GetProperty(propertyName, ref value, ref type);
                    return value;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取调用者
        /// </summary>
        /// <returns>调用者名称</returns>
        public String GetSender()
        {
            if (m_xml != null)
            {
                UIEvent uiEvent = m_xml.Event;
                if (uiEvent != null)
                {
                    return uiEvent.Sender;
                }
            }
            return null;
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="name">控件ID</param>
        /// <param name="propertyName">属性名称</param>
        /// <param name="propertyValue">属性值</param>
        public void SetProperty(String name, String propertyName, String propertyValue)
        {
            if (m_xml != null)
            {
                ControlA control = m_xml.FindControl(name);
                if (control != null)
                {
                    control.SetProperty(propertyName, propertyValue);
                }
            }
        }

        /// <summary>
        /// 设置脚本
        /// </summary>
        /// <param name="text">脚本</param>
        public void SetText(String text)
        {
            m_text = text;
            m_gaia = CFunctionEx.CreateIndicator("", text, m_xml);
        }
    }
}
