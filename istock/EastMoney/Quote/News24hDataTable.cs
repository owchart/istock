using System;
using System.Collections.Generic;
using System.Text;
using EmQComm;

namespace EmQDataCore
{
    /// <summary>
    /// News24hDataTable
    /// </summary>
    public class News24hDataTable : DataTableBase
    {
        private List<OneNews24HOrgDataRec> _importantNewsData;
        private List<OneNews24HOrgDataRec> _news24HOrgData;
        private List<OneNews24HOrgDataRec> _newsFlashData;
        private List<OneNews24HDataRec> _news24HData;

        /// <summary>
        /// 24小时滚动资讯，时间降序，最新的在前面
        /// </summary>
        public List<OneNews24HDataRec> News24HData
        {
            get { return _news24HData; }
            private set { _news24HData = value; }
        }

        /// <summary>
        /// 24小时滚动资讯（机构版），时间降序，最新的在前面
        /// </summary>
        public List<OneNews24HOrgDataRec> News24HOrgData
        {
            get { return _news24HOrgData; }
            private set { _news24HOrgData = value; }
        }

        /// <summary>
        /// 要闻精华
        /// </summary>
        public List<OneNews24HOrgDataRec> ImportantNewsData
        {
            get { return _importantNewsData; }
            private set { _importantNewsData = value; }
        }

