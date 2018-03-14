/********************************************************************************\
*                                                                                *
* TextObj.cs -    TextObj functions, types, and definitions                      *
*                                                                                *
*               Version 1.00                                                     *
*                                                                                *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.      *
*                                                                                *
*********************************************************************************/

using System;
using System.Drawing;
using System.Collections;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace OwLib
{
    /// <summary>
    /// ����
    /// </summary>
    [Serializable]
    public class TextObj : GraphObjRect
    {
        #region �յ� 2016/6/7
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="text">����</param>
        /// <param name="x">������ֵ</param>
        /// <param name="y">������ֵ</param>
        public TextObj(String text, double x, double y)
            : base(x, y)
        {
            Init(text);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="text">����</param>
        /// <param name="x">������ֵ</param>
        /// <param name="y">������ֵ</param>
        /// <param name="coordType">����������</param>
        public TextObj(String text, double x, double y, CoordType coordType)
            : base(x, y, coordType)
        {
            Init(text);
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="text">����</param>
        /// <param name="x">������ֵ</param>
        /// <param name="y">������ֵ</param>
        /// <param name="coordType">����������</param>
        /// <param name="alignH">�Ƿ�������</param>
        /// <param name="alignV">�Ƿ��������</param>
        public TextObj(String text, double x, double y, CoordType coordType, AlignH alignH, AlignV alignV)
            : base(x, y, coordType, alignH, alignV)
        {
            Init(text);
        }

        /// <summary>
        /// ��������
        /// </summary>
        public TextObj()
            : base(0, 0)
        {
            Init("");
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="rhs">��������</param>
        public TextObj(TextObj rhs)
            : base(rhs)
        {
            m_text = rhs.Text;
            m_fontSpec = new FontSpec(rhs.FontSpec);
        }

        /// <summary>
        /// Ĭ������
        /// </summary>
        public new struct Default
        {
            public static String FontFamily = "Arial";
            public static float FontSize = 12.0F;
            public static Color FontColor = Color.FromArgb(30, 30, 30);
            public static bool FontBold = false;
            public static bool FontUnderline = false;
            public static bool FontItalic = false;
        }

        private FontSpec m_fontSpec;

        /// <summary>
        /// 
        /// </summary>
        public FontSpec FontSpec
        {
            get { return m_fontSpec; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("Uninitialized FontSpec in TextObj");
                m_fontSpec = value;
            }
        }

        private SizeF m_layoutArea;

        /// <summary>
        /// 
        /// </summary>
        public SizeF LayoutArea
        {
            get { return m_layoutArea; }
            set { m_layoutArea = value; }
        }

        private String m_text;

        /// <summary>
        /// 
        /// </summary>
        public String Text
        {
            get { return m_text; }
            set { m_text = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g">��ͼ����</param>
        /// <param name="pane">ͼ��</param>
        /// <param name="scaleFactor">�̶�����</param>
        public override void Draw(Graphics g, PaneBase pane, float scaleFactor)
        {
            PointF pix = m_location.Transform(pane);
            if (pix.X > -100000 && pix.X < 100000 && pix.Y > -100000 && pix.Y < 100000)
            {
                this.FontSpec.Draw(g, pane, m_text, pix.X, pix.Y,
                    m_location.AlignH, m_location.AlignV, scaleFactor, m_layoutArea);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pane">ͼ��</param>
        /// <param name="g">��ͼ����</param>
        /// <param name="scaleFactor">�̶�����</param>
        /// <param name="shape">ͼ��</param>
        /// <param name="coords">����</param>
        public override void GetCoords(PaneBase pane, Graphics g, float scaleFactor,
        out String shape, out String coords)
        {
            PointF pix = m_location.Transform(pane);
            PointF[] pts = m_fontSpec.GetBox(g, m_text, pix.X, pix.Y, m_location.AlignH,
                m_location.AlignV, scaleFactor, new SizeF());
            shape = "poly";
            coords = String.Format("{0:f0},{1:f0},{2:f0},{3:f0},{4:f0},{5:f0},{6:f0},{7:f0},",
                        pts[0].X, pts[0].Y, pts[1].X, pts[1].Y,
                        pts[2].X, pts[2].Y, pts[3].X, pts[3].Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">����</param>
        private void Init(String text)
        {
            if (text != null)
                m_text = text;
            else
                text = "Text";
            m_fontSpec = new FontSpec(
                Default.FontFamily, Default.FontSize,
                Default.FontColor, Default.FontBold,
                Default.FontItalic, Default.FontUnderline);
            m_layoutArea = new SizeF(0, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt">����</param>
        /// <param name="pane">ͼ��</param>
        /// <param name="g">��ͼ����</param>
        /// <param name="scaleFactor">�̶�����</param>
        /// <returns></returns>
        public override bool PointInBox(PointF pt, PaneBase pane, Graphics g, float scaleFactor)
        {
            if (!base.PointInBox(pt, pane, g, scaleFactor))
                return false;
            PointF pix = m_location.Transform(pane);
            return m_fontSpec.PointInBox(pt, g, m_text, pix.X, pix.Y,
                                m_location.AlignH, m_location.AlignV, scaleFactor, this.LayoutArea);
        }
        #endregion
    }
}
