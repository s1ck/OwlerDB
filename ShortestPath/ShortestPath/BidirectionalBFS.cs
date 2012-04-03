using System;
using System.Collections.Generic;
using OwlAPI.Structure;

namespace OwlAlgos.ShortestPath
{
    public class BidirectionalBFS : ABidirectionalBFS
    {
        public static List<IVertex> Search(IGraph myGraph, IVertex mySource, IVertex myTarget, Func<IVertex, bool> myMatchingFunc = null)
        {
            // queue for bfs
            var leftQueue = new Queue<IVertex>();
            var rightQueue = new Queue<IVertex>();

            // store visited nodes in a hashset
            var visitedNodesLeft = new HashSet<IVertex>();
            var visitedNodesRight = new HashSet<IVertex>();

            var doMatching = myMatchingFunc != null;

            IVertex intersectionVertex = null;

            // init the queue with the source vertex
            mySource[PREDECESSOR_ATTRIBUTE_KEY] = null;
            myTarget[SUCCESSOR_ATTRIBUTE_KEY]   = null;

            leftQueue.Enqueue(mySource);
            rightQueue.Enqueue(myTarget);

            visitedNodesLeft.Add(mySource);
            visitedNodesRight.Add(myTarget);

            IVertex currentLeftVertex = null;
            IVertex currentRightVertex = null;

            List<IVertex> result = null;

            while (leftQueue.Count > 0 && rightQueue.Count > 0)
            {
                currentLeftVertex = leftQueue.Dequeue();
                currentRightVertex = rightQueue.Dequeue();
                
                if (currentLeftVertex.Equals(myTarget))
                {
                    result = PathConcatenation.ConcatPath(myTarget, PREDECESSOR_ATTRIBUTE_KEY);
                    break;
                }
                else if (currentRightVertex.Equals(mySource))
                {
                    result = PathConcatenation.ConcatPath(mySource, PREDECESSOR_ATTRIBUTE_KEY, false);
                    break;
                }

                // check all children left
                foreach (var edge in currentLeftVertex.OutgoingEdges)
                {
                    if (!visitedNodesLeft.Contains(edge.Target))
                    {
                        edge.Target[PREDECESSOR_ATTRIBUTE_KEY] = currentLeftVertex;

                        if (doMatching)
                        {
                            if (myMatchingFunc(edge.Target))
                            {
                                leftQueue.Enqueue(edge.Target);
                                edge.Target[MATCHING_ATTRIBUTE_KEY] = true;
                            }
                            else
                            {
                                edge.Target[MATCHING_ATTRIBUTE_KEY] = false;
                            }
                        }
                        else
                        {
                            leftQueue.Enqueue(edge.Target);
                        }

                        visitedNodesLeft.Add(edge.Target);
                    }

                }

                // check all parents right
                foreach (var edge in currentRightVertex.IncomingEdges)
                {
                    if (!visitedNodesRight.Contains(edge.Source))
                    {
                        edge.Source[SUCCESSOR_ATTRIBUTE_KEY] = currentRightVertex;

                        if (doMatching)
                        {
                            if (myMatchingFunc(edge.Source))
                            {
                                rightQueue.Enqueue(edge.Source);
                                edge.Target[MATCHING_ATTRIBUTE_KEY] = true;
                            }
                            else
                            {
                                edge.Target[MATCHING_ATTRIBUTE_KEY] = false;
                            }
                        }
                        else
                        {
                            rightQueue.Enqueue(edge.Source);
                        }

                        visitedNodesRight.Add(edge.Source);
                    }
                }

                #region check intersect between visited nodes

                intersectionVertex = GetIntersectionVertex(visitedNodesLeft, visitedNodesRight, doMatching);

                if (intersectionVertex != null)
                {
                    // got a connection between the searches
                    List<IVertex> pathLeft = null;
                    List<IVertex> pathRight = null;

                    if (intersectionVertex[PREDECESSOR_ATTRIBUTE_KEY].Equals(currentLeftVertex))
                    {
                        pathLeft = PathConcatenation.ConcatPath(intersectionVertex, PREDECESSOR_ATTRIBUTE_KEY);
                        pathRight = PathConcatenation.ConcatPath((IVertex)intersectionVertex[SUCCESSOR_ATTRIBUTE_KEY], SUCCESSOR_ATTRIBUTE_KEY, false);
                    }
                    else
                    {
                        pathLeft = PathConcatenation.ConcatPath((IVertex)intersectionVertex[PREDECESSOR_ATTRIBUTE_KEY], PREDECESSOR_ATTRIBUTE_KEY);
                        pathRight = PathConcatenation.ConcatPath(intersectionVertex, SUCCESSOR_ATTRIBUTE_KEY, false);
                    }

                    pathLeft.AddRange(pathRight);

                    result = pathLeft;
                    break;
                }

                #endregion
            }

            return result;
        }
    }
}
