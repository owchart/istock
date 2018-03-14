/*****************************************************************************\
*                                                                             *
* CircleButton.cs -  CircleButton functions, types, and definitions           *
*                                                                             *
*               Version 1.00                                                  *
*                                                                             *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.   *
*                                                                             *
*******************************************************************************/

using System;
using Color = System.Drawing.Color;

namespace OwLib
{
    /// <summary>
    /// 色彩符号旋转
    /// </summary>
    public class ColorSymbolRotator
    {
        #region 陶德 2016/5/31
        private static ColorSymbolRotator m_staticInstance;

        /// <summary>
        /// 单例模式
        /// </summary>
        public static ColorSymbolRotator StaticInstance
        {
            get
            {
                if (m_staticInstance == null)
                    m_staticInstance = new ColorSymbolRotator();
                return m_staticInstance;
            }
        }

        /// <summary>
        /// 颜色
        /// </summary>
        public static readonly Color[] COLORS = new Color[]
		{
			Color.Red,
			Color.Blue,
			Color.Green,
			Color.Purple,
			Color.Cyan,
			Color.Pink,
			Color.LightBlue,
			Color.PaleVioletRed,
			Color.SeaGreen,
			Color.Yellow
		};

        /// <summary>
        /// 符号
        /// </summary>
        public static readonly SymbolType[] SYMBOLS = new SymbolType[]
		{
			SymbolType.Circle,
			SymbolType.Diamond,
			SymbolType.Plus,
			SymbolType.Square,
			SymbolType.Star,
			SymbolType.Triangle,
			SymbolType.TriangleDown,
			SymbolType.XCross,
			SymbolType.HDash,
			SymbolType.VDash
		};

        /// <summary>
        /// 颜色索引
        /// </summary>
        protected int colorIndex = 0;

        /// <summary>
        /// 符号索引
        /// </summary>
        protected int symbolIndex = 0;

        /// <summary>
        /// 获取下一种颜色
        /// </summary>
        public Color NextColor
        {
            get { return COLORS[NextColorIndex]; }
        }

        /// <summary>
        /// 获取或设置下一种颜色的索引
        /// </summary>
        public int NextColorIndex
        {
            get
            {
                if (colorIndex >= COLORS.Length)
                    colorIndex = 0;
                return colorIndex++;
            }
            set
            {
                colorIndex = value;
            }
        }

        /// <summary>
        /// 获取下一种标记
        /// </summary>
        public SymbolType NextSymbol
        {
            get { return SYMBOLS[NextSymbolIndex]; }
        }

        /// <summary>
        /// 获取或设置下一种标记的索引
        /// </summary>
        public int NextSymbolIndex
        {
            get
            {
                if (symbolIndex >= SYMBOLS.Length)
                    symbolIndex = 0;
                return symbolIndex++;
            }
            set
            {
                symbolIndex = value;
            }
        }

        /// <summary>
        /// 获取下一种颜色
        /// </summary>
        public static Color StaticNextColor
        {
            get { return StaticInstance.NextColor; }
        }

        /// <summary>
        /// 获取下一种符号
        /// </summary>
        public static SymbolType StaticNextSymbol
        {
            get { return StaticInstance.NextSymbol; }
        }
        #endregion
    }
}
