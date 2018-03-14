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
using System.Drawing;

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
        /// 获取绘图对象
        /// </summary>
        public Graphics Graphics
        {
            get { return m_g; }
        }

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
        /// 获取容器的尺寸
        /// </summary>
        /// <param name="rect">矩形</param>
        /// <returns>尺寸</returns>
        public Rectangle GetContainer(RECT rect)
        {
            Rectangle gdiPlusRect = new Rectangle(rect.left + m_offsetX, rect.top + m_offsetY, rect.right - rect.left, rect.bottom - rect.top);
            AffectScaleFactor(ref gdiPlusRect);
            return gdiPlusRect;
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
