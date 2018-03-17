using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace OwLib
{
    /// <summary>
    /// 排序枚举
    /// </summary>
    public enum EStorType
    {
        /// <summary>
        /// 不排序
        /// </summary>
        None = 0,
        /// <summary>
        /// 升序
        /// </summary>
        Asc,
        /// <summary>
        /// 降序
        /// </summary>
        Desc
    }

    /// <summary>
    /// 背景状态枚举
    /// </summary>
    public enum EGridDataElementBackground
    {
        /// <summary>
        /// 正常背景
        /// </summary>
        Normal = 0,
        /// <summary>
        /// 背景1
        /// </summary>
        Light = 1,
        /// <summary>
        /// 背景2
        /// </summary>
        Dark = 2
    }

    /// <summary>
    /// 
    /// </summary>
    public class RGridColumn
    {
        private bool _Frozen = false;
        private bool _Display = true;
        private bool _CanOrder = true;
        private bool _CanWidthChange = true;
        private int _ColumnWidth = 100;
        private int _ColumnHeight = 20;
        private StringAlignment _Align = StringAlignment.Far;
        private StringAlignment _VAlign = StringAlignment.Far;
        private Brush _FontColor = QuoteDrawService.BrushColorNormal;

        private String _name;
        /// <summary>
        /// 列名
        /// </summary>
        public String Name
        {
            get { return _name; }
            set { this._name = value; }
        }

        private String _caption;
        /// <summary>
        /// 列头显示的中文名字
        /// </summary>
        public String Caption
        {
            get { return _caption; }
            set { this._caption = value; }
        }

        /// <summary>
        /// 是否冻结，(默认：不冻结)
        /// </summary>
        public bool Frozen
        {
            get { return _Frozen; }
            set { _Frozen = value; }
        }
        /// <summary>
        /// 是否隐藏，(默认：显示)
        /// </summary>
        public bool Display
        {
            get
            {
                return _Display || Frozen;
            }
            set { _Display = value; }
        }
        /// <summary>
        /// 绑定的指标
        /// </summary>
        public FieldIndex BindIndex;
        /// <summary>
        /// 是否可以排序，(默认：可排序)
        /// </summary>
        public bool CanOrder
        {
            get { return _CanOrder; }
            set { _CanOrder = value; }
        }
        /// <summary>
        /// 是否可以调整列宽，(默认：可调整)
        /// </summary>
        public bool CanWidthChange
        {
            get { return _CanWidthChange; }
            set { _CanWidthChange = value; }
        }
        /// <summary>
        /// 列头跨度，(默认：100px)
        /// </summary>
        public int ColumnWidth
        {
            get
            {
                int width = _ColumnWidth;
                if (this.ContainSubColumns)
                {
                    width = 0;
                    for (int index = 0; index < this.SubColumns.Count; index++)
                    {
                        if (this.SubColumns[index].Display)
                            width += this.SubColumns[index].ColumnWidth;
                    }
                }
                return width;
            }
            set
            {
                _ColumnWidth = value;
            }
        }
        /// <summary>
        /// 列头高度，(默认：20px)
        /// </summary>
        public int ColumnHeight
        {
            get
            {
                return _ColumnHeight + this.SubColumnsHeight;
            }
            set
            {
                _ColumnHeight = value;
            }
        }
        /// <summary>
        /// 是否包含子列
        /// </summary>
        public bool ContainSubColumns
        {
            get
            {
                return (this.SubColumns != null && this.SubColumns.Count > 0);
            }
        }
        /// <summary>
        /// 子列高度
        /// </summary>
        public int SubColumnsHeight
        {
            get
            {
                int subHeight = 0;
                if (this.ContainSubColumns)
                {
                    for (int index = 0; index < this.SubColumns.Count; index++)
                    {
                        if (this.SubColumns[index].ColumnHeight > subHeight)
                            subHeight = this.SubColumns[index].ColumnHeight;
                    }
                }
                return subHeight;
            }
        }

        private List<RGridColumn> _subColumns;
        /// <summary>
        /// 子列
        /// </summary>
        public List<RGridColumn> SubColumns
        {
            get { return _subColumns; }
            set { this._subColumns = value; }
        }

        private Point _headLocation;
        /// <summary>
        ///  
        /// </summary>
        public Point HeadLocation
        {
            get { return _headLocation; }
            set { this._headLocation = value; }
        }

        private Point _bottomLocation;
        /// <summary>
        /// 
        /// </summary>
        public Point BottomLocation
        {
            get { return _bottomLocation; }
            set { this._bottomLocation = value; }
        }
        /// <summary>
        /// 水平对齐方式（默认右对齐）
        /// </summary>
        public StringAlignment Align
        {
            get { return _Align; }
            set { _Align = value; }
        }

        /// <summary>
        /// 垂直对齐方式（默认底对齐）
        /// </summary>
        public StringAlignment VAlign
        {
            get { return _VAlign; }
            set { _VAlign = value; }
        }
        /// <summary>
        /// 当前列文本显示颜色
        /// </summary>
        public Brush FontColor
        {
            get { return _FontColor; }
            set { _FontColor = value; }
        }

        private bool _checkNull;
        /// <summary>
        /// 是否在没有数据或者==0的时候，不现实空白 显示--
        /// </summary>
        public bool CheckNull
        {
            get { return _checkNull; }
            set { this._checkNull = value; }
        }

        private bool _exportFlag;
        /// <summary>
        /// 指示本列是否需要导出
        /// </summary>
        public bool ExportFlag
        {
            get { return _exportFlag; }
            set { _exportFlag = value; }
        }

        private Type _exportType;
        /// <summary>
        /// 导出Excel时所使用的数据类型
        /// </summary>
        public Type ExportType
        {
            get { return _exportType; }
            set { _exportType = value; }
        }

        private Type _dataType;
        /// <summary>
        /// 当前列绑定字段的数据类型
        /// </summary>
        public Type DataType
        {
            get { return _dataType; }
            set { this._dataType = value; }
        }

        private String _format;
        /// <summary>
        /// 当前列绑定字段的格式化类别
        /// </summary>
        public String Format
        {
            get { return _format; }
            set { this._format = value; }
        }

        private bool _showTip;
        /// <summary>
        /// 列头是否展示Tip
        /// </summary>
        public bool ShowTip
        {
            get { return _showTip; }
            set { this._showTip = value; }
        }

        private String _tipId;
        /// <summary>
        /// TipID
        /// </summary>
        public String TipID
        {
            get { return _tipId; }
            set { this._tipId = value; }
        }

        private String _helpButtonId;
        /// <summary>
        /// 帮助按钮ID
        /// </summary>
        public String HelpButtonID
        {
            get { return _helpButtonId; }
            set { this._helpButtonId = value; }
        }

        private ReportField _reportFieldConfig;
        /// <summary>
        /// ★注意★尽量不要使用该属性，会形成耦合
        /// </summary>
        public ReportField ReportFieldConfig
        {
            get { return _reportFieldConfig; }
            set { this._reportFieldConfig = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override String ToString()
        {
            return this.Caption;
        }

        /// <summary>
        /// GridColumn对象的深拷贝
        /// </summary>
        /// <returns></returns>
        public RGridColumn DeepClone()
        {
            RGridColumn replica = new RGridColumn();
            replica.Name = this.Name;
            replica.Caption = this.Caption;
            replica.Frozen = this.Frozen;
            replica.Display = this.Display;
            replica.BindIndex = this.BindIndex;
            replica.CanOrder = this.CanOrder;
            replica.CanWidthChange = this.CanWidthChange;
            replica.ColumnWidth = this.ColumnWidth;
            replica.ColumnHeight = this.ColumnHeight;
            //ContainSubColumns = this.ContainSubColumns,
            //SubColumnsHeight = this.SubColumnsHeight,
            replica.HeadLocation = this.HeadLocation;
            replica.BottomLocation = this.BottomLocation;
            replica.Align = this.Align;
            replica.VAlign = this.VAlign;
            replica.FontColor = this.FontColor;
            replica.CheckNull = this.CheckNull;
            replica.ExportFlag = this.ExportFlag;
            replica.ExportType = this.ExportType;
            replica.DataType = this.DataType;
            replica.Format = this.Format;
            replica.ShowTip = this.ShowTip;
            replica.TipID = this.TipID;
            replica.HelpButtonID = this.HelpButtonID;
            replica.ReportFieldConfig = this.ReportFieldConfig;

            if (this.ContainSubColumns)
            {
                replica.SubColumns = new List<RGridColumn>(this.SubColumns.Count);

                foreach (RGridColumn item in this.SubColumns)
                {
                    replica.SubColumns.Add(item.DeepClone());
                }
            }

            return replica;
        }
    }

    /// <summary>
    /// 行
    /// </summary>
    public class RGridRow
    {
        private int _rowIndex;
        /// <summary>
        /// 行索引
        /// </summary>
        public int RowIndex
        {
            get { return _rowIndex; }
            set { this._rowIndex = value; }
        }

        private object _rowKey;
        /// <summary>
        /// 行键
        /// </summary>
        public object RowKey
        {
            get { return _rowKey; }
            set { this._rowKey = value; }
        }

        private Dictionary<FieldIndex, GridDataElement_New> _rowData;
        /// <summary>
        /// 行数据
        /// </summary>
        public Dictionary<FieldIndex, GridDataElement_New> RowData
        {
            get { return _rowData; }
            set { this._rowData = value; }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class GridDataElement
    {
        private FieldIndex _fieldIndex;
        /// <summary>
        /// 列的FieldIndex 
        /// </summary>
        public FieldIndex FieldIndex
        {
            get { return _fieldIndex; }
            set { this._fieldIndex = value; }
        }

        private object _data;
        /// <summary>
        /// 
        /// </summary>
        public object Data
        {
            get { return _data; }
            set { this._data = value; }
        }

        private EGridDataElementBackground _background;
        /// <summary>
        /// 
        /// </summary>
        public EGridDataElementBackground Background
        {
            get { return _background; }
            set { this._background = value; }
        }
    }

    /// <summary>
    /// Grid数据项
    /// </summary>
    public class GridDataElement_New
    {
        /// <summary>
        /// FieldIndex
        /// </summary>
        public FieldIndex FieldIndex;
        /// <summary>
        /// 数据内容
        /// </summary>
        public String DataContent;
        /// <summary>
        /// 前景
        /// </summary>
        public SolidBrush BrushForeground;
        /// <summary>
        /// 背景
        /// </summary>
        public SolidBrush BrushBackground;
    }
}
