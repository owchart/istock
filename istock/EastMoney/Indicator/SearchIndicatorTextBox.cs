using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using dataquery;

namespace dataquery.indicator
{
    /// ��ѯָ��Ļص�
    /// </summary>
    /// <param name="label">��ǩ</param>
    /// <param name="name">����</param>
    /// <param name="path">·��</param>
    /// <param name="pathCode">·������</param>
    /// <param name="topicIndicator">����</param>
    /// <param name="typeName">����</param>
    /// <param name="url">��ַ</param>
    public delegate void SearchIndicatorCallBack(String label, String name, String path, String pathCode, String topicIndicator, String typeName, String url);

    /// <summary>
    /// ��ѯָ����ı���
    /// </summary>
    public class SearchIndicatorTextBox : TextBox
    {
        #region �յ� 2014/7/25
        /// <summary>
        /// ����������
        /// </summary>
        public SearchIndicatorTextBox()
        {
        }

        /// <summary>
        /// ��ѯ�ص�����
        /// </summary>
        private List<SearchIndicatorCallBack> callBacks = new List<SearchIndicatorCallBack>();

        private bool classify = false;

        /// <summary>
        /// ��ȡ�����ý���Ƿ����
        /// </summary>
        public bool Classify
        {
            get { return classify; }
            set { classify = value; }
        }

        private int limit = 50;

        /// <summary>
        /// ��ȡ������������������
        /// </summary>
        public int Limit
        {
            get { return limit; }
            set { limit = value; }
        }

        private String parameters;

        /// <summary>
        /// ��ȡ�����ò���
        /// </summary>
        public String Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        private String searchType = "001001001";

        /// <summary>
        /// ��ȡ��������������
        /// </summary>
        public String SearchType
        {
            get { return searchType; }
            set { searchType = value; }
        }

        /// <summary>
        /// �������
        /// </summary>
        public void ClearText()
        {
            this.Text = "";
        }

        /// <summary>
        /// ��ʼ��ѯ
        /// </summary>
        /// <param name="type">����</param>
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
        /// ��ȡָ��ڵ���Ϣ
        /// </summary>
        /// <param name="categoryCodes">�������</param>
        /// <returns>�ڵ���Ϣ</returns>
        public static Dictionary<String, String> GetIndicatorListInfos(String categoryCodes)
        {
            Dictionary<String, String> list = new Dictionary<String, String>();
            try
            {
                String sendMessage = String.Empty;
                sendMessage = String.Format("{0}��{1}��{2}��{3}��{4}��{5}", "1012", DataCenter.UserID,
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
        /// ���̰��·���
        /// </summary>
        /// <param name="e">����</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            //���Ϸ���
            if (e.KeyData == Keys.Up)
            {
                BeginSearch(1);
            }
            //���·���
            else if (e.KeyData == Keys.Down)
            {
                BeginSearch(2);
            }
            //����
            else if (e.KeyData == Keys.Escape)
            {
                this.Text = "";
                BeginSearch(3);
            }
            //�س�
            else if (e.KeyData == Keys.Enter)
            {
                BeginSearch(4);
            }
            if ((int)e.KeyCode == 13)
            {
                //ִ���������       
                e.SuppressKeyPress = true;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0102)
            {
                if (this.Text.IndexOf("����ƴ������ָ��") >= 0)
                //if (this.Text == "����ƴ������ָ�꣺")
                {
                    this.Text = "";
                }
            }
            base.WndProc(ref m);
        }

        /// <summary>
        /// ������
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
        /// ��ѯ�ص�
        /// </summary>
        /// <param name="label">��ǩ</param>
        /// <param name="name">����</param>
        /// <param name="path">·��</param>
        /// <param name="path">·������</param>
        /// <param name="topicIndicator">����</param>
        /// <param name="typeName">����</param>
        /// <param name="url">��ַ</param>
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
            if (Text.Length > 0 && Text.IndexOf("����ƴ������ָ��") < 0)
            {
                BeginSearch(0);
            }
        }

        /// <summary>
        /// �ı��ı䷽��
        /// </summary>
        /// <param name="e">����</param>
        protected override void OnTextChanged(EventArgs e)
        {
            if (Focused && Text.IndexOf("����ƴ������ָ��") < 0)
            {
                BeginSearch(0);
            }
        }

        /// <summary>
        /// ע���ѯ�ص�
        /// </summary>
        /// <param name="callBack">�ص�</param>
        public void RegisterSearchCallBack(SearchIndicatorCallBack callBack)
        {
            lock (callBacks)
            {
                callBacks.Add(callBack);
            }
        }

        /// <summary>
        /// ȡ��ע���ѯ�ص�
        /// </summary>
        /// <param name="callBack">�ص�</param>
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
