using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace IS4
{
    public class RSAParser
    {
        public static List<byte[]> ParamsToList(RSAParameters parameters)
        {
            List<byte[]> list = new List<byte[]>();
            list.Add(parameters.D);
            list.Add(parameters.DP);
            list.Add(parameters.DQ);
            list.Add(parameters.Exponent);
            list.Add(parameters.InverseQ);
            list.Add(parameters.Modulus);
            list.Add(parameters.P);
            list.Add(parameters.Q);
            return list;
        }
        public static RSAParameters ListToParams(List<byte[]> parameters)
        {
            RSAParameters rsa = new RSAParameters();
            rsa.D = parameters[0];
            rsa.DP = parameters[1];
            rsa.DQ = parameters[2];
            rsa.Exponent = parameters[3];
            rsa.InverseQ = parameters[4];
            rsa.Modulus = parameters[5];
            rsa.P = parameters[6];
            rsa.Q = parameters[7];
            return rsa;
        }
    }
}
