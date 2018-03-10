using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using EmQComm;
using EmQDataIO;

namespace EmQDataCore
{
    /// <summary>
    /// 字段格式
    /// </summary>
    public class FieldFormat
    {
        /*        private static DataCenterCore _dc;

        static FieldFormat()
        {
            _dc = DataCenterCore.CreateInstance();
        }

        /// <summary>
        /// 获取一只股票某个字段的输出信息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="fieldName"></param>
        /// <param name="output"></param>
        /// <param name="brush"></param>
        public static void GetFieldOutput(int code, string fieldName, out string output, out SolidBrush brush)
        {
            output = string.Empty;
            brush = QuoteDrawService.BrushColorCode;

            try
            {
                FieldInfo fieldInfo = GetFieldInfo(code, fieldName);
                if (fieldInfo == null)
                    return;

                FieldIndex field;
                if (Enum.TryParse(fieldInfo.ColorSetting[1], out field))
                {

                    if (field >= 0 && (int) field <= 299)
                    {
                        int data = _dc.GetFieldDataInt32(code, field);
                        output = SetFieldFormat(data, fieldInfo, code);
                        brush = GetFieldBrush(data, fieldInfo, code);
                    }
                    if ((int) field >= 300 && (int) field <= 799)
                    {
                        float data = _dc.GetFieldDataSingle(code, field);
                        output = SetFieldFormat(data, fieldInfo, code);
                        brush = GetFieldBrush(data, fieldInfo, code);
                    }
                    if ((int) field >= 800 && (int) field <= 999)
                    {
                        double data = _dc.GetFieldDataDouble(code, field);
                        output = SetFieldFormat(data, fieldInfo, code);
                        brush = GetFieldBrush(data, fieldInfo, code);
                    }
                    if ((int) field >= 1000 && (int) field <= 1199)
                    {
                        long data = _dc.GetFieldDataInt64(code, field);
                        output = SetFieldFormat(data, fieldInfo, code);
                        brush = GetFieldBrush(data, fieldInfo, code);
                    }
                }
            }
            catch (Exception e)
            {
                LogUtilities.LogMessage("GetFieldOutput  + " + e.Message);
            }
        }

        /// <summary>
        /// 获取一只股票某个字段的导出格式
        /// </summary>
        /// <param name="code"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static string GetFieldExport(int code, string fieldName)
        {
            return string.Empty;
        }

        private static EmQDataIO.FieldInfo GetFieldInfo(int code, string fieldName)
        {
            FieldInfo fieldInfo = null;
            Dictionary<string, FieldInfo> fieldInfoArray;
            MarketType mt = _dc.GetMarketType(code);

            if (FieldInfoCfgFileIO.DicMarketFieldInfo.TryGetValue(mt, out fieldInfoArray))
            {
                if (fieldInfoArray.TryGetValue(fieldName, out fieldInfo))
                    return fieldInfo;
            }

            FieldInfoCfgFileIO.DicDefaultFieldInfo.TryGetValue(fieldName, out fieldInfo);

            return fieldInfo;

        }

        #region 设置Format字符串

        private static string SetFieldFormat(int data, FieldInfo fieldInfo, int code)  
        {
            string prefix = string.Empty;
            string postfix = string.Empty;
            string body = string.Empty;
            string round = string.Empty;
            bool isShowBody = false;

            foreach (List<string> oneFormat in fieldInfo.FormatSetting)
            {
                try
                {
                    switch (oneFormat[0])
                    {
                        case "A":
                            data = Convert.ToInt32(data + Convert.ToSingle(oneFormat[1]));
                            if (oneFormat.Count == 3)
                                round = oneFormat[2];
                            break;
                        case "C":
                            data = Convert.ToInt32(data - Convert.ToSingle(oneFormat[1]));
                            if (oneFormat.Count == 3)
                                round = oneFormat[2];
                            break;
                        case "M":
                            data = Convert.ToInt32(data*Convert.ToSingle(oneFormat[1]));
                            if (oneFormat.Count == 3)
                                round = oneFormat[2];
                            break;
                        case "D":
                            float d = Convert.ToSingle(oneFormat[1]);
                            if (d != 0)
                                data = Convert.ToInt32(data/d);
                            if (oneFormat.Count == 3)
                                round = oneFormat[2];
                            break;
                        case "N":
                            round = oneFormat[1];
                            break;
                        case "P":
                            if (oneFormat[1].Equals("$0") && data > 0)
                                prefix = "+";
                            else
                                prefix = oneFormat[1];
                            break;
                        case "Q":
                            postfix = oneFormat[1];
                            break;
                        case "S":
                            switch (oneFormat[1])
                            {
                                case "Date":
                                    if (oneFormat[1].Length == 8)
                                    {
                                        body = oneFormat[1].Substring(0, 4) + "-" + oneFormat[1].Substring(4, 2) + "-" +
                                               oneFormat[1].Substring(6, 2);
                                        isShowBody = true;
                                    }
                                    break;
                                case "Time":
                                    if (oneFormat[1].Length == 6)
                                    {
                                        body = oneFormat[1].Substring(0, 2) + ":" + oneFormat[1].Substring(2, 2) + ":" +
                                               oneFormat[1].Substring(4, 2);
                                        isShowBody = true;
                                    }
                                    break;
                                case "Volume":
                                    if (data >= 1000000 || data <= -1000000)//100亿
                                    {
                                        data /= 10000;
                                        postfix = "亿";
                                        round = "F0";
                                    }
                                    else if (data >= 10000 || data <= -10000)//99.99亿
                                    {
                                        data /= 10000;
                                        postfix = "亿";
                                        round = "F2";
                                    }
                                    else if (data >= 100 || data <= -100)//100~9999万
                                    {
                                        postfix = "万";
                                        round = "F0";
                                    }
                                    else if (data >= 1 || data <= -1) //99.99万
                                    {
                                        postfix = "万";
                                        round = "F2";
                                    }
                                    else
                                        round = "F0";
                                    break;
                            }
                            break;
                        case "Z":
                            bool isZero = false;
                            if (oneFormat[1].Equals("this"))
                            {
                                if (data == 0)
                                    isZero = true;
                            }
                            else
                            {
                                FieldIndex field;
                                if (Enum.TryParse(oneFormat[1], out field))
                                {
                                    if (field >= 0 && (int) field <= 299)
                                    {
                                        if (_dc.GetFieldDataInt32(code, field) == 0)
                                            isZero = true;
                                    }
                                    if ((int) field >= 300 && (int) field <= 799)
                                    {
                                        if (_dc.GetFieldDataSingle(code, field) == 0)
                                            isZero = true;
                                    }
                                    if ((int) field >= 800 && (int) field <= 999)
                                    {
                                        if (_dc.GetFieldDataDouble(code, field) == 0)
                                            isZero = true;
                                    }
                                    if ((int) field >= 1000 && (int) field <= 1199)
                                    {
                                        if (_dc.GetFieldDataInt64(code, field) == 0)
                                            isZero = true;
                                    }
                                }
                            }
                            if (isZero)
                            {
                                if (oneFormat[2].Equals("H"))
                                {
                                    body = "-";
                                    isShowBody = true;
                                }
                                else
                                {
                                    body = string.Empty;
                                    isShowBody = true;
                                }
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    LogUtilities.LogMessage("oneFormat error :" + e.Message);
                    continue;
                }
            }

            if (!isShowBody)
                body = data.ToString(round);
            return prefix + body + postfix;
        }

        private static string SetFieldFormat(float data, FieldInfo fieldInfo, int code)
        {
            string prefix = string.Empty;
            string postfix = string.Empty;
            string body = string.Empty;
            string round = string.Empty;
            bool isShowBody = false;

            foreach (List<string> oneFormat in fieldInfo.FormatSetting)
            {
                try
                {
                    switch (oneFormat[0])
                    {
                        case "A":
                            data = Convert.ToSingle(data + Convert.ToSingle(oneFormat[1]));
                            if (oneFormat.Count == 3)
                                round = oneFormat[2];
                            break;
                        case "C":
                            data = Convert.ToSingle(data - Convert.ToSingle(oneFormat[1]));
                            if (oneFormat.Count == 3)
                                round = oneFormat[2];
                            break;
                        case "M":
                            data = Convert.ToSingle(data * Convert.ToSingle(oneFormat[1]));
                            if (oneFormat.Count == 3)
                                round = oneFormat[2];
                            break;
                        case "D":
                            float d = Convert.ToSingle(oneFormat[1]);
                            if (d != 0)
                                data = Convert.ToSingle(data / d);
                            if (oneFormat.Count == 3)
                                round = oneFormat[2];
                            break;
                        case "N":
                            round = oneFormat[1];
                            break;
                        case "P":
                            if (oneFormat[1].Equals("$0") && data > 0)
                                prefix = "+";
                            else
                                prefix = oneFormat[1];
                            break;
                        case "Q":
                            postfix = oneFormat[1];
                            break;
                        case "S":
                            switch (oneFormat[1])
                            {
                                case "Date":
                                    if (oneFormat[1].Length == 8)
                                    {
                                        body = oneFormat[1].Substring(0, 4) + "-" + oneFormat[1].Substring(4, 2) + "-" +
                                               oneFormat[1].Substring(6, 2);
                                        isShowBody = true;
                                    }
                                    break;
                                case "Time":
                                    if (oneFormat[1].Length == 6)
                                    {
                                        body = oneFormat[1].Substring(0, 2) + ":" + oneFormat[1].Substring(2, 2) + ":" +
                                               oneFormat[1].Substring(4, 2);
                                        isShowBody = true;
                                    }
                                    break;
                                case "Volume":
                                    if (data >= 1000000 || data <= -1000000)//100亿
                                    {
                                        data /= 10000;
                                        postfix = "亿";
                                        round = "F0";
                                    }
                                    else if (data >= 10000 || data <= -10000)//99.99亿
                                    {
                                        data /= 10000;
                                        postfix = "亿";
                                        round = "F2";
                                    }
                                    else if (data >= 100 || data <= -100)//100~9999万
                                    {
                                        postfix = "万";
                                        round = "F0";
                                    }
                                    else if (data >= 1 || data <= -1) //99.99万
                                    {
                                        postfix = "万";
                                        round = "F2";
                                    }
                                    else
                                        round = "F0";
                                    break;
                            }
                            break;
                        case "Z":
                            bool isZero = false;
                            if (oneFormat[1].Equals("this"))
                            {
                                if (data == 0)
                                    isZero = true;
                            }
                            else
                            {
                                FieldIndex field;
                                if (Enum.TryParse(oneFormat[1], out field))
                                {
                                    if (field >= 0 && (int)field <= 299)
                                    {
                                        if (_dc.GetFieldDataInt32(code, field) == 0)
                                            isZero = true;
                                    }
                                    if ((int)field >= 300 && (int)field <= 799)
                                    {
                                        if (_dc.GetFieldDataSingle(code, field) == 0)
                                            isZero = true;
                                    }
                                    if ((int)field >= 800 && (int)field <= 999)
                                    {
                                        if (_dc.GetFieldDataDouble(code, field) == 0)
                                            isZero = true;
                                    }
                                    if ((int)field >= 1000 && (int)field <= 1199)
                                    {
                                        if (_dc.GetFieldDataInt64(code, field) == 0)
                                            isZero = true;
                                    }
                                }
                            }
                            if (isZero)
                            {
                                if (oneFormat[2].Equals("H"))
                                {
                                    body = "-";
                                    isShowBody = true;
                                }
                                else
                                {
                                    body = string.Empty;
                                    isShowBody = true;
                                }
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    LogUtilities.LogMessage("oneFormat error :" + e.Message);
                    continue;
                }
            }

            if (!isShowBody)
                body = data.ToString(round);
            return prefix + body + postfix;
        }

        private static string SetFieldFormat(long data, FieldInfo fieldInfo, int code)
        {
            string prefix = string.Empty;
            string postfix = string.Empty;
            string body = string.Empty;
            string round = string.Empty;
            bool isShowBody = false;

            foreach (List<string> oneFormat in fieldInfo.FormatSetting)
            {
                try
                {
                    switch (oneFormat[0])
                    {
                        case "A":
                            data = Convert.ToInt64(data + Convert.ToSingle(oneFormat[1]));
                            if (oneFormat.Count == 3)
                                round = oneFormat[2];
                            break;
                        case "C":
                            data = Convert.ToInt64(data - Convert.ToSingle(oneFormat[1]));
                            if (oneFormat.Count == 3)
                                round = oneFormat[2];
                            break;
                        case "M":
                            data = Convert.ToInt64(data * Convert.ToSingle(oneFormat[1]));
                            if (oneFormat.Count == 3)
                                round = oneFormat[2];
                            break;
                        case "D":
                            float d = Convert.ToSingle(oneFormat[1]);
                            if (d != 0)
                                data = Convert.ToInt64(data / d);
                            if (oneFormat.Count == 3)
                                round = oneFormat[2];
                            break;
                        case "N":
                            round = oneFormat[1];
                            break;
                        case "P":
                            if (oneFormat[1].Equals("$0") && data > 0)
                                prefix = "+";
                            else
                                prefix = oneFormat[1];
                            break;
                        case "Q":
                            postfix = oneFormat[1];
                            break;
                        case "S":
                            switch (oneFormat[1])
                            {
                                case "Date":
                                    if (oneFormat[1].Length == 8)
                                    {
                                        body = oneFormat[1].Substring(0, 4) + "-" + oneFormat[1].Substring(4, 2) + "-" +
                                               oneFormat[1].Substring(6, 2);
                                        isShowBody = true;
                                    }
                                    break;
                                case "Time":
                                    if (oneFormat[1].Length == 6)
                                    {
                                        body = oneFormat[1].Substring(0, 2) + ":" + oneFormat[1].Substring(2, 2) + ":" +
                                               oneFormat[1].Substring(4, 2);
                                        isShowBody = true;
                                    }
                                    break;
                                case "Volume":
                                    if (data >= 1000000 || data <= -1000000)//100亿
                                    {
                                        data /= 10000;
                                        postfix = "亿";
                                        round = "F0";
                                    }
                                    else if (data >= 10000 || data <= -10000)//99.99亿
                                    {
                                        data /= 10000;
                                        postfix = "亿";
                                        round = "F2";
                                    }
                                    else if (data >= 100 || data <= -100)//100~9999万
                                    {
                                        postfix = "万";
                                        round = "F0";
                                    }
                                    else if (data >= 1 || data <= -1) //99.99万
                                    {
                                        postfix = "万";
                                        round = "F2";
                                    }
                                    else
                                        round = "F0";
                                    break;
                            }
                            break;
                        case "Z":
                            bool isZero = false;
                            if (oneFormat[1].Equals("this"))
                            {
                                if (data == 0)
                                    isZero = true;
                            }
                            else
                            {
                                FieldIndex field;
                                if (Enum.TryParse(oneFormat[1], out field))
                                {
                                    if (field >= 0 && (int)field <= 299)
                                    {
                                        if (_dc.GetFieldDataInt32(code, field) == 0)
                                            isZero = true;
                                    }
                                    if ((int)field >= 300 && (int)field <= 799)
                                    {
                                        if (_dc.GetFieldDataSingle(code, field) == 0)
                                            isZero = true;
                                    }
                                    if ((int)field >= 800 && (int)field <= 999)
                                    {
                                        if (_dc.GetFieldDataDouble(code, field) == 0)
                                            isZero = true;
                                    }
                                    if ((int)field >= 1000 && (int)field <= 1199)
                                    {
                                        if (_dc.GetFieldDataInt64(code, field) == 0)
                                            isZero = true;
                                    }
                                }
                            }
                            if (isZero)
                            {
                                if (oneFormat[2].Equals("H"))
                                {
                                    body = "-";
                                    isShowBody = true;
                                }
                                else
                                {
                                    body = string.Empty;
                                    isShowBody = true;
                                }
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    LogUtilities.LogMessage("oneFormat error :" + e.Message);
                    continue;
                }
            }

            if (!isShowBody)
                body = data.ToString(round);
            return prefix + body + postfix;
        }

        private static string SetFieldFormat(double data, FieldInfo fieldInfo, int code)
        {
            string prefix = string.Empty;
            string postfix = string.Empty;
            string body = string.Empty;
            string round = string.Empty;
            bool isShowBody = false;

            foreach (List<string> oneFormat in fieldInfo.FormatSetting)
            {
                try
                {
                    switch (oneFormat[0])
                    {
                        case "A":
                            data = Convert.ToDouble(data + Convert.ToSingle(oneFormat[1]));
                            if (oneFormat.Count == 3)
                                round = oneFormat[2];
                            break;
                        case "C":
                            data = Convert.ToDouble(data - Convert.ToSingle(oneFormat[1]));
                            if (oneFormat.Count == 3)
                                round = oneFormat[2];
                            break;
                        case "M":
                            data = Convert.ToDouble(data * Convert.ToSingle(oneFormat[1]));
                            if (oneFormat.Count == 3)
                                round = oneFormat[2];
                            break;
                        case "D":
                            float d = Convert.ToSingle(oneFormat[1]);
                            if (d != 0)
                                data = Convert.ToDouble(data / d);
                            if (oneFormat.Count == 3)
                                round = oneFormat[2];
                            break;
                        case "N":
                            round = oneFormat[1];
                            break;
                        case "P":
                            if (oneFormat[1].Equals("$0") && data > 0)
                                prefix = "+";
                            else
                                prefix = oneFormat[1];
                            break;
                        case "Q":
                            postfix = oneFormat[1];
                            break;
                        case "S":
                            switch (oneFormat[1])
                            {
                                case "Date":
                                    if (oneFormat[1].Length == 8)
                                    {
                                        body = oneFormat[1].Substring(0, 4) + "-" + oneFormat[1].Substring(4, 2) + "-" +
                                               oneFormat[1].Substring(6, 2);
                                        isShowBody = true;
                                    }
                                    break;
                                case "Time":
                                    if (oneFormat[1].Length == 6)
                                    {
                                        body = oneFormat[1].Substring(0, 2) + ":" + oneFormat[1].Substring(2, 2) + ":" +
                                               oneFormat[1].Substring(4, 2);
                                        isShowBody = true;
                                    }
                                    break;
                                case "Volume":
                                    if (data >= 1000000 || data <= -1000000)//100亿
                                    {
                                        data /= 10000;
                                        postfix = "亿";
                                        round = "F0";
                                    }
                                    else if (data >= 10000 || data <= -10000)//99.99亿
                                    {
                                        data /= 10000;
                                        postfix = "亿";
                                        round = "F2";
                                    }
                                    else if (data >= 100 || data <= -100)//100~9999万
                                    {
                                        postfix = "万";
                                        round = "F0";
                                    }
                                    else if (data >= 1 || data <= -1) //99.99万
                                    {
                                        postfix = "万";
                                        round = "F2";
                                    }
                                    else
                                        round = "F0";
                                    break;
                            }
                            break;
                        case "Z":
                            bool isZero = false;
                            if (oneFormat[1].Equals("this"))
                            {
                                if (data == 0)
                                    isZero = true;
                            }
                            else
                            {
                                FieldIndex field;
                                if (Enum.TryParse(oneFormat[1], out field))
                                {
                                    if (field >= 0 && (int)field <= 299)
                                    {
                                        if (_dc.GetFieldDataInt32(code, field) == 0)
                                            isZero = true;
                                    }
                                    if ((int)field >= 300 && (int)field <= 799)
                                    {
                                        if (_dc.GetFieldDataSingle(code, field) == 0)
                                            isZero = true;
                                    }
                                    if ((int)field >= 800 && (int)field <= 999)
                                    {
                                        if (_dc.GetFieldDataDouble(code, field) == 0)
                                            isZero = true;
                                    }
                                    if ((int)field >= 1000 && (int)field <= 1199)
                                    {
                                        if (_dc.GetFieldDataInt64(code, field) == 0)
                                            isZero = true;
                                    }
                                }
                            }
                            if (isZero)
                            {
                                if (oneFormat[2].Equals("H"))
                                {
                                    body = "-";
                                    isShowBody = true;
                                }
                                else
                                {
                                    body = string.Empty;
                                    isShowBody = true;
                                }
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    LogUtilities.LogMessage("oneFormat error :" + e.Message);
                    continue;
                }
            }

            if (!isShowBody)
                body = data.ToString(round);
            return prefix + body + postfix;
        }

        private static string SetFieldFormat(string data, FieldInfo fieldInfo, int code)
        {
            string prefix = string.Empty;
            string postfix = string.Empty;
            string body = string.Empty;

            foreach (List<string> oneFormat in fieldInfo.FormatSetting)
            {
                try
                {
                    switch (oneFormat[0])
                    {
                        case "P":
                            prefix = oneFormat[1];
                            break;
                        case "Q":
                            postfix = oneFormat[1];
                            break;
                        case "S":
                            switch (oneFormat[1])
                            {
                                case "Normal":
                                    body = data;
                                break;
                            }
                            break;
                        case "Z":
                            bool isZero = false;
                            if (!oneFormat[1].Equals("this"))
                            {
                                FieldIndex field;
                                if (Enum.TryParse(oneFormat[1], out field))
                                {
                                    if (field >= 0 && (int)field <= 299)
                                    {
                                        if (_dc.GetFieldDataInt32(code, field) == 0)
                                            isZero = true;
                                    }
                                    if ((int)field >= 300 && (int)field <= 799)
                                    {
                                        if (_dc.GetFieldDataSingle(code, field) == 0)
                                            isZero = true;
                                    }
                                    if ((int)field >= 800 && (int)field <= 999)
                                    {
                                        if (_dc.GetFieldDataDouble(code, field) == 0)
                                            isZero = true;
                                    }
                                    if ((int)field >= 1000 && (int)field <= 1199)
                                    {
                                        if (_dc.GetFieldDataInt64(code, field) == 0)
                                            isZero = true;
                                    }
                                }
                            }
                            if (isZero)
                            {
                                if (oneFormat[2].Equals("H"))
                                {
                                    body = "-";
                                }
                                else
                                {
                                    body = string.Empty;
                                }
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    LogUtilities.LogMessage("oneFormat error :" + e.Message);
                    continue;
                }
            }
            return prefix + body + postfix;
        }

        #endregion

        #region 颜色
        private static SolidBrush GetFieldBrush(int data, FieldInfo fieldInfo, int code)
        {
            if (fieldInfo.ColorSetting.Count == 1)
            {
                switch (fieldInfo.ColorSetting[0])
                {
                    case "Normal":
                        return QuoteDrawService.BrushColorSame;
                    case "DownK":
                        return QuoteDrawService.BrushColorDownKline;
                    case "Up":
                        return QuoteDrawService.BrushColorUp;
                    case "Down":
                        return QuoteDrawService.BrushColorDown;
                }
            }
            else if (fieldInfo.ColorSetting.Count == 2 && fieldInfo.ColorSetting[0].Equals("$"))
            {
                switch (fieldInfo.ColorSetting[1])
                {
                    case "0":
                        if(data > 0 )
                            return QuoteDrawService.BrushColorUp;
                        if(data == 0)
                            return QuoteDrawService.BrushColorSame;
                        if(data < 0)
                            return QuoteDrawService.BrushColorDown;
                        break;
                    default:
                        FieldIndex field;
                        if (Enum.TryParse(fieldInfo.ColorSetting[1], out field))
                        {
                            if (field >= 0 && (int) field <= 299)
                            {
                                int compareData = _dc.GetFieldDataInt32(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                            if ((int) field >= 300 && (int) field <= 799)
                            {
                                float compareData = _dc.GetFieldDataSingle(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                            if ((int) field >= 800 && (int) field <= 999)
                            {
                                double compareData = _dc.GetFieldDataDouble(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                            if ((int) field >= 1000 && (int) field <= 1199)
                            {
                                long compareData = _dc.GetFieldDataInt64(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                        }
                        break;
                }
            }
            return QuoteDrawService.BrushColorSame;
        }

        private static SolidBrush GetFieldBrush(float data, FieldInfo fieldInfo, int code)
        {
            if (fieldInfo.ColorSetting.Count == 1)
            {
                switch (fieldInfo.ColorSetting[0])
                {
                    case "Normal":
                        return QuoteDrawService.BrushColorSame;
                    case "DownK":
                        return QuoteDrawService.BrushColorDownKline;
                    case "Up":
                        return QuoteDrawService.BrushColorUp;
                    case "Down":
                        return QuoteDrawService.BrushColorDown;
                }
            }
            else if (fieldInfo.ColorSetting.Count == 2 && fieldInfo.ColorSetting[0].Equals("$"))
            {
                switch (fieldInfo.ColorSetting[1])
                {
                    case "0":
                        if (data > 0)
                            return QuoteDrawService.BrushColorUp;
                        if (data == 0)
                            return QuoteDrawService.BrushColorSame;
                        if (data < 0)
                            return QuoteDrawService.BrushColorDown;
                        break;
                    default:
                        FieldIndex field;
                        if (Enum.TryParse(fieldInfo.ColorSetting[1], out field))
                        {
                            if (field >= 0 && (int)field <= 299)
                            {
                                int compareData = _dc.GetFieldDataInt32(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                            if ((int)field >= 300 && (int)field <= 799)
                            {
                                float compareData = _dc.GetFieldDataSingle(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                            if ((int)field >= 800 && (int)field <= 999)
                            {
                                double compareData = _dc.GetFieldDataDouble(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                            if ((int)field >= 1000 && (int)field <= 1199)
                            {
                                long compareData = _dc.GetFieldDataInt64(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                        }
                        break;
                }
            }
            return QuoteDrawService.BrushColorSame;
        }

        private static SolidBrush GetFieldBrush(long data, FieldInfo fieldInfo, int code)
        {
            if (fieldInfo.ColorSetting.Count == 1)
            {
                switch (fieldInfo.ColorSetting[0])
                {
                    case "Normal":
                        return QuoteDrawService.BrushColorSame;
                    case "DownK":
                        return QuoteDrawService.BrushColorDownKline;
                    case "Up":
                        return QuoteDrawService.BrushColorUp;
                    case "Down":
                        return QuoteDrawService.BrushColorDown;
                }
            }
            else if (fieldInfo.ColorSetting.Count == 2 && fieldInfo.ColorSetting[0].Equals("$"))
            {
                switch (fieldInfo.ColorSetting[1])
                {
                    case "0":
                        if (data > 0)
                            return QuoteDrawService.BrushColorUp;
                        if (data == 0)
                            return QuoteDrawService.BrushColorSame;
                        if (data < 0)
                            return QuoteDrawService.BrushColorDown;
                        break;
                    default:
                        FieldIndex field;
                        if (Enum.TryParse(fieldInfo.ColorSetting[1], out field))
                        {
                            if (field >= 0 && (int)field <= 299)
                            {
                                int compareData = _dc.GetFieldDataInt32(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                            if ((int)field >= 300 && (int)field <= 799)
                            {
                                float compareData = _dc.GetFieldDataSingle(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                            if ((int)field >= 800 && (int)field <= 999)
                            {
                                double compareData = _dc.GetFieldDataDouble(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                            if ((int)field >= 1000 && (int)field <= 1199)
                            {
                                long compareData = _dc.GetFieldDataInt64(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                        }
                        break;
                }
            }
            return QuoteDrawService.BrushColorSame;
        }

        private static SolidBrush GetFieldBrush(double data, FieldInfo fieldInfo, int code)
        {
            if (fieldInfo.ColorSetting.Count == 1)
            {
                switch (fieldInfo.ColorSetting[0])
                {
                    case "Normal":
                        return QuoteDrawService.BrushColorSame;
                    case "DownK":
                        return QuoteDrawService.BrushColorDownKline;
                    case "Up":
                        return QuoteDrawService.BrushColorUp;
                    case "Down":
                        return QuoteDrawService.BrushColorDown;
                }
            }
            else if (fieldInfo.ColorSetting.Count == 2 && fieldInfo.ColorSetting[0].Equals("$"))
            {
                switch (fieldInfo.ColorSetting[1])
                {
                    case "0":
                        if (data > 0)
                            return QuoteDrawService.BrushColorUp;
                        if (data == 0)
                            return QuoteDrawService.BrushColorSame;
                        if (data < 0)
                            return QuoteDrawService.BrushColorDown;
                        break;
                    default:
                        FieldIndex field;
                        if (Enum.TryParse(fieldInfo.ColorSetting[1], out field))
                        {
                            if (field >= 0 && (int)field <= 299)
                            {
                                int compareData = _dc.GetFieldDataInt32(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                            if ((int)field >= 300 && (int)field <= 799)
                            {
                                float compareData = _dc.GetFieldDataSingle(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                            if ((int)field >= 800 && (int)field <= 999)
                            {
                                double compareData = _dc.GetFieldDataDouble(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                            if ((int)field >= 1000 && (int)field <= 1199)
                            {
                                long compareData = _dc.GetFieldDataInt64(code, field);
                                if (data > compareData)
                                    return QuoteDrawService.BrushColorUp;
                                if (data == compareData)
                                    return QuoteDrawService.BrushColorSame;
                                if (data < compareData)
                                    return QuoteDrawService.BrushColorDown;
                            }
                        }
                        break;
                }
            }
            return QuoteDrawService.BrushColorSame;
        }

        #endregion
        **/
    }
}
