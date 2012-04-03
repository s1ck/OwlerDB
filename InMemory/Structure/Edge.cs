using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OwlAPI.Structure;

namespace OwlCore.Structure
{
    public class Edge : ADBObject, IEdge
    {
        #region Private Attributes

        private IGraph _Graph;

        private IVertex _Source;

        private IVertex _Target;

        #endregion

        #region Constructors

        public Edge(IVertex mySource, IVertex myTarget) 
            : this(null, mySource, myTarget, null)
        {
        }

        public Edge(String myUUID, IVertex mySource, IVertex myTarget)
            : this(myUUID, mySource, myTarget, null)
        {
        }

        public Edge(IVertex mySource, IVertex myTarget, IDictionary<string, object> myAttributes)
            : this(null, mySource, myTarget, myAttributes)
        {
        }

        public Edge(String myUUID, IVertex mySource, IVertex myTarget, IDictionary<string, object> myAttributes)
        {
            if (mySource == null)
            {
                throw new ArgumentException("source-vertex cannot be null");
            }
            if (myTarget == null)
            {
                throw new ArgumentException("target-vertex cannot be null");
            }

            InitDBObject(myUUID, myAttributes);

            _Source = mySource;
            _Target = myTarget;

        }

        #endregion

        #region Properties

        public IGraph Graph
        {
            get
            {
                return _Graph;
            }
            set
            {
                _Graph = value;
            }
        }

        public IVertex Source
        {
            get { return _Source; }
        }

        public IVertex Target
        {
            get { return _Target; }
        }

        #endregion

        #region Overrides

        public override bool Equals(object obj)
        {
            var tmp = obj as Edge;

            if (tmp != null)
            {
                return tmp.UUID.Equals(_UUID)
                    && tmp.Source.Equals(_Source)
                    && tmp.Target.Equals(_Target);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return _UUID.GetHashCode()
                + _Source.GetHashCode()
                + _Target.GetHashCode();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Edge - From:{0} To:{1} ", _Source.UUID, _Target.UUID);

            if (_Attributes.Count > 0)
            {
                sb.Append("Attributes -> ");
                foreach (var entry in _Attributes)
                {
                    sb.AppendFormat("{0}:{1} ", entry.Key, entry.Value);
                }
            }

            return sb.ToString();
        }

        #endregion

    }
}
