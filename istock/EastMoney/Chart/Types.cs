/********************************************************************************\
*                                                                                *
* Types.cs -    Types functions, types, and definitions                          *
*                                                                                *
*               Version 1.00                                                     *
*                                                                                *
*               Copyright (c) 2016-2016, Todd's graph. All rights reserved.      *
*                                                                                *
*********************************************************************************/

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace OwLib
{
    /// <summary>
    /// 
    /// </summary>
    public enum AlignH
    {
        Left,
        Center,
        Right
    }

    /// <summary>
    /// 
    /// </summary>
    public enum AlignP
    {
        Inside,
        Center,
        Outside
    }

    /// <summary>
    /// 
    /// </summary>
    public enum AlignV
    {
        Top,
        Center,
        Bottom
    }

    /// <summary>
    /// 
    /// </summary>
    public enum AxisType
    {
        Linear,
        Log,
        Date,
        Text,
        Ordinal,
        DateAsOrdinal,
        LinearAsOrdinal,
        Exponent
    }

    /// <summary>
    /// 
    /// </summary>
    public enum BarBase
    {
        X,
        Y,
        Y2
    }

    /// <summary>
    /// 
    /// </summary>
    public enum BarType
    {
        Cluster,
        Overlay,
        SortedOverlay,
        Stack,
        PercentStack
    }

    /// <summary>
    /// 
    /// </summary>
    public enum CoordType
    {
        ChartFraction,
        PaneFraction,
        AxisXYScale,
        AxisXY2Scale,
        XChartFractionYPaneFraction,
        XPaneFractionYChartFraction,
        XScaleYChartFraction,
        XChartFractionYScale,
        XChartFractionY2Scale
    }

    /// <summary>
    /// 
    /// </summary>
    public enum DateUnit
    {
        Year,
        Month,
        Day,
        Hour,
        Minute,
        Second,
        Millisecond
    }

    /// <summary>
    /// 
    /// </summary>
    public enum FillType
    {
        None,
        Solid,
        Brush,
        GradientByX,
        GradientByY,
        GradientByZ,
        GradientByColorValue
    }

    /// <summary>
    /// 
    /// </summary>
    public enum LegendPos
    {
        Top,
        Left,
        Right,
        Bottom,
        InsideTopLeft,
        InsideTopRight,
        InsideBotLeft,
        InsideBotRight,
        Float,
        TopCenter,
        TopRight,
        BottomCenter,
        TopFlushLeft,
        BottomFlushLeft
    }

    /// <summary>
    /// 
    /// </summary>
    public enum LineType
    {
        Normal,
        Stack
    }

    /// <summary>
    /// 
    /// </summary>
    public enum PaneLayout
    {
        ForceSquare,
        SquareColPreferred,
        SquareRowPreferred,
        SingleRow,
        SingleColumn,
        ExplicitCol12,
        ExplicitCol21,
        ExplicitCol23,
        ExplicitCol32,
        ExplicitRow12,
        ExplicitRow21,
        ExplicitRow23,
        ExplicitRow32,
        FixedRow,
        FixedCol
    }

    /// <summary>
    /// 
    /// </summary>
    public enum PieLabelType
    {
        Name_Value,
        Name_Percent,
        Name_Value_Percent,
        Value,
        Percent,
        Name,
        None
    }

    /// <summary>
    /// 
    /// </summary>
    public enum SortType2
    {
        YValues,
        XValues
    };

    /// <summary>
    /// 
    /// </summary>
    public enum StepType
    {
        ForwardStep,
        RearwardStep,
        NonStep,
        ForwardSegment,
        RearwardSegment
    }

    /// <summary>
    /// 
    /// </summary>
    public enum SymbolType
    {
        Square,
        Diamond,
        Triangle,
        Circle,
        XCross,
        Plus,
        Star,
        TriangleDown,
        HDash,
        VDash,
        UserDefined,
        Default,
        None
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ZOrder
    {
        H_BehindAll,
        G_BehindChartFill,
        F_BehindGrid,
        E_BehindCurves,
        D_BehindAxis,
        C_BehindChartBorder,
        B_BehindLegend,
        A_InFront
    }
}
