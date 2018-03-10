using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace dataquery.indicator
{
    public class ParameterType
    {
        #region лу╣б 2012/7/9
        private long _ID;
        [XmlIgnore]
        public long ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        private long _TYPECODE;
        [XmlAttribute]
        public long TYPECODE
        {
            get { return _TYPECODE; }
            set { _TYPECODE = value; }
        }

        private String _TYPENAME;
        [XmlAttribute]
        public String TYPENAME
        {
            get { return _TYPENAME; }
            set { _TYPENAME = value; }
        }

        private String _TYPEDESCRIPTION;
        [XmlAttribute]
        public String TYPEDESCRIPTION
        {
            get { return _TYPEDESCRIPTION; }
            set { _TYPEDESCRIPTION = value; }
        }

        private String _MODULECONTROL;
        [XmlAttribute]
        public String MODULECONTROL
        {
            get { return _MODULECONTROL; }
            set { _MODULECONTROL = value; }
        }

        private String _OFFICECONTROL;
        [XmlAttribute]
        public String OFFICECONTROL
        {
            get { return _OFFICECONTROL; }
            set { _OFFICECONTROL = value; }
        }

        private String _DATATYE;
        [XmlAttribute]
        public String DATATYE
        {
            get { return _DATATYE; }
            set { _DATATYE = value; }
        }

        private String _FORMTYPE;
        [XmlAttribute]
        public String FORMTYPE
        {
            get { return _FORMTYPE; }
            set { _FORMTYPE = value; }
        }

        private String _SOURCETYPE;
        [XmlAttribute]
        public String SOURCETYPE
        {
            get { return _SOURCETYPE; }
            set { _SOURCETYPE = value; }
        }

        private int _ISUSABLE;

        [XmlAttribute]
        public int ISUSABLE
        {
            get { return _ISUSABLE; }
            set { _ISUSABLE = value; }
        }

        private String _TAG;
        [XmlAttribute]
        public String TAG
        {
            get { return _TAG; }
            set { _TAG = value; }
        }

        private String _CUSTOMVALUE;
        [XmlAttribute]
        public String CUSTOMVALUE
        {
            get { return _CUSTOMVALUE; }
            set { _CUSTOMVALUE = value; }
        }

        private DateTime _ENTRYTIME;
        [XmlIgnore]
        public DateTime ENTRYTIME
        {
            get { return _ENTRYTIME; }
            set { _ENTRYTIME = value; }
        }

        private DateTime _UPDATETIME;
        [XmlIgnore]
        public DateTime UPDATETIME
        {
            get { return _UPDATETIME; }
            set { _UPDATETIME = value; }
        }

        private long _UPDATEPERSON;
        [XmlIgnore]
        public long UPDATEPERSON
        {
            get { return _UPDATEPERSON; }
            set { _UPDATEPERSON = value; }
        }

        private long _UPDATEID;

        [XmlIgnore]
        public long UPDATEID
        {
            get { return _UPDATEID; }
            set { _UPDATEID = value; }
        }
        #endregion
    }
}
