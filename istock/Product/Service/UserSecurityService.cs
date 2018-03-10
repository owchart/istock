/**************************************************************************************\
*                                                                                      *
* UserSecurityService.cs -  UserSecurity service functions, types, and definitions.       *
*                                                                                      *
*               Version 1.00 ★                                                        *
*                                                                                      *
*               Copyright (c) 2016-2016, iTeam. All rights reserved.               *
*               Created by Todd.                                                 *
*                                                                                      *
***************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace OwLib
{
    /// <summary>
    /// 自选股服务
    /// </summary>
    public class UserSecurityService
    {
        /// <summary>
        /// 创建服务
        /// </summary>
        public UserSecurityService()
        {
            UserCookie cookie = new UserCookie();
            UserCookieService cookieService = DataCenter.UserCookieService;
            if (cookieService.GetCookie("USERSECURITY", ref cookie) > 0)
            {
                try
                {
                    m_codes = JsonConvert.DeserializeObject<List<String>>(cookie.m_value);
                }
                catch (Exception ex)
                {
                }
                if (m_codes == null)
                {
                    try
                    {
                        m_codes = JsonConvert.DeserializeObject<List<String>>(cookie.m_value);
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

        /// <summary>
        /// 自选股信息
        /// </summary>
        public List<String> m_codes = new List<String>();

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="code">自选股</param>
        public void Add(String code)
        {
            bool modify = false;
            int awardsSize = m_codes.Count;
            for (int i = 0; i < awardsSize; i++)
            {
                if (m_codes[i] == code)
                {
                    modify = true;
                    break;
                }
            }
            if (!modify)
            {
                m_codes.Add(code);
                Save();
            }
        }

        /// <summary>
        /// 删除代码
        /// </summary>
        /// <param name="code">代码</param>
        public void Delete(String code)
        {
            int codesSize = m_codes.Count;
            for (int i = 0; i < codesSize; i++)
            {
                if (m_codes[i] == code)
                {
                    m_codes.RemoveAt(i);
                    Save();
                    break;
                }
            }
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        public void Save()
        {
            UserCookie cookie = new UserCookie();
            cookie.m_key = "USERSECURITY";
            cookie.m_value = JsonConvert.SerializeObject(m_codes);
            UserCookieService cookieService = DataCenter.UserCookieService;
            cookieService.AddCookie(cookie);
        }
    }
}
