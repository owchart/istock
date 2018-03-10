using System;
using System.Collections;
using System.IO;
using System.Xml;

namespace EmQComm
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMemento 
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 判断是否包含
        /// </summary>
        bool ContainsAttribute(string key);
        /// <summary>
        /// 获取布尔型
        /// </summary>
        bool GetBoolean(string key);
        /// <summary>
        /// 设置布尔型
        /// </summary>
        void SetBoolean(string key, bool value);
        /// <summary>
        /// 获取整型
        /// </summary>
        int GetInteger(string key);
        /// <summary>
        /// 设置整型
        /// </summary>
        void SetInteger(string key, int value);
        /// <summary>
        /// 获取浮点
        /// </summary>
        double GetFloat(string key);
        /// <summary>
        /// 设置浮点
        /// </summary>
        void SetFloat(string key, double value);
        /// <summary>
        /// 获取字符串
        /// </summary>
        string GetString(string key);
        /// <summary>
        /// 设置字符串
        /// </summary>
        void SetString(string key, string value);
        /// <summary>
        /// 获取DateTime
        /// </summary>
        DateTime GetDateTime(string key);
        /// <summary>
        /// 设置DateTime
        /// </summary>
        void SetDateTime(string key, DateTime value);
        /// <summary>
        /// 
        /// </summary>
        Object GetEnumValue(string key, Type enumType);
        /// <summary>
        /// 
        /// </summary>
        void SetEnumValue(string key, Type enumType, Object value);
        /// <summary>
        /// 
        /// </summary>
        int GetAttributeCount();
        /// <summary>
        /// 
        /// </summary>
        string GetAttributeName(int index);
        /// <summary>
        /// 
        /// </summary>
        string GetAttributeValue(int index);
        /// <summary>
        /// 
        /// </summary>
        IMemento CreateChild(string key);
        /// <summary>
        /// 
        /// </summary>
        int ChildCount { get; }
        /// <summary>
        /// 
        /// </summary>
        IMemento GetChild(string key);
        /// <summary>
        /// 
        /// </summary>
        IMemento GetChild(int index);
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IMementoReader
    {        
        /// <summary>
        /// 读取
        /// </summary>
        void Read(IMemento memento, Stream stream);
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IMementoWriter
    {
        /// <summary>
        /// 写入
        /// </summary>
        void Write(IMemento memento, Stream stream);
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IAssignable
    {
        /// <summary>
        /// 
        /// </summary>
        void Assign(Object source);
        /// <summary>
        /// 
        /// </summary>
        void AssignTo(Object destination);
    }
    /// <summary>
    /// 
    /// </summary>
    public interface IPersistable
    {
        /// <summary>
        /// 保存
        /// </summary>
        void SaveState(IMemento memento);
        /// <summary>
        /// 加载
        /// </summary>
        void LoadState(IMemento memento);
    }
    /// <summary>
    /// 
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// 
        /// </summary>
        void Serialization(IPersistable persistable);
        /// <summary>
        /// 
        /// </summary>
        void Deserialization(IPersistable persistable);
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class PersistableObject : IPersistable, ICloneable, IAssignable
    {
        /// <summary>
        /// 保存
        /// </summary>
        public abstract void SaveState(IMemento memento);
        /// <summary>
        /// 加载
        /// </summary>
        public abstract void LoadState(IMemento memento);
        /// <summary>
        /// 拷贝
        /// </summary>
        public object Clone()
        {
            object result = Activator.CreateInstance(this.GetType());
            (result as PersistableObject).Assign(this);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        public virtual void Assign(object source)
        {
            IMemento memento = new Memento();
            (source as PersistableObject).SaveState(memento);
            this.LoadState(memento);
        }
        /// <summary>
        /// 
        /// </summary>
        public void AssignTo(object destination)
        {
            (destination as PersistableObject).Assign(this);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Memento : IMemento
    {
        /// <summary>
        /// 
        /// </summary>
        public Memento()
        {
            Attributes = new KeyValueCollection();
            SubMementos = new KeyObjectCollection();
        }
        /// <summary>
        /// 
        /// </summary>
        public Memento(string name)
            : this()
        {
            this.name = name;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
        /// <summary>
        /// 是否包含指定键
        /// </summary>
        public bool ContainsAttribute(string key)
        {
            return (Attributes.IndexOf(key) > -1);
        }
        /// <summary>
        /// 由键获取布尔型
        /// </summary>
        public bool GetBoolean(string key)
        {
            return Convert.ToBoolean(Attributes.GetValue(key));
        }
        /// <summary>
        /// 由键设置布尔型
        /// </summary>
        public void SetBoolean(string key, bool value)
        {
            Attributes.Add(key, Convert.ToString(value));
        }
        /// <summary>
        /// 获取整型
        /// </summary>
        public int GetInteger(string key)
        {
            return Convert.ToInt32(Attributes.GetValue(key));
        }
        /// <summary>
        /// 设置整型
        /// </summary>
        public void SetInteger(string key, int value)
        {
            Attributes.Add(key, Convert.ToString(value));
        }
        /// <summary>
        /// 获取浮点
        /// </summary>
        public double GetFloat(string key)
        {
            return Convert.ToDouble(Attributes.GetValue(key));
        }
        /// <summary>
        /// 设置浮点
        /// </summary>
        public void SetFloat(string key, double value)
        {
            Attributes.Add(key, Convert.ToString(value));
        }
        /// <summary>
        /// 获取字符串
        /// </summary>
        public string GetString(string key)
        {
            return Attributes.GetValue(key);
        }
        /// <summary>
        /// 设置字符串
        /// </summary>
        public void SetString(string key, string value)
        {
            Attributes.Add(key, value);
        }
        /// <summary>
        /// 获取DateTime
        /// </summary>
        public DateTime GetDateTime(string key)
        {
            string dateTimeStr = Attributes.GetValue(key);
            return Convert.ToDateTime(dateTimeStr);
        }
        /// <summary>
        /// 设置DateTime
        /// </summary>
        public void SetDateTime(string key, DateTime value)
        {
            string dateTimeStr = value.ToLongTimeString();
            SetString(key, dateTimeStr);
        }
        /// <summary>
        /// 
        /// </summary>
        public Object GetEnumValue(string key, Type enumType)
        {
            string enumStr = Attributes.GetValue(key);
            return Enum.Parse(enumType, enumStr);
        }
        /// <summary>
        /// 
        /// </summary>
        public void SetEnumValue(string key, Type enumType, Object value)
        {
            string enumStr = Enum.GetName(enumType, value);
            SetString(key, enumStr);
        }
        /// <summary>
        /// 
        /// </summary>
        public int GetAttributeCount()
        {
            return Attributes.Count;
        }
        /// <summary>
        /// 
        /// </summary>
        public string GetAttributeName(int index)
        {
            return Attributes.GetKey(index);
        }
        /// <summary>
        /// 
        /// </summary>
        public string GetAttributeValue(int index)
        {
            return Attributes.GetValue(index);
        }
        /// <summary>
        /// 
        /// </summary>
        public IMemento CreateChild(string key)
        {
            IMemento result = new Memento();
            result.Name = key;
            SubMementos.Add(key, result);
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        public int ChildCount
        {
            get
            {
                return SubMementos.Count;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public IMemento GetChild(string key)
        {
            return SubMementos.GetObject(key) as IMemento;
        }
        /// <summary>
        /// 
        /// </summary>
        public IMemento GetChild(int index)
        {
            return SubMementos.GetObject(index) as IMemento;
        }

        private string name;
        private KeyValueCollection Attributes;
        private KeyObjectCollection SubMementos;
    }

    #region 实体对象，XML的序列化和反序列化

    /// <summary>
    /// 
    /// </summary>
    public class XmlMementoReader : IMementoReader
    {
        /// <summary>
        /// 读取
        /// </summary>
        public void Read(IMemento memento, Stream stream)
        {
            XmlDocument document = new XmlDocument();
            document.Load(stream);
            XmlNodeList nodes = document.ChildNodes;
            XmlNode node = nodes[nodes.Count - 1];
            memento.Name = node.Name;
            ProcessMemento(memento, node);
        }

        private void ProcessMemento(IMemento memento, XmlNode xmlnode)
        {
            //1.循环获取属性信息
            XmlAttributeCollection xmlAttributes = xmlnode.Attributes;
            IEnumerator attrEnumerator = xmlAttributes.GetEnumerator();
            while (attrEnumerator.MoveNext())
            {
                XmlAttribute xmlAttribute = (XmlAttribute)attrEnumerator.Current;
                if (!string.IsNullOrEmpty(xmlAttribute.Value))
                {
                    memento.SetString(xmlAttribute.Name, xmlAttribute.Value);
                }
            }

            //2.递归进行ChildMemento处理，需要创建ChildMemento对象
            XmlNodeList childrenNodes = xmlnode.ChildNodes;
            IEnumerator nodeEnumerator = childrenNodes.GetEnumerator();
            while (nodeEnumerator.MoveNext())
            {
                XmlNode childNode = (XmlNode)nodeEnumerator.Current;
                if (childNode.NodeType != XmlNodeType.Comment)
                {
                    IMemento childMemento = memento.CreateChild(childNode.Name);
                    ProcessMemento(childMemento, childNode);
                }
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class XmlMementoWriter : IMementoWriter
    {
        /// <summary>
        /// 写入
        /// </summary>
        public void Write(IMemento memento, Stream stream)
        {
            //1. 写入根结点内容
            XmlDocument xmlDocument = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "gb2312", "yes");
            xmlDocument.PrependChild(xmlDeclaration);

            XmlElement rootNode = xmlDocument.CreateElement(memento.Name);
            xmlDocument.AppendChild(rootNode);

            //2. 写入根结点属性信息
            for (int index = 0; index <= memento.GetAttributeCount() - 1; index++)
            {
                XmlAttribute attributeNode = xmlDocument.CreateAttribute(memento.GetAttributeName(index));
                attributeNode.Value = memento.GetAttributeValue(index);
                rootNode.SetAttributeNode(attributeNode);
            }

            //3. 写入子结点内容
            for (int index = 0; index <= memento.ChildCount - 1; index++)
            {
                IMemento childMemento = memento.GetChild(index);
                ProcessMemento(childMemento, rootNode, xmlDocument);
            }
            xmlDocument.Save(stream);
        }

        private void ProcessMemento(IMemento memento, XmlNode parentNode, XmlDocument xmlDocument)
        {
            //1.增加结点
            XmlElement newNode = xmlDocument.CreateElement(memento.Name);
            parentNode.AppendChild(newNode);

            //2.设置属性信息
            for (int index = 0; index <= memento.GetAttributeCount() - 1; index++)
            {
                XmlAttribute attributeNode = xmlDocument.CreateAttribute(memento.GetAttributeName(index));
                attributeNode.Value = memento.GetAttributeValue(index);
                newNode.SetAttributeNode(attributeNode);
            }

            //3.递归进行ChildMemento处理
            for (int index = 0; index <= memento.ChildCount - 1; index++)
            {
                IMemento childMemento = memento.GetChild(index);
                ProcessMemento(childMemento, newNode, xmlDocument);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class XmlFileSerializer : ISerializer
    {
        /// <summary>
        /// 
        /// </summary>
        public XmlFileSerializer(string filename)
        {
            this.filename = filename;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Serialization(IPersistable persistable)
        {
            if (File.Exists(filename)) File.Delete(filename);

            Stream stream = File.OpenWrite(filename);
            IMemento memento = new Memento();
            IMementoWriter mementoWriter = new XmlMementoWriter();
            persistable.SaveState(memento);
            mementoWriter.Write(memento, stream);
            stream.Close();
        }
        /// <summary>
        /// 
        /// </summary>
        public void Deserialization(IPersistable persistable)
        {
            Stream stream = File.OpenRead(filename);
            IMemento memento = new Memento();
            IMementoReader mementoReader = new XmlMementoReader();
            stream.Seek(0, SeekOrigin.Begin);
            mementoReader.Read(memento, stream);
            persistable.LoadState(memento);
            stream.Close();
        }

        private readonly string filename;
    }

    #endregion
}
