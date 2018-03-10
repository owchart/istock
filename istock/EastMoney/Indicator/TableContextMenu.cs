using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Runtime.InteropServices;

namespace OwLib
{
    public class TableContextMenu : IDisposable
    {
        private Add_ClickHandler add_Click;
        private MenuItemClickHandler blockSequenceClick;
        private Dictionary<MenuMode, ContextMenuStrip> dicMenu = new Dictionary<MenuMode, ContextMenuStrip>();
        private MenuItemClickChanged menuItemClick;
        private MenuMode menuMode = MenuMode.RightTableMenu;

        public event Add_ClickHandler Add_Click
        {
            add
            {
                Add_ClickHandler handler2;
                Add_ClickHandler handler = this.add_Click;
                do
                {
                    handler2 = handler;
                    Add_ClickHandler handler3 = (Add_ClickHandler)Delegate.Combine(handler2, value);
                    handler = Interlocked.CompareExchange<Add_ClickHandler>(ref this.add_Click, handler3, handler2);
                }
                while (handler != handler2);
            }
            remove
            {
                Add_ClickHandler handler2;
                Add_ClickHandler handler = this.add_Click;
                do
                {
                    handler2 = handler;
                    Add_ClickHandler handler3 = (Add_ClickHandler)Delegate.Remove(handler2, value);
                    handler = Interlocked.CompareExchange<Add_ClickHandler>(ref this.add_Click, handler3, handler2);
                }
                while (handler != handler2);
            }
        }

        public event MenuItemClickHandler BlockSequenceClick
        {
            add
            {
                MenuItemClickHandler handler2;
                MenuItemClickHandler blockSequenceClick = this.blockSequenceClick;
                do
                {
                    handler2 = blockSequenceClick;
                    MenuItemClickHandler handler3 = (MenuItemClickHandler)Delegate.Combine(handler2, value);
                    blockSequenceClick = Interlocked.CompareExchange<MenuItemClickHandler>(ref this.blockSequenceClick, handler3, handler2);
                }
                while (blockSequenceClick != handler2);
            }
            remove
            {
                MenuItemClickHandler handler2;
                MenuItemClickHandler blockSequenceClick = this.blockSequenceClick;
                do
                {
                    handler2 = blockSequenceClick;
                    MenuItemClickHandler handler3 = (MenuItemClickHandler)Delegate.Remove(handler2, value);
                    blockSequenceClick = Interlocked.CompareExchange<MenuItemClickHandler>(ref this.blockSequenceClick, handler3, handler2);
                }
                while (blockSequenceClick != handler2);
            }
        }

        public event MenuItemClickChanged MenuItemClick
        {
            add
            {
                MenuItemClickChanged changed2;
                MenuItemClickChanged menuItemClick = this.menuItemClick;
                do
                {
                    changed2 = menuItemClick;
                    MenuItemClickChanged changed3 = (MenuItemClickChanged)Delegate.Combine(changed2, value);
                    menuItemClick = Interlocked.CompareExchange<MenuItemClickChanged>(ref this.menuItemClick, changed3, changed2);
                }
                while (menuItemClick != changed2);
            }
            remove
            {
                MenuItemClickChanged changed2;
                MenuItemClickChanged menuItemClick = this.menuItemClick;
                do
                {
                    changed2 = menuItemClick;
                    MenuItemClickChanged changed3 = (MenuItemClickChanged)Delegate.Remove(changed2, value);
                    menuItemClick = Interlocked.CompareExchange<MenuItemClickChanged>(ref this.menuItemClick, changed3, changed2);
                }
                while (menuItemClick != changed2);
            }
        }

        private void AddSelfBlock(ToolStripMenuItem item)
        {
        }

        private void BlockMenu(ContextMenuStrip menu)
        {
        }

        private void childItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;
            if ((item.Tag != null) && (this.add_Click != null))
            {
                this.add_Click(item.Tag.ToString());
            }
        }

        private void CreateMenuItem(ContextMenuStrip menu, String text, object tag, [Optional, DefaultParameterValue(null)] Image image, [Optional, DefaultParameterValue(false)] bool isAddChild)
        {
            ToolStripMenuItem item = new ToolStripMenuItem(text, image);
            item.AutoSize = menu.AutoSize;
            item.Tag = tag;
            item.Height = 0x16;
            item.Width = menu.Width - 1;
            menu.Items.Add(item);
            if (isAddChild)
            {
                ToolStripMenuItem item2 = new ToolStripMenuItem("我的自选股");
                item2.Click += new EventHandler(this.childItem_Click);
                item.DropDownItems.Add(item2);
                this.AddSelfBlock(item);
            }
        }

        public void Dispose()
        {
            this.dicMenu.Clear();
            this.menuItemClick = null;
            this.blockSequenceClick = null;
            this.add_Click = null;
        }

        private Image GetIcon(Point location, Size size)
        {
            return null;
        }

        private void IniMenu(MenuMode contextMenuMode)
        {
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.ItemClicked += new ToolStripItemClickedEventHandler(this.menu_ItemClicked);
            this.menuMode = contextMenuMode;
            switch (this.menuMode)
            {
                case MenuMode.RightTableMenu:
                    this.RightTableMenu(menu);
                    return;

                case MenuMode.NewBroadMenu:
                    this.NewBroadMenu(menu);
                    return;

                case MenuMode.Block:
                    this.BlockMenu(menu);
                    return;
            }
        }

