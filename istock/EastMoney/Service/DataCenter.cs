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
    /// ȫ����������
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
        /// ��ȡ������֤ȯ����
        /// </summary>
        public static BlockService BlockService
        {
            get { return DataCenter.blockService; }
            set { DataCenter.blockService = value; }
        }

        private static DataQuery dataQuery = new DataQuery();

        /// <summary>
        /// ��ȡ�������������Ӷ���
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
        /// ��ȡ�������������з���
        /// </summary>
        public static QuoteSequencService QuoteSequencService
        {
            get { return DataCenter.quoteSequencService; }
            set { DataCenter.quoteSequencService = value; }
        }

        private static SecurityService securityService = new SecurityService();

        /// <summary>
        /// ��ȡ������֤ȯ����
        /// </summary>
        public static SecurityService SecurityService
        {
            get { return DataCenter.securityService; }
            set { DataCenter.securityService = value; }
        }

        /// <summary>
        /// ��ȡ�������û�ID
        /// </summary>
        public static String UserID
        {
            get { return users[rd.Next(0, users.Count)]; }
        }

        private static UserSecurityService userSecurityService = new UserSecurityService();

        /// <summary>
        /// ��ȡ��������ѡ�ɷ���
        /// </summary>
        public static UserSecurityService UserSecurityService
        {
            get { return DataCenter.userSecurityService; }
            set { DataCenter.userSecurityService = value; }
        }

        /// <summary>
        /// ��ȡ����·��
        /// </summary>
        /// <returns>����·��</returns>
        public static String GetAppPath()
        {
            return Application.StartupPath;
        }
        
        /// <summary>
        /// ��������
        /// </summary>
        public static void Load()
        {
            bool loadAll = false;
            if (MessageBox.Show("�Ƿ񲻼���ȫ����������߽����ٶ�(ĳЩ���ܽ��޷�ʹ��)?", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                loadAll = true;
            }
            SecurityService.Load(loadAll);
            BlockService.Load(loadAll);
        }

        /// <summary>
        /// ��������
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
