using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OwlAPI.Structure;
using OwlAlgos.ShortestPath;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace OwlAlgos.SSSP
{
    /// <summary>
    /// implements a parallel single source shortest path using concurrentqueue
    /// </summary>
    public class pSSSP : ABreadthFirstSearch
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
            var queue = new ConcurrentQueue<IVertex>();

            #endregion

            #region BFS

            // enqueue the source vertex
            queue.Enqueue(mySource);

            while (queue.Count > 0)
            {
                Parallel.ForEach(queue, u =>
                    {
                        if(queue.TryDequeue(out u))
                        {
                            // process neighbours in parallel
                            //Parallel.ForEach<IEdge>(u.OutgoingEdges, outEdge =>
                            foreach (var outEdge in u.OutgoingEdges)
                            {
                                // neighbour node
                                var v = outEdge.Target;

                                if (!v.IsVisited) // unexplorered node
                                {
                                    // set as visited (Color.RED)
                                    v.IsVisited = true;
                                    // set the predecessor
                                    v.Predecessor = u;
                                    //increment the distance
                                    v.Distance = u.Distance + 1;
                                    // and enqueue that node (if matching condition == true)
                                    if (doMatching)
                                    {
                                        // matches condition?
                                        if (myMatchingFunc(v))
                                        {
                                            // matches, enqueue
                                            queue.Enqueue(v);
                                        }
                                    }
                                    else
                                    {
                                        // no matching necessary
                                        queue.Enqueue(v);
                                    }
                                }
                            }
                        }
                    });
            }

            #endregion
        }
    }
}
