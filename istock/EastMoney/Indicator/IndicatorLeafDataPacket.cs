using System;
using System.Collections.Generic;
using System.Text;

namespace dataquery.indicator
{
    public class IndicatorLeafDataPacket : DataPacketBase
    {
        private List<NodeData> _nodeList;
        private String _pCategoryCode;

        public IndicatorLeafDataPacket(String pCategoryCode)
        {
            base.RequestId = RequestType.IndicatorLeaf;
            this._pCategoryCode = pCategoryCode;
        }

        public override String Coding()
        {
            return String.Format("1002◎UniqueId◎IndicatorService◎[PreVersion]◎[NowVersion]◎8,{0}", this._pCategoryCode);
        }

        public override bool Decoding(byte[] bytes)
        {
            if (((bytes != null) && (bytes.Length > 0)) && bytes[0].ToString().Equals("48"))
            {
                return false;
            }
            bool flag = true;
            try
            {
                List<NodeData> list = new List<NodeData>();
                foreach (String str2 in Encoding.UTF8.GetString(bytes).Split(new char[] { '}' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    String[] strArray2 = str2.Split(new char[] { '◎' }, StringSplitOptions.RemoveEmptyEntries);
                    NodeData data2 = new NodeData();
                    data2.Id = strArray2[6];
                    data2.ParentId = strArray2[1];
                    data2.Name = strArray2[2];
                    data2.SortIndex = int.Parse(strArray2[3]);
                    data2.IsCatalog = !strArray2[4].Equals("0");
                    NodeData item = data2;
                    list.Add(item);
                }
                this._nodeList = list;
            }
            catch (Exception exception)
            {
                //LogUtility.LogMessage("获取指标分类解包失败," + exception.ToString());
                flag = false;
            }
            return flag;
        }

        public List<NodeData> LeafNodeList
        {
            get
            {
                return this._nodeList;
            }
        }

        public String PCategoryCode
        {
            get
            {
                return this._pCategoryCode;
            }
        }
    }
}
