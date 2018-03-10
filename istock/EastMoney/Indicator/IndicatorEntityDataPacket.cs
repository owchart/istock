using System;
using System.Collections.Generic;
using System.Text;
using EmCore;

namespace dataquery.indicator
{
    public class IndicatorEntityDataPacket : DataPacketBase
    {
        private String _categoryCode;
        private CommonEnumerators.MouseClickType _clickType;

        public IndicatorEntityDataPacket(String categoryCode)
            : this(categoryCode, CommonEnumerators.MouseClickType.DoubleClick)
        {
        }

        public IndicatorEntityDataPacket(String categoryCode, CommonEnumerators.MouseClickType clickType)
        {
            base.RequestId = RequestType.IndicatorEntity;
            this._categoryCode = categoryCode;
            this._clickType = clickType;
        }

        public override String Coding()
        {
            return String.Format("1001◎UniqueId◎IndicatorService◎[PreVersion]◎[NowVersion]◎7,{0}", this._categoryCode);
        }

        public override bool Decoding(byte[] bytes)
        {
            if (((bytes != null) && (bytes.Length > 0)) && bytes[0].ToString().Equals("48"))
            {
                return false;
            }
            bool flag = true;
            IndicatorEntity entity = null;
            try
            {
                entity = JSONHelper.DeserializeObject<IndicatorEntity>(Encoding.UTF8.GetString(bytes));
                IndicatorDataCore.SetIndicatorEntity(this._categoryCode, entity);
            }
            catch (Exception exception)
            {
                //LogUtility.LogMessage("获取指标实体解包失败," + exception.ToString());
                flag = false;
            }
            return flag;
        }

        public String CategoryCode
        {
            get
            {
                return this._categoryCode;
            }
        }

        public CommonEnumerators.MouseClickType ClickType
        {
            get
            {
                return this._clickType;
            }
            set
            {
                this._clickType = value;
            }
        }
    }
}
