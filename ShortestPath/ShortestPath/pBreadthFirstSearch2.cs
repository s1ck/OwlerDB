using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OwlAPI.Structure;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace OwlAlgos.ShortestPath
{
    /// <summary>
    /// This implementation uses 2 concurrent queues to iterator throug the nodes of the same level
    /// (while-not-empty pattern)
    /// </summary>
    public class pBreadthFirstSearch2 : ABreadthFirstSearch
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
            var from    = new ConcurrentQueue<IVertex>();
            var to      = new ConcurrentQueue<IVertex>();

            #endregion

            #region BFS

            // enqueue the source vertex
            from.Enqueue(mySource);

            while (!from.IsEmpty && !done)
            {
                to = new ConcurrentQueue<IVertex>();

                // process queue in parallel
                Parallel.ForEach<IVertex>(from, u =>
                {
                    //IVertex u = null;
                    // try to dequeue from the queue
                    if (from.TryDequeue(out u))
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
                                        to.Enqueue(v);
                                    }
                                    // do nothing
                                }
                                else
                                {
                                    // no matching necessary
                                    to.Enqueue(v);
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

                from = to;
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
