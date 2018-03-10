using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace OwLib
{
    public class DllImportHelper
    {
        [DllImport("ChoiceDataMgr.dll", EntryPoint = "GetValue", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetFieldDataBoolean(int code, short field, ref bool nValue, ref int nLen, IntPtr pGoods);
        public static double GetFieldDataDouble(int code, FieldIndex field)
        {
            try
            {
                return DetailData.FieldIndexDataDouble[code][field];
            }
            catch (Exception)
            {
                return 0.0;
            }
        }

        [DllImport("ChoiceDataMgr.dll", EntryPoint = "GetValue", CallingConvention = CallingConvention.Cdecl)]
        private static extern int GetFieldDataDouble(int code, short field, ref double nValue, ref int nLen, IntPtr pGoods);
        public static int GetFieldDataInt32(int code, FieldIndex field)
        {
            try
            {
                return DetailData.FieldIndexDataInt32[code][field];
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static long GetFieldDataInt64(int code, FieldIndex field)
        {
            try
            {
                return DetailData.FieldIndexDataInt64[code][field];
            }
            catch (Exception)
            {
                return 0L;
            }
        }

        public static string GetFieldDataString(int code, FieldIndex field)
        {
            try
            {
                return DetailData.FieldIndexDataString[code][field];
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static float GetFieldDataSingle(int code, FieldIndex field)
        {
            try
            {
                return DetailData.FieldIndexDataSingle[code][field];
            }
            catch (Exception)
            {
                return 0f;
            }
        }


        [DllImport("ChoiceDataMgr.dll", EntryPoint="CreateGoodsObject", CallingConvention=CallingConvention.Cdecl)]
        private static extern void CreateMarketFieldObject(int marketType, int code, byte[] bytesName, int lenName, byte[] bytesCode, int lenCode, byte[] bytesEmCode, int lenEmCode);
        [DllImport("ChoiceIndexMgr.dll", EntryPoint="FM_DeleteFormulaFromDB", CallingConvention=CallingConvention.Cdecl)]
        public static extern bool DeleteFormulaFromDB(int fid);
        public static bool EqualZero(double value)
        {
            return ((value > -4.94065645841247E-324) && (value < double.Epsilon));
        }

        public static bool EqualZero(float value)
        {
            return ((value > -1.401298E-45f) && (value < float.Epsilon));
        }

     
        private static SetFuncFlag GetFlag(FieldIndex field)
        {
            SetFuncFlag setString = SetFuncFlag.SetInt32;
            int num = (int)field;
            if (num < 300)
            {
                return SetFuncFlag.SetInt32;
            }
            if (num < 800)
            {
                return SetFuncFlag.SetFloat;
            }
            if (num < 0x3e8)
            {
                return SetFuncFlag.SetDouble;
            }
            if (num < 0x4b0)
            {
                return SetFuncFlag.SetInt64;
            }
            if (num < 0x2328)
            {
                setString = SetFuncFlag.SetString;
            }
            return setString;
        }

        public static void SetFieldData<T>(int code, FieldIndex field, T fieldValue)
        {
            try
            {
                int nLen = 0;
                switch (GetFlag(field))
                {
                    case SetFuncFlag.SetInt32:
                        {
                            int nValue = Convert.ToInt32(fieldValue);
                            DetailData.FieldIndexDataInt32[code][field] = nValue;
                            return;
                        }
                    case SetFuncFlag.SetFloat:
                        {
                            float num3 = Convert.ToSingle(fieldValue);
                            DetailData.FieldIndexDataSingle[code][field] = num3;
                            return;
                        }
                    case SetFuncFlag.SetDouble:
                        {
                            double num4 = Convert.ToDouble(fieldValue);
                            DetailData.FieldIndexDataDouble[code][field] = num4;
                            return;
                        }
                    case SetFuncFlag.SetInt64:
                        {
                            long num5 = Convert.ToInt64(fieldValue);
                            DetailData.FieldIndexDataInt64[code][field] = num5;
                            return;
                        }
                    case SetFuncFlag.SetString:
                        {
                            string s = Convert.ToString(fieldValue);
                            DetailData.FieldIndexDataString[code][field] = s;
                            return;
                        }
                }
            }
            catch (Exception exception)
            {
                LogUtilities.LogMessage(exception.Message);
            }
        }

        public static MarketType GetMarketType(int code)
        {
            return DataCenterCore.CreateInstance().GetMarketType(code);
        }

        private enum SetFuncFlag
        {
            SetInt32,
            SetFloat,
            SetDouble,
            SetInt64,
            SetString
        }
    }
}

