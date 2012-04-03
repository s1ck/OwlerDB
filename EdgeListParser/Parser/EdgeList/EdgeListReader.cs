using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OwlAPI.Parser;
using OwlAPI.Structure;
using System.IO;

namespace Tools.Parser.EdgeListParser
{
    /// <summary>
    /// Simple EdgeList Reader which reads a graph in the following format
    /// 0   1
    /// 0   2
    /// 0   3
    /// 1   3
    /// 1   5
    /// 2   4
    /// ...
    /// 
    /// Examples for such graphs can be found here: 
    /// http://snap.stanford.edu/data/index.html
    /// 
    /// author: Martin Junghanns (martin.junghanns@gmx.net)
    /// </summary>
    public class EdgeListReader : IGraphReader
    {
        public bool Read(IGraph myGraph, FileStream myInputStream)
        {
            // use a streamreader .. better handling
            StreamReader reader = null;
            // stores the added vertices for fast retrieval
            var addedVertices = new Dictionary<string, IVertex>();

            try
            {
                reader = new StreamReader(myInputStream);
                String line;    // current line in file
                String[] edge;  // contains source and target vertex id
                IVertex source; // source vertex
                IVertex target; // target vertex
                var count = 0L; // number of processed edges

                while ((line = reader.ReadLine()) != null)
                {
                    if (!line.StartsWith(EdgeListTokens.LINE_COMMENT)) // just a comment
                    {
                        // line look like that "0   1", splitting it to get source and target id
                        edge = line.Split(EdgeListTokens.SOURCE_TARGET_SEPARATOR);

                        // got two id's?
                        if (edge.Length == 2)
                        {
                            // check if source has been added before
                            if (!addedVertices.TryGetValue(edge[0], out source))
                            {
                                // if not, create new vertex
                                source = myGraph.AddVertex(edge[0]);
                                addedVertices.Add(edge[0], source);
                            }
                            // check if target has been added before
                            if (!addedVertices.TryGetValue(edge[1], out target))
                            {
                                // if not, create new vertex
                                target = myGraph.AddVertex(edge[1]);
                                addedVertices.Add(edge[1], target);
                            }

                            if (myGraph.AddEdge(source, target) != null)
                            {
                                count++;
                            }
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                // TODO add logging here
                return false;
            }
            finally
            {

                reader.Close();
            }

            return true;
        }
    }
}
