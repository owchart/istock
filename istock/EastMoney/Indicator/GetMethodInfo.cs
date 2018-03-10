using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace OwLib
{
    public class GetMethodInfo
    {
        public static String GetToolsMethodValue(String toolsMethodInfo)
        {
            if (String.IsNullOrEmpty(toolsMethodInfo))
            {
                return String.Empty;
            }
            try
            {
                String name = toolsMethodInfo.Substring(toolsMethodInfo.LastIndexOf(".") + 1).Replace("()", "");
                String typeName = toolsMethodInfo.Substring(0, toolsMethodInfo.LastIndexOf("."));
                object obj2 = Assembly.Load(Assembly.GetExecutingAssembly().GetName().Name).CreateInstance(typeName);
                if (obj2 != null)
                {
                    MethodInfo method = typeof(TimeParameter).GetMethod(name, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance | BindingFlags.IgnoreCase);
                    if (method != null)
                    {
                        return (method.Invoke(obj2, null) as String);
                    }
                }
                return toolsMethodInfo;
            }
            catch
            {
                return toolsMethodInfo;
            }
        }
    }
}
