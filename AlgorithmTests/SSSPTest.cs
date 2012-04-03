using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureTests;
using OwlAlgos.SSSP;
using OwlAlgos.ShortestPath;

namespace AlgorithmTests
{
    [TestClass]
    public class SSSPTest : AGraphTest
    {
        [TestMethod]
        public void SSSP_Germany_Graph_Test()
        {
            var g = InitGermanyGraph();

            var Frankfurt = g.Vertices.Single(v => v.UUID.Equals("1"));
            var Muenchen = g.Vertices.Single(v => v.UUID.Equals("10"));

            pSSSP2.Search(g, Frankfurt, true);

            Assert.AreEqual(2, Muenchen.Distance);
            //Assert.AreEqual(2, (Int32)Muenchen["__dist"]);
        }
    }
}
