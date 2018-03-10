/*****************************************************************************\
*                                                                             *
* CFile.cs -    File functions, types, and definitions.                       *
*                                                                             *
*               Version 1.00 ★★★★★                                       *
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
    /// 文件操作类
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
        /// 向文件中追加内容
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="content">内容</param>
        /// <returns>是否成功</returns>
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
        /// 创建文件夹
        /// </summary>
        /// <param name="dir">文件夹</param>
        public static void CreateDirectory(String dir)
        {
            Directory.CreateDirectory(dir);
        }

        /// <summary>
        /// 获取文件夹中的文件夹
        /// </summary>
        /// <param name="dir">文件夹</param>
        /// <param name="dirs">文件夹集合</param>
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
        /// 获取文件夹中的文件
        /// </summary>
        /// <param name="dir">文件夹</param>
        /// <param name="files">文件集合</param>
        /// <returns>是否成功</returns>
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
        /// 判断文件夹是否存在
        /// </summary>
        /// <param name="dir">文件夹</param>
        /// <returns>是否存在</returns>
        public static bool IsDirectoryExist(String dir)
        {
            return Directory.Exists(dir);
        }

        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="file">文件</param>
        /// <returns>是否存在</returns>
        public static bool IsFileExist(String file)
        {
            return File.Exists(file);
        }

        /// <summary>
        /// 从文件中读取内容
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="content">返回内容</param>
        /// <returns>是否成功</returns>
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
        /// 移除文件
        /// </summary>
        /// <param name="file">文件</param>
        /// <returns>是否成功</returns>
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
        /// 向文件中写入内容
        /// </summary>
        /// <param name="file">文件</param>
        /// <param name="content">内容</param>
        /// <returns>是否成功</returns>
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
