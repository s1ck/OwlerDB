using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OwlCore.Structure;
using OwlAPI.Structure;

namespace StructureTests
{
    [TestClass]
    public class OwlerDBTest
    {
        [TestMethod]
        public void AddVertexTest()
        {
            var db = new OwlerDB();

            var v1 = new Vertex();

            db.AddVertex(v1);

            Assert.AreEqual(1, db.VertexCount);
        }

        [TestMethod]
        public void RemoveVertexTest()
        {
            var db = new OwlerDB();

            var v1 = new Vertex();

            db.AddVertex(v1);

            Assert.AreEqual(1, db.VertexCount);

            db.RemoveVertex(v1);

            Assert.AreEqual(0, db.VertexCount);
        }
    }
}
