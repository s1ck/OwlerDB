using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OwlCore.Structure;
using Tools.Parser.EdgeListParser;
using System.IO;

namespace ToolTests
{
    [TestClass]
    public class EdgeListTest
    {
        [TestMethod]
        public void ReadFromEdgeListTest()
        {
            var g = new OwlerDB();

            var reader = new EdgeListReader();
            var stream = File.OpenRead("../../../ToolTests/bin/Debug/graphs/p2p-Gnutella05.txt");

            var done = reader.Read(g, stream);

            Assert.IsTrue(done);

            Assert.AreEqual(8846, g.VertexCount);
            Assert.AreEqual(31839, g.EdgeCount);
        }
    }
}
