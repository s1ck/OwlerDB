using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OwlAPI.Structure;
using OwlCore.Structure;
using System.IO;
using System.Diagnostics;
using OwlAlgos.ShortestPath;
using System.Security.Cryptography;
using Tools.Parser.GraphML;
using Tools.Parser.EdgeListParser;
using OwlAPI.Parser;
using OwlAlgos.SSSP;

namespace OwlBench
{
    class Program
    {
        static void Main(string[] args)
        {
            //#region slashdot

            //BenchmarkClass.Benchmark(
            //    ExampleGraph.SLASHDOT_N_77360_M_905468,
            //    10,
            //    10,
            //    new List<Action<IGraph, IVertex, Func<IVertex, bool>>>() { 
            //        SSSP.Search,
            //        //pSSSP.Search,
            //        pSSSP2.Search,
            //        //pSSSP3.Search,
            //        pSSSP4.Search
            //    },
            //    true,
            //    null,
            //    BreadthFirstSearch.InitGraph);

            //GC.Collect();

            //#endregion

            //#region google

            //BenchmarkClass.Benchmark(
            //    ExampleGraph.GOOGLE_N_875713_M_5105039,
            //    10,
            //    10,
            //    new List<Action<IGraph, IVertex, Func<IVertex, bool>>>() { 
            //        SSSP.Search,
            //        //pSSSP.Search,
            //        pSSSP2.Search,
            //        //pSSSP3.Search
            //        pSSSP4.Search
            //    },
            //    true,
            //    null,
            //    BreadthFirstSearch.InitGraph);

            //GC.Collect();

            //#endregion

            #region wikitalk

            BenchmarkClass.Benchmark(
                ExampleGraph.WIKITALK_N_2394385_M_5021410,
                10,
                10,
                new List<Action<IGraph, IVertex, Func<IVertex, bool>>>() { 
                    SSSP.Search,
                    //pSSSP.Search,
                    pSSSP2.Search,
                    //pSSSP3.Search
                    pSSSP4.Search
                },
                true,
                null,
                BreadthFirstSearch.InitGraph);

            GC.Collect();

            #endregion

            //#region gnutella

            //BenchmarkClass.Benchmark(
            //    ExampleGraph.P2P_GNUTELLA_N_62586_M_147892,
            //    10,
            //    10,
            //    new List<Action<IGraph, IVertex, Func<IVertex, bool>>>() { 
            //        SSSP.Search,
            //        //pSSSP.Search,
            //        pSSSP2.Search,
            //        //pSSSP3.Search
            //        pSSSP4.Search
            //    },
            //    true,
            //    null,
            //    BreadthFirstSearch.InitGraph);

            //GC.Collect();

            //#endregion

            //#region road net texas

            //BenchmarkClass.Benchmark(
            //    ExampleGraph.ROADNET_TX_N_1379917_M_3843320,
            //    10,
            //    10,
            //    new List<Action<IGraph, IVertex, Func<IVertex, bool>>>() { 
            //        SSSP.Search,
            //        //pSSSP.Search,
            //        pSSSP2.Search,
            //        //pSSSP3.Search
            //        pSSSP4.Search
            //    },
            //    true,
            //    null,
            //    BreadthFirstSearch.InitGraph);

            //GC.Collect();

            //#endregion

            //#region ERDOS_N_100K_4M

            //BenchmarkClass.Benchmark(
            //    ExampleGraph.ERDOS_N_100K_4M,
            //    10,
            //    10,
            //    new List<Action<IGraph, IVertex, Func<IVertex, bool>>>() { 
            //        SSSP.Search,
            //        //pSSSP.Search,
            //        pSSSP2.Search,
            //        //pSSSP3.Search
            //        pSSSP4.Search
            //    },
            //    true,
            //    null,
            //    BreadthFirstSearch.InitGraph);

            //GC.Collect();

            //#endregion

            //#region ERDOS_N_1M_8M

            //BenchmarkClass.Benchmark(
            //    ExampleGraph.ERDOS_N_1M_8M,
            //    10,
            //    10,
            //    new List<Action<IGraph, IVertex, Func<IVertex, bool>>>() { 
            //        SSSP.Search,
            //        //pSSSP.Search,
            //        pSSSP2.Search,
            //        //pSSSP3.Search
            //        pSSSP4.Search
            //    },
            //    true,
            //    null,
            //    BreadthFirstSearch.InitGraph);

            //GC.Collect();

            //#endregion

            //#region BARABASI_N_100K

            //BenchmarkClass.Benchmark(
            //    ExampleGraph.BARABASI_N_100K,
            //    10,
            //    10,
            //    new List<Action<IGraph, IVertex, Func<IVertex, bool>>>() { 
            //        SSSP.Search,
            //        //pSSSP.Search,
            //        pSSSP2.Search,
            //        //pSSSP3.Search
            //        pSSSP4.Search
            //    },
            //    true,
            //    null,
            //    BreadthFirstSearch.InitGraph);

            //GC.Collect();

            //#endregion

            //#region BARABASI_N_1M

            //BenchmarkClass.Benchmark(
            //    ExampleGraph.BARABASI_N_1M,
            //    10,
            //    10,
            //    new List<Action<IGraph, IVertex, Func<IVertex, bool>>>() { 
            //        SSSP.Search,
            //        //pSSSP.Search,
            //        pSSSP2.Search,
            //        //pSSSP3.Search
            //        pSSSP4.Search
            //    },
            //    true,
            //    null,
            //    BreadthFirstSearch.InitGraph);

            //GC.Collect();

            //#endregion

            Console.ReadKey(true);
        }
    }
}
