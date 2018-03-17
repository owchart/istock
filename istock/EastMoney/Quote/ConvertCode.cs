using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    /// <summary>
    /// 
    /// </summary>
    public class ConvertCode
    {
        private const int FAKE_BK_MARKETID = 45;
        private const int ALLNUMBER = 0;
        private const int NUMANDSMALL = 1;
        private const int ALLBIGALPHA = 2;
        private const int NUMANDBIG = 3;

        public static String ConvertFuturesOrgCodeToCftShortCode(int code)
        {
            String fieldDataString = DetailData.FieldIndexDataString[code][FieldIndex.Code];
            String str2 = String.Empty;
            if (fieldDataString.StartsWith("IF"))
            {
                if (fieldDataString == "IF00C1")
                {
                    return "040120\0";
                }
                if (fieldDataString == "IF00C2")
                {
                    return "040121\0";
                }
                if (fieldDataString == "IF00C3")
                {
                    return "040122\0";
                }
                if (fieldDataString == "IF00C4")
                {
                    return "040123\0";
                }
                if (!String.IsNullOrEmpty(fieldDataString))
                {
                    str2 = "0411" + fieldDataString.Substring(fieldDataString.Length - 2, 2) + '\0';
                }
                return str2;
            }
            if (fieldDataString.StartsWith("TF"))
            {
                if (fieldDataString == "TF00C1")
                {
                    return "050120\0";
                }
                if (fieldDataString == "TF00C2")
                {
                    return "050121\0";
                }
                if (fieldDataString == "TF00C3")
                {
                    return "050122\0";
                }
                if (fieldDataString == "TF00C4")
                {
                    return "050123\0";
                }
                return ("0511" + fieldDataString.Substring(fieldDataString.Length - 2, 2) + '\0');
            }
            if (fieldDataString.StartsWith("IC"))
            {
                if (fieldDataString == "IC00C1")
                {
                    return "060120\0";
                }
                if (fieldDataString == "IC00C2")
                {
                    return "060121\0";
                }
                if (fieldDataString == "IC00C3")
                {
                    return "060122\0";
                }
                if (fieldDataString == "IC00C4")
                {
                    return "060123\0";
                }
                return ("0611" + fieldDataString.Substring(fieldDataString.Length - 2, 2) + '\0');
            }
            if (fieldDataString.StartsWith("IH"))
            {
                if (fieldDataString == "IH00C1")
                {
                    return "070120\0";
                }
                if (fieldDataString == "IH00C2")
                {
                    return "070121\0";
                }
                if (fieldDataString == "IH00C3")
                {
                    return "070122\0";
                }
                if (fieldDataString == "IH00C4")
                {
                    return "070123\0";
                }
                return ("0711" + fieldDataString.Substring(fieldDataString.Length - 2, 2) + '\0');
            }
            if (fieldDataString.StartsWith("IH"))
            {
                if (fieldDataString == "IH00C1")
                {
                    return "070120\0";
                }
                if (fieldDataString == "IH00C2")
                {
                    return "070121\0";
                }
                if (fieldDataString == "IH00C3")
                {
                    return "070122\0";
                }
                if (fieldDataString == "IH00C4")
                {
                    return "070123\0";
                }
                return ("0711" + fieldDataString.Substring(fieldDataString.Length - 2, 2) + '\0');
            }
            if (fieldDataString.StartsWith("TT"))
            {
                if (fieldDataString == "TT00C1")
                {
                    return "100120\0";
                }
                if (fieldDataString == "TT00C2")
                {
                    return "100121\0";
                }
                if (fieldDataString == "TT00C3")
                {
                    return "100122\0";
                }
                if (fieldDataString == "TT00C4")
                {
                    return "100123\0";
                }
                return ("1011" + fieldDataString.Substring(fieldDataString.Length - 2, 2) + '\0');
            }
            if (!fieldDataString.StartsWith("T"))
            {
                return str2;
            }
            if (fieldDataString == "T00C1")
            {
                return "110120\0";
            }
            if (fieldDataString == "T00C2")
            {
                return "110121\0";
            }
            if (fieldDataString == "T00C3")
            {
                return "110122\0";
            }
            if (fieldDataString == "T00C4")
            {
                return "110123\0";
            }
            return ("1111" + fieldDataString.Substring(fieldDataString.Length - 2, 2) + '\0');
        }

        public static int CovertFuturesOrgCodeToCftStockId(int code)
        {
            int num = 0;
            String str = ConvertFuturesOrgCodeToCftShortCode(code);
            if (!String.IsNullOrEmpty(str))
            {
                num = ConvertCodeToInt(Encoding.ASCII.GetBytes(str), 8);
            }
            return num;
        }

        public static String ConvertFuturesCftEmCodeToOrgEmCode(String emcode)
        {
            String str = emcode;
            if (str.StartsWith("04"))
            {
                if (str == "040120.CFE")
                {
                    return "IF00C1.CFE";
                }
                if (str == "040121.CFE")
                {
                    return "IF00C2.CFE";
                }
                if (str == "040122.CFE")
                {
                    return "IF00C3.CFE";
                }
                if (str == "040123.CFE")
                {
                    return "IF00C4.CFE";
                }
                String str2 = str.Substring(str.Length - 6, 2);
                String str3 = String.Empty;
                if (DateTime.Now.Month <= Convert.ToInt32(str2))
                {
                    str3 = "IF" + Convert.ToString((int)(DateTime.Now.Year % 100));
                }
                else
                {
                    str3 = "IF" + Convert.ToString((int)((DateTime.Now.Year % 100) + 1));
                }
                return (str3 + str2 + ".CFE");
            }
            if (str.StartsWith("05"))
            {
                switch (str)
                {
                    case "050120.CFE":
                        return "TF00C1.CFE";

                    case "050121.CFE":
                        return "TF00C2.CFE";

                    case "050122.CFE":
                        return "TF00C3.CFE";

                    case "050123.CFE":
                        return "TF00C4.CFE";
                }
                String str4 = str.Substring(str.Length - 6, 2);
                String str5 = String.Empty;
                if (DateTime.Now.Month <= Convert.ToInt32(str4))
                {
                    str5 = "TF" + Convert.ToString((int)(DateTime.Now.Year % 100));
                }
                else
                {
                    str5 = "TF" + Convert.ToString((int)((DateTime.Now.Year % 100) + 1));
                }
                return (str5 + str4 + ".CFE");
            }
            if (str.StartsWith("06"))
            {
                switch (str)
                {
                    case "060120.CFE":
                        return "IC00C1.CFE";

                    case "060121.CFE":
                        return "IC00C2.CFE";

                    case "060122.CFE":
                        return "IC00C3.CFE";

                    case "060123.CFE":
                        return "IC00C4.CFE";
                }
                String str6 = str.Substring(str.Length - 6, 2);
                String str7 = String.Empty;
                if (DateTime.Now.Month <= Convert.ToInt32(str6))
                {
                    str7 = "IC" + Convert.ToString((int)(DateTime.Now.Year % 100));
                }
                else
                {
                    str7 = "IC" + Convert.ToString((int)((DateTime.Now.Year % 100) + 1));
                }
                return (str7 + str6 + ".CFE");
            }
            if (str.StartsWith("07"))
            {
                switch (str)
                {
                    case "070120.CFE":
                        return "IH00C1.CFE";

                    case "070121.CFE":
                        return "IH00C2.CFE";

                    case "070122.CFE":
                        return "IH00C3.CFE";

                    case "070123.CFE":
                        return "IH00C4.CFE";
                }
                String str8 = str.Substring(str.Length - 6, 2);
                String str9 = String.Empty;
                if (DateTime.Now.Month <= Convert.ToInt32(str8))
                {
                    str9 = "IH" + Convert.ToString((int)(DateTime.Now.Year % 100));
                }
                else
                {
                    str9 = "IH" + Convert.ToString((int)((DateTime.Now.Year % 100) + 1));
                }
                return (str9 + str8 + ".CFE");
            }
            if (str.StartsWith("10"))
            {
                switch (str)
                {
                    case "100120.CFE":
                        return "TT00C1.CFE";

                    case "100121.CFE":
                        return "TT00C2.CFE";

                    case "100122.CFE":
                        return "TT00C3.CFE";

                    case "100123.CFE":
                        return "TT00C4.CFE";
                }
                String str10 = str.Substring(str.Length - 6, 2);
                String str11 = String.Empty;
                if (DateTime.Now.Month <= Convert.ToInt32(str10))
                {
                    str11 = "TT" + Convert.ToString((int)(DateTime.Now.Year % 100));
                }
                else
                {
                    str11 = "TT" + Convert.ToString((int)((DateTime.Now.Year % 100) + 1));
                }
                return (str11 + str10 + ".CFE");
            }
            if (!str.StartsWith("11"))
            {
                return str;
            }
            switch (str)
            {
                case "110120.CFE":
                    return "T00C1.CFE";

                case "110121.CFE":
                    return "T00C2.CFE";

                case "110122.CFE":
                    return "T00C3.CFE";

                case "110123.CFE":
                    return "T00C4.CFE";
            }
            String str12 = str.Substring(str.Length - 6, 2);
            String str13 = String.Empty;
            if (DateTime.Now.Month <= Convert.ToInt32(str12))
            {
                str13 = "T" + Convert.ToString((int)(DateTime.Now.Year % 100));
            }
            else
            {
                str13 = "T" + Convert.ToString((int)((DateTime.Now.Year % 100) + 1));
            }
            return (str13 + str12 + ".CFE");
        }

        /// <summary>
        /// code转唯一代码
        /// </summary>
        /// <param name="srcCode"></param>
        /// <param name="market"></param>
        /// <returns></returns>
        public static int ConvertCodeToInt(byte[] srcCode, byte market)
        {
            byte[] code;
            if (market == (byte) ReqMarketType.MT_Plate)
            {
                market = FAKE_BK_MARKETID;
                code = new byte[srcCode.Length - 2];
                for (int i = 0; i < code.Length; i++)
                    code[i] = srcCode[i + 2];
            }
            else
            {
                code = srcCode;
            }
            int len = Encoding.ASCII.GetString(code).TrimEnd('\0').Length;
            int j = 0;
            int nAlpha = 0;
            uint[] value = new uint[7];
            uint temp;

            if (len <= 0 || len > 6 || len > 63)
                return -1;

            int nType = GetCodeType(code, len, ref nAlpha);
            switch (nType)
            {
                case ALLBIGALPHA:
                    temp = (uint) 2 << 30;
                    for (j = 0; j < len; j++)
                    {
                        value[j] = (uint) code[j] - 'A' + 1;
                        temp = temp + (value[j] << (30 - (j + 1)*5));
                    }
                    for (; j < 6; j++)
                    {
                        temp = (uint) (temp + (31 << (30 - (j + 1)*5)));
                    }
                    return (int)(temp);


                case ALLNUMBER:
                    temp = 0;
                    for (j = 0; j < len; j++)
                    {
                        value[j] = (uint) code[j] - '0';
                        temp = temp + (value[j] << (30 - (j + 1)*4));
                    }
                    temp = temp + market;
                    return (int)(temp);


                case NUMANDBIG:

                    if (nAlpha > 3)
                    {
                        return -1;
                    }
                    temp = (uint) 3 << 30;
                    for (j = 0; j < nAlpha; j++)
                    {
                        value[j] = (uint) code[j] - 'A' + 1;
                        temp = temp + (value[j] << (30 - (j + 1)*5));
                    }
                    for (; j < 3; j++)
                    {
                        temp = temp + (uint) (31 << (30 - (j + 1)*5));
                    }
                    for (j = nAlpha; j < len; j++)
                    {
                        value[j] = (uint) code[j] - '0';
                        if (nAlpha == 3)
                        {
                            temp = temp + (value[j] << (15 - (j - 2)*4));
                        }
                        else
                        {
                            temp = temp + (value[j] << (15 - (j - 1)*4));
                        }
                    }
                    for (j = 3 + len - nAlpha; j < 6; j++)
                    {
                        temp = temp + (uint) (15 << (15 - (j - 2)*4));
                    }
                    temp = temp + (uint) nAlpha;
                    return (int)temp;

                case NUMANDSMALL:

                    if (nAlpha > 3)
                    {
                        return -1;
                    }

                    temp = 1 << 30;
                    for (j = 0; j < nAlpha; j++)
                    {
                        value[j] = (uint) code[j] - 'a' + 1;
                        temp = temp + (value[j] << (30 - (j + 1)*5));
                    }
                    for (; j < 3; j++)
                    {
                        temp = temp + (uint) (31 << (30 - (j + 1)*5));
                    }
                    for (j = nAlpha; j < len; j++)
                    {
                        value[j] = (uint) code[j] - '0';
                        if (nAlpha == 3)
                        {
                            temp = temp + (value[j] << (15 - (j - 2)*4));
                        }
                        else
                        {
                            temp = temp + (value[j] << (15 - (j - 1)*4));
                        }
                    }
                    for (j = 3 + len - nAlpha; j < 6; j++)
                    {
                        temp = temp + (uint) (15 << (15 - (j - 2)*4));
                    }
                    temp = temp + (uint) nAlpha;
                    return (int)(temp);
                default:
                    return -1;
            }
        }

        private static int GetCodeType(byte[] code, int num, ref int nAlpha)
        {
            int i;
            bool bHasBigAlpha = false;
            bool bHasSmallAlpha = false;
            bool bHasNum = false;

            nAlpha = 0;
            for (i = 0; i < num; i++)
            {
                if (code[i] - '0' >= 0 && code[i] - '9' <= 0)
                {
                    bHasNum = true;
                }
                else if (code[i] - 'A' >= 0 && code[i] - 'Z' <= 0)
                {
                    bHasBigAlpha = true;
                    nAlpha++;
                }
                else if (code[i] - 'a' >= 0 && code[i] - 'z' <= 0)
                {
                    bHasSmallAlpha = true;
                    nAlpha++;
                }
                else
                {
                    return -1;
                }
            }

            if (bHasNum)
            {
                if (bHasBigAlpha)
                {
                    return NUMANDBIG;
                }
                else if (bHasSmallAlpha)
                {
                    return NUMANDSMALL;
                }
                return ALLNUMBER;
            }
            else if (bHasBigAlpha)
            {
                return ALLBIGALPHA;
            }
            return -1;
        }

        /// <summary>
        /// 唯一代码转code
        /// </summary>
        /// <param name="stockId"></param>
        /// <returns></returns>
        public static String ConvertIntToCode(uint stockId)
        {
            byte[] shortCode = new byte[7];
            byte market = 0;

            uint nType = (stockId & 0xc0000000);
            nType = nType >> 30;
            uint temp = 0;
            int i = 0;
            int j = 0;

            switch (nType)
            {
                case ALLBIGALPHA:
                    while(temp != 31 && i < 6)
                    {
                        temp = (stockId << (2 + i*5));
				        temp &= 0xf8000000;
				        temp = temp >> 27;
				        if (temp == 31)
				        {
				            shortCode[i] = 0;
				        }
				        else
				        {
                            shortCode[i] = Convert.ToByte(temp + 'A' - 1);
				        }
                        i++;
                    }
                    break;
                case ALLNUMBER:
                    for (i = 0 ; i < 6; i++)
			        {
				        temp = (stockId << (2 + i*4));
				        temp &= 0xf8000000;
				        temp = temp >> 28;
				        shortCode[i] = Convert.ToByte(temp + '0');
			        }
			        temp = (stockId << (2 + i*4));
			        temp &= 0xfc000000;
			        temp = temp >> 26;
			        market = Convert.ToByte(temp);
			        if( market == (byte) ReqMarketType.MT_AH)
				        shortCode[5] = Convert.ToByte('\0');
			        else if( market == FAKE_BK_MARKETID)
			        {
				        shortCode[4] = Convert.ToByte('\0');
				        market = (byte) ReqMarketType.MT_Plate;
                        for(int k = 4; k >= 0; k--)
                            shortCode[k+2] = shortCode[k];
			            shortCode[0] = Convert.ToByte('B');
                        shortCode[1] = Convert.ToByte('K');
			        }
			        else
			        {
                        shortCode[6] = Convert.ToByte('\0');
			        }
                    break;
                case NUMANDBIG:
                    for (i = 0 ; i < 3 ; i++)
			        {
				        temp = (uint)(stockId << (2 + i*5));
				        temp &= 0xf8000000;
				        temp = temp >> 27;
				        if (temp == 31)
				        {
					        continue;
				        }
				        else
				        {
                            shortCode[j] = Convert.ToByte(temp + 'A' - 1);
					        j++;
				        }
			        }

			        for ( ; i < 6 ; i++)
			        {
                        temp = (stockId << (2 + 15 + (i - 3) * 4));
				        temp &= 0xfc000000;
				        temp = temp >> 28;
				        if (temp == 15)
				        {
                            shortCode[j] = 0;
					        continue;
				        }
				        else
				        {
                            shortCode[j] = Convert.ToByte(temp + '0');
					        j++;
				        }
			        }
                    break;
                case NUMANDSMALL:
                    for (i = 0 ; i < 3 ; i++)
			        {
				        temp = (stockId << (2 + i*5));
				        temp &= 0xf8000000;
				        temp = temp >> 27;
				        if (temp == 31)
				        {
					        continue;
				        }
				        else
				        {
                            shortCode[j] = Convert.ToByte(temp + 'a' - 1);
					        j++;
				        }
			        }
			
			        for ( ; i < 6 ; i++)
			        {
                        temp = (stockId << (2 + 15 + (i - 3) * 4));
				        temp &= 0xfc000000;
				        temp = temp >> 28;
				        if (temp == 15)
				        {
					        shortCode[j] = 0;
					        continue;
				        }
				        else
				        {
                            shortCode[j] = Convert.ToByte(temp + '0');
					        j++;
				        }
			        }
                    break;
                default:
                    break;
            }

            String code = Encoding.ASCII.GetString(shortCode);
            code = code.TrimEnd('\0');
            return DataPacket.GetEmCode((ReqMarketType)market, code);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ConvertCodeOrg
    {
        public static long CommonConvertUnicodeToLong(int unicode)
        {
            long num = 0L;
            String emcode = String.Empty;
            MarketType marketType = DllImportHelper.GetMarketType(unicode);
            emcode = DllImportHelper.GetFieldDataString(unicode, FieldIndex.EMCode);
            if (marketType < MarketType.NA)
            {
                return num;
            }
            switch (marketType)
            {
                case MarketType.SHALev1:
                case MarketType.SHALev2:
                case MarketType.SZALev1:
                case MarketType.SZALev2:
                case MarketType.SHBLev1:
                case MarketType.SHBLev2:
                case MarketType.SZBLev1:
                case MarketType.SZBLev2:
                case MarketType.SHINDEX:
                case MarketType.SZINDEX:
                case MarketType.CircuitBreakerIndex:
                case MarketType.IF:
                case MarketType.GoverFutures:
                case MarketType.SHConvertBondLev1:
                case MarketType.SHConvertBondLev2:
                case MarketType.SZConvertBondLev1:
                case MarketType.SZConvertBondLev2:
                case MarketType.SHNonConvertBondLev1:
                case MarketType.SHNonConvertBondLev2:
                case MarketType.SZNonConvertBondLev1:
                case MarketType.SZNonConvertBondLev2:
                case MarketType.SHFundLev1:
                case MarketType.SHFundLev2:
                case MarketType.SZFundLev1:
                case MarketType.SZFundLev2:
                case MarketType.SHRepurchaseLevel1:
                case MarketType.SHRepurchaseLevel2:
                case MarketType.SZRepurchaseLevel1:
                case MarketType.SZRepurchaseLevel2:
                case MarketType.TB_OLD:
                case MarketType.TB_NEW:
                    return ConvertCodeToLong(emcode);

                case MarketType.EMINDEX:
                    return unicode;

                case MarketType.OSFuturesLMEElec:
                case MarketType.OSFuturesLMEVenue:
                case MarketType.BCE:
                case MarketType.FME:
                case MarketType.IB:
                case MarketType.BC:
                case MarketType.MonetaryFund:
                case MarketType.NonMonetaryFund:
                case MarketType.InterBankRepurchase:
                    return unicode;
            }
            return unicode;
        }

        public static int CommonConvertLongToUnicode(long sid)
        {
            int num = 0;
            String str = String.Empty;
            if (sid > 0x2540be400L)
            {
                str = ConvertLongToCode(sid);
                if (!String.IsNullOrEmpty(str) && DetailData.EmCodeToUnicode.ContainsKey(str))
                {
                    num = DetailData.EmCodeToUnicode[str];
                }
                return num;
            }
            return Convert.ToInt32(sid);
        }

        /// <summary>
        /// code转到long
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static long ConvertCodeToLong(String code)
        {
            ReqMarketType reqMt;
            String shortCode;
            DataPacket.ParseCode(code, out reqMt, out shortCode);


            int codeLen = shortCode.Length;
            bool isVariable = codeLen > 8;
            char[] chars = null;

            if (!isVariable)
            {
                chars = new char[code.Length*2 + 2];

                if ((short) reqMt < 10)
                {
                    chars[0] = '1';
                    chars[1] = Convert.ToChar(((short) reqMt).ToString());

                }
                else
                {
                    ((short) reqMt).ToString().ToCharArray().CopyTo(chars, 0);
                }

                int i = 2;
                foreach (char c in shortCode)
                {
                    if (c >= 48 && c <= 57)
                    {
                        //48～57号为0～9十个阿拉伯数字,0补齐两位
                        chars[i] = '0';
                        chars[i + 1] = c;
                    }
                    else if (c >= 65 && c <= 90)
                    {
                        //65～90号为26个大写英文字母,10开始
                        // chars[i] = ('0'+ (10 + (c - 65)));
                        chars[i] = Convert.ToChar((((c - 65)/10) + 1).ToString());
                        chars[i + 1] = Convert.ToChar(((c - 65)%10).ToString());

                    }
                    else if (c >= 97 && c <= 122)
                    {
                        //97～122号为26个小写英文字母 ，40开始
                        chars[i] = Convert.ToChar((((c - 97)/10) + 4).ToString());
                        chars[i + 1] = Convert.ToChar(((c - 97)%10).ToString());
                    }

                    i = i + 2;

                }
                try
                {
                    long security = long.Parse(new String(chars));
                    return security;
                }
                catch (Exception e)
                {
                    LogUtilities.LogMessage(e.Message);
                }
            }
            else
            {
                shortCode = shortCode.ToLower();
                StringBuilder varStr = new StringBuilder();
                if ((short)reqMt < 10)
                {
                    varStr.Append('1').Append(Convert.ToChar(((short)reqMt).ToString()));
                }
                else
                {
                    varStr.Append(Convert.ToChar(((short)reqMt).ToString()));
                }
                varStr.Append(7);
                int i = 0;
                long varL = 0L;
                foreach (char c in shortCode)
                {
                    if (c >= 48 && c <= 57)
                    {
                        //48～57号为0～9十个阿拉伯数字
                        varL += (c - 48) * (long)Math.Pow(36, i);
                    }
                    else if (c >= 97 && c <= 122)
                    {
                        //97～122号为26个小写英文字母
                        varL += (c - 87) * (long)Math.Pow(36, i);
                    }
                    i++;
                }
                varStr.Append(varL);
                return long.Parse(varStr.ToString()); ;
            }
            return 0;
        }

        /// <summary>
        /// long转code
        /// </summary>
        /// <param name="securityId"></param>
        /// <returns></returns>
        public static String ConvertLongToCode(long securityId)
        {
            if (securityId == 0)
                return String.Empty;
            if(securityId > 10000000000)
            {
                String[] marketCode = new String[2];
                String sidStr = securityId + "";
                String marketStr = sidStr.Substring(0, 2);

                if (marketStr.StartsWith("1"))
                {
                    marketCode[0] = marketStr.Substring(1, 1);
                }
                else
                {
                    marketCode[0] = marketStr;
                }
                StringBuilder code = new StringBuilder();

                bool isVariable = "7".Equals(sidStr.Substring(2, 1));
                if (!isVariable)
                {
                    for (int i = 2; i < sidStr.Length; i += 2)
                    {
                        String sus = sidStr.Substring(i, 2);

                        Int32 tmp = Int32.Parse(sus);
                        if (tmp >= 0 && tmp <= 9)
                        {
                            //48～57号为0～9十个阿拉伯数字,0补齐两位
                            code.Append(tmp);
                        }
                        else if (tmp >= 10 && tmp <= 35)
                        {
                            //65～90号为26个大写英文字母,10开始
                            code.Append((char)(tmp + 55));
                        }
                        else if (tmp >= 40 && tmp <= 65)
                        {
                            //97～122号为26个小写英文字母 ，40开始
                            code.Append((char)(tmp + 57));
                        }
                    }

                    marketCode[1] = code.ToString();
                }
                else
                {
                    long tmp = long.Parse(sidStr.Substring(3));
                    while (tmp > 0)
                    {

                        int i = Convert.ToInt32(tmp % 36);
                        if (i >= 0 && i <= 9)
                        {
                            //48～57号为0～9十个阿拉伯数字,0补齐两位
                            code.Append(i);
                        }
                        else
                        {
                            code.Append((char)(i + 87));
                        }

                        tmp = tmp / 36;
                    }
                    marketCode[1] = code.ToString();
                }

                ReqMarketType mt = (ReqMarketType)Convert.ToInt32(marketCode[0]);
                return DataPacket.GetEmCode(mt, marketCode[1]);
            }
            else
            {
                String code = String.Empty;
                try
                {
                    if (DetailData.FieldIndexDataString.ContainsKey(Convert.ToInt32(securityId)))
                        DetailData.FieldIndexDataString[Convert.ToInt32(securityId)].TryGetValue(FieldIndex.EMCode, out code);
                }
                catch (Exception e)
                {
                    LogUtilities.LogMessage("ConvertCode error  " + e.Message);
                }
                
                return code;

            }

        }
    }
}
