using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace OwLib
{
    public class FormulaFunctionOutput:IDisposable
    {
        public virtual void Dispose()
        {
        }
    }

    public class PolyLineOutput : FormulaFunctionOutput
    {
        public double[] Price;

    }

    public class DrawLineOutput : FormulaFunctionOutput 
    {
        public double[] Price;

        public double[] Expand;

    }

    public class DrawKlineOutput : FormulaFunctionOutput 
    {
        public double[] High;

        public double[] Open;

        public double[] Low;

        public double[] Close;

    }

    public class StickLineOutput : FormulaFunctionOutput 
    {
        public double[] Price1;

        public double[] Price2;

        public double[] Width;

        public double[] Empty;

    }

    public class DrawIconOutput : FormulaFunctionOutput 
    {
        public double[] Price;
        //TODO: 确认Icon类型
        public int[] Icon;
    }

    public class DrawTextOutput : FormulaFunctionOutput
    {
        public double[] Price;
        
        public String Text;

    }

    public class DrawNumberOutput : FormulaFunctionOutput
    {
        public double[] Price;

        public double[] Number;

    }

    public class DrawBandOutput : FormulaFunctionOutput
    {
        public double[] Price1;

        public double[] Price2;

        public Color[] BandColor;

    }

    public class DrawFloatRGNOutput : FormulaFunctionOutput
    {
        public double[] Price;

        public double[] Width;

        public FloatRGNPara[] para;

        public int N;

    }

    public struct FloatRGNPara
    {
        public int[] Cond;

        public Color[] FloatRGNColor;
    }

    public class DrawTWROutput : FormulaFunctionOutput
    {
        public DrawTWRData[] data;

    }

    public struct DrawTWRData
    {
        public char Up;

        public double Top;

        public double Center;

        public double Bottom;
    }

    public class FillRGNOutput : FormulaFunctionOutput
    {
        public double[] Price1;

        public double[] Price2;

        public FillRGNPara[] Para;

        public Color[] FillRGNColor;

        public int N;

    }

    public struct FillRGNPara
    {
        public int[] Cond;

        public Color[] FillRGNColor;
    }

    public class DrawGBKOutput : FormulaFunctionOutput
    {
        public Color[] DrawGBKColor;

    }
}