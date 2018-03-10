using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EmQComm
{
    public class QuoteSortService
    {
        /// <summary>
        /// 新闻排序，按updateTime
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static void SortNews(ref List<OneNews24HDataRec> data)
        {
            data.Sort(new CompareNews());
            //return data;
        }

        #region 新版field排序
        /// <summary>
        /// 字段排序
        /// </summary>
        /// <param name="codes"></param>
        /// <param name="fieldSort"></param>
        /// <param name="sortMode"></param>
        /// <returns></returns>
        public static List<int> SortFieldValue(List<int> codes, FieldIndex fieldSort, SortMode sortMode)
        {
            FieldIndex _fieldSort = fieldSort;
            SortMode _sortMode = sortMode;

            List<int> result = new List<int>(codes.Count);
            if (sortMode == SortMode.Mode_Code)
            {
                _fieldSort = FieldIndex.Code;
                _sortMode = SortMode.Mode_ASC;
            }
            List<FieldValue<int>> fieldInt32List = null;
            List<FieldValue<float>> fieldSingleList = null;
            List<FieldValue<double>> fieldDoubleList = null;
            List<FieldValue<long>> fieldInt64List = null;
            List<FieldValue<string>> fieldStringList = null;

            if (_fieldSort >= 0 && (int)_fieldSort <= 299)
                fieldInt32List = new List<FieldValue<int>>(codes.Count);
            else if ((int)_fieldSort >= 300 && (int)_fieldSort <= 799)
                fieldSingleList = new List<FieldValue<float>>(codes.Count);
            else if ((int)_fieldSort >= 800 && (int)_fieldSort <= 999)
                fieldDoubleList = new List<FieldValue<double>>(codes.Count);
            else if ((int)_fieldSort >= 1000 && (int)_fieldSort <= 1199)
                fieldInt64List = new List<FieldValue<long>>(codes.Count);
            else if ((int)_fieldSort >= 1200 && (int)_fieldSort <= 8999)
               fieldStringList = new List<FieldValue<string>>(codes.Count);

            foreach (int code  in codes)
            {
                if (fieldInt32List != null)
                {
                    Dictionary<FieldIndex, int> fieldInt32;
                    if (DetailData.FieldIndexDataInt32.TryGetValue(code, out fieldInt32))
                    {
                        int intValue;
                        if (!fieldInt32.TryGetValue(fieldSort, out intValue))
                            intValue = 0;
                        fieldInt32List.Add(new FieldValue<int>(code, intValue));
                    }
                    else
                        fieldInt32List.Add(new FieldValue<int>(code, 0));

                }
                else if (fieldSingleList != null)
                {
                    Dictionary<FieldIndex, float> fieldSingle;
                    if (DetailData.FieldIndexDataSingle.TryGetValue(code, out fieldSingle))
                    {
                        float singleValue;
                        if (!fieldSingle.TryGetValue(fieldSort, out singleValue))
                            singleValue = 0;
                        fieldSingleList.Add(new FieldValue<float>(code, singleValue));
                    }
                    else
                        fieldSingleList.Add(new FieldValue<float>(code, 0));
                }
                else if (fieldDoubleList != null)
                {
                    Dictionary<FieldIndex, double> fieldDouble;
                    if (DetailData.FieldIndexDataDouble.TryGetValue(code, out fieldDouble))
                    {
                        double doubleValue;
                        if (!fieldDouble.TryGetValue(fieldSort, out doubleValue))
                            doubleValue = 0;
                        fieldDoubleList.Add(new FieldValue<double>(code, doubleValue));
                    }
                    else
                        fieldDoubleList.Add(new FieldValue<double>(code, 0));
                }
                else if (fieldInt64List != null)
                {
                    Dictionary<FieldIndex, long> fieldInt64;
                    if (DetailData.FieldIndexDataInt64.TryGetValue(code, out fieldInt64))
                    {
                        long int64Value;
                        if (!fieldInt64.TryGetValue(fieldSort, out int64Value))
                            int64Value = 0;
                        fieldInt64List.Add(new FieldValue<long>(code, int64Value));
                    }
                    else
                        fieldInt64List.Add(new FieldValue<long>(code, 0));
                }
                else if (fieldStringList != null)
                {
                    Dictionary<FieldIndex, string> fieldString;
                    if (DetailData.FieldIndexDataString.TryGetValue(code, out fieldString))
                    {
                        string stringValue;
                        if (!fieldString.TryGetValue(fieldSort, out stringValue))
                            stringValue = string.Empty;
                        fieldStringList.Add(new FieldValue<string>(code, stringValue));
                    }
                    else
                        fieldStringList.Add(new FieldValue<string>(code, string.Empty));
                }
            }

            if(fieldInt32List != null)
                fieldInt32List.Sort(new CompareFieldValue<int>());
            else if(fieldSingleList != null)
                fieldSingleList.Sort(new CompareFieldValue<float>());
            else if(fieldDoubleList != null)
                fieldDoubleList.Sort(new CompareFieldValue<double>());
            else if(fieldInt64List != null)
                fieldInt64List.Sort(new CompareFieldValue<long>());
            else if(fieldStringList != null)
                fieldStringList.Sort(new CompareFieldValue<string>());

            if (_sortMode == SortMode.Mode_ASC)
            {
                if (fieldInt32List != null)
                {
                    for (int i = 0; i < fieldInt32List.Count; i++)
                        result.Add(fieldInt32List[i].Code);
                }
                else if (fieldSingleList != null)
                {
                    for (int i = 0; i < fieldSingleList.Count; i++)
                        result.Add(fieldSingleList[i].Code);
                }
                else if (fieldDoubleList != null)
                {
                    for (int i = 0; i < fieldDoubleList.Count; i++)
                        result.Add(fieldDoubleList[i].Code);
                }
                else if (fieldInt64List != null)
                {
                    for (int i = 0; i < fieldInt64List.Count; i++)
                        result.Add(fieldInt64List[i].Code);
                }
                else if (fieldStringList != null)
                {
                    for (int i = 0; i < fieldStringList.Count; i++)
                        result.Add(fieldStringList[i].Code);
                }
                
            }
            else if (_sortMode == SortMode.Mode_DESC)
            {
                if (fieldInt32List != null)
                {
                    for (int i = fieldInt32List.Count - 1; i >= 0; i--)
                        result.Add(fieldInt32List[i].Code);
                }
                else if (fieldSingleList != null)
                {
                    for (int i = fieldSingleList.Count - 1; i >= 0; i--)
                        result.Add(fieldSingleList[i].Code);
                }
                else if (fieldDoubleList != null)
                {
                    for (int i = fieldDoubleList.Count - 1; i >= 0; i--)
                        result.Add(fieldDoubleList[i].Code);
                }
                else if (fieldInt64List != null)
                {
                    for (int i = fieldInt64List.Count - 1; i >= 0; i--)
                        result.Add(fieldInt64List[i].Code);
                }
                else if (fieldStringList != null)
                {
                    for (int i = fieldStringList.Count - 1; i >= 0; i--)
                        result.Add(fieldStringList[i].Code);
                }
            }
            return result;
        }

        /// <summary>
        /// FieldValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class FieldValue<T>
        {
            public int Code;
            public T Data;
            public string ShortCode;

            public FieldValue(int code, T data)
            {
                Code = code;
                Data = data;
                Dictionary<FieldIndex, string> fieldStirng;
                if (DetailData.FieldIndexDataString.TryGetValue(code, out fieldStirng))
                {
                    fieldStirng.TryGetValue(FieldIndex.Code, out ShortCode);
                }
            }

        }

        /// <summary>
        /// 比较两个fieldValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class CompareFieldValue<T> : IComparer<FieldValue<T>>
        {
            /// <summary>
            /// 比较
            /// </summary>
            /// <param name="obj1"></param>
            /// <param name="obj2"></param>
            /// <returns></returns>
            public int Compare(FieldValue<T> obj1, FieldValue<T> obj2)
            {
                try
                {
                    int first = System.Collections.Comparer.Default.Compare(obj1.Data, obj2.Data);
                    if (first == 0)
                        return System.Collections.Comparer.Default.Compare(obj1.ShortCode, obj2.ShortCode);
                    return first;
                }
                catch (Exception e)
                {
                    return 0;
                }
            }
        }
        #endregion

        #region 旧版排序，废除
        /// <summary>
        /// SortFieldValue
        /// </summary>
        /// <param name="codes"></param>
        /// <param name="datas"></param>
        /// <param name="fieldSort"></param>
        /// <param name="sortMode"></param>
        public static List<int> SortFieldValue(List<int> codes, Dictionary<int, Dictionary<FieldIndex, object>> datas,
                                               FieldIndex fieldSort, SortMode sortMode)
        {
            DateTime dtStart = DateTime.Now;
            FieldIndex _fieldSort = fieldSort;
            SortMode _sortMode = sortMode;

            List<int> result = new List<int>(codes.Count);
            if (sortMode == SortMode.Mode_Code)
            {
                _fieldSort = FieldIndex.Code;
                _sortMode = SortMode.Mode_ASC;
            }


            List<OneFieldValue> oneFieldValues = new List<OneFieldValue>(codes.Count);
            List<int> codeNoFind = new List<int>();

            foreach (int code in codes)
            {
                Dictionary<FieldIndex, object> indexValue;
                if (datas.TryGetValue(code, out indexValue))
                {
                    OneFieldValue oneFieldValue = new OneFieldValue();
                    oneFieldValue.Code = code;

                    object objectValue;
                    if (indexValue.TryGetValue(_fieldSort, out objectValue))
                        oneFieldValue.FieldValue = objectValue;
                    else
                    {
                        if ((int)_fieldSort >= 0 && (int)_fieldSort <= 299)//int
                        {
                            oneFieldValue.FieldValue = (int)0;
                        }
                        else if ((int)_fieldSort >= 300 && (int)_fieldSort <= 799)//float
                        {
                            oneFieldValue.FieldValue = (float)0.0f;
                        }
                        else if ((int)_fieldSort >= 800 && (int)_fieldSort <= 999)//double
                        {
                            oneFieldValue.FieldValue = (double)0;
                        }
                        else if ((int)_fieldSort >= 1000 && (int)_fieldSort <= 1199)
                        {
                            oneFieldValue.FieldValue = (long)0;
                        }
                        else
                        {
                            oneFieldValue.FieldValue = string.Empty;
                        }
                    }
                    object objectCode;
                    if (indexValue.TryGetValue(FieldIndex.Code, out objectCode))
                        oneFieldValue.ShortCode = objectCode;
                    oneFieldValues.Add(oneFieldValue);
                }
                else
                {
                    codeNoFind.Add(code);
                }
            }

            oneFieldValues.Sort(new CompareCodeFieldValue(_fieldSort));


            if (_sortMode == SortMode.Mode_ASC)
            {
                for (int i = 0; i < oneFieldValues.Count; i++)
                    result.Add(oneFieldValues[i].Code);
            }
            else if (_sortMode == SortMode.Mode_DESC)
            {
                for (int i = oneFieldValues.Count - 1; i >= 0; i--)
                    result.Add(oneFieldValues[i].Code);
            }

            TimeSpan ts = DateTime.Now - dtStart;
            Debug.Print("sort ：" + ts.TotalMilliseconds);
            return result;
        }

        public static List<Dictionary<FieldIndex, object>> SortFieldValue(List<Dictionary<FieldIndex, object>> datas, FieldIndex fieldSort, SortMode sortMode)
        {
            if (datas == null || datas.Count == 0)
            {
                return datas;
            }

            //datas.Sort(new CompareFieldIndexValue(fieldSort, sortMode));
            //return datas;

            if (sortMode == SortMode.Mode_ASC)
            {
                datas.Sort(
                    delegate(Dictionary<FieldIndex, object> a, Dictionary<FieldIndex, object> b)
                    {
                        if (!a.ContainsKey(fieldSort) && !b.ContainsKey(fieldSort))
                        {
                            return 0;
                        }
                        else if (!a.ContainsKey(fieldSort) && b.ContainsKey(fieldSort))
                        {
                            return -1;
                        }
                        else if (a.ContainsKey(fieldSort) && !b.ContainsKey(fieldSort))
                        {
                            return 1;
                        }
                        else
                        {
                            return System.Collections.Comparer.Default.Compare(a[fieldSort], b[fieldSort]);
                        }
                    }
                    );
            }
            else if (sortMode == SortMode.Mode_DESC)
            {
                datas.Sort(
                    delegate(Dictionary<FieldIndex, object> a, Dictionary<FieldIndex, object> b)
                    {
                        if (!a.ContainsKey(fieldSort) && !b.ContainsKey(fieldSort))
                        {
                            return 0;
                        }
                        else if (!a.ContainsKey(fieldSort) && b.ContainsKey(fieldSort))
                        {
                            return 1;
                        }
                        else if (a.ContainsKey(fieldSort) && !b.ContainsKey(fieldSort))
                        {
                            return -1;
                        }
                        else
                        {
                            return System.Collections.Comparer.Default.Compare(a[fieldSort], b[fieldSort]) * -1;
                        }
                    }
                    );
            }
            return datas;
        }

        /// <summary>
        /// OneFieldValue
        /// </summary>
        public class OneFieldValue
        {
            private int _code;
            private object _filedValue;
            private object _shorCode;
            /// <summary>
            /// 内码
            /// </summary>
            public int Code
            {
                get { return _code; }
                set { this._code = value; }
            }

            /// <summary>
            /// 值
            /// </summary>
            public object FieldValue
            {
                get { return _filedValue; }
                set { this._filedValue = value; }
            }

            /// <summary>
            /// 代码
            /// </summary>
            public object ShortCode
            {
                get { return _shorCode; }
                set { this._shorCode = value; }
            }
        }

        /// <summary>
        /// CompareCodeFieldValue
        /// </summary>
        public class CompareCodeFieldValue : IComparer<OneFieldValue>
        {
            private readonly FieldIndex _field;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="field"></param>
            public CompareCodeFieldValue(FieldIndex field)
            {
                _field = field;
            }

            /// <summary>
            /// Compare
            /// </summary>
            /// <param name="obj1"></param>
            /// <param name="obj2"></param>
            /// <returns></returns>
            public int Compare(OneFieldValue obj1, OneFieldValue obj2)
            {
                try
                {
                    int first = System.Collections.Comparer.Default.Compare(obj1.FieldValue, obj2.FieldValue);
                    if (first == 0)
                        return System.Collections.Comparer.Default.Compare(obj1.ShortCode, obj2.ShortCode);
                    return first;
                }
                catch (Exception e)
                {

                    return 0;
                }

                //if (Convert.ToInt32(_field) >= 0 && Convert.ToInt32(_field) < 300)
                //{
                //    int f1 = Convert.ToInt32(obj1.FieldValue);
                //    int f2 = Convert.ToInt32(obj2.FieldValue);

                //    CompareTwoItem<int> compareFloat = new CompareTwoItem<int>(f1, f2);
                //    return compareFloat.CompareTwo();
                //}
                //if (Convert.ToInt32(_field) >= 300 && Convert.ToInt32(_field) < 800)
                //{
                //    float f1 = Convert.ToSingle(obj1.FieldValue);
                //    float f2 = Convert.ToSingle(obj2.FieldValue);
                //    CompareTwoItem<float> compareFloat = new CompareTwoItem<float>(f1, f2);
                //    return compareFloat.CompareTwo();
                //}
                //if (Convert.ToInt32(_field) >= 800 && Convert.ToInt32(_field) < 1000)
                //{
                //    double f1 = Convert.ToDouble(obj1.FieldValue);
                //    double f2 = Convert.ToDouble(obj2.FieldValue);
                //    CompareTwoItem<double> compareFloat = new CompareTwoItem<double>(f1, f2);
                //    return compareFloat.CompareTwo();
                //}
                //if (Convert.ToInt32(_field) >= 1000 && Convert.ToInt32(_field) < 1200)
                //{
                //    long f1 = Convert.ToInt64(obj1.FieldValue);
                //    long f2 = Convert.ToInt64(obj2.FieldValue);
                //    CompareTwoItem<long> compareFloat = new CompareTwoItem<long>(f1, f2);
                //    return compareFloat.CompareTwo();
                //}
                //if (Convert.ToInt32(_field) >= 1200)
                //{
                //    string f1 = Convert.ToString(obj1.FieldValue);
                //    string f2 = Convert.ToString(obj2.FieldValue);
                //    CompareTwoItem<string> compareFloat = new CompareTwoItem<string>(f1, f2);
                //    return compareFloat.CompareTwo();
                //}
                //return 0;
            }
        }
        #endregion
    }


  

    /// <summary>
    /// CompareTwoItem
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CompareTwoItem<T> where T : IComparable
    {
        private readonly T _t1;
        private readonly T _t2;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        public CompareTwoItem(T t1, T t2)
        {
            _t1 = t1;
            _t2 = t2;
        }

        /// <summary>
        /// CompareTwo
        /// </summary>
        /// <returns></returns>
        public int CompareTwo()
        {
            return System.Collections.Comparer.Default.Compare(_t1, _t2);
        }

    }

    /// <summary>
    /// 24小时新闻
    /// </summary>
    public class CompareNews : IComparer<OneNews24HDataRec>
    {

        /// <summary>
        /// 比较
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="obj2"></param>
        /// <returns></returns>
        public int Compare(OneNews24HDataRec obj1, OneNews24HDataRec obj2)
        {
            return System.Collections.Comparer.Default.Compare(obj1.PublishTime, obj1.PublishTime) * -1;
        }
    }

    public class CompareFieldIndexValue : IComparer<Dictionary<FieldIndex, object>>
    {
        FieldIndex _SortIndex;
        SortMode _SortMode;

        public CompareFieldIndexValue(FieldIndex sortIndex, SortMode sortMode)
        {
            _SortIndex = sortIndex;
            _SortMode = sortMode;
        }

        public int Compare(Dictionary<FieldIndex, object> a, Dictionary<FieldIndex, object> b)
        {
            if (_SortMode == SortMode.Mode_ASC)
            {
                if (!a.ContainsKey(_SortIndex) && !b.ContainsKey(_SortIndex))
                {
                    return 0;
                }
                else if (!a.ContainsKey(_SortIndex) && b.ContainsKey(_SortIndex))
                {
                    return -1;
                }
                else if (a.ContainsKey(_SortIndex) && !b.ContainsKey(_SortIndex))
                {
                    return 1;
                }
                else
                {
                    int t = System.Collections.Comparer.Default.Compare(a[_SortIndex], b[_SortIndex]);
                    return t;
                }
            }
            else if (_SortMode == SortMode.Mode_DESC)
            {
                if (!a.ContainsKey(_SortIndex) && !b.ContainsKey(_SortIndex))
                {
                    return 0;
                }
                else if (!a.ContainsKey(_SortIndex) && b.ContainsKey(_SortIndex))
                {
                    return 1;
                }
                else if (a.ContainsKey(_SortIndex) && !b.ContainsKey(_SortIndex))
                {
                    return -1;
                }
                else
                {
                    return System.Collections.Comparer.Default.Compare(a[_SortIndex], b[_SortIndex]) * -1;
                }
            }
            else
            {
                return 0;
            }
        }
    }




}
