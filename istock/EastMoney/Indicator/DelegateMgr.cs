using System;
using System.Collections.Generic;
using System.Text;

namespace OwLib
{
    public class DelegateMgr2
    {
        public delegate void BlockTreeDataChangedHandler(String sysBlockTreeStr, String userBlockTreeStr, String categorys);

        public delegate void CommonDataReceiveHandler(object data);

        public delegate object QueryAllBrokersHandler(int type);

        public delegate String UploadFileHandler(String id, String filePath);
    }
}
