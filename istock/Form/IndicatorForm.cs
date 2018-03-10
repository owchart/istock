using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using dataquery.indicator;
using System.IO;
using EmCore.Utils;
using Newtonsoft.Json;

namespace dataquery
{
    /// <summary>
    /// 数据浏览器窗体
    /// </summary>
    public partial class IndicatorForm : Form
    {
        /// <summary>
        /// 创建窗体
        /// </summary>
        public IndicatorForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 板块明细列表
        /// </summary>
        private Dictionary<String, List<DMBlockDetailItem>> blockDetailItems;

        /// <summary>
        /// 类型切换改变事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private void cbTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            BrowserType type = (BrowserType)Enum.Parse(typeof(BrowserType), cbTypes.Text);
            LoadType(type);
        }

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
            String[] names = Enum.GetNames(typeof(BrowserType));
            for (int i = 0; i < names.Length; i++)
            {
                cbTypes.Items.Add(names[i]);
            }
            cbTypes.SelectedIndex = 0;
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

        /// <summary>
        /// 加载指标
        /// </summary>
        /// <param name="type">类型</param>
        private void LoadType(BrowserType type)
        {
            tvList.Nodes.Clear();
            //请求所有指标
            List<String> list = new List<String>();
            IndicatorCategoryDataPacket packet = new IndicatorCategoryDataPacket(type);
            ConnectManager.CreateInstance().Request(packet);
            while (packet.ReserveFlag == 0)
            {
                Thread.Sleep(100);
            }
            List<NodeData> nodes = DataCore.CreateInstance().GetCategoryList(type);
            Dictionary<String, TreeNode> nodesMap = new Dictionary<String, TreeNode>();
            if (nodes != null)
            {
                int nodesSzie = nodes.Count;
                for (int i = 0; i < nodesSzie; i++)
                {
                    NodeData node = nodes[i];
                    TreeNode tn = new TreeNode(node.Name);
                    tn.Name = node.Id;
                    tn.Tag = node;
                    if (nodesMap.ContainsKey(node.ParentId))
                    {
                        nodesMap[node.ParentId].Nodes.Add(tn);
                        nodesMap[node.Id] = tn;
                    }
                    else
                    {
                        tvList.Nodes.Add(tn);
                        nodesMap[node.Id] = tn;
                    }
                }
            }
        }

