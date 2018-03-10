using System;
using System.Collections.Generic;
using  EmReportWatch.SpecialEntity.Entity;
using EmCore;
using System.Windows.Forms;
using System.Drawing;

namespace EmReportWatch.SpecialCommon
{
    /// <summary>
    /// 公用变量类
    /// </summary>
    public class CommonContant
    {
       
        private static int _objDataIndex;
        /// <summary>
        /// 设置或获取数据序号
        /// </summary>
        public static int ObjDataIndex
        {
            get { return _objDataIndex; }
            set
            {
                _objDataIndex = value;
                //CommonService.Log(_objDataIndex.ToString( ));
            }
        }
        public static Control Control { get; set; }
        public static int  F5F9Type { get; set; }
        /// <summary>
        /// 宏观图形锁
        /// </summary>
        public static object LockEmMacChartControl = new object();
        /// <summary>
        /// 复选框锁
        /// </summary>
        public static object LockMultyNorms = new object();
        /// <summary>
        /// 专题锁
        /// </summary>
        public static object LockModSpecialtopic = new object();
        /// <summary>
        /// 数值锁
        /// </summary>
        public static Object ObjLockCurSerialNumber = new object();
        /// <summary>
        /// 当前专题页锁
        /// </summary>
        public static Object LockCurSpeicalPageInfo = new object();

        /// <summary>
        /// 当前板块锁
        /// </summary>
        public static Object LockBlockInfo = new object();
        /// <summary>
        /// 宏观多图专题图名集合
        /// </summary>
        public static List<string> ChartTitles = new List<string>() { "CPI/PPI", "CPI同比增长", "M1/M2/贷款余额增速比较", "货币回笼/投放", "存贷差与贷存比变化", "固定资产投资增速(%)", "GDP同比增长(%)", "贸易顺逆差与外汇储备", "人民币汇率", "保险公司国债投资比例" };

        /// <summary>
        /// 一次请求数据的大小
        /// </summary>
        public static int RequestCount = 1500;
        /// <summary>
        /// 同名专题
        /// </summary>
        public static Dictionary<string, string> SameSpecialCodes = new Dictionary<string, string>();
        /// <summary>
        /// 请求数据编号
        /// </summary>
        public static int Requestid;
        //public static int PageCount = 11550;
        /// <summary>
        /// 单次请求数据条数
        /// </summary>
        public static int PageGroupCount = 50000;

        /// <summary>
        /// 请求数锁
        /// </summary>
        public static object LockRequestid = new object();

        /// <summary>
        /// 宏观多图容器是否完成
        /// </summary>
        public static bool BlnReadyMacGridChartPannel = false;

        /// <summary>
        /// 专题树键值对
        /// </summary>
        public static Dictionary<string, EBlockCategory> SpecialBlockForSpecialTopicCodes = new Dictionary<string, EBlockCategory>();

        /// <summary>
        /// 相同专题映射
        /// </summary>
        public static Dictionary<string, string> SameSpecials = new Dictionary<string, string>();

        /// <summary>
        /// 红星
        /// </summary>
        public static readonly string RedStar = "红星";

        /// <summary>
        /// 蓝星
        /// </summary>
        public static readonly string BuleStar = "蓝星";

        /// <summary>
        /// 红星字符
        /// </summary>
        public static readonly string Star = "★";

        /// <summary>
        /// 空数据替换字符串
        /// </summary>
        public static readonly string DbNullValue = "--";

        /// <summary>
        /// 数据提取
        /// </summary>
        public static readonly string RemoteData = "提取数据";

        ///// <summary>
        ///// 停止提取
        ///// </summary>
        //  public static readonly string StopRemoteData = "停止提取";
        public static string ModName = null;
        /// <summary>
        /// 柱状
        /// </summary>
        public static readonly string Bar = "柱状";
        //专题控件类型容器,新加的类型都必须在容器存放
        /// <summary>
        /// 专题控件类型容器,新加的类型都必须在容器存放
        /// </summary>
        public static Dictionary<string, Type> Types = new Dictionary<string, Type>();

        public static bool BlnManageComp = false;
        /// <summary>
        /// 是否需要刷新单行转置
        /// </summary>
        public static bool BlnNeedRefreshLineTranspose = false;
        /// <summary>
        /// 当前模块是否初次运行
        /// </summary>
        public static bool BlnFirst = true;
        #region 原Common类的静态变量集合
       

        /// <summary>
        /// 专题收藏夹本地序列化文件存放路径
        /// </summary>
        public static string FavoritesDir = null;
        /// <summary>
        /// 判断当前专题收藏夹文件信息是否有更新
        /// </summary>
        public static List<BindingType> FavFlags = new List<BindingType>();
        //public static bool macFlag = false;
        //  public static List<ComboxItem> ListReportComboxItems = new List<ComboxItem>();
        public static List<string> QueryThreadIds = new List<string>();
        #endregion

        /// <summary>
        /// 系统时间是否准备好
        /// </summary>
        private static bool BlnReadServerDateTime = false;
        private static DateTime _ServerDateTime;
        /// <summary>
        /// 系统时间
        /// </summary>
        public static DateTime ServerDateTime
        {
            get
            {
                if (BlnReadServerDateTime)
                {
                    return _ServerDateTime;

                }
                else
                {
                    _ServerDateTime = EmSerDataService.DataQuery.ServerSysTime;
                    //_ServerDateTime = DateTime.Now.Date;
                    BlnReadServerDateTime = true;
                    return _ServerDateTime;
                }
            }
        }
        /// <summary>
        /// 板块成分详细
        /// </summary>
        public static object BlockDetails;
        /// <summary>
        /// 板块分类code
        /// </summary>
        public static string BlockCode;
        /// <summary>
        /// 板块分类code 明细
        /// </summary>
        public static string BlockCodeDetails;
        /// <summary>
        /// 需要刷新Combox 数据
        /// </summary>
        public static bool BlnUpdateComboxData = true;
        /// <summary>
        /// 需要刷新专题树
        /// </summary>
        public static bool BlnUpdateSpecialTree = true;
        //public static bool BlnBlockQuesting = false;

        public static Dictionary<string, Image> ImageCaches;
        /// <summary>
        /// 初始化时需要选中的专题
        /// </summary>
        public static string TreeCode;
        /// <summary>
        /// 初始化时需要选中的代码
        /// </summary>
        public static string SecCode;
    }
}
