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
    /// ������
    /// </summary>
    public class Sound
    {
        /// <summary>
        /// ��ý����ƽӿڷ��Ϳ�������
        /// </summary>
        /// <param name="lpszCommand">����μ�
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/dd743572(v=vs.85).aspx </param>
        /// <param name="lpszReturnString">����ص���Ϣ�����û����Ҫ���ص���Ϣ����Ϊnull</param>
        /// <param name="cchReturn">ָ��������Ϣ���ַ�����С</param>
        /// <param name="hwndCallback">�ص������������������û��ָ��notify��ʶ������Ϊnew IntPtr(0)</param>
        /// <returns>��������ִ��״̬�Ĵ������</returns>
        [DllImport("winmm.dll")]
        static extern Int32 mciSendString(String lpszCommand, StringBuilder returnString, int bufferSize, IntPtr hwndCallback);
        /// <summary>
        /// ���ض�ִ��״̬������������
        /// </summary>
        /// <param name="errorCode">mciSendCommand����mciSendString���صĴ������</param>
        /// <param name="errorText">�Դ������������ַ���</param>
        /// <param name="errorTextSize">ָ���ַ����Ĵ�С</param>
        /// <returns>���ERROR Codeδ֪������false</returns>
        [DllImport("winmm.dll")]
        static extern bool mciGetErrorString(Int32 errorCode, StringBuilder errorText, Int32 errorTextSize);


        /// <summary>
        /// ��ʼ��������
        /// </summary>
        /// <param name="args">����</param>
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
        /// ����
        /// </summary>
        /// <param name="key">�ļ���</param>
        public static void Play(String key)
        {
            Thread thread = new Thread(new ParameterizedThreadStart(StartPlay));
            thread.Start(key);
        }
    }
}
