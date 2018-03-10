/*****************************************************************************\
*                                                                             *
* CDraw.cs -    Draw functions, types, and definitions.                       *
*                                                                             *
*               Version 1.00  ����                                          *
*                                                                             *
*               Copyright (c) 2016-2016, iTeam. All rights reserved.      *
*               Created by Todd 2016/3/21.                                    *
*                                                                             *
******************************************************************************/


using System;
using System.Collections.Generic;
using System.Text;
using OwLib;

namespace OwLib
{
    /// <summary>
    /// ��ͼ��
    /// </summary>
    public class CDraw
    {
        #region Lord 2016/3/21
        /// <summary>
        /// �û��Զ���ɫ
        /// </summary>
        public const long PCOLORS_USERCOLOR = -200000000100;

        /// <summary>
        /// ����ɫ
        /// </summary>
        public const long PCOLORS_BACKCOLOR = PCOLORS_USERCOLOR - 1;

        /// <summary>
        /// ����ɫ2
        /// </summary>
        public const long PCOLORS_BACKCOLOR2 = PCOLORS_USERCOLOR - 2;

        /// <summary>
        /// ����ɫ3
        /// </summary>
        public const long PCOLORS_BACKCOLOR3 = PCOLORS_USERCOLOR - 3;

        /// <summary>
        /// ����ɫ4
        /// </summary>
        public const long PCOLORS_BACKCOLOR4 = PCOLORS_USERCOLOR - 4;

        /// <summary>
        /// ����ɫ5
        /// </summary>
        public const long PCOLORS_BACKCOLOR5 = PCOLORS_USERCOLOR - 5;

        /// <summary>
        /// ����ɫ6
        /// </summary>
        public const long PCOLORS_BACKCOLOR6 = PCOLORS_USERCOLOR - 6;

        /// <summary>
        /// ����ɫ7
        /// </summary>
        public const long PCOLORS_BACKCOLOR7 = PCOLORS_USERCOLOR - 7;

        /// <summary>
        /// ����ɫ8
        /// </summary>
        public const long PCOLORS_BACKCOLOR8 = PCOLORS_USERCOLOR - 8;

        /// <summary>
        /// ����ɫ9
        /// </summary>
        public const long PCOLORS_BACKCOLOR9 = PCOLORS_USERCOLOR - 9;

        /// <summary>
        /// ����ɫ10
        /// </summary>
        public const long PCOLORS_BACKCOLOR10 = PCOLORS_USERCOLOR - 10;

        /// <summary>
        /// ����ɫ11
        /// </summary>
        public const long PCOLORS_BACKCOLOR11 = PCOLORS_USERCOLOR - 11;

        /// <summary>
        /// ����ɫ12
        /// </summary>
        public const long PCOLORS_BACKCOLOR12 = PCOLORS_USERCOLOR - 12;

        /// <summary>
        /// ǰ��ɫ
        /// </summary>
        public const long PCOLORS_FORECOLOR = PCOLORS_USERCOLOR - 100;

        /// <summary>
        /// ǰ��ɫ2
        /// </summary>
        public const long PCOLORS_FORECOLOR2 = PCOLORS_USERCOLOR - 101;

        /// <summary>
        /// ǰ��ɫ3
        /// </summary>
        public const long PCOLORS_FORECOLOR3 = PCOLORS_USERCOLOR - 102;

        /// <summary>
        /// ǰ��ɫ4
        /// </summary>
        public const long PCOLORS_FORECOLOR4 = PCOLORS_USERCOLOR - 103;

        /// <summary>
        /// �ߵ���ɫ
        /// </summary>
        public const long PCOLORS_LINECOLOR = PCOLORS_USERCOLOR - 200;

        /// <summary>
        /// �ߵ���ɫ2
        /// </summary>
        public const long PCOLORS_LINECOLOR2 = PCOLORS_USERCOLOR - 201;

        /// <summary>
        /// �ߵ���ɫ3
        /// </summary>
        public const long PCOLORS_LINECOLOR3 = PCOLORS_USERCOLOR - 202;

