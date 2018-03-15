using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;
using System.Threading;

namespace OwLib
{
    /// <summary>
    /// 数据浏览器
    /// </summary>
    public class IndicatorBrowser
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mainFrame"></param>
        public IndicatorBrowser(MainFrame mainFrame)
        {
            m_mainFrame = mainFrame;
            m_gridIndicatorBrowser = mainFrame.GetGrid("gridIndicatorBrowser");
            m_gridIndicatorBrowser.GridLineColor = COLOR.EMPTY;
            m_gridIndicatorBrowser.BackColor = COLOR.ARGB(0, 0, 0);
            m_gridIndicatorBrowser.RowStyle = new GridRowStyle();
            m_gridIndicatorBrowser.RowStyle.BackColor = COLOR.ARGB(0, 0, 0);
            m_tvIndicatorBrowser = mainFrame.GetTree("tvIndicatorBrowser");
            m_tvIndicatorBrowser.BackColor = COLOR.ARGB(0, 0, 0);
            m_tvIndicatorBrowser.ForeColor = COLOR.ARGB(255, 255, 255);
            m_tvIndicatorBrowser.RowStyle = new GridRowStyle();
            m_tvIndicatorBrowser.RowStyle.BackColor = COLOR.ARGB(0, 0, 0);
            m_tvIndicatorBrowser.RegisterEvent(new GridCellMouseEvent(GridCellClick), EVENTID.GRIDCELLCLICK);
            LoadType(BrowserType.STOCK);
        }

        /// <summary>
        /// 数据表格
        /// </summary>
        private GridA m_gridIndicatorBrowser;

        /// <summary>
        /// 指标树
        /// </summary>
        private TreeA m_tvIndicatorBrowser;

        private MainFrame m_mainFrame;

        /// <summary>
        /// 获取或设置主框架
        /// </summary>
        public MainFrame MainFrame
        {
            get { return m_mainFrame; }
            set { m_mainFrame = value; }
        }

