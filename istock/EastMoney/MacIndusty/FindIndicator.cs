using System;

namespace OwLib
{
    public class FindIndicator
    {
        public String content;
        public String dataSource = String.Empty;
        public String excludeContent = String.Empty;
        public FindContentFlag findContentFlag = FindContentFlag.All;
        public FindOptionFlag findOptionFlag;
        public MacroDataType findRange;
        public String StrfindRange;
        public String Url = String.Empty;
    }
}

