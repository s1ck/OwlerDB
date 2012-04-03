using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OwlAPI.Structure;
using OwlAPI.Parser;
using OwlCore.Structure;
using System.IO;
using Tools.Parser.EdgeListParser;
using Tools.Parser.GraphML;
using System.Diagnostics;

namespace OwlBench
{
    public class BenchmarkClass
    {
        public static void Benchmark(String myExampleGraph,
            Int32 myRuns,
            Int32 myInnerRuns,
            List<Action<IGraph, IVertex, Func<IVertex, bool>>> mySSSPAlgos,
            bool myStore = false,
            Func<IVertex, bool> myMatchingFunc = null,
            Action<IGraph> myBeforeRunAction = null,
            Action<IGraph> myAfterRunAction = null)
        {
            IGraph g = null;
            var isGraphML = false;

            #region init graph

            g = InitGraph(myExampleGraph);

            if (g == null)
            {
                return;
            }

            // hack :/
            isGraphML = myExampleGraph.EndsWith(".xml");

            Console.WriteLine("Graph successfully loaded .. |V|={0}, |E|={1}", g.VertexCount, g.EdgeCount);

            #endregion

            // store the single results
            var runs = new long[mySSSPAlgos.Count, myRuns];
            var innerRuns = new long[myInnerRuns];

            var sw = new Stopwatch();

            IVertex source = null;

            for (int i = 0; i < myRuns; i++)
            {
                #region init source

                source = GetRandomNode(g, isGraphML);

                if (myBeforeRunAction != null)
                {
                    myBeforeRunAction(g);
                }

                Console.WriteLine("SourceID={0}", source.UUID);

                #endregion

                #region bench

                for (int j = 0; j < mySSSPAlgos.Count; j++)
                {
                    for (int k = 0; k < myInnerRuns; k++)
                    {
                        #region before run init

                        if (myBeforeRunAction != null)
                        {
                            myBeforeRunAction(g);
                        }

                        #endregion

                        sw.Start();
                        mySSSPAlgos[j](g, source, myMatchingFunc);
                        sw.Stop();
                        innerRuns[k] = sw.ElapsedMilliseconds;
                        
                        sw.Reset();

                        #region after run action

                        if (myAfterRunAction != null)
                        {
                            myAfterRunAction(g);
                        }

                        #endregion
                    }
                    runs[j, i] = innerRuns.Aggregate((a, b) => a + b) / myInnerRuns;

                    Console.WriteLine("Algorithm {0} took {1} ms (avg of {2} runs)", j, runs[j, i], myInnerRuns);
                }

                #endregion

            }

            #region calc avg time

            var res = CalculateResult(runs, mySSSPAlgos.Count, myRuns);

            if (myStore)
            {
                StoreResults(g, res, myExampleGraph);
            }
            #endregion
        }

        private static void StoreResults(IGraph myGraph, long[] myResults, String myGraphName)
        {
            var dir = Directory.CreateDirectory("results");
            var fileName = dir.Name + "/" + myGraphName + ".csv";

            var tw = new StreamWriter(fileName);

            var sb = new StringBuilder();

            sb.AppendFormat("{0};{1}", myGraph.VertexCount, myGraph.EdgeCount);

            foreach (var date in myResults)
            {
                sb.AppendFormat(";{0}", date);
            }

            tw.WriteLine(sb.ToString());

            tw.Close();
        }
        

