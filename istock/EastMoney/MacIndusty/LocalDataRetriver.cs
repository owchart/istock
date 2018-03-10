namespace EmMacIndustry.DAL
{
    using EmCore;
    using EmMacIndustry.Model.Constant;
    using EmMacIndustry.Model.Enum;
    using System;
    using System.Data;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    public static class LocalDataRetriver
    {
        public static bool _isReadSuccess = true;
        private static readonly string LocalStorageRootDir = Path.Combine(Application.StartupPath, "NecessaryData");
        private const string NecessaryData = "NecessaryData";

        static LocalDataRetriver()
        {
        }

        public static DataTable GetTreeCategoryTableFromFile(MacroDataType treeType)
        {
            DataTable table = null;
            try
            {
                string str = MongoDBConstant.DicTreeSource[treeType];
                string path = Path.Combine(LocalStorageRootDir, str);
                if (!File.Exists(path))
                {
                    //EMLoggerHelper.Write("File name '{0}' have not been found(path:'{1}')!", new object[] { str, path });
                    return null;
                }
                table = JSONHelper.DeserializeObject<DataSet>(File.ReadAllText(path)).Tables[0];
            }
            catch (Exception exception)
            {
                //EMLoggerHelper.Write(exception);
            }
            return table;
        }
    }
}

