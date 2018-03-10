namespace OwLib
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;

    public class CompressHelper
    {
        [DllImport("lcmex.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern IntPtr CreateInstance();
        [DllImport("lcmex.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern void DeleteInstance(IntPtr pIf);
        [DllImport("lcmex.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern void FreeInstance(IntPtr pIf);
        public static void GetMinFF(IntPtr ptr, byte[] zipData, int zipLen, out int stockId, out List<SRawDayFundFlow> datas)
        {
            datas = new List<SRawDayFundFlow>();
            int pDatas = 0;
            int intSize = 0;
            stockId = 0;
            UncompressSRawStockMinFF(ptr, zipData, zipLen, ref stockId, ref pDatas, ref intSize);
            int num3 = 0xc4 * intSize;
            byte[] buffer = new byte[num3];
            for (int i = 0; i < num3; i++)
            {
                try
                {
                    buffer[i] = Marshal.ReadByte((IntPtr) (pDatas + i));
                }
                catch (Exception exception)
                {
                    //LogUtilities.LogMessagePublishException(exception.StackTrace);
                }
            }
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    for (int j = 0; j < intSize; j++)
                    {
                        SRawDayFundFlow item = new SRawDayFundFlow();
                        item.m_dwTime = (uint) reader.ReadInt32();
                        item.m_pAmtOfBuy = new double[4];
                        for (int k = 0; k < 4; k++)
                        {
                            item.m_pAmtOfBuy[k] = reader.ReadDouble();
                        }
                        item.m_pAmtOfSell = new double[4];
                        for (int m = 0; m < 4; m++)
                        {
                            item.m_pAmtOfSell[m] = reader.ReadDouble();
                        }
                        item.m_pVolOfBuy = new long[4];
                        for (int n = 0; n < 4; n++)
                        {
                            item.m_pVolOfBuy[n] = reader.ReadInt64();
                        }
                        item.m_pVolOfSell = new long[4];
                        for (int num9 = 0; num9 < 4; num9++)
                        {
                            item.m_pVolOfSell[num9] = reader.ReadInt64();
                        }
                        item.m_pdwNumOfBuy = new int[4];
                        for (int num10 = 0; num10 < 4; num10++)
                        {
                            item.m_pdwNumOfBuy[num10] = reader.ReadInt32();
                        }
                        item.m_pdwNumOfSell = new int[4];
                        for (int num11 = 0; num11 < 4; num11++)
                        {
                            item.m_pdwNumOfSell[num11] = reader.ReadInt32();
                        }
                        datas.Add(item);
                    }
                }
            }
        }

        public static List<SRawOrderRec> GetOrderData(IntPtr ptr, byte[] zipData, int zipLen)
        {
            List<SRawOrderRec> list = new List<SRawOrderRec>();
            int data = 0;
            int intSize = 0;
            if (!UncompressSRawOrderRec(ptr, zipData, zipLen, ref data, ref intSize))
            {
                return list;
            }
            int num3 = Marshal.SizeOf(typeof(SRawOrderRec)) * intSize;
            byte[] buffer = new byte[num3];
            for (int i = 0; i < num3; i++)
            {
                buffer[i] = Marshal.ReadByte((IntPtr) (data + i));
            }
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    for (int j = 0; j < intSize; j++)
                    {
                        SRawOrderRec item = new SRawOrderRec();
                        item.m_dwTime = reader.ReadUInt32();
                        item.m_wTradeNo = reader.ReadUInt32();
                        item.m_fPrice = reader.ReadSingle();
                        item.m_dwVolume = reader.ReadInt32();
                        item.m_bytOrderKind = reader.ReadByte();
                        item.m_bytFunctionCode = reader.ReadByte();
                        list.Add(item);
                    }
                    return list;
                }
            }
        }

        public static List<SRawTransaction> GetTickData(IntPtr ptr, byte[] zipData, int zipLen)
        {
            List<SRawTransaction> list = new List<SRawTransaction>();
            int pData = 0;
            int pSize = 0;
            if (!UncompressTick(ptr, zipData, zipLen, ref pData, ref pSize))
            {
                return list;
            }
            int num3 = Marshal.SizeOf(typeof(SRawTransaction)) * pSize;
            byte[] buffer = new byte[num3];
            for (int i = 0; i < num3; i++)
            {
                buffer[i] = Marshal.ReadByte((IntPtr) (pData + i));
            }
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    for (int j = 0; j < pSize; j++)
                    {
                        SRawTransaction item = new SRawTransaction();
                        item.m_dwTime = reader.ReadUInt32();
                        item.m_wTradeNo = reader.ReadUInt32();
                        item.m_cTradeType = reader.ReadByte();
                        item.m_fPrice = reader.ReadSingle();
                        item.m_dwVolume = reader.ReadUInt32();
                        list.Add(item);
                    }
                    return list;
                }
            }
        }

        public static void GetTrendData(IntPtr ptr, byte[] zipData, int zipLen, out int stockId, out List<SRawNewRtMin> datas, out int date)
        {
            datas = new List<SRawNewRtMin>();
            date = 0;
            stockId = 0;
            int intSize = 0;
            int data = 0;
            if (UncompressSRawStockNRtMin(ptr, zipData, zipLen, ref stockId, ref data, ref intSize))
            {
                int num3 = Marshal.SizeOf(typeof(SRawNewRtMin)) * intSize;
                byte[] buffer = new byte[num3];
                for (int i = 0; i < num3; i++)
                {
                    buffer[i] = Marshal.ReadByte((IntPtr) (data + i));
                }
                using (MemoryStream stream = new MemoryStream(buffer))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        for (int j = 0; j < intSize; j++)
                        {
                            SRawNewRtMin item = new SRawNewRtMin();
                            int resultTime = 0;
                            DataPacket.TrendCaptialFlowTimeConverter(reader.ReadInt32(), out resultTime, out date);
                            item.m_dwTime = (uint) resultTime;
                            item.m_dblOpen = reader.ReadDouble();
                            item.m_dblHigh = reader.ReadDouble();
                            item.m_dblLow = reader.ReadDouble();
                            item.m_dblClose = reader.ReadDouble();
                            item.m_dblAve = reader.ReadDouble();
                            item.m_xVolume = reader.ReadInt64();
                            item.m_dblAmount = reader.ReadDouble();
                            item.m_dwTradeNum = reader.ReadUInt32();
                            item.m_xWaiPan = reader.ReadInt64();
                            item.m_xExt1 = reader.ReadInt64();
                            item.m_xExt2 = reader.ReadInt64();
                            item.m_dblExt1 = reader.ReadDouble();
                            item.m_dblExt2 = reader.ReadDouble();
                            datas.Add(item);
                        }
                    }
                }
            }
        }

        [DllImport("lcmex.dll", EntryPoint="UncompressSRawQuoteRecNew", CallingConvention=CallingConvention.Cdecl)]
        public static extern bool UncompressQuoteRecNew(IntPtr pIf, [MarshalAs(UnmanagedType.LPArray)] byte[] pBuf, int intLen, [MarshalAs(UnmanagedType.LPArray)] byte[] pNew);
        [DllImport("lcmex.dll", CallingConvention=CallingConvention.Cdecl)]
        public static extern bool UncompressSRawFlowSum(IntPtr pIf, [MarshalAs(UnmanagedType.LPArray)] byte[] pBuf, int intLen, [MarshalAs(UnmanagedType.LPArray)] byte[] pNew);
        [DllImport("lcmex.dll", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool UncompressSRawOrderRec(IntPtr ptr, [MarshalAs(UnmanagedType.LPArray)] byte[] pBuf, int intLen, ref int data, ref int intSize);
        [DllImport("lcmex.dll", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool UncompressSRawStockMinFF(IntPtr ptr, [MarshalAs(UnmanagedType.LPArray)] byte[] pBuf, int intLen, ref int m_dwStockID, ref int pDatas, ref int intSize);
        [DllImport("lcmex.dll", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool UncompressSRawStockNRtMin(IntPtr ptr, [MarshalAs(UnmanagedType.LPArray)] byte[] pBuf, int intLen, ref int stockId, ref int data, ref int intSize);
        [DllImport("lcmex.dll", EntryPoint="UncompressSRawTransaction", CallingConvention=CallingConvention.Cdecl)]
        private static extern bool UncompressTick(IntPtr pIf, [MarshalAs(UnmanagedType.LPArray)] byte[] pBuf, int intLen, ref int pData, ref int pSize);

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        public struct SRawDayFundFlow
        {
            public uint m_dwTime;
            public double[] m_pAmtOfBuy;
            public double[] m_pAmtOfSell;
            public long[] m_pVolOfBuy;
            public long[] m_pVolOfSell;
            public int[] m_pdwNumOfBuy;
            public int[] m_pdwNumOfSell;
        }

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        public struct SRawNewRtMin
        {
            public uint m_dwTime;
            public double m_dblOpen;
            public double m_dblHigh;
            public double m_dblLow;
            public double m_dblClose;
            public double m_dblAve;
            public long m_xVolume;
            public double m_dblAmount;
            public uint m_dwTradeNum;
            public long m_xWaiPan;
            public long m_xExt1;
            public long m_xExt2;
            public double m_dblExt1;
            public double m_dblExt2;
        }

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        public struct SRawOrderRec
        {
            public uint m_dwTime;
            public uint m_wTradeNo;
            public float m_fPrice;
            public int m_dwVolume;
            public byte m_bytOrderKind;
            public byte m_bytFunctionCode;
        }

        [StructLayout(LayoutKind.Sequential, Pack=1)]
        public struct SRawTransaction
        {
            public uint m_dwTime;
            public uint m_wTradeNo;
            public byte m_cTradeType;
            public float m_fPrice;
            public uint m_dwVolume;
        }
    }
}

