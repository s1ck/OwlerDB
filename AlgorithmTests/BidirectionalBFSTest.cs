using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OwlAlgos.ShortestPath;

namespace StructureTests
{
    [TestClass]
    public class BidirectionalBFSTest : AGraphTest
    {
        [TestMethod]
        public void BidirectionalBFSTest_GermanyGraph()
        {
            var g = InitGermanyGraph();

            var Frankfurt = g.Vertices.Single(v => v.UUID.Equals("1"));
            var Muenchen = g.Vertices.Single(v => v.UUID.Equals("10"));
            var Kassel = g.Vertices.Single(v => v.UUID.Equals("8"));

            var path = BidirectionalBFS.Search(g, Frankfurt, Muenchen);

            Assert.AreEqual(3, path.Count);
            Assert.AreEqual(Frankfurt, path[0]);
            Assert.AreEqual(Kassel, path[1]);
            Assert.AreEqual(Muenchen, path[2]);
        }

        [TestMethod]
        public void BidirectionalBFSTest_GermanyGraph_Matching()
        {
            var g = InitGermanyGraph();

            var Frankfurt = g.Vertices.Single(v => v.UUID.Equals("1"));
            var Muenchen = g.Vertices.Single(v => v.UUID.Equals("10"));
            var Wuerzburg = g.Vertices.Single(v => v.UUID.Equals("3"));
            var Nuernberg = g.Vertices.Single(v => v.UUID.Equals("7"));

            // search for path that doesnt contain Kassel
            var path = BidirectionalBFS.Search(g, Frankfurt, Muenchen, (v => !v["name"].Equals("Kassel")));

            // Frankfurt -> Wuerzburg -> Nuernberg -> Muenchen
            Assert.AreEqual(4, path.Count);
            Assert.AreEqual(Frankfurt, path[0]);
            Assert.AreEqual(Wuerzburg, path[1]);
            Assert.AreEqual(Nuernberg, path[2]);
            Assert.AreEqual(Muenchen, path[3]);
        }

        [TestMethod]
        public void BidirectionalBFSTest_Barabasi_100_Vertices()
        {
            var g = InitGraph(BARABASI_N_100);

            Assert.IsNotNull(g, "loading graph failed");

            var n0 = g.Vertices.Single(v => v.UUID.Equals("n0"));
            var n1 = g.Vertices.Single(v => v.UUID.Equals("n1"));
            var n10 = g.Vertices.Single(v => v.UUID.Equals("n10"));
            var n99 = g.Vertices.Single(v => v.UUID.Equals("n99"));

            Assert.IsNotNull(n0, "source node was null");
            Assert.IsNotNull(n99, "target node was null");

            var path = BidirectionalBFS.Search(g, n0, n99);

            // n0 -> n1 -> n10 -> n99
            Assert.AreEqual(4, path.Count);
            Assert.AreEqual(n0, path[0]);
            Assert.AreEqual(n1, path[1]);
            Assert.AreEqual(n10, path[2]);
            Assert.AreEqual(n99, path[3]);
        }
    }
}
