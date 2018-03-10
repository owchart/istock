using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using dataquery;

namespace dataquery.indicator
{
    /// 查询指标的回调
    /// </summary>
    /// <param name="label">标签</param>
    /// <param name="name">名称</param>
    /// <param name="path">路径</param>
    /// <param name="pathCode">路径代码</param>
    /// <param name="topicIndicator">主题</param>
    /// <param name="typeName">类型</param>
    /// <param name="url">地址</param>
    public delegate void SearchIndicatorCallBack(String label, String name, String path, String pathCode, String topicIndicator, String typeName, String url);

    /// <summary>
    /// 查询指标的文本框
    /// </summary>
    public class SearchIndicatorTextBox : TextBox
    {
        #region 陶德 2014/7/25
        /// <summary>
        /// 创建搜索框
        /// </summary>
        public SearchIndicatorTextBox()
        {
        }

        /// <summary>
        /// 查询回调集合
        /// </summary>
        private List<SearchIndicatorCallBack> callBacks = new List<SearchIndicatorCallBack>();

        private bool classify = false;

        /// <summary>
        /// 获取或设置结果是否归类
        /// </summary>
        public bool Classify
        {
            get { return classify; }
            set { classify = value; }
        }

        private int limit = 50;

        /// <summary>
        /// 获取或设置数据限制条数
        /// </summary>
        public int Limit
        {
            get { return limit; }
            set { limit = value; }
        }

        private String parameters;

        /// <summary>
        /// 获取或设置参数
        /// </summary>
        public String Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        private String searchType = "001001001";

        /// <summary>
        /// 获取或设置搜索类型
        /// </summary>
        public String SearchType
        {
            get { return searchType; }
            set { searchType = value; }
        }

        /// <summary>
        /// 清空文字
        /// </summary>
        public void ClearText()
        {
            this.Text = "";
        }

        /// <summary>
        /// 开始查询
        /// </summary>
        /// <param name="type">类型</param>
        private void BeginSearch(int type)
        {
            String key = null;
            String input = this.Text;
            if (input.Length > 50)
            {
                input = input.Substring(0, 50);
            }
            if (parameters != null && parameters.Length > 0)
            {
                key = String.Format("pageIndex=1&limit={0}&searchKey={1}&searchType={2}&classify={3}&parameters={4}", limit, input, searchType, classify.ToString().ToLower(), parameters);
            }
            else
            {
                key = String.Format("pageIndex=1&limit={0}&searchKey={1}&searchType={2}&classify={3}", limit, input, searchType, classify.ToString().ToLower());
            }
            Rectangle screenRect = Screen.GetWorkingArea(this);
            Point location = PointToScreen(new Point(0, this.Height));
            if (location.Y + 330 > screenRect.Bottom)
            {
                location.Y = location.Y - 270 - this.Height - 2;
            }
        }

        /// <summary>
        /// 读取指标节点信息
        /// </summary>
        /// <param name="categoryCodes">分类代码</param>
        /// <returns>节点信息</returns>
        public static Dictionary<String, String> GetIndicatorListInfos(String categoryCodes)
        {
            Dictionary<String, String> list = new Dictionary<String, String>();
            try
            {
                String sendMessage = String.Empty;
                sendMessage = String.Format("{0}◎{1}◎{2}◎{3}◎{4}◎{5}", "1012", DataCenter.UserID,
                                "IndicatorService", "3", "4", "5," + categoryCodes);
                byte[] bytes = DataCenter.DataQuery.NewQueryGlobalData(sendMessage) as byte[];
                String result = Encoding.UTF8.GetString(bytes);
                int idx = result.IndexOf('|');
                String head = result.Substring(0, idx);
                String body = result.Substring(idx + 1);
                String[] strHeads = head.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                int pos = 0;
                if (strHeads != null && strHeads.Length > 0)
                {
                    for (int i = 0; i < strHeads.Length; i++)
                    {
                        if (i % 2 == 0)
                        {
                            int categoryCodeLen = Convert.ToInt32(strHeads[i]);
                            String categoryCode = body.Substring(pos, categoryCodeLen);
                            pos += categoryCodeLen;
                            int listInfoLen = Convert.ToInt32(strHeads[i + 1]);
                            String listInfo = body.Substring(pos, listInfoLen);
                            pos += listInfoLen;
                            list[categoryCode] = listInfo;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
            }
            return list;
        }

        /// <summary>
        /// 键盘按下方法
        /// </summary>
        /// <param name="e">按键</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            //向上翻滚
            if (e.KeyData == Keys.Up)
            {
                BeginSearch(1);
            }
            //向下翻滚
            else if (e.KeyData == Keys.Down)
            {
                BeginSearch(2);
            }
            //隐藏
            else if (e.KeyData == Keys.Escape)
            {
                this.Text = "";
                BeginSearch(3);
            }
            //回车
            else if (e.KeyData == Keys.Enter)
            {
                BeginSearch(4);
            }
            if ((int)e.KeyCode == 13)
            {
                //执行你的内容       
                e.SuppressKeyPress = true;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0102)
            {
                if (this.Text.IndexOf("输入拼音查找指标") >= 0)
                //if (this.Text == "输入拼音查找指标：")
                {
                    this.Text = "";
                }
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// 鼠标滚动
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                BeginSearch(5);
            }
            else if (e.Delta < 0)
            {
                BeginSearch(6);
            }
        }

        /// <summary>
        /// 查询回调
        /// </summary>
        /// <param name="label">标签</param>
        /// <param name="name">名称</param>
        /// <param name="path">路径</param>
        /// <param name="path">路径代码</param>
        /// <param name="topicIndicator">主题</param>
        /// <param name="typeName">类型</param>
        /// <param name="url">地址</param>
        public virtual void OnSearchCallBack(String label, String name, String path, String pathCode, String topicIndicator, String typeName, String url)
        {
            List<SearchIndicatorCallBack> callBacksCopy = new List<SearchIndicatorCallBack>();
            lock (callBacks)
            {
                callBacksCopy.AddRange(callBacks.ToArray());
            }
            for (int i = 0; i < callBacksCopy.Count; i++)
            {
                callBacksCopy[i](label, name, path, pathCode, topicIndicator, typeName, url);
            }
        }

        protected override void OnClick(EventArgs e)
        {
            if (Text.Length > 0 && Text.IndexOf("输入拼音查找指标") < 0)
            {
                BeginSearch(0);
            }
        }

        /// <summary>
        /// 文本改变方法
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnTextChanged(EventArgs e)
        {
            if (Focused && Text.IndexOf("输入拼音查找指标") < 0)
            {
                BeginSearch(0);
            }
        }

        /// <summary>
        /// 注册查询回调
        /// </summary>
        /// <param name="callBack">回调</param>
        public void RegisterSearchCallBack(SearchIndicatorCallBack callBack)
        {
            lock (callBacks)
            {
                callBacks.Add(callBack);
            }
        }

        /// <summary>
        /// 取消注册查询回调
        /// </summary>
        /// <param name="callBack">回调</param>
        public void UnRegisterSearchCallBack(SearchIndicatorCallBack callBack)
        {
            lock (callBacks)
            {
                callBacks.Remove(callBack);
            }
        }
        #endregion
    }
}
