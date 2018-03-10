using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using EmCore;
using System.Xml;
using Newtonsoft.Json;

namespace OwLib
{
    /// <summary>
    /// 专题统计窗体
    /// </summary>
    public partial class SpecialForm : Form
    {
        /// <summary>
        /// 创建窗体
        /// </summary>
        public SpecialForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 板块明细列表
        /// </summary>
        private Dictionary<String, List<DMBlockDetailItem>> blockDetailItems;

        /// <summary>
        /// 窗体加载方法
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private void IndicatorForm_Load(object sender, EventArgs e)
        {
            LoadBlocks();
        }

        /// <summary>
        /// 加载板块
        /// </summary>
        private void LoadBlocks()
        {
            DataTable dt = DataCache.QuerySpeicalTopicCatory();
            DataTable dt2 = DataCache.QuerySpecialTopic();
            Dictionary<String, TreeNode> nodesMap2 = new Dictionary<String, TreeNode>();
            int nodesSzie = dt.Rows.Count;
            for (int i = 0; i < nodesSzie; i++)
            {
                DataRow node = dt.Rows[i];
                String id = node[0].ToString();
                string text = node[1].ToString();
                String parentID = node[2].ToString();
                TreeNode tn2 = new TreeNode(id);
                tn2.Name = id;
                tn2.Text = text;
                if (nodesMap2.ContainsKey(parentID))
                {
                    nodesMap2[parentID].Nodes.Add(tn2);
                    nodesMap2[id] = tn2;
                }
                else
                {
                    tvList.Nodes.Add(tn2);
                    nodesMap2[id] = tn2;
                }
            }
            List<DMBlockItem> blocks = BlockService.Blocks;
            int blocksSize = blocks.Count;
            Dictionary<String, TreeNode> nodesMap = new Dictionary<String, TreeNode>();
            Dictionary<String, String> roots = new Dictionary<String, String>();
            TreeNode tn = new TreeNode("沪深股票");
            tvBlocks.Nodes.Add(tn);
            nodesMap["1"] = tn;
            for (int i = 0; i < blocksSize; i++)
            {
                DMBlockItem blockItem = blocks[i];
                if (blockItem.name == "沪深股票")
                {
                    continue;
                }
                if (nodesMap.ContainsKey(blockItem.parentcode))
                {
                    TreeNode treeNode = new TreeNode(blockItem.name);
                    treeNode.Tag = blockItem;
                    nodesMap[blockItem.parentcode].Nodes.Add(treeNode);
                    nodesMap[blockItem.code] = treeNode;
                }
                else
                {
                    TreeNode treeNode = new TreeNode(blockItem.name);
                    treeNode.Tag = blockItem;
                    tvBlocks.Nodes.Add(treeNode);
                    nodesMap[blockItem.code] = treeNode;
                }
            }
            blockDetailItems = BlockService.BlockDetails;
            nodesMap.Clear();
            System.GC.Collect();
        }

        public string QuerySpecialCodeInfos(string id)
        {
            byte[] bytes = DataAccess.IDataQuery.NewQueryGlobalData("1009◎UID◎SpecialService◎◎◎5,-1," + id) as byte[];
            string result = Encoding.UTF8.GetString(bytes);
            return result;
        }

        public static string QuerySpecialXmlData(string pid, string version)
        {
            string path = DataCenter.GetAppPath() + @"\SpecialXmlData\";
            Directory.CreateDirectory(path);
            path = path + pid + ".xml";
            try
            {
                object obj2 = DataAccess.IDataQuery.NewQueryGlobalData("1006◎UID◎SpecialService◎◎◎2," + version + "," + pid);
                string data = Encoding.UTF8.GetString(obj2 as byte[]);
                if (!data.Trim().StartsWith("<"))
                {
                    data = data.Trim().Remove(0, 1);
                }
                if (data.StartsWith("\r\n"))
                {
                    data = data.Remove(0, 2);
                }
                WriteXMlFile(path, null, data);
                return data;
            }
            catch (Exception ex)
            {
                if (File.Exists(path))
                {
                    return ReadLocaXml(path);
                }
                else
                {
                    return null;
                }
            }
        }
        

