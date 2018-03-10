using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using OwLib;

namespace OwLib
{
    /// <summary>
    /// 窗体
    /// </summary>
    public partial class MainForm : Form
    {
        #region Lord 2012/7/4
        /// <summary>
        ///  创建图形控件
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 控件管理器
        /// </summary>
        private WinHost m_host;

        /// <summary>
        /// 控件库
        /// </summary>
        private INativeBase m_native;

        /// <summary>
        /// 计时器
        /// </summary>
        private int m_tick = 60;

        /// <summary>
        /// XML
        /// </summary>
        private UIXmlEx m_xml;

        /// <summary>
        /// 获取客户端尺寸
        /// </summary>
        /// <returns>客户端尺寸</returns>
        public SIZE GetClientSize()
        {
            return new SIZE(ClientSize.Width, ClientSize.Height);
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="name">名称</param>
        public void LoadXml(String name)
        {
            if (name == "MainFrame")
            {
                m_xml = new MainFrame();
            }
            m_xml.CreateNative();
            m_native = m_xml.Native;
            m_native.Paint = new GdiPlusPaintEx();
            m_host = new WinHostEx();
            m_host.Native = m_native;
            m_native.Host = m_host;
            m_host.HWnd = Handle;
            m_native.AllowScaleSize = true;
            m_native.DisplaySize = new SIZE(ClientSize.Width, ClientSize.Height);
            m_xml.ResetScaleSize(GetClientSize());
            m_xml.Script = new GaiaScript(m_xml);
            m_xml.Native.ResourcePath = DataCenter.GetAppPath() + "\\config";
            m_xml.Load(DataCenter.GetAppPath() + "\\config\\" + name + ".html");
            m_host.ToolTip = new ToolTipA();
            m_host.ToolTip.Font = new FONT("SimSun", 20, true, false, false);
            (m_host.ToolTip as ToolTipA).InitialDelay = 250;
            m_native.Update();
            Invalidate();
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            m_xml.Exit();
            Environment.Exit(0);
            base.OnFormClosing(e);
        }

        /// <summary>
        /// 键盘事件
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            m_tick = 60;
        }

        /// <summary>
        /// 鼠标事件
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            m_tick = 60;
        }

        /// <summary>
        /// 尺寸改变方法
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (m_host != null)
            {
                m_xml.ResetScaleSize(GetClientSize());
                Invalidate();
            }
        }

        /// <summary>
        /// 鼠标滚动方法
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (m_host != null)
            {
                if (m_host.IsKeyPress(0x11))
                {
                    double scaleFactor = m_xml.ScaleFactor;
                    if (e.Delta > 0)
                    {
                        if (scaleFactor > 0.2)
                        {
                            scaleFactor -= 0.1;
                        }
                    }
                    else if (e.Delta < 0)
                    {
                        if (scaleFactor < 10)
                        {
                            scaleFactor += 0.1;
                        }
                    }
                    m_xml.ScaleFactor = scaleFactor;
                    m_xml.ResetScaleSize(GetClientSize());
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 秒表事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private void timer_Tick(object sender, EventArgs e)
        {
            m_tick--;
            if (m_tick <= 0)
            {
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// 消息监听
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m_host != null)
            {
                if (m_host.OnMessage(ref m) > 0)
                {
                    return;
                }
            }
            base.WndProc(ref m);
        }
        #endregion
    }
}
