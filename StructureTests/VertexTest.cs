using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OwlCore.Structure;

namespace StructureTests
{
    [TestClass]
    public class VertexTest
    {
        [TestMethod]
        public void LinkTest()
        {
            var g = new OwlerDB();
            var v1 = new Vertex();
            var v2 = new Vertex();

            g.AddVertex(v1);
            g.AddVertex(v2);

            g.AddEdge(new Edge(v1, v2));

            //v1 side
            Assert.AreEqual(v2, v1.OutgoingEdges.First().Target);
            Assert.AreEqual(v1, v1.OutgoingEdges.First().Source);

            //v2 side
            Assert.AreEqual(v1, v2.IncomingEdges.First().Source);
            Assert.AreEqual(v2, v2.IncomingEdges.First().Target);
        }
    }
}
