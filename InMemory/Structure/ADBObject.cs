using System;
using System.Collections.Generic;
using System.Text;
using OwlAPI.Structure.Meta;
using OwlAPI.Structure;

namespace OwlCore.Structure
{
    public class ADBObject : IDBObject
    {
        #region private members

        protected String _UUID;

        protected IDictionary<string, object> _Attributes;

        #endregion

        #region IDBObject

        public String UUID
        {
            get { return _UUID; }
        }

        public IDictionary<string, object> Attributes
        {
            get { return _Attributes; }
        }

        public object this[string myKey]
        {
            get
            {
                return _Attributes[myKey];
            }
            set
            {
                _Attributes[myKey] = value;
            }
        }

        public bool RemoveAttribute(string myKey)
        {
            return _Attributes.Remove(myKey);
        }

        #endregion

        #region protected methods

        protected void InitDBObject()
        {
            InitDBObject(null, null);
        }

        protected void InitDBObject(String myUUID, IDictionary<string, object> myAttributes)
        {
            _UUID = (myUUID == null || "".Equals(myUUID)) ? _UUID = Guid.NewGuid().ToString() : myUUID; 

            _Attributes = (myAttributes == null) ? new Dictionary<string, object>() : myAttributes;
        }

        #endregion

        #region overrides

        public override bool Equals(object obj)
        {
            var tmp = obj as ADBObject;

            if (tmp != null)
            {
                return tmp._UUID.Equals(_UUID);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return _UUID.GetHashCode();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("ADBObject - UUID:{0} Type:{1}", _UUID, this.GetType().Name);

            //list all attributes
            if (_Attributes.Count > 0)
            {
                sb.Append("Attributes -> ");

                foreach (var entry in _Attributes)
                {
                    sb.AppendFormat("{0}:{1} ", entry.Key, (entry.Value is ADBObject) ? ((ADBObject) entry.Value).UUID : entry.Value);
                }
            }

            return sb.ToString();
        }

        #endregion
    }
}
