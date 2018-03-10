using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using dataquery;

namespace dataquery.indicator
{
    public class UnitDataHandle
    {
        private static List<UnitItem> unitItems;

        public static UnitNameValue GetBaseUnit(String paraString)
        {
            String[] strArray = paraString.Split(new String[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
            String str = strArray[0];
            String str2 = strArray[1];
            foreach (UnitItem item in UnitItems)
            {
                if (item.Code == str)
                {
                    foreach (UnitNameValue value2 in item.Units)
                    {
                        if (value2.Name == str2)
                        {
                            return value2;
                        }
                    }
                    break;
                }
            }
            return null;
        }

        public static UnitNameValue GetUnit(String paraString)
        {
            try
            {
                String[] strArray = paraString.Split(new String[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                String str = strArray[0];
                String text1 = strArray[1];
                String str2 = strArray[1];
                if (strArray.Length > 2)
                {
                    str2 = strArray[2];
                }
                foreach (UnitItem item in UnitItems)
                {
                    if (item.Code == str)
                    {
                        foreach (UnitNameValue value2 in item.Units)
                        {
                            if (value2.Name == str2)
                            {
                                return value2;
                            }
                        }
                        goto Label_00B9;
                    }
                }
            }
            catch (Exception)
            {
            }
        Label_00B9:
            return null;
        }

        public static UnitItem GetUnitItem(String paraString)
        {
            String str = paraString.Split(new String[] { ":" }, StringSplitOptions.RemoveEmptyEntries)[0];
            foreach (UnitItem item in UnitItems)
            {
                if (item.Code == str)
                {
                    return item;
                }
            }
            return null;
        }

        public static decimal GetUnitProportion(String paraString)
        {
            try
            {
                String[] strArray = paraString.Split(new String[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                String str = strArray[0];
                if (str.Equals("10802"))
                {
                    return 0.01M;
                }
                if (str.Equals("10803"))
                {
                    return 0.001M;
                }
                String str2 = strArray[1];
                String str3 = strArray[1];
                if (strArray.Length > 2)
                {
                    str3 = strArray[2];
                }
                UnitNameValue value2 = null;
                UnitNameValue value3 = null;
                foreach (UnitItem item in UnitItems)
                {
                    if (item.Code == str)
                    {
                        foreach (UnitNameValue value4 in item.Units)
                        {
                            if (value4.Name == str2)
                            {
                                value2 = value4;
                            }
                            if (value4.Name == str3)
                            {
                                value3 = value4;
                            }
                        }
                        break;
                    }
                }
                return (value3.GetValue() / value2.GetValue());
            }
            catch
            {
                return 1M;
            }
        }

        public static List<UnitItem> UnitItems
        {
            get
            {
                if (unitItems == null)
                {
                    unitItems = new List<UnitItem>();
                    String path = DataCenter.GetAppPath()+ "\\config\\UnitType.xml";
                    if (File.Exists(path))
                    {
                        XmlDocument document = new XmlDocument();
                        document.Load(path);
                        foreach (XmlNode node2 in document.DocumentElement.ChildNodes)
                        {
                            if (!(node2.Name.ToUpper() == "UNITTYPE"))
                            {
                                continue;
                            }
                            String code = String.Empty;
                            String text = String.Empty;
                            String str5 = String.Empty;
                            foreach (XmlAttribute attribute in node2.Attributes)
                            {
                                String str9 = attribute.Name.ToUpper();
                                if (str9 != null)
                                {
                                    if (!(str9 == "CODE"))
                                    {
                                        if (str9 == "DESC")
                                        {
                                            goto Label_0140;
                                        }
                                        if (str9 == "VALUE")
                                        {
                                            goto Label_014B;
                                        }
                                    }
                                    else
                                    {
                                        code = attribute.Value;
                                    }
                                }
                                continue;
                            Label_0140:
                                text = attribute.Value;
                                continue;
                            Label_014B:
                                str5 = attribute.Value;
                            }
                            String[] strArray = str5.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                            List<UnitNameValue> units = new List<UnitNameValue>();
                            if ((strArray != null) && (strArray.Length > 0))
                            {
                                for (int i = 0; i < strArray.Length; i++)
                                {
                                    String[] strArray2 = strArray[i].Split(new String[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                                    if ((strArray2 != null) && (strArray2.Length > 0))
                                    {
                                        String name = strArray2[0];
                                        String str8 = strArray2[1];
                                        units.Add(new UnitNameValue(name, str8));
                                    }
                                }
                            }
                            UnitItem item = new UnitItem(code, text, units);
                            unitItems.Add(item);
                        }
                    }
                }
                return unitItems;
            }
        }
    }
}
