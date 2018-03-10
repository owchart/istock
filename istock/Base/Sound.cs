using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Media;
using OwLib;
using System.Threading;
using System.Runtime.InteropServices;

namespace OwLib
{
    /// <summary>
    /// 声音类
    /// </summary>
    public class Sound
    {
        /// <summary>
        /// 向媒体控制接口发送控制命令
        /// </summary>
        /// <param name="lpszCommand">命令，参见
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/dd743572(v=vs.85).aspx </param>
        /// <param name="lpszReturnString">命令返回的信息，如果没有需要返回的信息可以为null</param>
        /// <param name="cchReturn">指定返回信息的字符串大小</param>
        /// <param name="hwndCallback">回调句柄，如果命令参数中没有指定notify标识，可以为new IntPtr(0)</param>
        /// <returns>返回命令执行状态的错误代码</returns>
        [DllImport("winmm.dll")]
        static extern Int32 mciSendString(String lpszCommand, StringBuilder returnString, int bufferSize, IntPtr hwndCallback);
        /// <summary>
        /// 返回对执行状态错误代码的描述
        /// </summary>
        /// <param name="errorCode">mciSendCommand或者mciSendString返回的错误代码</param>
        /// <param name="errorText">对错误代码的描述字符串</param>
        /// <param name="errorTextSize">指定字符串的大小</param>
        /// <returns>如果ERROR Code未知，返回false</returns>
        [DllImport("winmm.dll")]
        static extern bool mciGetErrorString(Int32 errorCode, StringBuilder errorText, Int32 errorTextSize);


        /// <summary>
        /// 开始播放声音
        /// </summary>
        /// <param name="args">参数</param>
        private static void StartPlay(object args)
        {
            String fileName = DataCenter.GetAppPath() + "\\config\\" + args.ToString();
            if (CFileA.IsFileExist(fileName))
            {
                try
                {
                    int error = mciSendString("open " + fileName, null, 0, new IntPtr(0));
                    if (error == 0)
                    {
                        mciSendString("play " + fileName, null, 0, new IntPtr(0));
                        Thread.Sleep(10000);
                        mciSendString("stop " + fileName, null, 0, new IntPtr(0));
                        mciSendString("close " + fileName, null, 0, new IntPtr(0));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
                }
            }
        }

        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="key">文件名</param>
        public static void Play(String key)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(StartPlay));
            thread.Start(key);
        }
    }
}
