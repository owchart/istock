using System;
using System.Collections.Generic;
using System.Text;
using EmCore;

namespace OwLib
{
    public class IndicatorEntityListDataPacket : DataPacketBase
    {
        private List<String> _categoryCodeList;
        private Dictionary<String, IndicatorEntity> _indicatorEntityDict;
        private List<String> _requestCategoryCodeList;

        public IndicatorEntityListDataPacket(List<String> categoryCodeList)
        {
            base.RequestId = RequestType.IndicatorEntityList;
            this.CategoryCodeList = categoryCodeList;
            this.IndicatorEntityDict = new Dictionary<String, IndicatorEntity>();
            this._requestCategoryCodeList = new List<String>();
            foreach (String str in categoryCodeList)
            {
                IndicatorEntity indicatorEntityByCategoryCode = IndicatorDataCore.GetIndicatorEntityByCategoryCode(str);
                if (indicatorEntityByCategoryCode != null)
                {
                    this.IndicatorEntityDict[str] = indicatorEntityByCategoryCode;
                }
                else
                {
                    this._requestCategoryCodeList.Add(str);
                }
            }
        }

        public override String Coding()
        {
            return String.Format("1006◎{1}◎IndicatorService◎3◎4◎10,{0}", String.Join("/", this._requestCategoryCodeList.ToArray()), DataCenter.UserID);
        }

        public override bool Decoding(byte[] bytes)
        {
            if (((bytes != null) && (bytes.Length > 0)) && bytes[0].ToString().Equals("48"))
            {
                return false;
            }
            try
            {
                String str = Encoding.UTF8.GetString(bytes);
                int index = str.IndexOf('|');
                String str2 = str.Substring(0, index);
                String str3 = str.Substring(index + 1);
                String[] strArray = str2.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                int startIndex = 0;
                if ((strArray != null) && (strArray.Length > 0))
                {
                    for (int i = 0; i < strArray.Length; i++)
                    {
                        if ((i % 2) == 0)
                        {
                            int length = Convert.ToInt32(strArray[i]);
                            String categoryCode = str3.Substring(startIndex, length);
                            startIndex += length;
                            int num5 = Convert.ToInt32(strArray[i + 1]);
                            String str5 = str3.Substring(startIndex, num5);
                            startIndex += num5;
                            IndicatorEntity entity = JSONHelper.DeserializeObject<IndicatorEntity>(str5);
                            this.IndicatorEntityDict[categoryCode] = entity;
                            IndicatorDataCore.SetIndicatorEntity(categoryCode, entity);
                        }
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                //LogUtility.LogMessage("获取指标实体List解包失败," + exception.Message + exception.StackTrace);
                return false;
            }
        }

        public List<String> CategoryCodeList
        {
            get
            {
                return this._categoryCodeList;
            }
            set
            {
                this._categoryCodeList = value;
            }
        }

        public Dictionary<String, IndicatorEntity> IndicatorEntityDict
        {
            get
            {
                return this._indicatorEntityDict;
            }
            set
            {
                this._indicatorEntityDict = value;
            }
        }
    }
}
