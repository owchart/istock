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
using EastMoney.FM.Web.Data;
using EmMacIndustry.Model.Enum;
using EmMacIndustry.DAL;

namespace dataquery
{
    /// <summary>
    /// �������
    /// </summary>
    public partial class MacIndustyForm : Form
    {
        /// <summary>
        /// ��������
        /// </summary>
        public MacIndustyForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// �����л��ı��¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="e">����</param>
        private void cbTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            MacroDataType type = (MacroDataType)Enum.Parse(typeof(MacroDataType), cbTypes.Text);
            LoadType(type);
        }

        /// <summary>
        /// ������ط���
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="e">����</param>
        private void IndicatorForm_Load(object sender, EventArgs e)
        {
            String[] names = Enum.GetNames(typeof(MacroDataType));
            for (int i = 0; i < names.Length; i++)
            {
                cbTypes.Items.Add(names[i]);
            }
            cbTypes.SelectedIndex = 0;
        }

        /// <summary>
        /// ����ָ��
        /// </summary>
        /// <param name="type">����</param>
        private void LoadType(MacroDataType type)
        {
            tvList.Nodes.Clear();
            DataTable treeData = LocalDataRetriver.GetTreeCategoryTableFromFile(type);
            DataView dv = treeData.DefaultView;
            dv.Sort = "STR_MACROCODE ASC";
            Dictionary<String, TreeNode> nodesMap = new Dictionary<String, TreeNode>();
            foreach (DataRowView dRow in dv)
            {
                DataRow row = dRow.Row;
                String code = row[0].ToString();
                String parentCode = row[1].ToString();
                String name = row[2].ToString();
                if (nodesMap.ContainsKey(parentCode))
                {
                    TreeNode treeNode = new TreeNode(name);
                    treeNode.Name = code;
                    nodesMap[parentCode].Nodes.Add(treeNode);
                    nodesMap[code] = treeNode;
                }
                else
                {
                    TreeNode treeNode = new TreeNode(name);
                    treeNode.Name = code;
                    tvList.Nodes.Add(treeNode);
                    nodesMap[code] = treeNode;
                }
            }
            Console.WriteLine("1");
        }

        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <param name="code"></param>
        /// <param name="macIndustyType"></param>
        /// <returns></returns>
        public static DataTable GetMacIndustyData(String code, String macIndustyType)
        {
            MacroDataType type = (MacroDataType)Enum.Parse(typeof(MacroDataType), macIndustyType);
            string[] ids = new string[] { code };
            DataSet ds = MongoRetriver.GetMutiIndicatorValuesFromService(ids, type);
            DataTable dt = ds.Tables[0];
            return dt;
        }

        /// <summary>
        /// ���ؼ�ѡ���¼�
        /// </summary>
        /// <param name="sender">������</param>
        /// <param name="e">����</param>
        private void tvList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            MacroDataType type = (MacroDataType)Enum.Parse(typeof(MacroDataType), cbTypes.Text);
            TreeNode tn = e.Node;
            if (tn.Nodes.Count == 0)
            {
                if (tn.Tag == null)
                {
                    try
                    {
                        DataTable dt = MongoRetriver.GetDescendantDataByService(tn.Name, type);
                        foreach (DataRow row in dt.Rows)
                        {
                            TreeNode node = new TreeNode(row[3].ToString());
                            node.Tag = "2";
                            node.Name = row[0].ToString();
                            tn.Nodes.Add(node);
                        }
                        tn.Tag = "1";
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else if (tn.Tag == "2")
                {
                    dgvData.DataSource = GetMacIndustyData(tn.Name, cbTypes.Text);
                }
            }
        }
    }
}