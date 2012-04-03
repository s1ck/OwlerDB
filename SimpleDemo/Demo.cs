using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OwlCore.Structure;
using OwlAlgos.ShortestPath;

namespace OwlDemo
{
    public class Demo
    {
        public static void Main(String[] args)
        {           
            #region graph

            var g = new OwlerDB();
            g["name"] = "Germany";

            #endregion

            #region cities

            var Frankfurt = new Vertex(new Dictionary<string, object>()
                {
                 {"name", "Frankfurt"}   
                });

            var Mannheim = new Vertex(new Dictionary<string, object>()
                {
                 {"name", "Mannheim"}   
                });

            var Wuerzburg = new Vertex(new Dictionary<string, object>()
                {
                 {"name", "Wuerzburg"}   
                });

            var Stuttgart = new Vertex(new Dictionary<string, object>()
                {
                 {"name", "Stuttgart"}   
                });

            var Karlsruhe = new Vertex(new Dictionary<string, object>()
                {
                 {"name", "Karlsruhe"}   
                });

            var Erfurt = new Vertex(new Dictionary<string, object>()
                {
                 {"name", "Erfurt"}   
                });

            var Nuernberg = new Vertex(new Dictionary<string, object>()
                {
                 {"name", "Nuernberg"}   
                });

            var Kassel = new Vertex(new Dictionary<string, object>()
                {
                 {"name", "Kassel"}   
                });

            var Augsburg = new Vertex(new Dictionary<string, object>()
                {
                 {"name", "Augsburg"}   
                });

            var Muenchen = new Vertex(new Dictionary<string, object>()
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


            foreach (var edge in Frankfurt.OutgoingEdges)
            {
                Console.WriteLine(edge.Target["name"] + " " + edge["s"] + "km");
            }

            Console.WriteLine(g.VertexCount);
            Console.WriteLine(g.EdgeCount);

            var path = BreadthFirstSearch.Search(g, Frankfurt, Muenchen);

            foreach (var v in path)
            {
                Console.WriteLine(v["name"]);
            }

            

            Console.ReadKey(true);
        }
    }
}
