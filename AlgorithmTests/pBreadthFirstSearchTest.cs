using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureTests;
using OwlAlgos.ShortestPath;

namespace AlgorithmTests
{
    [TestClass]
    public class pBreadthFirstSearchTest : AGraphTest
    {
        [TestMethod]
        public void pBFSTest_GermanyGraph()
        {
            var g = InitGermanyGraph();

            var Frankfurt = g.Vertices.Single(v => v.UUID.Equals("1"));
            var Muenchen = g.Vertices.Single(v => v.UUID.Equals("10"));
            var Kassel = g.Vertices.Single(v => v.UUID.Equals("8"));

            var path = pBreadthFirstSearch.Search(g, Frankfurt, Muenchen, true);

            // Frankfurt -> Kassel -> Muenchen
            Assert.AreEqual(3, path.Count);
            Assert.AreEqual(Frankfurt, path[0]);
            Assert.AreEqual(Kassel, path[1]);
            Assert.AreEqual(Muenchen, path[2]);
        }

        [TestMethod]
        public void pBFSTest_Barabasi_100_Vertices()
        {
            var g = InitGraph(BARABASI_N_100);

            Assert.IsNotNull(g, "loading graph failed");

            var n0 = g.Vertices.Single(v => v.UUID.Equals("n0"));
            var n99 = g.Vertices.Single(v => v.UUID.Equals("n99"));

            Assert.IsNotNull(n0, "source node was null");
            Assert.IsNotNull(n99, "target node was null");

            var path = pBreadthFirstSearch.Search(g, n0, n99, true);

            // pathL = number of edges
            Assert.AreEqual(4, path.Count);
        }

        [TestMethod]
        public void pBFSTest_GermanyGraph_Matching()
        {
            var g = InitGermanyGraph();

            var Frankfurt = g.Vertices.Single(v => v.UUID.Equals("1"));
            var Muenchen = g.Vertices.Single(v => v.UUID.Equals("10"));
            var Wuerzburg = g.Vertices.Single(v => v.UUID.Equals("3"));
            var Nuernberg = g.Vertices.Single(v => v.UUID.Equals("7"));

            // search for a shortest path that doesn't go through kassel
            var path = pBreadthFirstSearch.Search(g, Frankfurt, Muenchen, true, (v => !v["name"].Equals("Kassel")));

            // Frankfurt -> Wuerzburg -> Nuernberg -> Muenchen
            Assert.AreEqual(4, path.Count);
            Assert.AreEqual(Frankfurt, path[0]);
            Assert.AreEqual(Wuerzburg, path[1]);
            Assert.AreEqual(Nuernberg, path[2]);
            Assert.AreEqual(Muenchen, path[3]);
        }
    }
}
