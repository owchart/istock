/*****************************************************************************\
*                                                                             *
* CFile.cs -    File functions, types, and definitions.                       *
*                                                                             *
*               Version 1.00 ������                                       *
*                                                                             *
*               Copyright (c) 2016-2016, Server. All rights reserved.         *
*               Created by Lord.                                              *
*                                                                             *
*******************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace OwLib
{
    /// <summary>
    /// �ļ�������
    /// </summary>
    public class CFileA
    {
        #region Lord 2016/5/13
        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(String lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;

        /// <summary>
        /// ���ļ���׷������
        /// </summary>
        /// <param name="file">�ļ�</param>
        /// <param name="content">����</param>
        /// <returns>�Ƿ�ɹ�</returns>
        public static bool Append(String file, String content)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                sw.Write(content);
                sw.Close();
                fs.Dispose();
                return true;
            }
            catch 
            {
                return false;
            }
        }

        /// <summary>
        /// �����ļ���
        /// </summary>
        /// <param name="dir">�ļ���</param>
        public static void CreateDirectory(String dir)
        {
            Directory.CreateDirectory(dir);
        }

        /// <summary>
        /// ��ȡ�ļ����е��ļ���
        /// </summary>
        /// <param name="dir">�ļ���</param>
        /// <param name="dirs">�ļ��м���</param>
        /// <returns></returns>
        public static bool GetDirectories(String dir, List<String> dirs)
        {
            if (Directory.Exists(dir))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                DirectoryInfo[] lstDir = dirInfo.GetDirectories();
                int lstDirSize = lstDir.Length;
                if (lstDirSize > 0)
                {
                    for (int i = 0; i < lstDirSize; i++)
                    {
                        dirs.Add(lstDir[i].FullName);
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// ��ȡ�ļ����е��ļ�
        /// </summary>
        /// <param name="dir">�ļ���</param>
        /// <param name="files">�ļ�����</param>
        /// <returns>�Ƿ�ɹ�</returns>
        public static bool GetFiles(String dir, List<String> files)
        {
            if (Directory.Exists(dir))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                FileInfo[] lstFile = dirInfo.GetFiles();
                int lstFileSize = lstFile.Length;
                if (lstFileSize > 0)
                {
                    for (int i = 0; i < lstFileSize; i++)
                    {
                        files.Add(lstFile[i].FullName);
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// �ж��ļ����Ƿ����
        /// </summary>
        /// <param name="dir">�ļ���</param>
        /// <returns>�Ƿ����</returns>
        public static bool IsDirectoryExist(String dir)
        {
            return Directory.Exists(dir);
        }

        /// <summary>
        /// �ж��ļ��Ƿ����
        /// </summary>
        /// <param name="file">�ļ�</param>
        /// <returns>�Ƿ����</returns>
        public static bool IsFileExist(String file)
        {
            return File.Exists(file);
        }

        /// <summary>
        /// ���ļ��ж�ȡ����
        /// </summary>
        /// <param name="file">�ļ�</param>
        /// <param name="content">��������</param>
        /// <returns>�Ƿ�ɹ�</returns>
        public static bool Read(String file, ref String content)
        {
            try
            {
                if (File.Exists(file))
                {
                    FileStream fs = new FileStream(file, FileMode.Open);
                    StreamReader sr = new StreamReader(fs, Encoding.Default);
                    content = sr.ReadToEnd();
                    sr.Close();
                    fs.Dispose();
                    return true;
                }
            }
            catch { }
            return false;
        }

        /// <summary>
        /// �Ƴ��ļ�
        /// </summary>
        /// <param name="file">�ļ�</param>
        /// <returns>�Ƿ�ɹ�</returns>
        public static bool RemoveFile(String file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
                return true;
            }
            return false;
        }

        /// <summary>
        /// ���ļ���д������
        /// </summary>
        /// <param name="file">�ļ�</param>
        /// <param name="content">����</param>
        /// <returns>�Ƿ�ɹ�</returns>
        public static bool Write(String file, String content)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                sw.Write(content);
                sw.Close();
                fs.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
