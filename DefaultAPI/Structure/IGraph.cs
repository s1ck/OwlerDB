using System;
using System.Collections.Generic;
using OwlAPI.Structure.Meta;

namespace OwlAPI.Structure
{
    /// <summary>
    /// root interface for describing a graph structure.
    /// 
    /// author: s1ck
    /// </summary>
    public interface IGraph : IDBObject
    {
        #region Properties

        /// <summary>
        /// return all vertices
        /// </summary>
        IEnumerable<IVertex> Vertices { get; }

        /// <summary>
        /// return all edges
        /// </summary>
        IEnumerable<IEdge> Edges { get; }

        /// <summary>
        /// return number of vertices
        /// </summary>
        Int64 VertexCount { get; }

        /// <summary>
        /// return number of edges
        /// </summary>
        Int64 EdgeCount { get; }

        /// <summary>
        /// graph is directed or not
        /// </summary>
        bool IsDirected { get; }

        #endregion

        #region Methods

        #region Vertices

        #region Add

        /// <summary>
        /// Adds a vertex with the given uuid
        /// </summary>
        /// <param name="myUUID"></param>
        /// <returns></returns>
        IVertex AddVertex(String myUUID);

        /// <summary>
        /// Adds a vertex with the given attributes
        /// </summary>
        /// <param name="myAttributes"></param>
        /// <returns></returns>
        IVertex AddVertex(IDictionary<string, object> myAttributes);

        /// <summary>
        /// Adds a vertex to the graph.
        /// </summary>
        /// <param name="myVertex">The vertex</param>
        /// <returns></returns>
        bool AddVertex(IVertex myVertex);

        /// <summary>
        /// Add vertices to the graph.
        /// </summary>
        /// <param name="myVertices">The vertices to be added</param>
        /// <returns>The vertices which were not added</returns>
        IEnumerable<IVertex> AddVertices(IEnumerable<IVertex> myVertices);

        #endregion

        #region Remove

        /// <summary>
        /// Removes a vertex from the graph if it exists.
        /// </summary>
        /// <param name="myVertex">The vertex to be removed</param>
        /// <returns>True if the vertex has been removed, else false</returns>
        bool RemoveVertex(IVertex myVertex);

        /// <summary>
        /// Removes vertices from the graph if they exist.
        /// </summary>
        /// <param name="myVertices"></param>
        /// <returns></returns>
        IEnumerable<IVertex> RemoveVertices(IEnumerable<IVertex> myVertices);

        #endregion

        #region Degree

        Int64 GetDegree(IVertex myVertex);

        Int64 GetInDegree(IVertex myVertex);

        Int64 GetOutDegree(IVertex myVertex);

        #endregion

        #region Neighbourhood

        IEnumerable<IEdge> GetOutgoingEdges(IVertex myVertex);

        IEnumerable<IEdge> GetIncomingEdges(IVertex myVertex);

        #endregion

        #endregion

        #region Edges

        #region Add

        IEdge AddEdge(IVertex mySource, IVertex myTarget);

        IEdge AddEdge(String myUUID, IVertex mySource, IVertex myTarget);

        IEdge AddEdge(IVertex mySource, IVertex myTarget, IDictionary<string, object> myAttributes);

        IEdge AddEdge(String myUUID, IVertex mySource, IVertex myTarget, IDictionary<string, object> myAttributes);

        bool AddEdge(IEdge myEdge);

        IEnumerable<IEdge> AddEdges(IEnumerable<IEdge> myEdges);

        #endregion

        #region Remove

        bool RemoveEdge(IEdge myEdge);

        IEnumerable<IEdge> RemoveEdges(IEnumerable<IEdge> myEdges);

        #endregion

        #endregion

        #endregion

    }
}