        /// <summary>
        /// 获取指标数据
        /// </summary>
        /// <param name="indicatorCode"></param>
        /// <param name="blockType"></param>
        /// <param name="codes"></param>
        /// <returns></returns>
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
                IndicatorDataPacket2 indicatorDataPacket = new IndicatorDataPacket2(CommonEnumerators.IndicatorRequestType.Fun, stocks, ids);
                if (blockType == "Block")
                {
                    indicatorDataPacket = new IndicatorDataPacket2(CommonEnumerators.IndicatorRequestType.Blk, stocks, ids);
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
                        datas.Add(new IndicatorItemData(codesMap[key], _doubleIndicatorDict[key]));
                    }
                }
                foreach (String key in _floatIndicatorDict.Keys)
                {
                    if (codesMap.ContainsKey(key))
                    {
                        datas.Add(new IndicatorItemData(codesMap[key], _floatIndicatorDict[key]));
                    }
                }
                foreach (String key in _intIndicatorDict.Keys)
                {
                    if (codesMap.ContainsKey(key))
                    {
                        datas.Add(new IndicatorItemData(codesMap[key], _intIndicatorDict[key]));
                    }
                }
                foreach (String key in _longIndicatorDict.Keys)
                {
                    if (codesMap.ContainsKey(key))
                    {
                        datas.Add(new IndicatorItemData(codesMap[key], _longIndicatorDict[key]));
                    }
                }
                codesMap.Clear();
            }
            return rootData;
        }

        /// <summary>
        /// 单元格双击事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="cell">单元格</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮值</param>
        private void GridCellClick(object sender, GridCell cell, POINT mp, MouseButtonsA button, int clicks, int delta)
        {
            TreeNodeA tn = cell as TreeNodeA;
            if (tn.m_nodes.Count == 0)
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
                        Dictionary<String, TreeNodeA> nodesMap = new Dictionary<String, TreeNodeA>();
                        int nodesSzie = nodes.Count;
                        for (int i = 0; i < nodesSzie; i++)
                        {
                            NodeData node = nodes[i];
                            TreeNodeA subTn = new TreeNodeA();
                            subTn.Text = node.Name;
                            subTn.Style = new GridCellStyle();
                            subTn.Style.ForeColor = COLOR.ARGB(255, 255, 80);
                            subTn.Style.Font = new FONT("微软雅黑", 14, true, false, false);
                            subTn.Name = node.Id;
                            subTn.Tag = node;
                            if (nodesMap.ContainsKey(node.ParentId))
                            {
                                nodesMap[node.ParentId].AppendNode(subTn);
                                nodesMap[node.Id] = subTn;
                            }
                            else
                            {
                                tn.AppendNode(subTn);
                                nodesMap[node.Id] = subTn;
                            }
                            IndicatorEntityDataPacket entity = new IndicatorEntityDataPacket(node.Id);
                            ConnectManager.CreateInstance().Request(entity);
                        }
                        tn.ExpendAll();
                        m_tvIndicatorBrowser.Update();
                        m_tvIndicatorBrowser.Invalidate();
                    }
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    List<String> codes = new List<String>();
                    SecurityService.GetCodes(codes);
                    int codesSize = codes.Count;
                    for (int i = 0; i < codesSize; i++)
                    {
                        sb.Append(codes[i]);
                        if (i != codesSize - 1)
                        {
                            sb.Append(",");
                        }
                    }
                    m_gridIndicatorBrowser.ClearRows();
                    IndicatorRootData data = GetIndicatorData(nodeData.Id, "Stock", sb.ToString());
                    m_gridIndicatorBrowser.GetColumn("colN3").Text = data.categoryName;
                    foreach (IndicatorItemData indicatorItem in data.items)
                    {
                        GridRow row = new GridRow();
                        m_gridIndicatorBrowser.AddRow(row);
                        GridStringCell codeCell = new GridStringCell(indicatorItem.code);
                        row.AddCell("colN1", codeCell);
                        GSecurity security = new GSecurity();
                        SecurityService.GetSecurityByCode(indicatorItem.code, ref security);
                        row.AddCell("colN2", new GridStringCell(security.m_name));
                        if (indicatorItem.type == 0)
                        {
                            GridStringCell valueCell = new GridStringCell(indicatorItem.text);
                            row.AddCell("colN3", valueCell);
                        }
                        else if (indicatorItem.type == 1)
                        {
                            GridDoubleCell valueCell = new GridDoubleCell(indicatorItem.num);
                            row.AddCell("colN3", valueCell);
                        }
                        row.GetCell("colN1").Style = new GridCellStyle();
                        row.GetCell("colN1").Style.ForeColor = COLOR.ARGB(255, 255, 80);
                        row.GetCell("colN1").Style.Font = new FONT("微软雅黑", 14, true, false, false);
                        row.GetCell("colN1").Style.Font = new FONT("微软雅黑", 14, true, false, false);
                        row.GetCell("colN2").Style = new GridCellStyle();
                        row.GetCell("colN2").Style.ForeColor = COLOR.ARGB(255, 80, 80);
                        row.GetCell("colN2").Style.Font = new FONT("微软雅黑", 14, true, false, false);
                        row.GetCell("colN3").Style = new GridCellStyle();
                        row.GetCell("colN3").Style.ForeColor = COLOR.ARGB(80, 255, 255);
                        row.GetCell("colN3").Style.Font = new FONT("微软雅黑", 14, true, false, false);
                    }
                    m_gridIndicatorBrowser.Update();
                    m_gridIndicatorBrowser.Invalidate();
                }
            }
        }

        /// <summary>
        /// 加载指标
        /// </summary>
        /// <param name="type">类型</param>
        private void LoadType(BrowserType type)
        {
            m_tvIndicatorBrowser.ClearNodes();
            m_tvIndicatorBrowser.Update();
            //请求所有指标
            List<String> list = new List<String>();
            IndicatorCategoryDataPacket packet = new IndicatorCategoryDataPacket(type);
            ConnectManager.CreateInstance().Request(packet);
            while (packet.ReserveFlag == 0)
            {
                Thread.Sleep(100);
            }
            List<NodeData> nodes = DataCore.CreateInstance().GetCategoryList(type);
            Dictionary<String, TreeNodeA> nodesMap = new Dictionary<String, TreeNodeA>();
            if (nodes != null)
            {
                int nodesSzie = nodes.Count;
                for (int i = 0; i < nodesSzie; i++)
                {
                    NodeData node = nodes[i];
                    TreeNodeA tn = new TreeNodeA();
                    tn.Text = node.Name;
                    tn.Style = new GridCellStyle();
                    tn.Style.ForeColor = COLOR.ARGB(255, 255, 255);
                    tn.Style.Font = new FONT("微软雅黑", 14, true, false, false);
                    tn.Name = node.Id;
                    tn.Tag = node;
                    if (nodesMap.ContainsKey(node.ParentId))
                    {
                        nodesMap[node.ParentId].AppendNode(tn);
                        nodesMap[node.Id] = tn;
                    }
                    else
                    {
                        m_tvIndicatorBrowser.AppendNode(tn);
                        nodesMap[node.Id] = tn;
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

        public IndicatorItemData(String code, double num)
        {
           this.code = code;
           this.num = num;
           this.type = 1;
        }
        public String code;
        public String text;
        public double num;
        public int type;
    }
}
