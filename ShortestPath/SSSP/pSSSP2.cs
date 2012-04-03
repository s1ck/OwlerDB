using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OwlAlgos.ShortestPath;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using OwlAPI.Structure;

namespace OwlAlgos.SSSP
{
    public class pSSSP2 : ABreadthFirstSearch
    {
        public static void Search(IGraph myGraph, IVertex mySource, Func<IVertex, bool> myMatchingFunc = null)
        {
            Search(myGraph, mySource, false, myMatchingFunc);
        }

        /// <summary>
        /// BFS for st-connectivity
        /// </summary>
        /// <param name="myGraph"></param>
        /// <param name="mySource"></param>
        /// <param name="myTarget"></param>
        /// <param name="myMatchingFunc"></param>
        /// <returns></returns>
        public static void Search(IGraph myGraph, IVertex mySource, bool myInitGraph, Func<IVertex, bool> myMatchingFunc = null)
        {
            #region Init

            if (myInitGraph)
            {
                InitGraph(myGraph);
            }
            mySource.IsVisited = true;
            mySource.Distance = 0;

            // bool if matching function has to be called
            var doMatching = myMatchingFunc != null;

            // use Concurrent Queue for parallel access
            var from = new ConcurrentQueue<IVertex>();
            var to = new ConcurrentQueue<IVertex>();

            #endregion

            #region BFS

            // enqueue the source vertex
            from.Enqueue(mySource);

            while (from.Count > 0)
            {
                to = new ConcurrentQueue<IVertex>();

                Parallel.ForEach(from, u =>
                {
                    if (from.TryDequeue(out u))
                    {
                        // process neighbours in parallel
                        //Parallel.ForEach<IEdge>(u.OutgoingEdges, outEdge =>
                        foreach (var outEdge in u.OutgoingEdges)
                        {
                            // neighbour node
                            var v = outEdge.Target;

                            if (!v.IsVisited) // not the target
                            {
                                // set as visited (Color.RED)
                                v.IsVisited = true;
                                // set the predecessor
                                v.Predecessor = u;
                                //increment the distance
                                v.Distance = u.Distance + 1;

                                if (doMatching)
                                {
                                    // matches condition?
                                    if (myMatchingFunc(v))
                                    {
                                        // matches, enqueue
                                        to.Enqueue(v);
                                    }
                                }
                                else
                                {
                                    // no matching necessary
                                    to.Enqueue(v);
                                }
                            }
                        }
                    }
                });

                // switch queues
                from = to;
            }

            #endregion
        }
    }
}
