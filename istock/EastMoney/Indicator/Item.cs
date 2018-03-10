using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace dataquery.indicator
{
    public class Item
    {
        private Image icon;
        private Image selectedIcon;
        private object tag;
        private Rectangle textRect;
        private bool enabled = true;
        private int id;
        private int index;
        private bool isChecked;
        private bool isHalfChecked;
        private ItemType item_type;
        private String itemText;
        private TableContextMenu.MenuItemMode menu_item_mode;
        private String name;

        public override String ToString()
        {
            return this.itemText;
        }

        public bool Checked
        {
            get
            {
                return this.isChecked;
            }
            set
            {
                this.isChecked = value;
                this.isHalfChecked = false;
            }
        }

        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
            set
            {
                this.enabled = value;
            }
        }

        public Image Icon
        {
            get
            {
                return icon;
            }
            set
            {
                icon = value;
            }
        }

        public int Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public int Index
        {
            get
            {
                return this.index;
            }
            set
            {
                this.index = value;
            }
        }

        public bool IsHalfChecked
        {
            get
            {
                return this.isHalfChecked;
            }
            set
            {
                this.isHalfChecked = value;
                this.isChecked = false;
            }
        }

        public ItemType Item_Type
        {
            get
            {
                return this.item_type;
            }
            set
            {
                this.item_type = value;
            }
        }

        public int ItemHeight
        {
            get
            {
                return 0x16;
            }
        }

        public String ItemText
        {
            get
            {
                return this.itemText;
            }
            set
            {
                this.itemText = value;
            }
        }

        public TableContextMenu.MenuItemMode MenuItemMode
        {
            get
            {
                return this.menu_item_mode;
            }
            set
            {
                this.menu_item_mode = value;
            }
        }

        public String Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public Image SelectedIcon
        {
            get
            {
                return selectedIcon;
            }
            set
            {
                selectedIcon = value;
            }
        }

        public object Tag
        {
            get
            {
                return tag;
            }
            set
            {
                tag = value;
            }
        }

        public Rectangle TextRect
        {
            get
            {
                return textRect;
            }
            set
            {
                textRect = value;
            }
        }

        public enum ItemType
        {
            Text,
            Line
        }
    }
}
