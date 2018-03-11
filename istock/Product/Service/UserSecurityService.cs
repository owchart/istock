/**************************************************************************************\
*                                                                                      *
* UserSecurityService.cs -  UserSecurity service functions, types, and definitions.       *
*                                                                                      *
*               Version 1.00 ��                                                        *
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
    public class UserSecurity
    {
        public String m_code;

        public double m_up;

        public double m_down;
    }

    /// <summary>
    /// ��ѡ�ɷ���
    /// </summary>
    public class UserSecurityService
    {
        /// <summary>
        /// ��������
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
        /// ��ѡ����Ϣ
        /// </summary>
        public List<UserSecurity> m_codes = new List<UserSecurity>();

        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="code">��ѡ��</param>
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
        /// ɾ������
        /// </summary>
        /// <param name="code">����</param>
        public void Delete(String code)
        {
            int codesSize = m_codes.Count;
            for (int i = 0; i < codesSize; i++)
            {
                if (m_codes[i].m_code == code)
                {
                    m_codes.RemoveAt(i);
                    Save();
                    break;
                }
            }
        }

        /// <summary>
        /// ��ȡ�ν���Ϣ
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>�ν���Ϣ</returns>
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
        /// ������Ϣ
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