        public static IndicatorRootData GetIndicatorData(String indicatorCode, String blockType, String codes)
        {
            if (!IndicatorDataCore._IndicatorEntityDict.ContainsKey(indicatorCode))
            {
                IndicatorEntityDataPacket newEntity = new IndicatorEntityDataPacket(indicatorCode);
                ConnectManager.CreateInstance().Request(newEntity);
                int tick2 = 0;
                while (newEntity.ReserveFlag == 0 && tick2 < 100)
                {
                    tick2++;
                    Thread.Sleep(100);
                }
            }
            IndicatorRootData rootData = new IndicatorRootData();
            List<IndicatorItemData> datas = new List<IndicatorItemData>();
            rootData.items = datas;
            String[] strs = codes.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            int strsSize = strs.Length;
            IndicatorEntity entity = IndicatorDataCore.GetIndicatorEntityByCategoryCode(indicatorCode);
            if (entity != null)
            {
                if (entity.Parameters != null)
                {
                    entity.Parameters = entity.Parameters.Replace("EmDataPara.", "dataquery.");
                    entity.Parameters = entity.Parameters.Replace("\"DefaultValue\":\"2013\"", "\"DefaultValue\":\"2016\"");
                    entity.Parameters = entity.Parameters.Replace("\"DefaultValue\":\"2014-04-21\"", "\"DefaultValue\":\"2017-03-27\"");
                }
                List<int> ids = new List<int>();
                ids.Add((int)entity.ID);
                Dictionary<String, String> codesMap = new Dictionary<String, String>();
                List<StockEntity> stocks = new List<StockEntity>();
                String idStr = entity.ID.ToString();
                for (int i = 0; i < strsSize; i++)
                {
                    StockEntity stock = new StockEntity();
                    String[] subStrs = strs[i].Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    stock.StockCode = subStrs[0];
                    stocks.Add(stock);
                    codesMap[subStrs[0] + idStr] = subStrs[0];
                }
                IndicatorDataCore.SetCustomerIndicatorEntity((int)entity.ID, entity);
                IndicatorDataPacket indicatorDataPacket = new IndicatorDataPacket(CommonEnumerators.IndicatorRequestType.Fun, stocks, ids);
                if (blockType == "Block")
                {
                    indicatorDataPacket = new IndicatorDataPacket(CommonEnumerators.IndicatorRequestType.Blk, stocks, ids);
                }
                ConnectManager.CreateInstance().Request(indicatorDataPacket);
                int tick = 0;
                while (indicatorDataPacket.ReserveFlag == 0 && tick < 100)
                {
                    tick++;
                    Thread.Sleep(100);
                }
                Dictionary<String, double> _doubleIndicatorDict = IndicatorTableDataCore.DoubleIndicatorDict;
                Dictionary<String, float> _floatIndicatorDict = IndicatorTableDataCore.FloatIndicatorDict;
                Dictionary<String, int> _intIndicatorDict = IndicatorTableDataCore.Int32IndicatorDict;
                Dictionary<String, long> _longIndicatorDict = IndicatorTableDataCore.LongIndicatorDict;
                Dictionary<String, String> _strIndicatorDict = IndicatorTableDataCore.StrIndicatorDict;
                StringBuilder sb = new StringBuilder();
                rootData.categoryName = entity.CategoryName;
                rootData.categoryCode = entity.CategoryCode;
                if (entity.Parameters != null)
                {
                    rootData.parameters = entity.Parameters;
                    sb.Append("参数：" + entity.Parameters + "\r\n");
                }
                foreach (String key in _strIndicatorDict.Keys)
                {
                    if (codesMap.ContainsKey(key))
                    {
                        datas.Add(new IndicatorItemData(codesMap[key], _strIndicatorDict[key]));
                    }
                }
                foreach (String key in _doubleIndicatorDict.Keys)
                {
                    if (codesMap.ContainsKey(key))
                    {
                        datas.Add(new IndicatorItemData(codesMap[key], _strIndicatorDict[key]));
                    }
                }
                foreach (String key in _floatIndicatorDict.Keys)
                {
                    if (codesMap.ContainsKey(key))
                    {
                        datas.Add(new IndicatorItemData(codesMap[key], _strIndicatorDict[key]));
                    }
                }
                foreach (String key in _intIndicatorDict.Keys)
                {
                    if (codesMap.ContainsKey(key))
                    {
                        datas.Add(new IndicatorItemData(codesMap[key], _strIndicatorDict[key]));
                    }
                }
                foreach (String key in _longIndicatorDict.Keys)
                {
                    if (codesMap.ContainsKey(key))
                    {
                        datas.Add(new IndicatorItemData(codesMap[key], _strIndicatorDict[key]));
                    }
                }
                codesMap.Clear();
            }
            return rootData;
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
                NodeData nodeData = tn.Tag as NodeData;
                if (nodeData.IsCatalog)
                {
                    IndicatorLeafDataPacket leafPacket = new IndicatorLeafDataPacket(nodeData.Id);
                    ConnectManager.CreateInstance().Request(leafPacket);
                    int tick = 0;
                    while (leafPacket.ReserveFlag == 0 && tick < 50)
                    {
                        Thread.Sleep(100);
                        tick++;
                    }
                    if (leafPacket.LeafNodeList.Count > 0)
                    {
                        List<NodeData> nodes = leafPacket.LeafNodeList;
                        Dictionary<String, TreeNode> nodesMap = new Dictionary<String, TreeNode>();
                        int nodesSzie = nodes.Count;
                        for (int i = 0; i < nodesSzie; i++)
                        {
                            NodeData node = nodes[i];
                            TreeNode subTn = new TreeNode(node.Name);
                            subTn.Name = node.Id;
                            subTn.Tag = node;
                            if (nodesMap.ContainsKey(node.ParentId))
                            {
                                nodesMap[node.ParentId].Nodes.Add(subTn);
                                nodesMap[node.Id] = subTn;
                            }
                            else
                            {
                                tn.Nodes.Add(subTn);
                                nodesMap[node.Id] = subTn;
                            }
                            IndicatorEntityDataPacket entity = new IndicatorEntityDataPacket(node.Id);
                            ConnectManager.CreateInstance().Request(entity);
                        }
                        tn.Expand();
                    }
                }
                else
                {
                    rtbResult.Text = JsonConvert.SerializeObject(GetIndicatorData(nodeData.Id, cbTypes.Text, rtbCodes.Text));
                }
            }
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
                if (cbTypes.Text == "Block")
                {
                    if (String.IsNullOrEmpty(rtbCodes.Text))
                    {
                        rtbCodes.Text = blockItem.code;
                    }
                    else
                    {
                        rtbCodes.Text += "," + blockItem.code;
                    }
                }
                else
                {
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


    public class IndicatorRootData
    {
        public String categoryCode;
        public String categoryName;
        public String parameters;
        public List<IndicatorItemData> items;
    }

    public class IndicatorItemData
    {
        public IndicatorItemData(String code, String text)
        {
            this.code = code;
            this.text = text;
        }
        public String code;
        public String text;
    }
}