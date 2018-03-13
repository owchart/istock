using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace OwLib
{
    public class OrderTrade
    {
        public OrderTrade(MainFrame mainFrame)
        {
            m_mainFrame = mainFrame;
            // 委托表格
            m_gridCommissionAccount = m_mainFrame.GetGrid("gridOrderAccount");
            // 持仓表格
            m_gridPositionAccount = m_mainFrame.GetGrid("gridPositionAccount");
            // 成交表格
            m_gridTradeAccount = m_mainFrame.GetGrid("gridTradeAccount");

            // 获取区间交易策略
            ReloadStrategySetting();
            AutoTradeService.InitSysTreeView32Handle();

            DataCenter.ThsDealService.RegisterListener(5, ThsDealCallBack);
            DataCenter.ThsDealService.RegisterListener(6, ThsDealCallBack);
            DataCenter.ThsDealService.RegisterListener(7, ThsDealCallBack);
            DataCenter.ThsDealService.RegisterListener(8, ThsDealCallBack);

            ButtonA btnStart = m_mainFrame.GetButton("btnStart");
            if (btnStart == null)
            {
                return;
            }

            if (!m_isStart)
            {
                THSDealInfo req = new THSDealInfo();
                //req.m_operateType = 5;
                //req.m_reqID = DataCenter.ThsDealService.GetRequestID();
                //DataCenter.ThsDealService.AddTHSDealReq(req);
                //req = new THSDealInfo();
                //req.m_operateType = 6;
                //req.m_reqID = DataCenter.ThsDealService.GetRequestID();
                //DataCenter.ThsDealService.AddTHSDealReq(req);
                //req = new THSDealInfo();
                req.m_operateType = 7;
                req.m_reqID = DataCenter.ThsDealService.GetRequestID();
                DataCenter.ThsDealService.AddTHSDealReq(req);
                //req = new THSDealInfo();
                //req.m_operateType = 8;
                //req.m_reqID = DataCenter.ThsDealService.GetRequestID();
                //DataCenter.ThsDealService.AddTHSDealReq(req);
                DataCenter.ThsDealService.StartTHSDealService();
                m_isStart = true;
                btnStart.Text = "停止";
            }
            else
            {
                m_isStart = false;
                btnStart.Text = "启动";
            }
        }

        // 持仓字典
        private IDictionary<String, SecurityPosition> m_dictSecurityPositions = new Dictionary<String, SecurityPosition>();

        // 委托表格
        private GridA m_gridCommissionAccount;
        // 持仓表格
        private GridA m_gridPositionAccount;
        // 成交表格
        private GridA m_gridTradeAccount;
        // 是否启动
        private bool m_isStart = false;

        // 当前执行的交易策略
        private SecurityStrategySetting m_securityStrategySettingCurrnet = null;
        // 交易策略
        private List<SecurityStrategySetting> m_securityStrategySettings = new List<SecurityStrategySetting>();
        // 账户信息
        private SecurityTradingAccount m_securityTradingAccount = null;
        // 上次委托尚没有成交的价格
        private float m_lastCommissionNoTradePrice = 0;

        private MainFrame m_mainFrame;

        public MainFrame MainFrame
        {
            get { return m_mainFrame; }
            set { m_mainFrame = value; }
        }

        /// <summary>
        /// 执行策略
        /// </summary>
        /// <param name="latestData">最新数据</param>
        public void DealStrategy(SecurityLatestData latestData)
        {
            if (!m_isStart)
            {
                return;
            }

            Thread deadThread = new Thread(new ParameterizedThreadStart(DealStrategyThread));
            deadThread.IsBackground = true;
            deadThread.Start(latestData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        public void DealStrategyThread(object param)
        {
            SecurityLatestData latestData = param as SecurityLatestData;
            if (latestData == null)
            {
                return;
            }

            if (m_securityStrategySettingCurrnet == null || m_securityStrategySettingCurrnet.m_strategyType != 0)
            {
                return;
            }

            SecurityRangeTradeCondition securityRangeTradeCondition
                = JsonConvert.DeserializeObject<SecurityRangeTradeCondition>(m_securityStrategySettingCurrnet.m_strategySettingInfo);
            if (securityRangeTradeCondition == null)
            {
                return;
            }

            bool isInitBuild = securityRangeTradeCondition.m_initBuildFlag;
            float bottomRangePrice = securityRangeTradeCondition.m_bottomRangePrice;
            float topRangePrice = securityRangeTradeCondition.m_topRangePrice;
            // 当前价格超过了区间的上限价格或者低于区间的下限，则不做处理
            if (latestData.m_close > topRangePrice || latestData.m_close < bottomRangePrice)
            {
                return;
            }
            // 已经建仓完成
            if (isInitBuild)
            {
                bool isBasePrice = securityRangeTradeCondition.m_isBasePrice;
                float lastDealPrice = securityRangeTradeCondition.m_lastDealPrice;
                int buyCount = securityRangeTradeCondition.m_buyCount;
                int sellCount = securityRangeTradeCondition.m_sellCount;
                float lowLastDealBuy = securityRangeTradeCondition.m_lowLastDealBuy;
                float overLastDealSell = securityRangeTradeCondition.m_overLastDealSell;
                bool isCrossBuy = securityRangeTradeCondition.m_isCrossBuy;
                bool isCrossSell = securityRangeTradeCondition.m_isCrossSell;

                // 当前价格和上次委托未成交价格的比较
                double diffPrice1 = latestData.m_close - m_lastCommissionNoTradePrice;
                if (diffPrice1 == 0)
                {
                    return;
                }
                if (diffPrice1 > 0 && diffPrice1 < overLastDealSell)
                {
                    return;
                }
                if (diffPrice1 < 0 && Math.Abs(diffPrice1) < lowLastDealBuy)
                {
                    return;
                }

                // 计算当前价格和上次成交价格的差值
                double diffPrice = latestData.m_close - lastDealPrice;
                if (diffPrice > 0)
                {
                    SecurityPosition postion = null;
                    if (!m_dictSecurityPositions.TryGetValue(latestData.m_code, out postion))
                    {
                        // 没有持仓信息，不做处理
                        return;
                    }

                    // 高于上次成交价格
                    int readSellCount = 0;
                    int sepaCount = (int)(diffPrice / overLastDealSell);
                    if (sepaCount < 1)
                    {
                        // 价格没有达到预期值
                        return;
                    }
                    if (isCrossSell)
                    {
                        readSellCount = sepaCount * sellCount;
                    }
                    else
                    {
                        readSellCount = sellCount;
                    }

                    if (readSellCount < 100)
                    {
                        return;
                    }

                    // 股票余额小于可卖数量
                    if (postion.m_stockBalance < readSellCount)
                    {
                        readSellCount = (int)postion.m_stockBalance;
                    }

                    OrderInfo info = new OrderInfo();
                    info.m_code = CStrA.ConvertDBCodeToDealCode(latestData.m_code);
                    info.m_price = (float)Math.Round(latestData.m_close, 2);
                    info.m_qty = readSellCount;
                    m_lastCommissionNoTradePrice = info.m_price;
                    AutoTradeService.Sell(info);
                    Thread.Sleep(3000);
                    THSDealInfo req = new THSDealInfo();
                    req.m_operateType = 4;
                    req.m_reqID = DataCenter.ThsDealService.GetRequestID();
                    DataCenter.ThsDealService.AddTHSDealReq(req);
                }
                else if (diffPrice < 0)
                {
                    if (m_securityTradingAccount == null)
                    {
                        // 没有资金账户信息
                        return;
                    }

                    // 低于上次成交价格
                    int readBuyCount = 0;
                    int sepaCount = (int)(Math.Abs(diffPrice) / overLastDealSell);
                    if (sepaCount < 1)
                    {
                        // 价格没有达到预期值
                        return;
                    }
                    if (isCrossBuy)
                    {
                        readBuyCount = sepaCount * buyCount;
                    }
                    else
                    {
                        readBuyCount = buyCount;
                    }

                    if (readBuyCount < 100)
                    {
                        return;
                    }

                    int capitalAllowBuyCount = (int)((m_securityTradingAccount.m_capitalBalance - m_securityTradingAccount.m_frozenCash) / latestData.m_close) / 100 * 100;
                    // 资金余额小于可买的数量
                    if (capitalAllowBuyCount < readBuyCount)
                    {
                        readBuyCount = capitalAllowBuyCount;
                    }

                    OrderInfo info = new OrderInfo();
                    info.m_code = CStrA.ConvertDBCodeToDealCode(latestData.m_code);
                    info.m_price = (float)Math.Round(latestData.m_close, 2);
                    info.m_qty = readBuyCount;
                    m_lastCommissionNoTradePrice = info.m_price;
                    AutoTradeService.Buy(info);
                    Thread.Sleep(3000);
                    THSDealInfo req = new THSDealInfo();
                    req.m_operateType = 3;
                    req.m_reqID = DataCenter.ThsDealService.GetRequestID();
                    DataCenter.ThsDealService.AddTHSDealReq(req);
                }
            }
            else
            {
            }
        }

        /// <summary>
        /// 重新加载交易策略
        /// </summary>
        public void ReloadStrategySetting()
        {
            // 获取区间交易策略
            DataCenter.StrategySettingService.GetSecurityStrategySettings(0, m_securityStrategySettings);
            GSecurity security = null;
            if (m_securityStrategySettings != null && m_securityStrategySettings.Count > 0)
            {
                m_securityStrategySettingCurrnet = m_securityStrategySettings[0];
                security = new GSecurity();
                security.m_code = m_securityStrategySettingCurrnet.m_securityCode;
                security.m_name = m_securityStrategySettingCurrnet.m_securityName;
            }
        }

        /// <summary>
        /// 设置股票账户委托信息
        /// </summary>
        /// <param name="commissionResult"></param>
        private void SetSecurityCommission(String commissionResult)
        {
            if (commissionResult == null || commissionResult.Length == 0)
            {
                return;
            }
            String[] lines = commissionResult.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (lines == null || lines.Length < 2)
            {
                return;
            }

            m_gridCommissionAccount.BeginUpdate();
            m_gridCommissionAccount.ClearRows();
            for (int i = 1; i < lines.Length; i++)
            {
                SecurityCommission commission = SecurityCommission.ConvertToSecurityCommission(lines[i]);
                if (commission != null)
                {
                    GridRow row = new GridRow();
                    m_gridCommissionAccount.AddRow(row);
                    GridCell cell = new GridStringCell(commission.m_orderDate);
                    row.AddCell(0, cell);
                    cell = new GridStringCell(commission.m_stockCode);
                    row.AddCell(1, cell);
                    cell = new GridStringCell(commission.m_stockName);
                    row.AddCell(2, cell);
                    cell = new GridStringCell(commission.m_operate);
                    row.AddCell(3, cell);
                    cell = new GridStringCell(commission.m_remark);
                    row.AddCell(4, cell);
                    cell = new GridDoubleCell(commission.m_orderVolume);
                    row.AddCell(5, cell);
                    cell = new GridDoubleCell(commission.m_tradeVolume);
                    row.AddCell(6, cell);
                    cell = new GridDoubleCell(commission.m_cancelVolume);
                    row.AddCell(7, cell);
                    cell = new GridDoubleCell(commission.m_orderPrice);
                    row.AddCell(8, cell);
                    cell = new GridStringCell(commission.m_orderType);
                    row.AddCell(9, cell);
                    cell = new GridDoubleCell(commission.m_tradeAvgPrice);
                    row.AddCell(10, cell);
                    cell = new GridStringCell(commission.m_orderSysID);
                    row.AddCell(11, cell);
                }
            }
            m_gridCommissionAccount.EndUpdate();
            m_gridCommissionAccount.Invalidate();
        }

        /// <summary>
        /// 设置股票账户持仓信息
        /// </summary>
        /// <param name="stockPositionResult"></param>
        private void SetSecurityPosition(String stockPositionResult)
        {
            m_dictSecurityPositions.Clear();
            if (stockPositionResult == null || stockPositionResult.Length == 0)
            {
                return;
            }
            String[] lines = stockPositionResult.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (lines == null || lines.Length < 2)
            {
                return;
            }

            m_gridPositionAccount.BeginUpdate();
            m_gridPositionAccount.ClearRows();
            for (int i = 1; i < lines.Length; i++)
            {
                SecurityPosition position = SecurityPosition.ConvertToStockPosition(lines[i]);
                if (position != null)
                {
                    m_dictSecurityPositions[position.m_stockCode] = position;

                    GridRow row = new GridRow();
                    m_gridPositionAccount.AddRow(row);
                    GridCell cell = new GridStringCell(position.m_stockCode);
                    row.AddCell(0, cell);
                    cell = new GridStringCell(position.m_stockName);
                    row.AddCell(1, cell);
                    cell = new GridDoubleCell(position.m_stockBalance);
                    row.AddCell(2, cell);
                    cell = new GridDoubleCell(position.m_availableBalance);
                    row.AddCell(3, cell);
                    cell = new GridDoubleCell(position.m_volume);
                    row.AddCell(4, cell);
                    cell = new GridDoubleCell(position.m_frozenVolume);
                    row.AddCell(5, cell);
                    cell = new GridDoubleCell(position.m_positionProfit);
                    row.AddCell(6, cell);
                    cell = new GridDoubleCell(position.m_positionCost);
                    row.AddCell(7, cell);
                    cell = new GridDoubleCell(position.m_positionProfitRatio);
                    row.AddCell(8, cell);
                    cell = new GridDoubleCell(position.m_marketPrice);
                    row.AddCell(9, cell);
                    cell = new GridDoubleCell(position.m_marketValue);
                    row.AddCell(10, cell);
                    cell = new GridDoubleCell(position.m_redemptionVolume);
                    row.AddCell(11, cell);
                    cell = new GridStringCell(position.m_marketName);
                    row.AddCell(12, cell);
                    cell = new GridStringCell(position.m_investorAccount);
                    row.AddCell(13, cell);
                }
            }
            m_gridPositionAccount.EndUpdate();
            m_gridPositionAccount.Invalidate();
        }

        /// <summary>
        /// 设置股票账户成交信息
        /// </summary>
        /// <param name="tradeResult"></param>
        private void SetSecurityTrade(String tradeResult)
        {
            if (tradeResult == null || tradeResult.Length == 0)
            {
                return;
            }
            String[] lines = tradeResult.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (lines == null || lines.Length < 2)
            {
                return;
            }

            m_gridTradeAccount.BeginUpdate();
            m_gridTradeAccount.ClearRows();
            for (int i = 1; i < lines.Length; i++)
            {
                SecurityTrade trade = SecurityTrade.ConvertToStockTrade(lines[i]);
                if (trade != null)
                {
                    GridRow row = new GridRow();
                    m_gridTradeAccount.AddRow(row);
                    GridCell cell = new GridStringCell(trade.m_tradeDate);
                    row.AddCell(0, cell);
                    cell = new GridStringCell(trade.m_stockCode);
                    row.AddCell(1, cell);
                    cell = new GridStringCell(trade.m_stockName);
                    row.AddCell(2, cell);
                    cell = new GridStringCell(trade.m_operate);
                    row.AddCell(3, cell);
                    cell = new GridDoubleCell(trade.m_tradeVolume);
                    row.AddCell(4, cell);
                    cell = new GridDoubleCell(trade.m_tradeAvgPrice);
                    row.AddCell(5, cell);
                    cell = new GridDoubleCell(trade.m_tradeAmount);
                    row.AddCell(6, cell);
                    cell = new GridStringCell(trade.m_orderSysID);
                    row.AddCell(7, cell);
                    cell = new GridStringCell(trade.m_orderTradeID);
                    row.AddCell(8, cell);
                    cell = new GridDoubleCell(trade.m_cancelVolume);
                    row.AddCell(9, cell);
                    cell = new GridDoubleCell(trade.m_stockBalance);
                    row.AddCell(10, cell);
                }
            }
            m_gridTradeAccount.EndUpdate();
            m_gridTradeAccount.Invalidate();
        }

        /// <summary>
        /// 设置股票账户资金
        /// </summary>
        /// <param name="tradeResult"></param>
        private void SetSecurityTradingAccount(String stockCaptialResult)
        {
            m_securityTradingAccount = null;
            if (stockCaptialResult == null || stockCaptialResult.Length == 0)
            {
                return;
            }
            SecurityTradingAccount stockTradingAccount = SecurityTradingAccount.ConvertToStockTradingAccount(stockCaptialResult);
            if (stockTradingAccount == null)
            {
                return;
            }
            m_securityTradingAccount = stockTradingAccount;
            LabelA lblCapitalBalance = m_mainFrame.GetLabel("lblCapitalBalance");
            if (lblCapitalBalance != null)
            {
                lblCapitalBalance.Text = stockTradingAccount.m_capitalBalance.ToString();
            }
            LabelA lblFrozenCash = m_mainFrame.GetLabel("lblFrozenCash");
            if (lblFrozenCash != null)
            {
                lblFrozenCash.Text = stockTradingAccount.m_frozenCash.ToString();
            }
            LabelA lblAvailable = m_mainFrame.GetLabel("lblAvailable");
            if (lblAvailable != null)
            {
                lblAvailable.Text = stockTradingAccount.m_available.ToString();
            }
            LabelA lblWithdrawQuota = m_mainFrame.GetLabel("lblWithdrawQuota");
            if (lblWithdrawQuota != null)
            {
                lblWithdrawQuota.Text = stockTradingAccount.m_withdrawQuota.ToString();
            }
            LabelA lblStockValue = m_mainFrame.GetLabel("lblStockValue");
            if (lblStockValue != null)
            {
                lblStockValue.Text = stockTradingAccount.m_stockValue.ToString();
            }
            LabelA lblTotalCapital = m_mainFrame.GetLabel("lblTotalCapital");
            if (lblTotalCapital != null)
            {
                lblTotalCapital.Text = stockTradingAccount.m_totalCapital.ToString();
            }

            DivA divCapital = m_mainFrame.GetDiv("divCapital");
            divCapital.Invalidate();
        }

        /// <summary>
        /// 显示设置策略窗体
        /// </summary>
        public void ShowStrategySettingWindow()
        {
            SecurityStrategySetting securityStrategySetting = null;
            if (m_securityStrategySettings.Count > 0)
            {
                securityStrategySetting = m_securityStrategySettings[0];
            }
            RTCWindow strategySettingWindow = new RTCWindow(m_mainFrame.Native);
            strategySettingWindow.MainFrame = m_mainFrame;
            strategySettingWindow.SecurityStrategySetting = securityStrategySetting;
            strategySettingWindow.Show();
        }

        /// <summary>
        /// 同花顺消息返回
        /// </summary>
        /// <param name="operateType">操作类型</param>
        /// <param name="requstID">请求ID</param>
        /// <param name="result">返回的结果</param>
        public void ThsDealCallBack(int operateType, int requstID, String resutlt)
        {
            // 交易类型
            // 0:默认值
            // 1:买入
            // 2:卖出
            // 3:撤销买入
            // 4:撤销卖出
            // 5:查询持仓
            // 6:查询成交
            // 7:查询资金
            // 8:查询委托
            switch (operateType)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    SetSecurityPosition(resutlt);
                    break;
                case 6:
                    SetSecurityTrade(resutlt);
                    break;
                case 7:
                    SetSecurityTradingAccount(resutlt);
                    break;
                case 8:
                    SetSecurityCommission(resutlt);
                    break;
            }
        }
    }
}
