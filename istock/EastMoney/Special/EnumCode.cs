using System;

namespace EmReportWatch.SpecialEntity.Entity
{
    /// <summary>
    /// 过滤条件运算规则枚举
    /// </summary>
    [Serializable]
    public enum FILTER
    {
        /// <summary>
        /// 没有过滤条件
        /// </summary>
        DEFAULT,

        /// <summary>
        /// 大于
        /// </summary>
        DAYU,

        /// <summary>
        /// 等于
        /// </summary>
        DENGYU,

        /// <summary>
        /// 大于或等于
        /// </summary>
        DAYUDENGYU,

        /// <summary>
        /// 小于
        /// </summary>
        XIAOYU,

        /// <summary>
        /// 小于等于
        /// </summary>
        XIAOYUDENGYU,

        /// <summary>
        /// 包含(多项INn)
        /// </summary>
        BAOHANS,

        /// <summary>
        /// 相似(Like)
        /// </summary>
        LIKE,

        /// <summary>
        /// 或运算
        /// </summary>
        OR,
        /// <summary>
        /// IN运算
        /// </summary>
        IN

    }

    /// <summary>
    /// 客户端类型枚举
    /// </summary>
    public enum ClientType
    {
        /// <summary>
        /// 客户端
        /// </summary>
        Client,
        /// <summary>
        /// 业务平台终端
        /// </summary>
        Config,
        /// <summary>
        /// office终端
        /// </summary>
        Office
    }

    /// <summary>
    /// 命令枚举
    /// </summary>
    public enum COMMANDS
    {
        /// <summary>
        /// 不执行命令
        /// </summary>
        DEFAULT,

        /// <summary>
        /// 求和
        /// </summary>
        SUM,

        /// <summary>
        /// 求最大
        /// </summary>
        MAX,

        /// <summary>
        /// 求最小
        /// </summary>
        MIN
    }

    /// <summary>
    /// 数据类型枚举
    /// </summary>
    public enum DATATYPE
    {
        /// <summary>
        /// 字符串
        /// </summary>
        STRING,
        /// <summary>
        /// 整数
        /// </summary>
        NUMBERICAL,
        /// <summary>
        /// 日期
        /// </summary>
        DATE,
    }

    /// <summary>
    /// 组合类型枚举
    /// </summary>
    public enum GROUPTYPE
    {
        /// <summary>
        /// 默认
        /// </summary>
        DEFAULT,
        /// <summary>
        /// 组合
        /// </summary>
        GROUP,
        /// <summary>
        /// 最大值
        /// </summary>
        MAX,
        /// <summary>
        /// 求和
        /// </summary>
        SUM,
        /// <summary>
        /// 最小值
        /// </summary>
        MIN
    }
    /// <summary>
    /// tab页类型枚举
    /// </summary>
    public enum TABTYPE
    {
        /// <summary>
        /// 表格
        /// </summary>
        GRID,
        /// <summary>
        /// 多表头表格
        /// </summary>
        BANDGRID,
        /// <summary>
        /// 数据图
        /// </summary>
        CHART,
        /// <summary>
        /// 双表格
        /// </summary>
        TWOGRIDBOND
    }

    /// <summary>
    /// 取数据的方式
    /// </summary>
    public enum SOURCEPROVIDER
    {
        /// <summary>
        /// 远程
        /// </summary>
        REMOTE,
        /// <summary>
        /// 数据表
        /// </summary>
        TABLIE,
        /// <summary>
        /// 数据行
        /// </summary>
        ROWS,
        /// <summary>
        /// 行
        /// </summary>
        ROW
    }

    /// <summary>
    /// 浏览器方式
    /// </summary>
    public enum BrowseWay
    {
        /// <summary>
        /// 默认
        /// </summary>
        DEFAULT,
        /// <summary>
        /// 窗体浏览器
        /// </summary>
        FORMBROWSE,
        /// <summary>
        ///网页浏览器
        /// </summary>
        WEBBROWSE,
        /// <summary>
        /// 其他浏览器
        /// </summary>
        OTHERBREWSE,
        TABPAGE
    }

    /// <summary>
    /// 月或者季度
    /// </summary>
    public enum MonthOrQuarter
    {
        /// <summary>
        /// 默认
        /// </summary>
        DEFAULT,
        /// <summary>
        /// 月
        /// </summary>
        MONTH,
        /// <summary>
        /// 季度
        /// </summary>
        QUARTER
    }

    /// <summary>
    /// 值类型
    /// </summary>
    public enum ValueType
    {
        /// <summary>
        /// 字符串
        /// </summary>
        STRING,
        /// <summary>
        /// 整数型
        /// </summary>
        INT,
        /// <summary>
        /// 双精度型
        /// </summary>
        DOUBLE,
        /// <summary>
        /// 枚举
        /// </summary>
        ENUM,
        /// <summary>
        /// 日期
        /// </summary>
        DATE,
        /// <summary>
        /// 默认
        /// </summary>
        CUSTOM
    }

    /// <summary>
    /// 宏观报表类型
    /// </summary>
    public enum ReportDataType
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default,
        /// <summary>
        /// 日
        /// </summary>
        Day,
        /// <summary>
        /// 周
        /// </summary>
        Week,
        /// <summary>
        /// 月
        /// </summary>
        Month,
        /// <summary>
        /// 季
        /// </summary>
        Quarter,
        /// <summary>
        /// 年
        /// </summary>
        Year
    }
}
