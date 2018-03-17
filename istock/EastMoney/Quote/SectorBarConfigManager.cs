using System;
using System.Collections.Generic;
using System.Reflection;

namespace OwLib
{
    public class SectorBarConstantDefines
    {
        /// <summary>
        /// A股
        /// </summary>
        public static readonly String SB_AStock = "AStock";
        /// <summary>
        /// 指数
        /// </summary>
        public static readonly String SB_Index = "Index";
        /// <summary>
        /// 港股
        /// </summary>
        public static readonly String SB_HKStock = "HKStock";
        /// <summary>
        /// 美股
        /// </summary>
        public static readonly String SB_USAStock = "USAStock";
        /// <summary>
        /// 债券
        /// </summary>
        public static readonly String SB_Bond = "Bond";
        /// <summary>
        /// 利率
        /// </summary>
        public static readonly String SB_InterestRate = "InterestRate";
        /// <summary>
        /// 外汇
        /// </summary>
        public static readonly String SB_Exchange = "Exchange";
        /// <summary>
        /// 股指期货
        /// </summary>
        public static readonly String SB_IndexFuture = "IndexFuture";
        /// <summary>
        /// 商品期货
        /// </summary>
        public static readonly String SB_Future = "Future";
        /// <summary>
        /// 基金
        /// </summary>
        public static readonly String SB_Fund = "Fund";
        /// <summary>
        /// 理财产品
        /// </summary>
        public static readonly String SB_FinancialManager = "FinancialManager";
        /// <summary>
        /// 默认
        /// </summary>
        public static readonly String SB_Default = "Default";
    }

    /// <summary>
    /// 
    /// </summary>
    public class SectorBarButtonDescription : PersistableObject
    {
        /// <summary>
        /// 名称
        /// </summary>
        public String Name ;
        /// <summary>
        /// 板块名称
        /// </summary>
        public String SectorName ;
        /// <summary>
        /// 板块ID
        /// </summary>
        public String SectorID ;
        /// <summary>
        /// 
        /// </summary>
        public String VTabFlag ;
        /// <summary>
        /// 
        /// </summary>
        private SectorMenuBoxStripDescription _sectorMenuBoxStripDes;

