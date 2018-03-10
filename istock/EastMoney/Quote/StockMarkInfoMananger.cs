using System;
using System.Collections.Generic;
using System.IO;

namespace EmQComm
{
    /// <summary>
    /// 股票标记信息
    /// </summary>
    public class StockMarkInfo : PersistableObject
    {
        private int _Code;
        private StockTag _StockTag;
        private string _MarkInfo;

        /// <summary>
        /// 股票内码
        /// </summary>
        public int Code
        {
            get { return _Code; }
            set { _Code = value; }
        }
        /// <summary>
        /// 股票标记
        /// </summary>
        public StockTag StockTag
        {
            get { return _StockTag; }
            set { _StockTag = value; }
        }
        /// <summary>
        /// 标记文本
        /// </summary>
        public string MarkInfo
        {
            get { return _MarkInfo; }
            set { _MarkInfo = value; }
        }

        public override void SaveState(IMemento memento)
        {
            memento.SetInteger("Code", _Code);
            memento.SetEnumValue("StockTag", typeof(StockTag), _StockTag);
            if (_StockTag == StockTag.Text)
                memento.SetString("MarkInfo", _MarkInfo);
        }
        public override void LoadState(IMemento memento)
        {
            _Code = memento.GetInteger("Code");
            _StockTag = (StockTag)memento.GetEnumValue("StockTag", typeof(StockTag));
            if (_StockTag == StockTag.Text)
                _MarkInfo = memento.GetString("MarkInfo");
        }
    }
    /// <summary>
    /// 股票标记信息管理器
    /// </summary>
    public class StockMarkInfoMananger : PersistableObject
    {
        private static readonly string _StockMarkInfoFile = PathUtilities.UserPath + "StockMarkInfo.xml";
        private static StockMarkInfoMananger _Instance;
        private Dictionary<int, StockMarkInfo> _StockMarkInfo;
        private static readonly string _CurrentVersion = "v1.0";

        public Dictionary<int, StockMarkInfo> StockMarkInfo
        {
            get { return _StockMarkInfo; }
        }

        private StockMarkInfoMananger()
        {
            _StockMarkInfo = new Dictionary<int, StockMarkInfo>();
        }
        public static StockMarkInfoMananger GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new StockMarkInfoMananger();

                if (File.Exists(_StockMarkInfoFile))
                    new XmlFileSerializer(_StockMarkInfoFile).Deserialization(_Instance);
            }
            return _Instance;
        }
        public void SaveInfo()
        {
            new XmlFileSerializer(_StockMarkInfoFile).Serialization(_Instance);
        }

        /// <summary>
        /// 设置股票标记
        /// </summary>
        public void SetStockTag(int code, StockTag tag, string text)
        {
            StockMarkInfo sInfo;
            _StockMarkInfo.TryGetValue(code, out sInfo);
            if (sInfo == null)
                sInfo = new StockMarkInfo();
            sInfo.Code = code;
            sInfo.StockTag = tag;

            if (sInfo.StockTag == StockTag.Text)
                sInfo.MarkInfo = text;
            else
                sInfo.MarkInfo = string.Empty;

            _StockMarkInfo[code] = sInfo;

            SaveInfo();
        }
        /// <summary>
        /// 删除标记
        /// </summary>
        public void DeleteStockTag(int code)
        {
            if (_StockMarkInfo.ContainsKey(code))
                _StockMarkInfo.Remove(code);

            SaveInfo();
        }

        public override void SaveState(IMemento memento)
        {
            memento.Name = "StockMarkInfoMananger";
            memento.SetString("Version", _CurrentVersion);

            if (_StockMarkInfo != null)
            {
                foreach (int code in _StockMarkInfo.Keys)
                {
                    IMemento childMemento = memento.CreateChild("StockMarkInfo");

                    _StockMarkInfo[code].SaveState(childMemento);
                }
            }
        }
        public override void LoadState(IMemento memento)
        {
            _StockMarkInfo.Clear();

            string versionInfo = memento.GetString("Version");

            if (_CurrentVersion.Equals(versionInfo, StringComparison.OrdinalIgnoreCase))
            {
                for (int index = 0; index < memento.ChildCount; index++)
                {
                    IMemento subIMemento = memento.GetChild(index);
                    StockMarkInfo markInfo = new StockMarkInfo();
                    markInfo.LoadState(subIMemento);

                    _StockMarkInfo[markInfo.Code] = markInfo;
                }
            }
        }
    }
}
