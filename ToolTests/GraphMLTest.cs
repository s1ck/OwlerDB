using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ToolTests
{
    [TestClass]
    public class GraphMLTest : AGraphTest
    {
        [TestMethod]
        public void ReadFromGraphMLTest()
        {
            var g = InitGraph(BARABASI_N_100);

            Assert.IsNotNull(g);

            Assert.AreEqual(100, g.VertexCount);
            Assert.AreEqual(388, g.EdgeCount);
        }
    }
}
