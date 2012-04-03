using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using OwlAPI.Structure;
using System.Collections;

namespace OwlCore.Structure
{
    public class OwlerDB : ADBObject, IGraph
    {
        #region private members

        private HashSet<IVertex> _Vertices;

        private HashSet<IEdge> _Edges;

        private Int64 _VertexCount;

        private Int64 _EdgeCount;

        private bool _IsDirected;

        #endregion

        #region constructors

        public OwlerDB()
        {
            InitDBObject();

            InitOwlerDB();
        }

        private void InitOwlerDB()
        {
            _Vertices = new HashSet<IVertex>();
            _Edges = new HashSet<IEdge>();

            _VertexCount    = 0;
            _EdgeCount      = 0;

            _IsDirected = true;
        }
        #endregion

        #region IGraph

        public IEnumerable<IVertex> Vertices
        {
            get 
            {
                foreach (var v in _Vertices)
                {
                    yield return v;
                }
            }
        }

        public IEnumerable<IEdge> Edges
        {
            get 
            {
                foreach (var edge in _Edges)
                {
                    yield return edge;
                }
            }
        }

        public long VertexCount
        {
            get { return _VertexCount; }
        }

        public long EdgeCount
        {
            get { return _EdgeCount; }
        }

        public bool IsDirected
        {
            get
            {
                return _IsDirected;
            }
        }

        #region Vertices

        #region Add

        public IVertex AddVertex(String myUUID)
        {
            return AddVertex(myUUID, null);
        }

        public IVertex AddVertex(IDictionary<string, object> myAttributes)
        {
            return AddVertex(null, myAttributes);
        }

        public IVertex AddVertex(String myUUID, IDictionary<string, object> myAttributes)
        {
            var v = new Vertex(myUUID, myAttributes);

            return (AddVertex(v)) ? v : null;
        }

        public bool AddVertex(IVertex myVertex)
        {
            if (_Vertices.Contains(myVertex))
            {
                return false;
            }

            _Vertices.Add(myVertex);

            myVertex.Graph = this;
            _VertexCount++;

            return true;
        }

        public IEnumerable<IVertex> AddVertices(IEnumerable<IVertex> myVertices)
        {
            var nonAddedVertices = new List<IVertex>();

            foreach (var vertex in myVertices)
            {
                if (!AddVertex(vertex))
                {
                    nonAddedVertices.Add(vertex);
                }
            }

            return nonAddedVertices;
        }

        #endregion

        #region Remove

        public bool RemoveVertex(IVertex myVertex)
        {
            if (_Vertices.Contains(myVertex))
            {
                List<IEdge> removeEdges = null;

                foreach(var edge in _Edges)
                {
                    removeEdges = new List<IEdge>();

                    if (edge.Source.Equals(myVertex) || edge.Target.Equals(myVertex))
                    {
                        removeEdges.Add(edge);
                        _EdgeCount--;
                    }

                    foreach (var remEdge in removeEdges)
                    {
                        _Edges.Remove(remEdge);
                    }
                }

                _Vertices.Remove(myVertex);
                _VertexCount--;

                return true;
            }

            return false;
        }

        public IEnumerable<IVertex> RemoveVertices(IEnumerable<IVertex> myVertices)
        {
            var nonRemovedVertices = new List<IVertex>();

            foreach (var vertex in myVertices)
            {
                if (!RemoveVertex(vertex))
                {
                    nonRemovedVertices.Add(vertex);
                }
            }

            return nonRemovedVertices;
        }

        #endregion

        #region Degree

        public long GetDegree(IVertex myVertex)
        {
            return GetInDegree(myVertex) + GetOutDegree(myVertex);
        }

        public long GetInDegree(IVertex myVertex)
        {
            return (_Vertices.Contains(myVertex)) ? myVertex.InDegree : 0;
        }

        public long GetOutDegree(IVertex myVertex)
        {
            return (_Vertices.Contains(myVertex)) ? myVertex.OutDegree : 0;
        }

        #endregion

        #region Neighbourhood

        public IEnumerable<IEdge> GetOutgoingEdges(IVertex myVertex)
        {
            return (_Vertices.Contains(myVertex)) ? myVertex.OutgoingEdges : null;
        }

        public IEnumerable<IEdge> GetIncomingEdges(IVertex myVertex)
        {
            return (_Vertices.Contains(myVertex)) ? myVertex.IncomingEdges : null;
        }

        #endregion

        #endregion

        #region Edges

        #region Add

        public IEdge AddEdge(IVertex mySource, IVertex myTarget)
        {
            return AddEdge(null, mySource, myTarget, null);
        }

        public IEdge AddEdge(String myUUID, IVertex mySource, IVertex myTarget)
        {
            return AddEdge(myUUID, mySource, myTarget, null);
        }

        public IEdge AddEdge(IVertex mySource, IVertex myTarget, IDictionary<string, object> myAttributes)
        {
            return AddEdge(null, mySource, myTarget, myAttributes);
        }

        public IEdge AddEdge(String myUUID, IVertex mySource, IVertex myTarget, IDictionary<string, object> myAttributes)
        {
            var e = new Edge(myUUID, mySource, myTarget, myAttributes);

            return (AddEdge(e)) ? e : null;
        }

        public bool AddEdge(IEdge myEdge)
        {
            if (!_Edges.Contains(myEdge)
                && _Vertices.Contains(myEdge.Source)
                && _Vertices.Contains(myEdge.Target))
            {
                var source = myEdge.Source as Vertex;
                var target = myEdge.Target as Vertex;

                var added = source.AddOutgoingEdge(myEdge);

                if (added)
                {
                    // use the same edge as incoming edge for target vertex
                    added = target.AddIncomingEdge(myEdge);
                }

                if (added)
                {
                    if (_Edges.Add(myEdge))
                    {
                        myEdge.Graph = this;
                        _EdgeCount++;
                        return true;
                    }
                }

                return false;
            }

            return false;
        }

        public IEnumerable<IEdge> AddEdges(IEnumerable<IEdge> myEdges)
        {
            var nonAddedEdges = new List<IEdge>();

            foreach (var edge in myEdges)
            {
                if (!AddEdge(edge))
                {
                    nonAddedEdges.Add(edge);
                }
            }

            return nonAddedEdges;
        }

        #endregion

        #region Remove

        public bool RemoveEdge(IEdge myEdge)
        {
            if (_Edges.Remove(myEdge))
            {
                _EdgeCount--;
                return true;
            }
            return false;
        }

        public IEnumerable<IEdge> RemoveEdges(IEnumerable<IEdge> myEdges)
        {
            var nonRemovedEdges = new List<IEdge>();

            foreach (var edge in myEdges)
            {
                if (!RemoveEdge(edge))
                {
                    nonRemovedEdges.Add(edge);
                }
            }

            return nonRemovedEdges;
        }

        #endregion

        #endregion

        #endregion

    }
}
