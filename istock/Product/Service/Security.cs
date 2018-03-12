using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    /// <summary>
    /// 股票信息
    /// </summary>
    public class GSecurity
    {
        #region Lord 2016/4/20
        /// <summary>
        /// 创建键盘精灵
        /// </summary>
        public GSecurity()
        {
        }

        /// <summary>
        /// 股票代码
        /// </summary>
        public String m_code = "";

        /// <summary>
        /// 股票名称
        /// </summary>
        public String m_name = "";

        /// <summary>
        /// 拼音
        /// </summary>
        public String m_pingyin = "";

        /// <summary>
        /// 状态
        /// </summary>
        public int m_status;

        /// <summary>
        /// 市场类型
        /// </summary>
        public int m_type;
        #endregion
    }

    /// <summary>
    /// 证券历史数据
    /// </summary>
    public class SecurityData
    {
        #region Lord 2016/4/23
        /// <summary>
        /// 平均价格
        /// </summary>
        public double m_avgPrice;

        /// <summary>
        /// 收盘价
        /// </summary>
        public double m_close;

        /// <summary>
        /// 日期
        /// </summary>
        public double m_date;

        /// <summary>
        /// 最高价
        /// </summary>
        public double m_high;

        /// <summary>
        /// 最低价
        /// </summary>
        public double m_low;

        /// <summary>
        /// 开盘价
        /// </summary>
        public double m_open;

        /// <summary>
        /// 成交量
        /// </summary>
        public double m_volume;

        /// <summary>
        /// 成交额
        /// </summary>
        public double m_amount;

        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="data">数据</param>
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
    /// 股票实时数据
    /// </summary>
    public class SecurityLatestData
    {
        #region Lord 2016/3/5
        /// <summary>
        /// 成交额
        /// </summary>
        public double m_amount;

        /// <summary>
        /// 委买总量
        /// </summary>
        public double m_allBuyVol;

        /// <summary>
        /// 委卖总量
        /// </summary>
        public double m_allSellVol;

        /// <summary>
        /// 加权平均委卖价格
        /// </summary>
        public double m_avgBuyPrice;

        /// <summary>
        /// 加权平均委卖价格
        /// </summary>
        public double m_avgSellPrice;

        /// <summary>
        /// 买一量
        /// </summary>
        public int m_buyVolume1;

        /// <summary>
        /// 买二量
        /// </summary>
        public int m_buyVolume2;

        /// <summary>
        /// 买三量
        /// </summary>
        public int m_buyVolume3;

        /// <summary>
        /// 买四量
        /// </summary>
        public int m_buyVolume4;

        /// <summary>
        /// 买五量
        /// </summary>
        public int m_buyVolume5;

        /// <summary>
        /// 买五量
        /// </summary>
        public int m_buyVolume6;

        /// <summary>
        /// 买五量
        /// </summary>
        public int m_buyVolume7;

        /// <summary>
        /// 买五量
        /// </summary>
        public int m_buyVolume8;

        /// <summary>
        /// 买五量
        /// </summary>
        public int m_buyVolume9;

        /// <summary>
        /// 买五量
        /// </summary>
        public int m_buyVolume10;

        /// <summary>
        /// 买一价
        /// </summary>
        public double m_buyPrice1;

        /// <summary>
        /// 买二价
        /// </summary>
        public double m_buyPrice2;

        /// <summary>
        /// 买三价
        /// </summary>
        public double m_buyPrice3;

        /// <summary>
        /// 买四价
        /// </summary>
        public double m_buyPrice4;

        /// <summary>
        /// 买五价
        /// </summary>
        public double m_buyPrice5;

        /// <summary>
        /// 买一价
        /// </summary>
        public double m_buyPrice6;

        /// <summary>
        /// 买二价
        /// </summary>
        public double m_buyPrice7;

        /// <summary>
        /// 买三价
        /// </summary>
        public double m_buyPrice8;

        /// <summary>
        /// 买四价
        /// </summary>
        public double m_buyPrice9;

        /// <summary>
        /// 买五价
        /// </summary>
        public double m_buyPrice10;

        /// <summary>
        /// 当前价格
        /// </summary>
        public double m_close;

        /// <summary>
        /// 股票代码
        /// </summary>
        public String m_code = "";

        /// <summary>
        /// 日期及时间
        /// </summary>
        public double m_date;

        /// <summary>
        /// 最高价
        /// </summary>
        public double m_high;

        /// <summary>
        /// 内盘成交量
        /// </summary>
        public int m_innerVol;

        /// <summary>
        /// 昨日收盘价
        /// </summary>
        public double m_lastClose;

        /// <summary>
        /// 最低价
        /// </summary>
        public double m_low;

        /// <summary>
        /// 开盘价
        /// </summary>
        public double m_open;

        /// <summary>
        /// 期货持仓量
        /// </summary>
        public double m_openInterest;

        /// <summary>
        /// 外盘成交量
        /// </summary>
        public int m_outerVol;

        /// <summary>
        /// 卖一量
        /// </summary>
        public int m_sellVolume1;

        /// <summary>
        /// 卖二量
        /// </summary>
        public int m_sellVolume2;

        /// <summary>
        /// 卖三量
        /// </summary>
        public int m_sellVolume3;

        /// <summary>
        /// 卖四量
        /// </summary>
        public int m_sellVolume4;

        /// <summary>
        /// 卖五量
        /// </summary>
        public int m_sellVolume5;

        /// <summary>
        /// 卖一量
        /// </summary>
        public int m_sellVolume6;

        /// <summary>
        /// 卖二量
        /// </summary>
        public int m_sellVolume7;

        /// <summary>
        /// 卖三量
        /// </summary>
        public int m_sellVolume8;

        /// <summary>
        /// 卖四量
        /// </summary>
        public int m_sellVolume9;

        /// <summary>
        /// 卖五量
        /// </summary>
        public int m_sellVolume10;

        /// <summary>
        /// 卖一价
        /// </summary>
        public double m_sellPrice1;

        /// <summary>
        /// 卖二价
        /// </summary>
        public double m_sellPrice2;

        /// <summary>
        /// 卖三价
        /// </summary>
        public double m_sellPrice3;

        /// <summary>
        /// 卖四价
        /// </summary>
        public double m_sellPrice4;

        /// <summary>
        /// 卖五价
        /// </summary>
        public double m_sellPrice5;

        /// <summary>
        /// 卖一价
        /// </summary>
        public double m_sellPrice6;

        /// <summary>
        /// 卖二价
        /// </summary>
        public double m_sellPrice7;

        /// <summary>
        /// 卖三价
        /// </summary>
        public double m_sellPrice8;

        /// <summary>
        /// 卖四价
        /// </summary>
        public double m_sellPrice9;

        /// <summary>
        /// 卖五价
        /// </summary>
        public double m_sellPrice10;

        /// <summary>
        /// 期货结算价
        /// </summary>
        public double m_settlePrice;

        /// <summary>
        /// 换手率
        /// </summary>
        public double m_turnoverRate;

        /// <summary>
        /// 成交量
        /// </summary>
        public double m_volume;

        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="data">数据</param>
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
        /// 比较是否相同
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>是否相同</returns>
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
    /// 历史数据信息
    /// </summary>
    public class HistoryDataInfo
    {
        #region Lord 2016/3/27
        /// <summary>
        /// 周期
        /// </summary>
        public int m_cycle;

        /// <summary>
        /// 结束日期
        /// </summary>
        public double m_endDate;

        /// <summary>
        /// 是否需要推送数据
        /// </summary>
        public bool m_pushData;

        /// <summary>
        /// 股票代码
        /// </summary>
        public String m_code;

        /// <summary>
        /// 数据条数
        /// </summary>
        public int m_size;

        /// <summary>
        /// 开始日期
        /// </summary>
        public double m_startDate;

        /// <summary>
        /// 复权模式
        /// </summary>
        public int m_subscription;

        /// <summary>
        /// 类型
        /// </summary>
        public int m_type;
        #endregion
    }

    /// <summary>
    /// 最新数据信息
    /// </summary>
    public class LatestDataInfo
    {
        #region Lord 2016/5/18
        /// <summary>
        /// 代码
        /// </summary>
        public String m_codes = "";

        /// <summary>
        /// 格式
        /// </summary>
        public int m_formatType;

        /// <summary>
        /// 是否包含LV2
        /// </summary>
        public int m_lv2;

        /// <summary>
        /// 数据条数
        /// </summary>
        public int m_size;
        #endregion
    }

    /// <summary>
    /// 公共字段
    /// </summary>
    public class KeyFields
    {
        #region Lord 2016/10/03
        /// <summary>
        /// 收盘价
        /// </summary>
        public const String CLOSE = "CLOSE";
        /// <summary>
        /// 最高价
        /// </summary>
        public const String HIGH = "HIGH";
        /// <summary>
        /// 最低价
        /// </summary>
        public const String LOW = "LOW";
        /// <summary>
        /// 开盘价
        /// </summary>
        public const String OPEN = "OPEN";
        /// <summary>
        /// 成交量
        /// </summary>
        public const String VOL = "VOL";
        /// <summary>
        /// 成交额
        /// </summary>
        public const String AMOUNT = "AMOUNT";

        /// <summary>
        /// 平均价格
        /// </summary>
        public const String AVGPRICE = "AVGPRICE";

        /// <summary>
        /// 收盘价字段
        /// </summary>
        public const int CLOSE_INDEX = 0;
        /// <summary>
        /// 最高价字段
        /// </summary>
        public const int HIGH_INDEX = 1;
        /// <summary>
        /// 最低价字段
        /// </summary>
        public const int LOW_INDEX = 2;
        /// <summary>
        /// 开盘价字段
        /// </summary>
        public const int OPEN_INDEX = 3;
        /// <summary>
        /// 成交量字段
        /// </summary>
        public const int VOL_INDEX = 4;
        /// <summary>
        /// 成交额字段
        /// </summary>
        public const int AMOUNT_INDEX = 5;

        /// <summary>
        /// 平均价格字段
        /// </summary>
        public const int AVGPRICE_INDEX = 6;
        #endregion
    }

    /// <summary>
    /// 指标数据
    /// </summary>
    public class IndicatorData
    {
        /// <summary>
        /// 参数
        /// </summary>
        public String m_parameters;

        /// <summary>
        /// 脚本
        /// </summary>
        public String m_script;

    }

    /// <summary>
    /// 指标对象
    /// </summary>
    public class Indicator
    {
        #region Lord 2016/3/13
        /// <summary>
        /// 类别
        /// </summary>
        public String m_category = "";

        /// <summary>
        /// 预定显示坐标
        /// </summary>
        public String m_coordinate = "";

        /// <summary>
        /// 描述
        /// </summary>
        public String m_description = "";

        /// <summary>
        /// 显示小数的位数
        /// </summary>
        public int m_digit;

        /// <summary>
        /// 指标ID
        /// </summary>
        public String m_indicatorID = "";

        /// <summary>
        /// 名称
        /// </summary>
        public String m_name = "";

        /// <summary>
        /// 列表顺序
        /// </summary>
        public int m_orderNum;

        /// <summary>
        /// 画线方法
        /// </summary>
        public int m_paintType;

        /// <summary>
        /// 参数
        /// </summary>
        public String m_parameters = "";

        /// <summary>
        /// 密码
        /// </summary>
        public String m_password = "";

        /// <summary>
        /// 特殊Y轴坐标
        /// </summary>
        public String m_specialCoordinate = "";

        /// <summary>
        /// 文本
        /// </summary>
        public String m_text = "";

        /// <summary>
        /// 类型
        /// </summary>
        public int m_type;

        /// <summary>
        /// 是否使用密码
        /// </summary>
        public int m_usePassword;

        /// <summary>
        /// 用户ID
        /// </summary>
        public int m_userID;

        /// <summary>
        /// 版本
        /// </summary>
        public int m_version;
        #endregion
    }

    /// <summary>
    /// 交易信息
    /// </summary>
    public class OrderInfo
    {
        public String m_code;
        public float m_price;
        public int m_qty;
    }

    /// <summary>
    /// 交易策略设置
    /// </summary>
    public class SecurityStrategySetting
    {
        /// <summary>
        /// 设置的股票代码
        /// </summary>
        public String m_securityCode = "";
        /// <summary>
        /// 设置的股票名称
        /// </summary>
        public String m_securityName = "";
        /// <summary>
        /// 策略类型
        /// 0：区间交易
        /// </summary>
        public int m_strategyType = 0;
        /// <summary>
        /// 交易策略的内容
        /// </summary>
        public String m_strategySettingInfo = "";
    }

    /// <summary>
    /// 股票区间交易条件
    /// </summary>
    public class SecurityRangeTradeCondition
    {
        /// <summary>
        /// 区间下限
        /// </summary>
        public float m_bottomRangePrice = 0;
        /// <summary>
        /// 每次购买数量
        /// </summary>
        public int m_buyCount = 0;
        /// <summary>
        /// 委托买入不成交，多长时间后撤销,单位秒,0为不撤销
        /// </summary>
        public int m_cancelIntervalBuyNoDeal = 0;
        /// <summary>
        /// 委托卖出不成交，多长时间后撤销,单位秒,0为不撤销
        /// </summary>
        public int m_cancelIntervalSellNoDeal = 0;
        /// <summary>
        /// 清仓价格
        /// </summary>
        public float m_cleanPrice = 0;
        /// <summary>
        /// 有效时间段的开始时间
        /// </summary>
        public String m_fromEffectiveTransactionTime = "";
        /// <summary>
        /// 初始建仓数量
        /// </summary>
        public int m_initBuildCount = 0;
        /// <summary>
        /// 是否已经初始建仓
        /// </summary>
        public bool m_initBuildFlag = false;
        /// <summary>
        /// 初始建仓价格
        /// </summary>
        public float m_initBuildPrice = 0;
        /// <summary>
        /// 委托后，多长时间检查成交状态,单位秒,0为不查询
        /// </summary>
        public int m_intervalChceckTradeStatus = 0;
        /// <summary>
        /// 是否跨区间乘数买
        /// </summary>
        public bool m_isCrossBuy = false;
        /// <summary>
        /// 是否跨区间乘数卖
        /// </summary>
        public bool m_isCrossSell = false;
        /// <summary>
        /// 是否设置有效时间段
        /// </summary>
        public bool m_isSetEffectiveTransactionTime = false;
        /// <summary>
        /// 低于上次成交价的比例或者价格
        /// </summary>
        public float m_lowLastDealBuy = 0;
        /// <summary>
        /// 上次成交的数量
        /// </summary>
        public int m_lastDealCount = 0;
        /// <summary>
        /// 上次成交的价格
        /// </summary>
        public float m_lastDealPrice = 0;
        /// <summary>
        /// 有效时间段的结束时间
        /// </summary>
        public String m_toEffectiveTransactionTime = "";
        /// <summary>
        /// 设置的股票代码
        /// </summary>
        public String m_securityCode = "";
        /// <summary>
        /// 设置的股票名称
        /// </summary>
        public String m_securityName = "";
        /// <summary>
        /// 是否按价格买卖。默认按照百分比
        /// </summary>
        public bool m_isBasePrice = false;
        /// <summary>
        /// 涨跌计算标准
        /// </summary>
        public String m_priceDealBaseLine = "";
        /// <summary>
        /// 每次卖出的数量
        /// </summary>
        public int m_sellCount = 0;
        /// <summary>
        /// 价格上限
        /// </summary>
        public float m_topRangePrice = 0;
        /// <summary>
        /// 成交价上涨多少卖出
        /// </summary>
        public float m_overLastDealSell = 0;
    }

    /// <summary>
    /// 股票成交信息
    /// </summary>
    public class SecurityTrade
    {
        /// <summary>
        /// 撤销数量
        /// </summary>
        public double m_cancelVolume = 0;
        /// <summary>
        /// 操作
        /// </summary>
        public String m_operate;
        /// <summary>
        /// 合同编号
        /// </summary>
        public String m_orderSysID;
        /// <summary>
        /// 成交编号
        /// </summary>
        public String m_orderTradeID;
        /// <summary>
        /// 订单类型
        /// </summary>
        public String m_orderType;
        /// <summary>
        /// 股票余额
        /// </summary>
        public double m_stockBalance = 0;
        /// <summary>
        /// 证券代码
        /// </summary>
        public String m_stockCode;
        /// <summary>
        /// 证券名称
        /// </summary>
        public String m_stockName;
        /// <summary>
        /// 成交金额
        /// </summary>
        public double m_tradeAmount = 0;
        /// <summary>
        /// 成交时间
        /// </summary>
        public String m_tradeDate;
        /// <summary>
        /// 成交数量
        /// </summary>
        public double m_tradeVolume = 0;
        /// <summary>
        /// 成交均价
        /// </summary>
        public double m_tradeAvgPrice = 0;

        /// <summary>
        /// 字符串转换成成交对象
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
    /// 交易账户信息
    /// </summary>
    public class SecurityTradingAccount
    {
        /// <summary>
        ///  可用金额
        /// </summary>
        public double m_available = 0;
        /// <summary>
        ///  资金余额
        /// </summary>
        public double m_capitalBalance = 0;
        /// <summary>
        ///  冻结金额
        /// </summary>
        public double m_frozenCash = 0;
        /// <summary>
        ///  股票市值
        /// </summary>
        public double m_stockValue = 0;
        /// <summary>
        ///  总资产
        /// </summary>
        public double m_totalCapital = 0;
        /// <summary>
        ///  可取金额
        /// </summary>
        public double m_withdrawQuota = 0;

        /// <summary>
        /// 字符串转换成持仓对象
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
    /// 股票持仓信息
    /// </summary>
    public class SecurityPosition
    {
        /// <summary>
        ///  可用余额
        /// </summary>
        public double m_availableBalance = 0;

        /// <summary>
        /// 冻结数量
        /// </summary>
        public double m_frozenVolume = 0;

        /// <summary>
        /// 股东帐户
        /// </summary>
        public String m_investorAccount;

        /// <summary>
        /// 交易市场
        /// </summary>
        public String m_marketName;

        /// <summary>
        /// 市价
        /// </summary>
        public double m_marketPrice = 0;

        /// <summary>
        /// 市值
        /// </summary>
        public double m_marketValue = 0;

        /// <summary>
        /// 成本价
        /// </summary>
        public double m_positionCost;

        /// <summary>
        /// 盈亏
        /// </summary>
        public double m_positionProfit;

        /// <summary>
        /// 盈亏比(%)
        /// </summary>
        public double m_positionProfitRatio;

        /// <summary>
        /// 可申赎数量
        /// </summary>
        public double m_redemptionVolume = 0;

        /// <summary>
        /// 股票余额
        /// </summary>
        public double m_stockBalance = 0;

        /// <summary>
        /// 证券代码
        /// </summary>
        public String m_stockCode;
        /// <summary>
        /// 证券名称
        /// </summary>
        public String m_stockName;

        /// <summary>
        /// 实际数量
        /// </summary>
        public double m_volume = 0;

        /// <summary>
        /// 字符串转换成持仓对象
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
    /// 股票委托信息
    /// </summary>
    public class SecurityCommission
    {
        /// <summary>
        /// 撤销数量
        /// </summary>
        public double m_cancelVolume = 0;
        /// <summary>
        /// 操作
        /// </summary>
        public String m_operate;
        /// <summary>
        /// 委托时间
        /// </summary>
        public String m_orderDate;
        /// <summary>
        /// 委托价格
        /// </summary>
        public double m_orderPrice = 0;
        /// <summary>
        /// 合同编号
        /// </summary>
        public String m_orderSysID;
        /// <summary>
        /// 订单类型
        /// </summary>
        public String m_orderType;
        /// <summary>
        /// 委托数量
        /// </summary>
        public double m_orderVolume = 0;
        /// <summary>
        /// 备注
        /// </summary>
        public String m_remark;
        /// <summary>
        /// 证券代码
        /// </summary>
        public String m_stockCode;
        /// <summary>
        /// 证券名称
        /// </summary>
        public String m_stockName;
        /// <summary>
        /// 成交均价
        /// </summary>
        public double m_tradeAvgPrice = 0;
        /// <summary>
        /// 成交数量
        /// </summary>
        public double m_tradeVolume = 0;

        /// <summary>
        /// 字符串转换成委托对象
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
