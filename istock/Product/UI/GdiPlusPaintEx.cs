/*****************************************************************************\
*                                                                             *
* GdiPlusPaintEx.cs - GdiPlus paint functions, types, and definitions.        *
*                                                                             *
*               Version 1.00  ★★★                                          *
*                                                                             *
*               Copyright (c) 2016-2016, iTeam. All rights reserved.      *
*               Created by Todd 2016/4/29.                                    *
*                                                                             *
******************************************************************************/


using System;
using System.Collections.Generic;
using System.Text;
using OwLib;

namespace OwLib
{
    /// <summary>
    /// GDI+绘图扩展类
    /// </summary>
    public class GdiPlusPaintEx : GdiPlusPaint
    {
        #region Lord 2016/4/29
        /// <summary>
        /// 是否支持透明度
        /// </summary>
        private bool m_supportTransparent = true;

        /// <summary>
        /// 获取颜色
        /// </summary>
        /// <param name="dwPenColor">输入颜色</param>
        /// <returns>输出颜色</returns>
        public override long GetColor(long dwPenColor)
        {
            return CDraw.GetWhiteColor(dwPenColor);
        }

        /// <summary>
        /// 设置是否支持透明度
        /// </summary>
        /// <param name="supportTransparent">是否支持透明度</param>
        public void SetSupportTransparent(bool supportTransparent)
        {
            m_supportTransparent = supportTransparent;
        }

        /// <summary>
        /// 是否支持透明度
        /// </summary>
        /// <returns>支持透明度</returns>
        public override bool SupportTransparent()
        {
            return m_supportTransparent;
        }
        #endregion
    }
}