        /// <summary>
        /// ����ɫ
        /// </summary>
        public const long PCOLORS_UPCOLOR = PCOLORS_USERCOLOR - 301;

        /// <summary>
        /// �µ�ɫ
        /// </summary>
        public const long PCOLORS_DOWNCOLOR = PCOLORS_USERCOLOR - 302;

        /// <summary>
        /// �µ�ɫ3
        /// </summary>
        public const long PCOLORS_DOWNCOLOR3 = PCOLORS_USERCOLOR - 303;

        /// <summary>
        /// ���ǲ���ɫ
        /// </summary>
        public const long PCOLORS_MIDCOLOR = PCOLORS_USERCOLOR - 304;

        /// <summary>
        /// ѡ������ɫ
        /// </summary>
        public const long PCOLORS_SELECTEDROWCOLOR = PCOLORS_USERCOLOR - 400;

        /// <summary>
        /// ��ͣ����ɫ
        /// </summary>
        public const long PCOLORS_HOVEREDROWCOLOR = PCOLORS_USERCOLOR - 401;

        /// <summary>
        /// ��������ɫ
        /// </summary>
        public const long PCOLORS_ALTERNATEROWCOLOR = PCOLORS_USERCOLOR - 402;

        /// <summary>
        /// ����ǰ��ɫ
        /// </summary>
        public const long PCOLORS_WINDOWFORECOLOR = PCOLORS_USERCOLOR - 500;

        /// <summary>
        /// ���屳��ɫ
        /// </summary>
        public const long PCOLORS_WINDOWBACKCOLOR = PCOLORS_USERCOLOR - 501;

        /// <summary>
        /// ���屳��ɫ2
        /// </summary>
        public const long PCOLORS_WINDOWBACKCOLOR2 = PCOLORS_USERCOLOR - 502;

        /// <summary>
        /// ���屳��ɫ3
        /// </summary>
        public const long PCOLORS_WINDOWBACKCOLOR3 = PCOLORS_USERCOLOR - 503;

        /// <summary>
        /// ����ʮ������ɫ
        /// </summary>
        public const long PCOLORS_WINDOWCROSSLINECOLOR = PCOLORS_USERCOLOR - 504;

        /// <summary>
        /// ����ʮ������ɫ2
        /// </summary>
        public const long PCOLORS_WINDOWCROSSLINECOLOR2 = PCOLORS_USERCOLOR - 505;

        /// <summary>
        /// �������ݱ���ɫ
        /// </summary>
        public const long PCOLORS_WINDOWCONTENTBACKCOLOR = PCOLORS_USERCOLOR - 506;

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="text">����</param>
        /// <param name="dwPenColor">��ɫ</param>
        /// <param name="font">����</param>
        /// <param name="x">������</param>
        /// <param name="y">������</param>
        public static SIZE DrawText(CPaint paint, String text, long dwPenColor, FONT font, int x, int y)
        {
            SIZE tSize = paint.TextSize(text, font);
            RECT tRect = new RECT(x, y, x + tSize.cx, y + tSize.cy);
            paint.DrawText(text, dwPenColor, font, tRect);
            return tSize;
        }

