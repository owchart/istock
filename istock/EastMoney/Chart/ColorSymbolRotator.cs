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
    /// ɫ�ʷ�����ת
    /// </summary>
    public class ColorSymbolRotator
    {
        #region �յ� 2016/5/31
        private static ColorSymbolRotator m_staticInstance;

        /// <summary>
        /// ����ģʽ
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
        /// ��ɫ
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
        /// ����
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
        /// ��ɫ����
        /// </summary>
        protected int colorIndex = 0;

        /// <summary>
        /// ��������
        /// </summary>
        protected int symbolIndex = 0;

        /// <summary>
        /// ��ȡ��һ����ɫ
        /// </summary>
        public Color NextColor
        {
            get { return COLORS[NextColorIndex]; }
        }

        /// <summary>
        /// ��ȡ��������һ����ɫ������
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
        /// ��ȡ��һ�ֱ��
        /// </summary>
        public SymbolType NextSymbol
        {
            get { return SYMBOLS[NextSymbolIndex]; }
        }

        /// <summary>
        /// ��ȡ��������һ�ֱ�ǵ�����
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
        /// ��ȡ��һ����ɫ
        /// </summary>
        public static Color StaticNextColor
        {
            get { return StaticInstance.NextColor; }
        }

        /// <summary>
        /// ��ȡ��һ�ַ���
        /// </summary>
        public static SymbolType StaticNextSymbol
        {
            get { return StaticInstance.NextSymbol; }
        }
        #endregion
    }
}
