using System;
using System.Collections.Generic;
using System.Text;

namespace OwLibSV
{
    /// <summary>
    /// ��Ʊ��Ϣ
    /// </summary>
    public class Security
    {
        #region Lord 2016/4/20
        /// <summary>
        /// �������̾���
        /// </summary>
        public Security()
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
        /// ��ǰ�۸�
        /// </summary>
        public double m_close;

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
        /// ��Ʊ����
        /// </summary>
        public String m_securityCode = "";

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
            m_close = data.m_close;
            m_date = data.m_date;
            m_high = data.m_high;
            m_innerVol = data.m_innerVol;
            m_lastClose = data.m_lastClose;
            m_low = data.m_low;
            m_open = data.m_open;
            m_openInterest = data.m_openInterest;
            m_outerVol = data.m_outerVol;
            m_securityCode = data.m_securityCode;
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
            && m_securityCode == data.m_securityCode
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
}
