using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OwlAPI.Structure;
using System.Threading.Tasks;

namespace OwlAlgos.ShortestPath
{
    public class ABidirectionalBFS : ABreadthFirstSearch
    {
        /// <summary>
        /// Attribute key .. Used to store the reference to the successor at a vertex
        /// </summary>
        protected static string SUCCESSOR_ATTRIBUTE_KEY = "__succ";
        /// <summary>
        /// Attribute key .. Used to store a bool if the node matches a given function
        /// </summary>
        protected static string MATCHING_ATTRIBUTE_KEY = "__matches";

        protected static IVertex GetIntersectionVertex(HashSet<IVertex> myVisitedNodesLeft, HashSet<IVertex> myVisitedNodesRight, bool myDoMatching = false)
        {
            // check which set has more elements to avoid unnecessary checks
            if (myVisitedNodesLeft.Count > myVisitedNodesRight.Count)
            {
                foreach (var v in myVisitedNodesRight)
                {
                    if (myVisitedNodesLeft.Contains(v))
                    {
                        if (myDoMatching)
                        {
                            if ((bool)v[MATCHING_ATTRIBUTE_KEY])
                            {
                                return v;
                            }
                        }
                        else
                        {
                            return v;
                        }
                    }
                }
            }
            else
            {
                foreach (var v in myVisitedNodesLeft)
                {
                    if (myVisitedNodesRight.Contains(v))
                    {
                        if (myDoMatching)
                        {
                            if ((bool)v[MATCHING_ATTRIBUTE_KEY])
                            {
                               return v;
                            }
                        }
                        else
                        {
                            return v;
                        }
                    }
                }
            }

            return null;
        }

        protected static IVertex GetIntersectionVertexParallel(HashSet<IVertex> myVisitedNodesLeft, HashSet<IVertex> myVisitedNodesRight, bool myDoMatching = false)
        {
            IVertex matchNode = null;

            // check which set has more elements to avoid unnecessary checks
            if (myVisitedNodesLeft.Count > myVisitedNodesRight.Count)
            {
                //foreach (var v in myVisitedNodesRight)
                Parallel.ForEach(myVisitedNodesRight, (v, state) =>
                {
                    if (myVisitedNodesLeft.Contains(v))
                    {
                        if (myDoMatching)
                        {
                            if ((bool)v[MATCHING_ATTRIBUTE_KEY])
                            {
                                matchNode = v;
                                state.Break();
                            }
                        }
                        else
                        {
                            matchNode = v;
                            state.Break();
                        }
                    }
                });
            }
            else
            {
                //foreach (var v in myVisitedNodesLeft)
                Parallel.ForEach(myVisitedNodesLeft, (v, state) =>
                {
                    if (myVisitedNodesRight.Contains(v))
                    {
                        if (myDoMatching)
                        {
                            if ((bool)v[MATCHING_ATTRIBUTE_KEY])
                            {
                                matchNode = v;
                                state.Break();
                            }
                        }
                        else
                        {
                            matchNode = v;
                            state.Break();
                        }
                    }
                });
            }

            return matchNode;
        }
    }
}
