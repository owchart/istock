using System;
using System.Collections.Generic;
using System.Text;
using EmQComm;

namespace EmQDataCore
{
    /// <summary>
    /// �ۺ����ľ�̬����
    /// </summary>
    public class ResearchReportDatatable : DataTableBase
    {
        private List<ResearchReportItem> _infoResearchReportList;

        private List<BondPublicOpeartionItem> _bondpublicOpeartionList;

        /// <summary>
        /// ծȯ�ۺ���-�����г�����ģ��ľ�̬�����б�
        /// </summary>
        public List<BondPublicOpeartionItem> BondPublicOpeartionList
        {
            get { return _bondpublicOpeartionList; }
            set { _bondpublicOpeartionList = value; }
        }

        /// <summary>
        /// �о����棨�еĸ��������б�
        /// </summary>
        public List<ResearchReportItem> InfoResearchReportList
        {
            get { return _infoResearchReportList; }
            set { _infoResearchReportList = value; }
        }
        //private List<int> _codes;
        ///// <summary>
        ///// �����б�
        ///// </summary>
        //public List<int> Codes
        //{
        //    get { return _codes; }
        //    set { _codes = value; }
        //}
        /// <summary>
        /// ���캯��
        /// </summary>
        public ResearchReportDatatable()
        {
            InfoResearchReportList = new List<ResearchReportItem>();
            BondPublicOpeartionList = new List<BondPublicOpeartionItem>();
            //Codes = new List<int>();
        }
        /// <summary>
        /// SetData
        /// </summary>
        /// <param name="dataPacket"></param>
        public override void SetData(DataPacket dataPacket)
        {
            if (dataPacket is ResResearchReportOrgDataPacket)
            {
                ResResearchReportOrgDataPacket packet = dataPacket as ResResearchReportOrgDataPacket;

                if (null == packet)
                    return;

                InfoResearchReportList = packet.InfoResearchReportList;
            }
            else if (dataPacket is ResBondDashboardPublicMarketOpeartion)
            {
                ResBondDashboardPublicMarketOpeartion packet = dataPacket as ResBondDashboardPublicMarketOpeartion;

                if (null == packet)
                    return;

                BondPublicOpeartionList = packet.BondPublicOpeartionList;
            }           
        }
    }
}
