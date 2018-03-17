using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib {
	/// <summary>
	/// 
	/// </summary>
	public static class EnumUtility {
		/// <summary>
		/// 由InfoMine值获取中文名称
		/// </summary>
		public static String GetInfoMineString(InfoMineOrg info) {
			switch(info) {
                case InfoMineOrg.News:
					return "新闻";
                case InfoMineOrg.Notice:
					return "公告";
                case InfoMineOrg.Report:
					return "研报";
				default:
					return String.Empty;
			}
		}
		/// <summary>
		/// 由ShortLineType值获取中文名称
		/// </summary>
		public static String GetShortLineTypeString(ShortLineType type) {
			switch(type) {
				case ShortLineType.SurgedLimit:
					return "封涨停板";
				case ShortLineType.DeclineLimit:
					return "封跌停板";
				case ShortLineType.OpenSurgedLimit:
					return "打开涨停";
				case ShortLineType.OpenDeclineLimit:
					return "打开跌停";
				case ShortLineType.BiggerAskOrder:
					return "有大买盘";
				case ShortLineType.BiggerBidOrder:
					return "有大卖盘";
				case ShortLineType.InstitutionAskOrder:
					return "机构买单";
				case ShortLineType.InstitutionBidOrder:
					return "机构卖单";
				case ShortLineType.RocketLaunch:
					return "火箭发射";
				case ShortLineType.StrongRebound:
					return "快速反弹";
				case ShortLineType.HighDiving:
					return "高台跳水";
				case ShortLineType.SpeedupDown:
					return "加速下跌";
				case ShortLineType.CancelBigAskOrder:
					return "买入撤单";
				case ShortLineType.CancelBigBidOrder:
					return "卖出撤单";
				case ShortLineType.InstitutionBidTrans:
					return "大笔卖出";
				case ShortLineType.InstitutionAskTrans:
					return "大笔买入";
				case ShortLineType.MultiSameAskOrders:
					return "买单分单";
				case ShortLineType.MultiSameBidOrders:
					return "卖单分单";
				default:
					return String.Empty;
			}
		}

		/// <summary>
		/// 由字符串转换成shortlinetype
		/// </summary>
		/// <param name="caption"></param>
		/// <returns></returns>
		public static ShortLineType GetShortLineType(String caption) {
			switch(caption) {
				case "封涨停板": return ShortLineType.SurgedLimit;
				case "封跌停板":
					return ShortLineType.DeclineLimit;
				case "打开涨停":
					return ShortLineType.OpenSurgedLimit;
				case "打开跌停":
					return ShortLineType.OpenDeclineLimit;
				case "有大买盘":
					return ShortLineType.BiggerAskOrder;
				case "有大卖盘":
					return ShortLineType.BiggerBidOrder;
				case "机构买单":
					return ShortLineType.InstitutionAskOrder;
				case "机构卖单":
					return ShortLineType.InstitutionBidOrder;
				case "火箭发射":
					return ShortLineType.RocketLaunch;
				case "快速反弹":
					return ShortLineType.StrongRebound;
				case "高台跳水":
					return ShortLineType.HighDiving;
				case "加速下跌":
					return ShortLineType.SpeedupDown;
				case "买入撤单":
					return ShortLineType.CancelBigAskOrder;
				case "卖出撤单":
					return ShortLineType.CancelBigBidOrder;
				case "大笔卖出":
					return ShortLineType.InstitutionBidTrans;
				case "大笔买入":
					return ShortLineType.InstitutionAskTrans;
				case "买单分单":
					return ShortLineType.MultiSameAskOrders;
				case "卖单分单":
					return ShortLineType.MultiSameBidOrders;
				default:
					return ShortLineType.None;
			}
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static String GetEmRatingValueString(EmratingValue type)
        {
            switch (type)
            {
                case EmratingValue.AVoid:
                    return "回避";
                case EmratingValue.Add:
                    return "增持";
                case EmratingValue.Buy:
                    return "买入";
                case EmratingValue.Hold:
                    return "持有";
                case EmratingValue.Neutur:
                    return "中性";
                case EmratingValue.Reduce:
                    return "减持";
                case EmratingValue.Sell:
                    return "卖出";
                default:
                    return String.Empty;
            }
        }
	}
}
