using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OwlAPI.Structure;
using OwlCore.Structure;
using Tools.Parser.GraphML;
using System.IO;

namespace StructureTests
{
    public class AGraphTest
    {
        protected IGraph InitGermanyGraph()
        {
            #region graph

            var g = new OwlerDB();
            g["name"] = "Germany";

            #endregion

            #region cities

            var Frankfurt = new Vertex("1", new Dictionary<string, object>()
                {
                 {"name", "Frankfurt"}   
                });

            var Mannheim = new Vertex("2", new Dictionary<string, object>()
                {
                 {"name", "Mannheim"}   
                });

            var Wuerzburg = new Vertex("3", new Dictionary<string, object>()
                {
                 {"name", "Wuerzburg"}   
                });

            var Stuttgart = new Vertex("4", new Dictionary<string, object>()
                {
                 {"name", "Stuttgart"}   
                });

            var Karlsruhe = new Vertex("5", new Dictionary<string, object>()
                {
                 {"name", "Karlsruhe"}   
                });

            var Erfurt = new Vertex("6", new Dictionary<string, object>()
                {
                 {"name", "Erfurt"}   
                });

            var Nuernberg = new Vertex("7", new Dictionary<string, object>()
                {
                 {"name", "Nuernberg"}   
                });

            var Kassel = new Vertex("8", new Dictionary<string, object>()
                {
                 {"name", "Kassel"}   
                });

            var Augsburg = new Vertex("9", new Dictionary<string, object>()
                {
                 {"name", "Augsburg"}   
                });

            var Muenchen = new Vertex("10", new Dictionary<string, object>()
                {
                 {"name", "Muenchen"}
                });


            g.AddVertex(Frankfurt);
            g.AddVertex(Mannheim);
            g.AddVertex(Wuerzburg);
            g.AddVertex(Stuttgart);
            g.AddVertex(Karlsruhe);
            g.AddVertex(Erfurt);
            g.AddVertex(Nuernberg);
            g.AddVertex(Kassel);
            g.AddVertex(Augsburg);
            g.AddVertex(Muenchen);

            #endregion

            #region roads

            g.AddEdge(new Edge(Frankfurt, Mannheim, new Dictionary<string, object>()
                {
                    { "s", 85 }
                }));

            g.AddEdge(new Edge(Frankfurt, Wuerzburg, new Dictionary<string, object>()
                {
                    { "s", 127 }
                }));

            g.AddEdge(new Edge(Frankfurt, Kassel, new Dictionary<string, object>()
                {
                    { "s", 172 }
                }));

            g.AddEdge(new Edge(Mannheim, Karlsruhe, new Dictionary<string, object>()
                {
                    { "s", 80 }
                }));

            g.AddEdge(new Edge(Wuerzburg, Erfurt, new Dictionary<string, object>()
                {
                    { "s", 186 }
                }));

            g.AddEdge(new Edge(Wuerzburg, Nuernberg, new Dictionary<string, object>()
                {
                    { "s", 103 }
                }));

            g.AddEdge(new Edge(Nuernberg, Stuttgart, new Dictionary<string, object>()
                {
                    { "s", 183 }
                }));

            g.AddEdge(new Edge(Nuernberg, Muenchen, new Dictionary<string, object>()
                {
                    { "s", 167 }
                }));

            g.AddEdge(new Edge(Karlsruhe, Augsburg, new Dictionary<string, object>()
                {
                    { "s", 250 }
                }));

            g.AddEdge(new Edge(Augsburg, Muenchen, new Dictionary<string, object>()
                {
                    { "s", 84 }
                }));

            g.AddEdge(new Edge(Kassel, Muenchen, new Dictionary<string, object>()
                {
                    { "s", 502 }
                }));

            #endregion

            return g;
        }

        protected static string BARABASI_N_100  = "barabasi_100";

        protected IGraph InitGraph(String myGraphName)
        {
            var g = new OwlerDB();

            var reader = new GraphMLReader();
            var stream = File.OpenRead("../../../AlgorithmTests/bin/Debug/graphs/"+myGraphName+".xml");

            var done = reader.Read(g, stream);

            stream.Close();

            return (done) ? g : null;
        }
    }
}