        public static void Benchmark(String myExampleGraph,
            Int32 myRuns,
            Int32 myInnerRuns,
            List<Func<IGraph, IVertex, IVertex, Func<IVertex, bool>, List<IVertex>>> myBFSAlgos,
            bool myStore = false,
            Func<IVertex, bool> myMatchingFunc = null,
            Action<IGraph> myBeforeRunAction = null,
            Action<IGraph> myAfterRunAction = null)
        {
            IGraph g = null;
            var isGraphML = false;

            #region init graph

            g = InitGraph(myExampleGraph);

            if (g == null)
            {
                return;
            }

            // hack :/
            isGraphML = myExampleGraph.EndsWith(".xml");

            Console.WriteLine("Graph successfully loaded .. |V|={0}, |E|={1}", g.VertexCount, g.EdgeCount);

            #endregion

            #region runs

            // store the single results
            var runs = new long[myBFSAlgos.Count, myRuns];
            var innerRuns = new long[myInnerRuns];

            var sw = new Stopwatch();
            var sum = 0L;

            IVertex source = null;
            IVertex target = null;
            List<IVertex> path = null;
            List<IVertex> path2 = null;

            for (int i = 0; i < myRuns; i++)
            {
                #region init source and target

                while (path == null)
                {
                    source = GetRandomNode(g, isGraphML);
                    target = GetRandomNode(g, isGraphML);

                    if (myBeforeRunAction != null)
                    {
                        myBeforeRunAction(g);
                    }

                    path = myBFSAlgos[0](g, source, target, myMatchingFunc);

                    if (path == null)
                    {
                        Console.WriteLine("random source and target are not connected, trying new ones");
                    }
                }

                Console.WriteLine("got a working path from {0} to {1} with length {2}...starting benchmark", source.UUID, target.UUID, path.Count);
                
                // k, got a connected source and target
                

                #endregion

                #region bench

                for (int j = 0; j < myBFSAlgos.Count; j++)
                {
                    for (int k = 0; k < myInnerRuns; k++)
                    {
                        #region before run init

                        if (myBeforeRunAction != null)
                        {
                            myBeforeRunAction(g);
                        }

                        #endregion

                        sw.Start();
                        path2 = myBFSAlgos[j](g, source, target, myMatchingFunc);
                        path2.LongCount();
                        sw.Stop();
                        innerRuns[k] = sw.ElapsedMilliseconds;

                        
                        
                        sw.Reset();

                        #region after run action

                        if (myAfterRunAction != null)
                        {
                            myAfterRunAction(g);
                        }

                        #endregion
                    }
                    runs[j, i] = innerRuns.Aggregate((a, b) => a + b) / myInnerRuns;

                    Console.WriteLine("Algorithm {0} took {1} ms (avg of {2} runs)", j, runs[j, i], myInnerRuns);
                }

                #endregion

                path = null;
            }

            #region calc avg time

            var res = CalculateResult(runs, myBFSAlgos.Count, myRuns);

            #endregion

            #endregion
        }

        #region helper

        public static IGraph InitGraph(String myGraphName)
        {
            var g = new OwlerDB();

            IGraphReader myReader = null;

            if (myGraphName.EndsWith(".txt"))
            {
                myReader = new EdgeListReader();
            }
            else if (myGraphName.EndsWith(".xml"))
            {
                myReader = new GraphMLReader();
            }
            if (g == null)
            {
                Console.WriteLine("no suitable reader could be detected...exiting");
                return null;
            }
            else
            {
                var stream = File.OpenRead("graphs/" + myGraphName);

                var done = myReader.Read(g, stream);

                stream.Close();

                return (done) ? g : null;
            }
        }

        private static void PrintPath(List<IVertex> myPath)
        {
            int i = 0;

            for (i = 0; i < myPath.Count - 1; i++)
            {
                Console.Write(myPath[i].UUID + " -> ");
            }

            if (i < myPath.Count)
            {
                Console.Write(myPath[i].UUID);
            }
            Console.WriteLine();
        }

        private static long[] CalculateResult(long[,] myResults, int myX, int myY)
        {
            var results = new long[myX];
            var sum = 0L;
            var total = 0;

            for (int i = 0; i < myX; i++)
            {
                total = 0;
                for (int j = 0; j < myY; j++)
                {
                    sum += myResults[i, j];
                    if (myResults[i, j] > 0)
                    {
                        total++;
                    }
                }
                results[i] = (sum == 0) ? 0 : sum / total;
                Console.WriteLine("Algorithm {0}: {1} ms avg", i, results[i]);
                sum = 0L;
            }
            return results;
        }

        private static IVertex GetRandomNode(IGraph myGraph, bool myIsGraphML = false)
        {
            var random = new Random(DateTime.Now.Millisecond);
            String randomID = null;
            IVertex randomNode = null;

            while (randomNode == null)
            {
                randomID = random.Next(0, (Int32)myGraph.VertexCount - 1).ToString();
                // hack... the graphml files store their vertices in format "n0, n1,... n992321"
                if (myIsGraphML)
                {
                    randomID = "n" + randomID;
                }
                if(myGraph.Vertices.Select(v => v.UUID.Equals(randomID)).Count() > 0)
                {
                    randomNode = myGraph.Vertices.Single(v => v.UUID.Equals(randomID));
                }
            }

            return randomNode;
        }

        private static IVertex GetNodeByID(IGraph myGraph, String myID)
        {
            return myGraph.Vertices.AsParallel().Single(v => v.UUID.Equals(myID));
        }

        #endregion
    }
}
