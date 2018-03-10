using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    /// <summary>
    /// 板块重新加载成功回调委托
    /// </summary>
    public delegate void ReloadBlockTreeSuccessEventHandle();

    /// <summary>
    /// 获取板块树的回调委托
    /// </summary>
    /// <param name="args">返回的结果</param>
    public delegate void BlockTreeReceiveEventHandle(BlockTreeReceiveEventArgs args);

    /// <summary>
    /// 获取板块树的回调委托的参数
    /// </summary>
    public class BlockTreeReceiveEventArgs
    {
        /// <summary>
        /// 初始化构造函数
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="systemBlockStream"></param>
        /// <param name="userBlockStream"></param>
        public BlockTreeReceiveEventArgs(String guid, String systemBlockStream, String userBlockStream)
        {
            this._guid = guid;
            this._systemBlockStream = systemBlockStream;
            this._userBlockStream = userBlockStream;
        }

        private String _guid = "";
        /// <summary>
        /// GUID
        /// </summary>
        public String Guid
        {
            get { return _guid; }
            set { _guid = value; }
        }

        private String _systemBlockStream = "";
        /// <summary>
        /// 系统板块树
        /// </summary>
        public String SystemBlockStream
        {
            get { return _systemBlockStream; }
            set { _systemBlockStream = value; }
        }

        private String _userBlockStream = "";

        /// <summary>
        /// 用户自定义版块树
        /// </summary>
        public String UserBlockStream
        {
            get { return _userBlockStream; }
            set { _userBlockStream = value; }
        }
    }
}
