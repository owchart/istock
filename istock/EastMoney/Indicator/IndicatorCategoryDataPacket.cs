using System;
using System.Collections.Generic;
using System.Text;

namespace dataquery.indicator
{
    public class IndicatorCategoryDataPacket : DataPacketBase
    {
        private BrowserType _browser;
        private String _pCategoryCode;

        public IndicatorCategoryDataPacket(BrowserType browserType)
        {
            base.RequestId = RequestType.IndicatorCategory;
            this._browser = browserType;
        }

        public IndicatorCategoryDataPacket(String pCategoryCode)
        {
            base.RequestId = RequestType.IndicatorCategory;
            this._pCategoryCode = pCategoryCode;
        }

        public override string Coding()
        {
            switch (this.Browser)
            {
                case BrowserType.STOCK:
                    this._pCategoryCode = "101";
                    break;

                case BrowserType.FUND:
                case BrowserType.BankFinancial:
                case BrowserType.BrokerageFinancial:
                case BrowserType.InsuranceFinancial:
                case BrowserType.SunshinePrivate:
                case BrowserType.TrustFinancial:
                    this._pCategoryCode = "102";
                    break;

                case BrowserType.BOND:
                    this._pCategoryCode = "103";
                    break;

                case BrowserType.FUTURE:
                    this._pCategoryCode = "104";
                    break;

                case BrowserType.BLOCKSEQUENCE:
                    this._pCategoryCode = "110";
                    break;

                case BrowserType.INDEX:
                    this._pCategoryCode = "105";
                    break;

                case BrowserType.Option:
                    this._pCategoryCode = "108";
                    break;

                case BrowserType.New3Board:
                    this._pCategoryCode = "101";
                    break;

                case BrowserType.Combination:
                    this._pCategoryCode = "115";
                    break;

                case BrowserType.Block:
                    this._pCategoryCode = "107";
                    break;
            }
            return string.Format("1003◎UniqueId◎IndicatorService◎[PreVersion]◎[NowVersion]◎3,{0}", this._pCategoryCode);
        }

        public override bool Decoding(byte[] bytes)
        {
            if (((bytes != null) && (bytes.Length != 0)) && bytes[0].ToString().Equals("48"))
            {
                return false;
            }
            bool flag = true;
            try
            {
                List<NodeData> nodeList = new List<NodeData>();
                char[] separator = new char[] { '}' };
                string[] strArray = Encoding.UTF8.GetString(bytes).Split(separator, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < strArray.Length; i++)
                {
                    NodeData data1;
                    char[] chArray2 = new char[] { '◎' };
                    string[] strArray2 = strArray[i].Split(chArray2, StringSplitOptions.RemoveEmptyEntries);
                    if (this.Browser == BrowserType.STOCK)
                    {
                        if (!strArray2[0].StartsWith("101004"))
                        {
                            goto Label_015E;
                        }
                        continue;
                    }
                    if (this.Browser == BrowserType.New3Board)
                    {
                        if (strArray2[0].StartsWith("101004"))
                        {
                            goto Label_015E;
                        }
                        continue;
                    }
                    if (this.Browser == BrowserType.BLOCKSEQUENCE)
                    {
                        if (strArray2[0].StartsWith("110009"))
                        {
                            goto Label_015E;
                        }
                        continue;
                    }
                    if (this.Browser == BrowserType.BrokerageFinancial)
                    {
                        if (strArray2[0].StartsWith("102002"))
                        {
                            goto Label_015E;
                        }
                        continue;
                    }
                    if (this.Browser == BrowserType.SunshinePrivate)
                    {
                        if (strArray2[0].StartsWith("102003"))
                        {
                            goto Label_015E;
                        }
                        continue;
                    }
                    if (this.Browser == BrowserType.InsuranceFinancial)
                    {
                        if (strArray2[0].StartsWith("102004"))
                        {
                            goto Label_015E;
                        }
                        continue;
                    }
                    if (this.Browser == BrowserType.BankFinancial)
                    {
                        if (strArray2[0].StartsWith("102005"))
                        {
                            goto Label_015E;
                        }
                        continue;
                    }
                    if ((this.Browser == BrowserType.TrustFinancial) && !strArray2[0].StartsWith("102006"))
                    {
                        continue;
                    }
                Label_015E:
                    data1 = new NodeData();
                    data1.Id = strArray2[0];
                    data1.ParentId = strArray2[1];
                    data1.Name = strArray2[2];
                    data1.SortIndex = int.Parse(strArray2[3]);
                    data1.IsCatalog = !strArray2[4].Equals("0");
                    data1.HasLeaf = strArray2[4].Equals("1");
                    NodeData item = data1;
                    nodeList.Add(item);
                }
                if (nodeList.Count > 0)
                {
                    IndicatorDataCore.SetCategoryValue(this.Browser, nodeList);
                }
            }
            catch (Exception exception)
            {
                //LogUtility.LogMessage("获取指标分类解包失败," + exception.ToString());
                flag = false;
            }
            return flag;
        }

        public BrowserType Browser
        {
            get
            {
                return this._browser;
            }
            set
            {
                this._browser = value;
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
