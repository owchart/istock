namespace EmMacIndustry.Model.Enum
{
    using System;

    [Flags]
    public enum FindOptionFlag
    {
        None,
        CaseSensitive,
        FullText,
        All
    }
}

