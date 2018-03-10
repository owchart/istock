/*********************************************************************************\
*                                                                                 *
* WinHostEx.cs -Winform control host functions, types, and definitions.           *
*                                                                                 *
*               Version 6.00                                                      *
*                                                                                 *
*               Copyright (c) 2016-2016, iTeam. All rights reserved.             *
*               Created by Todd 2016/12/2.                                        *
*                                                                                 *
**********************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using OwLib;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace OwLib
{
    /// <summary>
    /// ���������
    /// </summary>
    public class WinHostEx : WinHost
    {
        #region Lord 2016/12/2
        [Flags]
        enum MouseEventFlag : uint
        {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            XDown = 0x0080,
            XUp = 0x0100,
            Wheel = 0x0800,
            VirtualDesk = 0x4000,
            Absolute = 0x8000
        }

        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_CHAR = 0x0102;
        public const int WM_SYSKEYDOWN = 0x0104;
        public const int WM_SYSKEYUP = 0x0105;
        public const int WM_SYSCHAR = 0x0106;

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(MouseEventFlag flags, int dx, int dy, int data, int extraInfo);

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(int hwnd, int wMsg, int wParam, int lParam);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(int hWnd, int Msg, int wParam, String lParam);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(int hWnd, int Msg, int wParam, StringBuilder lParam);

        [DllImport("user32.dll", EntryPoint = "WindowFromPoint")]
        private static extern IntPtr WindowFromPoint(int xPoint, int yPoint);

        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]
        public static extern bool GetCursorPos(ref POINT lpPoint);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern int FindWindow(String lpClassName, String lpWindowName);

        /// <summary>
        /// �����ڲ��ؼ�
        /// </summary>
        /// <param name="parent">���ؼ�</param>
        /// <param name="clsid">�ؼ���ʶ</param>
        /// <returns>�ڲ��ؼ�</returns>
        public override ControlA CreateInternalControl(ControlA parent, String clsid)
        {
            //�����ؼ�
            CalendarA calendar = parent as CalendarA;
            if (calendar != null)
            {
                if (clsid == "datetitle")
                {
                    return new DateTitle(calendar);
                }
                else if (clsid == "headdiv")
                {
                    HeadDiv headDiv = new HeadDiv(calendar);
                    headDiv.Width = parent.Width;
                    headDiv.Dock = DockStyleA.Top;
                    return headDiv;
                }
                else if (clsid == "lastbutton")
                {
                    return new ArrowButton(calendar);
                }
                else if (clsid == "nextbutton")
                {
                    ArrowButton nextBtn = new ArrowButton(calendar);
                    nextBtn.ToLast = false;
                    return nextBtn;
                }
            }
            //�ָ��
            SplitLayoutDivA splitLayoutDiv = parent as SplitLayoutDivA;
            if (splitLayoutDiv != null)
            {
                if (clsid == "splitter")
                {
                    ButtonA splitter = new ButtonA();
                    splitter.BackColor = COLOR.CONTROLBORDER;
                    splitter.BorderColor = COLOR.CONTROLBORDER;
                    splitter.Size = new SIZE(5, 5);
                    return splitter;
                }
            }
            //������
            ScrollBarA scrollBar = parent as ScrollBarA;
            if (scrollBar != null)
            {
                scrollBar.BorderColor = COLOR.EMPTY;
                scrollBar.BackColor = COLOR.EMPTY;
                if (clsid == "addbutton")
                {
                    RibbonButton addButton = new RibbonButton();
                    addButton.Size = new SIZE(15, 15);
                    if (scrollBar is HScrollBarA)
                    {
                        addButton.ArrowType = 2;
                    }
                    else if (scrollBar is VScrollBarA)
                    {
                        addButton.ArrowType = 4;
                    }
                    return addButton;
                }
                else if (clsid == "backbutton")
                {
                    ButtonA backButton = new ButtonA();
                    backButton.BorderColor = COLOR.EMPTY;
                    backButton.BackColor = COLOR.EMPTY;
                    return backButton;
                }
                else if (clsid == "scrollbutton")
                {
                    RibbonButton scrollButton = new RibbonButton();
                    scrollButton.AllowDrag = true;
                    if (scrollBar is VScrollBarA)
                    {
                        scrollButton.Angle = 0;
                    }
                    return scrollButton;
                }
                else if (clsid == "reducebutton")
                {
                    RibbonButton reduceButton = new RibbonButton();
                    reduceButton.Size = new SIZE(15, 15);
                    if (scrollBar is HScrollBarA)
                    {
                        reduceButton.ArrowType = 1;
                    }
                    else if (scrollBar is VScrollBarA)
                    {
                        reduceButton.ArrowType = 3;
                    }
                    return reduceButton;
                }
            }
            //ҳ��
            TabPageA tabPage = parent as TabPageA;
            if (tabPage != null)
            {
                if (clsid == "headerbutton")
                {
                    RibbonButton button = new RibbonButton();
                    button.AllowDrag = true;
                    SIZE size = new SIZE(100, 20);
                    button.Size = size;
                    return button;
                }
            }
            //�����б�
            ComboBoxA comboBox = parent as ComboBoxA;
            if (comboBox != null)
            {
                if (clsid == "dropdownbutton")
                {
                    RibbonButton dropDownButton = new RibbonButton();
                    dropDownButton.ArrowType = 4;
                    dropDownButton.DisplayOffset = false;
                    int width = comboBox.Width;
                    int height = comboBox.Height;
                    POINT location = new POINT(width - 20, 0);
                    dropDownButton.Location = location;
                    SIZE size = new SIZE(20, height);
                    dropDownButton.Size = size;
                    return dropDownButton;
                }
                else if (clsid == "dropdownmenu")
                {
                    ComboBoxMenu comboBoxMenu = new ComboBoxMenu();
                    comboBoxMenu.ComboBox = comboBox;
                    comboBoxMenu.Popup = true;
                    SIZE size = new SIZE(100, 200);
                    comboBoxMenu.Size = size;
                    return comboBoxMenu;
                }
            }
            //����ѡ��
            DateTimePickerA datePicker = parent as DateTimePickerA;
            if (datePicker != null)
            {
                if (clsid == "dropdownbutton")
                {
                    RibbonButton dropDownButton = new RibbonButton();
                    dropDownButton.ArrowType = 4;
                    dropDownButton.DisplayOffset = false;
                    int width = datePicker.Width;
                    int height = datePicker.Height;
                    POINT location = new POINT(width - 16, 0);
                    dropDownButton.Location = location;
                    SIZE size = new SIZE(16, height);
                    dropDownButton.Size = size;
                    return dropDownButton;
                }
                else if (clsid == "dropdownmenu")
                {
                    MenuA dropDownMenu = new MenuA();
                    dropDownMenu.Padding = new PADDING(1);
                    dropDownMenu.Popup = true;
                    SIZE size = new SIZE(200, 200);
                    dropDownMenu.Size = size;
                    return dropDownMenu;
                }
            }
            //����ѡ��
            SpinA spin = parent as SpinA;
            if (spin != null)
            {
                if (clsid == "downbutton")
                {
                    RibbonButton downButton = new RibbonButton();
                    downButton.DisplayOffset = false;
                    downButton.ArrowType = 4;
                    SIZE size = new SIZE(16, 16);
                    downButton.Size = size;
                    return downButton;
                }
                else if (clsid == "upbutton")
                {
                    RibbonButton upButton = new RibbonButton();
                    upButton.DisplayOffset = false;
                    upButton.ArrowType = 3;
                    SIZE size = new SIZE(16, 16);
                    upButton.Size = size;
                    return upButton;
                }
            }
            //������
            DivA div = parent as DivA;
            if (div != null)
            {
                if (clsid == "hscrollbar")
                {
                    HScrollBarA hScrollBar = new HScrollBarA();
                    hScrollBar.Visible = false;
                    hScrollBar.Size = new SIZE(10, 10);
                    return hScrollBar;
                }
                else if (clsid == "vscrollbar")
                {
                    VScrollBarA vScrollBar = new VScrollBarA();
                    vScrollBar.Visible = false;
                    vScrollBar.Size = new SIZE(10, 10);
                    return vScrollBar;
                }
            }
            //���
            GridA grid = parent as GridA;
            if (grid != null)
            {
                if (clsid == "edittextbox")
                {
                    return new TextBoxA();
                }
            }
            return base.CreateInternalControl(parent, clsid);
        }

        /// <summary>
        /// ִ�г���
        /// </summary>
        /// <param name="cmd">����</param>
        public static void Execute(String cmd)
        {
            try
            {
                Process.Start(cmd);
            }
            catch { }
        }

        /// <summary>
        /// ��ȡ�ı�
        /// </summary>
        /// <returns>�ı�</returns>
        public static String GetText()
        {
            POINT mp = new POINT();
            GetCursorPos(ref mp);
            IntPtr handle = WindowFromPoint(mp.x, mp.y);
            if (handle != IntPtr.Zero)
            {
                StringBuilder sb = new StringBuilder(10240);
                SendMessage((int)handle, 0xD, 10240, sb);
                return sb.ToString();
            }
            return "";
        }

        /// <summary>
        /// ��������¼�
        /// </summary>
        /// <param name="eventID">�¼�ID</param>
        /// <param name="dx">������</param>
        /// <param name="dy">������</param>
        /// <param name="data">����ֵ</param>
        public static void MouseEvent(String eventID, int dx, int dy, int data)
        {
            MouseEventFlag flag = MouseEventFlag.Move;
            if (eventID == "SETCURSOR")
            {
                SetCursorPos(dx, dy);
                return;
            }
            else if (eventID == "MOVE")
            {
                flag = MouseEventFlag.Move;
            }
            else if (eventID == "LEFTDOWN")
            {
                flag = MouseEventFlag.LeftDown;
            }
            else if (eventID == "LEFTUP")
            {
                flag = MouseEventFlag.LeftUp;
            }
            else if (eventID == "RIGHTDOWN")
            {
                flag = MouseEventFlag.RightDown;
            }
            else if (eventID == "RIGHTUP")
            {
                flag = MouseEventFlag.RightUp;
            }
            else if (eventID == "MIDDLEDOWN")
            {
                flag = MouseEventFlag.MiddleDown;
            }
            else if (eventID == "MIDDLEUP")
            {
                flag = MouseEventFlag.MiddleUp;
            }
            else if (eventID == "XDOWN")
            {
                flag = MouseEventFlag.XDown;
            }
            else if (eventID == "XUP")
            {
                flag = MouseEventFlag.XUp;
            }
            else if (eventID == "WHEEL")
            {
                flag = MouseEventFlag.Wheel;
            }
            else if (eventID == "VIRTUALDESK")
            {
                flag = MouseEventFlag.VirtualDesk;
            }
            else if (eventID == "ABSOLUTE")
            {
                flag = MouseEventFlag.Absolute;
            }
            mouse_event(flag, dx, dy, data, 0);
        }

        /// <summary>
        /// ���������¼�
        /// </summary>
        /// <param name="key">����</param>
        public static void SendKey(String key)
        {
            SendKeys.Send(key);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="text">����</param>
        public static void SetText(String text)
        {
            if (text != null && text.Length > 0)
            {
                Clipboard.Clear();
                Clipboard.SetText(text);
                SendKeys.SendWait("^v");
            }
        }

        /// <summary>
        /// ��ʾToolTip
        /// </summary>
        /// <param name="text">����</param>
        /// <param name="mp">λ��</param>
        public override void ShowToolTip(String text, POINT mp)
        {
            //toolTip.Show(text, Control.FromHandle(HWnd), new Point(mp.x, mp.y));
            base.ShowToolTip(text, mp);
        }
        #endregion
    }
}