        private static void WriteXMlFile(string filename, string version, string data)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                StreamWriter writer1 = new StreamWriter(stream, Encoding.UTF8);
                writer1.Write(data);
                writer1.Close();
            }
        }

        public static string QuerySpecialTreeData(string pid)
        {
            string path = DataCenter.GetAppPath() + @"\SpecialXmlData\";
            Directory.CreateDirectory(path);
            path = path + pid;
            try
            {
                object obj2 = DataAccess.IDataQuery.NewQueryGlobalData("1005◎UID◎SpecialService◎◎◎1," + pid);
                string data = Encoding.UTF8.GetString(obj2 as byte[]);
                //WriteXMlFile(path, null, data);
                return data;
            }
            catch (Exception ex)
            {
                return null;
                if (File.Exists(path))
                {
                    return ReadLocaXml(path);
                }
                else
                {
                    return null;
                }
            }
        }

        public static string ReadLocaXml(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
            {
                return new StreamReader(stream, Encoding.UTF8).ReadToEnd();
            }
        }

        public static DataSet QuerySpecialIndicator(String specialCode)
        {
            String treeDate = QuerySpecialTreeData(specialCode);
            List<BindingType> bindingType = Deserialize(treeDate);
            int bindingTypeSize = bindingType.Count;
            StringBuilder sb = new StringBuilder();
            DataSet ds2 = new DataSet();
            for (int i = 0; i < bindingTypeSize; i++)
            {
                String xmlData = QuerySpecialXmlData(bindingType[i].ID, "");
                try
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xmlData);
                    XmlNode elementNode = xmlDoc.DocumentElement;
                    XmlNodeList DataSourceNameNodes = elementNode.SelectNodes("//Table//Table//Component//SCGridGontrol//GridColumnsAttribute");
                    foreach (XmlNode DataSourceNameNode in DataSourceNameNodes)
                    {
                        String dataSourceName = "", statisticsEngName = "", dataSourceCode = "";
                        foreach (XmlNode subNode in DataSourceNameNode.ChildNodes)
                        {
                            String name = subNode.Name;
                            String value = subNode.InnerText;
                            if (name == "DataSourceName")
                            {
                                dataSourceName = value;
                            }
                            else if (name == "StatisticsEngName")
                            {
                                statisticsEngName = value;
                            }
                            else if (name == "DataSourceCode")
                            {
                                dataSourceCode = value;
                            }
                        }
                        if (dataSourceName != null && dataSourceName.Length > 0)
                        {
                            List<DataTransmission> dataTrans = new List<DataTransmission>();
                            DataTransmission dt = new DataTransmission();
                            dt.DataTableName = dataSourceName;
                            dt.DataSource = dataSourceName;
                            dataTrans.Add(dt);
                            DataSet ds = DataAccess.IDataQuery.Query(dataTrans);
                            DataTable dt2 = ds.Tables[0];
                            ds.Tables.Remove(dt2);
                            ds2.Tables.Add(dt2);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return ds2;
        }

        /// <summary>
        /// 数控件选中事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private void tvList_AfterSelect(object sender, TreeViewEventArgs e)
        {
             TreeNode tn = e.Node;
             if (tn.Nodes.Count == 0)
             {
                 if (tn.Tag == null)
                 {
                     String id = tn.Name;
                     String treeDate = QuerySpecialTreeData(id);
                     List<BindingType> bindingType = Deserialize(treeDate);
                     for (int i = 0; i < bindingType.Count; i++)
                     {
                         TreeNode node = new TreeNode(bindingType[i].Name);
                         node.Name = bindingType[i].ID;
                         tn.Nodes.Add(node);
                         node.Tag = "2";
                         node.ExpandAll();
                     }
                     tn.Tag = "1";
                 }
                 else if (tn.Tag == "2")
                 {
                     String[] codes = rtbCodes.Text.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                     Dictionary<String, String> dicCodes = new Dictionary<string, string>();
                     foreach (String code in codes)
                     {
                         dicCodes[code] = "";
                     }
                     rtbResults.Text = JsonConvert.SerializeObject(QuerySpecialIndicator(tn.Name));
                 }
             }
        }

        private static List<BindingType> Deserialize(string data)
        {
            List<BindingType> list = new List<BindingType>();
            char[] separator = new char[] { '}' };
            string[] strArray = data.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strArray.Length; i++)
            {
                char[] chArray2 = new char[] { '◎' };
                string[] arr = strArray[i].Split(chArray2);
                BindingType item = new BindingType();
                item.ID = arr[0];
                item.Name = arr[1];
                item.ParentID = arr[2];
                item.Category = arr[3];
                if (arr[4] != string.Empty)
                {
                    item.OrderNum = int.Parse(arr[4]);
                }
                ModSpecialtopic specialtopic = ArrToModSpecialtopic(arr);
                item.Entity = specialtopic;
                if (item.ParentID != "9999")
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public static ModSpecialtopic ArrToModSpecialtopic(string[] arr)
        {
            ModSpecialtopic specialtopic = new ModSpecialtopic();
            specialtopic.ID = arr[0];
            specialtopic.SpecialTopicCode = string.IsNullOrEmpty(arr[8]) ? arr[0] : arr[8];
            specialtopic.SpecialTopicName = arr[1];
            specialtopic.CateGoryCode = string.IsNullOrEmpty(arr[9]) ? arr[2] : arr[9];
            if (arr[6] != string.Empty)
            {
                specialtopic.IsImportant = int.Parse(arr[6]);
            }
            if (arr[5] != string.Empty)
            {
                specialtopic.IsShowBlock = int.Parse(arr[5]);
            }
            if (arr[4] != string.Empty)
            {
                specialtopic.SortCode = int.Parse(arr[4]);
            }
            if (arr[7] != string.Empty)
            {
                specialtopic.Version = string.IsNullOrEmpty(arr[11]) ? int.Parse(arr[7]) : int.Parse(arr[11]);
            }
            specialtopic.HlepUrl = arr[10];
            return specialtopic;
        }

        /// <summary>
        /// 选中板块树事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private void tvBlocks_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode tn = e.Node;
            if (tn.Nodes.Count == 0)
            {
                DMBlockItem blockItem = tn.Tag as DMBlockItem;
                if (blockDetailItems.ContainsKey(blockItem.code))
                {
                    List<DMBlockDetailItem> items = blockDetailItems[blockItem.code];
                    StringBuilder sb = new StringBuilder();
                    int itemsSize = items.Count;
                    for (int i = 0; i < itemsSize; i++)
                    {
                        sb.Append(items[i].code);
                        if (i != itemsSize - 1)
                        {
                            sb.Append(",");
                        }
                    }
                    rtbCodes.Text = sb.ToString();
                }
            }
        }
    }
}