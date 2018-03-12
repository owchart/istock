using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    /// <summary>
    /// ��Ʊ��Ϣ
    /// </summary>
    public class GSecurity
    {
        #region Lord 2016/4/20
        /// <summary>
        /// �������̾���
        /// </summary>
        public GSecurity()
        {
        }

        /// <summary>
        /// ��Ʊ����
        /// </summary>
        public String m_code = "";

        /// <summary>
        /// ��Ʊ����
        /// </summary>
        public String m_name = "";

        /// <summary>
        /// ƴ��
        /// </summary>
        public String m_pingyin = "";

        /// <summary>
        /// ״̬
        /// </summary>
        public int m_status;

        /// <summary>
        /// �г�����
        /// </summary>
        public int m_type;
        #endregion
    }

    /// <summary>
    /// ֤ȯ��ʷ����
    /// </summary>
    public class SecurityData
    {
        #region Lord 2016/4/23
        /// <summary>
        /// ƽ���۸�
        /// </summary>
        public double m_avgPrice;

        /// <summary>
        /// ���̼�
        /// </summary>
        public double m_close;

        /// <summary>
        /// ����
        /// </summary>
        public double m_date;

        /// <summary>
        /// ��߼�
        /// </summary>
        public double m_high;

        /// <summary>
        /// ��ͼ�
        /// </summary>
        public double m_low;

        /// <summary>
        /// ���̼�
        /// </summary>
        public double m_open;

        /// <summary>
        /// �ɽ���
        /// </summary>
        public double m_volume;

        /// <summary>
        /// �ɽ���
        /// </summary>
        public double m_amount;

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="data">����</param>
        public void Copy(SecurityData data)
        {
            m_close = data.m_close;
            m_date = data.m_date;
            m_high = data.m_high;
            m_low = data.m_low;
            m_open = data.m_open;
            m_volume = data.m_volume;
            m_amount = data.m_amount;
        }
        #endregion
    }

    /// <summary>
    /// ��Ʊʵʱ����
    /// </summary>
    public class SecurityLatestData
    {
        #region Lord 2016/3/5
        /// <summary>
        /// �ɽ���
        /// </summary>
        public double m_amount;

        /// <summary>
        /// ί������
        /// </summary>
        public double m_allBuyVol;

        /// <summary>
        /// ί������
        /// </summary>
        public double m_allSellVol;

        /// <summary>
        /// ��Ȩƽ��ί���۸�
        /// </summary>
        public double m_avgBuyPrice;

        /// <summary>
        /// ��Ȩƽ��ί���۸�
        /// </summary>
        public double m_avgSellPrice;

        /// <summary>
        /// ��һ��
        /// </summary>
        public int m_buyVolume1;

        /// <summary>
        /// �����
        /// </summary>
        public int m_buyVolume2;

        /// <summary>
        /// ������
        /// </summary>
        public int m_buyVolume3;

        /// <summary>
        /// ������
        /// </summary>
        public int m_buyVolume4;

        /// <summary>
        /// ������
        /// </summary>
        public int m_buyVolume5;

        /// <summary>
        /// ������
        /// </summary>
        public int m_buyVolume6;

        /// <summary>
        /// ������
        /// </summary>
        public int m_buyVolume7;

        /// <summary>
        /// ������
        /// </summary>
        public int m_buyVolume8;

        /// <summary>
        /// ������
        /// </summary>
        public int m_buyVolume9;

        /// <summary>
        /// ������
        /// </summary>
        public int m_buyVolume10;

        /// <summary>
        /// ��һ��
        /// </summary>
        public double m_buyPrice1;

        /// <summary>
        /// �����
        /// </summary>
        public double m_buyPrice2;

        /// <summary>
        /// ������
        /// </summary>
        public double m_buyPrice3;

        /// <summary>
        /// ���ļ�
        /// </summary>
        public double m_buyPrice4;

        /// <summary>
        /// �����
        /// </summary>
        public double m_buyPrice5;

        /// <summary>
        /// ��һ��
        /// </summary>
        public double m_buyPrice6;

        /// <summary>
        /// �����
        /// </summary>
        public double m_buyPrice7;

        /// <summary>
        /// ������
        /// </summary>
        public double m_buyPrice8;

        /// <summary>
        /// ���ļ�
        /// </summary>
        public double m_buyPrice9;

        /// <summary>
        /// �����
        /// </summary>
        public double m_buyPrice10;

        /// <summary>
        /// ��ǰ�۸�
        /// </summary>
        public double m_close;

        /// <summary>
        /// ��Ʊ����
        /// </summary>
        public String m_code = "";

        /// <summary>
        /// ���ڼ�ʱ��
        /// </summary>
        public double m_date;

        /// <summary>
        /// ��߼�
        /// </summary>
        public double m_high;

        /// <summary>
        /// ���̳ɽ���
        /// </summary>
        public int m_innerVol;

        /// <summary>
        /// �������̼�
        /// </summary>
        public double m_lastClose;

        /// <summary>
        /// ��ͼ�
        /// </summary>
        public double m_low;

        /// <summary>
        /// ���̼�
        /// </summary>
        public double m_open;

        /// <summary>
        /// �ڻ��ֲ���
        /// </summary>
        public double m_openInterest;

        /// <summary>
        /// ���̳ɽ���
        /// </summary>
        public int m_outerVol;

        /// <summary>
        /// ��һ��
        /// </summary>
        public int m_sellVolume1;

        /// <summary>
        /// ������
        /// </summary>
        public int m_sellVolume2;

        /// <summary>
        /// ������
        /// </summary>
        public int m_sellVolume3;

        /// <summary>
        /// ������
        /// </summary>
        public int m_sellVolume4;

        /// <summary>
        /// ������
        /// </summary>
        public int m_sellVolume5;

        /// <summary>
        /// ��һ��
        /// </summary>
        public int m_sellVolume6;

        /// <summary>
        /// ������
        /// </summary>
        public int m_sellVolume7;

        /// <summary>
        /// ������
        /// </summary>
        public int m_sellVolume8;

        /// <summary>
        /// ������
        /// </summary>
        public int m_sellVolume9;

        /// <summary>
        /// ������
        /// </summary>
        public int m_sellVolume10;

        /// <summary>
        /// ��һ��
        /// </summary>
        public double m_sellPrice1;

        /// <summary>
        /// ������
        /// </summary>
        public double m_sellPrice2;

        /// <summary>
        /// ������
        /// </summary>
        public double m_sellPrice3;

        /// <summary>
        /// ���ļ�
        /// </summary>
        public double m_sellPrice4;

        /// <summary>
        /// �����
        /// </summary>
        public double m_sellPrice5;

        /// <summary>
        /// ��һ��
        /// </summary>
        public double m_sellPrice6;

        /// <summary>
        /// ������
        /// </summary>
        public double m_sellPrice7;

        /// <summary>
        /// ������
        /// </summary>
        public double m_sellPrice8;

        /// <summary>
        /// ���ļ�
        /// </summary>
        public double m_sellPrice9;

        /// <summary>
        /// �����
        /// </summary>
        public double m_sellPrice10;

        /// <summary>
        /// �ڻ������
        /// </summary>
        public double m_settlePrice;

        /// <summary>
        /// ������
        /// </summary>
        public double m_turnoverRate;

        /// <summary>
        /// �ɽ���
        /// </summary>
        public double m_volume;

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="data">����</param>
        public void Copy(SecurityLatestData data)
        {
            if (data == null) return;
            m_amount = data.m_amount;
            m_allBuyVol = data.m_allBuyVol;
            m_allSellVol = data.m_allSellVol;
            m_avgBuyPrice = data.m_avgBuyPrice;
            m_avgSellPrice = data.m_avgSellPrice;
            m_buyVolume1 = data.m_buyVolume1;
            m_buyVolume2 = data.m_buyVolume2;
            m_buyVolume3 = data.m_buyVolume3;
            m_buyVolume4 = data.m_buyVolume4;
            m_buyVolume5 = data.m_buyVolume5;
            m_buyPrice1 = data.m_buyPrice1;
            m_buyPrice2 = data.m_buyPrice2;
            m_buyPrice3 = data.m_buyPrice3;
            m_buyPrice4 = data.m_buyPrice4;
            m_buyPrice5 = data.m_buyPrice5;
            m_buyVolume6 = data.m_buyVolume6;
            m_buyVolume7 = data.m_buyVolume7;
            m_buyVolume8 = data.m_buyVolume8;
            m_buyVolume9 = data.m_buyVolume9;
            m_buyVolume10 = data.m_buyVolume10;
            m_buyPrice6 = data.m_buyPrice6;
            m_buyPrice7 = data.m_buyPrice7;
            m_buyPrice8 = data.m_buyPrice8;
            m_buyPrice9 = data.m_buyPrice9;
            m_buyPrice10 = data.m_buyPrice10;
            m_close = data.m_close;
            m_date = data.m_date;
            m_high = data.m_high;
            m_innerVol = data.m_innerVol;
            m_lastClose = data.m_lastClose;
            m_low = data.m_low;
            m_open = data.m_open;
            m_openInterest = data.m_openInterest;
            m_outerVol = data.m_outerVol;
            m_code = data.m_code;
            m_sellVolume1 = data.m_sellVolume1;
            m_sellVolume2 = data.m_sellVolume2;
            m_sellVolume3 = data.m_sellVolume3;
            m_sellVolume4 = data.m_sellVolume4;
            m_sellVolume5 = data.m_sellVolume5;
            m_sellPrice1 = data.m_sellPrice1;
            m_sellPrice2 = data.m_sellPrice2;
            m_sellPrice3 = data.m_sellPrice3;
            m_sellPrice4 = data.m_sellPrice4;
            m_sellPrice5 = data.m_sellPrice5;
            m_settlePrice = data.m_settlePrice;
            m_sellVolume6 = data.m_sellVolume6;
            m_sellVolume7 = data.m_sellVolume7;
            m_sellVolume8 = data.m_sellVolume8;
            m_sellVolume9 = data.m_sellVolume9;
            m_sellVolume10 = data.m_sellVolume10;
            m_sellPrice6 = data.m_sellPrice6;
            m_sellPrice7 = data.m_sellPrice7;
            m_sellPrice8 = data.m_sellPrice8;
            m_sellPrice9 = data.m_sellPrice9;
            m_sellPrice10 = data.m_sellPrice10;
            m_settlePrice = data.m_settlePrice;
            m_turnoverRate = data.m_turnoverRate;
            m_volume = data.m_volume;
        }

        /// <summary>
        /// �Ƚ��Ƿ���ͬ
        /// </summary>
        /// <param name="data">����</param>
        /// <returns>�Ƿ���ͬ</returns>
        public bool Equal(SecurityLatestData data)
        {
            if (data == null) return false;
            if (m_amount == data.m_amount
            && m_buyVolume1 == data.m_buyVolume1
            && m_buyVolume2 == data.m_buyVolume2
            && m_buyVolume3 == data.m_buyVolume3
            && m_buyVolume4 == data.m_buyVolume4
            && m_buyVolume5 == data.m_buyVolume5
            && m_buyPrice1 == data.m_buyPrice1
            && m_buyPrice2 == data.m_buyPrice2
            && m_buyPrice3 == data.m_buyPrice3
            && m_buyPrice4 == data.m_buyPrice4
            && m_buyPrice5 == data.m_buyPrice5
            && m_close == data.m_close
            && m_date == data.m_date
            && m_high == data.m_high
            && m_innerVol == data.m_innerVol
            && m_lastClose == data.m_lastClose
            && m_low == data.m_low
            && m_open == data.m_open
            && m_openInterest == data.m_openInterest
            && m_outerVol == data.m_outerVol
            && m_code == data.m_code
            && m_sellVolume1 == data.m_sellVolume1
            && m_sellVolume2 == data.m_sellVolume2
            && m_sellVolume3 == data.m_sellVolume3
            && m_sellVolume4 == data.m_sellVolume4
            && m_sellVolume5 == data.m_sellVolume5
            && m_sellPrice1 == data.m_sellPrice1
            && m_sellPrice2 == data.m_sellPrice2
            && m_sellPrice3 == data.m_sellPrice3
            && m_sellPrice4 == data.m_sellPrice4
            && m_sellPrice5 == data.m_sellPrice5
            && m_settlePrice == data.m_settlePrice
            && m_turnoverRate == data.m_turnoverRate
            && m_volume == data.m_volume)
            {
                return true;
            }
            return false;
        }
        #endregion
    }

    /// <summary>
    /// ��ʷ������Ϣ
    /// </summary>
    public class HistoryDataInfo
    {
        #region Lord 2016/3/27
        /// <summary>
        /// ����
        /// </summary>
        public int m_cycle;

        /// <summary>
        /// ��������
        /// </summary>
        public double m_endDate;

        /// <summary>
        /// �Ƿ���Ҫ��������
        /// </summary>
        public bool m_pushData;

        /// <summary>
        /// ��Ʊ����
        /// </summary>
        public String m_code;

        /// <summary>
        /// ��������
        /// </summary>
        public int m_size;

        /// <summary>
        /// ��ʼ����
        /// </summary>
        public double m_startDate;

        /// <summary>
        /// ��Ȩģʽ
        /// </summary>
        public int m_subscription;

        /// <summary>
        /// ����
        /// </summary>
        public int m_type;
        #endregion
    }

    /// <summary>
    /// ����������Ϣ
    /// </summary>
    public class LatestDataInfo
    {
        #region Lord 2016/5/18
        /// <summary>
        /// ����
        /// </summary>
        public String m_codes = "";

        /// <summary>
        /// ��ʽ
        /// </summary>
        public int m_formatType;

        /// <summary>
        /// �Ƿ����LV2
        /// </summary>
        public int m_lv2;

        /// <summary>
        /// ��������
        /// </summary>
        public int m_size;
        #endregion
    }

    /// <summary>
    /// �����ֶ�
    /// </summary>
    public class KeyFields
    {
        #region Lord 2016/10/03
        /// <summary>
        /// ���̼�
        /// </summary>
        public const String CLOSE = "CLOSE";
        /// <summary>
        /// ��߼�
        /// </summary>
        public const String HIGH = "HIGH";
        /// <summary>
        /// ��ͼ�
        /// </summary>
        public const String LOW = "LOW";
        /// <summary>
        /// ���̼�
        /// </summary>
        public const String OPEN = "OPEN";
        /// <summary>
        /// �ɽ���
        /// </summary>
        public const String VOL = "VOL";
        /// <summary>
        /// �ɽ���
        /// </summary>
        public const String AMOUNT = "AMOUNT";

        /// <summary>
        /// ƽ���۸�
        /// </summary>
        public const String AVGPRICE = "AVGPRICE";

        /// <summary>
        /// ���̼��ֶ�
        /// </summary>
        public const int CLOSE_INDEX = 0;
        /// <summary>
        /// ��߼��ֶ�
        /// </summary>
        public const int HIGH_INDEX = 1;
        /// <summary>
        /// ��ͼ��ֶ�
        /// </summary>
        public const int LOW_INDEX = 2;
        /// <summary>
        /// ���̼��ֶ�
        /// </summary>
        public const int OPEN_INDEX = 3;
        /// <summary>
        /// �ɽ����ֶ�
        /// </summary>
        public const int VOL_INDEX = 4;
        /// <summary>
        /// �ɽ����ֶ�
        /// </summary>
        public const int AMOUNT_INDEX = 5;

        /// <summary>
        /// ƽ���۸��ֶ�
        /// </summary>
        public const int AVGPRICE_INDEX = 6;
        #endregion
    }

    /// <summary>
    /// ָ������
    /// </summary>
    public class IndicatorData
    {
        /// <summary>
        /// ����
        /// </summary>
        public String m_parameters;

        /// <summary>
        /// �ű�
        /// </summary>
        public String m_script;

    }

    /// <summary>
    /// ָ�����
    /// </summary>
    public class Indicator
    {
        #region Lord 2016/3/13
        /// <summary>
        /// ���
        /// </summary>
        public String m_category = "";

        /// <summary>
        /// Ԥ����ʾ����
        /// </summary>
        public String m_coordinate = "";

        /// <summary>
        /// ����
        /// </summary>
        public String m_description = "";

        /// <summary>
        /// ��ʾС����λ��
        /// </summary>
        public int m_digit;

        /// <summary>
        /// ָ��ID
        /// </summary>
        public String m_indicatorID = "";

        /// <summary>
        /// ����
        /// </summary>
        public String m_name = "";

        /// <summary>
        /// �б�˳��
        /// </summary>
        public int m_orderNum;

        /// <summary>
        /// ���߷���
        /// </summary>
        public int m_paintType;

        /// <summary>
        /// ����
        /// </summary>
        public String m_parameters = "";

        /// <summary>
        /// ����
        /// </summary>
        public String m_password = "";

        /// <summary>
        /// ����Y������
        /// </summary>
        public String m_specialCoordinate = "";

        /// <summary>
        /// �ı�
        /// </summary>
        public String m_text = "";

        /// <summary>
        /// ����
        /// </summary>
        public int m_type;

        /// <summary>
        /// �Ƿ�ʹ������
        /// </summary>
        public int m_usePassword;

        /// <summary>
        /// �û�ID
        /// </summary>
        public int m_userID;

        /// <summary>
        /// �汾
        /// </summary>
        public int m_version;
        #endregion
    }

    /// <summary>
    /// ������Ϣ
    /// </summary>
    public class OrderInfo
    {
        public String m_code;
        public float m_price;
        public int m_qty;
    }

    /// <summary>
    /// ���ײ�������
    /// </summary>
    public class SecurityStrategySetting
    {
        /// <summary>
        /// ���õĹ�Ʊ����
        /// </summary>
        public String m_securityCode = "";
        /// <summary>
        /// ���õĹ�Ʊ����
        /// </summary>
        public String m_securityName = "";
        /// <summary>
        /// ��������
        /// 0�����佻��
        /// </summary>
        public int m_strategyType = 0;
        /// <summary>
        /// ���ײ��Ե�����
        /// </summary>
        public String m_strategySettingInfo = "";
    }

    /// <summary>
    /// ��Ʊ���佻������
    /// </summary>
    public class SecurityRangeTradeCondition
    {
        /// <summary>
        /// ��������
        /// </summary>
        public float m_bottomRangePrice = 0;
        /// <summary>
        /// ÿ�ι�������
        /// </summary>
        public int m_buyCount = 0;
        /// <summary>
        /// ί�����벻�ɽ����೤ʱ�����,��λ��,0Ϊ������
        /// </summary>
        public int m_cancelIntervalBuyNoDeal = 0;
        /// <summary>
        /// ί���������ɽ����೤ʱ�����,��λ��,0Ϊ������
        /// </summary>
        public int m_cancelIntervalSellNoDeal = 0;
        /// <summary>
        /// ��ּ۸�
        /// </summary>
        public float m_cleanPrice = 0;
        /// <summary>
        /// ��Чʱ��εĿ�ʼʱ��
        /// </summary>
        public String m_fromEffectiveTransactionTime = "";
        /// <summary>
        /// ��ʼ��������
        /// </summary>
        public int m_initBuildCount = 0;
        /// <summary>
        /// �Ƿ��Ѿ���ʼ����
        /// </summary>
        public bool m_initBuildFlag = false;
        /// <summary>
        /// ��ʼ���ּ۸�
        /// </summary>
        public float m_initBuildPrice = 0;
        /// <summary>
        /// ί�к󣬶೤ʱ����ɽ�״̬,��λ��,0Ϊ����ѯ
        /// </summary>
        public int m_intervalChceckTradeStatus = 0;
        /// <summary>
        /// �Ƿ�����������
        /// </summary>
        public bool m_isCrossBuy = false;
        /// <summary>
        /// �Ƿ�����������
        /// </summary>
        public bool m_isCrossSell = false;
        /// <summary>
        /// �Ƿ�������Чʱ���
        /// </summary>
        public bool m_isSetEffectiveTransactionTime = false;
        /// <summary>
        /// �����ϴγɽ��۵ı������߼۸�
        /// </summary>
        public float m_lowLastDealBuy = 0;
        /// <summary>
        /// �ϴγɽ�������
        /// </summary>
        public int m_lastDealCount = 0;
        /// <summary>
        /// �ϴγɽ��ļ۸�
        /// </summary>
        public float m_lastDealPrice = 0;
        /// <summary>
        /// ��Чʱ��εĽ���ʱ��
        /// </summary>
        public String m_toEffectiveTransactionTime = "";
        /// <summary>
        /// ���õĹ�Ʊ����
        /// </summary>
        public String m_securityCode = "";
        /// <summary>
        /// ���õĹ�Ʊ����
        /// </summary>
        public String m_securityName = "";
        /// <summary>
        /// �Ƿ񰴼۸�������Ĭ�ϰ��հٷֱ�
        /// </summary>
        public bool m_isBasePrice = false;
        /// <summary>
        /// �ǵ������׼
        /// </summary>
        public String m_priceDealBaseLine = "";
        /// <summary>
        /// ÿ������������
        /// </summary>
        public int m_sellCount = 0;
        /// <summary>
        /// �۸�����
        /// </summary>
        public float m_topRangePrice = 0;
        /// <summary>
        /// �ɽ������Ƕ�������
        /// </summary>
        public float m_overLastDealSell = 0;
    }

    /// <summary>
    /// ��Ʊ�ɽ���Ϣ
    /// </summary>
    public class SecurityTrade
    {
        /// <summary>
        /// ��������
        /// </summary>
        public double m_cancelVolume = 0;
        /// <summary>
        /// ����
        /// </summary>
        public String m_operate;
        /// <summary>
        /// ��ͬ���
        /// </summary>
        public String m_orderSysID;
        /// <summary>
        /// �ɽ����
        /// </summary>
        public String m_orderTradeID;
        /// <summary>
        /// ��������
        /// </summary>
        public String m_orderType;
        /// <summary>
        /// ��Ʊ���
        /// </summary>
        public double m_stockBalance = 0;
        /// <summary>
        /// ֤ȯ����
        /// </summary>
        public String m_stockCode;
        /// <summary>
        /// ֤ȯ����
        /// </summary>
        public String m_stockName;
        /// <summary>
        /// �ɽ����
        /// </summary>
        public double m_tradeAmount = 0;
        /// <summary>
        /// �ɽ�ʱ��
        /// </summary>
        public String m_tradeDate;
        /// <summary>
        /// �ɽ�����
        /// </summary>
        public double m_tradeVolume = 0;
        /// <summary>
        /// �ɽ�����
        /// </summary>
        public double m_tradeAvgPrice = 0;

        /// <summary>
        /// �ַ���ת���ɳɽ�����
        /// </summary>
        /// <param name="tradeResult"></param>
        /// <returns></returns>
        public static SecurityTrade ConvertToStockTrade(String tradeResult)
        {
            if (tradeResult == null || tradeResult.Length == 0)
            {
                return null;
            }
            String[] tradeFields = tradeResult.Split("	".ToCharArray());
            if (tradeFields == null || tradeFields.Length < 11)
            {
                return null;
            }
            int index = 0;
            SecurityTrade trade = new SecurityTrade();
            trade.m_tradeDate = tradeFields[index++];
            trade.m_stockCode = tradeFields[index++];
            trade.m_stockName = tradeFields[index++];
            trade.m_operate = tradeFields[index++];
            trade.m_tradeVolume = CStrA.ConvertStrToDouble(tradeFields[index++]);
            trade.m_tradeAvgPrice = CStrA.ConvertStrToDouble(tradeFields[index++]);
            trade.m_tradeAmount = CStrA.ConvertStrToDouble(tradeFields[index++]);
            trade.m_orderSysID = tradeFields[index++];
            trade.m_orderTradeID = tradeFields[index++];
            trade.m_cancelVolume = CStrA.ConvertStrToDouble(tradeFields[index++]);
            trade.m_stockBalance = CStrA.ConvertStrToDouble(tradeFields[index++]);
            return trade;
        }
    }

    /// <summary>
    /// �����˻���Ϣ
    /// </summary>
    public class SecurityTradingAccount
    {
        /// <summary>
        ///  ���ý��
        /// </summary>
        public double m_available = 0;
        /// <summary>
        ///  �ʽ����
        /// </summary>
        public double m_capitalBalance = 0;
        /// <summary>
        ///  ������
        /// </summary>
        public double m_frozenCash = 0;
        /// <summary>
        ///  ��Ʊ��ֵ
        /// </summary>
        public double m_stockValue = 0;
        /// <summary>
        ///  ���ʲ�
        /// </summary>
        public double m_totalCapital = 0;
        /// <summary>
        ///  ��ȡ���
        /// </summary>
        public double m_withdrawQuota = 0;

        /// <summary>
        /// �ַ���ת���ɳֲֶ���
        /// </summary>
        /// <param name="tradingAccountResult"></param>
        /// <returns></returns>
        public static SecurityTradingAccount ConvertToStockTradingAccount(String tradingAccountResult)
        {
            if (tradingAccountResult == null || tradingAccountResult.Length == 0)
            {
                return null;
            }
            String[] tradingAccountFields = tradingAccountResult.Split(Environment.NewLine.ToCharArray(),
                StringSplitOptions.RemoveEmptyEntries);
            if (tradingAccountFields == null || tradingAccountFields.Length < 6)
            {
                return null;
            }
            int index = 0;
            SecurityTradingAccount stockTradingAccount = new SecurityTradingAccount();
            stockTradingAccount.m_capitalBalance = CStrA.ConvertStrToDouble(tradingAccountFields[index++]);
            stockTradingAccount.m_frozenCash = CStrA.ConvertStrToDouble(tradingAccountFields[index++]);
            stockTradingAccount.m_available = CStrA.ConvertStrToDouble(tradingAccountFields[index++]);
            stockTradingAccount.m_withdrawQuota = CStrA.ConvertStrToDouble(tradingAccountFields[index++]);
            stockTradingAccount.m_stockValue = CStrA.ConvertStrToDouble(tradingAccountFields[index++]);
            stockTradingAccount.m_totalCapital = CStrA.ConvertStrToDouble(tradingAccountFields[index++]);
            return stockTradingAccount;
        }
    }

    /// <summary>
    /// ��Ʊ�ֲ���Ϣ
    /// </summary>
    public class SecurityPosition
    {
        /// <summary>
        ///  �������
        /// </summary>
        public double m_availableBalance = 0;

        /// <summary>
        /// ��������
        /// </summary>
        public double m_frozenVolume = 0;

        /// <summary>
        /// �ɶ��ʻ�
        /// </summary>
        public String m_investorAccount;

        /// <summary>
        /// �����г�
        /// </summary>
        public String m_marketName;

        /// <summary>
        /// �м�
        /// </summary>
        public double m_marketPrice = 0;

        /// <summary>
        /// ��ֵ
        /// </summary>
        public double m_marketValue = 0;

        /// <summary>
        /// �ɱ���
        /// </summary>
        public double m_positionCost;

        /// <summary>
        /// ӯ��
        /// </summary>
        public double m_positionProfit;

        /// <summary>
        /// ӯ����(%)
        /// </summary>
        public double m_positionProfitRatio;

        /// <summary>
        /// ����������
        /// </summary>
        public double m_redemptionVolume = 0;

        /// <summary>
        /// ��Ʊ���
        /// </summary>
        public double m_stockBalance = 0;

        /// <summary>
        /// ֤ȯ����
        /// </summary>
        public String m_stockCode;
        /// <summary>
        /// ֤ȯ����
        /// </summary>
        public String m_stockName;

        /// <summary>
        /// ʵ������
        /// </summary>
        public double m_volume = 0;

        /// <summary>
        /// �ַ���ת���ɳֲֶ���
        /// </summary>
        /// <param name="positionResult"></param>
        /// <returns></returns>
        public static SecurityPosition ConvertToStockPosition(String positionResult)
        {
            if (positionResult == null || positionResult.Length == 0)
            {
                return null;
            }
            String[] positionFields = positionResult.Split("	".ToCharArray());
            if (positionFields == null || positionFields.Length < 14)
            {
                return null;
            }
            int index = 0;
            SecurityPosition position = new SecurityPosition();
            position.m_stockCode = positionFields[index++];
            position.m_stockName = positionFields[index++];
            position.m_stockBalance = CStrA.ConvertStrToDouble(positionFields[index++]);
            position.m_availableBalance = CStrA.ConvertStrToDouble(positionFields[index++]);
            position.m_volume = CStrA.ConvertStrToDouble(positionFields[index++]);
            position.m_frozenVolume = CStrA.ConvertStrToDouble(positionFields[index++]);
            position.m_positionProfit = CStrA.ConvertStrToDouble(positionFields[index++]);
            position.m_positionCost = CStrA.ConvertStrToDouble(positionFields[index++]);
            position.m_positionProfitRatio = CStrA.ConvertStrToDouble(positionFields[index++]);
            position.m_marketPrice = CStrA.ConvertStrToDouble(positionFields[index++]);
            position.m_marketValue = CStrA.ConvertStrToDouble(positionFields[index++]);
            position.m_redemptionVolume = CStrA.ConvertStrToDouble(positionFields[index++]);
            position.m_marketName = positionFields[index++];
            position.m_investorAccount = positionFields[index++];
            return position;
        }
    }


    /// <summary>
    /// ��Ʊί����Ϣ
    /// </summary>
    public class SecurityCommission
    {
        /// <summary>
        /// ��������
        /// </summary>
        public double m_cancelVolume = 0;
        /// <summary>
        /// ����
        /// </summary>
        public String m_operate;
        /// <summary>
        /// ί��ʱ��
        /// </summary>
        public String m_orderDate;
        /// <summary>
        /// ί�м۸�
        /// </summary>
        public double m_orderPrice = 0;
        /// <summary>
        /// ��ͬ���
        /// </summary>
        public String m_orderSysID;
        /// <summary>
        /// ��������
        /// </summary>
        public String m_orderType;
        /// <summary>
        /// ί������
        /// </summary>
        public double m_orderVolume = 0;
        /// <summary>
        /// ��ע
        /// </summary>
        public String m_remark;
        /// <summary>
        /// ֤ȯ����
        /// </summary>
        public String m_stockCode;
        /// <summary>
        /// ֤ȯ����
        /// </summary>
        public String m_stockName;
        /// <summary>
        /// �ɽ�����
        /// </summary>
        public double m_tradeAvgPrice = 0;
        /// <summary>
        /// �ɽ�����
        /// </summary>
        public double m_tradeVolume = 0;

        /// <summary>
        /// �ַ���ת����ί�ж���
        /// </summary>
        /// <param name="commissionResult"></param>
        /// <returns></returns>
        public static SecurityCommission ConvertToSecurityCommission(String commissionResult)
        {
            if (commissionResult == null || commissionResult.Length == 0)
            {
                return null;
            }
            String[] orderFields = commissionResult.Split("	".ToCharArray());
            if (orderFields == null || orderFields.Length < 12)
            {
                return null;
            }
            int index = 0;
            SecurityCommission commission = new SecurityCommission();
            commission.m_orderDate = orderFields[index++];
            commission.m_stockCode = orderFields[index++];
            commission.m_stockName = orderFields[index++];
            commission.m_operate = orderFields[index++];
            commission.m_remark = orderFields[index++];
            commission.m_orderVolume = CStrA.ConvertStrToDouble(orderFields[index++]);
            commission.m_tradeVolume = CStrA.ConvertStrToDouble(orderFields[index++]);
            commission.m_cancelVolume = CStrA.ConvertStrToDouble(orderFields[index++]);
            commission.m_orderPrice = CStrA.ConvertStrToDouble(orderFields[index++]);
            commission.m_orderType = orderFields[index++];
            commission.m_tradeAvgPrice = CStrA.ConvertStrToDouble(orderFields[index++]);
            commission.m_orderSysID = orderFields[index++];
            return commission;
        }
    }
}
