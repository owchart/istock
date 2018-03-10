namespace EmMacIndustry.Entity
{
    using EmMacIndustry.Model.Enum;
    using System;

    public class FindIndicator
    {
        public string content;
        public string dataSource = string.Empty;
        public string excludeContent = string.Empty;
        public FindContentFlag findContentFlag = FindContentFlag.All;
        public FindOptionFlag findOptionFlag;
        public MacroDataType findRange;
        public string StrfindRange;
        public string Url = string.Empty;
    }
}

