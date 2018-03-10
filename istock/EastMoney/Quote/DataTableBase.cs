using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    /// <summary>
    /// DataTableBase
    /// </summary>
    public class DataTableBase
    {
        /// <summary>
        /// Dc
        /// </summary>
        public DataCenterCore Dc;

        /// <summary>
        /// 设置内存数据
        /// </summary>
        /// <param name="dataPacket"></param>
        public virtual void SetData(DataPacket dataPacket){}

        /// <summary>
        /// dc初始化时，获得静态数据
        /// </summary>
        public virtual void GetStatciData(){}

        /// <summary>
        /// 清空内存数据
        /// </summary>
        public virtual void ClearData(InitOrgStatus status){}
    }
}
