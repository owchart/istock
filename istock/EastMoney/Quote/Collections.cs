using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace OwLib
{
    /// <summary>
    /// 键值集合类
    /// </summary>
    public sealed class KeyValueCollection
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public KeyValueCollection()
        {
            KeyValues = new KeyObjectCollection();
        }
        /// <summary>
        /// 清空集合
        /// </summary>
        public void Clear()
        {
            KeyValues.Clear();
        }
        /// <summary>
        /// 集合数量
        /// </summary>
        public int Count
        {
            get
            {
                return KeyValues.Count;
            }
        }
        /// <summary>
        /// 添加键和值
        /// </summary>
        public int Add(string key, string value)
        {
            return KeyValues.Add(key, value);
        }
        /// <summary>
        /// 插入键和值
        /// </summary>
        public void Insert(int index, string key, string value)
        {
            KeyValues.Insert(index, key, value);
        }
        /// <summary>
        /// 删除指定键的项
        /// </summary>
        public bool RemoveByKey(string key)
        {
            return KeyValues.RemoveByKey(key);
        }
        /// <summary>
        /// 删除指定值的项
        /// </summary>
        public void RemoveByValue(string value)
        {
            KeyValues.RemoveByObject(value);
        }
        /// <summary>
        /// 删除指定位置的元素
        /// </summary>
        public void RemoveAt(int index)
        {
            KeyValues.RemoveAt(index);
        }
        /// <summary>
        /// 由键获取值
        /// </summary>
        public string GetValue(string key)
        {
            return (string)(KeyValues.GetObject(key));
        }
        /// <summary>
        /// 设置对应键对应的值
        /// </summary>
        public bool SetValue(string key, string value)
        {
            return KeyValues.SetObject(key, value);
        }

        /// <summary>
        /// 获取对应键的索引号
        /// </summary>
        public int IndexOf(string key)
        {
            return KeyValues.IndexOf(key);
        }

        /// <summary>
        /// 由索引号获取键
        /// </summary>
        public string GetKey(int index)
        {
            return KeyValues.GetKey(index);
        }

        /// <summary>
        /// 由索引值获取值
        /// </summary>
        public string GetValue(int index)
        {
            return (string)KeyValues.GetObject(index);
        }

        /// <summary>
        /// 逆序从索引开始指定数量的集合
        /// </summary>
        public void Reverse(int index, int count)
        {
            KeyValues.Reverse(index, count);
        }

        private KeyObjectCollection KeyValues;
    }
    /// <summary>
    /// 继承自object的对象集合类
    /// </summary>
    public sealed class KeyObjectCollection : KeyObjectCollection<object>
    {
    }

    /// <summary>
    /// 存放KeyValueObject对象集合的类
    /// </summary>
    public class KeyObjectCollection<T>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public KeyObjectCollection()
        {
            KeyObjects = new ArrayList();
        }
        /// <summary>
        /// 清空集合
        /// </summary>
        public void Clear()
        {
            KeyObjects.Clear();
        }

        /// <summary>
        /// 集合数量
        /// </summary>
        public int Count
        {
            get
            {
                return KeyObjects.Count;
            }
        }

        /// <summary>
        /// 将键和值作为整体添加到集合中
        /// </summary>
        public int Add(string key, T obj)
        {
            return KeyObjects.Add(new KeyValueObject<T>(key, obj));
        }

        /// <summary>
        /// 将键和值作为整体插入到集合中的指定位置
        /// </summary>
        public void Insert(int index, string key, T obj)
        {
            KeyObjects.Insert(index, new KeyValueObject<T>(key, obj));
        }

        /// <summary>
        /// 从集合中删除有指定键的元素
        /// </summary>
        public bool RemoveByKey(string key)
        {
            bool result = false;
            int index = IndexOf(key);
            if (index >= 0)
            {
                KeyObjects.RemoveAt(index);
                result = true;
            }
            return result;
        }
        /// <summary>
        /// 从集合中删除有指定值的元素
        /// </summary>
        public bool RemoveByObject(T obj)
        {
            bool Result = false;
            object obj1 = obj;
            for (int i = Count - 1; i > 0; i--)
            {
                object obj2 = GetObject(i);
                if (obj1 == obj2)
                {
                    RemoveAt(i);
                    Result = true;
                }
            }
            return Result;
        }
        /// <summary>
        /// 从集合中删除有指定索引号的元素
        /// </summary>
        public void RemoveAt(int index)
        {
            KeyObjects.RemoveAt(index);
        }

        /// <summary>
        /// 从集合中获取拥有指定键的元素的值
        /// </summary>
        public T GetObject(string key)
        {
            T result = default(T);
            int index = IndexOf(key);
            if (index >= 0)
                result = ((KeyValueObject<T>)KeyObjects[index]).Value;
            return result;
        }
        /// <summary>
        /// 从集合中设置拥有指定键的元素的值
        /// </summary>
        public bool SetObject(string key, T obj)
        {
            bool result = false;
            int index = IndexOf(key);
            if (index >= 0)
            {
                ((KeyValueObject<T>)KeyObjects[index]).Value = obj;
                result = true;
            }
            return result;
        }

        /// <summary>
        /// 从集合中获取拥有指定键的元素的索引号
        /// </summary>
        public int IndexOf(string key)
        {
            for (int result = 0; result <= KeyObjects.Count - 1; result++)
            {
                KeyValueObject<T> obj = (KeyValueObject<T>)KeyObjects[result];
                if (obj.Key == key) return result;
            }
            return -1;
        }
        /// <summary>
        /// 从集合中获取拥有指定值的元素的索引号
        /// </summary>
        public int IndexOf(T obj)
        {
            for (int result = 0; result <= KeyObjects.Count - 1; result++)
            {
                if (((KeyValueObject<T>)KeyObjects[result]).Value.Equals(obj)) return result;
            }
            return -1;
        }
        /// <summary>
        /// 判断拥有指定键的元素是否存在
        /// </summary>
        public bool IsExists(string key)
        {
            return (IndexOf(key) != -1);
        }

        /// <summary>
        /// 获取指定索引处元素的键
        /// </summary>
        public string GetKey(int index)
        {
            return ((KeyValueObject<T>)KeyObjects[index]).Key;
        }

        /// <summary>
        /// 获取指定索引处元素的值
        /// </summary>
        public T GetObject(int index)
        {
            return ((KeyValueObject<T>)KeyObjects[index]).Value;
        }

        /// <summary>
        /// 逆序从索引处开始指定数量的集合
        /// </summary>
        public void Reverse(int index, int count)
        {
            KeyObjects.Reverse(index, count);
        }

        private ArrayList KeyObjects;
    }

    /// <summary>
    /// 键和值得集合类
    /// </summary>
    public sealed class KeyValueObject<T>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public KeyValueObject(string key, T value)
        {
            this.key = key;
            this.value = value;
        }
        /// <summary>
        /// 键
        /// </summary>
        public string Key
        {
            get
            {
                return this.key;
            }
        }
        /// <summary>
        /// 值
        /// </summary>
        public T Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        private readonly string key;
        private T value;
    }

    #region KeyObjectDictionary<T>

    //键必须唯一，不提供按索引查找,只提供迭代器遍历
    /// <summary>
    /// 
    /// </summary>
    public sealed class KeyObjectDictionary<T> : IEnumerable<KeyValuePair<string, T>>
    {
        /// <summary>
        /// 清空集合
        /// </summary>
        public void Clear()
        {
            KeyObjects.Clear();
        }
        /// <summary>
        /// 集合元素数量
        /// </summary>
        public int Count
        {
            get
            {
                return KeyObjects.Count;
            }
        }
        /// <summary>
        /// 判断是否包含指定的键
        /// </summary>
        public bool ContainsKey(string key)
        {
            return KeyObjects.ContainsKey(key);
        }
        /// <summary>
        /// 判断是否包含指定的值
        /// </summary>
        public bool ContainsValue(T value)
        {
            return KeyObjects.ContainsValue(value);
        }
        /// <summary>
        /// 获取指定键的值
        /// </summary>
        public T this[string key]
        {
            get
            {
                return KeyObjects[key];
            }
            set
            {
                KeyObjects[key] = value;
            }
        }
        /// <summary>
        /// 添加键值
        /// </summary>
        public void Add(string key, T value)
        {
            KeyObjects.Add(key, value);
        }
        /// <summary>
        /// 删除指定键的项
        /// </summary>
        public bool Remove(string key)
        {
            return KeyObjects.Remove(key);
        }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return KeyObjects.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        private Dictionary<string, T> KeyObjects = new Dictionary<string, T>();
    }

    #endregion



    public class HashSet<T> : ICollection<T>, ISerializable, IDeserializationCallback
    {
        private readonly Dictionary<T, object> dict;

        public HashSet()
        {
            dict = new Dictionary<T, object>();
        }

        public HashSet(IEnumerable<T> items)
            : this()
        {
            if (items == null)
            {
                return;
            }

            foreach (T item in items)
            {
                Add(item);
            }
        }
        public HashSet(int capacity)  
        {
            dict = new Dictionary<T, object>(capacity);
        }


        public HashSet<T> NullSet { get { return new HashSet<T>(); } }

        #region ICollection<T> Members

        public void Add(T item)
        {
            if (null == item)
            {
                throw new ArgumentNullException("item");
            }

            dict[item] = null;
        }

        /// <summary>
        /// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only. </exception>
        public void Clear()
        {
            dict.Clear();
        }

        public bool Contains(T item)
        {
            return dict.ContainsKey(item);
        }

        /// <summary>
        /// Copies the items of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the items copied from <see cref="T:System.Collections.Generic.ICollection`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.</param><param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param><exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception><exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception><exception cref="T:System.ArgumentException"><paramref name="array"/> is multidimensional.-or-<paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.-or-The number of items in the source <see cref="T:System.Collections.Generic.ICollection`1"/> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.-or-Type T cannot be cast automatically to the type of the destination <paramref name="array"/>.</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException("array");
            if (arrayIndex < 0 || arrayIndex >= array.Length || arrayIndex >= Count)
            {
                throw new ArgumentOutOfRangeException("arrayIndex");
            }

            dict.Keys.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param><exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
        public bool Remove(T item)
        {
            return dict.Remove(item);
        }

        /// <summary>
        /// Gets the number of items contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        /// <returns>
        /// The number of items contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </returns>
        public int Count
        {
            get { return dict.Count; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        /// <returns>
        /// true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.
        /// </returns>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        #endregion

        public HashSet<T> Union(HashSet<T> set)
        {
            HashSet<T> unionSet = new HashSet<T>(this);

            if (null == set)
            {
                return unionSet;
            }

            foreach (T item in set)
            {
                if (unionSet.Contains(item))
                {
                    continue;
                }

                unionSet.Add(item);
            }

            return unionSet;
        }

        public HashSet<T> Subtract(HashSet<T> set)
        {
            HashSet<T> subtractSet = new HashSet<T>(this);

            if (null == set)
            {
                return subtractSet;
            }

            foreach (T item in set)
            {
                if (!subtractSet.Contains(item))
                {
                    continue;
                }

                subtractSet.dict.Remove(item);
            }

            return subtractSet;
        }

        public bool IsSubsetOf(HashSet<T> set)
        {
            HashSet<T> setToCompare = set ?? NullSet;

            foreach (T item in this)
            {
                if (!setToCompare.Contains(item))
                {
                    return false;
                }
            }

            return true;
        }

        public HashSet<T> Intersection(HashSet<T> set)
        {
            HashSet<T> intersectionSet = NullSet;

            if (null == set)
            {
                return intersectionSet;
            }

            foreach (T item in this)
            {
                if (!set.Contains(item))
                {
                    continue;
                }

                intersectionSet.Add(item);
            }

            foreach (T item in set)
            {
                if (!Contains(item) || intersectionSet.Contains(item))
                {
                    continue;
                }

                intersectionSet.Add(item);
            }

            return intersectionSet;
        }

        public bool IsProperSubsetOf(HashSet<T> set)
        {
            HashSet<T> setToCompare = set ?? NullSet;

            // A is a proper subset of a if the b is a subset of a and a != b
            return (IsSubsetOf(setToCompare) && !setToCompare.IsSubsetOf(this));
        }

        public bool IsSupersetOf(HashSet<T> set)
        {
            HashSet<T> setToCompare = set ?? NullSet;

            foreach (T item in setToCompare)
            {
                if (!Contains(item))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsProperSupersetOf(HashSet<T> set)
        {
            HashSet<T> setToCompare = set ?? NullSet;

            // B is a proper superset of a if b is a superset of a and a != b
            return (IsSupersetOf(setToCompare) && !setToCompare.IsSupersetOf(this));
        }

        public List<T> ToList()
        {
            return new List<T>(this);
        }

        #region Implementation of ISerializable

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data. </param><param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization. </param><exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException("info");
            dict.GetObjectData(info, context);
        }

        #endregion

        #region Implementation of IDeserializationCallback

        /// <summary>
        /// Runs when the entire object graph has been deserialized.
        /// </summary>
        /// <param name="sender">The object that initiated the callback. The functionality for this parameter is not currently implemented. </param>
        public void OnDeserialization(object sender)
        {
            dict.OnDeserialization(sender);
        }

        #endregion

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<T> GetEnumerator()
        {
            return dict.Keys.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
