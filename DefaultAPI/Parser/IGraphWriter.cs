using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OwlAPI.Structure;

namespace OwlAPI.Parser
{
    public interface IGraphWriter
    {
        bool Write(IGraph myGraph, String myLocation);
    }
}
