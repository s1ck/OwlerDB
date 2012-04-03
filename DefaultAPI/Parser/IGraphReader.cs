using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OwlAPI.Structure;
using System.IO;

namespace OwlAPI.Parser
{
    public interface IGraphReader
    {
        #region Methods

        /// <summary>
        /// Reads a graph from a given inputstream
        /// </summary>
        /// <param name="myGraph">the graph instance where the data will be stored</param>
        /// <param name="myInputStream">the stream to read the graph from</param>
        /// <returns>true if stream was successfully read</returns>
        bool Read(IGraph myGraph, FileStream myInputStream);

        #endregion
    }
}
