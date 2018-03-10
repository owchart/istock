using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    /// <summary>
    /// ������¼��سɹ��ص�ί��
    /// </summary>
    public delegate void ReloadBlockTreeSuccessEventHandle();

    /// <summary>
    /// ��ȡ������Ļص�ί��
    /// </summary>
    /// <param name="args">���صĽ��</param>
    public delegate void BlockTreeReceiveEventHandle(BlockTreeReceiveEventArgs args);

    /// <summary>
    /// ��ȡ������Ļص�ί�еĲ���
    /// </summary>
    public class BlockTreeReceiveEventArgs
    {
        /// <summary>
        /// ��ʼ�����캯��
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
        /// ϵͳ�����
        /// </summary>
        public String SystemBlockStream
        {
            get { return _systemBlockStream; }
            set { _systemBlockStream = value; }
        }

        private String _userBlockStream = "";

        /// <summary>
        /// �û��Զ�������
        /// </summary>
        public String UserBlockStream
        {
            get { return _userBlockStream; }
            set { _userBlockStream = value; }
        }
    }
}
