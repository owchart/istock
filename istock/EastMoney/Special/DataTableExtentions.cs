using System.Collections.Generic;
using System.Data;

namespace OwLib
{
    /// <summary>
    /// 数据表逻辑处理
    /// </summary>
    public class DataTableExtentions
    {
        /// <summary>
        /// 将datarow拷贝到datatable中
        /// </summary>
        /// <param name="dataRows">datarow数组</param>
        /// <returns>拷贝完成的datatable</returns>
        public static DataTable CopyToDataTable(DataRow[] dataRows)
        {
            if (dataRows == null || dataRows.Length == 0)
                return null;
            DataTable dataTable = dataRows[0].Table.Clone();
            for (int i = 0; i < dataRows.Length; i++)
            {
                DataRow drTemp = dataTable.NewRow();
                drTemp.ItemArray = dataRows[i].ItemArray;
                dataTable.Rows.Add(drTemp);
            }
            dataTable.AcceptChanges();
            return dataTable;
        }

        /// <summary>
        /// 将数据表按行转换成数据行数组
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <returns>数据行数组</returns>
        public static DataRow[] DataTableToArray(DataTable dataTable)
        {
            int count = dataTable.Rows.Count;
            DataRow[] dataRows = new DataRow[count];
            for (int i = 0; i < count; i++)
            {
                dataRows[i] = dataTable.Rows[i];
            }
            return dataRows;
        }

        /// <summary>
        /// 将一个数据表融合到另一个数据表中
        /// </summary>
        /// <param name="Source">要融合的数据表</param>
        /// <param name="target">融合后的数据表</param>
        public static void MergeDataTable(DataTable Source, DataTable target)
        {
            //target.BeginLoadData();
            foreach (DataRow dataRow in Source.Rows)
            {
                DataRow data = target.NewRow();
                data.ItemArray = dataRow.ItemArray;
                target.Rows.Add(data);
            }
            target.AcceptChanges();
            // target.EndLoadData();
 
            CommonService.DisposeDataTable(Source);

        }

        /// <summary>
        /// 按照具体规则重新构建数据表
        /// </summary>
        /// <param name="dataTable">源数据表</param>
        /// <param name="count">构建完成的数据表的最大行数</param>
        /// <param name="blnSumn">是否要总计</param>
        /// <param name="columnnames">列名数组</param>
        /// <param name="strAxisX">X轴名称数组</param>
        /// <param name="strOterCaption">总计行的x轴名称</param>
        /// <param name="blnDestion"></param>
        /// <returns>新的数据表</returns>
        public static DataTable CopyToNewTable(DataTable dataTable, int count, bool blnSumn, List<string> columnnames,
                                               string strAxisX, string strOterCaption, bool blnDestion)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
                return dataTable;
            if (dataTable.Rows.Count <= count)
                return dataTable;
            if (blnDestion)
                return CopyToNewTableASC(dataTable, count, blnSumn, columnnames, strAxisX, strOterCaption);
            else
            {
                return CopyToNewTableDESC(dataTable, count, blnSumn, columnnames);
            }

        }

        /// <summary>
        /// DataTable 逆转
        /// </summary>
        /// <param name="dt">源数据表</param>
        /// <returns>逆转完成的数据表</returns>
        public static DataTable ReverseDataTable(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0)
                return dt;
            DataTable taget = dt.Clone();
            for (int i = dt.Rows.Count - 1; i > -1; i--)
            {
                DataRow dr = taget.NewRow();
                dr.ItemArray = dt.Rows[i].ItemArray;
                taget.Rows.Add(dr);
            }
            taget.AcceptChanges();
            return taget;
        }

        /// <summary>
        /// 按照升序重新构建数据表
        /// </summary>
        /// <param name="dataTable">源数据表</param>
        /// <param name="count">最大数据行数</param>
        /// <param name="blnSumn">是否总计</param>
        /// <param name="columnnames">数据列名数组</param>
        /// <param name="strAxisX">x轴名称数组</param>
        /// <param name="strOterCaption">总计行的x轴名称</param>
        /// <returns>拷贝完成的数据表</returns>
        public static DataTable CopyToNewTableASC(DataTable dataTable, int count, bool blnSumn, List<string> columnnames,
                                                  string strAxisX, string strOterCaption)
        {
            List<string> l = new List<string>();

            DataTable rDataTable = dataTable.Clone();
            for (int i = 0; i < count; i++)
            {
                DataRow dr = rDataTable.NewRow();
                dr.ItemArray = dataTable.Rows[i].ItemArray;
                rDataTable.Rows.Add(dr);
            }
            DataRow drSum = rDataTable.NewRow();
            if (blnSumn && columnnames != null && columnnames.Count > 0)
            {
                foreach (var columnname in columnnames)
                {
                    double values = 0;

                    for (int i = count - 1; i < dataTable.Rows.Count; i++)
                    {

                        DataRow row = dataTable.Rows[i];
                        double tmp;
                        double.TryParse(row[columnname].ToString(), out tmp);
                        values = values + tmp;
                    }
                    drSum[columnname] = values;

                }
                drSum[strAxisX] = strOterCaption;
                rDataTable.Rows.Add(drSum);
            }
            return rDataTable;
        }

        /// <summary>
        /// 按照降序重新构建的数据表
        /// </summary>
        /// <param name="dataTable">源数据表</param>
        /// <param name="count">行总数</param>
        /// <param name="blnSumn">是否总计</param>
        /// <param name="columnnames">X轴名称数组</param>
        /// <returns>构建完成的数据表</returns>
        public static DataTable CopyToNewTableDESC(DataTable dataTable, int count, bool blnSumn,
                                                   List<string> columnnames)
        {
            DataTable rDataTable = dataTable.Clone();
            int rowindex = dataTable.Rows.Count - count;
            for (int i = dataTable.Rows.Count - 1; i >= rowindex; i--)
            {
                DataRow dr = rDataTable.NewRow();
                dr.ItemArray = dataTable.Rows[i].ItemArray;
                rDataTable.Rows.Add(dr);
            }
            DataRow drSum = rDataTable.NewRow();
            if (blnSumn && columnnames != null && columnnames.Count > 0)
            {
                foreach (var columnname in columnnames)
                {
                    double values = 0;

                    for (int i = 0; i < rowindex; i++)
                    {
                        DataRow row = dataTable.Rows[i];
                        double tmp;
                        double.TryParse(row[columnname].ToString(), out tmp);
                        values = values + tmp;
                    }
                    drSum[columnname] = values;

                }
                //  drSum[strAxisX] = strOterCaption;
                rDataTable.Rows.Add(drSum);
            }
            return rDataTable;
        }
    }
}
