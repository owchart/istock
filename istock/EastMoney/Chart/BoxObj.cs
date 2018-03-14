using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Runtime.Serialization;
using System.Security.Permissions;
namespace OwLib
{
    [Serializable]
    public class BoxObj : GraphObjRect
    {
        public BoxObj(double x, double y, double width, double height, Color borderColor, Color fillColor)
            : base(x, y, width, height)
        {
            this.Border = new Border(borderColor, Default.PenWidth);
            this.Fill = new Fill(fillColor);
        }

        public BoxObj(double x, double y, double width, double height)
            :
            base(x, y, width, height)
        {
            this.Border = new Border(Default.BorderColor, Default.PenWidth);
            this.Fill = new Fill(Default.FillColor);
        }

        public BoxObj()
            : this(0, 0, 1, 1)
        {
        }

        public BoxObj(double x, double y, double width, double height, Color borderColor,
                            Color fillColor1, Color fillColor2)
            :
                base(x, y, width, height)
        {
            this.Border = new Border(borderColor, Default.PenWidth);
            this.Fill = new Fill(fillColor1, fillColor2);
        }

        public BoxObj(BoxObj rhs)
            : base(rhs)
        {
            this.Border = new Border(rhs.Border);
            this.Fill = new Fill(rhs.Fill);
        }

        public override void Draw(Graphics g, PaneBase pane, float scaleFactor)
        {
            RectangleF pixRect = this.Location.TransformRect(pane);
            RectangleF tmpRect = pane.Rect;
            tmpRect.Inflate(20, 20);
            pixRect.Intersect(tmpRect);
            if (Math.Abs(pixRect.Left) < 100000 &&
                    Math.Abs(pixRect.Top) < 100000 &&
                    Math.Abs(pixRect.Right) < 100000 &&
                    Math.Abs(pixRect.Bottom) < 100000)
            {
                m_fill.Draw(g, pixRect);
                m_border.Draw(g, pane, scaleFactor, pixRect);
            }
            DrawControler(g, pixRect);
            DrawText(g, scaleFactor, pixRect, null);
            this.rect = pixRect;
            this.graphPane = (GraphPane)pane;
        }
        public override bool PointInBox(PointF pt, PaneBase pane, Graphics g, float scaleFactor)
        {
            if (!base.PointInBox(pt, pane, g, scaleFactor))
                return false;
            RectangleF pixRect = m_location.TransformRect(pane);
            return pixRect.Contains(pt);
        }
        public override void GetCoords(PaneBase pane, Graphics g, float scaleFactor,
                out String shape, out String coords)
        {
            RectangleF pixRect = m_location.TransformRect(pane);
            shape = "rect";
            coords = String.Format("{0:f0},{1:f0},{2:f0},{3:f0}",
                        pixRect.Left, pixRect.Top, pixRect.Right, pixRect.Bottom);
        }
    }
}
