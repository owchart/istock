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
    /// 自选股
    /// </summary>
    public class UserSecurity
    {
        /// <summary>
        /// 代码
        /// </summary>
        public String m_code;

        /// <summary>
        /// 自动买入价
        /// </summary>
        public double m_buy;

        /// <summary>
        /// 状态
        /// </summary>
        public int m_state;

        /// <summary>
        /// 自动卖出价
        /// </summary>
        public double m_sell;

        /// <summary>
        /// 止损价
        /// </summary>
        public double m_stop;
    }

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
                    m_codes = JsonConvert.DeserializeObject<List<UserSecurity>>(cookie.m_value);
                }
                catch (Exception ex)
                {
                }
                if (m_codes == null)
                {
                    try
                    {
                        m_codes = JsonConvert.DeserializeObject<List<UserSecurity>>(cookie.m_value);
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
        public List<UserSecurity> m_codes = new List<UserSecurity>();

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="code">自选股</param>
        public void Add(UserSecurity code)
        {
            bool modify = false;
            int awardsSize = m_codes.Count;
            for (int i = 0; i < awardsSize; i++)
            {
                if (m_codes[i] == code)
                {
                    modify = true;
                    m_codes[i] = code;
                    Save();
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
        public void Delete(UserSecurity userSecurity)
        {
            int codesSize = m_codes.Count;
            for (int i = 0; i < codesSize; i++)
            {
                if (m_codes[i].m_code == userSecurity.m_code)
                {
                    m_codes.RemoveAt(i);
                    Save();
                    break;
                }
            }
        }

        /// <summary>
        /// 获取嘉奖信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>嘉奖信息</returns>
        public UserSecurity Get(String code)
        {
            int codesSize = m_codes.Count;
            for (int i = 0; i < codesSize; i++)
            {
                if (m_codes[i].m_code == code)
                {
                    return m_codes[i];
                }
            }
            return null;
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
