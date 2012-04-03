using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OwlAPI.Structure;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace OwlAlgos.ShortestPath
{
    /// <summary>
    /// This implementation uses a single concurrent queue to iterator over the nodes in the same level
    /// </summary>
    public class pBreadthFirstSearch : ABreadthFirstSearch
    {
        public static List<IVertex> Search(IGraph myGraph, IVertex mySource, IVertex myTarget, Func<IVertex, bool> myMatchingFunc = null)
        {
            return Search(myGraph, mySource, myTarget, false, myMatchingFunc);
        }

        /// <summary>
        /// BFS for st-connectivity
        /// </summary>
        /// <param name="myGraph"></param>
        /// <param name="mySource"></param>
        /// <param name="myTarget"></param>
        /// <param name="myMatchingFunc"></param>
        /// <returns></returns>
        public static List<IVertex> Search(IGraph myGraph, IVertex mySource, IVertex myTarget, bool myInitGraph, Func<IVertex, bool> myMatchingFunc = null)
        {
            #region Init

            if (myInitGraph)
            {
                InitGraph(myGraph);
            }
            mySource[COLOR_ATTRIBUTE_KEY] = Color.RED;
            mySource[PREDECESSOR_ATTRIBUTE_KEY] = null;
            myTarget[COLOR_ATTRIBUTE_KEY] = Color.GREEN;
            myTarget[PREDECESSOR_ATTRIBUTE_KEY] = null;

            var doMatching = myMatchingFunc != null;

            // used to indicate that the target node has been found
            var done = false;

            // use Concurrent Queue for parallel access
            var queue = new ConcurrentQueue<IVertex>();

            #endregion

            #region BFS

            // enqueue the source vertex
            queue.Enqueue(mySource);

            while (!queue.IsEmpty && !done)
            {
                // process queue in parallel
                Parallel.ForEach<IVertex>(queue, u =>
                {
                    //IVertex u = null;
                    // try to dequeue from the queue
                    if (queue.TryDequeue(out u))
                    {
                        // process neighbours in parall el
                        //Parallel.ForEach<IEdge>(u.OutgoingEdges, outEdge =>
                        foreach (var outEdge in u.OutgoingEdges)
                        {
                            // neighbour node
                            var v = outEdge.Target;
                            // get the color of that neighbour
                            var color = (Color)v[COLOR_ATTRIBUTE_KEY];

                            if (color == Color.WHITE) // not the target
                            {
                                // set as visited (Color.RED)
                                v[COLOR_ATTRIBUTE_KEY] = Color.RED;
                                // set the predecessor
                                v[PREDECESSOR_ATTRIBUTE_KEY] = u;
                                // and enqueue that node (if matching condition == true)
                                if (doMatching)
                                {
                                    // matches condition?
                                    if (myMatchingFunc(v))
                                    {
                                        // matches, enqueue
                                        queue.Enqueue(v);
                                    }
                                    // do nothing
                                }
                                else
                                {
                                    // no matching necessary
                                    queue.Enqueue(v);
                                }
                            }
                            else if (color == Color.GREEN) // done
                            {
                                // finished
                                done = true;
                                // set the predecessor
                                v[PREDECESSOR_ATTRIBUTE_KEY] = u;
                            }
                            //});
                        }
                        u[COLOR_ATTRIBUTE_KEY] = Color.RED;
                    }
                });
            }

            #endregion


            if (done)
            {
                return PathConcatenation.ConcatPath(myTarget, PREDECESSOR_ATTRIBUTE_KEY);
            }

            return null;
        }
    }
}