        /// <summary>
        /// 公司快讯
        /// </summary>
        public List<OneNews24HOrgDataRec> NewsFlashData
        {
            get { return _newsFlashData; }
            private set { _newsFlashData = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public News24hDataTable()
        {
            News24HData = new List<OneNews24HDataRec>(0);
            News24HOrgData = new List<OneNews24HOrgDataRec>(0);
            ImportantNewsData = new List<OneNews24HOrgDataRec>(0);
            NewsFlashData = new List<OneNews24HOrgDataRec>(0);
        }
        /// <summary>
        /// SetData
        /// </summary>
        /// <param name="dataPacket"></param>
        public override void SetData(DataPacket dataPacket)
        {
            if(dataPacket is ResNews24DataPacket)
            {
                List<OneNews24HDataRec> packetData = (dataPacket as ResNews24DataPacket).News24HData;
				if(News24HData.Count == 0){
					News24HData = packetData;
				} else{
					for(int i = 0; i < packetData.Count; i++) {
						for(int j = 0; j < News24HData.Count; j++) {
							if(packetData[i].NewsID == News24HData[j].NewsID) {
								News24HData[j] = packetData[i];
								break;
							}
							if(packetData[i].NewsID > News24HData[j].NewsID) {
								News24HData.Insert(j, packetData[i]);
								break;
							}
						}
					}
				}
                
            }
            else if(dataPacket is ResImportantNewsDataPacket)
            {
                SetNewsData((ResImportantNewsDataPacket)dataPacket);
            }
            else if (dataPacket is ResNewsFlashDataPacket)
            {
                SetNewsData((ResNewsFlashDataPacket)dataPacket);
            }
            else if (dataPacket is ResNews24HOrgDataPacket)
            {
                SetNewsData((ResNews24HOrgDataPacket)dataPacket);
            }
        }

        private void SetNewsData(ResNews24HOrgDataPacket dataPacket)
        {
            List<OneNews24HOrgDataRec> packetData = (dataPacket).News24HData;
            if (News24HOrgData.Count == 0)
            {
                News24HOrgData = packetData;
            }
            else
            {
                long memDate = 0;
                long packetDate = 0;
                int insertIndex = -1;
                bool isAddEnd = false;
                for (int i = 0; i < packetData.Count; i++)
                {
                    isAddEnd = false;
                    packetDate = ((long)packetData[i].PublishDate) * 1000000 + packetData[i].PublishTime;
                    bool isSame = false;
                    for (int k = 0; k < News24HOrgData.Count; k++)
                    {
                        if (packetData[i].InfoCode == News24HOrgData[k].InfoCode)
                        {
                            isSame = true;
                            break;
                        }
                        
                    }
                    if (!isSame)
                    {
                        for (int j = 0; j < News24HOrgData.Count; j++)
                        {
                            memDate = ((long)News24HOrgData[j].PublishDate * 1000000) + News24HOrgData[j].PublishTime;
                            if (packetDate >= memDate)
                            {
                                insertIndex = j;
                                break;
                            }
                            if (j == News24HOrgData.Count - 1)
                            {
                                isAddEnd = true;
                                insertIndex = -1;
                            }

                        }
                        if (insertIndex >= 0)
                            News24HOrgData.Insert(insertIndex, packetData[i]);
                        else if (isAddEnd)
                        {
                            News24HOrgData.AddRange(packetData.GetRange(i, packetData.Count - i));
                            break;
                        }
                    }
                }
            }
        }

        private void SetNewsData(ResImportantNewsDataPacket dataPacket)
        {
            List<OneNews24HOrgDataRec> packetData = (dataPacket).News24HData;
            if (ImportantNewsData.Count == 0)
            {
                ImportantNewsData = packetData;
            }
            else
            {
                long memDate = 0;
                long packetDate = 0;
                int insertIndex = -1;
                bool isAddEnd = false;
                for (int i = 0; i < packetData.Count; i++)
                {
                    isAddEnd = false;
                    packetDate = ((long)packetData[i].PublishDate) * 1000000 + packetData[i].PublishTime;
                    for (int j = 0; j < ImportantNewsData.Count; j++)
                    {
                        memDate = ((long)ImportantNewsData[j].PublishDate * 1000000) + ImportantNewsData[j].PublishTime;
                        if (packetData[i].InfoCode == ImportantNewsData[j].InfoCode)
                        {
                            insertIndex = -1;
                            break;
                        }
                        if (packetDate >= memDate)
                        {
                            insertIndex = j;
                            break;
                        }
                        if (j == ImportantNewsData.Count - 1)
                        {
                            isAddEnd = true;
                            insertIndex = -1;
                        }

                    }
                    if (insertIndex >= 0)
                        ImportantNewsData.Insert(insertIndex, packetData[i]);
                    else if (isAddEnd)
                    {
                        ImportantNewsData.AddRange(packetData.GetRange(i, packetData.Count - i));
                        break;
                    }
                }
            }
        }

        private void SetNewsData(ResNewsFlashDataPacket dataPacket)
        {
            List<OneNews24HOrgDataRec> packetData = (dataPacket).News24HData;
            if (NewsFlashData.Count == 0)
            {
                NewsFlashData = packetData;
            }
            else
            {
                long memDate = 0;
                long packetDate = 0;
                int insertIndex = -1;
                bool isAddEnd = false;
                for (int i = 0; i < packetData.Count; i++)
                {
                    isAddEnd = false;
                    packetDate = ((long)packetData[i].PublishDate) * 1000000 + packetData[i].PublishTime;
                    for (int j = 0; j < NewsFlashData.Count; j++)
                    {
                        memDate = ((long)NewsFlashData[j].PublishDate * 1000000) + NewsFlashData[j].PublishTime;
                        if (packetData[i].InfoCode == NewsFlashData[j].InfoCode)
                        {
                            insertIndex = -1;
                            break;
                        }
                        if (packetDate >= memDate)
                        {
                            insertIndex = j;
                            break;
                        }
                        if (j == NewsFlashData.Count - 1)
                        {
                            isAddEnd = true;
                            insertIndex = -1;
                        }

                    }
                    if (insertIndex >= 0)
                        NewsFlashData.Insert(insertIndex, packetData[i]);
                    else if (isAddEnd)
                    {
                        NewsFlashData.AddRange(packetData.GetRange(i, packetData.Count - i));
                        break;
                    }
                }
            }
        }

    }
}
