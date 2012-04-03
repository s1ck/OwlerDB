using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Globalization;
using OwlAPI.Parser;
using OwlAPI.Structure;
using OwlAPI.Structure.Meta;

namespace Tools.Parser.GraphML
{
    /// <summary>
    /// Class deserializes a graph stored in GraphML format.
    /// 
    /// GraphML description can be found here: http://graphml.graphdrawing.org/primer/graphml-primer.html
    /// 
    /// implemented:
    /// + attribute definition
    /// + vertex parsing
    /// + edge parsing
    /// + attribute parsing
    /// 
    /// not implemented:
    /// - hyperedges
    /// - nested graphs
    /// - graph meta
    /// - xml header stuff
    /// - ports
    /// - extends
    /// 
    /// author:         Martin Junghanns (martin.junghanns@gmx.net)
    /// </summary>
    public class GraphMLReader : IGraphReader
    {
        /// <summary>
        /// the graph instance where the data is stored
        /// </summary>
        private IGraph _Graph;

        /// <summary>
        /// maps attribute keys to attribute (for, name, type, default)
        /// </summary>
        private Dictionary<string, Tuple<string, string, string, string>> _AttributeDefinitions;

        /// <summary>
        /// stores the parsed vertices for fast retrieval
        /// </summary>
        private Dictionary<string, IVertex> _Vertices;

        /// <summary>
        /// stores the parsed edges for fast retrieval
        /// </summary>
        private Dictionary<string, IEdge> _Edges;

        public bool Read(IGraph myGraph, FileStream myInputStream)
        {
            #region data

            _Graph = myGraph;
            _AttributeDefinitions = new Dictionary<string, Tuple<string, string, string, string>>();
            _Vertices = new Dictionary<string, IVertex>();
            _Edges = new Dictionary<string, IEdge>();

            #endregion

            //TODO: read header and graph info

            #region read elements

            try
            {
                var reader = XmlReader.Create(myInputStream);
                
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case GraphMLTokens.GRAPHML:
                                break;
                            case GraphMLTokens.GRAPH:
                                break;
                            case GraphMLTokens.KEY:
                                ReadAttributeDefinition(reader);
                                break;
                            case GraphMLTokens.VERTEX:
                                ReadVertex(reader);
                                break;
                            case GraphMLTokens.EDGE:
                                ReadEdge(reader);
                                break;
                            default:
                                throw new XmlException(String.Format("unsupported node type: {0}", reader.Name));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                myInputStream.Close();
            }

            #endregion

            return true;
        }

        private void ReadAttributeDefinition(XmlReader myReader)
        {
            var attrId = myReader.GetAttribute(GraphMLTokens.ID);
            var attrFor = myReader.GetAttribute(GraphMLTokens.FOR);
            var attrName = myReader.GetAttribute(GraphMLTokens.ATTRIBUTE_NAME);
            var attrType = myReader.GetAttribute(GraphMLTokens.ATTRIBUTE_TYPE);

            string attrDefault = null;

            using (var readerAttribute = myReader.ReadSubtree())
            {
                while (readerAttribute.Read())
                {
                    if (readerAttribute.Name == GraphMLTokens.DEFAULT)
                    {
                        attrDefault = readerAttribute.ReadElementContentAsString();
                    }
                }
            }

            _AttributeDefinitions.Add(attrId, new Tuple<string, string, string, string>(attrFor, attrName, attrType, attrDefault));
        }

        private void ReadVertex(XmlReader myReader)
        {
            #region vertex info

            var vertexID = myReader.GetAttribute(GraphMLTokens.ID);
            var vertex = _Graph.AddVertex(vertexID);

            #endregion

            #region vertex attributes

            if (vertex != null) // if vertex is null, it has been added before
            {
                using (var vertexDataReader = myReader.ReadSubtree())
                {
                    // read attributes
                    while (vertexDataReader.Read())
                    {
                        if (vertexDataReader.Name == GraphMLTokens.DATA)
                        {
                            ReadAttribute(vertex, vertexDataReader);
                        }
                    }
                }
                _Vertices.Add(vertexID, vertex);
            }

            #endregion
        }

        private void ReadEdge(XmlReader myReader)
        {
            var eId = myReader.GetAttribute(GraphMLTokens.ID);
            var sourceID = myReader.GetAttribute(GraphMLTokens.SOURCE);
            var targetID = myReader.GetAttribute(GraphMLTokens.TARGET);

            var sourceVertex = _Vertices[sourceID];
            var targetVertex = _Vertices[targetID];

            IEdge edge = null;

            if (sourceVertex != null && targetVertex != null)
            {
                edge = _Graph.AddEdge(eId, sourceVertex, targetVertex);
            }

            if (edge != null) // has been added
            {
                using (var edgeDataReader = myReader.ReadSubtree())
                {
                    while (edgeDataReader.Read())
                    {
                        if (edgeDataReader.Name == GraphMLTokens.DATA)
                        {
                            ReadAttribute(edge, myReader);
                        }
                    }
                }
            }
        }

        private void ReadAttribute(IDBObject myDBObject, XmlReader myReader)
        {
            var key = myReader.GetAttribute(GraphMLTokens.KEY);
            var value = myReader.ReadElementContentAsString();

            if (key != null)
            {
                var tupel = _AttributeDefinitions[key];

                if (tupel != null)
                {
                    var attrName = tupel.Item2;
                    var attrType = tupel.Item3;

                    if (value == null) // use default value
                    {
                        value = tupel.Item4;
                    }

                    if (attrType != null && value != null)
                    {
                        // add or update property
                        myDBObject[attrName] = CastValue(attrType, value);
                    }
                }
            }
        }

        private object CastValue(String myType, String myValue)
        {
            if (myType.Equals(GraphMLTokens.INT))
            {
                return Int32.Parse(myValue);
            }
            else if (myType.Equals(GraphMLTokens.FLOAT))
            {
                return float.Parse(myValue);
            }
            else if (myType.Equals(GraphMLTokens.DOUBLE))
            {
                // mh..had to use invariant culture or he made 10.0 out of 1.0
                return Double.Parse(myValue, CultureInfo.InvariantCulture);
            }
            else if (myType.Equals(GraphMLTokens.LONG))
            {
                return Int64.Parse(myValue);
            }
            else
            {
                return myValue;
            }
        }
    }
}
