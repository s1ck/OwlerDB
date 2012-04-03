using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OwlAPI.Structure;

namespace OwlCore.Structure
{
    public class Vertex : ADBObject, IVertex
    {
        #region used for benchmarks

        public Boolean IsVisited { get; set; }

        public Int32 Distance { get; set; }

        public IVertex Predecessor { get; set; }

        #endregion

        #region Private Attributes

        private IGraph _Graph = null;

        private HashSet<IEdge> _OutgoingEdges;

        private HashSet<IEdge> _IncomingEdges;

        #endregion

        #region Constructors

        public Vertex() 
            : this(null, null)
        {
        }

        public Vertex(String myUUID) 
            : this(myUUID, null)
        {
        }

        public Vertex(IDictionary<string, object> myAttributes)
            : this(null, myAttributes)
        {
        }

        public Vertex(String myUUID, IDictionary<string, object> myAttributes)
        {
            InitDBObject(myUUID, myAttributes);
            InitVertex();
        }

        private void InitVertex()
        {
            _OutgoingEdges = new HashSet<IEdge>();
            _IncomingEdges = new HashSet<IEdge>();
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

        public IEnumerable<IEdge> OutgoingEdges
        {
            get
            {
                return _OutgoingEdges;
            }
        }

        public IEnumerable<IEdge> IncomingEdges
        {
            get
            {
                return _IncomingEdges;
            }
        }

        #endregion

        #region Degree

        public long Degree
        {
            get 
            {
                return _OutgoingEdges.Count + _IncomingEdges.Count;
            }
        }

        public long InDegree
        {
            get 
            {
                return _IncomingEdges.Count;
            }
        }

        public long OutDegree
        {
            get
            {
                return _OutgoingEdges.Count;
            }
        }

        #endregion

        #region Neighbourhood

        public bool AddIncomingEdge(IEdge myEdge)
        {
            if (_IncomingEdges.Add(myEdge))
            {
                return true;
            }
            return false;
        }

        public bool RemoveIncomingEdge(IEdge myEdge)
        {
            if (_IncomingEdges.Remove(myEdge))
            {
                return true;
            }
            return false;
        }

        public bool AddOutgoingEdge(IEdge myEdge)
        {
            if (_OutgoingEdges.Add(myEdge))
            {
                return true;
            }
            return false;
        }

        public bool RemoveOutgoingEdge(IEdge myEdge)
        {
            if (_OutgoingEdges.Remove(myEdge))
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}
