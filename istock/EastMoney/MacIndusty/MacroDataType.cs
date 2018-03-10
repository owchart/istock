namespace EmMacIndustry.Model.Enum
{
    using System;
    using System.ComponentModel;

    public enum MacroDataType
    {
        [Description("全部数据")]
        All = 3,
        [Description("铝产业链数据")]
        Aluminum = 8,
        [Description("沥青产业链数据")]
        Asphalt = 11,
        [Description("一带一路")]
        BeltandRoad = 0x16,
        [Description("菜籽油产业链数据库")]
        Canola = 0x15,
        [Description("化工产业链数据库")]
        Chemical = 0x10,
        [Description("中国宏观数据")]
        China = 0,
        [Description("铜产业链数据")]
        Copper = 7,
        [Description("房地产经济数据")]
        Estate = 6,
        [Description("大宗商品数据库")]
        Future = 5,
        [Description("大宗商品数据库(新)")]
        FutureNew = 10,
        [Description("全球宏观数据")]
        Global = 2,
        [Description("贵金属产业链数据库")]
        GoldMetal = 20,
        [Description("行业经济数据")]
        Industry = 1,
        [Description("信息技术行业数据库")]
        IT = 13,
        [Description("有色金属经济数据")]
        NonferrousMetals = 9,
        [Description("石油产业链数据库")]
        Oil = 15,
        [Description("利率走势分析")]
        Rate = 4,
        [Description("天然橡胶产业链数据库")]
        Rubber = 14,
        [Description("大豆产业链数据库")]
        Soybean = 0x11,
        [Description("动力煤产业链数据")]
        SteamCoal = 12,
        [Description("钢铁产业链数据库")]
        Steel = 0x12,
        [Description("世界银行数据库")]
        WorldBank = 0x13
    }
}