        private void menu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Tag != null)
            {
                if (this.menuMode == MenuMode.Block)
                {
                    if (this.blockSequenceClick != null)
                    {
                        this.blockSequenceClick(e.ClickedItem.Tag.ToString());
                    }
                }
                else
                {
                    MenuItemMode tag = (MenuItemMode)e.ClickedItem.Tag;
                    if (this.menuItemClick != null)
                    {
                        this.menuItemClick(tag, e.ClickedItem.Text == "收藏指标");
                    }
                }
            }
        }

        private void NewBroadMenu(ContextMenuStrip menu)
        {
            Image icon = this.GetIcon(new Point(0x260, 0x10), new Size(0x10, 0x10));
            this.CreateMenuItem(menu, "存为板块", MenuItemMode.SaveBlock, icon, false);
            icon = this.GetIcon(new Point(0x270, 0x10), new Size(0x10, 0x10));
            this.CreateMenuItem(menu, "加入自选股", MenuItemMode.AddSelf, icon, true);
            icon = this.GetIcon(new Point(0x290, 0x10), new Size(0x10, 0x10));
            this.CreateMenuItem(menu, "深度资料(F9)", MenuItemMode.DepthData, icon, false);
            icon = this.GetIcon(new Point(640, 0x10), new Size(0x10, 0x10));
            this.CreateMenuItem(menu, "行情走势(F5)", MenuItemMode.MarketTrend, icon, false);
            ToolStripSeparator separator = new ToolStripSeparator();
            menu.Items.Add(separator);
            icon = this.GetIcon(new Point(0, 0), new Size(0x10, 0x10));
            this.CreateMenuItem(menu, "Excel导出", MenuItemMode.ExcelExport, icon, false);
            this.dicMenu.Add(MenuMode.NewBroadMenu, menu);
        }

        private void RightTableMenu(ContextMenuStrip menu)
        {
           
        }

        public void ShowBlockRightMenu(Control control, Point postion)
        {
            if (!this.dicMenu.ContainsKey(MenuMode.Block))
            {
                this.IniMenu(MenuMode.Block);
            }
            ContextMenuStrip strip = this.dicMenu[MenuMode.Block];
            strip.AutoClose = true;
            strip.DropShadowEnabled = false;
            strip.Show(control, postion);
        }

        public void ShowRBroadRightMenu(Control control, Point postion)
        {
            if (!this.dicMenu.ContainsKey(MenuMode.NewBroadMenu))
            {
                this.IniMenu(MenuMode.NewBroadMenu);
            }
            ContextMenuStrip strip = this.dicMenu[MenuMode.NewBroadMenu];
            strip.AutoClose = true;
            strip.DropShadowEnabled = false;
            ToolStripMenuItem item = strip.Items[1] as ToolStripMenuItem;
            item.DropDownItems.Clear();
            this.AddSelfBlock(item);
            strip.Show(control, postion);
        }

        public void ShowTableRightMenu(Control control, Point postion, bool isCollectionOrCancel, bool isCustomerIndicator, bool isHasIndicator, bool isCalculateIndicator)
        {
            if (!this.dicMenu.ContainsKey(MenuMode.RightTableMenu))
            {
                this.IniMenu(MenuMode.RightTableMenu);
            }
            ContextMenuStrip strip = this.dicMenu[MenuMode.RightTableMenu];
            strip.AutoClose = true;
            strip.DropShadowEnabled = false;
            strip.Items[0].Enabled = true;
            strip.Items[1].Enabled = !isCustomerIndicator;
            strip.Items[1].Text = isCollectionOrCancel ? "收藏指标" : "取消收藏";
            strip.Items[2].Enabled = true;
            strip.Items[3].Enabled = true;
            strip.Items[4].Enabled = true;
            if (!isHasIndicator)
            {
                strip.Items[0].Enabled = false;
                strip.Items[1].Enabled = false;
                strip.Items[2].Enabled = false;
                strip.Items[3].Enabled = false;
                strip.Items[4].Enabled = false;
            }
            if (isCalculateIndicator)
            {
                strip.Items[0].Enabled = false;
            }
            ToolStripMenuItem item = strip.Items[6] as ToolStripMenuItem;
            item = strip.Items[7] as ToolStripMenuItem;
            item.DropDownItems.Clear();
            this.AddSelfBlock(item);
            strip.Show(control, postion);
        }

        public delegate void Add_ClickHandler(String blockId);

        public delegate void MenuItemClickChanged(TableContextMenu.MenuItemMode itemMode, bool text);

        public delegate void MenuItemClickHandler(String id);

        public enum MenuItemMode
        {
            None,
            All,
            Equal,
            NotEqual,
            Start,
            End,
            Contains,
            NotContains,
            More,
            MoreOrEqual,
            Less,
            LessOrEqual,
            Between,
            TenMax,
            AboveAverage,
            BelowAverage,
            Before,
            After,
            NextWeek,
            Week,
            LastWeek,
            NextMonth,
            Month,
            LastMonth,
            NextQuarter,
            Quarter,
            LastQuarter,
            NextYear,
            Year,
            LastYear,
            YearToNow,
            ModifyParam,
            CollectIndicator,
            DeleteIndicator,
            DeleteBlock,
            RepeatInsertIndicator,
            IndicatorHelper,
            SaveBlock,
            AddSelf,
            DepthData,
            MarketTrend,
            AllSelected,
            AllNoSelected,
            DeleteSelectedRow,
            UpEliminate,
            DownEliminate,
            DeleteNoSelectedRow,
            CheckSelectedRows,
            CheckNoSelectedRows,
            Clear,
            ExcelExport,
            AddBrotherNode,
            AddChildNode,
            ReName,
            Delete,
            MoveTo,
            IndicatorHint,
            SetRemarkName,
            UnCollect,
            AddIndicator,
            GeIndicator,
            LtIndicator
        }
    }
}
