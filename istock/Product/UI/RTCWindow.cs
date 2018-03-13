using OwLib;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace OwLib
{
    /// <summary>
    /// 设置区间策略窗体
    /// </summary>
    public class RTCWindow : WindowXmlEx
    {
        // 设置股票
        private TextBoxA m_txtSecurity = null;
        // 按价格
        private CheckBoxA m_chkBasePrice = null;
        // 区间上限
        private SpinA m_spinTop = null;
        // 区间下限
        private SpinA m_spinBottom = null;
        // 每次买入
        private SpinA m_spinBuyCount = null;
        // 每次卖出
        private SpinA m_spinSellCount = null;
        // 上次成交价
        private SpinA m_spinLastDealPrice = null;
        // 上次成交量
        private SpinA m_spinLastDealCount = null;
        // 成交价上涨
        private SpinA m_spinSellOverLastDeal = null;
        // 成交价下跌
        private SpinA m_spinBuyLowLastDeal = null;
        // 涨跌计算标准
        private ComboBoxA m_cbxBaseLine = null;

        // 初始建仓价格
        private SpinA m_spinInitPrice = null;
        // 初始建仓数量
        private SpinA m_spinInitCount = null;
        // 跨区间乘数买
        private CheckBoxA m_chkMultiBuy = null;
        // 是否已经建仓
        private CheckBoxA m_chkIsBuild = null;
        // 委托买入不成交 秒后撤单，0为不撤单
        private SpinA m_spinBuyCancelSecond = null;

        // 清仓价格
        private SpinA m_spinCleanPrice = null;
        // 跨区间乘数卖
        private CheckBoxA m_chkMultiSell = null;
        // 委托卖出不成交 秒后撤单，0为不撤单
        private SpinA m_spinSellCancelSecond = null;

        // 是否设置有效时间段
        private CheckBoxA m_chkSetEffectiveTransactionTime = null;
        // 有效时间段的开始时间
        private TextBoxA m_txtFromEffectiveTransactionTime = null;
        // 有效时间段的结束时间
        private TextBoxA m_txtToEffectiveTransactionTime = null;
        // 委托后多少秒后检查此次委托成交状态，0为不检查
        private SpinA m_spinCheckOrderStatusInte = null;
        // 股票名称
        private String m_securityName = "";

        /// <summary>
        /// 创建设置策略窗体
        /// </summary>
        /// <param name="native">方法库</param>
        public RTCWindow(INativeBase native)
        {
            Load(native, "RTCWindow", "windowRTC");
            m_invokeEvent = new ControlInvokeEvent(Invoke);
            m_window.RegisterEvent(m_invokeEvent, EVENTID.INVOKE);
            InitControl();
            //注册点击事件
            RegisterEvents(m_window);
        }

        /// <summary>
        /// 调用控件方法事件
        /// </summary>
        private ControlInvokeEvent m_invokeEvent;

        private MainFrame m_mainFrame;

        /// <summary>
        /// 获取或者设置LordManager
        /// </summary>
        public MainFrame MainFrame
        {
            get { return m_mainFrame; }
            set { m_mainFrame = value; }
        }

        private SecurityStrategySetting m_securityStrategySetting = null;

        /// <summary>
        /// 获取或者设置交易策略
        /// </summary>
        public SecurityStrategySetting SecurityStrategySetting
        {
            get  { return m_securityStrategySetting;  }
            set { m_securityStrategySetting = value; }
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮值/param>
        private void ClickEvent(object sender, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            if (button == MouseButtonsA.Left && clicks == 1)
            {
                ControlA control = sender as ControlA;
                String name = control.Name;
                if (name == "btnConfirm")
                {
                    UpdateSecurityRangeTradeCondition();
                    m_mainFrame.OrderTrade.ReloadStrategySetting();
                    Close();
                }
                else if (name == "btnCancel")
                {
                    Close();
                }
            }
        }

        /// <summary>
        /// 销毁资源方法
        /// </summary>
        public override void Dispose()
        {
            if (!IsDisposed)
            {
                base.Dispose();
            }
        }
        /// <summary>
        /// 初始化控件
        /// </summary>
        public void InitControl()
        {
            m_txtSecurity = GetTextBox("txtSecurity");
            m_chkBasePrice = GetCheckBox("chkBasePrice");
            m_spinTop = GetSpin("spinTop");
            m_spinBottom = GetSpin("spinBottom");
            m_spinBuyCount = GetSpin("spinBuyCount");
            m_spinSellCount = GetSpin("spinSellCount");
            m_spinLastDealPrice = GetSpin("spinLastDealPrice");
            m_spinLastDealCount = GetSpin("spinLastDealCount");
            m_spinSellOverLastDeal = GetSpin("spinSellOverLastDeal");
            m_spinBuyLowLastDeal = GetSpin("spinBuyLowLastDeal");
            m_cbxBaseLine = GetComboBox("cbxBaseLine");
            m_spinInitPrice = GetSpin("spinInitPrice");
            m_spinInitCount = GetSpin("spinInitCount");
            m_chkMultiBuy = GetCheckBox("chkMultiBuy");
            m_chkIsBuild = GetCheckBox("chkIsBuild");
            m_spinBuyCancelSecond = GetSpin("spinBuyCancelSecond");
            m_spinCleanPrice = GetSpin("spinCleanPrice");
            m_chkMultiSell = GetCheckBox("chkMultiSell");
            m_spinSellCancelSecond = GetSpin("spinSellCancelSecond");
            m_chkSetEffectiveTransactionTime = GetCheckBox("chkSetEffectiveTransactionTime");
            m_txtFromEffectiveTransactionTime = GetTextBox("txtFromEffectiveTransactionTime");
            m_txtToEffectiveTransactionTime = GetTextBox("txtToEffectiveTransactionTime");
            m_spinCheckOrderStatusInte = GetSpin("spinCheckOrderStatusInte");
        }

        /// <summary>
        /// 初始化画面
        /// </summary>
        private void InitForm()
        {
            if(m_securityStrategySetting != null)
            {
                SecurityRangeTradeCondition securityRangeTradeCondition
                    = JsonConvert.DeserializeObject<SecurityRangeTradeCondition>(m_securityStrategySetting.m_strategySettingInfo);
                if(securityRangeTradeCondition == null)
                {
                    return;
                }
                m_txtSecurity.Text = securityRangeTradeCondition.m_securityCode;
                m_chkBasePrice.Checked = securityRangeTradeCondition.m_isBasePrice;
                m_spinTop.Value = securityRangeTradeCondition.m_topRangePrice;
                m_spinBottom.Value = securityRangeTradeCondition.m_bottomRangePrice;
                m_spinBuyCount.Value = securityRangeTradeCondition.m_buyCount;
                m_spinSellCount.Value = securityRangeTradeCondition.m_sellCount;
                m_spinLastDealPrice.Value = securityRangeTradeCondition.m_lastDealPrice;
                m_spinLastDealCount.Value = securityRangeTradeCondition.m_lastDealCount;
                m_spinSellOverLastDeal.Value = securityRangeTradeCondition.m_overLastDealSell;
                m_spinBuyLowLastDeal.Value = securityRangeTradeCondition.m_lowLastDealBuy;
                m_cbxBaseLine.SelectedIndex = 0;
                m_spinInitPrice.Value = securityRangeTradeCondition.m_initBuildPrice;
                m_spinInitCount.Value = securityRangeTradeCondition.m_initBuildCount;
                m_chkMultiBuy.Checked = securityRangeTradeCondition.m_isCrossBuy;
                m_chkIsBuild.Checked = securityRangeTradeCondition.m_initBuildFlag;
                m_spinBuyCancelSecond.Value = securityRangeTradeCondition.m_cancelIntervalBuyNoDeal;
                m_spinCleanPrice.Value = securityRangeTradeCondition.m_cleanPrice;
                m_chkMultiSell.Checked = securityRangeTradeCondition.m_isCrossSell;
                m_spinSellCancelSecond.Value = securityRangeTradeCondition.m_cancelIntervalSellNoDeal;
                m_chkSetEffectiveTransactionTime.Checked = securityRangeTradeCondition.m_isSetEffectiveTransactionTime;
                m_txtFromEffectiveTransactionTime.Text = securityRangeTradeCondition.m_fromEffectiveTransactionTime;
                m_txtToEffectiveTransactionTime.Text = securityRangeTradeCondition.m_toEffectiveTransactionTime;
                m_spinCheckOrderStatusInte.Value = securityRangeTradeCondition.m_intervalChceckTradeStatus;
                m_securityName = securityRangeTradeCondition.m_securityName;
            }
        }

        /// <summary>
        /// 调用控件线程方法
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="args">参数</param>
        private void Invoke(object sender, object args)
        {
            OnInvoke(args);
        }

        /// <summary>
        /// 调用控件线程方法
        /// </summary>
        /// <param name="args">参数</param>
        public void OnInvoke(object args)
        {
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="control">控件</param>
        private void RegisterEvents(ControlA control)
        {
            ControlMouseEvent clickButtonEvent = new ControlMouseEvent(ClickEvent);
            List<ControlA> controls = control.GetControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++)
            {
                ControlA subControl = controls[i];
                ButtonA button = subControl as ButtonA;
                GridColumn column = subControl as GridColumn;
                GridA grid = subControl as GridA;
                CheckBoxA checkBox = subControl as CheckBoxA;
                if (column != null)
                {
                    column.AllowResize = true;
                    column.BackColor = CDraw.PCOLORS_BACKCOLOR;
                    column.BorderColor = CDraw.PCOLORS_LINECOLOR2;
                    column.ForeColor = CDraw.PCOLORS_FORECOLOR;
                }
                else if (checkBox != null)
                {
                    checkBox.ButtonBackColor = CDraw.PCOLORS_BACKCOLOR;
                }
                else if (button != null)
                {
                    button.RegisterEvent(clickButtonEvent, EVENTID.CLICK);
                }
                else if (grid != null)
                {
                    grid.BackColor = COLOR.EMPTY;
                    grid.GridLineColor = CDraw.PCOLORS_LINECOLOR2;
                    GridRowStyle rowStyle = new GridRowStyle();
                    grid.RowStyle = rowStyle;
                    rowStyle.BackColor = COLOR.EMPTY;
                    rowStyle.SelectedBackColor = CDraw.PCOLORS_SELECTEDROWCOLOR;
                    rowStyle.HoveredBackColor = CDraw.PCOLORS_HOVEREDROWCOLOR;
                    grid.HorizontalOffset = grid.Width;
                    grid.UseAnimation = true;
                }
                else
                {
                    if (subControl.GetControlType() == "Div" || subControl.GetControlType() == "TabControl"
                        || subControl.GetControlType() == "TabPage"
                        || subControl.GetControlType() == "SplitLayoutDiv")
                    {
                        subControl.BackColor = COLOR.EMPTY;
                    }
                }
                RegisterEvents(controls[i]);
            }
        }
        

        /// <summary>
        /// 显示
        /// </summary>
        public override void Show()
        {
            base.Show();
            InitForm();
        }

        /// <summary>
        /// 更新区间策略
        /// </summary>
        public void UpdateSecurityRangeTradeCondition()
        {
            SecurityRangeTradeCondition securityRangeTradeCondition = new SecurityRangeTradeCondition();
            securityRangeTradeCondition.m_securityCode = m_txtSecurity.Text;
            securityRangeTradeCondition.m_isBasePrice = m_chkBasePrice.Checked;
            securityRangeTradeCondition.m_topRangePrice = (float)m_spinTop.Value;
            securityRangeTradeCondition.m_bottomRangePrice = (float)m_spinBottom.Value;
            securityRangeTradeCondition.m_buyCount = (int)m_spinBuyCount.Value;
            securityRangeTradeCondition.m_sellCount = (int)m_spinSellCount.Value;
            securityRangeTradeCondition.m_lastDealPrice = (float)m_spinLastDealPrice.Value;
            securityRangeTradeCondition.m_lastDealCount = (int)m_spinLastDealCount.Value;
            securityRangeTradeCondition.m_overLastDealSell = (float)m_spinSellOverLastDeal.Value;
            securityRangeTradeCondition.m_lowLastDealBuy = (float)m_spinBuyLowLastDeal.Value;
            securityRangeTradeCondition.m_initBuildPrice = (float)m_spinInitPrice.Value;
            securityRangeTradeCondition.m_initBuildCount = (int)m_spinInitCount.Value;
            securityRangeTradeCondition.m_isCrossBuy = m_chkMultiBuy.Checked;
            securityRangeTradeCondition.m_initBuildFlag = m_chkIsBuild.Checked;
            securityRangeTradeCondition.m_cancelIntervalBuyNoDeal = (int)m_spinBuyCancelSecond.Value;
            securityRangeTradeCondition.m_cleanPrice = (float)m_spinCleanPrice.Value;
            securityRangeTradeCondition.m_isCrossSell = m_chkMultiSell.Checked;
            securityRangeTradeCondition.m_cancelIntervalSellNoDeal = (int)m_spinSellCancelSecond.Value;
            securityRangeTradeCondition.m_isSetEffectiveTransactionTime = m_chkSetEffectiveTransactionTime.Checked;
            securityRangeTradeCondition.m_fromEffectiveTransactionTime = m_txtFromEffectiveTransactionTime.Text;
            securityRangeTradeCondition.m_toEffectiveTransactionTime = m_txtToEffectiveTransactionTime.Text;
            securityRangeTradeCondition.m_intervalChceckTradeStatus = (int)m_spinCheckOrderStatusInte.Value;
            securityRangeTradeCondition.m_securityName = m_securityName;
            securityRangeTradeCondition.m_priceDealBaseLine = "上次成交价";
            bool isAdd = false;
            if (m_securityStrategySetting == null)
            { 
                m_securityStrategySetting = new SecurityStrategySetting();
                isAdd = true;
            }
            m_securityStrategySetting.m_securityCode = securityRangeTradeCondition.m_securityCode;
            m_securityStrategySetting.m_securityName = securityRangeTradeCondition.m_securityName;
            m_securityStrategySetting.m_strategyType = 0;
            m_securityStrategySetting.m_strategySettingInfo = JsonConvert.SerializeObject(securityRangeTradeCondition);
            if(isAdd)
            {
                DataCenter.StrategySettingService.AddStrategySetting(m_securityStrategySetting);
            }
            else
            {
                DataCenter.StrategySettingService.UpdateSecurityStrategySetting(m_securityStrategySetting);
            }
        }
    }
}
