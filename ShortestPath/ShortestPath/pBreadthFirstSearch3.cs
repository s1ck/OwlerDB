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
    /// This implementation uses local aggregration to calculate the nodes of one level
    /// </summary>
    public class pBreadthFirstSearch3 : ABreadthFirstSearch
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
            var from = new List<IVertex>();
            var to = new List<IVertex>();
            var lockObj = new Object();
            OrderablePartitioner<Tuple<int, int>> partitioner;

            #endregion

            #region BFS

            // enqueue the source vertex
            from.Add(mySource);

            while (from.Count > 0 && !done)
            {
                to = new List<IVertex>();
                partitioner = Partitioner.Create(0, from.Count);

                // process queue in parallel
                Parallel.ForEach(
                    // the values to be aggregated
                    partitioner,
                    // local initial partial result
                    () => new List<IVertex>(),
                    // loop body
                    (range, loopState, partialResult) =>
                    {
                        for (int i = range.Item1; i < range.Item2; i++)
                        {
                            IVertex u = from[i];
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
                                            // matches, add to local set
                                            partialResult.Add(v);
                                        }
                                        // do nothing
                                    }
                                    else
                                    {
                                        // no matching necessary, add to local set
                                        partialResult.Add(v);
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
                        
                        return partialResult;
                    },
                    //the final step of each local context
                    (localPartialSet) =>
                    {
                        lock (lockObj)
                        {
                            // sloooooow
                            //to = to.Union<IVertex>(localPartialSet).ToList();
                            to.AddRange(localPartialSet);
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
