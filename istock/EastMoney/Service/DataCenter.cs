using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using EmSerDataService;
using System.Data;
using EmSocketClient;
using EmCore;
using System.Windows.Forms;
using node;
using System.Threading;
using EmQDataCore;

namespace dataquery
{
    /// <summary>
    /// 全局数据中心
    /// </summary>
    public class DataCenter
    {
        static DataCenter()
        {
            String idFile = DataCenter.GetAppPath() + "\\config\\userid.txt";
            String content = File.ReadAllText(idFile);
            users.AddRange(content.Split(new String[]{"\r\n"},StringSplitOptions.RemoveEmptyEntries));
        }

        private static Random rd = new Random();

        private static List<String> users = new List<String>();

        private static BlockService blockService = new BlockService();

        /// <summary>
        /// 获取或设置证券服务
        /// </summary>
        public static BlockService BlockService
        {
            get { return DataCenter.blockService; }
            set { DataCenter.blockService = value; }
        }

        private static DataQuery dataQuery = new DataQuery();

        /// <summary>
        /// 获取或设置数据连接对象
        /// </summary>
        public static DataQuery DataQuery
        {
            get { return DataCenter.dataQuery; }
            set { DataCenter.dataQuery = value; }
        }

        private static NodeService nodeService = new NodeService();

        public static NodeService NodeService
        {
            get { return DataCenter.nodeService; }
            set { DataCenter.nodeService = value; }
        }

        private static QuoteSequencService quoteSequencService = new QuoteSequencService();

        /// <summary>
        /// 获取或设置行情序列服务
        /// </summary>
        public static QuoteSequencService QuoteSequencService
        {
            get { return DataCenter.quoteSequencService; }
            set { DataCenter.quoteSequencService = value; }
        }

        private static SecurityService securityService = new SecurityService();

        /// <summary>
        /// 获取或设置证券服务
        /// </summary>
        public static SecurityService SecurityService
        {
            get { return DataCenter.securityService; }
            set { DataCenter.securityService = value; }
        }

        /// <summary>
        /// 获取或设置用户ID
        /// </summary>
        public static String UserID
        {
            get { return users[rd.Next(0, users.Count)]; }
        }

        private static UserSecurityService userSecurityService = new UserSecurityService();

        /// <summary>
        /// 获取或设置自选股服务
        /// </summary>
        public static UserSecurityService UserSecurityService
        {
            get { return DataCenter.userSecurityService; }
            set { DataCenter.userSecurityService = value; }
        }

        /// <summary>
        /// 获取程序路径
        /// </summary>
        /// <returns>程序路径</returns>
        public static String GetAppPath()
        {
            return Application.StartupPath;
        }
        
        /// <summary>
        /// 加载数据
        /// </summary>
        public static void Load()
        {
            bool loadAll = false;
            if (MessageBox.Show("是否不加载全部数据以提高进入速度(某些功能将无法使用)?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                loadAll = true;
            }
            SecurityService.Load(loadAll);
            BlockService.Load(loadAll);
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        public static void StartService()
        {
            Load();
            QuoteStart.Start();
            Thread nodeThread = new Thread(new ThreadStart(nodeService.Start));
            nodeThread.Start();
        }

    }
}
