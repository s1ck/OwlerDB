using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OwlAPI.Structure;
using System.Security.Cryptography;

namespace OwlBench
{
    public class MatchingFunc
    {
        #region matching funcs

        public static Func<IVertex, bool> md5func = (v =>
        {
            var uuid = v.UUID;

            var encoding = new System.Text.UTF8Encoding();

            var byteArray = encoding.GetBytes(uuid.ToCharArray());

            var md5 = MD5.Create();

            md5.ComputeHash(byteArray, 0, byteArray.Length);

            return true;
        });

        public static Func<IVertex, bool> clusteringCoefficient = (v =>
        {
            var uuid = v.UUID;
            var k_i = v.OutDegree;

            var possibleConnections = k_i * (k_i - 1);

            // number of connected neighbours (triangle)
            var numberOfConnectedTriangles = 0;

            foreach (var j in v.OutgoingEdges)
            {
                foreach (var k in j.Target.OutgoingEdges)
                {
                    foreach (var l in v.OutgoingEdges)
                    {
                        if (k.Target.Equals(l.Target))
                        {
                            numberOfConnectedTriangles++;
                        }
                    }
                }
            }
            // the clustering coeffienct is the the relation between existing connections and all possible connections

            var c = (double)numberOfConnectedTriangles / possibleConnections;

            v["C"] = c;

            return true;
        });

        #endregion
    }
}
