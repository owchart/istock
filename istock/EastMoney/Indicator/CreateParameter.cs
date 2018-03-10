using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using EmCore;

namespace OwLib
{
    public class ParameterTypeCompare : IComparer<ParameterType>
    {
        public int Compare(ParameterType x, ParameterType y)
        {
            return x.TYPENAME.CompareTo(y.TYPENAME);
        }
    }

    public class CreateParameter
    {
        private static Dictionary<long, ParameterType> paramType = GetParamType();

        public static List<ParameterType> GetDataSourceParamType(String typeCode)
        {
            String path = DataCenter.GetAppPath() + "\\config\\ParameterType.xml";
            List<ParameterType> list = (List<ParameterType>)XmlConvertor.Deserialize(typeof(List<ParameterType>), path);
            List<ParameterType> list2 = new List<ParameterType>();
            foreach (ParameterType type in list)
            {
                if (type.TYPECODE.ToString() == typeCode)
                {
                    list2.Add(type);
                }
            }
            return list2;
        }

        public static Dictionary<long, ParameterType> GetParamType()
        {
            if (paramType != null)
            {
                return paramType;
            }
            Dictionary<long, ParameterType> dictionary = new Dictionary<long, ParameterType>();
            String path = DataCenter.GetAppPath() + @"\\config\\ParameterType.xml";
            List<ParameterType> list = (List<ParameterType>)XmlConvertor.Deserialize(typeof(List<ParameterType>), path);
            list.Sort(new ParameterTypeCompare());
            foreach (ParameterType type in list)
            {
                if (type.TYPECODE != 0L)
                {
                    dictionary[type.TYPECODE] = type;
                }
            }
            return dictionary;
        }

        public static bool IsDropDownSet(String typeCode)
        {
            foreach (ParameterType type in paramType.Values)
            {
                if ((type.TYPECODE.ToString() == typeCode) && (type.FORMTYPE == "SELECTCOLUMN"))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsIndexFormType(String typeCode)
        {
            foreach (ParameterType type in paramType.Values)
            {
                if ((type.TYPECODE.ToString() == typeCode) && (type.FORMTYPE == "INDEX"))
                {
                    return true;
                }
            }
            return false;
        }

        public static Dictionary<long, ParameterType> ParamType
        {
            get
            {
                return paramType;
            }
            set
            {
                paramType = value;
            }
        }
    }
}
