using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OwlBench
{
    public class ExampleGraph
    {
        public static string BARABASI_N_100 = "barabasi_100.xml";
        public static string BARABASI_N_1K = "barabasi_1K.xml";
        public static string BARABASI_N_10K = "barabasi_10K.xml";
        public static string BARABASI_N_50K = "barabasi_50K.xml";
        public static string BARABASI_N_100K = "barabasi_100K.xml";
        public static string BARABASI_N_200K = "barabasi_200K.xml";
        public static string BARABASI_N_500K = "barabasi_500K.xml";
        public static string BARABASI_N_1M = "barabasi_1M.xml";
        public static string BARABASI_N_2M = "barabasi_2M.xml";
        public static string ERDOS_N_100K = "erdos_100K.xml";
        public static string ERDOS_N_500K = "erdos_500K.xml";
        protected static string ERDOS_N_1M = "erdos_1M.xml";
        public static string ERDOS_N_2M = "erdos_2M.xml";

        public static string ERDOS_N_100K_200K = "erdos_100K_200K.xml";
        public static string ERDOS_N_100K_2M = "erdos_100K_2M.xml";
        public static string ERDOS_N_100K_4M = "erdos_100K_4M.xml";
        public static string ERDOS_N_500K_4M = "erdos_500K_4M.xml";
        public static string ERDOS_N_1M_8M = "erdos_1M_8M.xml";
        
        // social graph
        public static string SLASHDOT_N_77360_M_905468 = "Slashdot0811.txt";

        // communication graph
        public static string WIKITALK_N_2394385_M_5021410 = "WikiTalk.txt";

        // technological
        public static string ROADNET_TX_N_1379917_M_3843320 = "roadNet-TX.txt";
        public static string P2P_GNUTELLA_N_62586_M_147892 = "p2p-Gnutella31.txt";

        // web graphs
        public static string STANFORD_N_281903_M_2312497 = "web-Stanford.txt";
        public static string GOOGLE_N_875713_M_5105039 = "web-Google.txt";
    }
}
