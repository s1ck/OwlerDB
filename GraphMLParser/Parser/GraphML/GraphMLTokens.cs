using System;

namespace Tools.Parser.GraphML
{
    /// <summary>
    /// Class defines the XML tokens which are used for parsing.
    /// 
    /// author:         Martin Junghanns (martin.junghanns@gmx.net)
    /// </summary>
    public class GraphMLTokens
    {
        #region graph data

        public const String GRAPHML = "graphml";
        public const String GRAPH = "graph";
        public const String VERTEX = "node";
        public const String EDGE = "edge";
        public const String EDGEDEFAULT = "edgedefault";
        public const String DIRECTED = "directed"; 
        public const String UNDIRECTED = "undirected";
        public const String SOURCE = "source";
        public const String TARGET = "target";

        #region attributes

        public const string DATA = "data";
        public const string ID = "id";
        public const String KEY = "key";
        public const String FOR = "for";
        public const String ATTRIBUTE_NAME = "attr.name";
        public const String ATTRIBUTE_TYPE = "attr.type";
        public const String DEFAULT = "default";

        public const string LON = "lon";
        public const string LAT = "lat";

        #endregion

        #endregion

        #region base types

        public const String STRING = "string";
        public const String INT = "int";
        public const String BOOLEAN = "boolean";
        public const String FLOAT = "float";
        public const String DOUBLE = "double";
        public const String LONG = "long";

        #endregion

        #region tags

        public const String NODE_END_TAG = "</node>";

        public const String EDGE_END_TAG = "</edge>";

        public const String DATA_END_TAG = "</data>";

        public const String END_GRAPHML_TAG = "</graphml>";
        public const String END_GRAPH_TAG = "</graph>";

        public const String COMMENT_BEGIN_TAG = "<!--";
        public const String COMMENT_END_TAG = "-->";
        
        #endregion
    }
}
