/*************************************************************************************\
*                                                                                     *
* ControlerButtonList.cs -  ControlerButtonList functions, types, and definitions     *
*                                                                                     *
*               Version 1.00                                                          *
*                                                                                     *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.           *
*                                                                                     *
**************************************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
namespace OwLib
{
    /// <summary>
    /// 控制按钮集合
    /// </summary>
    public class ControlerButtonList : List<ControlerButton>
    {
        #region 陶德 2016/5/31
        /// <summary>
        /// 是否包含坐标
        /// </summary>
        /// <param name="mousePt">坐标</param>
        /// <returns>是否包含</returns>
        public bool Contains(Point mousePt)
        {
            foreach (ControlerButton cb in this)
            {
                if (cb.rect.Contains(mousePt))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 绘图方法
        /// </summary>
        /// <param name="g">绘图对象</param>
        public void Draw(Graphics g)
        {
            for (int i = 0; i < this.Count; i++)
            {
                this[i].Draw(g);
            }
        }
        #endregion
    }
}
