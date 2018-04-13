using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace OwLib
{
    /// <summary>
    /// 自动交易
    /// </summary>
    public class AutoTradeService
    {
        [Flags]
        public enum MouseEventFlag : uint
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


        [Flags]
        public enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0000,
            SMTO_BLOCK = 0x0001,
            SMTO_ABORTIFHUNG = 0x0002,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x0008
        }

        /// <summary>
        /// 坐标
        /// </summary>
        public struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct TVITEM
        {
            public int mask;
            public IntPtr hItem;
            public int state;
            public int stateMask;
            public IntPtr pszText;
            public int cchTextMax;
            public int iImage;
            public int iSelectedImage;
            public int cChildren;
            public IntPtr lParam;
            public IntPtr HTreeItem;
        }

        /// <summary>
        /// 区域
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        /// <summary>
        /// 窗体信息
        /// </summary>
        public class WindowInfo
        {
            public String m_className;
            public int m_hWnd;
            public RECT m_rect;
            public String m_text;
            public String m_parentWnd;
        }

        #region COMMAND
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_CHAR = 0x0102;
        public const int WM_SYSKEYDOWN = 0x0104;
        public const int WM_SYSKEYUP = 0x0105;
        public const int WM_SYSCHAR = 0x0106;
        private const int WM_ERASEBKGND = 0x0014;
        private const int WM_PAINT = 0x000F;
        private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_RBUTTONDOWN = 0x0204;
        private const int WM_LBUTTONDBLCLK = 0x0203;
        private const int WM_RBUTTONDBLCLK = 0x0206;
        private const int WM_LBUTTONUP = 0x0202;
        private const int WM_RBUTTONUP = 0x0205;
        private const int WM_MOUSEMOVE = 0x0200;
        private const int WM_MOUSEWHEEL = 0x020A;
        private const int WM_TIMER = 0x0113;
        private const int WM_IME_SETCONTEXT = 0x0281;
        private const int WM_IME_CHAR = 0x0286;
        private const int GCS_COMPSTR = 0x0008;
        private const int HC_ACTION = 0;
        private const int WM_HOTKEY = 0x0312;
        private const int PM_REMOVE = 0x0001;
        public const int VK_BACK = 0x8;
        public const int VK_TAB = 0x9;
        public const int VK_CLEAR = 0xC;
        public const int VK_RETURN = 0xD;
        public const int VK_SHIFT = 0x10;
        public const int VK_CONTROL = 0x11;
        public const int VK_C = 0x43;

        #region SysTreeView32

        public const uint MEM_COMMIT = 0x1000;
        public const uint MEM_RELEASE = 0x8000;

        public const uint MEM_RESERVE = 0x2000;
        public const uint PAGE_READWRITE = 4;

        public const uint PROCESS_VM_OPERATION = 0x0008;
        public const uint PROCESS_VM_READ = 0x0010;
        public const uint PROCESS_VM_WRITE = 0x0020;

        public const int TV_FIRST = 0x1100;
        public const int TVM_GETCOUNT = TV_FIRST + 5;
        public const int TVM_GETNEXTITEM = TV_FIRST + 10;
        public const int TVM_GETITEMA = TV_FIRST + 12;
        public const int TVM_GETITEMW = TV_FIRST + 62;

        public const int TVGN_ROOT = 0x0000;
        public const int TVGN_NEXT = 0x0001;
        public const int TVGN_PREVIOUS = 0x0002;
        public const int TVGN_PARENT = 0x0003;
        public const int TVGN_CHILD = 0x0004;
        public const int TVGN_FIRSTVISIBLE = 0x0005;
        public const int TVGN_NEXTVISIBLE = 0x0006;
        public const int TVGN_PREVIOUSVISIBLE = 0x0007;
        public const int TVGN_DROPHILITE = 0x0008;
        public const int TVGN_CARET = 0x0009;
        public const int TVGN_LASTVISIBLE = 0x000A;

        public const int TVIF_TEXT = 0x0001;
        public const int TVIF_IMAGE = 0x0002;
        public const int TVIF_PARAM = 0x0004;
        public const int TVIF_STATE = 0x0008;
        public const int TVIF_HANDLE = 0x0010;
        public const int TVIF_SELECTEDIMAGE = 0x0020;
        public const int TVIF_CHILDREN = 0x0040;
        public const int TVIF_INTEGRAL = 0x0080;

        public const int TVM_SELECTITEM = 0x110B;
        #endregion
        #endregion

        [DllImport("user32.dll")]
        private static extern void mouse_event(MouseEventFlag flags, int dx, int dy, int data, int extraInfo);

        //消息发送API
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(int hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(int hwnd, int wMsg, int wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(int hwnd, int wMsg, int wParam, long lParam);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(int hWnd, int Msg, int wParam, String lParam);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(int hWnd, int Msg, int wParam, StringBuilder lParam);

        [DllImport("user32.dll", EntryPoint = "WindowFromPoint")]
        private static extern IntPtr WindowFromPoint(int xPoint, int yPoint);

        [DllImport("user32.dll", EntryPoint = "GetTopWindow")]
        private static extern int GetTopWindow(int hWnd);

        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]
        public static extern bool GetCursorPos(ref POINT lpPoint);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern int FindWindow(String lpClassName, String lpWindowName);

        [DllImport("User32.dll", EntryPoint = "FindWindowEx")]
        private static extern int FindWindowEx(int hwndParent, int hwndChildAfter, String lpszClass, String lpszWindow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        public delegate bool CallBack(int hwnd, int lParam);

        [DllImport("user32")]
        public static extern int EnumWindows(CallBack x, int y);

        [DllImport("user32.dll")]
        public static extern int EnumChildWindows(int hWndParent, CallBack lpfn, int lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetClassName(int hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(int hWnd);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessageTimeout(
           IntPtr windowHandle,
           uint Msg,
           IntPtr wParam,
           String lParam,
           SendMessageTimeoutFlags flags,
           uint timeout,
           out IntPtr result
        );

        #region SysTreeView32

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll")]
        public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint dwFreeType);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);
        #endregion

        /// <summary>
        /// 窗体信息
        /// </summary>
        private static List<WindowInfo> m_allWindows = new List<WindowInfo>();

        /// <summary>
        /// 子窗体信息
        /// </summary>
        private static List<WindowInfo> m_childWindows = new List<WindowInfo>();

        /// <summary>
        /// 交易信息
        /// </summary>
        private static List<OrderInfo> m_orderInfos = new List<OrderInfo>();

        // 树的句柄
        private static int m_treeHandle = -1;

        // 树节点文本和句柄字典
        private static IDictionary<String, TVITEM> m_treeViewTextHandleMapping = new Dictionary<String, TVITEM>();

        /////////////////////////////////////////////////////////////////////////////////////////////////////

        #region SysTreeView32

        /// <summary>
        /// 获取树节点的显示名称
        /// </summary>
        /// <param name="aHandle"></param>
        /// <param name="treeTextHandleMapping"></param>
        /// <returns></returns>
        public static bool GetTreeViewText(IntPtr aHandle, IDictionary<String, TVITEM> treeTextHandleMapping)
        {
            uint vProcessId;
            GetWindowThreadProcessId(aHandle, out vProcessId);

            IntPtr vProcess = OpenProcess(PROCESS_VM_OPERATION | PROCESS_VM_READ |
                PROCESS_VM_WRITE, false, vProcessId);
            IntPtr vPointer = VirtualAllocEx(vProcess, IntPtr.Zero, 4096,
                MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);
            try
            {
                uint vItemCount = TreeView_GetCount(aHandle);
                IntPtr vTreeItem = TreeView_GetRoot(aHandle);
                Console.WriteLine(vItemCount);
                for (int i = 0; i < vItemCount; i++)
                {
                    byte[] vBuffer = new byte[256];
                    TVITEM[] vItem = new TVITEM[1];
                    vItem[0] = new TVITEM();
                    vItem[0].mask = TVIF_TEXT;
                    vItem[0].hItem = vTreeItem;
                    vItem[0].pszText = (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(TVITEM)));
                    vItem[0].cchTextMax = vBuffer.Length;
                    uint vNumberOfBytesRead = 0;
                    WriteProcessMemory(vProcess, vPointer,
                        Marshal.UnsafeAddrOfPinnedArrayElement(vItem, 0),
                        Marshal.SizeOf(typeof(TVITEM)), ref vNumberOfBytesRead);
                    SendMessage((int)aHandle, TVM_GETITEMA, 0, (int)vPointer);
                    ReadProcessMemory(vProcess,
                        (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(TVITEM))),
                        Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0),
                        vBuffer.Length, ref vNumberOfBytesRead);
                    String nodeText = Marshal.PtrToStringAnsi(
                        Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0));
                    Console.WriteLine(nodeText);
                    treeTextHandleMapping[nodeText] = vItem[0];
                    vTreeItem = TreeNodeGetNext(aHandle, vTreeItem);
                }
            }
            finally
            {
                VirtualFreeEx(vProcess, vPointer, 0, MEM_RELEASE);
                CloseHandle(vProcess);
            }
            return true;
        }

        /// <summary>
        /// 获取下一个节点
        /// </summary>
        /// <param name="aHandle"></param>
        /// <param name="aTreeItem"></param>
        /// <returns></returns>
        public static IntPtr TreeNodeGetNext(IntPtr aHandle, IntPtr aTreeItem)
        {
            if (aHandle == IntPtr.Zero || aTreeItem == IntPtr.Zero) return IntPtr.Zero;
            IntPtr result = TreeView_GetChild(aHandle, aTreeItem);
            if (result == IntPtr.Zero)
                result = TreeView_GetNextSibling(aHandle, aTreeItem);

            IntPtr vParentID = aTreeItem;
            while (result == IntPtr.Zero && vParentID != IntPtr.Zero)
            {
                vParentID = TreeView_GetParent(aHandle, vParentID);
                result = TreeView_GetNextSibling(aHandle, vParentID);
            }
            return result;
        }

        /// <summary>
        /// 获取某个节点的子节点
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="hitem"></param>
        /// <returns></returns>
        public static IntPtr TreeView_GetChild(IntPtr hwnd, IntPtr hitem)
        {
            return TreeView_GetNextItem(hwnd, hitem, TVGN_CHILD);
        }

        /// <summary>
        /// 获取树节点的个数
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        public static uint TreeView_GetCount(IntPtr hwnd)
        {
            return (uint)SendMessage((int)hwnd, TVM_GETCOUNT, 0, 0);
        }

        /// <summary>
        /// 获取树下一个节点
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="hitem"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static IntPtr TreeView_GetNextItem(IntPtr hwnd, IntPtr hitem, int code)
        {
            return (IntPtr)SendMessage((int)hwnd, TVM_GETNEXTITEM, code, (int)hitem);
        }

        /// <summary>
        /// 获取某个节点的兄弟节点
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="hitem"></param>
        /// <returns></returns>
        public static IntPtr TreeView_GetNextSibling(IntPtr hwnd, IntPtr hitem)
        {
            return TreeView_GetNextItem(hwnd, hitem, TVGN_NEXT);
        }

        /// <summary>
        /// 获取某个节点的父节点
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="hitem"></param>
        /// <returns></returns>
        public static IntPtr TreeView_GetParent(IntPtr hwnd, IntPtr hitem)
        {
            return TreeView_GetNextItem(hwnd, hitem, TVGN_PARENT);
        }

        /// <summary>
        /// 获取树的根目录
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        public static IntPtr TreeView_GetRoot(IntPtr hwnd)
        {
            return TreeView_GetNextItem(hwnd, IntPtr.Zero, TVGN_ROOT);
        }
        #endregion

        /// <summary>
        /// 获取子窗体信息
        /// </summary>
        /// <param name="windowInfos">窗体信息</param>
        /// <returns>状态</returns>
        public static void GetChildWindows(int hWnd, List<WindowInfo> childWindows)
        {
            EnumChildWindows(hWnd, new CallBack(GetWindowsCallBack), 0);
            int childWindowsSize = m_childWindows.Count;
            for (int i = 0; i < childWindowsSize; i++)
            {
                childWindows.Add(m_childWindows[i]);
            }
            m_childWindows.Clear();
        }

        /// <summary>
        /// 窗体句柄回调
        /// </summary>
        /// <param name="hwnd">句柄</param>
        /// <param name="lParam">参数</param>
        /// <returns>状态</returns>
        public static bool GetChildWindowsCallBack(int hwnd, int lParam)
        {
            RECT rect = new RECT();
            GetWindowRect((IntPtr)hwnd, ref rect);
            WindowInfo windowInfo = new WindowInfo();
            windowInfo.m_hWnd = hwnd;
            windowInfo.m_rect = rect;
            windowInfo.m_text = GetText(hwnd);
            m_allWindows.Add(windowInfo);
            return true;
        }

        /// <summary>
        /// 获取文本
        /// </summary>
        /// <returns>文本</returns>
        public static String GetText(int hWnd)
        {
            if (hWnd != 0)
            {
                IntPtr result1;
                SendMessageTimeout((IntPtr)hWnd, 0xD, IntPtr.Zero, "Environment", SendMessageTimeoutFlags.SMTO_ABORTIFHUNG, 2000, out result1);
                StringBuilder sb = new StringBuilder(10240);
                SendMessage((int)hWnd, 0xD, 10240, sb);
                return sb.ToString();
            }
            return "";
        }

        /// <summary>
        /// 获取窗体
        /// </summary>
        /// <returns>状态</returns>
        public static void GetWindows(List<WindowInfo> windows)
        {
            EnumWindows(new CallBack(GetChildWindowsCallBack), 0);
            int allWindowsSize = m_allWindows.Count;
            for (int i = 0; i < allWindowsSize; i++)
            {
                windows.Add(m_allWindows[i]);
            }
            m_allWindows.Clear();
        }

        /// <summary>
        /// 子窗体句柄回调
        /// </summary>
        /// <param name="hwnd">句柄</param>
        /// <param name="lParam">参数</param>
        /// <returns>状态</returns>
        public static bool GetWindowsCallBack(int hwnd, int lParam)
        {
            StringBuilder sb = new StringBuilder(256);
            GetClassName(hwnd, sb, 256);
            RECT rect = new RECT();
            GetWindowRect((IntPtr)hwnd, ref rect);
            WindowInfo windowInfo = new WindowInfo();
            windowInfo.m_className = sb.ToString();
            windowInfo.m_hWnd = hwnd;
            windowInfo.m_rect = rect;
            windowInfo.m_text = GetText(hwnd);
            m_childWindows.Add(windowInfo);
            //EnumChildWindows(hwnd, new CallBack(GetWindowsCallBack), lParam);
            return true;
        }

        /// <summary>
        /// 触发鼠标事件
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <param name="dx">横坐标</param>
        /// <param name="dy">纵坐标</param>
        /// <param name="data">滚轮值</param>
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
        /// 设置文字
        /// </summary>
        /// <param name="text">文字</param>
        public static void SetText(int hWnd, String text)
        {
            SendMessage(hWnd, 0x000C, 0, text);
        }

        /// <summary>
        /// 触发键盘事件
        /// </summary>
        /// <param name="key">命令</param>
        public static void SendKey(int key)
        {
            SendKeys.SendWait(((char)key).ToString());
        }

        /// <summary>
        /// 发送按键
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static int SendKey(String cmd)
        {
            int intKey = Convert.ToInt32(cmd);
            SendKeys.SendWait(((char)intKey).ToString());
            return 1;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 添加交易
        /// </summary>
        /// <param name="code">代码</param>
        /// <param name="price">价格</param>
        /// <param name="qty">成交量</param>
        public static void AddOrder(String code, float price, int qty)
        {
            OrderInfo orderInfo = new OrderInfo();
            orderInfo.m_code = code;
            orderInfo.m_price = price;
            orderInfo.m_qty = qty;
            m_orderInfos.Add(orderInfo);
        }

        /// <summary>
        /// 买入
        /// </summary>
        /// <param name="info"></param>
        public static void Buy(OrderInfo info)
        {
            List<WindowInfo> windowInfos = new List<WindowInfo>();
            GetWindows(windowInfos);
            int windowInfosSize = windowInfos.Count;
            for (int i = 0; i < windowInfosSize; i++)
            {
                //同花顺下单
                if (windowInfos[i].m_text == "网上股票交易系统5.0")
                {
                    #region 同花顺下单
                    int hWnd = windowInfos[i].m_hWnd;
                    SetForegroundWindow(hWnd);
                    Thread.Sleep(100);
                    AutoTradeService.ClickTreeMenu("买入[F1]");
                    Thread.Sleep(100);

                    List<WindowInfo> childWindowInfos = new List<WindowInfo>();
                    GetChildWindows(hWnd, childWindowInfos);
                    int childWindowInfosSize = childWindowInfos.Count;
                    int buyHandleIndex = -1;
                    for (int j = 0; j < childWindowInfosSize; j++)
                    {
                        WindowInfo childWindowInfo = childWindowInfos[j];
                        if (childWindowInfo.m_className == "Button")
                        {
                            if (childWindowInfo.m_text == "买入[B]")
                            {
                                buyHandleIndex = j;
                                break;
                            }
                        }
                    }
                    if (buyHandleIndex >= 5)
                    {
                        WindowInfo buyButtonInfo = childWindowInfos[buyHandleIndex];
                        WindowInfo securityCodeInfo = childWindowInfos[buyHandleIndex - 5];
                        WindowInfo priceInfo = childWindowInfos[buyHandleIndex - 3];
                        WindowInfo buyCountCodeInfo = childWindowInfos[buyHandleIndex - 1];

                        SetText(securityCodeInfo.m_hWnd, info.m_code);
                        Thread.Sleep(100);
                        SetText(priceInfo.m_hWnd, info.m_price.ToString());
                        Thread.Sleep(100);
                        SetText(buyCountCodeInfo.m_hWnd, info.m_qty.ToString());
                        Thread.Sleep(100);

                        List<WindowInfo> popupWindows = new List<WindowInfo>();
                        GetWindows(popupWindows);
                        Dictionary<int, int> keys = new Dictionary<int, int>();
                        int popupWindowsSize = popupWindows.Count;
                        for (int j = 0; j < popupWindowsSize; j++)
                        {
                            keys[popupWindows[j].m_hWnd] = 0;
                        }

                        PostMessage(buyButtonInfo.m_hWnd, WM_KEYDOWN, (int)Keys.B, 0);
                        Thread.Sleep(200);
                        List<String> titles = new List<String>();
                        titles.Add("是否确定以上买入委托？");
                        String tipTitle = CheckPopupWindow(titles, "是(&Y)", keys);
                        titles.Clear();
                        titles.Add("买入委托已成功提交");
                        titles.Add("提交失败");
                        tipTitle = CheckPopupWindow(titles, "确定", keys);
                        //SendKey(13);
                        //Thread.Sleep(100);
                        //SendKey(13);
                        //Thread.Sleep(100);
                    }
                    break;
                    #endregion
                }
                //通达信下单
                else if (windowInfos[i].m_text.StartsWith("通达信网上交易V6.20"))
                {
                    #region 通达信下单
                    break;
                    #endregion
                }
            }
        }

        /// <summary>
        /// 撤买
        /// </summary>
        public static void CancelBuy()
        {
            List<WindowInfo> windowInfos = new List<WindowInfo>();
            GetWindows(windowInfos);
            int windowInfosSize = windowInfos.Count;
            for (int i = 0; i < windowInfosSize; i++)
            {
                //同花顺下单
                if (windowInfos[i].m_text == "网上股票交易系统5.0")
                {
                    #region 同花顺下单
                    int hWnd = windowInfos[i].m_hWnd;
                    SetForegroundWindow(hWnd);
                    Thread.Sleep(100);
                    AutoTradeService.ClickTreeMenu("撤单[F3]");
                    Thread.Sleep(100);

                    List<WindowInfo> childWindowInfos = new List<WindowInfo>();
                    GetChildWindows(hWnd, childWindowInfos);
                    int childWindowInfosSize = childWindowInfos.Count;
                    int cancelBuyHandleIndex = -1;
                    for (int j = 0; j < childWindowInfosSize; j++)
                    {
                        WindowInfo childWindowInfo = childWindowInfos[j];
                        if (childWindowInfo.m_className == "Button")
                        {
                            if (childWindowInfo.m_text == "撤买(X)")
                            {
                                cancelBuyHandleIndex = j;
                                break;
                            }
                        }
                    }
                    if (cancelBuyHandleIndex >= 0)
                    {
                        WindowInfo cancelBuyButtonInfo = childWindowInfos[cancelBuyHandleIndex];

                        List<WindowInfo> popupWindows = new List<WindowInfo>();
                        GetWindows(popupWindows);
                        Dictionary<int, int> keys = new Dictionary<int, int>();
                        int popupWindowsSize = popupWindows.Count;
                        for (int j = 0; j < popupWindowsSize; j++)
                        {
                            keys[popupWindows[j].m_hWnd] = 0;
                        }

                        PostMessage(cancelBuyButtonInfo.m_hWnd, WM_KEYDOWN, (int)Keys.X, 0);
                        Thread.Sleep(200);
                        List<String> titles = new List<String>();
                        titles.Add("笔买入委托吗？");
                        String tipTitle = CheckPopupWindow(titles, "是(&Y)", keys);
                        titles.Clear();
                        titles.Add("撤单委托已成功提交");
                        titles.Add("提交失败");
                        tipTitle = CheckPopupWindow(titles, "确定", keys);
                        //SendKey(13);
                        //Thread.Sleep(100);
                        //SendKey(13);
                        //Thread.Sleep(100);
                    }
                    break;
                    #endregion
                }
                //通达信下单
                else if (windowInfos[i].m_text.StartsWith("通达信网上交易V6.20"))
                {
                    #region 通达信下单
                    break;
                    #endregion
                }
            }
        }
        
        /// <summary>
        /// 撤卖
        /// </summary>
        public static void CancelSell()
        {
            List<WindowInfo> windowInfos = new List<WindowInfo>();
            GetWindows(windowInfos);
            int windowInfosSize = windowInfos.Count;
            for (int i = 0; i < windowInfosSize; i++)
            {
                //同花顺下单
                if (windowInfos[i].m_text == "网上股票交易系统5.0")
                {
                    #region 同花顺下单
                    int hWnd = windowInfos[i].m_hWnd;
                    SetForegroundWindow(hWnd);
                    Thread.Sleep(100);
                    AutoTradeService.ClickTreeMenu("撤单[F3]");
                    Thread.Sleep(100);

                    List<WindowInfo> childWindowInfos = new List<WindowInfo>();
                    GetChildWindows(hWnd, childWindowInfos);
                    int childWindowInfosSize = childWindowInfos.Count;
                    int cancelSellHandleIndex = -1;
                    for (int j = 0; j < childWindowInfosSize; j++)
                    {
                        WindowInfo childWindowInfo = childWindowInfos[j];
                        if (childWindowInfo.m_className == "Button")
                        {
                            if (childWindowInfo.m_text == "撤卖(C)")
                            {
                                cancelSellHandleIndex = j;
                                break;
                            }
                        }
                    }
                    if (cancelSellHandleIndex >= 0)
                    {
                        WindowInfo cancelSellButtonInfo = childWindowInfos[cancelSellHandleIndex];

                        List<WindowInfo> popupWindows = new List<WindowInfo>();
                        GetWindows(popupWindows);
                        Dictionary<int, int> keys = new Dictionary<int, int>();
                        int popupWindowsSize = popupWindows.Count;
                        for (int j = 0; j < popupWindowsSize; j++)
                        {
                            keys[popupWindows[j].m_hWnd] = 0;
                        }

                        PostMessage(cancelSellButtonInfo.m_hWnd, WM_KEYDOWN, (int)Keys.C, 0);
                        Thread.Sleep(200);
                        List<String> titles = new List<String>();
                        titles.Add("笔卖出委托吗？");
                        String tipTitle = CheckPopupWindow(titles, "是(&Y)", keys);
                        titles.Clear();
                        titles.Add("撤单委托已成功提交");
                        titles.Add("提交失败");
                        tipTitle = CheckPopupWindow(titles, "确定", keys);
                        //SendKey(13);
                        //Thread.Sleep(100);
                        //SendKey(13);
                        //Thread.Sleep(100);
                    }
                    break;
                    #endregion
                }
                //通达信下单
                else if (windowInfos[i].m_text.StartsWith("通达信网上交易V6.20"))
                {
                    #region 通达信下单
                    break;
                    #endregion
                }
            }
        }

        // <summary>
        /// 检查弹出窗口
        /// </summary>
        /// <param name="parentHandle">父窗体</param>
        /// <param name="titles">标题</param>
        /// <param name="submitText">确认文字</param>
        /// <param name="keys">关键字</param>
        private static String CheckPopupWindow(List<String> titles, String submitText, Dictionary<int, int> keys)
        {
            bool hasPopup = false;
            String popUpWindowTitle = "";
            //while (!hasPopup)
            {
                List<WindowInfo> popupWindows2 = new List<WindowInfo>();
                GetWindows(popupWindows2);
                int popupWindows2Size = popupWindows2.Count;
                for (int z = 0; z < popupWindows2Size; z++)
                {
                    WindowInfo info = popupWindows2[z];
                    if (!keys.ContainsKey(info.m_hWnd))
                    {
                        keys[info.m_hWnd] = 0;
                        List<WindowInfo> popupChildWindows = new List<WindowInfo>();
                        GetChildWindows(info.m_hWnd, popupChildWindows);
                        int popupChildWindowsSize = popupChildWindows.Count;
                        WindowInfo submitButton = null;
                        if (popupChildWindowsSize > 0)
                        {
                            for (int x = 0; x < popupChildWindowsSize; x++)
                            {
                                WindowInfo pChildWindow = popupChildWindows[x];
                                foreach(String title in titles)
                                {
                                    if (pChildWindow.m_text.IndexOf(title) != -1)
                                    {
                                        popUpWindowTitle = pChildWindow.m_text;
                                        hasPopup = true;
                                        break;
                                    }
                                }

                                if (pChildWindow.m_text == submitText)
                                {
                                    submitButton = pChildWindow;
                                }
                            }
                        }
                        if (hasPopup)
                        {
                            PostMessage(submitButton.m_hWnd, WM_KEYDOWN, (int)Keys.Enter, 0);
                            Thread.Sleep(200);
                            //MouseEvent("SETCURSOR", submitButton.m_rect.Left + 5, submitButton.m_rect.Top + 5, 0);
                            //MouseEvent("LEFTDOWN", 0, 0, 0);
                            //MouseEvent("LEFTUP", 0, 0, 0);
                        }
                    }
                }
                return popUpWindowTitle;
            }
        }
		
        /// <summary>
        /// 点击树的某个按钮
        /// </summary>
        /// <param name="name"></param>
        public static int ClickTreeMenu(String name)
        {
            TVITEM itemNode;
            if (m_treeViewTextHandleMapping.TryGetValue(name, out itemNode) && m_treeHandle > 0)
            {
                SendMessage(m_treeHandle, TVM_SELECTITEM, TVGN_CARET, (int)itemNode.hItem);
                return 1;
            }
            return 0;
        }

		/// <summary>
        /// 获取股票账户资金
        /// </summary>
        public static String GetSecurityCaptial()
        {
            StringBuilder sbResult = new StringBuilder();
            //m_mapWindowInfos.Clear();
            List<WindowInfo> windowInfos = new List<WindowInfo>();
            GetWindows(windowInfos);
            int windowInfosSize = windowInfos.Count;
            for (int i = 0; i < windowInfosSize; i++)
            {
                //同花顺下单
                if (windowInfos[i].m_text == "网上股票交易系统5.0")
                {
                    int hWnd = windowInfos[i].m_hWnd;
                    SetForegroundWindow(hWnd);
                    Thread.Sleep(100);

                    #region 同花顺下单
                    AutoTradeService.ClickTreeMenu("查询[F4]");
                    Thread.Sleep(100);
                    AutoTradeService.ClickTreeMenu("资金股票");
                    Thread.Sleep(100);
                    List<WindowInfo> childWindowInfos = new List<WindowInfo>();
                    GetChildWindows(windowInfos[i].m_hWnd, childWindowInfos);
                    int childWindowInfosSize = childWindowInfos.Count;

                    int staticIndex = 0;
                    int afxWnd42sIndex = 0;
                    for (int j = 0; j < childWindowInfosSize; j++)
                    {
                        WindowInfo childWindowInfo = childWindowInfos[j];
                        if (childWindowInfo.m_className == "Static")
                        {
                            if (childWindowInfo.m_text == "查询资金股票")
                            {
                                staticIndex = j;
                            }
                        }
                        else if (staticIndex > 0 && childWindowInfo.m_className == "Afx:400000:0")
                        {
                            afxWnd42sIndex = j;
                            break;
                        }
                    }

                    if(staticIndex > 0)
                    {
                        for (int j = staticIndex; j < childWindowInfosSize; j++)
                        {
                            WindowInfo childWindowInfo = childWindowInfos[j];
                            if (childWindowInfo.m_className == "Static")
                            {
                                if (staticIndex + 4 == j)
                                {
                                    sbResult.AppendLine(childWindowInfo.m_text);
                                }
                                else if (staticIndex + 5 == j)
                                {
                                    sbResult.AppendLine(childWindowInfo.m_text);
                                }
                                else if (staticIndex + 6 == j)
                                {
                                    sbResult.AppendLine(childWindowInfo.m_text);
                                }
                                else if (staticIndex + 10 == j)
                                {
                                    sbResult.AppendLine(childWindowInfo.m_text);
                                }
                                else if (staticIndex + 11 == j)
                                {
                                    sbResult.AppendLine(childWindowInfo.m_text);
                                }
                                else if (staticIndex + 12 == j)
                                {
                                    sbResult.AppendLine(childWindowInfo.m_text);
                                }
                            }
                        }
                        return sbResult.ToString();
                    }
                    break;
                    #endregion
                }
                //通达信下单
                else if (windowInfos[i].m_text.StartsWith("通达信网上交易V6.20"))
                {
                    #region 通达信下单
                    break;
                    #endregion
                }
            }

            return "";
        }

        /// <summary>
        /// 获取股票委托信息
        /// </summary>
        /// <returns></returns>
        public static String GetSecurityCommission()
        {
            StringBuilder sbResult = new StringBuilder();
            List<WindowInfo> windowInfos = new List<WindowInfo>();
            GetWindows(windowInfos);
            int windowInfosSize = windowInfos.Count;
            for (int i = 0; i < windowInfosSize; i++)
            {
                //同花顺下单
                if (windowInfos[i].m_text == "网上股票交易系统5.0")
                {
                    int hWnd = windowInfos[i].m_hWnd;
                    SetForegroundWindow(hWnd);
                    Thread.Sleep(100);

                    #region 同花顺下单
                    AutoTradeService.ClickTreeMenu("查询[F4]");
                    Thread.Sleep(100);
                    AutoTradeService.ClickTreeMenu("当日委托");
                    Thread.Sleep(100);
                    List<WindowInfo> childWindowInfos = new List<WindowInfo>();
                    GetChildWindows(windowInfos[i].m_hWnd, childWindowInfos);
                    int childWindowInfosSize = childWindowInfos.Count;

                    int staticIndex = 0;
                    int afxWnd42sIndex = 0;
                    for (int j = 0; j < childWindowInfosSize; j++)
                    {
                        WindowInfo childWindowInfo = childWindowInfos[j];

                        if (childWindowInfo.m_className == "Button")
                        {
                            if (childWindowInfo.m_text == "显示撤单记录")
                            {
                                staticIndex = j;
                            }
                        }
                        else if (staticIndex > 0 && childWindowInfo.m_className == "Afx:400000:0")
                        {
                            afxWnd42sIndex = j;
                            break;
                        }
                    }

                    if (staticIndex > 0)
                    {
                        for (int j = staticIndex; j < childWindowInfosSize; j++)
                        {
                            WindowInfo childWindowInfo = childWindowInfos[j];
                            if (childWindowInfo.m_className == "CVirtualGridCtrl")
                            {
                                if (afxWnd42sIndex + 2 == j)
                                {
                                    MouseEvent("SETCURSOR", childWindowInfo.m_rect.Left + 20, childWindowInfo.m_rect.Top + 40, 0);
                                    MouseEvent("RIGHTDOWN", 0, 0, 0);
                                    MouseEvent("RIGHTUP", 0, 0, 0);
                                    Thread.Sleep(200);
                                    //PostMessage(childWindowInfo.m_hWnd, WM_KEYDOWN, 0x00000043, 0x102E0001);
                                    MouseEvent("SETCURSOR", childWindowInfo.m_rect.Left + 60, childWindowInfo.m_rect.Top + 100, 0);
                                    MouseEvent("LEFTDOWN", 0, 0, 0);
                                    MouseEvent("LEFTUP", 0, 0, 0);
                                    Thread.Sleep(200);
                                    IDataObject iData = Clipboard.GetDataObject();
                                    if (iData != null)
                                    {
                                        if (iData.GetDataPresent(DataFormats.Text))
                                        {
                                            sbResult.AppendLine((String)iData.GetData(DataFormats.Text));
                                        }
                                    }
                                    return sbResult.ToString();
                                }
                            }
                        }
                    }
                    break;
                    #endregion
                }
                //通达信下单
                else if (windowInfos[i].m_text.StartsWith("通达信网上交易V6.20"))
                {
                    #region 通达信下单
                    break;
                    #endregion
                }
            }

            return "";
        }

        /// <summary>
        /// 获取股票账户持仓
        /// </summary>
        public static String GetSecurityPosition()
        {
            StringBuilder sbResult = new StringBuilder();
            List<WindowInfo> windowInfos = new List<WindowInfo>();
            GetWindows(windowInfos);
            int windowInfosSize = windowInfos.Count;
            for (int i = 0; i < windowInfosSize; i++)
            {
                //同花顺下单
                if (windowInfos[i].m_text == "网上股票交易系统5.0")
                {
                    int hWnd = windowInfos[i].m_hWnd;
                    SetForegroundWindow(hWnd);
                    Thread.Sleep(100);

                    #region 同花顺下单
                    AutoTradeService.ClickTreeMenu("查询[F4]");
                    Thread.Sleep(100);
                    AutoTradeService.ClickTreeMenu("资金股票");
                    Thread.Sleep(100);
                    List<WindowInfo> childWindowInfos = new List<WindowInfo>();
                    GetChildWindows(windowInfos[i].m_hWnd, childWindowInfos);
                    int childWindowInfosSize = childWindowInfos.Count;

                    int staticIndex = 0;
                    int afxWnd42sIndex = 0;
                    for (int j = 0; j < childWindowInfosSize; j++)
                    {
                        WindowInfo childWindowInfo = childWindowInfos[j];

                        if (childWindowInfo.m_className == "Static")
                        {
                            if (childWindowInfo.m_text == "查询资金股票")
                            {
                                staticIndex = j;
                            }
                        }
                        else if (staticIndex > 0 && childWindowInfo.m_className == "Afx:400000:0")
                        {
                            afxWnd42sIndex = j;
                            break;
                        }
                    }

                    if(staticIndex > 0)
                    {
                        for (int j = staticIndex; j < childWindowInfosSize; j++)
                        {
                            WindowInfo childWindowInfo = childWindowInfos[j];
                            if (childWindowInfo.m_className == "CVirtualGridCtrl")
                            {
                                if (afxWnd42sIndex + 2 == j)
                                {
                                    MouseEvent("SETCURSOR", childWindowInfo.m_rect.Left + 20, childWindowInfo.m_rect.Top + 40, 0);
                                    MouseEvent("RIGHTDOWN", 0, 0, 0);
                                    MouseEvent("RIGHTUP", 0, 0, 0);
                                    Thread.Sleep(200);
                                    //PostMessage(childWindowInfo.m_hWnd, WM_KEYDOWN, 0x00000043, 0x102E0001);
                                    MouseEvent("SETCURSOR", childWindowInfo.m_rect.Left + 60, childWindowInfo.m_rect.Top + 100, 0);
                                    MouseEvent("LEFTDOWN", 0, 0, 0);
                                    MouseEvent("LEFTUP", 0, 0, 0);
                                    Thread.Sleep(200);
                                    IDataObject iData = Clipboard.GetDataObject();
                                    if (iData != null)
                                    {
                                        if (iData.GetDataPresent(DataFormats.Text))
                                        {
                                            sbResult.AppendLine((String)iData.GetData(DataFormats.Text));
                                        }
                                    }
                                    return sbResult.ToString();
                                }
                            }
                        }
                    }
                    break;
                    #endregion
                }
                //通达信下单
                else if (windowInfos[i].m_text.StartsWith("通达信网上交易V6.20"))
                {
                    #region 通达信下单
                    break;
                    #endregion
                }
            }

            return "";
        }

        /// <summary>
        /// 获取股票成交信息
        /// </summary>
        /// <returns></returns>
        public static String GetSecurityTrade()
        {
            StringBuilder sbResult = new StringBuilder();
            List<WindowInfo> windowInfos = new List<WindowInfo>();
            GetWindows(windowInfos);
            int windowInfosSize = windowInfos.Count;
            for (int i = 0; i < windowInfosSize; i++)
            {
                //同花顺下单
                if (windowInfos[i].m_text == "网上股票交易系统5.0")
                {
                    int hWnd = windowInfos[i].m_hWnd;
                    SetForegroundWindow(hWnd);
                    Thread.Sleep(100);

                    #region 同花顺下单
                    AutoTradeService.ClickTreeMenu("查询[F4]");
                    Thread.Sleep(100);
                    AutoTradeService.ClickTreeMenu("当日成交");
                    Thread.Sleep(100);
                    List<WindowInfo> childWindowInfos = new List<WindowInfo>();
                    GetChildWindows(windowInfos[i].m_hWnd, childWindowInfos);
                    int childWindowInfosSize = childWindowInfos.Count;

                    int staticIndex = 0;
                    int afxWnd42sIndex = 0;
                    for (int j = 0; j < childWindowInfosSize; j++)
                    {
                        WindowInfo childWindowInfo = childWindowInfos[j];

                        if (childWindowInfo.m_className == "Button")
                        {
                            if (childWindowInfo.m_text == "显示撤单记录")
                            {
                                staticIndex = j;
                            }
                        }
                        else if (staticIndex > 0 && childWindowInfo.m_className == "Afx:400000:0")
                        {
                            afxWnd42sIndex = j;
                            break;
                        }
                    }

                    if (staticIndex > 0)
                    {
                        for (int j = staticIndex; j < childWindowInfosSize; j++)
                        {
                            WindowInfo childWindowInfo = childWindowInfos[j];
                            if (childWindowInfo.m_className == "CVirtualGridCtrl")
                            {
                                if (afxWnd42sIndex + 2 == j)
                                {
                                    MouseEvent("SETCURSOR", childWindowInfo.m_rect.Left + 20, childWindowInfo.m_rect.Top + 40, 0);
                                    MouseEvent("RIGHTDOWN", 0, 0, 0);
                                    MouseEvent("RIGHTUP", 0, 0, 0);
                                    Thread.Sleep(200);
                                    MouseEvent("SETCURSOR", childWindowInfo.m_rect.Left + 60, childWindowInfo.m_rect.Top + 100, 0);
                                    MouseEvent("LEFTDOWN", 0, 0, 0);
                                    MouseEvent("LEFTUP", 0, 0, 0);
                                    //SendMessage(childWindowInfo.m_hWnd, WM_KEYDOWN, 0x00000043, 0x102E0001);
                                    Thread.Sleep(200);
                                    IDataObject iData = Clipboard.GetDataObject();
                                    if (iData != null)
                                    {
                                        if (iData.GetDataPresent(DataFormats.Text))
                                        {
                                            sbResult.AppendLine((String)iData.GetData(DataFormats.Text));
                                        }
                                    }
                                    return sbResult.ToString();
                                }
                            }
                        }
                    }
                    break;
                    #endregion
                }
                //通达信下单
                else if (windowInfos[i].m_text.StartsWith("通达信网上交易V6.20"))
                {
                    #region 通达信下单
                    break;
                    #endregion
                }
            }

            return "";
        }

        /// <summary>
        /// 初始化树节点句柄
        /// </summary>
        public static void InitSysTreeView32Handle()
        {
            List<WindowInfo> windowInfos = new List<WindowInfo>();
            GetWindows(windowInfos);
            int windowInfosSize = windowInfos.Count;
            m_treeViewTextHandleMapping.Clear();
            for (int i = 0; i < windowInfosSize; i++)
            {
                //同花顺下单
                if (windowInfos[i].m_text == "网上股票交易系统5.0")
                {
                    #region 同花顺下单
                    int hWnd = windowInfos[i].m_hWnd;
                    SetForegroundWindow(hWnd);
                    Thread.Sleep(100);

                    List<WindowInfo> childWindowInfos = new List<WindowInfo>();
                    GetChildWindows(windowInfos[i].m_hWnd, childWindowInfos);
                    int childWindowInfosSize = childWindowInfos.Count;

                    for (int j = 0; j < childWindowInfosSize; j++)
                    {
                        WindowInfo childWindowInfo = childWindowInfos[j];
                        if (childWindowInfo.m_className == "SysTreeView32")
                        {
                            int hWndTree = childWindowInfo.m_hWnd;
                            m_treeHandle = hWndTree;
                            GetTreeViewText((IntPtr)hWndTree, m_treeViewTextHandleMapping);
                            break;
                        }
                    }
                    break;
                    #endregion
                }
                //通达信下单
                else if (windowInfos[i].m_text.StartsWith("通达信网上交易V6.20"))
                {
                    #region 通达信下单
                    break;
                    #endregion
                }
            }
            return;
        }

        /// <summary>
        /// 卖出
        /// </summary>
        /// <param name="info"></param>
        public static void Sell(OrderInfo info)
        {
            List<WindowInfo> windowInfos = new List<WindowInfo>();
            GetWindows(windowInfos);
            int windowInfosSize = windowInfos.Count;
            for (int i = 0; i < windowInfosSize; i++)
            {
                //同花顺下单
                if (windowInfos[i].m_text == "网上股票交易系统5.0")
                {
                    #region 同花顺下单
                    int hWnd = windowInfos[i].m_hWnd;
                    SetForegroundWindow(hWnd);
                    Thread.Sleep(100);
                    AutoTradeService.ClickTreeMenu("卖出[F2]");
                    Thread.Sleep(100);

                    List<WindowInfo> childWindowInfos = new List<WindowInfo>();
                    GetChildWindows(hWnd, childWindowInfos);
                    int childWindowInfosSize = childWindowInfos.Count;
                    int sellHandleIndex = -1;
                    for (int j = 0; j < childWindowInfosSize; j++)
                    {
                        WindowInfo childWindowInfo = childWindowInfos[j];
                        if (childWindowInfo.m_className == "Button")
                        {
                            if (childWindowInfo.m_text == "卖出[S]")
                            {
                                sellHandleIndex = j;
                                break;
                            }
                        }
                    }
                    if (sellHandleIndex >= 5)
                    {
                        WindowInfo sellButtonInfo = childWindowInfos[sellHandleIndex];
                        WindowInfo securityCodeInfo = childWindowInfos[sellHandleIndex - 5];
                        WindowInfo priceInfo = childWindowInfos[sellHandleIndex - 3];
                        WindowInfo sellCountCodeInfo = childWindowInfos[sellHandleIndex - 1];

                        SetText(securityCodeInfo.m_hWnd, info.m_code);
                        Thread.Sleep(100);
                        SetText(priceInfo.m_hWnd, info.m_price.ToString());
                        Thread.Sleep(100);
                        SetText(sellCountCodeInfo.m_hWnd, info.m_qty.ToString());
                        Thread.Sleep(100);

                        List<WindowInfo> popupWindows = new List<WindowInfo>();
                        GetWindows(popupWindows);
                        Dictionary<int, int> keys = new Dictionary<int, int>();
                        int popupWindowsSize = popupWindows.Count;
                        for (int j = 0; j < popupWindowsSize; j++)
                        {
                            keys[popupWindows[j].m_hWnd] = 0;
                        }

                        PostMessage(sellButtonInfo.m_hWnd, WM_KEYDOWN, (int)Keys.S, 0);
                        Thread.Sleep(200);
                        List<String> titles = new List<String>();
                        titles.Add("是否确定以上卖出委托？");
                        String tipTitle = CheckPopupWindow(titles, "是(&Y)", keys);
                        titles.Clear();
                        titles.Add("卖出委托已成功提交");
                        titles.Add("提交失败");
                        tipTitle = CheckPopupWindow(titles, "确定", keys);
                    }
                    break;
                    #endregion
                }
                //通达信下单
                else if (windowInfos[i].m_text.StartsWith("通达信网上交易V6.20"))
                {
                    #region 通达信下单
                    break;
                    #endregion
                }
            }
        }

        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="state">状态</param>
        public static void StartOrder()
        {
            List<WindowInfo> windowInfos = new List<WindowInfo>();
            GetWindows(windowInfos);
            int windowInfosSize = windowInfos.Count;
            for (int i = 0; i < windowInfosSize; i++)
            {
                //同花顺下单
                if (windowInfos[i].m_text == "网上股票交易系统5.0")
                {
                    #region 同花顺下单
                    List<WindowInfo> childWindowInfos = new List<WindowInfo>();
                    GetChildWindows(windowInfos[i].m_hWnd, childWindowInfos);
                    int childWindowInfosSize = childWindowInfos.Count;
                    int orderInfosSize = m_orderInfos.Count;
                    for (int u = 0; u < orderInfosSize; u++)
                    {
                        int priceHandle = 0;
                        bool canBuy = false;
                        OrderInfo orderInfo = m_orderInfos[u];
                        int editIndex = 0;
                        WindowInfo buyButton = null;
                        StringBuilder sb = new StringBuilder();
                        for (int j = 0; j < childWindowInfosSize; j++)
                        {
                            WindowInfo childWindowInfo = childWindowInfos[j];
                            sb.AppendLine("m_className:" + childWindowInfo.m_className);
                            sb.AppendLine("m_hWnd:" + childWindowInfo.m_hWnd);
                            sb.AppendLine("m_text:" + childWindowInfo.m_text);
                            sb.AppendLine("");
                            if (childWindowInfo.m_className == "Edit")
                            {
                                if (editIndex == 0)
                                {
                                    SetText(childWindowInfo.m_hWnd, orderInfo.m_code);
                                }
                                else if (editIndex == 1)
                                {
                                    priceHandle = childWindowInfo.m_hWnd;
                                    String text = GetText(childWindowInfo.m_hWnd);
                                    int wait = 15;
                                    while (text.Length == 0 && wait > 0)
                                    {
                                        text = GetText(childWindowInfo.m_hWnd);
                                        Thread.Sleep(100);
                                        wait--;
                                    }
                                    if (text.Length > 0)
                                    {
                                        Thread.Sleep(500);
                                        canBuy = true;
                                        if (orderInfo.m_price > 0)
                                        {
                                            String newText = orderInfo.m_price.ToString();
                                            SetText(childWindowInfo.m_hWnd, newText);
                                        }
                                    }
                                }
                                else if (editIndex == 2)
                                {
                                    SetText(childWindowInfo.m_hWnd, "100");
                                }
                                editIndex++;
                            }
                            else if (childWindowInfo.m_className == "Button")
                            {
                                if (childWindowInfo.m_text == "买入[B]")
                                {
                                    buyButton = childWindowInfo;
                                }
                            }
                        }

                        File.WriteAllText("c://123.txt", sb.ToString());
                        if (canBuy)
                        {
                            if (orderInfo.m_price > 0)
                            {
                                String newText = orderInfo.m_price.ToString();
                                if (newText != GetText(priceHandle))
                                {
                                    SetText(priceHandle, newText);
                                }
                            }
                            List<WindowInfo> popupWindows = new List<WindowInfo>();
                            GetWindows(popupWindows);
                            Dictionary<int, int> keys = new Dictionary<int, int>();
                            int popupWindowsSize = popupWindows.Count;
                            for (int j = 0; j < popupWindowsSize; j++)
                            {
                                keys[popupWindows[j].m_hWnd] = 0;
                            }
                            MouseEvent("SETCURSOR", buyButton.m_rect.Left + 5, buyButton.m_rect.Top + 5, 0);
                            MouseEvent("LEFTDOWN", 0, 0, 0);
                            MouseEvent("LEFTUP", 0, 0, 0);
                            //CheckPopupWindow("您是否确定以上买入委托？", "是(&Y)", keys);
                            //CheckPopupWindow("您的买入委托已成功提交", "确定", keys);
                            SendKey(13);
                            Thread.Sleep(100);
                            SendKey(13);
                            Thread.Sleep(100);
                        }
                    }
                    break;
                    #endregion
                }
                //通达信下单
                else if (windowInfos[i].m_text.StartsWith("通达信网上交易V6.20"))
                {
                    #region 通达信下单
                    List<WindowInfo> childWindowInfos = new List<WindowInfo>();
                    GetChildWindows(windowInfos[i].m_hWnd, childWindowInfos);
                    int childWindowInfosSize = childWindowInfos.Count;
                    int orderInfosSize = m_orderInfos.Count;
                    for (int u = 0; u < orderInfosSize; u++)
                    {
                        int priceHandle = 0;
                        bool canBuy = false;
                        OrderInfo orderInfo = m_orderInfos[u];
                        int editIndex = 0;
                        WindowInfo buyButton = null;
                        for (int j = 0; j < childWindowInfosSize; j++)
                        {
                            WindowInfo childWindowInfo = childWindowInfos[j];
                            if (childWindowInfo.m_className == "Edit")
                            {
                                if (editIndex == 0)
                                {
                                    SetText(childWindowInfo.m_hWnd, orderInfo.m_code);
                                }
                                else if (editIndex == 1)
                                {
                                    priceHandle = childWindowInfo.m_hWnd;
                                    String text = GetText(childWindowInfo.m_hWnd);
                                    int wait = 15;
                                    while (text.Length == 0 && wait > 0)
                                    {
                                        text = GetText(childWindowInfo.m_hWnd);
                                        Thread.Sleep(100);
                                        wait--;
                                    }
                                    if (text.Length > 0)
                                    {
                                        canBuy = true;
                                        if (orderInfo.m_price > 0)
                                        {
                                            String newText = orderInfo.m_price.ToString();
                                            SetText(childWindowInfo.m_hWnd, newText);
                                        }
                                    }
                                }
                                else if (editIndex == 3)
                                {
                                    SetText(childWindowInfo.m_hWnd, "100");
                                }
                                editIndex++;
                            }
                            else if (childWindowInfo.m_className == "Button")
                            {
                                if (childWindowInfo.m_text == "买入下单")
                                {
                                    buyButton = childWindowInfo;
                                }
                            }
                        }
                        if (canBuy)
                        {
                            if (orderInfo.m_price > 0)
                            {
                                String newText = orderInfo.m_price.ToString();
                                if (newText != GetText(priceHandle))
                                {
                                    SetText(priceHandle, newText);
                                }
                            }
                            MouseEvent("SETCURSOR", buyButton.m_rect.Left + 5, buyButton.m_rect.Top + 5, 0);
                            MouseEvent("LEFTDOWN", 0, 0, 0);
                            MouseEvent("LEFTUP", 0, 0, 0);
                            Thread.Sleep(100);
                            SendKey(13);
                            Thread.Sleep(100);
                            SendKey(13);
                            Thread.Sleep(100);
                            SendKey(13);
                            Thread.Sleep(100);
                            SendKey(13);
                            Thread.Sleep(100);
                        }
                    }
                    break;
                    #endregion
                }
            }
            m_orderInfos.Clear();
        }
    }
}