        /// <summary>
        /// �������»��ߵ�����
        /// </summary>
        /// <param name="paint">��ͼ����</param>
        /// <param name="value">ֵ</param>
        /// <param name="digit">����С��λ��</param>
        /// <param name="font">����</param>
        /// <param name="fontColor">������ɫ</param>
        /// <param name="zeroAsEmpty">0�Ƿ�Ϊ��</param>
        /// <param name="x">������</param>
        /// <param name="y">������</param>
        /// <returns>���Ƶĺ�����</returns>
        public static int DrawUnderLineNum(CPaint paint, double value, int digit, FONT font, long fontColor, bool zeroAsEmpty, int x, int y)
        {
            if (zeroAsEmpty && value == 0)
            {
                String text = "-";
                SIZE size = paint.TextSize(text, font);
                CDraw.DrawText(paint, text, fontColor, font, x, y);
                return size.cx;
            }
            else
            {
                String[] nbs = CStr.GetValueByDigit(value, digit).Split(new String[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                if (nbs.Length == 1)
                {
                    SIZE size = paint.TextSize(nbs[0], font);
                    CDraw.DrawText(paint, nbs[0], fontColor, font, x, y);
                    return size.cx;
                }
                else
                {
                    SIZE decimalSize = paint.TextSize(nbs[0], font);
                    SIZE size = paint.TextSize(nbs[1], font);
                    CDraw.DrawText(paint, nbs[0], fontColor, font, x, y);
                    CDraw.DrawText(paint, nbs[1], fontColor, font, x
                        + decimalSize.cx + 1, y);
                    paint.DrawLine(fontColor, 1, 0, x
                        + decimalSize.cx + 1, y + decimalSize.cy,
                        x + decimalSize.cx + size.cx, y + decimalSize.cy);
                    return decimalSize.cx + size.cx;
                }
            }
        }

        /// <summary>
        /// ��ȡ��ɫ������ɫ
        /// </summary>
        /// <param name="color">��ɫ</param>
        /// <returns>�µ���ɫ</returns>
        public static long GetBlueColor(long dwPenColor)
        {
            long color = dwPenColor;
            if (color < COLOR.EMPTY)
            {
                if (color > PCOLORS_USERCOLOR)
                {
                    if (color == COLOR.CONTROL)
                    {
                        color = COLOR.ARGB(0, 0, 0);
                    }
                    else if (color == COLOR.CONTROLBORDER)
                    {
                        color = COLOR.ARGB(43, 138, 195);
                    }
                    else if (color == COLOR.CONTROLTEXT)
                    {
                        color = COLOR.ARGB(255, 255, 255);
                    }
                    else if (color == COLOR.DISABLEDCONTROL)
                    {
                        color = COLOR.ARGB(25, 255, 255, 255);
                    }
                    else if (color == COLOR.DISABLEDCONTROLTEXT)
                    {
                        color = 3289650;
                    }
                    else if (color == COLOR.HOVEREDCONTROL)
                    {
                        color = COLOR.ARGB(150, 43, 138, 195);
                    }
                    else if (color == COLOR.PUSHEDCONTROL)
                    {
                        color = COLOR.ARGB(100, 43, 138, 195);
                    }
                }
                else if (color == PCOLORS_BACKCOLOR)
                {
                    color = COLOR.ARGB(180, 43, 138, 195);
                }
                else if (color == PCOLORS_BACKCOLOR2)
                {
                    color = COLOR.ARGB(130, 43, 138, 195);
                }
                else if (color == PCOLORS_BACKCOLOR3)
                {
                    color = COLOR.ARGB(25, 255, 255, 255);
                }
                else if (color == PCOLORS_BACKCOLOR4)
                {
                    color = COLOR.ARGB(25, 0, 0, 0);
                }
                else if (color == PCOLORS_BACKCOLOR5)
                {
                    color = COLOR.ARGB(75, 51, 153, 255);
                }
                else if (color == PCOLORS_BACKCOLOR6)
                {
                    color = COLOR.ARGB(50, 51, 153, 255);
                }
                else if (color == PCOLORS_BACKCOLOR7)
                {
                    color = COLOR.ARGB(100, 255, 255, 255);
                }
                else if (color == PCOLORS_BACKCOLOR9)
                {
                    color = COLOR.ARGB(9, 30, 42);
                }
                else if (color == PCOLORS_FORECOLOR)
                {
                    color = COLOR.ARGB(255, 255, 255);
                }
                else if (color == PCOLORS_FORECOLOR2)
                {
                    color = COLOR.ARGB(112, 112, 112);
                }
                else if (color == PCOLORS_FORECOLOR3)
                {
                    color = COLOR.ARGB(100, 255, 255, 255);
                }
                else if (color == PCOLORS_LINECOLOR)
                {
                    color = COLOR.ARGB(43, 120, 195);
                }
                else if (color == PCOLORS_LINECOLOR2)
                {
                    color = COLOR.ARGB(51, 153, 255);
                }
                else if (color == CDraw.PCOLORS_UPCOLOR)
                {
                    color = COLOR.ARGB(255, 82, 82);
                }
                else if (color == CDraw.PCOLORS_DOWNCOLOR)
                {
                    color = COLOR.ARGB(80, 255, 80);
                }
                else if (color == CDraw.PCOLORS_DOWNCOLOR3)
                {
                    color = COLOR.ARGB(100, 0, 255);
                }
                else if (color == CDraw.PCOLORS_LINECOLOR3)
                {
                    color = COLOR.ARGB(5, 255, 255, 255);
                }
                else if (color == PCOLORS_SELECTEDROWCOLOR)
                {
                    color = COLOR.ARGB(43, 138, 195);
                }
                else if (color == PCOLORS_HOVEREDROWCOLOR)
                {
                    color = COLOR.ARGB(200, 43, 138, 195);
                }
                else if (color == PCOLORS_ALTERNATEROWCOLOR)
                {
                    color = COLOR.ARGB(100, 43, 138, 195);
                }
                else if (color == CDraw.PCOLORS_WINDOWFORECOLOR)
                {
                    color = COLOR.ARGB(255, 255, 255);
                }
                else if (color == CDraw.PCOLORS_WINDOWBACKCOLOR)
                {
                    color = COLOR.ARGB(255, 50, 50, 50);
                }
                else if (color == CDraw.PCOLORS_WINDOWBACKCOLOR2)
                {
                    color = COLOR.ARGB(230, 43, 138, 195);
                }
                else if (color == CDraw.PCOLORS_WINDOWCONTENTBACKCOLOR)
                {
                    color = COLOR.ARGB(235, 9, 30, 42);
                }
            }
            return color;
        }

        /// <summary>
        /// ��ȡ��ɫ������ɫ
        /// </summary>
        /// <param name="color">��ɫ</param>
        /// <returns>�µ���ɫ</returns>
        public static long GetWhiteColor(long dwPenColor)
        {
            long color = dwPenColor;
            if (color < COLOR.EMPTY)
            {
                if (color > PCOLORS_USERCOLOR)
                {
                    if (color == COLOR.CONTROL)
                    {
                        color = COLOR.ARGB(255, 255, 255);
                    }
                    else if (color == COLOR.CONTROLBORDER)
                    {
                        color = COLOR.ARGB(200, 200, 200);
                    }
                    else if (color == COLOR.CONTROLTEXT)
                    {
                        color = COLOR.ARGB(0, 0, 0);
                    }
                    else if (color == COLOR.DISABLEDCONTROL)
                    {
                        color = COLOR.ARGB(25, 255, 255, 255);
                    }
                    else if (color == COLOR.DISABLEDCONTROLTEXT)
                    {
                        color = 3289650;
                    }
                    else if (color == COLOR.HOVEREDCONTROL)
                    {
                        color = COLOR.ARGB(150, 200, 200, 200);
                    }
                    else if (color == COLOR.PUSHEDCONTROL)
                    {
                        color = COLOR.ARGB(150, 150, 150, 150);
                    }
                }
                else if (color == PCOLORS_BACKCOLOR)
                {
                    color = COLOR.ARGB(255, 255, 255);
                }
                else if (color == PCOLORS_BACKCOLOR2)
                {
                    color = COLOR.ARGB(230, 230, 230);
                }
                else if (color == PCOLORS_BACKCOLOR3)
                {
                    color = COLOR.ARGB(25, 200, 200, 200);
                }
                else if (color == PCOLORS_BACKCOLOR4)
                {
                    color = COLOR.ARGB(25, 100, 100, 100);
                }
                else if (color == PCOLORS_BACKCOLOR5)
                {
                    color = COLOR.ARGB(75, 51, 153, 255);
                }
                else if (color == PCOLORS_BACKCOLOR6)
                {
                    color = COLOR.ARGB(50, 51, 153, 255);
                }
                else if (color == PCOLORS_BACKCOLOR7)
                {
                    color = COLOR.ARGB(100, 255, 255, 255);
                }
                else if (color == PCOLORS_BACKCOLOR8)
                {
                    color = COLOR.ARGB(50, 105, 217);
                }
                else if (color == PCOLORS_BACKCOLOR9)
                {
                    color = COLOR.ARGB(75, 215, 99);
                }
                else if (color == PCOLORS_BACKCOLOR10)
                {
                    color = COLOR.ARGB(186, 85, 211);
                }
                else if (color == PCOLORS_BACKCOLOR11)
                {
                    color = COLOR.ARGB(160, 82, 45);
                }
                else if (color == PCOLORS_BACKCOLOR12)
                {
                    color = COLOR.ARGB(255, 105, 180);
                }
                else if (color == PCOLORS_FORECOLOR)
                {
                    color = COLOR.ARGB(0, 0, 0);
                }
                else if (color == PCOLORS_FORECOLOR2)
                {
                    color = COLOR.ARGB(112, 112, 112);
                }
                else if (color == PCOLORS_FORECOLOR3)
                {
                    color = COLOR.ARGB(100, 255, 255, 255);
                }
                else if (color == PCOLORS_FORECOLOR4)
                {
                    color = COLOR.ARGB(255, 255, 255);
                }
                else if (color == PCOLORS_LINECOLOR)
                {
                    color = COLOR.ARGB(100, 100, 100);
                }
                else if (color == PCOLORS_LINECOLOR2)
                {
                    color = COLOR.ARGB(0, 105, 217);
                }
                else if (color == CDraw.PCOLORS_UPCOLOR)
                {
                    color = COLOR.ARGB(255, 82, 82);
                }
                else if (color == CDraw.PCOLORS_DOWNCOLOR)
                {
                    color = COLOR.ARGB(80, 255, 80);
                }
                else if (color == CDraw.PCOLORS_DOWNCOLOR3)
                {
                    color = COLOR.ARGB(100, 0, 255);
                }
                else if (color == CDraw.PCOLORS_LINECOLOR3)
                {
                    color = COLOR.ARGB(5, 255, 255, 255);
                }
                else if (color == PCOLORS_SELECTEDROWCOLOR)
                {
                    color = COLOR.ARGB(0, 105, 217);
                }
                else if (color == PCOLORS_HOVEREDROWCOLOR)
                {
                    color = COLOR.ARGB(240, 240, 240);
                }
                else if (color == PCOLORS_ALTERNATEROWCOLOR)
                {
                    color = COLOR.ARGB(245, 245, 245);
                }
                else if (color == CDraw.PCOLORS_WINDOWFORECOLOR)
                {
                    color = COLOR.ARGB(0, 0, 0);
                }
                else if (color == CDraw.PCOLORS_WINDOWBACKCOLOR)
                {
                    color = COLOR.ARGB(255, 255, 255);
                }
                else if (color == CDraw.PCOLORS_WINDOWBACKCOLOR2)
                {
                    color = COLOR.ARGB(100, 255, 255, 255);
                }
                else if (color == CDraw.PCOLORS_WINDOWBACKCOLOR3)
                {
                    color = COLOR.ARGB(230, 255, 255, 255);
                }
                else if (color == CDraw.PCOLORS_WINDOWCROSSLINECOLOR)
                {
                    color = COLOR.ARGB(100, 100, 100);
                }
                else if (color == CDraw.PCOLORS_WINDOWCROSSLINECOLOR2)
                {
                    color = COLOR.ARGB(10, 255, 255, 255);
                }
                else if (color == CDraw.PCOLORS_WINDOWCONTENTBACKCOLOR)
                {
                    color = COLOR.ARGB(235, 255, 255, 255);
                }
            }
            return color;
        }

        /// <summary>
        /// ���ݼ۸��ȡ��ɫ
        /// </summary>
        /// <param name="price">�۸�</param>
        /// <param name="comparePrice">�Ƚϼ۸�</param>
        /// <returns>��ɫ</returns>
        public static long GetPriceColor(double price, double comparePrice)
        {
            if (price != 0)
            {
                if (price > comparePrice)
                {
                    return CDraw.PCOLORS_UPCOLOR;
                }
                else if (price < comparePrice)
                {
                    return CDraw.PCOLORS_DOWNCOLOR;
                }
            }
            return COLOR.ARGB(255, 255, 255);
        }
        #endregion
    }
}