        public SectorMenuBoxStripDescription SectorMenuBoxStripDes
        {
            get { return _sectorMenuBoxStripDes; }
            set { _sectorMenuBoxStripDes = value; }
        }
        /// <summary>
        /// 保存
        /// </summary>
        public override void SaveState(IMemento memento)
        {
        }
        /// <summary>
        /// 加载
        /// </summary>
        public override void LoadState(IMemento memento)
        {
            Name = memento.GetString("Name");
            SectorName = memento.GetString("SectorName");
            SectorID = memento.GetString("SectorID");
            VTabFlag = memento.GetString("VTabFlag");
            if (memento.ChildCount > 0)
            {
                SectorMenuBoxStripDes = new SectorMenuBoxStripDescription();
                SectorMenuBoxStripDes.LoadState(memento.GetChild(0));
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SectorMenuBoxStripDescription : PersistableObject
    {
        /// <summary>
        /// 名称
        /// </summary>
        public String Name ;
        /// <summary>
        /// 
        /// </summary>
        private Type _menuBoxStripType;

        public Type MenuBoxStripType
        {
            get { return _menuBoxStripType; }
            set { _menuBoxStripType = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        private List<SectorMenuBoxItem> _menuBoxItemList;

        public List<SectorMenuBoxItem> MenuBoxItemList
        {
            get { return _menuBoxItemList; }
            set { _menuBoxItemList = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public SectorMenuBoxStripDescription()
        {
            MenuBoxItemList = new List<SectorMenuBoxItem>();
        }
        /// <summary>
        /// 保存
        /// </summary>
        public override void SaveState(IMemento memento)
        {
        }
        /// <summary>
        /// 加载
        /// </summary>
        public override void LoadState(IMemento memento)
        {
            Name = memento.GetString("Name");
            String typeInfo = memento.GetString("MenuBoxStripType");
            Assembly assembly = Assembly.Load(typeInfo.Split(',')[1]);
            MenuBoxStripType = assembly.GetType(typeInfo.Split(',')[0]);

            MenuBoxItemList.Clear();
            for (int Index = 0; Index < memento.ChildCount; Index++)
            {
                IMemento childMemento = memento.GetChild(Index);
                SectorMenuBoxItem sectorMenuBoxItem = new SectorMenuBoxItem();
                sectorMenuBoxItem.LoadState(childMemento);
                MenuBoxItemList.Add(sectorMenuBoxItem);
            }
        }

        /// <summary>
        /// 此属性已被废弃
        /// </summary>
        public MenuListItem SectorSelected
        {
            get
            {
                //if (MenuBoxItemList != null)
                //{
                //    foreach (var item in MenuBoxItemList)
                //    {
                //        foreach (var subItem in item.SectorList)
                //        {
                //            if (subItem.IsChecked)
                //            {
                //                return subItem;
                //            }
                //        }
                //    }
                //}
                return null;
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SectorMenuBoxItem : PersistableObject
    {
        /// <summary>
        /// 名称
        /// </summary>
        private String _name;

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        private String _text;

        public String Text
        {
            get { return _text; }
            set { _text = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        private String _refSector;

        public String RefSector
        {
            get { return _refSector; }
            set { _refSector = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        private Type menuBoxItemType;

        public Type MenuBoxItemType
        {
            get { return menuBoxItemType; }
            set { menuBoxItemType = value; }
        }
        /// <summary>
        /// 板块集合
        /// </summary>
        private List<MenuListItem> _sectorList;

        public List<MenuListItem> SectorList
        {
            get { return _sectorList; }
            set { _sectorList = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public SectorMenuBoxItem()
        {
            SectorList = new List<MenuListItem>();
        }
        /// <summary>
        /// 保存
        /// </summary>
        public override void SaveState(IMemento memento)
        {
        }
        /// <summary>
        /// 加载
        /// </summary>
        public override void LoadState(IMemento memento)
        {
            Name = memento.GetString("Name");
            Text = memento.GetString("Text");
            RefSector = memento.GetString("RefSector");
            String typeInfo = memento.GetString("MenuBoxItemType");

            Assembly assembly = LoadAssembly(typeInfo.Split(',')[1]);
            MenuBoxItemType = assembly.GetType(typeInfo.Split(',')[0]);

            SectorList.Clear();
            for (int Index = 0; Index < memento.ChildCount; Index++)
            {
                IMemento childMemento = memento.GetChild(Index);
                MenuListItem menuListItem = new MenuListItem();
                menuListItem.LoadState(childMemento);

                //确保只有一个是选中状态
                if (menuListItem.IsChecked)
                    foreach (MenuListItem item in SectorList)
                        item.IsChecked = false;

                SectorList.Add(menuListItem);
            }
        }

        private Assembly LoadAssembly(String assemblyName)
        {
            Assembly[] ass = AppDomain.CurrentDomain.GetAssemblies();
            for (int index = 0; index < ass.Length; index++)
            {
                if (ass[index].FullName.IndexOf(assemblyName) != -1)
                {
                    return ass[index];
                }
            }

            return Assembly.Load(assemblyName);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class MenuListItem : PersistableObject
    {
        /// <summary>
        /// 按钮TEXT
        /// </summary>
        public String MenuText ;
        /// <summary>
        /// 按钮值
        /// </summary>
        public String MenuValue ;
        /// <summary>
        /// 是否选择
        /// </summary>
        public bool IsChecked ;
        /// <summary>
        /// 保存
        /// </summary>
        public override void SaveState(IMemento memento)
        {
        }
        /// <summary>
        /// 加载
        /// </summary>
        public override void LoadState(IMemento memento)
        {
            MenuText = memento.GetString("MenuText");
            MenuValue = memento.GetString("MenuValue");
            IsChecked = memento.GetBoolean("Checked");
        }
        /// <summary>
        /// 深拷贝
        /// </summary>
        public MenuListItem DeepClone()
        {
            MenuListItem temp = new MenuListItem();
            temp.MenuText = this.MenuText;
            temp.MenuValue = this.MenuValue;
            temp.IsChecked = this.IsChecked;
            return temp;
        }
    }

    /// <summary>
    /// 板块菜单条配置管理器 
    /// </summary>
    public class SectorBarConfigManager : PersistableObject
    {
        private static readonly String _ConfigFile = PathUtilities.CfgPath + "SectorMenuBox_V2.config";
        private static SectorBarConfigManager _Instance;
        /// <summary>
        /// 按钮集合
        /// </summary>
        private Dictionary<String, List<SectorBarButtonDescription>> _sectorBarDesps;

        public Dictionary<String, List<SectorBarButtonDescription>> SectorBarDesps
        {
            get { return _sectorBarDesps; }
            set { _sectorBarDesps = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        private SectorBarConfigManager()
        {
            SectorBarDesps = new Dictionary<String, List<SectorBarButtonDescription>>();
        }
        public static SectorBarConfigManager GetInstance()
        {
            if (_Instance == null)
            {
                _Instance = new SectorBarConfigManager();
                new XmlFileSerializer(_ConfigFile).Deserialization(_Instance);
            }

            return _Instance;
        }
        /// <summary>
        /// 加载配置
        /// </summary>
        public void LoadConfig()
        {
            new XmlFileSerializer(_ConfigFile).Deserialization(this);
        }
        /// <summary>
        /// 保存
        /// </summary>
        public override void SaveState(IMemento memento)
        {
        }
        /// <summary>
        /// 加载
        /// </summary>
        public override void LoadState(IMemento memento)
        {
            SectorBarDesps.Clear();
            for (int Index = 0; Index < memento.ChildCount; Index++)
            {
                IMemento childMemento = memento.GetChild(Index);
                String sectorBarType = childMemento.GetString("BarType") ?? String.Empty;
                if (String.IsNullOrEmpty(sectorBarType))
                {
                    continue;
                }
                else
                {
                    List<SectorBarButtonDescription> btnDesp = new List<SectorBarButtonDescription>();
                    for (int subIndex = 0; subIndex < childMemento.ChildCount; subIndex++)
                    {
                        IMemento subChild = childMemento.GetChild(subIndex);
                        SectorBarButtonDescription barBtn = new SectorBarButtonDescription();
                        barBtn.LoadState(subChild);
                        btnDesp.Add(barBtn);
                    }
                    SectorBarDesps[sectorBarType] = btnDesp;
                }
            }
        }
    }
}
