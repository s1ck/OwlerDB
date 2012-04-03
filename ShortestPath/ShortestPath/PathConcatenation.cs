using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OwlAPI.Structure;

namespace OwlAlgos.ShortestPath
{
    public class PathConcatenation
    {
        public static List<IVertex> ConcatPath(IVertex myTarget, String myAttributeKey, bool myReverted = true)
        {
            var path = new List<IVertex>();

            var tmp = myTarget;

            while (tmp != null)
            {
                path.Add(tmp);

                tmp = (IVertex)tmp[myAttributeKey];
            }

            if (myReverted)
            {
                path.Reverse();
            }

            return path;
        }
    }
}
